; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "thumbv7-pc-windows-msvc19.20.0"

%union.jvalue = type { i64 }
%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }

$sprintf = comdat any

$vsprintf = comdat any

$_snprintf = comdat any

$_vsnprintf = comdat any

$_vsprintf_l = comdat any

$_vsnprintf_l = comdat any

$__local_stdio_printf_options = comdat any

@__local_stdio_printf_options._OptionsStorage = internal global i64 0, align 8, !dbg !0

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat !dbg !1041 {
  %3 = alloca ptr, align 4
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  store ptr %1, ptr %3, align 4
  call void @llvm.dbg.declare(metadata ptr %3, metadata !1047, metadata !DIExpression()), !dbg !1048
  store ptr %0, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1049, metadata !DIExpression()), !dbg !1050
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1051, metadata !DIExpression()), !dbg !1052
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1053, metadata !DIExpression()), !dbg !1054
  call void @llvm.va_start(ptr %6), !dbg !1055
  %7 = load ptr, ptr %6, align 4, !dbg !1056
  %8 = load ptr, ptr %3, align 4, !dbg !1056
  %9 = load ptr, ptr %4, align 4, !dbg !1056
  %10 = call arm_aapcs_vfpcc i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1056
  store i32 %10, ptr %5, align 4, !dbg !1056
  call void @llvm.va_end(ptr %6), !dbg !1057
  %11 = load i32, ptr %5, align 4, !dbg !1058
  ret i32 %11, !dbg !1058
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat !dbg !1059 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1062, metadata !DIExpression()), !dbg !1063
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1064, metadata !DIExpression()), !dbg !1065
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1066, metadata !DIExpression()), !dbg !1067
  %7 = load ptr, ptr %4, align 4, !dbg !1068
  %8 = load ptr, ptr %5, align 4, !dbg !1068
  %9 = load ptr, ptr %6, align 4, !dbg !1068
  %10 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %9, i32 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1068
  ret i32 %10, !dbg !1068
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_snprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ...) #0 comdat !dbg !1069 {
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1073, metadata !DIExpression()), !dbg !1074
  store i32 %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1075, metadata !DIExpression()), !dbg !1076
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1077, metadata !DIExpression()), !dbg !1078
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1079, metadata !DIExpression()), !dbg !1080
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1081, metadata !DIExpression()), !dbg !1082
  call void @llvm.va_start(ptr %8), !dbg !1083
  %9 = load ptr, ptr %8, align 4, !dbg !1084
  %10 = load ptr, ptr %4, align 4, !dbg !1084
  %11 = load i32, ptr %5, align 4, !dbg !1084
  %12 = load ptr, ptr %6, align 4, !dbg !1084
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1084
  store i32 %13, ptr %7, align 4, !dbg !1084
  call void @llvm.va_end(ptr %8), !dbg !1085
  %14 = load i32, ptr %7, align 4, !dbg !1086
  ret i32 %14, !dbg !1086
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsnprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !1087 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1090, metadata !DIExpression()), !dbg !1091
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1092, metadata !DIExpression()), !dbg !1093
  store i32 %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1094, metadata !DIExpression()), !dbg !1095
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1096, metadata !DIExpression()), !dbg !1097
  %9 = load ptr, ptr %5, align 4, !dbg !1098
  %10 = load ptr, ptr %6, align 4, !dbg !1098
  %11 = load i32, ptr %7, align 4, !dbg !1098
  %12 = load ptr, ptr %8, align 4, !dbg !1098
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9), !dbg !1098
  ret i32 %13, !dbg !1098
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1099 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1100, metadata !DIExpression()), !dbg !1101
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1102, metadata !DIExpression()), !dbg !1101
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1103, metadata !DIExpression()), !dbg !1101
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1104, metadata !DIExpression()), !dbg !1101
  call void @llvm.va_start(ptr %7), !dbg !1101
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1105, metadata !DIExpression()), !dbg !1101
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1109, metadata !DIExpression()), !dbg !1101
  %13 = load ptr, ptr %6, align 4, !dbg !1101
  %14 = load ptr, ptr %13, align 4, !dbg !1101
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1101
  %16 = load ptr, ptr %15, align 4, !dbg !1101
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1101
  %18 = load ptr, ptr %4, align 4, !dbg !1101
  %19 = load ptr, ptr %6, align 4, !dbg !1101
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1101
  store i32 %20, ptr %9, align 4, !dbg !1101
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1110, metadata !DIExpression()), !dbg !1101
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1112, metadata !DIExpression()), !dbg !1114
  store i32 0, ptr %11, align 4, !dbg !1114
  br label %21, !dbg !1114

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1114
  %23 = load i32, ptr %9, align 4, !dbg !1114
  %24 = icmp slt i32 %22, %23, !dbg !1114
  br i1 %24, label %25, label %103, !dbg !1114

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1115
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1115
  %28 = load i8, ptr %27, align 1, !dbg !1115
  %29 = sext i8 %28 to i32, !dbg !1115
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1115

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1118
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1118
  store ptr %32, ptr %7, align 4, !dbg !1118
  %33 = load i32, ptr %31, align 4, !dbg !1118
  %34 = trunc i32 %33 to i8, !dbg !1118
  %35 = load i32, ptr %11, align 4, !dbg !1118
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1118
  store i8 %34, ptr %36, align 8, !dbg !1118
  br label %99, !dbg !1118

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1118
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1118
  store ptr %39, ptr %7, align 4, !dbg !1118
  %40 = load i32, ptr %38, align 4, !dbg !1118
  %41 = trunc i32 %40 to i8, !dbg !1118
  %42 = load i32, ptr %11, align 4, !dbg !1118
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1118
  store i8 %41, ptr %43, align 8, !dbg !1118
  br label %99, !dbg !1118

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1118
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1118
  store ptr %46, ptr %7, align 4, !dbg !1118
  %47 = load i32, ptr %45, align 4, !dbg !1118
  %48 = trunc i32 %47 to i16, !dbg !1118
  %49 = load i32, ptr %11, align 4, !dbg !1118
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1118
  store i16 %48, ptr %50, align 8, !dbg !1118
  br label %99, !dbg !1118

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1118
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1118
  store ptr %53, ptr %7, align 4, !dbg !1118
  %54 = load i32, ptr %52, align 4, !dbg !1118
  %55 = trunc i32 %54 to i16, !dbg !1118
  %56 = load i32, ptr %11, align 4, !dbg !1118
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1118
  store i16 %55, ptr %57, align 8, !dbg !1118
  br label %99, !dbg !1118

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1118
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1118
  store ptr %60, ptr %7, align 4, !dbg !1118
  %61 = load i32, ptr %59, align 4, !dbg !1118
  %62 = load i32, ptr %11, align 4, !dbg !1118
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1118
  store i32 %61, ptr %63, align 8, !dbg !1118
  br label %99, !dbg !1118

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1118
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1118
  store ptr %66, ptr %7, align 4, !dbg !1118
  %67 = load i32, ptr %65, align 4, !dbg !1118
  %68 = sext i32 %67 to i64, !dbg !1118
  %69 = load i32, ptr %11, align 4, !dbg !1118
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1118
  store i64 %68, ptr %70, align 8, !dbg !1118
  br label %99, !dbg !1118

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1118
  %73 = ptrtoint ptr %72 to i32, !dbg !1118
  %74 = add i32 %73, 7, !dbg !1118
  %75 = and i32 %74, -8, !dbg !1118
  %76 = inttoptr i32 %75 to ptr, !dbg !1118
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1118
  store ptr %77, ptr %7, align 4, !dbg !1118
  %78 = load double, ptr %76, align 8, !dbg !1118
  %79 = fptrunc double %78 to float, !dbg !1118
  %80 = load i32, ptr %11, align 4, !dbg !1118
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1118
  store float %79, ptr %81, align 8, !dbg !1118
  br label %99, !dbg !1118

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1118
  %84 = ptrtoint ptr %83 to i32, !dbg !1118
  %85 = add i32 %84, 7, !dbg !1118
  %86 = and i32 %85, -8, !dbg !1118
  %87 = inttoptr i32 %86 to ptr, !dbg !1118
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1118
  store ptr %88, ptr %7, align 4, !dbg !1118
  %89 = load double, ptr %87, align 8, !dbg !1118
  %90 = load i32, ptr %11, align 4, !dbg !1118
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1118
  store double %89, ptr %91, align 8, !dbg !1118
  br label %99, !dbg !1118

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1118
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1118
  store ptr %94, ptr %7, align 4, !dbg !1118
  %95 = load ptr, ptr %93, align 4, !dbg !1118
  %96 = load i32, ptr %11, align 4, !dbg !1118
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1118
  store ptr %95, ptr %97, align 8, !dbg !1118
  br label %99, !dbg !1118

98:                                               ; preds = %25
  br label %99, !dbg !1118

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1115

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1120
  %102 = add nsw i32 %101, 1, !dbg !1120
  store i32 %102, ptr %11, align 4, !dbg !1120
  br label %21, !dbg !1120, !llvm.loop !1121

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1123, metadata !DIExpression()), !dbg !1101
  %104 = load ptr, ptr %6, align 4, !dbg !1101
  %105 = load ptr, ptr %104, align 4, !dbg !1101
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 36, !dbg !1101
  %107 = load ptr, ptr %106, align 4, !dbg !1101
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1101
  %109 = load ptr, ptr %4, align 4, !dbg !1101
  %110 = load ptr, ptr %5, align 4, !dbg !1101
  %111 = load ptr, ptr %6, align 4, !dbg !1101
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1101
  store ptr %112, ptr %12, align 4, !dbg !1101
  call void @llvm.va_end(ptr %7), !dbg !1101
  %113 = load ptr, ptr %12, align 4, !dbg !1101
  ret ptr %113, !dbg !1101
}

; Function Attrs: nocallback nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1124 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1125, metadata !DIExpression()), !dbg !1126
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1127, metadata !DIExpression()), !dbg !1126
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1128, metadata !DIExpression()), !dbg !1126
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1129, metadata !DIExpression()), !dbg !1126
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1130, metadata !DIExpression()), !dbg !1126
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1131, metadata !DIExpression()), !dbg !1126
  %13 = load ptr, ptr %8, align 4, !dbg !1126
  %14 = load ptr, ptr %13, align 4, !dbg !1126
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1126
  %16 = load ptr, ptr %15, align 4, !dbg !1126
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1126
  %18 = load ptr, ptr %6, align 4, !dbg !1126
  %19 = load ptr, ptr %8, align 4, !dbg !1126
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1126
  store i32 %20, ptr %10, align 4, !dbg !1126
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1132, metadata !DIExpression()), !dbg !1126
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1133, metadata !DIExpression()), !dbg !1135
  store i32 0, ptr %12, align 4, !dbg !1135
  br label %21, !dbg !1135

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1135
  %23 = load i32, ptr %10, align 4, !dbg !1135
  %24 = icmp slt i32 %22, %23, !dbg !1135
  br i1 %24, label %25, label %103, !dbg !1135

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1136
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1136
  %28 = load i8, ptr %27, align 1, !dbg !1136
  %29 = sext i8 %28 to i32, !dbg !1136
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1136

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1139
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1139
  store ptr %32, ptr %5, align 4, !dbg !1139
  %33 = load i32, ptr %31, align 4, !dbg !1139
  %34 = trunc i32 %33 to i8, !dbg !1139
  %35 = load i32, ptr %12, align 4, !dbg !1139
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1139
  store i8 %34, ptr %36, align 8, !dbg !1139
  br label %99, !dbg !1139

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1139
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1139
  store ptr %39, ptr %5, align 4, !dbg !1139
  %40 = load i32, ptr %38, align 4, !dbg !1139
  %41 = trunc i32 %40 to i8, !dbg !1139
  %42 = load i32, ptr %12, align 4, !dbg !1139
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1139
  store i8 %41, ptr %43, align 8, !dbg !1139
  br label %99, !dbg !1139

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1139
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1139
  store ptr %46, ptr %5, align 4, !dbg !1139
  %47 = load i32, ptr %45, align 4, !dbg !1139
  %48 = trunc i32 %47 to i16, !dbg !1139
  %49 = load i32, ptr %12, align 4, !dbg !1139
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1139
  store i16 %48, ptr %50, align 8, !dbg !1139
  br label %99, !dbg !1139

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1139
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1139
  store ptr %53, ptr %5, align 4, !dbg !1139
  %54 = load i32, ptr %52, align 4, !dbg !1139
  %55 = trunc i32 %54 to i16, !dbg !1139
  %56 = load i32, ptr %12, align 4, !dbg !1139
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1139
  store i16 %55, ptr %57, align 8, !dbg !1139
  br label %99, !dbg !1139

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1139
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1139
  store ptr %60, ptr %5, align 4, !dbg !1139
  %61 = load i32, ptr %59, align 4, !dbg !1139
  %62 = load i32, ptr %12, align 4, !dbg !1139
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1139
  store i32 %61, ptr %63, align 8, !dbg !1139
  br label %99, !dbg !1139

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1139
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1139
  store ptr %66, ptr %5, align 4, !dbg !1139
  %67 = load i32, ptr %65, align 4, !dbg !1139
  %68 = sext i32 %67 to i64, !dbg !1139
  %69 = load i32, ptr %12, align 4, !dbg !1139
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1139
  store i64 %68, ptr %70, align 8, !dbg !1139
  br label %99, !dbg !1139

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1139
  %73 = ptrtoint ptr %72 to i32, !dbg !1139
  %74 = add i32 %73, 7, !dbg !1139
  %75 = and i32 %74, -8, !dbg !1139
  %76 = inttoptr i32 %75 to ptr, !dbg !1139
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1139
  store ptr %77, ptr %5, align 4, !dbg !1139
  %78 = load double, ptr %76, align 8, !dbg !1139
  %79 = fptrunc double %78 to float, !dbg !1139
  %80 = load i32, ptr %12, align 4, !dbg !1139
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1139
  store float %79, ptr %81, align 8, !dbg !1139
  br label %99, !dbg !1139

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1139
  %84 = ptrtoint ptr %83 to i32, !dbg !1139
  %85 = add i32 %84, 7, !dbg !1139
  %86 = and i32 %85, -8, !dbg !1139
  %87 = inttoptr i32 %86 to ptr, !dbg !1139
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1139
  store ptr %88, ptr %5, align 4, !dbg !1139
  %89 = load double, ptr %87, align 8, !dbg !1139
  %90 = load i32, ptr %12, align 4, !dbg !1139
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1139
  store double %89, ptr %91, align 8, !dbg !1139
  br label %99, !dbg !1139

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1139
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1139
  store ptr %94, ptr %5, align 4, !dbg !1139
  %95 = load ptr, ptr %93, align 4, !dbg !1139
  %96 = load i32, ptr %12, align 4, !dbg !1139
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1139
  store ptr %95, ptr %97, align 8, !dbg !1139
  br label %99, !dbg !1139

98:                                               ; preds = %25
  br label %99, !dbg !1139

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1136

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1141
  %102 = add nsw i32 %101, 1, !dbg !1141
  store i32 %102, ptr %12, align 4, !dbg !1141
  br label %21, !dbg !1141, !llvm.loop !1142

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1126
  %105 = load ptr, ptr %104, align 4, !dbg !1126
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 36, !dbg !1126
  %107 = load ptr, ptr %106, align 4, !dbg !1126
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1126
  %109 = load ptr, ptr %6, align 4, !dbg !1126
  %110 = load ptr, ptr %7, align 4, !dbg !1126
  %111 = load ptr, ptr %8, align 4, !dbg !1126
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1126
  ret ptr %112, !dbg !1126
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1143 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1144, metadata !DIExpression()), !dbg !1145
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1146, metadata !DIExpression()), !dbg !1145
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1147, metadata !DIExpression()), !dbg !1145
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1148, metadata !DIExpression()), !dbg !1145
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1149, metadata !DIExpression()), !dbg !1145
  call void @llvm.va_start(ptr %9), !dbg !1145
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1150, metadata !DIExpression()), !dbg !1145
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1151, metadata !DIExpression()), !dbg !1145
  %15 = load ptr, ptr %8, align 4, !dbg !1145
  %16 = load ptr, ptr %15, align 4, !dbg !1145
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1145
  %18 = load ptr, ptr %17, align 4, !dbg !1145
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1145
  %20 = load ptr, ptr %5, align 4, !dbg !1145
  %21 = load ptr, ptr %8, align 4, !dbg !1145
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1145
  store i32 %22, ptr %11, align 4, !dbg !1145
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1152, metadata !DIExpression()), !dbg !1145
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1153, metadata !DIExpression()), !dbg !1155
  store i32 0, ptr %13, align 4, !dbg !1155
  br label %23, !dbg !1155

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1155
  %25 = load i32, ptr %11, align 4, !dbg !1155
  %26 = icmp slt i32 %24, %25, !dbg !1155
  br i1 %26, label %27, label %105, !dbg !1155

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1156
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1156
  %30 = load i8, ptr %29, align 1, !dbg !1156
  %31 = sext i8 %30 to i32, !dbg !1156
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1156

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1159
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1159
  store ptr %34, ptr %9, align 4, !dbg !1159
  %35 = load i32, ptr %33, align 4, !dbg !1159
  %36 = trunc i32 %35 to i8, !dbg !1159
  %37 = load i32, ptr %13, align 4, !dbg !1159
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1159
  store i8 %36, ptr %38, align 8, !dbg !1159
  br label %101, !dbg !1159

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1159
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1159
  store ptr %41, ptr %9, align 4, !dbg !1159
  %42 = load i32, ptr %40, align 4, !dbg !1159
  %43 = trunc i32 %42 to i8, !dbg !1159
  %44 = load i32, ptr %13, align 4, !dbg !1159
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1159
  store i8 %43, ptr %45, align 8, !dbg !1159
  br label %101, !dbg !1159

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1159
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1159
  store ptr %48, ptr %9, align 4, !dbg !1159
  %49 = load i32, ptr %47, align 4, !dbg !1159
  %50 = trunc i32 %49 to i16, !dbg !1159
  %51 = load i32, ptr %13, align 4, !dbg !1159
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1159
  store i16 %50, ptr %52, align 8, !dbg !1159
  br label %101, !dbg !1159

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1159
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1159
  store ptr %55, ptr %9, align 4, !dbg !1159
  %56 = load i32, ptr %54, align 4, !dbg !1159
  %57 = trunc i32 %56 to i16, !dbg !1159
  %58 = load i32, ptr %13, align 4, !dbg !1159
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1159
  store i16 %57, ptr %59, align 8, !dbg !1159
  br label %101, !dbg !1159

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1159
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1159
  store ptr %62, ptr %9, align 4, !dbg !1159
  %63 = load i32, ptr %61, align 4, !dbg !1159
  %64 = load i32, ptr %13, align 4, !dbg !1159
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1159
  store i32 %63, ptr %65, align 8, !dbg !1159
  br label %101, !dbg !1159

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1159
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1159
  store ptr %68, ptr %9, align 4, !dbg !1159
  %69 = load i32, ptr %67, align 4, !dbg !1159
  %70 = sext i32 %69 to i64, !dbg !1159
  %71 = load i32, ptr %13, align 4, !dbg !1159
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1159
  store i64 %70, ptr %72, align 8, !dbg !1159
  br label %101, !dbg !1159

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1159
  %75 = ptrtoint ptr %74 to i32, !dbg !1159
  %76 = add i32 %75, 7, !dbg !1159
  %77 = and i32 %76, -8, !dbg !1159
  %78 = inttoptr i32 %77 to ptr, !dbg !1159
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1159
  store ptr %79, ptr %9, align 4, !dbg !1159
  %80 = load double, ptr %78, align 8, !dbg !1159
  %81 = fptrunc double %80 to float, !dbg !1159
  %82 = load i32, ptr %13, align 4, !dbg !1159
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1159
  store float %81, ptr %83, align 8, !dbg !1159
  br label %101, !dbg !1159

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1159
  %86 = ptrtoint ptr %85 to i32, !dbg !1159
  %87 = add i32 %86, 7, !dbg !1159
  %88 = and i32 %87, -8, !dbg !1159
  %89 = inttoptr i32 %88 to ptr, !dbg !1159
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1159
  store ptr %90, ptr %9, align 4, !dbg !1159
  %91 = load double, ptr %89, align 8, !dbg !1159
  %92 = load i32, ptr %13, align 4, !dbg !1159
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1159
  store double %91, ptr %93, align 8, !dbg !1159
  br label %101, !dbg !1159

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1159
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1159
  store ptr %96, ptr %9, align 4, !dbg !1159
  %97 = load ptr, ptr %95, align 4, !dbg !1159
  %98 = load i32, ptr %13, align 4, !dbg !1159
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1159
  store ptr %97, ptr %99, align 8, !dbg !1159
  br label %101, !dbg !1159

100:                                              ; preds = %27
  br label %101, !dbg !1159

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1156

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1161
  %104 = add nsw i32 %103, 1, !dbg !1161
  store i32 %104, ptr %13, align 4, !dbg !1161
  br label %23, !dbg !1161, !llvm.loop !1162

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1163, metadata !DIExpression()), !dbg !1145
  %106 = load ptr, ptr %8, align 4, !dbg !1145
  %107 = load ptr, ptr %106, align 4, !dbg !1145
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 66, !dbg !1145
  %109 = load ptr, ptr %108, align 4, !dbg !1145
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1145
  %111 = load ptr, ptr %5, align 4, !dbg !1145
  %112 = load ptr, ptr %6, align 4, !dbg !1145
  %113 = load ptr, ptr %7, align 4, !dbg !1145
  %114 = load ptr, ptr %8, align 4, !dbg !1145
  %115 = call arm_aapcs_vfpcc ptr %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1145
  store ptr %115, ptr %14, align 4, !dbg !1145
  call void @llvm.va_end(ptr %9), !dbg !1145
  %116 = load ptr, ptr %14, align 4, !dbg !1145
  ret ptr %116, !dbg !1145
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1164 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1165, metadata !DIExpression()), !dbg !1166
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1167, metadata !DIExpression()), !dbg !1166
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1168, metadata !DIExpression()), !dbg !1166
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1169, metadata !DIExpression()), !dbg !1166
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1170, metadata !DIExpression()), !dbg !1166
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1171, metadata !DIExpression()), !dbg !1166
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1172, metadata !DIExpression()), !dbg !1166
  %15 = load ptr, ptr %10, align 4, !dbg !1166
  %16 = load ptr, ptr %15, align 4, !dbg !1166
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1166
  %18 = load ptr, ptr %17, align 4, !dbg !1166
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1166
  %20 = load ptr, ptr %7, align 4, !dbg !1166
  %21 = load ptr, ptr %10, align 4, !dbg !1166
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1166
  store i32 %22, ptr %12, align 4, !dbg !1166
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1173, metadata !DIExpression()), !dbg !1166
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1174, metadata !DIExpression()), !dbg !1176
  store i32 0, ptr %14, align 4, !dbg !1176
  br label %23, !dbg !1176

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1176
  %25 = load i32, ptr %12, align 4, !dbg !1176
  %26 = icmp slt i32 %24, %25, !dbg !1176
  br i1 %26, label %27, label %105, !dbg !1176

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1177
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1177
  %30 = load i8, ptr %29, align 1, !dbg !1177
  %31 = sext i8 %30 to i32, !dbg !1177
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1177

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1180
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1180
  store ptr %34, ptr %6, align 4, !dbg !1180
  %35 = load i32, ptr %33, align 4, !dbg !1180
  %36 = trunc i32 %35 to i8, !dbg !1180
  %37 = load i32, ptr %14, align 4, !dbg !1180
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1180
  store i8 %36, ptr %38, align 8, !dbg !1180
  br label %101, !dbg !1180

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1180
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1180
  store ptr %41, ptr %6, align 4, !dbg !1180
  %42 = load i32, ptr %40, align 4, !dbg !1180
  %43 = trunc i32 %42 to i8, !dbg !1180
  %44 = load i32, ptr %14, align 4, !dbg !1180
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1180
  store i8 %43, ptr %45, align 8, !dbg !1180
  br label %101, !dbg !1180

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1180
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1180
  store ptr %48, ptr %6, align 4, !dbg !1180
  %49 = load i32, ptr %47, align 4, !dbg !1180
  %50 = trunc i32 %49 to i16, !dbg !1180
  %51 = load i32, ptr %14, align 4, !dbg !1180
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1180
  store i16 %50, ptr %52, align 8, !dbg !1180
  br label %101, !dbg !1180

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1180
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1180
  store ptr %55, ptr %6, align 4, !dbg !1180
  %56 = load i32, ptr %54, align 4, !dbg !1180
  %57 = trunc i32 %56 to i16, !dbg !1180
  %58 = load i32, ptr %14, align 4, !dbg !1180
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1180
  store i16 %57, ptr %59, align 8, !dbg !1180
  br label %101, !dbg !1180

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1180
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1180
  store ptr %62, ptr %6, align 4, !dbg !1180
  %63 = load i32, ptr %61, align 4, !dbg !1180
  %64 = load i32, ptr %14, align 4, !dbg !1180
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1180
  store i32 %63, ptr %65, align 8, !dbg !1180
  br label %101, !dbg !1180

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1180
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1180
  store ptr %68, ptr %6, align 4, !dbg !1180
  %69 = load i32, ptr %67, align 4, !dbg !1180
  %70 = sext i32 %69 to i64, !dbg !1180
  %71 = load i32, ptr %14, align 4, !dbg !1180
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1180
  store i64 %70, ptr %72, align 8, !dbg !1180
  br label %101, !dbg !1180

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1180
  %75 = ptrtoint ptr %74 to i32, !dbg !1180
  %76 = add i32 %75, 7, !dbg !1180
  %77 = and i32 %76, -8, !dbg !1180
  %78 = inttoptr i32 %77 to ptr, !dbg !1180
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1180
  store ptr %79, ptr %6, align 4, !dbg !1180
  %80 = load double, ptr %78, align 8, !dbg !1180
  %81 = fptrunc double %80 to float, !dbg !1180
  %82 = load i32, ptr %14, align 4, !dbg !1180
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1180
  store float %81, ptr %83, align 8, !dbg !1180
  br label %101, !dbg !1180

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1180
  %86 = ptrtoint ptr %85 to i32, !dbg !1180
  %87 = add i32 %86, 7, !dbg !1180
  %88 = and i32 %87, -8, !dbg !1180
  %89 = inttoptr i32 %88 to ptr, !dbg !1180
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1180
  store ptr %90, ptr %6, align 4, !dbg !1180
  %91 = load double, ptr %89, align 8, !dbg !1180
  %92 = load i32, ptr %14, align 4, !dbg !1180
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1180
  store double %91, ptr %93, align 8, !dbg !1180
  br label %101, !dbg !1180

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1180
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1180
  store ptr %96, ptr %6, align 4, !dbg !1180
  %97 = load ptr, ptr %95, align 4, !dbg !1180
  %98 = load i32, ptr %14, align 4, !dbg !1180
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1180
  store ptr %97, ptr %99, align 8, !dbg !1180
  br label %101, !dbg !1180

100:                                              ; preds = %27
  br label %101, !dbg !1180

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1177

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1182
  %104 = add nsw i32 %103, 1, !dbg !1182
  store i32 %104, ptr %14, align 4, !dbg !1182
  br label %23, !dbg !1182, !llvm.loop !1183

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1166
  %107 = load ptr, ptr %106, align 4, !dbg !1166
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 66, !dbg !1166
  %109 = load ptr, ptr %108, align 4, !dbg !1166
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1166
  %111 = load ptr, ptr %7, align 4, !dbg !1166
  %112 = load ptr, ptr %8, align 4, !dbg !1166
  %113 = load ptr, ptr %9, align 4, !dbg !1166
  %114 = load ptr, ptr %10, align 4, !dbg !1166
  %115 = call arm_aapcs_vfpcc ptr %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1166
  ret ptr %115, !dbg !1166
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1184 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1185, metadata !DIExpression()), !dbg !1186
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1187, metadata !DIExpression()), !dbg !1186
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1188, metadata !DIExpression()), !dbg !1186
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1189, metadata !DIExpression()), !dbg !1186
  call void @llvm.va_start(ptr %7), !dbg !1186
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1190, metadata !DIExpression()), !dbg !1186
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1191, metadata !DIExpression()), !dbg !1186
  %13 = load ptr, ptr %6, align 4, !dbg !1186
  %14 = load ptr, ptr %13, align 4, !dbg !1186
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1186
  %16 = load ptr, ptr %15, align 4, !dbg !1186
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1186
  %18 = load ptr, ptr %4, align 4, !dbg !1186
  %19 = load ptr, ptr %6, align 4, !dbg !1186
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1186
  store i32 %20, ptr %9, align 4, !dbg !1186
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1192, metadata !DIExpression()), !dbg !1186
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1193, metadata !DIExpression()), !dbg !1195
  store i32 0, ptr %11, align 4, !dbg !1195
  br label %21, !dbg !1195

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1195
  %23 = load i32, ptr %9, align 4, !dbg !1195
  %24 = icmp slt i32 %22, %23, !dbg !1195
  br i1 %24, label %25, label %103, !dbg !1195

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1196
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1196
  %28 = load i8, ptr %27, align 1, !dbg !1196
  %29 = sext i8 %28 to i32, !dbg !1196
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1196

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1199
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1199
  store ptr %32, ptr %7, align 4, !dbg !1199
  %33 = load i32, ptr %31, align 4, !dbg !1199
  %34 = trunc i32 %33 to i8, !dbg !1199
  %35 = load i32, ptr %11, align 4, !dbg !1199
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1199
  store i8 %34, ptr %36, align 8, !dbg !1199
  br label %99, !dbg !1199

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1199
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1199
  store ptr %39, ptr %7, align 4, !dbg !1199
  %40 = load i32, ptr %38, align 4, !dbg !1199
  %41 = trunc i32 %40 to i8, !dbg !1199
  %42 = load i32, ptr %11, align 4, !dbg !1199
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1199
  store i8 %41, ptr %43, align 8, !dbg !1199
  br label %99, !dbg !1199

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1199
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1199
  store ptr %46, ptr %7, align 4, !dbg !1199
  %47 = load i32, ptr %45, align 4, !dbg !1199
  %48 = trunc i32 %47 to i16, !dbg !1199
  %49 = load i32, ptr %11, align 4, !dbg !1199
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1199
  store i16 %48, ptr %50, align 8, !dbg !1199
  br label %99, !dbg !1199

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1199
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1199
  store ptr %53, ptr %7, align 4, !dbg !1199
  %54 = load i32, ptr %52, align 4, !dbg !1199
  %55 = trunc i32 %54 to i16, !dbg !1199
  %56 = load i32, ptr %11, align 4, !dbg !1199
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1199
  store i16 %55, ptr %57, align 8, !dbg !1199
  br label %99, !dbg !1199

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1199
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1199
  store ptr %60, ptr %7, align 4, !dbg !1199
  %61 = load i32, ptr %59, align 4, !dbg !1199
  %62 = load i32, ptr %11, align 4, !dbg !1199
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1199
  store i32 %61, ptr %63, align 8, !dbg !1199
  br label %99, !dbg !1199

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1199
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1199
  store ptr %66, ptr %7, align 4, !dbg !1199
  %67 = load i32, ptr %65, align 4, !dbg !1199
  %68 = sext i32 %67 to i64, !dbg !1199
  %69 = load i32, ptr %11, align 4, !dbg !1199
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1199
  store i64 %68, ptr %70, align 8, !dbg !1199
  br label %99, !dbg !1199

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1199
  %73 = ptrtoint ptr %72 to i32, !dbg !1199
  %74 = add i32 %73, 7, !dbg !1199
  %75 = and i32 %74, -8, !dbg !1199
  %76 = inttoptr i32 %75 to ptr, !dbg !1199
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1199
  store ptr %77, ptr %7, align 4, !dbg !1199
  %78 = load double, ptr %76, align 8, !dbg !1199
  %79 = fptrunc double %78 to float, !dbg !1199
  %80 = load i32, ptr %11, align 4, !dbg !1199
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1199
  store float %79, ptr %81, align 8, !dbg !1199
  br label %99, !dbg !1199

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1199
  %84 = ptrtoint ptr %83 to i32, !dbg !1199
  %85 = add i32 %84, 7, !dbg !1199
  %86 = and i32 %85, -8, !dbg !1199
  %87 = inttoptr i32 %86 to ptr, !dbg !1199
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1199
  store ptr %88, ptr %7, align 4, !dbg !1199
  %89 = load double, ptr %87, align 8, !dbg !1199
  %90 = load i32, ptr %11, align 4, !dbg !1199
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1199
  store double %89, ptr %91, align 8, !dbg !1199
  br label %99, !dbg !1199

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1199
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1199
  store ptr %94, ptr %7, align 4, !dbg !1199
  %95 = load ptr, ptr %93, align 4, !dbg !1199
  %96 = load i32, ptr %11, align 4, !dbg !1199
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1199
  store ptr %95, ptr %97, align 8, !dbg !1199
  br label %99, !dbg !1199

98:                                               ; preds = %25
  br label %99, !dbg !1199

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1196

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1201
  %102 = add nsw i32 %101, 1, !dbg !1201
  store i32 %102, ptr %11, align 4, !dbg !1201
  br label %21, !dbg !1201, !llvm.loop !1202

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1203, metadata !DIExpression()), !dbg !1186
  %104 = load ptr, ptr %6, align 4, !dbg !1186
  %105 = load ptr, ptr %104, align 4, !dbg !1186
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 116, !dbg !1186
  %107 = load ptr, ptr %106, align 4, !dbg !1186
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1186
  %109 = load ptr, ptr %4, align 4, !dbg !1186
  %110 = load ptr, ptr %5, align 4, !dbg !1186
  %111 = load ptr, ptr %6, align 4, !dbg !1186
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1186
  store ptr %112, ptr %12, align 4, !dbg !1186
  call void @llvm.va_end(ptr %7), !dbg !1186
  %113 = load ptr, ptr %12, align 4, !dbg !1186
  ret ptr %113, !dbg !1186
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1204 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1205, metadata !DIExpression()), !dbg !1206
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1207, metadata !DIExpression()), !dbg !1206
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1208, metadata !DIExpression()), !dbg !1206
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1209, metadata !DIExpression()), !dbg !1206
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1210, metadata !DIExpression()), !dbg !1206
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1211, metadata !DIExpression()), !dbg !1206
  %13 = load ptr, ptr %8, align 4, !dbg !1206
  %14 = load ptr, ptr %13, align 4, !dbg !1206
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1206
  %16 = load ptr, ptr %15, align 4, !dbg !1206
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1206
  %18 = load ptr, ptr %6, align 4, !dbg !1206
  %19 = load ptr, ptr %8, align 4, !dbg !1206
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1206
  store i32 %20, ptr %10, align 4, !dbg !1206
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1212, metadata !DIExpression()), !dbg !1206
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1213, metadata !DIExpression()), !dbg !1215
  store i32 0, ptr %12, align 4, !dbg !1215
  br label %21, !dbg !1215

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1215
  %23 = load i32, ptr %10, align 4, !dbg !1215
  %24 = icmp slt i32 %22, %23, !dbg !1215
  br i1 %24, label %25, label %103, !dbg !1215

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1216
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1216
  %28 = load i8, ptr %27, align 1, !dbg !1216
  %29 = sext i8 %28 to i32, !dbg !1216
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1216

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1219
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1219
  store ptr %32, ptr %5, align 4, !dbg !1219
  %33 = load i32, ptr %31, align 4, !dbg !1219
  %34 = trunc i32 %33 to i8, !dbg !1219
  %35 = load i32, ptr %12, align 4, !dbg !1219
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1219
  store i8 %34, ptr %36, align 8, !dbg !1219
  br label %99, !dbg !1219

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1219
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1219
  store ptr %39, ptr %5, align 4, !dbg !1219
  %40 = load i32, ptr %38, align 4, !dbg !1219
  %41 = trunc i32 %40 to i8, !dbg !1219
  %42 = load i32, ptr %12, align 4, !dbg !1219
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1219
  store i8 %41, ptr %43, align 8, !dbg !1219
  br label %99, !dbg !1219

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1219
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1219
  store ptr %46, ptr %5, align 4, !dbg !1219
  %47 = load i32, ptr %45, align 4, !dbg !1219
  %48 = trunc i32 %47 to i16, !dbg !1219
  %49 = load i32, ptr %12, align 4, !dbg !1219
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1219
  store i16 %48, ptr %50, align 8, !dbg !1219
  br label %99, !dbg !1219

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1219
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1219
  store ptr %53, ptr %5, align 4, !dbg !1219
  %54 = load i32, ptr %52, align 4, !dbg !1219
  %55 = trunc i32 %54 to i16, !dbg !1219
  %56 = load i32, ptr %12, align 4, !dbg !1219
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1219
  store i16 %55, ptr %57, align 8, !dbg !1219
  br label %99, !dbg !1219

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1219
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1219
  store ptr %60, ptr %5, align 4, !dbg !1219
  %61 = load i32, ptr %59, align 4, !dbg !1219
  %62 = load i32, ptr %12, align 4, !dbg !1219
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1219
  store i32 %61, ptr %63, align 8, !dbg !1219
  br label %99, !dbg !1219

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1219
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1219
  store ptr %66, ptr %5, align 4, !dbg !1219
  %67 = load i32, ptr %65, align 4, !dbg !1219
  %68 = sext i32 %67 to i64, !dbg !1219
  %69 = load i32, ptr %12, align 4, !dbg !1219
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1219
  store i64 %68, ptr %70, align 8, !dbg !1219
  br label %99, !dbg !1219

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1219
  %73 = ptrtoint ptr %72 to i32, !dbg !1219
  %74 = add i32 %73, 7, !dbg !1219
  %75 = and i32 %74, -8, !dbg !1219
  %76 = inttoptr i32 %75 to ptr, !dbg !1219
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1219
  store ptr %77, ptr %5, align 4, !dbg !1219
  %78 = load double, ptr %76, align 8, !dbg !1219
  %79 = fptrunc double %78 to float, !dbg !1219
  %80 = load i32, ptr %12, align 4, !dbg !1219
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1219
  store float %79, ptr %81, align 8, !dbg !1219
  br label %99, !dbg !1219

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1219
  %84 = ptrtoint ptr %83 to i32, !dbg !1219
  %85 = add i32 %84, 7, !dbg !1219
  %86 = and i32 %85, -8, !dbg !1219
  %87 = inttoptr i32 %86 to ptr, !dbg !1219
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1219
  store ptr %88, ptr %5, align 4, !dbg !1219
  %89 = load double, ptr %87, align 8, !dbg !1219
  %90 = load i32, ptr %12, align 4, !dbg !1219
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1219
  store double %89, ptr %91, align 8, !dbg !1219
  br label %99, !dbg !1219

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1219
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1219
  store ptr %94, ptr %5, align 4, !dbg !1219
  %95 = load ptr, ptr %93, align 4, !dbg !1219
  %96 = load i32, ptr %12, align 4, !dbg !1219
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1219
  store ptr %95, ptr %97, align 8, !dbg !1219
  br label %99, !dbg !1219

98:                                               ; preds = %25
  br label %99, !dbg !1219

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1216

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1221
  %102 = add nsw i32 %101, 1, !dbg !1221
  store i32 %102, ptr %12, align 4, !dbg !1221
  br label %21, !dbg !1221, !llvm.loop !1222

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1206
  %105 = load ptr, ptr %104, align 4, !dbg !1206
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 116, !dbg !1206
  %107 = load ptr, ptr %106, align 4, !dbg !1206
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1206
  %109 = load ptr, ptr %6, align 4, !dbg !1206
  %110 = load ptr, ptr %7, align 4, !dbg !1206
  %111 = load ptr, ptr %8, align 4, !dbg !1206
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1206
  ret ptr %112, !dbg !1206
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1223 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1224, metadata !DIExpression()), !dbg !1225
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1226, metadata !DIExpression()), !dbg !1225
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1227, metadata !DIExpression()), !dbg !1225
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1228, metadata !DIExpression()), !dbg !1225
  call void @llvm.va_start(ptr %7), !dbg !1225
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1229, metadata !DIExpression()), !dbg !1225
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1230, metadata !DIExpression()), !dbg !1225
  %13 = load ptr, ptr %6, align 4, !dbg !1225
  %14 = load ptr, ptr %13, align 4, !dbg !1225
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1225
  %16 = load ptr, ptr %15, align 4, !dbg !1225
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1225
  %18 = load ptr, ptr %4, align 4, !dbg !1225
  %19 = load ptr, ptr %6, align 4, !dbg !1225
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1225
  store i32 %20, ptr %9, align 4, !dbg !1225
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1231, metadata !DIExpression()), !dbg !1225
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1232, metadata !DIExpression()), !dbg !1234
  store i32 0, ptr %11, align 4, !dbg !1234
  br label %21, !dbg !1234

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1234
  %23 = load i32, ptr %9, align 4, !dbg !1234
  %24 = icmp slt i32 %22, %23, !dbg !1234
  br i1 %24, label %25, label %103, !dbg !1234

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1235
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1235
  %28 = load i8, ptr %27, align 1, !dbg !1235
  %29 = sext i8 %28 to i32, !dbg !1235
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1235

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1238
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1238
  store ptr %32, ptr %7, align 4, !dbg !1238
  %33 = load i32, ptr %31, align 4, !dbg !1238
  %34 = trunc i32 %33 to i8, !dbg !1238
  %35 = load i32, ptr %11, align 4, !dbg !1238
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1238
  store i8 %34, ptr %36, align 8, !dbg !1238
  br label %99, !dbg !1238

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1238
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1238
  store ptr %39, ptr %7, align 4, !dbg !1238
  %40 = load i32, ptr %38, align 4, !dbg !1238
  %41 = trunc i32 %40 to i8, !dbg !1238
  %42 = load i32, ptr %11, align 4, !dbg !1238
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1238
  store i8 %41, ptr %43, align 8, !dbg !1238
  br label %99, !dbg !1238

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1238
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1238
  store ptr %46, ptr %7, align 4, !dbg !1238
  %47 = load i32, ptr %45, align 4, !dbg !1238
  %48 = trunc i32 %47 to i16, !dbg !1238
  %49 = load i32, ptr %11, align 4, !dbg !1238
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1238
  store i16 %48, ptr %50, align 8, !dbg !1238
  br label %99, !dbg !1238

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1238
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1238
  store ptr %53, ptr %7, align 4, !dbg !1238
  %54 = load i32, ptr %52, align 4, !dbg !1238
  %55 = trunc i32 %54 to i16, !dbg !1238
  %56 = load i32, ptr %11, align 4, !dbg !1238
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1238
  store i16 %55, ptr %57, align 8, !dbg !1238
  br label %99, !dbg !1238

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1238
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1238
  store ptr %60, ptr %7, align 4, !dbg !1238
  %61 = load i32, ptr %59, align 4, !dbg !1238
  %62 = load i32, ptr %11, align 4, !dbg !1238
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1238
  store i32 %61, ptr %63, align 8, !dbg !1238
  br label %99, !dbg !1238

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1238
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1238
  store ptr %66, ptr %7, align 4, !dbg !1238
  %67 = load i32, ptr %65, align 4, !dbg !1238
  %68 = sext i32 %67 to i64, !dbg !1238
  %69 = load i32, ptr %11, align 4, !dbg !1238
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1238
  store i64 %68, ptr %70, align 8, !dbg !1238
  br label %99, !dbg !1238

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1238
  %73 = ptrtoint ptr %72 to i32, !dbg !1238
  %74 = add i32 %73, 7, !dbg !1238
  %75 = and i32 %74, -8, !dbg !1238
  %76 = inttoptr i32 %75 to ptr, !dbg !1238
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1238
  store ptr %77, ptr %7, align 4, !dbg !1238
  %78 = load double, ptr %76, align 8, !dbg !1238
  %79 = fptrunc double %78 to float, !dbg !1238
  %80 = load i32, ptr %11, align 4, !dbg !1238
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1238
  store float %79, ptr %81, align 8, !dbg !1238
  br label %99, !dbg !1238

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1238
  %84 = ptrtoint ptr %83 to i32, !dbg !1238
  %85 = add i32 %84, 7, !dbg !1238
  %86 = and i32 %85, -8, !dbg !1238
  %87 = inttoptr i32 %86 to ptr, !dbg !1238
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1238
  store ptr %88, ptr %7, align 4, !dbg !1238
  %89 = load double, ptr %87, align 8, !dbg !1238
  %90 = load i32, ptr %11, align 4, !dbg !1238
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1238
  store double %89, ptr %91, align 8, !dbg !1238
  br label %99, !dbg !1238

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1238
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1238
  store ptr %94, ptr %7, align 4, !dbg !1238
  %95 = load ptr, ptr %93, align 4, !dbg !1238
  %96 = load i32, ptr %11, align 4, !dbg !1238
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1238
  store ptr %95, ptr %97, align 8, !dbg !1238
  br label %99, !dbg !1238

98:                                               ; preds = %25
  br label %99, !dbg !1238

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1235

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1240
  %102 = add nsw i32 %101, 1, !dbg !1240
  store i32 %102, ptr %11, align 4, !dbg !1240
  br label %21, !dbg !1240, !llvm.loop !1241

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1242, metadata !DIExpression()), !dbg !1225
  %104 = load ptr, ptr %6, align 4, !dbg !1225
  %105 = load ptr, ptr %104, align 4, !dbg !1225
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 39, !dbg !1225
  %107 = load ptr, ptr %106, align 4, !dbg !1225
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1225
  %109 = load ptr, ptr %4, align 4, !dbg !1225
  %110 = load ptr, ptr %5, align 4, !dbg !1225
  %111 = load ptr, ptr %6, align 4, !dbg !1225
  %112 = call arm_aapcs_vfpcc zeroext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1225
  store i8 %112, ptr %12, align 1, !dbg !1225
  call void @llvm.va_end(ptr %7), !dbg !1225
  %113 = load i8, ptr %12, align 1, !dbg !1225
  ret i8 %113, !dbg !1225
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1243 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1244, metadata !DIExpression()), !dbg !1245
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1246, metadata !DIExpression()), !dbg !1245
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1247, metadata !DIExpression()), !dbg !1245
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1248, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1249, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1250, metadata !DIExpression()), !dbg !1245
  %13 = load ptr, ptr %8, align 4, !dbg !1245
  %14 = load ptr, ptr %13, align 4, !dbg !1245
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1245
  %16 = load ptr, ptr %15, align 4, !dbg !1245
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1245
  %18 = load ptr, ptr %6, align 4, !dbg !1245
  %19 = load ptr, ptr %8, align 4, !dbg !1245
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1245
  store i32 %20, ptr %10, align 4, !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1251, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1252, metadata !DIExpression()), !dbg !1254
  store i32 0, ptr %12, align 4, !dbg !1254
  br label %21, !dbg !1254

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1254
  %23 = load i32, ptr %10, align 4, !dbg !1254
  %24 = icmp slt i32 %22, %23, !dbg !1254
  br i1 %24, label %25, label %103, !dbg !1254

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1255
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1255
  %28 = load i8, ptr %27, align 1, !dbg !1255
  %29 = sext i8 %28 to i32, !dbg !1255
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1255

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1258
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1258
  store ptr %32, ptr %5, align 4, !dbg !1258
  %33 = load i32, ptr %31, align 4, !dbg !1258
  %34 = trunc i32 %33 to i8, !dbg !1258
  %35 = load i32, ptr %12, align 4, !dbg !1258
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1258
  store i8 %34, ptr %36, align 8, !dbg !1258
  br label %99, !dbg !1258

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1258
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1258
  store ptr %39, ptr %5, align 4, !dbg !1258
  %40 = load i32, ptr %38, align 4, !dbg !1258
  %41 = trunc i32 %40 to i8, !dbg !1258
  %42 = load i32, ptr %12, align 4, !dbg !1258
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1258
  store i8 %41, ptr %43, align 8, !dbg !1258
  br label %99, !dbg !1258

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1258
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1258
  store ptr %46, ptr %5, align 4, !dbg !1258
  %47 = load i32, ptr %45, align 4, !dbg !1258
  %48 = trunc i32 %47 to i16, !dbg !1258
  %49 = load i32, ptr %12, align 4, !dbg !1258
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1258
  store i16 %48, ptr %50, align 8, !dbg !1258
  br label %99, !dbg !1258

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1258
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1258
  store ptr %53, ptr %5, align 4, !dbg !1258
  %54 = load i32, ptr %52, align 4, !dbg !1258
  %55 = trunc i32 %54 to i16, !dbg !1258
  %56 = load i32, ptr %12, align 4, !dbg !1258
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1258
  store i16 %55, ptr %57, align 8, !dbg !1258
  br label %99, !dbg !1258

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1258
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1258
  store ptr %60, ptr %5, align 4, !dbg !1258
  %61 = load i32, ptr %59, align 4, !dbg !1258
  %62 = load i32, ptr %12, align 4, !dbg !1258
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1258
  store i32 %61, ptr %63, align 8, !dbg !1258
  br label %99, !dbg !1258

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1258
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1258
  store ptr %66, ptr %5, align 4, !dbg !1258
  %67 = load i32, ptr %65, align 4, !dbg !1258
  %68 = sext i32 %67 to i64, !dbg !1258
  %69 = load i32, ptr %12, align 4, !dbg !1258
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1258
  store i64 %68, ptr %70, align 8, !dbg !1258
  br label %99, !dbg !1258

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1258
  %73 = ptrtoint ptr %72 to i32, !dbg !1258
  %74 = add i32 %73, 7, !dbg !1258
  %75 = and i32 %74, -8, !dbg !1258
  %76 = inttoptr i32 %75 to ptr, !dbg !1258
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1258
  store ptr %77, ptr %5, align 4, !dbg !1258
  %78 = load double, ptr %76, align 8, !dbg !1258
  %79 = fptrunc double %78 to float, !dbg !1258
  %80 = load i32, ptr %12, align 4, !dbg !1258
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1258
  store float %79, ptr %81, align 8, !dbg !1258
  br label %99, !dbg !1258

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1258
  %84 = ptrtoint ptr %83 to i32, !dbg !1258
  %85 = add i32 %84, 7, !dbg !1258
  %86 = and i32 %85, -8, !dbg !1258
  %87 = inttoptr i32 %86 to ptr, !dbg !1258
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1258
  store ptr %88, ptr %5, align 4, !dbg !1258
  %89 = load double, ptr %87, align 8, !dbg !1258
  %90 = load i32, ptr %12, align 4, !dbg !1258
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1258
  store double %89, ptr %91, align 8, !dbg !1258
  br label %99, !dbg !1258

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1258
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1258
  store ptr %94, ptr %5, align 4, !dbg !1258
  %95 = load ptr, ptr %93, align 4, !dbg !1258
  %96 = load i32, ptr %12, align 4, !dbg !1258
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1258
  store ptr %95, ptr %97, align 8, !dbg !1258
  br label %99, !dbg !1258

98:                                               ; preds = %25
  br label %99, !dbg !1258

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1255

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1260
  %102 = add nsw i32 %101, 1, !dbg !1260
  store i32 %102, ptr %12, align 4, !dbg !1260
  br label %21, !dbg !1260, !llvm.loop !1261

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1245
  %105 = load ptr, ptr %104, align 4, !dbg !1245
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 39, !dbg !1245
  %107 = load ptr, ptr %106, align 4, !dbg !1245
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1245
  %109 = load ptr, ptr %6, align 4, !dbg !1245
  %110 = load ptr, ptr %7, align 4, !dbg !1245
  %111 = load ptr, ptr %8, align 4, !dbg !1245
  %112 = call arm_aapcs_vfpcc zeroext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1245
  ret i8 %112, !dbg !1245
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1262 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1263, metadata !DIExpression()), !dbg !1264
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1265, metadata !DIExpression()), !dbg !1264
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1266, metadata !DIExpression()), !dbg !1264
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1267, metadata !DIExpression()), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1268, metadata !DIExpression()), !dbg !1264
  call void @llvm.va_start(ptr %9), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1269, metadata !DIExpression()), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1270, metadata !DIExpression()), !dbg !1264
  %15 = load ptr, ptr %8, align 4, !dbg !1264
  %16 = load ptr, ptr %15, align 4, !dbg !1264
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1264
  %18 = load ptr, ptr %17, align 4, !dbg !1264
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1264
  %20 = load ptr, ptr %5, align 4, !dbg !1264
  %21 = load ptr, ptr %8, align 4, !dbg !1264
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1264
  store i32 %22, ptr %11, align 4, !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1271, metadata !DIExpression()), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1272, metadata !DIExpression()), !dbg !1274
  store i32 0, ptr %13, align 4, !dbg !1274
  br label %23, !dbg !1274

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1274
  %25 = load i32, ptr %11, align 4, !dbg !1274
  %26 = icmp slt i32 %24, %25, !dbg !1274
  br i1 %26, label %27, label %105, !dbg !1274

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1275
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1275
  %30 = load i8, ptr %29, align 1, !dbg !1275
  %31 = sext i8 %30 to i32, !dbg !1275
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1275

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1278
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1278
  store ptr %34, ptr %9, align 4, !dbg !1278
  %35 = load i32, ptr %33, align 4, !dbg !1278
  %36 = trunc i32 %35 to i8, !dbg !1278
  %37 = load i32, ptr %13, align 4, !dbg !1278
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1278
  store i8 %36, ptr %38, align 8, !dbg !1278
  br label %101, !dbg !1278

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1278
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1278
  store ptr %41, ptr %9, align 4, !dbg !1278
  %42 = load i32, ptr %40, align 4, !dbg !1278
  %43 = trunc i32 %42 to i8, !dbg !1278
  %44 = load i32, ptr %13, align 4, !dbg !1278
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1278
  store i8 %43, ptr %45, align 8, !dbg !1278
  br label %101, !dbg !1278

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1278
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1278
  store ptr %48, ptr %9, align 4, !dbg !1278
  %49 = load i32, ptr %47, align 4, !dbg !1278
  %50 = trunc i32 %49 to i16, !dbg !1278
  %51 = load i32, ptr %13, align 4, !dbg !1278
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1278
  store i16 %50, ptr %52, align 8, !dbg !1278
  br label %101, !dbg !1278

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1278
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1278
  store ptr %55, ptr %9, align 4, !dbg !1278
  %56 = load i32, ptr %54, align 4, !dbg !1278
  %57 = trunc i32 %56 to i16, !dbg !1278
  %58 = load i32, ptr %13, align 4, !dbg !1278
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1278
  store i16 %57, ptr %59, align 8, !dbg !1278
  br label %101, !dbg !1278

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1278
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1278
  store ptr %62, ptr %9, align 4, !dbg !1278
  %63 = load i32, ptr %61, align 4, !dbg !1278
  %64 = load i32, ptr %13, align 4, !dbg !1278
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1278
  store i32 %63, ptr %65, align 8, !dbg !1278
  br label %101, !dbg !1278

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1278
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1278
  store ptr %68, ptr %9, align 4, !dbg !1278
  %69 = load i32, ptr %67, align 4, !dbg !1278
  %70 = sext i32 %69 to i64, !dbg !1278
  %71 = load i32, ptr %13, align 4, !dbg !1278
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1278
  store i64 %70, ptr %72, align 8, !dbg !1278
  br label %101, !dbg !1278

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1278
  %75 = ptrtoint ptr %74 to i32, !dbg !1278
  %76 = add i32 %75, 7, !dbg !1278
  %77 = and i32 %76, -8, !dbg !1278
  %78 = inttoptr i32 %77 to ptr, !dbg !1278
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1278
  store ptr %79, ptr %9, align 4, !dbg !1278
  %80 = load double, ptr %78, align 8, !dbg !1278
  %81 = fptrunc double %80 to float, !dbg !1278
  %82 = load i32, ptr %13, align 4, !dbg !1278
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1278
  store float %81, ptr %83, align 8, !dbg !1278
  br label %101, !dbg !1278

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1278
  %86 = ptrtoint ptr %85 to i32, !dbg !1278
  %87 = add i32 %86, 7, !dbg !1278
  %88 = and i32 %87, -8, !dbg !1278
  %89 = inttoptr i32 %88 to ptr, !dbg !1278
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1278
  store ptr %90, ptr %9, align 4, !dbg !1278
  %91 = load double, ptr %89, align 8, !dbg !1278
  %92 = load i32, ptr %13, align 4, !dbg !1278
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1278
  store double %91, ptr %93, align 8, !dbg !1278
  br label %101, !dbg !1278

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1278
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1278
  store ptr %96, ptr %9, align 4, !dbg !1278
  %97 = load ptr, ptr %95, align 4, !dbg !1278
  %98 = load i32, ptr %13, align 4, !dbg !1278
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1278
  store ptr %97, ptr %99, align 8, !dbg !1278
  br label %101, !dbg !1278

100:                                              ; preds = %27
  br label %101, !dbg !1278

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1275

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1280
  %104 = add nsw i32 %103, 1, !dbg !1280
  store i32 %104, ptr %13, align 4, !dbg !1280
  br label %23, !dbg !1280, !llvm.loop !1281

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1282, metadata !DIExpression()), !dbg !1264
  %106 = load ptr, ptr %8, align 4, !dbg !1264
  %107 = load ptr, ptr %106, align 4, !dbg !1264
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 69, !dbg !1264
  %109 = load ptr, ptr %108, align 4, !dbg !1264
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1264
  %111 = load ptr, ptr %5, align 4, !dbg !1264
  %112 = load ptr, ptr %6, align 4, !dbg !1264
  %113 = load ptr, ptr %7, align 4, !dbg !1264
  %114 = load ptr, ptr %8, align 4, !dbg !1264
  %115 = call arm_aapcs_vfpcc zeroext i8 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1264
  store i8 %115, ptr %14, align 1, !dbg !1264
  call void @llvm.va_end(ptr %9), !dbg !1264
  %116 = load i8, ptr %14, align 1, !dbg !1264
  ret i8 %116, !dbg !1264
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1283 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1284, metadata !DIExpression()), !dbg !1285
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1286, metadata !DIExpression()), !dbg !1285
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1287, metadata !DIExpression()), !dbg !1285
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1288, metadata !DIExpression()), !dbg !1285
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1289, metadata !DIExpression()), !dbg !1285
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1290, metadata !DIExpression()), !dbg !1285
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1291, metadata !DIExpression()), !dbg !1285
  %15 = load ptr, ptr %10, align 4, !dbg !1285
  %16 = load ptr, ptr %15, align 4, !dbg !1285
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1285
  %18 = load ptr, ptr %17, align 4, !dbg !1285
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1285
  %20 = load ptr, ptr %7, align 4, !dbg !1285
  %21 = load ptr, ptr %10, align 4, !dbg !1285
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1285
  store i32 %22, ptr %12, align 4, !dbg !1285
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1292, metadata !DIExpression()), !dbg !1285
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1293, metadata !DIExpression()), !dbg !1295
  store i32 0, ptr %14, align 4, !dbg !1295
  br label %23, !dbg !1295

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1295
  %25 = load i32, ptr %12, align 4, !dbg !1295
  %26 = icmp slt i32 %24, %25, !dbg !1295
  br i1 %26, label %27, label %105, !dbg !1295

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1296
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1296
  %30 = load i8, ptr %29, align 1, !dbg !1296
  %31 = sext i8 %30 to i32, !dbg !1296
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1296

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1299
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1299
  store ptr %34, ptr %6, align 4, !dbg !1299
  %35 = load i32, ptr %33, align 4, !dbg !1299
  %36 = trunc i32 %35 to i8, !dbg !1299
  %37 = load i32, ptr %14, align 4, !dbg !1299
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1299
  store i8 %36, ptr %38, align 8, !dbg !1299
  br label %101, !dbg !1299

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1299
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1299
  store ptr %41, ptr %6, align 4, !dbg !1299
  %42 = load i32, ptr %40, align 4, !dbg !1299
  %43 = trunc i32 %42 to i8, !dbg !1299
  %44 = load i32, ptr %14, align 4, !dbg !1299
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1299
  store i8 %43, ptr %45, align 8, !dbg !1299
  br label %101, !dbg !1299

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1299
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1299
  store ptr %48, ptr %6, align 4, !dbg !1299
  %49 = load i32, ptr %47, align 4, !dbg !1299
  %50 = trunc i32 %49 to i16, !dbg !1299
  %51 = load i32, ptr %14, align 4, !dbg !1299
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1299
  store i16 %50, ptr %52, align 8, !dbg !1299
  br label %101, !dbg !1299

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1299
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1299
  store ptr %55, ptr %6, align 4, !dbg !1299
  %56 = load i32, ptr %54, align 4, !dbg !1299
  %57 = trunc i32 %56 to i16, !dbg !1299
  %58 = load i32, ptr %14, align 4, !dbg !1299
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1299
  store i16 %57, ptr %59, align 8, !dbg !1299
  br label %101, !dbg !1299

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1299
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1299
  store ptr %62, ptr %6, align 4, !dbg !1299
  %63 = load i32, ptr %61, align 4, !dbg !1299
  %64 = load i32, ptr %14, align 4, !dbg !1299
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1299
  store i32 %63, ptr %65, align 8, !dbg !1299
  br label %101, !dbg !1299

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1299
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1299
  store ptr %68, ptr %6, align 4, !dbg !1299
  %69 = load i32, ptr %67, align 4, !dbg !1299
  %70 = sext i32 %69 to i64, !dbg !1299
  %71 = load i32, ptr %14, align 4, !dbg !1299
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1299
  store i64 %70, ptr %72, align 8, !dbg !1299
  br label %101, !dbg !1299

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1299
  %75 = ptrtoint ptr %74 to i32, !dbg !1299
  %76 = add i32 %75, 7, !dbg !1299
  %77 = and i32 %76, -8, !dbg !1299
  %78 = inttoptr i32 %77 to ptr, !dbg !1299
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1299
  store ptr %79, ptr %6, align 4, !dbg !1299
  %80 = load double, ptr %78, align 8, !dbg !1299
  %81 = fptrunc double %80 to float, !dbg !1299
  %82 = load i32, ptr %14, align 4, !dbg !1299
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1299
  store float %81, ptr %83, align 8, !dbg !1299
  br label %101, !dbg !1299

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1299
  %86 = ptrtoint ptr %85 to i32, !dbg !1299
  %87 = add i32 %86, 7, !dbg !1299
  %88 = and i32 %87, -8, !dbg !1299
  %89 = inttoptr i32 %88 to ptr, !dbg !1299
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1299
  store ptr %90, ptr %6, align 4, !dbg !1299
  %91 = load double, ptr %89, align 8, !dbg !1299
  %92 = load i32, ptr %14, align 4, !dbg !1299
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1299
  store double %91, ptr %93, align 8, !dbg !1299
  br label %101, !dbg !1299

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1299
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1299
  store ptr %96, ptr %6, align 4, !dbg !1299
  %97 = load ptr, ptr %95, align 4, !dbg !1299
  %98 = load i32, ptr %14, align 4, !dbg !1299
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1299
  store ptr %97, ptr %99, align 8, !dbg !1299
  br label %101, !dbg !1299

100:                                              ; preds = %27
  br label %101, !dbg !1299

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1296

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1301
  %104 = add nsw i32 %103, 1, !dbg !1301
  store i32 %104, ptr %14, align 4, !dbg !1301
  br label %23, !dbg !1301, !llvm.loop !1302

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1285
  %107 = load ptr, ptr %106, align 4, !dbg !1285
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 69, !dbg !1285
  %109 = load ptr, ptr %108, align 4, !dbg !1285
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1285
  %111 = load ptr, ptr %7, align 4, !dbg !1285
  %112 = load ptr, ptr %8, align 4, !dbg !1285
  %113 = load ptr, ptr %9, align 4, !dbg !1285
  %114 = load ptr, ptr %10, align 4, !dbg !1285
  %115 = call arm_aapcs_vfpcc zeroext i8 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1285
  ret i8 %115, !dbg !1285
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1303 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1304, metadata !DIExpression()), !dbg !1305
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1306, metadata !DIExpression()), !dbg !1305
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1307, metadata !DIExpression()), !dbg !1305
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1308, metadata !DIExpression()), !dbg !1305
  call void @llvm.va_start(ptr %7), !dbg !1305
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1309, metadata !DIExpression()), !dbg !1305
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1310, metadata !DIExpression()), !dbg !1305
  %13 = load ptr, ptr %6, align 4, !dbg !1305
  %14 = load ptr, ptr %13, align 4, !dbg !1305
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1305
  %16 = load ptr, ptr %15, align 4, !dbg !1305
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1305
  %18 = load ptr, ptr %4, align 4, !dbg !1305
  %19 = load ptr, ptr %6, align 4, !dbg !1305
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1305
  store i32 %20, ptr %9, align 4, !dbg !1305
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1311, metadata !DIExpression()), !dbg !1305
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1312, metadata !DIExpression()), !dbg !1314
  store i32 0, ptr %11, align 4, !dbg !1314
  br label %21, !dbg !1314

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1314
  %23 = load i32, ptr %9, align 4, !dbg !1314
  %24 = icmp slt i32 %22, %23, !dbg !1314
  br i1 %24, label %25, label %103, !dbg !1314

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1315
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1315
  %28 = load i8, ptr %27, align 1, !dbg !1315
  %29 = sext i8 %28 to i32, !dbg !1315
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1315

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1318
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1318
  store ptr %32, ptr %7, align 4, !dbg !1318
  %33 = load i32, ptr %31, align 4, !dbg !1318
  %34 = trunc i32 %33 to i8, !dbg !1318
  %35 = load i32, ptr %11, align 4, !dbg !1318
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1318
  store i8 %34, ptr %36, align 8, !dbg !1318
  br label %99, !dbg !1318

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1318
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1318
  store ptr %39, ptr %7, align 4, !dbg !1318
  %40 = load i32, ptr %38, align 4, !dbg !1318
  %41 = trunc i32 %40 to i8, !dbg !1318
  %42 = load i32, ptr %11, align 4, !dbg !1318
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1318
  store i8 %41, ptr %43, align 8, !dbg !1318
  br label %99, !dbg !1318

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1318
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1318
  store ptr %46, ptr %7, align 4, !dbg !1318
  %47 = load i32, ptr %45, align 4, !dbg !1318
  %48 = trunc i32 %47 to i16, !dbg !1318
  %49 = load i32, ptr %11, align 4, !dbg !1318
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1318
  store i16 %48, ptr %50, align 8, !dbg !1318
  br label %99, !dbg !1318

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1318
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1318
  store ptr %53, ptr %7, align 4, !dbg !1318
  %54 = load i32, ptr %52, align 4, !dbg !1318
  %55 = trunc i32 %54 to i16, !dbg !1318
  %56 = load i32, ptr %11, align 4, !dbg !1318
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1318
  store i16 %55, ptr %57, align 8, !dbg !1318
  br label %99, !dbg !1318

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1318
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1318
  store ptr %60, ptr %7, align 4, !dbg !1318
  %61 = load i32, ptr %59, align 4, !dbg !1318
  %62 = load i32, ptr %11, align 4, !dbg !1318
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1318
  store i32 %61, ptr %63, align 8, !dbg !1318
  br label %99, !dbg !1318

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1318
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1318
  store ptr %66, ptr %7, align 4, !dbg !1318
  %67 = load i32, ptr %65, align 4, !dbg !1318
  %68 = sext i32 %67 to i64, !dbg !1318
  %69 = load i32, ptr %11, align 4, !dbg !1318
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1318
  store i64 %68, ptr %70, align 8, !dbg !1318
  br label %99, !dbg !1318

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1318
  %73 = ptrtoint ptr %72 to i32, !dbg !1318
  %74 = add i32 %73, 7, !dbg !1318
  %75 = and i32 %74, -8, !dbg !1318
  %76 = inttoptr i32 %75 to ptr, !dbg !1318
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1318
  store ptr %77, ptr %7, align 4, !dbg !1318
  %78 = load double, ptr %76, align 8, !dbg !1318
  %79 = fptrunc double %78 to float, !dbg !1318
  %80 = load i32, ptr %11, align 4, !dbg !1318
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1318
  store float %79, ptr %81, align 8, !dbg !1318
  br label %99, !dbg !1318

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1318
  %84 = ptrtoint ptr %83 to i32, !dbg !1318
  %85 = add i32 %84, 7, !dbg !1318
  %86 = and i32 %85, -8, !dbg !1318
  %87 = inttoptr i32 %86 to ptr, !dbg !1318
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1318
  store ptr %88, ptr %7, align 4, !dbg !1318
  %89 = load double, ptr %87, align 8, !dbg !1318
  %90 = load i32, ptr %11, align 4, !dbg !1318
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1318
  store double %89, ptr %91, align 8, !dbg !1318
  br label %99, !dbg !1318

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1318
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1318
  store ptr %94, ptr %7, align 4, !dbg !1318
  %95 = load ptr, ptr %93, align 4, !dbg !1318
  %96 = load i32, ptr %11, align 4, !dbg !1318
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1318
  store ptr %95, ptr %97, align 8, !dbg !1318
  br label %99, !dbg !1318

98:                                               ; preds = %25
  br label %99, !dbg !1318

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1315

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1320
  %102 = add nsw i32 %101, 1, !dbg !1320
  store i32 %102, ptr %11, align 4, !dbg !1320
  br label %21, !dbg !1320, !llvm.loop !1321

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1322, metadata !DIExpression()), !dbg !1305
  %104 = load ptr, ptr %6, align 4, !dbg !1305
  %105 = load ptr, ptr %104, align 4, !dbg !1305
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 119, !dbg !1305
  %107 = load ptr, ptr %106, align 4, !dbg !1305
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1305
  %109 = load ptr, ptr %4, align 4, !dbg !1305
  %110 = load ptr, ptr %5, align 4, !dbg !1305
  %111 = load ptr, ptr %6, align 4, !dbg !1305
  %112 = call arm_aapcs_vfpcc zeroext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1305
  store i8 %112, ptr %12, align 1, !dbg !1305
  call void @llvm.va_end(ptr %7), !dbg !1305
  %113 = load i8, ptr %12, align 1, !dbg !1305
  ret i8 %113, !dbg !1305
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1323 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1324, metadata !DIExpression()), !dbg !1325
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1326, metadata !DIExpression()), !dbg !1325
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1327, metadata !DIExpression()), !dbg !1325
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1328, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1329, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1330, metadata !DIExpression()), !dbg !1325
  %13 = load ptr, ptr %8, align 4, !dbg !1325
  %14 = load ptr, ptr %13, align 4, !dbg !1325
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1325
  %16 = load ptr, ptr %15, align 4, !dbg !1325
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1325
  %18 = load ptr, ptr %6, align 4, !dbg !1325
  %19 = load ptr, ptr %8, align 4, !dbg !1325
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1325
  store i32 %20, ptr %10, align 4, !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1331, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1332, metadata !DIExpression()), !dbg !1334
  store i32 0, ptr %12, align 4, !dbg !1334
  br label %21, !dbg !1334

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1334
  %23 = load i32, ptr %10, align 4, !dbg !1334
  %24 = icmp slt i32 %22, %23, !dbg !1334
  br i1 %24, label %25, label %103, !dbg !1334

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1335
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1335
  %28 = load i8, ptr %27, align 1, !dbg !1335
  %29 = sext i8 %28 to i32, !dbg !1335
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1335

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1338
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1338
  store ptr %32, ptr %5, align 4, !dbg !1338
  %33 = load i32, ptr %31, align 4, !dbg !1338
  %34 = trunc i32 %33 to i8, !dbg !1338
  %35 = load i32, ptr %12, align 4, !dbg !1338
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1338
  store i8 %34, ptr %36, align 8, !dbg !1338
  br label %99, !dbg !1338

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1338
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1338
  store ptr %39, ptr %5, align 4, !dbg !1338
  %40 = load i32, ptr %38, align 4, !dbg !1338
  %41 = trunc i32 %40 to i8, !dbg !1338
  %42 = load i32, ptr %12, align 4, !dbg !1338
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1338
  store i8 %41, ptr %43, align 8, !dbg !1338
  br label %99, !dbg !1338

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1338
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1338
  store ptr %46, ptr %5, align 4, !dbg !1338
  %47 = load i32, ptr %45, align 4, !dbg !1338
  %48 = trunc i32 %47 to i16, !dbg !1338
  %49 = load i32, ptr %12, align 4, !dbg !1338
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1338
  store i16 %48, ptr %50, align 8, !dbg !1338
  br label %99, !dbg !1338

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1338
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1338
  store ptr %53, ptr %5, align 4, !dbg !1338
  %54 = load i32, ptr %52, align 4, !dbg !1338
  %55 = trunc i32 %54 to i16, !dbg !1338
  %56 = load i32, ptr %12, align 4, !dbg !1338
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1338
  store i16 %55, ptr %57, align 8, !dbg !1338
  br label %99, !dbg !1338

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1338
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1338
  store ptr %60, ptr %5, align 4, !dbg !1338
  %61 = load i32, ptr %59, align 4, !dbg !1338
  %62 = load i32, ptr %12, align 4, !dbg !1338
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1338
  store i32 %61, ptr %63, align 8, !dbg !1338
  br label %99, !dbg !1338

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1338
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1338
  store ptr %66, ptr %5, align 4, !dbg !1338
  %67 = load i32, ptr %65, align 4, !dbg !1338
  %68 = sext i32 %67 to i64, !dbg !1338
  %69 = load i32, ptr %12, align 4, !dbg !1338
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1338
  store i64 %68, ptr %70, align 8, !dbg !1338
  br label %99, !dbg !1338

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1338
  %73 = ptrtoint ptr %72 to i32, !dbg !1338
  %74 = add i32 %73, 7, !dbg !1338
  %75 = and i32 %74, -8, !dbg !1338
  %76 = inttoptr i32 %75 to ptr, !dbg !1338
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1338
  store ptr %77, ptr %5, align 4, !dbg !1338
  %78 = load double, ptr %76, align 8, !dbg !1338
  %79 = fptrunc double %78 to float, !dbg !1338
  %80 = load i32, ptr %12, align 4, !dbg !1338
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1338
  store float %79, ptr %81, align 8, !dbg !1338
  br label %99, !dbg !1338

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1338
  %84 = ptrtoint ptr %83 to i32, !dbg !1338
  %85 = add i32 %84, 7, !dbg !1338
  %86 = and i32 %85, -8, !dbg !1338
  %87 = inttoptr i32 %86 to ptr, !dbg !1338
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1338
  store ptr %88, ptr %5, align 4, !dbg !1338
  %89 = load double, ptr %87, align 8, !dbg !1338
  %90 = load i32, ptr %12, align 4, !dbg !1338
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1338
  store double %89, ptr %91, align 8, !dbg !1338
  br label %99, !dbg !1338

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1338
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1338
  store ptr %94, ptr %5, align 4, !dbg !1338
  %95 = load ptr, ptr %93, align 4, !dbg !1338
  %96 = load i32, ptr %12, align 4, !dbg !1338
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1338
  store ptr %95, ptr %97, align 8, !dbg !1338
  br label %99, !dbg !1338

98:                                               ; preds = %25
  br label %99, !dbg !1338

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1335

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1340
  %102 = add nsw i32 %101, 1, !dbg !1340
  store i32 %102, ptr %12, align 4, !dbg !1340
  br label %21, !dbg !1340, !llvm.loop !1341

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1325
  %105 = load ptr, ptr %104, align 4, !dbg !1325
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 119, !dbg !1325
  %107 = load ptr, ptr %106, align 4, !dbg !1325
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1325
  %109 = load ptr, ptr %6, align 4, !dbg !1325
  %110 = load ptr, ptr %7, align 4, !dbg !1325
  %111 = load ptr, ptr %8, align 4, !dbg !1325
  %112 = call arm_aapcs_vfpcc zeroext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1325
  ret i8 %112, !dbg !1325
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1342 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1343, metadata !DIExpression()), !dbg !1344
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1345, metadata !DIExpression()), !dbg !1344
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1346, metadata !DIExpression()), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1347, metadata !DIExpression()), !dbg !1344
  call void @llvm.va_start(ptr %7), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1348, metadata !DIExpression()), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1349, metadata !DIExpression()), !dbg !1344
  %13 = load ptr, ptr %6, align 4, !dbg !1344
  %14 = load ptr, ptr %13, align 4, !dbg !1344
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1344
  %16 = load ptr, ptr %15, align 4, !dbg !1344
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1344
  %18 = load ptr, ptr %4, align 4, !dbg !1344
  %19 = load ptr, ptr %6, align 4, !dbg !1344
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1344
  store i32 %20, ptr %9, align 4, !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1350, metadata !DIExpression()), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1351, metadata !DIExpression()), !dbg !1353
  store i32 0, ptr %11, align 4, !dbg !1353
  br label %21, !dbg !1353

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1353
  %23 = load i32, ptr %9, align 4, !dbg !1353
  %24 = icmp slt i32 %22, %23, !dbg !1353
  br i1 %24, label %25, label %103, !dbg !1353

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1354
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1354
  %28 = load i8, ptr %27, align 1, !dbg !1354
  %29 = sext i8 %28 to i32, !dbg !1354
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1354

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1357
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1357
  store ptr %32, ptr %7, align 4, !dbg !1357
  %33 = load i32, ptr %31, align 4, !dbg !1357
  %34 = trunc i32 %33 to i8, !dbg !1357
  %35 = load i32, ptr %11, align 4, !dbg !1357
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1357
  store i8 %34, ptr %36, align 8, !dbg !1357
  br label %99, !dbg !1357

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1357
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1357
  store ptr %39, ptr %7, align 4, !dbg !1357
  %40 = load i32, ptr %38, align 4, !dbg !1357
  %41 = trunc i32 %40 to i8, !dbg !1357
  %42 = load i32, ptr %11, align 4, !dbg !1357
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1357
  store i8 %41, ptr %43, align 8, !dbg !1357
  br label %99, !dbg !1357

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1357
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1357
  store ptr %46, ptr %7, align 4, !dbg !1357
  %47 = load i32, ptr %45, align 4, !dbg !1357
  %48 = trunc i32 %47 to i16, !dbg !1357
  %49 = load i32, ptr %11, align 4, !dbg !1357
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1357
  store i16 %48, ptr %50, align 8, !dbg !1357
  br label %99, !dbg !1357

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1357
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1357
  store ptr %53, ptr %7, align 4, !dbg !1357
  %54 = load i32, ptr %52, align 4, !dbg !1357
  %55 = trunc i32 %54 to i16, !dbg !1357
  %56 = load i32, ptr %11, align 4, !dbg !1357
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1357
  store i16 %55, ptr %57, align 8, !dbg !1357
  br label %99, !dbg !1357

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1357
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1357
  store ptr %60, ptr %7, align 4, !dbg !1357
  %61 = load i32, ptr %59, align 4, !dbg !1357
  %62 = load i32, ptr %11, align 4, !dbg !1357
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1357
  store i32 %61, ptr %63, align 8, !dbg !1357
  br label %99, !dbg !1357

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1357
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1357
  store ptr %66, ptr %7, align 4, !dbg !1357
  %67 = load i32, ptr %65, align 4, !dbg !1357
  %68 = sext i32 %67 to i64, !dbg !1357
  %69 = load i32, ptr %11, align 4, !dbg !1357
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1357
  store i64 %68, ptr %70, align 8, !dbg !1357
  br label %99, !dbg !1357

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1357
  %73 = ptrtoint ptr %72 to i32, !dbg !1357
  %74 = add i32 %73, 7, !dbg !1357
  %75 = and i32 %74, -8, !dbg !1357
  %76 = inttoptr i32 %75 to ptr, !dbg !1357
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1357
  store ptr %77, ptr %7, align 4, !dbg !1357
  %78 = load double, ptr %76, align 8, !dbg !1357
  %79 = fptrunc double %78 to float, !dbg !1357
  %80 = load i32, ptr %11, align 4, !dbg !1357
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1357
  store float %79, ptr %81, align 8, !dbg !1357
  br label %99, !dbg !1357

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1357
  %84 = ptrtoint ptr %83 to i32, !dbg !1357
  %85 = add i32 %84, 7, !dbg !1357
  %86 = and i32 %85, -8, !dbg !1357
  %87 = inttoptr i32 %86 to ptr, !dbg !1357
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1357
  store ptr %88, ptr %7, align 4, !dbg !1357
  %89 = load double, ptr %87, align 8, !dbg !1357
  %90 = load i32, ptr %11, align 4, !dbg !1357
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1357
  store double %89, ptr %91, align 8, !dbg !1357
  br label %99, !dbg !1357

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1357
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1357
  store ptr %94, ptr %7, align 4, !dbg !1357
  %95 = load ptr, ptr %93, align 4, !dbg !1357
  %96 = load i32, ptr %11, align 4, !dbg !1357
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1357
  store ptr %95, ptr %97, align 8, !dbg !1357
  br label %99, !dbg !1357

98:                                               ; preds = %25
  br label %99, !dbg !1357

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1354

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1359
  %102 = add nsw i32 %101, 1, !dbg !1359
  store i32 %102, ptr %11, align 4, !dbg !1359
  br label %21, !dbg !1359, !llvm.loop !1360

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1361, metadata !DIExpression()), !dbg !1344
  %104 = load ptr, ptr %6, align 4, !dbg !1344
  %105 = load ptr, ptr %104, align 4, !dbg !1344
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 42, !dbg !1344
  %107 = load ptr, ptr %106, align 4, !dbg !1344
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1344
  %109 = load ptr, ptr %4, align 4, !dbg !1344
  %110 = load ptr, ptr %5, align 4, !dbg !1344
  %111 = load ptr, ptr %6, align 4, !dbg !1344
  %112 = call arm_aapcs_vfpcc signext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1344
  store i8 %112, ptr %12, align 1, !dbg !1344
  call void @llvm.va_end(ptr %7), !dbg !1344
  %113 = load i8, ptr %12, align 1, !dbg !1344
  ret i8 %113, !dbg !1344
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1362 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1363, metadata !DIExpression()), !dbg !1364
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1365, metadata !DIExpression()), !dbg !1364
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1366, metadata !DIExpression()), !dbg !1364
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1367, metadata !DIExpression()), !dbg !1364
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1368, metadata !DIExpression()), !dbg !1364
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1369, metadata !DIExpression()), !dbg !1364
  %13 = load ptr, ptr %8, align 4, !dbg !1364
  %14 = load ptr, ptr %13, align 4, !dbg !1364
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1364
  %16 = load ptr, ptr %15, align 4, !dbg !1364
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1364
  %18 = load ptr, ptr %6, align 4, !dbg !1364
  %19 = load ptr, ptr %8, align 4, !dbg !1364
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1364
  store i32 %20, ptr %10, align 4, !dbg !1364
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1370, metadata !DIExpression()), !dbg !1364
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1371, metadata !DIExpression()), !dbg !1373
  store i32 0, ptr %12, align 4, !dbg !1373
  br label %21, !dbg !1373

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1373
  %23 = load i32, ptr %10, align 4, !dbg !1373
  %24 = icmp slt i32 %22, %23, !dbg !1373
  br i1 %24, label %25, label %103, !dbg !1373

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1374
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1374
  %28 = load i8, ptr %27, align 1, !dbg !1374
  %29 = sext i8 %28 to i32, !dbg !1374
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1374

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1377
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1377
  store ptr %32, ptr %5, align 4, !dbg !1377
  %33 = load i32, ptr %31, align 4, !dbg !1377
  %34 = trunc i32 %33 to i8, !dbg !1377
  %35 = load i32, ptr %12, align 4, !dbg !1377
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1377
  store i8 %34, ptr %36, align 8, !dbg !1377
  br label %99, !dbg !1377

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1377
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1377
  store ptr %39, ptr %5, align 4, !dbg !1377
  %40 = load i32, ptr %38, align 4, !dbg !1377
  %41 = trunc i32 %40 to i8, !dbg !1377
  %42 = load i32, ptr %12, align 4, !dbg !1377
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1377
  store i8 %41, ptr %43, align 8, !dbg !1377
  br label %99, !dbg !1377

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1377
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1377
  store ptr %46, ptr %5, align 4, !dbg !1377
  %47 = load i32, ptr %45, align 4, !dbg !1377
  %48 = trunc i32 %47 to i16, !dbg !1377
  %49 = load i32, ptr %12, align 4, !dbg !1377
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1377
  store i16 %48, ptr %50, align 8, !dbg !1377
  br label %99, !dbg !1377

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1377
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1377
  store ptr %53, ptr %5, align 4, !dbg !1377
  %54 = load i32, ptr %52, align 4, !dbg !1377
  %55 = trunc i32 %54 to i16, !dbg !1377
  %56 = load i32, ptr %12, align 4, !dbg !1377
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1377
  store i16 %55, ptr %57, align 8, !dbg !1377
  br label %99, !dbg !1377

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1377
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1377
  store ptr %60, ptr %5, align 4, !dbg !1377
  %61 = load i32, ptr %59, align 4, !dbg !1377
  %62 = load i32, ptr %12, align 4, !dbg !1377
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1377
  store i32 %61, ptr %63, align 8, !dbg !1377
  br label %99, !dbg !1377

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1377
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1377
  store ptr %66, ptr %5, align 4, !dbg !1377
  %67 = load i32, ptr %65, align 4, !dbg !1377
  %68 = sext i32 %67 to i64, !dbg !1377
  %69 = load i32, ptr %12, align 4, !dbg !1377
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1377
  store i64 %68, ptr %70, align 8, !dbg !1377
  br label %99, !dbg !1377

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1377
  %73 = ptrtoint ptr %72 to i32, !dbg !1377
  %74 = add i32 %73, 7, !dbg !1377
  %75 = and i32 %74, -8, !dbg !1377
  %76 = inttoptr i32 %75 to ptr, !dbg !1377
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1377
  store ptr %77, ptr %5, align 4, !dbg !1377
  %78 = load double, ptr %76, align 8, !dbg !1377
  %79 = fptrunc double %78 to float, !dbg !1377
  %80 = load i32, ptr %12, align 4, !dbg !1377
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1377
  store float %79, ptr %81, align 8, !dbg !1377
  br label %99, !dbg !1377

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1377
  %84 = ptrtoint ptr %83 to i32, !dbg !1377
  %85 = add i32 %84, 7, !dbg !1377
  %86 = and i32 %85, -8, !dbg !1377
  %87 = inttoptr i32 %86 to ptr, !dbg !1377
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1377
  store ptr %88, ptr %5, align 4, !dbg !1377
  %89 = load double, ptr %87, align 8, !dbg !1377
  %90 = load i32, ptr %12, align 4, !dbg !1377
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1377
  store double %89, ptr %91, align 8, !dbg !1377
  br label %99, !dbg !1377

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1377
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1377
  store ptr %94, ptr %5, align 4, !dbg !1377
  %95 = load ptr, ptr %93, align 4, !dbg !1377
  %96 = load i32, ptr %12, align 4, !dbg !1377
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1377
  store ptr %95, ptr %97, align 8, !dbg !1377
  br label %99, !dbg !1377

98:                                               ; preds = %25
  br label %99, !dbg !1377

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1374

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1379
  %102 = add nsw i32 %101, 1, !dbg !1379
  store i32 %102, ptr %12, align 4, !dbg !1379
  br label %21, !dbg !1379, !llvm.loop !1380

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1364
  %105 = load ptr, ptr %104, align 4, !dbg !1364
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 42, !dbg !1364
  %107 = load ptr, ptr %106, align 4, !dbg !1364
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1364
  %109 = load ptr, ptr %6, align 4, !dbg !1364
  %110 = load ptr, ptr %7, align 4, !dbg !1364
  %111 = load ptr, ptr %8, align 4, !dbg !1364
  %112 = call arm_aapcs_vfpcc signext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1364
  ret i8 %112, !dbg !1364
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1381 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1382, metadata !DIExpression()), !dbg !1383
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1384, metadata !DIExpression()), !dbg !1383
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1385, metadata !DIExpression()), !dbg !1383
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1386, metadata !DIExpression()), !dbg !1383
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1387, metadata !DIExpression()), !dbg !1383
  call void @llvm.va_start(ptr %9), !dbg !1383
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1388, metadata !DIExpression()), !dbg !1383
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1389, metadata !DIExpression()), !dbg !1383
  %15 = load ptr, ptr %8, align 4, !dbg !1383
  %16 = load ptr, ptr %15, align 4, !dbg !1383
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1383
  %18 = load ptr, ptr %17, align 4, !dbg !1383
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1383
  %20 = load ptr, ptr %5, align 4, !dbg !1383
  %21 = load ptr, ptr %8, align 4, !dbg !1383
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1383
  store i32 %22, ptr %11, align 4, !dbg !1383
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1390, metadata !DIExpression()), !dbg !1383
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1391, metadata !DIExpression()), !dbg !1393
  store i32 0, ptr %13, align 4, !dbg !1393
  br label %23, !dbg !1393

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1393
  %25 = load i32, ptr %11, align 4, !dbg !1393
  %26 = icmp slt i32 %24, %25, !dbg !1393
  br i1 %26, label %27, label %105, !dbg !1393

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1394
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1394
  %30 = load i8, ptr %29, align 1, !dbg !1394
  %31 = sext i8 %30 to i32, !dbg !1394
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1394

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1397
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1397
  store ptr %34, ptr %9, align 4, !dbg !1397
  %35 = load i32, ptr %33, align 4, !dbg !1397
  %36 = trunc i32 %35 to i8, !dbg !1397
  %37 = load i32, ptr %13, align 4, !dbg !1397
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1397
  store i8 %36, ptr %38, align 8, !dbg !1397
  br label %101, !dbg !1397

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1397
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1397
  store ptr %41, ptr %9, align 4, !dbg !1397
  %42 = load i32, ptr %40, align 4, !dbg !1397
  %43 = trunc i32 %42 to i8, !dbg !1397
  %44 = load i32, ptr %13, align 4, !dbg !1397
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1397
  store i8 %43, ptr %45, align 8, !dbg !1397
  br label %101, !dbg !1397

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1397
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1397
  store ptr %48, ptr %9, align 4, !dbg !1397
  %49 = load i32, ptr %47, align 4, !dbg !1397
  %50 = trunc i32 %49 to i16, !dbg !1397
  %51 = load i32, ptr %13, align 4, !dbg !1397
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1397
  store i16 %50, ptr %52, align 8, !dbg !1397
  br label %101, !dbg !1397

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1397
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1397
  store ptr %55, ptr %9, align 4, !dbg !1397
  %56 = load i32, ptr %54, align 4, !dbg !1397
  %57 = trunc i32 %56 to i16, !dbg !1397
  %58 = load i32, ptr %13, align 4, !dbg !1397
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1397
  store i16 %57, ptr %59, align 8, !dbg !1397
  br label %101, !dbg !1397

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1397
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1397
  store ptr %62, ptr %9, align 4, !dbg !1397
  %63 = load i32, ptr %61, align 4, !dbg !1397
  %64 = load i32, ptr %13, align 4, !dbg !1397
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1397
  store i32 %63, ptr %65, align 8, !dbg !1397
  br label %101, !dbg !1397

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1397
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1397
  store ptr %68, ptr %9, align 4, !dbg !1397
  %69 = load i32, ptr %67, align 4, !dbg !1397
  %70 = sext i32 %69 to i64, !dbg !1397
  %71 = load i32, ptr %13, align 4, !dbg !1397
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1397
  store i64 %70, ptr %72, align 8, !dbg !1397
  br label %101, !dbg !1397

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1397
  %75 = ptrtoint ptr %74 to i32, !dbg !1397
  %76 = add i32 %75, 7, !dbg !1397
  %77 = and i32 %76, -8, !dbg !1397
  %78 = inttoptr i32 %77 to ptr, !dbg !1397
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1397
  store ptr %79, ptr %9, align 4, !dbg !1397
  %80 = load double, ptr %78, align 8, !dbg !1397
  %81 = fptrunc double %80 to float, !dbg !1397
  %82 = load i32, ptr %13, align 4, !dbg !1397
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1397
  store float %81, ptr %83, align 8, !dbg !1397
  br label %101, !dbg !1397

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1397
  %86 = ptrtoint ptr %85 to i32, !dbg !1397
  %87 = add i32 %86, 7, !dbg !1397
  %88 = and i32 %87, -8, !dbg !1397
  %89 = inttoptr i32 %88 to ptr, !dbg !1397
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1397
  store ptr %90, ptr %9, align 4, !dbg !1397
  %91 = load double, ptr %89, align 8, !dbg !1397
  %92 = load i32, ptr %13, align 4, !dbg !1397
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1397
  store double %91, ptr %93, align 8, !dbg !1397
  br label %101, !dbg !1397

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1397
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1397
  store ptr %96, ptr %9, align 4, !dbg !1397
  %97 = load ptr, ptr %95, align 4, !dbg !1397
  %98 = load i32, ptr %13, align 4, !dbg !1397
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1397
  store ptr %97, ptr %99, align 8, !dbg !1397
  br label %101, !dbg !1397

100:                                              ; preds = %27
  br label %101, !dbg !1397

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1394

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1399
  %104 = add nsw i32 %103, 1, !dbg !1399
  store i32 %104, ptr %13, align 4, !dbg !1399
  br label %23, !dbg !1399, !llvm.loop !1400

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1401, metadata !DIExpression()), !dbg !1383
  %106 = load ptr, ptr %8, align 4, !dbg !1383
  %107 = load ptr, ptr %106, align 4, !dbg !1383
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 72, !dbg !1383
  %109 = load ptr, ptr %108, align 4, !dbg !1383
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1383
  %111 = load ptr, ptr %5, align 4, !dbg !1383
  %112 = load ptr, ptr %6, align 4, !dbg !1383
  %113 = load ptr, ptr %7, align 4, !dbg !1383
  %114 = load ptr, ptr %8, align 4, !dbg !1383
  %115 = call arm_aapcs_vfpcc signext i8 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1383
  store i8 %115, ptr %14, align 1, !dbg !1383
  call void @llvm.va_end(ptr %9), !dbg !1383
  %116 = load i8, ptr %14, align 1, !dbg !1383
  ret i8 %116, !dbg !1383
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1402 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1403, metadata !DIExpression()), !dbg !1404
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1405, metadata !DIExpression()), !dbg !1404
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1406, metadata !DIExpression()), !dbg !1404
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1407, metadata !DIExpression()), !dbg !1404
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1408, metadata !DIExpression()), !dbg !1404
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1409, metadata !DIExpression()), !dbg !1404
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1410, metadata !DIExpression()), !dbg !1404
  %15 = load ptr, ptr %10, align 4, !dbg !1404
  %16 = load ptr, ptr %15, align 4, !dbg !1404
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1404
  %18 = load ptr, ptr %17, align 4, !dbg !1404
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1404
  %20 = load ptr, ptr %7, align 4, !dbg !1404
  %21 = load ptr, ptr %10, align 4, !dbg !1404
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1404
  store i32 %22, ptr %12, align 4, !dbg !1404
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1411, metadata !DIExpression()), !dbg !1404
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1412, metadata !DIExpression()), !dbg !1414
  store i32 0, ptr %14, align 4, !dbg !1414
  br label %23, !dbg !1414

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1414
  %25 = load i32, ptr %12, align 4, !dbg !1414
  %26 = icmp slt i32 %24, %25, !dbg !1414
  br i1 %26, label %27, label %105, !dbg !1414

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1415
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1415
  %30 = load i8, ptr %29, align 1, !dbg !1415
  %31 = sext i8 %30 to i32, !dbg !1415
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1415

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1418
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1418
  store ptr %34, ptr %6, align 4, !dbg !1418
  %35 = load i32, ptr %33, align 4, !dbg !1418
  %36 = trunc i32 %35 to i8, !dbg !1418
  %37 = load i32, ptr %14, align 4, !dbg !1418
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1418
  store i8 %36, ptr %38, align 8, !dbg !1418
  br label %101, !dbg !1418

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1418
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1418
  store ptr %41, ptr %6, align 4, !dbg !1418
  %42 = load i32, ptr %40, align 4, !dbg !1418
  %43 = trunc i32 %42 to i8, !dbg !1418
  %44 = load i32, ptr %14, align 4, !dbg !1418
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1418
  store i8 %43, ptr %45, align 8, !dbg !1418
  br label %101, !dbg !1418

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1418
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1418
  store ptr %48, ptr %6, align 4, !dbg !1418
  %49 = load i32, ptr %47, align 4, !dbg !1418
  %50 = trunc i32 %49 to i16, !dbg !1418
  %51 = load i32, ptr %14, align 4, !dbg !1418
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1418
  store i16 %50, ptr %52, align 8, !dbg !1418
  br label %101, !dbg !1418

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1418
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1418
  store ptr %55, ptr %6, align 4, !dbg !1418
  %56 = load i32, ptr %54, align 4, !dbg !1418
  %57 = trunc i32 %56 to i16, !dbg !1418
  %58 = load i32, ptr %14, align 4, !dbg !1418
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1418
  store i16 %57, ptr %59, align 8, !dbg !1418
  br label %101, !dbg !1418

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1418
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1418
  store ptr %62, ptr %6, align 4, !dbg !1418
  %63 = load i32, ptr %61, align 4, !dbg !1418
  %64 = load i32, ptr %14, align 4, !dbg !1418
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1418
  store i32 %63, ptr %65, align 8, !dbg !1418
  br label %101, !dbg !1418

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1418
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1418
  store ptr %68, ptr %6, align 4, !dbg !1418
  %69 = load i32, ptr %67, align 4, !dbg !1418
  %70 = sext i32 %69 to i64, !dbg !1418
  %71 = load i32, ptr %14, align 4, !dbg !1418
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1418
  store i64 %70, ptr %72, align 8, !dbg !1418
  br label %101, !dbg !1418

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1418
  %75 = ptrtoint ptr %74 to i32, !dbg !1418
  %76 = add i32 %75, 7, !dbg !1418
  %77 = and i32 %76, -8, !dbg !1418
  %78 = inttoptr i32 %77 to ptr, !dbg !1418
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1418
  store ptr %79, ptr %6, align 4, !dbg !1418
  %80 = load double, ptr %78, align 8, !dbg !1418
  %81 = fptrunc double %80 to float, !dbg !1418
  %82 = load i32, ptr %14, align 4, !dbg !1418
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1418
  store float %81, ptr %83, align 8, !dbg !1418
  br label %101, !dbg !1418

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1418
  %86 = ptrtoint ptr %85 to i32, !dbg !1418
  %87 = add i32 %86, 7, !dbg !1418
  %88 = and i32 %87, -8, !dbg !1418
  %89 = inttoptr i32 %88 to ptr, !dbg !1418
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1418
  store ptr %90, ptr %6, align 4, !dbg !1418
  %91 = load double, ptr %89, align 8, !dbg !1418
  %92 = load i32, ptr %14, align 4, !dbg !1418
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1418
  store double %91, ptr %93, align 8, !dbg !1418
  br label %101, !dbg !1418

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1418
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1418
  store ptr %96, ptr %6, align 4, !dbg !1418
  %97 = load ptr, ptr %95, align 4, !dbg !1418
  %98 = load i32, ptr %14, align 4, !dbg !1418
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1418
  store ptr %97, ptr %99, align 8, !dbg !1418
  br label %101, !dbg !1418

100:                                              ; preds = %27
  br label %101, !dbg !1418

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1415

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1420
  %104 = add nsw i32 %103, 1, !dbg !1420
  store i32 %104, ptr %14, align 4, !dbg !1420
  br label %23, !dbg !1420, !llvm.loop !1421

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1404
  %107 = load ptr, ptr %106, align 4, !dbg !1404
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 72, !dbg !1404
  %109 = load ptr, ptr %108, align 4, !dbg !1404
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1404
  %111 = load ptr, ptr %7, align 4, !dbg !1404
  %112 = load ptr, ptr %8, align 4, !dbg !1404
  %113 = load ptr, ptr %9, align 4, !dbg !1404
  %114 = load ptr, ptr %10, align 4, !dbg !1404
  %115 = call arm_aapcs_vfpcc signext i8 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1404
  ret i8 %115, !dbg !1404
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1422 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1423, metadata !DIExpression()), !dbg !1424
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1425, metadata !DIExpression()), !dbg !1424
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1426, metadata !DIExpression()), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1427, metadata !DIExpression()), !dbg !1424
  call void @llvm.va_start(ptr %7), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1428, metadata !DIExpression()), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1429, metadata !DIExpression()), !dbg !1424
  %13 = load ptr, ptr %6, align 4, !dbg !1424
  %14 = load ptr, ptr %13, align 4, !dbg !1424
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1424
  %16 = load ptr, ptr %15, align 4, !dbg !1424
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1424
  %18 = load ptr, ptr %4, align 4, !dbg !1424
  %19 = load ptr, ptr %6, align 4, !dbg !1424
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1424
  store i32 %20, ptr %9, align 4, !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1430, metadata !DIExpression()), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1431, metadata !DIExpression()), !dbg !1433
  store i32 0, ptr %11, align 4, !dbg !1433
  br label %21, !dbg !1433

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1433
  %23 = load i32, ptr %9, align 4, !dbg !1433
  %24 = icmp slt i32 %22, %23, !dbg !1433
  br i1 %24, label %25, label %103, !dbg !1433

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1434
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1434
  %28 = load i8, ptr %27, align 1, !dbg !1434
  %29 = sext i8 %28 to i32, !dbg !1434
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1434

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1437
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1437
  store ptr %32, ptr %7, align 4, !dbg !1437
  %33 = load i32, ptr %31, align 4, !dbg !1437
  %34 = trunc i32 %33 to i8, !dbg !1437
  %35 = load i32, ptr %11, align 4, !dbg !1437
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1437
  store i8 %34, ptr %36, align 8, !dbg !1437
  br label %99, !dbg !1437

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1437
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1437
  store ptr %39, ptr %7, align 4, !dbg !1437
  %40 = load i32, ptr %38, align 4, !dbg !1437
  %41 = trunc i32 %40 to i8, !dbg !1437
  %42 = load i32, ptr %11, align 4, !dbg !1437
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1437
  store i8 %41, ptr %43, align 8, !dbg !1437
  br label %99, !dbg !1437

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1437
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1437
  store ptr %46, ptr %7, align 4, !dbg !1437
  %47 = load i32, ptr %45, align 4, !dbg !1437
  %48 = trunc i32 %47 to i16, !dbg !1437
  %49 = load i32, ptr %11, align 4, !dbg !1437
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1437
  store i16 %48, ptr %50, align 8, !dbg !1437
  br label %99, !dbg !1437

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1437
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1437
  store ptr %53, ptr %7, align 4, !dbg !1437
  %54 = load i32, ptr %52, align 4, !dbg !1437
  %55 = trunc i32 %54 to i16, !dbg !1437
  %56 = load i32, ptr %11, align 4, !dbg !1437
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1437
  store i16 %55, ptr %57, align 8, !dbg !1437
  br label %99, !dbg !1437

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1437
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1437
  store ptr %60, ptr %7, align 4, !dbg !1437
  %61 = load i32, ptr %59, align 4, !dbg !1437
  %62 = load i32, ptr %11, align 4, !dbg !1437
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1437
  store i32 %61, ptr %63, align 8, !dbg !1437
  br label %99, !dbg !1437

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1437
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1437
  store ptr %66, ptr %7, align 4, !dbg !1437
  %67 = load i32, ptr %65, align 4, !dbg !1437
  %68 = sext i32 %67 to i64, !dbg !1437
  %69 = load i32, ptr %11, align 4, !dbg !1437
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1437
  store i64 %68, ptr %70, align 8, !dbg !1437
  br label %99, !dbg !1437

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1437
  %73 = ptrtoint ptr %72 to i32, !dbg !1437
  %74 = add i32 %73, 7, !dbg !1437
  %75 = and i32 %74, -8, !dbg !1437
  %76 = inttoptr i32 %75 to ptr, !dbg !1437
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1437
  store ptr %77, ptr %7, align 4, !dbg !1437
  %78 = load double, ptr %76, align 8, !dbg !1437
  %79 = fptrunc double %78 to float, !dbg !1437
  %80 = load i32, ptr %11, align 4, !dbg !1437
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1437
  store float %79, ptr %81, align 8, !dbg !1437
  br label %99, !dbg !1437

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1437
  %84 = ptrtoint ptr %83 to i32, !dbg !1437
  %85 = add i32 %84, 7, !dbg !1437
  %86 = and i32 %85, -8, !dbg !1437
  %87 = inttoptr i32 %86 to ptr, !dbg !1437
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1437
  store ptr %88, ptr %7, align 4, !dbg !1437
  %89 = load double, ptr %87, align 8, !dbg !1437
  %90 = load i32, ptr %11, align 4, !dbg !1437
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1437
  store double %89, ptr %91, align 8, !dbg !1437
  br label %99, !dbg !1437

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1437
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1437
  store ptr %94, ptr %7, align 4, !dbg !1437
  %95 = load ptr, ptr %93, align 4, !dbg !1437
  %96 = load i32, ptr %11, align 4, !dbg !1437
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1437
  store ptr %95, ptr %97, align 8, !dbg !1437
  br label %99, !dbg !1437

98:                                               ; preds = %25
  br label %99, !dbg !1437

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1434

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1439
  %102 = add nsw i32 %101, 1, !dbg !1439
  store i32 %102, ptr %11, align 4, !dbg !1439
  br label %21, !dbg !1439, !llvm.loop !1440

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1441, metadata !DIExpression()), !dbg !1424
  %104 = load ptr, ptr %6, align 4, !dbg !1424
  %105 = load ptr, ptr %104, align 4, !dbg !1424
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 122, !dbg !1424
  %107 = load ptr, ptr %106, align 4, !dbg !1424
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1424
  %109 = load ptr, ptr %4, align 4, !dbg !1424
  %110 = load ptr, ptr %5, align 4, !dbg !1424
  %111 = load ptr, ptr %6, align 4, !dbg !1424
  %112 = call arm_aapcs_vfpcc signext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1424
  store i8 %112, ptr %12, align 1, !dbg !1424
  call void @llvm.va_end(ptr %7), !dbg !1424
  %113 = load i8, ptr %12, align 1, !dbg !1424
  ret i8 %113, !dbg !1424
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1442 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1443, metadata !DIExpression()), !dbg !1444
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1445, metadata !DIExpression()), !dbg !1444
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1446, metadata !DIExpression()), !dbg !1444
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1447, metadata !DIExpression()), !dbg !1444
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1448, metadata !DIExpression()), !dbg !1444
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1449, metadata !DIExpression()), !dbg !1444
  %13 = load ptr, ptr %8, align 4, !dbg !1444
  %14 = load ptr, ptr %13, align 4, !dbg !1444
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1444
  %16 = load ptr, ptr %15, align 4, !dbg !1444
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1444
  %18 = load ptr, ptr %6, align 4, !dbg !1444
  %19 = load ptr, ptr %8, align 4, !dbg !1444
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1444
  store i32 %20, ptr %10, align 4, !dbg !1444
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1450, metadata !DIExpression()), !dbg !1444
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1451, metadata !DIExpression()), !dbg !1453
  store i32 0, ptr %12, align 4, !dbg !1453
  br label %21, !dbg !1453

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1453
  %23 = load i32, ptr %10, align 4, !dbg !1453
  %24 = icmp slt i32 %22, %23, !dbg !1453
  br i1 %24, label %25, label %103, !dbg !1453

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1454
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1454
  %28 = load i8, ptr %27, align 1, !dbg !1454
  %29 = sext i8 %28 to i32, !dbg !1454
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1454

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1457
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1457
  store ptr %32, ptr %5, align 4, !dbg !1457
  %33 = load i32, ptr %31, align 4, !dbg !1457
  %34 = trunc i32 %33 to i8, !dbg !1457
  %35 = load i32, ptr %12, align 4, !dbg !1457
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1457
  store i8 %34, ptr %36, align 8, !dbg !1457
  br label %99, !dbg !1457

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1457
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1457
  store ptr %39, ptr %5, align 4, !dbg !1457
  %40 = load i32, ptr %38, align 4, !dbg !1457
  %41 = trunc i32 %40 to i8, !dbg !1457
  %42 = load i32, ptr %12, align 4, !dbg !1457
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1457
  store i8 %41, ptr %43, align 8, !dbg !1457
  br label %99, !dbg !1457

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1457
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1457
  store ptr %46, ptr %5, align 4, !dbg !1457
  %47 = load i32, ptr %45, align 4, !dbg !1457
  %48 = trunc i32 %47 to i16, !dbg !1457
  %49 = load i32, ptr %12, align 4, !dbg !1457
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1457
  store i16 %48, ptr %50, align 8, !dbg !1457
  br label %99, !dbg !1457

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1457
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1457
  store ptr %53, ptr %5, align 4, !dbg !1457
  %54 = load i32, ptr %52, align 4, !dbg !1457
  %55 = trunc i32 %54 to i16, !dbg !1457
  %56 = load i32, ptr %12, align 4, !dbg !1457
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1457
  store i16 %55, ptr %57, align 8, !dbg !1457
  br label %99, !dbg !1457

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1457
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1457
  store ptr %60, ptr %5, align 4, !dbg !1457
  %61 = load i32, ptr %59, align 4, !dbg !1457
  %62 = load i32, ptr %12, align 4, !dbg !1457
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1457
  store i32 %61, ptr %63, align 8, !dbg !1457
  br label %99, !dbg !1457

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1457
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1457
  store ptr %66, ptr %5, align 4, !dbg !1457
  %67 = load i32, ptr %65, align 4, !dbg !1457
  %68 = sext i32 %67 to i64, !dbg !1457
  %69 = load i32, ptr %12, align 4, !dbg !1457
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1457
  store i64 %68, ptr %70, align 8, !dbg !1457
  br label %99, !dbg !1457

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1457
  %73 = ptrtoint ptr %72 to i32, !dbg !1457
  %74 = add i32 %73, 7, !dbg !1457
  %75 = and i32 %74, -8, !dbg !1457
  %76 = inttoptr i32 %75 to ptr, !dbg !1457
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1457
  store ptr %77, ptr %5, align 4, !dbg !1457
  %78 = load double, ptr %76, align 8, !dbg !1457
  %79 = fptrunc double %78 to float, !dbg !1457
  %80 = load i32, ptr %12, align 4, !dbg !1457
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1457
  store float %79, ptr %81, align 8, !dbg !1457
  br label %99, !dbg !1457

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1457
  %84 = ptrtoint ptr %83 to i32, !dbg !1457
  %85 = add i32 %84, 7, !dbg !1457
  %86 = and i32 %85, -8, !dbg !1457
  %87 = inttoptr i32 %86 to ptr, !dbg !1457
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1457
  store ptr %88, ptr %5, align 4, !dbg !1457
  %89 = load double, ptr %87, align 8, !dbg !1457
  %90 = load i32, ptr %12, align 4, !dbg !1457
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1457
  store double %89, ptr %91, align 8, !dbg !1457
  br label %99, !dbg !1457

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1457
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1457
  store ptr %94, ptr %5, align 4, !dbg !1457
  %95 = load ptr, ptr %93, align 4, !dbg !1457
  %96 = load i32, ptr %12, align 4, !dbg !1457
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1457
  store ptr %95, ptr %97, align 8, !dbg !1457
  br label %99, !dbg !1457

98:                                               ; preds = %25
  br label %99, !dbg !1457

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1454

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1459
  %102 = add nsw i32 %101, 1, !dbg !1459
  store i32 %102, ptr %12, align 4, !dbg !1459
  br label %21, !dbg !1459, !llvm.loop !1460

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1444
  %105 = load ptr, ptr %104, align 4, !dbg !1444
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 122, !dbg !1444
  %107 = load ptr, ptr %106, align 4, !dbg !1444
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1444
  %109 = load ptr, ptr %6, align 4, !dbg !1444
  %110 = load ptr, ptr %7, align 4, !dbg !1444
  %111 = load ptr, ptr %8, align 4, !dbg !1444
  %112 = call arm_aapcs_vfpcc signext i8 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1444
  ret i8 %112, !dbg !1444
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1461 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1462, metadata !DIExpression()), !dbg !1463
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1464, metadata !DIExpression()), !dbg !1463
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1465, metadata !DIExpression()), !dbg !1463
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1466, metadata !DIExpression()), !dbg !1463
  call void @llvm.va_start(ptr %7), !dbg !1463
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1467, metadata !DIExpression()), !dbg !1463
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1468, metadata !DIExpression()), !dbg !1463
  %13 = load ptr, ptr %6, align 4, !dbg !1463
  %14 = load ptr, ptr %13, align 4, !dbg !1463
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1463
  %16 = load ptr, ptr %15, align 4, !dbg !1463
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1463
  %18 = load ptr, ptr %4, align 4, !dbg !1463
  %19 = load ptr, ptr %6, align 4, !dbg !1463
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1463
  store i32 %20, ptr %9, align 4, !dbg !1463
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1469, metadata !DIExpression()), !dbg !1463
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1470, metadata !DIExpression()), !dbg !1472
  store i32 0, ptr %11, align 4, !dbg !1472
  br label %21, !dbg !1472

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1472
  %23 = load i32, ptr %9, align 4, !dbg !1472
  %24 = icmp slt i32 %22, %23, !dbg !1472
  br i1 %24, label %25, label %103, !dbg !1472

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1473
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1473
  %28 = load i8, ptr %27, align 1, !dbg !1473
  %29 = sext i8 %28 to i32, !dbg !1473
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1473

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1476
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1476
  store ptr %32, ptr %7, align 4, !dbg !1476
  %33 = load i32, ptr %31, align 4, !dbg !1476
  %34 = trunc i32 %33 to i8, !dbg !1476
  %35 = load i32, ptr %11, align 4, !dbg !1476
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1476
  store i8 %34, ptr %36, align 8, !dbg !1476
  br label %99, !dbg !1476

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1476
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1476
  store ptr %39, ptr %7, align 4, !dbg !1476
  %40 = load i32, ptr %38, align 4, !dbg !1476
  %41 = trunc i32 %40 to i8, !dbg !1476
  %42 = load i32, ptr %11, align 4, !dbg !1476
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1476
  store i8 %41, ptr %43, align 8, !dbg !1476
  br label %99, !dbg !1476

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1476
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1476
  store ptr %46, ptr %7, align 4, !dbg !1476
  %47 = load i32, ptr %45, align 4, !dbg !1476
  %48 = trunc i32 %47 to i16, !dbg !1476
  %49 = load i32, ptr %11, align 4, !dbg !1476
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1476
  store i16 %48, ptr %50, align 8, !dbg !1476
  br label %99, !dbg !1476

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1476
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1476
  store ptr %53, ptr %7, align 4, !dbg !1476
  %54 = load i32, ptr %52, align 4, !dbg !1476
  %55 = trunc i32 %54 to i16, !dbg !1476
  %56 = load i32, ptr %11, align 4, !dbg !1476
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1476
  store i16 %55, ptr %57, align 8, !dbg !1476
  br label %99, !dbg !1476

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1476
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1476
  store ptr %60, ptr %7, align 4, !dbg !1476
  %61 = load i32, ptr %59, align 4, !dbg !1476
  %62 = load i32, ptr %11, align 4, !dbg !1476
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1476
  store i32 %61, ptr %63, align 8, !dbg !1476
  br label %99, !dbg !1476

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1476
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1476
  store ptr %66, ptr %7, align 4, !dbg !1476
  %67 = load i32, ptr %65, align 4, !dbg !1476
  %68 = sext i32 %67 to i64, !dbg !1476
  %69 = load i32, ptr %11, align 4, !dbg !1476
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1476
  store i64 %68, ptr %70, align 8, !dbg !1476
  br label %99, !dbg !1476

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1476
  %73 = ptrtoint ptr %72 to i32, !dbg !1476
  %74 = add i32 %73, 7, !dbg !1476
  %75 = and i32 %74, -8, !dbg !1476
  %76 = inttoptr i32 %75 to ptr, !dbg !1476
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1476
  store ptr %77, ptr %7, align 4, !dbg !1476
  %78 = load double, ptr %76, align 8, !dbg !1476
  %79 = fptrunc double %78 to float, !dbg !1476
  %80 = load i32, ptr %11, align 4, !dbg !1476
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1476
  store float %79, ptr %81, align 8, !dbg !1476
  br label %99, !dbg !1476

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1476
  %84 = ptrtoint ptr %83 to i32, !dbg !1476
  %85 = add i32 %84, 7, !dbg !1476
  %86 = and i32 %85, -8, !dbg !1476
  %87 = inttoptr i32 %86 to ptr, !dbg !1476
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1476
  store ptr %88, ptr %7, align 4, !dbg !1476
  %89 = load double, ptr %87, align 8, !dbg !1476
  %90 = load i32, ptr %11, align 4, !dbg !1476
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1476
  store double %89, ptr %91, align 8, !dbg !1476
  br label %99, !dbg !1476

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1476
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1476
  store ptr %94, ptr %7, align 4, !dbg !1476
  %95 = load ptr, ptr %93, align 4, !dbg !1476
  %96 = load i32, ptr %11, align 4, !dbg !1476
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1476
  store ptr %95, ptr %97, align 8, !dbg !1476
  br label %99, !dbg !1476

98:                                               ; preds = %25
  br label %99, !dbg !1476

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1473

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1478
  %102 = add nsw i32 %101, 1, !dbg !1478
  store i32 %102, ptr %11, align 4, !dbg !1478
  br label %21, !dbg !1478, !llvm.loop !1479

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1480, metadata !DIExpression()), !dbg !1463
  %104 = load ptr, ptr %6, align 4, !dbg !1463
  %105 = load ptr, ptr %104, align 4, !dbg !1463
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 45, !dbg !1463
  %107 = load ptr, ptr %106, align 4, !dbg !1463
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1463
  %109 = load ptr, ptr %4, align 4, !dbg !1463
  %110 = load ptr, ptr %5, align 4, !dbg !1463
  %111 = load ptr, ptr %6, align 4, !dbg !1463
  %112 = call arm_aapcs_vfpcc zeroext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1463
  store i16 %112, ptr %12, align 2, !dbg !1463
  call void @llvm.va_end(ptr %7), !dbg !1463
  %113 = load i16, ptr %12, align 2, !dbg !1463
  ret i16 %113, !dbg !1463
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1481 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1482, metadata !DIExpression()), !dbg !1483
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1484, metadata !DIExpression()), !dbg !1483
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1485, metadata !DIExpression()), !dbg !1483
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1486, metadata !DIExpression()), !dbg !1483
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1487, metadata !DIExpression()), !dbg !1483
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1488, metadata !DIExpression()), !dbg !1483
  %13 = load ptr, ptr %8, align 4, !dbg !1483
  %14 = load ptr, ptr %13, align 4, !dbg !1483
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1483
  %16 = load ptr, ptr %15, align 4, !dbg !1483
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1483
  %18 = load ptr, ptr %6, align 4, !dbg !1483
  %19 = load ptr, ptr %8, align 4, !dbg !1483
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1483
  store i32 %20, ptr %10, align 4, !dbg !1483
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1489, metadata !DIExpression()), !dbg !1483
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1490, metadata !DIExpression()), !dbg !1492
  store i32 0, ptr %12, align 4, !dbg !1492
  br label %21, !dbg !1492

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1492
  %23 = load i32, ptr %10, align 4, !dbg !1492
  %24 = icmp slt i32 %22, %23, !dbg !1492
  br i1 %24, label %25, label %103, !dbg !1492

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1493
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1493
  %28 = load i8, ptr %27, align 1, !dbg !1493
  %29 = sext i8 %28 to i32, !dbg !1493
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1493

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1496
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1496
  store ptr %32, ptr %5, align 4, !dbg !1496
  %33 = load i32, ptr %31, align 4, !dbg !1496
  %34 = trunc i32 %33 to i8, !dbg !1496
  %35 = load i32, ptr %12, align 4, !dbg !1496
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1496
  store i8 %34, ptr %36, align 8, !dbg !1496
  br label %99, !dbg !1496

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1496
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1496
  store ptr %39, ptr %5, align 4, !dbg !1496
  %40 = load i32, ptr %38, align 4, !dbg !1496
  %41 = trunc i32 %40 to i8, !dbg !1496
  %42 = load i32, ptr %12, align 4, !dbg !1496
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1496
  store i8 %41, ptr %43, align 8, !dbg !1496
  br label %99, !dbg !1496

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1496
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1496
  store ptr %46, ptr %5, align 4, !dbg !1496
  %47 = load i32, ptr %45, align 4, !dbg !1496
  %48 = trunc i32 %47 to i16, !dbg !1496
  %49 = load i32, ptr %12, align 4, !dbg !1496
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1496
  store i16 %48, ptr %50, align 8, !dbg !1496
  br label %99, !dbg !1496

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1496
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1496
  store ptr %53, ptr %5, align 4, !dbg !1496
  %54 = load i32, ptr %52, align 4, !dbg !1496
  %55 = trunc i32 %54 to i16, !dbg !1496
  %56 = load i32, ptr %12, align 4, !dbg !1496
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1496
  store i16 %55, ptr %57, align 8, !dbg !1496
  br label %99, !dbg !1496

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1496
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1496
  store ptr %60, ptr %5, align 4, !dbg !1496
  %61 = load i32, ptr %59, align 4, !dbg !1496
  %62 = load i32, ptr %12, align 4, !dbg !1496
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1496
  store i32 %61, ptr %63, align 8, !dbg !1496
  br label %99, !dbg !1496

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1496
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1496
  store ptr %66, ptr %5, align 4, !dbg !1496
  %67 = load i32, ptr %65, align 4, !dbg !1496
  %68 = sext i32 %67 to i64, !dbg !1496
  %69 = load i32, ptr %12, align 4, !dbg !1496
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1496
  store i64 %68, ptr %70, align 8, !dbg !1496
  br label %99, !dbg !1496

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1496
  %73 = ptrtoint ptr %72 to i32, !dbg !1496
  %74 = add i32 %73, 7, !dbg !1496
  %75 = and i32 %74, -8, !dbg !1496
  %76 = inttoptr i32 %75 to ptr, !dbg !1496
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1496
  store ptr %77, ptr %5, align 4, !dbg !1496
  %78 = load double, ptr %76, align 8, !dbg !1496
  %79 = fptrunc double %78 to float, !dbg !1496
  %80 = load i32, ptr %12, align 4, !dbg !1496
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1496
  store float %79, ptr %81, align 8, !dbg !1496
  br label %99, !dbg !1496

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1496
  %84 = ptrtoint ptr %83 to i32, !dbg !1496
  %85 = add i32 %84, 7, !dbg !1496
  %86 = and i32 %85, -8, !dbg !1496
  %87 = inttoptr i32 %86 to ptr, !dbg !1496
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1496
  store ptr %88, ptr %5, align 4, !dbg !1496
  %89 = load double, ptr %87, align 8, !dbg !1496
  %90 = load i32, ptr %12, align 4, !dbg !1496
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1496
  store double %89, ptr %91, align 8, !dbg !1496
  br label %99, !dbg !1496

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1496
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1496
  store ptr %94, ptr %5, align 4, !dbg !1496
  %95 = load ptr, ptr %93, align 4, !dbg !1496
  %96 = load i32, ptr %12, align 4, !dbg !1496
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1496
  store ptr %95, ptr %97, align 8, !dbg !1496
  br label %99, !dbg !1496

98:                                               ; preds = %25
  br label %99, !dbg !1496

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1493

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1498
  %102 = add nsw i32 %101, 1, !dbg !1498
  store i32 %102, ptr %12, align 4, !dbg !1498
  br label %21, !dbg !1498, !llvm.loop !1499

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1483
  %105 = load ptr, ptr %104, align 4, !dbg !1483
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 45, !dbg !1483
  %107 = load ptr, ptr %106, align 4, !dbg !1483
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1483
  %109 = load ptr, ptr %6, align 4, !dbg !1483
  %110 = load ptr, ptr %7, align 4, !dbg !1483
  %111 = load ptr, ptr %8, align 4, !dbg !1483
  %112 = call arm_aapcs_vfpcc zeroext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1483
  ret i16 %112, !dbg !1483
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1500 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1501, metadata !DIExpression()), !dbg !1502
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1503, metadata !DIExpression()), !dbg !1502
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1504, metadata !DIExpression()), !dbg !1502
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1505, metadata !DIExpression()), !dbg !1502
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1506, metadata !DIExpression()), !dbg !1502
  call void @llvm.va_start(ptr %9), !dbg !1502
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1507, metadata !DIExpression()), !dbg !1502
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1508, metadata !DIExpression()), !dbg !1502
  %15 = load ptr, ptr %8, align 4, !dbg !1502
  %16 = load ptr, ptr %15, align 4, !dbg !1502
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1502
  %18 = load ptr, ptr %17, align 4, !dbg !1502
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1502
  %20 = load ptr, ptr %5, align 4, !dbg !1502
  %21 = load ptr, ptr %8, align 4, !dbg !1502
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1502
  store i32 %22, ptr %11, align 4, !dbg !1502
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1509, metadata !DIExpression()), !dbg !1502
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1510, metadata !DIExpression()), !dbg !1512
  store i32 0, ptr %13, align 4, !dbg !1512
  br label %23, !dbg !1512

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1512
  %25 = load i32, ptr %11, align 4, !dbg !1512
  %26 = icmp slt i32 %24, %25, !dbg !1512
  br i1 %26, label %27, label %105, !dbg !1512

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1513
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1513
  %30 = load i8, ptr %29, align 1, !dbg !1513
  %31 = sext i8 %30 to i32, !dbg !1513
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1513

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1516
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1516
  store ptr %34, ptr %9, align 4, !dbg !1516
  %35 = load i32, ptr %33, align 4, !dbg !1516
  %36 = trunc i32 %35 to i8, !dbg !1516
  %37 = load i32, ptr %13, align 4, !dbg !1516
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1516
  store i8 %36, ptr %38, align 8, !dbg !1516
  br label %101, !dbg !1516

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1516
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1516
  store ptr %41, ptr %9, align 4, !dbg !1516
  %42 = load i32, ptr %40, align 4, !dbg !1516
  %43 = trunc i32 %42 to i8, !dbg !1516
  %44 = load i32, ptr %13, align 4, !dbg !1516
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1516
  store i8 %43, ptr %45, align 8, !dbg !1516
  br label %101, !dbg !1516

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1516
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1516
  store ptr %48, ptr %9, align 4, !dbg !1516
  %49 = load i32, ptr %47, align 4, !dbg !1516
  %50 = trunc i32 %49 to i16, !dbg !1516
  %51 = load i32, ptr %13, align 4, !dbg !1516
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1516
  store i16 %50, ptr %52, align 8, !dbg !1516
  br label %101, !dbg !1516

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1516
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1516
  store ptr %55, ptr %9, align 4, !dbg !1516
  %56 = load i32, ptr %54, align 4, !dbg !1516
  %57 = trunc i32 %56 to i16, !dbg !1516
  %58 = load i32, ptr %13, align 4, !dbg !1516
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1516
  store i16 %57, ptr %59, align 8, !dbg !1516
  br label %101, !dbg !1516

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1516
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1516
  store ptr %62, ptr %9, align 4, !dbg !1516
  %63 = load i32, ptr %61, align 4, !dbg !1516
  %64 = load i32, ptr %13, align 4, !dbg !1516
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1516
  store i32 %63, ptr %65, align 8, !dbg !1516
  br label %101, !dbg !1516

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1516
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1516
  store ptr %68, ptr %9, align 4, !dbg !1516
  %69 = load i32, ptr %67, align 4, !dbg !1516
  %70 = sext i32 %69 to i64, !dbg !1516
  %71 = load i32, ptr %13, align 4, !dbg !1516
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1516
  store i64 %70, ptr %72, align 8, !dbg !1516
  br label %101, !dbg !1516

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1516
  %75 = ptrtoint ptr %74 to i32, !dbg !1516
  %76 = add i32 %75, 7, !dbg !1516
  %77 = and i32 %76, -8, !dbg !1516
  %78 = inttoptr i32 %77 to ptr, !dbg !1516
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1516
  store ptr %79, ptr %9, align 4, !dbg !1516
  %80 = load double, ptr %78, align 8, !dbg !1516
  %81 = fptrunc double %80 to float, !dbg !1516
  %82 = load i32, ptr %13, align 4, !dbg !1516
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1516
  store float %81, ptr %83, align 8, !dbg !1516
  br label %101, !dbg !1516

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1516
  %86 = ptrtoint ptr %85 to i32, !dbg !1516
  %87 = add i32 %86, 7, !dbg !1516
  %88 = and i32 %87, -8, !dbg !1516
  %89 = inttoptr i32 %88 to ptr, !dbg !1516
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1516
  store ptr %90, ptr %9, align 4, !dbg !1516
  %91 = load double, ptr %89, align 8, !dbg !1516
  %92 = load i32, ptr %13, align 4, !dbg !1516
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1516
  store double %91, ptr %93, align 8, !dbg !1516
  br label %101, !dbg !1516

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1516
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1516
  store ptr %96, ptr %9, align 4, !dbg !1516
  %97 = load ptr, ptr %95, align 4, !dbg !1516
  %98 = load i32, ptr %13, align 4, !dbg !1516
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1516
  store ptr %97, ptr %99, align 8, !dbg !1516
  br label %101, !dbg !1516

100:                                              ; preds = %27
  br label %101, !dbg !1516

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1513

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1518
  %104 = add nsw i32 %103, 1, !dbg !1518
  store i32 %104, ptr %13, align 4, !dbg !1518
  br label %23, !dbg !1518, !llvm.loop !1519

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1520, metadata !DIExpression()), !dbg !1502
  %106 = load ptr, ptr %8, align 4, !dbg !1502
  %107 = load ptr, ptr %106, align 4, !dbg !1502
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 75, !dbg !1502
  %109 = load ptr, ptr %108, align 4, !dbg !1502
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1502
  %111 = load ptr, ptr %5, align 4, !dbg !1502
  %112 = load ptr, ptr %6, align 4, !dbg !1502
  %113 = load ptr, ptr %7, align 4, !dbg !1502
  %114 = load ptr, ptr %8, align 4, !dbg !1502
  %115 = call arm_aapcs_vfpcc zeroext i16 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1502
  store i16 %115, ptr %14, align 2, !dbg !1502
  call void @llvm.va_end(ptr %9), !dbg !1502
  %116 = load i16, ptr %14, align 2, !dbg !1502
  ret i16 %116, !dbg !1502
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1521 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1522, metadata !DIExpression()), !dbg !1523
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1524, metadata !DIExpression()), !dbg !1523
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1525, metadata !DIExpression()), !dbg !1523
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1526, metadata !DIExpression()), !dbg !1523
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1527, metadata !DIExpression()), !dbg !1523
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1528, metadata !DIExpression()), !dbg !1523
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1529, metadata !DIExpression()), !dbg !1523
  %15 = load ptr, ptr %10, align 4, !dbg !1523
  %16 = load ptr, ptr %15, align 4, !dbg !1523
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1523
  %18 = load ptr, ptr %17, align 4, !dbg !1523
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1523
  %20 = load ptr, ptr %7, align 4, !dbg !1523
  %21 = load ptr, ptr %10, align 4, !dbg !1523
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1523
  store i32 %22, ptr %12, align 4, !dbg !1523
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1530, metadata !DIExpression()), !dbg !1523
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1531, metadata !DIExpression()), !dbg !1533
  store i32 0, ptr %14, align 4, !dbg !1533
  br label %23, !dbg !1533

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1533
  %25 = load i32, ptr %12, align 4, !dbg !1533
  %26 = icmp slt i32 %24, %25, !dbg !1533
  br i1 %26, label %27, label %105, !dbg !1533

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1534
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1534
  %30 = load i8, ptr %29, align 1, !dbg !1534
  %31 = sext i8 %30 to i32, !dbg !1534
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1534

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1537
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1537
  store ptr %34, ptr %6, align 4, !dbg !1537
  %35 = load i32, ptr %33, align 4, !dbg !1537
  %36 = trunc i32 %35 to i8, !dbg !1537
  %37 = load i32, ptr %14, align 4, !dbg !1537
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1537
  store i8 %36, ptr %38, align 8, !dbg !1537
  br label %101, !dbg !1537

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1537
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1537
  store ptr %41, ptr %6, align 4, !dbg !1537
  %42 = load i32, ptr %40, align 4, !dbg !1537
  %43 = trunc i32 %42 to i8, !dbg !1537
  %44 = load i32, ptr %14, align 4, !dbg !1537
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1537
  store i8 %43, ptr %45, align 8, !dbg !1537
  br label %101, !dbg !1537

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1537
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1537
  store ptr %48, ptr %6, align 4, !dbg !1537
  %49 = load i32, ptr %47, align 4, !dbg !1537
  %50 = trunc i32 %49 to i16, !dbg !1537
  %51 = load i32, ptr %14, align 4, !dbg !1537
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1537
  store i16 %50, ptr %52, align 8, !dbg !1537
  br label %101, !dbg !1537

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1537
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1537
  store ptr %55, ptr %6, align 4, !dbg !1537
  %56 = load i32, ptr %54, align 4, !dbg !1537
  %57 = trunc i32 %56 to i16, !dbg !1537
  %58 = load i32, ptr %14, align 4, !dbg !1537
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1537
  store i16 %57, ptr %59, align 8, !dbg !1537
  br label %101, !dbg !1537

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1537
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1537
  store ptr %62, ptr %6, align 4, !dbg !1537
  %63 = load i32, ptr %61, align 4, !dbg !1537
  %64 = load i32, ptr %14, align 4, !dbg !1537
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1537
  store i32 %63, ptr %65, align 8, !dbg !1537
  br label %101, !dbg !1537

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1537
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1537
  store ptr %68, ptr %6, align 4, !dbg !1537
  %69 = load i32, ptr %67, align 4, !dbg !1537
  %70 = sext i32 %69 to i64, !dbg !1537
  %71 = load i32, ptr %14, align 4, !dbg !1537
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1537
  store i64 %70, ptr %72, align 8, !dbg !1537
  br label %101, !dbg !1537

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1537
  %75 = ptrtoint ptr %74 to i32, !dbg !1537
  %76 = add i32 %75, 7, !dbg !1537
  %77 = and i32 %76, -8, !dbg !1537
  %78 = inttoptr i32 %77 to ptr, !dbg !1537
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1537
  store ptr %79, ptr %6, align 4, !dbg !1537
  %80 = load double, ptr %78, align 8, !dbg !1537
  %81 = fptrunc double %80 to float, !dbg !1537
  %82 = load i32, ptr %14, align 4, !dbg !1537
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1537
  store float %81, ptr %83, align 8, !dbg !1537
  br label %101, !dbg !1537

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1537
  %86 = ptrtoint ptr %85 to i32, !dbg !1537
  %87 = add i32 %86, 7, !dbg !1537
  %88 = and i32 %87, -8, !dbg !1537
  %89 = inttoptr i32 %88 to ptr, !dbg !1537
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1537
  store ptr %90, ptr %6, align 4, !dbg !1537
  %91 = load double, ptr %89, align 8, !dbg !1537
  %92 = load i32, ptr %14, align 4, !dbg !1537
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1537
  store double %91, ptr %93, align 8, !dbg !1537
  br label %101, !dbg !1537

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1537
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1537
  store ptr %96, ptr %6, align 4, !dbg !1537
  %97 = load ptr, ptr %95, align 4, !dbg !1537
  %98 = load i32, ptr %14, align 4, !dbg !1537
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1537
  store ptr %97, ptr %99, align 8, !dbg !1537
  br label %101, !dbg !1537

100:                                              ; preds = %27
  br label %101, !dbg !1537

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1534

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1539
  %104 = add nsw i32 %103, 1, !dbg !1539
  store i32 %104, ptr %14, align 4, !dbg !1539
  br label %23, !dbg !1539, !llvm.loop !1540

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1523
  %107 = load ptr, ptr %106, align 4, !dbg !1523
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 75, !dbg !1523
  %109 = load ptr, ptr %108, align 4, !dbg !1523
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1523
  %111 = load ptr, ptr %7, align 4, !dbg !1523
  %112 = load ptr, ptr %8, align 4, !dbg !1523
  %113 = load ptr, ptr %9, align 4, !dbg !1523
  %114 = load ptr, ptr %10, align 4, !dbg !1523
  %115 = call arm_aapcs_vfpcc zeroext i16 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1523
  ret i16 %115, !dbg !1523
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1541 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1542, metadata !DIExpression()), !dbg !1543
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1544, metadata !DIExpression()), !dbg !1543
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1545, metadata !DIExpression()), !dbg !1543
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1546, metadata !DIExpression()), !dbg !1543
  call void @llvm.va_start(ptr %7), !dbg !1543
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1547, metadata !DIExpression()), !dbg !1543
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1548, metadata !DIExpression()), !dbg !1543
  %13 = load ptr, ptr %6, align 4, !dbg !1543
  %14 = load ptr, ptr %13, align 4, !dbg !1543
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1543
  %16 = load ptr, ptr %15, align 4, !dbg !1543
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1543
  %18 = load ptr, ptr %4, align 4, !dbg !1543
  %19 = load ptr, ptr %6, align 4, !dbg !1543
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1543
  store i32 %20, ptr %9, align 4, !dbg !1543
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1549, metadata !DIExpression()), !dbg !1543
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1550, metadata !DIExpression()), !dbg !1552
  store i32 0, ptr %11, align 4, !dbg !1552
  br label %21, !dbg !1552

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1552
  %23 = load i32, ptr %9, align 4, !dbg !1552
  %24 = icmp slt i32 %22, %23, !dbg !1552
  br i1 %24, label %25, label %103, !dbg !1552

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1553
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1553
  %28 = load i8, ptr %27, align 1, !dbg !1553
  %29 = sext i8 %28 to i32, !dbg !1553
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1553

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1556
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1556
  store ptr %32, ptr %7, align 4, !dbg !1556
  %33 = load i32, ptr %31, align 4, !dbg !1556
  %34 = trunc i32 %33 to i8, !dbg !1556
  %35 = load i32, ptr %11, align 4, !dbg !1556
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1556
  store i8 %34, ptr %36, align 8, !dbg !1556
  br label %99, !dbg !1556

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1556
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1556
  store ptr %39, ptr %7, align 4, !dbg !1556
  %40 = load i32, ptr %38, align 4, !dbg !1556
  %41 = trunc i32 %40 to i8, !dbg !1556
  %42 = load i32, ptr %11, align 4, !dbg !1556
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1556
  store i8 %41, ptr %43, align 8, !dbg !1556
  br label %99, !dbg !1556

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1556
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1556
  store ptr %46, ptr %7, align 4, !dbg !1556
  %47 = load i32, ptr %45, align 4, !dbg !1556
  %48 = trunc i32 %47 to i16, !dbg !1556
  %49 = load i32, ptr %11, align 4, !dbg !1556
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1556
  store i16 %48, ptr %50, align 8, !dbg !1556
  br label %99, !dbg !1556

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1556
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1556
  store ptr %53, ptr %7, align 4, !dbg !1556
  %54 = load i32, ptr %52, align 4, !dbg !1556
  %55 = trunc i32 %54 to i16, !dbg !1556
  %56 = load i32, ptr %11, align 4, !dbg !1556
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1556
  store i16 %55, ptr %57, align 8, !dbg !1556
  br label %99, !dbg !1556

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1556
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1556
  store ptr %60, ptr %7, align 4, !dbg !1556
  %61 = load i32, ptr %59, align 4, !dbg !1556
  %62 = load i32, ptr %11, align 4, !dbg !1556
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1556
  store i32 %61, ptr %63, align 8, !dbg !1556
  br label %99, !dbg !1556

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1556
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1556
  store ptr %66, ptr %7, align 4, !dbg !1556
  %67 = load i32, ptr %65, align 4, !dbg !1556
  %68 = sext i32 %67 to i64, !dbg !1556
  %69 = load i32, ptr %11, align 4, !dbg !1556
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1556
  store i64 %68, ptr %70, align 8, !dbg !1556
  br label %99, !dbg !1556

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1556
  %73 = ptrtoint ptr %72 to i32, !dbg !1556
  %74 = add i32 %73, 7, !dbg !1556
  %75 = and i32 %74, -8, !dbg !1556
  %76 = inttoptr i32 %75 to ptr, !dbg !1556
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1556
  store ptr %77, ptr %7, align 4, !dbg !1556
  %78 = load double, ptr %76, align 8, !dbg !1556
  %79 = fptrunc double %78 to float, !dbg !1556
  %80 = load i32, ptr %11, align 4, !dbg !1556
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1556
  store float %79, ptr %81, align 8, !dbg !1556
  br label %99, !dbg !1556

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1556
  %84 = ptrtoint ptr %83 to i32, !dbg !1556
  %85 = add i32 %84, 7, !dbg !1556
  %86 = and i32 %85, -8, !dbg !1556
  %87 = inttoptr i32 %86 to ptr, !dbg !1556
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1556
  store ptr %88, ptr %7, align 4, !dbg !1556
  %89 = load double, ptr %87, align 8, !dbg !1556
  %90 = load i32, ptr %11, align 4, !dbg !1556
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1556
  store double %89, ptr %91, align 8, !dbg !1556
  br label %99, !dbg !1556

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1556
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1556
  store ptr %94, ptr %7, align 4, !dbg !1556
  %95 = load ptr, ptr %93, align 4, !dbg !1556
  %96 = load i32, ptr %11, align 4, !dbg !1556
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1556
  store ptr %95, ptr %97, align 8, !dbg !1556
  br label %99, !dbg !1556

98:                                               ; preds = %25
  br label %99, !dbg !1556

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1553

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1558
  %102 = add nsw i32 %101, 1, !dbg !1558
  store i32 %102, ptr %11, align 4, !dbg !1558
  br label %21, !dbg !1558, !llvm.loop !1559

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1560, metadata !DIExpression()), !dbg !1543
  %104 = load ptr, ptr %6, align 4, !dbg !1543
  %105 = load ptr, ptr %104, align 4, !dbg !1543
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 125, !dbg !1543
  %107 = load ptr, ptr %106, align 4, !dbg !1543
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1543
  %109 = load ptr, ptr %4, align 4, !dbg !1543
  %110 = load ptr, ptr %5, align 4, !dbg !1543
  %111 = load ptr, ptr %6, align 4, !dbg !1543
  %112 = call arm_aapcs_vfpcc zeroext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1543
  store i16 %112, ptr %12, align 2, !dbg !1543
  call void @llvm.va_end(ptr %7), !dbg !1543
  %113 = load i16, ptr %12, align 2, !dbg !1543
  ret i16 %113, !dbg !1543
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1561 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1562, metadata !DIExpression()), !dbg !1563
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1564, metadata !DIExpression()), !dbg !1563
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1565, metadata !DIExpression()), !dbg !1563
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1566, metadata !DIExpression()), !dbg !1563
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1567, metadata !DIExpression()), !dbg !1563
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1568, metadata !DIExpression()), !dbg !1563
  %13 = load ptr, ptr %8, align 4, !dbg !1563
  %14 = load ptr, ptr %13, align 4, !dbg !1563
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1563
  %16 = load ptr, ptr %15, align 4, !dbg !1563
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1563
  %18 = load ptr, ptr %6, align 4, !dbg !1563
  %19 = load ptr, ptr %8, align 4, !dbg !1563
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1563
  store i32 %20, ptr %10, align 4, !dbg !1563
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1569, metadata !DIExpression()), !dbg !1563
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1570, metadata !DIExpression()), !dbg !1572
  store i32 0, ptr %12, align 4, !dbg !1572
  br label %21, !dbg !1572

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1572
  %23 = load i32, ptr %10, align 4, !dbg !1572
  %24 = icmp slt i32 %22, %23, !dbg !1572
  br i1 %24, label %25, label %103, !dbg !1572

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1573
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1573
  %28 = load i8, ptr %27, align 1, !dbg !1573
  %29 = sext i8 %28 to i32, !dbg !1573
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1573

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1576
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1576
  store ptr %32, ptr %5, align 4, !dbg !1576
  %33 = load i32, ptr %31, align 4, !dbg !1576
  %34 = trunc i32 %33 to i8, !dbg !1576
  %35 = load i32, ptr %12, align 4, !dbg !1576
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1576
  store i8 %34, ptr %36, align 8, !dbg !1576
  br label %99, !dbg !1576

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1576
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1576
  store ptr %39, ptr %5, align 4, !dbg !1576
  %40 = load i32, ptr %38, align 4, !dbg !1576
  %41 = trunc i32 %40 to i8, !dbg !1576
  %42 = load i32, ptr %12, align 4, !dbg !1576
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1576
  store i8 %41, ptr %43, align 8, !dbg !1576
  br label %99, !dbg !1576

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1576
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1576
  store ptr %46, ptr %5, align 4, !dbg !1576
  %47 = load i32, ptr %45, align 4, !dbg !1576
  %48 = trunc i32 %47 to i16, !dbg !1576
  %49 = load i32, ptr %12, align 4, !dbg !1576
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1576
  store i16 %48, ptr %50, align 8, !dbg !1576
  br label %99, !dbg !1576

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1576
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1576
  store ptr %53, ptr %5, align 4, !dbg !1576
  %54 = load i32, ptr %52, align 4, !dbg !1576
  %55 = trunc i32 %54 to i16, !dbg !1576
  %56 = load i32, ptr %12, align 4, !dbg !1576
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1576
  store i16 %55, ptr %57, align 8, !dbg !1576
  br label %99, !dbg !1576

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1576
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1576
  store ptr %60, ptr %5, align 4, !dbg !1576
  %61 = load i32, ptr %59, align 4, !dbg !1576
  %62 = load i32, ptr %12, align 4, !dbg !1576
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1576
  store i32 %61, ptr %63, align 8, !dbg !1576
  br label %99, !dbg !1576

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1576
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1576
  store ptr %66, ptr %5, align 4, !dbg !1576
  %67 = load i32, ptr %65, align 4, !dbg !1576
  %68 = sext i32 %67 to i64, !dbg !1576
  %69 = load i32, ptr %12, align 4, !dbg !1576
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1576
  store i64 %68, ptr %70, align 8, !dbg !1576
  br label %99, !dbg !1576

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1576
  %73 = ptrtoint ptr %72 to i32, !dbg !1576
  %74 = add i32 %73, 7, !dbg !1576
  %75 = and i32 %74, -8, !dbg !1576
  %76 = inttoptr i32 %75 to ptr, !dbg !1576
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1576
  store ptr %77, ptr %5, align 4, !dbg !1576
  %78 = load double, ptr %76, align 8, !dbg !1576
  %79 = fptrunc double %78 to float, !dbg !1576
  %80 = load i32, ptr %12, align 4, !dbg !1576
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1576
  store float %79, ptr %81, align 8, !dbg !1576
  br label %99, !dbg !1576

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1576
  %84 = ptrtoint ptr %83 to i32, !dbg !1576
  %85 = add i32 %84, 7, !dbg !1576
  %86 = and i32 %85, -8, !dbg !1576
  %87 = inttoptr i32 %86 to ptr, !dbg !1576
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1576
  store ptr %88, ptr %5, align 4, !dbg !1576
  %89 = load double, ptr %87, align 8, !dbg !1576
  %90 = load i32, ptr %12, align 4, !dbg !1576
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1576
  store double %89, ptr %91, align 8, !dbg !1576
  br label %99, !dbg !1576

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1576
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1576
  store ptr %94, ptr %5, align 4, !dbg !1576
  %95 = load ptr, ptr %93, align 4, !dbg !1576
  %96 = load i32, ptr %12, align 4, !dbg !1576
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1576
  store ptr %95, ptr %97, align 8, !dbg !1576
  br label %99, !dbg !1576

98:                                               ; preds = %25
  br label %99, !dbg !1576

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1573

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1578
  %102 = add nsw i32 %101, 1, !dbg !1578
  store i32 %102, ptr %12, align 4, !dbg !1578
  br label %21, !dbg !1578, !llvm.loop !1579

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1563
  %105 = load ptr, ptr %104, align 4, !dbg !1563
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 125, !dbg !1563
  %107 = load ptr, ptr %106, align 4, !dbg !1563
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1563
  %109 = load ptr, ptr %6, align 4, !dbg !1563
  %110 = load ptr, ptr %7, align 4, !dbg !1563
  %111 = load ptr, ptr %8, align 4, !dbg !1563
  %112 = call arm_aapcs_vfpcc zeroext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1563
  ret i16 %112, !dbg !1563
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1580 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1581, metadata !DIExpression()), !dbg !1582
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1583, metadata !DIExpression()), !dbg !1582
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1584, metadata !DIExpression()), !dbg !1582
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1585, metadata !DIExpression()), !dbg !1582
  call void @llvm.va_start(ptr %7), !dbg !1582
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1586, metadata !DIExpression()), !dbg !1582
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1587, metadata !DIExpression()), !dbg !1582
  %13 = load ptr, ptr %6, align 4, !dbg !1582
  %14 = load ptr, ptr %13, align 4, !dbg !1582
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1582
  %16 = load ptr, ptr %15, align 4, !dbg !1582
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1582
  %18 = load ptr, ptr %4, align 4, !dbg !1582
  %19 = load ptr, ptr %6, align 4, !dbg !1582
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1582
  store i32 %20, ptr %9, align 4, !dbg !1582
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1588, metadata !DIExpression()), !dbg !1582
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1589, metadata !DIExpression()), !dbg !1591
  store i32 0, ptr %11, align 4, !dbg !1591
  br label %21, !dbg !1591

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1591
  %23 = load i32, ptr %9, align 4, !dbg !1591
  %24 = icmp slt i32 %22, %23, !dbg !1591
  br i1 %24, label %25, label %103, !dbg !1591

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1592
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1592
  %28 = load i8, ptr %27, align 1, !dbg !1592
  %29 = sext i8 %28 to i32, !dbg !1592
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1592

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1595
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1595
  store ptr %32, ptr %7, align 4, !dbg !1595
  %33 = load i32, ptr %31, align 4, !dbg !1595
  %34 = trunc i32 %33 to i8, !dbg !1595
  %35 = load i32, ptr %11, align 4, !dbg !1595
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1595
  store i8 %34, ptr %36, align 8, !dbg !1595
  br label %99, !dbg !1595

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1595
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1595
  store ptr %39, ptr %7, align 4, !dbg !1595
  %40 = load i32, ptr %38, align 4, !dbg !1595
  %41 = trunc i32 %40 to i8, !dbg !1595
  %42 = load i32, ptr %11, align 4, !dbg !1595
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1595
  store i8 %41, ptr %43, align 8, !dbg !1595
  br label %99, !dbg !1595

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1595
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1595
  store ptr %46, ptr %7, align 4, !dbg !1595
  %47 = load i32, ptr %45, align 4, !dbg !1595
  %48 = trunc i32 %47 to i16, !dbg !1595
  %49 = load i32, ptr %11, align 4, !dbg !1595
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1595
  store i16 %48, ptr %50, align 8, !dbg !1595
  br label %99, !dbg !1595

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1595
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1595
  store ptr %53, ptr %7, align 4, !dbg !1595
  %54 = load i32, ptr %52, align 4, !dbg !1595
  %55 = trunc i32 %54 to i16, !dbg !1595
  %56 = load i32, ptr %11, align 4, !dbg !1595
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1595
  store i16 %55, ptr %57, align 8, !dbg !1595
  br label %99, !dbg !1595

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1595
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1595
  store ptr %60, ptr %7, align 4, !dbg !1595
  %61 = load i32, ptr %59, align 4, !dbg !1595
  %62 = load i32, ptr %11, align 4, !dbg !1595
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1595
  store i32 %61, ptr %63, align 8, !dbg !1595
  br label %99, !dbg !1595

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1595
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1595
  store ptr %66, ptr %7, align 4, !dbg !1595
  %67 = load i32, ptr %65, align 4, !dbg !1595
  %68 = sext i32 %67 to i64, !dbg !1595
  %69 = load i32, ptr %11, align 4, !dbg !1595
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1595
  store i64 %68, ptr %70, align 8, !dbg !1595
  br label %99, !dbg !1595

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1595
  %73 = ptrtoint ptr %72 to i32, !dbg !1595
  %74 = add i32 %73, 7, !dbg !1595
  %75 = and i32 %74, -8, !dbg !1595
  %76 = inttoptr i32 %75 to ptr, !dbg !1595
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1595
  store ptr %77, ptr %7, align 4, !dbg !1595
  %78 = load double, ptr %76, align 8, !dbg !1595
  %79 = fptrunc double %78 to float, !dbg !1595
  %80 = load i32, ptr %11, align 4, !dbg !1595
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1595
  store float %79, ptr %81, align 8, !dbg !1595
  br label %99, !dbg !1595

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1595
  %84 = ptrtoint ptr %83 to i32, !dbg !1595
  %85 = add i32 %84, 7, !dbg !1595
  %86 = and i32 %85, -8, !dbg !1595
  %87 = inttoptr i32 %86 to ptr, !dbg !1595
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1595
  store ptr %88, ptr %7, align 4, !dbg !1595
  %89 = load double, ptr %87, align 8, !dbg !1595
  %90 = load i32, ptr %11, align 4, !dbg !1595
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1595
  store double %89, ptr %91, align 8, !dbg !1595
  br label %99, !dbg !1595

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1595
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1595
  store ptr %94, ptr %7, align 4, !dbg !1595
  %95 = load ptr, ptr %93, align 4, !dbg !1595
  %96 = load i32, ptr %11, align 4, !dbg !1595
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1595
  store ptr %95, ptr %97, align 8, !dbg !1595
  br label %99, !dbg !1595

98:                                               ; preds = %25
  br label %99, !dbg !1595

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1592

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1597
  %102 = add nsw i32 %101, 1, !dbg !1597
  store i32 %102, ptr %11, align 4, !dbg !1597
  br label %21, !dbg !1597, !llvm.loop !1598

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1599, metadata !DIExpression()), !dbg !1582
  %104 = load ptr, ptr %6, align 4, !dbg !1582
  %105 = load ptr, ptr %104, align 4, !dbg !1582
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 48, !dbg !1582
  %107 = load ptr, ptr %106, align 4, !dbg !1582
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1582
  %109 = load ptr, ptr %4, align 4, !dbg !1582
  %110 = load ptr, ptr %5, align 4, !dbg !1582
  %111 = load ptr, ptr %6, align 4, !dbg !1582
  %112 = call arm_aapcs_vfpcc signext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1582
  store i16 %112, ptr %12, align 2, !dbg !1582
  call void @llvm.va_end(ptr %7), !dbg !1582
  %113 = load i16, ptr %12, align 2, !dbg !1582
  ret i16 %113, !dbg !1582
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1600 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1601, metadata !DIExpression()), !dbg !1602
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1603, metadata !DIExpression()), !dbg !1602
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1604, metadata !DIExpression()), !dbg !1602
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1605, metadata !DIExpression()), !dbg !1602
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1606, metadata !DIExpression()), !dbg !1602
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1607, metadata !DIExpression()), !dbg !1602
  %13 = load ptr, ptr %8, align 4, !dbg !1602
  %14 = load ptr, ptr %13, align 4, !dbg !1602
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1602
  %16 = load ptr, ptr %15, align 4, !dbg !1602
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1602
  %18 = load ptr, ptr %6, align 4, !dbg !1602
  %19 = load ptr, ptr %8, align 4, !dbg !1602
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1602
  store i32 %20, ptr %10, align 4, !dbg !1602
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1608, metadata !DIExpression()), !dbg !1602
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1609, metadata !DIExpression()), !dbg !1611
  store i32 0, ptr %12, align 4, !dbg !1611
  br label %21, !dbg !1611

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1611
  %23 = load i32, ptr %10, align 4, !dbg !1611
  %24 = icmp slt i32 %22, %23, !dbg !1611
  br i1 %24, label %25, label %103, !dbg !1611

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1612
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1612
  %28 = load i8, ptr %27, align 1, !dbg !1612
  %29 = sext i8 %28 to i32, !dbg !1612
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1612

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1615
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1615
  store ptr %32, ptr %5, align 4, !dbg !1615
  %33 = load i32, ptr %31, align 4, !dbg !1615
  %34 = trunc i32 %33 to i8, !dbg !1615
  %35 = load i32, ptr %12, align 4, !dbg !1615
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1615
  store i8 %34, ptr %36, align 8, !dbg !1615
  br label %99, !dbg !1615

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1615
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1615
  store ptr %39, ptr %5, align 4, !dbg !1615
  %40 = load i32, ptr %38, align 4, !dbg !1615
  %41 = trunc i32 %40 to i8, !dbg !1615
  %42 = load i32, ptr %12, align 4, !dbg !1615
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1615
  store i8 %41, ptr %43, align 8, !dbg !1615
  br label %99, !dbg !1615

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1615
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1615
  store ptr %46, ptr %5, align 4, !dbg !1615
  %47 = load i32, ptr %45, align 4, !dbg !1615
  %48 = trunc i32 %47 to i16, !dbg !1615
  %49 = load i32, ptr %12, align 4, !dbg !1615
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1615
  store i16 %48, ptr %50, align 8, !dbg !1615
  br label %99, !dbg !1615

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1615
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1615
  store ptr %53, ptr %5, align 4, !dbg !1615
  %54 = load i32, ptr %52, align 4, !dbg !1615
  %55 = trunc i32 %54 to i16, !dbg !1615
  %56 = load i32, ptr %12, align 4, !dbg !1615
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1615
  store i16 %55, ptr %57, align 8, !dbg !1615
  br label %99, !dbg !1615

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1615
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1615
  store ptr %60, ptr %5, align 4, !dbg !1615
  %61 = load i32, ptr %59, align 4, !dbg !1615
  %62 = load i32, ptr %12, align 4, !dbg !1615
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1615
  store i32 %61, ptr %63, align 8, !dbg !1615
  br label %99, !dbg !1615

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1615
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1615
  store ptr %66, ptr %5, align 4, !dbg !1615
  %67 = load i32, ptr %65, align 4, !dbg !1615
  %68 = sext i32 %67 to i64, !dbg !1615
  %69 = load i32, ptr %12, align 4, !dbg !1615
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1615
  store i64 %68, ptr %70, align 8, !dbg !1615
  br label %99, !dbg !1615

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1615
  %73 = ptrtoint ptr %72 to i32, !dbg !1615
  %74 = add i32 %73, 7, !dbg !1615
  %75 = and i32 %74, -8, !dbg !1615
  %76 = inttoptr i32 %75 to ptr, !dbg !1615
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1615
  store ptr %77, ptr %5, align 4, !dbg !1615
  %78 = load double, ptr %76, align 8, !dbg !1615
  %79 = fptrunc double %78 to float, !dbg !1615
  %80 = load i32, ptr %12, align 4, !dbg !1615
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1615
  store float %79, ptr %81, align 8, !dbg !1615
  br label %99, !dbg !1615

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1615
  %84 = ptrtoint ptr %83 to i32, !dbg !1615
  %85 = add i32 %84, 7, !dbg !1615
  %86 = and i32 %85, -8, !dbg !1615
  %87 = inttoptr i32 %86 to ptr, !dbg !1615
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1615
  store ptr %88, ptr %5, align 4, !dbg !1615
  %89 = load double, ptr %87, align 8, !dbg !1615
  %90 = load i32, ptr %12, align 4, !dbg !1615
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1615
  store double %89, ptr %91, align 8, !dbg !1615
  br label %99, !dbg !1615

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1615
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1615
  store ptr %94, ptr %5, align 4, !dbg !1615
  %95 = load ptr, ptr %93, align 4, !dbg !1615
  %96 = load i32, ptr %12, align 4, !dbg !1615
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1615
  store ptr %95, ptr %97, align 8, !dbg !1615
  br label %99, !dbg !1615

98:                                               ; preds = %25
  br label %99, !dbg !1615

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1612

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1617
  %102 = add nsw i32 %101, 1, !dbg !1617
  store i32 %102, ptr %12, align 4, !dbg !1617
  br label %21, !dbg !1617, !llvm.loop !1618

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1602
  %105 = load ptr, ptr %104, align 4, !dbg !1602
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 48, !dbg !1602
  %107 = load ptr, ptr %106, align 4, !dbg !1602
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1602
  %109 = load ptr, ptr %6, align 4, !dbg !1602
  %110 = load ptr, ptr %7, align 4, !dbg !1602
  %111 = load ptr, ptr %8, align 4, !dbg !1602
  %112 = call arm_aapcs_vfpcc signext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1602
  ret i16 %112, !dbg !1602
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1619 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1620, metadata !DIExpression()), !dbg !1621
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1622, metadata !DIExpression()), !dbg !1621
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1623, metadata !DIExpression()), !dbg !1621
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1624, metadata !DIExpression()), !dbg !1621
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1625, metadata !DIExpression()), !dbg !1621
  call void @llvm.va_start(ptr %9), !dbg !1621
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1626, metadata !DIExpression()), !dbg !1621
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1627, metadata !DIExpression()), !dbg !1621
  %15 = load ptr, ptr %8, align 4, !dbg !1621
  %16 = load ptr, ptr %15, align 4, !dbg !1621
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1621
  %18 = load ptr, ptr %17, align 4, !dbg !1621
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1621
  %20 = load ptr, ptr %5, align 4, !dbg !1621
  %21 = load ptr, ptr %8, align 4, !dbg !1621
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1621
  store i32 %22, ptr %11, align 4, !dbg !1621
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1628, metadata !DIExpression()), !dbg !1621
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1629, metadata !DIExpression()), !dbg !1631
  store i32 0, ptr %13, align 4, !dbg !1631
  br label %23, !dbg !1631

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1631
  %25 = load i32, ptr %11, align 4, !dbg !1631
  %26 = icmp slt i32 %24, %25, !dbg !1631
  br i1 %26, label %27, label %105, !dbg !1631

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1632
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1632
  %30 = load i8, ptr %29, align 1, !dbg !1632
  %31 = sext i8 %30 to i32, !dbg !1632
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1632

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1635
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1635
  store ptr %34, ptr %9, align 4, !dbg !1635
  %35 = load i32, ptr %33, align 4, !dbg !1635
  %36 = trunc i32 %35 to i8, !dbg !1635
  %37 = load i32, ptr %13, align 4, !dbg !1635
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1635
  store i8 %36, ptr %38, align 8, !dbg !1635
  br label %101, !dbg !1635

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1635
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1635
  store ptr %41, ptr %9, align 4, !dbg !1635
  %42 = load i32, ptr %40, align 4, !dbg !1635
  %43 = trunc i32 %42 to i8, !dbg !1635
  %44 = load i32, ptr %13, align 4, !dbg !1635
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1635
  store i8 %43, ptr %45, align 8, !dbg !1635
  br label %101, !dbg !1635

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1635
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1635
  store ptr %48, ptr %9, align 4, !dbg !1635
  %49 = load i32, ptr %47, align 4, !dbg !1635
  %50 = trunc i32 %49 to i16, !dbg !1635
  %51 = load i32, ptr %13, align 4, !dbg !1635
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1635
  store i16 %50, ptr %52, align 8, !dbg !1635
  br label %101, !dbg !1635

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1635
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1635
  store ptr %55, ptr %9, align 4, !dbg !1635
  %56 = load i32, ptr %54, align 4, !dbg !1635
  %57 = trunc i32 %56 to i16, !dbg !1635
  %58 = load i32, ptr %13, align 4, !dbg !1635
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1635
  store i16 %57, ptr %59, align 8, !dbg !1635
  br label %101, !dbg !1635

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1635
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1635
  store ptr %62, ptr %9, align 4, !dbg !1635
  %63 = load i32, ptr %61, align 4, !dbg !1635
  %64 = load i32, ptr %13, align 4, !dbg !1635
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1635
  store i32 %63, ptr %65, align 8, !dbg !1635
  br label %101, !dbg !1635

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1635
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1635
  store ptr %68, ptr %9, align 4, !dbg !1635
  %69 = load i32, ptr %67, align 4, !dbg !1635
  %70 = sext i32 %69 to i64, !dbg !1635
  %71 = load i32, ptr %13, align 4, !dbg !1635
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1635
  store i64 %70, ptr %72, align 8, !dbg !1635
  br label %101, !dbg !1635

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1635
  %75 = ptrtoint ptr %74 to i32, !dbg !1635
  %76 = add i32 %75, 7, !dbg !1635
  %77 = and i32 %76, -8, !dbg !1635
  %78 = inttoptr i32 %77 to ptr, !dbg !1635
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1635
  store ptr %79, ptr %9, align 4, !dbg !1635
  %80 = load double, ptr %78, align 8, !dbg !1635
  %81 = fptrunc double %80 to float, !dbg !1635
  %82 = load i32, ptr %13, align 4, !dbg !1635
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1635
  store float %81, ptr %83, align 8, !dbg !1635
  br label %101, !dbg !1635

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1635
  %86 = ptrtoint ptr %85 to i32, !dbg !1635
  %87 = add i32 %86, 7, !dbg !1635
  %88 = and i32 %87, -8, !dbg !1635
  %89 = inttoptr i32 %88 to ptr, !dbg !1635
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1635
  store ptr %90, ptr %9, align 4, !dbg !1635
  %91 = load double, ptr %89, align 8, !dbg !1635
  %92 = load i32, ptr %13, align 4, !dbg !1635
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1635
  store double %91, ptr %93, align 8, !dbg !1635
  br label %101, !dbg !1635

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1635
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1635
  store ptr %96, ptr %9, align 4, !dbg !1635
  %97 = load ptr, ptr %95, align 4, !dbg !1635
  %98 = load i32, ptr %13, align 4, !dbg !1635
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1635
  store ptr %97, ptr %99, align 8, !dbg !1635
  br label %101, !dbg !1635

100:                                              ; preds = %27
  br label %101, !dbg !1635

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1632

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1637
  %104 = add nsw i32 %103, 1, !dbg !1637
  store i32 %104, ptr %13, align 4, !dbg !1637
  br label %23, !dbg !1637, !llvm.loop !1638

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1639, metadata !DIExpression()), !dbg !1621
  %106 = load ptr, ptr %8, align 4, !dbg !1621
  %107 = load ptr, ptr %106, align 4, !dbg !1621
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 78, !dbg !1621
  %109 = load ptr, ptr %108, align 4, !dbg !1621
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1621
  %111 = load ptr, ptr %5, align 4, !dbg !1621
  %112 = load ptr, ptr %6, align 4, !dbg !1621
  %113 = load ptr, ptr %7, align 4, !dbg !1621
  %114 = load ptr, ptr %8, align 4, !dbg !1621
  %115 = call arm_aapcs_vfpcc signext i16 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1621
  store i16 %115, ptr %14, align 2, !dbg !1621
  call void @llvm.va_end(ptr %9), !dbg !1621
  %116 = load i16, ptr %14, align 2, !dbg !1621
  ret i16 %116, !dbg !1621
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1640 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1641, metadata !DIExpression()), !dbg !1642
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1643, metadata !DIExpression()), !dbg !1642
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1644, metadata !DIExpression()), !dbg !1642
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1645, metadata !DIExpression()), !dbg !1642
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1646, metadata !DIExpression()), !dbg !1642
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1647, metadata !DIExpression()), !dbg !1642
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1648, metadata !DIExpression()), !dbg !1642
  %15 = load ptr, ptr %10, align 4, !dbg !1642
  %16 = load ptr, ptr %15, align 4, !dbg !1642
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1642
  %18 = load ptr, ptr %17, align 4, !dbg !1642
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1642
  %20 = load ptr, ptr %7, align 4, !dbg !1642
  %21 = load ptr, ptr %10, align 4, !dbg !1642
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1642
  store i32 %22, ptr %12, align 4, !dbg !1642
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1649, metadata !DIExpression()), !dbg !1642
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1650, metadata !DIExpression()), !dbg !1652
  store i32 0, ptr %14, align 4, !dbg !1652
  br label %23, !dbg !1652

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1652
  %25 = load i32, ptr %12, align 4, !dbg !1652
  %26 = icmp slt i32 %24, %25, !dbg !1652
  br i1 %26, label %27, label %105, !dbg !1652

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1653
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1653
  %30 = load i8, ptr %29, align 1, !dbg !1653
  %31 = sext i8 %30 to i32, !dbg !1653
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1653

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1656
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1656
  store ptr %34, ptr %6, align 4, !dbg !1656
  %35 = load i32, ptr %33, align 4, !dbg !1656
  %36 = trunc i32 %35 to i8, !dbg !1656
  %37 = load i32, ptr %14, align 4, !dbg !1656
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1656
  store i8 %36, ptr %38, align 8, !dbg !1656
  br label %101, !dbg !1656

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1656
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1656
  store ptr %41, ptr %6, align 4, !dbg !1656
  %42 = load i32, ptr %40, align 4, !dbg !1656
  %43 = trunc i32 %42 to i8, !dbg !1656
  %44 = load i32, ptr %14, align 4, !dbg !1656
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1656
  store i8 %43, ptr %45, align 8, !dbg !1656
  br label %101, !dbg !1656

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1656
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1656
  store ptr %48, ptr %6, align 4, !dbg !1656
  %49 = load i32, ptr %47, align 4, !dbg !1656
  %50 = trunc i32 %49 to i16, !dbg !1656
  %51 = load i32, ptr %14, align 4, !dbg !1656
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1656
  store i16 %50, ptr %52, align 8, !dbg !1656
  br label %101, !dbg !1656

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1656
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1656
  store ptr %55, ptr %6, align 4, !dbg !1656
  %56 = load i32, ptr %54, align 4, !dbg !1656
  %57 = trunc i32 %56 to i16, !dbg !1656
  %58 = load i32, ptr %14, align 4, !dbg !1656
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1656
  store i16 %57, ptr %59, align 8, !dbg !1656
  br label %101, !dbg !1656

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1656
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1656
  store ptr %62, ptr %6, align 4, !dbg !1656
  %63 = load i32, ptr %61, align 4, !dbg !1656
  %64 = load i32, ptr %14, align 4, !dbg !1656
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1656
  store i32 %63, ptr %65, align 8, !dbg !1656
  br label %101, !dbg !1656

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1656
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1656
  store ptr %68, ptr %6, align 4, !dbg !1656
  %69 = load i32, ptr %67, align 4, !dbg !1656
  %70 = sext i32 %69 to i64, !dbg !1656
  %71 = load i32, ptr %14, align 4, !dbg !1656
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1656
  store i64 %70, ptr %72, align 8, !dbg !1656
  br label %101, !dbg !1656

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1656
  %75 = ptrtoint ptr %74 to i32, !dbg !1656
  %76 = add i32 %75, 7, !dbg !1656
  %77 = and i32 %76, -8, !dbg !1656
  %78 = inttoptr i32 %77 to ptr, !dbg !1656
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1656
  store ptr %79, ptr %6, align 4, !dbg !1656
  %80 = load double, ptr %78, align 8, !dbg !1656
  %81 = fptrunc double %80 to float, !dbg !1656
  %82 = load i32, ptr %14, align 4, !dbg !1656
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1656
  store float %81, ptr %83, align 8, !dbg !1656
  br label %101, !dbg !1656

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1656
  %86 = ptrtoint ptr %85 to i32, !dbg !1656
  %87 = add i32 %86, 7, !dbg !1656
  %88 = and i32 %87, -8, !dbg !1656
  %89 = inttoptr i32 %88 to ptr, !dbg !1656
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1656
  store ptr %90, ptr %6, align 4, !dbg !1656
  %91 = load double, ptr %89, align 8, !dbg !1656
  %92 = load i32, ptr %14, align 4, !dbg !1656
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1656
  store double %91, ptr %93, align 8, !dbg !1656
  br label %101, !dbg !1656

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1656
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1656
  store ptr %96, ptr %6, align 4, !dbg !1656
  %97 = load ptr, ptr %95, align 4, !dbg !1656
  %98 = load i32, ptr %14, align 4, !dbg !1656
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1656
  store ptr %97, ptr %99, align 8, !dbg !1656
  br label %101, !dbg !1656

100:                                              ; preds = %27
  br label %101, !dbg !1656

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1653

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1658
  %104 = add nsw i32 %103, 1, !dbg !1658
  store i32 %104, ptr %14, align 4, !dbg !1658
  br label %23, !dbg !1658, !llvm.loop !1659

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1642
  %107 = load ptr, ptr %106, align 4, !dbg !1642
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 78, !dbg !1642
  %109 = load ptr, ptr %108, align 4, !dbg !1642
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1642
  %111 = load ptr, ptr %7, align 4, !dbg !1642
  %112 = load ptr, ptr %8, align 4, !dbg !1642
  %113 = load ptr, ptr %9, align 4, !dbg !1642
  %114 = load ptr, ptr %10, align 4, !dbg !1642
  %115 = call arm_aapcs_vfpcc signext i16 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1642
  ret i16 %115, !dbg !1642
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1660 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1661, metadata !DIExpression()), !dbg !1662
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1663, metadata !DIExpression()), !dbg !1662
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1664, metadata !DIExpression()), !dbg !1662
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1665, metadata !DIExpression()), !dbg !1662
  call void @llvm.va_start(ptr %7), !dbg !1662
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1666, metadata !DIExpression()), !dbg !1662
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1667, metadata !DIExpression()), !dbg !1662
  %13 = load ptr, ptr %6, align 4, !dbg !1662
  %14 = load ptr, ptr %13, align 4, !dbg !1662
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1662
  %16 = load ptr, ptr %15, align 4, !dbg !1662
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1662
  %18 = load ptr, ptr %4, align 4, !dbg !1662
  %19 = load ptr, ptr %6, align 4, !dbg !1662
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1662
  store i32 %20, ptr %9, align 4, !dbg !1662
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1668, metadata !DIExpression()), !dbg !1662
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1669, metadata !DIExpression()), !dbg !1671
  store i32 0, ptr %11, align 4, !dbg !1671
  br label %21, !dbg !1671

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1671
  %23 = load i32, ptr %9, align 4, !dbg !1671
  %24 = icmp slt i32 %22, %23, !dbg !1671
  br i1 %24, label %25, label %103, !dbg !1671

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1672
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1672
  %28 = load i8, ptr %27, align 1, !dbg !1672
  %29 = sext i8 %28 to i32, !dbg !1672
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1672

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1675
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1675
  store ptr %32, ptr %7, align 4, !dbg !1675
  %33 = load i32, ptr %31, align 4, !dbg !1675
  %34 = trunc i32 %33 to i8, !dbg !1675
  %35 = load i32, ptr %11, align 4, !dbg !1675
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1675
  store i8 %34, ptr %36, align 8, !dbg !1675
  br label %99, !dbg !1675

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1675
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1675
  store ptr %39, ptr %7, align 4, !dbg !1675
  %40 = load i32, ptr %38, align 4, !dbg !1675
  %41 = trunc i32 %40 to i8, !dbg !1675
  %42 = load i32, ptr %11, align 4, !dbg !1675
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1675
  store i8 %41, ptr %43, align 8, !dbg !1675
  br label %99, !dbg !1675

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1675
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1675
  store ptr %46, ptr %7, align 4, !dbg !1675
  %47 = load i32, ptr %45, align 4, !dbg !1675
  %48 = trunc i32 %47 to i16, !dbg !1675
  %49 = load i32, ptr %11, align 4, !dbg !1675
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1675
  store i16 %48, ptr %50, align 8, !dbg !1675
  br label %99, !dbg !1675

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1675
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1675
  store ptr %53, ptr %7, align 4, !dbg !1675
  %54 = load i32, ptr %52, align 4, !dbg !1675
  %55 = trunc i32 %54 to i16, !dbg !1675
  %56 = load i32, ptr %11, align 4, !dbg !1675
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1675
  store i16 %55, ptr %57, align 8, !dbg !1675
  br label %99, !dbg !1675

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1675
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1675
  store ptr %60, ptr %7, align 4, !dbg !1675
  %61 = load i32, ptr %59, align 4, !dbg !1675
  %62 = load i32, ptr %11, align 4, !dbg !1675
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1675
  store i32 %61, ptr %63, align 8, !dbg !1675
  br label %99, !dbg !1675

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1675
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1675
  store ptr %66, ptr %7, align 4, !dbg !1675
  %67 = load i32, ptr %65, align 4, !dbg !1675
  %68 = sext i32 %67 to i64, !dbg !1675
  %69 = load i32, ptr %11, align 4, !dbg !1675
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1675
  store i64 %68, ptr %70, align 8, !dbg !1675
  br label %99, !dbg !1675

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1675
  %73 = ptrtoint ptr %72 to i32, !dbg !1675
  %74 = add i32 %73, 7, !dbg !1675
  %75 = and i32 %74, -8, !dbg !1675
  %76 = inttoptr i32 %75 to ptr, !dbg !1675
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1675
  store ptr %77, ptr %7, align 4, !dbg !1675
  %78 = load double, ptr %76, align 8, !dbg !1675
  %79 = fptrunc double %78 to float, !dbg !1675
  %80 = load i32, ptr %11, align 4, !dbg !1675
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1675
  store float %79, ptr %81, align 8, !dbg !1675
  br label %99, !dbg !1675

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1675
  %84 = ptrtoint ptr %83 to i32, !dbg !1675
  %85 = add i32 %84, 7, !dbg !1675
  %86 = and i32 %85, -8, !dbg !1675
  %87 = inttoptr i32 %86 to ptr, !dbg !1675
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1675
  store ptr %88, ptr %7, align 4, !dbg !1675
  %89 = load double, ptr %87, align 8, !dbg !1675
  %90 = load i32, ptr %11, align 4, !dbg !1675
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1675
  store double %89, ptr %91, align 8, !dbg !1675
  br label %99, !dbg !1675

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1675
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1675
  store ptr %94, ptr %7, align 4, !dbg !1675
  %95 = load ptr, ptr %93, align 4, !dbg !1675
  %96 = load i32, ptr %11, align 4, !dbg !1675
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1675
  store ptr %95, ptr %97, align 8, !dbg !1675
  br label %99, !dbg !1675

98:                                               ; preds = %25
  br label %99, !dbg !1675

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1672

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1677
  %102 = add nsw i32 %101, 1, !dbg !1677
  store i32 %102, ptr %11, align 4, !dbg !1677
  br label %21, !dbg !1677, !llvm.loop !1678

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1679, metadata !DIExpression()), !dbg !1662
  %104 = load ptr, ptr %6, align 4, !dbg !1662
  %105 = load ptr, ptr %104, align 4, !dbg !1662
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 128, !dbg !1662
  %107 = load ptr, ptr %106, align 4, !dbg !1662
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1662
  %109 = load ptr, ptr %4, align 4, !dbg !1662
  %110 = load ptr, ptr %5, align 4, !dbg !1662
  %111 = load ptr, ptr %6, align 4, !dbg !1662
  %112 = call arm_aapcs_vfpcc signext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1662
  store i16 %112, ptr %12, align 2, !dbg !1662
  call void @llvm.va_end(ptr %7), !dbg !1662
  %113 = load i16, ptr %12, align 2, !dbg !1662
  ret i16 %113, !dbg !1662
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1680 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1681, metadata !DIExpression()), !dbg !1682
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1683, metadata !DIExpression()), !dbg !1682
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1684, metadata !DIExpression()), !dbg !1682
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1685, metadata !DIExpression()), !dbg !1682
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1686, metadata !DIExpression()), !dbg !1682
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1687, metadata !DIExpression()), !dbg !1682
  %13 = load ptr, ptr %8, align 4, !dbg !1682
  %14 = load ptr, ptr %13, align 4, !dbg !1682
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1682
  %16 = load ptr, ptr %15, align 4, !dbg !1682
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1682
  %18 = load ptr, ptr %6, align 4, !dbg !1682
  %19 = load ptr, ptr %8, align 4, !dbg !1682
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1682
  store i32 %20, ptr %10, align 4, !dbg !1682
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1688, metadata !DIExpression()), !dbg !1682
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1689, metadata !DIExpression()), !dbg !1691
  store i32 0, ptr %12, align 4, !dbg !1691
  br label %21, !dbg !1691

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1691
  %23 = load i32, ptr %10, align 4, !dbg !1691
  %24 = icmp slt i32 %22, %23, !dbg !1691
  br i1 %24, label %25, label %103, !dbg !1691

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1692
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1692
  %28 = load i8, ptr %27, align 1, !dbg !1692
  %29 = sext i8 %28 to i32, !dbg !1692
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1692

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1695
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1695
  store ptr %32, ptr %5, align 4, !dbg !1695
  %33 = load i32, ptr %31, align 4, !dbg !1695
  %34 = trunc i32 %33 to i8, !dbg !1695
  %35 = load i32, ptr %12, align 4, !dbg !1695
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1695
  store i8 %34, ptr %36, align 8, !dbg !1695
  br label %99, !dbg !1695

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1695
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1695
  store ptr %39, ptr %5, align 4, !dbg !1695
  %40 = load i32, ptr %38, align 4, !dbg !1695
  %41 = trunc i32 %40 to i8, !dbg !1695
  %42 = load i32, ptr %12, align 4, !dbg !1695
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1695
  store i8 %41, ptr %43, align 8, !dbg !1695
  br label %99, !dbg !1695

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1695
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1695
  store ptr %46, ptr %5, align 4, !dbg !1695
  %47 = load i32, ptr %45, align 4, !dbg !1695
  %48 = trunc i32 %47 to i16, !dbg !1695
  %49 = load i32, ptr %12, align 4, !dbg !1695
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1695
  store i16 %48, ptr %50, align 8, !dbg !1695
  br label %99, !dbg !1695

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1695
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1695
  store ptr %53, ptr %5, align 4, !dbg !1695
  %54 = load i32, ptr %52, align 4, !dbg !1695
  %55 = trunc i32 %54 to i16, !dbg !1695
  %56 = load i32, ptr %12, align 4, !dbg !1695
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1695
  store i16 %55, ptr %57, align 8, !dbg !1695
  br label %99, !dbg !1695

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1695
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1695
  store ptr %60, ptr %5, align 4, !dbg !1695
  %61 = load i32, ptr %59, align 4, !dbg !1695
  %62 = load i32, ptr %12, align 4, !dbg !1695
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1695
  store i32 %61, ptr %63, align 8, !dbg !1695
  br label %99, !dbg !1695

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1695
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1695
  store ptr %66, ptr %5, align 4, !dbg !1695
  %67 = load i32, ptr %65, align 4, !dbg !1695
  %68 = sext i32 %67 to i64, !dbg !1695
  %69 = load i32, ptr %12, align 4, !dbg !1695
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1695
  store i64 %68, ptr %70, align 8, !dbg !1695
  br label %99, !dbg !1695

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1695
  %73 = ptrtoint ptr %72 to i32, !dbg !1695
  %74 = add i32 %73, 7, !dbg !1695
  %75 = and i32 %74, -8, !dbg !1695
  %76 = inttoptr i32 %75 to ptr, !dbg !1695
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1695
  store ptr %77, ptr %5, align 4, !dbg !1695
  %78 = load double, ptr %76, align 8, !dbg !1695
  %79 = fptrunc double %78 to float, !dbg !1695
  %80 = load i32, ptr %12, align 4, !dbg !1695
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1695
  store float %79, ptr %81, align 8, !dbg !1695
  br label %99, !dbg !1695

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1695
  %84 = ptrtoint ptr %83 to i32, !dbg !1695
  %85 = add i32 %84, 7, !dbg !1695
  %86 = and i32 %85, -8, !dbg !1695
  %87 = inttoptr i32 %86 to ptr, !dbg !1695
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1695
  store ptr %88, ptr %5, align 4, !dbg !1695
  %89 = load double, ptr %87, align 8, !dbg !1695
  %90 = load i32, ptr %12, align 4, !dbg !1695
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1695
  store double %89, ptr %91, align 8, !dbg !1695
  br label %99, !dbg !1695

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1695
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1695
  store ptr %94, ptr %5, align 4, !dbg !1695
  %95 = load ptr, ptr %93, align 4, !dbg !1695
  %96 = load i32, ptr %12, align 4, !dbg !1695
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1695
  store ptr %95, ptr %97, align 8, !dbg !1695
  br label %99, !dbg !1695

98:                                               ; preds = %25
  br label %99, !dbg !1695

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1692

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1697
  %102 = add nsw i32 %101, 1, !dbg !1697
  store i32 %102, ptr %12, align 4, !dbg !1697
  br label %21, !dbg !1697, !llvm.loop !1698

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1682
  %105 = load ptr, ptr %104, align 4, !dbg !1682
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 128, !dbg !1682
  %107 = load ptr, ptr %106, align 4, !dbg !1682
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1682
  %109 = load ptr, ptr %6, align 4, !dbg !1682
  %110 = load ptr, ptr %7, align 4, !dbg !1682
  %111 = load ptr, ptr %8, align 4, !dbg !1682
  %112 = call arm_aapcs_vfpcc signext i16 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1682
  ret i16 %112, !dbg !1682
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1699 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1700, metadata !DIExpression()), !dbg !1701
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1702, metadata !DIExpression()), !dbg !1701
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1703, metadata !DIExpression()), !dbg !1701
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1704, metadata !DIExpression()), !dbg !1701
  call void @llvm.va_start(ptr %7), !dbg !1701
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1705, metadata !DIExpression()), !dbg !1701
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1706, metadata !DIExpression()), !dbg !1701
  %13 = load ptr, ptr %6, align 4, !dbg !1701
  %14 = load ptr, ptr %13, align 4, !dbg !1701
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1701
  %16 = load ptr, ptr %15, align 4, !dbg !1701
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1701
  %18 = load ptr, ptr %4, align 4, !dbg !1701
  %19 = load ptr, ptr %6, align 4, !dbg !1701
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1701
  store i32 %20, ptr %9, align 4, !dbg !1701
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1707, metadata !DIExpression()), !dbg !1701
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1708, metadata !DIExpression()), !dbg !1710
  store i32 0, ptr %11, align 4, !dbg !1710
  br label %21, !dbg !1710

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1710
  %23 = load i32, ptr %9, align 4, !dbg !1710
  %24 = icmp slt i32 %22, %23, !dbg !1710
  br i1 %24, label %25, label %103, !dbg !1710

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1711
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1711
  %28 = load i8, ptr %27, align 1, !dbg !1711
  %29 = sext i8 %28 to i32, !dbg !1711
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1711

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1714
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1714
  store ptr %32, ptr %7, align 4, !dbg !1714
  %33 = load i32, ptr %31, align 4, !dbg !1714
  %34 = trunc i32 %33 to i8, !dbg !1714
  %35 = load i32, ptr %11, align 4, !dbg !1714
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1714
  store i8 %34, ptr %36, align 8, !dbg !1714
  br label %99, !dbg !1714

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1714
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1714
  store ptr %39, ptr %7, align 4, !dbg !1714
  %40 = load i32, ptr %38, align 4, !dbg !1714
  %41 = trunc i32 %40 to i8, !dbg !1714
  %42 = load i32, ptr %11, align 4, !dbg !1714
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1714
  store i8 %41, ptr %43, align 8, !dbg !1714
  br label %99, !dbg !1714

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1714
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1714
  store ptr %46, ptr %7, align 4, !dbg !1714
  %47 = load i32, ptr %45, align 4, !dbg !1714
  %48 = trunc i32 %47 to i16, !dbg !1714
  %49 = load i32, ptr %11, align 4, !dbg !1714
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1714
  store i16 %48, ptr %50, align 8, !dbg !1714
  br label %99, !dbg !1714

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1714
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1714
  store ptr %53, ptr %7, align 4, !dbg !1714
  %54 = load i32, ptr %52, align 4, !dbg !1714
  %55 = trunc i32 %54 to i16, !dbg !1714
  %56 = load i32, ptr %11, align 4, !dbg !1714
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1714
  store i16 %55, ptr %57, align 8, !dbg !1714
  br label %99, !dbg !1714

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1714
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1714
  store ptr %60, ptr %7, align 4, !dbg !1714
  %61 = load i32, ptr %59, align 4, !dbg !1714
  %62 = load i32, ptr %11, align 4, !dbg !1714
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1714
  store i32 %61, ptr %63, align 8, !dbg !1714
  br label %99, !dbg !1714

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1714
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1714
  store ptr %66, ptr %7, align 4, !dbg !1714
  %67 = load i32, ptr %65, align 4, !dbg !1714
  %68 = sext i32 %67 to i64, !dbg !1714
  %69 = load i32, ptr %11, align 4, !dbg !1714
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1714
  store i64 %68, ptr %70, align 8, !dbg !1714
  br label %99, !dbg !1714

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1714
  %73 = ptrtoint ptr %72 to i32, !dbg !1714
  %74 = add i32 %73, 7, !dbg !1714
  %75 = and i32 %74, -8, !dbg !1714
  %76 = inttoptr i32 %75 to ptr, !dbg !1714
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1714
  store ptr %77, ptr %7, align 4, !dbg !1714
  %78 = load double, ptr %76, align 8, !dbg !1714
  %79 = fptrunc double %78 to float, !dbg !1714
  %80 = load i32, ptr %11, align 4, !dbg !1714
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1714
  store float %79, ptr %81, align 8, !dbg !1714
  br label %99, !dbg !1714

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1714
  %84 = ptrtoint ptr %83 to i32, !dbg !1714
  %85 = add i32 %84, 7, !dbg !1714
  %86 = and i32 %85, -8, !dbg !1714
  %87 = inttoptr i32 %86 to ptr, !dbg !1714
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1714
  store ptr %88, ptr %7, align 4, !dbg !1714
  %89 = load double, ptr %87, align 8, !dbg !1714
  %90 = load i32, ptr %11, align 4, !dbg !1714
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1714
  store double %89, ptr %91, align 8, !dbg !1714
  br label %99, !dbg !1714

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1714
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1714
  store ptr %94, ptr %7, align 4, !dbg !1714
  %95 = load ptr, ptr %93, align 4, !dbg !1714
  %96 = load i32, ptr %11, align 4, !dbg !1714
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1714
  store ptr %95, ptr %97, align 8, !dbg !1714
  br label %99, !dbg !1714

98:                                               ; preds = %25
  br label %99, !dbg !1714

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1711

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1716
  %102 = add nsw i32 %101, 1, !dbg !1716
  store i32 %102, ptr %11, align 4, !dbg !1716
  br label %21, !dbg !1716, !llvm.loop !1717

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1718, metadata !DIExpression()), !dbg !1701
  %104 = load ptr, ptr %6, align 4, !dbg !1701
  %105 = load ptr, ptr %104, align 4, !dbg !1701
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 51, !dbg !1701
  %107 = load ptr, ptr %106, align 4, !dbg !1701
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1701
  %109 = load ptr, ptr %4, align 4, !dbg !1701
  %110 = load ptr, ptr %5, align 4, !dbg !1701
  %111 = load ptr, ptr %6, align 4, !dbg !1701
  %112 = call arm_aapcs_vfpcc i32 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1701
  store i32 %112, ptr %12, align 4, !dbg !1701
  call void @llvm.va_end(ptr %7), !dbg !1701
  %113 = load i32, ptr %12, align 4, !dbg !1701
  ret i32 %113, !dbg !1701
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1719 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1720, metadata !DIExpression()), !dbg !1721
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1722, metadata !DIExpression()), !dbg !1721
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1723, metadata !DIExpression()), !dbg !1721
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1724, metadata !DIExpression()), !dbg !1721
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1725, metadata !DIExpression()), !dbg !1721
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1726, metadata !DIExpression()), !dbg !1721
  %13 = load ptr, ptr %8, align 4, !dbg !1721
  %14 = load ptr, ptr %13, align 4, !dbg !1721
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1721
  %16 = load ptr, ptr %15, align 4, !dbg !1721
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1721
  %18 = load ptr, ptr %6, align 4, !dbg !1721
  %19 = load ptr, ptr %8, align 4, !dbg !1721
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1721
  store i32 %20, ptr %10, align 4, !dbg !1721
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1727, metadata !DIExpression()), !dbg !1721
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1728, metadata !DIExpression()), !dbg !1730
  store i32 0, ptr %12, align 4, !dbg !1730
  br label %21, !dbg !1730

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1730
  %23 = load i32, ptr %10, align 4, !dbg !1730
  %24 = icmp slt i32 %22, %23, !dbg !1730
  br i1 %24, label %25, label %103, !dbg !1730

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1731
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1731
  %28 = load i8, ptr %27, align 1, !dbg !1731
  %29 = sext i8 %28 to i32, !dbg !1731
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1731

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1734
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1734
  store ptr %32, ptr %5, align 4, !dbg !1734
  %33 = load i32, ptr %31, align 4, !dbg !1734
  %34 = trunc i32 %33 to i8, !dbg !1734
  %35 = load i32, ptr %12, align 4, !dbg !1734
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1734
  store i8 %34, ptr %36, align 8, !dbg !1734
  br label %99, !dbg !1734

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1734
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1734
  store ptr %39, ptr %5, align 4, !dbg !1734
  %40 = load i32, ptr %38, align 4, !dbg !1734
  %41 = trunc i32 %40 to i8, !dbg !1734
  %42 = load i32, ptr %12, align 4, !dbg !1734
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1734
  store i8 %41, ptr %43, align 8, !dbg !1734
  br label %99, !dbg !1734

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1734
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1734
  store ptr %46, ptr %5, align 4, !dbg !1734
  %47 = load i32, ptr %45, align 4, !dbg !1734
  %48 = trunc i32 %47 to i16, !dbg !1734
  %49 = load i32, ptr %12, align 4, !dbg !1734
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1734
  store i16 %48, ptr %50, align 8, !dbg !1734
  br label %99, !dbg !1734

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1734
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1734
  store ptr %53, ptr %5, align 4, !dbg !1734
  %54 = load i32, ptr %52, align 4, !dbg !1734
  %55 = trunc i32 %54 to i16, !dbg !1734
  %56 = load i32, ptr %12, align 4, !dbg !1734
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1734
  store i16 %55, ptr %57, align 8, !dbg !1734
  br label %99, !dbg !1734

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1734
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1734
  store ptr %60, ptr %5, align 4, !dbg !1734
  %61 = load i32, ptr %59, align 4, !dbg !1734
  %62 = load i32, ptr %12, align 4, !dbg !1734
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1734
  store i32 %61, ptr %63, align 8, !dbg !1734
  br label %99, !dbg !1734

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1734
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1734
  store ptr %66, ptr %5, align 4, !dbg !1734
  %67 = load i32, ptr %65, align 4, !dbg !1734
  %68 = sext i32 %67 to i64, !dbg !1734
  %69 = load i32, ptr %12, align 4, !dbg !1734
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1734
  store i64 %68, ptr %70, align 8, !dbg !1734
  br label %99, !dbg !1734

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1734
  %73 = ptrtoint ptr %72 to i32, !dbg !1734
  %74 = add i32 %73, 7, !dbg !1734
  %75 = and i32 %74, -8, !dbg !1734
  %76 = inttoptr i32 %75 to ptr, !dbg !1734
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1734
  store ptr %77, ptr %5, align 4, !dbg !1734
  %78 = load double, ptr %76, align 8, !dbg !1734
  %79 = fptrunc double %78 to float, !dbg !1734
  %80 = load i32, ptr %12, align 4, !dbg !1734
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1734
  store float %79, ptr %81, align 8, !dbg !1734
  br label %99, !dbg !1734

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1734
  %84 = ptrtoint ptr %83 to i32, !dbg !1734
  %85 = add i32 %84, 7, !dbg !1734
  %86 = and i32 %85, -8, !dbg !1734
  %87 = inttoptr i32 %86 to ptr, !dbg !1734
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1734
  store ptr %88, ptr %5, align 4, !dbg !1734
  %89 = load double, ptr %87, align 8, !dbg !1734
  %90 = load i32, ptr %12, align 4, !dbg !1734
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1734
  store double %89, ptr %91, align 8, !dbg !1734
  br label %99, !dbg !1734

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1734
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1734
  store ptr %94, ptr %5, align 4, !dbg !1734
  %95 = load ptr, ptr %93, align 4, !dbg !1734
  %96 = load i32, ptr %12, align 4, !dbg !1734
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1734
  store ptr %95, ptr %97, align 8, !dbg !1734
  br label %99, !dbg !1734

98:                                               ; preds = %25
  br label %99, !dbg !1734

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1731

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1736
  %102 = add nsw i32 %101, 1, !dbg !1736
  store i32 %102, ptr %12, align 4, !dbg !1736
  br label %21, !dbg !1736, !llvm.loop !1737

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1721
  %105 = load ptr, ptr %104, align 4, !dbg !1721
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 51, !dbg !1721
  %107 = load ptr, ptr %106, align 4, !dbg !1721
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1721
  %109 = load ptr, ptr %6, align 4, !dbg !1721
  %110 = load ptr, ptr %7, align 4, !dbg !1721
  %111 = load ptr, ptr %8, align 4, !dbg !1721
  %112 = call arm_aapcs_vfpcc i32 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1721
  ret i32 %112, !dbg !1721
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1738 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1739, metadata !DIExpression()), !dbg !1740
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1741, metadata !DIExpression()), !dbg !1740
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1742, metadata !DIExpression()), !dbg !1740
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1743, metadata !DIExpression()), !dbg !1740
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1744, metadata !DIExpression()), !dbg !1740
  call void @llvm.va_start(ptr %9), !dbg !1740
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1745, metadata !DIExpression()), !dbg !1740
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1746, metadata !DIExpression()), !dbg !1740
  %15 = load ptr, ptr %8, align 4, !dbg !1740
  %16 = load ptr, ptr %15, align 4, !dbg !1740
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1740
  %18 = load ptr, ptr %17, align 4, !dbg !1740
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1740
  %20 = load ptr, ptr %5, align 4, !dbg !1740
  %21 = load ptr, ptr %8, align 4, !dbg !1740
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1740
  store i32 %22, ptr %11, align 4, !dbg !1740
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1747, metadata !DIExpression()), !dbg !1740
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1748, metadata !DIExpression()), !dbg !1750
  store i32 0, ptr %13, align 4, !dbg !1750
  br label %23, !dbg !1750

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1750
  %25 = load i32, ptr %11, align 4, !dbg !1750
  %26 = icmp slt i32 %24, %25, !dbg !1750
  br i1 %26, label %27, label %105, !dbg !1750

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1751
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1751
  %30 = load i8, ptr %29, align 1, !dbg !1751
  %31 = sext i8 %30 to i32, !dbg !1751
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1751

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1754
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1754
  store ptr %34, ptr %9, align 4, !dbg !1754
  %35 = load i32, ptr %33, align 4, !dbg !1754
  %36 = trunc i32 %35 to i8, !dbg !1754
  %37 = load i32, ptr %13, align 4, !dbg !1754
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1754
  store i8 %36, ptr %38, align 8, !dbg !1754
  br label %101, !dbg !1754

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1754
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1754
  store ptr %41, ptr %9, align 4, !dbg !1754
  %42 = load i32, ptr %40, align 4, !dbg !1754
  %43 = trunc i32 %42 to i8, !dbg !1754
  %44 = load i32, ptr %13, align 4, !dbg !1754
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1754
  store i8 %43, ptr %45, align 8, !dbg !1754
  br label %101, !dbg !1754

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1754
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1754
  store ptr %48, ptr %9, align 4, !dbg !1754
  %49 = load i32, ptr %47, align 4, !dbg !1754
  %50 = trunc i32 %49 to i16, !dbg !1754
  %51 = load i32, ptr %13, align 4, !dbg !1754
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1754
  store i16 %50, ptr %52, align 8, !dbg !1754
  br label %101, !dbg !1754

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1754
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1754
  store ptr %55, ptr %9, align 4, !dbg !1754
  %56 = load i32, ptr %54, align 4, !dbg !1754
  %57 = trunc i32 %56 to i16, !dbg !1754
  %58 = load i32, ptr %13, align 4, !dbg !1754
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1754
  store i16 %57, ptr %59, align 8, !dbg !1754
  br label %101, !dbg !1754

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1754
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1754
  store ptr %62, ptr %9, align 4, !dbg !1754
  %63 = load i32, ptr %61, align 4, !dbg !1754
  %64 = load i32, ptr %13, align 4, !dbg !1754
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1754
  store i32 %63, ptr %65, align 8, !dbg !1754
  br label %101, !dbg !1754

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1754
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1754
  store ptr %68, ptr %9, align 4, !dbg !1754
  %69 = load i32, ptr %67, align 4, !dbg !1754
  %70 = sext i32 %69 to i64, !dbg !1754
  %71 = load i32, ptr %13, align 4, !dbg !1754
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1754
  store i64 %70, ptr %72, align 8, !dbg !1754
  br label %101, !dbg !1754

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1754
  %75 = ptrtoint ptr %74 to i32, !dbg !1754
  %76 = add i32 %75, 7, !dbg !1754
  %77 = and i32 %76, -8, !dbg !1754
  %78 = inttoptr i32 %77 to ptr, !dbg !1754
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1754
  store ptr %79, ptr %9, align 4, !dbg !1754
  %80 = load double, ptr %78, align 8, !dbg !1754
  %81 = fptrunc double %80 to float, !dbg !1754
  %82 = load i32, ptr %13, align 4, !dbg !1754
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1754
  store float %81, ptr %83, align 8, !dbg !1754
  br label %101, !dbg !1754

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1754
  %86 = ptrtoint ptr %85 to i32, !dbg !1754
  %87 = add i32 %86, 7, !dbg !1754
  %88 = and i32 %87, -8, !dbg !1754
  %89 = inttoptr i32 %88 to ptr, !dbg !1754
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1754
  store ptr %90, ptr %9, align 4, !dbg !1754
  %91 = load double, ptr %89, align 8, !dbg !1754
  %92 = load i32, ptr %13, align 4, !dbg !1754
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1754
  store double %91, ptr %93, align 8, !dbg !1754
  br label %101, !dbg !1754

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1754
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1754
  store ptr %96, ptr %9, align 4, !dbg !1754
  %97 = load ptr, ptr %95, align 4, !dbg !1754
  %98 = load i32, ptr %13, align 4, !dbg !1754
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1754
  store ptr %97, ptr %99, align 8, !dbg !1754
  br label %101, !dbg !1754

100:                                              ; preds = %27
  br label %101, !dbg !1754

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1751

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1756
  %104 = add nsw i32 %103, 1, !dbg !1756
  store i32 %104, ptr %13, align 4, !dbg !1756
  br label %23, !dbg !1756, !llvm.loop !1757

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1758, metadata !DIExpression()), !dbg !1740
  %106 = load ptr, ptr %8, align 4, !dbg !1740
  %107 = load ptr, ptr %106, align 4, !dbg !1740
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 81, !dbg !1740
  %109 = load ptr, ptr %108, align 4, !dbg !1740
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1740
  %111 = load ptr, ptr %5, align 4, !dbg !1740
  %112 = load ptr, ptr %6, align 4, !dbg !1740
  %113 = load ptr, ptr %7, align 4, !dbg !1740
  %114 = load ptr, ptr %8, align 4, !dbg !1740
  %115 = call arm_aapcs_vfpcc i32 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1740
  store i32 %115, ptr %14, align 4, !dbg !1740
  call void @llvm.va_end(ptr %9), !dbg !1740
  %116 = load i32, ptr %14, align 4, !dbg !1740
  ret i32 %116, !dbg !1740
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1759 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1760, metadata !DIExpression()), !dbg !1761
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1762, metadata !DIExpression()), !dbg !1761
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1763, metadata !DIExpression()), !dbg !1761
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1764, metadata !DIExpression()), !dbg !1761
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1765, metadata !DIExpression()), !dbg !1761
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1766, metadata !DIExpression()), !dbg !1761
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1767, metadata !DIExpression()), !dbg !1761
  %15 = load ptr, ptr %10, align 4, !dbg !1761
  %16 = load ptr, ptr %15, align 4, !dbg !1761
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1761
  %18 = load ptr, ptr %17, align 4, !dbg !1761
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1761
  %20 = load ptr, ptr %7, align 4, !dbg !1761
  %21 = load ptr, ptr %10, align 4, !dbg !1761
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1761
  store i32 %22, ptr %12, align 4, !dbg !1761
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1768, metadata !DIExpression()), !dbg !1761
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1769, metadata !DIExpression()), !dbg !1771
  store i32 0, ptr %14, align 4, !dbg !1771
  br label %23, !dbg !1771

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1771
  %25 = load i32, ptr %12, align 4, !dbg !1771
  %26 = icmp slt i32 %24, %25, !dbg !1771
  br i1 %26, label %27, label %105, !dbg !1771

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1772
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1772
  %30 = load i8, ptr %29, align 1, !dbg !1772
  %31 = sext i8 %30 to i32, !dbg !1772
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1772

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1775
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1775
  store ptr %34, ptr %6, align 4, !dbg !1775
  %35 = load i32, ptr %33, align 4, !dbg !1775
  %36 = trunc i32 %35 to i8, !dbg !1775
  %37 = load i32, ptr %14, align 4, !dbg !1775
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1775
  store i8 %36, ptr %38, align 8, !dbg !1775
  br label %101, !dbg !1775

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1775
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1775
  store ptr %41, ptr %6, align 4, !dbg !1775
  %42 = load i32, ptr %40, align 4, !dbg !1775
  %43 = trunc i32 %42 to i8, !dbg !1775
  %44 = load i32, ptr %14, align 4, !dbg !1775
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1775
  store i8 %43, ptr %45, align 8, !dbg !1775
  br label %101, !dbg !1775

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1775
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1775
  store ptr %48, ptr %6, align 4, !dbg !1775
  %49 = load i32, ptr %47, align 4, !dbg !1775
  %50 = trunc i32 %49 to i16, !dbg !1775
  %51 = load i32, ptr %14, align 4, !dbg !1775
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1775
  store i16 %50, ptr %52, align 8, !dbg !1775
  br label %101, !dbg !1775

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1775
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1775
  store ptr %55, ptr %6, align 4, !dbg !1775
  %56 = load i32, ptr %54, align 4, !dbg !1775
  %57 = trunc i32 %56 to i16, !dbg !1775
  %58 = load i32, ptr %14, align 4, !dbg !1775
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1775
  store i16 %57, ptr %59, align 8, !dbg !1775
  br label %101, !dbg !1775

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1775
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1775
  store ptr %62, ptr %6, align 4, !dbg !1775
  %63 = load i32, ptr %61, align 4, !dbg !1775
  %64 = load i32, ptr %14, align 4, !dbg !1775
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1775
  store i32 %63, ptr %65, align 8, !dbg !1775
  br label %101, !dbg !1775

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1775
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1775
  store ptr %68, ptr %6, align 4, !dbg !1775
  %69 = load i32, ptr %67, align 4, !dbg !1775
  %70 = sext i32 %69 to i64, !dbg !1775
  %71 = load i32, ptr %14, align 4, !dbg !1775
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1775
  store i64 %70, ptr %72, align 8, !dbg !1775
  br label %101, !dbg !1775

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1775
  %75 = ptrtoint ptr %74 to i32, !dbg !1775
  %76 = add i32 %75, 7, !dbg !1775
  %77 = and i32 %76, -8, !dbg !1775
  %78 = inttoptr i32 %77 to ptr, !dbg !1775
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1775
  store ptr %79, ptr %6, align 4, !dbg !1775
  %80 = load double, ptr %78, align 8, !dbg !1775
  %81 = fptrunc double %80 to float, !dbg !1775
  %82 = load i32, ptr %14, align 4, !dbg !1775
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1775
  store float %81, ptr %83, align 8, !dbg !1775
  br label %101, !dbg !1775

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1775
  %86 = ptrtoint ptr %85 to i32, !dbg !1775
  %87 = add i32 %86, 7, !dbg !1775
  %88 = and i32 %87, -8, !dbg !1775
  %89 = inttoptr i32 %88 to ptr, !dbg !1775
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1775
  store ptr %90, ptr %6, align 4, !dbg !1775
  %91 = load double, ptr %89, align 8, !dbg !1775
  %92 = load i32, ptr %14, align 4, !dbg !1775
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1775
  store double %91, ptr %93, align 8, !dbg !1775
  br label %101, !dbg !1775

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1775
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1775
  store ptr %96, ptr %6, align 4, !dbg !1775
  %97 = load ptr, ptr %95, align 4, !dbg !1775
  %98 = load i32, ptr %14, align 4, !dbg !1775
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1775
  store ptr %97, ptr %99, align 8, !dbg !1775
  br label %101, !dbg !1775

100:                                              ; preds = %27
  br label %101, !dbg !1775

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1772

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1777
  %104 = add nsw i32 %103, 1, !dbg !1777
  store i32 %104, ptr %14, align 4, !dbg !1777
  br label %23, !dbg !1777, !llvm.loop !1778

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1761
  %107 = load ptr, ptr %106, align 4, !dbg !1761
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 81, !dbg !1761
  %109 = load ptr, ptr %108, align 4, !dbg !1761
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1761
  %111 = load ptr, ptr %7, align 4, !dbg !1761
  %112 = load ptr, ptr %8, align 4, !dbg !1761
  %113 = load ptr, ptr %9, align 4, !dbg !1761
  %114 = load ptr, ptr %10, align 4, !dbg !1761
  %115 = call arm_aapcs_vfpcc i32 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1761
  ret i32 %115, !dbg !1761
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1779 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1780, metadata !DIExpression()), !dbg !1781
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1782, metadata !DIExpression()), !dbg !1781
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1783, metadata !DIExpression()), !dbg !1781
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1784, metadata !DIExpression()), !dbg !1781
  call void @llvm.va_start(ptr %7), !dbg !1781
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1785, metadata !DIExpression()), !dbg !1781
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1786, metadata !DIExpression()), !dbg !1781
  %13 = load ptr, ptr %6, align 4, !dbg !1781
  %14 = load ptr, ptr %13, align 4, !dbg !1781
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1781
  %16 = load ptr, ptr %15, align 4, !dbg !1781
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1781
  %18 = load ptr, ptr %4, align 4, !dbg !1781
  %19 = load ptr, ptr %6, align 4, !dbg !1781
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1781
  store i32 %20, ptr %9, align 4, !dbg !1781
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1787, metadata !DIExpression()), !dbg !1781
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1788, metadata !DIExpression()), !dbg !1790
  store i32 0, ptr %11, align 4, !dbg !1790
  br label %21, !dbg !1790

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1790
  %23 = load i32, ptr %9, align 4, !dbg !1790
  %24 = icmp slt i32 %22, %23, !dbg !1790
  br i1 %24, label %25, label %103, !dbg !1790

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1791
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1791
  %28 = load i8, ptr %27, align 1, !dbg !1791
  %29 = sext i8 %28 to i32, !dbg !1791
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1791

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1794
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1794
  store ptr %32, ptr %7, align 4, !dbg !1794
  %33 = load i32, ptr %31, align 4, !dbg !1794
  %34 = trunc i32 %33 to i8, !dbg !1794
  %35 = load i32, ptr %11, align 4, !dbg !1794
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1794
  store i8 %34, ptr %36, align 8, !dbg !1794
  br label %99, !dbg !1794

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1794
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1794
  store ptr %39, ptr %7, align 4, !dbg !1794
  %40 = load i32, ptr %38, align 4, !dbg !1794
  %41 = trunc i32 %40 to i8, !dbg !1794
  %42 = load i32, ptr %11, align 4, !dbg !1794
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1794
  store i8 %41, ptr %43, align 8, !dbg !1794
  br label %99, !dbg !1794

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1794
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1794
  store ptr %46, ptr %7, align 4, !dbg !1794
  %47 = load i32, ptr %45, align 4, !dbg !1794
  %48 = trunc i32 %47 to i16, !dbg !1794
  %49 = load i32, ptr %11, align 4, !dbg !1794
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1794
  store i16 %48, ptr %50, align 8, !dbg !1794
  br label %99, !dbg !1794

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1794
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1794
  store ptr %53, ptr %7, align 4, !dbg !1794
  %54 = load i32, ptr %52, align 4, !dbg !1794
  %55 = trunc i32 %54 to i16, !dbg !1794
  %56 = load i32, ptr %11, align 4, !dbg !1794
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1794
  store i16 %55, ptr %57, align 8, !dbg !1794
  br label %99, !dbg !1794

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1794
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1794
  store ptr %60, ptr %7, align 4, !dbg !1794
  %61 = load i32, ptr %59, align 4, !dbg !1794
  %62 = load i32, ptr %11, align 4, !dbg !1794
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1794
  store i32 %61, ptr %63, align 8, !dbg !1794
  br label %99, !dbg !1794

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1794
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1794
  store ptr %66, ptr %7, align 4, !dbg !1794
  %67 = load i32, ptr %65, align 4, !dbg !1794
  %68 = sext i32 %67 to i64, !dbg !1794
  %69 = load i32, ptr %11, align 4, !dbg !1794
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1794
  store i64 %68, ptr %70, align 8, !dbg !1794
  br label %99, !dbg !1794

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1794
  %73 = ptrtoint ptr %72 to i32, !dbg !1794
  %74 = add i32 %73, 7, !dbg !1794
  %75 = and i32 %74, -8, !dbg !1794
  %76 = inttoptr i32 %75 to ptr, !dbg !1794
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1794
  store ptr %77, ptr %7, align 4, !dbg !1794
  %78 = load double, ptr %76, align 8, !dbg !1794
  %79 = fptrunc double %78 to float, !dbg !1794
  %80 = load i32, ptr %11, align 4, !dbg !1794
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1794
  store float %79, ptr %81, align 8, !dbg !1794
  br label %99, !dbg !1794

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1794
  %84 = ptrtoint ptr %83 to i32, !dbg !1794
  %85 = add i32 %84, 7, !dbg !1794
  %86 = and i32 %85, -8, !dbg !1794
  %87 = inttoptr i32 %86 to ptr, !dbg !1794
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1794
  store ptr %88, ptr %7, align 4, !dbg !1794
  %89 = load double, ptr %87, align 8, !dbg !1794
  %90 = load i32, ptr %11, align 4, !dbg !1794
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1794
  store double %89, ptr %91, align 8, !dbg !1794
  br label %99, !dbg !1794

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1794
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1794
  store ptr %94, ptr %7, align 4, !dbg !1794
  %95 = load ptr, ptr %93, align 4, !dbg !1794
  %96 = load i32, ptr %11, align 4, !dbg !1794
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1794
  store ptr %95, ptr %97, align 8, !dbg !1794
  br label %99, !dbg !1794

98:                                               ; preds = %25
  br label %99, !dbg !1794

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1791

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1796
  %102 = add nsw i32 %101, 1, !dbg !1796
  store i32 %102, ptr %11, align 4, !dbg !1796
  br label %21, !dbg !1796, !llvm.loop !1797

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1798, metadata !DIExpression()), !dbg !1781
  %104 = load ptr, ptr %6, align 4, !dbg !1781
  %105 = load ptr, ptr %104, align 4, !dbg !1781
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 131, !dbg !1781
  %107 = load ptr, ptr %106, align 4, !dbg !1781
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1781
  %109 = load ptr, ptr %4, align 4, !dbg !1781
  %110 = load ptr, ptr %5, align 4, !dbg !1781
  %111 = load ptr, ptr %6, align 4, !dbg !1781
  %112 = call arm_aapcs_vfpcc i32 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1781
  store i32 %112, ptr %12, align 4, !dbg !1781
  call void @llvm.va_end(ptr %7), !dbg !1781
  %113 = load i32, ptr %12, align 4, !dbg !1781
  ret i32 %113, !dbg !1781
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1799 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1800, metadata !DIExpression()), !dbg !1801
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1802, metadata !DIExpression()), !dbg !1801
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1803, metadata !DIExpression()), !dbg !1801
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1804, metadata !DIExpression()), !dbg !1801
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1805, metadata !DIExpression()), !dbg !1801
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1806, metadata !DIExpression()), !dbg !1801
  %13 = load ptr, ptr %8, align 4, !dbg !1801
  %14 = load ptr, ptr %13, align 4, !dbg !1801
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1801
  %16 = load ptr, ptr %15, align 4, !dbg !1801
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1801
  %18 = load ptr, ptr %6, align 4, !dbg !1801
  %19 = load ptr, ptr %8, align 4, !dbg !1801
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1801
  store i32 %20, ptr %10, align 4, !dbg !1801
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1807, metadata !DIExpression()), !dbg !1801
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1808, metadata !DIExpression()), !dbg !1810
  store i32 0, ptr %12, align 4, !dbg !1810
  br label %21, !dbg !1810

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1810
  %23 = load i32, ptr %10, align 4, !dbg !1810
  %24 = icmp slt i32 %22, %23, !dbg !1810
  br i1 %24, label %25, label %103, !dbg !1810

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1811
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1811
  %28 = load i8, ptr %27, align 1, !dbg !1811
  %29 = sext i8 %28 to i32, !dbg !1811
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1811

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1814
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1814
  store ptr %32, ptr %5, align 4, !dbg !1814
  %33 = load i32, ptr %31, align 4, !dbg !1814
  %34 = trunc i32 %33 to i8, !dbg !1814
  %35 = load i32, ptr %12, align 4, !dbg !1814
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1814
  store i8 %34, ptr %36, align 8, !dbg !1814
  br label %99, !dbg !1814

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1814
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1814
  store ptr %39, ptr %5, align 4, !dbg !1814
  %40 = load i32, ptr %38, align 4, !dbg !1814
  %41 = trunc i32 %40 to i8, !dbg !1814
  %42 = load i32, ptr %12, align 4, !dbg !1814
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1814
  store i8 %41, ptr %43, align 8, !dbg !1814
  br label %99, !dbg !1814

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1814
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1814
  store ptr %46, ptr %5, align 4, !dbg !1814
  %47 = load i32, ptr %45, align 4, !dbg !1814
  %48 = trunc i32 %47 to i16, !dbg !1814
  %49 = load i32, ptr %12, align 4, !dbg !1814
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1814
  store i16 %48, ptr %50, align 8, !dbg !1814
  br label %99, !dbg !1814

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1814
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1814
  store ptr %53, ptr %5, align 4, !dbg !1814
  %54 = load i32, ptr %52, align 4, !dbg !1814
  %55 = trunc i32 %54 to i16, !dbg !1814
  %56 = load i32, ptr %12, align 4, !dbg !1814
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1814
  store i16 %55, ptr %57, align 8, !dbg !1814
  br label %99, !dbg !1814

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1814
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1814
  store ptr %60, ptr %5, align 4, !dbg !1814
  %61 = load i32, ptr %59, align 4, !dbg !1814
  %62 = load i32, ptr %12, align 4, !dbg !1814
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1814
  store i32 %61, ptr %63, align 8, !dbg !1814
  br label %99, !dbg !1814

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1814
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1814
  store ptr %66, ptr %5, align 4, !dbg !1814
  %67 = load i32, ptr %65, align 4, !dbg !1814
  %68 = sext i32 %67 to i64, !dbg !1814
  %69 = load i32, ptr %12, align 4, !dbg !1814
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1814
  store i64 %68, ptr %70, align 8, !dbg !1814
  br label %99, !dbg !1814

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1814
  %73 = ptrtoint ptr %72 to i32, !dbg !1814
  %74 = add i32 %73, 7, !dbg !1814
  %75 = and i32 %74, -8, !dbg !1814
  %76 = inttoptr i32 %75 to ptr, !dbg !1814
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1814
  store ptr %77, ptr %5, align 4, !dbg !1814
  %78 = load double, ptr %76, align 8, !dbg !1814
  %79 = fptrunc double %78 to float, !dbg !1814
  %80 = load i32, ptr %12, align 4, !dbg !1814
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1814
  store float %79, ptr %81, align 8, !dbg !1814
  br label %99, !dbg !1814

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1814
  %84 = ptrtoint ptr %83 to i32, !dbg !1814
  %85 = add i32 %84, 7, !dbg !1814
  %86 = and i32 %85, -8, !dbg !1814
  %87 = inttoptr i32 %86 to ptr, !dbg !1814
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1814
  store ptr %88, ptr %5, align 4, !dbg !1814
  %89 = load double, ptr %87, align 8, !dbg !1814
  %90 = load i32, ptr %12, align 4, !dbg !1814
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1814
  store double %89, ptr %91, align 8, !dbg !1814
  br label %99, !dbg !1814

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1814
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1814
  store ptr %94, ptr %5, align 4, !dbg !1814
  %95 = load ptr, ptr %93, align 4, !dbg !1814
  %96 = load i32, ptr %12, align 4, !dbg !1814
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1814
  store ptr %95, ptr %97, align 8, !dbg !1814
  br label %99, !dbg !1814

98:                                               ; preds = %25
  br label %99, !dbg !1814

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1811

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1816
  %102 = add nsw i32 %101, 1, !dbg !1816
  store i32 %102, ptr %12, align 4, !dbg !1816
  br label %21, !dbg !1816, !llvm.loop !1817

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1801
  %105 = load ptr, ptr %104, align 4, !dbg !1801
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 131, !dbg !1801
  %107 = load ptr, ptr %106, align 4, !dbg !1801
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1801
  %109 = load ptr, ptr %6, align 4, !dbg !1801
  %110 = load ptr, ptr %7, align 4, !dbg !1801
  %111 = load ptr, ptr %8, align 4, !dbg !1801
  %112 = call arm_aapcs_vfpcc i32 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1801
  ret i32 %112, !dbg !1801
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1818 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1819, metadata !DIExpression()), !dbg !1820
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1821, metadata !DIExpression()), !dbg !1820
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1822, metadata !DIExpression()), !dbg !1820
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1823, metadata !DIExpression()), !dbg !1820
  call void @llvm.va_start(ptr %7), !dbg !1820
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1824, metadata !DIExpression()), !dbg !1820
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1825, metadata !DIExpression()), !dbg !1820
  %13 = load ptr, ptr %6, align 4, !dbg !1820
  %14 = load ptr, ptr %13, align 4, !dbg !1820
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1820
  %16 = load ptr, ptr %15, align 4, !dbg !1820
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1820
  %18 = load ptr, ptr %4, align 4, !dbg !1820
  %19 = load ptr, ptr %6, align 4, !dbg !1820
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1820
  store i32 %20, ptr %9, align 4, !dbg !1820
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1826, metadata !DIExpression()), !dbg !1820
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1827, metadata !DIExpression()), !dbg !1829
  store i32 0, ptr %11, align 4, !dbg !1829
  br label %21, !dbg !1829

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1829
  %23 = load i32, ptr %9, align 4, !dbg !1829
  %24 = icmp slt i32 %22, %23, !dbg !1829
  br i1 %24, label %25, label %103, !dbg !1829

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1830
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1830
  %28 = load i8, ptr %27, align 1, !dbg !1830
  %29 = sext i8 %28 to i32, !dbg !1830
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1830

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1833
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1833
  store ptr %32, ptr %7, align 4, !dbg !1833
  %33 = load i32, ptr %31, align 4, !dbg !1833
  %34 = trunc i32 %33 to i8, !dbg !1833
  %35 = load i32, ptr %11, align 4, !dbg !1833
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1833
  store i8 %34, ptr %36, align 8, !dbg !1833
  br label %99, !dbg !1833

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1833
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1833
  store ptr %39, ptr %7, align 4, !dbg !1833
  %40 = load i32, ptr %38, align 4, !dbg !1833
  %41 = trunc i32 %40 to i8, !dbg !1833
  %42 = load i32, ptr %11, align 4, !dbg !1833
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1833
  store i8 %41, ptr %43, align 8, !dbg !1833
  br label %99, !dbg !1833

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1833
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1833
  store ptr %46, ptr %7, align 4, !dbg !1833
  %47 = load i32, ptr %45, align 4, !dbg !1833
  %48 = trunc i32 %47 to i16, !dbg !1833
  %49 = load i32, ptr %11, align 4, !dbg !1833
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1833
  store i16 %48, ptr %50, align 8, !dbg !1833
  br label %99, !dbg !1833

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1833
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1833
  store ptr %53, ptr %7, align 4, !dbg !1833
  %54 = load i32, ptr %52, align 4, !dbg !1833
  %55 = trunc i32 %54 to i16, !dbg !1833
  %56 = load i32, ptr %11, align 4, !dbg !1833
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1833
  store i16 %55, ptr %57, align 8, !dbg !1833
  br label %99, !dbg !1833

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1833
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1833
  store ptr %60, ptr %7, align 4, !dbg !1833
  %61 = load i32, ptr %59, align 4, !dbg !1833
  %62 = load i32, ptr %11, align 4, !dbg !1833
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1833
  store i32 %61, ptr %63, align 8, !dbg !1833
  br label %99, !dbg !1833

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1833
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1833
  store ptr %66, ptr %7, align 4, !dbg !1833
  %67 = load i32, ptr %65, align 4, !dbg !1833
  %68 = sext i32 %67 to i64, !dbg !1833
  %69 = load i32, ptr %11, align 4, !dbg !1833
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1833
  store i64 %68, ptr %70, align 8, !dbg !1833
  br label %99, !dbg !1833

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1833
  %73 = ptrtoint ptr %72 to i32, !dbg !1833
  %74 = add i32 %73, 7, !dbg !1833
  %75 = and i32 %74, -8, !dbg !1833
  %76 = inttoptr i32 %75 to ptr, !dbg !1833
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1833
  store ptr %77, ptr %7, align 4, !dbg !1833
  %78 = load double, ptr %76, align 8, !dbg !1833
  %79 = fptrunc double %78 to float, !dbg !1833
  %80 = load i32, ptr %11, align 4, !dbg !1833
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1833
  store float %79, ptr %81, align 8, !dbg !1833
  br label %99, !dbg !1833

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1833
  %84 = ptrtoint ptr %83 to i32, !dbg !1833
  %85 = add i32 %84, 7, !dbg !1833
  %86 = and i32 %85, -8, !dbg !1833
  %87 = inttoptr i32 %86 to ptr, !dbg !1833
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1833
  store ptr %88, ptr %7, align 4, !dbg !1833
  %89 = load double, ptr %87, align 8, !dbg !1833
  %90 = load i32, ptr %11, align 4, !dbg !1833
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1833
  store double %89, ptr %91, align 8, !dbg !1833
  br label %99, !dbg !1833

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1833
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1833
  store ptr %94, ptr %7, align 4, !dbg !1833
  %95 = load ptr, ptr %93, align 4, !dbg !1833
  %96 = load i32, ptr %11, align 4, !dbg !1833
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1833
  store ptr %95, ptr %97, align 8, !dbg !1833
  br label %99, !dbg !1833

98:                                               ; preds = %25
  br label %99, !dbg !1833

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1830

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1835
  %102 = add nsw i32 %101, 1, !dbg !1835
  store i32 %102, ptr %11, align 4, !dbg !1835
  br label %21, !dbg !1835, !llvm.loop !1836

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1837, metadata !DIExpression()), !dbg !1820
  %104 = load ptr, ptr %6, align 4, !dbg !1820
  %105 = load ptr, ptr %104, align 4, !dbg !1820
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 54, !dbg !1820
  %107 = load ptr, ptr %106, align 4, !dbg !1820
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1820
  %109 = load ptr, ptr %4, align 4, !dbg !1820
  %110 = load ptr, ptr %5, align 4, !dbg !1820
  %111 = load ptr, ptr %6, align 4, !dbg !1820
  %112 = call arm_aapcs_vfpcc i64 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1820
  store i64 %112, ptr %12, align 8, !dbg !1820
  call void @llvm.va_end(ptr %7), !dbg !1820
  %113 = load i64, ptr %12, align 8, !dbg !1820
  ret i64 %113, !dbg !1820
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1838 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1839, metadata !DIExpression()), !dbg !1840
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1841, metadata !DIExpression()), !dbg !1840
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1842, metadata !DIExpression()), !dbg !1840
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1843, metadata !DIExpression()), !dbg !1840
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1844, metadata !DIExpression()), !dbg !1840
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1845, metadata !DIExpression()), !dbg !1840
  %13 = load ptr, ptr %8, align 4, !dbg !1840
  %14 = load ptr, ptr %13, align 4, !dbg !1840
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1840
  %16 = load ptr, ptr %15, align 4, !dbg !1840
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1840
  %18 = load ptr, ptr %6, align 4, !dbg !1840
  %19 = load ptr, ptr %8, align 4, !dbg !1840
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1840
  store i32 %20, ptr %10, align 4, !dbg !1840
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1846, metadata !DIExpression()), !dbg !1840
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1847, metadata !DIExpression()), !dbg !1849
  store i32 0, ptr %12, align 4, !dbg !1849
  br label %21, !dbg !1849

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1849
  %23 = load i32, ptr %10, align 4, !dbg !1849
  %24 = icmp slt i32 %22, %23, !dbg !1849
  br i1 %24, label %25, label %103, !dbg !1849

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1850
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1850
  %28 = load i8, ptr %27, align 1, !dbg !1850
  %29 = sext i8 %28 to i32, !dbg !1850
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1850

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1853
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1853
  store ptr %32, ptr %5, align 4, !dbg !1853
  %33 = load i32, ptr %31, align 4, !dbg !1853
  %34 = trunc i32 %33 to i8, !dbg !1853
  %35 = load i32, ptr %12, align 4, !dbg !1853
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1853
  store i8 %34, ptr %36, align 8, !dbg !1853
  br label %99, !dbg !1853

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1853
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1853
  store ptr %39, ptr %5, align 4, !dbg !1853
  %40 = load i32, ptr %38, align 4, !dbg !1853
  %41 = trunc i32 %40 to i8, !dbg !1853
  %42 = load i32, ptr %12, align 4, !dbg !1853
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1853
  store i8 %41, ptr %43, align 8, !dbg !1853
  br label %99, !dbg !1853

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1853
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1853
  store ptr %46, ptr %5, align 4, !dbg !1853
  %47 = load i32, ptr %45, align 4, !dbg !1853
  %48 = trunc i32 %47 to i16, !dbg !1853
  %49 = load i32, ptr %12, align 4, !dbg !1853
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1853
  store i16 %48, ptr %50, align 8, !dbg !1853
  br label %99, !dbg !1853

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1853
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1853
  store ptr %53, ptr %5, align 4, !dbg !1853
  %54 = load i32, ptr %52, align 4, !dbg !1853
  %55 = trunc i32 %54 to i16, !dbg !1853
  %56 = load i32, ptr %12, align 4, !dbg !1853
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1853
  store i16 %55, ptr %57, align 8, !dbg !1853
  br label %99, !dbg !1853

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1853
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1853
  store ptr %60, ptr %5, align 4, !dbg !1853
  %61 = load i32, ptr %59, align 4, !dbg !1853
  %62 = load i32, ptr %12, align 4, !dbg !1853
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1853
  store i32 %61, ptr %63, align 8, !dbg !1853
  br label %99, !dbg !1853

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1853
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1853
  store ptr %66, ptr %5, align 4, !dbg !1853
  %67 = load i32, ptr %65, align 4, !dbg !1853
  %68 = sext i32 %67 to i64, !dbg !1853
  %69 = load i32, ptr %12, align 4, !dbg !1853
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1853
  store i64 %68, ptr %70, align 8, !dbg !1853
  br label %99, !dbg !1853

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1853
  %73 = ptrtoint ptr %72 to i32, !dbg !1853
  %74 = add i32 %73, 7, !dbg !1853
  %75 = and i32 %74, -8, !dbg !1853
  %76 = inttoptr i32 %75 to ptr, !dbg !1853
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1853
  store ptr %77, ptr %5, align 4, !dbg !1853
  %78 = load double, ptr %76, align 8, !dbg !1853
  %79 = fptrunc double %78 to float, !dbg !1853
  %80 = load i32, ptr %12, align 4, !dbg !1853
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1853
  store float %79, ptr %81, align 8, !dbg !1853
  br label %99, !dbg !1853

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1853
  %84 = ptrtoint ptr %83 to i32, !dbg !1853
  %85 = add i32 %84, 7, !dbg !1853
  %86 = and i32 %85, -8, !dbg !1853
  %87 = inttoptr i32 %86 to ptr, !dbg !1853
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1853
  store ptr %88, ptr %5, align 4, !dbg !1853
  %89 = load double, ptr %87, align 8, !dbg !1853
  %90 = load i32, ptr %12, align 4, !dbg !1853
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1853
  store double %89, ptr %91, align 8, !dbg !1853
  br label %99, !dbg !1853

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1853
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1853
  store ptr %94, ptr %5, align 4, !dbg !1853
  %95 = load ptr, ptr %93, align 4, !dbg !1853
  %96 = load i32, ptr %12, align 4, !dbg !1853
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1853
  store ptr %95, ptr %97, align 8, !dbg !1853
  br label %99, !dbg !1853

98:                                               ; preds = %25
  br label %99, !dbg !1853

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1850

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1855
  %102 = add nsw i32 %101, 1, !dbg !1855
  store i32 %102, ptr %12, align 4, !dbg !1855
  br label %21, !dbg !1855, !llvm.loop !1856

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1840
  %105 = load ptr, ptr %104, align 4, !dbg !1840
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 54, !dbg !1840
  %107 = load ptr, ptr %106, align 4, !dbg !1840
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1840
  %109 = load ptr, ptr %6, align 4, !dbg !1840
  %110 = load ptr, ptr %7, align 4, !dbg !1840
  %111 = load ptr, ptr %8, align 4, !dbg !1840
  %112 = call arm_aapcs_vfpcc i64 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1840
  ret i64 %112, !dbg !1840
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1857 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i64, align 8
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1858, metadata !DIExpression()), !dbg !1859
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1860, metadata !DIExpression()), !dbg !1859
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1861, metadata !DIExpression()), !dbg !1859
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1862, metadata !DIExpression()), !dbg !1859
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1863, metadata !DIExpression()), !dbg !1859
  call void @llvm.va_start(ptr %9), !dbg !1859
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1864, metadata !DIExpression()), !dbg !1859
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1865, metadata !DIExpression()), !dbg !1859
  %15 = load ptr, ptr %8, align 4, !dbg !1859
  %16 = load ptr, ptr %15, align 4, !dbg !1859
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1859
  %18 = load ptr, ptr %17, align 4, !dbg !1859
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1859
  %20 = load ptr, ptr %5, align 4, !dbg !1859
  %21 = load ptr, ptr %8, align 4, !dbg !1859
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1859
  store i32 %22, ptr %11, align 4, !dbg !1859
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1866, metadata !DIExpression()), !dbg !1859
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1867, metadata !DIExpression()), !dbg !1869
  store i32 0, ptr %13, align 4, !dbg !1869
  br label %23, !dbg !1869

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1869
  %25 = load i32, ptr %11, align 4, !dbg !1869
  %26 = icmp slt i32 %24, %25, !dbg !1869
  br i1 %26, label %27, label %105, !dbg !1869

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1870
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1870
  %30 = load i8, ptr %29, align 1, !dbg !1870
  %31 = sext i8 %30 to i32, !dbg !1870
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1870

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1873
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1873
  store ptr %34, ptr %9, align 4, !dbg !1873
  %35 = load i32, ptr %33, align 4, !dbg !1873
  %36 = trunc i32 %35 to i8, !dbg !1873
  %37 = load i32, ptr %13, align 4, !dbg !1873
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1873
  store i8 %36, ptr %38, align 8, !dbg !1873
  br label %101, !dbg !1873

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1873
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1873
  store ptr %41, ptr %9, align 4, !dbg !1873
  %42 = load i32, ptr %40, align 4, !dbg !1873
  %43 = trunc i32 %42 to i8, !dbg !1873
  %44 = load i32, ptr %13, align 4, !dbg !1873
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1873
  store i8 %43, ptr %45, align 8, !dbg !1873
  br label %101, !dbg !1873

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1873
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1873
  store ptr %48, ptr %9, align 4, !dbg !1873
  %49 = load i32, ptr %47, align 4, !dbg !1873
  %50 = trunc i32 %49 to i16, !dbg !1873
  %51 = load i32, ptr %13, align 4, !dbg !1873
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1873
  store i16 %50, ptr %52, align 8, !dbg !1873
  br label %101, !dbg !1873

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1873
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1873
  store ptr %55, ptr %9, align 4, !dbg !1873
  %56 = load i32, ptr %54, align 4, !dbg !1873
  %57 = trunc i32 %56 to i16, !dbg !1873
  %58 = load i32, ptr %13, align 4, !dbg !1873
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1873
  store i16 %57, ptr %59, align 8, !dbg !1873
  br label %101, !dbg !1873

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1873
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1873
  store ptr %62, ptr %9, align 4, !dbg !1873
  %63 = load i32, ptr %61, align 4, !dbg !1873
  %64 = load i32, ptr %13, align 4, !dbg !1873
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1873
  store i32 %63, ptr %65, align 8, !dbg !1873
  br label %101, !dbg !1873

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1873
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1873
  store ptr %68, ptr %9, align 4, !dbg !1873
  %69 = load i32, ptr %67, align 4, !dbg !1873
  %70 = sext i32 %69 to i64, !dbg !1873
  %71 = load i32, ptr %13, align 4, !dbg !1873
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1873
  store i64 %70, ptr %72, align 8, !dbg !1873
  br label %101, !dbg !1873

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1873
  %75 = ptrtoint ptr %74 to i32, !dbg !1873
  %76 = add i32 %75, 7, !dbg !1873
  %77 = and i32 %76, -8, !dbg !1873
  %78 = inttoptr i32 %77 to ptr, !dbg !1873
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1873
  store ptr %79, ptr %9, align 4, !dbg !1873
  %80 = load double, ptr %78, align 8, !dbg !1873
  %81 = fptrunc double %80 to float, !dbg !1873
  %82 = load i32, ptr %13, align 4, !dbg !1873
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1873
  store float %81, ptr %83, align 8, !dbg !1873
  br label %101, !dbg !1873

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1873
  %86 = ptrtoint ptr %85 to i32, !dbg !1873
  %87 = add i32 %86, 7, !dbg !1873
  %88 = and i32 %87, -8, !dbg !1873
  %89 = inttoptr i32 %88 to ptr, !dbg !1873
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1873
  store ptr %90, ptr %9, align 4, !dbg !1873
  %91 = load double, ptr %89, align 8, !dbg !1873
  %92 = load i32, ptr %13, align 4, !dbg !1873
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1873
  store double %91, ptr %93, align 8, !dbg !1873
  br label %101, !dbg !1873

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1873
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1873
  store ptr %96, ptr %9, align 4, !dbg !1873
  %97 = load ptr, ptr %95, align 4, !dbg !1873
  %98 = load i32, ptr %13, align 4, !dbg !1873
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1873
  store ptr %97, ptr %99, align 8, !dbg !1873
  br label %101, !dbg !1873

100:                                              ; preds = %27
  br label %101, !dbg !1873

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1870

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1875
  %104 = add nsw i32 %103, 1, !dbg !1875
  store i32 %104, ptr %13, align 4, !dbg !1875
  br label %23, !dbg !1875, !llvm.loop !1876

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1877, metadata !DIExpression()), !dbg !1859
  %106 = load ptr, ptr %8, align 4, !dbg !1859
  %107 = load ptr, ptr %106, align 4, !dbg !1859
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 84, !dbg !1859
  %109 = load ptr, ptr %108, align 4, !dbg !1859
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1859
  %111 = load ptr, ptr %5, align 4, !dbg !1859
  %112 = load ptr, ptr %6, align 4, !dbg !1859
  %113 = load ptr, ptr %7, align 4, !dbg !1859
  %114 = load ptr, ptr %8, align 4, !dbg !1859
  %115 = call arm_aapcs_vfpcc i64 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1859
  store i64 %115, ptr %14, align 8, !dbg !1859
  call void @llvm.va_end(ptr %9), !dbg !1859
  %116 = load i64, ptr %14, align 8, !dbg !1859
  ret i64 %116, !dbg !1859
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1878 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1879, metadata !DIExpression()), !dbg !1880
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1881, metadata !DIExpression()), !dbg !1880
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1882, metadata !DIExpression()), !dbg !1880
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1883, metadata !DIExpression()), !dbg !1880
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1884, metadata !DIExpression()), !dbg !1880
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1885, metadata !DIExpression()), !dbg !1880
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1886, metadata !DIExpression()), !dbg !1880
  %15 = load ptr, ptr %10, align 4, !dbg !1880
  %16 = load ptr, ptr %15, align 4, !dbg !1880
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1880
  %18 = load ptr, ptr %17, align 4, !dbg !1880
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1880
  %20 = load ptr, ptr %7, align 4, !dbg !1880
  %21 = load ptr, ptr %10, align 4, !dbg !1880
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1880
  store i32 %22, ptr %12, align 4, !dbg !1880
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1887, metadata !DIExpression()), !dbg !1880
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1888, metadata !DIExpression()), !dbg !1890
  store i32 0, ptr %14, align 4, !dbg !1890
  br label %23, !dbg !1890

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !1890
  %25 = load i32, ptr %12, align 4, !dbg !1890
  %26 = icmp slt i32 %24, %25, !dbg !1890
  br i1 %26, label %27, label %105, !dbg !1890

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1891
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1891
  %30 = load i8, ptr %29, align 1, !dbg !1891
  %31 = sext i8 %30 to i32, !dbg !1891
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1891

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1894
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1894
  store ptr %34, ptr %6, align 4, !dbg !1894
  %35 = load i32, ptr %33, align 4, !dbg !1894
  %36 = trunc i32 %35 to i8, !dbg !1894
  %37 = load i32, ptr %14, align 4, !dbg !1894
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1894
  store i8 %36, ptr %38, align 8, !dbg !1894
  br label %101, !dbg !1894

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1894
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1894
  store ptr %41, ptr %6, align 4, !dbg !1894
  %42 = load i32, ptr %40, align 4, !dbg !1894
  %43 = trunc i32 %42 to i8, !dbg !1894
  %44 = load i32, ptr %14, align 4, !dbg !1894
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1894
  store i8 %43, ptr %45, align 8, !dbg !1894
  br label %101, !dbg !1894

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1894
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1894
  store ptr %48, ptr %6, align 4, !dbg !1894
  %49 = load i32, ptr %47, align 4, !dbg !1894
  %50 = trunc i32 %49 to i16, !dbg !1894
  %51 = load i32, ptr %14, align 4, !dbg !1894
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1894
  store i16 %50, ptr %52, align 8, !dbg !1894
  br label %101, !dbg !1894

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1894
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1894
  store ptr %55, ptr %6, align 4, !dbg !1894
  %56 = load i32, ptr %54, align 4, !dbg !1894
  %57 = trunc i32 %56 to i16, !dbg !1894
  %58 = load i32, ptr %14, align 4, !dbg !1894
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1894
  store i16 %57, ptr %59, align 8, !dbg !1894
  br label %101, !dbg !1894

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1894
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1894
  store ptr %62, ptr %6, align 4, !dbg !1894
  %63 = load i32, ptr %61, align 4, !dbg !1894
  %64 = load i32, ptr %14, align 4, !dbg !1894
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1894
  store i32 %63, ptr %65, align 8, !dbg !1894
  br label %101, !dbg !1894

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1894
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1894
  store ptr %68, ptr %6, align 4, !dbg !1894
  %69 = load i32, ptr %67, align 4, !dbg !1894
  %70 = sext i32 %69 to i64, !dbg !1894
  %71 = load i32, ptr %14, align 4, !dbg !1894
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1894
  store i64 %70, ptr %72, align 8, !dbg !1894
  br label %101, !dbg !1894

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1894
  %75 = ptrtoint ptr %74 to i32, !dbg !1894
  %76 = add i32 %75, 7, !dbg !1894
  %77 = and i32 %76, -8, !dbg !1894
  %78 = inttoptr i32 %77 to ptr, !dbg !1894
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1894
  store ptr %79, ptr %6, align 4, !dbg !1894
  %80 = load double, ptr %78, align 8, !dbg !1894
  %81 = fptrunc double %80 to float, !dbg !1894
  %82 = load i32, ptr %14, align 4, !dbg !1894
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !1894
  store float %81, ptr %83, align 8, !dbg !1894
  br label %101, !dbg !1894

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !1894
  %86 = ptrtoint ptr %85 to i32, !dbg !1894
  %87 = add i32 %86, 7, !dbg !1894
  %88 = and i32 %87, -8, !dbg !1894
  %89 = inttoptr i32 %88 to ptr, !dbg !1894
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1894
  store ptr %90, ptr %6, align 4, !dbg !1894
  %91 = load double, ptr %89, align 8, !dbg !1894
  %92 = load i32, ptr %14, align 4, !dbg !1894
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !1894
  store double %91, ptr %93, align 8, !dbg !1894
  br label %101, !dbg !1894

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !1894
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1894
  store ptr %96, ptr %6, align 4, !dbg !1894
  %97 = load ptr, ptr %95, align 4, !dbg !1894
  %98 = load i32, ptr %14, align 4, !dbg !1894
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !1894
  store ptr %97, ptr %99, align 8, !dbg !1894
  br label %101, !dbg !1894

100:                                              ; preds = %27
  br label %101, !dbg !1894

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1891

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !1896
  %104 = add nsw i32 %103, 1, !dbg !1896
  store i32 %104, ptr %14, align 4, !dbg !1896
  br label %23, !dbg !1896, !llvm.loop !1897

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1880
  %107 = load ptr, ptr %106, align 4, !dbg !1880
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 84, !dbg !1880
  %109 = load ptr, ptr %108, align 4, !dbg !1880
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1880
  %111 = load ptr, ptr %7, align 4, !dbg !1880
  %112 = load ptr, ptr %8, align 4, !dbg !1880
  %113 = load ptr, ptr %9, align 4, !dbg !1880
  %114 = load ptr, ptr %10, align 4, !dbg !1880
  %115 = call arm_aapcs_vfpcc i64 %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1880
  ret i64 %115, !dbg !1880
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1898 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1899, metadata !DIExpression()), !dbg !1900
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1901, metadata !DIExpression()), !dbg !1900
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1902, metadata !DIExpression()), !dbg !1900
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1903, metadata !DIExpression()), !dbg !1900
  call void @llvm.va_start(ptr %7), !dbg !1900
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1904, metadata !DIExpression()), !dbg !1900
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1905, metadata !DIExpression()), !dbg !1900
  %13 = load ptr, ptr %6, align 4, !dbg !1900
  %14 = load ptr, ptr %13, align 4, !dbg !1900
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1900
  %16 = load ptr, ptr %15, align 4, !dbg !1900
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1900
  %18 = load ptr, ptr %4, align 4, !dbg !1900
  %19 = load ptr, ptr %6, align 4, !dbg !1900
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1900
  store i32 %20, ptr %9, align 4, !dbg !1900
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1906, metadata !DIExpression()), !dbg !1900
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1907, metadata !DIExpression()), !dbg !1909
  store i32 0, ptr %11, align 4, !dbg !1909
  br label %21, !dbg !1909

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1909
  %23 = load i32, ptr %9, align 4, !dbg !1909
  %24 = icmp slt i32 %22, %23, !dbg !1909
  br i1 %24, label %25, label %103, !dbg !1909

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1910
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1910
  %28 = load i8, ptr %27, align 1, !dbg !1910
  %29 = sext i8 %28 to i32, !dbg !1910
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1910

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1913
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1913
  store ptr %32, ptr %7, align 4, !dbg !1913
  %33 = load i32, ptr %31, align 4, !dbg !1913
  %34 = trunc i32 %33 to i8, !dbg !1913
  %35 = load i32, ptr %11, align 4, !dbg !1913
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1913
  store i8 %34, ptr %36, align 8, !dbg !1913
  br label %99, !dbg !1913

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1913
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1913
  store ptr %39, ptr %7, align 4, !dbg !1913
  %40 = load i32, ptr %38, align 4, !dbg !1913
  %41 = trunc i32 %40 to i8, !dbg !1913
  %42 = load i32, ptr %11, align 4, !dbg !1913
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1913
  store i8 %41, ptr %43, align 8, !dbg !1913
  br label %99, !dbg !1913

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1913
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1913
  store ptr %46, ptr %7, align 4, !dbg !1913
  %47 = load i32, ptr %45, align 4, !dbg !1913
  %48 = trunc i32 %47 to i16, !dbg !1913
  %49 = load i32, ptr %11, align 4, !dbg !1913
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1913
  store i16 %48, ptr %50, align 8, !dbg !1913
  br label %99, !dbg !1913

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1913
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1913
  store ptr %53, ptr %7, align 4, !dbg !1913
  %54 = load i32, ptr %52, align 4, !dbg !1913
  %55 = trunc i32 %54 to i16, !dbg !1913
  %56 = load i32, ptr %11, align 4, !dbg !1913
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1913
  store i16 %55, ptr %57, align 8, !dbg !1913
  br label %99, !dbg !1913

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1913
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1913
  store ptr %60, ptr %7, align 4, !dbg !1913
  %61 = load i32, ptr %59, align 4, !dbg !1913
  %62 = load i32, ptr %11, align 4, !dbg !1913
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1913
  store i32 %61, ptr %63, align 8, !dbg !1913
  br label %99, !dbg !1913

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1913
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1913
  store ptr %66, ptr %7, align 4, !dbg !1913
  %67 = load i32, ptr %65, align 4, !dbg !1913
  %68 = sext i32 %67 to i64, !dbg !1913
  %69 = load i32, ptr %11, align 4, !dbg !1913
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1913
  store i64 %68, ptr %70, align 8, !dbg !1913
  br label %99, !dbg !1913

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1913
  %73 = ptrtoint ptr %72 to i32, !dbg !1913
  %74 = add i32 %73, 7, !dbg !1913
  %75 = and i32 %74, -8, !dbg !1913
  %76 = inttoptr i32 %75 to ptr, !dbg !1913
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1913
  store ptr %77, ptr %7, align 4, !dbg !1913
  %78 = load double, ptr %76, align 8, !dbg !1913
  %79 = fptrunc double %78 to float, !dbg !1913
  %80 = load i32, ptr %11, align 4, !dbg !1913
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1913
  store float %79, ptr %81, align 8, !dbg !1913
  br label %99, !dbg !1913

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1913
  %84 = ptrtoint ptr %83 to i32, !dbg !1913
  %85 = add i32 %84, 7, !dbg !1913
  %86 = and i32 %85, -8, !dbg !1913
  %87 = inttoptr i32 %86 to ptr, !dbg !1913
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1913
  store ptr %88, ptr %7, align 4, !dbg !1913
  %89 = load double, ptr %87, align 8, !dbg !1913
  %90 = load i32, ptr %11, align 4, !dbg !1913
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1913
  store double %89, ptr %91, align 8, !dbg !1913
  br label %99, !dbg !1913

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1913
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1913
  store ptr %94, ptr %7, align 4, !dbg !1913
  %95 = load ptr, ptr %93, align 4, !dbg !1913
  %96 = load i32, ptr %11, align 4, !dbg !1913
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1913
  store ptr %95, ptr %97, align 8, !dbg !1913
  br label %99, !dbg !1913

98:                                               ; preds = %25
  br label %99, !dbg !1913

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1910

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1915
  %102 = add nsw i32 %101, 1, !dbg !1915
  store i32 %102, ptr %11, align 4, !dbg !1915
  br label %21, !dbg !1915, !llvm.loop !1916

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1917, metadata !DIExpression()), !dbg !1900
  %104 = load ptr, ptr %6, align 4, !dbg !1900
  %105 = load ptr, ptr %104, align 4, !dbg !1900
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 134, !dbg !1900
  %107 = load ptr, ptr %106, align 4, !dbg !1900
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1900
  %109 = load ptr, ptr %4, align 4, !dbg !1900
  %110 = load ptr, ptr %5, align 4, !dbg !1900
  %111 = load ptr, ptr %6, align 4, !dbg !1900
  %112 = call arm_aapcs_vfpcc i64 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1900
  store i64 %112, ptr %12, align 8, !dbg !1900
  call void @llvm.va_end(ptr %7), !dbg !1900
  %113 = load i64, ptr %12, align 8, !dbg !1900
  ret i64 %113, !dbg !1900
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1918 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1919, metadata !DIExpression()), !dbg !1920
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1921, metadata !DIExpression()), !dbg !1920
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1922, metadata !DIExpression()), !dbg !1920
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1923, metadata !DIExpression()), !dbg !1920
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1924, metadata !DIExpression()), !dbg !1920
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1925, metadata !DIExpression()), !dbg !1920
  %13 = load ptr, ptr %8, align 4, !dbg !1920
  %14 = load ptr, ptr %13, align 4, !dbg !1920
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1920
  %16 = load ptr, ptr %15, align 4, !dbg !1920
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1920
  %18 = load ptr, ptr %6, align 4, !dbg !1920
  %19 = load ptr, ptr %8, align 4, !dbg !1920
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1920
  store i32 %20, ptr %10, align 4, !dbg !1920
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1926, metadata !DIExpression()), !dbg !1920
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1927, metadata !DIExpression()), !dbg !1929
  store i32 0, ptr %12, align 4, !dbg !1929
  br label %21, !dbg !1929

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1929
  %23 = load i32, ptr %10, align 4, !dbg !1929
  %24 = icmp slt i32 %22, %23, !dbg !1929
  br i1 %24, label %25, label %103, !dbg !1929

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1930
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1930
  %28 = load i8, ptr %27, align 1, !dbg !1930
  %29 = sext i8 %28 to i32, !dbg !1930
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1930

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1933
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1933
  store ptr %32, ptr %5, align 4, !dbg !1933
  %33 = load i32, ptr %31, align 4, !dbg !1933
  %34 = trunc i32 %33 to i8, !dbg !1933
  %35 = load i32, ptr %12, align 4, !dbg !1933
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1933
  store i8 %34, ptr %36, align 8, !dbg !1933
  br label %99, !dbg !1933

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1933
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1933
  store ptr %39, ptr %5, align 4, !dbg !1933
  %40 = load i32, ptr %38, align 4, !dbg !1933
  %41 = trunc i32 %40 to i8, !dbg !1933
  %42 = load i32, ptr %12, align 4, !dbg !1933
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1933
  store i8 %41, ptr %43, align 8, !dbg !1933
  br label %99, !dbg !1933

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1933
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1933
  store ptr %46, ptr %5, align 4, !dbg !1933
  %47 = load i32, ptr %45, align 4, !dbg !1933
  %48 = trunc i32 %47 to i16, !dbg !1933
  %49 = load i32, ptr %12, align 4, !dbg !1933
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1933
  store i16 %48, ptr %50, align 8, !dbg !1933
  br label %99, !dbg !1933

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1933
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1933
  store ptr %53, ptr %5, align 4, !dbg !1933
  %54 = load i32, ptr %52, align 4, !dbg !1933
  %55 = trunc i32 %54 to i16, !dbg !1933
  %56 = load i32, ptr %12, align 4, !dbg !1933
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1933
  store i16 %55, ptr %57, align 8, !dbg !1933
  br label %99, !dbg !1933

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1933
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1933
  store ptr %60, ptr %5, align 4, !dbg !1933
  %61 = load i32, ptr %59, align 4, !dbg !1933
  %62 = load i32, ptr %12, align 4, !dbg !1933
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1933
  store i32 %61, ptr %63, align 8, !dbg !1933
  br label %99, !dbg !1933

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1933
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1933
  store ptr %66, ptr %5, align 4, !dbg !1933
  %67 = load i32, ptr %65, align 4, !dbg !1933
  %68 = sext i32 %67 to i64, !dbg !1933
  %69 = load i32, ptr %12, align 4, !dbg !1933
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1933
  store i64 %68, ptr %70, align 8, !dbg !1933
  br label %99, !dbg !1933

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1933
  %73 = ptrtoint ptr %72 to i32, !dbg !1933
  %74 = add i32 %73, 7, !dbg !1933
  %75 = and i32 %74, -8, !dbg !1933
  %76 = inttoptr i32 %75 to ptr, !dbg !1933
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1933
  store ptr %77, ptr %5, align 4, !dbg !1933
  %78 = load double, ptr %76, align 8, !dbg !1933
  %79 = fptrunc double %78 to float, !dbg !1933
  %80 = load i32, ptr %12, align 4, !dbg !1933
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1933
  store float %79, ptr %81, align 8, !dbg !1933
  br label %99, !dbg !1933

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1933
  %84 = ptrtoint ptr %83 to i32, !dbg !1933
  %85 = add i32 %84, 7, !dbg !1933
  %86 = and i32 %85, -8, !dbg !1933
  %87 = inttoptr i32 %86 to ptr, !dbg !1933
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1933
  store ptr %88, ptr %5, align 4, !dbg !1933
  %89 = load double, ptr %87, align 8, !dbg !1933
  %90 = load i32, ptr %12, align 4, !dbg !1933
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1933
  store double %89, ptr %91, align 8, !dbg !1933
  br label %99, !dbg !1933

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1933
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1933
  store ptr %94, ptr %5, align 4, !dbg !1933
  %95 = load ptr, ptr %93, align 4, !dbg !1933
  %96 = load i32, ptr %12, align 4, !dbg !1933
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1933
  store ptr %95, ptr %97, align 8, !dbg !1933
  br label %99, !dbg !1933

98:                                               ; preds = %25
  br label %99, !dbg !1933

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1930

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1935
  %102 = add nsw i32 %101, 1, !dbg !1935
  store i32 %102, ptr %12, align 4, !dbg !1935
  br label %21, !dbg !1935, !llvm.loop !1936

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1920
  %105 = load ptr, ptr %104, align 4, !dbg !1920
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 134, !dbg !1920
  %107 = load ptr, ptr %106, align 4, !dbg !1920
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1920
  %109 = load ptr, ptr %6, align 4, !dbg !1920
  %110 = load ptr, ptr %7, align 4, !dbg !1920
  %111 = load ptr, ptr %8, align 4, !dbg !1920
  %112 = call arm_aapcs_vfpcc i64 %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1920
  ret i64 %112, !dbg !1920
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1937 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1938, metadata !DIExpression()), !dbg !1939
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1940, metadata !DIExpression()), !dbg !1939
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1941, metadata !DIExpression()), !dbg !1939
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1942, metadata !DIExpression()), !dbg !1939
  call void @llvm.va_start(ptr %7), !dbg !1939
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1943, metadata !DIExpression()), !dbg !1939
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1944, metadata !DIExpression()), !dbg !1939
  %13 = load ptr, ptr %6, align 4, !dbg !1939
  %14 = load ptr, ptr %13, align 4, !dbg !1939
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1939
  %16 = load ptr, ptr %15, align 4, !dbg !1939
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1939
  %18 = load ptr, ptr %4, align 4, !dbg !1939
  %19 = load ptr, ptr %6, align 4, !dbg !1939
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1939
  store i32 %20, ptr %9, align 4, !dbg !1939
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1945, metadata !DIExpression()), !dbg !1939
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1946, metadata !DIExpression()), !dbg !1948
  store i32 0, ptr %11, align 4, !dbg !1948
  br label %21, !dbg !1948

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !1948
  %23 = load i32, ptr %9, align 4, !dbg !1948
  %24 = icmp slt i32 %22, %23, !dbg !1948
  br i1 %24, label %25, label %103, !dbg !1948

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1949
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1949
  %28 = load i8, ptr %27, align 1, !dbg !1949
  %29 = sext i8 %28 to i32, !dbg !1949
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1949

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1952
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1952
  store ptr %32, ptr %7, align 4, !dbg !1952
  %33 = load i32, ptr %31, align 4, !dbg !1952
  %34 = trunc i32 %33 to i8, !dbg !1952
  %35 = load i32, ptr %11, align 4, !dbg !1952
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1952
  store i8 %34, ptr %36, align 8, !dbg !1952
  br label %99, !dbg !1952

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1952
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1952
  store ptr %39, ptr %7, align 4, !dbg !1952
  %40 = load i32, ptr %38, align 4, !dbg !1952
  %41 = trunc i32 %40 to i8, !dbg !1952
  %42 = load i32, ptr %11, align 4, !dbg !1952
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1952
  store i8 %41, ptr %43, align 8, !dbg !1952
  br label %99, !dbg !1952

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1952
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1952
  store ptr %46, ptr %7, align 4, !dbg !1952
  %47 = load i32, ptr %45, align 4, !dbg !1952
  %48 = trunc i32 %47 to i16, !dbg !1952
  %49 = load i32, ptr %11, align 4, !dbg !1952
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1952
  store i16 %48, ptr %50, align 8, !dbg !1952
  br label %99, !dbg !1952

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1952
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1952
  store ptr %53, ptr %7, align 4, !dbg !1952
  %54 = load i32, ptr %52, align 4, !dbg !1952
  %55 = trunc i32 %54 to i16, !dbg !1952
  %56 = load i32, ptr %11, align 4, !dbg !1952
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1952
  store i16 %55, ptr %57, align 8, !dbg !1952
  br label %99, !dbg !1952

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1952
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1952
  store ptr %60, ptr %7, align 4, !dbg !1952
  %61 = load i32, ptr %59, align 4, !dbg !1952
  %62 = load i32, ptr %11, align 4, !dbg !1952
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1952
  store i32 %61, ptr %63, align 8, !dbg !1952
  br label %99, !dbg !1952

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1952
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1952
  store ptr %66, ptr %7, align 4, !dbg !1952
  %67 = load i32, ptr %65, align 4, !dbg !1952
  %68 = sext i32 %67 to i64, !dbg !1952
  %69 = load i32, ptr %11, align 4, !dbg !1952
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1952
  store i64 %68, ptr %70, align 8, !dbg !1952
  br label %99, !dbg !1952

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1952
  %73 = ptrtoint ptr %72 to i32, !dbg !1952
  %74 = add i32 %73, 7, !dbg !1952
  %75 = and i32 %74, -8, !dbg !1952
  %76 = inttoptr i32 %75 to ptr, !dbg !1952
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1952
  store ptr %77, ptr %7, align 4, !dbg !1952
  %78 = load double, ptr %76, align 8, !dbg !1952
  %79 = fptrunc double %78 to float, !dbg !1952
  %80 = load i32, ptr %11, align 4, !dbg !1952
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !1952
  store float %79, ptr %81, align 8, !dbg !1952
  br label %99, !dbg !1952

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !1952
  %84 = ptrtoint ptr %83 to i32, !dbg !1952
  %85 = add i32 %84, 7, !dbg !1952
  %86 = and i32 %85, -8, !dbg !1952
  %87 = inttoptr i32 %86 to ptr, !dbg !1952
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1952
  store ptr %88, ptr %7, align 4, !dbg !1952
  %89 = load double, ptr %87, align 8, !dbg !1952
  %90 = load i32, ptr %11, align 4, !dbg !1952
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !1952
  store double %89, ptr %91, align 8, !dbg !1952
  br label %99, !dbg !1952

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !1952
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1952
  store ptr %94, ptr %7, align 4, !dbg !1952
  %95 = load ptr, ptr %93, align 4, !dbg !1952
  %96 = load i32, ptr %11, align 4, !dbg !1952
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !1952
  store ptr %95, ptr %97, align 8, !dbg !1952
  br label %99, !dbg !1952

98:                                               ; preds = %25
  br label %99, !dbg !1952

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1949

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !1954
  %102 = add nsw i32 %101, 1, !dbg !1954
  store i32 %102, ptr %11, align 4, !dbg !1954
  br label %21, !dbg !1954, !llvm.loop !1955

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1956, metadata !DIExpression()), !dbg !1939
  %104 = load ptr, ptr %6, align 4, !dbg !1939
  %105 = load ptr, ptr %104, align 4, !dbg !1939
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 57, !dbg !1939
  %107 = load ptr, ptr %106, align 4, !dbg !1939
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1939
  %109 = load ptr, ptr %4, align 4, !dbg !1939
  %110 = load ptr, ptr %5, align 4, !dbg !1939
  %111 = load ptr, ptr %6, align 4, !dbg !1939
  %112 = call arm_aapcs_vfpcc float %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1939
  store float %112, ptr %12, align 4, !dbg !1939
  call void @llvm.va_end(ptr %7), !dbg !1939
  %113 = load float, ptr %12, align 4, !dbg !1939
  ret float %113, !dbg !1939
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1957 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1958, metadata !DIExpression()), !dbg !1959
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1960, metadata !DIExpression()), !dbg !1959
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1961, metadata !DIExpression()), !dbg !1959
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1962, metadata !DIExpression()), !dbg !1959
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1963, metadata !DIExpression()), !dbg !1959
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1964, metadata !DIExpression()), !dbg !1959
  %13 = load ptr, ptr %8, align 4, !dbg !1959
  %14 = load ptr, ptr %13, align 4, !dbg !1959
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1959
  %16 = load ptr, ptr %15, align 4, !dbg !1959
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1959
  %18 = load ptr, ptr %6, align 4, !dbg !1959
  %19 = load ptr, ptr %8, align 4, !dbg !1959
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1959
  store i32 %20, ptr %10, align 4, !dbg !1959
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1965, metadata !DIExpression()), !dbg !1959
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1966, metadata !DIExpression()), !dbg !1968
  store i32 0, ptr %12, align 4, !dbg !1968
  br label %21, !dbg !1968

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !1968
  %23 = load i32, ptr %10, align 4, !dbg !1968
  %24 = icmp slt i32 %22, %23, !dbg !1968
  br i1 %24, label %25, label %103, !dbg !1968

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1969
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1969
  %28 = load i8, ptr %27, align 1, !dbg !1969
  %29 = sext i8 %28 to i32, !dbg !1969
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !1969

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1972
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1972
  store ptr %32, ptr %5, align 4, !dbg !1972
  %33 = load i32, ptr %31, align 4, !dbg !1972
  %34 = trunc i32 %33 to i8, !dbg !1972
  %35 = load i32, ptr %12, align 4, !dbg !1972
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1972
  store i8 %34, ptr %36, align 8, !dbg !1972
  br label %99, !dbg !1972

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1972
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1972
  store ptr %39, ptr %5, align 4, !dbg !1972
  %40 = load i32, ptr %38, align 4, !dbg !1972
  %41 = trunc i32 %40 to i8, !dbg !1972
  %42 = load i32, ptr %12, align 4, !dbg !1972
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1972
  store i8 %41, ptr %43, align 8, !dbg !1972
  br label %99, !dbg !1972

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1972
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1972
  store ptr %46, ptr %5, align 4, !dbg !1972
  %47 = load i32, ptr %45, align 4, !dbg !1972
  %48 = trunc i32 %47 to i16, !dbg !1972
  %49 = load i32, ptr %12, align 4, !dbg !1972
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1972
  store i16 %48, ptr %50, align 8, !dbg !1972
  br label %99, !dbg !1972

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1972
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1972
  store ptr %53, ptr %5, align 4, !dbg !1972
  %54 = load i32, ptr %52, align 4, !dbg !1972
  %55 = trunc i32 %54 to i16, !dbg !1972
  %56 = load i32, ptr %12, align 4, !dbg !1972
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1972
  store i16 %55, ptr %57, align 8, !dbg !1972
  br label %99, !dbg !1972

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1972
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1972
  store ptr %60, ptr %5, align 4, !dbg !1972
  %61 = load i32, ptr %59, align 4, !dbg !1972
  %62 = load i32, ptr %12, align 4, !dbg !1972
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1972
  store i32 %61, ptr %63, align 8, !dbg !1972
  br label %99, !dbg !1972

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1972
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1972
  store ptr %66, ptr %5, align 4, !dbg !1972
  %67 = load i32, ptr %65, align 4, !dbg !1972
  %68 = sext i32 %67 to i64, !dbg !1972
  %69 = load i32, ptr %12, align 4, !dbg !1972
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1972
  store i64 %68, ptr %70, align 8, !dbg !1972
  br label %99, !dbg !1972

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1972
  %73 = ptrtoint ptr %72 to i32, !dbg !1972
  %74 = add i32 %73, 7, !dbg !1972
  %75 = and i32 %74, -8, !dbg !1972
  %76 = inttoptr i32 %75 to ptr, !dbg !1972
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !1972
  store ptr %77, ptr %5, align 4, !dbg !1972
  %78 = load double, ptr %76, align 8, !dbg !1972
  %79 = fptrunc double %78 to float, !dbg !1972
  %80 = load i32, ptr %12, align 4, !dbg !1972
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !1972
  store float %79, ptr %81, align 8, !dbg !1972
  br label %99, !dbg !1972

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !1972
  %84 = ptrtoint ptr %83 to i32, !dbg !1972
  %85 = add i32 %84, 7, !dbg !1972
  %86 = and i32 %85, -8, !dbg !1972
  %87 = inttoptr i32 %86 to ptr, !dbg !1972
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !1972
  store ptr %88, ptr %5, align 4, !dbg !1972
  %89 = load double, ptr %87, align 8, !dbg !1972
  %90 = load i32, ptr %12, align 4, !dbg !1972
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !1972
  store double %89, ptr %91, align 8, !dbg !1972
  br label %99, !dbg !1972

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !1972
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !1972
  store ptr %94, ptr %5, align 4, !dbg !1972
  %95 = load ptr, ptr %93, align 4, !dbg !1972
  %96 = load i32, ptr %12, align 4, !dbg !1972
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !1972
  store ptr %95, ptr %97, align 8, !dbg !1972
  br label %99, !dbg !1972

98:                                               ; preds = %25
  br label %99, !dbg !1972

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !1969

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !1974
  %102 = add nsw i32 %101, 1, !dbg !1974
  store i32 %102, ptr %12, align 4, !dbg !1974
  br label %21, !dbg !1974, !llvm.loop !1975

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !1959
  %105 = load ptr, ptr %104, align 4, !dbg !1959
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 57, !dbg !1959
  %107 = load ptr, ptr %106, align 4, !dbg !1959
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1959
  %109 = load ptr, ptr %6, align 4, !dbg !1959
  %110 = load ptr, ptr %7, align 4, !dbg !1959
  %111 = load ptr, ptr %8, align 4, !dbg !1959
  %112 = call arm_aapcs_vfpcc float %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !1959
  ret float %112, !dbg !1959
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1976 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca float, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1977, metadata !DIExpression()), !dbg !1978
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1979, metadata !DIExpression()), !dbg !1978
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1980, metadata !DIExpression()), !dbg !1978
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1981, metadata !DIExpression()), !dbg !1978
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1982, metadata !DIExpression()), !dbg !1978
  call void @llvm.va_start(ptr %9), !dbg !1978
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1983, metadata !DIExpression()), !dbg !1978
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1984, metadata !DIExpression()), !dbg !1978
  %15 = load ptr, ptr %8, align 4, !dbg !1978
  %16 = load ptr, ptr %15, align 4, !dbg !1978
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1978
  %18 = load ptr, ptr %17, align 4, !dbg !1978
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1978
  %20 = load ptr, ptr %5, align 4, !dbg !1978
  %21 = load ptr, ptr %8, align 4, !dbg !1978
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1978
  store i32 %22, ptr %11, align 4, !dbg !1978
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1985, metadata !DIExpression()), !dbg !1978
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1986, metadata !DIExpression()), !dbg !1988
  store i32 0, ptr %13, align 4, !dbg !1988
  br label %23, !dbg !1988

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !1988
  %25 = load i32, ptr %11, align 4, !dbg !1988
  %26 = icmp slt i32 %24, %25, !dbg !1988
  br i1 %26, label %27, label %105, !dbg !1988

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1989
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1989
  %30 = load i8, ptr %29, align 1, !dbg !1989
  %31 = sext i8 %30 to i32, !dbg !1989
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !1989

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1992
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1992
  store ptr %34, ptr %9, align 4, !dbg !1992
  %35 = load i32, ptr %33, align 4, !dbg !1992
  %36 = trunc i32 %35 to i8, !dbg !1992
  %37 = load i32, ptr %13, align 4, !dbg !1992
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1992
  store i8 %36, ptr %38, align 8, !dbg !1992
  br label %101, !dbg !1992

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1992
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1992
  store ptr %41, ptr %9, align 4, !dbg !1992
  %42 = load i32, ptr %40, align 4, !dbg !1992
  %43 = trunc i32 %42 to i8, !dbg !1992
  %44 = load i32, ptr %13, align 4, !dbg !1992
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1992
  store i8 %43, ptr %45, align 8, !dbg !1992
  br label %101, !dbg !1992

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1992
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1992
  store ptr %48, ptr %9, align 4, !dbg !1992
  %49 = load i32, ptr %47, align 4, !dbg !1992
  %50 = trunc i32 %49 to i16, !dbg !1992
  %51 = load i32, ptr %13, align 4, !dbg !1992
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1992
  store i16 %50, ptr %52, align 8, !dbg !1992
  br label %101, !dbg !1992

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1992
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1992
  store ptr %55, ptr %9, align 4, !dbg !1992
  %56 = load i32, ptr %54, align 4, !dbg !1992
  %57 = trunc i32 %56 to i16, !dbg !1992
  %58 = load i32, ptr %13, align 4, !dbg !1992
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1992
  store i16 %57, ptr %59, align 8, !dbg !1992
  br label %101, !dbg !1992

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1992
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1992
  store ptr %62, ptr %9, align 4, !dbg !1992
  %63 = load i32, ptr %61, align 4, !dbg !1992
  %64 = load i32, ptr %13, align 4, !dbg !1992
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1992
  store i32 %63, ptr %65, align 8, !dbg !1992
  br label %101, !dbg !1992

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1992
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1992
  store ptr %68, ptr %9, align 4, !dbg !1992
  %69 = load i32, ptr %67, align 4, !dbg !1992
  %70 = sext i32 %69 to i64, !dbg !1992
  %71 = load i32, ptr %13, align 4, !dbg !1992
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1992
  store i64 %70, ptr %72, align 8, !dbg !1992
  br label %101, !dbg !1992

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1992
  %75 = ptrtoint ptr %74 to i32, !dbg !1992
  %76 = add i32 %75, 7, !dbg !1992
  %77 = and i32 %76, -8, !dbg !1992
  %78 = inttoptr i32 %77 to ptr, !dbg !1992
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !1992
  store ptr %79, ptr %9, align 4, !dbg !1992
  %80 = load double, ptr %78, align 8, !dbg !1992
  %81 = fptrunc double %80 to float, !dbg !1992
  %82 = load i32, ptr %13, align 4, !dbg !1992
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !1992
  store float %81, ptr %83, align 8, !dbg !1992
  br label %101, !dbg !1992

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !1992
  %86 = ptrtoint ptr %85 to i32, !dbg !1992
  %87 = add i32 %86, 7, !dbg !1992
  %88 = and i32 %87, -8, !dbg !1992
  %89 = inttoptr i32 %88 to ptr, !dbg !1992
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !1992
  store ptr %90, ptr %9, align 4, !dbg !1992
  %91 = load double, ptr %89, align 8, !dbg !1992
  %92 = load i32, ptr %13, align 4, !dbg !1992
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !1992
  store double %91, ptr %93, align 8, !dbg !1992
  br label %101, !dbg !1992

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !1992
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !1992
  store ptr %96, ptr %9, align 4, !dbg !1992
  %97 = load ptr, ptr %95, align 4, !dbg !1992
  %98 = load i32, ptr %13, align 4, !dbg !1992
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !1992
  store ptr %97, ptr %99, align 8, !dbg !1992
  br label %101, !dbg !1992

100:                                              ; preds = %27
  br label %101, !dbg !1992

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !1989

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !1994
  %104 = add nsw i32 %103, 1, !dbg !1994
  store i32 %104, ptr %13, align 4, !dbg !1994
  br label %23, !dbg !1994, !llvm.loop !1995

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1996, metadata !DIExpression()), !dbg !1978
  %106 = load ptr, ptr %8, align 4, !dbg !1978
  %107 = load ptr, ptr %106, align 4, !dbg !1978
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 87, !dbg !1978
  %109 = load ptr, ptr %108, align 4, !dbg !1978
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1978
  %111 = load ptr, ptr %5, align 4, !dbg !1978
  %112 = load ptr, ptr %6, align 4, !dbg !1978
  %113 = load ptr, ptr %7, align 4, !dbg !1978
  %114 = load ptr, ptr %8, align 4, !dbg !1978
  %115 = call arm_aapcs_vfpcc float %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1978
  store float %115, ptr %14, align 4, !dbg !1978
  call void @llvm.va_end(ptr %9), !dbg !1978
  %116 = load float, ptr %14, align 4, !dbg !1978
  ret float %116, !dbg !1978
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1997 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1998, metadata !DIExpression()), !dbg !1999
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2000, metadata !DIExpression()), !dbg !1999
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2001, metadata !DIExpression()), !dbg !1999
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2002, metadata !DIExpression()), !dbg !1999
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2003, metadata !DIExpression()), !dbg !1999
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2004, metadata !DIExpression()), !dbg !1999
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2005, metadata !DIExpression()), !dbg !1999
  %15 = load ptr, ptr %10, align 4, !dbg !1999
  %16 = load ptr, ptr %15, align 4, !dbg !1999
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1999
  %18 = load ptr, ptr %17, align 4, !dbg !1999
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1999
  %20 = load ptr, ptr %7, align 4, !dbg !1999
  %21 = load ptr, ptr %10, align 4, !dbg !1999
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1999
  store i32 %22, ptr %12, align 4, !dbg !1999
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2006, metadata !DIExpression()), !dbg !1999
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2007, metadata !DIExpression()), !dbg !2009
  store i32 0, ptr %14, align 4, !dbg !2009
  br label %23, !dbg !2009

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !2009
  %25 = load i32, ptr %12, align 4, !dbg !2009
  %26 = icmp slt i32 %24, %25, !dbg !2009
  br i1 %26, label %27, label %105, !dbg !2009

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2010
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2010
  %30 = load i8, ptr %29, align 1, !dbg !2010
  %31 = sext i8 %30 to i32, !dbg !2010
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !2010

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2013
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2013
  store ptr %34, ptr %6, align 4, !dbg !2013
  %35 = load i32, ptr %33, align 4, !dbg !2013
  %36 = trunc i32 %35 to i8, !dbg !2013
  %37 = load i32, ptr %14, align 4, !dbg !2013
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2013
  store i8 %36, ptr %38, align 8, !dbg !2013
  br label %101, !dbg !2013

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2013
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2013
  store ptr %41, ptr %6, align 4, !dbg !2013
  %42 = load i32, ptr %40, align 4, !dbg !2013
  %43 = trunc i32 %42 to i8, !dbg !2013
  %44 = load i32, ptr %14, align 4, !dbg !2013
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2013
  store i8 %43, ptr %45, align 8, !dbg !2013
  br label %101, !dbg !2013

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2013
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2013
  store ptr %48, ptr %6, align 4, !dbg !2013
  %49 = load i32, ptr %47, align 4, !dbg !2013
  %50 = trunc i32 %49 to i16, !dbg !2013
  %51 = load i32, ptr %14, align 4, !dbg !2013
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2013
  store i16 %50, ptr %52, align 8, !dbg !2013
  br label %101, !dbg !2013

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2013
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2013
  store ptr %55, ptr %6, align 4, !dbg !2013
  %56 = load i32, ptr %54, align 4, !dbg !2013
  %57 = trunc i32 %56 to i16, !dbg !2013
  %58 = load i32, ptr %14, align 4, !dbg !2013
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2013
  store i16 %57, ptr %59, align 8, !dbg !2013
  br label %101, !dbg !2013

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2013
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2013
  store ptr %62, ptr %6, align 4, !dbg !2013
  %63 = load i32, ptr %61, align 4, !dbg !2013
  %64 = load i32, ptr %14, align 4, !dbg !2013
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2013
  store i32 %63, ptr %65, align 8, !dbg !2013
  br label %101, !dbg !2013

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2013
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2013
  store ptr %68, ptr %6, align 4, !dbg !2013
  %69 = load i32, ptr %67, align 4, !dbg !2013
  %70 = sext i32 %69 to i64, !dbg !2013
  %71 = load i32, ptr %14, align 4, !dbg !2013
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2013
  store i64 %70, ptr %72, align 8, !dbg !2013
  br label %101, !dbg !2013

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2013
  %75 = ptrtoint ptr %74 to i32, !dbg !2013
  %76 = add i32 %75, 7, !dbg !2013
  %77 = and i32 %76, -8, !dbg !2013
  %78 = inttoptr i32 %77 to ptr, !dbg !2013
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2013
  store ptr %79, ptr %6, align 4, !dbg !2013
  %80 = load double, ptr %78, align 8, !dbg !2013
  %81 = fptrunc double %80 to float, !dbg !2013
  %82 = load i32, ptr %14, align 4, !dbg !2013
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !2013
  store float %81, ptr %83, align 8, !dbg !2013
  br label %101, !dbg !2013

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !2013
  %86 = ptrtoint ptr %85 to i32, !dbg !2013
  %87 = add i32 %86, 7, !dbg !2013
  %88 = and i32 %87, -8, !dbg !2013
  %89 = inttoptr i32 %88 to ptr, !dbg !2013
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !2013
  store ptr %90, ptr %6, align 4, !dbg !2013
  %91 = load double, ptr %89, align 8, !dbg !2013
  %92 = load i32, ptr %14, align 4, !dbg !2013
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !2013
  store double %91, ptr %93, align 8, !dbg !2013
  br label %101, !dbg !2013

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !2013
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !2013
  store ptr %96, ptr %6, align 4, !dbg !2013
  %97 = load ptr, ptr %95, align 4, !dbg !2013
  %98 = load i32, ptr %14, align 4, !dbg !2013
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !2013
  store ptr %97, ptr %99, align 8, !dbg !2013
  br label %101, !dbg !2013

100:                                              ; preds = %27
  br label %101, !dbg !2013

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !2010

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !2015
  %104 = add nsw i32 %103, 1, !dbg !2015
  store i32 %104, ptr %14, align 4, !dbg !2015
  br label %23, !dbg !2015, !llvm.loop !2016

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !1999
  %107 = load ptr, ptr %106, align 4, !dbg !1999
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 87, !dbg !1999
  %109 = load ptr, ptr %108, align 4, !dbg !1999
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1999
  %111 = load ptr, ptr %7, align 4, !dbg !1999
  %112 = load ptr, ptr %8, align 4, !dbg !1999
  %113 = load ptr, ptr %9, align 4, !dbg !1999
  %114 = load ptr, ptr %10, align 4, !dbg !1999
  %115 = call arm_aapcs_vfpcc float %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1999
  ret float %115, !dbg !1999
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2017 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2018, metadata !DIExpression()), !dbg !2019
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2020, metadata !DIExpression()), !dbg !2019
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2021, metadata !DIExpression()), !dbg !2019
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2022, metadata !DIExpression()), !dbg !2019
  call void @llvm.va_start(ptr %7), !dbg !2019
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2023, metadata !DIExpression()), !dbg !2019
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2024, metadata !DIExpression()), !dbg !2019
  %13 = load ptr, ptr %6, align 4, !dbg !2019
  %14 = load ptr, ptr %13, align 4, !dbg !2019
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2019
  %16 = load ptr, ptr %15, align 4, !dbg !2019
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2019
  %18 = load ptr, ptr %4, align 4, !dbg !2019
  %19 = load ptr, ptr %6, align 4, !dbg !2019
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2019
  store i32 %20, ptr %9, align 4, !dbg !2019
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2025, metadata !DIExpression()), !dbg !2019
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2026, metadata !DIExpression()), !dbg !2028
  store i32 0, ptr %11, align 4, !dbg !2028
  br label %21, !dbg !2028

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !2028
  %23 = load i32, ptr %9, align 4, !dbg !2028
  %24 = icmp slt i32 %22, %23, !dbg !2028
  br i1 %24, label %25, label %103, !dbg !2028

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2029
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2029
  %28 = load i8, ptr %27, align 1, !dbg !2029
  %29 = sext i8 %28 to i32, !dbg !2029
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2029

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2032
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2032
  store ptr %32, ptr %7, align 4, !dbg !2032
  %33 = load i32, ptr %31, align 4, !dbg !2032
  %34 = trunc i32 %33 to i8, !dbg !2032
  %35 = load i32, ptr %11, align 4, !dbg !2032
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2032
  store i8 %34, ptr %36, align 8, !dbg !2032
  br label %99, !dbg !2032

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2032
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2032
  store ptr %39, ptr %7, align 4, !dbg !2032
  %40 = load i32, ptr %38, align 4, !dbg !2032
  %41 = trunc i32 %40 to i8, !dbg !2032
  %42 = load i32, ptr %11, align 4, !dbg !2032
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2032
  store i8 %41, ptr %43, align 8, !dbg !2032
  br label %99, !dbg !2032

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2032
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2032
  store ptr %46, ptr %7, align 4, !dbg !2032
  %47 = load i32, ptr %45, align 4, !dbg !2032
  %48 = trunc i32 %47 to i16, !dbg !2032
  %49 = load i32, ptr %11, align 4, !dbg !2032
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2032
  store i16 %48, ptr %50, align 8, !dbg !2032
  br label %99, !dbg !2032

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2032
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2032
  store ptr %53, ptr %7, align 4, !dbg !2032
  %54 = load i32, ptr %52, align 4, !dbg !2032
  %55 = trunc i32 %54 to i16, !dbg !2032
  %56 = load i32, ptr %11, align 4, !dbg !2032
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2032
  store i16 %55, ptr %57, align 8, !dbg !2032
  br label %99, !dbg !2032

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2032
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2032
  store ptr %60, ptr %7, align 4, !dbg !2032
  %61 = load i32, ptr %59, align 4, !dbg !2032
  %62 = load i32, ptr %11, align 4, !dbg !2032
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2032
  store i32 %61, ptr %63, align 8, !dbg !2032
  br label %99, !dbg !2032

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2032
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2032
  store ptr %66, ptr %7, align 4, !dbg !2032
  %67 = load i32, ptr %65, align 4, !dbg !2032
  %68 = sext i32 %67 to i64, !dbg !2032
  %69 = load i32, ptr %11, align 4, !dbg !2032
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2032
  store i64 %68, ptr %70, align 8, !dbg !2032
  br label %99, !dbg !2032

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2032
  %73 = ptrtoint ptr %72 to i32, !dbg !2032
  %74 = add i32 %73, 7, !dbg !2032
  %75 = and i32 %74, -8, !dbg !2032
  %76 = inttoptr i32 %75 to ptr, !dbg !2032
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2032
  store ptr %77, ptr %7, align 4, !dbg !2032
  %78 = load double, ptr %76, align 8, !dbg !2032
  %79 = fptrunc double %78 to float, !dbg !2032
  %80 = load i32, ptr %11, align 4, !dbg !2032
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !2032
  store float %79, ptr %81, align 8, !dbg !2032
  br label %99, !dbg !2032

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !2032
  %84 = ptrtoint ptr %83 to i32, !dbg !2032
  %85 = add i32 %84, 7, !dbg !2032
  %86 = and i32 %85, -8, !dbg !2032
  %87 = inttoptr i32 %86 to ptr, !dbg !2032
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2032
  store ptr %88, ptr %7, align 4, !dbg !2032
  %89 = load double, ptr %87, align 8, !dbg !2032
  %90 = load i32, ptr %11, align 4, !dbg !2032
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !2032
  store double %89, ptr %91, align 8, !dbg !2032
  br label %99, !dbg !2032

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !2032
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2032
  store ptr %94, ptr %7, align 4, !dbg !2032
  %95 = load ptr, ptr %93, align 4, !dbg !2032
  %96 = load i32, ptr %11, align 4, !dbg !2032
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !2032
  store ptr %95, ptr %97, align 8, !dbg !2032
  br label %99, !dbg !2032

98:                                               ; preds = %25
  br label %99, !dbg !2032

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2029

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !2034
  %102 = add nsw i32 %101, 1, !dbg !2034
  store i32 %102, ptr %11, align 4, !dbg !2034
  br label %21, !dbg !2034, !llvm.loop !2035

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2036, metadata !DIExpression()), !dbg !2019
  %104 = load ptr, ptr %6, align 4, !dbg !2019
  %105 = load ptr, ptr %104, align 4, !dbg !2019
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 137, !dbg !2019
  %107 = load ptr, ptr %106, align 4, !dbg !2019
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2019
  %109 = load ptr, ptr %4, align 4, !dbg !2019
  %110 = load ptr, ptr %5, align 4, !dbg !2019
  %111 = load ptr, ptr %6, align 4, !dbg !2019
  %112 = call arm_aapcs_vfpcc float %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2019
  store float %112, ptr %12, align 4, !dbg !2019
  call void @llvm.va_end(ptr %7), !dbg !2019
  %113 = load float, ptr %12, align 4, !dbg !2019
  ret float %113, !dbg !2019
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2037 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2038, metadata !DIExpression()), !dbg !2039
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2040, metadata !DIExpression()), !dbg !2039
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2041, metadata !DIExpression()), !dbg !2039
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2042, metadata !DIExpression()), !dbg !2039
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2043, metadata !DIExpression()), !dbg !2039
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2044, metadata !DIExpression()), !dbg !2039
  %13 = load ptr, ptr %8, align 4, !dbg !2039
  %14 = load ptr, ptr %13, align 4, !dbg !2039
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2039
  %16 = load ptr, ptr %15, align 4, !dbg !2039
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2039
  %18 = load ptr, ptr %6, align 4, !dbg !2039
  %19 = load ptr, ptr %8, align 4, !dbg !2039
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2039
  store i32 %20, ptr %10, align 4, !dbg !2039
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2045, metadata !DIExpression()), !dbg !2039
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2046, metadata !DIExpression()), !dbg !2048
  store i32 0, ptr %12, align 4, !dbg !2048
  br label %21, !dbg !2048

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2048
  %23 = load i32, ptr %10, align 4, !dbg !2048
  %24 = icmp slt i32 %22, %23, !dbg !2048
  br i1 %24, label %25, label %103, !dbg !2048

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2049
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2049
  %28 = load i8, ptr %27, align 1, !dbg !2049
  %29 = sext i8 %28 to i32, !dbg !2049
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2049

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2052
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2052
  store ptr %32, ptr %5, align 4, !dbg !2052
  %33 = load i32, ptr %31, align 4, !dbg !2052
  %34 = trunc i32 %33 to i8, !dbg !2052
  %35 = load i32, ptr %12, align 4, !dbg !2052
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2052
  store i8 %34, ptr %36, align 8, !dbg !2052
  br label %99, !dbg !2052

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2052
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2052
  store ptr %39, ptr %5, align 4, !dbg !2052
  %40 = load i32, ptr %38, align 4, !dbg !2052
  %41 = trunc i32 %40 to i8, !dbg !2052
  %42 = load i32, ptr %12, align 4, !dbg !2052
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2052
  store i8 %41, ptr %43, align 8, !dbg !2052
  br label %99, !dbg !2052

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2052
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2052
  store ptr %46, ptr %5, align 4, !dbg !2052
  %47 = load i32, ptr %45, align 4, !dbg !2052
  %48 = trunc i32 %47 to i16, !dbg !2052
  %49 = load i32, ptr %12, align 4, !dbg !2052
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2052
  store i16 %48, ptr %50, align 8, !dbg !2052
  br label %99, !dbg !2052

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2052
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2052
  store ptr %53, ptr %5, align 4, !dbg !2052
  %54 = load i32, ptr %52, align 4, !dbg !2052
  %55 = trunc i32 %54 to i16, !dbg !2052
  %56 = load i32, ptr %12, align 4, !dbg !2052
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2052
  store i16 %55, ptr %57, align 8, !dbg !2052
  br label %99, !dbg !2052

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2052
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2052
  store ptr %60, ptr %5, align 4, !dbg !2052
  %61 = load i32, ptr %59, align 4, !dbg !2052
  %62 = load i32, ptr %12, align 4, !dbg !2052
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2052
  store i32 %61, ptr %63, align 8, !dbg !2052
  br label %99, !dbg !2052

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2052
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2052
  store ptr %66, ptr %5, align 4, !dbg !2052
  %67 = load i32, ptr %65, align 4, !dbg !2052
  %68 = sext i32 %67 to i64, !dbg !2052
  %69 = load i32, ptr %12, align 4, !dbg !2052
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2052
  store i64 %68, ptr %70, align 8, !dbg !2052
  br label %99, !dbg !2052

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2052
  %73 = ptrtoint ptr %72 to i32, !dbg !2052
  %74 = add i32 %73, 7, !dbg !2052
  %75 = and i32 %74, -8, !dbg !2052
  %76 = inttoptr i32 %75 to ptr, !dbg !2052
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2052
  store ptr %77, ptr %5, align 4, !dbg !2052
  %78 = load double, ptr %76, align 8, !dbg !2052
  %79 = fptrunc double %78 to float, !dbg !2052
  %80 = load i32, ptr %12, align 4, !dbg !2052
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2052
  store float %79, ptr %81, align 8, !dbg !2052
  br label %99, !dbg !2052

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2052
  %84 = ptrtoint ptr %83 to i32, !dbg !2052
  %85 = add i32 %84, 7, !dbg !2052
  %86 = and i32 %85, -8, !dbg !2052
  %87 = inttoptr i32 %86 to ptr, !dbg !2052
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2052
  store ptr %88, ptr %5, align 4, !dbg !2052
  %89 = load double, ptr %87, align 8, !dbg !2052
  %90 = load i32, ptr %12, align 4, !dbg !2052
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2052
  store double %89, ptr %91, align 8, !dbg !2052
  br label %99, !dbg !2052

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2052
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2052
  store ptr %94, ptr %5, align 4, !dbg !2052
  %95 = load ptr, ptr %93, align 4, !dbg !2052
  %96 = load i32, ptr %12, align 4, !dbg !2052
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2052
  store ptr %95, ptr %97, align 8, !dbg !2052
  br label %99, !dbg !2052

98:                                               ; preds = %25
  br label %99, !dbg !2052

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2049

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2054
  %102 = add nsw i32 %101, 1, !dbg !2054
  store i32 %102, ptr %12, align 4, !dbg !2054
  br label %21, !dbg !2054, !llvm.loop !2055

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2039
  %105 = load ptr, ptr %104, align 4, !dbg !2039
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 137, !dbg !2039
  %107 = load ptr, ptr %106, align 4, !dbg !2039
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2039
  %109 = load ptr, ptr %6, align 4, !dbg !2039
  %110 = load ptr, ptr %7, align 4, !dbg !2039
  %111 = load ptr, ptr %8, align 4, !dbg !2039
  %112 = call arm_aapcs_vfpcc float %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2039
  ret float %112, !dbg !2039
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2056 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2057, metadata !DIExpression()), !dbg !2058
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2059, metadata !DIExpression()), !dbg !2058
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2060, metadata !DIExpression()), !dbg !2058
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2061, metadata !DIExpression()), !dbg !2058
  call void @llvm.va_start(ptr %7), !dbg !2058
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2062, metadata !DIExpression()), !dbg !2058
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2063, metadata !DIExpression()), !dbg !2058
  %13 = load ptr, ptr %6, align 4, !dbg !2058
  %14 = load ptr, ptr %13, align 4, !dbg !2058
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2058
  %16 = load ptr, ptr %15, align 4, !dbg !2058
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2058
  %18 = load ptr, ptr %4, align 4, !dbg !2058
  %19 = load ptr, ptr %6, align 4, !dbg !2058
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2058
  store i32 %20, ptr %9, align 4, !dbg !2058
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2064, metadata !DIExpression()), !dbg !2058
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2065, metadata !DIExpression()), !dbg !2067
  store i32 0, ptr %11, align 4, !dbg !2067
  br label %21, !dbg !2067

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !2067
  %23 = load i32, ptr %9, align 4, !dbg !2067
  %24 = icmp slt i32 %22, %23, !dbg !2067
  br i1 %24, label %25, label %103, !dbg !2067

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2068
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2068
  %28 = load i8, ptr %27, align 1, !dbg !2068
  %29 = sext i8 %28 to i32, !dbg !2068
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2068

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2071
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2071
  store ptr %32, ptr %7, align 4, !dbg !2071
  %33 = load i32, ptr %31, align 4, !dbg !2071
  %34 = trunc i32 %33 to i8, !dbg !2071
  %35 = load i32, ptr %11, align 4, !dbg !2071
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2071
  store i8 %34, ptr %36, align 8, !dbg !2071
  br label %99, !dbg !2071

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2071
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2071
  store ptr %39, ptr %7, align 4, !dbg !2071
  %40 = load i32, ptr %38, align 4, !dbg !2071
  %41 = trunc i32 %40 to i8, !dbg !2071
  %42 = load i32, ptr %11, align 4, !dbg !2071
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2071
  store i8 %41, ptr %43, align 8, !dbg !2071
  br label %99, !dbg !2071

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2071
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2071
  store ptr %46, ptr %7, align 4, !dbg !2071
  %47 = load i32, ptr %45, align 4, !dbg !2071
  %48 = trunc i32 %47 to i16, !dbg !2071
  %49 = load i32, ptr %11, align 4, !dbg !2071
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2071
  store i16 %48, ptr %50, align 8, !dbg !2071
  br label %99, !dbg !2071

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2071
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2071
  store ptr %53, ptr %7, align 4, !dbg !2071
  %54 = load i32, ptr %52, align 4, !dbg !2071
  %55 = trunc i32 %54 to i16, !dbg !2071
  %56 = load i32, ptr %11, align 4, !dbg !2071
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2071
  store i16 %55, ptr %57, align 8, !dbg !2071
  br label %99, !dbg !2071

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2071
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2071
  store ptr %60, ptr %7, align 4, !dbg !2071
  %61 = load i32, ptr %59, align 4, !dbg !2071
  %62 = load i32, ptr %11, align 4, !dbg !2071
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2071
  store i32 %61, ptr %63, align 8, !dbg !2071
  br label %99, !dbg !2071

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2071
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2071
  store ptr %66, ptr %7, align 4, !dbg !2071
  %67 = load i32, ptr %65, align 4, !dbg !2071
  %68 = sext i32 %67 to i64, !dbg !2071
  %69 = load i32, ptr %11, align 4, !dbg !2071
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2071
  store i64 %68, ptr %70, align 8, !dbg !2071
  br label %99, !dbg !2071

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2071
  %73 = ptrtoint ptr %72 to i32, !dbg !2071
  %74 = add i32 %73, 7, !dbg !2071
  %75 = and i32 %74, -8, !dbg !2071
  %76 = inttoptr i32 %75 to ptr, !dbg !2071
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2071
  store ptr %77, ptr %7, align 4, !dbg !2071
  %78 = load double, ptr %76, align 8, !dbg !2071
  %79 = fptrunc double %78 to float, !dbg !2071
  %80 = load i32, ptr %11, align 4, !dbg !2071
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !2071
  store float %79, ptr %81, align 8, !dbg !2071
  br label %99, !dbg !2071

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !2071
  %84 = ptrtoint ptr %83 to i32, !dbg !2071
  %85 = add i32 %84, 7, !dbg !2071
  %86 = and i32 %85, -8, !dbg !2071
  %87 = inttoptr i32 %86 to ptr, !dbg !2071
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2071
  store ptr %88, ptr %7, align 4, !dbg !2071
  %89 = load double, ptr %87, align 8, !dbg !2071
  %90 = load i32, ptr %11, align 4, !dbg !2071
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !2071
  store double %89, ptr %91, align 8, !dbg !2071
  br label %99, !dbg !2071

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !2071
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2071
  store ptr %94, ptr %7, align 4, !dbg !2071
  %95 = load ptr, ptr %93, align 4, !dbg !2071
  %96 = load i32, ptr %11, align 4, !dbg !2071
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !2071
  store ptr %95, ptr %97, align 8, !dbg !2071
  br label %99, !dbg !2071

98:                                               ; preds = %25
  br label %99, !dbg !2071

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2068

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !2073
  %102 = add nsw i32 %101, 1, !dbg !2073
  store i32 %102, ptr %11, align 4, !dbg !2073
  br label %21, !dbg !2073, !llvm.loop !2074

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2075, metadata !DIExpression()), !dbg !2058
  %104 = load ptr, ptr %6, align 4, !dbg !2058
  %105 = load ptr, ptr %104, align 4, !dbg !2058
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 60, !dbg !2058
  %107 = load ptr, ptr %106, align 4, !dbg !2058
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2058
  %109 = load ptr, ptr %4, align 4, !dbg !2058
  %110 = load ptr, ptr %5, align 4, !dbg !2058
  %111 = load ptr, ptr %6, align 4, !dbg !2058
  %112 = call arm_aapcs_vfpcc double %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2058
  store double %112, ptr %12, align 8, !dbg !2058
  call void @llvm.va_end(ptr %7), !dbg !2058
  %113 = load double, ptr %12, align 8, !dbg !2058
  ret double %113, !dbg !2058
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2076 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2077, metadata !DIExpression()), !dbg !2078
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2079, metadata !DIExpression()), !dbg !2078
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2080, metadata !DIExpression()), !dbg !2078
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2081, metadata !DIExpression()), !dbg !2078
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2082, metadata !DIExpression()), !dbg !2078
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2083, metadata !DIExpression()), !dbg !2078
  %13 = load ptr, ptr %8, align 4, !dbg !2078
  %14 = load ptr, ptr %13, align 4, !dbg !2078
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2078
  %16 = load ptr, ptr %15, align 4, !dbg !2078
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2078
  %18 = load ptr, ptr %6, align 4, !dbg !2078
  %19 = load ptr, ptr %8, align 4, !dbg !2078
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2078
  store i32 %20, ptr %10, align 4, !dbg !2078
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2084, metadata !DIExpression()), !dbg !2078
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2085, metadata !DIExpression()), !dbg !2087
  store i32 0, ptr %12, align 4, !dbg !2087
  br label %21, !dbg !2087

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2087
  %23 = load i32, ptr %10, align 4, !dbg !2087
  %24 = icmp slt i32 %22, %23, !dbg !2087
  br i1 %24, label %25, label %103, !dbg !2087

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2088
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2088
  %28 = load i8, ptr %27, align 1, !dbg !2088
  %29 = sext i8 %28 to i32, !dbg !2088
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2088

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2091
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2091
  store ptr %32, ptr %5, align 4, !dbg !2091
  %33 = load i32, ptr %31, align 4, !dbg !2091
  %34 = trunc i32 %33 to i8, !dbg !2091
  %35 = load i32, ptr %12, align 4, !dbg !2091
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2091
  store i8 %34, ptr %36, align 8, !dbg !2091
  br label %99, !dbg !2091

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2091
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2091
  store ptr %39, ptr %5, align 4, !dbg !2091
  %40 = load i32, ptr %38, align 4, !dbg !2091
  %41 = trunc i32 %40 to i8, !dbg !2091
  %42 = load i32, ptr %12, align 4, !dbg !2091
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2091
  store i8 %41, ptr %43, align 8, !dbg !2091
  br label %99, !dbg !2091

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2091
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2091
  store ptr %46, ptr %5, align 4, !dbg !2091
  %47 = load i32, ptr %45, align 4, !dbg !2091
  %48 = trunc i32 %47 to i16, !dbg !2091
  %49 = load i32, ptr %12, align 4, !dbg !2091
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2091
  store i16 %48, ptr %50, align 8, !dbg !2091
  br label %99, !dbg !2091

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2091
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2091
  store ptr %53, ptr %5, align 4, !dbg !2091
  %54 = load i32, ptr %52, align 4, !dbg !2091
  %55 = trunc i32 %54 to i16, !dbg !2091
  %56 = load i32, ptr %12, align 4, !dbg !2091
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2091
  store i16 %55, ptr %57, align 8, !dbg !2091
  br label %99, !dbg !2091

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2091
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2091
  store ptr %60, ptr %5, align 4, !dbg !2091
  %61 = load i32, ptr %59, align 4, !dbg !2091
  %62 = load i32, ptr %12, align 4, !dbg !2091
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2091
  store i32 %61, ptr %63, align 8, !dbg !2091
  br label %99, !dbg !2091

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2091
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2091
  store ptr %66, ptr %5, align 4, !dbg !2091
  %67 = load i32, ptr %65, align 4, !dbg !2091
  %68 = sext i32 %67 to i64, !dbg !2091
  %69 = load i32, ptr %12, align 4, !dbg !2091
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2091
  store i64 %68, ptr %70, align 8, !dbg !2091
  br label %99, !dbg !2091

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2091
  %73 = ptrtoint ptr %72 to i32, !dbg !2091
  %74 = add i32 %73, 7, !dbg !2091
  %75 = and i32 %74, -8, !dbg !2091
  %76 = inttoptr i32 %75 to ptr, !dbg !2091
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2091
  store ptr %77, ptr %5, align 4, !dbg !2091
  %78 = load double, ptr %76, align 8, !dbg !2091
  %79 = fptrunc double %78 to float, !dbg !2091
  %80 = load i32, ptr %12, align 4, !dbg !2091
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2091
  store float %79, ptr %81, align 8, !dbg !2091
  br label %99, !dbg !2091

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2091
  %84 = ptrtoint ptr %83 to i32, !dbg !2091
  %85 = add i32 %84, 7, !dbg !2091
  %86 = and i32 %85, -8, !dbg !2091
  %87 = inttoptr i32 %86 to ptr, !dbg !2091
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2091
  store ptr %88, ptr %5, align 4, !dbg !2091
  %89 = load double, ptr %87, align 8, !dbg !2091
  %90 = load i32, ptr %12, align 4, !dbg !2091
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2091
  store double %89, ptr %91, align 8, !dbg !2091
  br label %99, !dbg !2091

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2091
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2091
  store ptr %94, ptr %5, align 4, !dbg !2091
  %95 = load ptr, ptr %93, align 4, !dbg !2091
  %96 = load i32, ptr %12, align 4, !dbg !2091
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2091
  store ptr %95, ptr %97, align 8, !dbg !2091
  br label %99, !dbg !2091

98:                                               ; preds = %25
  br label %99, !dbg !2091

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2088

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2093
  %102 = add nsw i32 %101, 1, !dbg !2093
  store i32 %102, ptr %12, align 4, !dbg !2093
  br label %21, !dbg !2093, !llvm.loop !2094

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2078
  %105 = load ptr, ptr %104, align 4, !dbg !2078
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 60, !dbg !2078
  %107 = load ptr, ptr %106, align 4, !dbg !2078
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2078
  %109 = load ptr, ptr %6, align 4, !dbg !2078
  %110 = load ptr, ptr %7, align 4, !dbg !2078
  %111 = load ptr, ptr %8, align 4, !dbg !2078
  %112 = call arm_aapcs_vfpcc double %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2078
  ret double %112, !dbg !2078
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2095 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca double, align 8
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2096, metadata !DIExpression()), !dbg !2097
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2098, metadata !DIExpression()), !dbg !2097
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2099, metadata !DIExpression()), !dbg !2097
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2100, metadata !DIExpression()), !dbg !2097
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2101, metadata !DIExpression()), !dbg !2097
  call void @llvm.va_start(ptr %9), !dbg !2097
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2102, metadata !DIExpression()), !dbg !2097
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2103, metadata !DIExpression()), !dbg !2097
  %15 = load ptr, ptr %8, align 4, !dbg !2097
  %16 = load ptr, ptr %15, align 4, !dbg !2097
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2097
  %18 = load ptr, ptr %17, align 4, !dbg !2097
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !2097
  %20 = load ptr, ptr %5, align 4, !dbg !2097
  %21 = load ptr, ptr %8, align 4, !dbg !2097
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2097
  store i32 %22, ptr %11, align 4, !dbg !2097
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2104, metadata !DIExpression()), !dbg !2097
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2105, metadata !DIExpression()), !dbg !2107
  store i32 0, ptr %13, align 4, !dbg !2107
  br label %23, !dbg !2107

23:                                               ; preds = %102, %4
  %24 = load i32, ptr %13, align 4, !dbg !2107
  %25 = load i32, ptr %11, align 4, !dbg !2107
  %26 = icmp slt i32 %24, %25, !dbg !2107
  br i1 %26, label %27, label %105, !dbg !2107

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !2108
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !2108
  %30 = load i8, ptr %29, align 1, !dbg !2108
  %31 = sext i8 %30 to i32, !dbg !2108
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !2108

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !2111
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2111
  store ptr %34, ptr %9, align 4, !dbg !2111
  %35 = load i32, ptr %33, align 4, !dbg !2111
  %36 = trunc i32 %35 to i8, !dbg !2111
  %37 = load i32, ptr %13, align 4, !dbg !2111
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !2111
  store i8 %36, ptr %38, align 8, !dbg !2111
  br label %101, !dbg !2111

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !2111
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2111
  store ptr %41, ptr %9, align 4, !dbg !2111
  %42 = load i32, ptr %40, align 4, !dbg !2111
  %43 = trunc i32 %42 to i8, !dbg !2111
  %44 = load i32, ptr %13, align 4, !dbg !2111
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !2111
  store i8 %43, ptr %45, align 8, !dbg !2111
  br label %101, !dbg !2111

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !2111
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2111
  store ptr %48, ptr %9, align 4, !dbg !2111
  %49 = load i32, ptr %47, align 4, !dbg !2111
  %50 = trunc i32 %49 to i16, !dbg !2111
  %51 = load i32, ptr %13, align 4, !dbg !2111
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !2111
  store i16 %50, ptr %52, align 8, !dbg !2111
  br label %101, !dbg !2111

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !2111
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2111
  store ptr %55, ptr %9, align 4, !dbg !2111
  %56 = load i32, ptr %54, align 4, !dbg !2111
  %57 = trunc i32 %56 to i16, !dbg !2111
  %58 = load i32, ptr %13, align 4, !dbg !2111
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !2111
  store i16 %57, ptr %59, align 8, !dbg !2111
  br label %101, !dbg !2111

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !2111
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2111
  store ptr %62, ptr %9, align 4, !dbg !2111
  %63 = load i32, ptr %61, align 4, !dbg !2111
  %64 = load i32, ptr %13, align 4, !dbg !2111
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !2111
  store i32 %63, ptr %65, align 8, !dbg !2111
  br label %101, !dbg !2111

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !2111
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2111
  store ptr %68, ptr %9, align 4, !dbg !2111
  %69 = load i32, ptr %67, align 4, !dbg !2111
  %70 = sext i32 %69 to i64, !dbg !2111
  %71 = load i32, ptr %13, align 4, !dbg !2111
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !2111
  store i64 %70, ptr %72, align 8, !dbg !2111
  br label %101, !dbg !2111

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !2111
  %75 = ptrtoint ptr %74 to i32, !dbg !2111
  %76 = add i32 %75, 7, !dbg !2111
  %77 = and i32 %76, -8, !dbg !2111
  %78 = inttoptr i32 %77 to ptr, !dbg !2111
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2111
  store ptr %79, ptr %9, align 4, !dbg !2111
  %80 = load double, ptr %78, align 8, !dbg !2111
  %81 = fptrunc double %80 to float, !dbg !2111
  %82 = load i32, ptr %13, align 4, !dbg !2111
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %82, !dbg !2111
  store float %81, ptr %83, align 8, !dbg !2111
  br label %101, !dbg !2111

84:                                               ; preds = %27
  %85 = load ptr, ptr %9, align 4, !dbg !2111
  %86 = ptrtoint ptr %85 to i32, !dbg !2111
  %87 = add i32 %86, 7, !dbg !2111
  %88 = and i32 %87, -8, !dbg !2111
  %89 = inttoptr i32 %88 to ptr, !dbg !2111
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !2111
  store ptr %90, ptr %9, align 4, !dbg !2111
  %91 = load double, ptr %89, align 8, !dbg !2111
  %92 = load i32, ptr %13, align 4, !dbg !2111
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %92, !dbg !2111
  store double %91, ptr %93, align 8, !dbg !2111
  br label %101, !dbg !2111

94:                                               ; preds = %27
  %95 = load ptr, ptr %9, align 4, !dbg !2111
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !2111
  store ptr %96, ptr %9, align 4, !dbg !2111
  %97 = load ptr, ptr %95, align 4, !dbg !2111
  %98 = load i32, ptr %13, align 4, !dbg !2111
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %98, !dbg !2111
  store ptr %97, ptr %99, align 8, !dbg !2111
  br label %101, !dbg !2111

100:                                              ; preds = %27
  br label %101, !dbg !2111

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !2108

102:                                              ; preds = %101
  %103 = load i32, ptr %13, align 4, !dbg !2113
  %104 = add nsw i32 %103, 1, !dbg !2113
  store i32 %104, ptr %13, align 4, !dbg !2113
  br label %23, !dbg !2113, !llvm.loop !2114

105:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2115, metadata !DIExpression()), !dbg !2097
  %106 = load ptr, ptr %8, align 4, !dbg !2097
  %107 = load ptr, ptr %106, align 4, !dbg !2097
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 90, !dbg !2097
  %109 = load ptr, ptr %108, align 4, !dbg !2097
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !2097
  %111 = load ptr, ptr %5, align 4, !dbg !2097
  %112 = load ptr, ptr %6, align 4, !dbg !2097
  %113 = load ptr, ptr %7, align 4, !dbg !2097
  %114 = load ptr, ptr %8, align 4, !dbg !2097
  %115 = call arm_aapcs_vfpcc double %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2097
  store double %115, ptr %14, align 8, !dbg !2097
  call void @llvm.va_end(ptr %9), !dbg !2097
  %116 = load double, ptr %14, align 8, !dbg !2097
  ret double %116, !dbg !2097
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2116 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2117, metadata !DIExpression()), !dbg !2118
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2119, metadata !DIExpression()), !dbg !2118
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2120, metadata !DIExpression()), !dbg !2118
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2121, metadata !DIExpression()), !dbg !2118
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2122, metadata !DIExpression()), !dbg !2118
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2123, metadata !DIExpression()), !dbg !2118
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2124, metadata !DIExpression()), !dbg !2118
  %15 = load ptr, ptr %10, align 4, !dbg !2118
  %16 = load ptr, ptr %15, align 4, !dbg !2118
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2118
  %18 = load ptr, ptr %17, align 4, !dbg !2118
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !2118
  %20 = load ptr, ptr %7, align 4, !dbg !2118
  %21 = load ptr, ptr %10, align 4, !dbg !2118
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2118
  store i32 %22, ptr %12, align 4, !dbg !2118
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2125, metadata !DIExpression()), !dbg !2118
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2126, metadata !DIExpression()), !dbg !2128
  store i32 0, ptr %14, align 4, !dbg !2128
  br label %23, !dbg !2128

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !2128
  %25 = load i32, ptr %12, align 4, !dbg !2128
  %26 = icmp slt i32 %24, %25, !dbg !2128
  br i1 %26, label %27, label %105, !dbg !2128

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2129
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2129
  %30 = load i8, ptr %29, align 1, !dbg !2129
  %31 = sext i8 %30 to i32, !dbg !2129
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !2129

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2132
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2132
  store ptr %34, ptr %6, align 4, !dbg !2132
  %35 = load i32, ptr %33, align 4, !dbg !2132
  %36 = trunc i32 %35 to i8, !dbg !2132
  %37 = load i32, ptr %14, align 4, !dbg !2132
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2132
  store i8 %36, ptr %38, align 8, !dbg !2132
  br label %101, !dbg !2132

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2132
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2132
  store ptr %41, ptr %6, align 4, !dbg !2132
  %42 = load i32, ptr %40, align 4, !dbg !2132
  %43 = trunc i32 %42 to i8, !dbg !2132
  %44 = load i32, ptr %14, align 4, !dbg !2132
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2132
  store i8 %43, ptr %45, align 8, !dbg !2132
  br label %101, !dbg !2132

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2132
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2132
  store ptr %48, ptr %6, align 4, !dbg !2132
  %49 = load i32, ptr %47, align 4, !dbg !2132
  %50 = trunc i32 %49 to i16, !dbg !2132
  %51 = load i32, ptr %14, align 4, !dbg !2132
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2132
  store i16 %50, ptr %52, align 8, !dbg !2132
  br label %101, !dbg !2132

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2132
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2132
  store ptr %55, ptr %6, align 4, !dbg !2132
  %56 = load i32, ptr %54, align 4, !dbg !2132
  %57 = trunc i32 %56 to i16, !dbg !2132
  %58 = load i32, ptr %14, align 4, !dbg !2132
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2132
  store i16 %57, ptr %59, align 8, !dbg !2132
  br label %101, !dbg !2132

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2132
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2132
  store ptr %62, ptr %6, align 4, !dbg !2132
  %63 = load i32, ptr %61, align 4, !dbg !2132
  %64 = load i32, ptr %14, align 4, !dbg !2132
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2132
  store i32 %63, ptr %65, align 8, !dbg !2132
  br label %101, !dbg !2132

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2132
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2132
  store ptr %68, ptr %6, align 4, !dbg !2132
  %69 = load i32, ptr %67, align 4, !dbg !2132
  %70 = sext i32 %69 to i64, !dbg !2132
  %71 = load i32, ptr %14, align 4, !dbg !2132
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2132
  store i64 %70, ptr %72, align 8, !dbg !2132
  br label %101, !dbg !2132

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2132
  %75 = ptrtoint ptr %74 to i32, !dbg !2132
  %76 = add i32 %75, 7, !dbg !2132
  %77 = and i32 %76, -8, !dbg !2132
  %78 = inttoptr i32 %77 to ptr, !dbg !2132
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2132
  store ptr %79, ptr %6, align 4, !dbg !2132
  %80 = load double, ptr %78, align 8, !dbg !2132
  %81 = fptrunc double %80 to float, !dbg !2132
  %82 = load i32, ptr %14, align 4, !dbg !2132
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !2132
  store float %81, ptr %83, align 8, !dbg !2132
  br label %101, !dbg !2132

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !2132
  %86 = ptrtoint ptr %85 to i32, !dbg !2132
  %87 = add i32 %86, 7, !dbg !2132
  %88 = and i32 %87, -8, !dbg !2132
  %89 = inttoptr i32 %88 to ptr, !dbg !2132
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !2132
  store ptr %90, ptr %6, align 4, !dbg !2132
  %91 = load double, ptr %89, align 8, !dbg !2132
  %92 = load i32, ptr %14, align 4, !dbg !2132
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !2132
  store double %91, ptr %93, align 8, !dbg !2132
  br label %101, !dbg !2132

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !2132
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !2132
  store ptr %96, ptr %6, align 4, !dbg !2132
  %97 = load ptr, ptr %95, align 4, !dbg !2132
  %98 = load i32, ptr %14, align 4, !dbg !2132
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !2132
  store ptr %97, ptr %99, align 8, !dbg !2132
  br label %101, !dbg !2132

100:                                              ; preds = %27
  br label %101, !dbg !2132

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !2129

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !2134
  %104 = add nsw i32 %103, 1, !dbg !2134
  store i32 %104, ptr %14, align 4, !dbg !2134
  br label %23, !dbg !2134, !llvm.loop !2135

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !2118
  %107 = load ptr, ptr %106, align 4, !dbg !2118
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 90, !dbg !2118
  %109 = load ptr, ptr %108, align 4, !dbg !2118
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !2118
  %111 = load ptr, ptr %7, align 4, !dbg !2118
  %112 = load ptr, ptr %8, align 4, !dbg !2118
  %113 = load ptr, ptr %9, align 4, !dbg !2118
  %114 = load ptr, ptr %10, align 4, !dbg !2118
  %115 = call arm_aapcs_vfpcc double %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2118
  ret double %115, !dbg !2118
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2136 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2137, metadata !DIExpression()), !dbg !2138
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2139, metadata !DIExpression()), !dbg !2138
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2140, metadata !DIExpression()), !dbg !2138
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2141, metadata !DIExpression()), !dbg !2138
  call void @llvm.va_start(ptr %7), !dbg !2138
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2142, metadata !DIExpression()), !dbg !2138
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2143, metadata !DIExpression()), !dbg !2138
  %13 = load ptr, ptr %6, align 4, !dbg !2138
  %14 = load ptr, ptr %13, align 4, !dbg !2138
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2138
  %16 = load ptr, ptr %15, align 4, !dbg !2138
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2138
  %18 = load ptr, ptr %4, align 4, !dbg !2138
  %19 = load ptr, ptr %6, align 4, !dbg !2138
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2138
  store i32 %20, ptr %9, align 4, !dbg !2138
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2144, metadata !DIExpression()), !dbg !2138
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2145, metadata !DIExpression()), !dbg !2147
  store i32 0, ptr %11, align 4, !dbg !2147
  br label %21, !dbg !2147

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !2147
  %23 = load i32, ptr %9, align 4, !dbg !2147
  %24 = icmp slt i32 %22, %23, !dbg !2147
  br i1 %24, label %25, label %103, !dbg !2147

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2148
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2148
  %28 = load i8, ptr %27, align 1, !dbg !2148
  %29 = sext i8 %28 to i32, !dbg !2148
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2148

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2151
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2151
  store ptr %32, ptr %7, align 4, !dbg !2151
  %33 = load i32, ptr %31, align 4, !dbg !2151
  %34 = trunc i32 %33 to i8, !dbg !2151
  %35 = load i32, ptr %11, align 4, !dbg !2151
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2151
  store i8 %34, ptr %36, align 8, !dbg !2151
  br label %99, !dbg !2151

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2151
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2151
  store ptr %39, ptr %7, align 4, !dbg !2151
  %40 = load i32, ptr %38, align 4, !dbg !2151
  %41 = trunc i32 %40 to i8, !dbg !2151
  %42 = load i32, ptr %11, align 4, !dbg !2151
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2151
  store i8 %41, ptr %43, align 8, !dbg !2151
  br label %99, !dbg !2151

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2151
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2151
  store ptr %46, ptr %7, align 4, !dbg !2151
  %47 = load i32, ptr %45, align 4, !dbg !2151
  %48 = trunc i32 %47 to i16, !dbg !2151
  %49 = load i32, ptr %11, align 4, !dbg !2151
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2151
  store i16 %48, ptr %50, align 8, !dbg !2151
  br label %99, !dbg !2151

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2151
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2151
  store ptr %53, ptr %7, align 4, !dbg !2151
  %54 = load i32, ptr %52, align 4, !dbg !2151
  %55 = trunc i32 %54 to i16, !dbg !2151
  %56 = load i32, ptr %11, align 4, !dbg !2151
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2151
  store i16 %55, ptr %57, align 8, !dbg !2151
  br label %99, !dbg !2151

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2151
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2151
  store ptr %60, ptr %7, align 4, !dbg !2151
  %61 = load i32, ptr %59, align 4, !dbg !2151
  %62 = load i32, ptr %11, align 4, !dbg !2151
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2151
  store i32 %61, ptr %63, align 8, !dbg !2151
  br label %99, !dbg !2151

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2151
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2151
  store ptr %66, ptr %7, align 4, !dbg !2151
  %67 = load i32, ptr %65, align 4, !dbg !2151
  %68 = sext i32 %67 to i64, !dbg !2151
  %69 = load i32, ptr %11, align 4, !dbg !2151
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2151
  store i64 %68, ptr %70, align 8, !dbg !2151
  br label %99, !dbg !2151

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2151
  %73 = ptrtoint ptr %72 to i32, !dbg !2151
  %74 = add i32 %73, 7, !dbg !2151
  %75 = and i32 %74, -8, !dbg !2151
  %76 = inttoptr i32 %75 to ptr, !dbg !2151
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2151
  store ptr %77, ptr %7, align 4, !dbg !2151
  %78 = load double, ptr %76, align 8, !dbg !2151
  %79 = fptrunc double %78 to float, !dbg !2151
  %80 = load i32, ptr %11, align 4, !dbg !2151
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !2151
  store float %79, ptr %81, align 8, !dbg !2151
  br label %99, !dbg !2151

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !2151
  %84 = ptrtoint ptr %83 to i32, !dbg !2151
  %85 = add i32 %84, 7, !dbg !2151
  %86 = and i32 %85, -8, !dbg !2151
  %87 = inttoptr i32 %86 to ptr, !dbg !2151
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2151
  store ptr %88, ptr %7, align 4, !dbg !2151
  %89 = load double, ptr %87, align 8, !dbg !2151
  %90 = load i32, ptr %11, align 4, !dbg !2151
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !2151
  store double %89, ptr %91, align 8, !dbg !2151
  br label %99, !dbg !2151

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !2151
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2151
  store ptr %94, ptr %7, align 4, !dbg !2151
  %95 = load ptr, ptr %93, align 4, !dbg !2151
  %96 = load i32, ptr %11, align 4, !dbg !2151
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !2151
  store ptr %95, ptr %97, align 8, !dbg !2151
  br label %99, !dbg !2151

98:                                               ; preds = %25
  br label %99, !dbg !2151

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2148

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !2153
  %102 = add nsw i32 %101, 1, !dbg !2153
  store i32 %102, ptr %11, align 4, !dbg !2153
  br label %21, !dbg !2153, !llvm.loop !2154

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2155, metadata !DIExpression()), !dbg !2138
  %104 = load ptr, ptr %6, align 4, !dbg !2138
  %105 = load ptr, ptr %104, align 4, !dbg !2138
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 140, !dbg !2138
  %107 = load ptr, ptr %106, align 4, !dbg !2138
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2138
  %109 = load ptr, ptr %4, align 4, !dbg !2138
  %110 = load ptr, ptr %5, align 4, !dbg !2138
  %111 = load ptr, ptr %6, align 4, !dbg !2138
  %112 = call arm_aapcs_vfpcc double %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2138
  store double %112, ptr %12, align 8, !dbg !2138
  call void @llvm.va_end(ptr %7), !dbg !2138
  %113 = load double, ptr %12, align 8, !dbg !2138
  ret double %113, !dbg !2138
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2156 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2157, metadata !DIExpression()), !dbg !2158
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2159, metadata !DIExpression()), !dbg !2158
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2160, metadata !DIExpression()), !dbg !2158
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2161, metadata !DIExpression()), !dbg !2158
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2162, metadata !DIExpression()), !dbg !2158
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2163, metadata !DIExpression()), !dbg !2158
  %13 = load ptr, ptr %8, align 4, !dbg !2158
  %14 = load ptr, ptr %13, align 4, !dbg !2158
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2158
  %16 = load ptr, ptr %15, align 4, !dbg !2158
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2158
  %18 = load ptr, ptr %6, align 4, !dbg !2158
  %19 = load ptr, ptr %8, align 4, !dbg !2158
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2158
  store i32 %20, ptr %10, align 4, !dbg !2158
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2164, metadata !DIExpression()), !dbg !2158
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2165, metadata !DIExpression()), !dbg !2167
  store i32 0, ptr %12, align 4, !dbg !2167
  br label %21, !dbg !2167

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2167
  %23 = load i32, ptr %10, align 4, !dbg !2167
  %24 = icmp slt i32 %22, %23, !dbg !2167
  br i1 %24, label %25, label %103, !dbg !2167

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2168
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2168
  %28 = load i8, ptr %27, align 1, !dbg !2168
  %29 = sext i8 %28 to i32, !dbg !2168
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2168

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2171
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2171
  store ptr %32, ptr %5, align 4, !dbg !2171
  %33 = load i32, ptr %31, align 4, !dbg !2171
  %34 = trunc i32 %33 to i8, !dbg !2171
  %35 = load i32, ptr %12, align 4, !dbg !2171
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2171
  store i8 %34, ptr %36, align 8, !dbg !2171
  br label %99, !dbg !2171

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2171
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2171
  store ptr %39, ptr %5, align 4, !dbg !2171
  %40 = load i32, ptr %38, align 4, !dbg !2171
  %41 = trunc i32 %40 to i8, !dbg !2171
  %42 = load i32, ptr %12, align 4, !dbg !2171
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2171
  store i8 %41, ptr %43, align 8, !dbg !2171
  br label %99, !dbg !2171

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2171
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2171
  store ptr %46, ptr %5, align 4, !dbg !2171
  %47 = load i32, ptr %45, align 4, !dbg !2171
  %48 = trunc i32 %47 to i16, !dbg !2171
  %49 = load i32, ptr %12, align 4, !dbg !2171
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2171
  store i16 %48, ptr %50, align 8, !dbg !2171
  br label %99, !dbg !2171

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2171
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2171
  store ptr %53, ptr %5, align 4, !dbg !2171
  %54 = load i32, ptr %52, align 4, !dbg !2171
  %55 = trunc i32 %54 to i16, !dbg !2171
  %56 = load i32, ptr %12, align 4, !dbg !2171
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2171
  store i16 %55, ptr %57, align 8, !dbg !2171
  br label %99, !dbg !2171

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2171
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2171
  store ptr %60, ptr %5, align 4, !dbg !2171
  %61 = load i32, ptr %59, align 4, !dbg !2171
  %62 = load i32, ptr %12, align 4, !dbg !2171
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2171
  store i32 %61, ptr %63, align 8, !dbg !2171
  br label %99, !dbg !2171

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2171
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2171
  store ptr %66, ptr %5, align 4, !dbg !2171
  %67 = load i32, ptr %65, align 4, !dbg !2171
  %68 = sext i32 %67 to i64, !dbg !2171
  %69 = load i32, ptr %12, align 4, !dbg !2171
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2171
  store i64 %68, ptr %70, align 8, !dbg !2171
  br label %99, !dbg !2171

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2171
  %73 = ptrtoint ptr %72 to i32, !dbg !2171
  %74 = add i32 %73, 7, !dbg !2171
  %75 = and i32 %74, -8, !dbg !2171
  %76 = inttoptr i32 %75 to ptr, !dbg !2171
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2171
  store ptr %77, ptr %5, align 4, !dbg !2171
  %78 = load double, ptr %76, align 8, !dbg !2171
  %79 = fptrunc double %78 to float, !dbg !2171
  %80 = load i32, ptr %12, align 4, !dbg !2171
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2171
  store float %79, ptr %81, align 8, !dbg !2171
  br label %99, !dbg !2171

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2171
  %84 = ptrtoint ptr %83 to i32, !dbg !2171
  %85 = add i32 %84, 7, !dbg !2171
  %86 = and i32 %85, -8, !dbg !2171
  %87 = inttoptr i32 %86 to ptr, !dbg !2171
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2171
  store ptr %88, ptr %5, align 4, !dbg !2171
  %89 = load double, ptr %87, align 8, !dbg !2171
  %90 = load i32, ptr %12, align 4, !dbg !2171
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2171
  store double %89, ptr %91, align 8, !dbg !2171
  br label %99, !dbg !2171

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2171
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2171
  store ptr %94, ptr %5, align 4, !dbg !2171
  %95 = load ptr, ptr %93, align 4, !dbg !2171
  %96 = load i32, ptr %12, align 4, !dbg !2171
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2171
  store ptr %95, ptr %97, align 8, !dbg !2171
  br label %99, !dbg !2171

98:                                               ; preds = %25
  br label %99, !dbg !2171

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2168

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2173
  %102 = add nsw i32 %101, 1, !dbg !2173
  store i32 %102, ptr %12, align 4, !dbg !2173
  br label %21, !dbg !2173, !llvm.loop !2174

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2158
  %105 = load ptr, ptr %104, align 4, !dbg !2158
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 140, !dbg !2158
  %107 = load ptr, ptr %106, align 4, !dbg !2158
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2158
  %109 = load ptr, ptr %6, align 4, !dbg !2158
  %110 = load ptr, ptr %7, align 4, !dbg !2158
  %111 = load ptr, ptr %8, align 4, !dbg !2158
  %112 = call arm_aapcs_vfpcc double %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2158
  ret double %112, !dbg !2158
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2175 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2176, metadata !DIExpression()), !dbg !2177
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2178, metadata !DIExpression()), !dbg !2177
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2179, metadata !DIExpression()), !dbg !2177
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2180, metadata !DIExpression()), !dbg !2181
  call void @llvm.va_start(ptr %7), !dbg !2182
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2183, metadata !DIExpression()), !dbg !2184
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2185, metadata !DIExpression()), !dbg !2184
  %13 = load ptr, ptr %6, align 4, !dbg !2184
  %14 = load ptr, ptr %13, align 4, !dbg !2184
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2184
  %16 = load ptr, ptr %15, align 4, !dbg !2184
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2184
  %18 = load ptr, ptr %4, align 4, !dbg !2184
  %19 = load ptr, ptr %6, align 4, !dbg !2184
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2184
  store i32 %20, ptr %9, align 4, !dbg !2184
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2186, metadata !DIExpression()), !dbg !2184
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2187, metadata !DIExpression()), !dbg !2189
  store i32 0, ptr %11, align 4, !dbg !2189
  br label %21, !dbg !2189

21:                                               ; preds = %100, %3
  %22 = load i32, ptr %11, align 4, !dbg !2189
  %23 = load i32, ptr %9, align 4, !dbg !2189
  %24 = icmp slt i32 %22, %23, !dbg !2189
  br i1 %24, label %25, label %103, !dbg !2189

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2190
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2190
  %28 = load i8, ptr %27, align 1, !dbg !2190
  %29 = sext i8 %28 to i32, !dbg !2190
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2190

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2193
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2193
  store ptr %32, ptr %7, align 4, !dbg !2193
  %33 = load i32, ptr %31, align 4, !dbg !2193
  %34 = trunc i32 %33 to i8, !dbg !2193
  %35 = load i32, ptr %11, align 4, !dbg !2193
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2193
  store i8 %34, ptr %36, align 8, !dbg !2193
  br label %99, !dbg !2193

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2193
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2193
  store ptr %39, ptr %7, align 4, !dbg !2193
  %40 = load i32, ptr %38, align 4, !dbg !2193
  %41 = trunc i32 %40 to i8, !dbg !2193
  %42 = load i32, ptr %11, align 4, !dbg !2193
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2193
  store i8 %41, ptr %43, align 8, !dbg !2193
  br label %99, !dbg !2193

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2193
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2193
  store ptr %46, ptr %7, align 4, !dbg !2193
  %47 = load i32, ptr %45, align 4, !dbg !2193
  %48 = trunc i32 %47 to i16, !dbg !2193
  %49 = load i32, ptr %11, align 4, !dbg !2193
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2193
  store i16 %48, ptr %50, align 8, !dbg !2193
  br label %99, !dbg !2193

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2193
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2193
  store ptr %53, ptr %7, align 4, !dbg !2193
  %54 = load i32, ptr %52, align 4, !dbg !2193
  %55 = trunc i32 %54 to i16, !dbg !2193
  %56 = load i32, ptr %11, align 4, !dbg !2193
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2193
  store i16 %55, ptr %57, align 8, !dbg !2193
  br label %99, !dbg !2193

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2193
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2193
  store ptr %60, ptr %7, align 4, !dbg !2193
  %61 = load i32, ptr %59, align 4, !dbg !2193
  %62 = load i32, ptr %11, align 4, !dbg !2193
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2193
  store i32 %61, ptr %63, align 8, !dbg !2193
  br label %99, !dbg !2193

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2193
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2193
  store ptr %66, ptr %7, align 4, !dbg !2193
  %67 = load i32, ptr %65, align 4, !dbg !2193
  %68 = sext i32 %67 to i64, !dbg !2193
  %69 = load i32, ptr %11, align 4, !dbg !2193
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2193
  store i64 %68, ptr %70, align 8, !dbg !2193
  br label %99, !dbg !2193

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2193
  %73 = ptrtoint ptr %72 to i32, !dbg !2193
  %74 = add i32 %73, 7, !dbg !2193
  %75 = and i32 %74, -8, !dbg !2193
  %76 = inttoptr i32 %75 to ptr, !dbg !2193
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2193
  store ptr %77, ptr %7, align 4, !dbg !2193
  %78 = load double, ptr %76, align 8, !dbg !2193
  %79 = fptrunc double %78 to float, !dbg !2193
  %80 = load i32, ptr %11, align 4, !dbg !2193
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %80, !dbg !2193
  store float %79, ptr %81, align 8, !dbg !2193
  br label %99, !dbg !2193

82:                                               ; preds = %25
  %83 = load ptr, ptr %7, align 4, !dbg !2193
  %84 = ptrtoint ptr %83 to i32, !dbg !2193
  %85 = add i32 %84, 7, !dbg !2193
  %86 = and i32 %85, -8, !dbg !2193
  %87 = inttoptr i32 %86 to ptr, !dbg !2193
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2193
  store ptr %88, ptr %7, align 4, !dbg !2193
  %89 = load double, ptr %87, align 8, !dbg !2193
  %90 = load i32, ptr %11, align 4, !dbg !2193
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %90, !dbg !2193
  store double %89, ptr %91, align 8, !dbg !2193
  br label %99, !dbg !2193

92:                                               ; preds = %25
  %93 = load ptr, ptr %7, align 4, !dbg !2193
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2193
  store ptr %94, ptr %7, align 4, !dbg !2193
  %95 = load ptr, ptr %93, align 4, !dbg !2193
  %96 = load i32, ptr %11, align 4, !dbg !2193
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %96, !dbg !2193
  store ptr %95, ptr %97, align 8, !dbg !2193
  br label %99, !dbg !2193

98:                                               ; preds = %25
  br label %99, !dbg !2193

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2190

100:                                              ; preds = %99
  %101 = load i32, ptr %11, align 4, !dbg !2195
  %102 = add nsw i32 %101, 1, !dbg !2195
  store i32 %102, ptr %11, align 4, !dbg !2195
  br label %21, !dbg !2195, !llvm.loop !2196

103:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2197, metadata !DIExpression()), !dbg !2198
  %104 = load ptr, ptr %6, align 4, !dbg !2198
  %105 = load ptr, ptr %104, align 4, !dbg !2198
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 30, !dbg !2198
  %107 = load ptr, ptr %106, align 4, !dbg !2198
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2198
  %109 = load ptr, ptr %4, align 4, !dbg !2198
  %110 = load ptr, ptr %5, align 4, !dbg !2198
  %111 = load ptr, ptr %6, align 4, !dbg !2198
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2198
  store ptr %112, ptr %12, align 4, !dbg !2198
  call void @llvm.va_end(ptr %7), !dbg !2199
  %113 = load ptr, ptr %12, align 4, !dbg !2200
  ret ptr %113, !dbg !2200
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2201 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2202, metadata !DIExpression()), !dbg !2203
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2204, metadata !DIExpression()), !dbg !2203
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2205, metadata !DIExpression()), !dbg !2203
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2206, metadata !DIExpression()), !dbg !2203
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2207, metadata !DIExpression()), !dbg !2208
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2209, metadata !DIExpression()), !dbg !2208
  %13 = load ptr, ptr %8, align 4, !dbg !2208
  %14 = load ptr, ptr %13, align 4, !dbg !2208
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2208
  %16 = load ptr, ptr %15, align 4, !dbg !2208
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2208
  %18 = load ptr, ptr %6, align 4, !dbg !2208
  %19 = load ptr, ptr %8, align 4, !dbg !2208
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2208
  store i32 %20, ptr %10, align 4, !dbg !2208
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2210, metadata !DIExpression()), !dbg !2208
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2211, metadata !DIExpression()), !dbg !2213
  store i32 0, ptr %12, align 4, !dbg !2213
  br label %21, !dbg !2213

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2213
  %23 = load i32, ptr %10, align 4, !dbg !2213
  %24 = icmp slt i32 %22, %23, !dbg !2213
  br i1 %24, label %25, label %103, !dbg !2213

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2214
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2214
  %28 = load i8, ptr %27, align 1, !dbg !2214
  %29 = sext i8 %28 to i32, !dbg !2214
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2214

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2217
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2217
  store ptr %32, ptr %5, align 4, !dbg !2217
  %33 = load i32, ptr %31, align 4, !dbg !2217
  %34 = trunc i32 %33 to i8, !dbg !2217
  %35 = load i32, ptr %12, align 4, !dbg !2217
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2217
  store i8 %34, ptr %36, align 8, !dbg !2217
  br label %99, !dbg !2217

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2217
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2217
  store ptr %39, ptr %5, align 4, !dbg !2217
  %40 = load i32, ptr %38, align 4, !dbg !2217
  %41 = trunc i32 %40 to i8, !dbg !2217
  %42 = load i32, ptr %12, align 4, !dbg !2217
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2217
  store i8 %41, ptr %43, align 8, !dbg !2217
  br label %99, !dbg !2217

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2217
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2217
  store ptr %46, ptr %5, align 4, !dbg !2217
  %47 = load i32, ptr %45, align 4, !dbg !2217
  %48 = trunc i32 %47 to i16, !dbg !2217
  %49 = load i32, ptr %12, align 4, !dbg !2217
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2217
  store i16 %48, ptr %50, align 8, !dbg !2217
  br label %99, !dbg !2217

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2217
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2217
  store ptr %53, ptr %5, align 4, !dbg !2217
  %54 = load i32, ptr %52, align 4, !dbg !2217
  %55 = trunc i32 %54 to i16, !dbg !2217
  %56 = load i32, ptr %12, align 4, !dbg !2217
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2217
  store i16 %55, ptr %57, align 8, !dbg !2217
  br label %99, !dbg !2217

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2217
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2217
  store ptr %60, ptr %5, align 4, !dbg !2217
  %61 = load i32, ptr %59, align 4, !dbg !2217
  %62 = load i32, ptr %12, align 4, !dbg !2217
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2217
  store i32 %61, ptr %63, align 8, !dbg !2217
  br label %99, !dbg !2217

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2217
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2217
  store ptr %66, ptr %5, align 4, !dbg !2217
  %67 = load i32, ptr %65, align 4, !dbg !2217
  %68 = sext i32 %67 to i64, !dbg !2217
  %69 = load i32, ptr %12, align 4, !dbg !2217
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2217
  store i64 %68, ptr %70, align 8, !dbg !2217
  br label %99, !dbg !2217

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2217
  %73 = ptrtoint ptr %72 to i32, !dbg !2217
  %74 = add i32 %73, 7, !dbg !2217
  %75 = and i32 %74, -8, !dbg !2217
  %76 = inttoptr i32 %75 to ptr, !dbg !2217
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2217
  store ptr %77, ptr %5, align 4, !dbg !2217
  %78 = load double, ptr %76, align 8, !dbg !2217
  %79 = fptrunc double %78 to float, !dbg !2217
  %80 = load i32, ptr %12, align 4, !dbg !2217
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2217
  store float %79, ptr %81, align 8, !dbg !2217
  br label %99, !dbg !2217

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2217
  %84 = ptrtoint ptr %83 to i32, !dbg !2217
  %85 = add i32 %84, 7, !dbg !2217
  %86 = and i32 %85, -8, !dbg !2217
  %87 = inttoptr i32 %86 to ptr, !dbg !2217
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2217
  store ptr %88, ptr %5, align 4, !dbg !2217
  %89 = load double, ptr %87, align 8, !dbg !2217
  %90 = load i32, ptr %12, align 4, !dbg !2217
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2217
  store double %89, ptr %91, align 8, !dbg !2217
  br label %99, !dbg !2217

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2217
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2217
  store ptr %94, ptr %5, align 4, !dbg !2217
  %95 = load ptr, ptr %93, align 4, !dbg !2217
  %96 = load i32, ptr %12, align 4, !dbg !2217
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2217
  store ptr %95, ptr %97, align 8, !dbg !2217
  br label %99, !dbg !2217

98:                                               ; preds = %25
  br label %99, !dbg !2217

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2214

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2219
  %102 = add nsw i32 %101, 1, !dbg !2219
  store i32 %102, ptr %12, align 4, !dbg !2219
  br label %21, !dbg !2219, !llvm.loop !2220

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2221
  %105 = load ptr, ptr %104, align 4, !dbg !2221
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 30, !dbg !2221
  %107 = load ptr, ptr %106, align 4, !dbg !2221
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2221
  %109 = load ptr, ptr %6, align 4, !dbg !2221
  %110 = load ptr, ptr %7, align 4, !dbg !2221
  %111 = load ptr, ptr %8, align 4, !dbg !2221
  %112 = call arm_aapcs_vfpcc ptr %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2221
  ret ptr %112, !dbg !2221
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2222 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2223, metadata !DIExpression()), !dbg !2224
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2225, metadata !DIExpression()), !dbg !2224
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2226, metadata !DIExpression()), !dbg !2224
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2227, metadata !DIExpression()), !dbg !2228
  call void @llvm.va_start(ptr %7), !dbg !2229
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2230, metadata !DIExpression()), !dbg !2231
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2232, metadata !DIExpression()), !dbg !2231
  %12 = load ptr, ptr %6, align 4, !dbg !2231
  %13 = load ptr, ptr %12, align 4, !dbg !2231
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2231
  %15 = load ptr, ptr %14, align 4, !dbg !2231
  %16 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2231
  %17 = load ptr, ptr %4, align 4, !dbg !2231
  %18 = load ptr, ptr %6, align 4, !dbg !2231
  %19 = call arm_aapcs_vfpcc i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2231
  store i32 %19, ptr %9, align 4, !dbg !2231
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2233, metadata !DIExpression()), !dbg !2231
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2234, metadata !DIExpression()), !dbg !2236
  store i32 0, ptr %11, align 4, !dbg !2236
  br label %20, !dbg !2236

20:                                               ; preds = %99, %3
  %21 = load i32, ptr %11, align 4, !dbg !2236
  %22 = load i32, ptr %9, align 4, !dbg !2236
  %23 = icmp slt i32 %21, %22, !dbg !2236
  br i1 %23, label %24, label %102, !dbg !2236

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2237
  %26 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %25, !dbg !2237
  %27 = load i8, ptr %26, align 1, !dbg !2237
  %28 = sext i8 %27 to i32, !dbg !2237
  switch i32 %28, label %97 [
    i32 90, label %29
    i32 66, label %36
    i32 67, label %43
    i32 83, label %50
    i32 73, label %57
    i32 74, label %63
    i32 70, label %70
    i32 68, label %81
    i32 76, label %91
  ], !dbg !2237

29:                                               ; preds = %24
  %30 = load ptr, ptr %7, align 4, !dbg !2240
  %31 = getelementptr inbounds i8, ptr %30, i32 4, !dbg !2240
  store ptr %31, ptr %7, align 4, !dbg !2240
  %32 = load i32, ptr %30, align 4, !dbg !2240
  %33 = trunc i32 %32 to i8, !dbg !2240
  %34 = load i32, ptr %11, align 4, !dbg !2240
  %35 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %34, !dbg !2240
  store i8 %33, ptr %35, align 8, !dbg !2240
  br label %98, !dbg !2240

36:                                               ; preds = %24
  %37 = load ptr, ptr %7, align 4, !dbg !2240
  %38 = getelementptr inbounds i8, ptr %37, i32 4, !dbg !2240
  store ptr %38, ptr %7, align 4, !dbg !2240
  %39 = load i32, ptr %37, align 4, !dbg !2240
  %40 = trunc i32 %39 to i8, !dbg !2240
  %41 = load i32, ptr %11, align 4, !dbg !2240
  %42 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %41, !dbg !2240
  store i8 %40, ptr %42, align 8, !dbg !2240
  br label %98, !dbg !2240

43:                                               ; preds = %24
  %44 = load ptr, ptr %7, align 4, !dbg !2240
  %45 = getelementptr inbounds i8, ptr %44, i32 4, !dbg !2240
  store ptr %45, ptr %7, align 4, !dbg !2240
  %46 = load i32, ptr %44, align 4, !dbg !2240
  %47 = trunc i32 %46 to i16, !dbg !2240
  %48 = load i32, ptr %11, align 4, !dbg !2240
  %49 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %48, !dbg !2240
  store i16 %47, ptr %49, align 8, !dbg !2240
  br label %98, !dbg !2240

50:                                               ; preds = %24
  %51 = load ptr, ptr %7, align 4, !dbg !2240
  %52 = getelementptr inbounds i8, ptr %51, i32 4, !dbg !2240
  store ptr %52, ptr %7, align 4, !dbg !2240
  %53 = load i32, ptr %51, align 4, !dbg !2240
  %54 = trunc i32 %53 to i16, !dbg !2240
  %55 = load i32, ptr %11, align 4, !dbg !2240
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %55, !dbg !2240
  store i16 %54, ptr %56, align 8, !dbg !2240
  br label %98, !dbg !2240

57:                                               ; preds = %24
  %58 = load ptr, ptr %7, align 4, !dbg !2240
  %59 = getelementptr inbounds i8, ptr %58, i32 4, !dbg !2240
  store ptr %59, ptr %7, align 4, !dbg !2240
  %60 = load i32, ptr %58, align 4, !dbg !2240
  %61 = load i32, ptr %11, align 4, !dbg !2240
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %61, !dbg !2240
  store i32 %60, ptr %62, align 8, !dbg !2240
  br label %98, !dbg !2240

63:                                               ; preds = %24
  %64 = load ptr, ptr %7, align 4, !dbg !2240
  %65 = getelementptr inbounds i8, ptr %64, i32 4, !dbg !2240
  store ptr %65, ptr %7, align 4, !dbg !2240
  %66 = load i32, ptr %64, align 4, !dbg !2240
  %67 = sext i32 %66 to i64, !dbg !2240
  %68 = load i32, ptr %11, align 4, !dbg !2240
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %68, !dbg !2240
  store i64 %67, ptr %69, align 8, !dbg !2240
  br label %98, !dbg !2240

70:                                               ; preds = %24
  %71 = load ptr, ptr %7, align 4, !dbg !2240
  %72 = ptrtoint ptr %71 to i32, !dbg !2240
  %73 = add i32 %72, 7, !dbg !2240
  %74 = and i32 %73, -8, !dbg !2240
  %75 = inttoptr i32 %74 to ptr, !dbg !2240
  %76 = getelementptr inbounds i8, ptr %75, i32 8, !dbg !2240
  store ptr %76, ptr %7, align 4, !dbg !2240
  %77 = load double, ptr %75, align 8, !dbg !2240
  %78 = fptrunc double %77 to float, !dbg !2240
  %79 = load i32, ptr %11, align 4, !dbg !2240
  %80 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %79, !dbg !2240
  store float %78, ptr %80, align 8, !dbg !2240
  br label %98, !dbg !2240

81:                                               ; preds = %24
  %82 = load ptr, ptr %7, align 4, !dbg !2240
  %83 = ptrtoint ptr %82 to i32, !dbg !2240
  %84 = add i32 %83, 7, !dbg !2240
  %85 = and i32 %84, -8, !dbg !2240
  %86 = inttoptr i32 %85 to ptr, !dbg !2240
  %87 = getelementptr inbounds i8, ptr %86, i32 8, !dbg !2240
  store ptr %87, ptr %7, align 4, !dbg !2240
  %88 = load double, ptr %86, align 8, !dbg !2240
  %89 = load i32, ptr %11, align 4, !dbg !2240
  %90 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %89, !dbg !2240
  store double %88, ptr %90, align 8, !dbg !2240
  br label %98, !dbg !2240

91:                                               ; preds = %24
  %92 = load ptr, ptr %7, align 4, !dbg !2240
  %93 = getelementptr inbounds i8, ptr %92, i32 4, !dbg !2240
  store ptr %93, ptr %7, align 4, !dbg !2240
  %94 = load ptr, ptr %92, align 4, !dbg !2240
  %95 = load i32, ptr %11, align 4, !dbg !2240
  %96 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %95, !dbg !2240
  store ptr %94, ptr %96, align 8, !dbg !2240
  br label %98, !dbg !2240

97:                                               ; preds = %24
  br label %98, !dbg !2240

98:                                               ; preds = %97, %91, %81, %70, %63, %57, %50, %43, %36, %29
  br label %99, !dbg !2237

99:                                               ; preds = %98
  %100 = load i32, ptr %11, align 4, !dbg !2242
  %101 = add nsw i32 %100, 1, !dbg !2242
  store i32 %101, ptr %11, align 4, !dbg !2242
  br label %20, !dbg !2242, !llvm.loop !2243

102:                                              ; preds = %20
  %103 = load ptr, ptr %6, align 4, !dbg !2244
  %104 = load ptr, ptr %103, align 4, !dbg !2244
  %105 = getelementptr inbounds %struct.JNINativeInterface_, ptr %104, i32 0, i32 63, !dbg !2244
  %106 = load ptr, ptr %105, align 4, !dbg !2244
  %107 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2244
  %108 = load ptr, ptr %4, align 4, !dbg !2244
  %109 = load ptr, ptr %5, align 4, !dbg !2244
  %110 = load ptr, ptr %6, align 4, !dbg !2244
  call arm_aapcs_vfpcc void %106(ptr noundef %110, ptr noundef %109, ptr noundef %108, ptr noundef %107), !dbg !2244
  call void @llvm.va_end(ptr %7), !dbg !2245
  ret void, !dbg !2246
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2247 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2248, metadata !DIExpression()), !dbg !2249
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2250, metadata !DIExpression()), !dbg !2249
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2251, metadata !DIExpression()), !dbg !2249
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2252, metadata !DIExpression()), !dbg !2249
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2253, metadata !DIExpression()), !dbg !2254
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2255, metadata !DIExpression()), !dbg !2254
  %13 = load ptr, ptr %8, align 4, !dbg !2254
  %14 = load ptr, ptr %13, align 4, !dbg !2254
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2254
  %16 = load ptr, ptr %15, align 4, !dbg !2254
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2254
  %18 = load ptr, ptr %6, align 4, !dbg !2254
  %19 = load ptr, ptr %8, align 4, !dbg !2254
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2254
  store i32 %20, ptr %10, align 4, !dbg !2254
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2256, metadata !DIExpression()), !dbg !2254
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2257, metadata !DIExpression()), !dbg !2259
  store i32 0, ptr %12, align 4, !dbg !2259
  br label %21, !dbg !2259

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2259
  %23 = load i32, ptr %10, align 4, !dbg !2259
  %24 = icmp slt i32 %22, %23, !dbg !2259
  br i1 %24, label %25, label %103, !dbg !2259

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2260
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2260
  %28 = load i8, ptr %27, align 1, !dbg !2260
  %29 = sext i8 %28 to i32, !dbg !2260
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2260

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2263
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2263
  store ptr %32, ptr %5, align 4, !dbg !2263
  %33 = load i32, ptr %31, align 4, !dbg !2263
  %34 = trunc i32 %33 to i8, !dbg !2263
  %35 = load i32, ptr %12, align 4, !dbg !2263
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2263
  store i8 %34, ptr %36, align 8, !dbg !2263
  br label %99, !dbg !2263

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2263
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2263
  store ptr %39, ptr %5, align 4, !dbg !2263
  %40 = load i32, ptr %38, align 4, !dbg !2263
  %41 = trunc i32 %40 to i8, !dbg !2263
  %42 = load i32, ptr %12, align 4, !dbg !2263
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2263
  store i8 %41, ptr %43, align 8, !dbg !2263
  br label %99, !dbg !2263

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2263
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2263
  store ptr %46, ptr %5, align 4, !dbg !2263
  %47 = load i32, ptr %45, align 4, !dbg !2263
  %48 = trunc i32 %47 to i16, !dbg !2263
  %49 = load i32, ptr %12, align 4, !dbg !2263
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2263
  store i16 %48, ptr %50, align 8, !dbg !2263
  br label %99, !dbg !2263

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2263
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2263
  store ptr %53, ptr %5, align 4, !dbg !2263
  %54 = load i32, ptr %52, align 4, !dbg !2263
  %55 = trunc i32 %54 to i16, !dbg !2263
  %56 = load i32, ptr %12, align 4, !dbg !2263
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2263
  store i16 %55, ptr %57, align 8, !dbg !2263
  br label %99, !dbg !2263

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2263
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2263
  store ptr %60, ptr %5, align 4, !dbg !2263
  %61 = load i32, ptr %59, align 4, !dbg !2263
  %62 = load i32, ptr %12, align 4, !dbg !2263
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2263
  store i32 %61, ptr %63, align 8, !dbg !2263
  br label %99, !dbg !2263

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2263
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2263
  store ptr %66, ptr %5, align 4, !dbg !2263
  %67 = load i32, ptr %65, align 4, !dbg !2263
  %68 = sext i32 %67 to i64, !dbg !2263
  %69 = load i32, ptr %12, align 4, !dbg !2263
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2263
  store i64 %68, ptr %70, align 8, !dbg !2263
  br label %99, !dbg !2263

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2263
  %73 = ptrtoint ptr %72 to i32, !dbg !2263
  %74 = add i32 %73, 7, !dbg !2263
  %75 = and i32 %74, -8, !dbg !2263
  %76 = inttoptr i32 %75 to ptr, !dbg !2263
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2263
  store ptr %77, ptr %5, align 4, !dbg !2263
  %78 = load double, ptr %76, align 8, !dbg !2263
  %79 = fptrunc double %78 to float, !dbg !2263
  %80 = load i32, ptr %12, align 4, !dbg !2263
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2263
  store float %79, ptr %81, align 8, !dbg !2263
  br label %99, !dbg !2263

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2263
  %84 = ptrtoint ptr %83 to i32, !dbg !2263
  %85 = add i32 %84, 7, !dbg !2263
  %86 = and i32 %85, -8, !dbg !2263
  %87 = inttoptr i32 %86 to ptr, !dbg !2263
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2263
  store ptr %88, ptr %5, align 4, !dbg !2263
  %89 = load double, ptr %87, align 8, !dbg !2263
  %90 = load i32, ptr %12, align 4, !dbg !2263
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2263
  store double %89, ptr %91, align 8, !dbg !2263
  br label %99, !dbg !2263

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2263
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2263
  store ptr %94, ptr %5, align 4, !dbg !2263
  %95 = load ptr, ptr %93, align 4, !dbg !2263
  %96 = load i32, ptr %12, align 4, !dbg !2263
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2263
  store ptr %95, ptr %97, align 8, !dbg !2263
  br label %99, !dbg !2263

98:                                               ; preds = %25
  br label %99, !dbg !2263

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2260

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2265
  %102 = add nsw i32 %101, 1, !dbg !2265
  store i32 %102, ptr %12, align 4, !dbg !2265
  br label %21, !dbg !2265, !llvm.loop !2266

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2267
  %105 = load ptr, ptr %104, align 4, !dbg !2267
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 63, !dbg !2267
  %107 = load ptr, ptr %106, align 4, !dbg !2267
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2267
  %109 = load ptr, ptr %6, align 4, !dbg !2267
  %110 = load ptr, ptr %7, align 4, !dbg !2267
  %111 = load ptr, ptr %8, align 4, !dbg !2267
  call arm_aapcs_vfpcc void %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2267
  ret void, !dbg !2268
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2269 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2270, metadata !DIExpression()), !dbg !2271
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2272, metadata !DIExpression()), !dbg !2271
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2273, metadata !DIExpression()), !dbg !2271
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2274, metadata !DIExpression()), !dbg !2271
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2275, metadata !DIExpression()), !dbg !2276
  call void @llvm.va_start(ptr %9), !dbg !2277
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2278, metadata !DIExpression()), !dbg !2279
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2280, metadata !DIExpression()), !dbg !2279
  %14 = load ptr, ptr %8, align 4, !dbg !2279
  %15 = load ptr, ptr %14, align 4, !dbg !2279
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0, !dbg !2279
  %17 = load ptr, ptr %16, align 4, !dbg !2279
  %18 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !2279
  %19 = load ptr, ptr %5, align 4, !dbg !2279
  %20 = load ptr, ptr %8, align 4, !dbg !2279
  %21 = call arm_aapcs_vfpcc i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18), !dbg !2279
  store i32 %21, ptr %11, align 4, !dbg !2279
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2281, metadata !DIExpression()), !dbg !2279
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2282, metadata !DIExpression()), !dbg !2284
  store i32 0, ptr %13, align 4, !dbg !2284
  br label %22, !dbg !2284

22:                                               ; preds = %101, %4
  %23 = load i32, ptr %13, align 4, !dbg !2284
  %24 = load i32, ptr %11, align 4, !dbg !2284
  %25 = icmp slt i32 %23, %24, !dbg !2284
  br i1 %25, label %26, label %104, !dbg !2284

26:                                               ; preds = %22
  %27 = load i32, ptr %13, align 4, !dbg !2285
  %28 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %27, !dbg !2285
  %29 = load i8, ptr %28, align 1, !dbg !2285
  %30 = sext i8 %29 to i32, !dbg !2285
  switch i32 %30, label %99 [
    i32 90, label %31
    i32 66, label %38
    i32 67, label %45
    i32 83, label %52
    i32 73, label %59
    i32 74, label %65
    i32 70, label %72
    i32 68, label %83
    i32 76, label %93
  ], !dbg !2285

31:                                               ; preds = %26
  %32 = load ptr, ptr %9, align 4, !dbg !2288
  %33 = getelementptr inbounds i8, ptr %32, i32 4, !dbg !2288
  store ptr %33, ptr %9, align 4, !dbg !2288
  %34 = load i32, ptr %32, align 4, !dbg !2288
  %35 = trunc i32 %34 to i8, !dbg !2288
  %36 = load i32, ptr %13, align 4, !dbg !2288
  %37 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %36, !dbg !2288
  store i8 %35, ptr %37, align 8, !dbg !2288
  br label %100, !dbg !2288

38:                                               ; preds = %26
  %39 = load ptr, ptr %9, align 4, !dbg !2288
  %40 = getelementptr inbounds i8, ptr %39, i32 4, !dbg !2288
  store ptr %40, ptr %9, align 4, !dbg !2288
  %41 = load i32, ptr %39, align 4, !dbg !2288
  %42 = trunc i32 %41 to i8, !dbg !2288
  %43 = load i32, ptr %13, align 4, !dbg !2288
  %44 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %43, !dbg !2288
  store i8 %42, ptr %44, align 8, !dbg !2288
  br label %100, !dbg !2288

45:                                               ; preds = %26
  %46 = load ptr, ptr %9, align 4, !dbg !2288
  %47 = getelementptr inbounds i8, ptr %46, i32 4, !dbg !2288
  store ptr %47, ptr %9, align 4, !dbg !2288
  %48 = load i32, ptr %46, align 4, !dbg !2288
  %49 = trunc i32 %48 to i16, !dbg !2288
  %50 = load i32, ptr %13, align 4, !dbg !2288
  %51 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %50, !dbg !2288
  store i16 %49, ptr %51, align 8, !dbg !2288
  br label %100, !dbg !2288

52:                                               ; preds = %26
  %53 = load ptr, ptr %9, align 4, !dbg !2288
  %54 = getelementptr inbounds i8, ptr %53, i32 4, !dbg !2288
  store ptr %54, ptr %9, align 4, !dbg !2288
  %55 = load i32, ptr %53, align 4, !dbg !2288
  %56 = trunc i32 %55 to i16, !dbg !2288
  %57 = load i32, ptr %13, align 4, !dbg !2288
  %58 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %57, !dbg !2288
  store i16 %56, ptr %58, align 8, !dbg !2288
  br label %100, !dbg !2288

59:                                               ; preds = %26
  %60 = load ptr, ptr %9, align 4, !dbg !2288
  %61 = getelementptr inbounds i8, ptr %60, i32 4, !dbg !2288
  store ptr %61, ptr %9, align 4, !dbg !2288
  %62 = load i32, ptr %60, align 4, !dbg !2288
  %63 = load i32, ptr %13, align 4, !dbg !2288
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %63, !dbg !2288
  store i32 %62, ptr %64, align 8, !dbg !2288
  br label %100, !dbg !2288

65:                                               ; preds = %26
  %66 = load ptr, ptr %9, align 4, !dbg !2288
  %67 = getelementptr inbounds i8, ptr %66, i32 4, !dbg !2288
  store ptr %67, ptr %9, align 4, !dbg !2288
  %68 = load i32, ptr %66, align 4, !dbg !2288
  %69 = sext i32 %68 to i64, !dbg !2288
  %70 = load i32, ptr %13, align 4, !dbg !2288
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %70, !dbg !2288
  store i64 %69, ptr %71, align 8, !dbg !2288
  br label %100, !dbg !2288

72:                                               ; preds = %26
  %73 = load ptr, ptr %9, align 4, !dbg !2288
  %74 = ptrtoint ptr %73 to i32, !dbg !2288
  %75 = add i32 %74, 7, !dbg !2288
  %76 = and i32 %75, -8, !dbg !2288
  %77 = inttoptr i32 %76 to ptr, !dbg !2288
  %78 = getelementptr inbounds i8, ptr %77, i32 8, !dbg !2288
  store ptr %78, ptr %9, align 4, !dbg !2288
  %79 = load double, ptr %77, align 8, !dbg !2288
  %80 = fptrunc double %79 to float, !dbg !2288
  %81 = load i32, ptr %13, align 4, !dbg !2288
  %82 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %81, !dbg !2288
  store float %80, ptr %82, align 8, !dbg !2288
  br label %100, !dbg !2288

83:                                               ; preds = %26
  %84 = load ptr, ptr %9, align 4, !dbg !2288
  %85 = ptrtoint ptr %84 to i32, !dbg !2288
  %86 = add i32 %85, 7, !dbg !2288
  %87 = and i32 %86, -8, !dbg !2288
  %88 = inttoptr i32 %87 to ptr, !dbg !2288
  %89 = getelementptr inbounds i8, ptr %88, i32 8, !dbg !2288
  store ptr %89, ptr %9, align 4, !dbg !2288
  %90 = load double, ptr %88, align 8, !dbg !2288
  %91 = load i32, ptr %13, align 4, !dbg !2288
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %91, !dbg !2288
  store double %90, ptr %92, align 8, !dbg !2288
  br label %100, !dbg !2288

93:                                               ; preds = %26
  %94 = load ptr, ptr %9, align 4, !dbg !2288
  %95 = getelementptr inbounds i8, ptr %94, i32 4, !dbg !2288
  store ptr %95, ptr %9, align 4, !dbg !2288
  %96 = load ptr, ptr %94, align 4, !dbg !2288
  %97 = load i32, ptr %13, align 4, !dbg !2288
  %98 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %97, !dbg !2288
  store ptr %96, ptr %98, align 8, !dbg !2288
  br label %100, !dbg !2288

99:                                               ; preds = %26
  br label %100, !dbg !2288

100:                                              ; preds = %99, %93, %83, %72, %65, %59, %52, %45, %38, %31
  br label %101, !dbg !2285

101:                                              ; preds = %100
  %102 = load i32, ptr %13, align 4, !dbg !2290
  %103 = add nsw i32 %102, 1, !dbg !2290
  store i32 %103, ptr %13, align 4, !dbg !2290
  br label %22, !dbg !2290, !llvm.loop !2291

104:                                              ; preds = %22
  %105 = load ptr, ptr %8, align 4, !dbg !2292
  %106 = load ptr, ptr %105, align 4, !dbg !2292
  %107 = getelementptr inbounds %struct.JNINativeInterface_, ptr %106, i32 0, i32 93, !dbg !2292
  %108 = load ptr, ptr %107, align 4, !dbg !2292
  %109 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !2292
  %110 = load ptr, ptr %5, align 4, !dbg !2292
  %111 = load ptr, ptr %6, align 4, !dbg !2292
  %112 = load ptr, ptr %7, align 4, !dbg !2292
  %113 = load ptr, ptr %8, align 4, !dbg !2292
  call arm_aapcs_vfpcc void %108(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110, ptr noundef %109), !dbg !2292
  call void @llvm.va_end(ptr %9), !dbg !2293
  ret void, !dbg !2294
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2295 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca ptr, align 4
  %10 = alloca ptr, align 4
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2296, metadata !DIExpression()), !dbg !2297
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2298, metadata !DIExpression()), !dbg !2297
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2299, metadata !DIExpression()), !dbg !2297
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2300, metadata !DIExpression()), !dbg !2297
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2301, metadata !DIExpression()), !dbg !2297
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2302, metadata !DIExpression()), !dbg !2303
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2304, metadata !DIExpression()), !dbg !2303
  %15 = load ptr, ptr %10, align 4, !dbg !2303
  %16 = load ptr, ptr %15, align 4, !dbg !2303
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2303
  %18 = load ptr, ptr %17, align 4, !dbg !2303
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !2303
  %20 = load ptr, ptr %7, align 4, !dbg !2303
  %21 = load ptr, ptr %10, align 4, !dbg !2303
  %22 = call arm_aapcs_vfpcc i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2303
  store i32 %22, ptr %12, align 4, !dbg !2303
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2305, metadata !DIExpression()), !dbg !2303
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2306, metadata !DIExpression()), !dbg !2308
  store i32 0, ptr %14, align 4, !dbg !2308
  br label %23, !dbg !2308

23:                                               ; preds = %102, %5
  %24 = load i32, ptr %14, align 4, !dbg !2308
  %25 = load i32, ptr %12, align 4, !dbg !2308
  %26 = icmp slt i32 %24, %25, !dbg !2308
  br i1 %26, label %27, label %105, !dbg !2308

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2309
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2309
  %30 = load i8, ptr %29, align 1, !dbg !2309
  %31 = sext i8 %30 to i32, !dbg !2309
  switch i32 %31, label %100 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %84
    i32 76, label %94
  ], !dbg !2309

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2312
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2312
  store ptr %34, ptr %6, align 4, !dbg !2312
  %35 = load i32, ptr %33, align 4, !dbg !2312
  %36 = trunc i32 %35 to i8, !dbg !2312
  %37 = load i32, ptr %14, align 4, !dbg !2312
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2312
  store i8 %36, ptr %38, align 8, !dbg !2312
  br label %101, !dbg !2312

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2312
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2312
  store ptr %41, ptr %6, align 4, !dbg !2312
  %42 = load i32, ptr %40, align 4, !dbg !2312
  %43 = trunc i32 %42 to i8, !dbg !2312
  %44 = load i32, ptr %14, align 4, !dbg !2312
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2312
  store i8 %43, ptr %45, align 8, !dbg !2312
  br label %101, !dbg !2312

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2312
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2312
  store ptr %48, ptr %6, align 4, !dbg !2312
  %49 = load i32, ptr %47, align 4, !dbg !2312
  %50 = trunc i32 %49 to i16, !dbg !2312
  %51 = load i32, ptr %14, align 4, !dbg !2312
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2312
  store i16 %50, ptr %52, align 8, !dbg !2312
  br label %101, !dbg !2312

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2312
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2312
  store ptr %55, ptr %6, align 4, !dbg !2312
  %56 = load i32, ptr %54, align 4, !dbg !2312
  %57 = trunc i32 %56 to i16, !dbg !2312
  %58 = load i32, ptr %14, align 4, !dbg !2312
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2312
  store i16 %57, ptr %59, align 8, !dbg !2312
  br label %101, !dbg !2312

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2312
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2312
  store ptr %62, ptr %6, align 4, !dbg !2312
  %63 = load i32, ptr %61, align 4, !dbg !2312
  %64 = load i32, ptr %14, align 4, !dbg !2312
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2312
  store i32 %63, ptr %65, align 8, !dbg !2312
  br label %101, !dbg !2312

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2312
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2312
  store ptr %68, ptr %6, align 4, !dbg !2312
  %69 = load i32, ptr %67, align 4, !dbg !2312
  %70 = sext i32 %69 to i64, !dbg !2312
  %71 = load i32, ptr %14, align 4, !dbg !2312
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2312
  store i64 %70, ptr %72, align 8, !dbg !2312
  br label %101, !dbg !2312

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2312
  %75 = ptrtoint ptr %74 to i32, !dbg !2312
  %76 = add i32 %75, 7, !dbg !2312
  %77 = and i32 %76, -8, !dbg !2312
  %78 = inttoptr i32 %77 to ptr, !dbg !2312
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2312
  store ptr %79, ptr %6, align 4, !dbg !2312
  %80 = load double, ptr %78, align 8, !dbg !2312
  %81 = fptrunc double %80 to float, !dbg !2312
  %82 = load i32, ptr %14, align 4, !dbg !2312
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %82, !dbg !2312
  store float %81, ptr %83, align 8, !dbg !2312
  br label %101, !dbg !2312

84:                                               ; preds = %27
  %85 = load ptr, ptr %6, align 4, !dbg !2312
  %86 = ptrtoint ptr %85 to i32, !dbg !2312
  %87 = add i32 %86, 7, !dbg !2312
  %88 = and i32 %87, -8, !dbg !2312
  %89 = inttoptr i32 %88 to ptr, !dbg !2312
  %90 = getelementptr inbounds i8, ptr %89, i32 8, !dbg !2312
  store ptr %90, ptr %6, align 4, !dbg !2312
  %91 = load double, ptr %89, align 8, !dbg !2312
  %92 = load i32, ptr %14, align 4, !dbg !2312
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %92, !dbg !2312
  store double %91, ptr %93, align 8, !dbg !2312
  br label %101, !dbg !2312

94:                                               ; preds = %27
  %95 = load ptr, ptr %6, align 4, !dbg !2312
  %96 = getelementptr inbounds i8, ptr %95, i32 4, !dbg !2312
  store ptr %96, ptr %6, align 4, !dbg !2312
  %97 = load ptr, ptr %95, align 4, !dbg !2312
  %98 = load i32, ptr %14, align 4, !dbg !2312
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %98, !dbg !2312
  store ptr %97, ptr %99, align 8, !dbg !2312
  br label %101, !dbg !2312

100:                                              ; preds = %27
  br label %101, !dbg !2312

101:                                              ; preds = %100, %94, %84, %73, %66, %60, %53, %46, %39, %32
  br label %102, !dbg !2309

102:                                              ; preds = %101
  %103 = load i32, ptr %14, align 4, !dbg !2314
  %104 = add nsw i32 %103, 1, !dbg !2314
  store i32 %104, ptr %14, align 4, !dbg !2314
  br label %23, !dbg !2314, !llvm.loop !2315

105:                                              ; preds = %23
  %106 = load ptr, ptr %10, align 4, !dbg !2316
  %107 = load ptr, ptr %106, align 4, !dbg !2316
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 93, !dbg !2316
  %109 = load ptr, ptr %108, align 4, !dbg !2316
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !2316
  %111 = load ptr, ptr %7, align 4, !dbg !2316
  %112 = load ptr, ptr %8, align 4, !dbg !2316
  %113 = load ptr, ptr %9, align 4, !dbg !2316
  %114 = load ptr, ptr %10, align 4, !dbg !2316
  call arm_aapcs_vfpcc void %109(ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2316
  ret void, !dbg !2317
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2318 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2319, metadata !DIExpression()), !dbg !2320
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2321, metadata !DIExpression()), !dbg !2320
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2322, metadata !DIExpression()), !dbg !2320
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2323, metadata !DIExpression()), !dbg !2324
  call void @llvm.va_start(ptr %7), !dbg !2325
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2326, metadata !DIExpression()), !dbg !2327
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2328, metadata !DIExpression()), !dbg !2327
  %12 = load ptr, ptr %6, align 4, !dbg !2327
  %13 = load ptr, ptr %12, align 4, !dbg !2327
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2327
  %15 = load ptr, ptr %14, align 4, !dbg !2327
  %16 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2327
  %17 = load ptr, ptr %4, align 4, !dbg !2327
  %18 = load ptr, ptr %6, align 4, !dbg !2327
  %19 = call arm_aapcs_vfpcc i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2327
  store i32 %19, ptr %9, align 4, !dbg !2327
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2329, metadata !DIExpression()), !dbg !2327
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2330, metadata !DIExpression()), !dbg !2332
  store i32 0, ptr %11, align 4, !dbg !2332
  br label %20, !dbg !2332

20:                                               ; preds = %99, %3
  %21 = load i32, ptr %11, align 4, !dbg !2332
  %22 = load i32, ptr %9, align 4, !dbg !2332
  %23 = icmp slt i32 %21, %22, !dbg !2332
  br i1 %23, label %24, label %102, !dbg !2332

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2333
  %26 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %25, !dbg !2333
  %27 = load i8, ptr %26, align 1, !dbg !2333
  %28 = sext i8 %27 to i32, !dbg !2333
  switch i32 %28, label %97 [
    i32 90, label %29
    i32 66, label %36
    i32 67, label %43
    i32 83, label %50
    i32 73, label %57
    i32 74, label %63
    i32 70, label %70
    i32 68, label %81
    i32 76, label %91
  ], !dbg !2333

29:                                               ; preds = %24
  %30 = load ptr, ptr %7, align 4, !dbg !2336
  %31 = getelementptr inbounds i8, ptr %30, i32 4, !dbg !2336
  store ptr %31, ptr %7, align 4, !dbg !2336
  %32 = load i32, ptr %30, align 4, !dbg !2336
  %33 = trunc i32 %32 to i8, !dbg !2336
  %34 = load i32, ptr %11, align 4, !dbg !2336
  %35 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %34, !dbg !2336
  store i8 %33, ptr %35, align 8, !dbg !2336
  br label %98, !dbg !2336

36:                                               ; preds = %24
  %37 = load ptr, ptr %7, align 4, !dbg !2336
  %38 = getelementptr inbounds i8, ptr %37, i32 4, !dbg !2336
  store ptr %38, ptr %7, align 4, !dbg !2336
  %39 = load i32, ptr %37, align 4, !dbg !2336
  %40 = trunc i32 %39 to i8, !dbg !2336
  %41 = load i32, ptr %11, align 4, !dbg !2336
  %42 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %41, !dbg !2336
  store i8 %40, ptr %42, align 8, !dbg !2336
  br label %98, !dbg !2336

43:                                               ; preds = %24
  %44 = load ptr, ptr %7, align 4, !dbg !2336
  %45 = getelementptr inbounds i8, ptr %44, i32 4, !dbg !2336
  store ptr %45, ptr %7, align 4, !dbg !2336
  %46 = load i32, ptr %44, align 4, !dbg !2336
  %47 = trunc i32 %46 to i16, !dbg !2336
  %48 = load i32, ptr %11, align 4, !dbg !2336
  %49 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %48, !dbg !2336
  store i16 %47, ptr %49, align 8, !dbg !2336
  br label %98, !dbg !2336

50:                                               ; preds = %24
  %51 = load ptr, ptr %7, align 4, !dbg !2336
  %52 = getelementptr inbounds i8, ptr %51, i32 4, !dbg !2336
  store ptr %52, ptr %7, align 4, !dbg !2336
  %53 = load i32, ptr %51, align 4, !dbg !2336
  %54 = trunc i32 %53 to i16, !dbg !2336
  %55 = load i32, ptr %11, align 4, !dbg !2336
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %55, !dbg !2336
  store i16 %54, ptr %56, align 8, !dbg !2336
  br label %98, !dbg !2336

57:                                               ; preds = %24
  %58 = load ptr, ptr %7, align 4, !dbg !2336
  %59 = getelementptr inbounds i8, ptr %58, i32 4, !dbg !2336
  store ptr %59, ptr %7, align 4, !dbg !2336
  %60 = load i32, ptr %58, align 4, !dbg !2336
  %61 = load i32, ptr %11, align 4, !dbg !2336
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %61, !dbg !2336
  store i32 %60, ptr %62, align 8, !dbg !2336
  br label %98, !dbg !2336

63:                                               ; preds = %24
  %64 = load ptr, ptr %7, align 4, !dbg !2336
  %65 = getelementptr inbounds i8, ptr %64, i32 4, !dbg !2336
  store ptr %65, ptr %7, align 4, !dbg !2336
  %66 = load i32, ptr %64, align 4, !dbg !2336
  %67 = sext i32 %66 to i64, !dbg !2336
  %68 = load i32, ptr %11, align 4, !dbg !2336
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %68, !dbg !2336
  store i64 %67, ptr %69, align 8, !dbg !2336
  br label %98, !dbg !2336

70:                                               ; preds = %24
  %71 = load ptr, ptr %7, align 4, !dbg !2336
  %72 = ptrtoint ptr %71 to i32, !dbg !2336
  %73 = add i32 %72, 7, !dbg !2336
  %74 = and i32 %73, -8, !dbg !2336
  %75 = inttoptr i32 %74 to ptr, !dbg !2336
  %76 = getelementptr inbounds i8, ptr %75, i32 8, !dbg !2336
  store ptr %76, ptr %7, align 4, !dbg !2336
  %77 = load double, ptr %75, align 8, !dbg !2336
  %78 = fptrunc double %77 to float, !dbg !2336
  %79 = load i32, ptr %11, align 4, !dbg !2336
  %80 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %79, !dbg !2336
  store float %78, ptr %80, align 8, !dbg !2336
  br label %98, !dbg !2336

81:                                               ; preds = %24
  %82 = load ptr, ptr %7, align 4, !dbg !2336
  %83 = ptrtoint ptr %82 to i32, !dbg !2336
  %84 = add i32 %83, 7, !dbg !2336
  %85 = and i32 %84, -8, !dbg !2336
  %86 = inttoptr i32 %85 to ptr, !dbg !2336
  %87 = getelementptr inbounds i8, ptr %86, i32 8, !dbg !2336
  store ptr %87, ptr %7, align 4, !dbg !2336
  %88 = load double, ptr %86, align 8, !dbg !2336
  %89 = load i32, ptr %11, align 4, !dbg !2336
  %90 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %89, !dbg !2336
  store double %88, ptr %90, align 8, !dbg !2336
  br label %98, !dbg !2336

91:                                               ; preds = %24
  %92 = load ptr, ptr %7, align 4, !dbg !2336
  %93 = getelementptr inbounds i8, ptr %92, i32 4, !dbg !2336
  store ptr %93, ptr %7, align 4, !dbg !2336
  %94 = load ptr, ptr %92, align 4, !dbg !2336
  %95 = load i32, ptr %11, align 4, !dbg !2336
  %96 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %95, !dbg !2336
  store ptr %94, ptr %96, align 8, !dbg !2336
  br label %98, !dbg !2336

97:                                               ; preds = %24
  br label %98, !dbg !2336

98:                                               ; preds = %97, %91, %81, %70, %63, %57, %50, %43, %36, %29
  br label %99, !dbg !2333

99:                                               ; preds = %98
  %100 = load i32, ptr %11, align 4, !dbg !2338
  %101 = add nsw i32 %100, 1, !dbg !2338
  store i32 %101, ptr %11, align 4, !dbg !2338
  br label %20, !dbg !2338, !llvm.loop !2339

102:                                              ; preds = %20
  %103 = load ptr, ptr %6, align 4, !dbg !2340
  %104 = load ptr, ptr %103, align 4, !dbg !2340
  %105 = getelementptr inbounds %struct.JNINativeInterface_, ptr %104, i32 0, i32 143, !dbg !2340
  %106 = load ptr, ptr %105, align 4, !dbg !2340
  %107 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2340
  %108 = load ptr, ptr %4, align 4, !dbg !2340
  %109 = load ptr, ptr %5, align 4, !dbg !2340
  %110 = load ptr, ptr %6, align 4, !dbg !2340
  call arm_aapcs_vfpcc void %106(ptr noundef %110, ptr noundef %109, ptr noundef %108, ptr noundef %107), !dbg !2340
  call void @llvm.va_end(ptr %7), !dbg !2341
  ret void, !dbg !2342
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2343 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2344, metadata !DIExpression()), !dbg !2345
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2346, metadata !DIExpression()), !dbg !2345
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2347, metadata !DIExpression()), !dbg !2345
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2348, metadata !DIExpression()), !dbg !2345
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2349, metadata !DIExpression()), !dbg !2350
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2351, metadata !DIExpression()), !dbg !2350
  %13 = load ptr, ptr %8, align 4, !dbg !2350
  %14 = load ptr, ptr %13, align 4, !dbg !2350
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2350
  %16 = load ptr, ptr %15, align 4, !dbg !2350
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2350
  %18 = load ptr, ptr %6, align 4, !dbg !2350
  %19 = load ptr, ptr %8, align 4, !dbg !2350
  %20 = call arm_aapcs_vfpcc i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2350
  store i32 %20, ptr %10, align 4, !dbg !2350
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2352, metadata !DIExpression()), !dbg !2350
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2353, metadata !DIExpression()), !dbg !2355
  store i32 0, ptr %12, align 4, !dbg !2355
  br label %21, !dbg !2355

21:                                               ; preds = %100, %4
  %22 = load i32, ptr %12, align 4, !dbg !2355
  %23 = load i32, ptr %10, align 4, !dbg !2355
  %24 = icmp slt i32 %22, %23, !dbg !2355
  br i1 %24, label %25, label %103, !dbg !2355

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2356
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2356
  %28 = load i8, ptr %27, align 1, !dbg !2356
  %29 = sext i8 %28 to i32, !dbg !2356
  switch i32 %29, label %98 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %82
    i32 76, label %92
  ], !dbg !2356

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2359
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2359
  store ptr %32, ptr %5, align 4, !dbg !2359
  %33 = load i32, ptr %31, align 4, !dbg !2359
  %34 = trunc i32 %33 to i8, !dbg !2359
  %35 = load i32, ptr %12, align 4, !dbg !2359
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2359
  store i8 %34, ptr %36, align 8, !dbg !2359
  br label %99, !dbg !2359

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2359
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2359
  store ptr %39, ptr %5, align 4, !dbg !2359
  %40 = load i32, ptr %38, align 4, !dbg !2359
  %41 = trunc i32 %40 to i8, !dbg !2359
  %42 = load i32, ptr %12, align 4, !dbg !2359
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2359
  store i8 %41, ptr %43, align 8, !dbg !2359
  br label %99, !dbg !2359

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2359
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2359
  store ptr %46, ptr %5, align 4, !dbg !2359
  %47 = load i32, ptr %45, align 4, !dbg !2359
  %48 = trunc i32 %47 to i16, !dbg !2359
  %49 = load i32, ptr %12, align 4, !dbg !2359
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2359
  store i16 %48, ptr %50, align 8, !dbg !2359
  br label %99, !dbg !2359

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2359
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2359
  store ptr %53, ptr %5, align 4, !dbg !2359
  %54 = load i32, ptr %52, align 4, !dbg !2359
  %55 = trunc i32 %54 to i16, !dbg !2359
  %56 = load i32, ptr %12, align 4, !dbg !2359
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2359
  store i16 %55, ptr %57, align 8, !dbg !2359
  br label %99, !dbg !2359

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2359
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2359
  store ptr %60, ptr %5, align 4, !dbg !2359
  %61 = load i32, ptr %59, align 4, !dbg !2359
  %62 = load i32, ptr %12, align 4, !dbg !2359
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2359
  store i32 %61, ptr %63, align 8, !dbg !2359
  br label %99, !dbg !2359

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2359
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2359
  store ptr %66, ptr %5, align 4, !dbg !2359
  %67 = load i32, ptr %65, align 4, !dbg !2359
  %68 = sext i32 %67 to i64, !dbg !2359
  %69 = load i32, ptr %12, align 4, !dbg !2359
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2359
  store i64 %68, ptr %70, align 8, !dbg !2359
  br label %99, !dbg !2359

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2359
  %73 = ptrtoint ptr %72 to i32, !dbg !2359
  %74 = add i32 %73, 7, !dbg !2359
  %75 = and i32 %74, -8, !dbg !2359
  %76 = inttoptr i32 %75 to ptr, !dbg !2359
  %77 = getelementptr inbounds i8, ptr %76, i32 8, !dbg !2359
  store ptr %77, ptr %5, align 4, !dbg !2359
  %78 = load double, ptr %76, align 8, !dbg !2359
  %79 = fptrunc double %78 to float, !dbg !2359
  %80 = load i32, ptr %12, align 4, !dbg !2359
  %81 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %80, !dbg !2359
  store float %79, ptr %81, align 8, !dbg !2359
  br label %99, !dbg !2359

82:                                               ; preds = %25
  %83 = load ptr, ptr %5, align 4, !dbg !2359
  %84 = ptrtoint ptr %83 to i32, !dbg !2359
  %85 = add i32 %84, 7, !dbg !2359
  %86 = and i32 %85, -8, !dbg !2359
  %87 = inttoptr i32 %86 to ptr, !dbg !2359
  %88 = getelementptr inbounds i8, ptr %87, i32 8, !dbg !2359
  store ptr %88, ptr %5, align 4, !dbg !2359
  %89 = load double, ptr %87, align 8, !dbg !2359
  %90 = load i32, ptr %12, align 4, !dbg !2359
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %90, !dbg !2359
  store double %89, ptr %91, align 8, !dbg !2359
  br label %99, !dbg !2359

92:                                               ; preds = %25
  %93 = load ptr, ptr %5, align 4, !dbg !2359
  %94 = getelementptr inbounds i8, ptr %93, i32 4, !dbg !2359
  store ptr %94, ptr %5, align 4, !dbg !2359
  %95 = load ptr, ptr %93, align 4, !dbg !2359
  %96 = load i32, ptr %12, align 4, !dbg !2359
  %97 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %96, !dbg !2359
  store ptr %95, ptr %97, align 8, !dbg !2359
  br label %99, !dbg !2359

98:                                               ; preds = %25
  br label %99, !dbg !2359

99:                                               ; preds = %98, %92, %82, %71, %64, %58, %51, %44, %37, %30
  br label %100, !dbg !2356

100:                                              ; preds = %99
  %101 = load i32, ptr %12, align 4, !dbg !2361
  %102 = add nsw i32 %101, 1, !dbg !2361
  store i32 %102, ptr %12, align 4, !dbg !2361
  br label %21, !dbg !2361, !llvm.loop !2362

103:                                              ; preds = %21
  %104 = load ptr, ptr %8, align 4, !dbg !2363
  %105 = load ptr, ptr %104, align 4, !dbg !2363
  %106 = getelementptr inbounds %struct.JNINativeInterface_, ptr %105, i32 0, i32 143, !dbg !2363
  %107 = load ptr, ptr %106, align 4, !dbg !2363
  %108 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2363
  %109 = load ptr, ptr %6, align 4, !dbg !2363
  %110 = load ptr, ptr %7, align 4, !dbg !2363
  %111 = load ptr, ptr %8, align 4, !dbg !2363
  call arm_aapcs_vfpcc void %107(ptr noundef %111, ptr noundef %110, ptr noundef %109, ptr noundef %108), !dbg !2363
  ret void, !dbg !2364
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !2365 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2381, metadata !DIExpression()), !dbg !2382
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2383, metadata !DIExpression()), !dbg !2384
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2385, metadata !DIExpression()), !dbg !2386
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2387, metadata !DIExpression()), !dbg !2388
  %9 = load ptr, ptr %5, align 4, !dbg !2389
  %10 = load ptr, ptr %6, align 4, !dbg !2389
  %11 = load ptr, ptr %7, align 4, !dbg !2389
  %12 = load ptr, ptr %8, align 4, !dbg !2389
  %13 = call arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %12, i32 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !2389
  ret i32 %13, !dbg !2389
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc i32 @_vsnprintf_l(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat !dbg !2390 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i32, align 4
  %10 = alloca ptr, align 4
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2393, metadata !DIExpression()), !dbg !2394
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2395, metadata !DIExpression()), !dbg !2396
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2397, metadata !DIExpression()), !dbg !2398
  store i32 %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2399, metadata !DIExpression()), !dbg !2400
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2401, metadata !DIExpression()), !dbg !2402
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2403, metadata !DIExpression()), !dbg !2405
  %12 = load ptr, ptr %6, align 4, !dbg !2405
  %13 = load ptr, ptr %7, align 4, !dbg !2405
  %14 = load ptr, ptr %8, align 4, !dbg !2405
  %15 = load i32, ptr %9, align 4, !dbg !2405
  %16 = load ptr, ptr %10, align 4, !dbg !2405
  %17 = call arm_aapcs_vfpcc ptr @__local_stdio_printf_options(), !dbg !2405
  %18 = load i64, ptr %17, align 8, !dbg !2405
  %19 = or i64 %18, 1, !dbg !2405
  %20 = call arm_aapcs_vfpcc i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i32 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12), !dbg !2405
  store i32 %20, ptr %11, align 4, !dbg !2405
  %21 = load i32, ptr %11, align 4, !dbg !2406
  %22 = icmp slt i32 %21, 0, !dbg !2406
  br i1 %22, label %23, label %24, !dbg !2406

23:                                               ; preds = %5
  br label %26, !dbg !2406

24:                                               ; preds = %5
  %25 = load i32, ptr %11, align 4, !dbg !2406
  br label %26, !dbg !2406

26:                                               ; preds = %24, %23
  %27 = phi i32 [ -1, %23 ], [ %25, %24 ], !dbg !2406
  ret i32 %27, !dbg !2406
}

declare dso_local arm_aapcs_vfpcc i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i32 noundef, ptr noundef, ptr noundef, ptr noundef) #3

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local arm_aapcs_vfpcc ptr @__local_stdio_printf_options() #0 comdat !dbg !2 {
  ret ptr @__local_stdio_printf_options._OptionsStorage, !dbg !2407
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { nocallback nofree nosync nounwind readnone speculatable willreturn }
attributes #2 = { nocallback nofree nosync nounwind willreturn }
attributes #3 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

!llvm.dbg.cu = !{!8}
!llvm.module.flags = !{!1034, !1035, !1036, !1037, !1038, !1039}
!llvm.ident = !{!1040}

!0 = !DIGlobalVariableExpression(var: !1, expr: !DIExpression())
!1 = distinct !DIGlobalVariable(name: "_OptionsStorage", scope: !2, file: !3, line: 91, type: !7, isLocal: true, isDefinition: true)
!2 = distinct !DISubprogram(name: "__local_stdio_printf_options", scope: !3, file: !3, line: 89, type: !4, scopeLine: 90, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!3 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt_stdio_config.h", directory: "", checksumkind: CSK_MD5, checksum: "dacf907bda504afb0b64f53a242bdae6")
!4 = !DISubroutineType(types: !5)
!5 = !{!6}
!6 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !7, size: 32)
!7 = !DIBasicType(name: "unsigned long long", size: 64, encoding: DW_ATE_unsigned)
!8 = distinct !DICompileUnit(language: DW_LANG_C99, file: !9, producer: "clang version 15.0.2", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, enums: !10, retainedTypes: !19, globals: !1032, splitDebugInlining: false, nameTableKind: None)
!9 = !DIFile(filename: "jni.c", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "8ccde5e6b3790ecbde9190016dbdf76e")
!10 = !{!11}
!11 = !DICompositeType(tag: DW_TAG_enumeration_type, name: "_jobjectType", file: !12, line: 139, baseType: !13, size: 32, elements: !14)
!12 = !DIFile(filename: "..\\..\\..\\openjdk\\jdk\\src\\share\\javavm\\export\\jni.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "7756575af5344f8caa05083993b01fbd")
!13 = !DIBasicType(name: "int", size: 32, encoding: DW_ATE_signed)
!14 = !{!15, !16, !17, !18}
!15 = !DIEnumerator(name: "JNIInvalidRefType", value: 0)
!16 = !DIEnumerator(name: "JNILocalRefType", value: 1)
!17 = !DIEnumerator(name: "JNIGlobalRefType", value: 2)
!18 = !DIEnumerator(name: "JNIWeakGlobalRefType", value: 3)
!19 = !{!20, !81, !56, !164, !167, !40, !171, !174, !177, !48, !1029}
!20 = !DIDerivedType(tag: DW_TAG_typedef, name: "GetMethodArgs_t", file: !21, line: 21, baseType: !22)
!21 = !DIFile(filename: "./jni.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "8409ac17d13b2da8dd4fd576a3178fb7")
!22 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !23, size: 32)
!23 = !DISubroutineType(types: !24)
!24 = !{!13, !25, !67, !151}
!25 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !26, size: 32)
!26 = !DIDerivedType(tag: DW_TAG_typedef, name: "JNIEnv", file: !12, line: 197, baseType: !27)
!27 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !28, size: 32)
!28 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !29)
!29 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNINativeInterface_", file: !12, line: 214, size: 7456, elements: !30)
!30 = !{!31, !33, !34, !35, !36, !43, !59, !63, !70, !77, !83, !87, !91, !95, !100, !104, !108, !112, !113, !117, !121, !125, !126, !130, !131, !135, !136, !137, !141, !145, !152, !180, !184, !188, !192, !196, !200, !204, !208, !212, !216, !220, !224, !228, !232, !236, !240, !244, !248, !252, !256, !260, !264, !268, !272, !276, !280, !284, !288, !292, !296, !300, !304, !308, !312, !316, !320, !324, !328, !332, !336, !340, !344, !348, !352, !356, !360, !364, !368, !372, !376, !380, !384, !388, !392, !396, !400, !404, !408, !412, !416, !420, !424, !428, !432, !436, !440, !444, !448, !452, !456, !460, !464, !468, !472, !476, !480, !484, !488, !492, !496, !500, !504, !508, !509, !510, !511, !512, !516, !520, !524, !528, !532, !536, !540, !544, !548, !552, !556, !560, !564, !568, !572, !576, !580, !584, !588, !592, !596, !600, !604, !608, !612, !616, !620, !621, !625, !629, !633, !637, !641, !645, !649, !653, !657, !661, !665, !669, !673, !677, !681, !685, !689, !693, !700, !704, !709, !713, !717, !718, !722, !726, !731, !736, !740, !744, !749, !754, !759, !764, !769, !774, !779, !784, !788, !793, !798, !803, !808, !813, !818, !823, !827, !831, !835, !839, !843, !847, !851, !855, !859, !863, !867, !871, !875, !879, !883, !887, !893, !897, !901, !907, !913, !919, !925, !931, !943, !947, !951, !952, !981, !985, !989, !993, !997, !998, !999, !1004, !1008, !1012, !1016, !1020, !1024}
!31 = !DIDerivedType(tag: DW_TAG_member, name: "reserved0", scope: !29, file: !12, line: 215, baseType: !32, size: 32)
!32 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: null, size: 32)
!33 = !DIDerivedType(tag: DW_TAG_member, name: "reserved1", scope: !29, file: !12, line: 216, baseType: !32, size: 32, offset: 32)
!34 = !DIDerivedType(tag: DW_TAG_member, name: "reserved2", scope: !29, file: !12, line: 217, baseType: !32, size: 32, offset: 64)
!35 = !DIDerivedType(tag: DW_TAG_member, name: "reserved3", scope: !29, file: !12, line: 219, baseType: !32, size: 32, offset: 96)
!36 = !DIDerivedType(tag: DW_TAG_member, name: "GetVersion", scope: !29, file: !12, line: 220, baseType: !37, size: 32, offset: 128)
!37 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !38, size: 32)
!38 = !DISubroutineType(types: !39)
!39 = !{!40, !25}
!40 = !DIDerivedType(tag: DW_TAG_typedef, name: "jint", file: !41, line: 33, baseType: !42)
!41 = !DIFile(filename: "..\\..\\..\\openjdk\\jdk\\src\\windows\\javavm\\export\\jni_md.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "1ea1808175ba5b9740cb94cde3a9f925")
!42 = !DIBasicType(name: "long", size: 32, encoding: DW_ATE_signed)
!43 = !DIDerivedType(tag: DW_TAG_member, name: "DefineClass", scope: !29, file: !12, line: 222, baseType: !44, size: 32, offset: 160)
!44 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !45, size: 32)
!45 = !DISubroutineType(types: !46)
!46 = !{!47, !25, !51, !48, !54, !58}
!47 = !DIDerivedType(tag: DW_TAG_typedef, name: "jclass", file: !12, line: 102, baseType: !48)
!48 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobject", file: !12, line: 101, baseType: !49)
!49 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !50, size: 32)
!50 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jobject", file: !12, line: 99, flags: DIFlagFwdDecl)
!51 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !52, size: 32)
!52 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !53)
!53 = !DIBasicType(name: "char", size: 8, encoding: DW_ATE_signed_char)
!54 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !55, size: 32)
!55 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !56)
!56 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbyte", file: !41, line: 35, baseType: !57)
!57 = !DIBasicType(name: "signed char", size: 8, encoding: DW_ATE_signed_char)
!58 = !DIDerivedType(tag: DW_TAG_typedef, name: "jsize", file: !12, line: 63, baseType: !40)
!59 = !DIDerivedType(tag: DW_TAG_member, name: "FindClass", scope: !29, file: !12, line: 225, baseType: !60, size: 32, offset: 192)
!60 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !61, size: 32)
!61 = !DISubroutineType(types: !62)
!62 = !{!47, !25, !51}
!63 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedMethod", scope: !29, file: !12, line: 228, baseType: !64, size: 32, offset: 224)
!64 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !65, size: 32)
!65 = !DISubroutineType(types: !66)
!66 = !{!67, !25, !48}
!67 = !DIDerivedType(tag: DW_TAG_typedef, name: "jmethodID", file: !12, line: 136, baseType: !68)
!68 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !69, size: 32)
!69 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jmethodID", file: !12, line: 135, flags: DIFlagFwdDecl)
!70 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedField", scope: !29, file: !12, line: 230, baseType: !71, size: 32, offset: 256)
!71 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !72, size: 32)
!72 = !DISubroutineType(types: !73)
!73 = !{!74, !25, !48}
!74 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfieldID", file: !12, line: 133, baseType: !75)
!75 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !76, size: 32)
!76 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jfieldID", file: !12, line: 132, flags: DIFlagFwdDecl)
!77 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedMethod", scope: !29, file: !12, line: 233, baseType: !78, size: 32, offset: 288)
!78 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !79, size: 32)
!79 = !DISubroutineType(types: !80)
!80 = !{!48, !25, !47, !67, !81}
!81 = !DIDerivedType(tag: DW_TAG_typedef, name: "jboolean", file: !12, line: 57, baseType: !82)
!82 = !DIBasicType(name: "unsigned char", size: 8, encoding: DW_ATE_unsigned_char)
!83 = !DIDerivedType(tag: DW_TAG_member, name: "GetSuperclass", scope: !29, file: !12, line: 236, baseType: !84, size: 32, offset: 320)
!84 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !85, size: 32)
!85 = !DISubroutineType(types: !86)
!86 = !{!47, !25, !47}
!87 = !DIDerivedType(tag: DW_TAG_member, name: "IsAssignableFrom", scope: !29, file: !12, line: 238, baseType: !88, size: 32, offset: 352)
!88 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !89, size: 32)
!89 = !DISubroutineType(types: !90)
!90 = !{!81, !25, !47, !47}
!91 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedField", scope: !29, file: !12, line: 241, baseType: !92, size: 32, offset: 384)
!92 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !93, size: 32)
!93 = !DISubroutineType(types: !94)
!94 = !{!48, !25, !47, !74, !81}
!95 = !DIDerivedType(tag: DW_TAG_member, name: "Throw", scope: !29, file: !12, line: 244, baseType: !96, size: 32, offset: 416)
!96 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !97, size: 32)
!97 = !DISubroutineType(types: !98)
!98 = !{!40, !25, !99}
!99 = !DIDerivedType(tag: DW_TAG_typedef, name: "jthrowable", file: !12, line: 103, baseType: !48)
!100 = !DIDerivedType(tag: DW_TAG_member, name: "ThrowNew", scope: !29, file: !12, line: 246, baseType: !101, size: 32, offset: 448)
!101 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !102, size: 32)
!102 = !DISubroutineType(types: !103)
!103 = !{!40, !25, !47, !51}
!104 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionOccurred", scope: !29, file: !12, line: 248, baseType: !105, size: 32, offset: 480)
!105 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !106, size: 32)
!106 = !DISubroutineType(types: !107)
!107 = !{!99, !25}
!108 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionDescribe", scope: !29, file: !12, line: 250, baseType: !109, size: 32, offset: 512)
!109 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !110, size: 32)
!110 = !DISubroutineType(types: !111)
!111 = !{null, !25}
!112 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionClear", scope: !29, file: !12, line: 252, baseType: !109, size: 32, offset: 544)
!113 = !DIDerivedType(tag: DW_TAG_member, name: "FatalError", scope: !29, file: !12, line: 254, baseType: !114, size: 32, offset: 576)
!114 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !115, size: 32)
!115 = !DISubroutineType(types: !116)
!116 = !{null, !25, !51}
!117 = !DIDerivedType(tag: DW_TAG_member, name: "PushLocalFrame", scope: !29, file: !12, line: 257, baseType: !118, size: 32, offset: 608)
!118 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !119, size: 32)
!119 = !DISubroutineType(types: !120)
!120 = !{!40, !25, !40}
!121 = !DIDerivedType(tag: DW_TAG_member, name: "PopLocalFrame", scope: !29, file: !12, line: 259, baseType: !122, size: 32, offset: 640)
!122 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !123, size: 32)
!123 = !DISubroutineType(types: !124)
!124 = !{!48, !25, !48}
!125 = !DIDerivedType(tag: DW_TAG_member, name: "NewGlobalRef", scope: !29, file: !12, line: 262, baseType: !122, size: 32, offset: 672)
!126 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteGlobalRef", scope: !29, file: !12, line: 264, baseType: !127, size: 32, offset: 704)
!127 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !128, size: 32)
!128 = !DISubroutineType(types: !129)
!129 = !{null, !25, !48}
!130 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteLocalRef", scope: !29, file: !12, line: 266, baseType: !127, size: 32, offset: 736)
!131 = !DIDerivedType(tag: DW_TAG_member, name: "IsSameObject", scope: !29, file: !12, line: 268, baseType: !132, size: 32, offset: 768)
!132 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !133, size: 32)
!133 = !DISubroutineType(types: !134)
!134 = !{!81, !25, !48, !48}
!135 = !DIDerivedType(tag: DW_TAG_member, name: "NewLocalRef", scope: !29, file: !12, line: 270, baseType: !122, size: 32, offset: 800)
!136 = !DIDerivedType(tag: DW_TAG_member, name: "EnsureLocalCapacity", scope: !29, file: !12, line: 272, baseType: !118, size: 32, offset: 832)
!137 = !DIDerivedType(tag: DW_TAG_member, name: "AllocObject", scope: !29, file: !12, line: 275, baseType: !138, size: 32, offset: 864)
!138 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !139, size: 32)
!139 = !DISubroutineType(types: !140)
!140 = !{!48, !25, !47}
!141 = !DIDerivedType(tag: DW_TAG_member, name: "NewObject", scope: !29, file: !12, line: 277, baseType: !142, size: 32, offset: 896)
!142 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !143, size: 32)
!143 = !DISubroutineType(types: !144)
!144 = !{!48, !25, !47, !67, null}
!145 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectV", scope: !29, file: !12, line: 279, baseType: !146, size: 32, offset: 928)
!146 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !147, size: 32)
!147 = !DISubroutineType(types: !148)
!148 = !{!48, !25, !47, !67, !149}
!149 = !DIDerivedType(tag: DW_TAG_typedef, name: "va_list", file: !150, line: 72, baseType: !151)
!150 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vadefs.h", directory: "", checksumkind: CSK_MD5, checksum: "a4b8f96637d0704c82f39ecb6bde2ab4")
!151 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !53, size: 32)
!152 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectA", scope: !29, file: !12, line: 281, baseType: !153, size: 32, offset: 960)
!153 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !154, size: 32)
!154 = !DISubroutineType(types: !155)
!155 = !{!48, !25, !47, !67, !156}
!156 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !157, size: 32)
!157 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !158)
!158 = !DIDerivedType(tag: DW_TAG_typedef, name: "jvalue", file: !12, line: 130, baseType: !159)
!159 = distinct !DICompositeType(tag: DW_TAG_union_type, name: "jvalue", file: !12, line: 120, size: 64, elements: !160)
!160 = !{!161, !162, !163, !166, !169, !170, !173, !176, !179}
!161 = !DIDerivedType(tag: DW_TAG_member, name: "z", scope: !159, file: !12, line: 121, baseType: !81, size: 8)
!162 = !DIDerivedType(tag: DW_TAG_member, name: "b", scope: !159, file: !12, line: 122, baseType: !56, size: 8)
!163 = !DIDerivedType(tag: DW_TAG_member, name: "c", scope: !159, file: !12, line: 123, baseType: !164, size: 16)
!164 = !DIDerivedType(tag: DW_TAG_typedef, name: "jchar", file: !12, line: 58, baseType: !165)
!165 = !DIBasicType(name: "unsigned short", size: 16, encoding: DW_ATE_unsigned)
!166 = !DIDerivedType(tag: DW_TAG_member, name: "s", scope: !159, file: !12, line: 124, baseType: !167, size: 16)
!167 = !DIDerivedType(tag: DW_TAG_typedef, name: "jshort", file: !12, line: 59, baseType: !168)
!168 = !DIBasicType(name: "short", size: 16, encoding: DW_ATE_signed)
!169 = !DIDerivedType(tag: DW_TAG_member, name: "i", scope: !159, file: !12, line: 125, baseType: !40, size: 32)
!170 = !DIDerivedType(tag: DW_TAG_member, name: "j", scope: !159, file: !12, line: 126, baseType: !171, size: 64)
!171 = !DIDerivedType(tag: DW_TAG_typedef, name: "jlong", file: !41, line: 34, baseType: !172)
!172 = !DIBasicType(name: "long long", size: 64, encoding: DW_ATE_signed)
!173 = !DIDerivedType(tag: DW_TAG_member, name: "f", scope: !159, file: !12, line: 127, baseType: !174, size: 32)
!174 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfloat", file: !12, line: 60, baseType: !175)
!175 = !DIBasicType(name: "float", size: 32, encoding: DW_ATE_float)
!176 = !DIDerivedType(tag: DW_TAG_member, name: "d", scope: !159, file: !12, line: 128, baseType: !177, size: 64)
!177 = !DIDerivedType(tag: DW_TAG_typedef, name: "jdouble", file: !12, line: 61, baseType: !178)
!178 = !DIBasicType(name: "double", size: 64, encoding: DW_ATE_float)
!179 = !DIDerivedType(tag: DW_TAG_member, name: "l", scope: !159, file: !12, line: 129, baseType: !48, size: 32)
!180 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectClass", scope: !29, file: !12, line: 284, baseType: !181, size: 32, offset: 992)
!181 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !182, size: 32)
!182 = !DISubroutineType(types: !183)
!183 = !{!47, !25, !48}
!184 = !DIDerivedType(tag: DW_TAG_member, name: "IsInstanceOf", scope: !29, file: !12, line: 286, baseType: !185, size: 32, offset: 1024)
!185 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !186, size: 32)
!186 = !DISubroutineType(types: !187)
!187 = !{!81, !25, !48, !47}
!188 = !DIDerivedType(tag: DW_TAG_member, name: "GetMethodID", scope: !29, file: !12, line: 289, baseType: !189, size: 32, offset: 1056)
!189 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !190, size: 32)
!190 = !DISubroutineType(types: !191)
!191 = !{!67, !25, !47, !51, !51}
!192 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethod", scope: !29, file: !12, line: 292, baseType: !193, size: 32, offset: 1088)
!193 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !194, size: 32)
!194 = !DISubroutineType(types: !195)
!195 = !{!48, !25, !48, !67, null}
!196 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodV", scope: !29, file: !12, line: 294, baseType: !197, size: 32, offset: 1120)
!197 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !198, size: 32)
!198 = !DISubroutineType(types: !199)
!199 = !{!48, !25, !48, !67, !149}
!200 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodA", scope: !29, file: !12, line: 296, baseType: !201, size: 32, offset: 1152)
!201 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !202, size: 32)
!202 = !DISubroutineType(types: !203)
!203 = !{!48, !25, !48, !67, !156}
!204 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethod", scope: !29, file: !12, line: 299, baseType: !205, size: 32, offset: 1184)
!205 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !206, size: 32)
!206 = !DISubroutineType(types: !207)
!207 = !{!81, !25, !48, !67, null}
!208 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodV", scope: !29, file: !12, line: 301, baseType: !209, size: 32, offset: 1216)
!209 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !210, size: 32)
!210 = !DISubroutineType(types: !211)
!211 = !{!81, !25, !48, !67, !149}
!212 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodA", scope: !29, file: !12, line: 303, baseType: !213, size: 32, offset: 1248)
!213 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !214, size: 32)
!214 = !DISubroutineType(types: !215)
!215 = !{!81, !25, !48, !67, !156}
!216 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethod", scope: !29, file: !12, line: 306, baseType: !217, size: 32, offset: 1280)
!217 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !218, size: 32)
!218 = !DISubroutineType(types: !219)
!219 = !{!56, !25, !48, !67, null}
!220 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodV", scope: !29, file: !12, line: 308, baseType: !221, size: 32, offset: 1312)
!221 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !222, size: 32)
!222 = !DISubroutineType(types: !223)
!223 = !{!56, !25, !48, !67, !149}
!224 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodA", scope: !29, file: !12, line: 310, baseType: !225, size: 32, offset: 1344)
!225 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !226, size: 32)
!226 = !DISubroutineType(types: !227)
!227 = !{!56, !25, !48, !67, !156}
!228 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethod", scope: !29, file: !12, line: 313, baseType: !229, size: 32, offset: 1376)
!229 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !230, size: 32)
!230 = !DISubroutineType(types: !231)
!231 = !{!164, !25, !48, !67, null}
!232 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodV", scope: !29, file: !12, line: 315, baseType: !233, size: 32, offset: 1408)
!233 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !234, size: 32)
!234 = !DISubroutineType(types: !235)
!235 = !{!164, !25, !48, !67, !149}
!236 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodA", scope: !29, file: !12, line: 317, baseType: !237, size: 32, offset: 1440)
!237 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !238, size: 32)
!238 = !DISubroutineType(types: !239)
!239 = !{!164, !25, !48, !67, !156}
!240 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethod", scope: !29, file: !12, line: 320, baseType: !241, size: 32, offset: 1472)
!241 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !242, size: 32)
!242 = !DISubroutineType(types: !243)
!243 = !{!167, !25, !48, !67, null}
!244 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodV", scope: !29, file: !12, line: 322, baseType: !245, size: 32, offset: 1504)
!245 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !246, size: 32)
!246 = !DISubroutineType(types: !247)
!247 = !{!167, !25, !48, !67, !149}
!248 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodA", scope: !29, file: !12, line: 324, baseType: !249, size: 32, offset: 1536)
!249 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !250, size: 32)
!250 = !DISubroutineType(types: !251)
!251 = !{!167, !25, !48, !67, !156}
!252 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethod", scope: !29, file: !12, line: 327, baseType: !253, size: 32, offset: 1568)
!253 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !254, size: 32)
!254 = !DISubroutineType(types: !255)
!255 = !{!40, !25, !48, !67, null}
!256 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodV", scope: !29, file: !12, line: 329, baseType: !257, size: 32, offset: 1600)
!257 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !258, size: 32)
!258 = !DISubroutineType(types: !259)
!259 = !{!40, !25, !48, !67, !149}
!260 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodA", scope: !29, file: !12, line: 331, baseType: !261, size: 32, offset: 1632)
!261 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !262, size: 32)
!262 = !DISubroutineType(types: !263)
!263 = !{!40, !25, !48, !67, !156}
!264 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethod", scope: !29, file: !12, line: 334, baseType: !265, size: 32, offset: 1664)
!265 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !266, size: 32)
!266 = !DISubroutineType(types: !267)
!267 = !{!171, !25, !48, !67, null}
!268 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodV", scope: !29, file: !12, line: 336, baseType: !269, size: 32, offset: 1696)
!269 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !270, size: 32)
!270 = !DISubroutineType(types: !271)
!271 = !{!171, !25, !48, !67, !149}
!272 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodA", scope: !29, file: !12, line: 338, baseType: !273, size: 32, offset: 1728)
!273 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !274, size: 32)
!274 = !DISubroutineType(types: !275)
!275 = !{!171, !25, !48, !67, !156}
!276 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethod", scope: !29, file: !12, line: 341, baseType: !277, size: 32, offset: 1760)
!277 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !278, size: 32)
!278 = !DISubroutineType(types: !279)
!279 = !{!174, !25, !48, !67, null}
!280 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodV", scope: !29, file: !12, line: 343, baseType: !281, size: 32, offset: 1792)
!281 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !282, size: 32)
!282 = !DISubroutineType(types: !283)
!283 = !{!174, !25, !48, !67, !149}
!284 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodA", scope: !29, file: !12, line: 345, baseType: !285, size: 32, offset: 1824)
!285 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !286, size: 32)
!286 = !DISubroutineType(types: !287)
!287 = !{!174, !25, !48, !67, !156}
!288 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethod", scope: !29, file: !12, line: 348, baseType: !289, size: 32, offset: 1856)
!289 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !290, size: 32)
!290 = !DISubroutineType(types: !291)
!291 = !{!177, !25, !48, !67, null}
!292 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodV", scope: !29, file: !12, line: 350, baseType: !293, size: 32, offset: 1888)
!293 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !294, size: 32)
!294 = !DISubroutineType(types: !295)
!295 = !{!177, !25, !48, !67, !149}
!296 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodA", scope: !29, file: !12, line: 352, baseType: !297, size: 32, offset: 1920)
!297 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !298, size: 32)
!298 = !DISubroutineType(types: !299)
!299 = !{!177, !25, !48, !67, !156}
!300 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethod", scope: !29, file: !12, line: 355, baseType: !301, size: 32, offset: 1952)
!301 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !302, size: 32)
!302 = !DISubroutineType(types: !303)
!303 = !{null, !25, !48, !67, null}
!304 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodV", scope: !29, file: !12, line: 357, baseType: !305, size: 32, offset: 1984)
!305 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !306, size: 32)
!306 = !DISubroutineType(types: !307)
!307 = !{null, !25, !48, !67, !149}
!308 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodA", scope: !29, file: !12, line: 359, baseType: !309, size: 32, offset: 2016)
!309 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !310, size: 32)
!310 = !DISubroutineType(types: !311)
!311 = !{null, !25, !48, !67, !156}
!312 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethod", scope: !29, file: !12, line: 362, baseType: !313, size: 32, offset: 2048)
!313 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !314, size: 32)
!314 = !DISubroutineType(types: !315)
!315 = !{!48, !25, !48, !47, !67, null}
!316 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodV", scope: !29, file: !12, line: 364, baseType: !317, size: 32, offset: 2080)
!317 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !318, size: 32)
!318 = !DISubroutineType(types: !319)
!319 = !{!48, !25, !48, !47, !67, !149}
!320 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodA", scope: !29, file: !12, line: 367, baseType: !321, size: 32, offset: 2112)
!321 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !322, size: 32)
!322 = !DISubroutineType(types: !323)
!323 = !{!48, !25, !48, !47, !67, !156}
!324 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethod", scope: !29, file: !12, line: 371, baseType: !325, size: 32, offset: 2144)
!325 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !326, size: 32)
!326 = !DISubroutineType(types: !327)
!327 = !{!81, !25, !48, !47, !67, null}
!328 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodV", scope: !29, file: !12, line: 373, baseType: !329, size: 32, offset: 2176)
!329 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !330, size: 32)
!330 = !DISubroutineType(types: !331)
!331 = !{!81, !25, !48, !47, !67, !149}
!332 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodA", scope: !29, file: !12, line: 376, baseType: !333, size: 32, offset: 2208)
!333 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !334, size: 32)
!334 = !DISubroutineType(types: !335)
!335 = !{!81, !25, !48, !47, !67, !156}
!336 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethod", scope: !29, file: !12, line: 380, baseType: !337, size: 32, offset: 2240)
!337 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !338, size: 32)
!338 = !DISubroutineType(types: !339)
!339 = !{!56, !25, !48, !47, !67, null}
!340 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodV", scope: !29, file: !12, line: 382, baseType: !341, size: 32, offset: 2272)
!341 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !342, size: 32)
!342 = !DISubroutineType(types: !343)
!343 = !{!56, !25, !48, !47, !67, !149}
!344 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodA", scope: !29, file: !12, line: 385, baseType: !345, size: 32, offset: 2304)
!345 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !346, size: 32)
!346 = !DISubroutineType(types: !347)
!347 = !{!56, !25, !48, !47, !67, !156}
!348 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethod", scope: !29, file: !12, line: 389, baseType: !349, size: 32, offset: 2336)
!349 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !350, size: 32)
!350 = !DISubroutineType(types: !351)
!351 = !{!164, !25, !48, !47, !67, null}
!352 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodV", scope: !29, file: !12, line: 391, baseType: !353, size: 32, offset: 2368)
!353 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !354, size: 32)
!354 = !DISubroutineType(types: !355)
!355 = !{!164, !25, !48, !47, !67, !149}
!356 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodA", scope: !29, file: !12, line: 394, baseType: !357, size: 32, offset: 2400)
!357 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !358, size: 32)
!358 = !DISubroutineType(types: !359)
!359 = !{!164, !25, !48, !47, !67, !156}
!360 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethod", scope: !29, file: !12, line: 398, baseType: !361, size: 32, offset: 2432)
!361 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !362, size: 32)
!362 = !DISubroutineType(types: !363)
!363 = !{!167, !25, !48, !47, !67, null}
!364 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodV", scope: !29, file: !12, line: 400, baseType: !365, size: 32, offset: 2464)
!365 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !366, size: 32)
!366 = !DISubroutineType(types: !367)
!367 = !{!167, !25, !48, !47, !67, !149}
!368 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodA", scope: !29, file: !12, line: 403, baseType: !369, size: 32, offset: 2496)
!369 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !370, size: 32)
!370 = !DISubroutineType(types: !371)
!371 = !{!167, !25, !48, !47, !67, !156}
!372 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethod", scope: !29, file: !12, line: 407, baseType: !373, size: 32, offset: 2528)
!373 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !374, size: 32)
!374 = !DISubroutineType(types: !375)
!375 = !{!40, !25, !48, !47, !67, null}
!376 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodV", scope: !29, file: !12, line: 409, baseType: !377, size: 32, offset: 2560)
!377 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !378, size: 32)
!378 = !DISubroutineType(types: !379)
!379 = !{!40, !25, !48, !47, !67, !149}
!380 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodA", scope: !29, file: !12, line: 412, baseType: !381, size: 32, offset: 2592)
!381 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !382, size: 32)
!382 = !DISubroutineType(types: !383)
!383 = !{!40, !25, !48, !47, !67, !156}
!384 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethod", scope: !29, file: !12, line: 416, baseType: !385, size: 32, offset: 2624)
!385 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !386, size: 32)
!386 = !DISubroutineType(types: !387)
!387 = !{!171, !25, !48, !47, !67, null}
!388 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodV", scope: !29, file: !12, line: 418, baseType: !389, size: 32, offset: 2656)
!389 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !390, size: 32)
!390 = !DISubroutineType(types: !391)
!391 = !{!171, !25, !48, !47, !67, !149}
!392 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodA", scope: !29, file: !12, line: 421, baseType: !393, size: 32, offset: 2688)
!393 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !394, size: 32)
!394 = !DISubroutineType(types: !395)
!395 = !{!171, !25, !48, !47, !67, !156}
!396 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethod", scope: !29, file: !12, line: 425, baseType: !397, size: 32, offset: 2720)
!397 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !398, size: 32)
!398 = !DISubroutineType(types: !399)
!399 = !{!174, !25, !48, !47, !67, null}
!400 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodV", scope: !29, file: !12, line: 427, baseType: !401, size: 32, offset: 2752)
!401 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !402, size: 32)
!402 = !DISubroutineType(types: !403)
!403 = !{!174, !25, !48, !47, !67, !149}
!404 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodA", scope: !29, file: !12, line: 430, baseType: !405, size: 32, offset: 2784)
!405 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !406, size: 32)
!406 = !DISubroutineType(types: !407)
!407 = !{!174, !25, !48, !47, !67, !156}
!408 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethod", scope: !29, file: !12, line: 434, baseType: !409, size: 32, offset: 2816)
!409 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !410, size: 32)
!410 = !DISubroutineType(types: !411)
!411 = !{!177, !25, !48, !47, !67, null}
!412 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodV", scope: !29, file: !12, line: 436, baseType: !413, size: 32, offset: 2848)
!413 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !414, size: 32)
!414 = !DISubroutineType(types: !415)
!415 = !{!177, !25, !48, !47, !67, !149}
!416 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodA", scope: !29, file: !12, line: 439, baseType: !417, size: 32, offset: 2880)
!417 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !418, size: 32)
!418 = !DISubroutineType(types: !419)
!419 = !{!177, !25, !48, !47, !67, !156}
!420 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethod", scope: !29, file: !12, line: 443, baseType: !421, size: 32, offset: 2912)
!421 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !422, size: 32)
!422 = !DISubroutineType(types: !423)
!423 = !{null, !25, !48, !47, !67, null}
!424 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodV", scope: !29, file: !12, line: 445, baseType: !425, size: 32, offset: 2944)
!425 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !426, size: 32)
!426 = !DISubroutineType(types: !427)
!427 = !{null, !25, !48, !47, !67, !149}
!428 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodA", scope: !29, file: !12, line: 448, baseType: !429, size: 32, offset: 2976)
!429 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !430, size: 32)
!430 = !DISubroutineType(types: !431)
!431 = !{null, !25, !48, !47, !67, !156}
!432 = !DIDerivedType(tag: DW_TAG_member, name: "GetFieldID", scope: !29, file: !12, line: 452, baseType: !433, size: 32, offset: 3008)
!433 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !434, size: 32)
!434 = !DISubroutineType(types: !435)
!435 = !{!74, !25, !47, !51, !51}
!436 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectField", scope: !29, file: !12, line: 455, baseType: !437, size: 32, offset: 3040)
!437 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !438, size: 32)
!438 = !DISubroutineType(types: !439)
!439 = !{!48, !25, !48, !74}
!440 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanField", scope: !29, file: !12, line: 457, baseType: !441, size: 32, offset: 3072)
!441 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !442, size: 32)
!442 = !DISubroutineType(types: !443)
!443 = !{!81, !25, !48, !74}
!444 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteField", scope: !29, file: !12, line: 459, baseType: !445, size: 32, offset: 3104)
!445 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !446, size: 32)
!446 = !DISubroutineType(types: !447)
!447 = !{!56, !25, !48, !74}
!448 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharField", scope: !29, file: !12, line: 461, baseType: !449, size: 32, offset: 3136)
!449 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !450, size: 32)
!450 = !DISubroutineType(types: !451)
!451 = !{!164, !25, !48, !74}
!452 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortField", scope: !29, file: !12, line: 463, baseType: !453, size: 32, offset: 3168)
!453 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !454, size: 32)
!454 = !DISubroutineType(types: !455)
!455 = !{!167, !25, !48, !74}
!456 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntField", scope: !29, file: !12, line: 465, baseType: !457, size: 32, offset: 3200)
!457 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !458, size: 32)
!458 = !DISubroutineType(types: !459)
!459 = !{!40, !25, !48, !74}
!460 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongField", scope: !29, file: !12, line: 467, baseType: !461, size: 32, offset: 3232)
!461 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !462, size: 32)
!462 = !DISubroutineType(types: !463)
!463 = !{!171, !25, !48, !74}
!464 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatField", scope: !29, file: !12, line: 469, baseType: !465, size: 32, offset: 3264)
!465 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !466, size: 32)
!466 = !DISubroutineType(types: !467)
!467 = !{!174, !25, !48, !74}
!468 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleField", scope: !29, file: !12, line: 471, baseType: !469, size: 32, offset: 3296)
!469 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !470, size: 32)
!470 = !DISubroutineType(types: !471)
!471 = !{!177, !25, !48, !74}
!472 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectField", scope: !29, file: !12, line: 474, baseType: !473, size: 32, offset: 3328)
!473 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !474, size: 32)
!474 = !DISubroutineType(types: !475)
!475 = !{null, !25, !48, !74, !48}
!476 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanField", scope: !29, file: !12, line: 476, baseType: !477, size: 32, offset: 3360)
!477 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !478, size: 32)
!478 = !DISubroutineType(types: !479)
!479 = !{null, !25, !48, !74, !81}
!480 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteField", scope: !29, file: !12, line: 478, baseType: !481, size: 32, offset: 3392)
!481 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !482, size: 32)
!482 = !DISubroutineType(types: !483)
!483 = !{null, !25, !48, !74, !56}
!484 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharField", scope: !29, file: !12, line: 480, baseType: !485, size: 32, offset: 3424)
!485 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !486, size: 32)
!486 = !DISubroutineType(types: !487)
!487 = !{null, !25, !48, !74, !164}
!488 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortField", scope: !29, file: !12, line: 482, baseType: !489, size: 32, offset: 3456)
!489 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !490, size: 32)
!490 = !DISubroutineType(types: !491)
!491 = !{null, !25, !48, !74, !167}
!492 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntField", scope: !29, file: !12, line: 484, baseType: !493, size: 32, offset: 3488)
!493 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !494, size: 32)
!494 = !DISubroutineType(types: !495)
!495 = !{null, !25, !48, !74, !40}
!496 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongField", scope: !29, file: !12, line: 486, baseType: !497, size: 32, offset: 3520)
!497 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !498, size: 32)
!498 = !DISubroutineType(types: !499)
!499 = !{null, !25, !48, !74, !171}
!500 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatField", scope: !29, file: !12, line: 488, baseType: !501, size: 32, offset: 3552)
!501 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !502, size: 32)
!502 = !DISubroutineType(types: !503)
!503 = !{null, !25, !48, !74, !174}
!504 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleField", scope: !29, file: !12, line: 490, baseType: !505, size: 32, offset: 3584)
!505 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !506, size: 32)
!506 = !DISubroutineType(types: !507)
!507 = !{null, !25, !48, !74, !177}
!508 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticMethodID", scope: !29, file: !12, line: 493, baseType: !189, size: 32, offset: 3616)
!509 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethod", scope: !29, file: !12, line: 496, baseType: !142, size: 32, offset: 3648)
!510 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethodV", scope: !29, file: !12, line: 498, baseType: !146, size: 32, offset: 3680)
!511 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethodA", scope: !29, file: !12, line: 500, baseType: !153, size: 32, offset: 3712)
!512 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethod", scope: !29, file: !12, line: 503, baseType: !513, size: 32, offset: 3744)
!513 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !514, size: 32)
!514 = !DISubroutineType(types: !515)
!515 = !{!81, !25, !47, !67, null}
!516 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethodV", scope: !29, file: !12, line: 505, baseType: !517, size: 32, offset: 3776)
!517 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !518, size: 32)
!518 = !DISubroutineType(types: !519)
!519 = !{!81, !25, !47, !67, !149}
!520 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethodA", scope: !29, file: !12, line: 507, baseType: !521, size: 32, offset: 3808)
!521 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !522, size: 32)
!522 = !DISubroutineType(types: !523)
!523 = !{!81, !25, !47, !67, !156}
!524 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethod", scope: !29, file: !12, line: 510, baseType: !525, size: 32, offset: 3840)
!525 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !526, size: 32)
!526 = !DISubroutineType(types: !527)
!527 = !{!56, !25, !47, !67, null}
!528 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodV", scope: !29, file: !12, line: 512, baseType: !529, size: 32, offset: 3872)
!529 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !530, size: 32)
!530 = !DISubroutineType(types: !531)
!531 = !{!56, !25, !47, !67, !149}
!532 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodA", scope: !29, file: !12, line: 514, baseType: !533, size: 32, offset: 3904)
!533 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !534, size: 32)
!534 = !DISubroutineType(types: !535)
!535 = !{!56, !25, !47, !67, !156}
!536 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethod", scope: !29, file: !12, line: 517, baseType: !537, size: 32, offset: 3936)
!537 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !538, size: 32)
!538 = !DISubroutineType(types: !539)
!539 = !{!164, !25, !47, !67, null}
!540 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodV", scope: !29, file: !12, line: 519, baseType: !541, size: 32, offset: 3968)
!541 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !542, size: 32)
!542 = !DISubroutineType(types: !543)
!543 = !{!164, !25, !47, !67, !149}
!544 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodA", scope: !29, file: !12, line: 521, baseType: !545, size: 32, offset: 4000)
!545 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !546, size: 32)
!546 = !DISubroutineType(types: !547)
!547 = !{!164, !25, !47, !67, !156}
!548 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethod", scope: !29, file: !12, line: 524, baseType: !549, size: 32, offset: 4032)
!549 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !550, size: 32)
!550 = !DISubroutineType(types: !551)
!551 = !{!167, !25, !47, !67, null}
!552 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodV", scope: !29, file: !12, line: 526, baseType: !553, size: 32, offset: 4064)
!553 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !554, size: 32)
!554 = !DISubroutineType(types: !555)
!555 = !{!167, !25, !47, !67, !149}
!556 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodA", scope: !29, file: !12, line: 528, baseType: !557, size: 32, offset: 4096)
!557 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !558, size: 32)
!558 = !DISubroutineType(types: !559)
!559 = !{!167, !25, !47, !67, !156}
!560 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethod", scope: !29, file: !12, line: 531, baseType: !561, size: 32, offset: 4128)
!561 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !562, size: 32)
!562 = !DISubroutineType(types: !563)
!563 = !{!40, !25, !47, !67, null}
!564 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodV", scope: !29, file: !12, line: 533, baseType: !565, size: 32, offset: 4160)
!565 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !566, size: 32)
!566 = !DISubroutineType(types: !567)
!567 = !{!40, !25, !47, !67, !149}
!568 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodA", scope: !29, file: !12, line: 535, baseType: !569, size: 32, offset: 4192)
!569 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !570, size: 32)
!570 = !DISubroutineType(types: !571)
!571 = !{!40, !25, !47, !67, !156}
!572 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethod", scope: !29, file: !12, line: 538, baseType: !573, size: 32, offset: 4224)
!573 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !574, size: 32)
!574 = !DISubroutineType(types: !575)
!575 = !{!171, !25, !47, !67, null}
!576 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodV", scope: !29, file: !12, line: 540, baseType: !577, size: 32, offset: 4256)
!577 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !578, size: 32)
!578 = !DISubroutineType(types: !579)
!579 = !{!171, !25, !47, !67, !149}
!580 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodA", scope: !29, file: !12, line: 542, baseType: !581, size: 32, offset: 4288)
!581 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !582, size: 32)
!582 = !DISubroutineType(types: !583)
!583 = !{!171, !25, !47, !67, !156}
!584 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethod", scope: !29, file: !12, line: 545, baseType: !585, size: 32, offset: 4320)
!585 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !586, size: 32)
!586 = !DISubroutineType(types: !587)
!587 = !{!174, !25, !47, !67, null}
!588 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodV", scope: !29, file: !12, line: 547, baseType: !589, size: 32, offset: 4352)
!589 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !590, size: 32)
!590 = !DISubroutineType(types: !591)
!591 = !{!174, !25, !47, !67, !149}
!592 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodA", scope: !29, file: !12, line: 549, baseType: !593, size: 32, offset: 4384)
!593 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !594, size: 32)
!594 = !DISubroutineType(types: !595)
!595 = !{!174, !25, !47, !67, !156}
!596 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethod", scope: !29, file: !12, line: 552, baseType: !597, size: 32, offset: 4416)
!597 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !598, size: 32)
!598 = !DISubroutineType(types: !599)
!599 = !{!177, !25, !47, !67, null}
!600 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodV", scope: !29, file: !12, line: 554, baseType: !601, size: 32, offset: 4448)
!601 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !602, size: 32)
!602 = !DISubroutineType(types: !603)
!603 = !{!177, !25, !47, !67, !149}
!604 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodA", scope: !29, file: !12, line: 556, baseType: !605, size: 32, offset: 4480)
!605 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !606, size: 32)
!606 = !DISubroutineType(types: !607)
!607 = !{!177, !25, !47, !67, !156}
!608 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethod", scope: !29, file: !12, line: 559, baseType: !609, size: 32, offset: 4512)
!609 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !610, size: 32)
!610 = !DISubroutineType(types: !611)
!611 = !{null, !25, !47, !67, null}
!612 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodV", scope: !29, file: !12, line: 561, baseType: !613, size: 32, offset: 4544)
!613 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !614, size: 32)
!614 = !DISubroutineType(types: !615)
!615 = !{null, !25, !47, !67, !149}
!616 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodA", scope: !29, file: !12, line: 563, baseType: !617, size: 32, offset: 4576)
!617 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !618, size: 32)
!618 = !DISubroutineType(types: !619)
!619 = !{null, !25, !47, !67, !156}
!620 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFieldID", scope: !29, file: !12, line: 566, baseType: !433, size: 32, offset: 4608)
!621 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticObjectField", scope: !29, file: !12, line: 568, baseType: !622, size: 32, offset: 4640)
!622 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !623, size: 32)
!623 = !DISubroutineType(types: !624)
!624 = !{!48, !25, !47, !74}
!625 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticBooleanField", scope: !29, file: !12, line: 570, baseType: !626, size: 32, offset: 4672)
!626 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !627, size: 32)
!627 = !DISubroutineType(types: !628)
!628 = !{!81, !25, !47, !74}
!629 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticByteField", scope: !29, file: !12, line: 572, baseType: !630, size: 32, offset: 4704)
!630 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !631, size: 32)
!631 = !DISubroutineType(types: !632)
!632 = !{!56, !25, !47, !74}
!633 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticCharField", scope: !29, file: !12, line: 574, baseType: !634, size: 32, offset: 4736)
!634 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !635, size: 32)
!635 = !DISubroutineType(types: !636)
!636 = !{!164, !25, !47, !74}
!637 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticShortField", scope: !29, file: !12, line: 576, baseType: !638, size: 32, offset: 4768)
!638 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !639, size: 32)
!639 = !DISubroutineType(types: !640)
!640 = !{!167, !25, !47, !74}
!641 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticIntField", scope: !29, file: !12, line: 578, baseType: !642, size: 32, offset: 4800)
!642 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !643, size: 32)
!643 = !DISubroutineType(types: !644)
!644 = !{!40, !25, !47, !74}
!645 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticLongField", scope: !29, file: !12, line: 580, baseType: !646, size: 32, offset: 4832)
!646 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !647, size: 32)
!647 = !DISubroutineType(types: !648)
!648 = !{!171, !25, !47, !74}
!649 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFloatField", scope: !29, file: !12, line: 582, baseType: !650, size: 32, offset: 4864)
!650 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !651, size: 32)
!651 = !DISubroutineType(types: !652)
!652 = !{!174, !25, !47, !74}
!653 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticDoubleField", scope: !29, file: !12, line: 584, baseType: !654, size: 32, offset: 4896)
!654 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !655, size: 32)
!655 = !DISubroutineType(types: !656)
!656 = !{!177, !25, !47, !74}
!657 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticObjectField", scope: !29, file: !12, line: 587, baseType: !658, size: 32, offset: 4928)
!658 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !659, size: 32)
!659 = !DISubroutineType(types: !660)
!660 = !{null, !25, !47, !74, !48}
!661 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticBooleanField", scope: !29, file: !12, line: 589, baseType: !662, size: 32, offset: 4960)
!662 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !663, size: 32)
!663 = !DISubroutineType(types: !664)
!664 = !{null, !25, !47, !74, !81}
!665 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticByteField", scope: !29, file: !12, line: 591, baseType: !666, size: 32, offset: 4992)
!666 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !667, size: 32)
!667 = !DISubroutineType(types: !668)
!668 = !{null, !25, !47, !74, !56}
!669 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticCharField", scope: !29, file: !12, line: 593, baseType: !670, size: 32, offset: 5024)
!670 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !671, size: 32)
!671 = !DISubroutineType(types: !672)
!672 = !{null, !25, !47, !74, !164}
!673 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticShortField", scope: !29, file: !12, line: 595, baseType: !674, size: 32, offset: 5056)
!674 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !675, size: 32)
!675 = !DISubroutineType(types: !676)
!676 = !{null, !25, !47, !74, !167}
!677 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticIntField", scope: !29, file: !12, line: 597, baseType: !678, size: 32, offset: 5088)
!678 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !679, size: 32)
!679 = !DISubroutineType(types: !680)
!680 = !{null, !25, !47, !74, !40}
!681 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticLongField", scope: !29, file: !12, line: 599, baseType: !682, size: 32, offset: 5120)
!682 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !683, size: 32)
!683 = !DISubroutineType(types: !684)
!684 = !{null, !25, !47, !74, !171}
!685 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticFloatField", scope: !29, file: !12, line: 601, baseType: !686, size: 32, offset: 5152)
!686 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !687, size: 32)
!687 = !DISubroutineType(types: !688)
!688 = !{null, !25, !47, !74, !174}
!689 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticDoubleField", scope: !29, file: !12, line: 603, baseType: !690, size: 32, offset: 5184)
!690 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !691, size: 32)
!691 = !DISubroutineType(types: !692)
!692 = !{null, !25, !47, !74, !177}
!693 = !DIDerivedType(tag: DW_TAG_member, name: "NewString", scope: !29, file: !12, line: 606, baseType: !694, size: 32, offset: 5216)
!694 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !695, size: 32)
!695 = !DISubroutineType(types: !696)
!696 = !{!697, !25, !698, !58}
!697 = !DIDerivedType(tag: DW_TAG_typedef, name: "jstring", file: !12, line: 104, baseType: !48)
!698 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !699, size: 32)
!699 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !164)
!700 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringLength", scope: !29, file: !12, line: 608, baseType: !701, size: 32, offset: 5248)
!701 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !702, size: 32)
!702 = !DISubroutineType(types: !703)
!703 = !{!58, !25, !697}
!704 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringChars", scope: !29, file: !12, line: 610, baseType: !705, size: 32, offset: 5280)
!705 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !706, size: 32)
!706 = !DISubroutineType(types: !707)
!707 = !{!698, !25, !697, !708}
!708 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !81, size: 32)
!709 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringChars", scope: !29, file: !12, line: 612, baseType: !710, size: 32, offset: 5312)
!710 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !711, size: 32)
!711 = !DISubroutineType(types: !712)
!712 = !{null, !25, !697, !698}
!713 = !DIDerivedType(tag: DW_TAG_member, name: "NewStringUTF", scope: !29, file: !12, line: 615, baseType: !714, size: 32, offset: 5344)
!714 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !715, size: 32)
!715 = !DISubroutineType(types: !716)
!716 = !{!697, !25, !51}
!717 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFLength", scope: !29, file: !12, line: 617, baseType: !701, size: 32, offset: 5376)
!718 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFChars", scope: !29, file: !12, line: 619, baseType: !719, size: 32, offset: 5408)
!719 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !720, size: 32)
!720 = !DISubroutineType(types: !721)
!721 = !{!51, !25, !697, !708}
!722 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringUTFChars", scope: !29, file: !12, line: 621, baseType: !723, size: 32, offset: 5440)
!723 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !724, size: 32)
!724 = !DISubroutineType(types: !725)
!725 = !{null, !25, !697, !51}
!726 = !DIDerivedType(tag: DW_TAG_member, name: "GetArrayLength", scope: !29, file: !12, line: 625, baseType: !727, size: 32, offset: 5472)
!727 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !728, size: 32)
!728 = !DISubroutineType(types: !729)
!729 = !{!58, !25, !730}
!730 = !DIDerivedType(tag: DW_TAG_typedef, name: "jarray", file: !12, line: 105, baseType: !48)
!731 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectArray", scope: !29, file: !12, line: 628, baseType: !732, size: 32, offset: 5504)
!732 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !733, size: 32)
!733 = !DISubroutineType(types: !734)
!734 = !{!735, !25, !58, !47, !48}
!735 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectArray", file: !12, line: 114, baseType: !730)
!736 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectArrayElement", scope: !29, file: !12, line: 630, baseType: !737, size: 32, offset: 5536)
!737 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !738, size: 32)
!738 = !DISubroutineType(types: !739)
!739 = !{!48, !25, !735, !58}
!740 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectArrayElement", scope: !29, file: !12, line: 632, baseType: !741, size: 32, offset: 5568)
!741 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !742, size: 32)
!742 = !DISubroutineType(types: !743)
!743 = !{null, !25, !735, !58, !48}
!744 = !DIDerivedType(tag: DW_TAG_member, name: "NewBooleanArray", scope: !29, file: !12, line: 635, baseType: !745, size: 32, offset: 5600)
!745 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !746, size: 32)
!746 = !DISubroutineType(types: !747)
!747 = !{!748, !25, !58}
!748 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbooleanArray", file: !12, line: 106, baseType: !730)
!749 = !DIDerivedType(tag: DW_TAG_member, name: "NewByteArray", scope: !29, file: !12, line: 637, baseType: !750, size: 32, offset: 5632)
!750 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !751, size: 32)
!751 = !DISubroutineType(types: !752)
!752 = !{!753, !25, !58}
!753 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbyteArray", file: !12, line: 107, baseType: !730)
!754 = !DIDerivedType(tag: DW_TAG_member, name: "NewCharArray", scope: !29, file: !12, line: 639, baseType: !755, size: 32, offset: 5664)
!755 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !756, size: 32)
!756 = !DISubroutineType(types: !757)
!757 = !{!758, !25, !58}
!758 = !DIDerivedType(tag: DW_TAG_typedef, name: "jcharArray", file: !12, line: 108, baseType: !730)
!759 = !DIDerivedType(tag: DW_TAG_member, name: "NewShortArray", scope: !29, file: !12, line: 641, baseType: !760, size: 32, offset: 5696)
!760 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !761, size: 32)
!761 = !DISubroutineType(types: !762)
!762 = !{!763, !25, !58}
!763 = !DIDerivedType(tag: DW_TAG_typedef, name: "jshortArray", file: !12, line: 109, baseType: !730)
!764 = !DIDerivedType(tag: DW_TAG_member, name: "NewIntArray", scope: !29, file: !12, line: 643, baseType: !765, size: 32, offset: 5728)
!765 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !766, size: 32)
!766 = !DISubroutineType(types: !767)
!767 = !{!768, !25, !58}
!768 = !DIDerivedType(tag: DW_TAG_typedef, name: "jintArray", file: !12, line: 110, baseType: !730)
!769 = !DIDerivedType(tag: DW_TAG_member, name: "NewLongArray", scope: !29, file: !12, line: 645, baseType: !770, size: 32, offset: 5760)
!770 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !771, size: 32)
!771 = !DISubroutineType(types: !772)
!772 = !{!773, !25, !58}
!773 = !DIDerivedType(tag: DW_TAG_typedef, name: "jlongArray", file: !12, line: 111, baseType: !730)
!774 = !DIDerivedType(tag: DW_TAG_member, name: "NewFloatArray", scope: !29, file: !12, line: 647, baseType: !775, size: 32, offset: 5792)
!775 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !776, size: 32)
!776 = !DISubroutineType(types: !777)
!777 = !{!778, !25, !58}
!778 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfloatArray", file: !12, line: 112, baseType: !730)
!779 = !DIDerivedType(tag: DW_TAG_member, name: "NewDoubleArray", scope: !29, file: !12, line: 649, baseType: !780, size: 32, offset: 5824)
!780 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !781, size: 32)
!781 = !DISubroutineType(types: !782)
!782 = !{!783, !25, !58}
!783 = !DIDerivedType(tag: DW_TAG_typedef, name: "jdoubleArray", file: !12, line: 113, baseType: !730)
!784 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayElements", scope: !29, file: !12, line: 652, baseType: !785, size: 32, offset: 5856)
!785 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !786, size: 32)
!786 = !DISubroutineType(types: !787)
!787 = !{!708, !25, !748, !708}
!788 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayElements", scope: !29, file: !12, line: 654, baseType: !789, size: 32, offset: 5888)
!789 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !790, size: 32)
!790 = !DISubroutineType(types: !791)
!791 = !{!792, !25, !753, !708}
!792 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !56, size: 32)
!793 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayElements", scope: !29, file: !12, line: 656, baseType: !794, size: 32, offset: 5920)
!794 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !795, size: 32)
!795 = !DISubroutineType(types: !796)
!796 = !{!797, !25, !758, !708}
!797 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !164, size: 32)
!798 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayElements", scope: !29, file: !12, line: 658, baseType: !799, size: 32, offset: 5952)
!799 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !800, size: 32)
!800 = !DISubroutineType(types: !801)
!801 = !{!802, !25, !763, !708}
!802 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !167, size: 32)
!803 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayElements", scope: !29, file: !12, line: 660, baseType: !804, size: 32, offset: 5984)
!804 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !805, size: 32)
!805 = !DISubroutineType(types: !806)
!806 = !{!807, !25, !768, !708}
!807 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !40, size: 32)
!808 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayElements", scope: !29, file: !12, line: 662, baseType: !809, size: 32, offset: 6016)
!809 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !810, size: 32)
!810 = !DISubroutineType(types: !811)
!811 = !{!812, !25, !773, !708}
!812 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !171, size: 32)
!813 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayElements", scope: !29, file: !12, line: 664, baseType: !814, size: 32, offset: 6048)
!814 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !815, size: 32)
!815 = !DISubroutineType(types: !816)
!816 = !{!817, !25, !778, !708}
!817 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !174, size: 32)
!818 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayElements", scope: !29, file: !12, line: 666, baseType: !819, size: 32, offset: 6080)
!819 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !820, size: 32)
!820 = !DISubroutineType(types: !821)
!821 = !{!822, !25, !783, !708}
!822 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !177, size: 32)
!823 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseBooleanArrayElements", scope: !29, file: !12, line: 669, baseType: !824, size: 32, offset: 6112)
!824 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !825, size: 32)
!825 = !DISubroutineType(types: !826)
!826 = !{null, !25, !748, !708, !40}
!827 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseByteArrayElements", scope: !29, file: !12, line: 671, baseType: !828, size: 32, offset: 6144)
!828 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !829, size: 32)
!829 = !DISubroutineType(types: !830)
!830 = !{null, !25, !753, !792, !40}
!831 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseCharArrayElements", scope: !29, file: !12, line: 673, baseType: !832, size: 32, offset: 6176)
!832 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !833, size: 32)
!833 = !DISubroutineType(types: !834)
!834 = !{null, !25, !758, !797, !40}
!835 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseShortArrayElements", scope: !29, file: !12, line: 675, baseType: !836, size: 32, offset: 6208)
!836 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !837, size: 32)
!837 = !DISubroutineType(types: !838)
!838 = !{null, !25, !763, !802, !40}
!839 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseIntArrayElements", scope: !29, file: !12, line: 677, baseType: !840, size: 32, offset: 6240)
!840 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !841, size: 32)
!841 = !DISubroutineType(types: !842)
!842 = !{null, !25, !768, !807, !40}
!843 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseLongArrayElements", scope: !29, file: !12, line: 679, baseType: !844, size: 32, offset: 6272)
!844 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !845, size: 32)
!845 = !DISubroutineType(types: !846)
!846 = !{null, !25, !773, !812, !40}
!847 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseFloatArrayElements", scope: !29, file: !12, line: 681, baseType: !848, size: 32, offset: 6304)
!848 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !849, size: 32)
!849 = !DISubroutineType(types: !850)
!850 = !{null, !25, !778, !817, !40}
!851 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseDoubleArrayElements", scope: !29, file: !12, line: 683, baseType: !852, size: 32, offset: 6336)
!852 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !853, size: 32)
!853 = !DISubroutineType(types: !854)
!854 = !{null, !25, !783, !822, !40}
!855 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayRegion", scope: !29, file: !12, line: 686, baseType: !856, size: 32, offset: 6368)
!856 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !857, size: 32)
!857 = !DISubroutineType(types: !858)
!858 = !{null, !25, !748, !58, !58, !708}
!859 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayRegion", scope: !29, file: !12, line: 688, baseType: !860, size: 32, offset: 6400)
!860 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !861, size: 32)
!861 = !DISubroutineType(types: !862)
!862 = !{null, !25, !753, !58, !58, !792}
!863 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayRegion", scope: !29, file: !12, line: 690, baseType: !864, size: 32, offset: 6432)
!864 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !865, size: 32)
!865 = !DISubroutineType(types: !866)
!866 = !{null, !25, !758, !58, !58, !797}
!867 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayRegion", scope: !29, file: !12, line: 692, baseType: !868, size: 32, offset: 6464)
!868 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !869, size: 32)
!869 = !DISubroutineType(types: !870)
!870 = !{null, !25, !763, !58, !58, !802}
!871 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayRegion", scope: !29, file: !12, line: 694, baseType: !872, size: 32, offset: 6496)
!872 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !873, size: 32)
!873 = !DISubroutineType(types: !874)
!874 = !{null, !25, !768, !58, !58, !807}
!875 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayRegion", scope: !29, file: !12, line: 696, baseType: !876, size: 32, offset: 6528)
!876 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !877, size: 32)
!877 = !DISubroutineType(types: !878)
!878 = !{null, !25, !773, !58, !58, !812}
!879 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayRegion", scope: !29, file: !12, line: 698, baseType: !880, size: 32, offset: 6560)
!880 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !881, size: 32)
!881 = !DISubroutineType(types: !882)
!882 = !{null, !25, !778, !58, !58, !817}
!883 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayRegion", scope: !29, file: !12, line: 700, baseType: !884, size: 32, offset: 6592)
!884 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !885, size: 32)
!885 = !DISubroutineType(types: !886)
!886 = !{null, !25, !783, !58, !58, !822}
!887 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanArrayRegion", scope: !29, file: !12, line: 703, baseType: !888, size: 32, offset: 6624)
!888 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !889, size: 32)
!889 = !DISubroutineType(types: !890)
!890 = !{null, !25, !748, !58, !58, !891}
!891 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !892, size: 32)
!892 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !81)
!893 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteArrayRegion", scope: !29, file: !12, line: 705, baseType: !894, size: 32, offset: 6656)
!894 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !895, size: 32)
!895 = !DISubroutineType(types: !896)
!896 = !{null, !25, !753, !58, !58, !54}
!897 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharArrayRegion", scope: !29, file: !12, line: 707, baseType: !898, size: 32, offset: 6688)
!898 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !899, size: 32)
!899 = !DISubroutineType(types: !900)
!900 = !{null, !25, !758, !58, !58, !698}
!901 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortArrayRegion", scope: !29, file: !12, line: 709, baseType: !902, size: 32, offset: 6720)
!902 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !903, size: 32)
!903 = !DISubroutineType(types: !904)
!904 = !{null, !25, !763, !58, !58, !905}
!905 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !906, size: 32)
!906 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !167)
!907 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntArrayRegion", scope: !29, file: !12, line: 711, baseType: !908, size: 32, offset: 6752)
!908 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !909, size: 32)
!909 = !DISubroutineType(types: !910)
!910 = !{null, !25, !768, !58, !58, !911}
!911 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !912, size: 32)
!912 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !40)
!913 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongArrayRegion", scope: !29, file: !12, line: 713, baseType: !914, size: 32, offset: 6784)
!914 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !915, size: 32)
!915 = !DISubroutineType(types: !916)
!916 = !{null, !25, !773, !58, !58, !917}
!917 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !918, size: 32)
!918 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !171)
!919 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatArrayRegion", scope: !29, file: !12, line: 715, baseType: !920, size: 32, offset: 6816)
!920 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !921, size: 32)
!921 = !DISubroutineType(types: !922)
!922 = !{null, !25, !778, !58, !58, !923}
!923 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !924, size: 32)
!924 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !174)
!925 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleArrayRegion", scope: !29, file: !12, line: 717, baseType: !926, size: 32, offset: 6848)
!926 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !927, size: 32)
!927 = !DISubroutineType(types: !928)
!928 = !{null, !25, !783, !58, !58, !929}
!929 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !930, size: 32)
!930 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !177)
!931 = !DIDerivedType(tag: DW_TAG_member, name: "RegisterNatives", scope: !29, file: !12, line: 720, baseType: !932, size: 32, offset: 6880)
!932 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !933, size: 32)
!933 = !DISubroutineType(types: !934)
!934 = !{!40, !25, !47, !935, !40}
!935 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !936, size: 32)
!936 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !937)
!937 = !DIDerivedType(tag: DW_TAG_typedef, name: "JNINativeMethod", file: !12, line: 184, baseType: !938)
!938 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNINativeMethod", file: !12, line: 180, size: 96, elements: !939)
!939 = !{!940, !941, !942}
!940 = !DIDerivedType(tag: DW_TAG_member, name: "name", scope: !938, file: !12, line: 181, baseType: !151, size: 32)
!941 = !DIDerivedType(tag: DW_TAG_member, name: "signature", scope: !938, file: !12, line: 182, baseType: !151, size: 32, offset: 32)
!942 = !DIDerivedType(tag: DW_TAG_member, name: "fnPtr", scope: !938, file: !12, line: 183, baseType: !32, size: 32, offset: 64)
!943 = !DIDerivedType(tag: DW_TAG_member, name: "UnregisterNatives", scope: !29, file: !12, line: 723, baseType: !944, size: 32, offset: 6912)
!944 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !945, size: 32)
!945 = !DISubroutineType(types: !946)
!946 = !{!40, !25, !47}
!947 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorEnter", scope: !29, file: !12, line: 726, baseType: !948, size: 32, offset: 6944)
!948 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !949, size: 32)
!949 = !DISubroutineType(types: !950)
!950 = !{!40, !25, !48}
!951 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorExit", scope: !29, file: !12, line: 728, baseType: !948, size: 32, offset: 6976)
!952 = !DIDerivedType(tag: DW_TAG_member, name: "GetJavaVM", scope: !29, file: !12, line: 731, baseType: !953, size: 32, offset: 7008)
!953 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !954, size: 32)
!954 = !DISubroutineType(types: !955)
!955 = !{!40, !25, !956}
!956 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !957, size: 32)
!957 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !958, size: 32)
!958 = !DIDerivedType(tag: DW_TAG_typedef, name: "JavaVM", file: !12, line: 211, baseType: !959)
!959 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !960, size: 32)
!960 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !961)
!961 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNIInvokeInterface_", file: !12, line: 1890, size: 256, elements: !962)
!962 = !{!963, !964, !965, !966, !970, !975, !976, !980}
!963 = !DIDerivedType(tag: DW_TAG_member, name: "reserved0", scope: !961, file: !12, line: 1891, baseType: !32, size: 32)
!964 = !DIDerivedType(tag: DW_TAG_member, name: "reserved1", scope: !961, file: !12, line: 1892, baseType: !32, size: 32, offset: 32)
!965 = !DIDerivedType(tag: DW_TAG_member, name: "reserved2", scope: !961, file: !12, line: 1893, baseType: !32, size: 32, offset: 64)
!966 = !DIDerivedType(tag: DW_TAG_member, name: "DestroyJavaVM", scope: !961, file: !12, line: 1895, baseType: !967, size: 32, offset: 96)
!967 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !968, size: 32)
!968 = !DISubroutineType(types: !969)
!969 = !{!40, !957}
!970 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThread", scope: !961, file: !12, line: 1897, baseType: !971, size: 32, offset: 128)
!971 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !972, size: 32)
!972 = !DISubroutineType(types: !973)
!973 = !{!40, !957, !974, !32}
!974 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !32, size: 32)
!975 = !DIDerivedType(tag: DW_TAG_member, name: "DetachCurrentThread", scope: !961, file: !12, line: 1899, baseType: !967, size: 32, offset: 160)
!976 = !DIDerivedType(tag: DW_TAG_member, name: "GetEnv", scope: !961, file: !12, line: 1901, baseType: !977, size: 32, offset: 192)
!977 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !978, size: 32)
!978 = !DISubroutineType(types: !979)
!979 = !{!40, !957, !974, !40}
!980 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThreadAsDaemon", scope: !961, file: !12, line: 1903, baseType: !971, size: 32, offset: 224)
!981 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringRegion", scope: !29, file: !12, line: 734, baseType: !982, size: 32, offset: 7040)
!982 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !983, size: 32)
!983 = !DISubroutineType(types: !984)
!984 = !{null, !25, !697, !58, !58, !797}
!985 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFRegion", scope: !29, file: !12, line: 736, baseType: !986, size: 32, offset: 7072)
!986 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !987, size: 32)
!987 = !DISubroutineType(types: !988)
!988 = !{null, !25, !697, !58, !58, !151}
!989 = !DIDerivedType(tag: DW_TAG_member, name: "GetPrimitiveArrayCritical", scope: !29, file: !12, line: 739, baseType: !990, size: 32, offset: 7104)
!990 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !991, size: 32)
!991 = !DISubroutineType(types: !992)
!992 = !{!32, !25, !730, !708}
!993 = !DIDerivedType(tag: DW_TAG_member, name: "ReleasePrimitiveArrayCritical", scope: !29, file: !12, line: 741, baseType: !994, size: 32, offset: 7136)
!994 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !995, size: 32)
!995 = !DISubroutineType(types: !996)
!996 = !{null, !25, !730, !32, !40}
!997 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringCritical", scope: !29, file: !12, line: 744, baseType: !705, size: 32, offset: 7168)
!998 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringCritical", scope: !29, file: !12, line: 746, baseType: !710, size: 32, offset: 7200)
!999 = !DIDerivedType(tag: DW_TAG_member, name: "NewWeakGlobalRef", scope: !29, file: !12, line: 749, baseType: !1000, size: 32, offset: 7232)
!1000 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1001, size: 32)
!1001 = !DISubroutineType(types: !1002)
!1002 = !{!1003, !25, !48}
!1003 = !DIDerivedType(tag: DW_TAG_typedef, name: "jweak", file: !12, line: 118, baseType: !48)
!1004 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteWeakGlobalRef", scope: !29, file: !12, line: 751, baseType: !1005, size: 32, offset: 7264)
!1005 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1006, size: 32)
!1006 = !DISubroutineType(types: !1007)
!1007 = !{null, !25, !1003}
!1008 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionCheck", scope: !29, file: !12, line: 754, baseType: !1009, size: 32, offset: 7296)
!1009 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1010, size: 32)
!1010 = !DISubroutineType(types: !1011)
!1011 = !{!81, !25}
!1012 = !DIDerivedType(tag: DW_TAG_member, name: "NewDirectByteBuffer", scope: !29, file: !12, line: 757, baseType: !1013, size: 32, offset: 7328)
!1013 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1014, size: 32)
!1014 = !DISubroutineType(types: !1015)
!1015 = !{!48, !25, !32, !171}
!1016 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferAddress", scope: !29, file: !12, line: 759, baseType: !1017, size: 32, offset: 7360)
!1017 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1018, size: 32)
!1018 = !DISubroutineType(types: !1019)
!1019 = !{!32, !25, !48}
!1020 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferCapacity", scope: !29, file: !12, line: 761, baseType: !1021, size: 32, offset: 7392)
!1021 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1022, size: 32)
!1022 = !DISubroutineType(types: !1023)
!1023 = !{!171, !25, !48}
!1024 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectRefType", scope: !29, file: !12, line: 766, baseType: !1025, size: 32, offset: 7424)
!1025 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1026, size: 32)
!1026 = !DISubroutineType(types: !1027)
!1027 = !{!1028, !25, !48}
!1028 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectRefType", file: !12, line: 144, baseType: !11)
!1029 = !DIDerivedType(tag: DW_TAG_typedef, name: "size_t", file: !1030, line: 197, baseType: !1031)
!1030 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vcruntime.h", directory: "", checksumkind: CSK_MD5, checksum: "39da3a8c8438e40538f3964bd55ef6b8")
!1031 = !DIBasicType(name: "unsigned int", size: 32, encoding: DW_ATE_unsigned)
!1032 = !{!0}
!1033 = !{}
!1034 = !{i32 2, !"CodeView", i32 1}
!1035 = !{i32 2, !"Debug Info Version", i32 3}
!1036 = !{i32 1, !"wchar_size", i32 2}
!1037 = !{i32 1, !"min_enum_size", i32 4}
!1038 = !{i32 7, !"uwtable", i32 2}
!1039 = !{i32 7, !"frame-pointer", i32 2}
!1040 = !{!"clang version 15.0.2"}
!1041 = distinct !DISubprogram(name: "sprintf", scope: !1042, file: !1042, line: 1764, type: !1043, scopeLine: 1771, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1042 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\stdio.h", directory: "", checksumkind: CSK_MD5, checksum: "c1a1fbc43e7d45f0ea4ae539ddcffb19")
!1043 = !DISubroutineType(types: !1044)
!1044 = !{!13, !1045, !1046, null}
!1045 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !151)
!1046 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !51)
!1047 = !DILocalVariable(name: "_Format", arg: 2, scope: !1041, file: !1042, line: 1766, type: !1046)
!1048 = !DILocation(line: 1766, scope: !1041)
!1049 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1041, file: !1042, line: 1765, type: !1045)
!1050 = !DILocation(line: 1765, scope: !1041)
!1051 = !DILocalVariable(name: "_Result", scope: !1041, file: !1042, line: 1772, type: !13)
!1052 = !DILocation(line: 1772, scope: !1041)
!1053 = !DILocalVariable(name: "_ArgList", scope: !1041, file: !1042, line: 1773, type: !149)
!1054 = !DILocation(line: 1773, scope: !1041)
!1055 = !DILocation(line: 1774, scope: !1041)
!1056 = !DILocation(line: 1776, scope: !1041)
!1057 = !DILocation(line: 1778, scope: !1041)
!1058 = !DILocation(line: 1779, scope: !1041)
!1059 = distinct !DISubprogram(name: "vsprintf", scope: !1042, file: !1042, line: 1465, type: !1060, scopeLine: 1473, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1060 = !DISubroutineType(types: !1061)
!1061 = !{!13, !1045, !1046, !149}
!1062 = !DILocalVariable(name: "_ArgList", arg: 3, scope: !1059, file: !1042, line: 1468, type: !149)
!1063 = !DILocation(line: 1468, scope: !1059)
!1064 = !DILocalVariable(name: "_Format", arg: 2, scope: !1059, file: !1042, line: 1467, type: !1046)
!1065 = !DILocation(line: 1467, scope: !1059)
!1066 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1059, file: !1042, line: 1466, type: !1045)
!1067 = !DILocation(line: 1466, scope: !1059)
!1068 = !DILocation(line: 1474, scope: !1059)
!1069 = distinct !DISubprogram(name: "_snprintf", scope: !1042, file: !1042, line: 1939, type: !1070, scopeLine: 1947, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1070 = !DISubroutineType(types: !1071)
!1071 = !{!13, !1045, !1072, !1046, null}
!1072 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !1029)
!1073 = !DILocalVariable(name: "_Format", arg: 3, scope: !1069, file: !1042, line: 1942, type: !1046)
!1074 = !DILocation(line: 1942, scope: !1069)
!1075 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1069, file: !1042, line: 1941, type: !1072)
!1076 = !DILocation(line: 1941, scope: !1069)
!1077 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1069, file: !1042, line: 1940, type: !1045)
!1078 = !DILocation(line: 1940, scope: !1069)
!1079 = !DILocalVariable(name: "_Result", scope: !1069, file: !1042, line: 1948, type: !13)
!1080 = !DILocation(line: 1948, scope: !1069)
!1081 = !DILocalVariable(name: "_ArgList", scope: !1069, file: !1042, line: 1949, type: !149)
!1082 = !DILocation(line: 1949, scope: !1069)
!1083 = !DILocation(line: 1950, scope: !1069)
!1084 = !DILocation(line: 1951, scope: !1069)
!1085 = !DILocation(line: 1952, scope: !1069)
!1086 = !DILocation(line: 1953, scope: !1069)
!1087 = distinct !DISubprogram(name: "_vsnprintf", scope: !1042, file: !1042, line: 1402, type: !1088, scopeLine: 1411, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1088 = !DISubroutineType(types: !1089)
!1089 = !{!13, !1045, !1072, !1046, !149}
!1090 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !1087, file: !1042, line: 1406, type: !149)
!1091 = !DILocation(line: 1406, scope: !1087)
!1092 = !DILocalVariable(name: "_Format", arg: 3, scope: !1087, file: !1042, line: 1405, type: !1046)
!1093 = !DILocation(line: 1405, scope: !1087)
!1094 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1087, file: !1042, line: 1404, type: !1072)
!1095 = !DILocation(line: 1404, scope: !1087)
!1096 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1087, file: !1042, line: 1403, type: !1045)
!1097 = !DILocation(line: 1403, scope: !1087)
!1098 = !DILocation(line: 1412, scope: !1087)
!1099 = distinct !DISubprogram(name: "JNI_CallObjectMethod", scope: !9, file: !9, line: 3, type: !194, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1100 = !DILocalVariable(name: "methodID", arg: 3, scope: !1099, file: !9, line: 3, type: !67)
!1101 = !DILocation(line: 3, scope: !1099)
!1102 = !DILocalVariable(name: "obj", arg: 2, scope: !1099, file: !9, line: 3, type: !48)
!1103 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1099, file: !9, line: 3, type: !25)
!1104 = !DILocalVariable(name: "args", scope: !1099, file: !9, line: 3, type: !149)
!1105 = !DILocalVariable(name: "sig", scope: !1099, file: !9, line: 3, type: !1106)
!1106 = !DICompositeType(tag: DW_TAG_array_type, baseType: !53, size: 2048, elements: !1107)
!1107 = !{!1108}
!1108 = !DISubrange(count: 256)
!1109 = !DILocalVariable(name: "argc", scope: !1099, file: !9, line: 3, type: !13)
!1110 = !DILocalVariable(name: "argv", scope: !1099, file: !9, line: 3, type: !1111)
!1111 = !DICompositeType(tag: DW_TAG_array_type, baseType: !158, size: 16384, elements: !1107)
!1112 = !DILocalVariable(name: "i", scope: !1113, file: !9, line: 3, type: !13)
!1113 = distinct !DILexicalBlock(scope: !1099, file: !9, line: 3)
!1114 = !DILocation(line: 3, scope: !1113)
!1115 = !DILocation(line: 3, scope: !1116)
!1116 = distinct !DILexicalBlock(scope: !1117, file: !9, line: 3)
!1117 = distinct !DILexicalBlock(scope: !1113, file: !9, line: 3)
!1118 = !DILocation(line: 3, scope: !1119)
!1119 = distinct !DILexicalBlock(scope: !1116, file: !9, line: 3)
!1120 = !DILocation(line: 3, scope: !1117)
!1121 = distinct !{!1121, !1114, !1114, !1122}
!1122 = !{!"llvm.loop.mustprogress"}
!1123 = !DILocalVariable(name: "ret", scope: !1099, file: !9, line: 3, type: !48)
!1124 = distinct !DISubprogram(name: "JNI_CallObjectMethodV", scope: !9, file: !9, line: 3, type: !198, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1125 = !DILocalVariable(name: "args", arg: 4, scope: !1124, file: !9, line: 3, type: !149)
!1126 = !DILocation(line: 3, scope: !1124)
!1127 = !DILocalVariable(name: "methodID", arg: 3, scope: !1124, file: !9, line: 3, type: !67)
!1128 = !DILocalVariable(name: "obj", arg: 2, scope: !1124, file: !9, line: 3, type: !48)
!1129 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1124, file: !9, line: 3, type: !25)
!1130 = !DILocalVariable(name: "sig", scope: !1124, file: !9, line: 3, type: !1106)
!1131 = !DILocalVariable(name: "argc", scope: !1124, file: !9, line: 3, type: !13)
!1132 = !DILocalVariable(name: "argv", scope: !1124, file: !9, line: 3, type: !1111)
!1133 = !DILocalVariable(name: "i", scope: !1134, file: !9, line: 3, type: !13)
!1134 = distinct !DILexicalBlock(scope: !1124, file: !9, line: 3)
!1135 = !DILocation(line: 3, scope: !1134)
!1136 = !DILocation(line: 3, scope: !1137)
!1137 = distinct !DILexicalBlock(scope: !1138, file: !9, line: 3)
!1138 = distinct !DILexicalBlock(scope: !1134, file: !9, line: 3)
!1139 = !DILocation(line: 3, scope: !1140)
!1140 = distinct !DILexicalBlock(scope: !1137, file: !9, line: 3)
!1141 = !DILocation(line: 3, scope: !1138)
!1142 = distinct !{!1142, !1135, !1135, !1122}
!1143 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethod", scope: !9, file: !9, line: 3, type: !314, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1144 = !DILocalVariable(name: "methodID", arg: 4, scope: !1143, file: !9, line: 3, type: !67)
!1145 = !DILocation(line: 3, scope: !1143)
!1146 = !DILocalVariable(name: "clazz", arg: 3, scope: !1143, file: !9, line: 3, type: !47)
!1147 = !DILocalVariable(name: "obj", arg: 2, scope: !1143, file: !9, line: 3, type: !48)
!1148 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1143, file: !9, line: 3, type: !25)
!1149 = !DILocalVariable(name: "args", scope: !1143, file: !9, line: 3, type: !149)
!1150 = !DILocalVariable(name: "sig", scope: !1143, file: !9, line: 3, type: !1106)
!1151 = !DILocalVariable(name: "argc", scope: !1143, file: !9, line: 3, type: !13)
!1152 = !DILocalVariable(name: "argv", scope: !1143, file: !9, line: 3, type: !1111)
!1153 = !DILocalVariable(name: "i", scope: !1154, file: !9, line: 3, type: !13)
!1154 = distinct !DILexicalBlock(scope: !1143, file: !9, line: 3)
!1155 = !DILocation(line: 3, scope: !1154)
!1156 = !DILocation(line: 3, scope: !1157)
!1157 = distinct !DILexicalBlock(scope: !1158, file: !9, line: 3)
!1158 = distinct !DILexicalBlock(scope: !1154, file: !9, line: 3)
!1159 = !DILocation(line: 3, scope: !1160)
!1160 = distinct !DILexicalBlock(scope: !1157, file: !9, line: 3)
!1161 = !DILocation(line: 3, scope: !1158)
!1162 = distinct !{!1162, !1155, !1155, !1122}
!1163 = !DILocalVariable(name: "ret", scope: !1143, file: !9, line: 3, type: !48)
!1164 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethodV", scope: !9, file: !9, line: 3, type: !318, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1165 = !DILocalVariable(name: "args", arg: 5, scope: !1164, file: !9, line: 3, type: !149)
!1166 = !DILocation(line: 3, scope: !1164)
!1167 = !DILocalVariable(name: "methodID", arg: 4, scope: !1164, file: !9, line: 3, type: !67)
!1168 = !DILocalVariable(name: "clazz", arg: 3, scope: !1164, file: !9, line: 3, type: !47)
!1169 = !DILocalVariable(name: "obj", arg: 2, scope: !1164, file: !9, line: 3, type: !48)
!1170 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1164, file: !9, line: 3, type: !25)
!1171 = !DILocalVariable(name: "sig", scope: !1164, file: !9, line: 3, type: !1106)
!1172 = !DILocalVariable(name: "argc", scope: !1164, file: !9, line: 3, type: !13)
!1173 = !DILocalVariable(name: "argv", scope: !1164, file: !9, line: 3, type: !1111)
!1174 = !DILocalVariable(name: "i", scope: !1175, file: !9, line: 3, type: !13)
!1175 = distinct !DILexicalBlock(scope: !1164, file: !9, line: 3)
!1176 = !DILocation(line: 3, scope: !1175)
!1177 = !DILocation(line: 3, scope: !1178)
!1178 = distinct !DILexicalBlock(scope: !1179, file: !9, line: 3)
!1179 = distinct !DILexicalBlock(scope: !1175, file: !9, line: 3)
!1180 = !DILocation(line: 3, scope: !1181)
!1181 = distinct !DILexicalBlock(scope: !1178, file: !9, line: 3)
!1182 = !DILocation(line: 3, scope: !1179)
!1183 = distinct !{!1183, !1176, !1176, !1122}
!1184 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethod", scope: !9, file: !9, line: 3, type: !143, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1185 = !DILocalVariable(name: "methodID", arg: 3, scope: !1184, file: !9, line: 3, type: !67)
!1186 = !DILocation(line: 3, scope: !1184)
!1187 = !DILocalVariable(name: "clazz", arg: 2, scope: !1184, file: !9, line: 3, type: !47)
!1188 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1184, file: !9, line: 3, type: !25)
!1189 = !DILocalVariable(name: "args", scope: !1184, file: !9, line: 3, type: !149)
!1190 = !DILocalVariable(name: "sig", scope: !1184, file: !9, line: 3, type: !1106)
!1191 = !DILocalVariable(name: "argc", scope: !1184, file: !9, line: 3, type: !13)
!1192 = !DILocalVariable(name: "argv", scope: !1184, file: !9, line: 3, type: !1111)
!1193 = !DILocalVariable(name: "i", scope: !1194, file: !9, line: 3, type: !13)
!1194 = distinct !DILexicalBlock(scope: !1184, file: !9, line: 3)
!1195 = !DILocation(line: 3, scope: !1194)
!1196 = !DILocation(line: 3, scope: !1197)
!1197 = distinct !DILexicalBlock(scope: !1198, file: !9, line: 3)
!1198 = distinct !DILexicalBlock(scope: !1194, file: !9, line: 3)
!1199 = !DILocation(line: 3, scope: !1200)
!1200 = distinct !DILexicalBlock(scope: !1197, file: !9, line: 3)
!1201 = !DILocation(line: 3, scope: !1198)
!1202 = distinct !{!1202, !1195, !1195, !1122}
!1203 = !DILocalVariable(name: "ret", scope: !1184, file: !9, line: 3, type: !48)
!1204 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethodV", scope: !9, file: !9, line: 3, type: !147, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1205 = !DILocalVariable(name: "args", arg: 4, scope: !1204, file: !9, line: 3, type: !149)
!1206 = !DILocation(line: 3, scope: !1204)
!1207 = !DILocalVariable(name: "methodID", arg: 3, scope: !1204, file: !9, line: 3, type: !67)
!1208 = !DILocalVariable(name: "clazz", arg: 2, scope: !1204, file: !9, line: 3, type: !47)
!1209 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1204, file: !9, line: 3, type: !25)
!1210 = !DILocalVariable(name: "sig", scope: !1204, file: !9, line: 3, type: !1106)
!1211 = !DILocalVariable(name: "argc", scope: !1204, file: !9, line: 3, type: !13)
!1212 = !DILocalVariable(name: "argv", scope: !1204, file: !9, line: 3, type: !1111)
!1213 = !DILocalVariable(name: "i", scope: !1214, file: !9, line: 3, type: !13)
!1214 = distinct !DILexicalBlock(scope: !1204, file: !9, line: 3)
!1215 = !DILocation(line: 3, scope: !1214)
!1216 = !DILocation(line: 3, scope: !1217)
!1217 = distinct !DILexicalBlock(scope: !1218, file: !9, line: 3)
!1218 = distinct !DILexicalBlock(scope: !1214, file: !9, line: 3)
!1219 = !DILocation(line: 3, scope: !1220)
!1220 = distinct !DILexicalBlock(scope: !1217, file: !9, line: 3)
!1221 = !DILocation(line: 3, scope: !1218)
!1222 = distinct !{!1222, !1215, !1215, !1122}
!1223 = distinct !DISubprogram(name: "JNI_CallBooleanMethod", scope: !9, file: !9, line: 4, type: !206, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1224 = !DILocalVariable(name: "methodID", arg: 3, scope: !1223, file: !9, line: 4, type: !67)
!1225 = !DILocation(line: 4, scope: !1223)
!1226 = !DILocalVariable(name: "obj", arg: 2, scope: !1223, file: !9, line: 4, type: !48)
!1227 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1223, file: !9, line: 4, type: !25)
!1228 = !DILocalVariable(name: "args", scope: !1223, file: !9, line: 4, type: !149)
!1229 = !DILocalVariable(name: "sig", scope: !1223, file: !9, line: 4, type: !1106)
!1230 = !DILocalVariable(name: "argc", scope: !1223, file: !9, line: 4, type: !13)
!1231 = !DILocalVariable(name: "argv", scope: !1223, file: !9, line: 4, type: !1111)
!1232 = !DILocalVariable(name: "i", scope: !1233, file: !9, line: 4, type: !13)
!1233 = distinct !DILexicalBlock(scope: !1223, file: !9, line: 4)
!1234 = !DILocation(line: 4, scope: !1233)
!1235 = !DILocation(line: 4, scope: !1236)
!1236 = distinct !DILexicalBlock(scope: !1237, file: !9, line: 4)
!1237 = distinct !DILexicalBlock(scope: !1233, file: !9, line: 4)
!1238 = !DILocation(line: 4, scope: !1239)
!1239 = distinct !DILexicalBlock(scope: !1236, file: !9, line: 4)
!1240 = !DILocation(line: 4, scope: !1237)
!1241 = distinct !{!1241, !1234, !1234, !1122}
!1242 = !DILocalVariable(name: "ret", scope: !1223, file: !9, line: 4, type: !81)
!1243 = distinct !DISubprogram(name: "JNI_CallBooleanMethodV", scope: !9, file: !9, line: 4, type: !210, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1244 = !DILocalVariable(name: "args", arg: 4, scope: !1243, file: !9, line: 4, type: !149)
!1245 = !DILocation(line: 4, scope: !1243)
!1246 = !DILocalVariable(name: "methodID", arg: 3, scope: !1243, file: !9, line: 4, type: !67)
!1247 = !DILocalVariable(name: "obj", arg: 2, scope: !1243, file: !9, line: 4, type: !48)
!1248 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1243, file: !9, line: 4, type: !25)
!1249 = !DILocalVariable(name: "sig", scope: !1243, file: !9, line: 4, type: !1106)
!1250 = !DILocalVariable(name: "argc", scope: !1243, file: !9, line: 4, type: !13)
!1251 = !DILocalVariable(name: "argv", scope: !1243, file: !9, line: 4, type: !1111)
!1252 = !DILocalVariable(name: "i", scope: !1253, file: !9, line: 4, type: !13)
!1253 = distinct !DILexicalBlock(scope: !1243, file: !9, line: 4)
!1254 = !DILocation(line: 4, scope: !1253)
!1255 = !DILocation(line: 4, scope: !1256)
!1256 = distinct !DILexicalBlock(scope: !1257, file: !9, line: 4)
!1257 = distinct !DILexicalBlock(scope: !1253, file: !9, line: 4)
!1258 = !DILocation(line: 4, scope: !1259)
!1259 = distinct !DILexicalBlock(scope: !1256, file: !9, line: 4)
!1260 = !DILocation(line: 4, scope: !1257)
!1261 = distinct !{!1261, !1254, !1254, !1122}
!1262 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethod", scope: !9, file: !9, line: 4, type: !326, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1263 = !DILocalVariable(name: "methodID", arg: 4, scope: !1262, file: !9, line: 4, type: !67)
!1264 = !DILocation(line: 4, scope: !1262)
!1265 = !DILocalVariable(name: "clazz", arg: 3, scope: !1262, file: !9, line: 4, type: !47)
!1266 = !DILocalVariable(name: "obj", arg: 2, scope: !1262, file: !9, line: 4, type: !48)
!1267 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1262, file: !9, line: 4, type: !25)
!1268 = !DILocalVariable(name: "args", scope: !1262, file: !9, line: 4, type: !149)
!1269 = !DILocalVariable(name: "sig", scope: !1262, file: !9, line: 4, type: !1106)
!1270 = !DILocalVariable(name: "argc", scope: !1262, file: !9, line: 4, type: !13)
!1271 = !DILocalVariable(name: "argv", scope: !1262, file: !9, line: 4, type: !1111)
!1272 = !DILocalVariable(name: "i", scope: !1273, file: !9, line: 4, type: !13)
!1273 = distinct !DILexicalBlock(scope: !1262, file: !9, line: 4)
!1274 = !DILocation(line: 4, scope: !1273)
!1275 = !DILocation(line: 4, scope: !1276)
!1276 = distinct !DILexicalBlock(scope: !1277, file: !9, line: 4)
!1277 = distinct !DILexicalBlock(scope: !1273, file: !9, line: 4)
!1278 = !DILocation(line: 4, scope: !1279)
!1279 = distinct !DILexicalBlock(scope: !1276, file: !9, line: 4)
!1280 = !DILocation(line: 4, scope: !1277)
!1281 = distinct !{!1281, !1274, !1274, !1122}
!1282 = !DILocalVariable(name: "ret", scope: !1262, file: !9, line: 4, type: !81)
!1283 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethodV", scope: !9, file: !9, line: 4, type: !330, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1284 = !DILocalVariable(name: "args", arg: 5, scope: !1283, file: !9, line: 4, type: !149)
!1285 = !DILocation(line: 4, scope: !1283)
!1286 = !DILocalVariable(name: "methodID", arg: 4, scope: !1283, file: !9, line: 4, type: !67)
!1287 = !DILocalVariable(name: "clazz", arg: 3, scope: !1283, file: !9, line: 4, type: !47)
!1288 = !DILocalVariable(name: "obj", arg: 2, scope: !1283, file: !9, line: 4, type: !48)
!1289 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1283, file: !9, line: 4, type: !25)
!1290 = !DILocalVariable(name: "sig", scope: !1283, file: !9, line: 4, type: !1106)
!1291 = !DILocalVariable(name: "argc", scope: !1283, file: !9, line: 4, type: !13)
!1292 = !DILocalVariable(name: "argv", scope: !1283, file: !9, line: 4, type: !1111)
!1293 = !DILocalVariable(name: "i", scope: !1294, file: !9, line: 4, type: !13)
!1294 = distinct !DILexicalBlock(scope: !1283, file: !9, line: 4)
!1295 = !DILocation(line: 4, scope: !1294)
!1296 = !DILocation(line: 4, scope: !1297)
!1297 = distinct !DILexicalBlock(scope: !1298, file: !9, line: 4)
!1298 = distinct !DILexicalBlock(scope: !1294, file: !9, line: 4)
!1299 = !DILocation(line: 4, scope: !1300)
!1300 = distinct !DILexicalBlock(scope: !1297, file: !9, line: 4)
!1301 = !DILocation(line: 4, scope: !1298)
!1302 = distinct !{!1302, !1295, !1295, !1122}
!1303 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethod", scope: !9, file: !9, line: 4, type: !514, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1304 = !DILocalVariable(name: "methodID", arg: 3, scope: !1303, file: !9, line: 4, type: !67)
!1305 = !DILocation(line: 4, scope: !1303)
!1306 = !DILocalVariable(name: "clazz", arg: 2, scope: !1303, file: !9, line: 4, type: !47)
!1307 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1303, file: !9, line: 4, type: !25)
!1308 = !DILocalVariable(name: "args", scope: !1303, file: !9, line: 4, type: !149)
!1309 = !DILocalVariable(name: "sig", scope: !1303, file: !9, line: 4, type: !1106)
!1310 = !DILocalVariable(name: "argc", scope: !1303, file: !9, line: 4, type: !13)
!1311 = !DILocalVariable(name: "argv", scope: !1303, file: !9, line: 4, type: !1111)
!1312 = !DILocalVariable(name: "i", scope: !1313, file: !9, line: 4, type: !13)
!1313 = distinct !DILexicalBlock(scope: !1303, file: !9, line: 4)
!1314 = !DILocation(line: 4, scope: !1313)
!1315 = !DILocation(line: 4, scope: !1316)
!1316 = distinct !DILexicalBlock(scope: !1317, file: !9, line: 4)
!1317 = distinct !DILexicalBlock(scope: !1313, file: !9, line: 4)
!1318 = !DILocation(line: 4, scope: !1319)
!1319 = distinct !DILexicalBlock(scope: !1316, file: !9, line: 4)
!1320 = !DILocation(line: 4, scope: !1317)
!1321 = distinct !{!1321, !1314, !1314, !1122}
!1322 = !DILocalVariable(name: "ret", scope: !1303, file: !9, line: 4, type: !81)
!1323 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethodV", scope: !9, file: !9, line: 4, type: !518, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1324 = !DILocalVariable(name: "args", arg: 4, scope: !1323, file: !9, line: 4, type: !149)
!1325 = !DILocation(line: 4, scope: !1323)
!1326 = !DILocalVariable(name: "methodID", arg: 3, scope: !1323, file: !9, line: 4, type: !67)
!1327 = !DILocalVariable(name: "clazz", arg: 2, scope: !1323, file: !9, line: 4, type: !47)
!1328 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1323, file: !9, line: 4, type: !25)
!1329 = !DILocalVariable(name: "sig", scope: !1323, file: !9, line: 4, type: !1106)
!1330 = !DILocalVariable(name: "argc", scope: !1323, file: !9, line: 4, type: !13)
!1331 = !DILocalVariable(name: "argv", scope: !1323, file: !9, line: 4, type: !1111)
!1332 = !DILocalVariable(name: "i", scope: !1333, file: !9, line: 4, type: !13)
!1333 = distinct !DILexicalBlock(scope: !1323, file: !9, line: 4)
!1334 = !DILocation(line: 4, scope: !1333)
!1335 = !DILocation(line: 4, scope: !1336)
!1336 = distinct !DILexicalBlock(scope: !1337, file: !9, line: 4)
!1337 = distinct !DILexicalBlock(scope: !1333, file: !9, line: 4)
!1338 = !DILocation(line: 4, scope: !1339)
!1339 = distinct !DILexicalBlock(scope: !1336, file: !9, line: 4)
!1340 = !DILocation(line: 4, scope: !1337)
!1341 = distinct !{!1341, !1334, !1334, !1122}
!1342 = distinct !DISubprogram(name: "JNI_CallByteMethod", scope: !9, file: !9, line: 5, type: !218, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1343 = !DILocalVariable(name: "methodID", arg: 3, scope: !1342, file: !9, line: 5, type: !67)
!1344 = !DILocation(line: 5, scope: !1342)
!1345 = !DILocalVariable(name: "obj", arg: 2, scope: !1342, file: !9, line: 5, type: !48)
!1346 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1342, file: !9, line: 5, type: !25)
!1347 = !DILocalVariable(name: "args", scope: !1342, file: !9, line: 5, type: !149)
!1348 = !DILocalVariable(name: "sig", scope: !1342, file: !9, line: 5, type: !1106)
!1349 = !DILocalVariable(name: "argc", scope: !1342, file: !9, line: 5, type: !13)
!1350 = !DILocalVariable(name: "argv", scope: !1342, file: !9, line: 5, type: !1111)
!1351 = !DILocalVariable(name: "i", scope: !1352, file: !9, line: 5, type: !13)
!1352 = distinct !DILexicalBlock(scope: !1342, file: !9, line: 5)
!1353 = !DILocation(line: 5, scope: !1352)
!1354 = !DILocation(line: 5, scope: !1355)
!1355 = distinct !DILexicalBlock(scope: !1356, file: !9, line: 5)
!1356 = distinct !DILexicalBlock(scope: !1352, file: !9, line: 5)
!1357 = !DILocation(line: 5, scope: !1358)
!1358 = distinct !DILexicalBlock(scope: !1355, file: !9, line: 5)
!1359 = !DILocation(line: 5, scope: !1356)
!1360 = distinct !{!1360, !1353, !1353, !1122}
!1361 = !DILocalVariable(name: "ret", scope: !1342, file: !9, line: 5, type: !56)
!1362 = distinct !DISubprogram(name: "JNI_CallByteMethodV", scope: !9, file: !9, line: 5, type: !222, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1363 = !DILocalVariable(name: "args", arg: 4, scope: !1362, file: !9, line: 5, type: !149)
!1364 = !DILocation(line: 5, scope: !1362)
!1365 = !DILocalVariable(name: "methodID", arg: 3, scope: !1362, file: !9, line: 5, type: !67)
!1366 = !DILocalVariable(name: "obj", arg: 2, scope: !1362, file: !9, line: 5, type: !48)
!1367 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1362, file: !9, line: 5, type: !25)
!1368 = !DILocalVariable(name: "sig", scope: !1362, file: !9, line: 5, type: !1106)
!1369 = !DILocalVariable(name: "argc", scope: !1362, file: !9, line: 5, type: !13)
!1370 = !DILocalVariable(name: "argv", scope: !1362, file: !9, line: 5, type: !1111)
!1371 = !DILocalVariable(name: "i", scope: !1372, file: !9, line: 5, type: !13)
!1372 = distinct !DILexicalBlock(scope: !1362, file: !9, line: 5)
!1373 = !DILocation(line: 5, scope: !1372)
!1374 = !DILocation(line: 5, scope: !1375)
!1375 = distinct !DILexicalBlock(scope: !1376, file: !9, line: 5)
!1376 = distinct !DILexicalBlock(scope: !1372, file: !9, line: 5)
!1377 = !DILocation(line: 5, scope: !1378)
!1378 = distinct !DILexicalBlock(scope: !1375, file: !9, line: 5)
!1379 = !DILocation(line: 5, scope: !1376)
!1380 = distinct !{!1380, !1373, !1373, !1122}
!1381 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethod", scope: !9, file: !9, line: 5, type: !338, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1382 = !DILocalVariable(name: "methodID", arg: 4, scope: !1381, file: !9, line: 5, type: !67)
!1383 = !DILocation(line: 5, scope: !1381)
!1384 = !DILocalVariable(name: "clazz", arg: 3, scope: !1381, file: !9, line: 5, type: !47)
!1385 = !DILocalVariable(name: "obj", arg: 2, scope: !1381, file: !9, line: 5, type: !48)
!1386 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1381, file: !9, line: 5, type: !25)
!1387 = !DILocalVariable(name: "args", scope: !1381, file: !9, line: 5, type: !149)
!1388 = !DILocalVariable(name: "sig", scope: !1381, file: !9, line: 5, type: !1106)
!1389 = !DILocalVariable(name: "argc", scope: !1381, file: !9, line: 5, type: !13)
!1390 = !DILocalVariable(name: "argv", scope: !1381, file: !9, line: 5, type: !1111)
!1391 = !DILocalVariable(name: "i", scope: !1392, file: !9, line: 5, type: !13)
!1392 = distinct !DILexicalBlock(scope: !1381, file: !9, line: 5)
!1393 = !DILocation(line: 5, scope: !1392)
!1394 = !DILocation(line: 5, scope: !1395)
!1395 = distinct !DILexicalBlock(scope: !1396, file: !9, line: 5)
!1396 = distinct !DILexicalBlock(scope: !1392, file: !9, line: 5)
!1397 = !DILocation(line: 5, scope: !1398)
!1398 = distinct !DILexicalBlock(scope: !1395, file: !9, line: 5)
!1399 = !DILocation(line: 5, scope: !1396)
!1400 = distinct !{!1400, !1393, !1393, !1122}
!1401 = !DILocalVariable(name: "ret", scope: !1381, file: !9, line: 5, type: !56)
!1402 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethodV", scope: !9, file: !9, line: 5, type: !342, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1403 = !DILocalVariable(name: "args", arg: 5, scope: !1402, file: !9, line: 5, type: !149)
!1404 = !DILocation(line: 5, scope: !1402)
!1405 = !DILocalVariable(name: "methodID", arg: 4, scope: !1402, file: !9, line: 5, type: !67)
!1406 = !DILocalVariable(name: "clazz", arg: 3, scope: !1402, file: !9, line: 5, type: !47)
!1407 = !DILocalVariable(name: "obj", arg: 2, scope: !1402, file: !9, line: 5, type: !48)
!1408 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1402, file: !9, line: 5, type: !25)
!1409 = !DILocalVariable(name: "sig", scope: !1402, file: !9, line: 5, type: !1106)
!1410 = !DILocalVariable(name: "argc", scope: !1402, file: !9, line: 5, type: !13)
!1411 = !DILocalVariable(name: "argv", scope: !1402, file: !9, line: 5, type: !1111)
!1412 = !DILocalVariable(name: "i", scope: !1413, file: !9, line: 5, type: !13)
!1413 = distinct !DILexicalBlock(scope: !1402, file: !9, line: 5)
!1414 = !DILocation(line: 5, scope: !1413)
!1415 = !DILocation(line: 5, scope: !1416)
!1416 = distinct !DILexicalBlock(scope: !1417, file: !9, line: 5)
!1417 = distinct !DILexicalBlock(scope: !1413, file: !9, line: 5)
!1418 = !DILocation(line: 5, scope: !1419)
!1419 = distinct !DILexicalBlock(scope: !1416, file: !9, line: 5)
!1420 = !DILocation(line: 5, scope: !1417)
!1421 = distinct !{!1421, !1414, !1414, !1122}
!1422 = distinct !DISubprogram(name: "JNI_CallStaticByteMethod", scope: !9, file: !9, line: 5, type: !526, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1423 = !DILocalVariable(name: "methodID", arg: 3, scope: !1422, file: !9, line: 5, type: !67)
!1424 = !DILocation(line: 5, scope: !1422)
!1425 = !DILocalVariable(name: "clazz", arg: 2, scope: !1422, file: !9, line: 5, type: !47)
!1426 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1422, file: !9, line: 5, type: !25)
!1427 = !DILocalVariable(name: "args", scope: !1422, file: !9, line: 5, type: !149)
!1428 = !DILocalVariable(name: "sig", scope: !1422, file: !9, line: 5, type: !1106)
!1429 = !DILocalVariable(name: "argc", scope: !1422, file: !9, line: 5, type: !13)
!1430 = !DILocalVariable(name: "argv", scope: !1422, file: !9, line: 5, type: !1111)
!1431 = !DILocalVariable(name: "i", scope: !1432, file: !9, line: 5, type: !13)
!1432 = distinct !DILexicalBlock(scope: !1422, file: !9, line: 5)
!1433 = !DILocation(line: 5, scope: !1432)
!1434 = !DILocation(line: 5, scope: !1435)
!1435 = distinct !DILexicalBlock(scope: !1436, file: !9, line: 5)
!1436 = distinct !DILexicalBlock(scope: !1432, file: !9, line: 5)
!1437 = !DILocation(line: 5, scope: !1438)
!1438 = distinct !DILexicalBlock(scope: !1435, file: !9, line: 5)
!1439 = !DILocation(line: 5, scope: !1436)
!1440 = distinct !{!1440, !1433, !1433, !1122}
!1441 = !DILocalVariable(name: "ret", scope: !1422, file: !9, line: 5, type: !56)
!1442 = distinct !DISubprogram(name: "JNI_CallStaticByteMethodV", scope: !9, file: !9, line: 5, type: !530, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1443 = !DILocalVariable(name: "args", arg: 4, scope: !1442, file: !9, line: 5, type: !149)
!1444 = !DILocation(line: 5, scope: !1442)
!1445 = !DILocalVariable(name: "methodID", arg: 3, scope: !1442, file: !9, line: 5, type: !67)
!1446 = !DILocalVariable(name: "clazz", arg: 2, scope: !1442, file: !9, line: 5, type: !47)
!1447 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1442, file: !9, line: 5, type: !25)
!1448 = !DILocalVariable(name: "sig", scope: !1442, file: !9, line: 5, type: !1106)
!1449 = !DILocalVariable(name: "argc", scope: !1442, file: !9, line: 5, type: !13)
!1450 = !DILocalVariable(name: "argv", scope: !1442, file: !9, line: 5, type: !1111)
!1451 = !DILocalVariable(name: "i", scope: !1452, file: !9, line: 5, type: !13)
!1452 = distinct !DILexicalBlock(scope: !1442, file: !9, line: 5)
!1453 = !DILocation(line: 5, scope: !1452)
!1454 = !DILocation(line: 5, scope: !1455)
!1455 = distinct !DILexicalBlock(scope: !1456, file: !9, line: 5)
!1456 = distinct !DILexicalBlock(scope: !1452, file: !9, line: 5)
!1457 = !DILocation(line: 5, scope: !1458)
!1458 = distinct !DILexicalBlock(scope: !1455, file: !9, line: 5)
!1459 = !DILocation(line: 5, scope: !1456)
!1460 = distinct !{!1460, !1453, !1453, !1122}
!1461 = distinct !DISubprogram(name: "JNI_CallCharMethod", scope: !9, file: !9, line: 6, type: !230, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1462 = !DILocalVariable(name: "methodID", arg: 3, scope: !1461, file: !9, line: 6, type: !67)
!1463 = !DILocation(line: 6, scope: !1461)
!1464 = !DILocalVariable(name: "obj", arg: 2, scope: !1461, file: !9, line: 6, type: !48)
!1465 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1461, file: !9, line: 6, type: !25)
!1466 = !DILocalVariable(name: "args", scope: !1461, file: !9, line: 6, type: !149)
!1467 = !DILocalVariable(name: "sig", scope: !1461, file: !9, line: 6, type: !1106)
!1468 = !DILocalVariable(name: "argc", scope: !1461, file: !9, line: 6, type: !13)
!1469 = !DILocalVariable(name: "argv", scope: !1461, file: !9, line: 6, type: !1111)
!1470 = !DILocalVariable(name: "i", scope: !1471, file: !9, line: 6, type: !13)
!1471 = distinct !DILexicalBlock(scope: !1461, file: !9, line: 6)
!1472 = !DILocation(line: 6, scope: !1471)
!1473 = !DILocation(line: 6, scope: !1474)
!1474 = distinct !DILexicalBlock(scope: !1475, file: !9, line: 6)
!1475 = distinct !DILexicalBlock(scope: !1471, file: !9, line: 6)
!1476 = !DILocation(line: 6, scope: !1477)
!1477 = distinct !DILexicalBlock(scope: !1474, file: !9, line: 6)
!1478 = !DILocation(line: 6, scope: !1475)
!1479 = distinct !{!1479, !1472, !1472, !1122}
!1480 = !DILocalVariable(name: "ret", scope: !1461, file: !9, line: 6, type: !164)
!1481 = distinct !DISubprogram(name: "JNI_CallCharMethodV", scope: !9, file: !9, line: 6, type: !234, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1482 = !DILocalVariable(name: "args", arg: 4, scope: !1481, file: !9, line: 6, type: !149)
!1483 = !DILocation(line: 6, scope: !1481)
!1484 = !DILocalVariable(name: "methodID", arg: 3, scope: !1481, file: !9, line: 6, type: !67)
!1485 = !DILocalVariable(name: "obj", arg: 2, scope: !1481, file: !9, line: 6, type: !48)
!1486 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1481, file: !9, line: 6, type: !25)
!1487 = !DILocalVariable(name: "sig", scope: !1481, file: !9, line: 6, type: !1106)
!1488 = !DILocalVariable(name: "argc", scope: !1481, file: !9, line: 6, type: !13)
!1489 = !DILocalVariable(name: "argv", scope: !1481, file: !9, line: 6, type: !1111)
!1490 = !DILocalVariable(name: "i", scope: !1491, file: !9, line: 6, type: !13)
!1491 = distinct !DILexicalBlock(scope: !1481, file: !9, line: 6)
!1492 = !DILocation(line: 6, scope: !1491)
!1493 = !DILocation(line: 6, scope: !1494)
!1494 = distinct !DILexicalBlock(scope: !1495, file: !9, line: 6)
!1495 = distinct !DILexicalBlock(scope: !1491, file: !9, line: 6)
!1496 = !DILocation(line: 6, scope: !1497)
!1497 = distinct !DILexicalBlock(scope: !1494, file: !9, line: 6)
!1498 = !DILocation(line: 6, scope: !1495)
!1499 = distinct !{!1499, !1492, !1492, !1122}
!1500 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethod", scope: !9, file: !9, line: 6, type: !350, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1501 = !DILocalVariable(name: "methodID", arg: 4, scope: !1500, file: !9, line: 6, type: !67)
!1502 = !DILocation(line: 6, scope: !1500)
!1503 = !DILocalVariable(name: "clazz", arg: 3, scope: !1500, file: !9, line: 6, type: !47)
!1504 = !DILocalVariable(name: "obj", arg: 2, scope: !1500, file: !9, line: 6, type: !48)
!1505 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1500, file: !9, line: 6, type: !25)
!1506 = !DILocalVariable(name: "args", scope: !1500, file: !9, line: 6, type: !149)
!1507 = !DILocalVariable(name: "sig", scope: !1500, file: !9, line: 6, type: !1106)
!1508 = !DILocalVariable(name: "argc", scope: !1500, file: !9, line: 6, type: !13)
!1509 = !DILocalVariable(name: "argv", scope: !1500, file: !9, line: 6, type: !1111)
!1510 = !DILocalVariable(name: "i", scope: !1511, file: !9, line: 6, type: !13)
!1511 = distinct !DILexicalBlock(scope: !1500, file: !9, line: 6)
!1512 = !DILocation(line: 6, scope: !1511)
!1513 = !DILocation(line: 6, scope: !1514)
!1514 = distinct !DILexicalBlock(scope: !1515, file: !9, line: 6)
!1515 = distinct !DILexicalBlock(scope: !1511, file: !9, line: 6)
!1516 = !DILocation(line: 6, scope: !1517)
!1517 = distinct !DILexicalBlock(scope: !1514, file: !9, line: 6)
!1518 = !DILocation(line: 6, scope: !1515)
!1519 = distinct !{!1519, !1512, !1512, !1122}
!1520 = !DILocalVariable(name: "ret", scope: !1500, file: !9, line: 6, type: !164)
!1521 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethodV", scope: !9, file: !9, line: 6, type: !354, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1522 = !DILocalVariable(name: "args", arg: 5, scope: !1521, file: !9, line: 6, type: !149)
!1523 = !DILocation(line: 6, scope: !1521)
!1524 = !DILocalVariable(name: "methodID", arg: 4, scope: !1521, file: !9, line: 6, type: !67)
!1525 = !DILocalVariable(name: "clazz", arg: 3, scope: !1521, file: !9, line: 6, type: !47)
!1526 = !DILocalVariable(name: "obj", arg: 2, scope: !1521, file: !9, line: 6, type: !48)
!1527 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1521, file: !9, line: 6, type: !25)
!1528 = !DILocalVariable(name: "sig", scope: !1521, file: !9, line: 6, type: !1106)
!1529 = !DILocalVariable(name: "argc", scope: !1521, file: !9, line: 6, type: !13)
!1530 = !DILocalVariable(name: "argv", scope: !1521, file: !9, line: 6, type: !1111)
!1531 = !DILocalVariable(name: "i", scope: !1532, file: !9, line: 6, type: !13)
!1532 = distinct !DILexicalBlock(scope: !1521, file: !9, line: 6)
!1533 = !DILocation(line: 6, scope: !1532)
!1534 = !DILocation(line: 6, scope: !1535)
!1535 = distinct !DILexicalBlock(scope: !1536, file: !9, line: 6)
!1536 = distinct !DILexicalBlock(scope: !1532, file: !9, line: 6)
!1537 = !DILocation(line: 6, scope: !1538)
!1538 = distinct !DILexicalBlock(scope: !1535, file: !9, line: 6)
!1539 = !DILocation(line: 6, scope: !1536)
!1540 = distinct !{!1540, !1533, !1533, !1122}
!1541 = distinct !DISubprogram(name: "JNI_CallStaticCharMethod", scope: !9, file: !9, line: 6, type: !538, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1542 = !DILocalVariable(name: "methodID", arg: 3, scope: !1541, file: !9, line: 6, type: !67)
!1543 = !DILocation(line: 6, scope: !1541)
!1544 = !DILocalVariable(name: "clazz", arg: 2, scope: !1541, file: !9, line: 6, type: !47)
!1545 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1541, file: !9, line: 6, type: !25)
!1546 = !DILocalVariable(name: "args", scope: !1541, file: !9, line: 6, type: !149)
!1547 = !DILocalVariable(name: "sig", scope: !1541, file: !9, line: 6, type: !1106)
!1548 = !DILocalVariable(name: "argc", scope: !1541, file: !9, line: 6, type: !13)
!1549 = !DILocalVariable(name: "argv", scope: !1541, file: !9, line: 6, type: !1111)
!1550 = !DILocalVariable(name: "i", scope: !1551, file: !9, line: 6, type: !13)
!1551 = distinct !DILexicalBlock(scope: !1541, file: !9, line: 6)
!1552 = !DILocation(line: 6, scope: !1551)
!1553 = !DILocation(line: 6, scope: !1554)
!1554 = distinct !DILexicalBlock(scope: !1555, file: !9, line: 6)
!1555 = distinct !DILexicalBlock(scope: !1551, file: !9, line: 6)
!1556 = !DILocation(line: 6, scope: !1557)
!1557 = distinct !DILexicalBlock(scope: !1554, file: !9, line: 6)
!1558 = !DILocation(line: 6, scope: !1555)
!1559 = distinct !{!1559, !1552, !1552, !1122}
!1560 = !DILocalVariable(name: "ret", scope: !1541, file: !9, line: 6, type: !164)
!1561 = distinct !DISubprogram(name: "JNI_CallStaticCharMethodV", scope: !9, file: !9, line: 6, type: !542, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1562 = !DILocalVariable(name: "args", arg: 4, scope: !1561, file: !9, line: 6, type: !149)
!1563 = !DILocation(line: 6, scope: !1561)
!1564 = !DILocalVariable(name: "methodID", arg: 3, scope: !1561, file: !9, line: 6, type: !67)
!1565 = !DILocalVariable(name: "clazz", arg: 2, scope: !1561, file: !9, line: 6, type: !47)
!1566 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1561, file: !9, line: 6, type: !25)
!1567 = !DILocalVariable(name: "sig", scope: !1561, file: !9, line: 6, type: !1106)
!1568 = !DILocalVariable(name: "argc", scope: !1561, file: !9, line: 6, type: !13)
!1569 = !DILocalVariable(name: "argv", scope: !1561, file: !9, line: 6, type: !1111)
!1570 = !DILocalVariable(name: "i", scope: !1571, file: !9, line: 6, type: !13)
!1571 = distinct !DILexicalBlock(scope: !1561, file: !9, line: 6)
!1572 = !DILocation(line: 6, scope: !1571)
!1573 = !DILocation(line: 6, scope: !1574)
!1574 = distinct !DILexicalBlock(scope: !1575, file: !9, line: 6)
!1575 = distinct !DILexicalBlock(scope: !1571, file: !9, line: 6)
!1576 = !DILocation(line: 6, scope: !1577)
!1577 = distinct !DILexicalBlock(scope: !1574, file: !9, line: 6)
!1578 = !DILocation(line: 6, scope: !1575)
!1579 = distinct !{!1579, !1572, !1572, !1122}
!1580 = distinct !DISubprogram(name: "JNI_CallShortMethod", scope: !9, file: !9, line: 7, type: !242, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1581 = !DILocalVariable(name: "methodID", arg: 3, scope: !1580, file: !9, line: 7, type: !67)
!1582 = !DILocation(line: 7, scope: !1580)
!1583 = !DILocalVariable(name: "obj", arg: 2, scope: !1580, file: !9, line: 7, type: !48)
!1584 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1580, file: !9, line: 7, type: !25)
!1585 = !DILocalVariable(name: "args", scope: !1580, file: !9, line: 7, type: !149)
!1586 = !DILocalVariable(name: "sig", scope: !1580, file: !9, line: 7, type: !1106)
!1587 = !DILocalVariable(name: "argc", scope: !1580, file: !9, line: 7, type: !13)
!1588 = !DILocalVariable(name: "argv", scope: !1580, file: !9, line: 7, type: !1111)
!1589 = !DILocalVariable(name: "i", scope: !1590, file: !9, line: 7, type: !13)
!1590 = distinct !DILexicalBlock(scope: !1580, file: !9, line: 7)
!1591 = !DILocation(line: 7, scope: !1590)
!1592 = !DILocation(line: 7, scope: !1593)
!1593 = distinct !DILexicalBlock(scope: !1594, file: !9, line: 7)
!1594 = distinct !DILexicalBlock(scope: !1590, file: !9, line: 7)
!1595 = !DILocation(line: 7, scope: !1596)
!1596 = distinct !DILexicalBlock(scope: !1593, file: !9, line: 7)
!1597 = !DILocation(line: 7, scope: !1594)
!1598 = distinct !{!1598, !1591, !1591, !1122}
!1599 = !DILocalVariable(name: "ret", scope: !1580, file: !9, line: 7, type: !167)
!1600 = distinct !DISubprogram(name: "JNI_CallShortMethodV", scope: !9, file: !9, line: 7, type: !246, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1601 = !DILocalVariable(name: "args", arg: 4, scope: !1600, file: !9, line: 7, type: !149)
!1602 = !DILocation(line: 7, scope: !1600)
!1603 = !DILocalVariable(name: "methodID", arg: 3, scope: !1600, file: !9, line: 7, type: !67)
!1604 = !DILocalVariable(name: "obj", arg: 2, scope: !1600, file: !9, line: 7, type: !48)
!1605 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1600, file: !9, line: 7, type: !25)
!1606 = !DILocalVariable(name: "sig", scope: !1600, file: !9, line: 7, type: !1106)
!1607 = !DILocalVariable(name: "argc", scope: !1600, file: !9, line: 7, type: !13)
!1608 = !DILocalVariable(name: "argv", scope: !1600, file: !9, line: 7, type: !1111)
!1609 = !DILocalVariable(name: "i", scope: !1610, file: !9, line: 7, type: !13)
!1610 = distinct !DILexicalBlock(scope: !1600, file: !9, line: 7)
!1611 = !DILocation(line: 7, scope: !1610)
!1612 = !DILocation(line: 7, scope: !1613)
!1613 = distinct !DILexicalBlock(scope: !1614, file: !9, line: 7)
!1614 = distinct !DILexicalBlock(scope: !1610, file: !9, line: 7)
!1615 = !DILocation(line: 7, scope: !1616)
!1616 = distinct !DILexicalBlock(scope: !1613, file: !9, line: 7)
!1617 = !DILocation(line: 7, scope: !1614)
!1618 = distinct !{!1618, !1611, !1611, !1122}
!1619 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethod", scope: !9, file: !9, line: 7, type: !362, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1620 = !DILocalVariable(name: "methodID", arg: 4, scope: !1619, file: !9, line: 7, type: !67)
!1621 = !DILocation(line: 7, scope: !1619)
!1622 = !DILocalVariable(name: "clazz", arg: 3, scope: !1619, file: !9, line: 7, type: !47)
!1623 = !DILocalVariable(name: "obj", arg: 2, scope: !1619, file: !9, line: 7, type: !48)
!1624 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1619, file: !9, line: 7, type: !25)
!1625 = !DILocalVariable(name: "args", scope: !1619, file: !9, line: 7, type: !149)
!1626 = !DILocalVariable(name: "sig", scope: !1619, file: !9, line: 7, type: !1106)
!1627 = !DILocalVariable(name: "argc", scope: !1619, file: !9, line: 7, type: !13)
!1628 = !DILocalVariable(name: "argv", scope: !1619, file: !9, line: 7, type: !1111)
!1629 = !DILocalVariable(name: "i", scope: !1630, file: !9, line: 7, type: !13)
!1630 = distinct !DILexicalBlock(scope: !1619, file: !9, line: 7)
!1631 = !DILocation(line: 7, scope: !1630)
!1632 = !DILocation(line: 7, scope: !1633)
!1633 = distinct !DILexicalBlock(scope: !1634, file: !9, line: 7)
!1634 = distinct !DILexicalBlock(scope: !1630, file: !9, line: 7)
!1635 = !DILocation(line: 7, scope: !1636)
!1636 = distinct !DILexicalBlock(scope: !1633, file: !9, line: 7)
!1637 = !DILocation(line: 7, scope: !1634)
!1638 = distinct !{!1638, !1631, !1631, !1122}
!1639 = !DILocalVariable(name: "ret", scope: !1619, file: !9, line: 7, type: !167)
!1640 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethodV", scope: !9, file: !9, line: 7, type: !366, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1641 = !DILocalVariable(name: "args", arg: 5, scope: !1640, file: !9, line: 7, type: !149)
!1642 = !DILocation(line: 7, scope: !1640)
!1643 = !DILocalVariable(name: "methodID", arg: 4, scope: !1640, file: !9, line: 7, type: !67)
!1644 = !DILocalVariable(name: "clazz", arg: 3, scope: !1640, file: !9, line: 7, type: !47)
!1645 = !DILocalVariable(name: "obj", arg: 2, scope: !1640, file: !9, line: 7, type: !48)
!1646 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1640, file: !9, line: 7, type: !25)
!1647 = !DILocalVariable(name: "sig", scope: !1640, file: !9, line: 7, type: !1106)
!1648 = !DILocalVariable(name: "argc", scope: !1640, file: !9, line: 7, type: !13)
!1649 = !DILocalVariable(name: "argv", scope: !1640, file: !9, line: 7, type: !1111)
!1650 = !DILocalVariable(name: "i", scope: !1651, file: !9, line: 7, type: !13)
!1651 = distinct !DILexicalBlock(scope: !1640, file: !9, line: 7)
!1652 = !DILocation(line: 7, scope: !1651)
!1653 = !DILocation(line: 7, scope: !1654)
!1654 = distinct !DILexicalBlock(scope: !1655, file: !9, line: 7)
!1655 = distinct !DILexicalBlock(scope: !1651, file: !9, line: 7)
!1656 = !DILocation(line: 7, scope: !1657)
!1657 = distinct !DILexicalBlock(scope: !1654, file: !9, line: 7)
!1658 = !DILocation(line: 7, scope: !1655)
!1659 = distinct !{!1659, !1652, !1652, !1122}
!1660 = distinct !DISubprogram(name: "JNI_CallStaticShortMethod", scope: !9, file: !9, line: 7, type: !550, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1661 = !DILocalVariable(name: "methodID", arg: 3, scope: !1660, file: !9, line: 7, type: !67)
!1662 = !DILocation(line: 7, scope: !1660)
!1663 = !DILocalVariable(name: "clazz", arg: 2, scope: !1660, file: !9, line: 7, type: !47)
!1664 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1660, file: !9, line: 7, type: !25)
!1665 = !DILocalVariable(name: "args", scope: !1660, file: !9, line: 7, type: !149)
!1666 = !DILocalVariable(name: "sig", scope: !1660, file: !9, line: 7, type: !1106)
!1667 = !DILocalVariable(name: "argc", scope: !1660, file: !9, line: 7, type: !13)
!1668 = !DILocalVariable(name: "argv", scope: !1660, file: !9, line: 7, type: !1111)
!1669 = !DILocalVariable(name: "i", scope: !1670, file: !9, line: 7, type: !13)
!1670 = distinct !DILexicalBlock(scope: !1660, file: !9, line: 7)
!1671 = !DILocation(line: 7, scope: !1670)
!1672 = !DILocation(line: 7, scope: !1673)
!1673 = distinct !DILexicalBlock(scope: !1674, file: !9, line: 7)
!1674 = distinct !DILexicalBlock(scope: !1670, file: !9, line: 7)
!1675 = !DILocation(line: 7, scope: !1676)
!1676 = distinct !DILexicalBlock(scope: !1673, file: !9, line: 7)
!1677 = !DILocation(line: 7, scope: !1674)
!1678 = distinct !{!1678, !1671, !1671, !1122}
!1679 = !DILocalVariable(name: "ret", scope: !1660, file: !9, line: 7, type: !167)
!1680 = distinct !DISubprogram(name: "JNI_CallStaticShortMethodV", scope: !9, file: !9, line: 7, type: !554, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1681 = !DILocalVariable(name: "args", arg: 4, scope: !1680, file: !9, line: 7, type: !149)
!1682 = !DILocation(line: 7, scope: !1680)
!1683 = !DILocalVariable(name: "methodID", arg: 3, scope: !1680, file: !9, line: 7, type: !67)
!1684 = !DILocalVariable(name: "clazz", arg: 2, scope: !1680, file: !9, line: 7, type: !47)
!1685 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1680, file: !9, line: 7, type: !25)
!1686 = !DILocalVariable(name: "sig", scope: !1680, file: !9, line: 7, type: !1106)
!1687 = !DILocalVariable(name: "argc", scope: !1680, file: !9, line: 7, type: !13)
!1688 = !DILocalVariable(name: "argv", scope: !1680, file: !9, line: 7, type: !1111)
!1689 = !DILocalVariable(name: "i", scope: !1690, file: !9, line: 7, type: !13)
!1690 = distinct !DILexicalBlock(scope: !1680, file: !9, line: 7)
!1691 = !DILocation(line: 7, scope: !1690)
!1692 = !DILocation(line: 7, scope: !1693)
!1693 = distinct !DILexicalBlock(scope: !1694, file: !9, line: 7)
!1694 = distinct !DILexicalBlock(scope: !1690, file: !9, line: 7)
!1695 = !DILocation(line: 7, scope: !1696)
!1696 = distinct !DILexicalBlock(scope: !1693, file: !9, line: 7)
!1697 = !DILocation(line: 7, scope: !1694)
!1698 = distinct !{!1698, !1691, !1691, !1122}
!1699 = distinct !DISubprogram(name: "JNI_CallIntMethod", scope: !9, file: !9, line: 8, type: !254, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1700 = !DILocalVariable(name: "methodID", arg: 3, scope: !1699, file: !9, line: 8, type: !67)
!1701 = !DILocation(line: 8, scope: !1699)
!1702 = !DILocalVariable(name: "obj", arg: 2, scope: !1699, file: !9, line: 8, type: !48)
!1703 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1699, file: !9, line: 8, type: !25)
!1704 = !DILocalVariable(name: "args", scope: !1699, file: !9, line: 8, type: !149)
!1705 = !DILocalVariable(name: "sig", scope: !1699, file: !9, line: 8, type: !1106)
!1706 = !DILocalVariable(name: "argc", scope: !1699, file: !9, line: 8, type: !13)
!1707 = !DILocalVariable(name: "argv", scope: !1699, file: !9, line: 8, type: !1111)
!1708 = !DILocalVariable(name: "i", scope: !1709, file: !9, line: 8, type: !13)
!1709 = distinct !DILexicalBlock(scope: !1699, file: !9, line: 8)
!1710 = !DILocation(line: 8, scope: !1709)
!1711 = !DILocation(line: 8, scope: !1712)
!1712 = distinct !DILexicalBlock(scope: !1713, file: !9, line: 8)
!1713 = distinct !DILexicalBlock(scope: !1709, file: !9, line: 8)
!1714 = !DILocation(line: 8, scope: !1715)
!1715 = distinct !DILexicalBlock(scope: !1712, file: !9, line: 8)
!1716 = !DILocation(line: 8, scope: !1713)
!1717 = distinct !{!1717, !1710, !1710, !1122}
!1718 = !DILocalVariable(name: "ret", scope: !1699, file: !9, line: 8, type: !40)
!1719 = distinct !DISubprogram(name: "JNI_CallIntMethodV", scope: !9, file: !9, line: 8, type: !258, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1720 = !DILocalVariable(name: "args", arg: 4, scope: !1719, file: !9, line: 8, type: !149)
!1721 = !DILocation(line: 8, scope: !1719)
!1722 = !DILocalVariable(name: "methodID", arg: 3, scope: !1719, file: !9, line: 8, type: !67)
!1723 = !DILocalVariable(name: "obj", arg: 2, scope: !1719, file: !9, line: 8, type: !48)
!1724 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1719, file: !9, line: 8, type: !25)
!1725 = !DILocalVariable(name: "sig", scope: !1719, file: !9, line: 8, type: !1106)
!1726 = !DILocalVariable(name: "argc", scope: !1719, file: !9, line: 8, type: !13)
!1727 = !DILocalVariable(name: "argv", scope: !1719, file: !9, line: 8, type: !1111)
!1728 = !DILocalVariable(name: "i", scope: !1729, file: !9, line: 8, type: !13)
!1729 = distinct !DILexicalBlock(scope: !1719, file: !9, line: 8)
!1730 = !DILocation(line: 8, scope: !1729)
!1731 = !DILocation(line: 8, scope: !1732)
!1732 = distinct !DILexicalBlock(scope: !1733, file: !9, line: 8)
!1733 = distinct !DILexicalBlock(scope: !1729, file: !9, line: 8)
!1734 = !DILocation(line: 8, scope: !1735)
!1735 = distinct !DILexicalBlock(scope: !1732, file: !9, line: 8)
!1736 = !DILocation(line: 8, scope: !1733)
!1737 = distinct !{!1737, !1730, !1730, !1122}
!1738 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethod", scope: !9, file: !9, line: 8, type: !374, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1739 = !DILocalVariable(name: "methodID", arg: 4, scope: !1738, file: !9, line: 8, type: !67)
!1740 = !DILocation(line: 8, scope: !1738)
!1741 = !DILocalVariable(name: "clazz", arg: 3, scope: !1738, file: !9, line: 8, type: !47)
!1742 = !DILocalVariable(name: "obj", arg: 2, scope: !1738, file: !9, line: 8, type: !48)
!1743 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1738, file: !9, line: 8, type: !25)
!1744 = !DILocalVariable(name: "args", scope: !1738, file: !9, line: 8, type: !149)
!1745 = !DILocalVariable(name: "sig", scope: !1738, file: !9, line: 8, type: !1106)
!1746 = !DILocalVariable(name: "argc", scope: !1738, file: !9, line: 8, type: !13)
!1747 = !DILocalVariable(name: "argv", scope: !1738, file: !9, line: 8, type: !1111)
!1748 = !DILocalVariable(name: "i", scope: !1749, file: !9, line: 8, type: !13)
!1749 = distinct !DILexicalBlock(scope: !1738, file: !9, line: 8)
!1750 = !DILocation(line: 8, scope: !1749)
!1751 = !DILocation(line: 8, scope: !1752)
!1752 = distinct !DILexicalBlock(scope: !1753, file: !9, line: 8)
!1753 = distinct !DILexicalBlock(scope: !1749, file: !9, line: 8)
!1754 = !DILocation(line: 8, scope: !1755)
!1755 = distinct !DILexicalBlock(scope: !1752, file: !9, line: 8)
!1756 = !DILocation(line: 8, scope: !1753)
!1757 = distinct !{!1757, !1750, !1750, !1122}
!1758 = !DILocalVariable(name: "ret", scope: !1738, file: !9, line: 8, type: !40)
!1759 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethodV", scope: !9, file: !9, line: 8, type: !378, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1760 = !DILocalVariable(name: "args", arg: 5, scope: !1759, file: !9, line: 8, type: !149)
!1761 = !DILocation(line: 8, scope: !1759)
!1762 = !DILocalVariable(name: "methodID", arg: 4, scope: !1759, file: !9, line: 8, type: !67)
!1763 = !DILocalVariable(name: "clazz", arg: 3, scope: !1759, file: !9, line: 8, type: !47)
!1764 = !DILocalVariable(name: "obj", arg: 2, scope: !1759, file: !9, line: 8, type: !48)
!1765 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1759, file: !9, line: 8, type: !25)
!1766 = !DILocalVariable(name: "sig", scope: !1759, file: !9, line: 8, type: !1106)
!1767 = !DILocalVariable(name: "argc", scope: !1759, file: !9, line: 8, type: !13)
!1768 = !DILocalVariable(name: "argv", scope: !1759, file: !9, line: 8, type: !1111)
!1769 = !DILocalVariable(name: "i", scope: !1770, file: !9, line: 8, type: !13)
!1770 = distinct !DILexicalBlock(scope: !1759, file: !9, line: 8)
!1771 = !DILocation(line: 8, scope: !1770)
!1772 = !DILocation(line: 8, scope: !1773)
!1773 = distinct !DILexicalBlock(scope: !1774, file: !9, line: 8)
!1774 = distinct !DILexicalBlock(scope: !1770, file: !9, line: 8)
!1775 = !DILocation(line: 8, scope: !1776)
!1776 = distinct !DILexicalBlock(scope: !1773, file: !9, line: 8)
!1777 = !DILocation(line: 8, scope: !1774)
!1778 = distinct !{!1778, !1771, !1771, !1122}
!1779 = distinct !DISubprogram(name: "JNI_CallStaticIntMethod", scope: !9, file: !9, line: 8, type: !562, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1780 = !DILocalVariable(name: "methodID", arg: 3, scope: !1779, file: !9, line: 8, type: !67)
!1781 = !DILocation(line: 8, scope: !1779)
!1782 = !DILocalVariable(name: "clazz", arg: 2, scope: !1779, file: !9, line: 8, type: !47)
!1783 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1779, file: !9, line: 8, type: !25)
!1784 = !DILocalVariable(name: "args", scope: !1779, file: !9, line: 8, type: !149)
!1785 = !DILocalVariable(name: "sig", scope: !1779, file: !9, line: 8, type: !1106)
!1786 = !DILocalVariable(name: "argc", scope: !1779, file: !9, line: 8, type: !13)
!1787 = !DILocalVariable(name: "argv", scope: !1779, file: !9, line: 8, type: !1111)
!1788 = !DILocalVariable(name: "i", scope: !1789, file: !9, line: 8, type: !13)
!1789 = distinct !DILexicalBlock(scope: !1779, file: !9, line: 8)
!1790 = !DILocation(line: 8, scope: !1789)
!1791 = !DILocation(line: 8, scope: !1792)
!1792 = distinct !DILexicalBlock(scope: !1793, file: !9, line: 8)
!1793 = distinct !DILexicalBlock(scope: !1789, file: !9, line: 8)
!1794 = !DILocation(line: 8, scope: !1795)
!1795 = distinct !DILexicalBlock(scope: !1792, file: !9, line: 8)
!1796 = !DILocation(line: 8, scope: !1793)
!1797 = distinct !{!1797, !1790, !1790, !1122}
!1798 = !DILocalVariable(name: "ret", scope: !1779, file: !9, line: 8, type: !40)
!1799 = distinct !DISubprogram(name: "JNI_CallStaticIntMethodV", scope: !9, file: !9, line: 8, type: !566, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1800 = !DILocalVariable(name: "args", arg: 4, scope: !1799, file: !9, line: 8, type: !149)
!1801 = !DILocation(line: 8, scope: !1799)
!1802 = !DILocalVariable(name: "methodID", arg: 3, scope: !1799, file: !9, line: 8, type: !67)
!1803 = !DILocalVariable(name: "clazz", arg: 2, scope: !1799, file: !9, line: 8, type: !47)
!1804 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1799, file: !9, line: 8, type: !25)
!1805 = !DILocalVariable(name: "sig", scope: !1799, file: !9, line: 8, type: !1106)
!1806 = !DILocalVariable(name: "argc", scope: !1799, file: !9, line: 8, type: !13)
!1807 = !DILocalVariable(name: "argv", scope: !1799, file: !9, line: 8, type: !1111)
!1808 = !DILocalVariable(name: "i", scope: !1809, file: !9, line: 8, type: !13)
!1809 = distinct !DILexicalBlock(scope: !1799, file: !9, line: 8)
!1810 = !DILocation(line: 8, scope: !1809)
!1811 = !DILocation(line: 8, scope: !1812)
!1812 = distinct !DILexicalBlock(scope: !1813, file: !9, line: 8)
!1813 = distinct !DILexicalBlock(scope: !1809, file: !9, line: 8)
!1814 = !DILocation(line: 8, scope: !1815)
!1815 = distinct !DILexicalBlock(scope: !1812, file: !9, line: 8)
!1816 = !DILocation(line: 8, scope: !1813)
!1817 = distinct !{!1817, !1810, !1810, !1122}
!1818 = distinct !DISubprogram(name: "JNI_CallLongMethod", scope: !9, file: !9, line: 9, type: !266, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1819 = !DILocalVariable(name: "methodID", arg: 3, scope: !1818, file: !9, line: 9, type: !67)
!1820 = !DILocation(line: 9, scope: !1818)
!1821 = !DILocalVariable(name: "obj", arg: 2, scope: !1818, file: !9, line: 9, type: !48)
!1822 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1818, file: !9, line: 9, type: !25)
!1823 = !DILocalVariable(name: "args", scope: !1818, file: !9, line: 9, type: !149)
!1824 = !DILocalVariable(name: "sig", scope: !1818, file: !9, line: 9, type: !1106)
!1825 = !DILocalVariable(name: "argc", scope: !1818, file: !9, line: 9, type: !13)
!1826 = !DILocalVariable(name: "argv", scope: !1818, file: !9, line: 9, type: !1111)
!1827 = !DILocalVariable(name: "i", scope: !1828, file: !9, line: 9, type: !13)
!1828 = distinct !DILexicalBlock(scope: !1818, file: !9, line: 9)
!1829 = !DILocation(line: 9, scope: !1828)
!1830 = !DILocation(line: 9, scope: !1831)
!1831 = distinct !DILexicalBlock(scope: !1832, file: !9, line: 9)
!1832 = distinct !DILexicalBlock(scope: !1828, file: !9, line: 9)
!1833 = !DILocation(line: 9, scope: !1834)
!1834 = distinct !DILexicalBlock(scope: !1831, file: !9, line: 9)
!1835 = !DILocation(line: 9, scope: !1832)
!1836 = distinct !{!1836, !1829, !1829, !1122}
!1837 = !DILocalVariable(name: "ret", scope: !1818, file: !9, line: 9, type: !171)
!1838 = distinct !DISubprogram(name: "JNI_CallLongMethodV", scope: !9, file: !9, line: 9, type: !270, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1839 = !DILocalVariable(name: "args", arg: 4, scope: !1838, file: !9, line: 9, type: !149)
!1840 = !DILocation(line: 9, scope: !1838)
!1841 = !DILocalVariable(name: "methodID", arg: 3, scope: !1838, file: !9, line: 9, type: !67)
!1842 = !DILocalVariable(name: "obj", arg: 2, scope: !1838, file: !9, line: 9, type: !48)
!1843 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1838, file: !9, line: 9, type: !25)
!1844 = !DILocalVariable(name: "sig", scope: !1838, file: !9, line: 9, type: !1106)
!1845 = !DILocalVariable(name: "argc", scope: !1838, file: !9, line: 9, type: !13)
!1846 = !DILocalVariable(name: "argv", scope: !1838, file: !9, line: 9, type: !1111)
!1847 = !DILocalVariable(name: "i", scope: !1848, file: !9, line: 9, type: !13)
!1848 = distinct !DILexicalBlock(scope: !1838, file: !9, line: 9)
!1849 = !DILocation(line: 9, scope: !1848)
!1850 = !DILocation(line: 9, scope: !1851)
!1851 = distinct !DILexicalBlock(scope: !1852, file: !9, line: 9)
!1852 = distinct !DILexicalBlock(scope: !1848, file: !9, line: 9)
!1853 = !DILocation(line: 9, scope: !1854)
!1854 = distinct !DILexicalBlock(scope: !1851, file: !9, line: 9)
!1855 = !DILocation(line: 9, scope: !1852)
!1856 = distinct !{!1856, !1849, !1849, !1122}
!1857 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethod", scope: !9, file: !9, line: 9, type: !386, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1858 = !DILocalVariable(name: "methodID", arg: 4, scope: !1857, file: !9, line: 9, type: !67)
!1859 = !DILocation(line: 9, scope: !1857)
!1860 = !DILocalVariable(name: "clazz", arg: 3, scope: !1857, file: !9, line: 9, type: !47)
!1861 = !DILocalVariable(name: "obj", arg: 2, scope: !1857, file: !9, line: 9, type: !48)
!1862 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1857, file: !9, line: 9, type: !25)
!1863 = !DILocalVariable(name: "args", scope: !1857, file: !9, line: 9, type: !149)
!1864 = !DILocalVariable(name: "sig", scope: !1857, file: !9, line: 9, type: !1106)
!1865 = !DILocalVariable(name: "argc", scope: !1857, file: !9, line: 9, type: !13)
!1866 = !DILocalVariable(name: "argv", scope: !1857, file: !9, line: 9, type: !1111)
!1867 = !DILocalVariable(name: "i", scope: !1868, file: !9, line: 9, type: !13)
!1868 = distinct !DILexicalBlock(scope: !1857, file: !9, line: 9)
!1869 = !DILocation(line: 9, scope: !1868)
!1870 = !DILocation(line: 9, scope: !1871)
!1871 = distinct !DILexicalBlock(scope: !1872, file: !9, line: 9)
!1872 = distinct !DILexicalBlock(scope: !1868, file: !9, line: 9)
!1873 = !DILocation(line: 9, scope: !1874)
!1874 = distinct !DILexicalBlock(scope: !1871, file: !9, line: 9)
!1875 = !DILocation(line: 9, scope: !1872)
!1876 = distinct !{!1876, !1869, !1869, !1122}
!1877 = !DILocalVariable(name: "ret", scope: !1857, file: !9, line: 9, type: !171)
!1878 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethodV", scope: !9, file: !9, line: 9, type: !390, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1879 = !DILocalVariable(name: "args", arg: 5, scope: !1878, file: !9, line: 9, type: !149)
!1880 = !DILocation(line: 9, scope: !1878)
!1881 = !DILocalVariable(name: "methodID", arg: 4, scope: !1878, file: !9, line: 9, type: !67)
!1882 = !DILocalVariable(name: "clazz", arg: 3, scope: !1878, file: !9, line: 9, type: !47)
!1883 = !DILocalVariable(name: "obj", arg: 2, scope: !1878, file: !9, line: 9, type: !48)
!1884 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1878, file: !9, line: 9, type: !25)
!1885 = !DILocalVariable(name: "sig", scope: !1878, file: !9, line: 9, type: !1106)
!1886 = !DILocalVariable(name: "argc", scope: !1878, file: !9, line: 9, type: !13)
!1887 = !DILocalVariable(name: "argv", scope: !1878, file: !9, line: 9, type: !1111)
!1888 = !DILocalVariable(name: "i", scope: !1889, file: !9, line: 9, type: !13)
!1889 = distinct !DILexicalBlock(scope: !1878, file: !9, line: 9)
!1890 = !DILocation(line: 9, scope: !1889)
!1891 = !DILocation(line: 9, scope: !1892)
!1892 = distinct !DILexicalBlock(scope: !1893, file: !9, line: 9)
!1893 = distinct !DILexicalBlock(scope: !1889, file: !9, line: 9)
!1894 = !DILocation(line: 9, scope: !1895)
!1895 = distinct !DILexicalBlock(scope: !1892, file: !9, line: 9)
!1896 = !DILocation(line: 9, scope: !1893)
!1897 = distinct !{!1897, !1890, !1890, !1122}
!1898 = distinct !DISubprogram(name: "JNI_CallStaticLongMethod", scope: !9, file: !9, line: 9, type: !574, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1899 = !DILocalVariable(name: "methodID", arg: 3, scope: !1898, file: !9, line: 9, type: !67)
!1900 = !DILocation(line: 9, scope: !1898)
!1901 = !DILocalVariable(name: "clazz", arg: 2, scope: !1898, file: !9, line: 9, type: !47)
!1902 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1898, file: !9, line: 9, type: !25)
!1903 = !DILocalVariable(name: "args", scope: !1898, file: !9, line: 9, type: !149)
!1904 = !DILocalVariable(name: "sig", scope: !1898, file: !9, line: 9, type: !1106)
!1905 = !DILocalVariable(name: "argc", scope: !1898, file: !9, line: 9, type: !13)
!1906 = !DILocalVariable(name: "argv", scope: !1898, file: !9, line: 9, type: !1111)
!1907 = !DILocalVariable(name: "i", scope: !1908, file: !9, line: 9, type: !13)
!1908 = distinct !DILexicalBlock(scope: !1898, file: !9, line: 9)
!1909 = !DILocation(line: 9, scope: !1908)
!1910 = !DILocation(line: 9, scope: !1911)
!1911 = distinct !DILexicalBlock(scope: !1912, file: !9, line: 9)
!1912 = distinct !DILexicalBlock(scope: !1908, file: !9, line: 9)
!1913 = !DILocation(line: 9, scope: !1914)
!1914 = distinct !DILexicalBlock(scope: !1911, file: !9, line: 9)
!1915 = !DILocation(line: 9, scope: !1912)
!1916 = distinct !{!1916, !1909, !1909, !1122}
!1917 = !DILocalVariable(name: "ret", scope: !1898, file: !9, line: 9, type: !171)
!1918 = distinct !DISubprogram(name: "JNI_CallStaticLongMethodV", scope: !9, file: !9, line: 9, type: !578, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1919 = !DILocalVariable(name: "args", arg: 4, scope: !1918, file: !9, line: 9, type: !149)
!1920 = !DILocation(line: 9, scope: !1918)
!1921 = !DILocalVariable(name: "methodID", arg: 3, scope: !1918, file: !9, line: 9, type: !67)
!1922 = !DILocalVariable(name: "clazz", arg: 2, scope: !1918, file: !9, line: 9, type: !47)
!1923 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1918, file: !9, line: 9, type: !25)
!1924 = !DILocalVariable(name: "sig", scope: !1918, file: !9, line: 9, type: !1106)
!1925 = !DILocalVariable(name: "argc", scope: !1918, file: !9, line: 9, type: !13)
!1926 = !DILocalVariable(name: "argv", scope: !1918, file: !9, line: 9, type: !1111)
!1927 = !DILocalVariable(name: "i", scope: !1928, file: !9, line: 9, type: !13)
!1928 = distinct !DILexicalBlock(scope: !1918, file: !9, line: 9)
!1929 = !DILocation(line: 9, scope: !1928)
!1930 = !DILocation(line: 9, scope: !1931)
!1931 = distinct !DILexicalBlock(scope: !1932, file: !9, line: 9)
!1932 = distinct !DILexicalBlock(scope: !1928, file: !9, line: 9)
!1933 = !DILocation(line: 9, scope: !1934)
!1934 = distinct !DILexicalBlock(scope: !1931, file: !9, line: 9)
!1935 = !DILocation(line: 9, scope: !1932)
!1936 = distinct !{!1936, !1929, !1929, !1122}
!1937 = distinct !DISubprogram(name: "JNI_CallFloatMethod", scope: !9, file: !9, line: 10, type: !278, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1938 = !DILocalVariable(name: "methodID", arg: 3, scope: !1937, file: !9, line: 10, type: !67)
!1939 = !DILocation(line: 10, scope: !1937)
!1940 = !DILocalVariable(name: "obj", arg: 2, scope: !1937, file: !9, line: 10, type: !48)
!1941 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1937, file: !9, line: 10, type: !25)
!1942 = !DILocalVariable(name: "args", scope: !1937, file: !9, line: 10, type: !149)
!1943 = !DILocalVariable(name: "sig", scope: !1937, file: !9, line: 10, type: !1106)
!1944 = !DILocalVariable(name: "argc", scope: !1937, file: !9, line: 10, type: !13)
!1945 = !DILocalVariable(name: "argv", scope: !1937, file: !9, line: 10, type: !1111)
!1946 = !DILocalVariable(name: "i", scope: !1947, file: !9, line: 10, type: !13)
!1947 = distinct !DILexicalBlock(scope: !1937, file: !9, line: 10)
!1948 = !DILocation(line: 10, scope: !1947)
!1949 = !DILocation(line: 10, scope: !1950)
!1950 = distinct !DILexicalBlock(scope: !1951, file: !9, line: 10)
!1951 = distinct !DILexicalBlock(scope: !1947, file: !9, line: 10)
!1952 = !DILocation(line: 10, scope: !1953)
!1953 = distinct !DILexicalBlock(scope: !1950, file: !9, line: 10)
!1954 = !DILocation(line: 10, scope: !1951)
!1955 = distinct !{!1955, !1948, !1948, !1122}
!1956 = !DILocalVariable(name: "ret", scope: !1937, file: !9, line: 10, type: !174)
!1957 = distinct !DISubprogram(name: "JNI_CallFloatMethodV", scope: !9, file: !9, line: 10, type: !282, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1958 = !DILocalVariable(name: "args", arg: 4, scope: !1957, file: !9, line: 10, type: !149)
!1959 = !DILocation(line: 10, scope: !1957)
!1960 = !DILocalVariable(name: "methodID", arg: 3, scope: !1957, file: !9, line: 10, type: !67)
!1961 = !DILocalVariable(name: "obj", arg: 2, scope: !1957, file: !9, line: 10, type: !48)
!1962 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1957, file: !9, line: 10, type: !25)
!1963 = !DILocalVariable(name: "sig", scope: !1957, file: !9, line: 10, type: !1106)
!1964 = !DILocalVariable(name: "argc", scope: !1957, file: !9, line: 10, type: !13)
!1965 = !DILocalVariable(name: "argv", scope: !1957, file: !9, line: 10, type: !1111)
!1966 = !DILocalVariable(name: "i", scope: !1967, file: !9, line: 10, type: !13)
!1967 = distinct !DILexicalBlock(scope: !1957, file: !9, line: 10)
!1968 = !DILocation(line: 10, scope: !1967)
!1969 = !DILocation(line: 10, scope: !1970)
!1970 = distinct !DILexicalBlock(scope: !1971, file: !9, line: 10)
!1971 = distinct !DILexicalBlock(scope: !1967, file: !9, line: 10)
!1972 = !DILocation(line: 10, scope: !1973)
!1973 = distinct !DILexicalBlock(scope: !1970, file: !9, line: 10)
!1974 = !DILocation(line: 10, scope: !1971)
!1975 = distinct !{!1975, !1968, !1968, !1122}
!1976 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethod", scope: !9, file: !9, line: 10, type: !398, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1977 = !DILocalVariable(name: "methodID", arg: 4, scope: !1976, file: !9, line: 10, type: !67)
!1978 = !DILocation(line: 10, scope: !1976)
!1979 = !DILocalVariable(name: "clazz", arg: 3, scope: !1976, file: !9, line: 10, type: !47)
!1980 = !DILocalVariable(name: "obj", arg: 2, scope: !1976, file: !9, line: 10, type: !48)
!1981 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1976, file: !9, line: 10, type: !25)
!1982 = !DILocalVariable(name: "args", scope: !1976, file: !9, line: 10, type: !149)
!1983 = !DILocalVariable(name: "sig", scope: !1976, file: !9, line: 10, type: !1106)
!1984 = !DILocalVariable(name: "argc", scope: !1976, file: !9, line: 10, type: !13)
!1985 = !DILocalVariable(name: "argv", scope: !1976, file: !9, line: 10, type: !1111)
!1986 = !DILocalVariable(name: "i", scope: !1987, file: !9, line: 10, type: !13)
!1987 = distinct !DILexicalBlock(scope: !1976, file: !9, line: 10)
!1988 = !DILocation(line: 10, scope: !1987)
!1989 = !DILocation(line: 10, scope: !1990)
!1990 = distinct !DILexicalBlock(scope: !1991, file: !9, line: 10)
!1991 = distinct !DILexicalBlock(scope: !1987, file: !9, line: 10)
!1992 = !DILocation(line: 10, scope: !1993)
!1993 = distinct !DILexicalBlock(scope: !1990, file: !9, line: 10)
!1994 = !DILocation(line: 10, scope: !1991)
!1995 = distinct !{!1995, !1988, !1988, !1122}
!1996 = !DILocalVariable(name: "ret", scope: !1976, file: !9, line: 10, type: !174)
!1997 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethodV", scope: !9, file: !9, line: 10, type: !402, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1998 = !DILocalVariable(name: "args", arg: 5, scope: !1997, file: !9, line: 10, type: !149)
!1999 = !DILocation(line: 10, scope: !1997)
!2000 = !DILocalVariable(name: "methodID", arg: 4, scope: !1997, file: !9, line: 10, type: !67)
!2001 = !DILocalVariable(name: "clazz", arg: 3, scope: !1997, file: !9, line: 10, type: !47)
!2002 = !DILocalVariable(name: "obj", arg: 2, scope: !1997, file: !9, line: 10, type: !48)
!2003 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1997, file: !9, line: 10, type: !25)
!2004 = !DILocalVariable(name: "sig", scope: !1997, file: !9, line: 10, type: !1106)
!2005 = !DILocalVariable(name: "argc", scope: !1997, file: !9, line: 10, type: !13)
!2006 = !DILocalVariable(name: "argv", scope: !1997, file: !9, line: 10, type: !1111)
!2007 = !DILocalVariable(name: "i", scope: !2008, file: !9, line: 10, type: !13)
!2008 = distinct !DILexicalBlock(scope: !1997, file: !9, line: 10)
!2009 = !DILocation(line: 10, scope: !2008)
!2010 = !DILocation(line: 10, scope: !2011)
!2011 = distinct !DILexicalBlock(scope: !2012, file: !9, line: 10)
!2012 = distinct !DILexicalBlock(scope: !2008, file: !9, line: 10)
!2013 = !DILocation(line: 10, scope: !2014)
!2014 = distinct !DILexicalBlock(scope: !2011, file: !9, line: 10)
!2015 = !DILocation(line: 10, scope: !2012)
!2016 = distinct !{!2016, !2009, !2009, !1122}
!2017 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethod", scope: !9, file: !9, line: 10, type: !586, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2018 = !DILocalVariable(name: "methodID", arg: 3, scope: !2017, file: !9, line: 10, type: !67)
!2019 = !DILocation(line: 10, scope: !2017)
!2020 = !DILocalVariable(name: "clazz", arg: 2, scope: !2017, file: !9, line: 10, type: !47)
!2021 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2017, file: !9, line: 10, type: !25)
!2022 = !DILocalVariable(name: "args", scope: !2017, file: !9, line: 10, type: !149)
!2023 = !DILocalVariable(name: "sig", scope: !2017, file: !9, line: 10, type: !1106)
!2024 = !DILocalVariable(name: "argc", scope: !2017, file: !9, line: 10, type: !13)
!2025 = !DILocalVariable(name: "argv", scope: !2017, file: !9, line: 10, type: !1111)
!2026 = !DILocalVariable(name: "i", scope: !2027, file: !9, line: 10, type: !13)
!2027 = distinct !DILexicalBlock(scope: !2017, file: !9, line: 10)
!2028 = !DILocation(line: 10, scope: !2027)
!2029 = !DILocation(line: 10, scope: !2030)
!2030 = distinct !DILexicalBlock(scope: !2031, file: !9, line: 10)
!2031 = distinct !DILexicalBlock(scope: !2027, file: !9, line: 10)
!2032 = !DILocation(line: 10, scope: !2033)
!2033 = distinct !DILexicalBlock(scope: !2030, file: !9, line: 10)
!2034 = !DILocation(line: 10, scope: !2031)
!2035 = distinct !{!2035, !2028, !2028, !1122}
!2036 = !DILocalVariable(name: "ret", scope: !2017, file: !9, line: 10, type: !174)
!2037 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethodV", scope: !9, file: !9, line: 10, type: !590, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2038 = !DILocalVariable(name: "args", arg: 4, scope: !2037, file: !9, line: 10, type: !149)
!2039 = !DILocation(line: 10, scope: !2037)
!2040 = !DILocalVariable(name: "methodID", arg: 3, scope: !2037, file: !9, line: 10, type: !67)
!2041 = !DILocalVariable(name: "clazz", arg: 2, scope: !2037, file: !9, line: 10, type: !47)
!2042 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2037, file: !9, line: 10, type: !25)
!2043 = !DILocalVariable(name: "sig", scope: !2037, file: !9, line: 10, type: !1106)
!2044 = !DILocalVariable(name: "argc", scope: !2037, file: !9, line: 10, type: !13)
!2045 = !DILocalVariable(name: "argv", scope: !2037, file: !9, line: 10, type: !1111)
!2046 = !DILocalVariable(name: "i", scope: !2047, file: !9, line: 10, type: !13)
!2047 = distinct !DILexicalBlock(scope: !2037, file: !9, line: 10)
!2048 = !DILocation(line: 10, scope: !2047)
!2049 = !DILocation(line: 10, scope: !2050)
!2050 = distinct !DILexicalBlock(scope: !2051, file: !9, line: 10)
!2051 = distinct !DILexicalBlock(scope: !2047, file: !9, line: 10)
!2052 = !DILocation(line: 10, scope: !2053)
!2053 = distinct !DILexicalBlock(scope: !2050, file: !9, line: 10)
!2054 = !DILocation(line: 10, scope: !2051)
!2055 = distinct !{!2055, !2048, !2048, !1122}
!2056 = distinct !DISubprogram(name: "JNI_CallDoubleMethod", scope: !9, file: !9, line: 11, type: !290, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2057 = !DILocalVariable(name: "methodID", arg: 3, scope: !2056, file: !9, line: 11, type: !67)
!2058 = !DILocation(line: 11, scope: !2056)
!2059 = !DILocalVariable(name: "obj", arg: 2, scope: !2056, file: !9, line: 11, type: !48)
!2060 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2056, file: !9, line: 11, type: !25)
!2061 = !DILocalVariable(name: "args", scope: !2056, file: !9, line: 11, type: !149)
!2062 = !DILocalVariable(name: "sig", scope: !2056, file: !9, line: 11, type: !1106)
!2063 = !DILocalVariable(name: "argc", scope: !2056, file: !9, line: 11, type: !13)
!2064 = !DILocalVariable(name: "argv", scope: !2056, file: !9, line: 11, type: !1111)
!2065 = !DILocalVariable(name: "i", scope: !2066, file: !9, line: 11, type: !13)
!2066 = distinct !DILexicalBlock(scope: !2056, file: !9, line: 11)
!2067 = !DILocation(line: 11, scope: !2066)
!2068 = !DILocation(line: 11, scope: !2069)
!2069 = distinct !DILexicalBlock(scope: !2070, file: !9, line: 11)
!2070 = distinct !DILexicalBlock(scope: !2066, file: !9, line: 11)
!2071 = !DILocation(line: 11, scope: !2072)
!2072 = distinct !DILexicalBlock(scope: !2069, file: !9, line: 11)
!2073 = !DILocation(line: 11, scope: !2070)
!2074 = distinct !{!2074, !2067, !2067, !1122}
!2075 = !DILocalVariable(name: "ret", scope: !2056, file: !9, line: 11, type: !177)
!2076 = distinct !DISubprogram(name: "JNI_CallDoubleMethodV", scope: !9, file: !9, line: 11, type: !294, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2077 = !DILocalVariable(name: "args", arg: 4, scope: !2076, file: !9, line: 11, type: !149)
!2078 = !DILocation(line: 11, scope: !2076)
!2079 = !DILocalVariable(name: "methodID", arg: 3, scope: !2076, file: !9, line: 11, type: !67)
!2080 = !DILocalVariable(name: "obj", arg: 2, scope: !2076, file: !9, line: 11, type: !48)
!2081 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2076, file: !9, line: 11, type: !25)
!2082 = !DILocalVariable(name: "sig", scope: !2076, file: !9, line: 11, type: !1106)
!2083 = !DILocalVariable(name: "argc", scope: !2076, file: !9, line: 11, type: !13)
!2084 = !DILocalVariable(name: "argv", scope: !2076, file: !9, line: 11, type: !1111)
!2085 = !DILocalVariable(name: "i", scope: !2086, file: !9, line: 11, type: !13)
!2086 = distinct !DILexicalBlock(scope: !2076, file: !9, line: 11)
!2087 = !DILocation(line: 11, scope: !2086)
!2088 = !DILocation(line: 11, scope: !2089)
!2089 = distinct !DILexicalBlock(scope: !2090, file: !9, line: 11)
!2090 = distinct !DILexicalBlock(scope: !2086, file: !9, line: 11)
!2091 = !DILocation(line: 11, scope: !2092)
!2092 = distinct !DILexicalBlock(scope: !2089, file: !9, line: 11)
!2093 = !DILocation(line: 11, scope: !2090)
!2094 = distinct !{!2094, !2087, !2087, !1122}
!2095 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethod", scope: !9, file: !9, line: 11, type: !410, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2096 = !DILocalVariable(name: "methodID", arg: 4, scope: !2095, file: !9, line: 11, type: !67)
!2097 = !DILocation(line: 11, scope: !2095)
!2098 = !DILocalVariable(name: "clazz", arg: 3, scope: !2095, file: !9, line: 11, type: !47)
!2099 = !DILocalVariable(name: "obj", arg: 2, scope: !2095, file: !9, line: 11, type: !48)
!2100 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2095, file: !9, line: 11, type: !25)
!2101 = !DILocalVariable(name: "args", scope: !2095, file: !9, line: 11, type: !149)
!2102 = !DILocalVariable(name: "sig", scope: !2095, file: !9, line: 11, type: !1106)
!2103 = !DILocalVariable(name: "argc", scope: !2095, file: !9, line: 11, type: !13)
!2104 = !DILocalVariable(name: "argv", scope: !2095, file: !9, line: 11, type: !1111)
!2105 = !DILocalVariable(name: "i", scope: !2106, file: !9, line: 11, type: !13)
!2106 = distinct !DILexicalBlock(scope: !2095, file: !9, line: 11)
!2107 = !DILocation(line: 11, scope: !2106)
!2108 = !DILocation(line: 11, scope: !2109)
!2109 = distinct !DILexicalBlock(scope: !2110, file: !9, line: 11)
!2110 = distinct !DILexicalBlock(scope: !2106, file: !9, line: 11)
!2111 = !DILocation(line: 11, scope: !2112)
!2112 = distinct !DILexicalBlock(scope: !2109, file: !9, line: 11)
!2113 = !DILocation(line: 11, scope: !2110)
!2114 = distinct !{!2114, !2107, !2107, !1122}
!2115 = !DILocalVariable(name: "ret", scope: !2095, file: !9, line: 11, type: !177)
!2116 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethodV", scope: !9, file: !9, line: 11, type: !414, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2117 = !DILocalVariable(name: "args", arg: 5, scope: !2116, file: !9, line: 11, type: !149)
!2118 = !DILocation(line: 11, scope: !2116)
!2119 = !DILocalVariable(name: "methodID", arg: 4, scope: !2116, file: !9, line: 11, type: !67)
!2120 = !DILocalVariable(name: "clazz", arg: 3, scope: !2116, file: !9, line: 11, type: !47)
!2121 = !DILocalVariable(name: "obj", arg: 2, scope: !2116, file: !9, line: 11, type: !48)
!2122 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2116, file: !9, line: 11, type: !25)
!2123 = !DILocalVariable(name: "sig", scope: !2116, file: !9, line: 11, type: !1106)
!2124 = !DILocalVariable(name: "argc", scope: !2116, file: !9, line: 11, type: !13)
!2125 = !DILocalVariable(name: "argv", scope: !2116, file: !9, line: 11, type: !1111)
!2126 = !DILocalVariable(name: "i", scope: !2127, file: !9, line: 11, type: !13)
!2127 = distinct !DILexicalBlock(scope: !2116, file: !9, line: 11)
!2128 = !DILocation(line: 11, scope: !2127)
!2129 = !DILocation(line: 11, scope: !2130)
!2130 = distinct !DILexicalBlock(scope: !2131, file: !9, line: 11)
!2131 = distinct !DILexicalBlock(scope: !2127, file: !9, line: 11)
!2132 = !DILocation(line: 11, scope: !2133)
!2133 = distinct !DILexicalBlock(scope: !2130, file: !9, line: 11)
!2134 = !DILocation(line: 11, scope: !2131)
!2135 = distinct !{!2135, !2128, !2128, !1122}
!2136 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethod", scope: !9, file: !9, line: 11, type: !598, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2137 = !DILocalVariable(name: "methodID", arg: 3, scope: !2136, file: !9, line: 11, type: !67)
!2138 = !DILocation(line: 11, scope: !2136)
!2139 = !DILocalVariable(name: "clazz", arg: 2, scope: !2136, file: !9, line: 11, type: !47)
!2140 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2136, file: !9, line: 11, type: !25)
!2141 = !DILocalVariable(name: "args", scope: !2136, file: !9, line: 11, type: !149)
!2142 = !DILocalVariable(name: "sig", scope: !2136, file: !9, line: 11, type: !1106)
!2143 = !DILocalVariable(name: "argc", scope: !2136, file: !9, line: 11, type: !13)
!2144 = !DILocalVariable(name: "argv", scope: !2136, file: !9, line: 11, type: !1111)
!2145 = !DILocalVariable(name: "i", scope: !2146, file: !9, line: 11, type: !13)
!2146 = distinct !DILexicalBlock(scope: !2136, file: !9, line: 11)
!2147 = !DILocation(line: 11, scope: !2146)
!2148 = !DILocation(line: 11, scope: !2149)
!2149 = distinct !DILexicalBlock(scope: !2150, file: !9, line: 11)
!2150 = distinct !DILexicalBlock(scope: !2146, file: !9, line: 11)
!2151 = !DILocation(line: 11, scope: !2152)
!2152 = distinct !DILexicalBlock(scope: !2149, file: !9, line: 11)
!2153 = !DILocation(line: 11, scope: !2150)
!2154 = distinct !{!2154, !2147, !2147, !1122}
!2155 = !DILocalVariable(name: "ret", scope: !2136, file: !9, line: 11, type: !177)
!2156 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethodV", scope: !9, file: !9, line: 11, type: !602, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2157 = !DILocalVariable(name: "args", arg: 4, scope: !2156, file: !9, line: 11, type: !149)
!2158 = !DILocation(line: 11, scope: !2156)
!2159 = !DILocalVariable(name: "methodID", arg: 3, scope: !2156, file: !9, line: 11, type: !67)
!2160 = !DILocalVariable(name: "clazz", arg: 2, scope: !2156, file: !9, line: 11, type: !47)
!2161 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2156, file: !9, line: 11, type: !25)
!2162 = !DILocalVariable(name: "sig", scope: !2156, file: !9, line: 11, type: !1106)
!2163 = !DILocalVariable(name: "argc", scope: !2156, file: !9, line: 11, type: !13)
!2164 = !DILocalVariable(name: "argv", scope: !2156, file: !9, line: 11, type: !1111)
!2165 = !DILocalVariable(name: "i", scope: !2166, file: !9, line: 11, type: !13)
!2166 = distinct !DILexicalBlock(scope: !2156, file: !9, line: 11)
!2167 = !DILocation(line: 11, scope: !2166)
!2168 = !DILocation(line: 11, scope: !2169)
!2169 = distinct !DILexicalBlock(scope: !2170, file: !9, line: 11)
!2170 = distinct !DILexicalBlock(scope: !2166, file: !9, line: 11)
!2171 = !DILocation(line: 11, scope: !2172)
!2172 = distinct !DILexicalBlock(scope: !2169, file: !9, line: 11)
!2173 = !DILocation(line: 11, scope: !2170)
!2174 = distinct !{!2174, !2167, !2167, !1122}
!2175 = distinct !DISubprogram(name: "JNI_NewObject", scope: !9, file: !9, line: 13, type: !143, scopeLine: 14, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2176 = !DILocalVariable(name: "methodID", arg: 3, scope: !2175, file: !9, line: 13, type: !67)
!2177 = !DILocation(line: 13, scope: !2175)
!2178 = !DILocalVariable(name: "clazz", arg: 2, scope: !2175, file: !9, line: 13, type: !47)
!2179 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2175, file: !9, line: 13, type: !25)
!2180 = !DILocalVariable(name: "args", scope: !2175, file: !9, line: 15, type: !149)
!2181 = !DILocation(line: 15, scope: !2175)
!2182 = !DILocation(line: 16, scope: !2175)
!2183 = !DILocalVariable(name: "sig", scope: !2175, file: !9, line: 17, type: !1106)
!2184 = !DILocation(line: 17, scope: !2175)
!2185 = !DILocalVariable(name: "argc", scope: !2175, file: !9, line: 17, type: !13)
!2186 = !DILocalVariable(name: "argv", scope: !2175, file: !9, line: 17, type: !1111)
!2187 = !DILocalVariable(name: "i", scope: !2188, file: !9, line: 17, type: !13)
!2188 = distinct !DILexicalBlock(scope: !2175, file: !9, line: 17)
!2189 = !DILocation(line: 17, scope: !2188)
!2190 = !DILocation(line: 17, scope: !2191)
!2191 = distinct !DILexicalBlock(scope: !2192, file: !9, line: 17)
!2192 = distinct !DILexicalBlock(scope: !2188, file: !9, line: 17)
!2193 = !DILocation(line: 17, scope: !2194)
!2194 = distinct !DILexicalBlock(scope: !2191, file: !9, line: 17)
!2195 = !DILocation(line: 17, scope: !2192)
!2196 = distinct !{!2196, !2189, !2189, !1122}
!2197 = !DILocalVariable(name: "o", scope: !2175, file: !9, line: 18, type: !48)
!2198 = !DILocation(line: 18, scope: !2175)
!2199 = !DILocation(line: 19, scope: !2175)
!2200 = !DILocation(line: 20, scope: !2175)
!2201 = distinct !DISubprogram(name: "JNI_NewObjectV", scope: !9, file: !9, line: 23, type: !147, scopeLine: 24, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2202 = !DILocalVariable(name: "args", arg: 4, scope: !2201, file: !9, line: 23, type: !149)
!2203 = !DILocation(line: 23, scope: !2201)
!2204 = !DILocalVariable(name: "methodID", arg: 3, scope: !2201, file: !9, line: 23, type: !67)
!2205 = !DILocalVariable(name: "clazz", arg: 2, scope: !2201, file: !9, line: 23, type: !47)
!2206 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2201, file: !9, line: 23, type: !25)
!2207 = !DILocalVariable(name: "sig", scope: !2201, file: !9, line: 25, type: !1106)
!2208 = !DILocation(line: 25, scope: !2201)
!2209 = !DILocalVariable(name: "argc", scope: !2201, file: !9, line: 25, type: !13)
!2210 = !DILocalVariable(name: "argv", scope: !2201, file: !9, line: 25, type: !1111)
!2211 = !DILocalVariable(name: "i", scope: !2212, file: !9, line: 25, type: !13)
!2212 = distinct !DILexicalBlock(scope: !2201, file: !9, line: 25)
!2213 = !DILocation(line: 25, scope: !2212)
!2214 = !DILocation(line: 25, scope: !2215)
!2215 = distinct !DILexicalBlock(scope: !2216, file: !9, line: 25)
!2216 = distinct !DILexicalBlock(scope: !2212, file: !9, line: 25)
!2217 = !DILocation(line: 25, scope: !2218)
!2218 = distinct !DILexicalBlock(scope: !2215, file: !9, line: 25)
!2219 = !DILocation(line: 25, scope: !2216)
!2220 = distinct !{!2220, !2213, !2213, !1122}
!2221 = !DILocation(line: 26, scope: !2201)
!2222 = distinct !DISubprogram(name: "JNI_CallVoidMethod", scope: !9, file: !9, line: 29, type: !302, scopeLine: 30, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2223 = !DILocalVariable(name: "methodID", arg: 3, scope: !2222, file: !9, line: 29, type: !67)
!2224 = !DILocation(line: 29, scope: !2222)
!2225 = !DILocalVariable(name: "obj", arg: 2, scope: !2222, file: !9, line: 29, type: !48)
!2226 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2222, file: !9, line: 29, type: !25)
!2227 = !DILocalVariable(name: "args", scope: !2222, file: !9, line: 31, type: !149)
!2228 = !DILocation(line: 31, scope: !2222)
!2229 = !DILocation(line: 32, scope: !2222)
!2230 = !DILocalVariable(name: "sig", scope: !2222, file: !9, line: 33, type: !1106)
!2231 = !DILocation(line: 33, scope: !2222)
!2232 = !DILocalVariable(name: "argc", scope: !2222, file: !9, line: 33, type: !13)
!2233 = !DILocalVariable(name: "argv", scope: !2222, file: !9, line: 33, type: !1111)
!2234 = !DILocalVariable(name: "i", scope: !2235, file: !9, line: 33, type: !13)
!2235 = distinct !DILexicalBlock(scope: !2222, file: !9, line: 33)
!2236 = !DILocation(line: 33, scope: !2235)
!2237 = !DILocation(line: 33, scope: !2238)
!2238 = distinct !DILexicalBlock(scope: !2239, file: !9, line: 33)
!2239 = distinct !DILexicalBlock(scope: !2235, file: !9, line: 33)
!2240 = !DILocation(line: 33, scope: !2241)
!2241 = distinct !DILexicalBlock(scope: !2238, file: !9, line: 33)
!2242 = !DILocation(line: 33, scope: !2239)
!2243 = distinct !{!2243, !2236, !2236, !1122}
!2244 = !DILocation(line: 34, scope: !2222)
!2245 = !DILocation(line: 35, scope: !2222)
!2246 = !DILocation(line: 36, scope: !2222)
!2247 = distinct !DISubprogram(name: "JNI_CallVoidMethodV", scope: !9, file: !9, line: 38, type: !306, scopeLine: 39, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2248 = !DILocalVariable(name: "args", arg: 4, scope: !2247, file: !9, line: 38, type: !149)
!2249 = !DILocation(line: 38, scope: !2247)
!2250 = !DILocalVariable(name: "methodID", arg: 3, scope: !2247, file: !9, line: 38, type: !67)
!2251 = !DILocalVariable(name: "obj", arg: 2, scope: !2247, file: !9, line: 38, type: !48)
!2252 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2247, file: !9, line: 38, type: !25)
!2253 = !DILocalVariable(name: "sig", scope: !2247, file: !9, line: 40, type: !1106)
!2254 = !DILocation(line: 40, scope: !2247)
!2255 = !DILocalVariable(name: "argc", scope: !2247, file: !9, line: 40, type: !13)
!2256 = !DILocalVariable(name: "argv", scope: !2247, file: !9, line: 40, type: !1111)
!2257 = !DILocalVariable(name: "i", scope: !2258, file: !9, line: 40, type: !13)
!2258 = distinct !DILexicalBlock(scope: !2247, file: !9, line: 40)
!2259 = !DILocation(line: 40, scope: !2258)
!2260 = !DILocation(line: 40, scope: !2261)
!2261 = distinct !DILexicalBlock(scope: !2262, file: !9, line: 40)
!2262 = distinct !DILexicalBlock(scope: !2258, file: !9, line: 40)
!2263 = !DILocation(line: 40, scope: !2264)
!2264 = distinct !DILexicalBlock(scope: !2261, file: !9, line: 40)
!2265 = !DILocation(line: 40, scope: !2262)
!2266 = distinct !{!2266, !2259, !2259, !1122}
!2267 = !DILocation(line: 41, scope: !2247)
!2268 = !DILocation(line: 42, scope: !2247)
!2269 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethod", scope: !9, file: !9, line: 44, type: !422, scopeLine: 45, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2270 = !DILocalVariable(name: "methodID", arg: 4, scope: !2269, file: !9, line: 44, type: !67)
!2271 = !DILocation(line: 44, scope: !2269)
!2272 = !DILocalVariable(name: "clazz", arg: 3, scope: !2269, file: !9, line: 44, type: !47)
!2273 = !DILocalVariable(name: "obj", arg: 2, scope: !2269, file: !9, line: 44, type: !48)
!2274 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2269, file: !9, line: 44, type: !25)
!2275 = !DILocalVariable(name: "args", scope: !2269, file: !9, line: 46, type: !149)
!2276 = !DILocation(line: 46, scope: !2269)
!2277 = !DILocation(line: 47, scope: !2269)
!2278 = !DILocalVariable(name: "sig", scope: !2269, file: !9, line: 48, type: !1106)
!2279 = !DILocation(line: 48, scope: !2269)
!2280 = !DILocalVariable(name: "argc", scope: !2269, file: !9, line: 48, type: !13)
!2281 = !DILocalVariable(name: "argv", scope: !2269, file: !9, line: 48, type: !1111)
!2282 = !DILocalVariable(name: "i", scope: !2283, file: !9, line: 48, type: !13)
!2283 = distinct !DILexicalBlock(scope: !2269, file: !9, line: 48)
!2284 = !DILocation(line: 48, scope: !2283)
!2285 = !DILocation(line: 48, scope: !2286)
!2286 = distinct !DILexicalBlock(scope: !2287, file: !9, line: 48)
!2287 = distinct !DILexicalBlock(scope: !2283, file: !9, line: 48)
!2288 = !DILocation(line: 48, scope: !2289)
!2289 = distinct !DILexicalBlock(scope: !2286, file: !9, line: 48)
!2290 = !DILocation(line: 48, scope: !2287)
!2291 = distinct !{!2291, !2284, !2284, !1122}
!2292 = !DILocation(line: 49, scope: !2269)
!2293 = !DILocation(line: 50, scope: !2269)
!2294 = !DILocation(line: 51, scope: !2269)
!2295 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethodV", scope: !9, file: !9, line: 53, type: !426, scopeLine: 54, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2296 = !DILocalVariable(name: "args", arg: 5, scope: !2295, file: !9, line: 53, type: !149)
!2297 = !DILocation(line: 53, scope: !2295)
!2298 = !DILocalVariable(name: "methodID", arg: 4, scope: !2295, file: !9, line: 53, type: !67)
!2299 = !DILocalVariable(name: "clazz", arg: 3, scope: !2295, file: !9, line: 53, type: !47)
!2300 = !DILocalVariable(name: "obj", arg: 2, scope: !2295, file: !9, line: 53, type: !48)
!2301 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2295, file: !9, line: 53, type: !25)
!2302 = !DILocalVariable(name: "sig", scope: !2295, file: !9, line: 55, type: !1106)
!2303 = !DILocation(line: 55, scope: !2295)
!2304 = !DILocalVariable(name: "argc", scope: !2295, file: !9, line: 55, type: !13)
!2305 = !DILocalVariable(name: "argv", scope: !2295, file: !9, line: 55, type: !1111)
!2306 = !DILocalVariable(name: "i", scope: !2307, file: !9, line: 55, type: !13)
!2307 = distinct !DILexicalBlock(scope: !2295, file: !9, line: 55)
!2308 = !DILocation(line: 55, scope: !2307)
!2309 = !DILocation(line: 55, scope: !2310)
!2310 = distinct !DILexicalBlock(scope: !2311, file: !9, line: 55)
!2311 = distinct !DILexicalBlock(scope: !2307, file: !9, line: 55)
!2312 = !DILocation(line: 55, scope: !2313)
!2313 = distinct !DILexicalBlock(scope: !2310, file: !9, line: 55)
!2314 = !DILocation(line: 55, scope: !2311)
!2315 = distinct !{!2315, !2308, !2308, !1122}
!2316 = !DILocation(line: 56, scope: !2295)
!2317 = !DILocation(line: 57, scope: !2295)
!2318 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethod", scope: !9, file: !9, line: 59, type: !610, scopeLine: 60, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2319 = !DILocalVariable(name: "methodID", arg: 3, scope: !2318, file: !9, line: 59, type: !67)
!2320 = !DILocation(line: 59, scope: !2318)
!2321 = !DILocalVariable(name: "clazz", arg: 2, scope: !2318, file: !9, line: 59, type: !47)
!2322 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2318, file: !9, line: 59, type: !25)
!2323 = !DILocalVariable(name: "args", scope: !2318, file: !9, line: 61, type: !149)
!2324 = !DILocation(line: 61, scope: !2318)
!2325 = !DILocation(line: 62, scope: !2318)
!2326 = !DILocalVariable(name: "sig", scope: !2318, file: !9, line: 63, type: !1106)
!2327 = !DILocation(line: 63, scope: !2318)
!2328 = !DILocalVariable(name: "argc", scope: !2318, file: !9, line: 63, type: !13)
!2329 = !DILocalVariable(name: "argv", scope: !2318, file: !9, line: 63, type: !1111)
!2330 = !DILocalVariable(name: "i", scope: !2331, file: !9, line: 63, type: !13)
!2331 = distinct !DILexicalBlock(scope: !2318, file: !9, line: 63)
!2332 = !DILocation(line: 63, scope: !2331)
!2333 = !DILocation(line: 63, scope: !2334)
!2334 = distinct !DILexicalBlock(scope: !2335, file: !9, line: 63)
!2335 = distinct !DILexicalBlock(scope: !2331, file: !9, line: 63)
!2336 = !DILocation(line: 63, scope: !2337)
!2337 = distinct !DILexicalBlock(scope: !2334, file: !9, line: 63)
!2338 = !DILocation(line: 63, scope: !2335)
!2339 = distinct !{!2339, !2332, !2332, !1122}
!2340 = !DILocation(line: 64, scope: !2318)
!2341 = !DILocation(line: 65, scope: !2318)
!2342 = !DILocation(line: 66, scope: !2318)
!2343 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethodV", scope: !9, file: !9, line: 68, type: !614, scopeLine: 69, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2344 = !DILocalVariable(name: "args", arg: 4, scope: !2343, file: !9, line: 68, type: !149)
!2345 = !DILocation(line: 68, scope: !2343)
!2346 = !DILocalVariable(name: "methodID", arg: 3, scope: !2343, file: !9, line: 68, type: !67)
!2347 = !DILocalVariable(name: "clazz", arg: 2, scope: !2343, file: !9, line: 68, type: !47)
!2348 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2343, file: !9, line: 68, type: !25)
!2349 = !DILocalVariable(name: "sig", scope: !2343, file: !9, line: 70, type: !1106)
!2350 = !DILocation(line: 70, scope: !2343)
!2351 = !DILocalVariable(name: "argc", scope: !2343, file: !9, line: 70, type: !13)
!2352 = !DILocalVariable(name: "argv", scope: !2343, file: !9, line: 70, type: !1111)
!2353 = !DILocalVariable(name: "i", scope: !2354, file: !9, line: 70, type: !13)
!2354 = distinct !DILexicalBlock(scope: !2343, file: !9, line: 70)
!2355 = !DILocation(line: 70, scope: !2354)
!2356 = !DILocation(line: 70, scope: !2357)
!2357 = distinct !DILexicalBlock(scope: !2358, file: !9, line: 70)
!2358 = distinct !DILexicalBlock(scope: !2354, file: !9, line: 70)
!2359 = !DILocation(line: 70, scope: !2360)
!2360 = distinct !DILexicalBlock(scope: !2357, file: !9, line: 70)
!2361 = !DILocation(line: 70, scope: !2358)
!2362 = distinct !{!2362, !2355, !2355, !1122}
!2363 = !DILocation(line: 71, scope: !2343)
!2364 = !DILocation(line: 72, scope: !2343)
!2365 = distinct !DISubprogram(name: "_vsprintf_l", scope: !1042, file: !1042, line: 1449, type: !2366, scopeLine: 1458, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2366 = !DISubroutineType(types: !2367)
!2367 = !{!13, !1045, !1046, !2368, !149}
!2368 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !2369)
!2369 = !DIDerivedType(tag: DW_TAG_typedef, name: "_locale_t", file: !2370, line: 623, baseType: !2371)
!2370 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt.h", directory: "", checksumkind: CSK_MD5, checksum: "4ce81db8e96f94c79f8dce86dd46b97f")
!2371 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2372, size: 32)
!2372 = !DIDerivedType(tag: DW_TAG_typedef, name: "__crt_locale_pointers", file: !2370, line: 621, baseType: !2373)
!2373 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_pointers", file: !2370, line: 617, size: 64, elements: !2374)
!2374 = !{!2375, !2378}
!2375 = !DIDerivedType(tag: DW_TAG_member, name: "locinfo", scope: !2373, file: !2370, line: 619, baseType: !2376, size: 32)
!2376 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2377, size: 32)
!2377 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_data", file: !2370, line: 619, flags: DIFlagFwdDecl)
!2378 = !DIDerivedType(tag: DW_TAG_member, name: "mbcinfo", scope: !2373, file: !2370, line: 620, baseType: !2379, size: 32, offset: 32)
!2379 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2380, size: 32)
!2380 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_multibyte_data", file: !2370, line: 620, flags: DIFlagFwdDecl)
!2381 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !2365, file: !1042, line: 1453, type: !149)
!2382 = !DILocation(line: 1453, scope: !2365)
!2383 = !DILocalVariable(name: "_Locale", arg: 3, scope: !2365, file: !1042, line: 1452, type: !2368)
!2384 = !DILocation(line: 1452, scope: !2365)
!2385 = !DILocalVariable(name: "_Format", arg: 2, scope: !2365, file: !1042, line: 1451, type: !1046)
!2386 = !DILocation(line: 1451, scope: !2365)
!2387 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2365, file: !1042, line: 1450, type: !1045)
!2388 = !DILocation(line: 1450, scope: !2365)
!2389 = !DILocation(line: 1459, scope: !2365)
!2390 = distinct !DISubprogram(name: "_vsnprintf_l", scope: !1042, file: !1042, line: 1381, type: !2391, scopeLine: 1391, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2391 = !DISubroutineType(types: !2392)
!2392 = !{!13, !1045, !1072, !1046, !2368, !149}
!2393 = !DILocalVariable(name: "_ArgList", arg: 5, scope: !2390, file: !1042, line: 1386, type: !149)
!2394 = !DILocation(line: 1386, scope: !2390)
!2395 = !DILocalVariable(name: "_Locale", arg: 4, scope: !2390, file: !1042, line: 1385, type: !2368)
!2396 = !DILocation(line: 1385, scope: !2390)
!2397 = !DILocalVariable(name: "_Format", arg: 3, scope: !2390, file: !1042, line: 1384, type: !1046)
!2398 = !DILocation(line: 1384, scope: !2390)
!2399 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !2390, file: !1042, line: 1383, type: !1072)
!2400 = !DILocation(line: 1383, scope: !2390)
!2401 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2390, file: !1042, line: 1382, type: !1045)
!2402 = !DILocation(line: 1382, scope: !2390)
!2403 = !DILocalVariable(name: "_Result", scope: !2390, file: !1042, line: 1392, type: !2404)
!2404 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !13)
!2405 = !DILocation(line: 1392, scope: !2390)
!2406 = !DILocation(line: 1396, scope: !2390)
!2407 = !DILocation(line: 92, scope: !2)
