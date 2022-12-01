; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.34.31933"

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
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call ptr @JNI_CallObjectMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store ptr %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load ptr, ptr %8, align 8
  ret ptr %14
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !4

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 36
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call ptr %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret ptr %122
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare ptr @llvm.stacksave() #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.stackrestore(ptr) #1

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
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store ptr %16, ptr %10, align 8
  call void @llvm.va_end(ptr %9)
  %17 = load ptr, ptr %10, align 8
  ret ptr %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !6

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 66
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call ptr %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret ptr %125
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
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call ptr @JNI_CallStaticObjectMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store ptr %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load ptr, ptr %8, align 8
  ret ptr %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !7

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 116
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call ptr %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret ptr %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i8 @JNI_CallBooleanMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i8 %13, ptr %8, align 1
  call void @llvm.va_end(ptr %7)
  %14 = load i8, ptr %8, align 1
  ret i8 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !8

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 39
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i8 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i8 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i8 %16, ptr %10, align 1
  call void @llvm.va_end(ptr %9)
  %17 = load i8, ptr %10, align 1
  ret i8 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !9

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 69
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i8 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i8 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i8 @JNI_CallStaticBooleanMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i8 %13, ptr %8, align 1
  call void @llvm.va_end(ptr %7)
  %14 = load i8, ptr %8, align 1
  ret i8 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !10

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 119
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i8 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i8 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i8 @JNI_CallByteMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i8 %13, ptr %8, align 1
  call void @llvm.va_end(ptr %7)
  %14 = load i8, ptr %8, align 1
  ret i8 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !11

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 42
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i8 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i8 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i8 %16, ptr %10, align 1
  call void @llvm.va_end(ptr %9)
  %17 = load i8, ptr %10, align 1
  ret i8 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !12

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 72
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i8 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i8 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i8 @JNI_CallStaticByteMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i8 %13, ptr %8, align 1
  call void @llvm.va_end(ptr %7)
  %14 = load i8, ptr %8, align 1
  ret i8 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !13

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 122
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i8 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i8 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i16 @JNI_CallCharMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i16 %13, ptr %8, align 2
  call void @llvm.va_end(ptr %7)
  %14 = load i16, ptr %8, align 2
  ret i16 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !14

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 45
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i16 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i16 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i16 %16, ptr %10, align 2
  call void @llvm.va_end(ptr %9)
  %17 = load i16, ptr %10, align 2
  ret i16 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !15

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 75
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i16 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i16 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i16 @JNI_CallStaticCharMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i16 %13, ptr %8, align 2
  call void @llvm.va_end(ptr %7)
  %14 = load i16, ptr %8, align 2
  ret i16 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !16

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 125
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i16 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i16 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i16 @JNI_CallShortMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i16 %13, ptr %8, align 2
  call void @llvm.va_end(ptr %7)
  %14 = load i16, ptr %8, align 2
  ret i16 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !17

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 48
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i16 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i16 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i16 %16, ptr %10, align 2
  call void @llvm.va_end(ptr %9)
  %17 = load i16, ptr %10, align 2
  ret i16 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !18

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 78
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i16 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i16 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i16 @JNI_CallStaticShortMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i16 %13, ptr %8, align 2
  call void @llvm.va_end(ptr %7)
  %14 = load i16, ptr %8, align 2
  ret i16 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !19

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 128
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i16 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i16 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i32 @JNI_CallIntMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i32 %13, ptr %8, align 4
  call void @llvm.va_end(ptr %7)
  %14 = load i32, ptr %8, align 4
  ret i32 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !20

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 51
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i32 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i32 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i32 %16, ptr %10, align 4
  call void @llvm.va_end(ptr %9)
  %17 = load i32, ptr %10, align 4
  ret i32 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !21

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 81
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i32 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i32 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i32 @JNI_CallStaticIntMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i32 %13, ptr %8, align 4
  call void @llvm.va_end(ptr %7)
  %14 = load i32, ptr %8, align 4
  ret i32 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !22

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 131
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i32 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i32 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i64 @JNI_CallLongMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i64 %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load i64, ptr %8, align 8
  ret i64 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !23

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 54
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i64 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i64 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i64, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store i64 %16, ptr %10, align 8
  call void @llvm.va_end(ptr %9)
  %17 = load i64, ptr %10, align 8
  ret i64 %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !24

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 84
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call i64 %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret i64 %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call i64 @JNI_CallStaticLongMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store i64 %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load i64, ptr %8, align 8
  ret i64 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !25

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 134
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call i64 %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret i64 %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call float @JNI_CallFloatMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store float %13, ptr %8, align 4
  call void @llvm.va_end(ptr %7)
  %14 = load float, ptr %8, align 4
  ret float %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !26

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 57
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call float %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret float %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca float, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call float @JNI_CallNonvirtualFloatMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store float %16, ptr %10, align 4
  call void @llvm.va_end(ptr %9)
  %17 = load float, ptr %10, align 4
  ret float %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !27

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 87
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call float %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret float %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call float @JNI_CallStaticFloatMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store float %13, ptr %8, align 4
  call void @llvm.va_end(ptr %7)
  %14 = load float, ptr %8, align 4
  ret float %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !28

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 137
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call float %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret float %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call double @JNI_CallDoubleMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store double %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load double, ptr %8, align 8
  ret double %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !29

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 60
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call double %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret double %122
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca double, align 8
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  call void @llvm.va_start(ptr %9)
  %11 = load ptr, ptr %9, align 8
  %12 = load ptr, ptr %5, align 8
  %13 = load ptr, ptr %6, align 8
  %14 = load ptr, ptr %7, align 8
  %15 = load ptr, ptr %8, align 8
  %16 = call double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11)
  store double %16, ptr %10, align 8
  call void @llvm.va_end(ptr %9)
  %17 = load double, ptr %10, align 8
  ret double %17
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !30

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 90
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  %125 = call double %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %126 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %126)
  ret double %125
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  store ptr %1, ptr %5, align 8
  store ptr %0, ptr %6, align 8
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call double @JNI_CallStaticDoubleMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store double %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load double, ptr %8, align 8
  ret double %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !31

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 140
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call double %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret double %122
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
  call void @llvm.va_start(ptr %7)
  %9 = load ptr, ptr %7, align 8
  %10 = load ptr, ptr %4, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = call ptr @JNI_NewObjectV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  store ptr %13, ptr %8, align 8
  call void @llvm.va_end(ptr %7)
  %14 = load ptr, ptr %8, align 8
  ret ptr %14
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !32

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 30
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  %122 = call ptr %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %123 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %123)
  ret ptr %122
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
  %8 = load ptr, ptr %7, align 8
  %9 = load ptr, ptr %4, align 8
  %10 = load ptr, ptr %5, align 8
  %11 = load ptr, ptr %6, align 8
  call void @JNI_CallVoidMethodV(ptr noundef %11, ptr noundef %10, ptr noundef %9, ptr noundef %8)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !33

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 63
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  call void %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %122 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %122)
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
  %10 = load ptr, ptr %9, align 8
  %11 = load ptr, ptr %5, align 8
  %12 = load ptr, ptr %6, align 8
  %13 = load ptr, ptr %7, align 8
  %14 = load ptr, ptr %8, align 8
  call void @JNI_CallNonvirtualVoidMethodV(ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11, ptr noundef %10)
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
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca ptr, align 8
  %14 = alloca i64, align 8
  %15 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  store ptr %3, ptr %7, align 8
  store ptr %2, ptr %8, align 8
  store ptr %1, ptr %9, align 8
  store ptr %0, ptr %10, align 8
  %16 = load ptr, ptr %10, align 8
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds %struct.JNINativeInterface_, ptr %17, i32 0, i32 0
  %19 = load ptr, ptr %18, align 8
  %20 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0
  %21 = load ptr, ptr %7, align 8
  %22 = load ptr, ptr %10, align 8
  %23 = call i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %12, align 4
  %24 = load ptr, ptr %10, align 8
  %25 = load ptr, ptr %24, align 8
  %26 = getelementptr inbounds %struct.JNINativeInterface_, ptr %25, i32 0, i32 1
  %27 = load ptr, ptr %26, align 8
  %28 = load i32, ptr %12, align 4
  call void %27(i32 noundef %28)
  %29 = load i32, ptr %12, align 4
  %30 = zext i32 %29 to i64
  %31 = call ptr @llvm.stacksave()
  store ptr %31, ptr %13, align 8
  %32 = alloca %union.jvalue, i64 %30, align 16
  store i64 %30, ptr %14, align 8
  store i32 0, ptr %15, align 4
  br label %33

