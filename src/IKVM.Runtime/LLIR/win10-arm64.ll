; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:64:64-i32:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-pc-windows-msvc19.34.31933"

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

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat {
  %3 = alloca ptr, align 8
  %4 = alloca ptr, align 8
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 8
  store ptr %1, ptr %3, align 8
  store ptr %0, ptr %4, align 8
  call void @llvm.va_start(ptr %6)
  %7 = load ptr, ptr %6, align 8
  %8 = load ptr, ptr %3, align 8
  %9 = load ptr, ptr %4, align 8
  %10 = call i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7)
  store i32 %10, ptr %5, align 4
  call void @llvm.va_end(ptr %6)
  %11 = load i32, ptr %5, align 4
  ret i32 %11
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  %7 = load ptr, ptr %4, align 8
  %8 = load ptr, ptr %5, align 8
  %9 = load ptr, ptr %6, align 8
  %10 = call i32 @_vsnprintf_l(ptr noundef %9, i64 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7)
  ret i32 %10
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_snprintf(ptr noundef %0, i64 noundef %1, ptr noundef %2, ...) #0 comdat {
  %4 = alloca ptr, align 8
  %5 = alloca i64, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store i64 %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %8, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load i64, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i32 @_vsnprintf(ptr noundef %12, i64 noundef %11, ptr noundef %10, ptr noundef %9)
  store i32 %13, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %14 = load i32, ptr %7, align 4
  ret i32 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsnprintf(ptr noundef %0, i64 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store i64 %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %9 = load ptr, ptr %5, align 8
  %10 = load ptr, ptr %6, align 8
  %11 = load i64, ptr %7, align 8
  %12 = load ptr, ptr %8, align 8
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i64 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 35
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 8
  ret ptr %18
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !4

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 36
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call ptr %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret ptr %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 65
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call ptr %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store ptr %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load ptr, ptr %9, align 8
  ret ptr %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !6

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 66
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call ptr %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret ptr %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 115
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 8
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !7

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 116
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call ptr %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret ptr %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 38
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !8

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 39
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i8 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i8, align 1
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 68
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !9

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 69
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i8 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i8 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 118
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !10

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 119
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i8 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 41
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !11

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 42
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i8 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i8, align 1
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 71
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !12

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 72
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i8 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i8 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i8, align 1
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 121
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !13

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 122
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i8 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 44
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !14

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 45
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i16 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i16, align 2
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 74
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !15

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 75
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i16 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i16 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 124
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !16

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 125
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i16 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 47
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !17

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 48
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i16 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i16, align 2
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 77
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !18

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 78
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i16 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i16 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i16, align 2
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 127
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !19

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 128
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i16 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 50
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !20

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 51
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i32 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i32 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i32, align 4
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 80
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i32 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i32 %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load i32, ptr %9, align 4
  ret i32 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !21

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 81
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i32 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i32 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 130
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !22

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 131
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i32 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i32 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 53
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !23

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 54
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i64 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i64 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i64, align 8
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 83
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call i64 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i64 %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load i64, ptr %9, align 8
  ret i64 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !24

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 84
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call i64 %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret i64 %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 133
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !25

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 134
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call i64 %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret i64 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca float, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 56
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !26

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 57
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call float %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret float %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca float, align 4
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 86
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call float %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store float %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load float, ptr %9, align 4
  ret float %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !27

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 87
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call float %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret float %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca float, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 136
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !28

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 137
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call float %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret float %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca double, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 59
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !29

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 60
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call double %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret double %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca double, align 8
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %10)
  %11 = load ptr, ptr %8, align 8
  %12 = load ptr, ptr %11, align 8
  %13 = getelementptr inbounds %struct.JNINativeInterface_, ptr %12, i32 0, i32 89
  %14 = load ptr, ptr %13, align 8
  %15 = load ptr, ptr %10, align 8
  %16 = load ptr, ptr %5, align 8
  %17 = load ptr, ptr %6, align 8
  %18 = load ptr, ptr %7, align 8
  %19 = load ptr, ptr %8, align 8
  %20 = call double %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store double %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load double, ptr %9, align 8
  ret double %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !30

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 90
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  %132 = call double %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret double %132
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca double, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 139
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !31

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 140
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call double %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret double %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %8)
  %9 = load ptr, ptr %6, align 8
  %10 = load ptr, ptr %9, align 8
  %11 = getelementptr inbounds %struct.JNINativeInterface_, ptr %10, i32 0, i32 29
  %12 = load ptr, ptr %11, align 8
  %13 = load ptr, ptr %8, align 8
  %14 = load ptr, ptr %4, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = call ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 8
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !32

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 30
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  %129 = call ptr %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret ptr %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %8 = load ptr, ptr %6, align 8
  %9 = load ptr, ptr %8, align 8
  %10 = getelementptr inbounds %struct.JNINativeInterface_, ptr %9, i32 0, i32 62
  %11 = load ptr, ptr %10, align 8
  %12 = load ptr, ptr %7, align 8
  %13 = load ptr, ptr %4, align 8
  %14 = load ptr, ptr %5, align 8
  %15 = load ptr, ptr %6, align 8
  call void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !33

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 63
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  call void %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %10 = load ptr, ptr %8, align 8
  %11 = load ptr, ptr %10, align 8
  %12 = getelementptr inbounds %struct.JNINativeInterface_, ptr %11, i32 0, i32 92
  %13 = load ptr, ptr %12, align 8
  %14 = load ptr, ptr %9, align 8
  %15 = load ptr, ptr %5, align 8
  %16 = load ptr, ptr %6, align 8
  %17 = load ptr, ptr %7, align 8
  %18 = load ptr, ptr %8, align 8
  call void %13(ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15, ptr noundef %14)
  call void @llvm.va_end(ptr %9)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca ptr, align 8
  %12 = alloca [257 x i8], align 1
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  br label %15

