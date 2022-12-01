; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:x-p:32:32-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32-a:0:32-S32"
target triple = "i686-pc-windows-msvc19.34.31933"

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

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat !dbg !1040 {
  %3 = alloca ptr, align 4
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  store ptr %1, ptr %3, align 4
  call void @llvm.dbg.declare(metadata ptr %3, metadata !1046, metadata !DIExpression()), !dbg !1047
  store ptr %0, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1048, metadata !DIExpression()), !dbg !1049
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1050, metadata !DIExpression()), !dbg !1051
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1052, metadata !DIExpression()), !dbg !1053
  call void @llvm.va_start(ptr %6), !dbg !1054
  %7 = load ptr, ptr %6, align 4, !dbg !1055
  %8 = load ptr, ptr %3, align 4, !dbg !1055
  %9 = load ptr, ptr %4, align 4, !dbg !1055
  %10 = call i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1055
  store i32 %10, ptr %5, align 4, !dbg !1055
  call void @llvm.va_end(ptr %6), !dbg !1056
  %11 = load i32, ptr %5, align 4, !dbg !1057
  ret i32 %11, !dbg !1057
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat !dbg !1058 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1061, metadata !DIExpression()), !dbg !1062
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1063, metadata !DIExpression()), !dbg !1064
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1065, metadata !DIExpression()), !dbg !1066
  %7 = load ptr, ptr %4, align 4, !dbg !1067
  %8 = load ptr, ptr %5, align 4, !dbg !1067
  %9 = load ptr, ptr %6, align 4, !dbg !1067
  %10 = call i32 @_vsnprintf_l(ptr noundef %9, i32 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1067
  ret i32 %10, !dbg !1067
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_snprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ...) #0 comdat !dbg !1068 {
  %4 = alloca ptr, align 4
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1072, metadata !DIExpression()), !dbg !1073
  store i32 %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1074, metadata !DIExpression()), !dbg !1075
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1076, metadata !DIExpression()), !dbg !1077
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1078, metadata !DIExpression()), !dbg !1079
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1080, metadata !DIExpression()), !dbg !1081
  call void @llvm.va_start(ptr %8), !dbg !1082
  %9 = load ptr, ptr %8, align 4, !dbg !1083
  %10 = load ptr, ptr %4, align 4, !dbg !1083
  %11 = load i32, ptr %5, align 4, !dbg !1083
  %12 = load ptr, ptr %6, align 4, !dbg !1083
  %13 = call i32 @_vsnprintf(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1083
  store i32 %13, ptr %7, align 4, !dbg !1083
  call void @llvm.va_end(ptr %8), !dbg !1084
  %14 = load i32, ptr %7, align 4, !dbg !1085
  ret i32 %14, !dbg !1085
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsnprintf(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !1086 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1089, metadata !DIExpression()), !dbg !1090
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1091, metadata !DIExpression()), !dbg !1092
  store i32 %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1093, metadata !DIExpression()), !dbg !1094
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1095, metadata !DIExpression()), !dbg !1096
  %9 = load ptr, ptr %5, align 4, !dbg !1097
  %10 = load ptr, ptr %6, align 4, !dbg !1097
  %11 = load i32, ptr %7, align 4, !dbg !1097
  %12 = load ptr, ptr %8, align 4, !dbg !1097
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i32 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9), !dbg !1097
  ret i32 %13, !dbg !1097
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1098 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1099, metadata !DIExpression()), !dbg !1100
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1101, metadata !DIExpression()), !dbg !1100
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1102, metadata !DIExpression()), !dbg !1100
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1103, metadata !DIExpression()), !dbg !1100
  call void @llvm.va_start(ptr %7), !dbg !1100
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1104, metadata !DIExpression()), !dbg !1100
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1108, metadata !DIExpression()), !dbg !1100
  %13 = load ptr, ptr %6, align 4, !dbg !1100
  %14 = load ptr, ptr %13, align 4, !dbg !1100
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1100
  %16 = load ptr, ptr %15, align 4, !dbg !1100
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1100
  %18 = load ptr, ptr %4, align 4, !dbg !1100
  %19 = load ptr, ptr %6, align 4, !dbg !1100
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1100
  store i32 %20, ptr %9, align 4, !dbg !1100
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1109, metadata !DIExpression()), !dbg !1100
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1111, metadata !DIExpression()), !dbg !1113
  store i32 0, ptr %11, align 4, !dbg !1113
  br label %21, !dbg !1113

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1113
  %23 = load i32, ptr %9, align 4, !dbg !1113
  %24 = icmp slt i32 %22, %23, !dbg !1113
  br i1 %24, label %25, label %95, !dbg !1113

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1114
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1114
  %28 = load i8, ptr %27, align 1, !dbg !1114
  %29 = sext i8 %28 to i32, !dbg !1114
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1114

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1117
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1117
  store ptr %32, ptr %7, align 4, !dbg !1117
  %33 = load i32, ptr %31, align 4, !dbg !1117
  %34 = trunc i32 %33 to i8, !dbg !1117
  %35 = load i32, ptr %11, align 4, !dbg !1117
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1117
  store i8 %34, ptr %36, align 8, !dbg !1117
  br label %91, !dbg !1117

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1117
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1117
  store ptr %39, ptr %7, align 4, !dbg !1117
  %40 = load i32, ptr %38, align 4, !dbg !1117
  %41 = trunc i32 %40 to i8, !dbg !1117
  %42 = load i32, ptr %11, align 4, !dbg !1117
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1117
  store i8 %41, ptr %43, align 8, !dbg !1117
  br label %91, !dbg !1117

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1117
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1117
  store ptr %46, ptr %7, align 4, !dbg !1117
  %47 = load i32, ptr %45, align 4, !dbg !1117
  %48 = trunc i32 %47 to i16, !dbg !1117
  %49 = load i32, ptr %11, align 4, !dbg !1117
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1117
  store i16 %48, ptr %50, align 8, !dbg !1117
  br label %91, !dbg !1117

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1117
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1117
  store ptr %53, ptr %7, align 4, !dbg !1117
  %54 = load i32, ptr %52, align 4, !dbg !1117
  %55 = trunc i32 %54 to i16, !dbg !1117
  %56 = load i32, ptr %11, align 4, !dbg !1117
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1117
  store i16 %55, ptr %57, align 8, !dbg !1117
  br label %91, !dbg !1117

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1117
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1117
  store ptr %60, ptr %7, align 4, !dbg !1117
  %61 = load i32, ptr %59, align 4, !dbg !1117
  %62 = load i32, ptr %11, align 4, !dbg !1117
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1117
  store i32 %61, ptr %63, align 8, !dbg !1117
  br label %91, !dbg !1117

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1117
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1117
  store ptr %66, ptr %7, align 4, !dbg !1117
  %67 = load i32, ptr %65, align 4, !dbg !1117
  %68 = sext i32 %67 to i64, !dbg !1117
  %69 = load i32, ptr %11, align 4, !dbg !1117
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1117
  store i64 %68, ptr %70, align 8, !dbg !1117
  br label %91, !dbg !1117

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1117
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1117
  store ptr %73, ptr %7, align 4, !dbg !1117
  %74 = load double, ptr %72, align 4, !dbg !1117
  %75 = fptrunc double %74 to float, !dbg !1117
  %76 = load i32, ptr %11, align 4, !dbg !1117
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1117
  store float %75, ptr %77, align 8, !dbg !1117
  br label %91, !dbg !1117

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1117
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1117
  store ptr %80, ptr %7, align 4, !dbg !1117
  %81 = load double, ptr %79, align 4, !dbg !1117
  %82 = load i32, ptr %11, align 4, !dbg !1117
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1117
  store double %81, ptr %83, align 8, !dbg !1117
  br label %91, !dbg !1117

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1117
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1117
  store ptr %86, ptr %7, align 4, !dbg !1117
  %87 = load ptr, ptr %85, align 4, !dbg !1117
  %88 = load i32, ptr %11, align 4, !dbg !1117
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1117
  store ptr %87, ptr %89, align 8, !dbg !1117
  br label %91, !dbg !1117

90:                                               ; preds = %25
  br label %91, !dbg !1117

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1114

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1119
  %94 = add nsw i32 %93, 1, !dbg !1119
  store i32 %94, ptr %11, align 4, !dbg !1119
  br label %21, !dbg !1119, !llvm.loop !1120

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1122, metadata !DIExpression()), !dbg !1100
  %96 = load ptr, ptr %6, align 4, !dbg !1100
  %97 = load ptr, ptr %96, align 4, !dbg !1100
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 36, !dbg !1100
  %99 = load ptr, ptr %98, align 4, !dbg !1100
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1100
  %101 = load ptr, ptr %4, align 4, !dbg !1100
  %102 = load ptr, ptr %5, align 4, !dbg !1100
  %103 = load ptr, ptr %6, align 4, !dbg !1100
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1100
  store ptr %104, ptr %12, align 4, !dbg !1100
  call void @llvm.va_end(ptr %7), !dbg !1100
  %105 = load ptr, ptr %12, align 4, !dbg !1100
  ret ptr %105, !dbg !1100
}

; Function Attrs: nocallback nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc ptr @"\01_JNI_CallObjectMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1123 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1124, metadata !DIExpression()), !dbg !1125
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1126, metadata !DIExpression()), !dbg !1125
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1127, metadata !DIExpression()), !dbg !1125
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1128, metadata !DIExpression()), !dbg !1125
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1129, metadata !DIExpression()), !dbg !1125
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1130, metadata !DIExpression()), !dbg !1125
  %13 = load ptr, ptr %8, align 4, !dbg !1125
  %14 = load ptr, ptr %13, align 4, !dbg !1125
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1125
  %16 = load ptr, ptr %15, align 4, !dbg !1125
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1125
  %18 = load ptr, ptr %6, align 4, !dbg !1125
  %19 = load ptr, ptr %8, align 4, !dbg !1125
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1125
  store i32 %20, ptr %10, align 4, !dbg !1125
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1131, metadata !DIExpression()), !dbg !1125
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1132, metadata !DIExpression()), !dbg !1134
  store i32 0, ptr %12, align 4, !dbg !1134
  br label %21, !dbg !1134

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1134
  %23 = load i32, ptr %10, align 4, !dbg !1134
  %24 = icmp slt i32 %22, %23, !dbg !1134
  br i1 %24, label %25, label %95, !dbg !1134

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1135
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1135
  %28 = load i8, ptr %27, align 1, !dbg !1135
  %29 = sext i8 %28 to i32, !dbg !1135
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1135

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1138
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1138
  store ptr %32, ptr %5, align 4, !dbg !1138
  %33 = load i32, ptr %31, align 4, !dbg !1138
  %34 = trunc i32 %33 to i8, !dbg !1138
  %35 = load i32, ptr %12, align 4, !dbg !1138
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1138
  store i8 %34, ptr %36, align 8, !dbg !1138
  br label %91, !dbg !1138

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1138
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1138
  store ptr %39, ptr %5, align 4, !dbg !1138
  %40 = load i32, ptr %38, align 4, !dbg !1138
  %41 = trunc i32 %40 to i8, !dbg !1138
  %42 = load i32, ptr %12, align 4, !dbg !1138
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1138
  store i8 %41, ptr %43, align 8, !dbg !1138
  br label %91, !dbg !1138

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1138
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1138
  store ptr %46, ptr %5, align 4, !dbg !1138
  %47 = load i32, ptr %45, align 4, !dbg !1138
  %48 = trunc i32 %47 to i16, !dbg !1138
  %49 = load i32, ptr %12, align 4, !dbg !1138
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1138
  store i16 %48, ptr %50, align 8, !dbg !1138
  br label %91, !dbg !1138

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1138
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1138
  store ptr %53, ptr %5, align 4, !dbg !1138
  %54 = load i32, ptr %52, align 4, !dbg !1138
  %55 = trunc i32 %54 to i16, !dbg !1138
  %56 = load i32, ptr %12, align 4, !dbg !1138
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1138
  store i16 %55, ptr %57, align 8, !dbg !1138
  br label %91, !dbg !1138

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1138
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1138
  store ptr %60, ptr %5, align 4, !dbg !1138
  %61 = load i32, ptr %59, align 4, !dbg !1138
  %62 = load i32, ptr %12, align 4, !dbg !1138
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1138
  store i32 %61, ptr %63, align 8, !dbg !1138
  br label %91, !dbg !1138

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1138
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1138
  store ptr %66, ptr %5, align 4, !dbg !1138
  %67 = load i32, ptr %65, align 4, !dbg !1138
  %68 = sext i32 %67 to i64, !dbg !1138
  %69 = load i32, ptr %12, align 4, !dbg !1138
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1138
  store i64 %68, ptr %70, align 8, !dbg !1138
  br label %91, !dbg !1138

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1138
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1138
  store ptr %73, ptr %5, align 4, !dbg !1138
  %74 = load double, ptr %72, align 4, !dbg !1138
  %75 = fptrunc double %74 to float, !dbg !1138
  %76 = load i32, ptr %12, align 4, !dbg !1138
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1138
  store float %75, ptr %77, align 8, !dbg !1138
  br label %91, !dbg !1138

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1138
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1138
  store ptr %80, ptr %5, align 4, !dbg !1138
  %81 = load double, ptr %79, align 4, !dbg !1138
  %82 = load i32, ptr %12, align 4, !dbg !1138
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1138
  store double %81, ptr %83, align 8, !dbg !1138
  br label %91, !dbg !1138

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1138
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1138
  store ptr %86, ptr %5, align 4, !dbg !1138
  %87 = load ptr, ptr %85, align 4, !dbg !1138
  %88 = load i32, ptr %12, align 4, !dbg !1138
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1138
  store ptr %87, ptr %89, align 8, !dbg !1138
  br label %91, !dbg !1138

90:                                               ; preds = %25
  br label %91, !dbg !1138

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1135

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1140
  %94 = add nsw i32 %93, 1, !dbg !1140
  store i32 %94, ptr %12, align 4, !dbg !1140
  br label %21, !dbg !1140, !llvm.loop !1141

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1125
  %97 = load ptr, ptr %96, align 4, !dbg !1125
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 36, !dbg !1125
  %99 = load ptr, ptr %98, align 4, !dbg !1125
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1125
  %101 = load ptr, ptr %6, align 4, !dbg !1125
  %102 = load ptr, ptr %7, align 4, !dbg !1125
  %103 = load ptr, ptr %8, align 4, !dbg !1125
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1125
  ret ptr %104, !dbg !1125
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1142 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1143, metadata !DIExpression()), !dbg !1144
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1145, metadata !DIExpression()), !dbg !1144
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1146, metadata !DIExpression()), !dbg !1144
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1147, metadata !DIExpression()), !dbg !1144
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1148, metadata !DIExpression()), !dbg !1144
  call void @llvm.va_start(ptr %9), !dbg !1144
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1149, metadata !DIExpression()), !dbg !1144
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1150, metadata !DIExpression()), !dbg !1144
  %15 = load ptr, ptr %8, align 4, !dbg !1144
  %16 = load ptr, ptr %15, align 4, !dbg !1144
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1144
  %18 = load ptr, ptr %17, align 4, !dbg !1144
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1144
  %20 = load ptr, ptr %5, align 4, !dbg !1144
  %21 = load ptr, ptr %8, align 4, !dbg !1144
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1144
  store i32 %22, ptr %11, align 4, !dbg !1144
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1151, metadata !DIExpression()), !dbg !1144
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1152, metadata !DIExpression()), !dbg !1154
  store i32 0, ptr %13, align 4, !dbg !1154
  br label %23, !dbg !1154

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1154
  %25 = load i32, ptr %11, align 4, !dbg !1154
  %26 = icmp slt i32 %24, %25, !dbg !1154
  br i1 %26, label %27, label %97, !dbg !1154

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1155
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1155
  %30 = load i8, ptr %29, align 1, !dbg !1155
  %31 = sext i8 %30 to i32, !dbg !1155
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1155

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1158
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1158
  store ptr %34, ptr %9, align 4, !dbg !1158
  %35 = load i32, ptr %33, align 4, !dbg !1158
  %36 = trunc i32 %35 to i8, !dbg !1158
  %37 = load i32, ptr %13, align 4, !dbg !1158
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1158
  store i8 %36, ptr %38, align 8, !dbg !1158
  br label %93, !dbg !1158

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1158
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1158
  store ptr %41, ptr %9, align 4, !dbg !1158
  %42 = load i32, ptr %40, align 4, !dbg !1158
  %43 = trunc i32 %42 to i8, !dbg !1158
  %44 = load i32, ptr %13, align 4, !dbg !1158
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1158
  store i8 %43, ptr %45, align 8, !dbg !1158
  br label %93, !dbg !1158

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1158
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1158
  store ptr %48, ptr %9, align 4, !dbg !1158
  %49 = load i32, ptr %47, align 4, !dbg !1158
  %50 = trunc i32 %49 to i16, !dbg !1158
  %51 = load i32, ptr %13, align 4, !dbg !1158
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1158
  store i16 %50, ptr %52, align 8, !dbg !1158
  br label %93, !dbg !1158

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1158
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1158
  store ptr %55, ptr %9, align 4, !dbg !1158
  %56 = load i32, ptr %54, align 4, !dbg !1158
  %57 = trunc i32 %56 to i16, !dbg !1158
  %58 = load i32, ptr %13, align 4, !dbg !1158
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1158
  store i16 %57, ptr %59, align 8, !dbg !1158
  br label %93, !dbg !1158

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1158
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1158
  store ptr %62, ptr %9, align 4, !dbg !1158
  %63 = load i32, ptr %61, align 4, !dbg !1158
  %64 = load i32, ptr %13, align 4, !dbg !1158
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1158
  store i32 %63, ptr %65, align 8, !dbg !1158
  br label %93, !dbg !1158

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1158
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1158
  store ptr %68, ptr %9, align 4, !dbg !1158
  %69 = load i32, ptr %67, align 4, !dbg !1158
  %70 = sext i32 %69 to i64, !dbg !1158
  %71 = load i32, ptr %13, align 4, !dbg !1158
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1158
  store i64 %70, ptr %72, align 8, !dbg !1158
  br label %93, !dbg !1158

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1158
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1158
  store ptr %75, ptr %9, align 4, !dbg !1158
  %76 = load double, ptr %74, align 4, !dbg !1158
  %77 = fptrunc double %76 to float, !dbg !1158
  %78 = load i32, ptr %13, align 4, !dbg !1158
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1158
  store float %77, ptr %79, align 8, !dbg !1158
  br label %93, !dbg !1158

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1158
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1158
  store ptr %82, ptr %9, align 4, !dbg !1158
  %83 = load double, ptr %81, align 4, !dbg !1158
  %84 = load i32, ptr %13, align 4, !dbg !1158
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1158
  store double %83, ptr %85, align 8, !dbg !1158
  br label %93, !dbg !1158

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1158
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1158
  store ptr %88, ptr %9, align 4, !dbg !1158
  %89 = load ptr, ptr %87, align 4, !dbg !1158
  %90 = load i32, ptr %13, align 4, !dbg !1158
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1158
  store ptr %89, ptr %91, align 8, !dbg !1158
  br label %93, !dbg !1158

92:                                               ; preds = %27
  br label %93, !dbg !1158

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1155

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1160
  %96 = add nsw i32 %95, 1, !dbg !1160
  store i32 %96, ptr %13, align 4, !dbg !1160
  br label %23, !dbg !1160, !llvm.loop !1161

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1162, metadata !DIExpression()), !dbg !1144
  %98 = load ptr, ptr %8, align 4, !dbg !1144
  %99 = load ptr, ptr %98, align 4, !dbg !1144
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 66, !dbg !1144
  %101 = load ptr, ptr %100, align 4, !dbg !1144
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1144
  %103 = load ptr, ptr %5, align 4, !dbg !1144
  %104 = load ptr, ptr %6, align 4, !dbg !1144
  %105 = load ptr, ptr %7, align 4, !dbg !1144
  %106 = load ptr, ptr %8, align 4, !dbg !1144
  %107 = call x86_stdcallcc ptr %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1144
  store ptr %107, ptr %14, align 4, !dbg !1144
  call void @llvm.va_end(ptr %9), !dbg !1144
  %108 = load ptr, ptr %14, align 4, !dbg !1144
  ret ptr %108, !dbg !1144
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc ptr @"\01_JNI_CallNonvirtualObjectMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1163 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1164, metadata !DIExpression()), !dbg !1165
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1166, metadata !DIExpression()), !dbg !1165
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1167, metadata !DIExpression()), !dbg !1165
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1168, metadata !DIExpression()), !dbg !1165
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1169, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1170, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1171, metadata !DIExpression()), !dbg !1165
  %15 = load ptr, ptr %10, align 4, !dbg !1165
  %16 = load ptr, ptr %15, align 4, !dbg !1165
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1165
  %18 = load ptr, ptr %17, align 4, !dbg !1165
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1165
  %20 = load ptr, ptr %7, align 4, !dbg !1165
  %21 = load ptr, ptr %10, align 4, !dbg !1165
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1165
  store i32 %22, ptr %12, align 4, !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1172, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1173, metadata !DIExpression()), !dbg !1175
  store i32 0, ptr %14, align 4, !dbg !1175
  br label %23, !dbg !1175

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1175
  %25 = load i32, ptr %12, align 4, !dbg !1175
  %26 = icmp slt i32 %24, %25, !dbg !1175
  br i1 %26, label %27, label %97, !dbg !1175

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1176
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1176
  %30 = load i8, ptr %29, align 1, !dbg !1176
  %31 = sext i8 %30 to i32, !dbg !1176
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1176

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1179
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1179
  store ptr %34, ptr %6, align 4, !dbg !1179
  %35 = load i32, ptr %33, align 4, !dbg !1179
  %36 = trunc i32 %35 to i8, !dbg !1179
  %37 = load i32, ptr %14, align 4, !dbg !1179
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1179
  store i8 %36, ptr %38, align 8, !dbg !1179
  br label %93, !dbg !1179

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1179
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1179
  store ptr %41, ptr %6, align 4, !dbg !1179
  %42 = load i32, ptr %40, align 4, !dbg !1179
  %43 = trunc i32 %42 to i8, !dbg !1179
  %44 = load i32, ptr %14, align 4, !dbg !1179
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1179
  store i8 %43, ptr %45, align 8, !dbg !1179
  br label %93, !dbg !1179

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1179
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1179
  store ptr %48, ptr %6, align 4, !dbg !1179
  %49 = load i32, ptr %47, align 4, !dbg !1179
  %50 = trunc i32 %49 to i16, !dbg !1179
  %51 = load i32, ptr %14, align 4, !dbg !1179
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1179
  store i16 %50, ptr %52, align 8, !dbg !1179
  br label %93, !dbg !1179

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1179
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1179
  store ptr %55, ptr %6, align 4, !dbg !1179
  %56 = load i32, ptr %54, align 4, !dbg !1179
  %57 = trunc i32 %56 to i16, !dbg !1179
  %58 = load i32, ptr %14, align 4, !dbg !1179
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1179
  store i16 %57, ptr %59, align 8, !dbg !1179
  br label %93, !dbg !1179

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1179
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1179
  store ptr %62, ptr %6, align 4, !dbg !1179
  %63 = load i32, ptr %61, align 4, !dbg !1179
  %64 = load i32, ptr %14, align 4, !dbg !1179
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1179
  store i32 %63, ptr %65, align 8, !dbg !1179
  br label %93, !dbg !1179

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1179
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1179
  store ptr %68, ptr %6, align 4, !dbg !1179
  %69 = load i32, ptr %67, align 4, !dbg !1179
  %70 = sext i32 %69 to i64, !dbg !1179
  %71 = load i32, ptr %14, align 4, !dbg !1179
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1179
  store i64 %70, ptr %72, align 8, !dbg !1179
  br label %93, !dbg !1179

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1179
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1179
  store ptr %75, ptr %6, align 4, !dbg !1179
  %76 = load double, ptr %74, align 4, !dbg !1179
  %77 = fptrunc double %76 to float, !dbg !1179
  %78 = load i32, ptr %14, align 4, !dbg !1179
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1179
  store float %77, ptr %79, align 8, !dbg !1179
  br label %93, !dbg !1179

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1179
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1179
  store ptr %82, ptr %6, align 4, !dbg !1179
  %83 = load double, ptr %81, align 4, !dbg !1179
  %84 = load i32, ptr %14, align 4, !dbg !1179
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1179
  store double %83, ptr %85, align 8, !dbg !1179
  br label %93, !dbg !1179

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1179
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1179
  store ptr %88, ptr %6, align 4, !dbg !1179
  %89 = load ptr, ptr %87, align 4, !dbg !1179
  %90 = load i32, ptr %14, align 4, !dbg !1179
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1179
  store ptr %89, ptr %91, align 8, !dbg !1179
  br label %93, !dbg !1179

92:                                               ; preds = %27
  br label %93, !dbg !1179

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1176

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1181
  %96 = add nsw i32 %95, 1, !dbg !1181
  store i32 %96, ptr %14, align 4, !dbg !1181
  br label %23, !dbg !1181, !llvm.loop !1182

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1165
  %99 = load ptr, ptr %98, align 4, !dbg !1165
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 66, !dbg !1165
  %101 = load ptr, ptr %100, align 4, !dbg !1165
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1165
  %103 = load ptr, ptr %7, align 4, !dbg !1165
  %104 = load ptr, ptr %8, align 4, !dbg !1165
  %105 = load ptr, ptr %9, align 4, !dbg !1165
  %106 = load ptr, ptr %10, align 4, !dbg !1165
  %107 = call x86_stdcallcc ptr %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1165
  ret ptr %107, !dbg !1165
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1183 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1184, metadata !DIExpression()), !dbg !1185
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1186, metadata !DIExpression()), !dbg !1185
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1187, metadata !DIExpression()), !dbg !1185
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1188, metadata !DIExpression()), !dbg !1185
  call void @llvm.va_start(ptr %7), !dbg !1185
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1189, metadata !DIExpression()), !dbg !1185
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1190, metadata !DIExpression()), !dbg !1185
  %13 = load ptr, ptr %6, align 4, !dbg !1185
  %14 = load ptr, ptr %13, align 4, !dbg !1185
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1185
  %16 = load ptr, ptr %15, align 4, !dbg !1185
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1185
  %18 = load ptr, ptr %4, align 4, !dbg !1185
  %19 = load ptr, ptr %6, align 4, !dbg !1185
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1185
  store i32 %20, ptr %9, align 4, !dbg !1185
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1191, metadata !DIExpression()), !dbg !1185
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1192, metadata !DIExpression()), !dbg !1194
  store i32 0, ptr %11, align 4, !dbg !1194
  br label %21, !dbg !1194

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1194
  %23 = load i32, ptr %9, align 4, !dbg !1194
  %24 = icmp slt i32 %22, %23, !dbg !1194
  br i1 %24, label %25, label %95, !dbg !1194

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1195
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1195
  %28 = load i8, ptr %27, align 1, !dbg !1195
  %29 = sext i8 %28 to i32, !dbg !1195
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1195

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1198
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1198
  store ptr %32, ptr %7, align 4, !dbg !1198
  %33 = load i32, ptr %31, align 4, !dbg !1198
  %34 = trunc i32 %33 to i8, !dbg !1198
  %35 = load i32, ptr %11, align 4, !dbg !1198
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1198
  store i8 %34, ptr %36, align 8, !dbg !1198
  br label %91, !dbg !1198

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1198
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1198
  store ptr %39, ptr %7, align 4, !dbg !1198
  %40 = load i32, ptr %38, align 4, !dbg !1198
  %41 = trunc i32 %40 to i8, !dbg !1198
  %42 = load i32, ptr %11, align 4, !dbg !1198
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1198
  store i8 %41, ptr %43, align 8, !dbg !1198
  br label %91, !dbg !1198

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1198
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1198
  store ptr %46, ptr %7, align 4, !dbg !1198
  %47 = load i32, ptr %45, align 4, !dbg !1198
  %48 = trunc i32 %47 to i16, !dbg !1198
  %49 = load i32, ptr %11, align 4, !dbg !1198
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1198
  store i16 %48, ptr %50, align 8, !dbg !1198
  br label %91, !dbg !1198

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1198
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1198
  store ptr %53, ptr %7, align 4, !dbg !1198
  %54 = load i32, ptr %52, align 4, !dbg !1198
  %55 = trunc i32 %54 to i16, !dbg !1198
  %56 = load i32, ptr %11, align 4, !dbg !1198
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1198
  store i16 %55, ptr %57, align 8, !dbg !1198
  br label %91, !dbg !1198

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1198
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1198
  store ptr %60, ptr %7, align 4, !dbg !1198
  %61 = load i32, ptr %59, align 4, !dbg !1198
  %62 = load i32, ptr %11, align 4, !dbg !1198
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1198
  store i32 %61, ptr %63, align 8, !dbg !1198
  br label %91, !dbg !1198

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1198
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1198
  store ptr %66, ptr %7, align 4, !dbg !1198
  %67 = load i32, ptr %65, align 4, !dbg !1198
  %68 = sext i32 %67 to i64, !dbg !1198
  %69 = load i32, ptr %11, align 4, !dbg !1198
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1198
  store i64 %68, ptr %70, align 8, !dbg !1198
  br label %91, !dbg !1198

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1198
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1198
  store ptr %73, ptr %7, align 4, !dbg !1198
  %74 = load double, ptr %72, align 4, !dbg !1198
  %75 = fptrunc double %74 to float, !dbg !1198
  %76 = load i32, ptr %11, align 4, !dbg !1198
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1198
  store float %75, ptr %77, align 8, !dbg !1198
  br label %91, !dbg !1198

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1198
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1198
  store ptr %80, ptr %7, align 4, !dbg !1198
  %81 = load double, ptr %79, align 4, !dbg !1198
  %82 = load i32, ptr %11, align 4, !dbg !1198
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1198
  store double %81, ptr %83, align 8, !dbg !1198
  br label %91, !dbg !1198

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1198
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1198
  store ptr %86, ptr %7, align 4, !dbg !1198
  %87 = load ptr, ptr %85, align 4, !dbg !1198
  %88 = load i32, ptr %11, align 4, !dbg !1198
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1198
  store ptr %87, ptr %89, align 8, !dbg !1198
  br label %91, !dbg !1198

90:                                               ; preds = %25
  br label %91, !dbg !1198

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1195

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1200
  %94 = add nsw i32 %93, 1, !dbg !1200
  store i32 %94, ptr %11, align 4, !dbg !1200
  br label %21, !dbg !1200, !llvm.loop !1201

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1202, metadata !DIExpression()), !dbg !1185
  %96 = load ptr, ptr %6, align 4, !dbg !1185
  %97 = load ptr, ptr %96, align 4, !dbg !1185
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 116, !dbg !1185
  %99 = load ptr, ptr %98, align 4, !dbg !1185
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1185
  %101 = load ptr, ptr %4, align 4, !dbg !1185
  %102 = load ptr, ptr %5, align 4, !dbg !1185
  %103 = load ptr, ptr %6, align 4, !dbg !1185
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1185
  store ptr %104, ptr %12, align 4, !dbg !1185
  call void @llvm.va_end(ptr %7), !dbg !1185
  %105 = load ptr, ptr %12, align 4, !dbg !1185
  ret ptr %105, !dbg !1185
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc ptr @"\01_JNI_CallStaticObjectMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1203 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1204, metadata !DIExpression()), !dbg !1205
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1206, metadata !DIExpression()), !dbg !1205
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1207, metadata !DIExpression()), !dbg !1205
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1208, metadata !DIExpression()), !dbg !1205
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1209, metadata !DIExpression()), !dbg !1205
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1210, metadata !DIExpression()), !dbg !1205
  %13 = load ptr, ptr %8, align 4, !dbg !1205
  %14 = load ptr, ptr %13, align 4, !dbg !1205
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1205
  %16 = load ptr, ptr %15, align 4, !dbg !1205
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1205
  %18 = load ptr, ptr %6, align 4, !dbg !1205
  %19 = load ptr, ptr %8, align 4, !dbg !1205
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1205
  store i32 %20, ptr %10, align 4, !dbg !1205
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1211, metadata !DIExpression()), !dbg !1205
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1212, metadata !DIExpression()), !dbg !1214
  store i32 0, ptr %12, align 4, !dbg !1214
  br label %21, !dbg !1214

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1214
  %23 = load i32, ptr %10, align 4, !dbg !1214
  %24 = icmp slt i32 %22, %23, !dbg !1214
  br i1 %24, label %25, label %95, !dbg !1214

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1215
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1215
  %28 = load i8, ptr %27, align 1, !dbg !1215
  %29 = sext i8 %28 to i32, !dbg !1215
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1215

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1218
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1218
  store ptr %32, ptr %5, align 4, !dbg !1218
  %33 = load i32, ptr %31, align 4, !dbg !1218
  %34 = trunc i32 %33 to i8, !dbg !1218
  %35 = load i32, ptr %12, align 4, !dbg !1218
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1218
  store i8 %34, ptr %36, align 8, !dbg !1218
  br label %91, !dbg !1218

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1218
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1218
  store ptr %39, ptr %5, align 4, !dbg !1218
  %40 = load i32, ptr %38, align 4, !dbg !1218
  %41 = trunc i32 %40 to i8, !dbg !1218
  %42 = load i32, ptr %12, align 4, !dbg !1218
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1218
  store i8 %41, ptr %43, align 8, !dbg !1218
  br label %91, !dbg !1218

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1218
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1218
  store ptr %46, ptr %5, align 4, !dbg !1218
  %47 = load i32, ptr %45, align 4, !dbg !1218
  %48 = trunc i32 %47 to i16, !dbg !1218
  %49 = load i32, ptr %12, align 4, !dbg !1218
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1218
  store i16 %48, ptr %50, align 8, !dbg !1218
  br label %91, !dbg !1218

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1218
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1218
  store ptr %53, ptr %5, align 4, !dbg !1218
  %54 = load i32, ptr %52, align 4, !dbg !1218
  %55 = trunc i32 %54 to i16, !dbg !1218
  %56 = load i32, ptr %12, align 4, !dbg !1218
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1218
  store i16 %55, ptr %57, align 8, !dbg !1218
  br label %91, !dbg !1218

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1218
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1218
  store ptr %60, ptr %5, align 4, !dbg !1218
  %61 = load i32, ptr %59, align 4, !dbg !1218
  %62 = load i32, ptr %12, align 4, !dbg !1218
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1218
  store i32 %61, ptr %63, align 8, !dbg !1218
  br label %91, !dbg !1218

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1218
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1218
  store ptr %66, ptr %5, align 4, !dbg !1218
  %67 = load i32, ptr %65, align 4, !dbg !1218
  %68 = sext i32 %67 to i64, !dbg !1218
  %69 = load i32, ptr %12, align 4, !dbg !1218
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1218
  store i64 %68, ptr %70, align 8, !dbg !1218
  br label %91, !dbg !1218

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1218
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1218
  store ptr %73, ptr %5, align 4, !dbg !1218
  %74 = load double, ptr %72, align 4, !dbg !1218
  %75 = fptrunc double %74 to float, !dbg !1218
  %76 = load i32, ptr %12, align 4, !dbg !1218
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1218
  store float %75, ptr %77, align 8, !dbg !1218
  br label %91, !dbg !1218

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1218
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1218
  store ptr %80, ptr %5, align 4, !dbg !1218
  %81 = load double, ptr %79, align 4, !dbg !1218
  %82 = load i32, ptr %12, align 4, !dbg !1218
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1218
  store double %81, ptr %83, align 8, !dbg !1218
  br label %91, !dbg !1218

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1218
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1218
  store ptr %86, ptr %5, align 4, !dbg !1218
  %87 = load ptr, ptr %85, align 4, !dbg !1218
  %88 = load i32, ptr %12, align 4, !dbg !1218
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1218
  store ptr %87, ptr %89, align 8, !dbg !1218
  br label %91, !dbg !1218

90:                                               ; preds = %25
  br label %91, !dbg !1218

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1215

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1220
  %94 = add nsw i32 %93, 1, !dbg !1220
  store i32 %94, ptr %12, align 4, !dbg !1220
  br label %21, !dbg !1220, !llvm.loop !1221

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1205
  %97 = load ptr, ptr %96, align 4, !dbg !1205
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 116, !dbg !1205
  %99 = load ptr, ptr %98, align 4, !dbg !1205
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1205
  %101 = load ptr, ptr %6, align 4, !dbg !1205
  %102 = load ptr, ptr %7, align 4, !dbg !1205
  %103 = load ptr, ptr %8, align 4, !dbg !1205
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1205
  ret ptr %104, !dbg !1205
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1222 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1223, metadata !DIExpression()), !dbg !1224
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1225, metadata !DIExpression()), !dbg !1224
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1226, metadata !DIExpression()), !dbg !1224
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1227, metadata !DIExpression()), !dbg !1224
  call void @llvm.va_start(ptr %7), !dbg !1224
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1228, metadata !DIExpression()), !dbg !1224
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1229, metadata !DIExpression()), !dbg !1224
  %13 = load ptr, ptr %6, align 4, !dbg !1224
  %14 = load ptr, ptr %13, align 4, !dbg !1224
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1224
  %16 = load ptr, ptr %15, align 4, !dbg !1224
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1224
  %18 = load ptr, ptr %4, align 4, !dbg !1224
  %19 = load ptr, ptr %6, align 4, !dbg !1224
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1224
  store i32 %20, ptr %9, align 4, !dbg !1224
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1230, metadata !DIExpression()), !dbg !1224
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1231, metadata !DIExpression()), !dbg !1233
  store i32 0, ptr %11, align 4, !dbg !1233
  br label %21, !dbg !1233

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1233
  %23 = load i32, ptr %9, align 4, !dbg !1233
  %24 = icmp slt i32 %22, %23, !dbg !1233
  br i1 %24, label %25, label %95, !dbg !1233

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1234
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1234
  %28 = load i8, ptr %27, align 1, !dbg !1234
  %29 = sext i8 %28 to i32, !dbg !1234
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1234

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1237
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1237
  store ptr %32, ptr %7, align 4, !dbg !1237
  %33 = load i32, ptr %31, align 4, !dbg !1237
  %34 = trunc i32 %33 to i8, !dbg !1237
  %35 = load i32, ptr %11, align 4, !dbg !1237
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1237
  store i8 %34, ptr %36, align 8, !dbg !1237
  br label %91, !dbg !1237

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1237
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1237
  store ptr %39, ptr %7, align 4, !dbg !1237
  %40 = load i32, ptr %38, align 4, !dbg !1237
  %41 = trunc i32 %40 to i8, !dbg !1237
  %42 = load i32, ptr %11, align 4, !dbg !1237
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1237
  store i8 %41, ptr %43, align 8, !dbg !1237
  br label %91, !dbg !1237

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1237
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1237
  store ptr %46, ptr %7, align 4, !dbg !1237
  %47 = load i32, ptr %45, align 4, !dbg !1237
  %48 = trunc i32 %47 to i16, !dbg !1237
  %49 = load i32, ptr %11, align 4, !dbg !1237
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1237
  store i16 %48, ptr %50, align 8, !dbg !1237
  br label %91, !dbg !1237

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1237
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1237
  store ptr %53, ptr %7, align 4, !dbg !1237
  %54 = load i32, ptr %52, align 4, !dbg !1237
  %55 = trunc i32 %54 to i16, !dbg !1237
  %56 = load i32, ptr %11, align 4, !dbg !1237
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1237
  store i16 %55, ptr %57, align 8, !dbg !1237
  br label %91, !dbg !1237

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1237
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1237
  store ptr %60, ptr %7, align 4, !dbg !1237
  %61 = load i32, ptr %59, align 4, !dbg !1237
  %62 = load i32, ptr %11, align 4, !dbg !1237
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1237
  store i32 %61, ptr %63, align 8, !dbg !1237
  br label %91, !dbg !1237

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1237
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1237
  store ptr %66, ptr %7, align 4, !dbg !1237
  %67 = load i32, ptr %65, align 4, !dbg !1237
  %68 = sext i32 %67 to i64, !dbg !1237
  %69 = load i32, ptr %11, align 4, !dbg !1237
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1237
  store i64 %68, ptr %70, align 8, !dbg !1237
  br label %91, !dbg !1237

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1237
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1237
  store ptr %73, ptr %7, align 4, !dbg !1237
  %74 = load double, ptr %72, align 4, !dbg !1237
  %75 = fptrunc double %74 to float, !dbg !1237
  %76 = load i32, ptr %11, align 4, !dbg !1237
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1237
  store float %75, ptr %77, align 8, !dbg !1237
  br label %91, !dbg !1237

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1237
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1237
  store ptr %80, ptr %7, align 4, !dbg !1237
  %81 = load double, ptr %79, align 4, !dbg !1237
  %82 = load i32, ptr %11, align 4, !dbg !1237
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1237
  store double %81, ptr %83, align 8, !dbg !1237
  br label %91, !dbg !1237

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1237
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1237
  store ptr %86, ptr %7, align 4, !dbg !1237
  %87 = load ptr, ptr %85, align 4, !dbg !1237
  %88 = load i32, ptr %11, align 4, !dbg !1237
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1237
  store ptr %87, ptr %89, align 8, !dbg !1237
  br label %91, !dbg !1237

90:                                               ; preds = %25
  br label %91, !dbg !1237

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1234

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1239
  %94 = add nsw i32 %93, 1, !dbg !1239
  store i32 %94, ptr %11, align 4, !dbg !1239
  br label %21, !dbg !1239, !llvm.loop !1240

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1241, metadata !DIExpression()), !dbg !1224
  %96 = load ptr, ptr %6, align 4, !dbg !1224
  %97 = load ptr, ptr %96, align 4, !dbg !1224
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 39, !dbg !1224
  %99 = load ptr, ptr %98, align 4, !dbg !1224
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1224
  %101 = load ptr, ptr %4, align 4, !dbg !1224
  %102 = load ptr, ptr %5, align 4, !dbg !1224
  %103 = load ptr, ptr %6, align 4, !dbg !1224
  %104 = call x86_stdcallcc zeroext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1224
  store i8 %104, ptr %12, align 1, !dbg !1224
  call void @llvm.va_end(ptr %7), !dbg !1224
  %105 = load i8, ptr %12, align 1, !dbg !1224
  ret i8 %105, !dbg !1224
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i8 @"\01_JNI_CallBooleanMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1242 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1243, metadata !DIExpression()), !dbg !1244
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1245, metadata !DIExpression()), !dbg !1244
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1246, metadata !DIExpression()), !dbg !1244
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1247, metadata !DIExpression()), !dbg !1244
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1248, metadata !DIExpression()), !dbg !1244
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1249, metadata !DIExpression()), !dbg !1244
  %13 = load ptr, ptr %8, align 4, !dbg !1244
  %14 = load ptr, ptr %13, align 4, !dbg !1244
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1244
  %16 = load ptr, ptr %15, align 4, !dbg !1244
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1244
  %18 = load ptr, ptr %6, align 4, !dbg !1244
  %19 = load ptr, ptr %8, align 4, !dbg !1244
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1244
  store i32 %20, ptr %10, align 4, !dbg !1244
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1250, metadata !DIExpression()), !dbg !1244
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1251, metadata !DIExpression()), !dbg !1253
  store i32 0, ptr %12, align 4, !dbg !1253
  br label %21, !dbg !1253

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1253
  %23 = load i32, ptr %10, align 4, !dbg !1253
  %24 = icmp slt i32 %22, %23, !dbg !1253
  br i1 %24, label %25, label %95, !dbg !1253

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1254
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1254
  %28 = load i8, ptr %27, align 1, !dbg !1254
  %29 = sext i8 %28 to i32, !dbg !1254
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1254

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1257
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1257
  store ptr %32, ptr %5, align 4, !dbg !1257
  %33 = load i32, ptr %31, align 4, !dbg !1257
  %34 = trunc i32 %33 to i8, !dbg !1257
  %35 = load i32, ptr %12, align 4, !dbg !1257
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1257
  store i8 %34, ptr %36, align 8, !dbg !1257
  br label %91, !dbg !1257

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1257
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1257
  store ptr %39, ptr %5, align 4, !dbg !1257
  %40 = load i32, ptr %38, align 4, !dbg !1257
  %41 = trunc i32 %40 to i8, !dbg !1257
  %42 = load i32, ptr %12, align 4, !dbg !1257
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1257
  store i8 %41, ptr %43, align 8, !dbg !1257
  br label %91, !dbg !1257

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1257
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1257
  store ptr %46, ptr %5, align 4, !dbg !1257
  %47 = load i32, ptr %45, align 4, !dbg !1257
  %48 = trunc i32 %47 to i16, !dbg !1257
  %49 = load i32, ptr %12, align 4, !dbg !1257
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1257
  store i16 %48, ptr %50, align 8, !dbg !1257
  br label %91, !dbg !1257

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1257
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1257
  store ptr %53, ptr %5, align 4, !dbg !1257
  %54 = load i32, ptr %52, align 4, !dbg !1257
  %55 = trunc i32 %54 to i16, !dbg !1257
  %56 = load i32, ptr %12, align 4, !dbg !1257
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1257
  store i16 %55, ptr %57, align 8, !dbg !1257
  br label %91, !dbg !1257

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1257
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1257
  store ptr %60, ptr %5, align 4, !dbg !1257
  %61 = load i32, ptr %59, align 4, !dbg !1257
  %62 = load i32, ptr %12, align 4, !dbg !1257
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1257
  store i32 %61, ptr %63, align 8, !dbg !1257
  br label %91, !dbg !1257

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1257
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1257
  store ptr %66, ptr %5, align 4, !dbg !1257
  %67 = load i32, ptr %65, align 4, !dbg !1257
  %68 = sext i32 %67 to i64, !dbg !1257
  %69 = load i32, ptr %12, align 4, !dbg !1257
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1257
  store i64 %68, ptr %70, align 8, !dbg !1257
  br label %91, !dbg !1257

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1257
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1257
  store ptr %73, ptr %5, align 4, !dbg !1257
  %74 = load double, ptr %72, align 4, !dbg !1257
  %75 = fptrunc double %74 to float, !dbg !1257
  %76 = load i32, ptr %12, align 4, !dbg !1257
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1257
  store float %75, ptr %77, align 8, !dbg !1257
  br label %91, !dbg !1257

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1257
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1257
  store ptr %80, ptr %5, align 4, !dbg !1257
  %81 = load double, ptr %79, align 4, !dbg !1257
  %82 = load i32, ptr %12, align 4, !dbg !1257
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1257
  store double %81, ptr %83, align 8, !dbg !1257
  br label %91, !dbg !1257

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1257
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1257
  store ptr %86, ptr %5, align 4, !dbg !1257
  %87 = load ptr, ptr %85, align 4, !dbg !1257
  %88 = load i32, ptr %12, align 4, !dbg !1257
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1257
  store ptr %87, ptr %89, align 8, !dbg !1257
  br label %91, !dbg !1257

90:                                               ; preds = %25
  br label %91, !dbg !1257

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1254

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1259
  %94 = add nsw i32 %93, 1, !dbg !1259
  store i32 %94, ptr %12, align 4, !dbg !1259
  br label %21, !dbg !1259, !llvm.loop !1260

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1244
  %97 = load ptr, ptr %96, align 4, !dbg !1244
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 39, !dbg !1244
  %99 = load ptr, ptr %98, align 4, !dbg !1244
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1244
  %101 = load ptr, ptr %6, align 4, !dbg !1244
  %102 = load ptr, ptr %7, align 4, !dbg !1244
  %103 = load ptr, ptr %8, align 4, !dbg !1244
  %104 = call x86_stdcallcc zeroext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1244
  ret i8 %104, !dbg !1244
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1261 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1262, metadata !DIExpression()), !dbg !1263
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1264, metadata !DIExpression()), !dbg !1263
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1265, metadata !DIExpression()), !dbg !1263
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1266, metadata !DIExpression()), !dbg !1263
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1267, metadata !DIExpression()), !dbg !1263
  call void @llvm.va_start(ptr %9), !dbg !1263
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1268, metadata !DIExpression()), !dbg !1263
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1269, metadata !DIExpression()), !dbg !1263
  %15 = load ptr, ptr %8, align 4, !dbg !1263
  %16 = load ptr, ptr %15, align 4, !dbg !1263
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1263
  %18 = load ptr, ptr %17, align 4, !dbg !1263
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1263
  %20 = load ptr, ptr %5, align 4, !dbg !1263
  %21 = load ptr, ptr %8, align 4, !dbg !1263
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1263
  store i32 %22, ptr %11, align 4, !dbg !1263
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1270, metadata !DIExpression()), !dbg !1263
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1271, metadata !DIExpression()), !dbg !1273
  store i32 0, ptr %13, align 4, !dbg !1273
  br label %23, !dbg !1273

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1273
  %25 = load i32, ptr %11, align 4, !dbg !1273
  %26 = icmp slt i32 %24, %25, !dbg !1273
  br i1 %26, label %27, label %97, !dbg !1273

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1274
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1274
  %30 = load i8, ptr %29, align 1, !dbg !1274
  %31 = sext i8 %30 to i32, !dbg !1274
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1274

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1277
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1277
  store ptr %34, ptr %9, align 4, !dbg !1277
  %35 = load i32, ptr %33, align 4, !dbg !1277
  %36 = trunc i32 %35 to i8, !dbg !1277
  %37 = load i32, ptr %13, align 4, !dbg !1277
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1277
  store i8 %36, ptr %38, align 8, !dbg !1277
  br label %93, !dbg !1277

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1277
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1277
  store ptr %41, ptr %9, align 4, !dbg !1277
  %42 = load i32, ptr %40, align 4, !dbg !1277
  %43 = trunc i32 %42 to i8, !dbg !1277
  %44 = load i32, ptr %13, align 4, !dbg !1277
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1277
  store i8 %43, ptr %45, align 8, !dbg !1277
  br label %93, !dbg !1277

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1277
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1277
  store ptr %48, ptr %9, align 4, !dbg !1277
  %49 = load i32, ptr %47, align 4, !dbg !1277
  %50 = trunc i32 %49 to i16, !dbg !1277
  %51 = load i32, ptr %13, align 4, !dbg !1277
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1277
  store i16 %50, ptr %52, align 8, !dbg !1277
  br label %93, !dbg !1277

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1277
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1277
  store ptr %55, ptr %9, align 4, !dbg !1277
  %56 = load i32, ptr %54, align 4, !dbg !1277
  %57 = trunc i32 %56 to i16, !dbg !1277
  %58 = load i32, ptr %13, align 4, !dbg !1277
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1277
  store i16 %57, ptr %59, align 8, !dbg !1277
  br label %93, !dbg !1277

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1277
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1277
  store ptr %62, ptr %9, align 4, !dbg !1277
  %63 = load i32, ptr %61, align 4, !dbg !1277
  %64 = load i32, ptr %13, align 4, !dbg !1277
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1277
  store i32 %63, ptr %65, align 8, !dbg !1277
  br label %93, !dbg !1277

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1277
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1277
  store ptr %68, ptr %9, align 4, !dbg !1277
  %69 = load i32, ptr %67, align 4, !dbg !1277
  %70 = sext i32 %69 to i64, !dbg !1277
  %71 = load i32, ptr %13, align 4, !dbg !1277
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1277
  store i64 %70, ptr %72, align 8, !dbg !1277
  br label %93, !dbg !1277

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1277
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1277
  store ptr %75, ptr %9, align 4, !dbg !1277
  %76 = load double, ptr %74, align 4, !dbg !1277
  %77 = fptrunc double %76 to float, !dbg !1277
  %78 = load i32, ptr %13, align 4, !dbg !1277
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1277
  store float %77, ptr %79, align 8, !dbg !1277
  br label %93, !dbg !1277

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1277
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1277
  store ptr %82, ptr %9, align 4, !dbg !1277
  %83 = load double, ptr %81, align 4, !dbg !1277
  %84 = load i32, ptr %13, align 4, !dbg !1277
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1277
  store double %83, ptr %85, align 8, !dbg !1277
  br label %93, !dbg !1277

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1277
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1277
  store ptr %88, ptr %9, align 4, !dbg !1277
  %89 = load ptr, ptr %87, align 4, !dbg !1277
  %90 = load i32, ptr %13, align 4, !dbg !1277
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1277
  store ptr %89, ptr %91, align 8, !dbg !1277
  br label %93, !dbg !1277

92:                                               ; preds = %27
  br label %93, !dbg !1277

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1274

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1279
  %96 = add nsw i32 %95, 1, !dbg !1279
  store i32 %96, ptr %13, align 4, !dbg !1279
  br label %23, !dbg !1279, !llvm.loop !1280

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1281, metadata !DIExpression()), !dbg !1263
  %98 = load ptr, ptr %8, align 4, !dbg !1263
  %99 = load ptr, ptr %98, align 4, !dbg !1263
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 69, !dbg !1263
  %101 = load ptr, ptr %100, align 4, !dbg !1263
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1263
  %103 = load ptr, ptr %5, align 4, !dbg !1263
  %104 = load ptr, ptr %6, align 4, !dbg !1263
  %105 = load ptr, ptr %7, align 4, !dbg !1263
  %106 = load ptr, ptr %8, align 4, !dbg !1263
  %107 = call x86_stdcallcc zeroext i8 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1263
  store i8 %107, ptr %14, align 1, !dbg !1263
  call void @llvm.va_end(ptr %9), !dbg !1263
  %108 = load i8, ptr %14, align 1, !dbg !1263
  ret i8 %108, !dbg !1263
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i8 @"\01_JNI_CallNonvirtualBooleanMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1282 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1283, metadata !DIExpression()), !dbg !1284
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1285, metadata !DIExpression()), !dbg !1284
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1286, metadata !DIExpression()), !dbg !1284
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1287, metadata !DIExpression()), !dbg !1284
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1288, metadata !DIExpression()), !dbg !1284
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1289, metadata !DIExpression()), !dbg !1284
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1290, metadata !DIExpression()), !dbg !1284
  %15 = load ptr, ptr %10, align 4, !dbg !1284
  %16 = load ptr, ptr %15, align 4, !dbg !1284
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1284
  %18 = load ptr, ptr %17, align 4, !dbg !1284
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1284
  %20 = load ptr, ptr %7, align 4, !dbg !1284
  %21 = load ptr, ptr %10, align 4, !dbg !1284
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1284
  store i32 %22, ptr %12, align 4, !dbg !1284
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1291, metadata !DIExpression()), !dbg !1284
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1292, metadata !DIExpression()), !dbg !1294
  store i32 0, ptr %14, align 4, !dbg !1294
  br label %23, !dbg !1294

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1294
  %25 = load i32, ptr %12, align 4, !dbg !1294
  %26 = icmp slt i32 %24, %25, !dbg !1294
  br i1 %26, label %27, label %97, !dbg !1294

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1295
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1295
  %30 = load i8, ptr %29, align 1, !dbg !1295
  %31 = sext i8 %30 to i32, !dbg !1295
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1295

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1298
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1298
  store ptr %34, ptr %6, align 4, !dbg !1298
  %35 = load i32, ptr %33, align 4, !dbg !1298
  %36 = trunc i32 %35 to i8, !dbg !1298
  %37 = load i32, ptr %14, align 4, !dbg !1298
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1298
  store i8 %36, ptr %38, align 8, !dbg !1298
  br label %93, !dbg !1298

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1298
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1298
  store ptr %41, ptr %6, align 4, !dbg !1298
  %42 = load i32, ptr %40, align 4, !dbg !1298
  %43 = trunc i32 %42 to i8, !dbg !1298
  %44 = load i32, ptr %14, align 4, !dbg !1298
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1298
  store i8 %43, ptr %45, align 8, !dbg !1298
  br label %93, !dbg !1298

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1298
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1298
  store ptr %48, ptr %6, align 4, !dbg !1298
  %49 = load i32, ptr %47, align 4, !dbg !1298
  %50 = trunc i32 %49 to i16, !dbg !1298
  %51 = load i32, ptr %14, align 4, !dbg !1298
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1298
  store i16 %50, ptr %52, align 8, !dbg !1298
  br label %93, !dbg !1298

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1298
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1298
  store ptr %55, ptr %6, align 4, !dbg !1298
  %56 = load i32, ptr %54, align 4, !dbg !1298
  %57 = trunc i32 %56 to i16, !dbg !1298
  %58 = load i32, ptr %14, align 4, !dbg !1298
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1298
  store i16 %57, ptr %59, align 8, !dbg !1298
  br label %93, !dbg !1298

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1298
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1298
  store ptr %62, ptr %6, align 4, !dbg !1298
  %63 = load i32, ptr %61, align 4, !dbg !1298
  %64 = load i32, ptr %14, align 4, !dbg !1298
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1298
  store i32 %63, ptr %65, align 8, !dbg !1298
  br label %93, !dbg !1298

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1298
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1298
  store ptr %68, ptr %6, align 4, !dbg !1298
  %69 = load i32, ptr %67, align 4, !dbg !1298
  %70 = sext i32 %69 to i64, !dbg !1298
  %71 = load i32, ptr %14, align 4, !dbg !1298
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1298
  store i64 %70, ptr %72, align 8, !dbg !1298
  br label %93, !dbg !1298

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1298
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1298
  store ptr %75, ptr %6, align 4, !dbg !1298
  %76 = load double, ptr %74, align 4, !dbg !1298
  %77 = fptrunc double %76 to float, !dbg !1298
  %78 = load i32, ptr %14, align 4, !dbg !1298
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1298
  store float %77, ptr %79, align 8, !dbg !1298
  br label %93, !dbg !1298

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1298
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1298
  store ptr %82, ptr %6, align 4, !dbg !1298
  %83 = load double, ptr %81, align 4, !dbg !1298
  %84 = load i32, ptr %14, align 4, !dbg !1298
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1298
  store double %83, ptr %85, align 8, !dbg !1298
  br label %93, !dbg !1298

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1298
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1298
  store ptr %88, ptr %6, align 4, !dbg !1298
  %89 = load ptr, ptr %87, align 4, !dbg !1298
  %90 = load i32, ptr %14, align 4, !dbg !1298
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1298
  store ptr %89, ptr %91, align 8, !dbg !1298
  br label %93, !dbg !1298

92:                                               ; preds = %27
  br label %93, !dbg !1298

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1295

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1300
  %96 = add nsw i32 %95, 1, !dbg !1300
  store i32 %96, ptr %14, align 4, !dbg !1300
  br label %23, !dbg !1300, !llvm.loop !1301

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1284
  %99 = load ptr, ptr %98, align 4, !dbg !1284
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 69, !dbg !1284
  %101 = load ptr, ptr %100, align 4, !dbg !1284
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1284
  %103 = load ptr, ptr %7, align 4, !dbg !1284
  %104 = load ptr, ptr %8, align 4, !dbg !1284
  %105 = load ptr, ptr %9, align 4, !dbg !1284
  %106 = load ptr, ptr %10, align 4, !dbg !1284
  %107 = call x86_stdcallcc zeroext i8 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1284
  ret i8 %107, !dbg !1284
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1302 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1303, metadata !DIExpression()), !dbg !1304
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1305, metadata !DIExpression()), !dbg !1304
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1306, metadata !DIExpression()), !dbg !1304
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1307, metadata !DIExpression()), !dbg !1304
  call void @llvm.va_start(ptr %7), !dbg !1304
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1308, metadata !DIExpression()), !dbg !1304
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1309, metadata !DIExpression()), !dbg !1304
  %13 = load ptr, ptr %6, align 4, !dbg !1304
  %14 = load ptr, ptr %13, align 4, !dbg !1304
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1304
  %16 = load ptr, ptr %15, align 4, !dbg !1304
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1304
  %18 = load ptr, ptr %4, align 4, !dbg !1304
  %19 = load ptr, ptr %6, align 4, !dbg !1304
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1304
  store i32 %20, ptr %9, align 4, !dbg !1304
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1310, metadata !DIExpression()), !dbg !1304
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1311, metadata !DIExpression()), !dbg !1313
  store i32 0, ptr %11, align 4, !dbg !1313
  br label %21, !dbg !1313

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1313
  %23 = load i32, ptr %9, align 4, !dbg !1313
  %24 = icmp slt i32 %22, %23, !dbg !1313
  br i1 %24, label %25, label %95, !dbg !1313

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1314
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1314
  %28 = load i8, ptr %27, align 1, !dbg !1314
  %29 = sext i8 %28 to i32, !dbg !1314
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1314

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1317
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1317
  store ptr %32, ptr %7, align 4, !dbg !1317
  %33 = load i32, ptr %31, align 4, !dbg !1317
  %34 = trunc i32 %33 to i8, !dbg !1317
  %35 = load i32, ptr %11, align 4, !dbg !1317
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1317
  store i8 %34, ptr %36, align 8, !dbg !1317
  br label %91, !dbg !1317

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1317
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1317
  store ptr %39, ptr %7, align 4, !dbg !1317
  %40 = load i32, ptr %38, align 4, !dbg !1317
  %41 = trunc i32 %40 to i8, !dbg !1317
  %42 = load i32, ptr %11, align 4, !dbg !1317
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1317
  store i8 %41, ptr %43, align 8, !dbg !1317
  br label %91, !dbg !1317

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1317
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1317
  store ptr %46, ptr %7, align 4, !dbg !1317
  %47 = load i32, ptr %45, align 4, !dbg !1317
  %48 = trunc i32 %47 to i16, !dbg !1317
  %49 = load i32, ptr %11, align 4, !dbg !1317
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1317
  store i16 %48, ptr %50, align 8, !dbg !1317
  br label %91, !dbg !1317

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1317
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1317
  store ptr %53, ptr %7, align 4, !dbg !1317
  %54 = load i32, ptr %52, align 4, !dbg !1317
  %55 = trunc i32 %54 to i16, !dbg !1317
  %56 = load i32, ptr %11, align 4, !dbg !1317
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1317
  store i16 %55, ptr %57, align 8, !dbg !1317
  br label %91, !dbg !1317

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1317
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1317
  store ptr %60, ptr %7, align 4, !dbg !1317
  %61 = load i32, ptr %59, align 4, !dbg !1317
  %62 = load i32, ptr %11, align 4, !dbg !1317
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1317
  store i32 %61, ptr %63, align 8, !dbg !1317
  br label %91, !dbg !1317

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1317
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1317
  store ptr %66, ptr %7, align 4, !dbg !1317
  %67 = load i32, ptr %65, align 4, !dbg !1317
  %68 = sext i32 %67 to i64, !dbg !1317
  %69 = load i32, ptr %11, align 4, !dbg !1317
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1317
  store i64 %68, ptr %70, align 8, !dbg !1317
  br label %91, !dbg !1317

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1317
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1317
  store ptr %73, ptr %7, align 4, !dbg !1317
  %74 = load double, ptr %72, align 4, !dbg !1317
  %75 = fptrunc double %74 to float, !dbg !1317
  %76 = load i32, ptr %11, align 4, !dbg !1317
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1317
  store float %75, ptr %77, align 8, !dbg !1317
  br label %91, !dbg !1317

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1317
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1317
  store ptr %80, ptr %7, align 4, !dbg !1317
  %81 = load double, ptr %79, align 4, !dbg !1317
  %82 = load i32, ptr %11, align 4, !dbg !1317
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1317
  store double %81, ptr %83, align 8, !dbg !1317
  br label %91, !dbg !1317

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1317
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1317
  store ptr %86, ptr %7, align 4, !dbg !1317
  %87 = load ptr, ptr %85, align 4, !dbg !1317
  %88 = load i32, ptr %11, align 4, !dbg !1317
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1317
  store ptr %87, ptr %89, align 8, !dbg !1317
  br label %91, !dbg !1317

90:                                               ; preds = %25
  br label %91, !dbg !1317

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1314

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1319
  %94 = add nsw i32 %93, 1, !dbg !1319
  store i32 %94, ptr %11, align 4, !dbg !1319
  br label %21, !dbg !1319, !llvm.loop !1320

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1321, metadata !DIExpression()), !dbg !1304
  %96 = load ptr, ptr %6, align 4, !dbg !1304
  %97 = load ptr, ptr %96, align 4, !dbg !1304
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 119, !dbg !1304
  %99 = load ptr, ptr %98, align 4, !dbg !1304
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1304
  %101 = load ptr, ptr %4, align 4, !dbg !1304
  %102 = load ptr, ptr %5, align 4, !dbg !1304
  %103 = load ptr, ptr %6, align 4, !dbg !1304
  %104 = call x86_stdcallcc zeroext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1304
  store i8 %104, ptr %12, align 1, !dbg !1304
  call void @llvm.va_end(ptr %7), !dbg !1304
  %105 = load i8, ptr %12, align 1, !dbg !1304
  ret i8 %105, !dbg !1304
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i8 @"\01_JNI_CallStaticBooleanMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1322 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1323, metadata !DIExpression()), !dbg !1324
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1325, metadata !DIExpression()), !dbg !1324
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1326, metadata !DIExpression()), !dbg !1324
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1327, metadata !DIExpression()), !dbg !1324
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1328, metadata !DIExpression()), !dbg !1324
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1329, metadata !DIExpression()), !dbg !1324
  %13 = load ptr, ptr %8, align 4, !dbg !1324
  %14 = load ptr, ptr %13, align 4, !dbg !1324
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1324
  %16 = load ptr, ptr %15, align 4, !dbg !1324
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1324
  %18 = load ptr, ptr %6, align 4, !dbg !1324
  %19 = load ptr, ptr %8, align 4, !dbg !1324
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1324
  store i32 %20, ptr %10, align 4, !dbg !1324
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1330, metadata !DIExpression()), !dbg !1324
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1331, metadata !DIExpression()), !dbg !1333
  store i32 0, ptr %12, align 4, !dbg !1333
  br label %21, !dbg !1333

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1333
  %23 = load i32, ptr %10, align 4, !dbg !1333
  %24 = icmp slt i32 %22, %23, !dbg !1333
  br i1 %24, label %25, label %95, !dbg !1333

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1334
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1334
  %28 = load i8, ptr %27, align 1, !dbg !1334
  %29 = sext i8 %28 to i32, !dbg !1334
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1334

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1337
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1337
  store ptr %32, ptr %5, align 4, !dbg !1337
  %33 = load i32, ptr %31, align 4, !dbg !1337
  %34 = trunc i32 %33 to i8, !dbg !1337
  %35 = load i32, ptr %12, align 4, !dbg !1337
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1337
  store i8 %34, ptr %36, align 8, !dbg !1337
  br label %91, !dbg !1337

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1337
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1337
  store ptr %39, ptr %5, align 4, !dbg !1337
  %40 = load i32, ptr %38, align 4, !dbg !1337
  %41 = trunc i32 %40 to i8, !dbg !1337
  %42 = load i32, ptr %12, align 4, !dbg !1337
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1337
  store i8 %41, ptr %43, align 8, !dbg !1337
  br label %91, !dbg !1337

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1337
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1337
  store ptr %46, ptr %5, align 4, !dbg !1337
  %47 = load i32, ptr %45, align 4, !dbg !1337
  %48 = trunc i32 %47 to i16, !dbg !1337
  %49 = load i32, ptr %12, align 4, !dbg !1337
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1337
  store i16 %48, ptr %50, align 8, !dbg !1337
  br label %91, !dbg !1337

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1337
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1337
  store ptr %53, ptr %5, align 4, !dbg !1337
  %54 = load i32, ptr %52, align 4, !dbg !1337
  %55 = trunc i32 %54 to i16, !dbg !1337
  %56 = load i32, ptr %12, align 4, !dbg !1337
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1337
  store i16 %55, ptr %57, align 8, !dbg !1337
  br label %91, !dbg !1337

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1337
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1337
  store ptr %60, ptr %5, align 4, !dbg !1337
  %61 = load i32, ptr %59, align 4, !dbg !1337
  %62 = load i32, ptr %12, align 4, !dbg !1337
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1337
  store i32 %61, ptr %63, align 8, !dbg !1337
  br label %91, !dbg !1337

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1337
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1337
  store ptr %66, ptr %5, align 4, !dbg !1337
  %67 = load i32, ptr %65, align 4, !dbg !1337
  %68 = sext i32 %67 to i64, !dbg !1337
  %69 = load i32, ptr %12, align 4, !dbg !1337
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1337
  store i64 %68, ptr %70, align 8, !dbg !1337
  br label %91, !dbg !1337

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1337
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1337
  store ptr %73, ptr %5, align 4, !dbg !1337
  %74 = load double, ptr %72, align 4, !dbg !1337
  %75 = fptrunc double %74 to float, !dbg !1337
  %76 = load i32, ptr %12, align 4, !dbg !1337
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1337
  store float %75, ptr %77, align 8, !dbg !1337
  br label %91, !dbg !1337

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1337
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1337
  store ptr %80, ptr %5, align 4, !dbg !1337
  %81 = load double, ptr %79, align 4, !dbg !1337
  %82 = load i32, ptr %12, align 4, !dbg !1337
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1337
  store double %81, ptr %83, align 8, !dbg !1337
  br label %91, !dbg !1337

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1337
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1337
  store ptr %86, ptr %5, align 4, !dbg !1337
  %87 = load ptr, ptr %85, align 4, !dbg !1337
  %88 = load i32, ptr %12, align 4, !dbg !1337
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1337
  store ptr %87, ptr %89, align 8, !dbg !1337
  br label %91, !dbg !1337

90:                                               ; preds = %25
  br label %91, !dbg !1337

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1334

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1339
  %94 = add nsw i32 %93, 1, !dbg !1339
  store i32 %94, ptr %12, align 4, !dbg !1339
  br label %21, !dbg !1339, !llvm.loop !1340

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1324
  %97 = load ptr, ptr %96, align 4, !dbg !1324
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 119, !dbg !1324
  %99 = load ptr, ptr %98, align 4, !dbg !1324
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1324
  %101 = load ptr, ptr %6, align 4, !dbg !1324
  %102 = load ptr, ptr %7, align 4, !dbg !1324
  %103 = load ptr, ptr %8, align 4, !dbg !1324
  %104 = call x86_stdcallcc zeroext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1324
  ret i8 %104, !dbg !1324
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1341 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1342, metadata !DIExpression()), !dbg !1343
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1344, metadata !DIExpression()), !dbg !1343
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1345, metadata !DIExpression()), !dbg !1343
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1346, metadata !DIExpression()), !dbg !1343
  call void @llvm.va_start(ptr %7), !dbg !1343
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1347, metadata !DIExpression()), !dbg !1343
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1348, metadata !DIExpression()), !dbg !1343
  %13 = load ptr, ptr %6, align 4, !dbg !1343
  %14 = load ptr, ptr %13, align 4, !dbg !1343
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1343
  %16 = load ptr, ptr %15, align 4, !dbg !1343
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1343
  %18 = load ptr, ptr %4, align 4, !dbg !1343
  %19 = load ptr, ptr %6, align 4, !dbg !1343
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1343
  store i32 %20, ptr %9, align 4, !dbg !1343
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1349, metadata !DIExpression()), !dbg !1343
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1350, metadata !DIExpression()), !dbg !1352
  store i32 0, ptr %11, align 4, !dbg !1352
  br label %21, !dbg !1352

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1352
  %23 = load i32, ptr %9, align 4, !dbg !1352
  %24 = icmp slt i32 %22, %23, !dbg !1352
  br i1 %24, label %25, label %95, !dbg !1352

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1353
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1353
  %28 = load i8, ptr %27, align 1, !dbg !1353
  %29 = sext i8 %28 to i32, !dbg !1353
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1353

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1356
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1356
  store ptr %32, ptr %7, align 4, !dbg !1356
  %33 = load i32, ptr %31, align 4, !dbg !1356
  %34 = trunc i32 %33 to i8, !dbg !1356
  %35 = load i32, ptr %11, align 4, !dbg !1356
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1356
  store i8 %34, ptr %36, align 8, !dbg !1356
  br label %91, !dbg !1356

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1356
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1356
  store ptr %39, ptr %7, align 4, !dbg !1356
  %40 = load i32, ptr %38, align 4, !dbg !1356
  %41 = trunc i32 %40 to i8, !dbg !1356
  %42 = load i32, ptr %11, align 4, !dbg !1356
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1356
  store i8 %41, ptr %43, align 8, !dbg !1356
  br label %91, !dbg !1356

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1356
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1356
  store ptr %46, ptr %7, align 4, !dbg !1356
  %47 = load i32, ptr %45, align 4, !dbg !1356
  %48 = trunc i32 %47 to i16, !dbg !1356
  %49 = load i32, ptr %11, align 4, !dbg !1356
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1356
  store i16 %48, ptr %50, align 8, !dbg !1356
  br label %91, !dbg !1356

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1356
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1356
  store ptr %53, ptr %7, align 4, !dbg !1356
  %54 = load i32, ptr %52, align 4, !dbg !1356
  %55 = trunc i32 %54 to i16, !dbg !1356
  %56 = load i32, ptr %11, align 4, !dbg !1356
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1356
  store i16 %55, ptr %57, align 8, !dbg !1356
  br label %91, !dbg !1356

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1356
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1356
  store ptr %60, ptr %7, align 4, !dbg !1356
  %61 = load i32, ptr %59, align 4, !dbg !1356
  %62 = load i32, ptr %11, align 4, !dbg !1356
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1356
  store i32 %61, ptr %63, align 8, !dbg !1356
  br label %91, !dbg !1356

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1356
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1356
  store ptr %66, ptr %7, align 4, !dbg !1356
  %67 = load i32, ptr %65, align 4, !dbg !1356
  %68 = sext i32 %67 to i64, !dbg !1356
  %69 = load i32, ptr %11, align 4, !dbg !1356
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1356
  store i64 %68, ptr %70, align 8, !dbg !1356
  br label %91, !dbg !1356

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1356
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1356
  store ptr %73, ptr %7, align 4, !dbg !1356
  %74 = load double, ptr %72, align 4, !dbg !1356
  %75 = fptrunc double %74 to float, !dbg !1356
  %76 = load i32, ptr %11, align 4, !dbg !1356
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1356
  store float %75, ptr %77, align 8, !dbg !1356
  br label %91, !dbg !1356

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1356
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1356
  store ptr %80, ptr %7, align 4, !dbg !1356
  %81 = load double, ptr %79, align 4, !dbg !1356
  %82 = load i32, ptr %11, align 4, !dbg !1356
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1356
  store double %81, ptr %83, align 8, !dbg !1356
  br label %91, !dbg !1356

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1356
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1356
  store ptr %86, ptr %7, align 4, !dbg !1356
  %87 = load ptr, ptr %85, align 4, !dbg !1356
  %88 = load i32, ptr %11, align 4, !dbg !1356
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1356
  store ptr %87, ptr %89, align 8, !dbg !1356
  br label %91, !dbg !1356

90:                                               ; preds = %25
  br label %91, !dbg !1356

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1353

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1358
  %94 = add nsw i32 %93, 1, !dbg !1358
  store i32 %94, ptr %11, align 4, !dbg !1358
  br label %21, !dbg !1358, !llvm.loop !1359

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1360, metadata !DIExpression()), !dbg !1343
  %96 = load ptr, ptr %6, align 4, !dbg !1343
  %97 = load ptr, ptr %96, align 4, !dbg !1343
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 42, !dbg !1343
  %99 = load ptr, ptr %98, align 4, !dbg !1343
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1343
  %101 = load ptr, ptr %4, align 4, !dbg !1343
  %102 = load ptr, ptr %5, align 4, !dbg !1343
  %103 = load ptr, ptr %6, align 4, !dbg !1343
  %104 = call x86_stdcallcc signext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1343
  store i8 %104, ptr %12, align 1, !dbg !1343
  call void @llvm.va_end(ptr %7), !dbg !1343
  %105 = load i8, ptr %12, align 1, !dbg !1343
  ret i8 %105, !dbg !1343
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i8 @"\01_JNI_CallByteMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1361 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1362, metadata !DIExpression()), !dbg !1363
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1364, metadata !DIExpression()), !dbg !1363
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1365, metadata !DIExpression()), !dbg !1363
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1366, metadata !DIExpression()), !dbg !1363
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1367, metadata !DIExpression()), !dbg !1363
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1368, metadata !DIExpression()), !dbg !1363
  %13 = load ptr, ptr %8, align 4, !dbg !1363
  %14 = load ptr, ptr %13, align 4, !dbg !1363
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1363
  %16 = load ptr, ptr %15, align 4, !dbg !1363
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1363
  %18 = load ptr, ptr %6, align 4, !dbg !1363
  %19 = load ptr, ptr %8, align 4, !dbg !1363
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1363
  store i32 %20, ptr %10, align 4, !dbg !1363
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1369, metadata !DIExpression()), !dbg !1363
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1370, metadata !DIExpression()), !dbg !1372
  store i32 0, ptr %12, align 4, !dbg !1372
  br label %21, !dbg !1372

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1372
  %23 = load i32, ptr %10, align 4, !dbg !1372
  %24 = icmp slt i32 %22, %23, !dbg !1372
  br i1 %24, label %25, label %95, !dbg !1372

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1373
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1373
  %28 = load i8, ptr %27, align 1, !dbg !1373
  %29 = sext i8 %28 to i32, !dbg !1373
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1373

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1376
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1376
  store ptr %32, ptr %5, align 4, !dbg !1376
  %33 = load i32, ptr %31, align 4, !dbg !1376
  %34 = trunc i32 %33 to i8, !dbg !1376
  %35 = load i32, ptr %12, align 4, !dbg !1376
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1376
  store i8 %34, ptr %36, align 8, !dbg !1376
  br label %91, !dbg !1376

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1376
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1376
  store ptr %39, ptr %5, align 4, !dbg !1376
  %40 = load i32, ptr %38, align 4, !dbg !1376
  %41 = trunc i32 %40 to i8, !dbg !1376
  %42 = load i32, ptr %12, align 4, !dbg !1376
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1376
  store i8 %41, ptr %43, align 8, !dbg !1376
  br label %91, !dbg !1376

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1376
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1376
  store ptr %46, ptr %5, align 4, !dbg !1376
  %47 = load i32, ptr %45, align 4, !dbg !1376
  %48 = trunc i32 %47 to i16, !dbg !1376
  %49 = load i32, ptr %12, align 4, !dbg !1376
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1376
  store i16 %48, ptr %50, align 8, !dbg !1376
  br label %91, !dbg !1376

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1376
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1376
  store ptr %53, ptr %5, align 4, !dbg !1376
  %54 = load i32, ptr %52, align 4, !dbg !1376
  %55 = trunc i32 %54 to i16, !dbg !1376
  %56 = load i32, ptr %12, align 4, !dbg !1376
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1376
  store i16 %55, ptr %57, align 8, !dbg !1376
  br label %91, !dbg !1376

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1376
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1376
  store ptr %60, ptr %5, align 4, !dbg !1376
  %61 = load i32, ptr %59, align 4, !dbg !1376
  %62 = load i32, ptr %12, align 4, !dbg !1376
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1376
  store i32 %61, ptr %63, align 8, !dbg !1376
  br label %91, !dbg !1376

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1376
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1376
  store ptr %66, ptr %5, align 4, !dbg !1376
  %67 = load i32, ptr %65, align 4, !dbg !1376
  %68 = sext i32 %67 to i64, !dbg !1376
  %69 = load i32, ptr %12, align 4, !dbg !1376
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1376
  store i64 %68, ptr %70, align 8, !dbg !1376
  br label %91, !dbg !1376

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1376
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1376
  store ptr %73, ptr %5, align 4, !dbg !1376
  %74 = load double, ptr %72, align 4, !dbg !1376
  %75 = fptrunc double %74 to float, !dbg !1376
  %76 = load i32, ptr %12, align 4, !dbg !1376
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1376
  store float %75, ptr %77, align 8, !dbg !1376
  br label %91, !dbg !1376

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1376
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1376
  store ptr %80, ptr %5, align 4, !dbg !1376
  %81 = load double, ptr %79, align 4, !dbg !1376
  %82 = load i32, ptr %12, align 4, !dbg !1376
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1376
  store double %81, ptr %83, align 8, !dbg !1376
  br label %91, !dbg !1376

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1376
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1376
  store ptr %86, ptr %5, align 4, !dbg !1376
  %87 = load ptr, ptr %85, align 4, !dbg !1376
  %88 = load i32, ptr %12, align 4, !dbg !1376
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1376
  store ptr %87, ptr %89, align 8, !dbg !1376
  br label %91, !dbg !1376

90:                                               ; preds = %25
  br label %91, !dbg !1376

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1373

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1378
  %94 = add nsw i32 %93, 1, !dbg !1378
  store i32 %94, ptr %12, align 4, !dbg !1378
  br label %21, !dbg !1378, !llvm.loop !1379

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1363
  %97 = load ptr, ptr %96, align 4, !dbg !1363
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 42, !dbg !1363
  %99 = load ptr, ptr %98, align 4, !dbg !1363
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1363
  %101 = load ptr, ptr %6, align 4, !dbg !1363
  %102 = load ptr, ptr %7, align 4, !dbg !1363
  %103 = load ptr, ptr %8, align 4, !dbg !1363
  %104 = call x86_stdcallcc signext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1363
  ret i8 %104, !dbg !1363
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1380 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1381, metadata !DIExpression()), !dbg !1382
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1383, metadata !DIExpression()), !dbg !1382
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1384, metadata !DIExpression()), !dbg !1382
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1385, metadata !DIExpression()), !dbg !1382
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1386, metadata !DIExpression()), !dbg !1382
  call void @llvm.va_start(ptr %9), !dbg !1382
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1387, metadata !DIExpression()), !dbg !1382
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1388, metadata !DIExpression()), !dbg !1382
  %15 = load ptr, ptr %8, align 4, !dbg !1382
  %16 = load ptr, ptr %15, align 4, !dbg !1382
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1382
  %18 = load ptr, ptr %17, align 4, !dbg !1382
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1382
  %20 = load ptr, ptr %5, align 4, !dbg !1382
  %21 = load ptr, ptr %8, align 4, !dbg !1382
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1382
  store i32 %22, ptr %11, align 4, !dbg !1382
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1389, metadata !DIExpression()), !dbg !1382
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1390, metadata !DIExpression()), !dbg !1392
  store i32 0, ptr %13, align 4, !dbg !1392
  br label %23, !dbg !1392

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1392
  %25 = load i32, ptr %11, align 4, !dbg !1392
  %26 = icmp slt i32 %24, %25, !dbg !1392
  br i1 %26, label %27, label %97, !dbg !1392

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1393
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1393
  %30 = load i8, ptr %29, align 1, !dbg !1393
  %31 = sext i8 %30 to i32, !dbg !1393
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1393

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1396
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1396
  store ptr %34, ptr %9, align 4, !dbg !1396
  %35 = load i32, ptr %33, align 4, !dbg !1396
  %36 = trunc i32 %35 to i8, !dbg !1396
  %37 = load i32, ptr %13, align 4, !dbg !1396
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1396
  store i8 %36, ptr %38, align 8, !dbg !1396
  br label %93, !dbg !1396

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1396
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1396
  store ptr %41, ptr %9, align 4, !dbg !1396
  %42 = load i32, ptr %40, align 4, !dbg !1396
  %43 = trunc i32 %42 to i8, !dbg !1396
  %44 = load i32, ptr %13, align 4, !dbg !1396
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1396
  store i8 %43, ptr %45, align 8, !dbg !1396
  br label %93, !dbg !1396

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1396
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1396
  store ptr %48, ptr %9, align 4, !dbg !1396
  %49 = load i32, ptr %47, align 4, !dbg !1396
  %50 = trunc i32 %49 to i16, !dbg !1396
  %51 = load i32, ptr %13, align 4, !dbg !1396
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1396
  store i16 %50, ptr %52, align 8, !dbg !1396
  br label %93, !dbg !1396

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1396
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1396
  store ptr %55, ptr %9, align 4, !dbg !1396
  %56 = load i32, ptr %54, align 4, !dbg !1396
  %57 = trunc i32 %56 to i16, !dbg !1396
  %58 = load i32, ptr %13, align 4, !dbg !1396
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1396
  store i16 %57, ptr %59, align 8, !dbg !1396
  br label %93, !dbg !1396

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1396
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1396
  store ptr %62, ptr %9, align 4, !dbg !1396
  %63 = load i32, ptr %61, align 4, !dbg !1396
  %64 = load i32, ptr %13, align 4, !dbg !1396
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1396
  store i32 %63, ptr %65, align 8, !dbg !1396
  br label %93, !dbg !1396

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1396
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1396
  store ptr %68, ptr %9, align 4, !dbg !1396
  %69 = load i32, ptr %67, align 4, !dbg !1396
  %70 = sext i32 %69 to i64, !dbg !1396
  %71 = load i32, ptr %13, align 4, !dbg !1396
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1396
  store i64 %70, ptr %72, align 8, !dbg !1396
  br label %93, !dbg !1396

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1396
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1396
  store ptr %75, ptr %9, align 4, !dbg !1396
  %76 = load double, ptr %74, align 4, !dbg !1396
  %77 = fptrunc double %76 to float, !dbg !1396
  %78 = load i32, ptr %13, align 4, !dbg !1396
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1396
  store float %77, ptr %79, align 8, !dbg !1396
  br label %93, !dbg !1396

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1396
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1396
  store ptr %82, ptr %9, align 4, !dbg !1396
  %83 = load double, ptr %81, align 4, !dbg !1396
  %84 = load i32, ptr %13, align 4, !dbg !1396
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1396
  store double %83, ptr %85, align 8, !dbg !1396
  br label %93, !dbg !1396

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1396
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1396
  store ptr %88, ptr %9, align 4, !dbg !1396
  %89 = load ptr, ptr %87, align 4, !dbg !1396
  %90 = load i32, ptr %13, align 4, !dbg !1396
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1396
  store ptr %89, ptr %91, align 8, !dbg !1396
  br label %93, !dbg !1396

92:                                               ; preds = %27
  br label %93, !dbg !1396

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1393

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1398
  %96 = add nsw i32 %95, 1, !dbg !1398
  store i32 %96, ptr %13, align 4, !dbg !1398
  br label %23, !dbg !1398, !llvm.loop !1399

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1400, metadata !DIExpression()), !dbg !1382
  %98 = load ptr, ptr %8, align 4, !dbg !1382
  %99 = load ptr, ptr %98, align 4, !dbg !1382
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 72, !dbg !1382
  %101 = load ptr, ptr %100, align 4, !dbg !1382
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1382
  %103 = load ptr, ptr %5, align 4, !dbg !1382
  %104 = load ptr, ptr %6, align 4, !dbg !1382
  %105 = load ptr, ptr %7, align 4, !dbg !1382
  %106 = load ptr, ptr %8, align 4, !dbg !1382
  %107 = call x86_stdcallcc signext i8 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1382
  store i8 %107, ptr %14, align 1, !dbg !1382
  call void @llvm.va_end(ptr %9), !dbg !1382
  %108 = load i8, ptr %14, align 1, !dbg !1382
  ret i8 %108, !dbg !1382
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i8 @"\01_JNI_CallNonvirtualByteMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1401 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1402, metadata !DIExpression()), !dbg !1403
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1404, metadata !DIExpression()), !dbg !1403
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1405, metadata !DIExpression()), !dbg !1403
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1406, metadata !DIExpression()), !dbg !1403
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1407, metadata !DIExpression()), !dbg !1403
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1408, metadata !DIExpression()), !dbg !1403
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1409, metadata !DIExpression()), !dbg !1403
  %15 = load ptr, ptr %10, align 4, !dbg !1403
  %16 = load ptr, ptr %15, align 4, !dbg !1403
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1403
  %18 = load ptr, ptr %17, align 4, !dbg !1403
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1403
  %20 = load ptr, ptr %7, align 4, !dbg !1403
  %21 = load ptr, ptr %10, align 4, !dbg !1403
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1403
  store i32 %22, ptr %12, align 4, !dbg !1403
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1410, metadata !DIExpression()), !dbg !1403
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1411, metadata !DIExpression()), !dbg !1413
  store i32 0, ptr %14, align 4, !dbg !1413
  br label %23, !dbg !1413

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1413
  %25 = load i32, ptr %12, align 4, !dbg !1413
  %26 = icmp slt i32 %24, %25, !dbg !1413
  br i1 %26, label %27, label %97, !dbg !1413

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1414
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1414
  %30 = load i8, ptr %29, align 1, !dbg !1414
  %31 = sext i8 %30 to i32, !dbg !1414
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1414

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1417
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1417
  store ptr %34, ptr %6, align 4, !dbg !1417
  %35 = load i32, ptr %33, align 4, !dbg !1417
  %36 = trunc i32 %35 to i8, !dbg !1417
  %37 = load i32, ptr %14, align 4, !dbg !1417
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1417
  store i8 %36, ptr %38, align 8, !dbg !1417
  br label %93, !dbg !1417

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1417
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1417
  store ptr %41, ptr %6, align 4, !dbg !1417
  %42 = load i32, ptr %40, align 4, !dbg !1417
  %43 = trunc i32 %42 to i8, !dbg !1417
  %44 = load i32, ptr %14, align 4, !dbg !1417
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1417
  store i8 %43, ptr %45, align 8, !dbg !1417
  br label %93, !dbg !1417

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1417
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1417
  store ptr %48, ptr %6, align 4, !dbg !1417
  %49 = load i32, ptr %47, align 4, !dbg !1417
  %50 = trunc i32 %49 to i16, !dbg !1417
  %51 = load i32, ptr %14, align 4, !dbg !1417
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1417
  store i16 %50, ptr %52, align 8, !dbg !1417
  br label %93, !dbg !1417

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1417
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1417
  store ptr %55, ptr %6, align 4, !dbg !1417
  %56 = load i32, ptr %54, align 4, !dbg !1417
  %57 = trunc i32 %56 to i16, !dbg !1417
  %58 = load i32, ptr %14, align 4, !dbg !1417
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1417
  store i16 %57, ptr %59, align 8, !dbg !1417
  br label %93, !dbg !1417

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1417
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1417
  store ptr %62, ptr %6, align 4, !dbg !1417
  %63 = load i32, ptr %61, align 4, !dbg !1417
  %64 = load i32, ptr %14, align 4, !dbg !1417
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1417
  store i32 %63, ptr %65, align 8, !dbg !1417
  br label %93, !dbg !1417

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1417
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1417
  store ptr %68, ptr %6, align 4, !dbg !1417
  %69 = load i32, ptr %67, align 4, !dbg !1417
  %70 = sext i32 %69 to i64, !dbg !1417
  %71 = load i32, ptr %14, align 4, !dbg !1417
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1417
  store i64 %70, ptr %72, align 8, !dbg !1417
  br label %93, !dbg !1417

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1417
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1417
  store ptr %75, ptr %6, align 4, !dbg !1417
  %76 = load double, ptr %74, align 4, !dbg !1417
  %77 = fptrunc double %76 to float, !dbg !1417
  %78 = load i32, ptr %14, align 4, !dbg !1417
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1417
  store float %77, ptr %79, align 8, !dbg !1417
  br label %93, !dbg !1417

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1417
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1417
  store ptr %82, ptr %6, align 4, !dbg !1417
  %83 = load double, ptr %81, align 4, !dbg !1417
  %84 = load i32, ptr %14, align 4, !dbg !1417
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1417
  store double %83, ptr %85, align 8, !dbg !1417
  br label %93, !dbg !1417

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1417
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1417
  store ptr %88, ptr %6, align 4, !dbg !1417
  %89 = load ptr, ptr %87, align 4, !dbg !1417
  %90 = load i32, ptr %14, align 4, !dbg !1417
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1417
  store ptr %89, ptr %91, align 8, !dbg !1417
  br label %93, !dbg !1417

92:                                               ; preds = %27
  br label %93, !dbg !1417

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1414

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1419
  %96 = add nsw i32 %95, 1, !dbg !1419
  store i32 %96, ptr %14, align 4, !dbg !1419
  br label %23, !dbg !1419, !llvm.loop !1420

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1403
  %99 = load ptr, ptr %98, align 4, !dbg !1403
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 72, !dbg !1403
  %101 = load ptr, ptr %100, align 4, !dbg !1403
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1403
  %103 = load ptr, ptr %7, align 4, !dbg !1403
  %104 = load ptr, ptr %8, align 4, !dbg !1403
  %105 = load ptr, ptr %9, align 4, !dbg !1403
  %106 = load ptr, ptr %10, align 4, !dbg !1403
  %107 = call x86_stdcallcc signext i8 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1403
  ret i8 %107, !dbg !1403
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1421 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1422, metadata !DIExpression()), !dbg !1423
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1424, metadata !DIExpression()), !dbg !1423
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1425, metadata !DIExpression()), !dbg !1423
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1426, metadata !DIExpression()), !dbg !1423
  call void @llvm.va_start(ptr %7), !dbg !1423
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1427, metadata !DIExpression()), !dbg !1423
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1428, metadata !DIExpression()), !dbg !1423
  %13 = load ptr, ptr %6, align 4, !dbg !1423
  %14 = load ptr, ptr %13, align 4, !dbg !1423
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1423
  %16 = load ptr, ptr %15, align 4, !dbg !1423
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1423
  %18 = load ptr, ptr %4, align 4, !dbg !1423
  %19 = load ptr, ptr %6, align 4, !dbg !1423
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1423
  store i32 %20, ptr %9, align 4, !dbg !1423
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1429, metadata !DIExpression()), !dbg !1423
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1430, metadata !DIExpression()), !dbg !1432
  store i32 0, ptr %11, align 4, !dbg !1432
  br label %21, !dbg !1432

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1432
  %23 = load i32, ptr %9, align 4, !dbg !1432
  %24 = icmp slt i32 %22, %23, !dbg !1432
  br i1 %24, label %25, label %95, !dbg !1432

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1433
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1433
  %28 = load i8, ptr %27, align 1, !dbg !1433
  %29 = sext i8 %28 to i32, !dbg !1433
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1433

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1436
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1436
  store ptr %32, ptr %7, align 4, !dbg !1436
  %33 = load i32, ptr %31, align 4, !dbg !1436
  %34 = trunc i32 %33 to i8, !dbg !1436
  %35 = load i32, ptr %11, align 4, !dbg !1436
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1436
  store i8 %34, ptr %36, align 8, !dbg !1436
  br label %91, !dbg !1436

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1436
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1436
  store ptr %39, ptr %7, align 4, !dbg !1436
  %40 = load i32, ptr %38, align 4, !dbg !1436
  %41 = trunc i32 %40 to i8, !dbg !1436
  %42 = load i32, ptr %11, align 4, !dbg !1436
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1436
  store i8 %41, ptr %43, align 8, !dbg !1436
  br label %91, !dbg !1436

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1436
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1436
  store ptr %46, ptr %7, align 4, !dbg !1436
  %47 = load i32, ptr %45, align 4, !dbg !1436
  %48 = trunc i32 %47 to i16, !dbg !1436
  %49 = load i32, ptr %11, align 4, !dbg !1436
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1436
  store i16 %48, ptr %50, align 8, !dbg !1436
  br label %91, !dbg !1436

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1436
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1436
  store ptr %53, ptr %7, align 4, !dbg !1436
  %54 = load i32, ptr %52, align 4, !dbg !1436
  %55 = trunc i32 %54 to i16, !dbg !1436
  %56 = load i32, ptr %11, align 4, !dbg !1436
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1436
  store i16 %55, ptr %57, align 8, !dbg !1436
  br label %91, !dbg !1436

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1436
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1436
  store ptr %60, ptr %7, align 4, !dbg !1436
  %61 = load i32, ptr %59, align 4, !dbg !1436
  %62 = load i32, ptr %11, align 4, !dbg !1436
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1436
  store i32 %61, ptr %63, align 8, !dbg !1436
  br label %91, !dbg !1436

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1436
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1436
  store ptr %66, ptr %7, align 4, !dbg !1436
  %67 = load i32, ptr %65, align 4, !dbg !1436
  %68 = sext i32 %67 to i64, !dbg !1436
  %69 = load i32, ptr %11, align 4, !dbg !1436
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1436
  store i64 %68, ptr %70, align 8, !dbg !1436
  br label %91, !dbg !1436

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1436
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1436
  store ptr %73, ptr %7, align 4, !dbg !1436
  %74 = load double, ptr %72, align 4, !dbg !1436
  %75 = fptrunc double %74 to float, !dbg !1436
  %76 = load i32, ptr %11, align 4, !dbg !1436
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1436
  store float %75, ptr %77, align 8, !dbg !1436
  br label %91, !dbg !1436

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1436
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1436
  store ptr %80, ptr %7, align 4, !dbg !1436
  %81 = load double, ptr %79, align 4, !dbg !1436
  %82 = load i32, ptr %11, align 4, !dbg !1436
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1436
  store double %81, ptr %83, align 8, !dbg !1436
  br label %91, !dbg !1436

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1436
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1436
  store ptr %86, ptr %7, align 4, !dbg !1436
  %87 = load ptr, ptr %85, align 4, !dbg !1436
  %88 = load i32, ptr %11, align 4, !dbg !1436
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1436
  store ptr %87, ptr %89, align 8, !dbg !1436
  br label %91, !dbg !1436

90:                                               ; preds = %25
  br label %91, !dbg !1436

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1433

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1438
  %94 = add nsw i32 %93, 1, !dbg !1438
  store i32 %94, ptr %11, align 4, !dbg !1438
  br label %21, !dbg !1438, !llvm.loop !1439

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1440, metadata !DIExpression()), !dbg !1423
  %96 = load ptr, ptr %6, align 4, !dbg !1423
  %97 = load ptr, ptr %96, align 4, !dbg !1423
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 122, !dbg !1423
  %99 = load ptr, ptr %98, align 4, !dbg !1423
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1423
  %101 = load ptr, ptr %4, align 4, !dbg !1423
  %102 = load ptr, ptr %5, align 4, !dbg !1423
  %103 = load ptr, ptr %6, align 4, !dbg !1423
  %104 = call x86_stdcallcc signext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1423
  store i8 %104, ptr %12, align 1, !dbg !1423
  call void @llvm.va_end(ptr %7), !dbg !1423
  %105 = load i8, ptr %12, align 1, !dbg !1423
  ret i8 %105, !dbg !1423
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i8 @"\01_JNI_CallStaticByteMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1441 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1442, metadata !DIExpression()), !dbg !1443
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1444, metadata !DIExpression()), !dbg !1443
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1445, metadata !DIExpression()), !dbg !1443
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1446, metadata !DIExpression()), !dbg !1443
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1447, metadata !DIExpression()), !dbg !1443
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1448, metadata !DIExpression()), !dbg !1443
  %13 = load ptr, ptr %8, align 4, !dbg !1443
  %14 = load ptr, ptr %13, align 4, !dbg !1443
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1443
  %16 = load ptr, ptr %15, align 4, !dbg !1443
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1443
  %18 = load ptr, ptr %6, align 4, !dbg !1443
  %19 = load ptr, ptr %8, align 4, !dbg !1443
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1443
  store i32 %20, ptr %10, align 4, !dbg !1443
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1449, metadata !DIExpression()), !dbg !1443
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1450, metadata !DIExpression()), !dbg !1452
  store i32 0, ptr %12, align 4, !dbg !1452
  br label %21, !dbg !1452

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1452
  %23 = load i32, ptr %10, align 4, !dbg !1452
  %24 = icmp slt i32 %22, %23, !dbg !1452
  br i1 %24, label %25, label %95, !dbg !1452

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1453
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1453
  %28 = load i8, ptr %27, align 1, !dbg !1453
  %29 = sext i8 %28 to i32, !dbg !1453
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1453

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1456
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1456
  store ptr %32, ptr %5, align 4, !dbg !1456
  %33 = load i32, ptr %31, align 4, !dbg !1456
  %34 = trunc i32 %33 to i8, !dbg !1456
  %35 = load i32, ptr %12, align 4, !dbg !1456
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1456
  store i8 %34, ptr %36, align 8, !dbg !1456
  br label %91, !dbg !1456

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1456
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1456
  store ptr %39, ptr %5, align 4, !dbg !1456
  %40 = load i32, ptr %38, align 4, !dbg !1456
  %41 = trunc i32 %40 to i8, !dbg !1456
  %42 = load i32, ptr %12, align 4, !dbg !1456
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1456
  store i8 %41, ptr %43, align 8, !dbg !1456
  br label %91, !dbg !1456

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1456
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1456
  store ptr %46, ptr %5, align 4, !dbg !1456
  %47 = load i32, ptr %45, align 4, !dbg !1456
  %48 = trunc i32 %47 to i16, !dbg !1456
  %49 = load i32, ptr %12, align 4, !dbg !1456
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1456
  store i16 %48, ptr %50, align 8, !dbg !1456
  br label %91, !dbg !1456

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1456
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1456
  store ptr %53, ptr %5, align 4, !dbg !1456
  %54 = load i32, ptr %52, align 4, !dbg !1456
  %55 = trunc i32 %54 to i16, !dbg !1456
  %56 = load i32, ptr %12, align 4, !dbg !1456
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1456
  store i16 %55, ptr %57, align 8, !dbg !1456
  br label %91, !dbg !1456

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1456
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1456
  store ptr %60, ptr %5, align 4, !dbg !1456
  %61 = load i32, ptr %59, align 4, !dbg !1456
  %62 = load i32, ptr %12, align 4, !dbg !1456
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1456
  store i32 %61, ptr %63, align 8, !dbg !1456
  br label %91, !dbg !1456

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1456
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1456
  store ptr %66, ptr %5, align 4, !dbg !1456
  %67 = load i32, ptr %65, align 4, !dbg !1456
  %68 = sext i32 %67 to i64, !dbg !1456
  %69 = load i32, ptr %12, align 4, !dbg !1456
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1456
  store i64 %68, ptr %70, align 8, !dbg !1456
  br label %91, !dbg !1456

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1456
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1456
  store ptr %73, ptr %5, align 4, !dbg !1456
  %74 = load double, ptr %72, align 4, !dbg !1456
  %75 = fptrunc double %74 to float, !dbg !1456
  %76 = load i32, ptr %12, align 4, !dbg !1456
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1456
  store float %75, ptr %77, align 8, !dbg !1456
  br label %91, !dbg !1456

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1456
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1456
  store ptr %80, ptr %5, align 4, !dbg !1456
  %81 = load double, ptr %79, align 4, !dbg !1456
  %82 = load i32, ptr %12, align 4, !dbg !1456
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1456
  store double %81, ptr %83, align 8, !dbg !1456
  br label %91, !dbg !1456

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1456
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1456
  store ptr %86, ptr %5, align 4, !dbg !1456
  %87 = load ptr, ptr %85, align 4, !dbg !1456
  %88 = load i32, ptr %12, align 4, !dbg !1456
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1456
  store ptr %87, ptr %89, align 8, !dbg !1456
  br label %91, !dbg !1456

90:                                               ; preds = %25
  br label %91, !dbg !1456

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1453

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1458
  %94 = add nsw i32 %93, 1, !dbg !1458
  store i32 %94, ptr %12, align 4, !dbg !1458
  br label %21, !dbg !1458, !llvm.loop !1459

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1443
  %97 = load ptr, ptr %96, align 4, !dbg !1443
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 122, !dbg !1443
  %99 = load ptr, ptr %98, align 4, !dbg !1443
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1443
  %101 = load ptr, ptr %6, align 4, !dbg !1443
  %102 = load ptr, ptr %7, align 4, !dbg !1443
  %103 = load ptr, ptr %8, align 4, !dbg !1443
  %104 = call x86_stdcallcc signext i8 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1443
  ret i8 %104, !dbg !1443
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1460 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1461, metadata !DIExpression()), !dbg !1462
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1463, metadata !DIExpression()), !dbg !1462
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1464, metadata !DIExpression()), !dbg !1462
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1465, metadata !DIExpression()), !dbg !1462
  call void @llvm.va_start(ptr %7), !dbg !1462
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1466, metadata !DIExpression()), !dbg !1462
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1467, metadata !DIExpression()), !dbg !1462
  %13 = load ptr, ptr %6, align 4, !dbg !1462
  %14 = load ptr, ptr %13, align 4, !dbg !1462
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1462
  %16 = load ptr, ptr %15, align 4, !dbg !1462
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1462
  %18 = load ptr, ptr %4, align 4, !dbg !1462
  %19 = load ptr, ptr %6, align 4, !dbg !1462
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1462
  store i32 %20, ptr %9, align 4, !dbg !1462
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1468, metadata !DIExpression()), !dbg !1462
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1469, metadata !DIExpression()), !dbg !1471
  store i32 0, ptr %11, align 4, !dbg !1471
  br label %21, !dbg !1471

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1471
  %23 = load i32, ptr %9, align 4, !dbg !1471
  %24 = icmp slt i32 %22, %23, !dbg !1471
  br i1 %24, label %25, label %95, !dbg !1471

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1472
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1472
  %28 = load i8, ptr %27, align 1, !dbg !1472
  %29 = sext i8 %28 to i32, !dbg !1472
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1472

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1475
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1475
  store ptr %32, ptr %7, align 4, !dbg !1475
  %33 = load i32, ptr %31, align 4, !dbg !1475
  %34 = trunc i32 %33 to i8, !dbg !1475
  %35 = load i32, ptr %11, align 4, !dbg !1475
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1475
  store i8 %34, ptr %36, align 8, !dbg !1475
  br label %91, !dbg !1475

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1475
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1475
  store ptr %39, ptr %7, align 4, !dbg !1475
  %40 = load i32, ptr %38, align 4, !dbg !1475
  %41 = trunc i32 %40 to i8, !dbg !1475
  %42 = load i32, ptr %11, align 4, !dbg !1475
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1475
  store i8 %41, ptr %43, align 8, !dbg !1475
  br label %91, !dbg !1475

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1475
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1475
  store ptr %46, ptr %7, align 4, !dbg !1475
  %47 = load i32, ptr %45, align 4, !dbg !1475
  %48 = trunc i32 %47 to i16, !dbg !1475
  %49 = load i32, ptr %11, align 4, !dbg !1475
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1475
  store i16 %48, ptr %50, align 8, !dbg !1475
  br label %91, !dbg !1475

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1475
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1475
  store ptr %53, ptr %7, align 4, !dbg !1475
  %54 = load i32, ptr %52, align 4, !dbg !1475
  %55 = trunc i32 %54 to i16, !dbg !1475
  %56 = load i32, ptr %11, align 4, !dbg !1475
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1475
  store i16 %55, ptr %57, align 8, !dbg !1475
  br label %91, !dbg !1475

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1475
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1475
  store ptr %60, ptr %7, align 4, !dbg !1475
  %61 = load i32, ptr %59, align 4, !dbg !1475
  %62 = load i32, ptr %11, align 4, !dbg !1475
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1475
  store i32 %61, ptr %63, align 8, !dbg !1475
  br label %91, !dbg !1475

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1475
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1475
  store ptr %66, ptr %7, align 4, !dbg !1475
  %67 = load i32, ptr %65, align 4, !dbg !1475
  %68 = sext i32 %67 to i64, !dbg !1475
  %69 = load i32, ptr %11, align 4, !dbg !1475
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1475
  store i64 %68, ptr %70, align 8, !dbg !1475
  br label %91, !dbg !1475

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1475
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1475
  store ptr %73, ptr %7, align 4, !dbg !1475
  %74 = load double, ptr %72, align 4, !dbg !1475
  %75 = fptrunc double %74 to float, !dbg !1475
  %76 = load i32, ptr %11, align 4, !dbg !1475
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1475
  store float %75, ptr %77, align 8, !dbg !1475
  br label %91, !dbg !1475

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1475
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1475
  store ptr %80, ptr %7, align 4, !dbg !1475
  %81 = load double, ptr %79, align 4, !dbg !1475
  %82 = load i32, ptr %11, align 4, !dbg !1475
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1475
  store double %81, ptr %83, align 8, !dbg !1475
  br label %91, !dbg !1475

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1475
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1475
  store ptr %86, ptr %7, align 4, !dbg !1475
  %87 = load ptr, ptr %85, align 4, !dbg !1475
  %88 = load i32, ptr %11, align 4, !dbg !1475
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1475
  store ptr %87, ptr %89, align 8, !dbg !1475
  br label %91, !dbg !1475

90:                                               ; preds = %25
  br label %91, !dbg !1475

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1472

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1477
  %94 = add nsw i32 %93, 1, !dbg !1477
  store i32 %94, ptr %11, align 4, !dbg !1477
  br label %21, !dbg !1477, !llvm.loop !1478

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1479, metadata !DIExpression()), !dbg !1462
  %96 = load ptr, ptr %6, align 4, !dbg !1462
  %97 = load ptr, ptr %96, align 4, !dbg !1462
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 45, !dbg !1462
  %99 = load ptr, ptr %98, align 4, !dbg !1462
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1462
  %101 = load ptr, ptr %4, align 4, !dbg !1462
  %102 = load ptr, ptr %5, align 4, !dbg !1462
  %103 = load ptr, ptr %6, align 4, !dbg !1462
  %104 = call x86_stdcallcc zeroext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1462
  store i16 %104, ptr %12, align 2, !dbg !1462
  call void @llvm.va_end(ptr %7), !dbg !1462
  %105 = load i16, ptr %12, align 2, !dbg !1462
  ret i16 %105, !dbg !1462
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i16 @"\01_JNI_CallCharMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1480 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1481, metadata !DIExpression()), !dbg !1482
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1483, metadata !DIExpression()), !dbg !1482
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1484, metadata !DIExpression()), !dbg !1482
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1485, metadata !DIExpression()), !dbg !1482
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1486, metadata !DIExpression()), !dbg !1482
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1487, metadata !DIExpression()), !dbg !1482
  %13 = load ptr, ptr %8, align 4, !dbg !1482
  %14 = load ptr, ptr %13, align 4, !dbg !1482
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1482
  %16 = load ptr, ptr %15, align 4, !dbg !1482
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1482
  %18 = load ptr, ptr %6, align 4, !dbg !1482
  %19 = load ptr, ptr %8, align 4, !dbg !1482
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1482
  store i32 %20, ptr %10, align 4, !dbg !1482
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1488, metadata !DIExpression()), !dbg !1482
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1489, metadata !DIExpression()), !dbg !1491
  store i32 0, ptr %12, align 4, !dbg !1491
  br label %21, !dbg !1491

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1491
  %23 = load i32, ptr %10, align 4, !dbg !1491
  %24 = icmp slt i32 %22, %23, !dbg !1491
  br i1 %24, label %25, label %95, !dbg !1491

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1492
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1492
  %28 = load i8, ptr %27, align 1, !dbg !1492
  %29 = sext i8 %28 to i32, !dbg !1492
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1492

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1495
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1495
  store ptr %32, ptr %5, align 4, !dbg !1495
  %33 = load i32, ptr %31, align 4, !dbg !1495
  %34 = trunc i32 %33 to i8, !dbg !1495
  %35 = load i32, ptr %12, align 4, !dbg !1495
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1495
  store i8 %34, ptr %36, align 8, !dbg !1495
  br label %91, !dbg !1495

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1495
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1495
  store ptr %39, ptr %5, align 4, !dbg !1495
  %40 = load i32, ptr %38, align 4, !dbg !1495
  %41 = trunc i32 %40 to i8, !dbg !1495
  %42 = load i32, ptr %12, align 4, !dbg !1495
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1495
  store i8 %41, ptr %43, align 8, !dbg !1495
  br label %91, !dbg !1495

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1495
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1495
  store ptr %46, ptr %5, align 4, !dbg !1495
  %47 = load i32, ptr %45, align 4, !dbg !1495
  %48 = trunc i32 %47 to i16, !dbg !1495
  %49 = load i32, ptr %12, align 4, !dbg !1495
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1495
  store i16 %48, ptr %50, align 8, !dbg !1495
  br label %91, !dbg !1495

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1495
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1495
  store ptr %53, ptr %5, align 4, !dbg !1495
  %54 = load i32, ptr %52, align 4, !dbg !1495
  %55 = trunc i32 %54 to i16, !dbg !1495
  %56 = load i32, ptr %12, align 4, !dbg !1495
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1495
  store i16 %55, ptr %57, align 8, !dbg !1495
  br label %91, !dbg !1495

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1495
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1495
  store ptr %60, ptr %5, align 4, !dbg !1495
  %61 = load i32, ptr %59, align 4, !dbg !1495
  %62 = load i32, ptr %12, align 4, !dbg !1495
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1495
  store i32 %61, ptr %63, align 8, !dbg !1495
  br label %91, !dbg !1495

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1495
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1495
  store ptr %66, ptr %5, align 4, !dbg !1495
  %67 = load i32, ptr %65, align 4, !dbg !1495
  %68 = sext i32 %67 to i64, !dbg !1495
  %69 = load i32, ptr %12, align 4, !dbg !1495
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1495
  store i64 %68, ptr %70, align 8, !dbg !1495
  br label %91, !dbg !1495

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1495
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1495
  store ptr %73, ptr %5, align 4, !dbg !1495
  %74 = load double, ptr %72, align 4, !dbg !1495
  %75 = fptrunc double %74 to float, !dbg !1495
  %76 = load i32, ptr %12, align 4, !dbg !1495
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1495
  store float %75, ptr %77, align 8, !dbg !1495
  br label %91, !dbg !1495

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1495
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1495
  store ptr %80, ptr %5, align 4, !dbg !1495
  %81 = load double, ptr %79, align 4, !dbg !1495
  %82 = load i32, ptr %12, align 4, !dbg !1495
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1495
  store double %81, ptr %83, align 8, !dbg !1495
  br label %91, !dbg !1495

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1495
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1495
  store ptr %86, ptr %5, align 4, !dbg !1495
  %87 = load ptr, ptr %85, align 4, !dbg !1495
  %88 = load i32, ptr %12, align 4, !dbg !1495
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1495
  store ptr %87, ptr %89, align 8, !dbg !1495
  br label %91, !dbg !1495

90:                                               ; preds = %25
  br label %91, !dbg !1495

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1492

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1497
  %94 = add nsw i32 %93, 1, !dbg !1497
  store i32 %94, ptr %12, align 4, !dbg !1497
  br label %21, !dbg !1497, !llvm.loop !1498

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1482
  %97 = load ptr, ptr %96, align 4, !dbg !1482
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 45, !dbg !1482
  %99 = load ptr, ptr %98, align 4, !dbg !1482
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1482
  %101 = load ptr, ptr %6, align 4, !dbg !1482
  %102 = load ptr, ptr %7, align 4, !dbg !1482
  %103 = load ptr, ptr %8, align 4, !dbg !1482
  %104 = call x86_stdcallcc zeroext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1482
  ret i16 %104, !dbg !1482
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1499 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1500, metadata !DIExpression()), !dbg !1501
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1502, metadata !DIExpression()), !dbg !1501
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1503, metadata !DIExpression()), !dbg !1501
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1504, metadata !DIExpression()), !dbg !1501
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1505, metadata !DIExpression()), !dbg !1501
  call void @llvm.va_start(ptr %9), !dbg !1501
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1506, metadata !DIExpression()), !dbg !1501
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1507, metadata !DIExpression()), !dbg !1501
  %15 = load ptr, ptr %8, align 4, !dbg !1501
  %16 = load ptr, ptr %15, align 4, !dbg !1501
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1501
  %18 = load ptr, ptr %17, align 4, !dbg !1501
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1501
  %20 = load ptr, ptr %5, align 4, !dbg !1501
  %21 = load ptr, ptr %8, align 4, !dbg !1501
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1501
  store i32 %22, ptr %11, align 4, !dbg !1501
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1508, metadata !DIExpression()), !dbg !1501
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1509, metadata !DIExpression()), !dbg !1511
  store i32 0, ptr %13, align 4, !dbg !1511
  br label %23, !dbg !1511

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1511
  %25 = load i32, ptr %11, align 4, !dbg !1511
  %26 = icmp slt i32 %24, %25, !dbg !1511
  br i1 %26, label %27, label %97, !dbg !1511

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1512
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1512
  %30 = load i8, ptr %29, align 1, !dbg !1512
  %31 = sext i8 %30 to i32, !dbg !1512
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1512

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1515
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1515
  store ptr %34, ptr %9, align 4, !dbg !1515
  %35 = load i32, ptr %33, align 4, !dbg !1515
  %36 = trunc i32 %35 to i8, !dbg !1515
  %37 = load i32, ptr %13, align 4, !dbg !1515
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1515
  store i8 %36, ptr %38, align 8, !dbg !1515
  br label %93, !dbg !1515

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1515
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1515
  store ptr %41, ptr %9, align 4, !dbg !1515
  %42 = load i32, ptr %40, align 4, !dbg !1515
  %43 = trunc i32 %42 to i8, !dbg !1515
  %44 = load i32, ptr %13, align 4, !dbg !1515
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1515
  store i8 %43, ptr %45, align 8, !dbg !1515
  br label %93, !dbg !1515

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1515
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1515
  store ptr %48, ptr %9, align 4, !dbg !1515
  %49 = load i32, ptr %47, align 4, !dbg !1515
  %50 = trunc i32 %49 to i16, !dbg !1515
  %51 = load i32, ptr %13, align 4, !dbg !1515
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1515
  store i16 %50, ptr %52, align 8, !dbg !1515
  br label %93, !dbg !1515

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1515
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1515
  store ptr %55, ptr %9, align 4, !dbg !1515
  %56 = load i32, ptr %54, align 4, !dbg !1515
  %57 = trunc i32 %56 to i16, !dbg !1515
  %58 = load i32, ptr %13, align 4, !dbg !1515
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1515
  store i16 %57, ptr %59, align 8, !dbg !1515
  br label %93, !dbg !1515

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1515
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1515
  store ptr %62, ptr %9, align 4, !dbg !1515
  %63 = load i32, ptr %61, align 4, !dbg !1515
  %64 = load i32, ptr %13, align 4, !dbg !1515
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1515
  store i32 %63, ptr %65, align 8, !dbg !1515
  br label %93, !dbg !1515

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1515
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1515
  store ptr %68, ptr %9, align 4, !dbg !1515
  %69 = load i32, ptr %67, align 4, !dbg !1515
  %70 = sext i32 %69 to i64, !dbg !1515
  %71 = load i32, ptr %13, align 4, !dbg !1515
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1515
  store i64 %70, ptr %72, align 8, !dbg !1515
  br label %93, !dbg !1515

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1515
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1515
  store ptr %75, ptr %9, align 4, !dbg !1515
  %76 = load double, ptr %74, align 4, !dbg !1515
  %77 = fptrunc double %76 to float, !dbg !1515
  %78 = load i32, ptr %13, align 4, !dbg !1515
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1515
  store float %77, ptr %79, align 8, !dbg !1515
  br label %93, !dbg !1515

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1515
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1515
  store ptr %82, ptr %9, align 4, !dbg !1515
  %83 = load double, ptr %81, align 4, !dbg !1515
  %84 = load i32, ptr %13, align 4, !dbg !1515
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1515
  store double %83, ptr %85, align 8, !dbg !1515
  br label %93, !dbg !1515

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1515
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1515
  store ptr %88, ptr %9, align 4, !dbg !1515
  %89 = load ptr, ptr %87, align 4, !dbg !1515
  %90 = load i32, ptr %13, align 4, !dbg !1515
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1515
  store ptr %89, ptr %91, align 8, !dbg !1515
  br label %93, !dbg !1515

92:                                               ; preds = %27
  br label %93, !dbg !1515

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1512

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1517
  %96 = add nsw i32 %95, 1, !dbg !1517
  store i32 %96, ptr %13, align 4, !dbg !1517
  br label %23, !dbg !1517, !llvm.loop !1518

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1519, metadata !DIExpression()), !dbg !1501
  %98 = load ptr, ptr %8, align 4, !dbg !1501
  %99 = load ptr, ptr %98, align 4, !dbg !1501
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 75, !dbg !1501
  %101 = load ptr, ptr %100, align 4, !dbg !1501
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1501
  %103 = load ptr, ptr %5, align 4, !dbg !1501
  %104 = load ptr, ptr %6, align 4, !dbg !1501
  %105 = load ptr, ptr %7, align 4, !dbg !1501
  %106 = load ptr, ptr %8, align 4, !dbg !1501
  %107 = call x86_stdcallcc zeroext i16 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1501
  store i16 %107, ptr %14, align 2, !dbg !1501
  call void @llvm.va_end(ptr %9), !dbg !1501
  %108 = load i16, ptr %14, align 2, !dbg !1501
  ret i16 %108, !dbg !1501
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i16 @"\01_JNI_CallNonvirtualCharMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1520 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1521, metadata !DIExpression()), !dbg !1522
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1523, metadata !DIExpression()), !dbg !1522
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1524, metadata !DIExpression()), !dbg !1522
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1525, metadata !DIExpression()), !dbg !1522
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1526, metadata !DIExpression()), !dbg !1522
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1527, metadata !DIExpression()), !dbg !1522
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1528, metadata !DIExpression()), !dbg !1522
  %15 = load ptr, ptr %10, align 4, !dbg !1522
  %16 = load ptr, ptr %15, align 4, !dbg !1522
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1522
  %18 = load ptr, ptr %17, align 4, !dbg !1522
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1522
  %20 = load ptr, ptr %7, align 4, !dbg !1522
  %21 = load ptr, ptr %10, align 4, !dbg !1522
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1522
  store i32 %22, ptr %12, align 4, !dbg !1522
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1529, metadata !DIExpression()), !dbg !1522
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1530, metadata !DIExpression()), !dbg !1532
  store i32 0, ptr %14, align 4, !dbg !1532
  br label %23, !dbg !1532

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1532
  %25 = load i32, ptr %12, align 4, !dbg !1532
  %26 = icmp slt i32 %24, %25, !dbg !1532
  br i1 %26, label %27, label %97, !dbg !1532

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1533
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1533
  %30 = load i8, ptr %29, align 1, !dbg !1533
  %31 = sext i8 %30 to i32, !dbg !1533
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1533

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1536
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1536
  store ptr %34, ptr %6, align 4, !dbg !1536
  %35 = load i32, ptr %33, align 4, !dbg !1536
  %36 = trunc i32 %35 to i8, !dbg !1536
  %37 = load i32, ptr %14, align 4, !dbg !1536
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1536
  store i8 %36, ptr %38, align 8, !dbg !1536
  br label %93, !dbg !1536

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1536
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1536
  store ptr %41, ptr %6, align 4, !dbg !1536
  %42 = load i32, ptr %40, align 4, !dbg !1536
  %43 = trunc i32 %42 to i8, !dbg !1536
  %44 = load i32, ptr %14, align 4, !dbg !1536
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1536
  store i8 %43, ptr %45, align 8, !dbg !1536
  br label %93, !dbg !1536

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1536
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1536
  store ptr %48, ptr %6, align 4, !dbg !1536
  %49 = load i32, ptr %47, align 4, !dbg !1536
  %50 = trunc i32 %49 to i16, !dbg !1536
  %51 = load i32, ptr %14, align 4, !dbg !1536
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1536
  store i16 %50, ptr %52, align 8, !dbg !1536
  br label %93, !dbg !1536

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1536
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1536
  store ptr %55, ptr %6, align 4, !dbg !1536
  %56 = load i32, ptr %54, align 4, !dbg !1536
  %57 = trunc i32 %56 to i16, !dbg !1536
  %58 = load i32, ptr %14, align 4, !dbg !1536
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1536
  store i16 %57, ptr %59, align 8, !dbg !1536
  br label %93, !dbg !1536

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1536
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1536
  store ptr %62, ptr %6, align 4, !dbg !1536
  %63 = load i32, ptr %61, align 4, !dbg !1536
  %64 = load i32, ptr %14, align 4, !dbg !1536
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1536
  store i32 %63, ptr %65, align 8, !dbg !1536
  br label %93, !dbg !1536

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1536
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1536
  store ptr %68, ptr %6, align 4, !dbg !1536
  %69 = load i32, ptr %67, align 4, !dbg !1536
  %70 = sext i32 %69 to i64, !dbg !1536
  %71 = load i32, ptr %14, align 4, !dbg !1536
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1536
  store i64 %70, ptr %72, align 8, !dbg !1536
  br label %93, !dbg !1536

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1536
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1536
  store ptr %75, ptr %6, align 4, !dbg !1536
  %76 = load double, ptr %74, align 4, !dbg !1536
  %77 = fptrunc double %76 to float, !dbg !1536
  %78 = load i32, ptr %14, align 4, !dbg !1536
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1536
  store float %77, ptr %79, align 8, !dbg !1536
  br label %93, !dbg !1536

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1536
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1536
  store ptr %82, ptr %6, align 4, !dbg !1536
  %83 = load double, ptr %81, align 4, !dbg !1536
  %84 = load i32, ptr %14, align 4, !dbg !1536
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1536
  store double %83, ptr %85, align 8, !dbg !1536
  br label %93, !dbg !1536

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1536
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1536
  store ptr %88, ptr %6, align 4, !dbg !1536
  %89 = load ptr, ptr %87, align 4, !dbg !1536
  %90 = load i32, ptr %14, align 4, !dbg !1536
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1536
  store ptr %89, ptr %91, align 8, !dbg !1536
  br label %93, !dbg !1536

92:                                               ; preds = %27
  br label %93, !dbg !1536

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1533

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1538
  %96 = add nsw i32 %95, 1, !dbg !1538
  store i32 %96, ptr %14, align 4, !dbg !1538
  br label %23, !dbg !1538, !llvm.loop !1539

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1522
  %99 = load ptr, ptr %98, align 4, !dbg !1522
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 75, !dbg !1522
  %101 = load ptr, ptr %100, align 4, !dbg !1522
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1522
  %103 = load ptr, ptr %7, align 4, !dbg !1522
  %104 = load ptr, ptr %8, align 4, !dbg !1522
  %105 = load ptr, ptr %9, align 4, !dbg !1522
  %106 = load ptr, ptr %10, align 4, !dbg !1522
  %107 = call x86_stdcallcc zeroext i16 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1522
  ret i16 %107, !dbg !1522
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1540 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1541, metadata !DIExpression()), !dbg !1542
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1543, metadata !DIExpression()), !dbg !1542
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1544, metadata !DIExpression()), !dbg !1542
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1545, metadata !DIExpression()), !dbg !1542
  call void @llvm.va_start(ptr %7), !dbg !1542
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1546, metadata !DIExpression()), !dbg !1542
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1547, metadata !DIExpression()), !dbg !1542
  %13 = load ptr, ptr %6, align 4, !dbg !1542
  %14 = load ptr, ptr %13, align 4, !dbg !1542
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1542
  %16 = load ptr, ptr %15, align 4, !dbg !1542
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1542
  %18 = load ptr, ptr %4, align 4, !dbg !1542
  %19 = load ptr, ptr %6, align 4, !dbg !1542
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1542
  store i32 %20, ptr %9, align 4, !dbg !1542
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1548, metadata !DIExpression()), !dbg !1542
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1549, metadata !DIExpression()), !dbg !1551
  store i32 0, ptr %11, align 4, !dbg !1551
  br label %21, !dbg !1551

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1551
  %23 = load i32, ptr %9, align 4, !dbg !1551
  %24 = icmp slt i32 %22, %23, !dbg !1551
  br i1 %24, label %25, label %95, !dbg !1551

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1552
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1552
  %28 = load i8, ptr %27, align 1, !dbg !1552
  %29 = sext i8 %28 to i32, !dbg !1552
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1552

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1555
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1555
  store ptr %32, ptr %7, align 4, !dbg !1555
  %33 = load i32, ptr %31, align 4, !dbg !1555
  %34 = trunc i32 %33 to i8, !dbg !1555
  %35 = load i32, ptr %11, align 4, !dbg !1555
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1555
  store i8 %34, ptr %36, align 8, !dbg !1555
  br label %91, !dbg !1555

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1555
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1555
  store ptr %39, ptr %7, align 4, !dbg !1555
  %40 = load i32, ptr %38, align 4, !dbg !1555
  %41 = trunc i32 %40 to i8, !dbg !1555
  %42 = load i32, ptr %11, align 4, !dbg !1555
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1555
  store i8 %41, ptr %43, align 8, !dbg !1555
  br label %91, !dbg !1555

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1555
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1555
  store ptr %46, ptr %7, align 4, !dbg !1555
  %47 = load i32, ptr %45, align 4, !dbg !1555
  %48 = trunc i32 %47 to i16, !dbg !1555
  %49 = load i32, ptr %11, align 4, !dbg !1555
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1555
  store i16 %48, ptr %50, align 8, !dbg !1555
  br label %91, !dbg !1555

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1555
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1555
  store ptr %53, ptr %7, align 4, !dbg !1555
  %54 = load i32, ptr %52, align 4, !dbg !1555
  %55 = trunc i32 %54 to i16, !dbg !1555
  %56 = load i32, ptr %11, align 4, !dbg !1555
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1555
  store i16 %55, ptr %57, align 8, !dbg !1555
  br label %91, !dbg !1555

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1555
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1555
  store ptr %60, ptr %7, align 4, !dbg !1555
  %61 = load i32, ptr %59, align 4, !dbg !1555
  %62 = load i32, ptr %11, align 4, !dbg !1555
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1555
  store i32 %61, ptr %63, align 8, !dbg !1555
  br label %91, !dbg !1555

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1555
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1555
  store ptr %66, ptr %7, align 4, !dbg !1555
  %67 = load i32, ptr %65, align 4, !dbg !1555
  %68 = sext i32 %67 to i64, !dbg !1555
  %69 = load i32, ptr %11, align 4, !dbg !1555
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1555
  store i64 %68, ptr %70, align 8, !dbg !1555
  br label %91, !dbg !1555

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1555
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1555
  store ptr %73, ptr %7, align 4, !dbg !1555
  %74 = load double, ptr %72, align 4, !dbg !1555
  %75 = fptrunc double %74 to float, !dbg !1555
  %76 = load i32, ptr %11, align 4, !dbg !1555
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1555
  store float %75, ptr %77, align 8, !dbg !1555
  br label %91, !dbg !1555

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1555
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1555
  store ptr %80, ptr %7, align 4, !dbg !1555
  %81 = load double, ptr %79, align 4, !dbg !1555
  %82 = load i32, ptr %11, align 4, !dbg !1555
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1555
  store double %81, ptr %83, align 8, !dbg !1555
  br label %91, !dbg !1555

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1555
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1555
  store ptr %86, ptr %7, align 4, !dbg !1555
  %87 = load ptr, ptr %85, align 4, !dbg !1555
  %88 = load i32, ptr %11, align 4, !dbg !1555
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1555
  store ptr %87, ptr %89, align 8, !dbg !1555
  br label %91, !dbg !1555

90:                                               ; preds = %25
  br label %91, !dbg !1555

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1552

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1557
  %94 = add nsw i32 %93, 1, !dbg !1557
  store i32 %94, ptr %11, align 4, !dbg !1557
  br label %21, !dbg !1557, !llvm.loop !1558

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1559, metadata !DIExpression()), !dbg !1542
  %96 = load ptr, ptr %6, align 4, !dbg !1542
  %97 = load ptr, ptr %96, align 4, !dbg !1542
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 125, !dbg !1542
  %99 = load ptr, ptr %98, align 4, !dbg !1542
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1542
  %101 = load ptr, ptr %4, align 4, !dbg !1542
  %102 = load ptr, ptr %5, align 4, !dbg !1542
  %103 = load ptr, ptr %6, align 4, !dbg !1542
  %104 = call x86_stdcallcc zeroext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1542
  store i16 %104, ptr %12, align 2, !dbg !1542
  call void @llvm.va_end(ptr %7), !dbg !1542
  %105 = load i16, ptr %12, align 2, !dbg !1542
  ret i16 %105, !dbg !1542
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc zeroext i16 @"\01_JNI_CallStaticCharMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1560 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1561, metadata !DIExpression()), !dbg !1562
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1563, metadata !DIExpression()), !dbg !1562
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1564, metadata !DIExpression()), !dbg !1562
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1565, metadata !DIExpression()), !dbg !1562
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1566, metadata !DIExpression()), !dbg !1562
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1567, metadata !DIExpression()), !dbg !1562
  %13 = load ptr, ptr %8, align 4, !dbg !1562
  %14 = load ptr, ptr %13, align 4, !dbg !1562
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1562
  %16 = load ptr, ptr %15, align 4, !dbg !1562
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1562
  %18 = load ptr, ptr %6, align 4, !dbg !1562
  %19 = load ptr, ptr %8, align 4, !dbg !1562
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1562
  store i32 %20, ptr %10, align 4, !dbg !1562
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1568, metadata !DIExpression()), !dbg !1562
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1569, metadata !DIExpression()), !dbg !1571
  store i32 0, ptr %12, align 4, !dbg !1571
  br label %21, !dbg !1571

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1571
  %23 = load i32, ptr %10, align 4, !dbg !1571
  %24 = icmp slt i32 %22, %23, !dbg !1571
  br i1 %24, label %25, label %95, !dbg !1571

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1572
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1572
  %28 = load i8, ptr %27, align 1, !dbg !1572
  %29 = sext i8 %28 to i32, !dbg !1572
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1572

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1575
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1575
  store ptr %32, ptr %5, align 4, !dbg !1575
  %33 = load i32, ptr %31, align 4, !dbg !1575
  %34 = trunc i32 %33 to i8, !dbg !1575
  %35 = load i32, ptr %12, align 4, !dbg !1575
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1575
  store i8 %34, ptr %36, align 8, !dbg !1575
  br label %91, !dbg !1575

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1575
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1575
  store ptr %39, ptr %5, align 4, !dbg !1575
  %40 = load i32, ptr %38, align 4, !dbg !1575
  %41 = trunc i32 %40 to i8, !dbg !1575
  %42 = load i32, ptr %12, align 4, !dbg !1575
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1575
  store i8 %41, ptr %43, align 8, !dbg !1575
  br label %91, !dbg !1575

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1575
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1575
  store ptr %46, ptr %5, align 4, !dbg !1575
  %47 = load i32, ptr %45, align 4, !dbg !1575
  %48 = trunc i32 %47 to i16, !dbg !1575
  %49 = load i32, ptr %12, align 4, !dbg !1575
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1575
  store i16 %48, ptr %50, align 8, !dbg !1575
  br label %91, !dbg !1575

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1575
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1575
  store ptr %53, ptr %5, align 4, !dbg !1575
  %54 = load i32, ptr %52, align 4, !dbg !1575
  %55 = trunc i32 %54 to i16, !dbg !1575
  %56 = load i32, ptr %12, align 4, !dbg !1575
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1575
  store i16 %55, ptr %57, align 8, !dbg !1575
  br label %91, !dbg !1575

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1575
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1575
  store ptr %60, ptr %5, align 4, !dbg !1575
  %61 = load i32, ptr %59, align 4, !dbg !1575
  %62 = load i32, ptr %12, align 4, !dbg !1575
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1575
  store i32 %61, ptr %63, align 8, !dbg !1575
  br label %91, !dbg !1575

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1575
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1575
  store ptr %66, ptr %5, align 4, !dbg !1575
  %67 = load i32, ptr %65, align 4, !dbg !1575
  %68 = sext i32 %67 to i64, !dbg !1575
  %69 = load i32, ptr %12, align 4, !dbg !1575
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1575
  store i64 %68, ptr %70, align 8, !dbg !1575
  br label %91, !dbg !1575

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1575
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1575
  store ptr %73, ptr %5, align 4, !dbg !1575
  %74 = load double, ptr %72, align 4, !dbg !1575
  %75 = fptrunc double %74 to float, !dbg !1575
  %76 = load i32, ptr %12, align 4, !dbg !1575
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1575
  store float %75, ptr %77, align 8, !dbg !1575
  br label %91, !dbg !1575

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1575
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1575
  store ptr %80, ptr %5, align 4, !dbg !1575
  %81 = load double, ptr %79, align 4, !dbg !1575
  %82 = load i32, ptr %12, align 4, !dbg !1575
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1575
  store double %81, ptr %83, align 8, !dbg !1575
  br label %91, !dbg !1575

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1575
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1575
  store ptr %86, ptr %5, align 4, !dbg !1575
  %87 = load ptr, ptr %85, align 4, !dbg !1575
  %88 = load i32, ptr %12, align 4, !dbg !1575
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1575
  store ptr %87, ptr %89, align 8, !dbg !1575
  br label %91, !dbg !1575

90:                                               ; preds = %25
  br label %91, !dbg !1575

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1572

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1577
  %94 = add nsw i32 %93, 1, !dbg !1577
  store i32 %94, ptr %12, align 4, !dbg !1577
  br label %21, !dbg !1577, !llvm.loop !1578

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1562
  %97 = load ptr, ptr %96, align 4, !dbg !1562
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 125, !dbg !1562
  %99 = load ptr, ptr %98, align 4, !dbg !1562
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1562
  %101 = load ptr, ptr %6, align 4, !dbg !1562
  %102 = load ptr, ptr %7, align 4, !dbg !1562
  %103 = load ptr, ptr %8, align 4, !dbg !1562
  %104 = call x86_stdcallcc zeroext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1562
  ret i16 %104, !dbg !1562
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1579 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1580, metadata !DIExpression()), !dbg !1581
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1582, metadata !DIExpression()), !dbg !1581
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1583, metadata !DIExpression()), !dbg !1581
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1584, metadata !DIExpression()), !dbg !1581
  call void @llvm.va_start(ptr %7), !dbg !1581
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1585, metadata !DIExpression()), !dbg !1581
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1586, metadata !DIExpression()), !dbg !1581
  %13 = load ptr, ptr %6, align 4, !dbg !1581
  %14 = load ptr, ptr %13, align 4, !dbg !1581
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1581
  %16 = load ptr, ptr %15, align 4, !dbg !1581
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1581
  %18 = load ptr, ptr %4, align 4, !dbg !1581
  %19 = load ptr, ptr %6, align 4, !dbg !1581
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1581
  store i32 %20, ptr %9, align 4, !dbg !1581
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1587, metadata !DIExpression()), !dbg !1581
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1588, metadata !DIExpression()), !dbg !1590
  store i32 0, ptr %11, align 4, !dbg !1590
  br label %21, !dbg !1590

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1590
  %23 = load i32, ptr %9, align 4, !dbg !1590
  %24 = icmp slt i32 %22, %23, !dbg !1590
  br i1 %24, label %25, label %95, !dbg !1590

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1591
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1591
  %28 = load i8, ptr %27, align 1, !dbg !1591
  %29 = sext i8 %28 to i32, !dbg !1591
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1591

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1594
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1594
  store ptr %32, ptr %7, align 4, !dbg !1594
  %33 = load i32, ptr %31, align 4, !dbg !1594
  %34 = trunc i32 %33 to i8, !dbg !1594
  %35 = load i32, ptr %11, align 4, !dbg !1594
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1594
  store i8 %34, ptr %36, align 8, !dbg !1594
  br label %91, !dbg !1594

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1594
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1594
  store ptr %39, ptr %7, align 4, !dbg !1594
  %40 = load i32, ptr %38, align 4, !dbg !1594
  %41 = trunc i32 %40 to i8, !dbg !1594
  %42 = load i32, ptr %11, align 4, !dbg !1594
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1594
  store i8 %41, ptr %43, align 8, !dbg !1594
  br label %91, !dbg !1594

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1594
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1594
  store ptr %46, ptr %7, align 4, !dbg !1594
  %47 = load i32, ptr %45, align 4, !dbg !1594
  %48 = trunc i32 %47 to i16, !dbg !1594
  %49 = load i32, ptr %11, align 4, !dbg !1594
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1594
  store i16 %48, ptr %50, align 8, !dbg !1594
  br label %91, !dbg !1594

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1594
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1594
  store ptr %53, ptr %7, align 4, !dbg !1594
  %54 = load i32, ptr %52, align 4, !dbg !1594
  %55 = trunc i32 %54 to i16, !dbg !1594
  %56 = load i32, ptr %11, align 4, !dbg !1594
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1594
  store i16 %55, ptr %57, align 8, !dbg !1594
  br label %91, !dbg !1594

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1594
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1594
  store ptr %60, ptr %7, align 4, !dbg !1594
  %61 = load i32, ptr %59, align 4, !dbg !1594
  %62 = load i32, ptr %11, align 4, !dbg !1594
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1594
  store i32 %61, ptr %63, align 8, !dbg !1594
  br label %91, !dbg !1594

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1594
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1594
  store ptr %66, ptr %7, align 4, !dbg !1594
  %67 = load i32, ptr %65, align 4, !dbg !1594
  %68 = sext i32 %67 to i64, !dbg !1594
  %69 = load i32, ptr %11, align 4, !dbg !1594
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1594
  store i64 %68, ptr %70, align 8, !dbg !1594
  br label %91, !dbg !1594

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1594
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1594
  store ptr %73, ptr %7, align 4, !dbg !1594
  %74 = load double, ptr %72, align 4, !dbg !1594
  %75 = fptrunc double %74 to float, !dbg !1594
  %76 = load i32, ptr %11, align 4, !dbg !1594
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1594
  store float %75, ptr %77, align 8, !dbg !1594
  br label %91, !dbg !1594

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1594
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1594
  store ptr %80, ptr %7, align 4, !dbg !1594
  %81 = load double, ptr %79, align 4, !dbg !1594
  %82 = load i32, ptr %11, align 4, !dbg !1594
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1594
  store double %81, ptr %83, align 8, !dbg !1594
  br label %91, !dbg !1594

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1594
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1594
  store ptr %86, ptr %7, align 4, !dbg !1594
  %87 = load ptr, ptr %85, align 4, !dbg !1594
  %88 = load i32, ptr %11, align 4, !dbg !1594
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1594
  store ptr %87, ptr %89, align 8, !dbg !1594
  br label %91, !dbg !1594

90:                                               ; preds = %25
  br label %91, !dbg !1594

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1591

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1596
  %94 = add nsw i32 %93, 1, !dbg !1596
  store i32 %94, ptr %11, align 4, !dbg !1596
  br label %21, !dbg !1596, !llvm.loop !1597

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1598, metadata !DIExpression()), !dbg !1581
  %96 = load ptr, ptr %6, align 4, !dbg !1581
  %97 = load ptr, ptr %96, align 4, !dbg !1581
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 48, !dbg !1581
  %99 = load ptr, ptr %98, align 4, !dbg !1581
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1581
  %101 = load ptr, ptr %4, align 4, !dbg !1581
  %102 = load ptr, ptr %5, align 4, !dbg !1581
  %103 = load ptr, ptr %6, align 4, !dbg !1581
  %104 = call x86_stdcallcc signext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1581
  store i16 %104, ptr %12, align 2, !dbg !1581
  call void @llvm.va_end(ptr %7), !dbg !1581
  %105 = load i16, ptr %12, align 2, !dbg !1581
  ret i16 %105, !dbg !1581
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i16 @"\01_JNI_CallShortMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1599 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1600, metadata !DIExpression()), !dbg !1601
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1602, metadata !DIExpression()), !dbg !1601
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1603, metadata !DIExpression()), !dbg !1601
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1604, metadata !DIExpression()), !dbg !1601
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1605, metadata !DIExpression()), !dbg !1601
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1606, metadata !DIExpression()), !dbg !1601
  %13 = load ptr, ptr %8, align 4, !dbg !1601
  %14 = load ptr, ptr %13, align 4, !dbg !1601
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1601
  %16 = load ptr, ptr %15, align 4, !dbg !1601
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1601
  %18 = load ptr, ptr %6, align 4, !dbg !1601
  %19 = load ptr, ptr %8, align 4, !dbg !1601
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1601
  store i32 %20, ptr %10, align 4, !dbg !1601
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1607, metadata !DIExpression()), !dbg !1601
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1608, metadata !DIExpression()), !dbg !1610
  store i32 0, ptr %12, align 4, !dbg !1610
  br label %21, !dbg !1610

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1610
  %23 = load i32, ptr %10, align 4, !dbg !1610
  %24 = icmp slt i32 %22, %23, !dbg !1610
  br i1 %24, label %25, label %95, !dbg !1610

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1611
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1611
  %28 = load i8, ptr %27, align 1, !dbg !1611
  %29 = sext i8 %28 to i32, !dbg !1611
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1611

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1614
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1614
  store ptr %32, ptr %5, align 4, !dbg !1614
  %33 = load i32, ptr %31, align 4, !dbg !1614
  %34 = trunc i32 %33 to i8, !dbg !1614
  %35 = load i32, ptr %12, align 4, !dbg !1614
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1614
  store i8 %34, ptr %36, align 8, !dbg !1614
  br label %91, !dbg !1614

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1614
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1614
  store ptr %39, ptr %5, align 4, !dbg !1614
  %40 = load i32, ptr %38, align 4, !dbg !1614
  %41 = trunc i32 %40 to i8, !dbg !1614
  %42 = load i32, ptr %12, align 4, !dbg !1614
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1614
  store i8 %41, ptr %43, align 8, !dbg !1614
  br label %91, !dbg !1614

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1614
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1614
  store ptr %46, ptr %5, align 4, !dbg !1614
  %47 = load i32, ptr %45, align 4, !dbg !1614
  %48 = trunc i32 %47 to i16, !dbg !1614
  %49 = load i32, ptr %12, align 4, !dbg !1614
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1614
  store i16 %48, ptr %50, align 8, !dbg !1614
  br label %91, !dbg !1614

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1614
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1614
  store ptr %53, ptr %5, align 4, !dbg !1614
  %54 = load i32, ptr %52, align 4, !dbg !1614
  %55 = trunc i32 %54 to i16, !dbg !1614
  %56 = load i32, ptr %12, align 4, !dbg !1614
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1614
  store i16 %55, ptr %57, align 8, !dbg !1614
  br label %91, !dbg !1614

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1614
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1614
  store ptr %60, ptr %5, align 4, !dbg !1614
  %61 = load i32, ptr %59, align 4, !dbg !1614
  %62 = load i32, ptr %12, align 4, !dbg !1614
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1614
  store i32 %61, ptr %63, align 8, !dbg !1614
  br label %91, !dbg !1614

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1614
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1614
  store ptr %66, ptr %5, align 4, !dbg !1614
  %67 = load i32, ptr %65, align 4, !dbg !1614
  %68 = sext i32 %67 to i64, !dbg !1614
  %69 = load i32, ptr %12, align 4, !dbg !1614
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1614
  store i64 %68, ptr %70, align 8, !dbg !1614
  br label %91, !dbg !1614

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1614
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1614
  store ptr %73, ptr %5, align 4, !dbg !1614
  %74 = load double, ptr %72, align 4, !dbg !1614
  %75 = fptrunc double %74 to float, !dbg !1614
  %76 = load i32, ptr %12, align 4, !dbg !1614
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1614
  store float %75, ptr %77, align 8, !dbg !1614
  br label %91, !dbg !1614

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1614
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1614
  store ptr %80, ptr %5, align 4, !dbg !1614
  %81 = load double, ptr %79, align 4, !dbg !1614
  %82 = load i32, ptr %12, align 4, !dbg !1614
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1614
  store double %81, ptr %83, align 8, !dbg !1614
  br label %91, !dbg !1614

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1614
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1614
  store ptr %86, ptr %5, align 4, !dbg !1614
  %87 = load ptr, ptr %85, align 4, !dbg !1614
  %88 = load i32, ptr %12, align 4, !dbg !1614
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1614
  store ptr %87, ptr %89, align 8, !dbg !1614
  br label %91, !dbg !1614

90:                                               ; preds = %25
  br label %91, !dbg !1614

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1611

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1616
  %94 = add nsw i32 %93, 1, !dbg !1616
  store i32 %94, ptr %12, align 4, !dbg !1616
  br label %21, !dbg !1616, !llvm.loop !1617

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1601
  %97 = load ptr, ptr %96, align 4, !dbg !1601
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 48, !dbg !1601
  %99 = load ptr, ptr %98, align 4, !dbg !1601
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1601
  %101 = load ptr, ptr %6, align 4, !dbg !1601
  %102 = load ptr, ptr %7, align 4, !dbg !1601
  %103 = load ptr, ptr %8, align 4, !dbg !1601
  %104 = call x86_stdcallcc signext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1601
  ret i16 %104, !dbg !1601
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1618 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1619, metadata !DIExpression()), !dbg !1620
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1621, metadata !DIExpression()), !dbg !1620
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1622, metadata !DIExpression()), !dbg !1620
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1623, metadata !DIExpression()), !dbg !1620
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1624, metadata !DIExpression()), !dbg !1620
  call void @llvm.va_start(ptr %9), !dbg !1620
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1625, metadata !DIExpression()), !dbg !1620
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1626, metadata !DIExpression()), !dbg !1620
  %15 = load ptr, ptr %8, align 4, !dbg !1620
  %16 = load ptr, ptr %15, align 4, !dbg !1620
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1620
  %18 = load ptr, ptr %17, align 4, !dbg !1620
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1620
  %20 = load ptr, ptr %5, align 4, !dbg !1620
  %21 = load ptr, ptr %8, align 4, !dbg !1620
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1620
  store i32 %22, ptr %11, align 4, !dbg !1620
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1627, metadata !DIExpression()), !dbg !1620
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1628, metadata !DIExpression()), !dbg !1630
  store i32 0, ptr %13, align 4, !dbg !1630
  br label %23, !dbg !1630

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1630
  %25 = load i32, ptr %11, align 4, !dbg !1630
  %26 = icmp slt i32 %24, %25, !dbg !1630
  br i1 %26, label %27, label %97, !dbg !1630

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1631
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1631
  %30 = load i8, ptr %29, align 1, !dbg !1631
  %31 = sext i8 %30 to i32, !dbg !1631
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1631

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1634
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1634
  store ptr %34, ptr %9, align 4, !dbg !1634
  %35 = load i32, ptr %33, align 4, !dbg !1634
  %36 = trunc i32 %35 to i8, !dbg !1634
  %37 = load i32, ptr %13, align 4, !dbg !1634
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1634
  store i8 %36, ptr %38, align 8, !dbg !1634
  br label %93, !dbg !1634

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1634
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1634
  store ptr %41, ptr %9, align 4, !dbg !1634
  %42 = load i32, ptr %40, align 4, !dbg !1634
  %43 = trunc i32 %42 to i8, !dbg !1634
  %44 = load i32, ptr %13, align 4, !dbg !1634
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1634
  store i8 %43, ptr %45, align 8, !dbg !1634
  br label %93, !dbg !1634

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1634
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1634
  store ptr %48, ptr %9, align 4, !dbg !1634
  %49 = load i32, ptr %47, align 4, !dbg !1634
  %50 = trunc i32 %49 to i16, !dbg !1634
  %51 = load i32, ptr %13, align 4, !dbg !1634
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1634
  store i16 %50, ptr %52, align 8, !dbg !1634
  br label %93, !dbg !1634

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1634
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1634
  store ptr %55, ptr %9, align 4, !dbg !1634
  %56 = load i32, ptr %54, align 4, !dbg !1634
  %57 = trunc i32 %56 to i16, !dbg !1634
  %58 = load i32, ptr %13, align 4, !dbg !1634
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1634
  store i16 %57, ptr %59, align 8, !dbg !1634
  br label %93, !dbg !1634

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1634
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1634
  store ptr %62, ptr %9, align 4, !dbg !1634
  %63 = load i32, ptr %61, align 4, !dbg !1634
  %64 = load i32, ptr %13, align 4, !dbg !1634
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1634
  store i32 %63, ptr %65, align 8, !dbg !1634
  br label %93, !dbg !1634

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1634
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1634
  store ptr %68, ptr %9, align 4, !dbg !1634
  %69 = load i32, ptr %67, align 4, !dbg !1634
  %70 = sext i32 %69 to i64, !dbg !1634
  %71 = load i32, ptr %13, align 4, !dbg !1634
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1634
  store i64 %70, ptr %72, align 8, !dbg !1634
  br label %93, !dbg !1634

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1634
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1634
  store ptr %75, ptr %9, align 4, !dbg !1634
  %76 = load double, ptr %74, align 4, !dbg !1634
  %77 = fptrunc double %76 to float, !dbg !1634
  %78 = load i32, ptr %13, align 4, !dbg !1634
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1634
  store float %77, ptr %79, align 8, !dbg !1634
  br label %93, !dbg !1634

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1634
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1634
  store ptr %82, ptr %9, align 4, !dbg !1634
  %83 = load double, ptr %81, align 4, !dbg !1634
  %84 = load i32, ptr %13, align 4, !dbg !1634
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1634
  store double %83, ptr %85, align 8, !dbg !1634
  br label %93, !dbg !1634

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1634
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1634
  store ptr %88, ptr %9, align 4, !dbg !1634
  %89 = load ptr, ptr %87, align 4, !dbg !1634
  %90 = load i32, ptr %13, align 4, !dbg !1634
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1634
  store ptr %89, ptr %91, align 8, !dbg !1634
  br label %93, !dbg !1634

92:                                               ; preds = %27
  br label %93, !dbg !1634

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1631

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1636
  %96 = add nsw i32 %95, 1, !dbg !1636
  store i32 %96, ptr %13, align 4, !dbg !1636
  br label %23, !dbg !1636, !llvm.loop !1637

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1638, metadata !DIExpression()), !dbg !1620
  %98 = load ptr, ptr %8, align 4, !dbg !1620
  %99 = load ptr, ptr %98, align 4, !dbg !1620
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 78, !dbg !1620
  %101 = load ptr, ptr %100, align 4, !dbg !1620
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1620
  %103 = load ptr, ptr %5, align 4, !dbg !1620
  %104 = load ptr, ptr %6, align 4, !dbg !1620
  %105 = load ptr, ptr %7, align 4, !dbg !1620
  %106 = load ptr, ptr %8, align 4, !dbg !1620
  %107 = call x86_stdcallcc signext i16 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1620
  store i16 %107, ptr %14, align 2, !dbg !1620
  call void @llvm.va_end(ptr %9), !dbg !1620
  %108 = load i16, ptr %14, align 2, !dbg !1620
  ret i16 %108, !dbg !1620
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i16 @"\01_JNI_CallNonvirtualShortMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1639 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1640, metadata !DIExpression()), !dbg !1641
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1642, metadata !DIExpression()), !dbg !1641
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1643, metadata !DIExpression()), !dbg !1641
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1644, metadata !DIExpression()), !dbg !1641
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1645, metadata !DIExpression()), !dbg !1641
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1646, metadata !DIExpression()), !dbg !1641
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1647, metadata !DIExpression()), !dbg !1641
  %15 = load ptr, ptr %10, align 4, !dbg !1641
  %16 = load ptr, ptr %15, align 4, !dbg !1641
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1641
  %18 = load ptr, ptr %17, align 4, !dbg !1641
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1641
  %20 = load ptr, ptr %7, align 4, !dbg !1641
  %21 = load ptr, ptr %10, align 4, !dbg !1641
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1641
  store i32 %22, ptr %12, align 4, !dbg !1641
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1648, metadata !DIExpression()), !dbg !1641
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1649, metadata !DIExpression()), !dbg !1651
  store i32 0, ptr %14, align 4, !dbg !1651
  br label %23, !dbg !1651

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1651
  %25 = load i32, ptr %12, align 4, !dbg !1651
  %26 = icmp slt i32 %24, %25, !dbg !1651
  br i1 %26, label %27, label %97, !dbg !1651

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1652
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1652
  %30 = load i8, ptr %29, align 1, !dbg !1652
  %31 = sext i8 %30 to i32, !dbg !1652
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1652

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1655
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1655
  store ptr %34, ptr %6, align 4, !dbg !1655
  %35 = load i32, ptr %33, align 4, !dbg !1655
  %36 = trunc i32 %35 to i8, !dbg !1655
  %37 = load i32, ptr %14, align 4, !dbg !1655
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1655
  store i8 %36, ptr %38, align 8, !dbg !1655
  br label %93, !dbg !1655

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1655
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1655
  store ptr %41, ptr %6, align 4, !dbg !1655
  %42 = load i32, ptr %40, align 4, !dbg !1655
  %43 = trunc i32 %42 to i8, !dbg !1655
  %44 = load i32, ptr %14, align 4, !dbg !1655
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1655
  store i8 %43, ptr %45, align 8, !dbg !1655
  br label %93, !dbg !1655

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1655
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1655
  store ptr %48, ptr %6, align 4, !dbg !1655
  %49 = load i32, ptr %47, align 4, !dbg !1655
  %50 = trunc i32 %49 to i16, !dbg !1655
  %51 = load i32, ptr %14, align 4, !dbg !1655
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1655
  store i16 %50, ptr %52, align 8, !dbg !1655
  br label %93, !dbg !1655

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1655
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1655
  store ptr %55, ptr %6, align 4, !dbg !1655
  %56 = load i32, ptr %54, align 4, !dbg !1655
  %57 = trunc i32 %56 to i16, !dbg !1655
  %58 = load i32, ptr %14, align 4, !dbg !1655
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1655
  store i16 %57, ptr %59, align 8, !dbg !1655
  br label %93, !dbg !1655

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1655
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1655
  store ptr %62, ptr %6, align 4, !dbg !1655
  %63 = load i32, ptr %61, align 4, !dbg !1655
  %64 = load i32, ptr %14, align 4, !dbg !1655
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1655
  store i32 %63, ptr %65, align 8, !dbg !1655
  br label %93, !dbg !1655

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1655
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1655
  store ptr %68, ptr %6, align 4, !dbg !1655
  %69 = load i32, ptr %67, align 4, !dbg !1655
  %70 = sext i32 %69 to i64, !dbg !1655
  %71 = load i32, ptr %14, align 4, !dbg !1655
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1655
  store i64 %70, ptr %72, align 8, !dbg !1655
  br label %93, !dbg !1655

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1655
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1655
  store ptr %75, ptr %6, align 4, !dbg !1655
  %76 = load double, ptr %74, align 4, !dbg !1655
  %77 = fptrunc double %76 to float, !dbg !1655
  %78 = load i32, ptr %14, align 4, !dbg !1655
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1655
  store float %77, ptr %79, align 8, !dbg !1655
  br label %93, !dbg !1655

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1655
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1655
  store ptr %82, ptr %6, align 4, !dbg !1655
  %83 = load double, ptr %81, align 4, !dbg !1655
  %84 = load i32, ptr %14, align 4, !dbg !1655
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1655
  store double %83, ptr %85, align 8, !dbg !1655
  br label %93, !dbg !1655

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1655
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1655
  store ptr %88, ptr %6, align 4, !dbg !1655
  %89 = load ptr, ptr %87, align 4, !dbg !1655
  %90 = load i32, ptr %14, align 4, !dbg !1655
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1655
  store ptr %89, ptr %91, align 8, !dbg !1655
  br label %93, !dbg !1655

92:                                               ; preds = %27
  br label %93, !dbg !1655

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1652

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1657
  %96 = add nsw i32 %95, 1, !dbg !1657
  store i32 %96, ptr %14, align 4, !dbg !1657
  br label %23, !dbg !1657, !llvm.loop !1658

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1641
  %99 = load ptr, ptr %98, align 4, !dbg !1641
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 78, !dbg !1641
  %101 = load ptr, ptr %100, align 4, !dbg !1641
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1641
  %103 = load ptr, ptr %7, align 4, !dbg !1641
  %104 = load ptr, ptr %8, align 4, !dbg !1641
  %105 = load ptr, ptr %9, align 4, !dbg !1641
  %106 = load ptr, ptr %10, align 4, !dbg !1641
  %107 = call x86_stdcallcc signext i16 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1641
  ret i16 %107, !dbg !1641
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1659 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1660, metadata !DIExpression()), !dbg !1661
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1662, metadata !DIExpression()), !dbg !1661
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1663, metadata !DIExpression()), !dbg !1661
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1664, metadata !DIExpression()), !dbg !1661
  call void @llvm.va_start(ptr %7), !dbg !1661
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1665, metadata !DIExpression()), !dbg !1661
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1666, metadata !DIExpression()), !dbg !1661
  %13 = load ptr, ptr %6, align 4, !dbg !1661
  %14 = load ptr, ptr %13, align 4, !dbg !1661
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1661
  %16 = load ptr, ptr %15, align 4, !dbg !1661
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1661
  %18 = load ptr, ptr %4, align 4, !dbg !1661
  %19 = load ptr, ptr %6, align 4, !dbg !1661
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1661
  store i32 %20, ptr %9, align 4, !dbg !1661
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1667, metadata !DIExpression()), !dbg !1661
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1668, metadata !DIExpression()), !dbg !1670
  store i32 0, ptr %11, align 4, !dbg !1670
  br label %21, !dbg !1670

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1670
  %23 = load i32, ptr %9, align 4, !dbg !1670
  %24 = icmp slt i32 %22, %23, !dbg !1670
  br i1 %24, label %25, label %95, !dbg !1670

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1671
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1671
  %28 = load i8, ptr %27, align 1, !dbg !1671
  %29 = sext i8 %28 to i32, !dbg !1671
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1671

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1674
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1674
  store ptr %32, ptr %7, align 4, !dbg !1674
  %33 = load i32, ptr %31, align 4, !dbg !1674
  %34 = trunc i32 %33 to i8, !dbg !1674
  %35 = load i32, ptr %11, align 4, !dbg !1674
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1674
  store i8 %34, ptr %36, align 8, !dbg !1674
  br label %91, !dbg !1674

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1674
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1674
  store ptr %39, ptr %7, align 4, !dbg !1674
  %40 = load i32, ptr %38, align 4, !dbg !1674
  %41 = trunc i32 %40 to i8, !dbg !1674
  %42 = load i32, ptr %11, align 4, !dbg !1674
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1674
  store i8 %41, ptr %43, align 8, !dbg !1674
  br label %91, !dbg !1674

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1674
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1674
  store ptr %46, ptr %7, align 4, !dbg !1674
  %47 = load i32, ptr %45, align 4, !dbg !1674
  %48 = trunc i32 %47 to i16, !dbg !1674
  %49 = load i32, ptr %11, align 4, !dbg !1674
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1674
  store i16 %48, ptr %50, align 8, !dbg !1674
  br label %91, !dbg !1674

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1674
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1674
  store ptr %53, ptr %7, align 4, !dbg !1674
  %54 = load i32, ptr %52, align 4, !dbg !1674
  %55 = trunc i32 %54 to i16, !dbg !1674
  %56 = load i32, ptr %11, align 4, !dbg !1674
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1674
  store i16 %55, ptr %57, align 8, !dbg !1674
  br label %91, !dbg !1674

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1674
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1674
  store ptr %60, ptr %7, align 4, !dbg !1674
  %61 = load i32, ptr %59, align 4, !dbg !1674
  %62 = load i32, ptr %11, align 4, !dbg !1674
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1674
  store i32 %61, ptr %63, align 8, !dbg !1674
  br label %91, !dbg !1674

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1674
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1674
  store ptr %66, ptr %7, align 4, !dbg !1674
  %67 = load i32, ptr %65, align 4, !dbg !1674
  %68 = sext i32 %67 to i64, !dbg !1674
  %69 = load i32, ptr %11, align 4, !dbg !1674
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1674
  store i64 %68, ptr %70, align 8, !dbg !1674
  br label %91, !dbg !1674

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1674
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1674
  store ptr %73, ptr %7, align 4, !dbg !1674
  %74 = load double, ptr %72, align 4, !dbg !1674
  %75 = fptrunc double %74 to float, !dbg !1674
  %76 = load i32, ptr %11, align 4, !dbg !1674
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1674
  store float %75, ptr %77, align 8, !dbg !1674
  br label %91, !dbg !1674

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1674
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1674
  store ptr %80, ptr %7, align 4, !dbg !1674
  %81 = load double, ptr %79, align 4, !dbg !1674
  %82 = load i32, ptr %11, align 4, !dbg !1674
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1674
  store double %81, ptr %83, align 8, !dbg !1674
  br label %91, !dbg !1674

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1674
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1674
  store ptr %86, ptr %7, align 4, !dbg !1674
  %87 = load ptr, ptr %85, align 4, !dbg !1674
  %88 = load i32, ptr %11, align 4, !dbg !1674
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1674
  store ptr %87, ptr %89, align 8, !dbg !1674
  br label %91, !dbg !1674

90:                                               ; preds = %25
  br label %91, !dbg !1674

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1671

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1676
  %94 = add nsw i32 %93, 1, !dbg !1676
  store i32 %94, ptr %11, align 4, !dbg !1676
  br label %21, !dbg !1676, !llvm.loop !1677

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1678, metadata !DIExpression()), !dbg !1661
  %96 = load ptr, ptr %6, align 4, !dbg !1661
  %97 = load ptr, ptr %96, align 4, !dbg !1661
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 128, !dbg !1661
  %99 = load ptr, ptr %98, align 4, !dbg !1661
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1661
  %101 = load ptr, ptr %4, align 4, !dbg !1661
  %102 = load ptr, ptr %5, align 4, !dbg !1661
  %103 = load ptr, ptr %6, align 4, !dbg !1661
  %104 = call x86_stdcallcc signext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1661
  store i16 %104, ptr %12, align 2, !dbg !1661
  call void @llvm.va_end(ptr %7), !dbg !1661
  %105 = load i16, ptr %12, align 2, !dbg !1661
  ret i16 %105, !dbg !1661
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc signext i16 @"\01_JNI_CallStaticShortMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1679 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1680, metadata !DIExpression()), !dbg !1681
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1682, metadata !DIExpression()), !dbg !1681
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1683, metadata !DIExpression()), !dbg !1681
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1684, metadata !DIExpression()), !dbg !1681
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1685, metadata !DIExpression()), !dbg !1681
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1686, metadata !DIExpression()), !dbg !1681
  %13 = load ptr, ptr %8, align 4, !dbg !1681
  %14 = load ptr, ptr %13, align 4, !dbg !1681
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1681
  %16 = load ptr, ptr %15, align 4, !dbg !1681
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1681
  %18 = load ptr, ptr %6, align 4, !dbg !1681
  %19 = load ptr, ptr %8, align 4, !dbg !1681
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1681
  store i32 %20, ptr %10, align 4, !dbg !1681
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1687, metadata !DIExpression()), !dbg !1681
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1688, metadata !DIExpression()), !dbg !1690
  store i32 0, ptr %12, align 4, !dbg !1690
  br label %21, !dbg !1690

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1690
  %23 = load i32, ptr %10, align 4, !dbg !1690
  %24 = icmp slt i32 %22, %23, !dbg !1690
  br i1 %24, label %25, label %95, !dbg !1690

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1691
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1691
  %28 = load i8, ptr %27, align 1, !dbg !1691
  %29 = sext i8 %28 to i32, !dbg !1691
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1691

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1694
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1694
  store ptr %32, ptr %5, align 4, !dbg !1694
  %33 = load i32, ptr %31, align 4, !dbg !1694
  %34 = trunc i32 %33 to i8, !dbg !1694
  %35 = load i32, ptr %12, align 4, !dbg !1694
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1694
  store i8 %34, ptr %36, align 8, !dbg !1694
  br label %91, !dbg !1694

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1694
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1694
  store ptr %39, ptr %5, align 4, !dbg !1694
  %40 = load i32, ptr %38, align 4, !dbg !1694
  %41 = trunc i32 %40 to i8, !dbg !1694
  %42 = load i32, ptr %12, align 4, !dbg !1694
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1694
  store i8 %41, ptr %43, align 8, !dbg !1694
  br label %91, !dbg !1694

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1694
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1694
  store ptr %46, ptr %5, align 4, !dbg !1694
  %47 = load i32, ptr %45, align 4, !dbg !1694
  %48 = trunc i32 %47 to i16, !dbg !1694
  %49 = load i32, ptr %12, align 4, !dbg !1694
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1694
  store i16 %48, ptr %50, align 8, !dbg !1694
  br label %91, !dbg !1694

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1694
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1694
  store ptr %53, ptr %5, align 4, !dbg !1694
  %54 = load i32, ptr %52, align 4, !dbg !1694
  %55 = trunc i32 %54 to i16, !dbg !1694
  %56 = load i32, ptr %12, align 4, !dbg !1694
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1694
  store i16 %55, ptr %57, align 8, !dbg !1694
  br label %91, !dbg !1694

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1694
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1694
  store ptr %60, ptr %5, align 4, !dbg !1694
  %61 = load i32, ptr %59, align 4, !dbg !1694
  %62 = load i32, ptr %12, align 4, !dbg !1694
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1694
  store i32 %61, ptr %63, align 8, !dbg !1694
  br label %91, !dbg !1694

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1694
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1694
  store ptr %66, ptr %5, align 4, !dbg !1694
  %67 = load i32, ptr %65, align 4, !dbg !1694
  %68 = sext i32 %67 to i64, !dbg !1694
  %69 = load i32, ptr %12, align 4, !dbg !1694
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1694
  store i64 %68, ptr %70, align 8, !dbg !1694
  br label %91, !dbg !1694

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1694
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1694
  store ptr %73, ptr %5, align 4, !dbg !1694
  %74 = load double, ptr %72, align 4, !dbg !1694
  %75 = fptrunc double %74 to float, !dbg !1694
  %76 = load i32, ptr %12, align 4, !dbg !1694
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1694
  store float %75, ptr %77, align 8, !dbg !1694
  br label %91, !dbg !1694

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1694
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1694
  store ptr %80, ptr %5, align 4, !dbg !1694
  %81 = load double, ptr %79, align 4, !dbg !1694
  %82 = load i32, ptr %12, align 4, !dbg !1694
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1694
  store double %81, ptr %83, align 8, !dbg !1694
  br label %91, !dbg !1694

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1694
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1694
  store ptr %86, ptr %5, align 4, !dbg !1694
  %87 = load ptr, ptr %85, align 4, !dbg !1694
  %88 = load i32, ptr %12, align 4, !dbg !1694
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1694
  store ptr %87, ptr %89, align 8, !dbg !1694
  br label %91, !dbg !1694

90:                                               ; preds = %25
  br label %91, !dbg !1694

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1691

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1696
  %94 = add nsw i32 %93, 1, !dbg !1696
  store i32 %94, ptr %12, align 4, !dbg !1696
  br label %21, !dbg !1696, !llvm.loop !1697

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1681
  %97 = load ptr, ptr %96, align 4, !dbg !1681
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 128, !dbg !1681
  %99 = load ptr, ptr %98, align 4, !dbg !1681
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1681
  %101 = load ptr, ptr %6, align 4, !dbg !1681
  %102 = load ptr, ptr %7, align 4, !dbg !1681
  %103 = load ptr, ptr %8, align 4, !dbg !1681
  %104 = call x86_stdcallcc signext i16 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1681
  ret i16 %104, !dbg !1681
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1698 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1699, metadata !DIExpression()), !dbg !1700
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1701, metadata !DIExpression()), !dbg !1700
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1702, metadata !DIExpression()), !dbg !1700
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1703, metadata !DIExpression()), !dbg !1700
  call void @llvm.va_start(ptr %7), !dbg !1700
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1704, metadata !DIExpression()), !dbg !1700
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1705, metadata !DIExpression()), !dbg !1700
  %13 = load ptr, ptr %6, align 4, !dbg !1700
  %14 = load ptr, ptr %13, align 4, !dbg !1700
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1700
  %16 = load ptr, ptr %15, align 4, !dbg !1700
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1700
  %18 = load ptr, ptr %4, align 4, !dbg !1700
  %19 = load ptr, ptr %6, align 4, !dbg !1700
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1700
  store i32 %20, ptr %9, align 4, !dbg !1700
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1706, metadata !DIExpression()), !dbg !1700
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1707, metadata !DIExpression()), !dbg !1709
  store i32 0, ptr %11, align 4, !dbg !1709
  br label %21, !dbg !1709

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1709
  %23 = load i32, ptr %9, align 4, !dbg !1709
  %24 = icmp slt i32 %22, %23, !dbg !1709
  br i1 %24, label %25, label %95, !dbg !1709

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1710
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1710
  %28 = load i8, ptr %27, align 1, !dbg !1710
  %29 = sext i8 %28 to i32, !dbg !1710
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1710

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1713
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1713
  store ptr %32, ptr %7, align 4, !dbg !1713
  %33 = load i32, ptr %31, align 4, !dbg !1713
  %34 = trunc i32 %33 to i8, !dbg !1713
  %35 = load i32, ptr %11, align 4, !dbg !1713
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1713
  store i8 %34, ptr %36, align 8, !dbg !1713
  br label %91, !dbg !1713

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1713
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1713
  store ptr %39, ptr %7, align 4, !dbg !1713
  %40 = load i32, ptr %38, align 4, !dbg !1713
  %41 = trunc i32 %40 to i8, !dbg !1713
  %42 = load i32, ptr %11, align 4, !dbg !1713
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1713
  store i8 %41, ptr %43, align 8, !dbg !1713
  br label %91, !dbg !1713

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1713
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1713
  store ptr %46, ptr %7, align 4, !dbg !1713
  %47 = load i32, ptr %45, align 4, !dbg !1713
  %48 = trunc i32 %47 to i16, !dbg !1713
  %49 = load i32, ptr %11, align 4, !dbg !1713
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1713
  store i16 %48, ptr %50, align 8, !dbg !1713
  br label %91, !dbg !1713

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1713
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1713
  store ptr %53, ptr %7, align 4, !dbg !1713
  %54 = load i32, ptr %52, align 4, !dbg !1713
  %55 = trunc i32 %54 to i16, !dbg !1713
  %56 = load i32, ptr %11, align 4, !dbg !1713
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1713
  store i16 %55, ptr %57, align 8, !dbg !1713
  br label %91, !dbg !1713

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1713
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1713
  store ptr %60, ptr %7, align 4, !dbg !1713
  %61 = load i32, ptr %59, align 4, !dbg !1713
  %62 = load i32, ptr %11, align 4, !dbg !1713
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1713
  store i32 %61, ptr %63, align 8, !dbg !1713
  br label %91, !dbg !1713

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1713
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1713
  store ptr %66, ptr %7, align 4, !dbg !1713
  %67 = load i32, ptr %65, align 4, !dbg !1713
  %68 = sext i32 %67 to i64, !dbg !1713
  %69 = load i32, ptr %11, align 4, !dbg !1713
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1713
  store i64 %68, ptr %70, align 8, !dbg !1713
  br label %91, !dbg !1713

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1713
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1713
  store ptr %73, ptr %7, align 4, !dbg !1713
  %74 = load double, ptr %72, align 4, !dbg !1713
  %75 = fptrunc double %74 to float, !dbg !1713
  %76 = load i32, ptr %11, align 4, !dbg !1713
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1713
  store float %75, ptr %77, align 8, !dbg !1713
  br label %91, !dbg !1713

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1713
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1713
  store ptr %80, ptr %7, align 4, !dbg !1713
  %81 = load double, ptr %79, align 4, !dbg !1713
  %82 = load i32, ptr %11, align 4, !dbg !1713
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1713
  store double %81, ptr %83, align 8, !dbg !1713
  br label %91, !dbg !1713

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1713
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1713
  store ptr %86, ptr %7, align 4, !dbg !1713
  %87 = load ptr, ptr %85, align 4, !dbg !1713
  %88 = load i32, ptr %11, align 4, !dbg !1713
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1713
  store ptr %87, ptr %89, align 8, !dbg !1713
  br label %91, !dbg !1713

90:                                               ; preds = %25
  br label %91, !dbg !1713

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1710

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1715
  %94 = add nsw i32 %93, 1, !dbg !1715
  store i32 %94, ptr %11, align 4, !dbg !1715
  br label %21, !dbg !1715, !llvm.loop !1716

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1717, metadata !DIExpression()), !dbg !1700
  %96 = load ptr, ptr %6, align 4, !dbg !1700
  %97 = load ptr, ptr %96, align 4, !dbg !1700
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 51, !dbg !1700
  %99 = load ptr, ptr %98, align 4, !dbg !1700
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1700
  %101 = load ptr, ptr %4, align 4, !dbg !1700
  %102 = load ptr, ptr %5, align 4, !dbg !1700
  %103 = load ptr, ptr %6, align 4, !dbg !1700
  %104 = call x86_stdcallcc i32 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1700
  store i32 %104, ptr %12, align 4, !dbg !1700
  call void @llvm.va_end(ptr %7), !dbg !1700
  %105 = load i32, ptr %12, align 4, !dbg !1700
  ret i32 %105, !dbg !1700
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i32 @"\01_JNI_CallIntMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1718 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1719, metadata !DIExpression()), !dbg !1720
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1721, metadata !DIExpression()), !dbg !1720
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1722, metadata !DIExpression()), !dbg !1720
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1723, metadata !DIExpression()), !dbg !1720
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1724, metadata !DIExpression()), !dbg !1720
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1725, metadata !DIExpression()), !dbg !1720
  %13 = load ptr, ptr %8, align 4, !dbg !1720
  %14 = load ptr, ptr %13, align 4, !dbg !1720
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1720
  %16 = load ptr, ptr %15, align 4, !dbg !1720
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1720
  %18 = load ptr, ptr %6, align 4, !dbg !1720
  %19 = load ptr, ptr %8, align 4, !dbg !1720
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1720
  store i32 %20, ptr %10, align 4, !dbg !1720
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1726, metadata !DIExpression()), !dbg !1720
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1727, metadata !DIExpression()), !dbg !1729
  store i32 0, ptr %12, align 4, !dbg !1729
  br label %21, !dbg !1729

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1729
  %23 = load i32, ptr %10, align 4, !dbg !1729
  %24 = icmp slt i32 %22, %23, !dbg !1729
  br i1 %24, label %25, label %95, !dbg !1729

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1730
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1730
  %28 = load i8, ptr %27, align 1, !dbg !1730
  %29 = sext i8 %28 to i32, !dbg !1730
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1730

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1733
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1733
  store ptr %32, ptr %5, align 4, !dbg !1733
  %33 = load i32, ptr %31, align 4, !dbg !1733
  %34 = trunc i32 %33 to i8, !dbg !1733
  %35 = load i32, ptr %12, align 4, !dbg !1733
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1733
  store i8 %34, ptr %36, align 8, !dbg !1733
  br label %91, !dbg !1733

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1733
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1733
  store ptr %39, ptr %5, align 4, !dbg !1733
  %40 = load i32, ptr %38, align 4, !dbg !1733
  %41 = trunc i32 %40 to i8, !dbg !1733
  %42 = load i32, ptr %12, align 4, !dbg !1733
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1733
  store i8 %41, ptr %43, align 8, !dbg !1733
  br label %91, !dbg !1733

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1733
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1733
  store ptr %46, ptr %5, align 4, !dbg !1733
  %47 = load i32, ptr %45, align 4, !dbg !1733
  %48 = trunc i32 %47 to i16, !dbg !1733
  %49 = load i32, ptr %12, align 4, !dbg !1733
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1733
  store i16 %48, ptr %50, align 8, !dbg !1733
  br label %91, !dbg !1733

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1733
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1733
  store ptr %53, ptr %5, align 4, !dbg !1733
  %54 = load i32, ptr %52, align 4, !dbg !1733
  %55 = trunc i32 %54 to i16, !dbg !1733
  %56 = load i32, ptr %12, align 4, !dbg !1733
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1733
  store i16 %55, ptr %57, align 8, !dbg !1733
  br label %91, !dbg !1733

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1733
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1733
  store ptr %60, ptr %5, align 4, !dbg !1733
  %61 = load i32, ptr %59, align 4, !dbg !1733
  %62 = load i32, ptr %12, align 4, !dbg !1733
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1733
  store i32 %61, ptr %63, align 8, !dbg !1733
  br label %91, !dbg !1733

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1733
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1733
  store ptr %66, ptr %5, align 4, !dbg !1733
  %67 = load i32, ptr %65, align 4, !dbg !1733
  %68 = sext i32 %67 to i64, !dbg !1733
  %69 = load i32, ptr %12, align 4, !dbg !1733
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1733
  store i64 %68, ptr %70, align 8, !dbg !1733
  br label %91, !dbg !1733

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1733
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1733
  store ptr %73, ptr %5, align 4, !dbg !1733
  %74 = load double, ptr %72, align 4, !dbg !1733
  %75 = fptrunc double %74 to float, !dbg !1733
  %76 = load i32, ptr %12, align 4, !dbg !1733
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1733
  store float %75, ptr %77, align 8, !dbg !1733
  br label %91, !dbg !1733

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1733
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1733
  store ptr %80, ptr %5, align 4, !dbg !1733
  %81 = load double, ptr %79, align 4, !dbg !1733
  %82 = load i32, ptr %12, align 4, !dbg !1733
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1733
  store double %81, ptr %83, align 8, !dbg !1733
  br label %91, !dbg !1733

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1733
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1733
  store ptr %86, ptr %5, align 4, !dbg !1733
  %87 = load ptr, ptr %85, align 4, !dbg !1733
  %88 = load i32, ptr %12, align 4, !dbg !1733
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1733
  store ptr %87, ptr %89, align 8, !dbg !1733
  br label %91, !dbg !1733

90:                                               ; preds = %25
  br label %91, !dbg !1733

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1730

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1735
  %94 = add nsw i32 %93, 1, !dbg !1735
  store i32 %94, ptr %12, align 4, !dbg !1735
  br label %21, !dbg !1735, !llvm.loop !1736

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1720
  %97 = load ptr, ptr %96, align 4, !dbg !1720
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 51, !dbg !1720
  %99 = load ptr, ptr %98, align 4, !dbg !1720
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1720
  %101 = load ptr, ptr %6, align 4, !dbg !1720
  %102 = load ptr, ptr %7, align 4, !dbg !1720
  %103 = load ptr, ptr %8, align 4, !dbg !1720
  %104 = call x86_stdcallcc i32 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1720
  ret i32 %104, !dbg !1720
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1737 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1738, metadata !DIExpression()), !dbg !1739
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1740, metadata !DIExpression()), !dbg !1739
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1741, metadata !DIExpression()), !dbg !1739
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1742, metadata !DIExpression()), !dbg !1739
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1743, metadata !DIExpression()), !dbg !1739
  call void @llvm.va_start(ptr %9), !dbg !1739
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1744, metadata !DIExpression()), !dbg !1739
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1745, metadata !DIExpression()), !dbg !1739
  %15 = load ptr, ptr %8, align 4, !dbg !1739
  %16 = load ptr, ptr %15, align 4, !dbg !1739
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1739
  %18 = load ptr, ptr %17, align 4, !dbg !1739
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1739
  %20 = load ptr, ptr %5, align 4, !dbg !1739
  %21 = load ptr, ptr %8, align 4, !dbg !1739
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1739
  store i32 %22, ptr %11, align 4, !dbg !1739
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1746, metadata !DIExpression()), !dbg !1739
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1747, metadata !DIExpression()), !dbg !1749
  store i32 0, ptr %13, align 4, !dbg !1749
  br label %23, !dbg !1749

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1749
  %25 = load i32, ptr %11, align 4, !dbg !1749
  %26 = icmp slt i32 %24, %25, !dbg !1749
  br i1 %26, label %27, label %97, !dbg !1749

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1750
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1750
  %30 = load i8, ptr %29, align 1, !dbg !1750
  %31 = sext i8 %30 to i32, !dbg !1750
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1750

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1753
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1753
  store ptr %34, ptr %9, align 4, !dbg !1753
  %35 = load i32, ptr %33, align 4, !dbg !1753
  %36 = trunc i32 %35 to i8, !dbg !1753
  %37 = load i32, ptr %13, align 4, !dbg !1753
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1753
  store i8 %36, ptr %38, align 8, !dbg !1753
  br label %93, !dbg !1753

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1753
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1753
  store ptr %41, ptr %9, align 4, !dbg !1753
  %42 = load i32, ptr %40, align 4, !dbg !1753
  %43 = trunc i32 %42 to i8, !dbg !1753
  %44 = load i32, ptr %13, align 4, !dbg !1753
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1753
  store i8 %43, ptr %45, align 8, !dbg !1753
  br label %93, !dbg !1753

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1753
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1753
  store ptr %48, ptr %9, align 4, !dbg !1753
  %49 = load i32, ptr %47, align 4, !dbg !1753
  %50 = trunc i32 %49 to i16, !dbg !1753
  %51 = load i32, ptr %13, align 4, !dbg !1753
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1753
  store i16 %50, ptr %52, align 8, !dbg !1753
  br label %93, !dbg !1753

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1753
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1753
  store ptr %55, ptr %9, align 4, !dbg !1753
  %56 = load i32, ptr %54, align 4, !dbg !1753
  %57 = trunc i32 %56 to i16, !dbg !1753
  %58 = load i32, ptr %13, align 4, !dbg !1753
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1753
  store i16 %57, ptr %59, align 8, !dbg !1753
  br label %93, !dbg !1753

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1753
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1753
  store ptr %62, ptr %9, align 4, !dbg !1753
  %63 = load i32, ptr %61, align 4, !dbg !1753
  %64 = load i32, ptr %13, align 4, !dbg !1753
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1753
  store i32 %63, ptr %65, align 8, !dbg !1753
  br label %93, !dbg !1753

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1753
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1753
  store ptr %68, ptr %9, align 4, !dbg !1753
  %69 = load i32, ptr %67, align 4, !dbg !1753
  %70 = sext i32 %69 to i64, !dbg !1753
  %71 = load i32, ptr %13, align 4, !dbg !1753
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1753
  store i64 %70, ptr %72, align 8, !dbg !1753
  br label %93, !dbg !1753

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1753
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1753
  store ptr %75, ptr %9, align 4, !dbg !1753
  %76 = load double, ptr %74, align 4, !dbg !1753
  %77 = fptrunc double %76 to float, !dbg !1753
  %78 = load i32, ptr %13, align 4, !dbg !1753
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1753
  store float %77, ptr %79, align 8, !dbg !1753
  br label %93, !dbg !1753

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1753
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1753
  store ptr %82, ptr %9, align 4, !dbg !1753
  %83 = load double, ptr %81, align 4, !dbg !1753
  %84 = load i32, ptr %13, align 4, !dbg !1753
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1753
  store double %83, ptr %85, align 8, !dbg !1753
  br label %93, !dbg !1753

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1753
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1753
  store ptr %88, ptr %9, align 4, !dbg !1753
  %89 = load ptr, ptr %87, align 4, !dbg !1753
  %90 = load i32, ptr %13, align 4, !dbg !1753
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1753
  store ptr %89, ptr %91, align 8, !dbg !1753
  br label %93, !dbg !1753

92:                                               ; preds = %27
  br label %93, !dbg !1753

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1750

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1755
  %96 = add nsw i32 %95, 1, !dbg !1755
  store i32 %96, ptr %13, align 4, !dbg !1755
  br label %23, !dbg !1755, !llvm.loop !1756

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1757, metadata !DIExpression()), !dbg !1739
  %98 = load ptr, ptr %8, align 4, !dbg !1739
  %99 = load ptr, ptr %98, align 4, !dbg !1739
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 81, !dbg !1739
  %101 = load ptr, ptr %100, align 4, !dbg !1739
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1739
  %103 = load ptr, ptr %5, align 4, !dbg !1739
  %104 = load ptr, ptr %6, align 4, !dbg !1739
  %105 = load ptr, ptr %7, align 4, !dbg !1739
  %106 = load ptr, ptr %8, align 4, !dbg !1739
  %107 = call x86_stdcallcc i32 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1739
  store i32 %107, ptr %14, align 4, !dbg !1739
  call void @llvm.va_end(ptr %9), !dbg !1739
  %108 = load i32, ptr %14, align 4, !dbg !1739
  ret i32 %108, !dbg !1739
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i32 @"\01_JNI_CallNonvirtualIntMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1758 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1759, metadata !DIExpression()), !dbg !1760
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1761, metadata !DIExpression()), !dbg !1760
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1762, metadata !DIExpression()), !dbg !1760
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1763, metadata !DIExpression()), !dbg !1760
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1764, metadata !DIExpression()), !dbg !1760
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1765, metadata !DIExpression()), !dbg !1760
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1766, metadata !DIExpression()), !dbg !1760
  %15 = load ptr, ptr %10, align 4, !dbg !1760
  %16 = load ptr, ptr %15, align 4, !dbg !1760
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1760
  %18 = load ptr, ptr %17, align 4, !dbg !1760
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1760
  %20 = load ptr, ptr %7, align 4, !dbg !1760
  %21 = load ptr, ptr %10, align 4, !dbg !1760
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1760
  store i32 %22, ptr %12, align 4, !dbg !1760
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1767, metadata !DIExpression()), !dbg !1760
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1768, metadata !DIExpression()), !dbg !1770
  store i32 0, ptr %14, align 4, !dbg !1770
  br label %23, !dbg !1770

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1770
  %25 = load i32, ptr %12, align 4, !dbg !1770
  %26 = icmp slt i32 %24, %25, !dbg !1770
  br i1 %26, label %27, label %97, !dbg !1770

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1771
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1771
  %30 = load i8, ptr %29, align 1, !dbg !1771
  %31 = sext i8 %30 to i32, !dbg !1771
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1771

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1774
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1774
  store ptr %34, ptr %6, align 4, !dbg !1774
  %35 = load i32, ptr %33, align 4, !dbg !1774
  %36 = trunc i32 %35 to i8, !dbg !1774
  %37 = load i32, ptr %14, align 4, !dbg !1774
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1774
  store i8 %36, ptr %38, align 8, !dbg !1774
  br label %93, !dbg !1774

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1774
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1774
  store ptr %41, ptr %6, align 4, !dbg !1774
  %42 = load i32, ptr %40, align 4, !dbg !1774
  %43 = trunc i32 %42 to i8, !dbg !1774
  %44 = load i32, ptr %14, align 4, !dbg !1774
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1774
  store i8 %43, ptr %45, align 8, !dbg !1774
  br label %93, !dbg !1774

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1774
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1774
  store ptr %48, ptr %6, align 4, !dbg !1774
  %49 = load i32, ptr %47, align 4, !dbg !1774
  %50 = trunc i32 %49 to i16, !dbg !1774
  %51 = load i32, ptr %14, align 4, !dbg !1774
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1774
  store i16 %50, ptr %52, align 8, !dbg !1774
  br label %93, !dbg !1774

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1774
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1774
  store ptr %55, ptr %6, align 4, !dbg !1774
  %56 = load i32, ptr %54, align 4, !dbg !1774
  %57 = trunc i32 %56 to i16, !dbg !1774
  %58 = load i32, ptr %14, align 4, !dbg !1774
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1774
  store i16 %57, ptr %59, align 8, !dbg !1774
  br label %93, !dbg !1774

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1774
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1774
  store ptr %62, ptr %6, align 4, !dbg !1774
  %63 = load i32, ptr %61, align 4, !dbg !1774
  %64 = load i32, ptr %14, align 4, !dbg !1774
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1774
  store i32 %63, ptr %65, align 8, !dbg !1774
  br label %93, !dbg !1774

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1774
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1774
  store ptr %68, ptr %6, align 4, !dbg !1774
  %69 = load i32, ptr %67, align 4, !dbg !1774
  %70 = sext i32 %69 to i64, !dbg !1774
  %71 = load i32, ptr %14, align 4, !dbg !1774
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1774
  store i64 %70, ptr %72, align 8, !dbg !1774
  br label %93, !dbg !1774

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1774
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1774
  store ptr %75, ptr %6, align 4, !dbg !1774
  %76 = load double, ptr %74, align 4, !dbg !1774
  %77 = fptrunc double %76 to float, !dbg !1774
  %78 = load i32, ptr %14, align 4, !dbg !1774
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1774
  store float %77, ptr %79, align 8, !dbg !1774
  br label %93, !dbg !1774

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1774
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1774
  store ptr %82, ptr %6, align 4, !dbg !1774
  %83 = load double, ptr %81, align 4, !dbg !1774
  %84 = load i32, ptr %14, align 4, !dbg !1774
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1774
  store double %83, ptr %85, align 8, !dbg !1774
  br label %93, !dbg !1774

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1774
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1774
  store ptr %88, ptr %6, align 4, !dbg !1774
  %89 = load ptr, ptr %87, align 4, !dbg !1774
  %90 = load i32, ptr %14, align 4, !dbg !1774
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1774
  store ptr %89, ptr %91, align 8, !dbg !1774
  br label %93, !dbg !1774

92:                                               ; preds = %27
  br label %93, !dbg !1774

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1771

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1776
  %96 = add nsw i32 %95, 1, !dbg !1776
  store i32 %96, ptr %14, align 4, !dbg !1776
  br label %23, !dbg !1776, !llvm.loop !1777

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1760
  %99 = load ptr, ptr %98, align 4, !dbg !1760
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 81, !dbg !1760
  %101 = load ptr, ptr %100, align 4, !dbg !1760
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1760
  %103 = load ptr, ptr %7, align 4, !dbg !1760
  %104 = load ptr, ptr %8, align 4, !dbg !1760
  %105 = load ptr, ptr %9, align 4, !dbg !1760
  %106 = load ptr, ptr %10, align 4, !dbg !1760
  %107 = call x86_stdcallcc i32 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1760
  ret i32 %107, !dbg !1760
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1778 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1779, metadata !DIExpression()), !dbg !1780
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1781, metadata !DIExpression()), !dbg !1780
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1782, metadata !DIExpression()), !dbg !1780
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1783, metadata !DIExpression()), !dbg !1780
  call void @llvm.va_start(ptr %7), !dbg !1780
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1784, metadata !DIExpression()), !dbg !1780
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1785, metadata !DIExpression()), !dbg !1780
  %13 = load ptr, ptr %6, align 4, !dbg !1780
  %14 = load ptr, ptr %13, align 4, !dbg !1780
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1780
  %16 = load ptr, ptr %15, align 4, !dbg !1780
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1780
  %18 = load ptr, ptr %4, align 4, !dbg !1780
  %19 = load ptr, ptr %6, align 4, !dbg !1780
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1780
  store i32 %20, ptr %9, align 4, !dbg !1780
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1786, metadata !DIExpression()), !dbg !1780
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1787, metadata !DIExpression()), !dbg !1789
  store i32 0, ptr %11, align 4, !dbg !1789
  br label %21, !dbg !1789

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1789
  %23 = load i32, ptr %9, align 4, !dbg !1789
  %24 = icmp slt i32 %22, %23, !dbg !1789
  br i1 %24, label %25, label %95, !dbg !1789

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1790
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1790
  %28 = load i8, ptr %27, align 1, !dbg !1790
  %29 = sext i8 %28 to i32, !dbg !1790
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1790

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1793
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1793
  store ptr %32, ptr %7, align 4, !dbg !1793
  %33 = load i32, ptr %31, align 4, !dbg !1793
  %34 = trunc i32 %33 to i8, !dbg !1793
  %35 = load i32, ptr %11, align 4, !dbg !1793
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1793
  store i8 %34, ptr %36, align 8, !dbg !1793
  br label %91, !dbg !1793

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1793
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1793
  store ptr %39, ptr %7, align 4, !dbg !1793
  %40 = load i32, ptr %38, align 4, !dbg !1793
  %41 = trunc i32 %40 to i8, !dbg !1793
  %42 = load i32, ptr %11, align 4, !dbg !1793
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1793
  store i8 %41, ptr %43, align 8, !dbg !1793
  br label %91, !dbg !1793

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1793
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1793
  store ptr %46, ptr %7, align 4, !dbg !1793
  %47 = load i32, ptr %45, align 4, !dbg !1793
  %48 = trunc i32 %47 to i16, !dbg !1793
  %49 = load i32, ptr %11, align 4, !dbg !1793
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1793
  store i16 %48, ptr %50, align 8, !dbg !1793
  br label %91, !dbg !1793

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1793
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1793
  store ptr %53, ptr %7, align 4, !dbg !1793
  %54 = load i32, ptr %52, align 4, !dbg !1793
  %55 = trunc i32 %54 to i16, !dbg !1793
  %56 = load i32, ptr %11, align 4, !dbg !1793
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1793
  store i16 %55, ptr %57, align 8, !dbg !1793
  br label %91, !dbg !1793

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1793
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1793
  store ptr %60, ptr %7, align 4, !dbg !1793
  %61 = load i32, ptr %59, align 4, !dbg !1793
  %62 = load i32, ptr %11, align 4, !dbg !1793
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1793
  store i32 %61, ptr %63, align 8, !dbg !1793
  br label %91, !dbg !1793

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1793
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1793
  store ptr %66, ptr %7, align 4, !dbg !1793
  %67 = load i32, ptr %65, align 4, !dbg !1793
  %68 = sext i32 %67 to i64, !dbg !1793
  %69 = load i32, ptr %11, align 4, !dbg !1793
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1793
  store i64 %68, ptr %70, align 8, !dbg !1793
  br label %91, !dbg !1793

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1793
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1793
  store ptr %73, ptr %7, align 4, !dbg !1793
  %74 = load double, ptr %72, align 4, !dbg !1793
  %75 = fptrunc double %74 to float, !dbg !1793
  %76 = load i32, ptr %11, align 4, !dbg !1793
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1793
  store float %75, ptr %77, align 8, !dbg !1793
  br label %91, !dbg !1793

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1793
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1793
  store ptr %80, ptr %7, align 4, !dbg !1793
  %81 = load double, ptr %79, align 4, !dbg !1793
  %82 = load i32, ptr %11, align 4, !dbg !1793
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1793
  store double %81, ptr %83, align 8, !dbg !1793
  br label %91, !dbg !1793

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1793
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1793
  store ptr %86, ptr %7, align 4, !dbg !1793
  %87 = load ptr, ptr %85, align 4, !dbg !1793
  %88 = load i32, ptr %11, align 4, !dbg !1793
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1793
  store ptr %87, ptr %89, align 8, !dbg !1793
  br label %91, !dbg !1793

90:                                               ; preds = %25
  br label %91, !dbg !1793

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1790

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1795
  %94 = add nsw i32 %93, 1, !dbg !1795
  store i32 %94, ptr %11, align 4, !dbg !1795
  br label %21, !dbg !1795, !llvm.loop !1796

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1797, metadata !DIExpression()), !dbg !1780
  %96 = load ptr, ptr %6, align 4, !dbg !1780
  %97 = load ptr, ptr %96, align 4, !dbg !1780
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 131, !dbg !1780
  %99 = load ptr, ptr %98, align 4, !dbg !1780
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1780
  %101 = load ptr, ptr %4, align 4, !dbg !1780
  %102 = load ptr, ptr %5, align 4, !dbg !1780
  %103 = load ptr, ptr %6, align 4, !dbg !1780
  %104 = call x86_stdcallcc i32 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1780
  store i32 %104, ptr %12, align 4, !dbg !1780
  call void @llvm.va_end(ptr %7), !dbg !1780
  %105 = load i32, ptr %12, align 4, !dbg !1780
  ret i32 %105, !dbg !1780
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i32 @"\01_JNI_CallStaticIntMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1798 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1799, metadata !DIExpression()), !dbg !1800
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1801, metadata !DIExpression()), !dbg !1800
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1802, metadata !DIExpression()), !dbg !1800
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1803, metadata !DIExpression()), !dbg !1800
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1804, metadata !DIExpression()), !dbg !1800
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1805, metadata !DIExpression()), !dbg !1800
  %13 = load ptr, ptr %8, align 4, !dbg !1800
  %14 = load ptr, ptr %13, align 4, !dbg !1800
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1800
  %16 = load ptr, ptr %15, align 4, !dbg !1800
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1800
  %18 = load ptr, ptr %6, align 4, !dbg !1800
  %19 = load ptr, ptr %8, align 4, !dbg !1800
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1800
  store i32 %20, ptr %10, align 4, !dbg !1800
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1806, metadata !DIExpression()), !dbg !1800
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1807, metadata !DIExpression()), !dbg !1809
  store i32 0, ptr %12, align 4, !dbg !1809
  br label %21, !dbg !1809

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1809
  %23 = load i32, ptr %10, align 4, !dbg !1809
  %24 = icmp slt i32 %22, %23, !dbg !1809
  br i1 %24, label %25, label %95, !dbg !1809

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1810
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1810
  %28 = load i8, ptr %27, align 1, !dbg !1810
  %29 = sext i8 %28 to i32, !dbg !1810
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1810

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1813
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1813
  store ptr %32, ptr %5, align 4, !dbg !1813
  %33 = load i32, ptr %31, align 4, !dbg !1813
  %34 = trunc i32 %33 to i8, !dbg !1813
  %35 = load i32, ptr %12, align 4, !dbg !1813
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1813
  store i8 %34, ptr %36, align 8, !dbg !1813
  br label %91, !dbg !1813

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1813
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1813
  store ptr %39, ptr %5, align 4, !dbg !1813
  %40 = load i32, ptr %38, align 4, !dbg !1813
  %41 = trunc i32 %40 to i8, !dbg !1813
  %42 = load i32, ptr %12, align 4, !dbg !1813
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1813
  store i8 %41, ptr %43, align 8, !dbg !1813
  br label %91, !dbg !1813

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1813
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1813
  store ptr %46, ptr %5, align 4, !dbg !1813
  %47 = load i32, ptr %45, align 4, !dbg !1813
  %48 = trunc i32 %47 to i16, !dbg !1813
  %49 = load i32, ptr %12, align 4, !dbg !1813
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1813
  store i16 %48, ptr %50, align 8, !dbg !1813
  br label %91, !dbg !1813

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1813
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1813
  store ptr %53, ptr %5, align 4, !dbg !1813
  %54 = load i32, ptr %52, align 4, !dbg !1813
  %55 = trunc i32 %54 to i16, !dbg !1813
  %56 = load i32, ptr %12, align 4, !dbg !1813
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1813
  store i16 %55, ptr %57, align 8, !dbg !1813
  br label %91, !dbg !1813

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1813
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1813
  store ptr %60, ptr %5, align 4, !dbg !1813
  %61 = load i32, ptr %59, align 4, !dbg !1813
  %62 = load i32, ptr %12, align 4, !dbg !1813
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1813
  store i32 %61, ptr %63, align 8, !dbg !1813
  br label %91, !dbg !1813

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1813
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1813
  store ptr %66, ptr %5, align 4, !dbg !1813
  %67 = load i32, ptr %65, align 4, !dbg !1813
  %68 = sext i32 %67 to i64, !dbg !1813
  %69 = load i32, ptr %12, align 4, !dbg !1813
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1813
  store i64 %68, ptr %70, align 8, !dbg !1813
  br label %91, !dbg !1813

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1813
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1813
  store ptr %73, ptr %5, align 4, !dbg !1813
  %74 = load double, ptr %72, align 4, !dbg !1813
  %75 = fptrunc double %74 to float, !dbg !1813
  %76 = load i32, ptr %12, align 4, !dbg !1813
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1813
  store float %75, ptr %77, align 8, !dbg !1813
  br label %91, !dbg !1813

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1813
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1813
  store ptr %80, ptr %5, align 4, !dbg !1813
  %81 = load double, ptr %79, align 4, !dbg !1813
  %82 = load i32, ptr %12, align 4, !dbg !1813
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1813
  store double %81, ptr %83, align 8, !dbg !1813
  br label %91, !dbg !1813

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1813
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1813
  store ptr %86, ptr %5, align 4, !dbg !1813
  %87 = load ptr, ptr %85, align 4, !dbg !1813
  %88 = load i32, ptr %12, align 4, !dbg !1813
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1813
  store ptr %87, ptr %89, align 8, !dbg !1813
  br label %91, !dbg !1813

90:                                               ; preds = %25
  br label %91, !dbg !1813

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1810

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1815
  %94 = add nsw i32 %93, 1, !dbg !1815
  store i32 %94, ptr %12, align 4, !dbg !1815
  br label %21, !dbg !1815, !llvm.loop !1816

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1800
  %97 = load ptr, ptr %96, align 4, !dbg !1800
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 131, !dbg !1800
  %99 = load ptr, ptr %98, align 4, !dbg !1800
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1800
  %101 = load ptr, ptr %6, align 4, !dbg !1800
  %102 = load ptr, ptr %7, align 4, !dbg !1800
  %103 = load ptr, ptr %8, align 4, !dbg !1800
  %104 = call x86_stdcallcc i32 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1800
  ret i32 %104, !dbg !1800
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1817 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1818, metadata !DIExpression()), !dbg !1819
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1820, metadata !DIExpression()), !dbg !1819
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1821, metadata !DIExpression()), !dbg !1819
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1822, metadata !DIExpression()), !dbg !1819
  call void @llvm.va_start(ptr %7), !dbg !1819
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1823, metadata !DIExpression()), !dbg !1819
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1824, metadata !DIExpression()), !dbg !1819
  %13 = load ptr, ptr %6, align 4, !dbg !1819
  %14 = load ptr, ptr %13, align 4, !dbg !1819
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1819
  %16 = load ptr, ptr %15, align 4, !dbg !1819
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1819
  %18 = load ptr, ptr %4, align 4, !dbg !1819
  %19 = load ptr, ptr %6, align 4, !dbg !1819
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1819
  store i32 %20, ptr %9, align 4, !dbg !1819
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1825, metadata !DIExpression()), !dbg !1819
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1826, metadata !DIExpression()), !dbg !1828
  store i32 0, ptr %11, align 4, !dbg !1828
  br label %21, !dbg !1828

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1828
  %23 = load i32, ptr %9, align 4, !dbg !1828
  %24 = icmp slt i32 %22, %23, !dbg !1828
  br i1 %24, label %25, label %95, !dbg !1828

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1829
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1829
  %28 = load i8, ptr %27, align 1, !dbg !1829
  %29 = sext i8 %28 to i32, !dbg !1829
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1829

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1832
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1832
  store ptr %32, ptr %7, align 4, !dbg !1832
  %33 = load i32, ptr %31, align 4, !dbg !1832
  %34 = trunc i32 %33 to i8, !dbg !1832
  %35 = load i32, ptr %11, align 4, !dbg !1832
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1832
  store i8 %34, ptr %36, align 8, !dbg !1832
  br label %91, !dbg !1832

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1832
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1832
  store ptr %39, ptr %7, align 4, !dbg !1832
  %40 = load i32, ptr %38, align 4, !dbg !1832
  %41 = trunc i32 %40 to i8, !dbg !1832
  %42 = load i32, ptr %11, align 4, !dbg !1832
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1832
  store i8 %41, ptr %43, align 8, !dbg !1832
  br label %91, !dbg !1832

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1832
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1832
  store ptr %46, ptr %7, align 4, !dbg !1832
  %47 = load i32, ptr %45, align 4, !dbg !1832
  %48 = trunc i32 %47 to i16, !dbg !1832
  %49 = load i32, ptr %11, align 4, !dbg !1832
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1832
  store i16 %48, ptr %50, align 8, !dbg !1832
  br label %91, !dbg !1832

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1832
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1832
  store ptr %53, ptr %7, align 4, !dbg !1832
  %54 = load i32, ptr %52, align 4, !dbg !1832
  %55 = trunc i32 %54 to i16, !dbg !1832
  %56 = load i32, ptr %11, align 4, !dbg !1832
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1832
  store i16 %55, ptr %57, align 8, !dbg !1832
  br label %91, !dbg !1832

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1832
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1832
  store ptr %60, ptr %7, align 4, !dbg !1832
  %61 = load i32, ptr %59, align 4, !dbg !1832
  %62 = load i32, ptr %11, align 4, !dbg !1832
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1832
  store i32 %61, ptr %63, align 8, !dbg !1832
  br label %91, !dbg !1832

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1832
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1832
  store ptr %66, ptr %7, align 4, !dbg !1832
  %67 = load i32, ptr %65, align 4, !dbg !1832
  %68 = sext i32 %67 to i64, !dbg !1832
  %69 = load i32, ptr %11, align 4, !dbg !1832
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1832
  store i64 %68, ptr %70, align 8, !dbg !1832
  br label %91, !dbg !1832

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1832
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1832
  store ptr %73, ptr %7, align 4, !dbg !1832
  %74 = load double, ptr %72, align 4, !dbg !1832
  %75 = fptrunc double %74 to float, !dbg !1832
  %76 = load i32, ptr %11, align 4, !dbg !1832
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1832
  store float %75, ptr %77, align 8, !dbg !1832
  br label %91, !dbg !1832

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1832
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1832
  store ptr %80, ptr %7, align 4, !dbg !1832
  %81 = load double, ptr %79, align 4, !dbg !1832
  %82 = load i32, ptr %11, align 4, !dbg !1832
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1832
  store double %81, ptr %83, align 8, !dbg !1832
  br label %91, !dbg !1832

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1832
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1832
  store ptr %86, ptr %7, align 4, !dbg !1832
  %87 = load ptr, ptr %85, align 4, !dbg !1832
  %88 = load i32, ptr %11, align 4, !dbg !1832
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1832
  store ptr %87, ptr %89, align 8, !dbg !1832
  br label %91, !dbg !1832

90:                                               ; preds = %25
  br label %91, !dbg !1832

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1829

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1834
  %94 = add nsw i32 %93, 1, !dbg !1834
  store i32 %94, ptr %11, align 4, !dbg !1834
  br label %21, !dbg !1834, !llvm.loop !1835

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1836, metadata !DIExpression()), !dbg !1819
  %96 = load ptr, ptr %6, align 4, !dbg !1819
  %97 = load ptr, ptr %96, align 4, !dbg !1819
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 54, !dbg !1819
  %99 = load ptr, ptr %98, align 4, !dbg !1819
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1819
  %101 = load ptr, ptr %4, align 4, !dbg !1819
  %102 = load ptr, ptr %5, align 4, !dbg !1819
  %103 = load ptr, ptr %6, align 4, !dbg !1819
  %104 = call x86_stdcallcc i64 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1819
  store i64 %104, ptr %12, align 8, !dbg !1819
  call void @llvm.va_end(ptr %7), !dbg !1819
  %105 = load i64, ptr %12, align 8, !dbg !1819
  ret i64 %105, !dbg !1819
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i64 @"\01_JNI_CallLongMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1837 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1838, metadata !DIExpression()), !dbg !1839
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1840, metadata !DIExpression()), !dbg !1839
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1841, metadata !DIExpression()), !dbg !1839
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1842, metadata !DIExpression()), !dbg !1839
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1843, metadata !DIExpression()), !dbg !1839
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1844, metadata !DIExpression()), !dbg !1839
  %13 = load ptr, ptr %8, align 4, !dbg !1839
  %14 = load ptr, ptr %13, align 4, !dbg !1839
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1839
  %16 = load ptr, ptr %15, align 4, !dbg !1839
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1839
  %18 = load ptr, ptr %6, align 4, !dbg !1839
  %19 = load ptr, ptr %8, align 4, !dbg !1839
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1839
  store i32 %20, ptr %10, align 4, !dbg !1839
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1845, metadata !DIExpression()), !dbg !1839
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1846, metadata !DIExpression()), !dbg !1848
  store i32 0, ptr %12, align 4, !dbg !1848
  br label %21, !dbg !1848

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1848
  %23 = load i32, ptr %10, align 4, !dbg !1848
  %24 = icmp slt i32 %22, %23, !dbg !1848
  br i1 %24, label %25, label %95, !dbg !1848

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1849
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1849
  %28 = load i8, ptr %27, align 1, !dbg !1849
  %29 = sext i8 %28 to i32, !dbg !1849
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1849

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1852
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1852
  store ptr %32, ptr %5, align 4, !dbg !1852
  %33 = load i32, ptr %31, align 4, !dbg !1852
  %34 = trunc i32 %33 to i8, !dbg !1852
  %35 = load i32, ptr %12, align 4, !dbg !1852
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1852
  store i8 %34, ptr %36, align 8, !dbg !1852
  br label %91, !dbg !1852

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1852
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1852
  store ptr %39, ptr %5, align 4, !dbg !1852
  %40 = load i32, ptr %38, align 4, !dbg !1852
  %41 = trunc i32 %40 to i8, !dbg !1852
  %42 = load i32, ptr %12, align 4, !dbg !1852
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1852
  store i8 %41, ptr %43, align 8, !dbg !1852
  br label %91, !dbg !1852

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1852
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1852
  store ptr %46, ptr %5, align 4, !dbg !1852
  %47 = load i32, ptr %45, align 4, !dbg !1852
  %48 = trunc i32 %47 to i16, !dbg !1852
  %49 = load i32, ptr %12, align 4, !dbg !1852
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1852
  store i16 %48, ptr %50, align 8, !dbg !1852
  br label %91, !dbg !1852

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1852
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1852
  store ptr %53, ptr %5, align 4, !dbg !1852
  %54 = load i32, ptr %52, align 4, !dbg !1852
  %55 = trunc i32 %54 to i16, !dbg !1852
  %56 = load i32, ptr %12, align 4, !dbg !1852
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1852
  store i16 %55, ptr %57, align 8, !dbg !1852
  br label %91, !dbg !1852

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1852
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1852
  store ptr %60, ptr %5, align 4, !dbg !1852
  %61 = load i32, ptr %59, align 4, !dbg !1852
  %62 = load i32, ptr %12, align 4, !dbg !1852
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1852
  store i32 %61, ptr %63, align 8, !dbg !1852
  br label %91, !dbg !1852

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1852
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1852
  store ptr %66, ptr %5, align 4, !dbg !1852
  %67 = load i32, ptr %65, align 4, !dbg !1852
  %68 = sext i32 %67 to i64, !dbg !1852
  %69 = load i32, ptr %12, align 4, !dbg !1852
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1852
  store i64 %68, ptr %70, align 8, !dbg !1852
  br label %91, !dbg !1852

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1852
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1852
  store ptr %73, ptr %5, align 4, !dbg !1852
  %74 = load double, ptr %72, align 4, !dbg !1852
  %75 = fptrunc double %74 to float, !dbg !1852
  %76 = load i32, ptr %12, align 4, !dbg !1852
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1852
  store float %75, ptr %77, align 8, !dbg !1852
  br label %91, !dbg !1852

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1852
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1852
  store ptr %80, ptr %5, align 4, !dbg !1852
  %81 = load double, ptr %79, align 4, !dbg !1852
  %82 = load i32, ptr %12, align 4, !dbg !1852
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1852
  store double %81, ptr %83, align 8, !dbg !1852
  br label %91, !dbg !1852

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1852
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1852
  store ptr %86, ptr %5, align 4, !dbg !1852
  %87 = load ptr, ptr %85, align 4, !dbg !1852
  %88 = load i32, ptr %12, align 4, !dbg !1852
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1852
  store ptr %87, ptr %89, align 8, !dbg !1852
  br label %91, !dbg !1852

90:                                               ; preds = %25
  br label %91, !dbg !1852

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1849

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1854
  %94 = add nsw i32 %93, 1, !dbg !1854
  store i32 %94, ptr %12, align 4, !dbg !1854
  br label %21, !dbg !1854, !llvm.loop !1855

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1839
  %97 = load ptr, ptr %96, align 4, !dbg !1839
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 54, !dbg !1839
  %99 = load ptr, ptr %98, align 4, !dbg !1839
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1839
  %101 = load ptr, ptr %6, align 4, !dbg !1839
  %102 = load ptr, ptr %7, align 4, !dbg !1839
  %103 = load ptr, ptr %8, align 4, !dbg !1839
  %104 = call x86_stdcallcc i64 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1839
  ret i64 %104, !dbg !1839
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1856 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1857, metadata !DIExpression()), !dbg !1858
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1859, metadata !DIExpression()), !dbg !1858
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1860, metadata !DIExpression()), !dbg !1858
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1861, metadata !DIExpression()), !dbg !1858
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1862, metadata !DIExpression()), !dbg !1858
  call void @llvm.va_start(ptr %9), !dbg !1858
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1863, metadata !DIExpression()), !dbg !1858
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1864, metadata !DIExpression()), !dbg !1858
  %15 = load ptr, ptr %8, align 4, !dbg !1858
  %16 = load ptr, ptr %15, align 4, !dbg !1858
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1858
  %18 = load ptr, ptr %17, align 4, !dbg !1858
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1858
  %20 = load ptr, ptr %5, align 4, !dbg !1858
  %21 = load ptr, ptr %8, align 4, !dbg !1858
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1858
  store i32 %22, ptr %11, align 4, !dbg !1858
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1865, metadata !DIExpression()), !dbg !1858
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1866, metadata !DIExpression()), !dbg !1868
  store i32 0, ptr %13, align 4, !dbg !1868
  br label %23, !dbg !1868

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1868
  %25 = load i32, ptr %11, align 4, !dbg !1868
  %26 = icmp slt i32 %24, %25, !dbg !1868
  br i1 %26, label %27, label %97, !dbg !1868

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1869
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1869
  %30 = load i8, ptr %29, align 1, !dbg !1869
  %31 = sext i8 %30 to i32, !dbg !1869
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1869

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1872
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1872
  store ptr %34, ptr %9, align 4, !dbg !1872
  %35 = load i32, ptr %33, align 4, !dbg !1872
  %36 = trunc i32 %35 to i8, !dbg !1872
  %37 = load i32, ptr %13, align 4, !dbg !1872
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1872
  store i8 %36, ptr %38, align 8, !dbg !1872
  br label %93, !dbg !1872

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1872
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1872
  store ptr %41, ptr %9, align 4, !dbg !1872
  %42 = load i32, ptr %40, align 4, !dbg !1872
  %43 = trunc i32 %42 to i8, !dbg !1872
  %44 = load i32, ptr %13, align 4, !dbg !1872
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1872
  store i8 %43, ptr %45, align 8, !dbg !1872
  br label %93, !dbg !1872

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1872
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1872
  store ptr %48, ptr %9, align 4, !dbg !1872
  %49 = load i32, ptr %47, align 4, !dbg !1872
  %50 = trunc i32 %49 to i16, !dbg !1872
  %51 = load i32, ptr %13, align 4, !dbg !1872
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1872
  store i16 %50, ptr %52, align 8, !dbg !1872
  br label %93, !dbg !1872

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1872
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1872
  store ptr %55, ptr %9, align 4, !dbg !1872
  %56 = load i32, ptr %54, align 4, !dbg !1872
  %57 = trunc i32 %56 to i16, !dbg !1872
  %58 = load i32, ptr %13, align 4, !dbg !1872
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1872
  store i16 %57, ptr %59, align 8, !dbg !1872
  br label %93, !dbg !1872

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1872
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1872
  store ptr %62, ptr %9, align 4, !dbg !1872
  %63 = load i32, ptr %61, align 4, !dbg !1872
  %64 = load i32, ptr %13, align 4, !dbg !1872
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1872
  store i32 %63, ptr %65, align 8, !dbg !1872
  br label %93, !dbg !1872

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1872
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1872
  store ptr %68, ptr %9, align 4, !dbg !1872
  %69 = load i32, ptr %67, align 4, !dbg !1872
  %70 = sext i32 %69 to i64, !dbg !1872
  %71 = load i32, ptr %13, align 4, !dbg !1872
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1872
  store i64 %70, ptr %72, align 8, !dbg !1872
  br label %93, !dbg !1872

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1872
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1872
  store ptr %75, ptr %9, align 4, !dbg !1872
  %76 = load double, ptr %74, align 4, !dbg !1872
  %77 = fptrunc double %76 to float, !dbg !1872
  %78 = load i32, ptr %13, align 4, !dbg !1872
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1872
  store float %77, ptr %79, align 8, !dbg !1872
  br label %93, !dbg !1872

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1872
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1872
  store ptr %82, ptr %9, align 4, !dbg !1872
  %83 = load double, ptr %81, align 4, !dbg !1872
  %84 = load i32, ptr %13, align 4, !dbg !1872
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1872
  store double %83, ptr %85, align 8, !dbg !1872
  br label %93, !dbg !1872

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1872
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1872
  store ptr %88, ptr %9, align 4, !dbg !1872
  %89 = load ptr, ptr %87, align 4, !dbg !1872
  %90 = load i32, ptr %13, align 4, !dbg !1872
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1872
  store ptr %89, ptr %91, align 8, !dbg !1872
  br label %93, !dbg !1872

92:                                               ; preds = %27
  br label %93, !dbg !1872

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1869

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1874
  %96 = add nsw i32 %95, 1, !dbg !1874
  store i32 %96, ptr %13, align 4, !dbg !1874
  br label %23, !dbg !1874, !llvm.loop !1875

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1876, metadata !DIExpression()), !dbg !1858
  %98 = load ptr, ptr %8, align 4, !dbg !1858
  %99 = load ptr, ptr %98, align 4, !dbg !1858
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 84, !dbg !1858
  %101 = load ptr, ptr %100, align 4, !dbg !1858
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1858
  %103 = load ptr, ptr %5, align 4, !dbg !1858
  %104 = load ptr, ptr %6, align 4, !dbg !1858
  %105 = load ptr, ptr %7, align 4, !dbg !1858
  %106 = load ptr, ptr %8, align 4, !dbg !1858
  %107 = call x86_stdcallcc i64 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1858
  store i64 %107, ptr %14, align 8, !dbg !1858
  call void @llvm.va_end(ptr %9), !dbg !1858
  %108 = load i64, ptr %14, align 8, !dbg !1858
  ret i64 %108, !dbg !1858
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i64 @"\01_JNI_CallNonvirtualLongMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1877 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1878, metadata !DIExpression()), !dbg !1879
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1880, metadata !DIExpression()), !dbg !1879
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1881, metadata !DIExpression()), !dbg !1879
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1882, metadata !DIExpression()), !dbg !1879
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1883, metadata !DIExpression()), !dbg !1879
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1884, metadata !DIExpression()), !dbg !1879
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1885, metadata !DIExpression()), !dbg !1879
  %15 = load ptr, ptr %10, align 4, !dbg !1879
  %16 = load ptr, ptr %15, align 4, !dbg !1879
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1879
  %18 = load ptr, ptr %17, align 4, !dbg !1879
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1879
  %20 = load ptr, ptr %7, align 4, !dbg !1879
  %21 = load ptr, ptr %10, align 4, !dbg !1879
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1879
  store i32 %22, ptr %12, align 4, !dbg !1879
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1886, metadata !DIExpression()), !dbg !1879
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1887, metadata !DIExpression()), !dbg !1889
  store i32 0, ptr %14, align 4, !dbg !1889
  br label %23, !dbg !1889

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !1889
  %25 = load i32, ptr %12, align 4, !dbg !1889
  %26 = icmp slt i32 %24, %25, !dbg !1889
  br i1 %26, label %27, label %97, !dbg !1889

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1890
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !1890
  %30 = load i8, ptr %29, align 1, !dbg !1890
  %31 = sext i8 %30 to i32, !dbg !1890
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1890

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !1893
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1893
  store ptr %34, ptr %6, align 4, !dbg !1893
  %35 = load i32, ptr %33, align 4, !dbg !1893
  %36 = trunc i32 %35 to i8, !dbg !1893
  %37 = load i32, ptr %14, align 4, !dbg !1893
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !1893
  store i8 %36, ptr %38, align 8, !dbg !1893
  br label %93, !dbg !1893

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !1893
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1893
  store ptr %41, ptr %6, align 4, !dbg !1893
  %42 = load i32, ptr %40, align 4, !dbg !1893
  %43 = trunc i32 %42 to i8, !dbg !1893
  %44 = load i32, ptr %14, align 4, !dbg !1893
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !1893
  store i8 %43, ptr %45, align 8, !dbg !1893
  br label %93, !dbg !1893

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !1893
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1893
  store ptr %48, ptr %6, align 4, !dbg !1893
  %49 = load i32, ptr %47, align 4, !dbg !1893
  %50 = trunc i32 %49 to i16, !dbg !1893
  %51 = load i32, ptr %14, align 4, !dbg !1893
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !1893
  store i16 %50, ptr %52, align 8, !dbg !1893
  br label %93, !dbg !1893

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !1893
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1893
  store ptr %55, ptr %6, align 4, !dbg !1893
  %56 = load i32, ptr %54, align 4, !dbg !1893
  %57 = trunc i32 %56 to i16, !dbg !1893
  %58 = load i32, ptr %14, align 4, !dbg !1893
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !1893
  store i16 %57, ptr %59, align 8, !dbg !1893
  br label %93, !dbg !1893

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !1893
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1893
  store ptr %62, ptr %6, align 4, !dbg !1893
  %63 = load i32, ptr %61, align 4, !dbg !1893
  %64 = load i32, ptr %14, align 4, !dbg !1893
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !1893
  store i32 %63, ptr %65, align 8, !dbg !1893
  br label %93, !dbg !1893

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !1893
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1893
  store ptr %68, ptr %6, align 4, !dbg !1893
  %69 = load i32, ptr %67, align 4, !dbg !1893
  %70 = sext i32 %69 to i64, !dbg !1893
  %71 = load i32, ptr %14, align 4, !dbg !1893
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !1893
  store i64 %70, ptr %72, align 8, !dbg !1893
  br label %93, !dbg !1893

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !1893
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1893
  store ptr %75, ptr %6, align 4, !dbg !1893
  %76 = load double, ptr %74, align 4, !dbg !1893
  %77 = fptrunc double %76 to float, !dbg !1893
  %78 = load i32, ptr %14, align 4, !dbg !1893
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !1893
  store float %77, ptr %79, align 8, !dbg !1893
  br label %93, !dbg !1893

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !1893
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1893
  store ptr %82, ptr %6, align 4, !dbg !1893
  %83 = load double, ptr %81, align 4, !dbg !1893
  %84 = load i32, ptr %14, align 4, !dbg !1893
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !1893
  store double %83, ptr %85, align 8, !dbg !1893
  br label %93, !dbg !1893

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !1893
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1893
  store ptr %88, ptr %6, align 4, !dbg !1893
  %89 = load ptr, ptr %87, align 4, !dbg !1893
  %90 = load i32, ptr %14, align 4, !dbg !1893
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !1893
  store ptr %89, ptr %91, align 8, !dbg !1893
  br label %93, !dbg !1893

92:                                               ; preds = %27
  br label %93, !dbg !1893

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1890

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !1895
  %96 = add nsw i32 %95, 1, !dbg !1895
  store i32 %96, ptr %14, align 4, !dbg !1895
  br label %23, !dbg !1895, !llvm.loop !1896

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1879
  %99 = load ptr, ptr %98, align 4, !dbg !1879
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 84, !dbg !1879
  %101 = load ptr, ptr %100, align 4, !dbg !1879
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1879
  %103 = load ptr, ptr %7, align 4, !dbg !1879
  %104 = load ptr, ptr %8, align 4, !dbg !1879
  %105 = load ptr, ptr %9, align 4, !dbg !1879
  %106 = load ptr, ptr %10, align 4, !dbg !1879
  %107 = call x86_stdcallcc i64 %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1879
  ret i64 %107, !dbg !1879
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1897 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1898, metadata !DIExpression()), !dbg !1899
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1900, metadata !DIExpression()), !dbg !1899
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1901, metadata !DIExpression()), !dbg !1899
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1902, metadata !DIExpression()), !dbg !1899
  call void @llvm.va_start(ptr %7), !dbg !1899
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1903, metadata !DIExpression()), !dbg !1899
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1904, metadata !DIExpression()), !dbg !1899
  %13 = load ptr, ptr %6, align 4, !dbg !1899
  %14 = load ptr, ptr %13, align 4, !dbg !1899
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1899
  %16 = load ptr, ptr %15, align 4, !dbg !1899
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1899
  %18 = load ptr, ptr %4, align 4, !dbg !1899
  %19 = load ptr, ptr %6, align 4, !dbg !1899
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1899
  store i32 %20, ptr %9, align 4, !dbg !1899
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1905, metadata !DIExpression()), !dbg !1899
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1906, metadata !DIExpression()), !dbg !1908
  store i32 0, ptr %11, align 4, !dbg !1908
  br label %21, !dbg !1908

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1908
  %23 = load i32, ptr %9, align 4, !dbg !1908
  %24 = icmp slt i32 %22, %23, !dbg !1908
  br i1 %24, label %25, label %95, !dbg !1908

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1909
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1909
  %28 = load i8, ptr %27, align 1, !dbg !1909
  %29 = sext i8 %28 to i32, !dbg !1909
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1909

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1912
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1912
  store ptr %32, ptr %7, align 4, !dbg !1912
  %33 = load i32, ptr %31, align 4, !dbg !1912
  %34 = trunc i32 %33 to i8, !dbg !1912
  %35 = load i32, ptr %11, align 4, !dbg !1912
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1912
  store i8 %34, ptr %36, align 8, !dbg !1912
  br label %91, !dbg !1912

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1912
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1912
  store ptr %39, ptr %7, align 4, !dbg !1912
  %40 = load i32, ptr %38, align 4, !dbg !1912
  %41 = trunc i32 %40 to i8, !dbg !1912
  %42 = load i32, ptr %11, align 4, !dbg !1912
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1912
  store i8 %41, ptr %43, align 8, !dbg !1912
  br label %91, !dbg !1912

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1912
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1912
  store ptr %46, ptr %7, align 4, !dbg !1912
  %47 = load i32, ptr %45, align 4, !dbg !1912
  %48 = trunc i32 %47 to i16, !dbg !1912
  %49 = load i32, ptr %11, align 4, !dbg !1912
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1912
  store i16 %48, ptr %50, align 8, !dbg !1912
  br label %91, !dbg !1912

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1912
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1912
  store ptr %53, ptr %7, align 4, !dbg !1912
  %54 = load i32, ptr %52, align 4, !dbg !1912
  %55 = trunc i32 %54 to i16, !dbg !1912
  %56 = load i32, ptr %11, align 4, !dbg !1912
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1912
  store i16 %55, ptr %57, align 8, !dbg !1912
  br label %91, !dbg !1912

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1912
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1912
  store ptr %60, ptr %7, align 4, !dbg !1912
  %61 = load i32, ptr %59, align 4, !dbg !1912
  %62 = load i32, ptr %11, align 4, !dbg !1912
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1912
  store i32 %61, ptr %63, align 8, !dbg !1912
  br label %91, !dbg !1912

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1912
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1912
  store ptr %66, ptr %7, align 4, !dbg !1912
  %67 = load i32, ptr %65, align 4, !dbg !1912
  %68 = sext i32 %67 to i64, !dbg !1912
  %69 = load i32, ptr %11, align 4, !dbg !1912
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1912
  store i64 %68, ptr %70, align 8, !dbg !1912
  br label %91, !dbg !1912

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1912
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1912
  store ptr %73, ptr %7, align 4, !dbg !1912
  %74 = load double, ptr %72, align 4, !dbg !1912
  %75 = fptrunc double %74 to float, !dbg !1912
  %76 = load i32, ptr %11, align 4, !dbg !1912
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1912
  store float %75, ptr %77, align 8, !dbg !1912
  br label %91, !dbg !1912

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1912
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1912
  store ptr %80, ptr %7, align 4, !dbg !1912
  %81 = load double, ptr %79, align 4, !dbg !1912
  %82 = load i32, ptr %11, align 4, !dbg !1912
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1912
  store double %81, ptr %83, align 8, !dbg !1912
  br label %91, !dbg !1912

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1912
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1912
  store ptr %86, ptr %7, align 4, !dbg !1912
  %87 = load ptr, ptr %85, align 4, !dbg !1912
  %88 = load i32, ptr %11, align 4, !dbg !1912
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1912
  store ptr %87, ptr %89, align 8, !dbg !1912
  br label %91, !dbg !1912

90:                                               ; preds = %25
  br label %91, !dbg !1912

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1909

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1914
  %94 = add nsw i32 %93, 1, !dbg !1914
  store i32 %94, ptr %11, align 4, !dbg !1914
  br label %21, !dbg !1914, !llvm.loop !1915

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1916, metadata !DIExpression()), !dbg !1899
  %96 = load ptr, ptr %6, align 4, !dbg !1899
  %97 = load ptr, ptr %96, align 4, !dbg !1899
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 134, !dbg !1899
  %99 = load ptr, ptr %98, align 4, !dbg !1899
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1899
  %101 = load ptr, ptr %4, align 4, !dbg !1899
  %102 = load ptr, ptr %5, align 4, !dbg !1899
  %103 = load ptr, ptr %6, align 4, !dbg !1899
  %104 = call x86_stdcallcc i64 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1899
  store i64 %104, ptr %12, align 8, !dbg !1899
  call void @llvm.va_end(ptr %7), !dbg !1899
  %105 = load i64, ptr %12, align 8, !dbg !1899
  ret i64 %105, !dbg !1899
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc i64 @"\01_JNI_CallStaticLongMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1917 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1918, metadata !DIExpression()), !dbg !1919
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1920, metadata !DIExpression()), !dbg !1919
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1921, metadata !DIExpression()), !dbg !1919
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1922, metadata !DIExpression()), !dbg !1919
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1923, metadata !DIExpression()), !dbg !1919
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1924, metadata !DIExpression()), !dbg !1919
  %13 = load ptr, ptr %8, align 4, !dbg !1919
  %14 = load ptr, ptr %13, align 4, !dbg !1919
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1919
  %16 = load ptr, ptr %15, align 4, !dbg !1919
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1919
  %18 = load ptr, ptr %6, align 4, !dbg !1919
  %19 = load ptr, ptr %8, align 4, !dbg !1919
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1919
  store i32 %20, ptr %10, align 4, !dbg !1919
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1925, metadata !DIExpression()), !dbg !1919
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1926, metadata !DIExpression()), !dbg !1928
  store i32 0, ptr %12, align 4, !dbg !1928
  br label %21, !dbg !1928

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1928
  %23 = load i32, ptr %10, align 4, !dbg !1928
  %24 = icmp slt i32 %22, %23, !dbg !1928
  br i1 %24, label %25, label %95, !dbg !1928

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1929
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1929
  %28 = load i8, ptr %27, align 1, !dbg !1929
  %29 = sext i8 %28 to i32, !dbg !1929
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1929

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1932
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1932
  store ptr %32, ptr %5, align 4, !dbg !1932
  %33 = load i32, ptr %31, align 4, !dbg !1932
  %34 = trunc i32 %33 to i8, !dbg !1932
  %35 = load i32, ptr %12, align 4, !dbg !1932
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1932
  store i8 %34, ptr %36, align 8, !dbg !1932
  br label %91, !dbg !1932

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1932
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1932
  store ptr %39, ptr %5, align 4, !dbg !1932
  %40 = load i32, ptr %38, align 4, !dbg !1932
  %41 = trunc i32 %40 to i8, !dbg !1932
  %42 = load i32, ptr %12, align 4, !dbg !1932
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1932
  store i8 %41, ptr %43, align 8, !dbg !1932
  br label %91, !dbg !1932

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1932
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1932
  store ptr %46, ptr %5, align 4, !dbg !1932
  %47 = load i32, ptr %45, align 4, !dbg !1932
  %48 = trunc i32 %47 to i16, !dbg !1932
  %49 = load i32, ptr %12, align 4, !dbg !1932
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1932
  store i16 %48, ptr %50, align 8, !dbg !1932
  br label %91, !dbg !1932

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1932
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1932
  store ptr %53, ptr %5, align 4, !dbg !1932
  %54 = load i32, ptr %52, align 4, !dbg !1932
  %55 = trunc i32 %54 to i16, !dbg !1932
  %56 = load i32, ptr %12, align 4, !dbg !1932
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1932
  store i16 %55, ptr %57, align 8, !dbg !1932
  br label %91, !dbg !1932

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1932
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1932
  store ptr %60, ptr %5, align 4, !dbg !1932
  %61 = load i32, ptr %59, align 4, !dbg !1932
  %62 = load i32, ptr %12, align 4, !dbg !1932
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1932
  store i32 %61, ptr %63, align 8, !dbg !1932
  br label %91, !dbg !1932

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1932
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1932
  store ptr %66, ptr %5, align 4, !dbg !1932
  %67 = load i32, ptr %65, align 4, !dbg !1932
  %68 = sext i32 %67 to i64, !dbg !1932
  %69 = load i32, ptr %12, align 4, !dbg !1932
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1932
  store i64 %68, ptr %70, align 8, !dbg !1932
  br label %91, !dbg !1932

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1932
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1932
  store ptr %73, ptr %5, align 4, !dbg !1932
  %74 = load double, ptr %72, align 4, !dbg !1932
  %75 = fptrunc double %74 to float, !dbg !1932
  %76 = load i32, ptr %12, align 4, !dbg !1932
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1932
  store float %75, ptr %77, align 8, !dbg !1932
  br label %91, !dbg !1932

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1932
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1932
  store ptr %80, ptr %5, align 4, !dbg !1932
  %81 = load double, ptr %79, align 4, !dbg !1932
  %82 = load i32, ptr %12, align 4, !dbg !1932
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1932
  store double %81, ptr %83, align 8, !dbg !1932
  br label %91, !dbg !1932

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1932
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1932
  store ptr %86, ptr %5, align 4, !dbg !1932
  %87 = load ptr, ptr %85, align 4, !dbg !1932
  %88 = load i32, ptr %12, align 4, !dbg !1932
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1932
  store ptr %87, ptr %89, align 8, !dbg !1932
  br label %91, !dbg !1932

90:                                               ; preds = %25
  br label %91, !dbg !1932

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1929

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1934
  %94 = add nsw i32 %93, 1, !dbg !1934
  store i32 %94, ptr %12, align 4, !dbg !1934
  br label %21, !dbg !1934, !llvm.loop !1935

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1919
  %97 = load ptr, ptr %96, align 4, !dbg !1919
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 134, !dbg !1919
  %99 = load ptr, ptr %98, align 4, !dbg !1919
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1919
  %101 = load ptr, ptr %6, align 4, !dbg !1919
  %102 = load ptr, ptr %7, align 4, !dbg !1919
  %103 = load ptr, ptr %8, align 4, !dbg !1919
  %104 = call x86_stdcallcc i64 %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1919
  ret i64 %104, !dbg !1919
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1936 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1937, metadata !DIExpression()), !dbg !1938
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1939, metadata !DIExpression()), !dbg !1938
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1940, metadata !DIExpression()), !dbg !1938
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1941, metadata !DIExpression()), !dbg !1938
  call void @llvm.va_start(ptr %7), !dbg !1938
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1942, metadata !DIExpression()), !dbg !1938
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1943, metadata !DIExpression()), !dbg !1938
  %13 = load ptr, ptr %6, align 4, !dbg !1938
  %14 = load ptr, ptr %13, align 4, !dbg !1938
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1938
  %16 = load ptr, ptr %15, align 4, !dbg !1938
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !1938
  %18 = load ptr, ptr %4, align 4, !dbg !1938
  %19 = load ptr, ptr %6, align 4, !dbg !1938
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1938
  store i32 %20, ptr %9, align 4, !dbg !1938
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1944, metadata !DIExpression()), !dbg !1938
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1945, metadata !DIExpression()), !dbg !1947
  store i32 0, ptr %11, align 4, !dbg !1947
  br label %21, !dbg !1947

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !1947
  %23 = load i32, ptr %9, align 4, !dbg !1947
  %24 = icmp slt i32 %22, %23, !dbg !1947
  br i1 %24, label %25, label %95, !dbg !1947

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1948
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !1948
  %28 = load i8, ptr %27, align 1, !dbg !1948
  %29 = sext i8 %28 to i32, !dbg !1948
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1948

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !1951
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1951
  store ptr %32, ptr %7, align 4, !dbg !1951
  %33 = load i32, ptr %31, align 4, !dbg !1951
  %34 = trunc i32 %33 to i8, !dbg !1951
  %35 = load i32, ptr %11, align 4, !dbg !1951
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !1951
  store i8 %34, ptr %36, align 8, !dbg !1951
  br label %91, !dbg !1951

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !1951
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1951
  store ptr %39, ptr %7, align 4, !dbg !1951
  %40 = load i32, ptr %38, align 4, !dbg !1951
  %41 = trunc i32 %40 to i8, !dbg !1951
  %42 = load i32, ptr %11, align 4, !dbg !1951
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !1951
  store i8 %41, ptr %43, align 8, !dbg !1951
  br label %91, !dbg !1951

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !1951
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1951
  store ptr %46, ptr %7, align 4, !dbg !1951
  %47 = load i32, ptr %45, align 4, !dbg !1951
  %48 = trunc i32 %47 to i16, !dbg !1951
  %49 = load i32, ptr %11, align 4, !dbg !1951
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !1951
  store i16 %48, ptr %50, align 8, !dbg !1951
  br label %91, !dbg !1951

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !1951
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1951
  store ptr %53, ptr %7, align 4, !dbg !1951
  %54 = load i32, ptr %52, align 4, !dbg !1951
  %55 = trunc i32 %54 to i16, !dbg !1951
  %56 = load i32, ptr %11, align 4, !dbg !1951
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !1951
  store i16 %55, ptr %57, align 8, !dbg !1951
  br label %91, !dbg !1951

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !1951
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1951
  store ptr %60, ptr %7, align 4, !dbg !1951
  %61 = load i32, ptr %59, align 4, !dbg !1951
  %62 = load i32, ptr %11, align 4, !dbg !1951
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !1951
  store i32 %61, ptr %63, align 8, !dbg !1951
  br label %91, !dbg !1951

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !1951
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1951
  store ptr %66, ptr %7, align 4, !dbg !1951
  %67 = load i32, ptr %65, align 4, !dbg !1951
  %68 = sext i32 %67 to i64, !dbg !1951
  %69 = load i32, ptr %11, align 4, !dbg !1951
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !1951
  store i64 %68, ptr %70, align 8, !dbg !1951
  br label %91, !dbg !1951

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !1951
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1951
  store ptr %73, ptr %7, align 4, !dbg !1951
  %74 = load double, ptr %72, align 4, !dbg !1951
  %75 = fptrunc double %74 to float, !dbg !1951
  %76 = load i32, ptr %11, align 4, !dbg !1951
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !1951
  store float %75, ptr %77, align 8, !dbg !1951
  br label %91, !dbg !1951

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !1951
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1951
  store ptr %80, ptr %7, align 4, !dbg !1951
  %81 = load double, ptr %79, align 4, !dbg !1951
  %82 = load i32, ptr %11, align 4, !dbg !1951
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !1951
  store double %81, ptr %83, align 8, !dbg !1951
  br label %91, !dbg !1951

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !1951
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1951
  store ptr %86, ptr %7, align 4, !dbg !1951
  %87 = load ptr, ptr %85, align 4, !dbg !1951
  %88 = load i32, ptr %11, align 4, !dbg !1951
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !1951
  store ptr %87, ptr %89, align 8, !dbg !1951
  br label %91, !dbg !1951

90:                                               ; preds = %25
  br label %91, !dbg !1951

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1948

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !1953
  %94 = add nsw i32 %93, 1, !dbg !1953
  store i32 %94, ptr %11, align 4, !dbg !1953
  br label %21, !dbg !1953, !llvm.loop !1954

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1955, metadata !DIExpression()), !dbg !1938
  %96 = load ptr, ptr %6, align 4, !dbg !1938
  %97 = load ptr, ptr %96, align 4, !dbg !1938
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 57, !dbg !1938
  %99 = load ptr, ptr %98, align 4, !dbg !1938
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !1938
  %101 = load ptr, ptr %4, align 4, !dbg !1938
  %102 = load ptr, ptr %5, align 4, !dbg !1938
  %103 = load ptr, ptr %6, align 4, !dbg !1938
  %104 = call x86_stdcallcc float %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1938
  store float %104, ptr %12, align 4, !dbg !1938
  call void @llvm.va_end(ptr %7), !dbg !1938
  %105 = load float, ptr %12, align 4, !dbg !1938
  ret float %105, !dbg !1938
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc float @"\01_JNI_CallFloatMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1956 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1957, metadata !DIExpression()), !dbg !1958
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1959, metadata !DIExpression()), !dbg !1958
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1960, metadata !DIExpression()), !dbg !1958
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1961, metadata !DIExpression()), !dbg !1958
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1962, metadata !DIExpression()), !dbg !1958
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1963, metadata !DIExpression()), !dbg !1958
  %13 = load ptr, ptr %8, align 4, !dbg !1958
  %14 = load ptr, ptr %13, align 4, !dbg !1958
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1958
  %16 = load ptr, ptr %15, align 4, !dbg !1958
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !1958
  %18 = load ptr, ptr %6, align 4, !dbg !1958
  %19 = load ptr, ptr %8, align 4, !dbg !1958
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1958
  store i32 %20, ptr %10, align 4, !dbg !1958
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1964, metadata !DIExpression()), !dbg !1958
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1965, metadata !DIExpression()), !dbg !1967
  store i32 0, ptr %12, align 4, !dbg !1967
  br label %21, !dbg !1967

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !1967
  %23 = load i32, ptr %10, align 4, !dbg !1967
  %24 = icmp slt i32 %22, %23, !dbg !1967
  br i1 %24, label %25, label %95, !dbg !1967

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1968
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !1968
  %28 = load i8, ptr %27, align 1, !dbg !1968
  %29 = sext i8 %28 to i32, !dbg !1968
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !1968

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !1971
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !1971
  store ptr %32, ptr %5, align 4, !dbg !1971
  %33 = load i32, ptr %31, align 4, !dbg !1971
  %34 = trunc i32 %33 to i8, !dbg !1971
  %35 = load i32, ptr %12, align 4, !dbg !1971
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !1971
  store i8 %34, ptr %36, align 8, !dbg !1971
  br label %91, !dbg !1971

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !1971
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !1971
  store ptr %39, ptr %5, align 4, !dbg !1971
  %40 = load i32, ptr %38, align 4, !dbg !1971
  %41 = trunc i32 %40 to i8, !dbg !1971
  %42 = load i32, ptr %12, align 4, !dbg !1971
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !1971
  store i8 %41, ptr %43, align 8, !dbg !1971
  br label %91, !dbg !1971

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !1971
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !1971
  store ptr %46, ptr %5, align 4, !dbg !1971
  %47 = load i32, ptr %45, align 4, !dbg !1971
  %48 = trunc i32 %47 to i16, !dbg !1971
  %49 = load i32, ptr %12, align 4, !dbg !1971
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !1971
  store i16 %48, ptr %50, align 8, !dbg !1971
  br label %91, !dbg !1971

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !1971
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !1971
  store ptr %53, ptr %5, align 4, !dbg !1971
  %54 = load i32, ptr %52, align 4, !dbg !1971
  %55 = trunc i32 %54 to i16, !dbg !1971
  %56 = load i32, ptr %12, align 4, !dbg !1971
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !1971
  store i16 %55, ptr %57, align 8, !dbg !1971
  br label %91, !dbg !1971

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !1971
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !1971
  store ptr %60, ptr %5, align 4, !dbg !1971
  %61 = load i32, ptr %59, align 4, !dbg !1971
  %62 = load i32, ptr %12, align 4, !dbg !1971
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !1971
  store i32 %61, ptr %63, align 8, !dbg !1971
  br label %91, !dbg !1971

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !1971
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !1971
  store ptr %66, ptr %5, align 4, !dbg !1971
  %67 = load i32, ptr %65, align 4, !dbg !1971
  %68 = sext i32 %67 to i64, !dbg !1971
  %69 = load i32, ptr %12, align 4, !dbg !1971
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !1971
  store i64 %68, ptr %70, align 8, !dbg !1971
  br label %91, !dbg !1971

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !1971
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !1971
  store ptr %73, ptr %5, align 4, !dbg !1971
  %74 = load double, ptr %72, align 4, !dbg !1971
  %75 = fptrunc double %74 to float, !dbg !1971
  %76 = load i32, ptr %12, align 4, !dbg !1971
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !1971
  store float %75, ptr %77, align 8, !dbg !1971
  br label %91, !dbg !1971

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !1971
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !1971
  store ptr %80, ptr %5, align 4, !dbg !1971
  %81 = load double, ptr %79, align 4, !dbg !1971
  %82 = load i32, ptr %12, align 4, !dbg !1971
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !1971
  store double %81, ptr %83, align 8, !dbg !1971
  br label %91, !dbg !1971

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !1971
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !1971
  store ptr %86, ptr %5, align 4, !dbg !1971
  %87 = load ptr, ptr %85, align 4, !dbg !1971
  %88 = load i32, ptr %12, align 4, !dbg !1971
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !1971
  store ptr %87, ptr %89, align 8, !dbg !1971
  br label %91, !dbg !1971

90:                                               ; preds = %25
  br label %91, !dbg !1971

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !1968

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !1973
  %94 = add nsw i32 %93, 1, !dbg !1973
  store i32 %94, ptr %12, align 4, !dbg !1973
  br label %21, !dbg !1973, !llvm.loop !1974

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !1958
  %97 = load ptr, ptr %96, align 4, !dbg !1958
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 57, !dbg !1958
  %99 = load ptr, ptr %98, align 4, !dbg !1958
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !1958
  %101 = load ptr, ptr %6, align 4, !dbg !1958
  %102 = load ptr, ptr %7, align 4, !dbg !1958
  %103 = load ptr, ptr %8, align 4, !dbg !1958
  %104 = call x86_stdcallcc float %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !1958
  ret float %104, !dbg !1958
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1975 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1976, metadata !DIExpression()), !dbg !1977
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1978, metadata !DIExpression()), !dbg !1977
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1979, metadata !DIExpression()), !dbg !1977
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1980, metadata !DIExpression()), !dbg !1977
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1981, metadata !DIExpression()), !dbg !1977
  call void @llvm.va_start(ptr %9), !dbg !1977
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1982, metadata !DIExpression()), !dbg !1977
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1983, metadata !DIExpression()), !dbg !1977
  %15 = load ptr, ptr %8, align 4, !dbg !1977
  %16 = load ptr, ptr %15, align 4, !dbg !1977
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1977
  %18 = load ptr, ptr %17, align 4, !dbg !1977
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !1977
  %20 = load ptr, ptr %5, align 4, !dbg !1977
  %21 = load ptr, ptr %8, align 4, !dbg !1977
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1977
  store i32 %22, ptr %11, align 4, !dbg !1977
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1984, metadata !DIExpression()), !dbg !1977
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1985, metadata !DIExpression()), !dbg !1987
  store i32 0, ptr %13, align 4, !dbg !1987
  br label %23, !dbg !1987

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !1987
  %25 = load i32, ptr %11, align 4, !dbg !1987
  %26 = icmp slt i32 %24, %25, !dbg !1987
  br i1 %26, label %27, label %97, !dbg !1987

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1988
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !1988
  %30 = load i8, ptr %29, align 1, !dbg !1988
  %31 = sext i8 %30 to i32, !dbg !1988
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !1988

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !1991
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !1991
  store ptr %34, ptr %9, align 4, !dbg !1991
  %35 = load i32, ptr %33, align 4, !dbg !1991
  %36 = trunc i32 %35 to i8, !dbg !1991
  %37 = load i32, ptr %13, align 4, !dbg !1991
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !1991
  store i8 %36, ptr %38, align 8, !dbg !1991
  br label %93, !dbg !1991

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !1991
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !1991
  store ptr %41, ptr %9, align 4, !dbg !1991
  %42 = load i32, ptr %40, align 4, !dbg !1991
  %43 = trunc i32 %42 to i8, !dbg !1991
  %44 = load i32, ptr %13, align 4, !dbg !1991
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !1991
  store i8 %43, ptr %45, align 8, !dbg !1991
  br label %93, !dbg !1991

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !1991
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !1991
  store ptr %48, ptr %9, align 4, !dbg !1991
  %49 = load i32, ptr %47, align 4, !dbg !1991
  %50 = trunc i32 %49 to i16, !dbg !1991
  %51 = load i32, ptr %13, align 4, !dbg !1991
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !1991
  store i16 %50, ptr %52, align 8, !dbg !1991
  br label %93, !dbg !1991

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !1991
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !1991
  store ptr %55, ptr %9, align 4, !dbg !1991
  %56 = load i32, ptr %54, align 4, !dbg !1991
  %57 = trunc i32 %56 to i16, !dbg !1991
  %58 = load i32, ptr %13, align 4, !dbg !1991
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !1991
  store i16 %57, ptr %59, align 8, !dbg !1991
  br label %93, !dbg !1991

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !1991
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !1991
  store ptr %62, ptr %9, align 4, !dbg !1991
  %63 = load i32, ptr %61, align 4, !dbg !1991
  %64 = load i32, ptr %13, align 4, !dbg !1991
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !1991
  store i32 %63, ptr %65, align 8, !dbg !1991
  br label %93, !dbg !1991

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !1991
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !1991
  store ptr %68, ptr %9, align 4, !dbg !1991
  %69 = load i32, ptr %67, align 4, !dbg !1991
  %70 = sext i32 %69 to i64, !dbg !1991
  %71 = load i32, ptr %13, align 4, !dbg !1991
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !1991
  store i64 %70, ptr %72, align 8, !dbg !1991
  br label %93, !dbg !1991

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !1991
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !1991
  store ptr %75, ptr %9, align 4, !dbg !1991
  %76 = load double, ptr %74, align 4, !dbg !1991
  %77 = fptrunc double %76 to float, !dbg !1991
  %78 = load i32, ptr %13, align 4, !dbg !1991
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !1991
  store float %77, ptr %79, align 8, !dbg !1991
  br label %93, !dbg !1991

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !1991
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !1991
  store ptr %82, ptr %9, align 4, !dbg !1991
  %83 = load double, ptr %81, align 4, !dbg !1991
  %84 = load i32, ptr %13, align 4, !dbg !1991
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !1991
  store double %83, ptr %85, align 8, !dbg !1991
  br label %93, !dbg !1991

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !1991
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !1991
  store ptr %88, ptr %9, align 4, !dbg !1991
  %89 = load ptr, ptr %87, align 4, !dbg !1991
  %90 = load i32, ptr %13, align 4, !dbg !1991
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !1991
  store ptr %89, ptr %91, align 8, !dbg !1991
  br label %93, !dbg !1991

92:                                               ; preds = %27
  br label %93, !dbg !1991

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !1988

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !1993
  %96 = add nsw i32 %95, 1, !dbg !1993
  store i32 %96, ptr %13, align 4, !dbg !1993
  br label %23, !dbg !1993, !llvm.loop !1994

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1995, metadata !DIExpression()), !dbg !1977
  %98 = load ptr, ptr %8, align 4, !dbg !1977
  %99 = load ptr, ptr %98, align 4, !dbg !1977
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 87, !dbg !1977
  %101 = load ptr, ptr %100, align 4, !dbg !1977
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !1977
  %103 = load ptr, ptr %5, align 4, !dbg !1977
  %104 = load ptr, ptr %6, align 4, !dbg !1977
  %105 = load ptr, ptr %7, align 4, !dbg !1977
  %106 = load ptr, ptr %8, align 4, !dbg !1977
  %107 = call x86_stdcallcc float %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1977
  store float %107, ptr %14, align 4, !dbg !1977
  call void @llvm.va_end(ptr %9), !dbg !1977
  %108 = load float, ptr %14, align 4, !dbg !1977
  ret float %108, !dbg !1977
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc float @"\01_JNI_CallNonvirtualFloatMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1996 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1997, metadata !DIExpression()), !dbg !1998
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1999, metadata !DIExpression()), !dbg !1998
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2000, metadata !DIExpression()), !dbg !1998
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2001, metadata !DIExpression()), !dbg !1998
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2002, metadata !DIExpression()), !dbg !1998
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2003, metadata !DIExpression()), !dbg !1998
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2004, metadata !DIExpression()), !dbg !1998
  %15 = load ptr, ptr %10, align 4, !dbg !1998
  %16 = load ptr, ptr %15, align 4, !dbg !1998
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1998
  %18 = load ptr, ptr %17, align 4, !dbg !1998
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !1998
  %20 = load ptr, ptr %7, align 4, !dbg !1998
  %21 = load ptr, ptr %10, align 4, !dbg !1998
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1998
  store i32 %22, ptr %12, align 4, !dbg !1998
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2005, metadata !DIExpression()), !dbg !1998
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2006, metadata !DIExpression()), !dbg !2008
  store i32 0, ptr %14, align 4, !dbg !2008
  br label %23, !dbg !2008

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !2008
  %25 = load i32, ptr %12, align 4, !dbg !2008
  %26 = icmp slt i32 %24, %25, !dbg !2008
  br i1 %26, label %27, label %97, !dbg !2008

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2009
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2009
  %30 = load i8, ptr %29, align 1, !dbg !2009
  %31 = sext i8 %30 to i32, !dbg !2009
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !2009

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2012
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2012
  store ptr %34, ptr %6, align 4, !dbg !2012
  %35 = load i32, ptr %33, align 4, !dbg !2012
  %36 = trunc i32 %35 to i8, !dbg !2012
  %37 = load i32, ptr %14, align 4, !dbg !2012
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2012
  store i8 %36, ptr %38, align 8, !dbg !2012
  br label %93, !dbg !2012

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2012
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2012
  store ptr %41, ptr %6, align 4, !dbg !2012
  %42 = load i32, ptr %40, align 4, !dbg !2012
  %43 = trunc i32 %42 to i8, !dbg !2012
  %44 = load i32, ptr %14, align 4, !dbg !2012
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2012
  store i8 %43, ptr %45, align 8, !dbg !2012
  br label %93, !dbg !2012

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2012
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2012
  store ptr %48, ptr %6, align 4, !dbg !2012
  %49 = load i32, ptr %47, align 4, !dbg !2012
  %50 = trunc i32 %49 to i16, !dbg !2012
  %51 = load i32, ptr %14, align 4, !dbg !2012
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2012
  store i16 %50, ptr %52, align 8, !dbg !2012
  br label %93, !dbg !2012

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2012
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2012
  store ptr %55, ptr %6, align 4, !dbg !2012
  %56 = load i32, ptr %54, align 4, !dbg !2012
  %57 = trunc i32 %56 to i16, !dbg !2012
  %58 = load i32, ptr %14, align 4, !dbg !2012
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2012
  store i16 %57, ptr %59, align 8, !dbg !2012
  br label %93, !dbg !2012

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2012
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2012
  store ptr %62, ptr %6, align 4, !dbg !2012
  %63 = load i32, ptr %61, align 4, !dbg !2012
  %64 = load i32, ptr %14, align 4, !dbg !2012
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2012
  store i32 %63, ptr %65, align 8, !dbg !2012
  br label %93, !dbg !2012

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2012
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2012
  store ptr %68, ptr %6, align 4, !dbg !2012
  %69 = load i32, ptr %67, align 4, !dbg !2012
  %70 = sext i32 %69 to i64, !dbg !2012
  %71 = load i32, ptr %14, align 4, !dbg !2012
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2012
  store i64 %70, ptr %72, align 8, !dbg !2012
  br label %93, !dbg !2012

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2012
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !2012
  store ptr %75, ptr %6, align 4, !dbg !2012
  %76 = load double, ptr %74, align 4, !dbg !2012
  %77 = fptrunc double %76 to float, !dbg !2012
  %78 = load i32, ptr %14, align 4, !dbg !2012
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !2012
  store float %77, ptr %79, align 8, !dbg !2012
  br label %93, !dbg !2012

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !2012
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !2012
  store ptr %82, ptr %6, align 4, !dbg !2012
  %83 = load double, ptr %81, align 4, !dbg !2012
  %84 = load i32, ptr %14, align 4, !dbg !2012
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !2012
  store double %83, ptr %85, align 8, !dbg !2012
  br label %93, !dbg !2012

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !2012
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !2012
  store ptr %88, ptr %6, align 4, !dbg !2012
  %89 = load ptr, ptr %87, align 4, !dbg !2012
  %90 = load i32, ptr %14, align 4, !dbg !2012
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !2012
  store ptr %89, ptr %91, align 8, !dbg !2012
  br label %93, !dbg !2012

92:                                               ; preds = %27
  br label %93, !dbg !2012

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !2009

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !2014
  %96 = add nsw i32 %95, 1, !dbg !2014
  store i32 %96, ptr %14, align 4, !dbg !2014
  br label %23, !dbg !2014, !llvm.loop !2015

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !1998
  %99 = load ptr, ptr %98, align 4, !dbg !1998
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 87, !dbg !1998
  %101 = load ptr, ptr %100, align 4, !dbg !1998
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !1998
  %103 = load ptr, ptr %7, align 4, !dbg !1998
  %104 = load ptr, ptr %8, align 4, !dbg !1998
  %105 = load ptr, ptr %9, align 4, !dbg !1998
  %106 = load ptr, ptr %10, align 4, !dbg !1998
  %107 = call x86_stdcallcc float %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !1998
  ret float %107, !dbg !1998
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2016 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2017, metadata !DIExpression()), !dbg !2018
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2019, metadata !DIExpression()), !dbg !2018
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2020, metadata !DIExpression()), !dbg !2018
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2021, metadata !DIExpression()), !dbg !2018
  call void @llvm.va_start(ptr %7), !dbg !2018
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2022, metadata !DIExpression()), !dbg !2018
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2023, metadata !DIExpression()), !dbg !2018
  %13 = load ptr, ptr %6, align 4, !dbg !2018
  %14 = load ptr, ptr %13, align 4, !dbg !2018
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2018
  %16 = load ptr, ptr %15, align 4, !dbg !2018
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2018
  %18 = load ptr, ptr %4, align 4, !dbg !2018
  %19 = load ptr, ptr %6, align 4, !dbg !2018
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2018
  store i32 %20, ptr %9, align 4, !dbg !2018
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2024, metadata !DIExpression()), !dbg !2018
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2025, metadata !DIExpression()), !dbg !2027
  store i32 0, ptr %11, align 4, !dbg !2027
  br label %21, !dbg !2027

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !2027
  %23 = load i32, ptr %9, align 4, !dbg !2027
  %24 = icmp slt i32 %22, %23, !dbg !2027
  br i1 %24, label %25, label %95, !dbg !2027

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2028
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2028
  %28 = load i8, ptr %27, align 1, !dbg !2028
  %29 = sext i8 %28 to i32, !dbg !2028
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2028

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2031
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2031
  store ptr %32, ptr %7, align 4, !dbg !2031
  %33 = load i32, ptr %31, align 4, !dbg !2031
  %34 = trunc i32 %33 to i8, !dbg !2031
  %35 = load i32, ptr %11, align 4, !dbg !2031
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2031
  store i8 %34, ptr %36, align 8, !dbg !2031
  br label %91, !dbg !2031

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2031
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2031
  store ptr %39, ptr %7, align 4, !dbg !2031
  %40 = load i32, ptr %38, align 4, !dbg !2031
  %41 = trunc i32 %40 to i8, !dbg !2031
  %42 = load i32, ptr %11, align 4, !dbg !2031
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2031
  store i8 %41, ptr %43, align 8, !dbg !2031
  br label %91, !dbg !2031

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2031
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2031
  store ptr %46, ptr %7, align 4, !dbg !2031
  %47 = load i32, ptr %45, align 4, !dbg !2031
  %48 = trunc i32 %47 to i16, !dbg !2031
  %49 = load i32, ptr %11, align 4, !dbg !2031
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2031
  store i16 %48, ptr %50, align 8, !dbg !2031
  br label %91, !dbg !2031

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2031
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2031
  store ptr %53, ptr %7, align 4, !dbg !2031
  %54 = load i32, ptr %52, align 4, !dbg !2031
  %55 = trunc i32 %54 to i16, !dbg !2031
  %56 = load i32, ptr %11, align 4, !dbg !2031
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2031
  store i16 %55, ptr %57, align 8, !dbg !2031
  br label %91, !dbg !2031

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2031
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2031
  store ptr %60, ptr %7, align 4, !dbg !2031
  %61 = load i32, ptr %59, align 4, !dbg !2031
  %62 = load i32, ptr %11, align 4, !dbg !2031
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2031
  store i32 %61, ptr %63, align 8, !dbg !2031
  br label %91, !dbg !2031

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2031
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2031
  store ptr %66, ptr %7, align 4, !dbg !2031
  %67 = load i32, ptr %65, align 4, !dbg !2031
  %68 = sext i32 %67 to i64, !dbg !2031
  %69 = load i32, ptr %11, align 4, !dbg !2031
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2031
  store i64 %68, ptr %70, align 8, !dbg !2031
  br label %91, !dbg !2031

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2031
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2031
  store ptr %73, ptr %7, align 4, !dbg !2031
  %74 = load double, ptr %72, align 4, !dbg !2031
  %75 = fptrunc double %74 to float, !dbg !2031
  %76 = load i32, ptr %11, align 4, !dbg !2031
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !2031
  store float %75, ptr %77, align 8, !dbg !2031
  br label %91, !dbg !2031

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !2031
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2031
  store ptr %80, ptr %7, align 4, !dbg !2031
  %81 = load double, ptr %79, align 4, !dbg !2031
  %82 = load i32, ptr %11, align 4, !dbg !2031
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !2031
  store double %81, ptr %83, align 8, !dbg !2031
  br label %91, !dbg !2031

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !2031
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2031
  store ptr %86, ptr %7, align 4, !dbg !2031
  %87 = load ptr, ptr %85, align 4, !dbg !2031
  %88 = load i32, ptr %11, align 4, !dbg !2031
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !2031
  store ptr %87, ptr %89, align 8, !dbg !2031
  br label %91, !dbg !2031

90:                                               ; preds = %25
  br label %91, !dbg !2031

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2028

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !2033
  %94 = add nsw i32 %93, 1, !dbg !2033
  store i32 %94, ptr %11, align 4, !dbg !2033
  br label %21, !dbg !2033, !llvm.loop !2034

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2035, metadata !DIExpression()), !dbg !2018
  %96 = load ptr, ptr %6, align 4, !dbg !2018
  %97 = load ptr, ptr %96, align 4, !dbg !2018
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 137, !dbg !2018
  %99 = load ptr, ptr %98, align 4, !dbg !2018
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2018
  %101 = load ptr, ptr %4, align 4, !dbg !2018
  %102 = load ptr, ptr %5, align 4, !dbg !2018
  %103 = load ptr, ptr %6, align 4, !dbg !2018
  %104 = call x86_stdcallcc float %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2018
  store float %104, ptr %12, align 4, !dbg !2018
  call void @llvm.va_end(ptr %7), !dbg !2018
  %105 = load float, ptr %12, align 4, !dbg !2018
  ret float %105, !dbg !2018
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc float @"\01_JNI_CallStaticFloatMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2036 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2037, metadata !DIExpression()), !dbg !2038
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2039, metadata !DIExpression()), !dbg !2038
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2040, metadata !DIExpression()), !dbg !2038
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2041, metadata !DIExpression()), !dbg !2038
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2042, metadata !DIExpression()), !dbg !2038
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2043, metadata !DIExpression()), !dbg !2038
  %13 = load ptr, ptr %8, align 4, !dbg !2038
  %14 = load ptr, ptr %13, align 4, !dbg !2038
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2038
  %16 = load ptr, ptr %15, align 4, !dbg !2038
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2038
  %18 = load ptr, ptr %6, align 4, !dbg !2038
  %19 = load ptr, ptr %8, align 4, !dbg !2038
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2038
  store i32 %20, ptr %10, align 4, !dbg !2038
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2044, metadata !DIExpression()), !dbg !2038
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2045, metadata !DIExpression()), !dbg !2047
  store i32 0, ptr %12, align 4, !dbg !2047
  br label %21, !dbg !2047

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2047
  %23 = load i32, ptr %10, align 4, !dbg !2047
  %24 = icmp slt i32 %22, %23, !dbg !2047
  br i1 %24, label %25, label %95, !dbg !2047

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2048
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2048
  %28 = load i8, ptr %27, align 1, !dbg !2048
  %29 = sext i8 %28 to i32, !dbg !2048
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2048

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2051
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2051
  store ptr %32, ptr %5, align 4, !dbg !2051
  %33 = load i32, ptr %31, align 4, !dbg !2051
  %34 = trunc i32 %33 to i8, !dbg !2051
  %35 = load i32, ptr %12, align 4, !dbg !2051
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2051
  store i8 %34, ptr %36, align 8, !dbg !2051
  br label %91, !dbg !2051

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2051
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2051
  store ptr %39, ptr %5, align 4, !dbg !2051
  %40 = load i32, ptr %38, align 4, !dbg !2051
  %41 = trunc i32 %40 to i8, !dbg !2051
  %42 = load i32, ptr %12, align 4, !dbg !2051
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2051
  store i8 %41, ptr %43, align 8, !dbg !2051
  br label %91, !dbg !2051

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2051
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2051
  store ptr %46, ptr %5, align 4, !dbg !2051
  %47 = load i32, ptr %45, align 4, !dbg !2051
  %48 = trunc i32 %47 to i16, !dbg !2051
  %49 = load i32, ptr %12, align 4, !dbg !2051
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2051
  store i16 %48, ptr %50, align 8, !dbg !2051
  br label %91, !dbg !2051

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2051
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2051
  store ptr %53, ptr %5, align 4, !dbg !2051
  %54 = load i32, ptr %52, align 4, !dbg !2051
  %55 = trunc i32 %54 to i16, !dbg !2051
  %56 = load i32, ptr %12, align 4, !dbg !2051
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2051
  store i16 %55, ptr %57, align 8, !dbg !2051
  br label %91, !dbg !2051

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2051
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2051
  store ptr %60, ptr %5, align 4, !dbg !2051
  %61 = load i32, ptr %59, align 4, !dbg !2051
  %62 = load i32, ptr %12, align 4, !dbg !2051
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2051
  store i32 %61, ptr %63, align 8, !dbg !2051
  br label %91, !dbg !2051

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2051
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2051
  store ptr %66, ptr %5, align 4, !dbg !2051
  %67 = load i32, ptr %65, align 4, !dbg !2051
  %68 = sext i32 %67 to i64, !dbg !2051
  %69 = load i32, ptr %12, align 4, !dbg !2051
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2051
  store i64 %68, ptr %70, align 8, !dbg !2051
  br label %91, !dbg !2051

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2051
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2051
  store ptr %73, ptr %5, align 4, !dbg !2051
  %74 = load double, ptr %72, align 4, !dbg !2051
  %75 = fptrunc double %74 to float, !dbg !2051
  %76 = load i32, ptr %12, align 4, !dbg !2051
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2051
  store float %75, ptr %77, align 8, !dbg !2051
  br label %91, !dbg !2051

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2051
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2051
  store ptr %80, ptr %5, align 4, !dbg !2051
  %81 = load double, ptr %79, align 4, !dbg !2051
  %82 = load i32, ptr %12, align 4, !dbg !2051
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2051
  store double %81, ptr %83, align 8, !dbg !2051
  br label %91, !dbg !2051

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2051
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2051
  store ptr %86, ptr %5, align 4, !dbg !2051
  %87 = load ptr, ptr %85, align 4, !dbg !2051
  %88 = load i32, ptr %12, align 4, !dbg !2051
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2051
  store ptr %87, ptr %89, align 8, !dbg !2051
  br label %91, !dbg !2051

90:                                               ; preds = %25
  br label %91, !dbg !2051

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2048

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2053
  %94 = add nsw i32 %93, 1, !dbg !2053
  store i32 %94, ptr %12, align 4, !dbg !2053
  br label %21, !dbg !2053, !llvm.loop !2054

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2038
  %97 = load ptr, ptr %96, align 4, !dbg !2038
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 137, !dbg !2038
  %99 = load ptr, ptr %98, align 4, !dbg !2038
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2038
  %101 = load ptr, ptr %6, align 4, !dbg !2038
  %102 = load ptr, ptr %7, align 4, !dbg !2038
  %103 = load ptr, ptr %8, align 4, !dbg !2038
  %104 = call x86_stdcallcc float %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2038
  ret float %104, !dbg !2038
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2055 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2056, metadata !DIExpression()), !dbg !2057
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2058, metadata !DIExpression()), !dbg !2057
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2059, metadata !DIExpression()), !dbg !2057
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2060, metadata !DIExpression()), !dbg !2057
  call void @llvm.va_start(ptr %7), !dbg !2057
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2061, metadata !DIExpression()), !dbg !2057
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2062, metadata !DIExpression()), !dbg !2057
  %13 = load ptr, ptr %6, align 4, !dbg !2057
  %14 = load ptr, ptr %13, align 4, !dbg !2057
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2057
  %16 = load ptr, ptr %15, align 4, !dbg !2057
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2057
  %18 = load ptr, ptr %4, align 4, !dbg !2057
  %19 = load ptr, ptr %6, align 4, !dbg !2057
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2057
  store i32 %20, ptr %9, align 4, !dbg !2057
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2063, metadata !DIExpression()), !dbg !2057
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2064, metadata !DIExpression()), !dbg !2066
  store i32 0, ptr %11, align 4, !dbg !2066
  br label %21, !dbg !2066

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !2066
  %23 = load i32, ptr %9, align 4, !dbg !2066
  %24 = icmp slt i32 %22, %23, !dbg !2066
  br i1 %24, label %25, label %95, !dbg !2066

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2067
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2067
  %28 = load i8, ptr %27, align 1, !dbg !2067
  %29 = sext i8 %28 to i32, !dbg !2067
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2067

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2070
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2070
  store ptr %32, ptr %7, align 4, !dbg !2070
  %33 = load i32, ptr %31, align 4, !dbg !2070
  %34 = trunc i32 %33 to i8, !dbg !2070
  %35 = load i32, ptr %11, align 4, !dbg !2070
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2070
  store i8 %34, ptr %36, align 8, !dbg !2070
  br label %91, !dbg !2070

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2070
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2070
  store ptr %39, ptr %7, align 4, !dbg !2070
  %40 = load i32, ptr %38, align 4, !dbg !2070
  %41 = trunc i32 %40 to i8, !dbg !2070
  %42 = load i32, ptr %11, align 4, !dbg !2070
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2070
  store i8 %41, ptr %43, align 8, !dbg !2070
  br label %91, !dbg !2070

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2070
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2070
  store ptr %46, ptr %7, align 4, !dbg !2070
  %47 = load i32, ptr %45, align 4, !dbg !2070
  %48 = trunc i32 %47 to i16, !dbg !2070
  %49 = load i32, ptr %11, align 4, !dbg !2070
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2070
  store i16 %48, ptr %50, align 8, !dbg !2070
  br label %91, !dbg !2070

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2070
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2070
  store ptr %53, ptr %7, align 4, !dbg !2070
  %54 = load i32, ptr %52, align 4, !dbg !2070
  %55 = trunc i32 %54 to i16, !dbg !2070
  %56 = load i32, ptr %11, align 4, !dbg !2070
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2070
  store i16 %55, ptr %57, align 8, !dbg !2070
  br label %91, !dbg !2070

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2070
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2070
  store ptr %60, ptr %7, align 4, !dbg !2070
  %61 = load i32, ptr %59, align 4, !dbg !2070
  %62 = load i32, ptr %11, align 4, !dbg !2070
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2070
  store i32 %61, ptr %63, align 8, !dbg !2070
  br label %91, !dbg !2070

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2070
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2070
  store ptr %66, ptr %7, align 4, !dbg !2070
  %67 = load i32, ptr %65, align 4, !dbg !2070
  %68 = sext i32 %67 to i64, !dbg !2070
  %69 = load i32, ptr %11, align 4, !dbg !2070
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2070
  store i64 %68, ptr %70, align 8, !dbg !2070
  br label %91, !dbg !2070

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2070
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2070
  store ptr %73, ptr %7, align 4, !dbg !2070
  %74 = load double, ptr %72, align 4, !dbg !2070
  %75 = fptrunc double %74 to float, !dbg !2070
  %76 = load i32, ptr %11, align 4, !dbg !2070
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !2070
  store float %75, ptr %77, align 8, !dbg !2070
  br label %91, !dbg !2070

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !2070
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2070
  store ptr %80, ptr %7, align 4, !dbg !2070
  %81 = load double, ptr %79, align 4, !dbg !2070
  %82 = load i32, ptr %11, align 4, !dbg !2070
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !2070
  store double %81, ptr %83, align 8, !dbg !2070
  br label %91, !dbg !2070

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !2070
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2070
  store ptr %86, ptr %7, align 4, !dbg !2070
  %87 = load ptr, ptr %85, align 4, !dbg !2070
  %88 = load i32, ptr %11, align 4, !dbg !2070
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !2070
  store ptr %87, ptr %89, align 8, !dbg !2070
  br label %91, !dbg !2070

90:                                               ; preds = %25
  br label %91, !dbg !2070

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2067

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !2072
  %94 = add nsw i32 %93, 1, !dbg !2072
  store i32 %94, ptr %11, align 4, !dbg !2072
  br label %21, !dbg !2072, !llvm.loop !2073

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2074, metadata !DIExpression()), !dbg !2057
  %96 = load ptr, ptr %6, align 4, !dbg !2057
  %97 = load ptr, ptr %96, align 4, !dbg !2057
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 60, !dbg !2057
  %99 = load ptr, ptr %98, align 4, !dbg !2057
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2057
  %101 = load ptr, ptr %4, align 4, !dbg !2057
  %102 = load ptr, ptr %5, align 4, !dbg !2057
  %103 = load ptr, ptr %6, align 4, !dbg !2057
  %104 = call x86_stdcallcc double %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2057
  store double %104, ptr %12, align 8, !dbg !2057
  call void @llvm.va_end(ptr %7), !dbg !2057
  %105 = load double, ptr %12, align 8, !dbg !2057
  ret double %105, !dbg !2057
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc double @"\01_JNI_CallDoubleMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2075 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2076, metadata !DIExpression()), !dbg !2077
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2078, metadata !DIExpression()), !dbg !2077
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2079, metadata !DIExpression()), !dbg !2077
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2080, metadata !DIExpression()), !dbg !2077
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2081, metadata !DIExpression()), !dbg !2077
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2082, metadata !DIExpression()), !dbg !2077
  %13 = load ptr, ptr %8, align 4, !dbg !2077
  %14 = load ptr, ptr %13, align 4, !dbg !2077
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2077
  %16 = load ptr, ptr %15, align 4, !dbg !2077
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2077
  %18 = load ptr, ptr %6, align 4, !dbg !2077
  %19 = load ptr, ptr %8, align 4, !dbg !2077
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2077
  store i32 %20, ptr %10, align 4, !dbg !2077
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2083, metadata !DIExpression()), !dbg !2077
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2084, metadata !DIExpression()), !dbg !2086
  store i32 0, ptr %12, align 4, !dbg !2086
  br label %21, !dbg !2086

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2086
  %23 = load i32, ptr %10, align 4, !dbg !2086
  %24 = icmp slt i32 %22, %23, !dbg !2086
  br i1 %24, label %25, label %95, !dbg !2086

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2087
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2087
  %28 = load i8, ptr %27, align 1, !dbg !2087
  %29 = sext i8 %28 to i32, !dbg !2087
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2087

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2090
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2090
  store ptr %32, ptr %5, align 4, !dbg !2090
  %33 = load i32, ptr %31, align 4, !dbg !2090
  %34 = trunc i32 %33 to i8, !dbg !2090
  %35 = load i32, ptr %12, align 4, !dbg !2090
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2090
  store i8 %34, ptr %36, align 8, !dbg !2090
  br label %91, !dbg !2090

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2090
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2090
  store ptr %39, ptr %5, align 4, !dbg !2090
  %40 = load i32, ptr %38, align 4, !dbg !2090
  %41 = trunc i32 %40 to i8, !dbg !2090
  %42 = load i32, ptr %12, align 4, !dbg !2090
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2090
  store i8 %41, ptr %43, align 8, !dbg !2090
  br label %91, !dbg !2090

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2090
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2090
  store ptr %46, ptr %5, align 4, !dbg !2090
  %47 = load i32, ptr %45, align 4, !dbg !2090
  %48 = trunc i32 %47 to i16, !dbg !2090
  %49 = load i32, ptr %12, align 4, !dbg !2090
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2090
  store i16 %48, ptr %50, align 8, !dbg !2090
  br label %91, !dbg !2090

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2090
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2090
  store ptr %53, ptr %5, align 4, !dbg !2090
  %54 = load i32, ptr %52, align 4, !dbg !2090
  %55 = trunc i32 %54 to i16, !dbg !2090
  %56 = load i32, ptr %12, align 4, !dbg !2090
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2090
  store i16 %55, ptr %57, align 8, !dbg !2090
  br label %91, !dbg !2090

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2090
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2090
  store ptr %60, ptr %5, align 4, !dbg !2090
  %61 = load i32, ptr %59, align 4, !dbg !2090
  %62 = load i32, ptr %12, align 4, !dbg !2090
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2090
  store i32 %61, ptr %63, align 8, !dbg !2090
  br label %91, !dbg !2090

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2090
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2090
  store ptr %66, ptr %5, align 4, !dbg !2090
  %67 = load i32, ptr %65, align 4, !dbg !2090
  %68 = sext i32 %67 to i64, !dbg !2090
  %69 = load i32, ptr %12, align 4, !dbg !2090
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2090
  store i64 %68, ptr %70, align 8, !dbg !2090
  br label %91, !dbg !2090

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2090
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2090
  store ptr %73, ptr %5, align 4, !dbg !2090
  %74 = load double, ptr %72, align 4, !dbg !2090
  %75 = fptrunc double %74 to float, !dbg !2090
  %76 = load i32, ptr %12, align 4, !dbg !2090
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2090
  store float %75, ptr %77, align 8, !dbg !2090
  br label %91, !dbg !2090

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2090
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2090
  store ptr %80, ptr %5, align 4, !dbg !2090
  %81 = load double, ptr %79, align 4, !dbg !2090
  %82 = load i32, ptr %12, align 4, !dbg !2090
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2090
  store double %81, ptr %83, align 8, !dbg !2090
  br label %91, !dbg !2090

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2090
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2090
  store ptr %86, ptr %5, align 4, !dbg !2090
  %87 = load ptr, ptr %85, align 4, !dbg !2090
  %88 = load i32, ptr %12, align 4, !dbg !2090
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2090
  store ptr %87, ptr %89, align 8, !dbg !2090
  br label %91, !dbg !2090

90:                                               ; preds = %25
  br label %91, !dbg !2090

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2087

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2092
  %94 = add nsw i32 %93, 1, !dbg !2092
  store i32 %94, ptr %12, align 4, !dbg !2092
  br label %21, !dbg !2092, !llvm.loop !2093

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2077
  %97 = load ptr, ptr %96, align 4, !dbg !2077
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 60, !dbg !2077
  %99 = load ptr, ptr %98, align 4, !dbg !2077
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2077
  %101 = load ptr, ptr %6, align 4, !dbg !2077
  %102 = load ptr, ptr %7, align 4, !dbg !2077
  %103 = load ptr, ptr %8, align 4, !dbg !2077
  %104 = call x86_stdcallcc double %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2077
  ret double %104, !dbg !2077
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2094 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2095, metadata !DIExpression()), !dbg !2096
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2097, metadata !DIExpression()), !dbg !2096
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2098, metadata !DIExpression()), !dbg !2096
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2099, metadata !DIExpression()), !dbg !2096
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2100, metadata !DIExpression()), !dbg !2096
  call void @llvm.va_start(ptr %9), !dbg !2096
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2101, metadata !DIExpression()), !dbg !2096
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2102, metadata !DIExpression()), !dbg !2096
  %15 = load ptr, ptr %8, align 4, !dbg !2096
  %16 = load ptr, ptr %15, align 4, !dbg !2096
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2096
  %18 = load ptr, ptr %17, align 4, !dbg !2096
  %19 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !2096
  %20 = load ptr, ptr %5, align 4, !dbg !2096
  %21 = load ptr, ptr %8, align 4, !dbg !2096
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2096
  store i32 %22, ptr %11, align 4, !dbg !2096
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2103, metadata !DIExpression()), !dbg !2096
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2104, metadata !DIExpression()), !dbg !2106
  store i32 0, ptr %13, align 4, !dbg !2106
  br label %23, !dbg !2106

23:                                               ; preds = %94, %4
  %24 = load i32, ptr %13, align 4, !dbg !2106
  %25 = load i32, ptr %11, align 4, !dbg !2106
  %26 = icmp slt i32 %24, %25, !dbg !2106
  br i1 %26, label %27, label %97, !dbg !2106

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !2107
  %29 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %28, !dbg !2107
  %30 = load i8, ptr %29, align 1, !dbg !2107
  %31 = sext i8 %30 to i32, !dbg !2107
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !2107

32:                                               ; preds = %27
  %33 = load ptr, ptr %9, align 4, !dbg !2110
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2110
  store ptr %34, ptr %9, align 4, !dbg !2110
  %35 = load i32, ptr %33, align 4, !dbg !2110
  %36 = trunc i32 %35 to i8, !dbg !2110
  %37 = load i32, ptr %13, align 4, !dbg !2110
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %37, !dbg !2110
  store i8 %36, ptr %38, align 8, !dbg !2110
  br label %93, !dbg !2110

39:                                               ; preds = %27
  %40 = load ptr, ptr %9, align 4, !dbg !2110
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2110
  store ptr %41, ptr %9, align 4, !dbg !2110
  %42 = load i32, ptr %40, align 4, !dbg !2110
  %43 = trunc i32 %42 to i8, !dbg !2110
  %44 = load i32, ptr %13, align 4, !dbg !2110
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %44, !dbg !2110
  store i8 %43, ptr %45, align 8, !dbg !2110
  br label %93, !dbg !2110

46:                                               ; preds = %27
  %47 = load ptr, ptr %9, align 4, !dbg !2110
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2110
  store ptr %48, ptr %9, align 4, !dbg !2110
  %49 = load i32, ptr %47, align 4, !dbg !2110
  %50 = trunc i32 %49 to i16, !dbg !2110
  %51 = load i32, ptr %13, align 4, !dbg !2110
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %51, !dbg !2110
  store i16 %50, ptr %52, align 8, !dbg !2110
  br label %93, !dbg !2110

53:                                               ; preds = %27
  %54 = load ptr, ptr %9, align 4, !dbg !2110
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2110
  store ptr %55, ptr %9, align 4, !dbg !2110
  %56 = load i32, ptr %54, align 4, !dbg !2110
  %57 = trunc i32 %56 to i16, !dbg !2110
  %58 = load i32, ptr %13, align 4, !dbg !2110
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %58, !dbg !2110
  store i16 %57, ptr %59, align 8, !dbg !2110
  br label %93, !dbg !2110

60:                                               ; preds = %27
  %61 = load ptr, ptr %9, align 4, !dbg !2110
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2110
  store ptr %62, ptr %9, align 4, !dbg !2110
  %63 = load i32, ptr %61, align 4, !dbg !2110
  %64 = load i32, ptr %13, align 4, !dbg !2110
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %64, !dbg !2110
  store i32 %63, ptr %65, align 8, !dbg !2110
  br label %93, !dbg !2110

66:                                               ; preds = %27
  %67 = load ptr, ptr %9, align 4, !dbg !2110
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2110
  store ptr %68, ptr %9, align 4, !dbg !2110
  %69 = load i32, ptr %67, align 4, !dbg !2110
  %70 = sext i32 %69 to i64, !dbg !2110
  %71 = load i32, ptr %13, align 4, !dbg !2110
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %71, !dbg !2110
  store i64 %70, ptr %72, align 8, !dbg !2110
  br label %93, !dbg !2110

73:                                               ; preds = %27
  %74 = load ptr, ptr %9, align 4, !dbg !2110
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !2110
  store ptr %75, ptr %9, align 4, !dbg !2110
  %76 = load double, ptr %74, align 4, !dbg !2110
  %77 = fptrunc double %76 to float, !dbg !2110
  %78 = load i32, ptr %13, align 4, !dbg !2110
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %78, !dbg !2110
  store float %77, ptr %79, align 8, !dbg !2110
  br label %93, !dbg !2110

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 4, !dbg !2110
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !2110
  store ptr %82, ptr %9, align 4, !dbg !2110
  %83 = load double, ptr %81, align 4, !dbg !2110
  %84 = load i32, ptr %13, align 4, !dbg !2110
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %84, !dbg !2110
  store double %83, ptr %85, align 8, !dbg !2110
  br label %93, !dbg !2110

86:                                               ; preds = %27
  %87 = load ptr, ptr %9, align 4, !dbg !2110
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !2110
  store ptr %88, ptr %9, align 4, !dbg !2110
  %89 = load ptr, ptr %87, align 4, !dbg !2110
  %90 = load i32, ptr %13, align 4, !dbg !2110
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %90, !dbg !2110
  store ptr %89, ptr %91, align 8, !dbg !2110
  br label %93, !dbg !2110

92:                                               ; preds = %27
  br label %93, !dbg !2110

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !2107

94:                                               ; preds = %93
  %95 = load i32, ptr %13, align 4, !dbg !2112
  %96 = add nsw i32 %95, 1, !dbg !2112
  store i32 %96, ptr %13, align 4, !dbg !2112
  br label %23, !dbg !2112, !llvm.loop !2113

97:                                               ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2114, metadata !DIExpression()), !dbg !2096
  %98 = load ptr, ptr %8, align 4, !dbg !2096
  %99 = load ptr, ptr %98, align 4, !dbg !2096
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 90, !dbg !2096
  %101 = load ptr, ptr %100, align 4, !dbg !2096
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !2096
  %103 = load ptr, ptr %5, align 4, !dbg !2096
  %104 = load ptr, ptr %6, align 4, !dbg !2096
  %105 = load ptr, ptr %7, align 4, !dbg !2096
  %106 = load ptr, ptr %8, align 4, !dbg !2096
  %107 = call x86_stdcallcc double %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !2096
  store double %107, ptr %14, align 8, !dbg !2096
  call void @llvm.va_end(ptr %9), !dbg !2096
  %108 = load double, ptr %14, align 8, !dbg !2096
  ret double %108, !dbg !2096
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc double @"\01_JNI_CallNonvirtualDoubleMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2115 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2116, metadata !DIExpression()), !dbg !2117
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2118, metadata !DIExpression()), !dbg !2117
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2119, metadata !DIExpression()), !dbg !2117
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2120, metadata !DIExpression()), !dbg !2117
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2121, metadata !DIExpression()), !dbg !2117
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2122, metadata !DIExpression()), !dbg !2117
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2123, metadata !DIExpression()), !dbg !2117
  %15 = load ptr, ptr %10, align 4, !dbg !2117
  %16 = load ptr, ptr %15, align 4, !dbg !2117
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2117
  %18 = load ptr, ptr %17, align 4, !dbg !2117
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !2117
  %20 = load ptr, ptr %7, align 4, !dbg !2117
  %21 = load ptr, ptr %10, align 4, !dbg !2117
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2117
  store i32 %22, ptr %12, align 4, !dbg !2117
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2124, metadata !DIExpression()), !dbg !2117
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2125, metadata !DIExpression()), !dbg !2127
  store i32 0, ptr %14, align 4, !dbg !2127
  br label %23, !dbg !2127

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !2127
  %25 = load i32, ptr %12, align 4, !dbg !2127
  %26 = icmp slt i32 %24, %25, !dbg !2127
  br i1 %26, label %27, label %97, !dbg !2127

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2128
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2128
  %30 = load i8, ptr %29, align 1, !dbg !2128
  %31 = sext i8 %30 to i32, !dbg !2128
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !2128

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2131
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2131
  store ptr %34, ptr %6, align 4, !dbg !2131
  %35 = load i32, ptr %33, align 4, !dbg !2131
  %36 = trunc i32 %35 to i8, !dbg !2131
  %37 = load i32, ptr %14, align 4, !dbg !2131
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2131
  store i8 %36, ptr %38, align 8, !dbg !2131
  br label %93, !dbg !2131

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2131
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2131
  store ptr %41, ptr %6, align 4, !dbg !2131
  %42 = load i32, ptr %40, align 4, !dbg !2131
  %43 = trunc i32 %42 to i8, !dbg !2131
  %44 = load i32, ptr %14, align 4, !dbg !2131
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2131
  store i8 %43, ptr %45, align 8, !dbg !2131
  br label %93, !dbg !2131

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2131
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2131
  store ptr %48, ptr %6, align 4, !dbg !2131
  %49 = load i32, ptr %47, align 4, !dbg !2131
  %50 = trunc i32 %49 to i16, !dbg !2131
  %51 = load i32, ptr %14, align 4, !dbg !2131
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2131
  store i16 %50, ptr %52, align 8, !dbg !2131
  br label %93, !dbg !2131

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2131
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2131
  store ptr %55, ptr %6, align 4, !dbg !2131
  %56 = load i32, ptr %54, align 4, !dbg !2131
  %57 = trunc i32 %56 to i16, !dbg !2131
  %58 = load i32, ptr %14, align 4, !dbg !2131
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2131
  store i16 %57, ptr %59, align 8, !dbg !2131
  br label %93, !dbg !2131

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2131
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2131
  store ptr %62, ptr %6, align 4, !dbg !2131
  %63 = load i32, ptr %61, align 4, !dbg !2131
  %64 = load i32, ptr %14, align 4, !dbg !2131
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2131
  store i32 %63, ptr %65, align 8, !dbg !2131
  br label %93, !dbg !2131

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2131
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2131
  store ptr %68, ptr %6, align 4, !dbg !2131
  %69 = load i32, ptr %67, align 4, !dbg !2131
  %70 = sext i32 %69 to i64, !dbg !2131
  %71 = load i32, ptr %14, align 4, !dbg !2131
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2131
  store i64 %70, ptr %72, align 8, !dbg !2131
  br label %93, !dbg !2131

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2131
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !2131
  store ptr %75, ptr %6, align 4, !dbg !2131
  %76 = load double, ptr %74, align 4, !dbg !2131
  %77 = fptrunc double %76 to float, !dbg !2131
  %78 = load i32, ptr %14, align 4, !dbg !2131
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !2131
  store float %77, ptr %79, align 8, !dbg !2131
  br label %93, !dbg !2131

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !2131
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !2131
  store ptr %82, ptr %6, align 4, !dbg !2131
  %83 = load double, ptr %81, align 4, !dbg !2131
  %84 = load i32, ptr %14, align 4, !dbg !2131
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !2131
  store double %83, ptr %85, align 8, !dbg !2131
  br label %93, !dbg !2131

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !2131
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !2131
  store ptr %88, ptr %6, align 4, !dbg !2131
  %89 = load ptr, ptr %87, align 4, !dbg !2131
  %90 = load i32, ptr %14, align 4, !dbg !2131
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !2131
  store ptr %89, ptr %91, align 8, !dbg !2131
  br label %93, !dbg !2131

92:                                               ; preds = %27
  br label %93, !dbg !2131

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !2128

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !2133
  %96 = add nsw i32 %95, 1, !dbg !2133
  store i32 %96, ptr %14, align 4, !dbg !2133
  br label %23, !dbg !2133, !llvm.loop !2134

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !2117
  %99 = load ptr, ptr %98, align 4, !dbg !2117
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 90, !dbg !2117
  %101 = load ptr, ptr %100, align 4, !dbg !2117
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !2117
  %103 = load ptr, ptr %7, align 4, !dbg !2117
  %104 = load ptr, ptr %8, align 4, !dbg !2117
  %105 = load ptr, ptr %9, align 4, !dbg !2117
  %106 = load ptr, ptr %10, align 4, !dbg !2117
  %107 = call x86_stdcallcc double %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !2117
  ret double %107, !dbg !2117
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2135 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2136, metadata !DIExpression()), !dbg !2137
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2138, metadata !DIExpression()), !dbg !2137
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2139, metadata !DIExpression()), !dbg !2137
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2140, metadata !DIExpression()), !dbg !2137
  call void @llvm.va_start(ptr %7), !dbg !2137
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2141, metadata !DIExpression()), !dbg !2137
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2142, metadata !DIExpression()), !dbg !2137
  %13 = load ptr, ptr %6, align 4, !dbg !2137
  %14 = load ptr, ptr %13, align 4, !dbg !2137
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2137
  %16 = load ptr, ptr %15, align 4, !dbg !2137
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2137
  %18 = load ptr, ptr %4, align 4, !dbg !2137
  %19 = load ptr, ptr %6, align 4, !dbg !2137
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2137
  store i32 %20, ptr %9, align 4, !dbg !2137
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2143, metadata !DIExpression()), !dbg !2137
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2144, metadata !DIExpression()), !dbg !2146
  store i32 0, ptr %11, align 4, !dbg !2146
  br label %21, !dbg !2146

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !2146
  %23 = load i32, ptr %9, align 4, !dbg !2146
  %24 = icmp slt i32 %22, %23, !dbg !2146
  br i1 %24, label %25, label %95, !dbg !2146

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2147
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2147
  %28 = load i8, ptr %27, align 1, !dbg !2147
  %29 = sext i8 %28 to i32, !dbg !2147
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2147

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2150
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2150
  store ptr %32, ptr %7, align 4, !dbg !2150
  %33 = load i32, ptr %31, align 4, !dbg !2150
  %34 = trunc i32 %33 to i8, !dbg !2150
  %35 = load i32, ptr %11, align 4, !dbg !2150
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2150
  store i8 %34, ptr %36, align 8, !dbg !2150
  br label %91, !dbg !2150

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2150
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2150
  store ptr %39, ptr %7, align 4, !dbg !2150
  %40 = load i32, ptr %38, align 4, !dbg !2150
  %41 = trunc i32 %40 to i8, !dbg !2150
  %42 = load i32, ptr %11, align 4, !dbg !2150
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2150
  store i8 %41, ptr %43, align 8, !dbg !2150
  br label %91, !dbg !2150

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2150
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2150
  store ptr %46, ptr %7, align 4, !dbg !2150
  %47 = load i32, ptr %45, align 4, !dbg !2150
  %48 = trunc i32 %47 to i16, !dbg !2150
  %49 = load i32, ptr %11, align 4, !dbg !2150
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2150
  store i16 %48, ptr %50, align 8, !dbg !2150
  br label %91, !dbg !2150

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2150
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2150
  store ptr %53, ptr %7, align 4, !dbg !2150
  %54 = load i32, ptr %52, align 4, !dbg !2150
  %55 = trunc i32 %54 to i16, !dbg !2150
  %56 = load i32, ptr %11, align 4, !dbg !2150
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2150
  store i16 %55, ptr %57, align 8, !dbg !2150
  br label %91, !dbg !2150

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2150
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2150
  store ptr %60, ptr %7, align 4, !dbg !2150
  %61 = load i32, ptr %59, align 4, !dbg !2150
  %62 = load i32, ptr %11, align 4, !dbg !2150
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2150
  store i32 %61, ptr %63, align 8, !dbg !2150
  br label %91, !dbg !2150

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2150
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2150
  store ptr %66, ptr %7, align 4, !dbg !2150
  %67 = load i32, ptr %65, align 4, !dbg !2150
  %68 = sext i32 %67 to i64, !dbg !2150
  %69 = load i32, ptr %11, align 4, !dbg !2150
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2150
  store i64 %68, ptr %70, align 8, !dbg !2150
  br label %91, !dbg !2150

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2150
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2150
  store ptr %73, ptr %7, align 4, !dbg !2150
  %74 = load double, ptr %72, align 4, !dbg !2150
  %75 = fptrunc double %74 to float, !dbg !2150
  %76 = load i32, ptr %11, align 4, !dbg !2150
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !2150
  store float %75, ptr %77, align 8, !dbg !2150
  br label %91, !dbg !2150

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !2150
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2150
  store ptr %80, ptr %7, align 4, !dbg !2150
  %81 = load double, ptr %79, align 4, !dbg !2150
  %82 = load i32, ptr %11, align 4, !dbg !2150
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !2150
  store double %81, ptr %83, align 8, !dbg !2150
  br label %91, !dbg !2150

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !2150
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2150
  store ptr %86, ptr %7, align 4, !dbg !2150
  %87 = load ptr, ptr %85, align 4, !dbg !2150
  %88 = load i32, ptr %11, align 4, !dbg !2150
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !2150
  store ptr %87, ptr %89, align 8, !dbg !2150
  br label %91, !dbg !2150

90:                                               ; preds = %25
  br label %91, !dbg !2150

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2147

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !2152
  %94 = add nsw i32 %93, 1, !dbg !2152
  store i32 %94, ptr %11, align 4, !dbg !2152
  br label %21, !dbg !2152, !llvm.loop !2153

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2154, metadata !DIExpression()), !dbg !2137
  %96 = load ptr, ptr %6, align 4, !dbg !2137
  %97 = load ptr, ptr %96, align 4, !dbg !2137
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 140, !dbg !2137
  %99 = load ptr, ptr %98, align 4, !dbg !2137
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2137
  %101 = load ptr, ptr %4, align 4, !dbg !2137
  %102 = load ptr, ptr %5, align 4, !dbg !2137
  %103 = load ptr, ptr %6, align 4, !dbg !2137
  %104 = call x86_stdcallcc double %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2137
  store double %104, ptr %12, align 8, !dbg !2137
  call void @llvm.va_end(ptr %7), !dbg !2137
  %105 = load double, ptr %12, align 8, !dbg !2137
  ret double %105, !dbg !2137
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc double @"\01_JNI_CallStaticDoubleMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2155 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2156, metadata !DIExpression()), !dbg !2157
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2158, metadata !DIExpression()), !dbg !2157
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2159, metadata !DIExpression()), !dbg !2157
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2160, metadata !DIExpression()), !dbg !2157
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2161, metadata !DIExpression()), !dbg !2157
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2162, metadata !DIExpression()), !dbg !2157
  %13 = load ptr, ptr %8, align 4, !dbg !2157
  %14 = load ptr, ptr %13, align 4, !dbg !2157
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2157
  %16 = load ptr, ptr %15, align 4, !dbg !2157
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2157
  %18 = load ptr, ptr %6, align 4, !dbg !2157
  %19 = load ptr, ptr %8, align 4, !dbg !2157
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2157
  store i32 %20, ptr %10, align 4, !dbg !2157
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2163, metadata !DIExpression()), !dbg !2157
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2164, metadata !DIExpression()), !dbg !2166
  store i32 0, ptr %12, align 4, !dbg !2166
  br label %21, !dbg !2166

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2166
  %23 = load i32, ptr %10, align 4, !dbg !2166
  %24 = icmp slt i32 %22, %23, !dbg !2166
  br i1 %24, label %25, label %95, !dbg !2166

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2167
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2167
  %28 = load i8, ptr %27, align 1, !dbg !2167
  %29 = sext i8 %28 to i32, !dbg !2167
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2167

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2170
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2170
  store ptr %32, ptr %5, align 4, !dbg !2170
  %33 = load i32, ptr %31, align 4, !dbg !2170
  %34 = trunc i32 %33 to i8, !dbg !2170
  %35 = load i32, ptr %12, align 4, !dbg !2170
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2170
  store i8 %34, ptr %36, align 8, !dbg !2170
  br label %91, !dbg !2170

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2170
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2170
  store ptr %39, ptr %5, align 4, !dbg !2170
  %40 = load i32, ptr %38, align 4, !dbg !2170
  %41 = trunc i32 %40 to i8, !dbg !2170
  %42 = load i32, ptr %12, align 4, !dbg !2170
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2170
  store i8 %41, ptr %43, align 8, !dbg !2170
  br label %91, !dbg !2170

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2170
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2170
  store ptr %46, ptr %5, align 4, !dbg !2170
  %47 = load i32, ptr %45, align 4, !dbg !2170
  %48 = trunc i32 %47 to i16, !dbg !2170
  %49 = load i32, ptr %12, align 4, !dbg !2170
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2170
  store i16 %48, ptr %50, align 8, !dbg !2170
  br label %91, !dbg !2170

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2170
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2170
  store ptr %53, ptr %5, align 4, !dbg !2170
  %54 = load i32, ptr %52, align 4, !dbg !2170
  %55 = trunc i32 %54 to i16, !dbg !2170
  %56 = load i32, ptr %12, align 4, !dbg !2170
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2170
  store i16 %55, ptr %57, align 8, !dbg !2170
  br label %91, !dbg !2170

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2170
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2170
  store ptr %60, ptr %5, align 4, !dbg !2170
  %61 = load i32, ptr %59, align 4, !dbg !2170
  %62 = load i32, ptr %12, align 4, !dbg !2170
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2170
  store i32 %61, ptr %63, align 8, !dbg !2170
  br label %91, !dbg !2170

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2170
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2170
  store ptr %66, ptr %5, align 4, !dbg !2170
  %67 = load i32, ptr %65, align 4, !dbg !2170
  %68 = sext i32 %67 to i64, !dbg !2170
  %69 = load i32, ptr %12, align 4, !dbg !2170
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2170
  store i64 %68, ptr %70, align 8, !dbg !2170
  br label %91, !dbg !2170

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2170
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2170
  store ptr %73, ptr %5, align 4, !dbg !2170
  %74 = load double, ptr %72, align 4, !dbg !2170
  %75 = fptrunc double %74 to float, !dbg !2170
  %76 = load i32, ptr %12, align 4, !dbg !2170
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2170
  store float %75, ptr %77, align 8, !dbg !2170
  br label %91, !dbg !2170

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2170
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2170
  store ptr %80, ptr %5, align 4, !dbg !2170
  %81 = load double, ptr %79, align 4, !dbg !2170
  %82 = load i32, ptr %12, align 4, !dbg !2170
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2170
  store double %81, ptr %83, align 8, !dbg !2170
  br label %91, !dbg !2170

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2170
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2170
  store ptr %86, ptr %5, align 4, !dbg !2170
  %87 = load ptr, ptr %85, align 4, !dbg !2170
  %88 = load i32, ptr %12, align 4, !dbg !2170
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2170
  store ptr %87, ptr %89, align 8, !dbg !2170
  br label %91, !dbg !2170

90:                                               ; preds = %25
  br label %91, !dbg !2170

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2167

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2172
  %94 = add nsw i32 %93, 1, !dbg !2172
  store i32 %94, ptr %12, align 4, !dbg !2172
  br label %21, !dbg !2172, !llvm.loop !2173

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2157
  %97 = load ptr, ptr %96, align 4, !dbg !2157
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 140, !dbg !2157
  %99 = load ptr, ptr %98, align 4, !dbg !2157
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2157
  %101 = load ptr, ptr %6, align 4, !dbg !2157
  %102 = load ptr, ptr %7, align 4, !dbg !2157
  %103 = load ptr, ptr %8, align 4, !dbg !2157
  %104 = call x86_stdcallcc double %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2157
  ret double %104, !dbg !2157
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2174 {
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
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2175, metadata !DIExpression()), !dbg !2176
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2177, metadata !DIExpression()), !dbg !2176
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2178, metadata !DIExpression()), !dbg !2176
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2179, metadata !DIExpression()), !dbg !2180
  call void @llvm.va_start(ptr %7), !dbg !2181
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2182, metadata !DIExpression()), !dbg !2183
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2184, metadata !DIExpression()), !dbg !2183
  %13 = load ptr, ptr %6, align 4, !dbg !2183
  %14 = load ptr, ptr %13, align 4, !dbg !2183
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2183
  %16 = load ptr, ptr %15, align 4, !dbg !2183
  %17 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2183
  %18 = load ptr, ptr %4, align 4, !dbg !2183
  %19 = load ptr, ptr %6, align 4, !dbg !2183
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2183
  store i32 %20, ptr %9, align 4, !dbg !2183
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2185, metadata !DIExpression()), !dbg !2183
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2186, metadata !DIExpression()), !dbg !2188
  store i32 0, ptr %11, align 4, !dbg !2188
  br label %21, !dbg !2188

21:                                               ; preds = %92, %3
  %22 = load i32, ptr %11, align 4, !dbg !2188
  %23 = load i32, ptr %9, align 4, !dbg !2188
  %24 = icmp slt i32 %22, %23, !dbg !2188
  br i1 %24, label %25, label %95, !dbg !2188

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2189
  %27 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %26, !dbg !2189
  %28 = load i8, ptr %27, align 1, !dbg !2189
  %29 = sext i8 %28 to i32, !dbg !2189
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2189

30:                                               ; preds = %25
  %31 = load ptr, ptr %7, align 4, !dbg !2192
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2192
  store ptr %32, ptr %7, align 4, !dbg !2192
  %33 = load i32, ptr %31, align 4, !dbg !2192
  %34 = trunc i32 %33 to i8, !dbg !2192
  %35 = load i32, ptr %11, align 4, !dbg !2192
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %35, !dbg !2192
  store i8 %34, ptr %36, align 8, !dbg !2192
  br label %91, !dbg !2192

37:                                               ; preds = %25
  %38 = load ptr, ptr %7, align 4, !dbg !2192
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2192
  store ptr %39, ptr %7, align 4, !dbg !2192
  %40 = load i32, ptr %38, align 4, !dbg !2192
  %41 = trunc i32 %40 to i8, !dbg !2192
  %42 = load i32, ptr %11, align 4, !dbg !2192
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %42, !dbg !2192
  store i8 %41, ptr %43, align 8, !dbg !2192
  br label %91, !dbg !2192

44:                                               ; preds = %25
  %45 = load ptr, ptr %7, align 4, !dbg !2192
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2192
  store ptr %46, ptr %7, align 4, !dbg !2192
  %47 = load i32, ptr %45, align 4, !dbg !2192
  %48 = trunc i32 %47 to i16, !dbg !2192
  %49 = load i32, ptr %11, align 4, !dbg !2192
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %49, !dbg !2192
  store i16 %48, ptr %50, align 8, !dbg !2192
  br label %91, !dbg !2192

51:                                               ; preds = %25
  %52 = load ptr, ptr %7, align 4, !dbg !2192
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2192
  store ptr %53, ptr %7, align 4, !dbg !2192
  %54 = load i32, ptr %52, align 4, !dbg !2192
  %55 = trunc i32 %54 to i16, !dbg !2192
  %56 = load i32, ptr %11, align 4, !dbg !2192
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %56, !dbg !2192
  store i16 %55, ptr %57, align 8, !dbg !2192
  br label %91, !dbg !2192

58:                                               ; preds = %25
  %59 = load ptr, ptr %7, align 4, !dbg !2192
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2192
  store ptr %60, ptr %7, align 4, !dbg !2192
  %61 = load i32, ptr %59, align 4, !dbg !2192
  %62 = load i32, ptr %11, align 4, !dbg !2192
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %62, !dbg !2192
  store i32 %61, ptr %63, align 8, !dbg !2192
  br label %91, !dbg !2192

64:                                               ; preds = %25
  %65 = load ptr, ptr %7, align 4, !dbg !2192
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2192
  store ptr %66, ptr %7, align 4, !dbg !2192
  %67 = load i32, ptr %65, align 4, !dbg !2192
  %68 = sext i32 %67 to i64, !dbg !2192
  %69 = load i32, ptr %11, align 4, !dbg !2192
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %69, !dbg !2192
  store i64 %68, ptr %70, align 8, !dbg !2192
  br label %91, !dbg !2192

71:                                               ; preds = %25
  %72 = load ptr, ptr %7, align 4, !dbg !2192
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2192
  store ptr %73, ptr %7, align 4, !dbg !2192
  %74 = load double, ptr %72, align 4, !dbg !2192
  %75 = fptrunc double %74 to float, !dbg !2192
  %76 = load i32, ptr %11, align 4, !dbg !2192
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %76, !dbg !2192
  store float %75, ptr %77, align 8, !dbg !2192
  br label %91, !dbg !2192

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 4, !dbg !2192
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2192
  store ptr %80, ptr %7, align 4, !dbg !2192
  %81 = load double, ptr %79, align 4, !dbg !2192
  %82 = load i32, ptr %11, align 4, !dbg !2192
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %82, !dbg !2192
  store double %81, ptr %83, align 8, !dbg !2192
  br label %91, !dbg !2192

84:                                               ; preds = %25
  %85 = load ptr, ptr %7, align 4, !dbg !2192
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2192
  store ptr %86, ptr %7, align 4, !dbg !2192
  %87 = load ptr, ptr %85, align 4, !dbg !2192
  %88 = load i32, ptr %11, align 4, !dbg !2192
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %88, !dbg !2192
  store ptr %87, ptr %89, align 8, !dbg !2192
  br label %91, !dbg !2192

90:                                               ; preds = %25
  br label %91, !dbg !2192

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2189

92:                                               ; preds = %91
  %93 = load i32, ptr %11, align 4, !dbg !2194
  %94 = add nsw i32 %93, 1, !dbg !2194
  store i32 %94, ptr %11, align 4, !dbg !2194
  br label %21, !dbg !2194, !llvm.loop !2195

95:                                               ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2196, metadata !DIExpression()), !dbg !2197
  %96 = load ptr, ptr %6, align 4, !dbg !2197
  %97 = load ptr, ptr %96, align 4, !dbg !2197
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 30, !dbg !2197
  %99 = load ptr, ptr %98, align 4, !dbg !2197
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2197
  %101 = load ptr, ptr %4, align 4, !dbg !2197
  %102 = load ptr, ptr %5, align 4, !dbg !2197
  %103 = load ptr, ptr %6, align 4, !dbg !2197
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2197
  store ptr %104, ptr %12, align 4, !dbg !2197
  call void @llvm.va_end(ptr %7), !dbg !2198
  %105 = load ptr, ptr %12, align 4, !dbg !2199
  ret ptr %105, !dbg !2199
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc ptr @"\01_JNI_NewObjectV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2200 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2201, metadata !DIExpression()), !dbg !2202
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2203, metadata !DIExpression()), !dbg !2202
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2204, metadata !DIExpression()), !dbg !2202
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2205, metadata !DIExpression()), !dbg !2202
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2206, metadata !DIExpression()), !dbg !2207
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2208, metadata !DIExpression()), !dbg !2207
  %13 = load ptr, ptr %8, align 4, !dbg !2207
  %14 = load ptr, ptr %13, align 4, !dbg !2207
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2207
  %16 = load ptr, ptr %15, align 4, !dbg !2207
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2207
  %18 = load ptr, ptr %6, align 4, !dbg !2207
  %19 = load ptr, ptr %8, align 4, !dbg !2207
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2207
  store i32 %20, ptr %10, align 4, !dbg !2207
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2209, metadata !DIExpression()), !dbg !2207
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2210, metadata !DIExpression()), !dbg !2212
  store i32 0, ptr %12, align 4, !dbg !2212
  br label %21, !dbg !2212

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2212
  %23 = load i32, ptr %10, align 4, !dbg !2212
  %24 = icmp slt i32 %22, %23, !dbg !2212
  br i1 %24, label %25, label %95, !dbg !2212

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2213
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2213
  %28 = load i8, ptr %27, align 1, !dbg !2213
  %29 = sext i8 %28 to i32, !dbg !2213
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2213

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2216
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2216
  store ptr %32, ptr %5, align 4, !dbg !2216
  %33 = load i32, ptr %31, align 4, !dbg !2216
  %34 = trunc i32 %33 to i8, !dbg !2216
  %35 = load i32, ptr %12, align 4, !dbg !2216
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2216
  store i8 %34, ptr %36, align 8, !dbg !2216
  br label %91, !dbg !2216

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2216
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2216
  store ptr %39, ptr %5, align 4, !dbg !2216
  %40 = load i32, ptr %38, align 4, !dbg !2216
  %41 = trunc i32 %40 to i8, !dbg !2216
  %42 = load i32, ptr %12, align 4, !dbg !2216
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2216
  store i8 %41, ptr %43, align 8, !dbg !2216
  br label %91, !dbg !2216

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2216
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2216
  store ptr %46, ptr %5, align 4, !dbg !2216
  %47 = load i32, ptr %45, align 4, !dbg !2216
  %48 = trunc i32 %47 to i16, !dbg !2216
  %49 = load i32, ptr %12, align 4, !dbg !2216
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2216
  store i16 %48, ptr %50, align 8, !dbg !2216
  br label %91, !dbg !2216

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2216
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2216
  store ptr %53, ptr %5, align 4, !dbg !2216
  %54 = load i32, ptr %52, align 4, !dbg !2216
  %55 = trunc i32 %54 to i16, !dbg !2216
  %56 = load i32, ptr %12, align 4, !dbg !2216
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2216
  store i16 %55, ptr %57, align 8, !dbg !2216
  br label %91, !dbg !2216

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2216
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2216
  store ptr %60, ptr %5, align 4, !dbg !2216
  %61 = load i32, ptr %59, align 4, !dbg !2216
  %62 = load i32, ptr %12, align 4, !dbg !2216
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2216
  store i32 %61, ptr %63, align 8, !dbg !2216
  br label %91, !dbg !2216

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2216
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2216
  store ptr %66, ptr %5, align 4, !dbg !2216
  %67 = load i32, ptr %65, align 4, !dbg !2216
  %68 = sext i32 %67 to i64, !dbg !2216
  %69 = load i32, ptr %12, align 4, !dbg !2216
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2216
  store i64 %68, ptr %70, align 8, !dbg !2216
  br label %91, !dbg !2216

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2216
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2216
  store ptr %73, ptr %5, align 4, !dbg !2216
  %74 = load double, ptr %72, align 4, !dbg !2216
  %75 = fptrunc double %74 to float, !dbg !2216
  %76 = load i32, ptr %12, align 4, !dbg !2216
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2216
  store float %75, ptr %77, align 8, !dbg !2216
  br label %91, !dbg !2216

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2216
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2216
  store ptr %80, ptr %5, align 4, !dbg !2216
  %81 = load double, ptr %79, align 4, !dbg !2216
  %82 = load i32, ptr %12, align 4, !dbg !2216
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2216
  store double %81, ptr %83, align 8, !dbg !2216
  br label %91, !dbg !2216

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2216
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2216
  store ptr %86, ptr %5, align 4, !dbg !2216
  %87 = load ptr, ptr %85, align 4, !dbg !2216
  %88 = load i32, ptr %12, align 4, !dbg !2216
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2216
  store ptr %87, ptr %89, align 8, !dbg !2216
  br label %91, !dbg !2216

90:                                               ; preds = %25
  br label %91, !dbg !2216

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2213

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2218
  %94 = add nsw i32 %93, 1, !dbg !2218
  store i32 %94, ptr %12, align 4, !dbg !2218
  br label %21, !dbg !2218, !llvm.loop !2219

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2220
  %97 = load ptr, ptr %96, align 4, !dbg !2220
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 30, !dbg !2220
  %99 = load ptr, ptr %98, align 4, !dbg !2220
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2220
  %101 = load ptr, ptr %6, align 4, !dbg !2220
  %102 = load ptr, ptr %7, align 4, !dbg !2220
  %103 = load ptr, ptr %8, align 4, !dbg !2220
  %104 = call x86_stdcallcc ptr %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2220
  ret ptr %104, !dbg !2220
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2221 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2222, metadata !DIExpression()), !dbg !2223
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2224, metadata !DIExpression()), !dbg !2223
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2225, metadata !DIExpression()), !dbg !2223
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2226, metadata !DIExpression()), !dbg !2227
  call void @llvm.va_start(ptr %7), !dbg !2228
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2229, metadata !DIExpression()), !dbg !2230
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2231, metadata !DIExpression()), !dbg !2230
  %12 = load ptr, ptr %6, align 4, !dbg !2230
  %13 = load ptr, ptr %12, align 4, !dbg !2230
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2230
  %15 = load ptr, ptr %14, align 4, !dbg !2230
  %16 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2230
  %17 = load ptr, ptr %4, align 4, !dbg !2230
  %18 = load ptr, ptr %6, align 4, !dbg !2230
  %19 = call i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2230
  store i32 %19, ptr %9, align 4, !dbg !2230
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2232, metadata !DIExpression()), !dbg !2230
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2233, metadata !DIExpression()), !dbg !2235
  store i32 0, ptr %11, align 4, !dbg !2235
  br label %20, !dbg !2235

20:                                               ; preds = %91, %3
  %21 = load i32, ptr %11, align 4, !dbg !2235
  %22 = load i32, ptr %9, align 4, !dbg !2235
  %23 = icmp slt i32 %21, %22, !dbg !2235
  br i1 %23, label %24, label %94, !dbg !2235

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2236
  %26 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %25, !dbg !2236
  %27 = load i8, ptr %26, align 1, !dbg !2236
  %28 = sext i8 %27 to i32, !dbg !2236
  switch i32 %28, label %89 [
    i32 90, label %29
    i32 66, label %36
    i32 67, label %43
    i32 83, label %50
    i32 73, label %57
    i32 74, label %63
    i32 70, label %70
    i32 68, label %77
    i32 76, label %83
  ], !dbg !2236

29:                                               ; preds = %24
  %30 = load ptr, ptr %7, align 4, !dbg !2239
  %31 = getelementptr inbounds i8, ptr %30, i32 4, !dbg !2239
  store ptr %31, ptr %7, align 4, !dbg !2239
  %32 = load i32, ptr %30, align 4, !dbg !2239
  %33 = trunc i32 %32 to i8, !dbg !2239
  %34 = load i32, ptr %11, align 4, !dbg !2239
  %35 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %34, !dbg !2239
  store i8 %33, ptr %35, align 8, !dbg !2239
  br label %90, !dbg !2239

36:                                               ; preds = %24
  %37 = load ptr, ptr %7, align 4, !dbg !2239
  %38 = getelementptr inbounds i8, ptr %37, i32 4, !dbg !2239
  store ptr %38, ptr %7, align 4, !dbg !2239
  %39 = load i32, ptr %37, align 4, !dbg !2239
  %40 = trunc i32 %39 to i8, !dbg !2239
  %41 = load i32, ptr %11, align 4, !dbg !2239
  %42 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %41, !dbg !2239
  store i8 %40, ptr %42, align 8, !dbg !2239
  br label %90, !dbg !2239

43:                                               ; preds = %24
  %44 = load ptr, ptr %7, align 4, !dbg !2239
  %45 = getelementptr inbounds i8, ptr %44, i32 4, !dbg !2239
  store ptr %45, ptr %7, align 4, !dbg !2239
  %46 = load i32, ptr %44, align 4, !dbg !2239
  %47 = trunc i32 %46 to i16, !dbg !2239
  %48 = load i32, ptr %11, align 4, !dbg !2239
  %49 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %48, !dbg !2239
  store i16 %47, ptr %49, align 8, !dbg !2239
  br label %90, !dbg !2239

50:                                               ; preds = %24
  %51 = load ptr, ptr %7, align 4, !dbg !2239
  %52 = getelementptr inbounds i8, ptr %51, i32 4, !dbg !2239
  store ptr %52, ptr %7, align 4, !dbg !2239
  %53 = load i32, ptr %51, align 4, !dbg !2239
  %54 = trunc i32 %53 to i16, !dbg !2239
  %55 = load i32, ptr %11, align 4, !dbg !2239
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %55, !dbg !2239
  store i16 %54, ptr %56, align 8, !dbg !2239
  br label %90, !dbg !2239

57:                                               ; preds = %24
  %58 = load ptr, ptr %7, align 4, !dbg !2239
  %59 = getelementptr inbounds i8, ptr %58, i32 4, !dbg !2239
  store ptr %59, ptr %7, align 4, !dbg !2239
  %60 = load i32, ptr %58, align 4, !dbg !2239
  %61 = load i32, ptr %11, align 4, !dbg !2239
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %61, !dbg !2239
  store i32 %60, ptr %62, align 8, !dbg !2239
  br label %90, !dbg !2239

63:                                               ; preds = %24
  %64 = load ptr, ptr %7, align 4, !dbg !2239
  %65 = getelementptr inbounds i8, ptr %64, i32 4, !dbg !2239
  store ptr %65, ptr %7, align 4, !dbg !2239
  %66 = load i32, ptr %64, align 4, !dbg !2239
  %67 = sext i32 %66 to i64, !dbg !2239
  %68 = load i32, ptr %11, align 4, !dbg !2239
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %68, !dbg !2239
  store i64 %67, ptr %69, align 8, !dbg !2239
  br label %90, !dbg !2239

70:                                               ; preds = %24
  %71 = load ptr, ptr %7, align 4, !dbg !2239
  %72 = getelementptr inbounds i8, ptr %71, i32 8, !dbg !2239
  store ptr %72, ptr %7, align 4, !dbg !2239
  %73 = load double, ptr %71, align 4, !dbg !2239
  %74 = fptrunc double %73 to float, !dbg !2239
  %75 = load i32, ptr %11, align 4, !dbg !2239
  %76 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %75, !dbg !2239
  store float %74, ptr %76, align 8, !dbg !2239
  br label %90, !dbg !2239

77:                                               ; preds = %24
  %78 = load ptr, ptr %7, align 4, !dbg !2239
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2239
  store ptr %79, ptr %7, align 4, !dbg !2239
  %80 = load double, ptr %78, align 4, !dbg !2239
  %81 = load i32, ptr %11, align 4, !dbg !2239
  %82 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %81, !dbg !2239
  store double %80, ptr %82, align 8, !dbg !2239
  br label %90, !dbg !2239

83:                                               ; preds = %24
  %84 = load ptr, ptr %7, align 4, !dbg !2239
  %85 = getelementptr inbounds i8, ptr %84, i32 4, !dbg !2239
  store ptr %85, ptr %7, align 4, !dbg !2239
  %86 = load ptr, ptr %84, align 4, !dbg !2239
  %87 = load i32, ptr %11, align 4, !dbg !2239
  %88 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %87, !dbg !2239
  store ptr %86, ptr %88, align 8, !dbg !2239
  br label %90, !dbg !2239

89:                                               ; preds = %24
  br label %90, !dbg !2239

90:                                               ; preds = %89, %83, %77, %70, %63, %57, %50, %43, %36, %29
  br label %91, !dbg !2236

91:                                               ; preds = %90
  %92 = load i32, ptr %11, align 4, !dbg !2241
  %93 = add nsw i32 %92, 1, !dbg !2241
  store i32 %93, ptr %11, align 4, !dbg !2241
  br label %20, !dbg !2241, !llvm.loop !2242

94:                                               ; preds = %20
  %95 = load ptr, ptr %6, align 4, !dbg !2243
  %96 = load ptr, ptr %95, align 4, !dbg !2243
  %97 = getelementptr inbounds %struct.JNINativeInterface_, ptr %96, i32 0, i32 63, !dbg !2243
  %98 = load ptr, ptr %97, align 4, !dbg !2243
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2243
  %100 = load ptr, ptr %4, align 4, !dbg !2243
  %101 = load ptr, ptr %5, align 4, !dbg !2243
  %102 = load ptr, ptr %6, align 4, !dbg !2243
  call x86_stdcallcc void %98(ptr noundef %102, ptr noundef %101, ptr noundef %100, ptr noundef %99), !dbg !2243
  call void @llvm.va_end(ptr %7), !dbg !2244
  ret void, !dbg !2245
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc void @"\01_JNI_CallVoidMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2246 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2247, metadata !DIExpression()), !dbg !2248
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2249, metadata !DIExpression()), !dbg !2248
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2250, metadata !DIExpression()), !dbg !2248
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2251, metadata !DIExpression()), !dbg !2248
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2252, metadata !DIExpression()), !dbg !2253
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2254, metadata !DIExpression()), !dbg !2253
  %13 = load ptr, ptr %8, align 4, !dbg !2253
  %14 = load ptr, ptr %13, align 4, !dbg !2253
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2253
  %16 = load ptr, ptr %15, align 4, !dbg !2253
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2253
  %18 = load ptr, ptr %6, align 4, !dbg !2253
  %19 = load ptr, ptr %8, align 4, !dbg !2253
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2253
  store i32 %20, ptr %10, align 4, !dbg !2253
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2255, metadata !DIExpression()), !dbg !2253
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2256, metadata !DIExpression()), !dbg !2258
  store i32 0, ptr %12, align 4, !dbg !2258
  br label %21, !dbg !2258

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2258
  %23 = load i32, ptr %10, align 4, !dbg !2258
  %24 = icmp slt i32 %22, %23, !dbg !2258
  br i1 %24, label %25, label %95, !dbg !2258

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2259
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2259
  %28 = load i8, ptr %27, align 1, !dbg !2259
  %29 = sext i8 %28 to i32, !dbg !2259
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2259

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2262
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2262
  store ptr %32, ptr %5, align 4, !dbg !2262
  %33 = load i32, ptr %31, align 4, !dbg !2262
  %34 = trunc i32 %33 to i8, !dbg !2262
  %35 = load i32, ptr %12, align 4, !dbg !2262
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2262
  store i8 %34, ptr %36, align 8, !dbg !2262
  br label %91, !dbg !2262

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2262
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2262
  store ptr %39, ptr %5, align 4, !dbg !2262
  %40 = load i32, ptr %38, align 4, !dbg !2262
  %41 = trunc i32 %40 to i8, !dbg !2262
  %42 = load i32, ptr %12, align 4, !dbg !2262
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2262
  store i8 %41, ptr %43, align 8, !dbg !2262
  br label %91, !dbg !2262

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2262
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2262
  store ptr %46, ptr %5, align 4, !dbg !2262
  %47 = load i32, ptr %45, align 4, !dbg !2262
  %48 = trunc i32 %47 to i16, !dbg !2262
  %49 = load i32, ptr %12, align 4, !dbg !2262
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2262
  store i16 %48, ptr %50, align 8, !dbg !2262
  br label %91, !dbg !2262

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2262
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2262
  store ptr %53, ptr %5, align 4, !dbg !2262
  %54 = load i32, ptr %52, align 4, !dbg !2262
  %55 = trunc i32 %54 to i16, !dbg !2262
  %56 = load i32, ptr %12, align 4, !dbg !2262
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2262
  store i16 %55, ptr %57, align 8, !dbg !2262
  br label %91, !dbg !2262

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2262
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2262
  store ptr %60, ptr %5, align 4, !dbg !2262
  %61 = load i32, ptr %59, align 4, !dbg !2262
  %62 = load i32, ptr %12, align 4, !dbg !2262
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2262
  store i32 %61, ptr %63, align 8, !dbg !2262
  br label %91, !dbg !2262

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2262
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2262
  store ptr %66, ptr %5, align 4, !dbg !2262
  %67 = load i32, ptr %65, align 4, !dbg !2262
  %68 = sext i32 %67 to i64, !dbg !2262
  %69 = load i32, ptr %12, align 4, !dbg !2262
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2262
  store i64 %68, ptr %70, align 8, !dbg !2262
  br label %91, !dbg !2262

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2262
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2262
  store ptr %73, ptr %5, align 4, !dbg !2262
  %74 = load double, ptr %72, align 4, !dbg !2262
  %75 = fptrunc double %74 to float, !dbg !2262
  %76 = load i32, ptr %12, align 4, !dbg !2262
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2262
  store float %75, ptr %77, align 8, !dbg !2262
  br label %91, !dbg !2262

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2262
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2262
  store ptr %80, ptr %5, align 4, !dbg !2262
  %81 = load double, ptr %79, align 4, !dbg !2262
  %82 = load i32, ptr %12, align 4, !dbg !2262
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2262
  store double %81, ptr %83, align 8, !dbg !2262
  br label %91, !dbg !2262

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2262
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2262
  store ptr %86, ptr %5, align 4, !dbg !2262
  %87 = load ptr, ptr %85, align 4, !dbg !2262
  %88 = load i32, ptr %12, align 4, !dbg !2262
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2262
  store ptr %87, ptr %89, align 8, !dbg !2262
  br label %91, !dbg !2262

90:                                               ; preds = %25
  br label %91, !dbg !2262

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2259

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2264
  %94 = add nsw i32 %93, 1, !dbg !2264
  store i32 %94, ptr %12, align 4, !dbg !2264
  br label %21, !dbg !2264, !llvm.loop !2265

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2266
  %97 = load ptr, ptr %96, align 4, !dbg !2266
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 63, !dbg !2266
  %99 = load ptr, ptr %98, align 4, !dbg !2266
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2266
  %101 = load ptr, ptr %6, align 4, !dbg !2266
  %102 = load ptr, ptr %7, align 4, !dbg !2266
  %103 = load ptr, ptr %8, align 4, !dbg !2266
  call x86_stdcallcc void %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2266
  ret void, !dbg !2267
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2268 {
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
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2269, metadata !DIExpression()), !dbg !2270
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2271, metadata !DIExpression()), !dbg !2270
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2272, metadata !DIExpression()), !dbg !2270
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2273, metadata !DIExpression()), !dbg !2270
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2274, metadata !DIExpression()), !dbg !2275
  call void @llvm.va_start(ptr %9), !dbg !2276
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2277, metadata !DIExpression()), !dbg !2278
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2279, metadata !DIExpression()), !dbg !2278
  %14 = load ptr, ptr %8, align 4, !dbg !2278
  %15 = load ptr, ptr %14, align 4, !dbg !2278
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0, !dbg !2278
  %17 = load ptr, ptr %16, align 4, !dbg !2278
  %18 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 0, !dbg !2278
  %19 = load ptr, ptr %5, align 4, !dbg !2278
  %20 = load ptr, ptr %8, align 4, !dbg !2278
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18), !dbg !2278
  store i32 %21, ptr %11, align 4, !dbg !2278
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2280, metadata !DIExpression()), !dbg !2278
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2281, metadata !DIExpression()), !dbg !2283
  store i32 0, ptr %13, align 4, !dbg !2283
  br label %22, !dbg !2283

22:                                               ; preds = %93, %4
  %23 = load i32, ptr %13, align 4, !dbg !2283
  %24 = load i32, ptr %11, align 4, !dbg !2283
  %25 = icmp slt i32 %23, %24, !dbg !2283
  br i1 %25, label %26, label %96, !dbg !2283

26:                                               ; preds = %22
  %27 = load i32, ptr %13, align 4, !dbg !2284
  %28 = getelementptr inbounds [256 x i8], ptr %10, i32 0, i32 %27, !dbg !2284
  %29 = load i8, ptr %28, align 1, !dbg !2284
  %30 = sext i8 %29 to i32, !dbg !2284
  switch i32 %30, label %91 [
    i32 90, label %31
    i32 66, label %38
    i32 67, label %45
    i32 83, label %52
    i32 73, label %59
    i32 74, label %65
    i32 70, label %72
    i32 68, label %79
    i32 76, label %85
  ], !dbg !2284

31:                                               ; preds = %26
  %32 = load ptr, ptr %9, align 4, !dbg !2287
  %33 = getelementptr inbounds i8, ptr %32, i32 4, !dbg !2287
  store ptr %33, ptr %9, align 4, !dbg !2287
  %34 = load i32, ptr %32, align 4, !dbg !2287
  %35 = trunc i32 %34 to i8, !dbg !2287
  %36 = load i32, ptr %13, align 4, !dbg !2287
  %37 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %36, !dbg !2287
  store i8 %35, ptr %37, align 8, !dbg !2287
  br label %92, !dbg !2287

38:                                               ; preds = %26
  %39 = load ptr, ptr %9, align 4, !dbg !2287
  %40 = getelementptr inbounds i8, ptr %39, i32 4, !dbg !2287
  store ptr %40, ptr %9, align 4, !dbg !2287
  %41 = load i32, ptr %39, align 4, !dbg !2287
  %42 = trunc i32 %41 to i8, !dbg !2287
  %43 = load i32, ptr %13, align 4, !dbg !2287
  %44 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %43, !dbg !2287
  store i8 %42, ptr %44, align 8, !dbg !2287
  br label %92, !dbg !2287

45:                                               ; preds = %26
  %46 = load ptr, ptr %9, align 4, !dbg !2287
  %47 = getelementptr inbounds i8, ptr %46, i32 4, !dbg !2287
  store ptr %47, ptr %9, align 4, !dbg !2287
  %48 = load i32, ptr %46, align 4, !dbg !2287
  %49 = trunc i32 %48 to i16, !dbg !2287
  %50 = load i32, ptr %13, align 4, !dbg !2287
  %51 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %50, !dbg !2287
  store i16 %49, ptr %51, align 8, !dbg !2287
  br label %92, !dbg !2287

52:                                               ; preds = %26
  %53 = load ptr, ptr %9, align 4, !dbg !2287
  %54 = getelementptr inbounds i8, ptr %53, i32 4, !dbg !2287
  store ptr %54, ptr %9, align 4, !dbg !2287
  %55 = load i32, ptr %53, align 4, !dbg !2287
  %56 = trunc i32 %55 to i16, !dbg !2287
  %57 = load i32, ptr %13, align 4, !dbg !2287
  %58 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %57, !dbg !2287
  store i16 %56, ptr %58, align 8, !dbg !2287
  br label %92, !dbg !2287

59:                                               ; preds = %26
  %60 = load ptr, ptr %9, align 4, !dbg !2287
  %61 = getelementptr inbounds i8, ptr %60, i32 4, !dbg !2287
  store ptr %61, ptr %9, align 4, !dbg !2287
  %62 = load i32, ptr %60, align 4, !dbg !2287
  %63 = load i32, ptr %13, align 4, !dbg !2287
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %63, !dbg !2287
  store i32 %62, ptr %64, align 8, !dbg !2287
  br label %92, !dbg !2287

65:                                               ; preds = %26
  %66 = load ptr, ptr %9, align 4, !dbg !2287
  %67 = getelementptr inbounds i8, ptr %66, i32 4, !dbg !2287
  store ptr %67, ptr %9, align 4, !dbg !2287
  %68 = load i32, ptr %66, align 4, !dbg !2287
  %69 = sext i32 %68 to i64, !dbg !2287
  %70 = load i32, ptr %13, align 4, !dbg !2287
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %70, !dbg !2287
  store i64 %69, ptr %71, align 8, !dbg !2287
  br label %92, !dbg !2287

72:                                               ; preds = %26
  %73 = load ptr, ptr %9, align 4, !dbg !2287
  %74 = getelementptr inbounds i8, ptr %73, i32 8, !dbg !2287
  store ptr %74, ptr %9, align 4, !dbg !2287
  %75 = load double, ptr %73, align 4, !dbg !2287
  %76 = fptrunc double %75 to float, !dbg !2287
  %77 = load i32, ptr %13, align 4, !dbg !2287
  %78 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %77, !dbg !2287
  store float %76, ptr %78, align 8, !dbg !2287
  br label %92, !dbg !2287

79:                                               ; preds = %26
  %80 = load ptr, ptr %9, align 4, !dbg !2287
  %81 = getelementptr inbounds i8, ptr %80, i32 8, !dbg !2287
  store ptr %81, ptr %9, align 4, !dbg !2287
  %82 = load double, ptr %80, align 4, !dbg !2287
  %83 = load i32, ptr %13, align 4, !dbg !2287
  %84 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %83, !dbg !2287
  store double %82, ptr %84, align 8, !dbg !2287
  br label %92, !dbg !2287

85:                                               ; preds = %26
  %86 = load ptr, ptr %9, align 4, !dbg !2287
  %87 = getelementptr inbounds i8, ptr %86, i32 4, !dbg !2287
  store ptr %87, ptr %9, align 4, !dbg !2287
  %88 = load ptr, ptr %86, align 4, !dbg !2287
  %89 = load i32, ptr %13, align 4, !dbg !2287
  %90 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 %89, !dbg !2287
  store ptr %88, ptr %90, align 8, !dbg !2287
  br label %92, !dbg !2287

91:                                               ; preds = %26
  br label %92, !dbg !2287

92:                                               ; preds = %91, %85, %79, %72, %65, %59, %52, %45, %38, %31
  br label %93, !dbg !2284

93:                                               ; preds = %92
  %94 = load i32, ptr %13, align 4, !dbg !2289
  %95 = add nsw i32 %94, 1, !dbg !2289
  store i32 %95, ptr %13, align 4, !dbg !2289
  br label %22, !dbg !2289, !llvm.loop !2290

96:                                               ; preds = %22
  %97 = load ptr, ptr %8, align 4, !dbg !2291
  %98 = load ptr, ptr %97, align 4, !dbg !2291
  %99 = getelementptr inbounds %struct.JNINativeInterface_, ptr %98, i32 0, i32 93, !dbg !2291
  %100 = load ptr, ptr %99, align 4, !dbg !2291
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i32 0, i32 0, !dbg !2291
  %102 = load ptr, ptr %5, align 4, !dbg !2291
  %103 = load ptr, ptr %6, align 4, !dbg !2291
  %104 = load ptr, ptr %7, align 4, !dbg !2291
  %105 = load ptr, ptr %8, align 4, !dbg !2291
  call x86_stdcallcc void %100(ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102, ptr noundef %101), !dbg !2291
  call void @llvm.va_end(ptr %9), !dbg !2292
  ret void, !dbg !2293
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc void @"\01_JNI_CallNonvirtualVoidMethodV@20"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2294 {
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
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2295, metadata !DIExpression()), !dbg !2296
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2297, metadata !DIExpression()), !dbg !2296
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2298, metadata !DIExpression()), !dbg !2296
  store ptr %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2299, metadata !DIExpression()), !dbg !2296
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2300, metadata !DIExpression()), !dbg !2296
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2301, metadata !DIExpression()), !dbg !2302
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2303, metadata !DIExpression()), !dbg !2302
  %15 = load ptr, ptr %10, align 4, !dbg !2302
  %16 = load ptr, ptr %15, align 4, !dbg !2302
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2302
  %18 = load ptr, ptr %17, align 4, !dbg !2302
  %19 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 0, !dbg !2302
  %20 = load ptr, ptr %7, align 4, !dbg !2302
  %21 = load ptr, ptr %10, align 4, !dbg !2302
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2302
  store i32 %22, ptr %12, align 4, !dbg !2302
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2304, metadata !DIExpression()), !dbg !2302
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2305, metadata !DIExpression()), !dbg !2307
  store i32 0, ptr %14, align 4, !dbg !2307
  br label %23, !dbg !2307

23:                                               ; preds = %94, %5
  %24 = load i32, ptr %14, align 4, !dbg !2307
  %25 = load i32, ptr %12, align 4, !dbg !2307
  %26 = icmp slt i32 %24, %25, !dbg !2307
  br i1 %26, label %27, label %97, !dbg !2307

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2308
  %29 = getelementptr inbounds [256 x i8], ptr %11, i32 0, i32 %28, !dbg !2308
  %30 = load i8, ptr %29, align 1, !dbg !2308
  %31 = sext i8 %30 to i32, !dbg !2308
  switch i32 %31, label %92 [
    i32 90, label %32
    i32 66, label %39
    i32 67, label %46
    i32 83, label %53
    i32 73, label %60
    i32 74, label %66
    i32 70, label %73
    i32 68, label %80
    i32 76, label %86
  ], !dbg !2308

32:                                               ; preds = %27
  %33 = load ptr, ptr %6, align 4, !dbg !2311
  %34 = getelementptr inbounds i8, ptr %33, i32 4, !dbg !2311
  store ptr %34, ptr %6, align 4, !dbg !2311
  %35 = load i32, ptr %33, align 4, !dbg !2311
  %36 = trunc i32 %35 to i8, !dbg !2311
  %37 = load i32, ptr %14, align 4, !dbg !2311
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %37, !dbg !2311
  store i8 %36, ptr %38, align 8, !dbg !2311
  br label %93, !dbg !2311

39:                                               ; preds = %27
  %40 = load ptr, ptr %6, align 4, !dbg !2311
  %41 = getelementptr inbounds i8, ptr %40, i32 4, !dbg !2311
  store ptr %41, ptr %6, align 4, !dbg !2311
  %42 = load i32, ptr %40, align 4, !dbg !2311
  %43 = trunc i32 %42 to i8, !dbg !2311
  %44 = load i32, ptr %14, align 4, !dbg !2311
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %44, !dbg !2311
  store i8 %43, ptr %45, align 8, !dbg !2311
  br label %93, !dbg !2311

46:                                               ; preds = %27
  %47 = load ptr, ptr %6, align 4, !dbg !2311
  %48 = getelementptr inbounds i8, ptr %47, i32 4, !dbg !2311
  store ptr %48, ptr %6, align 4, !dbg !2311
  %49 = load i32, ptr %47, align 4, !dbg !2311
  %50 = trunc i32 %49 to i16, !dbg !2311
  %51 = load i32, ptr %14, align 4, !dbg !2311
  %52 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %51, !dbg !2311
  store i16 %50, ptr %52, align 8, !dbg !2311
  br label %93, !dbg !2311

53:                                               ; preds = %27
  %54 = load ptr, ptr %6, align 4, !dbg !2311
  %55 = getelementptr inbounds i8, ptr %54, i32 4, !dbg !2311
  store ptr %55, ptr %6, align 4, !dbg !2311
  %56 = load i32, ptr %54, align 4, !dbg !2311
  %57 = trunc i32 %56 to i16, !dbg !2311
  %58 = load i32, ptr %14, align 4, !dbg !2311
  %59 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %58, !dbg !2311
  store i16 %57, ptr %59, align 8, !dbg !2311
  br label %93, !dbg !2311

60:                                               ; preds = %27
  %61 = load ptr, ptr %6, align 4, !dbg !2311
  %62 = getelementptr inbounds i8, ptr %61, i32 4, !dbg !2311
  store ptr %62, ptr %6, align 4, !dbg !2311
  %63 = load i32, ptr %61, align 4, !dbg !2311
  %64 = load i32, ptr %14, align 4, !dbg !2311
  %65 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %64, !dbg !2311
  store i32 %63, ptr %65, align 8, !dbg !2311
  br label %93, !dbg !2311

66:                                               ; preds = %27
  %67 = load ptr, ptr %6, align 4, !dbg !2311
  %68 = getelementptr inbounds i8, ptr %67, i32 4, !dbg !2311
  store ptr %68, ptr %6, align 4, !dbg !2311
  %69 = load i32, ptr %67, align 4, !dbg !2311
  %70 = sext i32 %69 to i64, !dbg !2311
  %71 = load i32, ptr %14, align 4, !dbg !2311
  %72 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %71, !dbg !2311
  store i64 %70, ptr %72, align 8, !dbg !2311
  br label %93, !dbg !2311

73:                                               ; preds = %27
  %74 = load ptr, ptr %6, align 4, !dbg !2311
  %75 = getelementptr inbounds i8, ptr %74, i32 8, !dbg !2311
  store ptr %75, ptr %6, align 4, !dbg !2311
  %76 = load double, ptr %74, align 4, !dbg !2311
  %77 = fptrunc double %76 to float, !dbg !2311
  %78 = load i32, ptr %14, align 4, !dbg !2311
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %78, !dbg !2311
  store float %77, ptr %79, align 8, !dbg !2311
  br label %93, !dbg !2311

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 4, !dbg !2311
  %82 = getelementptr inbounds i8, ptr %81, i32 8, !dbg !2311
  store ptr %82, ptr %6, align 4, !dbg !2311
  %83 = load double, ptr %81, align 4, !dbg !2311
  %84 = load i32, ptr %14, align 4, !dbg !2311
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %84, !dbg !2311
  store double %83, ptr %85, align 8, !dbg !2311
  br label %93, !dbg !2311

86:                                               ; preds = %27
  %87 = load ptr, ptr %6, align 4, !dbg !2311
  %88 = getelementptr inbounds i8, ptr %87, i32 4, !dbg !2311
  store ptr %88, ptr %6, align 4, !dbg !2311
  %89 = load ptr, ptr %87, align 4, !dbg !2311
  %90 = load i32, ptr %14, align 4, !dbg !2311
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 %90, !dbg !2311
  store ptr %89, ptr %91, align 8, !dbg !2311
  br label %93, !dbg !2311

92:                                               ; preds = %27
  br label %93, !dbg !2311

93:                                               ; preds = %92, %86, %80, %73, %66, %60, %53, %46, %39, %32
  br label %94, !dbg !2308

94:                                               ; preds = %93
  %95 = load i32, ptr %14, align 4, !dbg !2313
  %96 = add nsw i32 %95, 1, !dbg !2313
  store i32 %96, ptr %14, align 4, !dbg !2313
  br label %23, !dbg !2313, !llvm.loop !2314

97:                                               ; preds = %23
  %98 = load ptr, ptr %10, align 4, !dbg !2315
  %99 = load ptr, ptr %98, align 4, !dbg !2315
  %100 = getelementptr inbounds %struct.JNINativeInterface_, ptr %99, i32 0, i32 93, !dbg !2315
  %101 = load ptr, ptr %100, align 4, !dbg !2315
  %102 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i32 0, i32 0, !dbg !2315
  %103 = load ptr, ptr %7, align 4, !dbg !2315
  %104 = load ptr, ptr %8, align 4, !dbg !2315
  %105 = load ptr, ptr %9, align 4, !dbg !2315
  %106 = load ptr, ptr %10, align 4, !dbg !2315
  call x86_stdcallcc void %101(ptr noundef %106, ptr noundef %105, ptr noundef %104, ptr noundef %103, ptr noundef %102), !dbg !2315
  ret void, !dbg !2316
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2317 {
  %4 = alloca ptr, align 4
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 4
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2318, metadata !DIExpression()), !dbg !2319
  store ptr %1, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2320, metadata !DIExpression()), !dbg !2319
  store ptr %0, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2321, metadata !DIExpression()), !dbg !2319
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2322, metadata !DIExpression()), !dbg !2323
  call void @llvm.va_start(ptr %7), !dbg !2324
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2325, metadata !DIExpression()), !dbg !2326
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2327, metadata !DIExpression()), !dbg !2326
  %12 = load ptr, ptr %6, align 4, !dbg !2326
  %13 = load ptr, ptr %12, align 4, !dbg !2326
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2326
  %15 = load ptr, ptr %14, align 4, !dbg !2326
  %16 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 0, !dbg !2326
  %17 = load ptr, ptr %4, align 4, !dbg !2326
  %18 = load ptr, ptr %6, align 4, !dbg !2326
  %19 = call i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2326
  store i32 %19, ptr %9, align 4, !dbg !2326
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2328, metadata !DIExpression()), !dbg !2326
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2329, metadata !DIExpression()), !dbg !2331
  store i32 0, ptr %11, align 4, !dbg !2331
  br label %20, !dbg !2331

20:                                               ; preds = %91, %3
  %21 = load i32, ptr %11, align 4, !dbg !2331
  %22 = load i32, ptr %9, align 4, !dbg !2331
  %23 = icmp slt i32 %21, %22, !dbg !2331
  br i1 %23, label %24, label %94, !dbg !2331

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2332
  %26 = getelementptr inbounds [256 x i8], ptr %8, i32 0, i32 %25, !dbg !2332
  %27 = load i8, ptr %26, align 1, !dbg !2332
  %28 = sext i8 %27 to i32, !dbg !2332
  switch i32 %28, label %89 [
    i32 90, label %29
    i32 66, label %36
    i32 67, label %43
    i32 83, label %50
    i32 73, label %57
    i32 74, label %63
    i32 70, label %70
    i32 68, label %77
    i32 76, label %83
  ], !dbg !2332

29:                                               ; preds = %24
  %30 = load ptr, ptr %7, align 4, !dbg !2335
  %31 = getelementptr inbounds i8, ptr %30, i32 4, !dbg !2335
  store ptr %31, ptr %7, align 4, !dbg !2335
  %32 = load i32, ptr %30, align 4, !dbg !2335
  %33 = trunc i32 %32 to i8, !dbg !2335
  %34 = load i32, ptr %11, align 4, !dbg !2335
  %35 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %34, !dbg !2335
  store i8 %33, ptr %35, align 8, !dbg !2335
  br label %90, !dbg !2335

36:                                               ; preds = %24
  %37 = load ptr, ptr %7, align 4, !dbg !2335
  %38 = getelementptr inbounds i8, ptr %37, i32 4, !dbg !2335
  store ptr %38, ptr %7, align 4, !dbg !2335
  %39 = load i32, ptr %37, align 4, !dbg !2335
  %40 = trunc i32 %39 to i8, !dbg !2335
  %41 = load i32, ptr %11, align 4, !dbg !2335
  %42 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %41, !dbg !2335
  store i8 %40, ptr %42, align 8, !dbg !2335
  br label %90, !dbg !2335

43:                                               ; preds = %24
  %44 = load ptr, ptr %7, align 4, !dbg !2335
  %45 = getelementptr inbounds i8, ptr %44, i32 4, !dbg !2335
  store ptr %45, ptr %7, align 4, !dbg !2335
  %46 = load i32, ptr %44, align 4, !dbg !2335
  %47 = trunc i32 %46 to i16, !dbg !2335
  %48 = load i32, ptr %11, align 4, !dbg !2335
  %49 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %48, !dbg !2335
  store i16 %47, ptr %49, align 8, !dbg !2335
  br label %90, !dbg !2335

50:                                               ; preds = %24
  %51 = load ptr, ptr %7, align 4, !dbg !2335
  %52 = getelementptr inbounds i8, ptr %51, i32 4, !dbg !2335
  store ptr %52, ptr %7, align 4, !dbg !2335
  %53 = load i32, ptr %51, align 4, !dbg !2335
  %54 = trunc i32 %53 to i16, !dbg !2335
  %55 = load i32, ptr %11, align 4, !dbg !2335
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %55, !dbg !2335
  store i16 %54, ptr %56, align 8, !dbg !2335
  br label %90, !dbg !2335

57:                                               ; preds = %24
  %58 = load ptr, ptr %7, align 4, !dbg !2335
  %59 = getelementptr inbounds i8, ptr %58, i32 4, !dbg !2335
  store ptr %59, ptr %7, align 4, !dbg !2335
  %60 = load i32, ptr %58, align 4, !dbg !2335
  %61 = load i32, ptr %11, align 4, !dbg !2335
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %61, !dbg !2335
  store i32 %60, ptr %62, align 8, !dbg !2335
  br label %90, !dbg !2335

63:                                               ; preds = %24
  %64 = load ptr, ptr %7, align 4, !dbg !2335
  %65 = getelementptr inbounds i8, ptr %64, i32 4, !dbg !2335
  store ptr %65, ptr %7, align 4, !dbg !2335
  %66 = load i32, ptr %64, align 4, !dbg !2335
  %67 = sext i32 %66 to i64, !dbg !2335
  %68 = load i32, ptr %11, align 4, !dbg !2335
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %68, !dbg !2335
  store i64 %67, ptr %69, align 8, !dbg !2335
  br label %90, !dbg !2335

70:                                               ; preds = %24
  %71 = load ptr, ptr %7, align 4, !dbg !2335
  %72 = getelementptr inbounds i8, ptr %71, i32 8, !dbg !2335
  store ptr %72, ptr %7, align 4, !dbg !2335
  %73 = load double, ptr %71, align 4, !dbg !2335
  %74 = fptrunc double %73 to float, !dbg !2335
  %75 = load i32, ptr %11, align 4, !dbg !2335
  %76 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %75, !dbg !2335
  store float %74, ptr %76, align 8, !dbg !2335
  br label %90, !dbg !2335

77:                                               ; preds = %24
  %78 = load ptr, ptr %7, align 4, !dbg !2335
  %79 = getelementptr inbounds i8, ptr %78, i32 8, !dbg !2335
  store ptr %79, ptr %7, align 4, !dbg !2335
  %80 = load double, ptr %78, align 4, !dbg !2335
  %81 = load i32, ptr %11, align 4, !dbg !2335
  %82 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %81, !dbg !2335
  store double %80, ptr %82, align 8, !dbg !2335
  br label %90, !dbg !2335

83:                                               ; preds = %24
  %84 = load ptr, ptr %7, align 4, !dbg !2335
  %85 = getelementptr inbounds i8, ptr %84, i32 4, !dbg !2335
  store ptr %85, ptr %7, align 4, !dbg !2335
  %86 = load ptr, ptr %84, align 4, !dbg !2335
  %87 = load i32, ptr %11, align 4, !dbg !2335
  %88 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 %87, !dbg !2335
  store ptr %86, ptr %88, align 8, !dbg !2335
  br label %90, !dbg !2335

89:                                               ; preds = %24
  br label %90, !dbg !2335

90:                                               ; preds = %89, %83, %77, %70, %63, %57, %50, %43, %36, %29
  br label %91, !dbg !2332

91:                                               ; preds = %90
  %92 = load i32, ptr %11, align 4, !dbg !2337
  %93 = add nsw i32 %92, 1, !dbg !2337
  store i32 %93, ptr %11, align 4, !dbg !2337
  br label %20, !dbg !2337, !llvm.loop !2338

94:                                               ; preds = %20
  %95 = load ptr, ptr %6, align 4, !dbg !2339
  %96 = load ptr, ptr %95, align 4, !dbg !2339
  %97 = getelementptr inbounds %struct.JNINativeInterface_, ptr %96, i32 0, i32 143, !dbg !2339
  %98 = load ptr, ptr %97, align 4, !dbg !2339
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i32 0, i32 0, !dbg !2339
  %100 = load ptr, ptr %4, align 4, !dbg !2339
  %101 = load ptr, ptr %5, align 4, !dbg !2339
  %102 = load ptr, ptr %6, align 4, !dbg !2339
  call x86_stdcallcc void %98(ptr noundef %102, ptr noundef %101, ptr noundef %100, ptr noundef %99), !dbg !2339
  call void @llvm.va_end(ptr %7), !dbg !2340
  ret void, !dbg !2341
}

; Function Attrs: noinline nounwind optnone
define dso_local dllexport x86_stdcallcc void @"\01_JNI_CallStaticVoidMethodV@16"(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2342 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2343, metadata !DIExpression()), !dbg !2344
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2345, metadata !DIExpression()), !dbg !2344
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2346, metadata !DIExpression()), !dbg !2344
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2347, metadata !DIExpression()), !dbg !2344
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2348, metadata !DIExpression()), !dbg !2349
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2350, metadata !DIExpression()), !dbg !2349
  %13 = load ptr, ptr %8, align 4, !dbg !2349
  %14 = load ptr, ptr %13, align 4, !dbg !2349
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2349
  %16 = load ptr, ptr %15, align 4, !dbg !2349
  %17 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 0, !dbg !2349
  %18 = load ptr, ptr %6, align 4, !dbg !2349
  %19 = load ptr, ptr %8, align 4, !dbg !2349
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2349
  store i32 %20, ptr %10, align 4, !dbg !2349
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2351, metadata !DIExpression()), !dbg !2349
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2352, metadata !DIExpression()), !dbg !2354
  store i32 0, ptr %12, align 4, !dbg !2354
  br label %21, !dbg !2354

21:                                               ; preds = %92, %4
  %22 = load i32, ptr %12, align 4, !dbg !2354
  %23 = load i32, ptr %10, align 4, !dbg !2354
  %24 = icmp slt i32 %22, %23, !dbg !2354
  br i1 %24, label %25, label %95, !dbg !2354

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2355
  %27 = getelementptr inbounds [256 x i8], ptr %9, i32 0, i32 %26, !dbg !2355
  %28 = load i8, ptr %27, align 1, !dbg !2355
  %29 = sext i8 %28 to i32, !dbg !2355
  switch i32 %29, label %90 [
    i32 90, label %30
    i32 66, label %37
    i32 67, label %44
    i32 83, label %51
    i32 73, label %58
    i32 74, label %64
    i32 70, label %71
    i32 68, label %78
    i32 76, label %84
  ], !dbg !2355

30:                                               ; preds = %25
  %31 = load ptr, ptr %5, align 4, !dbg !2358
  %32 = getelementptr inbounds i8, ptr %31, i32 4, !dbg !2358
  store ptr %32, ptr %5, align 4, !dbg !2358
  %33 = load i32, ptr %31, align 4, !dbg !2358
  %34 = trunc i32 %33 to i8, !dbg !2358
  %35 = load i32, ptr %12, align 4, !dbg !2358
  %36 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %35, !dbg !2358
  store i8 %34, ptr %36, align 8, !dbg !2358
  br label %91, !dbg !2358

37:                                               ; preds = %25
  %38 = load ptr, ptr %5, align 4, !dbg !2358
  %39 = getelementptr inbounds i8, ptr %38, i32 4, !dbg !2358
  store ptr %39, ptr %5, align 4, !dbg !2358
  %40 = load i32, ptr %38, align 4, !dbg !2358
  %41 = trunc i32 %40 to i8, !dbg !2358
  %42 = load i32, ptr %12, align 4, !dbg !2358
  %43 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %42, !dbg !2358
  store i8 %41, ptr %43, align 8, !dbg !2358
  br label %91, !dbg !2358

44:                                               ; preds = %25
  %45 = load ptr, ptr %5, align 4, !dbg !2358
  %46 = getelementptr inbounds i8, ptr %45, i32 4, !dbg !2358
  store ptr %46, ptr %5, align 4, !dbg !2358
  %47 = load i32, ptr %45, align 4, !dbg !2358
  %48 = trunc i32 %47 to i16, !dbg !2358
  %49 = load i32, ptr %12, align 4, !dbg !2358
  %50 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %49, !dbg !2358
  store i16 %48, ptr %50, align 8, !dbg !2358
  br label %91, !dbg !2358

51:                                               ; preds = %25
  %52 = load ptr, ptr %5, align 4, !dbg !2358
  %53 = getelementptr inbounds i8, ptr %52, i32 4, !dbg !2358
  store ptr %53, ptr %5, align 4, !dbg !2358
  %54 = load i32, ptr %52, align 4, !dbg !2358
  %55 = trunc i32 %54 to i16, !dbg !2358
  %56 = load i32, ptr %12, align 4, !dbg !2358
  %57 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %56, !dbg !2358
  store i16 %55, ptr %57, align 8, !dbg !2358
  br label %91, !dbg !2358

58:                                               ; preds = %25
  %59 = load ptr, ptr %5, align 4, !dbg !2358
  %60 = getelementptr inbounds i8, ptr %59, i32 4, !dbg !2358
  store ptr %60, ptr %5, align 4, !dbg !2358
  %61 = load i32, ptr %59, align 4, !dbg !2358
  %62 = load i32, ptr %12, align 4, !dbg !2358
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %62, !dbg !2358
  store i32 %61, ptr %63, align 8, !dbg !2358
  br label %91, !dbg !2358

64:                                               ; preds = %25
  %65 = load ptr, ptr %5, align 4, !dbg !2358
  %66 = getelementptr inbounds i8, ptr %65, i32 4, !dbg !2358
  store ptr %66, ptr %5, align 4, !dbg !2358
  %67 = load i32, ptr %65, align 4, !dbg !2358
  %68 = sext i32 %67 to i64, !dbg !2358
  %69 = load i32, ptr %12, align 4, !dbg !2358
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %69, !dbg !2358
  store i64 %68, ptr %70, align 8, !dbg !2358
  br label %91, !dbg !2358

71:                                               ; preds = %25
  %72 = load ptr, ptr %5, align 4, !dbg !2358
  %73 = getelementptr inbounds i8, ptr %72, i32 8, !dbg !2358
  store ptr %73, ptr %5, align 4, !dbg !2358
  %74 = load double, ptr %72, align 4, !dbg !2358
  %75 = fptrunc double %74 to float, !dbg !2358
  %76 = load i32, ptr %12, align 4, !dbg !2358
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %76, !dbg !2358
  store float %75, ptr %77, align 8, !dbg !2358
  br label %91, !dbg !2358

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 4, !dbg !2358
  %80 = getelementptr inbounds i8, ptr %79, i32 8, !dbg !2358
  store ptr %80, ptr %5, align 4, !dbg !2358
  %81 = load double, ptr %79, align 4, !dbg !2358
  %82 = load i32, ptr %12, align 4, !dbg !2358
  %83 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %82, !dbg !2358
  store double %81, ptr %83, align 8, !dbg !2358
  br label %91, !dbg !2358

84:                                               ; preds = %25
  %85 = load ptr, ptr %5, align 4, !dbg !2358
  %86 = getelementptr inbounds i8, ptr %85, i32 4, !dbg !2358
  store ptr %86, ptr %5, align 4, !dbg !2358
  %87 = load ptr, ptr %85, align 4, !dbg !2358
  %88 = load i32, ptr %12, align 4, !dbg !2358
  %89 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 %88, !dbg !2358
  store ptr %87, ptr %89, align 8, !dbg !2358
  br label %91, !dbg !2358

90:                                               ; preds = %25
  br label %91, !dbg !2358

91:                                               ; preds = %90, %84, %78, %71, %64, %58, %51, %44, %37, %30
  br label %92, !dbg !2355

92:                                               ; preds = %91
  %93 = load i32, ptr %12, align 4, !dbg !2360
  %94 = add nsw i32 %93, 1, !dbg !2360
  store i32 %94, ptr %12, align 4, !dbg !2360
  br label %21, !dbg !2360, !llvm.loop !2361

95:                                               ; preds = %21
  %96 = load ptr, ptr %8, align 4, !dbg !2362
  %97 = load ptr, ptr %96, align 4, !dbg !2362
  %98 = getelementptr inbounds %struct.JNINativeInterface_, ptr %97, i32 0, i32 143, !dbg !2362
  %99 = load ptr, ptr %98, align 4, !dbg !2362
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i32 0, i32 0, !dbg !2362
  %101 = load ptr, ptr %6, align 4, !dbg !2362
  %102 = load ptr, ptr %7, align 4, !dbg !2362
  %103 = load ptr, ptr %8, align 4, !dbg !2362
  call x86_stdcallcc void %99(ptr noundef %103, ptr noundef %102, ptr noundef %101, ptr noundef %100), !dbg !2362
  ret void, !dbg !2363
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !2364 {
  %5 = alloca ptr, align 4
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  store ptr %3, ptr %5, align 4
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2380, metadata !DIExpression()), !dbg !2381
  store ptr %2, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2382, metadata !DIExpression()), !dbg !2383
  store ptr %1, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2384, metadata !DIExpression()), !dbg !2385
  store ptr %0, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2386, metadata !DIExpression()), !dbg !2387
  %9 = load ptr, ptr %5, align 4, !dbg !2388
  %10 = load ptr, ptr %6, align 4, !dbg !2388
  %11 = load ptr, ptr %7, align 4, !dbg !2388
  %12 = load ptr, ptr %8, align 4, !dbg !2388
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i32 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !2388
  ret i32 %13, !dbg !2388
}

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local i32 @_vsnprintf_l(ptr noundef %0, i32 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat !dbg !2389 {
  %6 = alloca ptr, align 4
  %7 = alloca ptr, align 4
  %8 = alloca ptr, align 4
  %9 = alloca i32, align 4
  %10 = alloca ptr, align 4
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 4
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2392, metadata !DIExpression()), !dbg !2393
  store ptr %3, ptr %7, align 4
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2394, metadata !DIExpression()), !dbg !2395
  store ptr %2, ptr %8, align 4
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2396, metadata !DIExpression()), !dbg !2397
  store i32 %1, ptr %9, align 4
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2398, metadata !DIExpression()), !dbg !2399
  store ptr %0, ptr %10, align 4
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2400, metadata !DIExpression()), !dbg !2401
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2402, metadata !DIExpression()), !dbg !2404
  %12 = load ptr, ptr %6, align 4, !dbg !2404
  %13 = load ptr, ptr %7, align 4, !dbg !2404
  %14 = load ptr, ptr %8, align 4, !dbg !2404
  %15 = load i32, ptr %9, align 4, !dbg !2404
  %16 = load ptr, ptr %10, align 4, !dbg !2404
  %17 = call ptr @__local_stdio_printf_options(), !dbg !2404
  %18 = load i64, ptr %17, align 8, !dbg !2404
  %19 = or i64 %18, 1, !dbg !2404
  %20 = call i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i32 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12), !dbg !2404
  store i32 %20, ptr %11, align 4, !dbg !2404
  %21 = load i32, ptr %11, align 4, !dbg !2405
  %22 = icmp slt i32 %21, 0, !dbg !2405
  br i1 %22, label %23, label %24, !dbg !2405

23:                                               ; preds = %5
  br label %26, !dbg !2405

24:                                               ; preds = %5
  %25 = load i32, ptr %11, align 4, !dbg !2405
  br label %26, !dbg !2405

26:                                               ; preds = %24, %23
  %27 = phi i32 [ -1, %23 ], [ %25, %24 ], !dbg !2405
  ret i32 %27, !dbg !2405
}

declare dso_local i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i32 noundef, ptr noundef, ptr noundef, ptr noundef) #3

; Function Attrs: noinline nounwind optnone
define linkonce_odr dso_local ptr @__local_stdio_printf_options() #0 comdat !dbg !2 {
  ret ptr @__local_stdio_printf_options._OptionsStorage, !dbg !2406
}

attributes #0 = { noinline nounwind optnone "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nocallback nofree nosync nounwind readnone speculatable willreturn }
attributes #2 = { nocallback nofree nosync nounwind willreturn }
attributes #3 = { "frame-pointer"="all" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.dbg.cu = !{!8}
!llvm.module.flags = !{!1034, !1035, !1036, !1037, !1038}
!llvm.ident = !{!1039}

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
!38 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !39)
!39 = !{!40, !25}
!40 = !DIDerivedType(tag: DW_TAG_typedef, name: "jint", file: !41, line: 33, baseType: !42)
!41 = !DIFile(filename: "..\\..\\..\\openjdk\\jdk\\src\\windows\\javavm\\export\\jni_md.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "1ea1808175ba5b9740cb94cde3a9f925")
!42 = !DIBasicType(name: "long", size: 32, encoding: DW_ATE_signed)
!43 = !DIDerivedType(tag: DW_TAG_member, name: "DefineClass", scope: !29, file: !12, line: 222, baseType: !44, size: 32, offset: 160)
!44 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !45, size: 32)
!45 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !46)
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
!61 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !62)
!62 = !{!47, !25, !51}
!63 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedMethod", scope: !29, file: !12, line: 228, baseType: !64, size: 32, offset: 224)
!64 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !65, size: 32)
!65 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !66)
!66 = !{!67, !25, !48}
!67 = !DIDerivedType(tag: DW_TAG_typedef, name: "jmethodID", file: !12, line: 136, baseType: !68)
!68 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !69, size: 32)
!69 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jmethodID", file: !12, line: 135, flags: DIFlagFwdDecl)
!70 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedField", scope: !29, file: !12, line: 230, baseType: !71, size: 32, offset: 256)
!71 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !72, size: 32)
!72 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !73)
!73 = !{!74, !25, !48}
!74 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfieldID", file: !12, line: 133, baseType: !75)
!75 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !76, size: 32)
!76 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jfieldID", file: !12, line: 132, flags: DIFlagFwdDecl)
!77 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedMethod", scope: !29, file: !12, line: 233, baseType: !78, size: 32, offset: 288)
!78 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !79, size: 32)
!79 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !80)
!80 = !{!48, !25, !47, !67, !81}
!81 = !DIDerivedType(tag: DW_TAG_typedef, name: "jboolean", file: !12, line: 57, baseType: !82)
!82 = !DIBasicType(name: "unsigned char", size: 8, encoding: DW_ATE_unsigned_char)
!83 = !DIDerivedType(tag: DW_TAG_member, name: "GetSuperclass", scope: !29, file: !12, line: 236, baseType: !84, size: 32, offset: 320)
!84 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !85, size: 32)
!85 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !86)
!86 = !{!47, !25, !47}
!87 = !DIDerivedType(tag: DW_TAG_member, name: "IsAssignableFrom", scope: !29, file: !12, line: 238, baseType: !88, size: 32, offset: 352)
!88 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !89, size: 32)
!89 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !90)
!90 = !{!81, !25, !47, !47}
!91 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedField", scope: !29, file: !12, line: 241, baseType: !92, size: 32, offset: 384)
!92 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !93, size: 32)
!93 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !94)
!94 = !{!48, !25, !47, !74, !81}
!95 = !DIDerivedType(tag: DW_TAG_member, name: "Throw", scope: !29, file: !12, line: 244, baseType: !96, size: 32, offset: 416)
!96 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !97, size: 32)
!97 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !98)
!98 = !{!40, !25, !99}
!99 = !DIDerivedType(tag: DW_TAG_typedef, name: "jthrowable", file: !12, line: 103, baseType: !48)
!100 = !DIDerivedType(tag: DW_TAG_member, name: "ThrowNew", scope: !29, file: !12, line: 246, baseType: !101, size: 32, offset: 448)
!101 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !102, size: 32)
!102 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !103)
!103 = !{!40, !25, !47, !51}
!104 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionOccurred", scope: !29, file: !12, line: 248, baseType: !105, size: 32, offset: 480)
!105 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !106, size: 32)
!106 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !107)
!107 = !{!99, !25}
!108 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionDescribe", scope: !29, file: !12, line: 250, baseType: !109, size: 32, offset: 512)
!109 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !110, size: 32)
!110 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !111)
!111 = !{null, !25}
!112 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionClear", scope: !29, file: !12, line: 252, baseType: !109, size: 32, offset: 544)
!113 = !DIDerivedType(tag: DW_TAG_member, name: "FatalError", scope: !29, file: !12, line: 254, baseType: !114, size: 32, offset: 576)
!114 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !115, size: 32)
!115 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !116)
!116 = !{null, !25, !51}
!117 = !DIDerivedType(tag: DW_TAG_member, name: "PushLocalFrame", scope: !29, file: !12, line: 257, baseType: !118, size: 32, offset: 608)
!118 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !119, size: 32)
!119 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !120)
!120 = !{!40, !25, !40}
!121 = !DIDerivedType(tag: DW_TAG_member, name: "PopLocalFrame", scope: !29, file: !12, line: 259, baseType: !122, size: 32, offset: 640)
!122 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !123, size: 32)
!123 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !124)
!124 = !{!48, !25, !48}
!125 = !DIDerivedType(tag: DW_TAG_member, name: "NewGlobalRef", scope: !29, file: !12, line: 262, baseType: !122, size: 32, offset: 672)
!126 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteGlobalRef", scope: !29, file: !12, line: 264, baseType: !127, size: 32, offset: 704)
!127 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !128, size: 32)
!128 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !129)
!129 = !{null, !25, !48}
!130 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteLocalRef", scope: !29, file: !12, line: 266, baseType: !127, size: 32, offset: 736)
!131 = !DIDerivedType(tag: DW_TAG_member, name: "IsSameObject", scope: !29, file: !12, line: 268, baseType: !132, size: 32, offset: 768)
!132 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !133, size: 32)
!133 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !134)
!134 = !{!81, !25, !48, !48}
!135 = !DIDerivedType(tag: DW_TAG_member, name: "NewLocalRef", scope: !29, file: !12, line: 270, baseType: !122, size: 32, offset: 800)
!136 = !DIDerivedType(tag: DW_TAG_member, name: "EnsureLocalCapacity", scope: !29, file: !12, line: 272, baseType: !118, size: 32, offset: 832)
!137 = !DIDerivedType(tag: DW_TAG_member, name: "AllocObject", scope: !29, file: !12, line: 275, baseType: !138, size: 32, offset: 864)
!138 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !139, size: 32)
!139 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !140)
!140 = !{!48, !25, !47}
!141 = !DIDerivedType(tag: DW_TAG_member, name: "NewObject", scope: !29, file: !12, line: 277, baseType: !142, size: 32, offset: 896)
!142 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !143, size: 32)
!143 = !DISubroutineType(types: !144)
!144 = !{!48, !25, !47, !67, null}
!145 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectV", scope: !29, file: !12, line: 279, baseType: !146, size: 32, offset: 928)
!146 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !147, size: 32)
!147 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !148)
!148 = !{!48, !25, !47, !67, !149}
!149 = !DIDerivedType(tag: DW_TAG_typedef, name: "va_list", file: !150, line: 72, baseType: !151)
!150 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vadefs.h", directory: "", checksumkind: CSK_MD5, checksum: "a4b8f96637d0704c82f39ecb6bde2ab4")
!151 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !53, size: 32)
!152 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectA", scope: !29, file: !12, line: 281, baseType: !153, size: 32, offset: 960)
!153 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !154, size: 32)
!154 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !155)
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
!182 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !183)
!183 = !{!47, !25, !48}
!184 = !DIDerivedType(tag: DW_TAG_member, name: "IsInstanceOf", scope: !29, file: !12, line: 286, baseType: !185, size: 32, offset: 1024)
!185 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !186, size: 32)
!186 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !187)
!187 = !{!81, !25, !48, !47}
!188 = !DIDerivedType(tag: DW_TAG_member, name: "GetMethodID", scope: !29, file: !12, line: 289, baseType: !189, size: 32, offset: 1056)
!189 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !190, size: 32)
!190 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !191)
!191 = !{!67, !25, !47, !51, !51}
!192 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethod", scope: !29, file: !12, line: 292, baseType: !193, size: 32, offset: 1088)
!193 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !194, size: 32)
!194 = !DISubroutineType(types: !195)
!195 = !{!48, !25, !48, !67, null}
!196 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodV", scope: !29, file: !12, line: 294, baseType: !197, size: 32, offset: 1120)
!197 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !198, size: 32)
!198 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !199)
!199 = !{!48, !25, !48, !67, !149}
!200 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodA", scope: !29, file: !12, line: 296, baseType: !201, size: 32, offset: 1152)
!201 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !202, size: 32)
!202 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !203)
!203 = !{!48, !25, !48, !67, !156}
!204 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethod", scope: !29, file: !12, line: 299, baseType: !205, size: 32, offset: 1184)
!205 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !206, size: 32)
!206 = !DISubroutineType(types: !207)
!207 = !{!81, !25, !48, !67, null}
!208 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodV", scope: !29, file: !12, line: 301, baseType: !209, size: 32, offset: 1216)
!209 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !210, size: 32)
!210 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !211)
!211 = !{!81, !25, !48, !67, !149}
!212 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodA", scope: !29, file: !12, line: 303, baseType: !213, size: 32, offset: 1248)
!213 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !214, size: 32)
!214 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !215)
!215 = !{!81, !25, !48, !67, !156}
!216 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethod", scope: !29, file: !12, line: 306, baseType: !217, size: 32, offset: 1280)
!217 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !218, size: 32)
!218 = !DISubroutineType(types: !219)
!219 = !{!56, !25, !48, !67, null}
!220 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodV", scope: !29, file: !12, line: 308, baseType: !221, size: 32, offset: 1312)
!221 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !222, size: 32)
!222 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !223)
!223 = !{!56, !25, !48, !67, !149}
!224 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodA", scope: !29, file: !12, line: 310, baseType: !225, size: 32, offset: 1344)
!225 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !226, size: 32)
!226 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !227)
!227 = !{!56, !25, !48, !67, !156}
!228 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethod", scope: !29, file: !12, line: 313, baseType: !229, size: 32, offset: 1376)
!229 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !230, size: 32)
!230 = !DISubroutineType(types: !231)
!231 = !{!164, !25, !48, !67, null}
!232 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodV", scope: !29, file: !12, line: 315, baseType: !233, size: 32, offset: 1408)
!233 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !234, size: 32)
!234 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !235)
!235 = !{!164, !25, !48, !67, !149}
!236 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodA", scope: !29, file: !12, line: 317, baseType: !237, size: 32, offset: 1440)
!237 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !238, size: 32)
!238 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !239)
!239 = !{!164, !25, !48, !67, !156}
!240 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethod", scope: !29, file: !12, line: 320, baseType: !241, size: 32, offset: 1472)
!241 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !242, size: 32)
!242 = !DISubroutineType(types: !243)
!243 = !{!167, !25, !48, !67, null}
!244 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodV", scope: !29, file: !12, line: 322, baseType: !245, size: 32, offset: 1504)
!245 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !246, size: 32)
!246 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !247)
!247 = !{!167, !25, !48, !67, !149}
!248 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodA", scope: !29, file: !12, line: 324, baseType: !249, size: 32, offset: 1536)
!249 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !250, size: 32)
!250 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !251)
!251 = !{!167, !25, !48, !67, !156}
!252 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethod", scope: !29, file: !12, line: 327, baseType: !253, size: 32, offset: 1568)
!253 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !254, size: 32)
!254 = !DISubroutineType(types: !255)
!255 = !{!40, !25, !48, !67, null}
!256 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodV", scope: !29, file: !12, line: 329, baseType: !257, size: 32, offset: 1600)
!257 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !258, size: 32)
!258 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !259)
!259 = !{!40, !25, !48, !67, !149}
!260 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodA", scope: !29, file: !12, line: 331, baseType: !261, size: 32, offset: 1632)
!261 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !262, size: 32)
!262 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !263)
!263 = !{!40, !25, !48, !67, !156}
!264 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethod", scope: !29, file: !12, line: 334, baseType: !265, size: 32, offset: 1664)
!265 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !266, size: 32)
!266 = !DISubroutineType(types: !267)
!267 = !{!171, !25, !48, !67, null}
!268 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodV", scope: !29, file: !12, line: 336, baseType: !269, size: 32, offset: 1696)
!269 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !270, size: 32)
!270 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !271)
!271 = !{!171, !25, !48, !67, !149}
!272 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodA", scope: !29, file: !12, line: 338, baseType: !273, size: 32, offset: 1728)
!273 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !274, size: 32)
!274 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !275)
!275 = !{!171, !25, !48, !67, !156}
!276 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethod", scope: !29, file: !12, line: 341, baseType: !277, size: 32, offset: 1760)
!277 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !278, size: 32)
!278 = !DISubroutineType(types: !279)
!279 = !{!174, !25, !48, !67, null}
!280 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodV", scope: !29, file: !12, line: 343, baseType: !281, size: 32, offset: 1792)
!281 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !282, size: 32)
!282 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !283)
!283 = !{!174, !25, !48, !67, !149}
!284 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodA", scope: !29, file: !12, line: 345, baseType: !285, size: 32, offset: 1824)
!285 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !286, size: 32)
!286 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !287)
!287 = !{!174, !25, !48, !67, !156}
!288 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethod", scope: !29, file: !12, line: 348, baseType: !289, size: 32, offset: 1856)
!289 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !290, size: 32)
!290 = !DISubroutineType(types: !291)
!291 = !{!177, !25, !48, !67, null}
!292 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodV", scope: !29, file: !12, line: 350, baseType: !293, size: 32, offset: 1888)
!293 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !294, size: 32)
!294 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !295)
!295 = !{!177, !25, !48, !67, !149}
!296 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodA", scope: !29, file: !12, line: 352, baseType: !297, size: 32, offset: 1920)
!297 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !298, size: 32)
!298 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !299)
!299 = !{!177, !25, !48, !67, !156}
!300 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethod", scope: !29, file: !12, line: 355, baseType: !301, size: 32, offset: 1952)
!301 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !302, size: 32)
!302 = !DISubroutineType(types: !303)
!303 = !{null, !25, !48, !67, null}
!304 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodV", scope: !29, file: !12, line: 357, baseType: !305, size: 32, offset: 1984)
!305 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !306, size: 32)
!306 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !307)
!307 = !{null, !25, !48, !67, !149}
!308 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodA", scope: !29, file: !12, line: 359, baseType: !309, size: 32, offset: 2016)
!309 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !310, size: 32)
!310 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !311)
!311 = !{null, !25, !48, !67, !156}
!312 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethod", scope: !29, file: !12, line: 362, baseType: !313, size: 32, offset: 2048)
!313 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !314, size: 32)
!314 = !DISubroutineType(types: !315)
!315 = !{!48, !25, !48, !47, !67, null}
!316 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodV", scope: !29, file: !12, line: 364, baseType: !317, size: 32, offset: 2080)
!317 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !318, size: 32)
!318 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !319)
!319 = !{!48, !25, !48, !47, !67, !149}
!320 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodA", scope: !29, file: !12, line: 367, baseType: !321, size: 32, offset: 2112)
!321 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !322, size: 32)
!322 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !323)
!323 = !{!48, !25, !48, !47, !67, !156}
!324 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethod", scope: !29, file: !12, line: 371, baseType: !325, size: 32, offset: 2144)
!325 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !326, size: 32)
!326 = !DISubroutineType(types: !327)
!327 = !{!81, !25, !48, !47, !67, null}
!328 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodV", scope: !29, file: !12, line: 373, baseType: !329, size: 32, offset: 2176)
!329 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !330, size: 32)
!330 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !331)
!331 = !{!81, !25, !48, !47, !67, !149}
!332 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodA", scope: !29, file: !12, line: 376, baseType: !333, size: 32, offset: 2208)
!333 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !334, size: 32)
!334 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !335)
!335 = !{!81, !25, !48, !47, !67, !156}
!336 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethod", scope: !29, file: !12, line: 380, baseType: !337, size: 32, offset: 2240)
!337 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !338, size: 32)
!338 = !DISubroutineType(types: !339)
!339 = !{!56, !25, !48, !47, !67, null}
!340 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodV", scope: !29, file: !12, line: 382, baseType: !341, size: 32, offset: 2272)
!341 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !342, size: 32)
!342 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !343)
!343 = !{!56, !25, !48, !47, !67, !149}
!344 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodA", scope: !29, file: !12, line: 385, baseType: !345, size: 32, offset: 2304)
!345 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !346, size: 32)
!346 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !347)
!347 = !{!56, !25, !48, !47, !67, !156}
!348 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethod", scope: !29, file: !12, line: 389, baseType: !349, size: 32, offset: 2336)
!349 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !350, size: 32)
!350 = !DISubroutineType(types: !351)
!351 = !{!164, !25, !48, !47, !67, null}
!352 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodV", scope: !29, file: !12, line: 391, baseType: !353, size: 32, offset: 2368)
!353 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !354, size: 32)
!354 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !355)
!355 = !{!164, !25, !48, !47, !67, !149}
!356 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodA", scope: !29, file: !12, line: 394, baseType: !357, size: 32, offset: 2400)
!357 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !358, size: 32)
!358 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !359)
!359 = !{!164, !25, !48, !47, !67, !156}
!360 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethod", scope: !29, file: !12, line: 398, baseType: !361, size: 32, offset: 2432)
!361 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !362, size: 32)
!362 = !DISubroutineType(types: !363)
!363 = !{!167, !25, !48, !47, !67, null}
!364 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodV", scope: !29, file: !12, line: 400, baseType: !365, size: 32, offset: 2464)
!365 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !366, size: 32)
!366 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !367)
!367 = !{!167, !25, !48, !47, !67, !149}
!368 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodA", scope: !29, file: !12, line: 403, baseType: !369, size: 32, offset: 2496)
!369 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !370, size: 32)
!370 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !371)
!371 = !{!167, !25, !48, !47, !67, !156}
!372 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethod", scope: !29, file: !12, line: 407, baseType: !373, size: 32, offset: 2528)
!373 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !374, size: 32)
!374 = !DISubroutineType(types: !375)
!375 = !{!40, !25, !48, !47, !67, null}
!376 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodV", scope: !29, file: !12, line: 409, baseType: !377, size: 32, offset: 2560)
!377 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !378, size: 32)
!378 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !379)
!379 = !{!40, !25, !48, !47, !67, !149}
!380 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodA", scope: !29, file: !12, line: 412, baseType: !381, size: 32, offset: 2592)
!381 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !382, size: 32)
!382 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !383)
!383 = !{!40, !25, !48, !47, !67, !156}
!384 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethod", scope: !29, file: !12, line: 416, baseType: !385, size: 32, offset: 2624)
!385 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !386, size: 32)
!386 = !DISubroutineType(types: !387)
!387 = !{!171, !25, !48, !47, !67, null}
!388 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodV", scope: !29, file: !12, line: 418, baseType: !389, size: 32, offset: 2656)
!389 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !390, size: 32)
!390 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !391)
!391 = !{!171, !25, !48, !47, !67, !149}
!392 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodA", scope: !29, file: !12, line: 421, baseType: !393, size: 32, offset: 2688)
!393 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !394, size: 32)
!394 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !395)
!395 = !{!171, !25, !48, !47, !67, !156}
!396 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethod", scope: !29, file: !12, line: 425, baseType: !397, size: 32, offset: 2720)
!397 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !398, size: 32)
!398 = !DISubroutineType(types: !399)
!399 = !{!174, !25, !48, !47, !67, null}
!400 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodV", scope: !29, file: !12, line: 427, baseType: !401, size: 32, offset: 2752)
!401 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !402, size: 32)
!402 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !403)
!403 = !{!174, !25, !48, !47, !67, !149}
!404 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodA", scope: !29, file: !12, line: 430, baseType: !405, size: 32, offset: 2784)
!405 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !406, size: 32)
!406 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !407)
!407 = !{!174, !25, !48, !47, !67, !156}
!408 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethod", scope: !29, file: !12, line: 434, baseType: !409, size: 32, offset: 2816)
!409 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !410, size: 32)
!410 = !DISubroutineType(types: !411)
!411 = !{!177, !25, !48, !47, !67, null}
!412 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodV", scope: !29, file: !12, line: 436, baseType: !413, size: 32, offset: 2848)
!413 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !414, size: 32)
!414 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !415)
!415 = !{!177, !25, !48, !47, !67, !149}
!416 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodA", scope: !29, file: !12, line: 439, baseType: !417, size: 32, offset: 2880)
!417 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !418, size: 32)
!418 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !419)
!419 = !{!177, !25, !48, !47, !67, !156}
!420 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethod", scope: !29, file: !12, line: 443, baseType: !421, size: 32, offset: 2912)
!421 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !422, size: 32)
!422 = !DISubroutineType(types: !423)
!423 = !{null, !25, !48, !47, !67, null}
!424 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodV", scope: !29, file: !12, line: 445, baseType: !425, size: 32, offset: 2944)
!425 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !426, size: 32)
!426 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !427)
!427 = !{null, !25, !48, !47, !67, !149}
!428 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodA", scope: !29, file: !12, line: 448, baseType: !429, size: 32, offset: 2976)
!429 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !430, size: 32)
!430 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !431)
!431 = !{null, !25, !48, !47, !67, !156}
!432 = !DIDerivedType(tag: DW_TAG_member, name: "GetFieldID", scope: !29, file: !12, line: 452, baseType: !433, size: 32, offset: 3008)
!433 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !434, size: 32)
!434 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !435)
!435 = !{!74, !25, !47, !51, !51}
!436 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectField", scope: !29, file: !12, line: 455, baseType: !437, size: 32, offset: 3040)
!437 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !438, size: 32)
!438 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !439)
!439 = !{!48, !25, !48, !74}
!440 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanField", scope: !29, file: !12, line: 457, baseType: !441, size: 32, offset: 3072)
!441 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !442, size: 32)
!442 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !443)
!443 = !{!81, !25, !48, !74}
!444 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteField", scope: !29, file: !12, line: 459, baseType: !445, size: 32, offset: 3104)
!445 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !446, size: 32)
!446 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !447)
!447 = !{!56, !25, !48, !74}
!448 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharField", scope: !29, file: !12, line: 461, baseType: !449, size: 32, offset: 3136)
!449 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !450, size: 32)
!450 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !451)
!451 = !{!164, !25, !48, !74}
!452 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortField", scope: !29, file: !12, line: 463, baseType: !453, size: 32, offset: 3168)
!453 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !454, size: 32)
!454 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !455)
!455 = !{!167, !25, !48, !74}
!456 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntField", scope: !29, file: !12, line: 465, baseType: !457, size: 32, offset: 3200)
!457 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !458, size: 32)
!458 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !459)
!459 = !{!40, !25, !48, !74}
!460 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongField", scope: !29, file: !12, line: 467, baseType: !461, size: 32, offset: 3232)
!461 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !462, size: 32)
!462 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !463)
!463 = !{!171, !25, !48, !74}
!464 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatField", scope: !29, file: !12, line: 469, baseType: !465, size: 32, offset: 3264)
!465 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !466, size: 32)
!466 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !467)
!467 = !{!174, !25, !48, !74}
!468 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleField", scope: !29, file: !12, line: 471, baseType: !469, size: 32, offset: 3296)
!469 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !470, size: 32)
!470 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !471)
!471 = !{!177, !25, !48, !74}
!472 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectField", scope: !29, file: !12, line: 474, baseType: !473, size: 32, offset: 3328)
!473 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !474, size: 32)
!474 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !475)
!475 = !{null, !25, !48, !74, !48}
!476 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanField", scope: !29, file: !12, line: 476, baseType: !477, size: 32, offset: 3360)
!477 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !478, size: 32)
!478 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !479)
!479 = !{null, !25, !48, !74, !81}
!480 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteField", scope: !29, file: !12, line: 478, baseType: !481, size: 32, offset: 3392)
!481 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !482, size: 32)
!482 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !483)
!483 = !{null, !25, !48, !74, !56}
!484 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharField", scope: !29, file: !12, line: 480, baseType: !485, size: 32, offset: 3424)
!485 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !486, size: 32)
!486 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !487)
!487 = !{null, !25, !48, !74, !164}
!488 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortField", scope: !29, file: !12, line: 482, baseType: !489, size: 32, offset: 3456)
!489 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !490, size: 32)
!490 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !491)
!491 = !{null, !25, !48, !74, !167}
!492 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntField", scope: !29, file: !12, line: 484, baseType: !493, size: 32, offset: 3488)
!493 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !494, size: 32)
!494 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !495)
!495 = !{null, !25, !48, !74, !40}
!496 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongField", scope: !29, file: !12, line: 486, baseType: !497, size: 32, offset: 3520)
!497 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !498, size: 32)
!498 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !499)
!499 = !{null, !25, !48, !74, !171}
!500 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatField", scope: !29, file: !12, line: 488, baseType: !501, size: 32, offset: 3552)
!501 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !502, size: 32)
!502 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !503)
!503 = !{null, !25, !48, !74, !174}
!504 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleField", scope: !29, file: !12, line: 490, baseType: !505, size: 32, offset: 3584)
!505 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !506, size: 32)
!506 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !507)
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
!518 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !519)
!519 = !{!81, !25, !47, !67, !149}
!520 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethodA", scope: !29, file: !12, line: 507, baseType: !521, size: 32, offset: 3808)
!521 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !522, size: 32)
!522 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !523)
!523 = !{!81, !25, !47, !67, !156}
!524 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethod", scope: !29, file: !12, line: 510, baseType: !525, size: 32, offset: 3840)
!525 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !526, size: 32)
!526 = !DISubroutineType(types: !527)
!527 = !{!56, !25, !47, !67, null}
!528 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodV", scope: !29, file: !12, line: 512, baseType: !529, size: 32, offset: 3872)
!529 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !530, size: 32)
!530 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !531)
!531 = !{!56, !25, !47, !67, !149}
!532 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodA", scope: !29, file: !12, line: 514, baseType: !533, size: 32, offset: 3904)
!533 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !534, size: 32)
!534 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !535)
!535 = !{!56, !25, !47, !67, !156}
!536 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethod", scope: !29, file: !12, line: 517, baseType: !537, size: 32, offset: 3936)
!537 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !538, size: 32)
!538 = !DISubroutineType(types: !539)
!539 = !{!164, !25, !47, !67, null}
!540 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodV", scope: !29, file: !12, line: 519, baseType: !541, size: 32, offset: 3968)
!541 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !542, size: 32)
!542 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !543)
!543 = !{!164, !25, !47, !67, !149}
!544 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodA", scope: !29, file: !12, line: 521, baseType: !545, size: 32, offset: 4000)
!545 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !546, size: 32)
!546 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !547)
!547 = !{!164, !25, !47, !67, !156}
!548 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethod", scope: !29, file: !12, line: 524, baseType: !549, size: 32, offset: 4032)
!549 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !550, size: 32)
!550 = !DISubroutineType(types: !551)
!551 = !{!167, !25, !47, !67, null}
!552 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodV", scope: !29, file: !12, line: 526, baseType: !553, size: 32, offset: 4064)
!553 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !554, size: 32)
!554 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !555)
!555 = !{!167, !25, !47, !67, !149}
!556 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodA", scope: !29, file: !12, line: 528, baseType: !557, size: 32, offset: 4096)
!557 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !558, size: 32)
!558 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !559)
!559 = !{!167, !25, !47, !67, !156}
!560 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethod", scope: !29, file: !12, line: 531, baseType: !561, size: 32, offset: 4128)
!561 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !562, size: 32)
!562 = !DISubroutineType(types: !563)
!563 = !{!40, !25, !47, !67, null}
!564 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodV", scope: !29, file: !12, line: 533, baseType: !565, size: 32, offset: 4160)
!565 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !566, size: 32)
!566 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !567)
!567 = !{!40, !25, !47, !67, !149}
!568 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodA", scope: !29, file: !12, line: 535, baseType: !569, size: 32, offset: 4192)
!569 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !570, size: 32)
!570 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !571)
!571 = !{!40, !25, !47, !67, !156}
!572 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethod", scope: !29, file: !12, line: 538, baseType: !573, size: 32, offset: 4224)
!573 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !574, size: 32)
!574 = !DISubroutineType(types: !575)
!575 = !{!171, !25, !47, !67, null}
!576 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodV", scope: !29, file: !12, line: 540, baseType: !577, size: 32, offset: 4256)
!577 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !578, size: 32)
!578 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !579)
!579 = !{!171, !25, !47, !67, !149}
!580 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodA", scope: !29, file: !12, line: 542, baseType: !581, size: 32, offset: 4288)
!581 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !582, size: 32)
!582 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !583)
!583 = !{!171, !25, !47, !67, !156}
!584 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethod", scope: !29, file: !12, line: 545, baseType: !585, size: 32, offset: 4320)
!585 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !586, size: 32)
!586 = !DISubroutineType(types: !587)
!587 = !{!174, !25, !47, !67, null}
!588 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodV", scope: !29, file: !12, line: 547, baseType: !589, size: 32, offset: 4352)
!589 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !590, size: 32)
!590 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !591)
!591 = !{!174, !25, !47, !67, !149}
!592 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodA", scope: !29, file: !12, line: 549, baseType: !593, size: 32, offset: 4384)
!593 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !594, size: 32)
!594 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !595)
!595 = !{!174, !25, !47, !67, !156}
!596 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethod", scope: !29, file: !12, line: 552, baseType: !597, size: 32, offset: 4416)
!597 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !598, size: 32)
!598 = !DISubroutineType(types: !599)
!599 = !{!177, !25, !47, !67, null}
!600 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodV", scope: !29, file: !12, line: 554, baseType: !601, size: 32, offset: 4448)
!601 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !602, size: 32)
!602 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !603)
!603 = !{!177, !25, !47, !67, !149}
!604 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodA", scope: !29, file: !12, line: 556, baseType: !605, size: 32, offset: 4480)
!605 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !606, size: 32)
!606 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !607)
!607 = !{!177, !25, !47, !67, !156}
!608 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethod", scope: !29, file: !12, line: 559, baseType: !609, size: 32, offset: 4512)
!609 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !610, size: 32)
!610 = !DISubroutineType(types: !611)
!611 = !{null, !25, !47, !67, null}
!612 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodV", scope: !29, file: !12, line: 561, baseType: !613, size: 32, offset: 4544)
!613 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !614, size: 32)
!614 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !615)
!615 = !{null, !25, !47, !67, !149}
!616 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodA", scope: !29, file: !12, line: 563, baseType: !617, size: 32, offset: 4576)
!617 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !618, size: 32)
!618 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !619)
!619 = !{null, !25, !47, !67, !156}
!620 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFieldID", scope: !29, file: !12, line: 566, baseType: !433, size: 32, offset: 4608)
!621 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticObjectField", scope: !29, file: !12, line: 568, baseType: !622, size: 32, offset: 4640)
!622 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !623, size: 32)
!623 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !624)
!624 = !{!48, !25, !47, !74}
!625 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticBooleanField", scope: !29, file: !12, line: 570, baseType: !626, size: 32, offset: 4672)
!626 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !627, size: 32)
!627 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !628)
!628 = !{!81, !25, !47, !74}
!629 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticByteField", scope: !29, file: !12, line: 572, baseType: !630, size: 32, offset: 4704)
!630 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !631, size: 32)
!631 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !632)
!632 = !{!56, !25, !47, !74}
!633 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticCharField", scope: !29, file: !12, line: 574, baseType: !634, size: 32, offset: 4736)
!634 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !635, size: 32)
!635 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !636)
!636 = !{!164, !25, !47, !74}
!637 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticShortField", scope: !29, file: !12, line: 576, baseType: !638, size: 32, offset: 4768)
!638 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !639, size: 32)
!639 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !640)
!640 = !{!167, !25, !47, !74}
!641 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticIntField", scope: !29, file: !12, line: 578, baseType: !642, size: 32, offset: 4800)
!642 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !643, size: 32)
!643 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !644)
!644 = !{!40, !25, !47, !74}
!645 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticLongField", scope: !29, file: !12, line: 580, baseType: !646, size: 32, offset: 4832)
!646 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !647, size: 32)
!647 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !648)
!648 = !{!171, !25, !47, !74}
!649 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFloatField", scope: !29, file: !12, line: 582, baseType: !650, size: 32, offset: 4864)
!650 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !651, size: 32)
!651 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !652)
!652 = !{!174, !25, !47, !74}
!653 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticDoubleField", scope: !29, file: !12, line: 584, baseType: !654, size: 32, offset: 4896)
!654 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !655, size: 32)
!655 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !656)
!656 = !{!177, !25, !47, !74}
!657 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticObjectField", scope: !29, file: !12, line: 587, baseType: !658, size: 32, offset: 4928)
!658 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !659, size: 32)
!659 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !660)
!660 = !{null, !25, !47, !74, !48}
!661 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticBooleanField", scope: !29, file: !12, line: 589, baseType: !662, size: 32, offset: 4960)
!662 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !663, size: 32)
!663 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !664)
!664 = !{null, !25, !47, !74, !81}
!665 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticByteField", scope: !29, file: !12, line: 591, baseType: !666, size: 32, offset: 4992)
!666 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !667, size: 32)
!667 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !668)
!668 = !{null, !25, !47, !74, !56}
!669 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticCharField", scope: !29, file: !12, line: 593, baseType: !670, size: 32, offset: 5024)
!670 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !671, size: 32)
!671 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !672)
!672 = !{null, !25, !47, !74, !164}
!673 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticShortField", scope: !29, file: !12, line: 595, baseType: !674, size: 32, offset: 5056)
!674 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !675, size: 32)
!675 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !676)
!676 = !{null, !25, !47, !74, !167}
!677 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticIntField", scope: !29, file: !12, line: 597, baseType: !678, size: 32, offset: 5088)
!678 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !679, size: 32)
!679 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !680)
!680 = !{null, !25, !47, !74, !40}
!681 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticLongField", scope: !29, file: !12, line: 599, baseType: !682, size: 32, offset: 5120)
!682 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !683, size: 32)
!683 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !684)
!684 = !{null, !25, !47, !74, !171}
!685 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticFloatField", scope: !29, file: !12, line: 601, baseType: !686, size: 32, offset: 5152)
!686 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !687, size: 32)
!687 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !688)
!688 = !{null, !25, !47, !74, !174}
!689 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticDoubleField", scope: !29, file: !12, line: 603, baseType: !690, size: 32, offset: 5184)
!690 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !691, size: 32)
!691 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !692)
!692 = !{null, !25, !47, !74, !177}
!693 = !DIDerivedType(tag: DW_TAG_member, name: "NewString", scope: !29, file: !12, line: 606, baseType: !694, size: 32, offset: 5216)
!694 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !695, size: 32)
!695 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !696)
!696 = !{!697, !25, !698, !58}
!697 = !DIDerivedType(tag: DW_TAG_typedef, name: "jstring", file: !12, line: 104, baseType: !48)
!698 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !699, size: 32)
!699 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !164)
!700 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringLength", scope: !29, file: !12, line: 608, baseType: !701, size: 32, offset: 5248)
!701 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !702, size: 32)
!702 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !703)
!703 = !{!58, !25, !697}
!704 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringChars", scope: !29, file: !12, line: 610, baseType: !705, size: 32, offset: 5280)
!705 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !706, size: 32)
!706 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !707)
!707 = !{!698, !25, !697, !708}
!708 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !81, size: 32)
!709 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringChars", scope: !29, file: !12, line: 612, baseType: !710, size: 32, offset: 5312)
!710 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !711, size: 32)
!711 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !712)
!712 = !{null, !25, !697, !698}
!713 = !DIDerivedType(tag: DW_TAG_member, name: "NewStringUTF", scope: !29, file: !12, line: 615, baseType: !714, size: 32, offset: 5344)
!714 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !715, size: 32)
!715 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !716)
!716 = !{!697, !25, !51}
!717 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFLength", scope: !29, file: !12, line: 617, baseType: !701, size: 32, offset: 5376)
!718 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFChars", scope: !29, file: !12, line: 619, baseType: !719, size: 32, offset: 5408)
!719 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !720, size: 32)
!720 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !721)
!721 = !{!51, !25, !697, !708}
!722 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringUTFChars", scope: !29, file: !12, line: 621, baseType: !723, size: 32, offset: 5440)
!723 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !724, size: 32)
!724 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !725)
!725 = !{null, !25, !697, !51}
!726 = !DIDerivedType(tag: DW_TAG_member, name: "GetArrayLength", scope: !29, file: !12, line: 625, baseType: !727, size: 32, offset: 5472)
!727 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !728, size: 32)
!728 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !729)
!729 = !{!58, !25, !730}
!730 = !DIDerivedType(tag: DW_TAG_typedef, name: "jarray", file: !12, line: 105, baseType: !48)
!731 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectArray", scope: !29, file: !12, line: 628, baseType: !732, size: 32, offset: 5504)
!732 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !733, size: 32)
!733 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !734)
!734 = !{!735, !25, !58, !47, !48}
!735 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectArray", file: !12, line: 114, baseType: !730)
!736 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectArrayElement", scope: !29, file: !12, line: 630, baseType: !737, size: 32, offset: 5536)
!737 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !738, size: 32)
!738 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !739)
!739 = !{!48, !25, !735, !58}
!740 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectArrayElement", scope: !29, file: !12, line: 632, baseType: !741, size: 32, offset: 5568)
!741 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !742, size: 32)
!742 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !743)
!743 = !{null, !25, !735, !58, !48}
!744 = !DIDerivedType(tag: DW_TAG_member, name: "NewBooleanArray", scope: !29, file: !12, line: 635, baseType: !745, size: 32, offset: 5600)
!745 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !746, size: 32)
!746 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !747)
!747 = !{!748, !25, !58}
!748 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbooleanArray", file: !12, line: 106, baseType: !730)
!749 = !DIDerivedType(tag: DW_TAG_member, name: "NewByteArray", scope: !29, file: !12, line: 637, baseType: !750, size: 32, offset: 5632)
!750 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !751, size: 32)
!751 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !752)
!752 = !{!753, !25, !58}
!753 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbyteArray", file: !12, line: 107, baseType: !730)
!754 = !DIDerivedType(tag: DW_TAG_member, name: "NewCharArray", scope: !29, file: !12, line: 639, baseType: !755, size: 32, offset: 5664)
!755 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !756, size: 32)
!756 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !757)
!757 = !{!758, !25, !58}
!758 = !DIDerivedType(tag: DW_TAG_typedef, name: "jcharArray", file: !12, line: 108, baseType: !730)
!759 = !DIDerivedType(tag: DW_TAG_member, name: "NewShortArray", scope: !29, file: !12, line: 641, baseType: !760, size: 32, offset: 5696)
!760 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !761, size: 32)
!761 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !762)
!762 = !{!763, !25, !58}
!763 = !DIDerivedType(tag: DW_TAG_typedef, name: "jshortArray", file: !12, line: 109, baseType: !730)
!764 = !DIDerivedType(tag: DW_TAG_member, name: "NewIntArray", scope: !29, file: !12, line: 643, baseType: !765, size: 32, offset: 5728)
!765 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !766, size: 32)
!766 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !767)
!767 = !{!768, !25, !58}
!768 = !DIDerivedType(tag: DW_TAG_typedef, name: "jintArray", file: !12, line: 110, baseType: !730)
!769 = !DIDerivedType(tag: DW_TAG_member, name: "NewLongArray", scope: !29, file: !12, line: 645, baseType: !770, size: 32, offset: 5760)
!770 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !771, size: 32)
!771 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !772)
!772 = !{!773, !25, !58}
!773 = !DIDerivedType(tag: DW_TAG_typedef, name: "jlongArray", file: !12, line: 111, baseType: !730)
!774 = !DIDerivedType(tag: DW_TAG_member, name: "NewFloatArray", scope: !29, file: !12, line: 647, baseType: !775, size: 32, offset: 5792)
!775 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !776, size: 32)
!776 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !777)
!777 = !{!778, !25, !58}
!778 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfloatArray", file: !12, line: 112, baseType: !730)
!779 = !DIDerivedType(tag: DW_TAG_member, name: "NewDoubleArray", scope: !29, file: !12, line: 649, baseType: !780, size: 32, offset: 5824)
!780 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !781, size: 32)
!781 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !782)
!782 = !{!783, !25, !58}
!783 = !DIDerivedType(tag: DW_TAG_typedef, name: "jdoubleArray", file: !12, line: 113, baseType: !730)
!784 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayElements", scope: !29, file: !12, line: 652, baseType: !785, size: 32, offset: 5856)
!785 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !786, size: 32)
!786 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !787)
!787 = !{!708, !25, !748, !708}
!788 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayElements", scope: !29, file: !12, line: 654, baseType: !789, size: 32, offset: 5888)
!789 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !790, size: 32)
!790 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !791)
!791 = !{!792, !25, !753, !708}
!792 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !56, size: 32)
!793 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayElements", scope: !29, file: !12, line: 656, baseType: !794, size: 32, offset: 5920)
!794 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !795, size: 32)
!795 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !796)
!796 = !{!797, !25, !758, !708}
!797 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !164, size: 32)
!798 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayElements", scope: !29, file: !12, line: 658, baseType: !799, size: 32, offset: 5952)
!799 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !800, size: 32)
!800 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !801)
!801 = !{!802, !25, !763, !708}
!802 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !167, size: 32)
!803 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayElements", scope: !29, file: !12, line: 660, baseType: !804, size: 32, offset: 5984)
!804 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !805, size: 32)
!805 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !806)
!806 = !{!807, !25, !768, !708}
!807 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !40, size: 32)
!808 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayElements", scope: !29, file: !12, line: 662, baseType: !809, size: 32, offset: 6016)
!809 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !810, size: 32)
!810 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !811)
!811 = !{!812, !25, !773, !708}
!812 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !171, size: 32)
!813 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayElements", scope: !29, file: !12, line: 664, baseType: !814, size: 32, offset: 6048)
!814 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !815, size: 32)
!815 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !816)
!816 = !{!817, !25, !778, !708}
!817 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !174, size: 32)
!818 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayElements", scope: !29, file: !12, line: 666, baseType: !819, size: 32, offset: 6080)
!819 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !820, size: 32)
!820 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !821)
!821 = !{!822, !25, !783, !708}
!822 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !177, size: 32)
!823 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseBooleanArrayElements", scope: !29, file: !12, line: 669, baseType: !824, size: 32, offset: 6112)
!824 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !825, size: 32)
!825 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !826)
!826 = !{null, !25, !748, !708, !40}
!827 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseByteArrayElements", scope: !29, file: !12, line: 671, baseType: !828, size: 32, offset: 6144)
!828 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !829, size: 32)
!829 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !830)
!830 = !{null, !25, !753, !792, !40}
!831 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseCharArrayElements", scope: !29, file: !12, line: 673, baseType: !832, size: 32, offset: 6176)
!832 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !833, size: 32)
!833 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !834)
!834 = !{null, !25, !758, !797, !40}
!835 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseShortArrayElements", scope: !29, file: !12, line: 675, baseType: !836, size: 32, offset: 6208)
!836 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !837, size: 32)
!837 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !838)
!838 = !{null, !25, !763, !802, !40}
!839 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseIntArrayElements", scope: !29, file: !12, line: 677, baseType: !840, size: 32, offset: 6240)
!840 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !841, size: 32)
!841 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !842)
!842 = !{null, !25, !768, !807, !40}
!843 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseLongArrayElements", scope: !29, file: !12, line: 679, baseType: !844, size: 32, offset: 6272)
!844 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !845, size: 32)
!845 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !846)
!846 = !{null, !25, !773, !812, !40}
!847 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseFloatArrayElements", scope: !29, file: !12, line: 681, baseType: !848, size: 32, offset: 6304)
!848 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !849, size: 32)
!849 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !850)
!850 = !{null, !25, !778, !817, !40}
!851 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseDoubleArrayElements", scope: !29, file: !12, line: 683, baseType: !852, size: 32, offset: 6336)
!852 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !853, size: 32)
!853 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !854)
!854 = !{null, !25, !783, !822, !40}
!855 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayRegion", scope: !29, file: !12, line: 686, baseType: !856, size: 32, offset: 6368)
!856 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !857, size: 32)
!857 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !858)
!858 = !{null, !25, !748, !58, !58, !708}
!859 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayRegion", scope: !29, file: !12, line: 688, baseType: !860, size: 32, offset: 6400)
!860 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !861, size: 32)
!861 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !862)
!862 = !{null, !25, !753, !58, !58, !792}
!863 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayRegion", scope: !29, file: !12, line: 690, baseType: !864, size: 32, offset: 6432)
!864 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !865, size: 32)
!865 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !866)
!866 = !{null, !25, !758, !58, !58, !797}
!867 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayRegion", scope: !29, file: !12, line: 692, baseType: !868, size: 32, offset: 6464)
!868 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !869, size: 32)
!869 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !870)
!870 = !{null, !25, !763, !58, !58, !802}
!871 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayRegion", scope: !29, file: !12, line: 694, baseType: !872, size: 32, offset: 6496)
!872 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !873, size: 32)
!873 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !874)
!874 = !{null, !25, !768, !58, !58, !807}
!875 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayRegion", scope: !29, file: !12, line: 696, baseType: !876, size: 32, offset: 6528)
!876 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !877, size: 32)
!877 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !878)
!878 = !{null, !25, !773, !58, !58, !812}
!879 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayRegion", scope: !29, file: !12, line: 698, baseType: !880, size: 32, offset: 6560)
!880 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !881, size: 32)
!881 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !882)
!882 = !{null, !25, !778, !58, !58, !817}
!883 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayRegion", scope: !29, file: !12, line: 700, baseType: !884, size: 32, offset: 6592)
!884 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !885, size: 32)
!885 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !886)
!886 = !{null, !25, !783, !58, !58, !822}
!887 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanArrayRegion", scope: !29, file: !12, line: 703, baseType: !888, size: 32, offset: 6624)
!888 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !889, size: 32)
!889 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !890)
!890 = !{null, !25, !748, !58, !58, !891}
!891 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !892, size: 32)
!892 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !81)
!893 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteArrayRegion", scope: !29, file: !12, line: 705, baseType: !894, size: 32, offset: 6656)
!894 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !895, size: 32)
!895 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !896)
!896 = !{null, !25, !753, !58, !58, !54}
!897 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharArrayRegion", scope: !29, file: !12, line: 707, baseType: !898, size: 32, offset: 6688)
!898 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !899, size: 32)
!899 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !900)
!900 = !{null, !25, !758, !58, !58, !698}
!901 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortArrayRegion", scope: !29, file: !12, line: 709, baseType: !902, size: 32, offset: 6720)
!902 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !903, size: 32)
!903 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !904)
!904 = !{null, !25, !763, !58, !58, !905}
!905 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !906, size: 32)
!906 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !167)
!907 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntArrayRegion", scope: !29, file: !12, line: 711, baseType: !908, size: 32, offset: 6752)
!908 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !909, size: 32)
!909 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !910)
!910 = !{null, !25, !768, !58, !58, !911}
!911 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !912, size: 32)
!912 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !40)
!913 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongArrayRegion", scope: !29, file: !12, line: 713, baseType: !914, size: 32, offset: 6784)
!914 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !915, size: 32)
!915 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !916)
!916 = !{null, !25, !773, !58, !58, !917}
!917 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !918, size: 32)
!918 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !171)
!919 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatArrayRegion", scope: !29, file: !12, line: 715, baseType: !920, size: 32, offset: 6816)
!920 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !921, size: 32)
!921 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !922)
!922 = !{null, !25, !778, !58, !58, !923}
!923 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !924, size: 32)
!924 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !174)
!925 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleArrayRegion", scope: !29, file: !12, line: 717, baseType: !926, size: 32, offset: 6848)
!926 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !927, size: 32)
!927 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !928)
!928 = !{null, !25, !783, !58, !58, !929}
!929 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !930, size: 32)
!930 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !177)
!931 = !DIDerivedType(tag: DW_TAG_member, name: "RegisterNatives", scope: !29, file: !12, line: 720, baseType: !932, size: 32, offset: 6880)
!932 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !933, size: 32)
!933 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !934)
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
!945 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !946)
!946 = !{!40, !25, !47}
!947 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorEnter", scope: !29, file: !12, line: 726, baseType: !948, size: 32, offset: 6944)
!948 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !949, size: 32)
!949 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !950)
!950 = !{!40, !25, !48}
!951 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorExit", scope: !29, file: !12, line: 728, baseType: !948, size: 32, offset: 6976)
!952 = !DIDerivedType(tag: DW_TAG_member, name: "GetJavaVM", scope: !29, file: !12, line: 731, baseType: !953, size: 32, offset: 7008)
!953 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !954, size: 32)
!954 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !955)
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
!968 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !969)
!969 = !{!40, !957}
!970 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThread", scope: !961, file: !12, line: 1897, baseType: !971, size: 32, offset: 128)
!971 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !972, size: 32)
!972 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !973)
!973 = !{!40, !957, !974, !32}
!974 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !32, size: 32)
!975 = !DIDerivedType(tag: DW_TAG_member, name: "DetachCurrentThread", scope: !961, file: !12, line: 1899, baseType: !967, size: 32, offset: 160)
!976 = !DIDerivedType(tag: DW_TAG_member, name: "GetEnv", scope: !961, file: !12, line: 1901, baseType: !977, size: 32, offset: 192)
!977 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !978, size: 32)
!978 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !979)
!979 = !{!40, !957, !974, !40}
!980 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThreadAsDaemon", scope: !961, file: !12, line: 1903, baseType: !971, size: 32, offset: 224)
!981 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringRegion", scope: !29, file: !12, line: 734, baseType: !982, size: 32, offset: 7040)
!982 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !983, size: 32)
!983 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !984)
!984 = !{null, !25, !697, !58, !58, !797}
!985 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFRegion", scope: !29, file: !12, line: 736, baseType: !986, size: 32, offset: 7072)
!986 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !987, size: 32)
!987 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !988)
!988 = !{null, !25, !697, !58, !58, !151}
!989 = !DIDerivedType(tag: DW_TAG_member, name: "GetPrimitiveArrayCritical", scope: !29, file: !12, line: 739, baseType: !990, size: 32, offset: 7104)
!990 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !991, size: 32)
!991 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !992)
!992 = !{!32, !25, !730, !708}
!993 = !DIDerivedType(tag: DW_TAG_member, name: "ReleasePrimitiveArrayCritical", scope: !29, file: !12, line: 741, baseType: !994, size: 32, offset: 7136)
!994 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !995, size: 32)
!995 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !996)
!996 = !{null, !25, !730, !32, !40}
!997 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringCritical", scope: !29, file: !12, line: 744, baseType: !705, size: 32, offset: 7168)
!998 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringCritical", scope: !29, file: !12, line: 746, baseType: !710, size: 32, offset: 7200)
!999 = !DIDerivedType(tag: DW_TAG_member, name: "NewWeakGlobalRef", scope: !29, file: !12, line: 749, baseType: !1000, size: 32, offset: 7232)
!1000 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1001, size: 32)
!1001 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1002)
!1002 = !{!1003, !25, !48}
!1003 = !DIDerivedType(tag: DW_TAG_typedef, name: "jweak", file: !12, line: 118, baseType: !48)
!1004 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteWeakGlobalRef", scope: !29, file: !12, line: 751, baseType: !1005, size: 32, offset: 7264)
!1005 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1006, size: 32)
!1006 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1007)
!1007 = !{null, !25, !1003}
!1008 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionCheck", scope: !29, file: !12, line: 754, baseType: !1009, size: 32, offset: 7296)
!1009 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1010, size: 32)
!1010 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1011)
!1011 = !{!81, !25}
!1012 = !DIDerivedType(tag: DW_TAG_member, name: "NewDirectByteBuffer", scope: !29, file: !12, line: 757, baseType: !1013, size: 32, offset: 7328)
!1013 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1014, size: 32)
!1014 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1015)
!1015 = !{!48, !25, !32, !171}
!1016 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferAddress", scope: !29, file: !12, line: 759, baseType: !1017, size: 32, offset: 7360)
!1017 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1018, size: 32)
!1018 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1019)
!1019 = !{!32, !25, !48}
!1020 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferCapacity", scope: !29, file: !12, line: 761, baseType: !1021, size: 32, offset: 7392)
!1021 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1022, size: 32)
!1022 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1023)
!1023 = !{!171, !25, !48}
!1024 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectRefType", scope: !29, file: !12, line: 766, baseType: !1025, size: 32, offset: 7424)
!1025 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1026, size: 32)
!1026 = !DISubroutineType(cc: DW_CC_BORLAND_stdcall, types: !1027)
!1027 = !{!1028, !25, !48}
!1028 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectRefType", file: !12, line: 144, baseType: !11)
!1029 = !DIDerivedType(tag: DW_TAG_typedef, name: "size_t", file: !1030, line: 197, baseType: !1031)
!1030 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vcruntime.h", directory: "", checksumkind: CSK_MD5, checksum: "39da3a8c8438e40538f3964bd55ef6b8")
!1031 = !DIBasicType(name: "unsigned int", size: 32, encoding: DW_ATE_unsigned)
!1032 = !{!0}
!1033 = !{}
!1034 = !{i32 1, !"NumRegisterParameters", i32 0}
!1035 = !{i32 2, !"CodeView", i32 1}
!1036 = !{i32 2, !"Debug Info Version", i32 3}
!1037 = !{i32 1, !"wchar_size", i32 2}
!1038 = !{i32 7, !"frame-pointer", i32 2}
!1039 = !{!"clang version 15.0.2"}
!1040 = distinct !DISubprogram(name: "sprintf", scope: !1041, file: !1041, line: 1764, type: !1042, scopeLine: 1771, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1041 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\stdio.h", directory: "", checksumkind: CSK_MD5, checksum: "c1a1fbc43e7d45f0ea4ae539ddcffb19")
!1042 = !DISubroutineType(types: !1043)
!1043 = !{!13, !1044, !1045, null}
!1044 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !151)
!1045 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !51)
!1046 = !DILocalVariable(name: "_Format", arg: 2, scope: !1040, file: !1041, line: 1766, type: !1045)
!1047 = !DILocation(line: 1766, scope: !1040)
!1048 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1040, file: !1041, line: 1765, type: !1044)
!1049 = !DILocation(line: 1765, scope: !1040)
!1050 = !DILocalVariable(name: "_Result", scope: !1040, file: !1041, line: 1772, type: !13)
!1051 = !DILocation(line: 1772, scope: !1040)
!1052 = !DILocalVariable(name: "_ArgList", scope: !1040, file: !1041, line: 1773, type: !149)
!1053 = !DILocation(line: 1773, scope: !1040)
!1054 = !DILocation(line: 1774, scope: !1040)
!1055 = !DILocation(line: 1776, scope: !1040)
!1056 = !DILocation(line: 1778, scope: !1040)
!1057 = !DILocation(line: 1779, scope: !1040)
!1058 = distinct !DISubprogram(name: "vsprintf", scope: !1041, file: !1041, line: 1465, type: !1059, scopeLine: 1473, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1059 = !DISubroutineType(types: !1060)
!1060 = !{!13, !1044, !1045, !149}
!1061 = !DILocalVariable(name: "_ArgList", arg: 3, scope: !1058, file: !1041, line: 1468, type: !149)
!1062 = !DILocation(line: 1468, scope: !1058)
!1063 = !DILocalVariable(name: "_Format", arg: 2, scope: !1058, file: !1041, line: 1467, type: !1045)
!1064 = !DILocation(line: 1467, scope: !1058)
!1065 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1058, file: !1041, line: 1466, type: !1044)
!1066 = !DILocation(line: 1466, scope: !1058)
!1067 = !DILocation(line: 1474, scope: !1058)
!1068 = distinct !DISubprogram(name: "_snprintf", scope: !1041, file: !1041, line: 1939, type: !1069, scopeLine: 1947, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1069 = !DISubroutineType(types: !1070)
!1070 = !{!13, !1044, !1071, !1045, null}
!1071 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !1029)
!1072 = !DILocalVariable(name: "_Format", arg: 3, scope: !1068, file: !1041, line: 1942, type: !1045)
!1073 = !DILocation(line: 1942, scope: !1068)
!1074 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1068, file: !1041, line: 1941, type: !1071)
!1075 = !DILocation(line: 1941, scope: !1068)
!1076 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1068, file: !1041, line: 1940, type: !1044)
!1077 = !DILocation(line: 1940, scope: !1068)
!1078 = !DILocalVariable(name: "_Result", scope: !1068, file: !1041, line: 1948, type: !13)
!1079 = !DILocation(line: 1948, scope: !1068)
!1080 = !DILocalVariable(name: "_ArgList", scope: !1068, file: !1041, line: 1949, type: !149)
!1081 = !DILocation(line: 1949, scope: !1068)
!1082 = !DILocation(line: 1950, scope: !1068)
!1083 = !DILocation(line: 1951, scope: !1068)
!1084 = !DILocation(line: 1952, scope: !1068)
!1085 = !DILocation(line: 1953, scope: !1068)
!1086 = distinct !DISubprogram(name: "_vsnprintf", scope: !1041, file: !1041, line: 1402, type: !1087, scopeLine: 1411, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1087 = !DISubroutineType(types: !1088)
!1088 = !{!13, !1044, !1071, !1045, !149}
!1089 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !1086, file: !1041, line: 1406, type: !149)
!1090 = !DILocation(line: 1406, scope: !1086)
!1091 = !DILocalVariable(name: "_Format", arg: 3, scope: !1086, file: !1041, line: 1405, type: !1045)
!1092 = !DILocation(line: 1405, scope: !1086)
!1093 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1086, file: !1041, line: 1404, type: !1071)
!1094 = !DILocation(line: 1404, scope: !1086)
!1095 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1086, file: !1041, line: 1403, type: !1044)
!1096 = !DILocation(line: 1403, scope: !1086)
!1097 = !DILocation(line: 1412, scope: !1086)
!1098 = distinct !DISubprogram(name: "JNI_CallObjectMethod", scope: !9, file: !9, line: 3, type: !194, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1099 = !DILocalVariable(name: "methodID", arg: 3, scope: !1098, file: !9, line: 3, type: !67)
!1100 = !DILocation(line: 3, scope: !1098)
!1101 = !DILocalVariable(name: "obj", arg: 2, scope: !1098, file: !9, line: 3, type: !48)
!1102 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1098, file: !9, line: 3, type: !25)
!1103 = !DILocalVariable(name: "args", scope: !1098, file: !9, line: 3, type: !149)
!1104 = !DILocalVariable(name: "sig", scope: !1098, file: !9, line: 3, type: !1105)
!1105 = !DICompositeType(tag: DW_TAG_array_type, baseType: !53, size: 2048, elements: !1106)
!1106 = !{!1107}
!1107 = !DISubrange(count: 256)
!1108 = !DILocalVariable(name: "argc", scope: !1098, file: !9, line: 3, type: !13)
!1109 = !DILocalVariable(name: "argv", scope: !1098, file: !9, line: 3, type: !1110)
!1110 = !DICompositeType(tag: DW_TAG_array_type, baseType: !158, size: 16384, elements: !1106)
!1111 = !DILocalVariable(name: "i", scope: !1112, file: !9, line: 3, type: !13)
!1112 = distinct !DILexicalBlock(scope: !1098, file: !9, line: 3)
!1113 = !DILocation(line: 3, scope: !1112)
!1114 = !DILocation(line: 3, scope: !1115)
!1115 = distinct !DILexicalBlock(scope: !1116, file: !9, line: 3)
!1116 = distinct !DILexicalBlock(scope: !1112, file: !9, line: 3)
!1117 = !DILocation(line: 3, scope: !1118)
!1118 = distinct !DILexicalBlock(scope: !1115, file: !9, line: 3)
!1119 = !DILocation(line: 3, scope: !1116)
!1120 = distinct !{!1120, !1113, !1113, !1121}
!1121 = !{!"llvm.loop.mustprogress"}
!1122 = !DILocalVariable(name: "ret", scope: !1098, file: !9, line: 3, type: !48)
!1123 = distinct !DISubprogram(name: "JNI_CallObjectMethodV", linkageName: "\01_JNI_CallObjectMethodV@16", scope: !9, file: !9, line: 3, type: !198, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1124 = !DILocalVariable(name: "args", arg: 4, scope: !1123, file: !9, line: 3, type: !149)
!1125 = !DILocation(line: 3, scope: !1123)
!1126 = !DILocalVariable(name: "methodID", arg: 3, scope: !1123, file: !9, line: 3, type: !67)
!1127 = !DILocalVariable(name: "obj", arg: 2, scope: !1123, file: !9, line: 3, type: !48)
!1128 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1123, file: !9, line: 3, type: !25)
!1129 = !DILocalVariable(name: "sig", scope: !1123, file: !9, line: 3, type: !1105)
!1130 = !DILocalVariable(name: "argc", scope: !1123, file: !9, line: 3, type: !13)
!1131 = !DILocalVariable(name: "argv", scope: !1123, file: !9, line: 3, type: !1110)
!1132 = !DILocalVariable(name: "i", scope: !1133, file: !9, line: 3, type: !13)
!1133 = distinct !DILexicalBlock(scope: !1123, file: !9, line: 3)
!1134 = !DILocation(line: 3, scope: !1133)
!1135 = !DILocation(line: 3, scope: !1136)
!1136 = distinct !DILexicalBlock(scope: !1137, file: !9, line: 3)
!1137 = distinct !DILexicalBlock(scope: !1133, file: !9, line: 3)
!1138 = !DILocation(line: 3, scope: !1139)
!1139 = distinct !DILexicalBlock(scope: !1136, file: !9, line: 3)
!1140 = !DILocation(line: 3, scope: !1137)
!1141 = distinct !{!1141, !1134, !1134, !1121}
!1142 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethod", scope: !9, file: !9, line: 3, type: !314, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1143 = !DILocalVariable(name: "methodID", arg: 4, scope: !1142, file: !9, line: 3, type: !67)
!1144 = !DILocation(line: 3, scope: !1142)
!1145 = !DILocalVariable(name: "clazz", arg: 3, scope: !1142, file: !9, line: 3, type: !47)
!1146 = !DILocalVariable(name: "obj", arg: 2, scope: !1142, file: !9, line: 3, type: !48)
!1147 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1142, file: !9, line: 3, type: !25)
!1148 = !DILocalVariable(name: "args", scope: !1142, file: !9, line: 3, type: !149)
!1149 = !DILocalVariable(name: "sig", scope: !1142, file: !9, line: 3, type: !1105)
!1150 = !DILocalVariable(name: "argc", scope: !1142, file: !9, line: 3, type: !13)
!1151 = !DILocalVariable(name: "argv", scope: !1142, file: !9, line: 3, type: !1110)
!1152 = !DILocalVariable(name: "i", scope: !1153, file: !9, line: 3, type: !13)
!1153 = distinct !DILexicalBlock(scope: !1142, file: !9, line: 3)
!1154 = !DILocation(line: 3, scope: !1153)
!1155 = !DILocation(line: 3, scope: !1156)
!1156 = distinct !DILexicalBlock(scope: !1157, file: !9, line: 3)
!1157 = distinct !DILexicalBlock(scope: !1153, file: !9, line: 3)
!1158 = !DILocation(line: 3, scope: !1159)
!1159 = distinct !DILexicalBlock(scope: !1156, file: !9, line: 3)
!1160 = !DILocation(line: 3, scope: !1157)
!1161 = distinct !{!1161, !1154, !1154, !1121}
!1162 = !DILocalVariable(name: "ret", scope: !1142, file: !9, line: 3, type: !48)
!1163 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethodV", linkageName: "\01_JNI_CallNonvirtualObjectMethodV@20", scope: !9, file: !9, line: 3, type: !318, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1164 = !DILocalVariable(name: "args", arg: 5, scope: !1163, file: !9, line: 3, type: !149)
!1165 = !DILocation(line: 3, scope: !1163)
!1166 = !DILocalVariable(name: "methodID", arg: 4, scope: !1163, file: !9, line: 3, type: !67)
!1167 = !DILocalVariable(name: "clazz", arg: 3, scope: !1163, file: !9, line: 3, type: !47)
!1168 = !DILocalVariable(name: "obj", arg: 2, scope: !1163, file: !9, line: 3, type: !48)
!1169 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1163, file: !9, line: 3, type: !25)
!1170 = !DILocalVariable(name: "sig", scope: !1163, file: !9, line: 3, type: !1105)
!1171 = !DILocalVariable(name: "argc", scope: !1163, file: !9, line: 3, type: !13)
!1172 = !DILocalVariable(name: "argv", scope: !1163, file: !9, line: 3, type: !1110)
!1173 = !DILocalVariable(name: "i", scope: !1174, file: !9, line: 3, type: !13)
!1174 = distinct !DILexicalBlock(scope: !1163, file: !9, line: 3)
!1175 = !DILocation(line: 3, scope: !1174)
!1176 = !DILocation(line: 3, scope: !1177)
!1177 = distinct !DILexicalBlock(scope: !1178, file: !9, line: 3)
!1178 = distinct !DILexicalBlock(scope: !1174, file: !9, line: 3)
!1179 = !DILocation(line: 3, scope: !1180)
!1180 = distinct !DILexicalBlock(scope: !1177, file: !9, line: 3)
!1181 = !DILocation(line: 3, scope: !1178)
!1182 = distinct !{!1182, !1175, !1175, !1121}
!1183 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethod", scope: !9, file: !9, line: 3, type: !143, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1184 = !DILocalVariable(name: "methodID", arg: 3, scope: !1183, file: !9, line: 3, type: !67)
!1185 = !DILocation(line: 3, scope: !1183)
!1186 = !DILocalVariable(name: "clazz", arg: 2, scope: !1183, file: !9, line: 3, type: !47)
!1187 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1183, file: !9, line: 3, type: !25)
!1188 = !DILocalVariable(name: "args", scope: !1183, file: !9, line: 3, type: !149)
!1189 = !DILocalVariable(name: "sig", scope: !1183, file: !9, line: 3, type: !1105)
!1190 = !DILocalVariable(name: "argc", scope: !1183, file: !9, line: 3, type: !13)
!1191 = !DILocalVariable(name: "argv", scope: !1183, file: !9, line: 3, type: !1110)
!1192 = !DILocalVariable(name: "i", scope: !1193, file: !9, line: 3, type: !13)
!1193 = distinct !DILexicalBlock(scope: !1183, file: !9, line: 3)
!1194 = !DILocation(line: 3, scope: !1193)
!1195 = !DILocation(line: 3, scope: !1196)
!1196 = distinct !DILexicalBlock(scope: !1197, file: !9, line: 3)
!1197 = distinct !DILexicalBlock(scope: !1193, file: !9, line: 3)
!1198 = !DILocation(line: 3, scope: !1199)
!1199 = distinct !DILexicalBlock(scope: !1196, file: !9, line: 3)
!1200 = !DILocation(line: 3, scope: !1197)
!1201 = distinct !{!1201, !1194, !1194, !1121}
!1202 = !DILocalVariable(name: "ret", scope: !1183, file: !9, line: 3, type: !48)
!1203 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethodV", linkageName: "\01_JNI_CallStaticObjectMethodV@16", scope: !9, file: !9, line: 3, type: !147, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1204 = !DILocalVariable(name: "args", arg: 4, scope: !1203, file: !9, line: 3, type: !149)
!1205 = !DILocation(line: 3, scope: !1203)
!1206 = !DILocalVariable(name: "methodID", arg: 3, scope: !1203, file: !9, line: 3, type: !67)
!1207 = !DILocalVariable(name: "clazz", arg: 2, scope: !1203, file: !9, line: 3, type: !47)
!1208 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1203, file: !9, line: 3, type: !25)
!1209 = !DILocalVariable(name: "sig", scope: !1203, file: !9, line: 3, type: !1105)
!1210 = !DILocalVariable(name: "argc", scope: !1203, file: !9, line: 3, type: !13)
!1211 = !DILocalVariable(name: "argv", scope: !1203, file: !9, line: 3, type: !1110)
!1212 = !DILocalVariable(name: "i", scope: !1213, file: !9, line: 3, type: !13)
!1213 = distinct !DILexicalBlock(scope: !1203, file: !9, line: 3)
!1214 = !DILocation(line: 3, scope: !1213)
!1215 = !DILocation(line: 3, scope: !1216)
!1216 = distinct !DILexicalBlock(scope: !1217, file: !9, line: 3)
!1217 = distinct !DILexicalBlock(scope: !1213, file: !9, line: 3)
!1218 = !DILocation(line: 3, scope: !1219)
!1219 = distinct !DILexicalBlock(scope: !1216, file: !9, line: 3)
!1220 = !DILocation(line: 3, scope: !1217)
!1221 = distinct !{!1221, !1214, !1214, !1121}
!1222 = distinct !DISubprogram(name: "JNI_CallBooleanMethod", scope: !9, file: !9, line: 4, type: !206, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1223 = !DILocalVariable(name: "methodID", arg: 3, scope: !1222, file: !9, line: 4, type: !67)
!1224 = !DILocation(line: 4, scope: !1222)
!1225 = !DILocalVariable(name: "obj", arg: 2, scope: !1222, file: !9, line: 4, type: !48)
!1226 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1222, file: !9, line: 4, type: !25)
!1227 = !DILocalVariable(name: "args", scope: !1222, file: !9, line: 4, type: !149)
!1228 = !DILocalVariable(name: "sig", scope: !1222, file: !9, line: 4, type: !1105)
!1229 = !DILocalVariable(name: "argc", scope: !1222, file: !9, line: 4, type: !13)
!1230 = !DILocalVariable(name: "argv", scope: !1222, file: !9, line: 4, type: !1110)
!1231 = !DILocalVariable(name: "i", scope: !1232, file: !9, line: 4, type: !13)
!1232 = distinct !DILexicalBlock(scope: !1222, file: !9, line: 4)
!1233 = !DILocation(line: 4, scope: !1232)
!1234 = !DILocation(line: 4, scope: !1235)
!1235 = distinct !DILexicalBlock(scope: !1236, file: !9, line: 4)
!1236 = distinct !DILexicalBlock(scope: !1232, file: !9, line: 4)
!1237 = !DILocation(line: 4, scope: !1238)
!1238 = distinct !DILexicalBlock(scope: !1235, file: !9, line: 4)
!1239 = !DILocation(line: 4, scope: !1236)
!1240 = distinct !{!1240, !1233, !1233, !1121}
!1241 = !DILocalVariable(name: "ret", scope: !1222, file: !9, line: 4, type: !81)
!1242 = distinct !DISubprogram(name: "JNI_CallBooleanMethodV", linkageName: "\01_JNI_CallBooleanMethodV@16", scope: !9, file: !9, line: 4, type: !210, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1243 = !DILocalVariable(name: "args", arg: 4, scope: !1242, file: !9, line: 4, type: !149)
!1244 = !DILocation(line: 4, scope: !1242)
!1245 = !DILocalVariable(name: "methodID", arg: 3, scope: !1242, file: !9, line: 4, type: !67)
!1246 = !DILocalVariable(name: "obj", arg: 2, scope: !1242, file: !9, line: 4, type: !48)
!1247 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1242, file: !9, line: 4, type: !25)
!1248 = !DILocalVariable(name: "sig", scope: !1242, file: !9, line: 4, type: !1105)
!1249 = !DILocalVariable(name: "argc", scope: !1242, file: !9, line: 4, type: !13)
!1250 = !DILocalVariable(name: "argv", scope: !1242, file: !9, line: 4, type: !1110)
!1251 = !DILocalVariable(name: "i", scope: !1252, file: !9, line: 4, type: !13)
!1252 = distinct !DILexicalBlock(scope: !1242, file: !9, line: 4)
!1253 = !DILocation(line: 4, scope: !1252)
!1254 = !DILocation(line: 4, scope: !1255)
!1255 = distinct !DILexicalBlock(scope: !1256, file: !9, line: 4)
!1256 = distinct !DILexicalBlock(scope: !1252, file: !9, line: 4)
!1257 = !DILocation(line: 4, scope: !1258)
!1258 = distinct !DILexicalBlock(scope: !1255, file: !9, line: 4)
!1259 = !DILocation(line: 4, scope: !1256)
!1260 = distinct !{!1260, !1253, !1253, !1121}
!1261 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethod", scope: !9, file: !9, line: 4, type: !326, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1262 = !DILocalVariable(name: "methodID", arg: 4, scope: !1261, file: !9, line: 4, type: !67)
!1263 = !DILocation(line: 4, scope: !1261)
!1264 = !DILocalVariable(name: "clazz", arg: 3, scope: !1261, file: !9, line: 4, type: !47)
!1265 = !DILocalVariable(name: "obj", arg: 2, scope: !1261, file: !9, line: 4, type: !48)
!1266 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1261, file: !9, line: 4, type: !25)
!1267 = !DILocalVariable(name: "args", scope: !1261, file: !9, line: 4, type: !149)
!1268 = !DILocalVariable(name: "sig", scope: !1261, file: !9, line: 4, type: !1105)
!1269 = !DILocalVariable(name: "argc", scope: !1261, file: !9, line: 4, type: !13)
!1270 = !DILocalVariable(name: "argv", scope: !1261, file: !9, line: 4, type: !1110)
!1271 = !DILocalVariable(name: "i", scope: !1272, file: !9, line: 4, type: !13)
!1272 = distinct !DILexicalBlock(scope: !1261, file: !9, line: 4)
!1273 = !DILocation(line: 4, scope: !1272)
!1274 = !DILocation(line: 4, scope: !1275)
!1275 = distinct !DILexicalBlock(scope: !1276, file: !9, line: 4)
!1276 = distinct !DILexicalBlock(scope: !1272, file: !9, line: 4)
!1277 = !DILocation(line: 4, scope: !1278)
!1278 = distinct !DILexicalBlock(scope: !1275, file: !9, line: 4)
!1279 = !DILocation(line: 4, scope: !1276)
!1280 = distinct !{!1280, !1273, !1273, !1121}
!1281 = !DILocalVariable(name: "ret", scope: !1261, file: !9, line: 4, type: !81)
!1282 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethodV", linkageName: "\01_JNI_CallNonvirtualBooleanMethodV@20", scope: !9, file: !9, line: 4, type: !330, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1283 = !DILocalVariable(name: "args", arg: 5, scope: !1282, file: !9, line: 4, type: !149)
!1284 = !DILocation(line: 4, scope: !1282)
!1285 = !DILocalVariable(name: "methodID", arg: 4, scope: !1282, file: !9, line: 4, type: !67)
!1286 = !DILocalVariable(name: "clazz", arg: 3, scope: !1282, file: !9, line: 4, type: !47)
!1287 = !DILocalVariable(name: "obj", arg: 2, scope: !1282, file: !9, line: 4, type: !48)
!1288 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1282, file: !9, line: 4, type: !25)
!1289 = !DILocalVariable(name: "sig", scope: !1282, file: !9, line: 4, type: !1105)
!1290 = !DILocalVariable(name: "argc", scope: !1282, file: !9, line: 4, type: !13)
!1291 = !DILocalVariable(name: "argv", scope: !1282, file: !9, line: 4, type: !1110)
!1292 = !DILocalVariable(name: "i", scope: !1293, file: !9, line: 4, type: !13)
!1293 = distinct !DILexicalBlock(scope: !1282, file: !9, line: 4)
!1294 = !DILocation(line: 4, scope: !1293)
!1295 = !DILocation(line: 4, scope: !1296)
!1296 = distinct !DILexicalBlock(scope: !1297, file: !9, line: 4)
!1297 = distinct !DILexicalBlock(scope: !1293, file: !9, line: 4)
!1298 = !DILocation(line: 4, scope: !1299)
!1299 = distinct !DILexicalBlock(scope: !1296, file: !9, line: 4)
!1300 = !DILocation(line: 4, scope: !1297)
!1301 = distinct !{!1301, !1294, !1294, !1121}
!1302 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethod", scope: !9, file: !9, line: 4, type: !514, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1303 = !DILocalVariable(name: "methodID", arg: 3, scope: !1302, file: !9, line: 4, type: !67)
!1304 = !DILocation(line: 4, scope: !1302)
!1305 = !DILocalVariable(name: "clazz", arg: 2, scope: !1302, file: !9, line: 4, type: !47)
!1306 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1302, file: !9, line: 4, type: !25)
!1307 = !DILocalVariable(name: "args", scope: !1302, file: !9, line: 4, type: !149)
!1308 = !DILocalVariable(name: "sig", scope: !1302, file: !9, line: 4, type: !1105)
!1309 = !DILocalVariable(name: "argc", scope: !1302, file: !9, line: 4, type: !13)
!1310 = !DILocalVariable(name: "argv", scope: !1302, file: !9, line: 4, type: !1110)
!1311 = !DILocalVariable(name: "i", scope: !1312, file: !9, line: 4, type: !13)
!1312 = distinct !DILexicalBlock(scope: !1302, file: !9, line: 4)
!1313 = !DILocation(line: 4, scope: !1312)
!1314 = !DILocation(line: 4, scope: !1315)
!1315 = distinct !DILexicalBlock(scope: !1316, file: !9, line: 4)
!1316 = distinct !DILexicalBlock(scope: !1312, file: !9, line: 4)
!1317 = !DILocation(line: 4, scope: !1318)
!1318 = distinct !DILexicalBlock(scope: !1315, file: !9, line: 4)
!1319 = !DILocation(line: 4, scope: !1316)
!1320 = distinct !{!1320, !1313, !1313, !1121}
!1321 = !DILocalVariable(name: "ret", scope: !1302, file: !9, line: 4, type: !81)
!1322 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethodV", linkageName: "\01_JNI_CallStaticBooleanMethodV@16", scope: !9, file: !9, line: 4, type: !518, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1323 = !DILocalVariable(name: "args", arg: 4, scope: !1322, file: !9, line: 4, type: !149)
!1324 = !DILocation(line: 4, scope: !1322)
!1325 = !DILocalVariable(name: "methodID", arg: 3, scope: !1322, file: !9, line: 4, type: !67)
!1326 = !DILocalVariable(name: "clazz", arg: 2, scope: !1322, file: !9, line: 4, type: !47)
!1327 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1322, file: !9, line: 4, type: !25)
!1328 = !DILocalVariable(name: "sig", scope: !1322, file: !9, line: 4, type: !1105)
!1329 = !DILocalVariable(name: "argc", scope: !1322, file: !9, line: 4, type: !13)
!1330 = !DILocalVariable(name: "argv", scope: !1322, file: !9, line: 4, type: !1110)
!1331 = !DILocalVariable(name: "i", scope: !1332, file: !9, line: 4, type: !13)
!1332 = distinct !DILexicalBlock(scope: !1322, file: !9, line: 4)
!1333 = !DILocation(line: 4, scope: !1332)
!1334 = !DILocation(line: 4, scope: !1335)
!1335 = distinct !DILexicalBlock(scope: !1336, file: !9, line: 4)
!1336 = distinct !DILexicalBlock(scope: !1332, file: !9, line: 4)
!1337 = !DILocation(line: 4, scope: !1338)
!1338 = distinct !DILexicalBlock(scope: !1335, file: !9, line: 4)
!1339 = !DILocation(line: 4, scope: !1336)
!1340 = distinct !{!1340, !1333, !1333, !1121}
!1341 = distinct !DISubprogram(name: "JNI_CallByteMethod", scope: !9, file: !9, line: 5, type: !218, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1342 = !DILocalVariable(name: "methodID", arg: 3, scope: !1341, file: !9, line: 5, type: !67)
!1343 = !DILocation(line: 5, scope: !1341)
!1344 = !DILocalVariable(name: "obj", arg: 2, scope: !1341, file: !9, line: 5, type: !48)
!1345 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1341, file: !9, line: 5, type: !25)
!1346 = !DILocalVariable(name: "args", scope: !1341, file: !9, line: 5, type: !149)
!1347 = !DILocalVariable(name: "sig", scope: !1341, file: !9, line: 5, type: !1105)
!1348 = !DILocalVariable(name: "argc", scope: !1341, file: !9, line: 5, type: !13)
!1349 = !DILocalVariable(name: "argv", scope: !1341, file: !9, line: 5, type: !1110)
!1350 = !DILocalVariable(name: "i", scope: !1351, file: !9, line: 5, type: !13)
!1351 = distinct !DILexicalBlock(scope: !1341, file: !9, line: 5)
!1352 = !DILocation(line: 5, scope: !1351)
!1353 = !DILocation(line: 5, scope: !1354)
!1354 = distinct !DILexicalBlock(scope: !1355, file: !9, line: 5)
!1355 = distinct !DILexicalBlock(scope: !1351, file: !9, line: 5)
!1356 = !DILocation(line: 5, scope: !1357)
!1357 = distinct !DILexicalBlock(scope: !1354, file: !9, line: 5)
!1358 = !DILocation(line: 5, scope: !1355)
!1359 = distinct !{!1359, !1352, !1352, !1121}
!1360 = !DILocalVariable(name: "ret", scope: !1341, file: !9, line: 5, type: !56)
!1361 = distinct !DISubprogram(name: "JNI_CallByteMethodV", linkageName: "\01_JNI_CallByteMethodV@16", scope: !9, file: !9, line: 5, type: !222, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1362 = !DILocalVariable(name: "args", arg: 4, scope: !1361, file: !9, line: 5, type: !149)
!1363 = !DILocation(line: 5, scope: !1361)
!1364 = !DILocalVariable(name: "methodID", arg: 3, scope: !1361, file: !9, line: 5, type: !67)
!1365 = !DILocalVariable(name: "obj", arg: 2, scope: !1361, file: !9, line: 5, type: !48)
!1366 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1361, file: !9, line: 5, type: !25)
!1367 = !DILocalVariable(name: "sig", scope: !1361, file: !9, line: 5, type: !1105)
!1368 = !DILocalVariable(name: "argc", scope: !1361, file: !9, line: 5, type: !13)
!1369 = !DILocalVariable(name: "argv", scope: !1361, file: !9, line: 5, type: !1110)
!1370 = !DILocalVariable(name: "i", scope: !1371, file: !9, line: 5, type: !13)
!1371 = distinct !DILexicalBlock(scope: !1361, file: !9, line: 5)
!1372 = !DILocation(line: 5, scope: !1371)
!1373 = !DILocation(line: 5, scope: !1374)
!1374 = distinct !DILexicalBlock(scope: !1375, file: !9, line: 5)
!1375 = distinct !DILexicalBlock(scope: !1371, file: !9, line: 5)
!1376 = !DILocation(line: 5, scope: !1377)
!1377 = distinct !DILexicalBlock(scope: !1374, file: !9, line: 5)
!1378 = !DILocation(line: 5, scope: !1375)
!1379 = distinct !{!1379, !1372, !1372, !1121}
!1380 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethod", scope: !9, file: !9, line: 5, type: !338, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1381 = !DILocalVariable(name: "methodID", arg: 4, scope: !1380, file: !9, line: 5, type: !67)
!1382 = !DILocation(line: 5, scope: !1380)
!1383 = !DILocalVariable(name: "clazz", arg: 3, scope: !1380, file: !9, line: 5, type: !47)
!1384 = !DILocalVariable(name: "obj", arg: 2, scope: !1380, file: !9, line: 5, type: !48)
!1385 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1380, file: !9, line: 5, type: !25)
!1386 = !DILocalVariable(name: "args", scope: !1380, file: !9, line: 5, type: !149)
!1387 = !DILocalVariable(name: "sig", scope: !1380, file: !9, line: 5, type: !1105)
!1388 = !DILocalVariable(name: "argc", scope: !1380, file: !9, line: 5, type: !13)
!1389 = !DILocalVariable(name: "argv", scope: !1380, file: !9, line: 5, type: !1110)
!1390 = !DILocalVariable(name: "i", scope: !1391, file: !9, line: 5, type: !13)
!1391 = distinct !DILexicalBlock(scope: !1380, file: !9, line: 5)
!1392 = !DILocation(line: 5, scope: !1391)
!1393 = !DILocation(line: 5, scope: !1394)
!1394 = distinct !DILexicalBlock(scope: !1395, file: !9, line: 5)
!1395 = distinct !DILexicalBlock(scope: !1391, file: !9, line: 5)
!1396 = !DILocation(line: 5, scope: !1397)
!1397 = distinct !DILexicalBlock(scope: !1394, file: !9, line: 5)
!1398 = !DILocation(line: 5, scope: !1395)
!1399 = distinct !{!1399, !1392, !1392, !1121}
!1400 = !DILocalVariable(name: "ret", scope: !1380, file: !9, line: 5, type: !56)
!1401 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethodV", linkageName: "\01_JNI_CallNonvirtualByteMethodV@20", scope: !9, file: !9, line: 5, type: !342, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1402 = !DILocalVariable(name: "args", arg: 5, scope: !1401, file: !9, line: 5, type: !149)
!1403 = !DILocation(line: 5, scope: !1401)
!1404 = !DILocalVariable(name: "methodID", arg: 4, scope: !1401, file: !9, line: 5, type: !67)
!1405 = !DILocalVariable(name: "clazz", arg: 3, scope: !1401, file: !9, line: 5, type: !47)
!1406 = !DILocalVariable(name: "obj", arg: 2, scope: !1401, file: !9, line: 5, type: !48)
!1407 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1401, file: !9, line: 5, type: !25)
!1408 = !DILocalVariable(name: "sig", scope: !1401, file: !9, line: 5, type: !1105)
!1409 = !DILocalVariable(name: "argc", scope: !1401, file: !9, line: 5, type: !13)
!1410 = !DILocalVariable(name: "argv", scope: !1401, file: !9, line: 5, type: !1110)
!1411 = !DILocalVariable(name: "i", scope: !1412, file: !9, line: 5, type: !13)
!1412 = distinct !DILexicalBlock(scope: !1401, file: !9, line: 5)
!1413 = !DILocation(line: 5, scope: !1412)
!1414 = !DILocation(line: 5, scope: !1415)
!1415 = distinct !DILexicalBlock(scope: !1416, file: !9, line: 5)
!1416 = distinct !DILexicalBlock(scope: !1412, file: !9, line: 5)
!1417 = !DILocation(line: 5, scope: !1418)
!1418 = distinct !DILexicalBlock(scope: !1415, file: !9, line: 5)
!1419 = !DILocation(line: 5, scope: !1416)
!1420 = distinct !{!1420, !1413, !1413, !1121}
!1421 = distinct !DISubprogram(name: "JNI_CallStaticByteMethod", scope: !9, file: !9, line: 5, type: !526, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1422 = !DILocalVariable(name: "methodID", arg: 3, scope: !1421, file: !9, line: 5, type: !67)
!1423 = !DILocation(line: 5, scope: !1421)
!1424 = !DILocalVariable(name: "clazz", arg: 2, scope: !1421, file: !9, line: 5, type: !47)
!1425 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1421, file: !9, line: 5, type: !25)
!1426 = !DILocalVariable(name: "args", scope: !1421, file: !9, line: 5, type: !149)
!1427 = !DILocalVariable(name: "sig", scope: !1421, file: !9, line: 5, type: !1105)
!1428 = !DILocalVariable(name: "argc", scope: !1421, file: !9, line: 5, type: !13)
!1429 = !DILocalVariable(name: "argv", scope: !1421, file: !9, line: 5, type: !1110)
!1430 = !DILocalVariable(name: "i", scope: !1431, file: !9, line: 5, type: !13)
!1431 = distinct !DILexicalBlock(scope: !1421, file: !9, line: 5)
!1432 = !DILocation(line: 5, scope: !1431)
!1433 = !DILocation(line: 5, scope: !1434)
!1434 = distinct !DILexicalBlock(scope: !1435, file: !9, line: 5)
!1435 = distinct !DILexicalBlock(scope: !1431, file: !9, line: 5)
!1436 = !DILocation(line: 5, scope: !1437)
!1437 = distinct !DILexicalBlock(scope: !1434, file: !9, line: 5)
!1438 = !DILocation(line: 5, scope: !1435)
!1439 = distinct !{!1439, !1432, !1432, !1121}
!1440 = !DILocalVariable(name: "ret", scope: !1421, file: !9, line: 5, type: !56)
!1441 = distinct !DISubprogram(name: "JNI_CallStaticByteMethodV", linkageName: "\01_JNI_CallStaticByteMethodV@16", scope: !9, file: !9, line: 5, type: !530, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1442 = !DILocalVariable(name: "args", arg: 4, scope: !1441, file: !9, line: 5, type: !149)
!1443 = !DILocation(line: 5, scope: !1441)
!1444 = !DILocalVariable(name: "methodID", arg: 3, scope: !1441, file: !9, line: 5, type: !67)
!1445 = !DILocalVariable(name: "clazz", arg: 2, scope: !1441, file: !9, line: 5, type: !47)
!1446 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1441, file: !9, line: 5, type: !25)
!1447 = !DILocalVariable(name: "sig", scope: !1441, file: !9, line: 5, type: !1105)
!1448 = !DILocalVariable(name: "argc", scope: !1441, file: !9, line: 5, type: !13)
!1449 = !DILocalVariable(name: "argv", scope: !1441, file: !9, line: 5, type: !1110)
!1450 = !DILocalVariable(name: "i", scope: !1451, file: !9, line: 5, type: !13)
!1451 = distinct !DILexicalBlock(scope: !1441, file: !9, line: 5)
!1452 = !DILocation(line: 5, scope: !1451)
!1453 = !DILocation(line: 5, scope: !1454)
!1454 = distinct !DILexicalBlock(scope: !1455, file: !9, line: 5)
!1455 = distinct !DILexicalBlock(scope: !1451, file: !9, line: 5)
!1456 = !DILocation(line: 5, scope: !1457)
!1457 = distinct !DILexicalBlock(scope: !1454, file: !9, line: 5)
!1458 = !DILocation(line: 5, scope: !1455)
!1459 = distinct !{!1459, !1452, !1452, !1121}
!1460 = distinct !DISubprogram(name: "JNI_CallCharMethod", scope: !9, file: !9, line: 6, type: !230, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1461 = !DILocalVariable(name: "methodID", arg: 3, scope: !1460, file: !9, line: 6, type: !67)
!1462 = !DILocation(line: 6, scope: !1460)
!1463 = !DILocalVariable(name: "obj", arg: 2, scope: !1460, file: !9, line: 6, type: !48)
!1464 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1460, file: !9, line: 6, type: !25)
!1465 = !DILocalVariable(name: "args", scope: !1460, file: !9, line: 6, type: !149)
!1466 = !DILocalVariable(name: "sig", scope: !1460, file: !9, line: 6, type: !1105)
!1467 = !DILocalVariable(name: "argc", scope: !1460, file: !9, line: 6, type: !13)
!1468 = !DILocalVariable(name: "argv", scope: !1460, file: !9, line: 6, type: !1110)
!1469 = !DILocalVariable(name: "i", scope: !1470, file: !9, line: 6, type: !13)
!1470 = distinct !DILexicalBlock(scope: !1460, file: !9, line: 6)
!1471 = !DILocation(line: 6, scope: !1470)
!1472 = !DILocation(line: 6, scope: !1473)
!1473 = distinct !DILexicalBlock(scope: !1474, file: !9, line: 6)
!1474 = distinct !DILexicalBlock(scope: !1470, file: !9, line: 6)
!1475 = !DILocation(line: 6, scope: !1476)
!1476 = distinct !DILexicalBlock(scope: !1473, file: !9, line: 6)
!1477 = !DILocation(line: 6, scope: !1474)
!1478 = distinct !{!1478, !1471, !1471, !1121}
!1479 = !DILocalVariable(name: "ret", scope: !1460, file: !9, line: 6, type: !164)
!1480 = distinct !DISubprogram(name: "JNI_CallCharMethodV", linkageName: "\01_JNI_CallCharMethodV@16", scope: !9, file: !9, line: 6, type: !234, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1481 = !DILocalVariable(name: "args", arg: 4, scope: !1480, file: !9, line: 6, type: !149)
!1482 = !DILocation(line: 6, scope: !1480)
!1483 = !DILocalVariable(name: "methodID", arg: 3, scope: !1480, file: !9, line: 6, type: !67)
!1484 = !DILocalVariable(name: "obj", arg: 2, scope: !1480, file: !9, line: 6, type: !48)
!1485 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1480, file: !9, line: 6, type: !25)
!1486 = !DILocalVariable(name: "sig", scope: !1480, file: !9, line: 6, type: !1105)
!1487 = !DILocalVariable(name: "argc", scope: !1480, file: !9, line: 6, type: !13)
!1488 = !DILocalVariable(name: "argv", scope: !1480, file: !9, line: 6, type: !1110)
!1489 = !DILocalVariable(name: "i", scope: !1490, file: !9, line: 6, type: !13)
!1490 = distinct !DILexicalBlock(scope: !1480, file: !9, line: 6)
!1491 = !DILocation(line: 6, scope: !1490)
!1492 = !DILocation(line: 6, scope: !1493)
!1493 = distinct !DILexicalBlock(scope: !1494, file: !9, line: 6)
!1494 = distinct !DILexicalBlock(scope: !1490, file: !9, line: 6)
!1495 = !DILocation(line: 6, scope: !1496)
!1496 = distinct !DILexicalBlock(scope: !1493, file: !9, line: 6)
!1497 = !DILocation(line: 6, scope: !1494)
!1498 = distinct !{!1498, !1491, !1491, !1121}
!1499 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethod", scope: !9, file: !9, line: 6, type: !350, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1500 = !DILocalVariable(name: "methodID", arg: 4, scope: !1499, file: !9, line: 6, type: !67)
!1501 = !DILocation(line: 6, scope: !1499)
!1502 = !DILocalVariable(name: "clazz", arg: 3, scope: !1499, file: !9, line: 6, type: !47)
!1503 = !DILocalVariable(name: "obj", arg: 2, scope: !1499, file: !9, line: 6, type: !48)
!1504 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1499, file: !9, line: 6, type: !25)
!1505 = !DILocalVariable(name: "args", scope: !1499, file: !9, line: 6, type: !149)
!1506 = !DILocalVariable(name: "sig", scope: !1499, file: !9, line: 6, type: !1105)
!1507 = !DILocalVariable(name: "argc", scope: !1499, file: !9, line: 6, type: !13)
!1508 = !DILocalVariable(name: "argv", scope: !1499, file: !9, line: 6, type: !1110)
!1509 = !DILocalVariable(name: "i", scope: !1510, file: !9, line: 6, type: !13)
!1510 = distinct !DILexicalBlock(scope: !1499, file: !9, line: 6)
!1511 = !DILocation(line: 6, scope: !1510)
!1512 = !DILocation(line: 6, scope: !1513)
!1513 = distinct !DILexicalBlock(scope: !1514, file: !9, line: 6)
!1514 = distinct !DILexicalBlock(scope: !1510, file: !9, line: 6)
!1515 = !DILocation(line: 6, scope: !1516)
!1516 = distinct !DILexicalBlock(scope: !1513, file: !9, line: 6)
!1517 = !DILocation(line: 6, scope: !1514)
!1518 = distinct !{!1518, !1511, !1511, !1121}
!1519 = !DILocalVariable(name: "ret", scope: !1499, file: !9, line: 6, type: !164)
!1520 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethodV", linkageName: "\01_JNI_CallNonvirtualCharMethodV@20", scope: !9, file: !9, line: 6, type: !354, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1521 = !DILocalVariable(name: "args", arg: 5, scope: !1520, file: !9, line: 6, type: !149)
!1522 = !DILocation(line: 6, scope: !1520)
!1523 = !DILocalVariable(name: "methodID", arg: 4, scope: !1520, file: !9, line: 6, type: !67)
!1524 = !DILocalVariable(name: "clazz", arg: 3, scope: !1520, file: !9, line: 6, type: !47)
!1525 = !DILocalVariable(name: "obj", arg: 2, scope: !1520, file: !9, line: 6, type: !48)
!1526 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1520, file: !9, line: 6, type: !25)
!1527 = !DILocalVariable(name: "sig", scope: !1520, file: !9, line: 6, type: !1105)
!1528 = !DILocalVariable(name: "argc", scope: !1520, file: !9, line: 6, type: !13)
!1529 = !DILocalVariable(name: "argv", scope: !1520, file: !9, line: 6, type: !1110)
!1530 = !DILocalVariable(name: "i", scope: !1531, file: !9, line: 6, type: !13)
!1531 = distinct !DILexicalBlock(scope: !1520, file: !9, line: 6)
!1532 = !DILocation(line: 6, scope: !1531)
!1533 = !DILocation(line: 6, scope: !1534)
!1534 = distinct !DILexicalBlock(scope: !1535, file: !9, line: 6)
!1535 = distinct !DILexicalBlock(scope: !1531, file: !9, line: 6)
!1536 = !DILocation(line: 6, scope: !1537)
!1537 = distinct !DILexicalBlock(scope: !1534, file: !9, line: 6)
!1538 = !DILocation(line: 6, scope: !1535)
!1539 = distinct !{!1539, !1532, !1532, !1121}
!1540 = distinct !DISubprogram(name: "JNI_CallStaticCharMethod", scope: !9, file: !9, line: 6, type: !538, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1541 = !DILocalVariable(name: "methodID", arg: 3, scope: !1540, file: !9, line: 6, type: !67)
!1542 = !DILocation(line: 6, scope: !1540)
!1543 = !DILocalVariable(name: "clazz", arg: 2, scope: !1540, file: !9, line: 6, type: !47)
!1544 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1540, file: !9, line: 6, type: !25)
!1545 = !DILocalVariable(name: "args", scope: !1540, file: !9, line: 6, type: !149)
!1546 = !DILocalVariable(name: "sig", scope: !1540, file: !9, line: 6, type: !1105)
!1547 = !DILocalVariable(name: "argc", scope: !1540, file: !9, line: 6, type: !13)
!1548 = !DILocalVariable(name: "argv", scope: !1540, file: !9, line: 6, type: !1110)
!1549 = !DILocalVariable(name: "i", scope: !1550, file: !9, line: 6, type: !13)
!1550 = distinct !DILexicalBlock(scope: !1540, file: !9, line: 6)
!1551 = !DILocation(line: 6, scope: !1550)
!1552 = !DILocation(line: 6, scope: !1553)
!1553 = distinct !DILexicalBlock(scope: !1554, file: !9, line: 6)
!1554 = distinct !DILexicalBlock(scope: !1550, file: !9, line: 6)
!1555 = !DILocation(line: 6, scope: !1556)
!1556 = distinct !DILexicalBlock(scope: !1553, file: !9, line: 6)
!1557 = !DILocation(line: 6, scope: !1554)
!1558 = distinct !{!1558, !1551, !1551, !1121}
!1559 = !DILocalVariable(name: "ret", scope: !1540, file: !9, line: 6, type: !164)
!1560 = distinct !DISubprogram(name: "JNI_CallStaticCharMethodV", linkageName: "\01_JNI_CallStaticCharMethodV@16", scope: !9, file: !9, line: 6, type: !542, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1561 = !DILocalVariable(name: "args", arg: 4, scope: !1560, file: !9, line: 6, type: !149)
!1562 = !DILocation(line: 6, scope: !1560)
!1563 = !DILocalVariable(name: "methodID", arg: 3, scope: !1560, file: !9, line: 6, type: !67)
!1564 = !DILocalVariable(name: "clazz", arg: 2, scope: !1560, file: !9, line: 6, type: !47)
!1565 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1560, file: !9, line: 6, type: !25)
!1566 = !DILocalVariable(name: "sig", scope: !1560, file: !9, line: 6, type: !1105)
!1567 = !DILocalVariable(name: "argc", scope: !1560, file: !9, line: 6, type: !13)
!1568 = !DILocalVariable(name: "argv", scope: !1560, file: !9, line: 6, type: !1110)
!1569 = !DILocalVariable(name: "i", scope: !1570, file: !9, line: 6, type: !13)
!1570 = distinct !DILexicalBlock(scope: !1560, file: !9, line: 6)
!1571 = !DILocation(line: 6, scope: !1570)
!1572 = !DILocation(line: 6, scope: !1573)
!1573 = distinct !DILexicalBlock(scope: !1574, file: !9, line: 6)
!1574 = distinct !DILexicalBlock(scope: !1570, file: !9, line: 6)
!1575 = !DILocation(line: 6, scope: !1576)
!1576 = distinct !DILexicalBlock(scope: !1573, file: !9, line: 6)
!1577 = !DILocation(line: 6, scope: !1574)
!1578 = distinct !{!1578, !1571, !1571, !1121}
!1579 = distinct !DISubprogram(name: "JNI_CallShortMethod", scope: !9, file: !9, line: 7, type: !242, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1580 = !DILocalVariable(name: "methodID", arg: 3, scope: !1579, file: !9, line: 7, type: !67)
!1581 = !DILocation(line: 7, scope: !1579)
!1582 = !DILocalVariable(name: "obj", arg: 2, scope: !1579, file: !9, line: 7, type: !48)
!1583 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1579, file: !9, line: 7, type: !25)
!1584 = !DILocalVariable(name: "args", scope: !1579, file: !9, line: 7, type: !149)
!1585 = !DILocalVariable(name: "sig", scope: !1579, file: !9, line: 7, type: !1105)
!1586 = !DILocalVariable(name: "argc", scope: !1579, file: !9, line: 7, type: !13)
!1587 = !DILocalVariable(name: "argv", scope: !1579, file: !9, line: 7, type: !1110)
!1588 = !DILocalVariable(name: "i", scope: !1589, file: !9, line: 7, type: !13)
!1589 = distinct !DILexicalBlock(scope: !1579, file: !9, line: 7)
!1590 = !DILocation(line: 7, scope: !1589)
!1591 = !DILocation(line: 7, scope: !1592)
!1592 = distinct !DILexicalBlock(scope: !1593, file: !9, line: 7)
!1593 = distinct !DILexicalBlock(scope: !1589, file: !9, line: 7)
!1594 = !DILocation(line: 7, scope: !1595)
!1595 = distinct !DILexicalBlock(scope: !1592, file: !9, line: 7)
!1596 = !DILocation(line: 7, scope: !1593)
!1597 = distinct !{!1597, !1590, !1590, !1121}
!1598 = !DILocalVariable(name: "ret", scope: !1579, file: !9, line: 7, type: !167)
!1599 = distinct !DISubprogram(name: "JNI_CallShortMethodV", linkageName: "\01_JNI_CallShortMethodV@16", scope: !9, file: !9, line: 7, type: !246, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1600 = !DILocalVariable(name: "args", arg: 4, scope: !1599, file: !9, line: 7, type: !149)
!1601 = !DILocation(line: 7, scope: !1599)
!1602 = !DILocalVariable(name: "methodID", arg: 3, scope: !1599, file: !9, line: 7, type: !67)
!1603 = !DILocalVariable(name: "obj", arg: 2, scope: !1599, file: !9, line: 7, type: !48)
!1604 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1599, file: !9, line: 7, type: !25)
!1605 = !DILocalVariable(name: "sig", scope: !1599, file: !9, line: 7, type: !1105)
!1606 = !DILocalVariable(name: "argc", scope: !1599, file: !9, line: 7, type: !13)
!1607 = !DILocalVariable(name: "argv", scope: !1599, file: !9, line: 7, type: !1110)
!1608 = !DILocalVariable(name: "i", scope: !1609, file: !9, line: 7, type: !13)
!1609 = distinct !DILexicalBlock(scope: !1599, file: !9, line: 7)
!1610 = !DILocation(line: 7, scope: !1609)
!1611 = !DILocation(line: 7, scope: !1612)
!1612 = distinct !DILexicalBlock(scope: !1613, file: !9, line: 7)
!1613 = distinct !DILexicalBlock(scope: !1609, file: !9, line: 7)
!1614 = !DILocation(line: 7, scope: !1615)
!1615 = distinct !DILexicalBlock(scope: !1612, file: !9, line: 7)
!1616 = !DILocation(line: 7, scope: !1613)
!1617 = distinct !{!1617, !1610, !1610, !1121}
!1618 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethod", scope: !9, file: !9, line: 7, type: !362, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1619 = !DILocalVariable(name: "methodID", arg: 4, scope: !1618, file: !9, line: 7, type: !67)
!1620 = !DILocation(line: 7, scope: !1618)
!1621 = !DILocalVariable(name: "clazz", arg: 3, scope: !1618, file: !9, line: 7, type: !47)
!1622 = !DILocalVariable(name: "obj", arg: 2, scope: !1618, file: !9, line: 7, type: !48)
!1623 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1618, file: !9, line: 7, type: !25)
!1624 = !DILocalVariable(name: "args", scope: !1618, file: !9, line: 7, type: !149)
!1625 = !DILocalVariable(name: "sig", scope: !1618, file: !9, line: 7, type: !1105)
!1626 = !DILocalVariable(name: "argc", scope: !1618, file: !9, line: 7, type: !13)
!1627 = !DILocalVariable(name: "argv", scope: !1618, file: !9, line: 7, type: !1110)
!1628 = !DILocalVariable(name: "i", scope: !1629, file: !9, line: 7, type: !13)
!1629 = distinct !DILexicalBlock(scope: !1618, file: !9, line: 7)
!1630 = !DILocation(line: 7, scope: !1629)
!1631 = !DILocation(line: 7, scope: !1632)
!1632 = distinct !DILexicalBlock(scope: !1633, file: !9, line: 7)
!1633 = distinct !DILexicalBlock(scope: !1629, file: !9, line: 7)
!1634 = !DILocation(line: 7, scope: !1635)
!1635 = distinct !DILexicalBlock(scope: !1632, file: !9, line: 7)
!1636 = !DILocation(line: 7, scope: !1633)
!1637 = distinct !{!1637, !1630, !1630, !1121}
!1638 = !DILocalVariable(name: "ret", scope: !1618, file: !9, line: 7, type: !167)
!1639 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethodV", linkageName: "\01_JNI_CallNonvirtualShortMethodV@20", scope: !9, file: !9, line: 7, type: !366, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1640 = !DILocalVariable(name: "args", arg: 5, scope: !1639, file: !9, line: 7, type: !149)
!1641 = !DILocation(line: 7, scope: !1639)
!1642 = !DILocalVariable(name: "methodID", arg: 4, scope: !1639, file: !9, line: 7, type: !67)
!1643 = !DILocalVariable(name: "clazz", arg: 3, scope: !1639, file: !9, line: 7, type: !47)
!1644 = !DILocalVariable(name: "obj", arg: 2, scope: !1639, file: !9, line: 7, type: !48)
!1645 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1639, file: !9, line: 7, type: !25)
!1646 = !DILocalVariable(name: "sig", scope: !1639, file: !9, line: 7, type: !1105)
!1647 = !DILocalVariable(name: "argc", scope: !1639, file: !9, line: 7, type: !13)
!1648 = !DILocalVariable(name: "argv", scope: !1639, file: !9, line: 7, type: !1110)
!1649 = !DILocalVariable(name: "i", scope: !1650, file: !9, line: 7, type: !13)
!1650 = distinct !DILexicalBlock(scope: !1639, file: !9, line: 7)
!1651 = !DILocation(line: 7, scope: !1650)
!1652 = !DILocation(line: 7, scope: !1653)
!1653 = distinct !DILexicalBlock(scope: !1654, file: !9, line: 7)
!1654 = distinct !DILexicalBlock(scope: !1650, file: !9, line: 7)
!1655 = !DILocation(line: 7, scope: !1656)
!1656 = distinct !DILexicalBlock(scope: !1653, file: !9, line: 7)
!1657 = !DILocation(line: 7, scope: !1654)
!1658 = distinct !{!1658, !1651, !1651, !1121}
!1659 = distinct !DISubprogram(name: "JNI_CallStaticShortMethod", scope: !9, file: !9, line: 7, type: !550, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1660 = !DILocalVariable(name: "methodID", arg: 3, scope: !1659, file: !9, line: 7, type: !67)
!1661 = !DILocation(line: 7, scope: !1659)
!1662 = !DILocalVariable(name: "clazz", arg: 2, scope: !1659, file: !9, line: 7, type: !47)
!1663 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1659, file: !9, line: 7, type: !25)
!1664 = !DILocalVariable(name: "args", scope: !1659, file: !9, line: 7, type: !149)
!1665 = !DILocalVariable(name: "sig", scope: !1659, file: !9, line: 7, type: !1105)
!1666 = !DILocalVariable(name: "argc", scope: !1659, file: !9, line: 7, type: !13)
!1667 = !DILocalVariable(name: "argv", scope: !1659, file: !9, line: 7, type: !1110)
!1668 = !DILocalVariable(name: "i", scope: !1669, file: !9, line: 7, type: !13)
!1669 = distinct !DILexicalBlock(scope: !1659, file: !9, line: 7)
!1670 = !DILocation(line: 7, scope: !1669)
!1671 = !DILocation(line: 7, scope: !1672)
!1672 = distinct !DILexicalBlock(scope: !1673, file: !9, line: 7)
!1673 = distinct !DILexicalBlock(scope: !1669, file: !9, line: 7)
!1674 = !DILocation(line: 7, scope: !1675)
!1675 = distinct !DILexicalBlock(scope: !1672, file: !9, line: 7)
!1676 = !DILocation(line: 7, scope: !1673)
!1677 = distinct !{!1677, !1670, !1670, !1121}
!1678 = !DILocalVariable(name: "ret", scope: !1659, file: !9, line: 7, type: !167)
!1679 = distinct !DISubprogram(name: "JNI_CallStaticShortMethodV", linkageName: "\01_JNI_CallStaticShortMethodV@16", scope: !9, file: !9, line: 7, type: !554, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1680 = !DILocalVariable(name: "args", arg: 4, scope: !1679, file: !9, line: 7, type: !149)
!1681 = !DILocation(line: 7, scope: !1679)
!1682 = !DILocalVariable(name: "methodID", arg: 3, scope: !1679, file: !9, line: 7, type: !67)
!1683 = !DILocalVariable(name: "clazz", arg: 2, scope: !1679, file: !9, line: 7, type: !47)
!1684 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1679, file: !9, line: 7, type: !25)
!1685 = !DILocalVariable(name: "sig", scope: !1679, file: !9, line: 7, type: !1105)
!1686 = !DILocalVariable(name: "argc", scope: !1679, file: !9, line: 7, type: !13)
!1687 = !DILocalVariable(name: "argv", scope: !1679, file: !9, line: 7, type: !1110)
!1688 = !DILocalVariable(name: "i", scope: !1689, file: !9, line: 7, type: !13)
!1689 = distinct !DILexicalBlock(scope: !1679, file: !9, line: 7)
!1690 = !DILocation(line: 7, scope: !1689)
!1691 = !DILocation(line: 7, scope: !1692)
!1692 = distinct !DILexicalBlock(scope: !1693, file: !9, line: 7)
!1693 = distinct !DILexicalBlock(scope: !1689, file: !9, line: 7)
!1694 = !DILocation(line: 7, scope: !1695)
!1695 = distinct !DILexicalBlock(scope: !1692, file: !9, line: 7)
!1696 = !DILocation(line: 7, scope: !1693)
!1697 = distinct !{!1697, !1690, !1690, !1121}
!1698 = distinct !DISubprogram(name: "JNI_CallIntMethod", scope: !9, file: !9, line: 8, type: !254, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1699 = !DILocalVariable(name: "methodID", arg: 3, scope: !1698, file: !9, line: 8, type: !67)
!1700 = !DILocation(line: 8, scope: !1698)
!1701 = !DILocalVariable(name: "obj", arg: 2, scope: !1698, file: !9, line: 8, type: !48)
!1702 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1698, file: !9, line: 8, type: !25)
!1703 = !DILocalVariable(name: "args", scope: !1698, file: !9, line: 8, type: !149)
!1704 = !DILocalVariable(name: "sig", scope: !1698, file: !9, line: 8, type: !1105)
!1705 = !DILocalVariable(name: "argc", scope: !1698, file: !9, line: 8, type: !13)
!1706 = !DILocalVariable(name: "argv", scope: !1698, file: !9, line: 8, type: !1110)
!1707 = !DILocalVariable(name: "i", scope: !1708, file: !9, line: 8, type: !13)
!1708 = distinct !DILexicalBlock(scope: !1698, file: !9, line: 8)
!1709 = !DILocation(line: 8, scope: !1708)
!1710 = !DILocation(line: 8, scope: !1711)
!1711 = distinct !DILexicalBlock(scope: !1712, file: !9, line: 8)
!1712 = distinct !DILexicalBlock(scope: !1708, file: !9, line: 8)
!1713 = !DILocation(line: 8, scope: !1714)
!1714 = distinct !DILexicalBlock(scope: !1711, file: !9, line: 8)
!1715 = !DILocation(line: 8, scope: !1712)
!1716 = distinct !{!1716, !1709, !1709, !1121}
!1717 = !DILocalVariable(name: "ret", scope: !1698, file: !9, line: 8, type: !40)
!1718 = distinct !DISubprogram(name: "JNI_CallIntMethodV", linkageName: "\01_JNI_CallIntMethodV@16", scope: !9, file: !9, line: 8, type: !258, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1719 = !DILocalVariable(name: "args", arg: 4, scope: !1718, file: !9, line: 8, type: !149)
!1720 = !DILocation(line: 8, scope: !1718)
!1721 = !DILocalVariable(name: "methodID", arg: 3, scope: !1718, file: !9, line: 8, type: !67)
!1722 = !DILocalVariable(name: "obj", arg: 2, scope: !1718, file: !9, line: 8, type: !48)
!1723 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1718, file: !9, line: 8, type: !25)
!1724 = !DILocalVariable(name: "sig", scope: !1718, file: !9, line: 8, type: !1105)
!1725 = !DILocalVariable(name: "argc", scope: !1718, file: !9, line: 8, type: !13)
!1726 = !DILocalVariable(name: "argv", scope: !1718, file: !9, line: 8, type: !1110)
!1727 = !DILocalVariable(name: "i", scope: !1728, file: !9, line: 8, type: !13)
!1728 = distinct !DILexicalBlock(scope: !1718, file: !9, line: 8)
!1729 = !DILocation(line: 8, scope: !1728)
!1730 = !DILocation(line: 8, scope: !1731)
!1731 = distinct !DILexicalBlock(scope: !1732, file: !9, line: 8)
!1732 = distinct !DILexicalBlock(scope: !1728, file: !9, line: 8)
!1733 = !DILocation(line: 8, scope: !1734)
!1734 = distinct !DILexicalBlock(scope: !1731, file: !9, line: 8)
!1735 = !DILocation(line: 8, scope: !1732)
!1736 = distinct !{!1736, !1729, !1729, !1121}
!1737 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethod", scope: !9, file: !9, line: 8, type: !374, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1738 = !DILocalVariable(name: "methodID", arg: 4, scope: !1737, file: !9, line: 8, type: !67)
!1739 = !DILocation(line: 8, scope: !1737)
!1740 = !DILocalVariable(name: "clazz", arg: 3, scope: !1737, file: !9, line: 8, type: !47)
!1741 = !DILocalVariable(name: "obj", arg: 2, scope: !1737, file: !9, line: 8, type: !48)
!1742 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1737, file: !9, line: 8, type: !25)
!1743 = !DILocalVariable(name: "args", scope: !1737, file: !9, line: 8, type: !149)
!1744 = !DILocalVariable(name: "sig", scope: !1737, file: !9, line: 8, type: !1105)
!1745 = !DILocalVariable(name: "argc", scope: !1737, file: !9, line: 8, type: !13)
!1746 = !DILocalVariable(name: "argv", scope: !1737, file: !9, line: 8, type: !1110)
!1747 = !DILocalVariable(name: "i", scope: !1748, file: !9, line: 8, type: !13)
!1748 = distinct !DILexicalBlock(scope: !1737, file: !9, line: 8)
!1749 = !DILocation(line: 8, scope: !1748)
!1750 = !DILocation(line: 8, scope: !1751)
!1751 = distinct !DILexicalBlock(scope: !1752, file: !9, line: 8)
!1752 = distinct !DILexicalBlock(scope: !1748, file: !9, line: 8)
!1753 = !DILocation(line: 8, scope: !1754)
!1754 = distinct !DILexicalBlock(scope: !1751, file: !9, line: 8)
!1755 = !DILocation(line: 8, scope: !1752)
!1756 = distinct !{!1756, !1749, !1749, !1121}
!1757 = !DILocalVariable(name: "ret", scope: !1737, file: !9, line: 8, type: !40)
!1758 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethodV", linkageName: "\01_JNI_CallNonvirtualIntMethodV@20", scope: !9, file: !9, line: 8, type: !378, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1759 = !DILocalVariable(name: "args", arg: 5, scope: !1758, file: !9, line: 8, type: !149)
!1760 = !DILocation(line: 8, scope: !1758)
!1761 = !DILocalVariable(name: "methodID", arg: 4, scope: !1758, file: !9, line: 8, type: !67)
!1762 = !DILocalVariable(name: "clazz", arg: 3, scope: !1758, file: !9, line: 8, type: !47)
!1763 = !DILocalVariable(name: "obj", arg: 2, scope: !1758, file: !9, line: 8, type: !48)
!1764 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1758, file: !9, line: 8, type: !25)
!1765 = !DILocalVariable(name: "sig", scope: !1758, file: !9, line: 8, type: !1105)
!1766 = !DILocalVariable(name: "argc", scope: !1758, file: !9, line: 8, type: !13)
!1767 = !DILocalVariable(name: "argv", scope: !1758, file: !9, line: 8, type: !1110)
!1768 = !DILocalVariable(name: "i", scope: !1769, file: !9, line: 8, type: !13)
!1769 = distinct !DILexicalBlock(scope: !1758, file: !9, line: 8)
!1770 = !DILocation(line: 8, scope: !1769)
!1771 = !DILocation(line: 8, scope: !1772)
!1772 = distinct !DILexicalBlock(scope: !1773, file: !9, line: 8)
!1773 = distinct !DILexicalBlock(scope: !1769, file: !9, line: 8)
!1774 = !DILocation(line: 8, scope: !1775)
!1775 = distinct !DILexicalBlock(scope: !1772, file: !9, line: 8)
!1776 = !DILocation(line: 8, scope: !1773)
!1777 = distinct !{!1777, !1770, !1770, !1121}
!1778 = distinct !DISubprogram(name: "JNI_CallStaticIntMethod", scope: !9, file: !9, line: 8, type: !562, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1779 = !DILocalVariable(name: "methodID", arg: 3, scope: !1778, file: !9, line: 8, type: !67)
!1780 = !DILocation(line: 8, scope: !1778)
!1781 = !DILocalVariable(name: "clazz", arg: 2, scope: !1778, file: !9, line: 8, type: !47)
!1782 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1778, file: !9, line: 8, type: !25)
!1783 = !DILocalVariable(name: "args", scope: !1778, file: !9, line: 8, type: !149)
!1784 = !DILocalVariable(name: "sig", scope: !1778, file: !9, line: 8, type: !1105)
!1785 = !DILocalVariable(name: "argc", scope: !1778, file: !9, line: 8, type: !13)
!1786 = !DILocalVariable(name: "argv", scope: !1778, file: !9, line: 8, type: !1110)
!1787 = !DILocalVariable(name: "i", scope: !1788, file: !9, line: 8, type: !13)
!1788 = distinct !DILexicalBlock(scope: !1778, file: !9, line: 8)
!1789 = !DILocation(line: 8, scope: !1788)
!1790 = !DILocation(line: 8, scope: !1791)
!1791 = distinct !DILexicalBlock(scope: !1792, file: !9, line: 8)
!1792 = distinct !DILexicalBlock(scope: !1788, file: !9, line: 8)
!1793 = !DILocation(line: 8, scope: !1794)
!1794 = distinct !DILexicalBlock(scope: !1791, file: !9, line: 8)
!1795 = !DILocation(line: 8, scope: !1792)
!1796 = distinct !{!1796, !1789, !1789, !1121}
!1797 = !DILocalVariable(name: "ret", scope: !1778, file: !9, line: 8, type: !40)
!1798 = distinct !DISubprogram(name: "JNI_CallStaticIntMethodV", linkageName: "\01_JNI_CallStaticIntMethodV@16", scope: !9, file: !9, line: 8, type: !566, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1799 = !DILocalVariable(name: "args", arg: 4, scope: !1798, file: !9, line: 8, type: !149)
!1800 = !DILocation(line: 8, scope: !1798)
!1801 = !DILocalVariable(name: "methodID", arg: 3, scope: !1798, file: !9, line: 8, type: !67)
!1802 = !DILocalVariable(name: "clazz", arg: 2, scope: !1798, file: !9, line: 8, type: !47)
!1803 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1798, file: !9, line: 8, type: !25)
!1804 = !DILocalVariable(name: "sig", scope: !1798, file: !9, line: 8, type: !1105)
!1805 = !DILocalVariable(name: "argc", scope: !1798, file: !9, line: 8, type: !13)
!1806 = !DILocalVariable(name: "argv", scope: !1798, file: !9, line: 8, type: !1110)
!1807 = !DILocalVariable(name: "i", scope: !1808, file: !9, line: 8, type: !13)
!1808 = distinct !DILexicalBlock(scope: !1798, file: !9, line: 8)
!1809 = !DILocation(line: 8, scope: !1808)
!1810 = !DILocation(line: 8, scope: !1811)
!1811 = distinct !DILexicalBlock(scope: !1812, file: !9, line: 8)
!1812 = distinct !DILexicalBlock(scope: !1808, file: !9, line: 8)
!1813 = !DILocation(line: 8, scope: !1814)
!1814 = distinct !DILexicalBlock(scope: !1811, file: !9, line: 8)
!1815 = !DILocation(line: 8, scope: !1812)
!1816 = distinct !{!1816, !1809, !1809, !1121}
!1817 = distinct !DISubprogram(name: "JNI_CallLongMethod", scope: !9, file: !9, line: 9, type: !266, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1818 = !DILocalVariable(name: "methodID", arg: 3, scope: !1817, file: !9, line: 9, type: !67)
!1819 = !DILocation(line: 9, scope: !1817)
!1820 = !DILocalVariable(name: "obj", arg: 2, scope: !1817, file: !9, line: 9, type: !48)
!1821 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1817, file: !9, line: 9, type: !25)
!1822 = !DILocalVariable(name: "args", scope: !1817, file: !9, line: 9, type: !149)
!1823 = !DILocalVariable(name: "sig", scope: !1817, file: !9, line: 9, type: !1105)
!1824 = !DILocalVariable(name: "argc", scope: !1817, file: !9, line: 9, type: !13)
!1825 = !DILocalVariable(name: "argv", scope: !1817, file: !9, line: 9, type: !1110)
!1826 = !DILocalVariable(name: "i", scope: !1827, file: !9, line: 9, type: !13)
!1827 = distinct !DILexicalBlock(scope: !1817, file: !9, line: 9)
!1828 = !DILocation(line: 9, scope: !1827)
!1829 = !DILocation(line: 9, scope: !1830)
!1830 = distinct !DILexicalBlock(scope: !1831, file: !9, line: 9)
!1831 = distinct !DILexicalBlock(scope: !1827, file: !9, line: 9)
!1832 = !DILocation(line: 9, scope: !1833)
!1833 = distinct !DILexicalBlock(scope: !1830, file: !9, line: 9)
!1834 = !DILocation(line: 9, scope: !1831)
!1835 = distinct !{!1835, !1828, !1828, !1121}
!1836 = !DILocalVariable(name: "ret", scope: !1817, file: !9, line: 9, type: !171)
!1837 = distinct !DISubprogram(name: "JNI_CallLongMethodV", linkageName: "\01_JNI_CallLongMethodV@16", scope: !9, file: !9, line: 9, type: !270, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1838 = !DILocalVariable(name: "args", arg: 4, scope: !1837, file: !9, line: 9, type: !149)
!1839 = !DILocation(line: 9, scope: !1837)
!1840 = !DILocalVariable(name: "methodID", arg: 3, scope: !1837, file: !9, line: 9, type: !67)
!1841 = !DILocalVariable(name: "obj", arg: 2, scope: !1837, file: !9, line: 9, type: !48)
!1842 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1837, file: !9, line: 9, type: !25)
!1843 = !DILocalVariable(name: "sig", scope: !1837, file: !9, line: 9, type: !1105)
!1844 = !DILocalVariable(name: "argc", scope: !1837, file: !9, line: 9, type: !13)
!1845 = !DILocalVariable(name: "argv", scope: !1837, file: !9, line: 9, type: !1110)
!1846 = !DILocalVariable(name: "i", scope: !1847, file: !9, line: 9, type: !13)
!1847 = distinct !DILexicalBlock(scope: !1837, file: !9, line: 9)
!1848 = !DILocation(line: 9, scope: !1847)
!1849 = !DILocation(line: 9, scope: !1850)
!1850 = distinct !DILexicalBlock(scope: !1851, file: !9, line: 9)
!1851 = distinct !DILexicalBlock(scope: !1847, file: !9, line: 9)
!1852 = !DILocation(line: 9, scope: !1853)
!1853 = distinct !DILexicalBlock(scope: !1850, file: !9, line: 9)
!1854 = !DILocation(line: 9, scope: !1851)
!1855 = distinct !{!1855, !1848, !1848, !1121}
!1856 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethod", scope: !9, file: !9, line: 9, type: !386, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1857 = !DILocalVariable(name: "methodID", arg: 4, scope: !1856, file: !9, line: 9, type: !67)
!1858 = !DILocation(line: 9, scope: !1856)
!1859 = !DILocalVariable(name: "clazz", arg: 3, scope: !1856, file: !9, line: 9, type: !47)
!1860 = !DILocalVariable(name: "obj", arg: 2, scope: !1856, file: !9, line: 9, type: !48)
!1861 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1856, file: !9, line: 9, type: !25)
!1862 = !DILocalVariable(name: "args", scope: !1856, file: !9, line: 9, type: !149)
!1863 = !DILocalVariable(name: "sig", scope: !1856, file: !9, line: 9, type: !1105)
!1864 = !DILocalVariable(name: "argc", scope: !1856, file: !9, line: 9, type: !13)
!1865 = !DILocalVariable(name: "argv", scope: !1856, file: !9, line: 9, type: !1110)
!1866 = !DILocalVariable(name: "i", scope: !1867, file: !9, line: 9, type: !13)
!1867 = distinct !DILexicalBlock(scope: !1856, file: !9, line: 9)
!1868 = !DILocation(line: 9, scope: !1867)
!1869 = !DILocation(line: 9, scope: !1870)
!1870 = distinct !DILexicalBlock(scope: !1871, file: !9, line: 9)
!1871 = distinct !DILexicalBlock(scope: !1867, file: !9, line: 9)
!1872 = !DILocation(line: 9, scope: !1873)
!1873 = distinct !DILexicalBlock(scope: !1870, file: !9, line: 9)
!1874 = !DILocation(line: 9, scope: !1871)
!1875 = distinct !{!1875, !1868, !1868, !1121}
!1876 = !DILocalVariable(name: "ret", scope: !1856, file: !9, line: 9, type: !171)
!1877 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethodV", linkageName: "\01_JNI_CallNonvirtualLongMethodV@20", scope: !9, file: !9, line: 9, type: !390, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1878 = !DILocalVariable(name: "args", arg: 5, scope: !1877, file: !9, line: 9, type: !149)
!1879 = !DILocation(line: 9, scope: !1877)
!1880 = !DILocalVariable(name: "methodID", arg: 4, scope: !1877, file: !9, line: 9, type: !67)
!1881 = !DILocalVariable(name: "clazz", arg: 3, scope: !1877, file: !9, line: 9, type: !47)
!1882 = !DILocalVariable(name: "obj", arg: 2, scope: !1877, file: !9, line: 9, type: !48)
!1883 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1877, file: !9, line: 9, type: !25)
!1884 = !DILocalVariable(name: "sig", scope: !1877, file: !9, line: 9, type: !1105)
!1885 = !DILocalVariable(name: "argc", scope: !1877, file: !9, line: 9, type: !13)
!1886 = !DILocalVariable(name: "argv", scope: !1877, file: !9, line: 9, type: !1110)
!1887 = !DILocalVariable(name: "i", scope: !1888, file: !9, line: 9, type: !13)
!1888 = distinct !DILexicalBlock(scope: !1877, file: !9, line: 9)
!1889 = !DILocation(line: 9, scope: !1888)
!1890 = !DILocation(line: 9, scope: !1891)
!1891 = distinct !DILexicalBlock(scope: !1892, file: !9, line: 9)
!1892 = distinct !DILexicalBlock(scope: !1888, file: !9, line: 9)
!1893 = !DILocation(line: 9, scope: !1894)
!1894 = distinct !DILexicalBlock(scope: !1891, file: !9, line: 9)
!1895 = !DILocation(line: 9, scope: !1892)
!1896 = distinct !{!1896, !1889, !1889, !1121}
!1897 = distinct !DISubprogram(name: "JNI_CallStaticLongMethod", scope: !9, file: !9, line: 9, type: !574, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1898 = !DILocalVariable(name: "methodID", arg: 3, scope: !1897, file: !9, line: 9, type: !67)
!1899 = !DILocation(line: 9, scope: !1897)
!1900 = !DILocalVariable(name: "clazz", arg: 2, scope: !1897, file: !9, line: 9, type: !47)
!1901 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1897, file: !9, line: 9, type: !25)
!1902 = !DILocalVariable(name: "args", scope: !1897, file: !9, line: 9, type: !149)
!1903 = !DILocalVariable(name: "sig", scope: !1897, file: !9, line: 9, type: !1105)
!1904 = !DILocalVariable(name: "argc", scope: !1897, file: !9, line: 9, type: !13)
!1905 = !DILocalVariable(name: "argv", scope: !1897, file: !9, line: 9, type: !1110)
!1906 = !DILocalVariable(name: "i", scope: !1907, file: !9, line: 9, type: !13)
!1907 = distinct !DILexicalBlock(scope: !1897, file: !9, line: 9)
!1908 = !DILocation(line: 9, scope: !1907)
!1909 = !DILocation(line: 9, scope: !1910)
!1910 = distinct !DILexicalBlock(scope: !1911, file: !9, line: 9)
!1911 = distinct !DILexicalBlock(scope: !1907, file: !9, line: 9)
!1912 = !DILocation(line: 9, scope: !1913)
!1913 = distinct !DILexicalBlock(scope: !1910, file: !9, line: 9)
!1914 = !DILocation(line: 9, scope: !1911)
!1915 = distinct !{!1915, !1908, !1908, !1121}
!1916 = !DILocalVariable(name: "ret", scope: !1897, file: !9, line: 9, type: !171)
!1917 = distinct !DISubprogram(name: "JNI_CallStaticLongMethodV", linkageName: "\01_JNI_CallStaticLongMethodV@16", scope: !9, file: !9, line: 9, type: !578, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1918 = !DILocalVariable(name: "args", arg: 4, scope: !1917, file: !9, line: 9, type: !149)
!1919 = !DILocation(line: 9, scope: !1917)
!1920 = !DILocalVariable(name: "methodID", arg: 3, scope: !1917, file: !9, line: 9, type: !67)
!1921 = !DILocalVariable(name: "clazz", arg: 2, scope: !1917, file: !9, line: 9, type: !47)
!1922 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1917, file: !9, line: 9, type: !25)
!1923 = !DILocalVariable(name: "sig", scope: !1917, file: !9, line: 9, type: !1105)
!1924 = !DILocalVariable(name: "argc", scope: !1917, file: !9, line: 9, type: !13)
!1925 = !DILocalVariable(name: "argv", scope: !1917, file: !9, line: 9, type: !1110)
!1926 = !DILocalVariable(name: "i", scope: !1927, file: !9, line: 9, type: !13)
!1927 = distinct !DILexicalBlock(scope: !1917, file: !9, line: 9)
!1928 = !DILocation(line: 9, scope: !1927)
!1929 = !DILocation(line: 9, scope: !1930)
!1930 = distinct !DILexicalBlock(scope: !1931, file: !9, line: 9)
!1931 = distinct !DILexicalBlock(scope: !1927, file: !9, line: 9)
!1932 = !DILocation(line: 9, scope: !1933)
!1933 = distinct !DILexicalBlock(scope: !1930, file: !9, line: 9)
!1934 = !DILocation(line: 9, scope: !1931)
!1935 = distinct !{!1935, !1928, !1928, !1121}
!1936 = distinct !DISubprogram(name: "JNI_CallFloatMethod", scope: !9, file: !9, line: 10, type: !278, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1937 = !DILocalVariable(name: "methodID", arg: 3, scope: !1936, file: !9, line: 10, type: !67)
!1938 = !DILocation(line: 10, scope: !1936)
!1939 = !DILocalVariable(name: "obj", arg: 2, scope: !1936, file: !9, line: 10, type: !48)
!1940 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1936, file: !9, line: 10, type: !25)
!1941 = !DILocalVariable(name: "args", scope: !1936, file: !9, line: 10, type: !149)
!1942 = !DILocalVariable(name: "sig", scope: !1936, file: !9, line: 10, type: !1105)
!1943 = !DILocalVariable(name: "argc", scope: !1936, file: !9, line: 10, type: !13)
!1944 = !DILocalVariable(name: "argv", scope: !1936, file: !9, line: 10, type: !1110)
!1945 = !DILocalVariable(name: "i", scope: !1946, file: !9, line: 10, type: !13)
!1946 = distinct !DILexicalBlock(scope: !1936, file: !9, line: 10)
!1947 = !DILocation(line: 10, scope: !1946)
!1948 = !DILocation(line: 10, scope: !1949)
!1949 = distinct !DILexicalBlock(scope: !1950, file: !9, line: 10)
!1950 = distinct !DILexicalBlock(scope: !1946, file: !9, line: 10)
!1951 = !DILocation(line: 10, scope: !1952)
!1952 = distinct !DILexicalBlock(scope: !1949, file: !9, line: 10)
!1953 = !DILocation(line: 10, scope: !1950)
!1954 = distinct !{!1954, !1947, !1947, !1121}
!1955 = !DILocalVariable(name: "ret", scope: !1936, file: !9, line: 10, type: !174)
!1956 = distinct !DISubprogram(name: "JNI_CallFloatMethodV", linkageName: "\01_JNI_CallFloatMethodV@16", scope: !9, file: !9, line: 10, type: !282, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1957 = !DILocalVariable(name: "args", arg: 4, scope: !1956, file: !9, line: 10, type: !149)
!1958 = !DILocation(line: 10, scope: !1956)
!1959 = !DILocalVariable(name: "methodID", arg: 3, scope: !1956, file: !9, line: 10, type: !67)
!1960 = !DILocalVariable(name: "obj", arg: 2, scope: !1956, file: !9, line: 10, type: !48)
!1961 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1956, file: !9, line: 10, type: !25)
!1962 = !DILocalVariable(name: "sig", scope: !1956, file: !9, line: 10, type: !1105)
!1963 = !DILocalVariable(name: "argc", scope: !1956, file: !9, line: 10, type: !13)
!1964 = !DILocalVariable(name: "argv", scope: !1956, file: !9, line: 10, type: !1110)
!1965 = !DILocalVariable(name: "i", scope: !1966, file: !9, line: 10, type: !13)
!1966 = distinct !DILexicalBlock(scope: !1956, file: !9, line: 10)
!1967 = !DILocation(line: 10, scope: !1966)
!1968 = !DILocation(line: 10, scope: !1969)
!1969 = distinct !DILexicalBlock(scope: !1970, file: !9, line: 10)
!1970 = distinct !DILexicalBlock(scope: !1966, file: !9, line: 10)
!1971 = !DILocation(line: 10, scope: !1972)
!1972 = distinct !DILexicalBlock(scope: !1969, file: !9, line: 10)
!1973 = !DILocation(line: 10, scope: !1970)
!1974 = distinct !{!1974, !1967, !1967, !1121}
!1975 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethod", scope: !9, file: !9, line: 10, type: !398, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1976 = !DILocalVariable(name: "methodID", arg: 4, scope: !1975, file: !9, line: 10, type: !67)
!1977 = !DILocation(line: 10, scope: !1975)
!1978 = !DILocalVariable(name: "clazz", arg: 3, scope: !1975, file: !9, line: 10, type: !47)
!1979 = !DILocalVariable(name: "obj", arg: 2, scope: !1975, file: !9, line: 10, type: !48)
!1980 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1975, file: !9, line: 10, type: !25)
!1981 = !DILocalVariable(name: "args", scope: !1975, file: !9, line: 10, type: !149)
!1982 = !DILocalVariable(name: "sig", scope: !1975, file: !9, line: 10, type: !1105)
!1983 = !DILocalVariable(name: "argc", scope: !1975, file: !9, line: 10, type: !13)
!1984 = !DILocalVariable(name: "argv", scope: !1975, file: !9, line: 10, type: !1110)
!1985 = !DILocalVariable(name: "i", scope: !1986, file: !9, line: 10, type: !13)
!1986 = distinct !DILexicalBlock(scope: !1975, file: !9, line: 10)
!1987 = !DILocation(line: 10, scope: !1986)
!1988 = !DILocation(line: 10, scope: !1989)
!1989 = distinct !DILexicalBlock(scope: !1990, file: !9, line: 10)
!1990 = distinct !DILexicalBlock(scope: !1986, file: !9, line: 10)
!1991 = !DILocation(line: 10, scope: !1992)
!1992 = distinct !DILexicalBlock(scope: !1989, file: !9, line: 10)
!1993 = !DILocation(line: 10, scope: !1990)
!1994 = distinct !{!1994, !1987, !1987, !1121}
!1995 = !DILocalVariable(name: "ret", scope: !1975, file: !9, line: 10, type: !174)
!1996 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethodV", linkageName: "\01_JNI_CallNonvirtualFloatMethodV@20", scope: !9, file: !9, line: 10, type: !402, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!1997 = !DILocalVariable(name: "args", arg: 5, scope: !1996, file: !9, line: 10, type: !149)
!1998 = !DILocation(line: 10, scope: !1996)
!1999 = !DILocalVariable(name: "methodID", arg: 4, scope: !1996, file: !9, line: 10, type: !67)
!2000 = !DILocalVariable(name: "clazz", arg: 3, scope: !1996, file: !9, line: 10, type: !47)
!2001 = !DILocalVariable(name: "obj", arg: 2, scope: !1996, file: !9, line: 10, type: !48)
!2002 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1996, file: !9, line: 10, type: !25)
!2003 = !DILocalVariable(name: "sig", scope: !1996, file: !9, line: 10, type: !1105)
!2004 = !DILocalVariable(name: "argc", scope: !1996, file: !9, line: 10, type: !13)
!2005 = !DILocalVariable(name: "argv", scope: !1996, file: !9, line: 10, type: !1110)
!2006 = !DILocalVariable(name: "i", scope: !2007, file: !9, line: 10, type: !13)
!2007 = distinct !DILexicalBlock(scope: !1996, file: !9, line: 10)
!2008 = !DILocation(line: 10, scope: !2007)
!2009 = !DILocation(line: 10, scope: !2010)
!2010 = distinct !DILexicalBlock(scope: !2011, file: !9, line: 10)
!2011 = distinct !DILexicalBlock(scope: !2007, file: !9, line: 10)
!2012 = !DILocation(line: 10, scope: !2013)
!2013 = distinct !DILexicalBlock(scope: !2010, file: !9, line: 10)
!2014 = !DILocation(line: 10, scope: !2011)
!2015 = distinct !{!2015, !2008, !2008, !1121}
!2016 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethod", scope: !9, file: !9, line: 10, type: !586, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2017 = !DILocalVariable(name: "methodID", arg: 3, scope: !2016, file: !9, line: 10, type: !67)
!2018 = !DILocation(line: 10, scope: !2016)
!2019 = !DILocalVariable(name: "clazz", arg: 2, scope: !2016, file: !9, line: 10, type: !47)
!2020 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2016, file: !9, line: 10, type: !25)
!2021 = !DILocalVariable(name: "args", scope: !2016, file: !9, line: 10, type: !149)
!2022 = !DILocalVariable(name: "sig", scope: !2016, file: !9, line: 10, type: !1105)
!2023 = !DILocalVariable(name: "argc", scope: !2016, file: !9, line: 10, type: !13)
!2024 = !DILocalVariable(name: "argv", scope: !2016, file: !9, line: 10, type: !1110)
!2025 = !DILocalVariable(name: "i", scope: !2026, file: !9, line: 10, type: !13)
!2026 = distinct !DILexicalBlock(scope: !2016, file: !9, line: 10)
!2027 = !DILocation(line: 10, scope: !2026)
!2028 = !DILocation(line: 10, scope: !2029)
!2029 = distinct !DILexicalBlock(scope: !2030, file: !9, line: 10)
!2030 = distinct !DILexicalBlock(scope: !2026, file: !9, line: 10)
!2031 = !DILocation(line: 10, scope: !2032)
!2032 = distinct !DILexicalBlock(scope: !2029, file: !9, line: 10)
!2033 = !DILocation(line: 10, scope: !2030)
!2034 = distinct !{!2034, !2027, !2027, !1121}
!2035 = !DILocalVariable(name: "ret", scope: !2016, file: !9, line: 10, type: !174)
!2036 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethodV", linkageName: "\01_JNI_CallStaticFloatMethodV@16", scope: !9, file: !9, line: 10, type: !590, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2037 = !DILocalVariable(name: "args", arg: 4, scope: !2036, file: !9, line: 10, type: !149)
!2038 = !DILocation(line: 10, scope: !2036)
!2039 = !DILocalVariable(name: "methodID", arg: 3, scope: !2036, file: !9, line: 10, type: !67)
!2040 = !DILocalVariable(name: "clazz", arg: 2, scope: !2036, file: !9, line: 10, type: !47)
!2041 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2036, file: !9, line: 10, type: !25)
!2042 = !DILocalVariable(name: "sig", scope: !2036, file: !9, line: 10, type: !1105)
!2043 = !DILocalVariable(name: "argc", scope: !2036, file: !9, line: 10, type: !13)
!2044 = !DILocalVariable(name: "argv", scope: !2036, file: !9, line: 10, type: !1110)
!2045 = !DILocalVariable(name: "i", scope: !2046, file: !9, line: 10, type: !13)
!2046 = distinct !DILexicalBlock(scope: !2036, file: !9, line: 10)
!2047 = !DILocation(line: 10, scope: !2046)
!2048 = !DILocation(line: 10, scope: !2049)
!2049 = distinct !DILexicalBlock(scope: !2050, file: !9, line: 10)
!2050 = distinct !DILexicalBlock(scope: !2046, file: !9, line: 10)
!2051 = !DILocation(line: 10, scope: !2052)
!2052 = distinct !DILexicalBlock(scope: !2049, file: !9, line: 10)
!2053 = !DILocation(line: 10, scope: !2050)
!2054 = distinct !{!2054, !2047, !2047, !1121}
!2055 = distinct !DISubprogram(name: "JNI_CallDoubleMethod", scope: !9, file: !9, line: 11, type: !290, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2056 = !DILocalVariable(name: "methodID", arg: 3, scope: !2055, file: !9, line: 11, type: !67)
!2057 = !DILocation(line: 11, scope: !2055)
!2058 = !DILocalVariable(name: "obj", arg: 2, scope: !2055, file: !9, line: 11, type: !48)
!2059 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2055, file: !9, line: 11, type: !25)
!2060 = !DILocalVariable(name: "args", scope: !2055, file: !9, line: 11, type: !149)
!2061 = !DILocalVariable(name: "sig", scope: !2055, file: !9, line: 11, type: !1105)
!2062 = !DILocalVariable(name: "argc", scope: !2055, file: !9, line: 11, type: !13)
!2063 = !DILocalVariable(name: "argv", scope: !2055, file: !9, line: 11, type: !1110)
!2064 = !DILocalVariable(name: "i", scope: !2065, file: !9, line: 11, type: !13)
!2065 = distinct !DILexicalBlock(scope: !2055, file: !9, line: 11)
!2066 = !DILocation(line: 11, scope: !2065)
!2067 = !DILocation(line: 11, scope: !2068)
!2068 = distinct !DILexicalBlock(scope: !2069, file: !9, line: 11)
!2069 = distinct !DILexicalBlock(scope: !2065, file: !9, line: 11)
!2070 = !DILocation(line: 11, scope: !2071)
!2071 = distinct !DILexicalBlock(scope: !2068, file: !9, line: 11)
!2072 = !DILocation(line: 11, scope: !2069)
!2073 = distinct !{!2073, !2066, !2066, !1121}
!2074 = !DILocalVariable(name: "ret", scope: !2055, file: !9, line: 11, type: !177)
!2075 = distinct !DISubprogram(name: "JNI_CallDoubleMethodV", linkageName: "\01_JNI_CallDoubleMethodV@16", scope: !9, file: !9, line: 11, type: !294, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2076 = !DILocalVariable(name: "args", arg: 4, scope: !2075, file: !9, line: 11, type: !149)
!2077 = !DILocation(line: 11, scope: !2075)
!2078 = !DILocalVariable(name: "methodID", arg: 3, scope: !2075, file: !9, line: 11, type: !67)
!2079 = !DILocalVariable(name: "obj", arg: 2, scope: !2075, file: !9, line: 11, type: !48)
!2080 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2075, file: !9, line: 11, type: !25)
!2081 = !DILocalVariable(name: "sig", scope: !2075, file: !9, line: 11, type: !1105)
!2082 = !DILocalVariable(name: "argc", scope: !2075, file: !9, line: 11, type: !13)
!2083 = !DILocalVariable(name: "argv", scope: !2075, file: !9, line: 11, type: !1110)
!2084 = !DILocalVariable(name: "i", scope: !2085, file: !9, line: 11, type: !13)
!2085 = distinct !DILexicalBlock(scope: !2075, file: !9, line: 11)
!2086 = !DILocation(line: 11, scope: !2085)
!2087 = !DILocation(line: 11, scope: !2088)
!2088 = distinct !DILexicalBlock(scope: !2089, file: !9, line: 11)
!2089 = distinct !DILexicalBlock(scope: !2085, file: !9, line: 11)
!2090 = !DILocation(line: 11, scope: !2091)
!2091 = distinct !DILexicalBlock(scope: !2088, file: !9, line: 11)
!2092 = !DILocation(line: 11, scope: !2089)
!2093 = distinct !{!2093, !2086, !2086, !1121}
!2094 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethod", scope: !9, file: !9, line: 11, type: !410, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2095 = !DILocalVariable(name: "methodID", arg: 4, scope: !2094, file: !9, line: 11, type: !67)
!2096 = !DILocation(line: 11, scope: !2094)
!2097 = !DILocalVariable(name: "clazz", arg: 3, scope: !2094, file: !9, line: 11, type: !47)
!2098 = !DILocalVariable(name: "obj", arg: 2, scope: !2094, file: !9, line: 11, type: !48)
!2099 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2094, file: !9, line: 11, type: !25)
!2100 = !DILocalVariable(name: "args", scope: !2094, file: !9, line: 11, type: !149)
!2101 = !DILocalVariable(name: "sig", scope: !2094, file: !9, line: 11, type: !1105)
!2102 = !DILocalVariable(name: "argc", scope: !2094, file: !9, line: 11, type: !13)
!2103 = !DILocalVariable(name: "argv", scope: !2094, file: !9, line: 11, type: !1110)
!2104 = !DILocalVariable(name: "i", scope: !2105, file: !9, line: 11, type: !13)
!2105 = distinct !DILexicalBlock(scope: !2094, file: !9, line: 11)
!2106 = !DILocation(line: 11, scope: !2105)
!2107 = !DILocation(line: 11, scope: !2108)
!2108 = distinct !DILexicalBlock(scope: !2109, file: !9, line: 11)
!2109 = distinct !DILexicalBlock(scope: !2105, file: !9, line: 11)
!2110 = !DILocation(line: 11, scope: !2111)
!2111 = distinct !DILexicalBlock(scope: !2108, file: !9, line: 11)
!2112 = !DILocation(line: 11, scope: !2109)
!2113 = distinct !{!2113, !2106, !2106, !1121}
!2114 = !DILocalVariable(name: "ret", scope: !2094, file: !9, line: 11, type: !177)
!2115 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethodV", linkageName: "\01_JNI_CallNonvirtualDoubleMethodV@20", scope: !9, file: !9, line: 11, type: !414, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2116 = !DILocalVariable(name: "args", arg: 5, scope: !2115, file: !9, line: 11, type: !149)
!2117 = !DILocation(line: 11, scope: !2115)
!2118 = !DILocalVariable(name: "methodID", arg: 4, scope: !2115, file: !9, line: 11, type: !67)
!2119 = !DILocalVariable(name: "clazz", arg: 3, scope: !2115, file: !9, line: 11, type: !47)
!2120 = !DILocalVariable(name: "obj", arg: 2, scope: !2115, file: !9, line: 11, type: !48)
!2121 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2115, file: !9, line: 11, type: !25)
!2122 = !DILocalVariable(name: "sig", scope: !2115, file: !9, line: 11, type: !1105)
!2123 = !DILocalVariable(name: "argc", scope: !2115, file: !9, line: 11, type: !13)
!2124 = !DILocalVariable(name: "argv", scope: !2115, file: !9, line: 11, type: !1110)
!2125 = !DILocalVariable(name: "i", scope: !2126, file: !9, line: 11, type: !13)
!2126 = distinct !DILexicalBlock(scope: !2115, file: !9, line: 11)
!2127 = !DILocation(line: 11, scope: !2126)
!2128 = !DILocation(line: 11, scope: !2129)
!2129 = distinct !DILexicalBlock(scope: !2130, file: !9, line: 11)
!2130 = distinct !DILexicalBlock(scope: !2126, file: !9, line: 11)
!2131 = !DILocation(line: 11, scope: !2132)
!2132 = distinct !DILexicalBlock(scope: !2129, file: !9, line: 11)
!2133 = !DILocation(line: 11, scope: !2130)
!2134 = distinct !{!2134, !2127, !2127, !1121}
!2135 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethod", scope: !9, file: !9, line: 11, type: !598, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2136 = !DILocalVariable(name: "methodID", arg: 3, scope: !2135, file: !9, line: 11, type: !67)
!2137 = !DILocation(line: 11, scope: !2135)
!2138 = !DILocalVariable(name: "clazz", arg: 2, scope: !2135, file: !9, line: 11, type: !47)
!2139 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2135, file: !9, line: 11, type: !25)
!2140 = !DILocalVariable(name: "args", scope: !2135, file: !9, line: 11, type: !149)
!2141 = !DILocalVariable(name: "sig", scope: !2135, file: !9, line: 11, type: !1105)
!2142 = !DILocalVariable(name: "argc", scope: !2135, file: !9, line: 11, type: !13)
!2143 = !DILocalVariable(name: "argv", scope: !2135, file: !9, line: 11, type: !1110)
!2144 = !DILocalVariable(name: "i", scope: !2145, file: !9, line: 11, type: !13)
!2145 = distinct !DILexicalBlock(scope: !2135, file: !9, line: 11)
!2146 = !DILocation(line: 11, scope: !2145)
!2147 = !DILocation(line: 11, scope: !2148)
!2148 = distinct !DILexicalBlock(scope: !2149, file: !9, line: 11)
!2149 = distinct !DILexicalBlock(scope: !2145, file: !9, line: 11)
!2150 = !DILocation(line: 11, scope: !2151)
!2151 = distinct !DILexicalBlock(scope: !2148, file: !9, line: 11)
!2152 = !DILocation(line: 11, scope: !2149)
!2153 = distinct !{!2153, !2146, !2146, !1121}
!2154 = !DILocalVariable(name: "ret", scope: !2135, file: !9, line: 11, type: !177)
!2155 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethodV", linkageName: "\01_JNI_CallStaticDoubleMethodV@16", scope: !9, file: !9, line: 11, type: !602, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2156 = !DILocalVariable(name: "args", arg: 4, scope: !2155, file: !9, line: 11, type: !149)
!2157 = !DILocation(line: 11, scope: !2155)
!2158 = !DILocalVariable(name: "methodID", arg: 3, scope: !2155, file: !9, line: 11, type: !67)
!2159 = !DILocalVariable(name: "clazz", arg: 2, scope: !2155, file: !9, line: 11, type: !47)
!2160 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2155, file: !9, line: 11, type: !25)
!2161 = !DILocalVariable(name: "sig", scope: !2155, file: !9, line: 11, type: !1105)
!2162 = !DILocalVariable(name: "argc", scope: !2155, file: !9, line: 11, type: !13)
!2163 = !DILocalVariable(name: "argv", scope: !2155, file: !9, line: 11, type: !1110)
!2164 = !DILocalVariable(name: "i", scope: !2165, file: !9, line: 11, type: !13)
!2165 = distinct !DILexicalBlock(scope: !2155, file: !9, line: 11)
!2166 = !DILocation(line: 11, scope: !2165)
!2167 = !DILocation(line: 11, scope: !2168)
!2168 = distinct !DILexicalBlock(scope: !2169, file: !9, line: 11)
!2169 = distinct !DILexicalBlock(scope: !2165, file: !9, line: 11)
!2170 = !DILocation(line: 11, scope: !2171)
!2171 = distinct !DILexicalBlock(scope: !2168, file: !9, line: 11)
!2172 = !DILocation(line: 11, scope: !2169)
!2173 = distinct !{!2173, !2166, !2166, !1121}
!2174 = distinct !DISubprogram(name: "JNI_NewObject", scope: !9, file: !9, line: 13, type: !143, scopeLine: 14, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2175 = !DILocalVariable(name: "methodID", arg: 3, scope: !2174, file: !9, line: 13, type: !67)
!2176 = !DILocation(line: 13, scope: !2174)
!2177 = !DILocalVariable(name: "clazz", arg: 2, scope: !2174, file: !9, line: 13, type: !47)
!2178 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2174, file: !9, line: 13, type: !25)
!2179 = !DILocalVariable(name: "args", scope: !2174, file: !9, line: 15, type: !149)
!2180 = !DILocation(line: 15, scope: !2174)
!2181 = !DILocation(line: 16, scope: !2174)
!2182 = !DILocalVariable(name: "sig", scope: !2174, file: !9, line: 17, type: !1105)
!2183 = !DILocation(line: 17, scope: !2174)
!2184 = !DILocalVariable(name: "argc", scope: !2174, file: !9, line: 17, type: !13)
!2185 = !DILocalVariable(name: "argv", scope: !2174, file: !9, line: 17, type: !1110)
!2186 = !DILocalVariable(name: "i", scope: !2187, file: !9, line: 17, type: !13)
!2187 = distinct !DILexicalBlock(scope: !2174, file: !9, line: 17)
!2188 = !DILocation(line: 17, scope: !2187)
!2189 = !DILocation(line: 17, scope: !2190)
!2190 = distinct !DILexicalBlock(scope: !2191, file: !9, line: 17)
!2191 = distinct !DILexicalBlock(scope: !2187, file: !9, line: 17)
!2192 = !DILocation(line: 17, scope: !2193)
!2193 = distinct !DILexicalBlock(scope: !2190, file: !9, line: 17)
!2194 = !DILocation(line: 17, scope: !2191)
!2195 = distinct !{!2195, !2188, !2188, !1121}
!2196 = !DILocalVariable(name: "o", scope: !2174, file: !9, line: 18, type: !48)
!2197 = !DILocation(line: 18, scope: !2174)
!2198 = !DILocation(line: 19, scope: !2174)
!2199 = !DILocation(line: 20, scope: !2174)
!2200 = distinct !DISubprogram(name: "JNI_NewObjectV", linkageName: "\01_JNI_NewObjectV@16", scope: !9, file: !9, line: 23, type: !147, scopeLine: 24, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2201 = !DILocalVariable(name: "args", arg: 4, scope: !2200, file: !9, line: 23, type: !149)
!2202 = !DILocation(line: 23, scope: !2200)
!2203 = !DILocalVariable(name: "methodID", arg: 3, scope: !2200, file: !9, line: 23, type: !67)
!2204 = !DILocalVariable(name: "clazz", arg: 2, scope: !2200, file: !9, line: 23, type: !47)
!2205 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2200, file: !9, line: 23, type: !25)
!2206 = !DILocalVariable(name: "sig", scope: !2200, file: !9, line: 25, type: !1105)
!2207 = !DILocation(line: 25, scope: !2200)
!2208 = !DILocalVariable(name: "argc", scope: !2200, file: !9, line: 25, type: !13)
!2209 = !DILocalVariable(name: "argv", scope: !2200, file: !9, line: 25, type: !1110)
!2210 = !DILocalVariable(name: "i", scope: !2211, file: !9, line: 25, type: !13)
!2211 = distinct !DILexicalBlock(scope: !2200, file: !9, line: 25)
!2212 = !DILocation(line: 25, scope: !2211)
!2213 = !DILocation(line: 25, scope: !2214)
!2214 = distinct !DILexicalBlock(scope: !2215, file: !9, line: 25)
!2215 = distinct !DILexicalBlock(scope: !2211, file: !9, line: 25)
!2216 = !DILocation(line: 25, scope: !2217)
!2217 = distinct !DILexicalBlock(scope: !2214, file: !9, line: 25)
!2218 = !DILocation(line: 25, scope: !2215)
!2219 = distinct !{!2219, !2212, !2212, !1121}
!2220 = !DILocation(line: 26, scope: !2200)
!2221 = distinct !DISubprogram(name: "JNI_CallVoidMethod", scope: !9, file: !9, line: 29, type: !302, scopeLine: 30, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2222 = !DILocalVariable(name: "methodID", arg: 3, scope: !2221, file: !9, line: 29, type: !67)
!2223 = !DILocation(line: 29, scope: !2221)
!2224 = !DILocalVariable(name: "obj", arg: 2, scope: !2221, file: !9, line: 29, type: !48)
!2225 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2221, file: !9, line: 29, type: !25)
!2226 = !DILocalVariable(name: "args", scope: !2221, file: !9, line: 31, type: !149)
!2227 = !DILocation(line: 31, scope: !2221)
!2228 = !DILocation(line: 32, scope: !2221)
!2229 = !DILocalVariable(name: "sig", scope: !2221, file: !9, line: 33, type: !1105)
!2230 = !DILocation(line: 33, scope: !2221)
!2231 = !DILocalVariable(name: "argc", scope: !2221, file: !9, line: 33, type: !13)
!2232 = !DILocalVariable(name: "argv", scope: !2221, file: !9, line: 33, type: !1110)
!2233 = !DILocalVariable(name: "i", scope: !2234, file: !9, line: 33, type: !13)
!2234 = distinct !DILexicalBlock(scope: !2221, file: !9, line: 33)
!2235 = !DILocation(line: 33, scope: !2234)
!2236 = !DILocation(line: 33, scope: !2237)
!2237 = distinct !DILexicalBlock(scope: !2238, file: !9, line: 33)
!2238 = distinct !DILexicalBlock(scope: !2234, file: !9, line: 33)
!2239 = !DILocation(line: 33, scope: !2240)
!2240 = distinct !DILexicalBlock(scope: !2237, file: !9, line: 33)
!2241 = !DILocation(line: 33, scope: !2238)
!2242 = distinct !{!2242, !2235, !2235, !1121}
!2243 = !DILocation(line: 34, scope: !2221)
!2244 = !DILocation(line: 35, scope: !2221)
!2245 = !DILocation(line: 36, scope: !2221)
!2246 = distinct !DISubprogram(name: "JNI_CallVoidMethodV", linkageName: "\01_JNI_CallVoidMethodV@16", scope: !9, file: !9, line: 38, type: !306, scopeLine: 39, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2247 = !DILocalVariable(name: "args", arg: 4, scope: !2246, file: !9, line: 38, type: !149)
!2248 = !DILocation(line: 38, scope: !2246)
!2249 = !DILocalVariable(name: "methodID", arg: 3, scope: !2246, file: !9, line: 38, type: !67)
!2250 = !DILocalVariable(name: "obj", arg: 2, scope: !2246, file: !9, line: 38, type: !48)
!2251 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2246, file: !9, line: 38, type: !25)
!2252 = !DILocalVariable(name: "sig", scope: !2246, file: !9, line: 40, type: !1105)
!2253 = !DILocation(line: 40, scope: !2246)
!2254 = !DILocalVariable(name: "argc", scope: !2246, file: !9, line: 40, type: !13)
!2255 = !DILocalVariable(name: "argv", scope: !2246, file: !9, line: 40, type: !1110)
!2256 = !DILocalVariable(name: "i", scope: !2257, file: !9, line: 40, type: !13)
!2257 = distinct !DILexicalBlock(scope: !2246, file: !9, line: 40)
!2258 = !DILocation(line: 40, scope: !2257)
!2259 = !DILocation(line: 40, scope: !2260)
!2260 = distinct !DILexicalBlock(scope: !2261, file: !9, line: 40)
!2261 = distinct !DILexicalBlock(scope: !2257, file: !9, line: 40)
!2262 = !DILocation(line: 40, scope: !2263)
!2263 = distinct !DILexicalBlock(scope: !2260, file: !9, line: 40)
!2264 = !DILocation(line: 40, scope: !2261)
!2265 = distinct !{!2265, !2258, !2258, !1121}
!2266 = !DILocation(line: 41, scope: !2246)
!2267 = !DILocation(line: 42, scope: !2246)
!2268 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethod", scope: !9, file: !9, line: 44, type: !422, scopeLine: 45, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2269 = !DILocalVariable(name: "methodID", arg: 4, scope: !2268, file: !9, line: 44, type: !67)
!2270 = !DILocation(line: 44, scope: !2268)
!2271 = !DILocalVariable(name: "clazz", arg: 3, scope: !2268, file: !9, line: 44, type: !47)
!2272 = !DILocalVariable(name: "obj", arg: 2, scope: !2268, file: !9, line: 44, type: !48)
!2273 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2268, file: !9, line: 44, type: !25)
!2274 = !DILocalVariable(name: "args", scope: !2268, file: !9, line: 46, type: !149)
!2275 = !DILocation(line: 46, scope: !2268)
!2276 = !DILocation(line: 47, scope: !2268)
!2277 = !DILocalVariable(name: "sig", scope: !2268, file: !9, line: 48, type: !1105)
!2278 = !DILocation(line: 48, scope: !2268)
!2279 = !DILocalVariable(name: "argc", scope: !2268, file: !9, line: 48, type: !13)
!2280 = !DILocalVariable(name: "argv", scope: !2268, file: !9, line: 48, type: !1110)
!2281 = !DILocalVariable(name: "i", scope: !2282, file: !9, line: 48, type: !13)
!2282 = distinct !DILexicalBlock(scope: !2268, file: !9, line: 48)
!2283 = !DILocation(line: 48, scope: !2282)
!2284 = !DILocation(line: 48, scope: !2285)
!2285 = distinct !DILexicalBlock(scope: !2286, file: !9, line: 48)
!2286 = distinct !DILexicalBlock(scope: !2282, file: !9, line: 48)
!2287 = !DILocation(line: 48, scope: !2288)
!2288 = distinct !DILexicalBlock(scope: !2285, file: !9, line: 48)
!2289 = !DILocation(line: 48, scope: !2286)
!2290 = distinct !{!2290, !2283, !2283, !1121}
!2291 = !DILocation(line: 49, scope: !2268)
!2292 = !DILocation(line: 50, scope: !2268)
!2293 = !DILocation(line: 51, scope: !2268)
!2294 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethodV", linkageName: "\01_JNI_CallNonvirtualVoidMethodV@20", scope: !9, file: !9, line: 53, type: !426, scopeLine: 54, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2295 = !DILocalVariable(name: "args", arg: 5, scope: !2294, file: !9, line: 53, type: !149)
!2296 = !DILocation(line: 53, scope: !2294)
!2297 = !DILocalVariable(name: "methodID", arg: 4, scope: !2294, file: !9, line: 53, type: !67)
!2298 = !DILocalVariable(name: "clazz", arg: 3, scope: !2294, file: !9, line: 53, type: !47)
!2299 = !DILocalVariable(name: "obj", arg: 2, scope: !2294, file: !9, line: 53, type: !48)
!2300 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2294, file: !9, line: 53, type: !25)
!2301 = !DILocalVariable(name: "sig", scope: !2294, file: !9, line: 55, type: !1105)
!2302 = !DILocation(line: 55, scope: !2294)
!2303 = !DILocalVariable(name: "argc", scope: !2294, file: !9, line: 55, type: !13)
!2304 = !DILocalVariable(name: "argv", scope: !2294, file: !9, line: 55, type: !1110)
!2305 = !DILocalVariable(name: "i", scope: !2306, file: !9, line: 55, type: !13)
!2306 = distinct !DILexicalBlock(scope: !2294, file: !9, line: 55)
!2307 = !DILocation(line: 55, scope: !2306)
!2308 = !DILocation(line: 55, scope: !2309)
!2309 = distinct !DILexicalBlock(scope: !2310, file: !9, line: 55)
!2310 = distinct !DILexicalBlock(scope: !2306, file: !9, line: 55)
!2311 = !DILocation(line: 55, scope: !2312)
!2312 = distinct !DILexicalBlock(scope: !2309, file: !9, line: 55)
!2313 = !DILocation(line: 55, scope: !2310)
!2314 = distinct !{!2314, !2307, !2307, !1121}
!2315 = !DILocation(line: 56, scope: !2294)
!2316 = !DILocation(line: 57, scope: !2294)
!2317 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethod", scope: !9, file: !9, line: 59, type: !610, scopeLine: 60, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2318 = !DILocalVariable(name: "methodID", arg: 3, scope: !2317, file: !9, line: 59, type: !67)
!2319 = !DILocation(line: 59, scope: !2317)
!2320 = !DILocalVariable(name: "clazz", arg: 2, scope: !2317, file: !9, line: 59, type: !47)
!2321 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2317, file: !9, line: 59, type: !25)
!2322 = !DILocalVariable(name: "args", scope: !2317, file: !9, line: 61, type: !149)
!2323 = !DILocation(line: 61, scope: !2317)
!2324 = !DILocation(line: 62, scope: !2317)
!2325 = !DILocalVariable(name: "sig", scope: !2317, file: !9, line: 63, type: !1105)
!2326 = !DILocation(line: 63, scope: !2317)
!2327 = !DILocalVariable(name: "argc", scope: !2317, file: !9, line: 63, type: !13)
!2328 = !DILocalVariable(name: "argv", scope: !2317, file: !9, line: 63, type: !1110)
!2329 = !DILocalVariable(name: "i", scope: !2330, file: !9, line: 63, type: !13)
!2330 = distinct !DILexicalBlock(scope: !2317, file: !9, line: 63)
!2331 = !DILocation(line: 63, scope: !2330)
!2332 = !DILocation(line: 63, scope: !2333)
!2333 = distinct !DILexicalBlock(scope: !2334, file: !9, line: 63)
!2334 = distinct !DILexicalBlock(scope: !2330, file: !9, line: 63)
!2335 = !DILocation(line: 63, scope: !2336)
!2336 = distinct !DILexicalBlock(scope: !2333, file: !9, line: 63)
!2337 = !DILocation(line: 63, scope: !2334)
!2338 = distinct !{!2338, !2331, !2331, !1121}
!2339 = !DILocation(line: 64, scope: !2317)
!2340 = !DILocation(line: 65, scope: !2317)
!2341 = !DILocation(line: 66, scope: !2317)
!2342 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethodV", linkageName: "\01_JNI_CallStaticVoidMethodV@16", scope: !9, file: !9, line: 68, type: !614, scopeLine: 69, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2343 = !DILocalVariable(name: "args", arg: 4, scope: !2342, file: !9, line: 68, type: !149)
!2344 = !DILocation(line: 68, scope: !2342)
!2345 = !DILocalVariable(name: "methodID", arg: 3, scope: !2342, file: !9, line: 68, type: !67)
!2346 = !DILocalVariable(name: "clazz", arg: 2, scope: !2342, file: !9, line: 68, type: !47)
!2347 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2342, file: !9, line: 68, type: !25)
!2348 = !DILocalVariable(name: "sig", scope: !2342, file: !9, line: 70, type: !1105)
!2349 = !DILocation(line: 70, scope: !2342)
!2350 = !DILocalVariable(name: "argc", scope: !2342, file: !9, line: 70, type: !13)
!2351 = !DILocalVariable(name: "argv", scope: !2342, file: !9, line: 70, type: !1110)
!2352 = !DILocalVariable(name: "i", scope: !2353, file: !9, line: 70, type: !13)
!2353 = distinct !DILexicalBlock(scope: !2342, file: !9, line: 70)
!2354 = !DILocation(line: 70, scope: !2353)
!2355 = !DILocation(line: 70, scope: !2356)
!2356 = distinct !DILexicalBlock(scope: !2357, file: !9, line: 70)
!2357 = distinct !DILexicalBlock(scope: !2353, file: !9, line: 70)
!2358 = !DILocation(line: 70, scope: !2359)
!2359 = distinct !DILexicalBlock(scope: !2356, file: !9, line: 70)
!2360 = !DILocation(line: 70, scope: !2357)
!2361 = distinct !{!2361, !2354, !2354, !1121}
!2362 = !DILocation(line: 71, scope: !2342)
!2363 = !DILocation(line: 72, scope: !2342)
!2364 = distinct !DISubprogram(name: "_vsprintf_l", scope: !1041, file: !1041, line: 1449, type: !2365, scopeLine: 1458, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2365 = !DISubroutineType(types: !2366)
!2366 = !{!13, !1044, !1045, !2367, !149}
!2367 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !2368)
!2368 = !DIDerivedType(tag: DW_TAG_typedef, name: "_locale_t", file: !2369, line: 623, baseType: !2370)
!2369 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt.h", directory: "", checksumkind: CSK_MD5, checksum: "4ce81db8e96f94c79f8dce86dd46b97f")
!2370 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2371, size: 32)
!2371 = !DIDerivedType(tag: DW_TAG_typedef, name: "__crt_locale_pointers", file: !2369, line: 621, baseType: !2372)
!2372 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_pointers", file: !2369, line: 617, size: 64, elements: !2373)
!2373 = !{!2374, !2377}
!2374 = !DIDerivedType(tag: DW_TAG_member, name: "locinfo", scope: !2372, file: !2369, line: 619, baseType: !2375, size: 32)
!2375 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2376, size: 32)
!2376 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_data", file: !2369, line: 619, flags: DIFlagFwdDecl)
!2377 = !DIDerivedType(tag: DW_TAG_member, name: "mbcinfo", scope: !2372, file: !2369, line: 620, baseType: !2378, size: 32, offset: 32)
!2378 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2379, size: 32)
!2379 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_multibyte_data", file: !2369, line: 620, flags: DIFlagFwdDecl)
!2380 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !2364, file: !1041, line: 1453, type: !149)
!2381 = !DILocation(line: 1453, scope: !2364)
!2382 = !DILocalVariable(name: "_Locale", arg: 3, scope: !2364, file: !1041, line: 1452, type: !2367)
!2383 = !DILocation(line: 1452, scope: !2364)
!2384 = !DILocalVariable(name: "_Format", arg: 2, scope: !2364, file: !1041, line: 1451, type: !1045)
!2385 = !DILocation(line: 1451, scope: !2364)
!2386 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2364, file: !1041, line: 1450, type: !1044)
!2387 = !DILocation(line: 1450, scope: !2364)
!2388 = !DILocation(line: 1459, scope: !2364)
!2389 = distinct !DISubprogram(name: "_vsnprintf_l", scope: !1041, file: !1041, line: 1381, type: !2390, scopeLine: 1391, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1033)
!2390 = !DISubroutineType(types: !2391)
!2391 = !{!13, !1044, !1071, !1045, !2367, !149}
!2392 = !DILocalVariable(name: "_ArgList", arg: 5, scope: !2389, file: !1041, line: 1386, type: !149)
!2393 = !DILocation(line: 1386, scope: !2389)
!2394 = !DILocalVariable(name: "_Locale", arg: 4, scope: !2389, file: !1041, line: 1385, type: !2367)
!2395 = !DILocation(line: 1385, scope: !2389)
!2396 = !DILocalVariable(name: "_Format", arg: 3, scope: !2389, file: !1041, line: 1384, type: !1045)
!2397 = !DILocation(line: 1384, scope: !2389)
!2398 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !2389, file: !1041, line: 1383, type: !1071)
!2399 = !DILocation(line: 1383, scope: !2389)
!2400 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2389, file: !1041, line: 1382, type: !1044)
!2401 = !DILocation(line: 1382, scope: !2389)
!2402 = !DILocalVariable(name: "_Result", scope: !2389, file: !1041, line: 1392, type: !2403)
!2403 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !13)
!2404 = !DILocation(line: 1392, scope: !2389)
!2405 = !DILocation(line: 1396, scope: !2389)
!2406 = !DILocation(line: 92, scope: !2)
