; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "thumbv7-pc-windows-msvc19.20.0"

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
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat {
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
  %10 = call arm_aapcs_vfpcc i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7)
  store i32 %10, ptr %5, align 4
  call void @llvm.va_end(ptr %6)
  %11 = load i32, ptr %5, align 4
  ret i32 %11
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  store ptr %1, ptr %5, align 4
  store ptr %0, ptr %6, align 4
  %7 = load ptr, ptr %4, align 4
  %8 = load ptr, ptr %5, align 4
  %9 = load ptr, ptr %6, align 4
  %10 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %9, i32 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7)
  ret i32 %10
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_snprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ...) #0 comdat {
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
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef %9)
  store i32 %13, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %14 = load i32, ptr %7, align 4
  ret i32 %14
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsnprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
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
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #1

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !5

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 36
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc ptr %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret ptr %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc ptr %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store ptr %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load ptr, ptr %9, align 4
  ret ptr %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !7

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 66
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc ptr %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret ptr %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !8

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 116
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc ptr %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret ptr %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc zeroext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !9

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 39
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc zeroext i8 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i8 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc zeroext i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !10

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 69
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc zeroext i8 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc zeroext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !11

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 119
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc zeroext i8 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i8 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc signext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !12

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 42
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc signext i8 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i8 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc signext i8 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i8 %20, ptr %9, align 1
  call void @llvm.va_end(ptr %10)
  %21 = load i8, ptr %9, align 1
  ret i8 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !13

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 72
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc signext i8 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i8 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc signext i8 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i8 %17, ptr %7, align 1
  call void @llvm.va_end(ptr %8)
  %18 = load i8, ptr %7, align 1
  ret i8 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !14

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 122
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc signext i8 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i8 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc zeroext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !15

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 45
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc zeroext i16 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i16 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc zeroext i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !16

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 75
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc zeroext i16 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc zeroext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !17

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 125
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc zeroext i16 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i16 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc signext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !18

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 48
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc signext i16 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i16 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc signext i16 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i16 %20, ptr %9, align 2
  call void @llvm.va_end(ptr %10)
  %21 = load i16, ptr %9, align 2
  ret i16 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !19

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 78
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc signext i16 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i16 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc signext i16 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i16 %17, ptr %7, align 2
  call void @llvm.va_end(ptr %8)
  %18 = load i16, ptr %7, align 2
  ret i16 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !20

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 128
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc signext i16 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i16 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !21

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 51
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc i32 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i32 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc i32 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i32 %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load i32, ptr %9, align 4
  ret i32 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !22

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 81
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc i32 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i32 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc i32 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i32 %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load i32, ptr %7, align 4
  ret i32 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !23

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 131
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc i32 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i32 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !24

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 54
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc i64 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i64 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc i64 %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store i64 %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load i64, ptr %9, align 8
  ret i64 %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !25

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 84
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc i64 %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret i64 %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc i64 %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store i64 %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load i64, ptr %7, align 8
  ret i64 %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !26

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 134
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc i64 %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret i64 %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !27

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 57
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc float %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret float %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc float %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store float %20, ptr %9, align 4
  call void @llvm.va_end(ptr %10)
  %21 = load float, ptr %9, align 4
  ret float %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !28

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 87
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc float %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret float %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc float %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store float %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load float, ptr %7, align 4
  ret float %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !29

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 137
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc float %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret float %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !30

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 60
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc double %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret double %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  %20 = call arm_aapcs_vfpcc double %14(ptr noundef %19, ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15)
  store double %20, ptr %9, align 8
  call void @llvm.va_end(ptr %10)
  %21 = load double, ptr %9, align 8
  ret double %21
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !31

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 90
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  %129 = call arm_aapcs_vfpcc double %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret double %129
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc double %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store double %17, ptr %7, align 8
  call void @llvm.va_end(ptr %8)
  %18 = load double, ptr %7, align 8
  ret double %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !32

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 140
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc double %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret double %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  %17 = call arm_aapcs_vfpcc ptr %12(ptr noundef %16, ptr noundef %15, ptr noundef %14, ptr noundef %13)
  store ptr %17, ptr %7, align 4
  call void @llvm.va_end(ptr %8)
  %18 = load ptr, ptr %7, align 4
  ret ptr %18
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !33

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 30
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  %126 = call arm_aapcs_vfpcc ptr %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret ptr %126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  call arm_aapcs_vfpcc void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !34

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 63
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  call arm_aapcs_vfpcc void %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 {
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
  call arm_aapcs_vfpcc void %13(ptr noundef %18, ptr noundef %17, ptr noundef %16, ptr noundef %15, ptr noundef %14)
  call void @llvm.va_end(ptr %9)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 {
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
  %23 = call arm_aapcs_vfpcc i32 %19(ptr noundef %22, ptr noundef %21, ptr noundef %20)
  store i32 %23, ptr %13, align 4
  %24 = load i32, ptr %13, align 4
  %25 = mul i32 %24, 8
  %26 = alloca i8, i32 %25, align 8
  store ptr %26, ptr %11, align 4
  store i32 0, ptr %14, align 4
  br label %27

27:                                               ; preds = %115, %15
  %28 = load i32, ptr %14, align 4
  %29 = load i32, ptr %13, align 4
  %30 = icmp slt i32 %28, %29
  br i1 %30, label %31, label %118

31:                                               ; preds = %27
  %32 = load i32, ptr %14, align 4
  %33 = getelementptr inbounds [257 x i8], ptr %12, i32 0, i32 %32
  %34 = load i8, ptr %33, align 1
  %35 = sext i8 %34 to i32
  switch i32 %35, label %114 [
    i32 90, label %36
    i32 66, label %44
    i32 83, label %52
    i32 67, label %60
    i32 73, label %69
    i32 74, label %76
    i32 68, label %84
    i32 70, label %95
    i32 76, label %107
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
  br label %114

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
  br label %114

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
  br label %114

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
  br label %114

69:                                               ; preds = %31
  %70 = load ptr, ptr %6, align 4
  %71 = getelementptr inbounds i8, ptr %70, i32 4
  store ptr %71, ptr %6, align 4
  %72 = load i32, ptr %70, align 4
  %73 = load ptr, ptr %11, align 4
  %74 = load i32, ptr %14, align 4
  %75 = getelementptr inbounds %union.jvalue, ptr %73, i32 %74
  store i32 %72, ptr %75, align 8
  br label %114

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
  br label %114

84:                                               ; preds = %31
  %85 = load ptr, ptr %6, align 4
  %86 = ptrtoint ptr %85 to i32
  %87 = add i32 %86, 7
  %88 = and i32 %87, -8
  %89 = inttoptr i32 %88 to ptr
  %90 = getelementptr inbounds i8, ptr %89, i32 8
  store ptr %90, ptr %6, align 4
  %91 = load double, ptr %89, align 8
  %92 = load ptr, ptr %11, align 4
  %93 = load i32, ptr %14, align 4
  %94 = getelementptr inbounds %union.jvalue, ptr %92, i32 %93
  store double %91, ptr %94, align 8
  br label %114

95:                                               ; preds = %31
  %96 = load ptr, ptr %6, align 4
  %97 = ptrtoint ptr %96 to i32
  %98 = add i32 %97, 7
  %99 = and i32 %98, -8
  %100 = inttoptr i32 %99 to ptr
  %101 = getelementptr inbounds i8, ptr %100, i32 8
  store ptr %101, ptr %6, align 4
  %102 = load double, ptr %100, align 8
  %103 = fptrunc double %102 to float
  %104 = load ptr, ptr %11, align 4
  %105 = load i32, ptr %14, align 4
  %106 = getelementptr inbounds %union.jvalue, ptr %104, i32 %105
  store float %103, ptr %106, align 8
  br label %114

107:                                              ; preds = %31
  %108 = load ptr, ptr %6, align 4
  %109 = getelementptr inbounds i8, ptr %108, i32 4
  store ptr %109, ptr %6, align 4
  %110 = load ptr, ptr %108, align 4
  %111 = load ptr, ptr %11, align 4
  %112 = load i32, ptr %14, align 4
  %113 = getelementptr inbounds %union.jvalue, ptr %111, i32 %112
  store ptr %110, ptr %113, align 8
  br label %114

114:                                              ; preds = %31, %107, %95, %84, %76, %69, %60, %52, %44, %36
  br label %115

115:                                              ; preds = %114
  %116 = load i32, ptr %14, align 4
  %117 = add nsw i32 %116, 1
  store i32 %117, ptr %14, align 4
  br label %27, !llvm.loop !35

118:                                              ; preds = %27
  br label %119

119:                                              ; preds = %118
  %120 = load ptr, ptr %10, align 4
  %121 = load ptr, ptr %120, align 4
  %122 = getelementptr inbounds %struct.JNINativeInterface_, ptr %121, i32 0, i32 93
  %123 = load ptr, ptr %122, align 4
  %124 = load ptr, ptr %11, align 4
  %125 = load ptr, ptr %7, align 4
  %126 = load ptr, ptr %8, align 4
  %127 = load ptr, ptr %9, align 4
  %128 = load ptr, ptr %10, align 4
  call arm_aapcs_vfpcc void %123(ptr noundef %128, ptr noundef %127, ptr noundef %126, ptr noundef %125, ptr noundef %124)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 {
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
  call arm_aapcs_vfpcc void %11(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
  call void @llvm.va_end(ptr %7)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 {
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
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18)
  store i32 %21, ptr %11, align 4
  %22 = load i32, ptr %11, align 4
  %23 = mul i32 %22, 8
  %24 = alloca i8, i32 %23, align 8
  store ptr %24, ptr %9, align 4
  store i32 0, ptr %12, align 4
  br label %25

25:                                               ; preds = %113, %13
  %26 = load i32, ptr %12, align 4
  %27 = load i32, ptr %11, align 4
  %28 = icmp slt i32 %26, %27
  br i1 %28, label %29, label %116

29:                                               ; preds = %25
  %30 = load i32, ptr %12, align 4
  %31 = getelementptr inbounds [257 x i8], ptr %10, i32 0, i32 %30
  %32 = load i8, ptr %31, align 1
  %33 = sext i8 %32 to i32
  switch i32 %33, label %112 [
    i32 90, label %34
    i32 66, label %42
    i32 83, label %50
    i32 67, label %58
    i32 73, label %67
    i32 74, label %74
    i32 68, label %82
    i32 70, label %93
    i32 76, label %105
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
  br label %112

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
  br label %112

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
  br label %112

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
  br label %112

67:                                               ; preds = %29
  %68 = load ptr, ptr %5, align 4
  %69 = getelementptr inbounds i8, ptr %68, i32 4
  store ptr %69, ptr %5, align 4
  %70 = load i32, ptr %68, align 4
  %71 = load ptr, ptr %9, align 4
  %72 = load i32, ptr %12, align 4
  %73 = getelementptr inbounds %union.jvalue, ptr %71, i32 %72
  store i32 %70, ptr %73, align 8
  br label %112

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
  br label %112

82:                                               ; preds = %29
  %83 = load ptr, ptr %5, align 4
  %84 = ptrtoint ptr %83 to i32
  %85 = add i32 %84, 7
  %86 = and i32 %85, -8
  %87 = inttoptr i32 %86 to ptr
  %88 = getelementptr inbounds i8, ptr %87, i32 8
  store ptr %88, ptr %5, align 4
  %89 = load double, ptr %87, align 8
  %90 = load ptr, ptr %9, align 4
  %91 = load i32, ptr %12, align 4
  %92 = getelementptr inbounds %union.jvalue, ptr %90, i32 %91
  store double %89, ptr %92, align 8
  br label %112

93:                                               ; preds = %29
  %94 = load ptr, ptr %5, align 4
  %95 = ptrtoint ptr %94 to i32
  %96 = add i32 %95, 7
  %97 = and i32 %96, -8
  %98 = inttoptr i32 %97 to ptr
  %99 = getelementptr inbounds i8, ptr %98, i32 8
  store ptr %99, ptr %5, align 4
  %100 = load double, ptr %98, align 8
  %101 = fptrunc double %100 to float
  %102 = load ptr, ptr %9, align 4
  %103 = load i32, ptr %12, align 4
  %104 = getelementptr inbounds %union.jvalue, ptr %102, i32 %103
  store float %101, ptr %104, align 8
  br label %112

105:                                              ; preds = %29
  %106 = load ptr, ptr %5, align 4
  %107 = getelementptr inbounds i8, ptr %106, i32 4
  store ptr %107, ptr %5, align 4
  %108 = load ptr, ptr %106, align 4
  %109 = load ptr, ptr %9, align 4
  %110 = load i32, ptr %12, align 4
  %111 = getelementptr inbounds %union.jvalue, ptr %109, i32 %110
  store ptr %108, ptr %111, align 8
  br label %112

112:                                              ; preds = %29, %105, %93, %82, %74, %67, %58, %50, %42, %34
  br label %113

113:                                              ; preds = %112
  %114 = load i32, ptr %12, align 4
  %115 = add nsw i32 %114, 1
  store i32 %115, ptr %12, align 4
  br label %25, !llvm.loop !36

116:                                              ; preds = %25
  br label %117

117:                                              ; preds = %116
  %118 = load ptr, ptr %8, align 4
  %119 = load ptr, ptr %118, align 4
  %120 = getelementptr inbounds %struct.JNINativeInterface_, ptr %119, i32 0, i32 143
  %121 = load ptr, ptr %120, align 4
  %122 = load ptr, ptr %9, align 4
  %123 = load ptr, ptr %6, align 4
  %124 = load ptr, ptr %7, align 4
  %125 = load ptr, ptr %8, align 4
  call arm_aapcs_vfpcc void %121(ptr noundef %125, ptr noundef %124, ptr noundef %123, ptr noundef %122)
  ret void
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat {
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
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %12, i32 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9)
  ret i32 %13
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat {
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
  %17 = call arm_aapcs_vfpcc ptr @__local_stdio_printf_options()
  %18 = load i64, ptr %17, align 8
  %19 = or i64 %18, 1
  %20 = call arm_aapcs_vfpcc i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i32 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12)
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

declare dso_local arm_aapcs_vfpcc i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i32 noundef, ptr noundef, ptr noundef, ptr noundef) #2

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc ptr @__local_stdio_printf_options() #0 comdat {
  ret ptr @__local_stdio_printf_options._OptionsStorage
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { nocallback nofree nosync nounwind willreturn }
attributes #2 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 1, !"min_enum_size", i32 4}
!2 = !{i32 7, !"uwtable", i32 2}
!3 = !{i32 7, !"frame-pointer", i32 2}
!4 = !{!"clang version 15.0.2"}
!5 = distinct !{!5, !6}
!6 = !{!"llvm.loop.mustprogress"}
!7 = distinct !{!7, !6}
!8 = distinct !{!8, !6}
!9 = distinct !{!9, !6}
!10 = distinct !{!10, !6}
!11 = distinct !{!11, !6}
!12 = distinct !{!12, !6}
!13 = distinct !{!13, !6}
!14 = distinct !{!14, !6}
!15 = distinct !{!15, !6}
!16 = distinct !{!16, !6}
!17 = distinct !{!17, !6}
!18 = distinct !{!18, !6}
!19 = distinct !{!19, !6}
!20 = distinct !{!20, !6}
!21 = distinct !{!21, !6}
!22 = distinct !{!22, !6}
!23 = distinct !{!23, !6}
!24 = distinct !{!24, !6}
!25 = distinct !{!25, !6}
!26 = distinct !{!26, !6}
!27 = distinct !{!27, !6}
!28 = distinct !{!28, !6}
!29 = distinct !{!29, !6}
!30 = distinct !{!30, !6}
!31 = distinct !{!31, !6}
!32 = distinct !{!32, !6}
!33 = distinct !{!33, !6}
!34 = distinct !{!34, !6}
!35 = distinct !{!35, !6}
!36 = distinct !{!36, !6}