15:                                               ; preds = %5
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = zext i32 %24 to i64
  %26 = mul i64 %25, 8
  %27 = alloca i8, i64 %26, align 16
  store ptr %27, ptr %11, align 8
  store i32 0, ptr %14, align 4
  br label %28

28:                                               ; preds = %118, %15
  %29 = load i32, ptr %14, align 4
  %30 = load i32, ptr %13, align 4
  %31 = icmp slt i32 %29, %30
  br i1 %31, label %32, label %121

32:                                               ; preds = %28
  %33 = load i32, ptr %14, align 4
  %34 = sext i32 %33 to i64
  %35 = getelementptr inbounds [257 x i8], ptr %12, i64 0, i64 %34
  %36 = load i8, ptr %35, align 1
  %37 = sext i8 %36 to i32
  switch i32 %37, label %117 [
    i32 90, label %38
    i32 66, label %47
    i32 83, label %56
    i32 67, label %65
    i32 73, label %75
    i32 74, label %83
    i32 68, label %92
    i32 70, label %100
    i32 76, label %109
  ]

38:                                               ; preds = %32
  %39 = load ptr, ptr %6, align 8
  %40 = getelementptr inbounds i8, ptr %39, i64 8
  store ptr %40, ptr %6, align 8
  %41 = load i32, ptr %39, align 8
  %42 = trunc i32 %41 to i8
  %43 = load ptr, ptr %11, align 8
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %43, i64 %45
  store i8 %42, ptr %46, align 8
  br label %117

47:                                               ; preds = %32
  %48 = load ptr, ptr %6, align 8
  %49 = getelementptr inbounds i8, ptr %48, i64 8
  store ptr %49, ptr %6, align 8
  %50 = load i32, ptr %48, align 8
  %51 = trunc i32 %50 to i8
  %52 = load ptr, ptr %11, align 8
  %53 = load i32, ptr %14, align 4
  %54 = sext i32 %53 to i64
  %55 = getelementptr inbounds %union.jvalue, ptr %52, i64 %54
  store i8 %51, ptr %55, align 8
  br label %117