33:                                               ; preds = %113, %5
  %34 = load i32, ptr %15, align 4
  %35 = load i32, ptr %12, align 4
  %36 = icmp slt i32 %34, %35
  br i1 %36, label %37, label %116

37:                                               ; preds = %33
  %38 = load i32, ptr %15, align 4
  %39 = sext i32 %38 to i64
  %40 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %39
  %41 = load i8, ptr %40, align 1
  %42 = sext i8 %41 to i32
  switch i32 %42, label %112 [
    i32 90, label %43
    i32 66, label %51
    i32 67, label %59
    i32 83, label %67
    i32 73, label %75
    i32 74, label %82
    i32 70, label %90
    i32 68, label %98
    i32 76, label %105
  ]

43:                                               ; preds = %37
  %44 = load ptr, ptr %6, align 8
  %45 = getelementptr inbounds i8, ptr %44, i64 8
  store ptr %45, ptr %6, align 8
  %46 = load i32, ptr %44, align 8
  %47 = trunc i32 %46 to i8
  %48 = load i32, ptr %15, align 4
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %32, i64 %49
  store i8 %47, ptr %50, align 8
  br label %112

51:                                               ; preds = %37
  %52 = load ptr, ptr %6, align 8
  %53 = getelementptr inbounds i8, ptr %52, i64 8
  store ptr %53, ptr %6, align 8
  %54 = load i32, ptr %52, align 8
  %55 = trunc i32 %54 to i8
  %56 = load i32, ptr %15, align 4
  %57 = sext i32 %56 to i64
  %58 = getelementptr inbounds %union.jvalue, ptr %32, i64 %57
  store i8 %55, ptr %58, align 8
  br label %112