56:                                               ; preds = %32
  %57 = load ptr, ptr %6, align 8
  %58 = getelementptr inbounds i8, ptr %57, i64 8
  store ptr %58, ptr %6, align 8
  %59 = load i32, ptr %57, align 8
  %60 = trunc i32 %59 to i16
  %61 = load ptr, ptr %11, align 8
  %62 = load i32, ptr %14, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %61, i64 %63
  store i16 %60, ptr %64, align 8
  br label %117

65:                                               ; preds = %32
  %66 = load ptr, ptr %6, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %6, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = zext i16 %69 to i32
  %71 = load ptr, ptr %11, align 8
  %72 = load i32, ptr %14, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %71, i64 %73
  store i32 %70, ptr %74, align 8
  br label %117

75:                                               ; preds = %32
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load ptr, ptr %11, align 8
  %80 = load i32, ptr %14, align 4
  %81 = sext i32 %80 to i64
  %82 = getelementptr inbounds %union.jvalue, ptr %79, i64 %81
  store i32 %78, ptr %82, align 8
  br label %117

83:                                               ; preds = %32
  %84 = load ptr, ptr %6, align 8
  %85 = getelementptr inbounds i8, ptr %84, i64 8
  store ptr %85, ptr %6, align 8
  %86 = load i32, ptr %84, align 8
  %87 = sext i32 %86 to i64
  %88 = load ptr, ptr %11, align 8
  %89 = load i32, ptr %14, align 4
  %90 = sext i32 %89 to i64
  %91 = getelementptr inbounds %union.jvalue, ptr %88, i64 %90
  store i64 %87, ptr %91, align 8
  br label %117

92:                                               ; preds = %32
  %93 = load ptr, ptr %6, align 8
  %94 = getelementptr inbounds i8, ptr %93, i64 8
  store ptr %94, ptr %6, align 8
  %95 = load double, ptr %93, align 8
  %96 = load ptr, ptr %11, align 8
  %97 = load i32, ptr %14, align 4
  %98 = sext i32 %97 to i64
  %99 = getelementptr inbounds %union.jvalue, ptr %96, i64 %98
  store double %95, ptr %99, align 8
  br label %117

100:                                              ; preds = %32
  %101 = load ptr, ptr %6, align 8
  %102 = getelementptr inbounds i8, ptr %101, i64 8
  store ptr %102, ptr %6, align 8
  %103 = load double, ptr %101, align 8
  %104 = fptrunc double %103 to float
  %105 = load ptr, ptr %11, align 8
  %106 = load i32, ptr %14, align 4
  %107 = sext i32 %106 to i64
  %108 = getelementptr inbounds %union.jvalue, ptr %105, i64 %107
  store float %104, ptr %108, align 8
  br label %117

109:                                              ; preds = %32
  %110 = load ptr, ptr %6, align 8
  %111 = getelementptr inbounds i8, ptr %110, i64 8
  store ptr %111, ptr %6, align 8
  %112 = load ptr, ptr %110, align 8
  %113 = load ptr, ptr %11, align 8
  %114 = load i32, ptr %14, align 4
  %115 = sext i32 %114 to i64
  %116 = getelementptr inbounds %union.jvalue, ptr %113, i64 %115
  store ptr %112, ptr %116, align 8
  br label %117

117:                                              ; preds = %32, %109, %100, %92, %83, %75, %65, %56, %47, %38
  br label %118

118:                                              ; preds = %117
  %119 = load i32, ptr %14, align 4
  %120 = add nsw i32 %119, 1
  store i32 %120, ptr %14, align 4
  br label %28, !llvm.loop !34

121:                                              ; preds = %28
  br label %122

122:                                              ; preds = %121
  %123 = load ptr, ptr %10, align 8
  %124 = load ptr, ptr %123, align 8
  %125 = getelementptr inbounds %struct.JNINativeInterface_, ptr %124, i32 0, i32 93
  %126 = load ptr, ptr %125, align 8
  %127 = load ptr, ptr %11, align 8
  %128 = load ptr, ptr %7, align 8
  %129 = load ptr, ptr %8, align 8
  %130 = load ptr, ptr %9, align 8
  %131 = load ptr, ptr %10, align 8
  call void %126(ptr noundef %131, ptr noundef %130, ptr noundef %129, ptr noundef %128, ptr noundef %127)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %8 = load ptr, ptr %6, align 8
  %9 = load ptr, ptr %8, align 8
  %10 = getelementptr inbounds %struct.JNINativeInterface_, ptr %9, i32 0, i32 142
  %11 = load ptr, ptr %10, align 8
  %12 = load ptr, ptr %7, align 8
  %13 = load ptr, ptr %4, align 8
  %14 = load ptr, ptr %5, align 8
  %15 = load ptr, ptr %6, align 8
  call void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [257 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  br label %13

13:                                               ; preds = %4
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = zext i32 %22 to i64
  %24 = mul i64 %23, 8
  %25 = alloca i8, i64 %24, align 16
  store ptr %25, ptr %9, align 8
  store i32 0, ptr %12, align 4
  br label %26

26:                                               ; preds = %116, %13
  %27 = load i32, ptr %12, align 4
  %28 = load i32, ptr %11, align 4
  %29 = icmp slt i32 %27, %28
  br i1 %29, label %30, label %119

30:                                               ; preds = %26
  %31 = load i32, ptr %12, align 4
  %32 = sext i32 %31 to i64
  %33 = getelementptr inbounds [257 x i8], ptr %10, i64 0, i64 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %115 [
    i32 90, label %36
    i32 66, label %45
    i32 83, label %54
    i32 67, label %63
    i32 73, label %73
    i32 74, label %81
    i32 68, label %90
    i32 70, label %98
    i32 76, label %107
  ]

36:                                               ; preds = %30
  %37 = load ptr, ptr %5, align 8
  %38 = getelementptr inbounds i8, ptr %37, i64 8
  store ptr %38, ptr %5, align 8
  %39 = load i32, ptr %37, align 8
  %40 = trunc i32 %39 to i8
  %41 = load ptr, ptr %9, align 8
  %42 = load i32, ptr %12, align 4
  %43 = sext i32 %42 to i64
  %44 = getelementptr inbounds %union.jvalue, ptr %41, i64 %43
  store i8 %40, ptr %44, align 8
  br label %115

45:                                               ; preds = %30
  %46 = load ptr, ptr %5, align 8
  %47 = getelementptr inbounds i8, ptr %46, i64 8
  store ptr %47, ptr %5, align 8
  %48 = load i32, ptr %46, align 8
  %49 = trunc i32 %48 to i8
  %50 = load ptr, ptr %9, align 8
  %51 = load i32, ptr %12, align 4
  %52 = sext i32 %51 to i64
  %53 = getelementptr inbounds %union.jvalue, ptr %50, i64 %52
  store i8 %49, ptr %53, align 8
  br label %115

54:                                               ; preds = %30
  %55 = load ptr, ptr %5, align 8
  %56 = getelementptr inbounds i8, ptr %55, i64 8
  store ptr %56, ptr %5, align 8
  %57 = load i32, ptr %55, align 8
  %58 = trunc i32 %57 to i16
  %59 = load ptr, ptr %9, align 8
  %60 = load i32, ptr %12, align 4
  %61 = sext i32 %60 to i64
  %62 = getelementptr inbounds %union.jvalue, ptr %59, i64 %61
  store i16 %58, ptr %62, align 8
  br label %115

63:                                               ; preds = %30
  %64 = load ptr, ptr %5, align 8
  %65 = getelementptr inbounds i8, ptr %64, i64 8
  store ptr %65, ptr %5, align 8
  %66 = load i32, ptr %64, align 8
  %67 = trunc i32 %66 to i16
  %68 = zext i16 %67 to i32
  %69 = load ptr, ptr %9, align 8
  %70 = load i32, ptr %12, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %69, i64 %71
  store i32 %68, ptr %72, align 8
  br label %115

73:                                               ; preds = %30
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load ptr, ptr %9, align 8
  %78 = load i32, ptr %12, align 4
  %79 = sext i32 %78 to i64
  %80 = getelementptr inbounds %union.jvalue, ptr %77, i64 %79
  store i32 %76, ptr %80, align 8
  br label %115

81:                                               ; preds = %30
  %82 = load ptr, ptr %5, align 8
  %83 = getelementptr inbounds i8, ptr %82, i64 8
  store ptr %83, ptr %5, align 8
  %84 = load i32, ptr %82, align 8
  %85 = sext i32 %84 to i64
  %86 = load ptr, ptr %9, align 8
  %87 = load i32, ptr %12, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %86, i64 %88
  store i64 %85, ptr %89, align 8
  br label %115

90:                                               ; preds = %30
  %91 = load ptr, ptr %5, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %5, align 8
  %93 = load double, ptr %91, align 8
  %94 = load ptr, ptr %9, align 8
  %95 = load i32, ptr %12, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %94, i64 %96
  store double %93, ptr %97, align 8
  br label %115

98:                                               ; preds = %30
  %99 = load ptr, ptr %5, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %5, align 8
  %101 = load double, ptr %99, align 8
  %102 = fptrunc double %101 to float
  %103 = load ptr, ptr %9, align 8
  %104 = load i32, ptr %12, align 4
  %105 = sext i32 %104 to i64
  %106 = getelementptr inbounds %union.jvalue, ptr %103, i64 %105
  store float %102, ptr %106, align 8
  br label %115

107:                                              ; preds = %30
  %108 = load ptr, ptr %5, align 8
  %109 = getelementptr inbounds i8, ptr %108, i64 8
  store ptr %109, ptr %5, align 8
  %110 = load ptr, ptr %108, align 8
  %111 = load ptr, ptr %9, align 8
  %112 = load i32, ptr %12, align 4
  %113 = sext i32 %112 to i64
  %114 = getelementptr inbounds %union.jvalue, ptr %111, i64 %113
  store ptr %110, ptr %114, align 8
  br label %115

115:                                              ; preds = %30, %107, %98, %90, %81, %73, %63, %54, %45, %36
  br label %116

116:                                              ; preds = %115
  %117 = load i32, ptr %12, align 4
  %118 = add nsw i32 %117, 1
  store i32 %118, ptr %12, align 4
  br label %26, !llvm.loop !35

119:                                              ; preds = %26
  br label %120

120:                                              ; preds = %119
  %121 = load ptr, ptr %8, align 8
  %122 = load ptr, ptr %121, align 8
  %123 = getelementptr inbounds %struct.JNINativeInterface_, ptr %122, i32 0, i32 143
  %124 = load ptr, ptr %123, align 8
  %125 = load ptr, ptr %9, align 8
  %126 = load ptr, ptr %6, align 8
  %127 = load ptr, ptr %7, align 8
  %128 = load ptr, ptr %8, align 8
  call void %124(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %9 = load ptr, ptr %5, align 8
  %10 = load ptr, ptr %6, align 8
  %11 = load ptr, ptr %7, align 8
  %12 = load ptr, ptr %8, align 8
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i64 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsnprintf_l(ptr noundef %0, i64 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i64, align 8
  %10 = alloca ptr, align 8
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store i64 %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = load ptr, ptr %7, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load i64, ptr %9, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = call ptr @__local_stdio_printf_options()
  %18 = load i64, ptr %17, align 8
  %19 = or i64 %18, 1
  %20 = call i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i64 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
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

declare dso_local i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i64 noundef, ptr noundef, ptr noundef, ptr noundef) #2

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local ptr @__local_stdio_printf_options() #0 comdat {
  ret ptr @__local_stdio_printf_options._OptionsStorage
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+v8a" }
attributes #1 = { nocallback nofree nosync nounwind willreturn }
attributes #2 = { "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+v8a" }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"uwtable", i32 2}
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