59:                                               ; preds = %37
  %60 = load ptr, ptr %6, align 8
  %61 = getelementptr inbounds i8, ptr %60, i64 8
  store ptr %61, ptr %6, align 8
  %62 = load i32, ptr %60, align 8
  %63 = trunc i32 %62 to i16
  %64 = load i32, ptr %15, align 4
  %65 = sext i32 %64 to i64
  %66 = getelementptr inbounds %union.jvalue, ptr %32, i64 %65
  store i16 %63, ptr %66, align 8
  br label %112

67:                                               ; preds = %37
  %68 = load ptr, ptr %6, align 8
  %69 = getelementptr inbounds i8, ptr %68, i64 8
  store ptr %69, ptr %6, align 8
  %70 = load i32, ptr %68, align 8
  %71 = trunc i32 %70 to i16
  %72 = load i32, ptr %15, align 4
  %73 = sext i32 %72 to i64
  %74 = getelementptr inbounds %union.jvalue, ptr %32, i64 %73
  store i16 %71, ptr %74, align 8
  br label %112

75:                                               ; preds = %37
  %76 = load ptr, ptr %6, align 8
  %77 = getelementptr inbounds i8, ptr %76, i64 8
  store ptr %77, ptr %6, align 8
  %78 = load i32, ptr %76, align 8
  %79 = load i32, ptr %15, align 4
  %80 = sext i32 %79 to i64
  %81 = getelementptr inbounds %union.jvalue, ptr %32, i64 %80
  store i32 %78, ptr %81, align 8
  br label %112

82:                                               ; preds = %37
  %83 = load ptr, ptr %6, align 8
  %84 = getelementptr inbounds i8, ptr %83, i64 8
  store ptr %84, ptr %6, align 8
  %85 = load i32, ptr %83, align 8
  %86 = sext i32 %85 to i64
  %87 = load i32, ptr %15, align 4
  %88 = sext i32 %87 to i64
  %89 = getelementptr inbounds %union.jvalue, ptr %32, i64 %88
  store i64 %86, ptr %89, align 8
  br label %112

90:                                               ; preds = %37
  %91 = load ptr, ptr %6, align 8
  %92 = getelementptr inbounds i8, ptr %91, i64 8
  store ptr %92, ptr %6, align 8
  %93 = load double, ptr %91, align 8
  %94 = fptrunc double %93 to float
  %95 = load i32, ptr %15, align 4
  %96 = sext i32 %95 to i64
  %97 = getelementptr inbounds %union.jvalue, ptr %32, i64 %96
  store float %94, ptr %97, align 8
  br label %112

98:                                               ; preds = %37
  %99 = load ptr, ptr %6, align 8
  %100 = getelementptr inbounds i8, ptr %99, i64 8
  store ptr %100, ptr %6, align 8
  %101 = load double, ptr %99, align 8
  %102 = load i32, ptr %15, align 4
  %103 = sext i32 %102 to i64
  %104 = getelementptr inbounds %union.jvalue, ptr %32, i64 %103
  store double %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %37
  %106 = load ptr, ptr %6, align 8
  %107 = getelementptr inbounds i8, ptr %106, i64 8
  store ptr %107, ptr %6, align 8
  %108 = load ptr, ptr %106, align 8
  %109 = load i32, ptr %15, align 4
  %110 = sext i32 %109 to i64
  %111 = getelementptr inbounds %union.jvalue, ptr %32, i64 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %37, %105, %98, %90, %82, %75, %67, %59, %51, %43
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %15, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %15, align 4
  br label %33, !llvm.loop !34

116:                                              ; preds = %33
  %117 = load ptr, ptr %10, align 8
  %118 = load ptr, ptr %117, align 8
  %119 = getelementptr inbounds %struct.JNINativeInterface_, ptr %118, i32 0, i32 93
  %120 = load ptr, ptr %119, align 8
  %121 = load ptr, ptr %7, align 8
  %122 = load ptr, ptr %8, align 8
  %123 = load ptr, ptr %9, align 8
  %124 = load ptr, ptr %10, align 8
  call void %120(ptr noundef %124, ptr noundef %123, ptr noundef %122, ptr noundef %121, ptr noundef %32)
  %125 = load ptr, ptr %13, align 8
  call void @llvm.stackrestore(ptr %125)
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
  %8 = load ptr, ptr %7, align 8
  %9 = load ptr, ptr %4, align 8
  %10 = load ptr, ptr %5, align 8
  %11 = load ptr, ptr %6, align 8
  call void @JNI_CallStaticVoidMethodV(ptr noundef %11, ptr noundef %10, ptr noundef %9, ptr noundef %8)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca ptr, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  store ptr %2, ptr %6, align 8
  store ptr %1, ptr %7, align 8
  store ptr %0, ptr %8, align 8
  %14 = load ptr, ptr %8, align 8
  %15 = load ptr, ptr %14, align 8
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0
  %17 = load ptr, ptr %16, align 8
  %18 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0
  %19 = load ptr, ptr %6, align 8
  %20 = load ptr, ptr %8, align 8
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %10, align 4
  %22 = load ptr, ptr %8, align 8
  %23 = load ptr, ptr %22, align 8
  %24 = getelementptr inbounds %struct.JNINativeInterface_, ptr %23, i32 0, i32 1
  %25 = load ptr, ptr %24, align 8
  %26 = load i32, ptr %10, align 4
  call void %25(i32 noundef %26)
  %27 = load i32, ptr %10, align 4
  %28 = zext i32 %27 to i64
  %29 = call ptr @llvm.stacksave()
  store ptr %29, ptr %11, align 8
  %30 = alloca %union.jvalue, i64 %28, align 16
  store i64 %28, ptr %12, align 8
  store i32 0, ptr %13, align 4
  br label %31

31:                                               ; preds = %111, %4
  %32 = load i32, ptr %13, align 4
  %33 = load i32, ptr %10, align 4
  %34 = icmp slt i32 %32, %33
  br i1 %34, label %35, label %114

35:                                               ; preds = %31
  %36 = load i32, ptr %13, align 4
  %37 = sext i32 %36 to i64
  %38 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %37
  %39 = load i8, ptr %38, align 1
  %40 = sext i8 %39 to i32
  switch i32 %40, label %110 [
    i32 90, label %41
    i32 66, label %49
    i32 67, label %57
    i32 83, label %65
    i32 73, label %73
    i32 74, label %80
    i32 70, label %88
    i32 68, label %96
    i32 76, label %103
  ]

41:                                               ; preds = %35
  %42 = load ptr, ptr %5, align 8
  %43 = getelementptr inbounds i8, ptr %42, i64 8
  store ptr %43, ptr %5, align 8
  %44 = load i32, ptr %42, align 8
  %45 = trunc i32 %44 to i8
  %46 = load i32, ptr %13, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %30, i64 %47
  store i8 %45, ptr %48, align 8
  br label %110

49:                                               ; preds = %35
  %50 = load ptr, ptr %5, align 8
  %51 = getelementptr inbounds i8, ptr %50, i64 8
  store ptr %51, ptr %5, align 8
  %52 = load i32, ptr %50, align 8
  %53 = trunc i32 %52 to i8
  %54 = load i32, ptr %13, align 4
  %55 = sext i32 %54 to i64
  %56 = getelementptr inbounds %union.jvalue, ptr %30, i64 %55
  store i8 %53, ptr %56, align 8
  br label %110

57:                                               ; preds = %35
  %58 = load ptr, ptr %5, align 8
  %59 = getelementptr inbounds i8, ptr %58, i64 8
  store ptr %59, ptr %5, align 8
  %60 = load i32, ptr %58, align 8
  %61 = trunc i32 %60 to i16
  %62 = load i32, ptr %13, align 4
  %63 = sext i32 %62 to i64
  %64 = getelementptr inbounds %union.jvalue, ptr %30, i64 %63
  store i16 %61, ptr %64, align 8
  br label %110

65:                                               ; preds = %35
  %66 = load ptr, ptr %5, align 8
  %67 = getelementptr inbounds i8, ptr %66, i64 8
  store ptr %67, ptr %5, align 8
  %68 = load i32, ptr %66, align 8
  %69 = trunc i32 %68 to i16
  %70 = load i32, ptr %13, align 4
  %71 = sext i32 %70 to i64
  %72 = getelementptr inbounds %union.jvalue, ptr %30, i64 %71
  store i16 %69, ptr %72, align 8
  br label %110

73:                                               ; preds = %35
  %74 = load ptr, ptr %5, align 8
  %75 = getelementptr inbounds i8, ptr %74, i64 8
  store ptr %75, ptr %5, align 8
  %76 = load i32, ptr %74, align 8
  %77 = load i32, ptr %13, align 4
  %78 = sext i32 %77 to i64
  %79 = getelementptr inbounds %union.jvalue, ptr %30, i64 %78
  store i32 %76, ptr %79, align 8
  br label %110

80:                                               ; preds = %35
  %81 = load ptr, ptr %5, align 8
  %82 = getelementptr inbounds i8, ptr %81, i64 8
  store ptr %82, ptr %5, align 8
  %83 = load i32, ptr %81, align 8
  %84 = sext i32 %83 to i64
  %85 = load i32, ptr %13, align 4
  %86 = sext i32 %85 to i64
  %87 = getelementptr inbounds %union.jvalue, ptr %30, i64 %86
  store i64 %84, ptr %87, align 8
  br label %110

88:                                               ; preds = %35
  %89 = load ptr, ptr %5, align 8
  %90 = getelementptr inbounds i8, ptr %89, i64 8
  store ptr %90, ptr %5, align 8
  %91 = load double, ptr %89, align 8
  %92 = fptrunc double %91 to float
  %93 = load i32, ptr %13, align 4
  %94 = sext i32 %93 to i64
  %95 = getelementptr inbounds %union.jvalue, ptr %30, i64 %94
  store float %92, ptr %95, align 8
  br label %110

96:                                               ; preds = %35
  %97 = load ptr, ptr %5, align 8
  %98 = getelementptr inbounds i8, ptr %97, i64 8
  store ptr %98, ptr %5, align 8
  %99 = load double, ptr %97, align 8
  %100 = load i32, ptr %13, align 4
  %101 = sext i32 %100 to i64
  %102 = getelementptr inbounds %union.jvalue, ptr %30, i64 %101
  store double %99, ptr %102, align 8
  br label %110

103:                                              ; preds = %35
  %104 = load ptr, ptr %5, align 8
  %105 = getelementptr inbounds i8, ptr %104, i64 8
  store ptr %105, ptr %5, align 8
  %106 = load ptr, ptr %104, align 8
  %107 = load i32, ptr %13, align 4
  %108 = sext i32 %107 to i64
  %109 = getelementptr inbounds %union.jvalue, ptr %30, i64 %108
  store ptr %106, ptr %109, align 8
  br label %110

110:                                              ; preds = %35, %103, %96, %88, %80, %73, %65, %57, %49, %41
  br label %111

111:                                              ; preds = %110
  %112 = load i32, ptr %13, align 4
  %113 = add nsw i32 %112, 1
  store i32 %113, ptr %13, align 4
  br label %31, !llvm.loop !35

114:                                              ; preds = %31
  %115 = load ptr, ptr %8, align 8
  %116 = load ptr, ptr %115, align 8
  %117 = getelementptr inbounds %struct.JNINativeInterface_, ptr %116, i32 0, i32 143
  %118 = load ptr, ptr %117, align 8
  %119 = load ptr, ptr %6, align 8
  %120 = load ptr, ptr %7, align 8
  %121 = load ptr, ptr %8, align 8
  call void %118(ptr noundef %121, ptr noundef %120, ptr noundef %119, ptr noundef %30)
  %122 = load ptr, ptr %11, align 8
  call void @llvm.stackrestore(ptr %122)
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

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nocallback nofree nosync nounwind willreturn }
attributes #2 = { "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

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
