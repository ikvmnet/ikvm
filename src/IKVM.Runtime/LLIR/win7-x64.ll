; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.34.31933"

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
define linkonce_odr dso_local i32 @sprintf(ptr noundef %0, ptr noundef %1, ...) #0 comdat !dbg !1039 {
  %3 = alloca ptr, align 8
  %4 = alloca ptr, align 8
  %5 = alloca i32, align 4
  %6 = alloca ptr, align 8
  store ptr %1, ptr %3, align 8
  call void @llvm.dbg.declare(metadata ptr %3, metadata !1045, metadata !DIExpression()), !dbg !1046
  store ptr %0, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1047, metadata !DIExpression()), !dbg !1048
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1049, metadata !DIExpression()), !dbg !1050
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1051, metadata !DIExpression()), !dbg !1052
  call void @llvm.va_start(ptr %6), !dbg !1053
  %7 = load ptr, ptr %6, align 8, !dbg !1054
  %8 = load ptr, ptr %3, align 8, !dbg !1054
  %9 = load ptr, ptr %4, align 8, !dbg !1054
  %10 = call i32 @_vsprintf_l(ptr noundef %9, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1054
  store i32 %10, ptr %5, align 4, !dbg !1054
  call void @llvm.va_end(ptr %6), !dbg !1055
  %11 = load i32, ptr %5, align 4, !dbg !1056
  ret i32 %11, !dbg !1056
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @vsprintf(ptr noundef %0, ptr noundef %1, ptr noundef %2) #0 comdat !dbg !1057 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1060, metadata !DIExpression()), !dbg !1061
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1062, metadata !DIExpression()), !dbg !1063
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1064, metadata !DIExpression()), !dbg !1065
  %7 = load ptr, ptr %4, align 8, !dbg !1066
  %8 = load ptr, ptr %5, align 8, !dbg !1066
  %9 = load ptr, ptr %6, align 8, !dbg !1066
  %10 = call i32 @_vsnprintf_l(ptr noundef %9, i64 noundef -1, ptr noundef %8, ptr noundef null, ptr noundef %7), !dbg !1066
  ret i32 %10, !dbg !1066
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_snprintf(ptr noundef %0, i64 noundef %1, ptr noundef %2, ...) #0 comdat !dbg !1067 {
  %4 = alloca ptr, align 8
  %5 = alloca i64, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i32, align 4
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1071, metadata !DIExpression()), !dbg !1072
  store i64 %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1073, metadata !DIExpression()), !dbg !1074
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1075, metadata !DIExpression()), !dbg !1076
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1077, metadata !DIExpression()), !dbg !1078
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1079, metadata !DIExpression()), !dbg !1080
  call void @llvm.va_start(ptr %8), !dbg !1081
  %9 = load ptr, ptr %8, align 8, !dbg !1082
  %10 = load ptr, ptr %4, align 8, !dbg !1082
  %11 = load i64, ptr %5, align 8, !dbg !1082
  %12 = load ptr, ptr %6, align 8, !dbg !1082
  %13 = call i32 @_vsnprintf(ptr noundef %12, i64 noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1082
  store i32 %13, ptr %7, align 4, !dbg !1082
  call void @llvm.va_end(ptr %8), !dbg !1083
  %14 = load i32, ptr %7, align 4, !dbg !1084
  ret i32 %14, !dbg !1084
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsnprintf(ptr noundef %0, i64 noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !1085 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca i64, align 8
  %8 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1088, metadata !DIExpression()), !dbg !1089
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1090, metadata !DIExpression()), !dbg !1091
  store i64 %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1092, metadata !DIExpression()), !dbg !1093
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1094, metadata !DIExpression()), !dbg !1095
  %9 = load ptr, ptr %5, align 8, !dbg !1096
  %10 = load ptr, ptr %6, align 8, !dbg !1096
  %11 = load i64, ptr %7, align 8, !dbg !1096
  %12 = load ptr, ptr %8, align 8, !dbg !1096
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i64 noundef %11, ptr noundef %10, ptr noundef null, ptr noundef %9), !dbg !1096
  ret i32 %13, !dbg !1096
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1097 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1098, metadata !DIExpression()), !dbg !1099
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1100, metadata !DIExpression()), !dbg !1099
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1101, metadata !DIExpression()), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1102, metadata !DIExpression()), !dbg !1099
  call void @llvm.va_start(ptr %7), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1103, metadata !DIExpression()), !dbg !1099
  %9 = load ptr, ptr %7, align 8, !dbg !1099
  %10 = load ptr, ptr %4, align 8, !dbg !1099
  %11 = load ptr, ptr %5, align 8, !dbg !1099
  %12 = load ptr, ptr %6, align 8, !dbg !1099
  %13 = call ptr @JNI_CallObjectMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1099
  store ptr %13, ptr %8, align 8, !dbg !1099
  call void @llvm.va_end(ptr %7), !dbg !1099
  %14 = load ptr, ptr %8, align 8, !dbg !1099
  ret ptr %14, !dbg !1099
}

; Function Attrs: nocallback nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1104 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1105, metadata !DIExpression()), !dbg !1106
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1107, metadata !DIExpression()), !dbg !1106
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1108, metadata !DIExpression()), !dbg !1106
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1109, metadata !DIExpression()), !dbg !1106
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1110, metadata !DIExpression()), !dbg !1106
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1114, metadata !DIExpression()), !dbg !1106
  %13 = load ptr, ptr %8, align 8, !dbg !1106
  %14 = load ptr, ptr %13, align 8, !dbg !1106
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1106
  %16 = load ptr, ptr %15, align 8, !dbg !1106
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1106
  %18 = load ptr, ptr %6, align 8, !dbg !1106
  %19 = load ptr, ptr %8, align 8, !dbg !1106
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1106
  store i32 %20, ptr %10, align 4, !dbg !1106
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1115, metadata !DIExpression()), !dbg !1106
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1117, metadata !DIExpression()), !dbg !1119
  store i32 0, ptr %12, align 4, !dbg !1119
  br label %21, !dbg !1119

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1119
  %23 = load i32, ptr %10, align 4, !dbg !1119
  %24 = icmp slt i32 %22, %23, !dbg !1119
  br i1 %24, label %25, label %105, !dbg !1119

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1120
  %27 = sext i32 %26 to i64, !dbg !1120
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1120
  %29 = load i8, ptr %28, align 1, !dbg !1120
  %30 = sext i8 %29 to i32, !dbg !1120
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1120

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1123
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1123
  store ptr %33, ptr %5, align 8, !dbg !1123
  %34 = load i32, ptr %32, align 8, !dbg !1123
  %35 = trunc i32 %34 to i8, !dbg !1123
  %36 = load i32, ptr %12, align 4, !dbg !1123
  %37 = sext i32 %36 to i64, !dbg !1123
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1123
  store i8 %35, ptr %38, align 8, !dbg !1123
  br label %101, !dbg !1123

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1123
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1123
  store ptr %41, ptr %5, align 8, !dbg !1123
  %42 = load i32, ptr %40, align 8, !dbg !1123
  %43 = trunc i32 %42 to i8, !dbg !1123
  %44 = load i32, ptr %12, align 4, !dbg !1123
  %45 = sext i32 %44 to i64, !dbg !1123
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1123
  store i8 %43, ptr %46, align 8, !dbg !1123
  br label %101, !dbg !1123

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1123
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1123
  store ptr %49, ptr %5, align 8, !dbg !1123
  %50 = load i32, ptr %48, align 8, !dbg !1123
  %51 = trunc i32 %50 to i16, !dbg !1123
  %52 = load i32, ptr %12, align 4, !dbg !1123
  %53 = sext i32 %52 to i64, !dbg !1123
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1123
  store i16 %51, ptr %54, align 8, !dbg !1123
  br label %101, !dbg !1123

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1123
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1123
  store ptr %57, ptr %5, align 8, !dbg !1123
  %58 = load i32, ptr %56, align 8, !dbg !1123
  %59 = trunc i32 %58 to i16, !dbg !1123
  %60 = load i32, ptr %12, align 4, !dbg !1123
  %61 = sext i32 %60 to i64, !dbg !1123
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1123
  store i16 %59, ptr %62, align 8, !dbg !1123
  br label %101, !dbg !1123

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1123
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1123
  store ptr %65, ptr %5, align 8, !dbg !1123
  %66 = load i32, ptr %64, align 8, !dbg !1123
  %67 = load i32, ptr %12, align 4, !dbg !1123
  %68 = sext i32 %67 to i64, !dbg !1123
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1123
  store i32 %66, ptr %69, align 8, !dbg !1123
  br label %101, !dbg !1123

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1123
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1123
  store ptr %72, ptr %5, align 8, !dbg !1123
  %73 = load i32, ptr %71, align 8, !dbg !1123
  %74 = sext i32 %73 to i64, !dbg !1123
  %75 = load i32, ptr %12, align 4, !dbg !1123
  %76 = sext i32 %75 to i64, !dbg !1123
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1123
  store i64 %74, ptr %77, align 8, !dbg !1123
  br label %101, !dbg !1123

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1123
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1123
  store ptr %80, ptr %5, align 8, !dbg !1123
  %81 = load double, ptr %79, align 8, !dbg !1123
  %82 = fptrunc double %81 to float, !dbg !1123
  %83 = load i32, ptr %12, align 4, !dbg !1123
  %84 = sext i32 %83 to i64, !dbg !1123
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1123
  store float %82, ptr %85, align 8, !dbg !1123
  br label %101, !dbg !1123

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1123
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1123
  store ptr %88, ptr %5, align 8, !dbg !1123
  %89 = load double, ptr %87, align 8, !dbg !1123
  %90 = load i32, ptr %12, align 4, !dbg !1123
  %91 = sext i32 %90 to i64, !dbg !1123
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1123
  store double %89, ptr %92, align 8, !dbg !1123
  br label %101, !dbg !1123

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1123
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1123
  store ptr %95, ptr %5, align 8, !dbg !1123
  %96 = load ptr, ptr %94, align 8, !dbg !1123
  %97 = load i32, ptr %12, align 4, !dbg !1123
  %98 = sext i32 %97 to i64, !dbg !1123
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1123
  store ptr %96, ptr %99, align 8, !dbg !1123
  br label %101, !dbg !1123

100:                                              ; preds = %25
  br label %101, !dbg !1123

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1120

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1125
  %104 = add nsw i32 %103, 1, !dbg !1125
  store i32 %104, ptr %12, align 4, !dbg !1125
  br label %21, !dbg !1125, !llvm.loop !1126

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1106
  %107 = load ptr, ptr %106, align 8, !dbg !1106
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 36, !dbg !1106
  %109 = load ptr, ptr %108, align 8, !dbg !1106
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1106
  %111 = load ptr, ptr %6, align 8, !dbg !1106
  %112 = load ptr, ptr %7, align 8, !dbg !1106
  %113 = load ptr, ptr %8, align 8, !dbg !1106
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1106
  ret ptr %114, !dbg !1106
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1128 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1129, metadata !DIExpression()), !dbg !1130
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1131, metadata !DIExpression()), !dbg !1130
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1132, metadata !DIExpression()), !dbg !1130
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1133, metadata !DIExpression()), !dbg !1130
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1134, metadata !DIExpression()), !dbg !1130
  call void @llvm.va_start(ptr %9), !dbg !1130
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1135, metadata !DIExpression()), !dbg !1130
  %11 = load ptr, ptr %9, align 8, !dbg !1130
  %12 = load ptr, ptr %5, align 8, !dbg !1130
  %13 = load ptr, ptr %6, align 8, !dbg !1130
  %14 = load ptr, ptr %7, align 8, !dbg !1130
  %15 = load ptr, ptr %8, align 8, !dbg !1130
  %16 = call ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1130
  store ptr %16, ptr %10, align 8, !dbg !1130
  call void @llvm.va_end(ptr %9), !dbg !1130
  %17 = load ptr, ptr %10, align 8, !dbg !1130
  ret ptr %17, !dbg !1130
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1136 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1137, metadata !DIExpression()), !dbg !1138
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1139, metadata !DIExpression()), !dbg !1138
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1140, metadata !DIExpression()), !dbg !1138
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1141, metadata !DIExpression()), !dbg !1138
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1142, metadata !DIExpression()), !dbg !1138
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1143, metadata !DIExpression()), !dbg !1138
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1144, metadata !DIExpression()), !dbg !1138
  %15 = load ptr, ptr %10, align 8, !dbg !1138
  %16 = load ptr, ptr %15, align 8, !dbg !1138
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1138
  %18 = load ptr, ptr %17, align 8, !dbg !1138
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1138
  %20 = load ptr, ptr %7, align 8, !dbg !1138
  %21 = load ptr, ptr %10, align 8, !dbg !1138
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1138
  store i32 %22, ptr %12, align 4, !dbg !1138
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1145, metadata !DIExpression()), !dbg !1138
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1146, metadata !DIExpression()), !dbg !1148
  store i32 0, ptr %14, align 4, !dbg !1148
  br label %23, !dbg !1148

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1148
  %25 = load i32, ptr %12, align 4, !dbg !1148
  %26 = icmp slt i32 %24, %25, !dbg !1148
  br i1 %26, label %27, label %107, !dbg !1148

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1149
  %29 = sext i32 %28 to i64, !dbg !1149
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1149
  %31 = load i8, ptr %30, align 1, !dbg !1149
  %32 = sext i8 %31 to i32, !dbg !1149
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1149

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1152
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1152
  store ptr %35, ptr %6, align 8, !dbg !1152
  %36 = load i32, ptr %34, align 8, !dbg !1152
  %37 = trunc i32 %36 to i8, !dbg !1152
  %38 = load i32, ptr %14, align 4, !dbg !1152
  %39 = sext i32 %38 to i64, !dbg !1152
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1152
  store i8 %37, ptr %40, align 8, !dbg !1152
  br label %103, !dbg !1152

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1152
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1152
  store ptr %43, ptr %6, align 8, !dbg !1152
  %44 = load i32, ptr %42, align 8, !dbg !1152
  %45 = trunc i32 %44 to i8, !dbg !1152
  %46 = load i32, ptr %14, align 4, !dbg !1152
  %47 = sext i32 %46 to i64, !dbg !1152
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1152
  store i8 %45, ptr %48, align 8, !dbg !1152
  br label %103, !dbg !1152

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1152
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1152
  store ptr %51, ptr %6, align 8, !dbg !1152
  %52 = load i32, ptr %50, align 8, !dbg !1152
  %53 = trunc i32 %52 to i16, !dbg !1152
  %54 = load i32, ptr %14, align 4, !dbg !1152
  %55 = sext i32 %54 to i64, !dbg !1152
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1152
  store i16 %53, ptr %56, align 8, !dbg !1152
  br label %103, !dbg !1152

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1152
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1152
  store ptr %59, ptr %6, align 8, !dbg !1152
  %60 = load i32, ptr %58, align 8, !dbg !1152
  %61 = trunc i32 %60 to i16, !dbg !1152
  %62 = load i32, ptr %14, align 4, !dbg !1152
  %63 = sext i32 %62 to i64, !dbg !1152
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1152
  store i16 %61, ptr %64, align 8, !dbg !1152
  br label %103, !dbg !1152

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1152
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1152
  store ptr %67, ptr %6, align 8, !dbg !1152
  %68 = load i32, ptr %66, align 8, !dbg !1152
  %69 = load i32, ptr %14, align 4, !dbg !1152
  %70 = sext i32 %69 to i64, !dbg !1152
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1152
  store i32 %68, ptr %71, align 8, !dbg !1152
  br label %103, !dbg !1152

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1152
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1152
  store ptr %74, ptr %6, align 8, !dbg !1152
  %75 = load i32, ptr %73, align 8, !dbg !1152
  %76 = sext i32 %75 to i64, !dbg !1152
  %77 = load i32, ptr %14, align 4, !dbg !1152
  %78 = sext i32 %77 to i64, !dbg !1152
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1152
  store i64 %76, ptr %79, align 8, !dbg !1152
  br label %103, !dbg !1152

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1152
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1152
  store ptr %82, ptr %6, align 8, !dbg !1152
  %83 = load double, ptr %81, align 8, !dbg !1152
  %84 = fptrunc double %83 to float, !dbg !1152
  %85 = load i32, ptr %14, align 4, !dbg !1152
  %86 = sext i32 %85 to i64, !dbg !1152
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1152
  store float %84, ptr %87, align 8, !dbg !1152
  br label %103, !dbg !1152

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1152
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1152
  store ptr %90, ptr %6, align 8, !dbg !1152
  %91 = load double, ptr %89, align 8, !dbg !1152
  %92 = load i32, ptr %14, align 4, !dbg !1152
  %93 = sext i32 %92 to i64, !dbg !1152
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1152
  store double %91, ptr %94, align 8, !dbg !1152
  br label %103, !dbg !1152

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1152
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1152
  store ptr %97, ptr %6, align 8, !dbg !1152
  %98 = load ptr, ptr %96, align 8, !dbg !1152
  %99 = load i32, ptr %14, align 4, !dbg !1152
  %100 = sext i32 %99 to i64, !dbg !1152
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1152
  store ptr %98, ptr %101, align 8, !dbg !1152
  br label %103, !dbg !1152

102:                                              ; preds = %27
  br label %103, !dbg !1152

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1149

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1154
  %106 = add nsw i32 %105, 1, !dbg !1154
  store i32 %106, ptr %14, align 4, !dbg !1154
  br label %23, !dbg !1154, !llvm.loop !1155

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1138
  %109 = load ptr, ptr %108, align 8, !dbg !1138
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 66, !dbg !1138
  %111 = load ptr, ptr %110, align 8, !dbg !1138
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1138
  %113 = load ptr, ptr %7, align 8, !dbg !1138
  %114 = load ptr, ptr %8, align 8, !dbg !1138
  %115 = load ptr, ptr %9, align 8, !dbg !1138
  %116 = load ptr, ptr %10, align 8, !dbg !1138
  %117 = call ptr %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1138
  ret ptr %117, !dbg !1138
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1156 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1157, metadata !DIExpression()), !dbg !1158
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1159, metadata !DIExpression()), !dbg !1158
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1160, metadata !DIExpression()), !dbg !1158
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1161, metadata !DIExpression()), !dbg !1158
  call void @llvm.va_start(ptr %7), !dbg !1158
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1162, metadata !DIExpression()), !dbg !1158
  %9 = load ptr, ptr %7, align 8, !dbg !1158
  %10 = load ptr, ptr %4, align 8, !dbg !1158
  %11 = load ptr, ptr %5, align 8, !dbg !1158
  %12 = load ptr, ptr %6, align 8, !dbg !1158
  %13 = call ptr @JNI_CallStaticObjectMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1158
  store ptr %13, ptr %8, align 8, !dbg !1158
  call void @llvm.va_end(ptr %7), !dbg !1158
  %14 = load ptr, ptr %8, align 8, !dbg !1158
  ret ptr %14, !dbg !1158
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1163 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1164, metadata !DIExpression()), !dbg !1165
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1166, metadata !DIExpression()), !dbg !1165
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1167, metadata !DIExpression()), !dbg !1165
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1168, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1169, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1170, metadata !DIExpression()), !dbg !1165
  %13 = load ptr, ptr %8, align 8, !dbg !1165
  %14 = load ptr, ptr %13, align 8, !dbg !1165
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1165
  %16 = load ptr, ptr %15, align 8, !dbg !1165
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1165
  %18 = load ptr, ptr %6, align 8, !dbg !1165
  %19 = load ptr, ptr %8, align 8, !dbg !1165
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1165
  store i32 %20, ptr %10, align 4, !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1171, metadata !DIExpression()), !dbg !1165
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1172, metadata !DIExpression()), !dbg !1174
  store i32 0, ptr %12, align 4, !dbg !1174
  br label %21, !dbg !1174

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1174
  %23 = load i32, ptr %10, align 4, !dbg !1174
  %24 = icmp slt i32 %22, %23, !dbg !1174
  br i1 %24, label %25, label %105, !dbg !1174

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1175
  %27 = sext i32 %26 to i64, !dbg !1175
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1175
  %29 = load i8, ptr %28, align 1, !dbg !1175
  %30 = sext i8 %29 to i32, !dbg !1175
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1175

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1178
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1178
  store ptr %33, ptr %5, align 8, !dbg !1178
  %34 = load i32, ptr %32, align 8, !dbg !1178
  %35 = trunc i32 %34 to i8, !dbg !1178
  %36 = load i32, ptr %12, align 4, !dbg !1178
  %37 = sext i32 %36 to i64, !dbg !1178
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1178
  store i8 %35, ptr %38, align 8, !dbg !1178
  br label %101, !dbg !1178

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1178
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1178
  store ptr %41, ptr %5, align 8, !dbg !1178
  %42 = load i32, ptr %40, align 8, !dbg !1178
  %43 = trunc i32 %42 to i8, !dbg !1178
  %44 = load i32, ptr %12, align 4, !dbg !1178
  %45 = sext i32 %44 to i64, !dbg !1178
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1178
  store i8 %43, ptr %46, align 8, !dbg !1178
  br label %101, !dbg !1178

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1178
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1178
  store ptr %49, ptr %5, align 8, !dbg !1178
  %50 = load i32, ptr %48, align 8, !dbg !1178
  %51 = trunc i32 %50 to i16, !dbg !1178
  %52 = load i32, ptr %12, align 4, !dbg !1178
  %53 = sext i32 %52 to i64, !dbg !1178
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1178
  store i16 %51, ptr %54, align 8, !dbg !1178
  br label %101, !dbg !1178

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1178
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1178
  store ptr %57, ptr %5, align 8, !dbg !1178
  %58 = load i32, ptr %56, align 8, !dbg !1178
  %59 = trunc i32 %58 to i16, !dbg !1178
  %60 = load i32, ptr %12, align 4, !dbg !1178
  %61 = sext i32 %60 to i64, !dbg !1178
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1178
  store i16 %59, ptr %62, align 8, !dbg !1178
  br label %101, !dbg !1178

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1178
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1178
  store ptr %65, ptr %5, align 8, !dbg !1178
  %66 = load i32, ptr %64, align 8, !dbg !1178
  %67 = load i32, ptr %12, align 4, !dbg !1178
  %68 = sext i32 %67 to i64, !dbg !1178
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1178
  store i32 %66, ptr %69, align 8, !dbg !1178
  br label %101, !dbg !1178

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1178
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1178
  store ptr %72, ptr %5, align 8, !dbg !1178
  %73 = load i32, ptr %71, align 8, !dbg !1178
  %74 = sext i32 %73 to i64, !dbg !1178
  %75 = load i32, ptr %12, align 4, !dbg !1178
  %76 = sext i32 %75 to i64, !dbg !1178
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1178
  store i64 %74, ptr %77, align 8, !dbg !1178
  br label %101, !dbg !1178

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1178
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1178
  store ptr %80, ptr %5, align 8, !dbg !1178
  %81 = load double, ptr %79, align 8, !dbg !1178
  %82 = fptrunc double %81 to float, !dbg !1178
  %83 = load i32, ptr %12, align 4, !dbg !1178
  %84 = sext i32 %83 to i64, !dbg !1178
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1178
  store float %82, ptr %85, align 8, !dbg !1178
  br label %101, !dbg !1178

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1178
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1178
  store ptr %88, ptr %5, align 8, !dbg !1178
  %89 = load double, ptr %87, align 8, !dbg !1178
  %90 = load i32, ptr %12, align 4, !dbg !1178
  %91 = sext i32 %90 to i64, !dbg !1178
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1178
  store double %89, ptr %92, align 8, !dbg !1178
  br label %101, !dbg !1178

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1178
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1178
  store ptr %95, ptr %5, align 8, !dbg !1178
  %96 = load ptr, ptr %94, align 8, !dbg !1178
  %97 = load i32, ptr %12, align 4, !dbg !1178
  %98 = sext i32 %97 to i64, !dbg !1178
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1178
  store ptr %96, ptr %99, align 8, !dbg !1178
  br label %101, !dbg !1178

100:                                              ; preds = %25
  br label %101, !dbg !1178

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1175

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1180
  %104 = add nsw i32 %103, 1, !dbg !1180
  store i32 %104, ptr %12, align 4, !dbg !1180
  br label %21, !dbg !1180, !llvm.loop !1181

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1165
  %107 = load ptr, ptr %106, align 8, !dbg !1165
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 116, !dbg !1165
  %109 = load ptr, ptr %108, align 8, !dbg !1165
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1165
  %111 = load ptr, ptr %6, align 8, !dbg !1165
  %112 = load ptr, ptr %7, align 8, !dbg !1165
  %113 = load ptr, ptr %8, align 8, !dbg !1165
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1165
  ret ptr %114, !dbg !1165
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1182 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1183, metadata !DIExpression()), !dbg !1184
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1185, metadata !DIExpression()), !dbg !1184
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1186, metadata !DIExpression()), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1187, metadata !DIExpression()), !dbg !1184
  call void @llvm.va_start(ptr %7), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1188, metadata !DIExpression()), !dbg !1184
  %9 = load ptr, ptr %7, align 8, !dbg !1184
  %10 = load ptr, ptr %4, align 8, !dbg !1184
  %11 = load ptr, ptr %5, align 8, !dbg !1184
  %12 = load ptr, ptr %6, align 8, !dbg !1184
  %13 = call i8 @JNI_CallBooleanMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1184
  store i8 %13, ptr %8, align 1, !dbg !1184
  call void @llvm.va_end(ptr %7), !dbg !1184
  %14 = load i8, ptr %8, align 1, !dbg !1184
  ret i8 %14, !dbg !1184
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1189 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1190, metadata !DIExpression()), !dbg !1191
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1192, metadata !DIExpression()), !dbg !1191
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1193, metadata !DIExpression()), !dbg !1191
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1194, metadata !DIExpression()), !dbg !1191
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1195, metadata !DIExpression()), !dbg !1191
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1196, metadata !DIExpression()), !dbg !1191
  %13 = load ptr, ptr %8, align 8, !dbg !1191
  %14 = load ptr, ptr %13, align 8, !dbg !1191
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1191
  %16 = load ptr, ptr %15, align 8, !dbg !1191
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1191
  %18 = load ptr, ptr %6, align 8, !dbg !1191
  %19 = load ptr, ptr %8, align 8, !dbg !1191
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1191
  store i32 %20, ptr %10, align 4, !dbg !1191
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1197, metadata !DIExpression()), !dbg !1191
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1198, metadata !DIExpression()), !dbg !1200
  store i32 0, ptr %12, align 4, !dbg !1200
  br label %21, !dbg !1200

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1200
  %23 = load i32, ptr %10, align 4, !dbg !1200
  %24 = icmp slt i32 %22, %23, !dbg !1200
  br i1 %24, label %25, label %105, !dbg !1200

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1201
  %27 = sext i32 %26 to i64, !dbg !1201
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1201
  %29 = load i8, ptr %28, align 1, !dbg !1201
  %30 = sext i8 %29 to i32, !dbg !1201
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1201

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1204
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1204
  store ptr %33, ptr %5, align 8, !dbg !1204
  %34 = load i32, ptr %32, align 8, !dbg !1204
  %35 = trunc i32 %34 to i8, !dbg !1204
  %36 = load i32, ptr %12, align 4, !dbg !1204
  %37 = sext i32 %36 to i64, !dbg !1204
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1204
  store i8 %35, ptr %38, align 8, !dbg !1204
  br label %101, !dbg !1204

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1204
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1204
  store ptr %41, ptr %5, align 8, !dbg !1204
  %42 = load i32, ptr %40, align 8, !dbg !1204
  %43 = trunc i32 %42 to i8, !dbg !1204
  %44 = load i32, ptr %12, align 4, !dbg !1204
  %45 = sext i32 %44 to i64, !dbg !1204
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1204
  store i8 %43, ptr %46, align 8, !dbg !1204
  br label %101, !dbg !1204

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1204
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1204
  store ptr %49, ptr %5, align 8, !dbg !1204
  %50 = load i32, ptr %48, align 8, !dbg !1204
  %51 = trunc i32 %50 to i16, !dbg !1204
  %52 = load i32, ptr %12, align 4, !dbg !1204
  %53 = sext i32 %52 to i64, !dbg !1204
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1204
  store i16 %51, ptr %54, align 8, !dbg !1204
  br label %101, !dbg !1204

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1204
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1204
  store ptr %57, ptr %5, align 8, !dbg !1204
  %58 = load i32, ptr %56, align 8, !dbg !1204
  %59 = trunc i32 %58 to i16, !dbg !1204
  %60 = load i32, ptr %12, align 4, !dbg !1204
  %61 = sext i32 %60 to i64, !dbg !1204
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1204
  store i16 %59, ptr %62, align 8, !dbg !1204
  br label %101, !dbg !1204

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1204
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1204
  store ptr %65, ptr %5, align 8, !dbg !1204
  %66 = load i32, ptr %64, align 8, !dbg !1204
  %67 = load i32, ptr %12, align 4, !dbg !1204
  %68 = sext i32 %67 to i64, !dbg !1204
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1204
  store i32 %66, ptr %69, align 8, !dbg !1204
  br label %101, !dbg !1204

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1204
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1204
  store ptr %72, ptr %5, align 8, !dbg !1204
  %73 = load i32, ptr %71, align 8, !dbg !1204
  %74 = sext i32 %73 to i64, !dbg !1204
  %75 = load i32, ptr %12, align 4, !dbg !1204
  %76 = sext i32 %75 to i64, !dbg !1204
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1204
  store i64 %74, ptr %77, align 8, !dbg !1204
  br label %101, !dbg !1204

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1204
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1204
  store ptr %80, ptr %5, align 8, !dbg !1204
  %81 = load double, ptr %79, align 8, !dbg !1204
  %82 = fptrunc double %81 to float, !dbg !1204
  %83 = load i32, ptr %12, align 4, !dbg !1204
  %84 = sext i32 %83 to i64, !dbg !1204
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1204
  store float %82, ptr %85, align 8, !dbg !1204
  br label %101, !dbg !1204

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1204
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1204
  store ptr %88, ptr %5, align 8, !dbg !1204
  %89 = load double, ptr %87, align 8, !dbg !1204
  %90 = load i32, ptr %12, align 4, !dbg !1204
  %91 = sext i32 %90 to i64, !dbg !1204
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1204
  store double %89, ptr %92, align 8, !dbg !1204
  br label %101, !dbg !1204

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1204
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1204
  store ptr %95, ptr %5, align 8, !dbg !1204
  %96 = load ptr, ptr %94, align 8, !dbg !1204
  %97 = load i32, ptr %12, align 4, !dbg !1204
  %98 = sext i32 %97 to i64, !dbg !1204
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1204
  store ptr %96, ptr %99, align 8, !dbg !1204
  br label %101, !dbg !1204

100:                                              ; preds = %25
  br label %101, !dbg !1204

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1201

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1206
  %104 = add nsw i32 %103, 1, !dbg !1206
  store i32 %104, ptr %12, align 4, !dbg !1206
  br label %21, !dbg !1206, !llvm.loop !1207

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1191
  %107 = load ptr, ptr %106, align 8, !dbg !1191
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 39, !dbg !1191
  %109 = load ptr, ptr %108, align 8, !dbg !1191
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1191
  %111 = load ptr, ptr %6, align 8, !dbg !1191
  %112 = load ptr, ptr %7, align 8, !dbg !1191
  %113 = load ptr, ptr %8, align 8, !dbg !1191
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1191
  ret i8 %114, !dbg !1191
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1208 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1209, metadata !DIExpression()), !dbg !1210
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1211, metadata !DIExpression()), !dbg !1210
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1212, metadata !DIExpression()), !dbg !1210
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1213, metadata !DIExpression()), !dbg !1210
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1214, metadata !DIExpression()), !dbg !1210
  call void @llvm.va_start(ptr %9), !dbg !1210
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1215, metadata !DIExpression()), !dbg !1210
  %11 = load ptr, ptr %9, align 8, !dbg !1210
  %12 = load ptr, ptr %5, align 8, !dbg !1210
  %13 = load ptr, ptr %6, align 8, !dbg !1210
  %14 = load ptr, ptr %7, align 8, !dbg !1210
  %15 = load ptr, ptr %8, align 8, !dbg !1210
  %16 = call i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1210
  store i8 %16, ptr %10, align 1, !dbg !1210
  call void @llvm.va_end(ptr %9), !dbg !1210
  %17 = load i8, ptr %10, align 1, !dbg !1210
  ret i8 %17, !dbg !1210
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1216 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1217, metadata !DIExpression()), !dbg !1218
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1219, metadata !DIExpression()), !dbg !1218
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1220, metadata !DIExpression()), !dbg !1218
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1221, metadata !DIExpression()), !dbg !1218
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1222, metadata !DIExpression()), !dbg !1218
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1223, metadata !DIExpression()), !dbg !1218
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1224, metadata !DIExpression()), !dbg !1218
  %15 = load ptr, ptr %10, align 8, !dbg !1218
  %16 = load ptr, ptr %15, align 8, !dbg !1218
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1218
  %18 = load ptr, ptr %17, align 8, !dbg !1218
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1218
  %20 = load ptr, ptr %7, align 8, !dbg !1218
  %21 = load ptr, ptr %10, align 8, !dbg !1218
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1218
  store i32 %22, ptr %12, align 4, !dbg !1218
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1225, metadata !DIExpression()), !dbg !1218
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1226, metadata !DIExpression()), !dbg !1228
  store i32 0, ptr %14, align 4, !dbg !1228
  br label %23, !dbg !1228

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1228
  %25 = load i32, ptr %12, align 4, !dbg !1228
  %26 = icmp slt i32 %24, %25, !dbg !1228
  br i1 %26, label %27, label %107, !dbg !1228

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1229
  %29 = sext i32 %28 to i64, !dbg !1229
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1229
  %31 = load i8, ptr %30, align 1, !dbg !1229
  %32 = sext i8 %31 to i32, !dbg !1229
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1229

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1232
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1232
  store ptr %35, ptr %6, align 8, !dbg !1232
  %36 = load i32, ptr %34, align 8, !dbg !1232
  %37 = trunc i32 %36 to i8, !dbg !1232
  %38 = load i32, ptr %14, align 4, !dbg !1232
  %39 = sext i32 %38 to i64, !dbg !1232
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1232
  store i8 %37, ptr %40, align 8, !dbg !1232
  br label %103, !dbg !1232

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1232
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1232
  store ptr %43, ptr %6, align 8, !dbg !1232
  %44 = load i32, ptr %42, align 8, !dbg !1232
  %45 = trunc i32 %44 to i8, !dbg !1232
  %46 = load i32, ptr %14, align 4, !dbg !1232
  %47 = sext i32 %46 to i64, !dbg !1232
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1232
  store i8 %45, ptr %48, align 8, !dbg !1232
  br label %103, !dbg !1232

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1232
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1232
  store ptr %51, ptr %6, align 8, !dbg !1232
  %52 = load i32, ptr %50, align 8, !dbg !1232
  %53 = trunc i32 %52 to i16, !dbg !1232
  %54 = load i32, ptr %14, align 4, !dbg !1232
  %55 = sext i32 %54 to i64, !dbg !1232
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1232
  store i16 %53, ptr %56, align 8, !dbg !1232
  br label %103, !dbg !1232

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1232
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1232
  store ptr %59, ptr %6, align 8, !dbg !1232
  %60 = load i32, ptr %58, align 8, !dbg !1232
  %61 = trunc i32 %60 to i16, !dbg !1232
  %62 = load i32, ptr %14, align 4, !dbg !1232
  %63 = sext i32 %62 to i64, !dbg !1232
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1232
  store i16 %61, ptr %64, align 8, !dbg !1232
  br label %103, !dbg !1232

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1232
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1232
  store ptr %67, ptr %6, align 8, !dbg !1232
  %68 = load i32, ptr %66, align 8, !dbg !1232
  %69 = load i32, ptr %14, align 4, !dbg !1232
  %70 = sext i32 %69 to i64, !dbg !1232
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1232
  store i32 %68, ptr %71, align 8, !dbg !1232
  br label %103, !dbg !1232

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1232
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1232
  store ptr %74, ptr %6, align 8, !dbg !1232
  %75 = load i32, ptr %73, align 8, !dbg !1232
  %76 = sext i32 %75 to i64, !dbg !1232
  %77 = load i32, ptr %14, align 4, !dbg !1232
  %78 = sext i32 %77 to i64, !dbg !1232
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1232
  store i64 %76, ptr %79, align 8, !dbg !1232
  br label %103, !dbg !1232

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1232
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1232
  store ptr %82, ptr %6, align 8, !dbg !1232
  %83 = load double, ptr %81, align 8, !dbg !1232
  %84 = fptrunc double %83 to float, !dbg !1232
  %85 = load i32, ptr %14, align 4, !dbg !1232
  %86 = sext i32 %85 to i64, !dbg !1232
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1232
  store float %84, ptr %87, align 8, !dbg !1232
  br label %103, !dbg !1232

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1232
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1232
  store ptr %90, ptr %6, align 8, !dbg !1232
  %91 = load double, ptr %89, align 8, !dbg !1232
  %92 = load i32, ptr %14, align 4, !dbg !1232
  %93 = sext i32 %92 to i64, !dbg !1232
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1232
  store double %91, ptr %94, align 8, !dbg !1232
  br label %103, !dbg !1232

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1232
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1232
  store ptr %97, ptr %6, align 8, !dbg !1232
  %98 = load ptr, ptr %96, align 8, !dbg !1232
  %99 = load i32, ptr %14, align 4, !dbg !1232
  %100 = sext i32 %99 to i64, !dbg !1232
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1232
  store ptr %98, ptr %101, align 8, !dbg !1232
  br label %103, !dbg !1232

102:                                              ; preds = %27
  br label %103, !dbg !1232

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1229

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1234
  %106 = add nsw i32 %105, 1, !dbg !1234
  store i32 %106, ptr %14, align 4, !dbg !1234
  br label %23, !dbg !1234, !llvm.loop !1235

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1218
  %109 = load ptr, ptr %108, align 8, !dbg !1218
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 69, !dbg !1218
  %111 = load ptr, ptr %110, align 8, !dbg !1218
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1218
  %113 = load ptr, ptr %7, align 8, !dbg !1218
  %114 = load ptr, ptr %8, align 8, !dbg !1218
  %115 = load ptr, ptr %9, align 8, !dbg !1218
  %116 = load ptr, ptr %10, align 8, !dbg !1218
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1218
  ret i8 %117, !dbg !1218
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1236 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1237, metadata !DIExpression()), !dbg !1238
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1239, metadata !DIExpression()), !dbg !1238
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1240, metadata !DIExpression()), !dbg !1238
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1241, metadata !DIExpression()), !dbg !1238
  call void @llvm.va_start(ptr %7), !dbg !1238
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1242, metadata !DIExpression()), !dbg !1238
  %9 = load ptr, ptr %7, align 8, !dbg !1238
  %10 = load ptr, ptr %4, align 8, !dbg !1238
  %11 = load ptr, ptr %5, align 8, !dbg !1238
  %12 = load ptr, ptr %6, align 8, !dbg !1238
  %13 = call i8 @JNI_CallStaticBooleanMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1238
  store i8 %13, ptr %8, align 1, !dbg !1238
  call void @llvm.va_end(ptr %7), !dbg !1238
  %14 = load i8, ptr %8, align 1, !dbg !1238
  ret i8 %14, !dbg !1238
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1243 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1244, metadata !DIExpression()), !dbg !1245
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1246, metadata !DIExpression()), !dbg !1245
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1247, metadata !DIExpression()), !dbg !1245
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1248, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1249, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1250, metadata !DIExpression()), !dbg !1245
  %13 = load ptr, ptr %8, align 8, !dbg !1245
  %14 = load ptr, ptr %13, align 8, !dbg !1245
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1245
  %16 = load ptr, ptr %15, align 8, !dbg !1245
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1245
  %18 = load ptr, ptr %6, align 8, !dbg !1245
  %19 = load ptr, ptr %8, align 8, !dbg !1245
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1245
  store i32 %20, ptr %10, align 4, !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1251, metadata !DIExpression()), !dbg !1245
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1252, metadata !DIExpression()), !dbg !1254
  store i32 0, ptr %12, align 4, !dbg !1254
  br label %21, !dbg !1254

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1254
  %23 = load i32, ptr %10, align 4, !dbg !1254
  %24 = icmp slt i32 %22, %23, !dbg !1254
  br i1 %24, label %25, label %105, !dbg !1254

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1255
  %27 = sext i32 %26 to i64, !dbg !1255
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1255
  %29 = load i8, ptr %28, align 1, !dbg !1255
  %30 = sext i8 %29 to i32, !dbg !1255
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1255

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1258
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1258
  store ptr %33, ptr %5, align 8, !dbg !1258
  %34 = load i32, ptr %32, align 8, !dbg !1258
  %35 = trunc i32 %34 to i8, !dbg !1258
  %36 = load i32, ptr %12, align 4, !dbg !1258
  %37 = sext i32 %36 to i64, !dbg !1258
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1258
  store i8 %35, ptr %38, align 8, !dbg !1258
  br label %101, !dbg !1258

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1258
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1258
  store ptr %41, ptr %5, align 8, !dbg !1258
  %42 = load i32, ptr %40, align 8, !dbg !1258
  %43 = trunc i32 %42 to i8, !dbg !1258
  %44 = load i32, ptr %12, align 4, !dbg !1258
  %45 = sext i32 %44 to i64, !dbg !1258
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1258
  store i8 %43, ptr %46, align 8, !dbg !1258
  br label %101, !dbg !1258

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1258
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1258
  store ptr %49, ptr %5, align 8, !dbg !1258
  %50 = load i32, ptr %48, align 8, !dbg !1258
  %51 = trunc i32 %50 to i16, !dbg !1258
  %52 = load i32, ptr %12, align 4, !dbg !1258
  %53 = sext i32 %52 to i64, !dbg !1258
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1258
  store i16 %51, ptr %54, align 8, !dbg !1258
  br label %101, !dbg !1258

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1258
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1258
  store ptr %57, ptr %5, align 8, !dbg !1258
  %58 = load i32, ptr %56, align 8, !dbg !1258
  %59 = trunc i32 %58 to i16, !dbg !1258
  %60 = load i32, ptr %12, align 4, !dbg !1258
  %61 = sext i32 %60 to i64, !dbg !1258
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1258
  store i16 %59, ptr %62, align 8, !dbg !1258
  br label %101, !dbg !1258

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1258
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1258
  store ptr %65, ptr %5, align 8, !dbg !1258
  %66 = load i32, ptr %64, align 8, !dbg !1258
  %67 = load i32, ptr %12, align 4, !dbg !1258
  %68 = sext i32 %67 to i64, !dbg !1258
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1258
  store i32 %66, ptr %69, align 8, !dbg !1258
  br label %101, !dbg !1258

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1258
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1258
  store ptr %72, ptr %5, align 8, !dbg !1258
  %73 = load i32, ptr %71, align 8, !dbg !1258
  %74 = sext i32 %73 to i64, !dbg !1258
  %75 = load i32, ptr %12, align 4, !dbg !1258
  %76 = sext i32 %75 to i64, !dbg !1258
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1258
  store i64 %74, ptr %77, align 8, !dbg !1258
  br label %101, !dbg !1258

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1258
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1258
  store ptr %80, ptr %5, align 8, !dbg !1258
  %81 = load double, ptr %79, align 8, !dbg !1258
  %82 = fptrunc double %81 to float, !dbg !1258
  %83 = load i32, ptr %12, align 4, !dbg !1258
  %84 = sext i32 %83 to i64, !dbg !1258
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1258
  store float %82, ptr %85, align 8, !dbg !1258
  br label %101, !dbg !1258

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1258
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1258
  store ptr %88, ptr %5, align 8, !dbg !1258
  %89 = load double, ptr %87, align 8, !dbg !1258
  %90 = load i32, ptr %12, align 4, !dbg !1258
  %91 = sext i32 %90 to i64, !dbg !1258
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1258
  store double %89, ptr %92, align 8, !dbg !1258
  br label %101, !dbg !1258

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1258
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1258
  store ptr %95, ptr %5, align 8, !dbg !1258
  %96 = load ptr, ptr %94, align 8, !dbg !1258
  %97 = load i32, ptr %12, align 4, !dbg !1258
  %98 = sext i32 %97 to i64, !dbg !1258
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1258
  store ptr %96, ptr %99, align 8, !dbg !1258
  br label %101, !dbg !1258

100:                                              ; preds = %25
  br label %101, !dbg !1258

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1255

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1260
  %104 = add nsw i32 %103, 1, !dbg !1260
  store i32 %104, ptr %12, align 4, !dbg !1260
  br label %21, !dbg !1260, !llvm.loop !1261

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1245
  %107 = load ptr, ptr %106, align 8, !dbg !1245
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 119, !dbg !1245
  %109 = load ptr, ptr %108, align 8, !dbg !1245
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1245
  %111 = load ptr, ptr %6, align 8, !dbg !1245
  %112 = load ptr, ptr %7, align 8, !dbg !1245
  %113 = load ptr, ptr %8, align 8, !dbg !1245
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1245
  ret i8 %114, !dbg !1245
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1262 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1263, metadata !DIExpression()), !dbg !1264
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1265, metadata !DIExpression()), !dbg !1264
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1266, metadata !DIExpression()), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1267, metadata !DIExpression()), !dbg !1264
  call void @llvm.va_start(ptr %7), !dbg !1264
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1268, metadata !DIExpression()), !dbg !1264
  %9 = load ptr, ptr %7, align 8, !dbg !1264
  %10 = load ptr, ptr %4, align 8, !dbg !1264
  %11 = load ptr, ptr %5, align 8, !dbg !1264
  %12 = load ptr, ptr %6, align 8, !dbg !1264
  %13 = call i8 @JNI_CallByteMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1264
  store i8 %13, ptr %8, align 1, !dbg !1264
  call void @llvm.va_end(ptr %7), !dbg !1264
  %14 = load i8, ptr %8, align 1, !dbg !1264
  ret i8 %14, !dbg !1264
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1269 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1270, metadata !DIExpression()), !dbg !1271
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1272, metadata !DIExpression()), !dbg !1271
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1273, metadata !DIExpression()), !dbg !1271
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1274, metadata !DIExpression()), !dbg !1271
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1275, metadata !DIExpression()), !dbg !1271
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1276, metadata !DIExpression()), !dbg !1271
  %13 = load ptr, ptr %8, align 8, !dbg !1271
  %14 = load ptr, ptr %13, align 8, !dbg !1271
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1271
  %16 = load ptr, ptr %15, align 8, !dbg !1271
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1271
  %18 = load ptr, ptr %6, align 8, !dbg !1271
  %19 = load ptr, ptr %8, align 8, !dbg !1271
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1271
  store i32 %20, ptr %10, align 4, !dbg !1271
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1277, metadata !DIExpression()), !dbg !1271
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1278, metadata !DIExpression()), !dbg !1280
  store i32 0, ptr %12, align 4, !dbg !1280
  br label %21, !dbg !1280

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1280
  %23 = load i32, ptr %10, align 4, !dbg !1280
  %24 = icmp slt i32 %22, %23, !dbg !1280
  br i1 %24, label %25, label %105, !dbg !1280

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1281
  %27 = sext i32 %26 to i64, !dbg !1281
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1281
  %29 = load i8, ptr %28, align 1, !dbg !1281
  %30 = sext i8 %29 to i32, !dbg !1281
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1281

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1284
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1284
  store ptr %33, ptr %5, align 8, !dbg !1284
  %34 = load i32, ptr %32, align 8, !dbg !1284
  %35 = trunc i32 %34 to i8, !dbg !1284
  %36 = load i32, ptr %12, align 4, !dbg !1284
  %37 = sext i32 %36 to i64, !dbg !1284
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1284
  store i8 %35, ptr %38, align 8, !dbg !1284
  br label %101, !dbg !1284

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1284
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1284
  store ptr %41, ptr %5, align 8, !dbg !1284
  %42 = load i32, ptr %40, align 8, !dbg !1284
  %43 = trunc i32 %42 to i8, !dbg !1284
  %44 = load i32, ptr %12, align 4, !dbg !1284
  %45 = sext i32 %44 to i64, !dbg !1284
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1284
  store i8 %43, ptr %46, align 8, !dbg !1284
  br label %101, !dbg !1284

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1284
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1284
  store ptr %49, ptr %5, align 8, !dbg !1284
  %50 = load i32, ptr %48, align 8, !dbg !1284
  %51 = trunc i32 %50 to i16, !dbg !1284
  %52 = load i32, ptr %12, align 4, !dbg !1284
  %53 = sext i32 %52 to i64, !dbg !1284
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1284
  store i16 %51, ptr %54, align 8, !dbg !1284
  br label %101, !dbg !1284

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1284
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1284
  store ptr %57, ptr %5, align 8, !dbg !1284
  %58 = load i32, ptr %56, align 8, !dbg !1284
  %59 = trunc i32 %58 to i16, !dbg !1284
  %60 = load i32, ptr %12, align 4, !dbg !1284
  %61 = sext i32 %60 to i64, !dbg !1284
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1284
  store i16 %59, ptr %62, align 8, !dbg !1284
  br label %101, !dbg !1284

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1284
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1284
  store ptr %65, ptr %5, align 8, !dbg !1284
  %66 = load i32, ptr %64, align 8, !dbg !1284
  %67 = load i32, ptr %12, align 4, !dbg !1284
  %68 = sext i32 %67 to i64, !dbg !1284
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1284
  store i32 %66, ptr %69, align 8, !dbg !1284
  br label %101, !dbg !1284

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1284
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1284
  store ptr %72, ptr %5, align 8, !dbg !1284
  %73 = load i32, ptr %71, align 8, !dbg !1284
  %74 = sext i32 %73 to i64, !dbg !1284
  %75 = load i32, ptr %12, align 4, !dbg !1284
  %76 = sext i32 %75 to i64, !dbg !1284
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1284
  store i64 %74, ptr %77, align 8, !dbg !1284
  br label %101, !dbg !1284

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1284
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1284
  store ptr %80, ptr %5, align 8, !dbg !1284
  %81 = load double, ptr %79, align 8, !dbg !1284
  %82 = fptrunc double %81 to float, !dbg !1284
  %83 = load i32, ptr %12, align 4, !dbg !1284
  %84 = sext i32 %83 to i64, !dbg !1284
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1284
  store float %82, ptr %85, align 8, !dbg !1284
  br label %101, !dbg !1284

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1284
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1284
  store ptr %88, ptr %5, align 8, !dbg !1284
  %89 = load double, ptr %87, align 8, !dbg !1284
  %90 = load i32, ptr %12, align 4, !dbg !1284
  %91 = sext i32 %90 to i64, !dbg !1284
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1284
  store double %89, ptr %92, align 8, !dbg !1284
  br label %101, !dbg !1284

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1284
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1284
  store ptr %95, ptr %5, align 8, !dbg !1284
  %96 = load ptr, ptr %94, align 8, !dbg !1284
  %97 = load i32, ptr %12, align 4, !dbg !1284
  %98 = sext i32 %97 to i64, !dbg !1284
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1284
  store ptr %96, ptr %99, align 8, !dbg !1284
  br label %101, !dbg !1284

100:                                              ; preds = %25
  br label %101, !dbg !1284

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1281

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1286
  %104 = add nsw i32 %103, 1, !dbg !1286
  store i32 %104, ptr %12, align 4, !dbg !1286
  br label %21, !dbg !1286, !llvm.loop !1287

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1271
  %107 = load ptr, ptr %106, align 8, !dbg !1271
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 42, !dbg !1271
  %109 = load ptr, ptr %108, align 8, !dbg !1271
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1271
  %111 = load ptr, ptr %6, align 8, !dbg !1271
  %112 = load ptr, ptr %7, align 8, !dbg !1271
  %113 = load ptr, ptr %8, align 8, !dbg !1271
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1271
  ret i8 %114, !dbg !1271
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1288 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1289, metadata !DIExpression()), !dbg !1290
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1291, metadata !DIExpression()), !dbg !1290
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1292, metadata !DIExpression()), !dbg !1290
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1293, metadata !DIExpression()), !dbg !1290
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1294, metadata !DIExpression()), !dbg !1290
  call void @llvm.va_start(ptr %9), !dbg !1290
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1295, metadata !DIExpression()), !dbg !1290
  %11 = load ptr, ptr %9, align 8, !dbg !1290
  %12 = load ptr, ptr %5, align 8, !dbg !1290
  %13 = load ptr, ptr %6, align 8, !dbg !1290
  %14 = load ptr, ptr %7, align 8, !dbg !1290
  %15 = load ptr, ptr %8, align 8, !dbg !1290
  %16 = call i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1290
  store i8 %16, ptr %10, align 1, !dbg !1290
  call void @llvm.va_end(ptr %9), !dbg !1290
  %17 = load i8, ptr %10, align 1, !dbg !1290
  ret i8 %17, !dbg !1290
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1296 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1297, metadata !DIExpression()), !dbg !1298
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1299, metadata !DIExpression()), !dbg !1298
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1300, metadata !DIExpression()), !dbg !1298
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1301, metadata !DIExpression()), !dbg !1298
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1302, metadata !DIExpression()), !dbg !1298
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1303, metadata !DIExpression()), !dbg !1298
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1304, metadata !DIExpression()), !dbg !1298
  %15 = load ptr, ptr %10, align 8, !dbg !1298
  %16 = load ptr, ptr %15, align 8, !dbg !1298
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1298
  %18 = load ptr, ptr %17, align 8, !dbg !1298
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1298
  %20 = load ptr, ptr %7, align 8, !dbg !1298
  %21 = load ptr, ptr %10, align 8, !dbg !1298
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1298
  store i32 %22, ptr %12, align 4, !dbg !1298
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1305, metadata !DIExpression()), !dbg !1298
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1306, metadata !DIExpression()), !dbg !1308
  store i32 0, ptr %14, align 4, !dbg !1308
  br label %23, !dbg !1308

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1308
  %25 = load i32, ptr %12, align 4, !dbg !1308
  %26 = icmp slt i32 %24, %25, !dbg !1308
  br i1 %26, label %27, label %107, !dbg !1308

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1309
  %29 = sext i32 %28 to i64, !dbg !1309
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1309
  %31 = load i8, ptr %30, align 1, !dbg !1309
  %32 = sext i8 %31 to i32, !dbg !1309
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1309

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1312
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1312
  store ptr %35, ptr %6, align 8, !dbg !1312
  %36 = load i32, ptr %34, align 8, !dbg !1312
  %37 = trunc i32 %36 to i8, !dbg !1312
  %38 = load i32, ptr %14, align 4, !dbg !1312
  %39 = sext i32 %38 to i64, !dbg !1312
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1312
  store i8 %37, ptr %40, align 8, !dbg !1312
  br label %103, !dbg !1312

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1312
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1312
  store ptr %43, ptr %6, align 8, !dbg !1312
  %44 = load i32, ptr %42, align 8, !dbg !1312
  %45 = trunc i32 %44 to i8, !dbg !1312
  %46 = load i32, ptr %14, align 4, !dbg !1312
  %47 = sext i32 %46 to i64, !dbg !1312
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1312
  store i8 %45, ptr %48, align 8, !dbg !1312
  br label %103, !dbg !1312

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1312
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1312
  store ptr %51, ptr %6, align 8, !dbg !1312
  %52 = load i32, ptr %50, align 8, !dbg !1312
  %53 = trunc i32 %52 to i16, !dbg !1312
  %54 = load i32, ptr %14, align 4, !dbg !1312
  %55 = sext i32 %54 to i64, !dbg !1312
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1312
  store i16 %53, ptr %56, align 8, !dbg !1312
  br label %103, !dbg !1312

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1312
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1312
  store ptr %59, ptr %6, align 8, !dbg !1312
  %60 = load i32, ptr %58, align 8, !dbg !1312
  %61 = trunc i32 %60 to i16, !dbg !1312
  %62 = load i32, ptr %14, align 4, !dbg !1312
  %63 = sext i32 %62 to i64, !dbg !1312
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1312
  store i16 %61, ptr %64, align 8, !dbg !1312
  br label %103, !dbg !1312

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1312
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1312
  store ptr %67, ptr %6, align 8, !dbg !1312
  %68 = load i32, ptr %66, align 8, !dbg !1312
  %69 = load i32, ptr %14, align 4, !dbg !1312
  %70 = sext i32 %69 to i64, !dbg !1312
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1312
  store i32 %68, ptr %71, align 8, !dbg !1312
  br label %103, !dbg !1312

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1312
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1312
  store ptr %74, ptr %6, align 8, !dbg !1312
  %75 = load i32, ptr %73, align 8, !dbg !1312
  %76 = sext i32 %75 to i64, !dbg !1312
  %77 = load i32, ptr %14, align 4, !dbg !1312
  %78 = sext i32 %77 to i64, !dbg !1312
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1312
  store i64 %76, ptr %79, align 8, !dbg !1312
  br label %103, !dbg !1312

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1312
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1312
  store ptr %82, ptr %6, align 8, !dbg !1312
  %83 = load double, ptr %81, align 8, !dbg !1312
  %84 = fptrunc double %83 to float, !dbg !1312
  %85 = load i32, ptr %14, align 4, !dbg !1312
  %86 = sext i32 %85 to i64, !dbg !1312
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1312
  store float %84, ptr %87, align 8, !dbg !1312
  br label %103, !dbg !1312

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1312
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1312
  store ptr %90, ptr %6, align 8, !dbg !1312
  %91 = load double, ptr %89, align 8, !dbg !1312
  %92 = load i32, ptr %14, align 4, !dbg !1312
  %93 = sext i32 %92 to i64, !dbg !1312
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1312
  store double %91, ptr %94, align 8, !dbg !1312
  br label %103, !dbg !1312

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1312
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1312
  store ptr %97, ptr %6, align 8, !dbg !1312
  %98 = load ptr, ptr %96, align 8, !dbg !1312
  %99 = load i32, ptr %14, align 4, !dbg !1312
  %100 = sext i32 %99 to i64, !dbg !1312
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1312
  store ptr %98, ptr %101, align 8, !dbg !1312
  br label %103, !dbg !1312

102:                                              ; preds = %27
  br label %103, !dbg !1312

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1309

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1314
  %106 = add nsw i32 %105, 1, !dbg !1314
  store i32 %106, ptr %14, align 4, !dbg !1314
  br label %23, !dbg !1314, !llvm.loop !1315

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1298
  %109 = load ptr, ptr %108, align 8, !dbg !1298
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 72, !dbg !1298
  %111 = load ptr, ptr %110, align 8, !dbg !1298
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1298
  %113 = load ptr, ptr %7, align 8, !dbg !1298
  %114 = load ptr, ptr %8, align 8, !dbg !1298
  %115 = load ptr, ptr %9, align 8, !dbg !1298
  %116 = load ptr, ptr %10, align 8, !dbg !1298
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1298
  ret i8 %117, !dbg !1298
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1316 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1317, metadata !DIExpression()), !dbg !1318
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1319, metadata !DIExpression()), !dbg !1318
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1320, metadata !DIExpression()), !dbg !1318
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1321, metadata !DIExpression()), !dbg !1318
  call void @llvm.va_start(ptr %7), !dbg !1318
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1322, metadata !DIExpression()), !dbg !1318
  %9 = load ptr, ptr %7, align 8, !dbg !1318
  %10 = load ptr, ptr %4, align 8, !dbg !1318
  %11 = load ptr, ptr %5, align 8, !dbg !1318
  %12 = load ptr, ptr %6, align 8, !dbg !1318
  %13 = call i8 @JNI_CallStaticByteMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1318
  store i8 %13, ptr %8, align 1, !dbg !1318
  call void @llvm.va_end(ptr %7), !dbg !1318
  %14 = load i8, ptr %8, align 1, !dbg !1318
  ret i8 %14, !dbg !1318
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1323 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1324, metadata !DIExpression()), !dbg !1325
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1326, metadata !DIExpression()), !dbg !1325
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1327, metadata !DIExpression()), !dbg !1325
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1328, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1329, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1330, metadata !DIExpression()), !dbg !1325
  %13 = load ptr, ptr %8, align 8, !dbg !1325
  %14 = load ptr, ptr %13, align 8, !dbg !1325
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1325
  %16 = load ptr, ptr %15, align 8, !dbg !1325
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1325
  %18 = load ptr, ptr %6, align 8, !dbg !1325
  %19 = load ptr, ptr %8, align 8, !dbg !1325
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1325
  store i32 %20, ptr %10, align 4, !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1331, metadata !DIExpression()), !dbg !1325
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1332, metadata !DIExpression()), !dbg !1334
  store i32 0, ptr %12, align 4, !dbg !1334
  br label %21, !dbg !1334

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1334
  %23 = load i32, ptr %10, align 4, !dbg !1334
  %24 = icmp slt i32 %22, %23, !dbg !1334
  br i1 %24, label %25, label %105, !dbg !1334

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1335
  %27 = sext i32 %26 to i64, !dbg !1335
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1335
  %29 = load i8, ptr %28, align 1, !dbg !1335
  %30 = sext i8 %29 to i32, !dbg !1335
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1335

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1338
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1338
  store ptr %33, ptr %5, align 8, !dbg !1338
  %34 = load i32, ptr %32, align 8, !dbg !1338
  %35 = trunc i32 %34 to i8, !dbg !1338
  %36 = load i32, ptr %12, align 4, !dbg !1338
  %37 = sext i32 %36 to i64, !dbg !1338
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1338
  store i8 %35, ptr %38, align 8, !dbg !1338
  br label %101, !dbg !1338

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1338
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1338
  store ptr %41, ptr %5, align 8, !dbg !1338
  %42 = load i32, ptr %40, align 8, !dbg !1338
  %43 = trunc i32 %42 to i8, !dbg !1338
  %44 = load i32, ptr %12, align 4, !dbg !1338
  %45 = sext i32 %44 to i64, !dbg !1338
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1338
  store i8 %43, ptr %46, align 8, !dbg !1338
  br label %101, !dbg !1338

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1338
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1338
  store ptr %49, ptr %5, align 8, !dbg !1338
  %50 = load i32, ptr %48, align 8, !dbg !1338
  %51 = trunc i32 %50 to i16, !dbg !1338
  %52 = load i32, ptr %12, align 4, !dbg !1338
  %53 = sext i32 %52 to i64, !dbg !1338
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1338
  store i16 %51, ptr %54, align 8, !dbg !1338
  br label %101, !dbg !1338

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1338
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1338
  store ptr %57, ptr %5, align 8, !dbg !1338
  %58 = load i32, ptr %56, align 8, !dbg !1338
  %59 = trunc i32 %58 to i16, !dbg !1338
  %60 = load i32, ptr %12, align 4, !dbg !1338
  %61 = sext i32 %60 to i64, !dbg !1338
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1338
  store i16 %59, ptr %62, align 8, !dbg !1338
  br label %101, !dbg !1338

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1338
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1338
  store ptr %65, ptr %5, align 8, !dbg !1338
  %66 = load i32, ptr %64, align 8, !dbg !1338
  %67 = load i32, ptr %12, align 4, !dbg !1338
  %68 = sext i32 %67 to i64, !dbg !1338
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1338
  store i32 %66, ptr %69, align 8, !dbg !1338
  br label %101, !dbg !1338

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1338
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1338
  store ptr %72, ptr %5, align 8, !dbg !1338
  %73 = load i32, ptr %71, align 8, !dbg !1338
  %74 = sext i32 %73 to i64, !dbg !1338
  %75 = load i32, ptr %12, align 4, !dbg !1338
  %76 = sext i32 %75 to i64, !dbg !1338
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1338
  store i64 %74, ptr %77, align 8, !dbg !1338
  br label %101, !dbg !1338

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1338
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1338
  store ptr %80, ptr %5, align 8, !dbg !1338
  %81 = load double, ptr %79, align 8, !dbg !1338
  %82 = fptrunc double %81 to float, !dbg !1338
  %83 = load i32, ptr %12, align 4, !dbg !1338
  %84 = sext i32 %83 to i64, !dbg !1338
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1338
  store float %82, ptr %85, align 8, !dbg !1338
  br label %101, !dbg !1338

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1338
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1338
  store ptr %88, ptr %5, align 8, !dbg !1338
  %89 = load double, ptr %87, align 8, !dbg !1338
  %90 = load i32, ptr %12, align 4, !dbg !1338
  %91 = sext i32 %90 to i64, !dbg !1338
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1338
  store double %89, ptr %92, align 8, !dbg !1338
  br label %101, !dbg !1338

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1338
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1338
  store ptr %95, ptr %5, align 8, !dbg !1338
  %96 = load ptr, ptr %94, align 8, !dbg !1338
  %97 = load i32, ptr %12, align 4, !dbg !1338
  %98 = sext i32 %97 to i64, !dbg !1338
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1338
  store ptr %96, ptr %99, align 8, !dbg !1338
  br label %101, !dbg !1338

100:                                              ; preds = %25
  br label %101, !dbg !1338

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1335

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1340
  %104 = add nsw i32 %103, 1, !dbg !1340
  store i32 %104, ptr %12, align 4, !dbg !1340
  br label %21, !dbg !1340, !llvm.loop !1341

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1325
  %107 = load ptr, ptr %106, align 8, !dbg !1325
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 122, !dbg !1325
  %109 = load ptr, ptr %108, align 8, !dbg !1325
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1325
  %111 = load ptr, ptr %6, align 8, !dbg !1325
  %112 = load ptr, ptr %7, align 8, !dbg !1325
  %113 = load ptr, ptr %8, align 8, !dbg !1325
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1325
  ret i8 %114, !dbg !1325
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1342 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1343, metadata !DIExpression()), !dbg !1344
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1345, metadata !DIExpression()), !dbg !1344
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1346, metadata !DIExpression()), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1347, metadata !DIExpression()), !dbg !1344
  call void @llvm.va_start(ptr %7), !dbg !1344
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1348, metadata !DIExpression()), !dbg !1344
  %9 = load ptr, ptr %7, align 8, !dbg !1344
  %10 = load ptr, ptr %4, align 8, !dbg !1344
  %11 = load ptr, ptr %5, align 8, !dbg !1344
  %12 = load ptr, ptr %6, align 8, !dbg !1344
  %13 = call i16 @JNI_CallCharMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1344
  store i16 %13, ptr %8, align 2, !dbg !1344
  call void @llvm.va_end(ptr %7), !dbg !1344
  %14 = load i16, ptr %8, align 2, !dbg !1344
  ret i16 %14, !dbg !1344
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1349 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1350, metadata !DIExpression()), !dbg !1351
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1352, metadata !DIExpression()), !dbg !1351
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1353, metadata !DIExpression()), !dbg !1351
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1354, metadata !DIExpression()), !dbg !1351
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1355, metadata !DIExpression()), !dbg !1351
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1356, metadata !DIExpression()), !dbg !1351
  %13 = load ptr, ptr %8, align 8, !dbg !1351
  %14 = load ptr, ptr %13, align 8, !dbg !1351
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1351
  %16 = load ptr, ptr %15, align 8, !dbg !1351
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1351
  %18 = load ptr, ptr %6, align 8, !dbg !1351
  %19 = load ptr, ptr %8, align 8, !dbg !1351
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1351
  store i32 %20, ptr %10, align 4, !dbg !1351
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1357, metadata !DIExpression()), !dbg !1351
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1358, metadata !DIExpression()), !dbg !1360
  store i32 0, ptr %12, align 4, !dbg !1360
  br label %21, !dbg !1360

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1360
  %23 = load i32, ptr %10, align 4, !dbg !1360
  %24 = icmp slt i32 %22, %23, !dbg !1360
  br i1 %24, label %25, label %105, !dbg !1360

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1361
  %27 = sext i32 %26 to i64, !dbg !1361
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1361
  %29 = load i8, ptr %28, align 1, !dbg !1361
  %30 = sext i8 %29 to i32, !dbg !1361
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1361

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1364
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1364
  store ptr %33, ptr %5, align 8, !dbg !1364
  %34 = load i32, ptr %32, align 8, !dbg !1364
  %35 = trunc i32 %34 to i8, !dbg !1364
  %36 = load i32, ptr %12, align 4, !dbg !1364
  %37 = sext i32 %36 to i64, !dbg !1364
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1364
  store i8 %35, ptr %38, align 8, !dbg !1364
  br label %101, !dbg !1364

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1364
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1364
  store ptr %41, ptr %5, align 8, !dbg !1364
  %42 = load i32, ptr %40, align 8, !dbg !1364
  %43 = trunc i32 %42 to i8, !dbg !1364
  %44 = load i32, ptr %12, align 4, !dbg !1364
  %45 = sext i32 %44 to i64, !dbg !1364
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1364
  store i8 %43, ptr %46, align 8, !dbg !1364
  br label %101, !dbg !1364

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1364
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1364
  store ptr %49, ptr %5, align 8, !dbg !1364
  %50 = load i32, ptr %48, align 8, !dbg !1364
  %51 = trunc i32 %50 to i16, !dbg !1364
  %52 = load i32, ptr %12, align 4, !dbg !1364
  %53 = sext i32 %52 to i64, !dbg !1364
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1364
  store i16 %51, ptr %54, align 8, !dbg !1364
  br label %101, !dbg !1364

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1364
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1364
  store ptr %57, ptr %5, align 8, !dbg !1364
  %58 = load i32, ptr %56, align 8, !dbg !1364
  %59 = trunc i32 %58 to i16, !dbg !1364
  %60 = load i32, ptr %12, align 4, !dbg !1364
  %61 = sext i32 %60 to i64, !dbg !1364
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1364
  store i16 %59, ptr %62, align 8, !dbg !1364
  br label %101, !dbg !1364

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1364
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1364
  store ptr %65, ptr %5, align 8, !dbg !1364
  %66 = load i32, ptr %64, align 8, !dbg !1364
  %67 = load i32, ptr %12, align 4, !dbg !1364
  %68 = sext i32 %67 to i64, !dbg !1364
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1364
  store i32 %66, ptr %69, align 8, !dbg !1364
  br label %101, !dbg !1364

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1364
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1364
  store ptr %72, ptr %5, align 8, !dbg !1364
  %73 = load i32, ptr %71, align 8, !dbg !1364
  %74 = sext i32 %73 to i64, !dbg !1364
  %75 = load i32, ptr %12, align 4, !dbg !1364
  %76 = sext i32 %75 to i64, !dbg !1364
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1364
  store i64 %74, ptr %77, align 8, !dbg !1364
  br label %101, !dbg !1364

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1364
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1364
  store ptr %80, ptr %5, align 8, !dbg !1364
  %81 = load double, ptr %79, align 8, !dbg !1364
  %82 = fptrunc double %81 to float, !dbg !1364
  %83 = load i32, ptr %12, align 4, !dbg !1364
  %84 = sext i32 %83 to i64, !dbg !1364
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1364
  store float %82, ptr %85, align 8, !dbg !1364
  br label %101, !dbg !1364

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1364
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1364
  store ptr %88, ptr %5, align 8, !dbg !1364
  %89 = load double, ptr %87, align 8, !dbg !1364
  %90 = load i32, ptr %12, align 4, !dbg !1364
  %91 = sext i32 %90 to i64, !dbg !1364
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1364
  store double %89, ptr %92, align 8, !dbg !1364
  br label %101, !dbg !1364

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1364
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1364
  store ptr %95, ptr %5, align 8, !dbg !1364
  %96 = load ptr, ptr %94, align 8, !dbg !1364
  %97 = load i32, ptr %12, align 4, !dbg !1364
  %98 = sext i32 %97 to i64, !dbg !1364
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1364
  store ptr %96, ptr %99, align 8, !dbg !1364
  br label %101, !dbg !1364

100:                                              ; preds = %25
  br label %101, !dbg !1364

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1361

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1366
  %104 = add nsw i32 %103, 1, !dbg !1366
  store i32 %104, ptr %12, align 4, !dbg !1366
  br label %21, !dbg !1366, !llvm.loop !1367

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1351
  %107 = load ptr, ptr %106, align 8, !dbg !1351
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 45, !dbg !1351
  %109 = load ptr, ptr %108, align 8, !dbg !1351
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1351
  %111 = load ptr, ptr %6, align 8, !dbg !1351
  %112 = load ptr, ptr %7, align 8, !dbg !1351
  %113 = load ptr, ptr %8, align 8, !dbg !1351
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1351
  ret i16 %114, !dbg !1351
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1368 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1369, metadata !DIExpression()), !dbg !1370
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1371, metadata !DIExpression()), !dbg !1370
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1372, metadata !DIExpression()), !dbg !1370
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1373, metadata !DIExpression()), !dbg !1370
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1374, metadata !DIExpression()), !dbg !1370
  call void @llvm.va_start(ptr %9), !dbg !1370
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1375, metadata !DIExpression()), !dbg !1370
  %11 = load ptr, ptr %9, align 8, !dbg !1370
  %12 = load ptr, ptr %5, align 8, !dbg !1370
  %13 = load ptr, ptr %6, align 8, !dbg !1370
  %14 = load ptr, ptr %7, align 8, !dbg !1370
  %15 = load ptr, ptr %8, align 8, !dbg !1370
  %16 = call i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1370
  store i16 %16, ptr %10, align 2, !dbg !1370
  call void @llvm.va_end(ptr %9), !dbg !1370
  %17 = load i16, ptr %10, align 2, !dbg !1370
  ret i16 %17, !dbg !1370
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1376 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1377, metadata !DIExpression()), !dbg !1378
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1379, metadata !DIExpression()), !dbg !1378
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1380, metadata !DIExpression()), !dbg !1378
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1381, metadata !DIExpression()), !dbg !1378
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1382, metadata !DIExpression()), !dbg !1378
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1383, metadata !DIExpression()), !dbg !1378
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1384, metadata !DIExpression()), !dbg !1378
  %15 = load ptr, ptr %10, align 8, !dbg !1378
  %16 = load ptr, ptr %15, align 8, !dbg !1378
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1378
  %18 = load ptr, ptr %17, align 8, !dbg !1378
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1378
  %20 = load ptr, ptr %7, align 8, !dbg !1378
  %21 = load ptr, ptr %10, align 8, !dbg !1378
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1378
  store i32 %22, ptr %12, align 4, !dbg !1378
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1385, metadata !DIExpression()), !dbg !1378
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1386, metadata !DIExpression()), !dbg !1388
  store i32 0, ptr %14, align 4, !dbg !1388
  br label %23, !dbg !1388

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1388
  %25 = load i32, ptr %12, align 4, !dbg !1388
  %26 = icmp slt i32 %24, %25, !dbg !1388
  br i1 %26, label %27, label %107, !dbg !1388

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1389
  %29 = sext i32 %28 to i64, !dbg !1389
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1389
  %31 = load i8, ptr %30, align 1, !dbg !1389
  %32 = sext i8 %31 to i32, !dbg !1389
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1389

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1392
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1392
  store ptr %35, ptr %6, align 8, !dbg !1392
  %36 = load i32, ptr %34, align 8, !dbg !1392
  %37 = trunc i32 %36 to i8, !dbg !1392
  %38 = load i32, ptr %14, align 4, !dbg !1392
  %39 = sext i32 %38 to i64, !dbg !1392
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1392
  store i8 %37, ptr %40, align 8, !dbg !1392
  br label %103, !dbg !1392

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1392
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1392
  store ptr %43, ptr %6, align 8, !dbg !1392
  %44 = load i32, ptr %42, align 8, !dbg !1392
  %45 = trunc i32 %44 to i8, !dbg !1392
  %46 = load i32, ptr %14, align 4, !dbg !1392
  %47 = sext i32 %46 to i64, !dbg !1392
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1392
  store i8 %45, ptr %48, align 8, !dbg !1392
  br label %103, !dbg !1392

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1392
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1392
  store ptr %51, ptr %6, align 8, !dbg !1392
  %52 = load i32, ptr %50, align 8, !dbg !1392
  %53 = trunc i32 %52 to i16, !dbg !1392
  %54 = load i32, ptr %14, align 4, !dbg !1392
  %55 = sext i32 %54 to i64, !dbg !1392
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1392
  store i16 %53, ptr %56, align 8, !dbg !1392
  br label %103, !dbg !1392

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1392
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1392
  store ptr %59, ptr %6, align 8, !dbg !1392
  %60 = load i32, ptr %58, align 8, !dbg !1392
  %61 = trunc i32 %60 to i16, !dbg !1392
  %62 = load i32, ptr %14, align 4, !dbg !1392
  %63 = sext i32 %62 to i64, !dbg !1392
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1392
  store i16 %61, ptr %64, align 8, !dbg !1392
  br label %103, !dbg !1392

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1392
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1392
  store ptr %67, ptr %6, align 8, !dbg !1392
  %68 = load i32, ptr %66, align 8, !dbg !1392
  %69 = load i32, ptr %14, align 4, !dbg !1392
  %70 = sext i32 %69 to i64, !dbg !1392
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1392
  store i32 %68, ptr %71, align 8, !dbg !1392
  br label %103, !dbg !1392

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1392
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1392
  store ptr %74, ptr %6, align 8, !dbg !1392
  %75 = load i32, ptr %73, align 8, !dbg !1392
  %76 = sext i32 %75 to i64, !dbg !1392
  %77 = load i32, ptr %14, align 4, !dbg !1392
  %78 = sext i32 %77 to i64, !dbg !1392
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1392
  store i64 %76, ptr %79, align 8, !dbg !1392
  br label %103, !dbg !1392

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1392
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1392
  store ptr %82, ptr %6, align 8, !dbg !1392
  %83 = load double, ptr %81, align 8, !dbg !1392
  %84 = fptrunc double %83 to float, !dbg !1392
  %85 = load i32, ptr %14, align 4, !dbg !1392
  %86 = sext i32 %85 to i64, !dbg !1392
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1392
  store float %84, ptr %87, align 8, !dbg !1392
  br label %103, !dbg !1392

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1392
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1392
  store ptr %90, ptr %6, align 8, !dbg !1392
  %91 = load double, ptr %89, align 8, !dbg !1392
  %92 = load i32, ptr %14, align 4, !dbg !1392
  %93 = sext i32 %92 to i64, !dbg !1392
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1392
  store double %91, ptr %94, align 8, !dbg !1392
  br label %103, !dbg !1392

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1392
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1392
  store ptr %97, ptr %6, align 8, !dbg !1392
  %98 = load ptr, ptr %96, align 8, !dbg !1392
  %99 = load i32, ptr %14, align 4, !dbg !1392
  %100 = sext i32 %99 to i64, !dbg !1392
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1392
  store ptr %98, ptr %101, align 8, !dbg !1392
  br label %103, !dbg !1392

102:                                              ; preds = %27
  br label %103, !dbg !1392

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1389

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1394
  %106 = add nsw i32 %105, 1, !dbg !1394
  store i32 %106, ptr %14, align 4, !dbg !1394
  br label %23, !dbg !1394, !llvm.loop !1395

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1378
  %109 = load ptr, ptr %108, align 8, !dbg !1378
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 75, !dbg !1378
  %111 = load ptr, ptr %110, align 8, !dbg !1378
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1378
  %113 = load ptr, ptr %7, align 8, !dbg !1378
  %114 = load ptr, ptr %8, align 8, !dbg !1378
  %115 = load ptr, ptr %9, align 8, !dbg !1378
  %116 = load ptr, ptr %10, align 8, !dbg !1378
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1378
  ret i16 %117, !dbg !1378
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1396 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1397, metadata !DIExpression()), !dbg !1398
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1399, metadata !DIExpression()), !dbg !1398
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1400, metadata !DIExpression()), !dbg !1398
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1401, metadata !DIExpression()), !dbg !1398
  call void @llvm.va_start(ptr %7), !dbg !1398
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1402, metadata !DIExpression()), !dbg !1398
  %9 = load ptr, ptr %7, align 8, !dbg !1398
  %10 = load ptr, ptr %4, align 8, !dbg !1398
  %11 = load ptr, ptr %5, align 8, !dbg !1398
  %12 = load ptr, ptr %6, align 8, !dbg !1398
  %13 = call i16 @JNI_CallStaticCharMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1398
  store i16 %13, ptr %8, align 2, !dbg !1398
  call void @llvm.va_end(ptr %7), !dbg !1398
  %14 = load i16, ptr %8, align 2, !dbg !1398
  ret i16 %14, !dbg !1398
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1403 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1404, metadata !DIExpression()), !dbg !1405
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1406, metadata !DIExpression()), !dbg !1405
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1407, metadata !DIExpression()), !dbg !1405
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1408, metadata !DIExpression()), !dbg !1405
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1409, metadata !DIExpression()), !dbg !1405
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1410, metadata !DIExpression()), !dbg !1405
  %13 = load ptr, ptr %8, align 8, !dbg !1405
  %14 = load ptr, ptr %13, align 8, !dbg !1405
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1405
  %16 = load ptr, ptr %15, align 8, !dbg !1405
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1405
  %18 = load ptr, ptr %6, align 8, !dbg !1405
  %19 = load ptr, ptr %8, align 8, !dbg !1405
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1405
  store i32 %20, ptr %10, align 4, !dbg !1405
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1411, metadata !DIExpression()), !dbg !1405
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1412, metadata !DIExpression()), !dbg !1414
  store i32 0, ptr %12, align 4, !dbg !1414
  br label %21, !dbg !1414

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1414
  %23 = load i32, ptr %10, align 4, !dbg !1414
  %24 = icmp slt i32 %22, %23, !dbg !1414
  br i1 %24, label %25, label %105, !dbg !1414

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1415
  %27 = sext i32 %26 to i64, !dbg !1415
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1415
  %29 = load i8, ptr %28, align 1, !dbg !1415
  %30 = sext i8 %29 to i32, !dbg !1415
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1415

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1418
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1418
  store ptr %33, ptr %5, align 8, !dbg !1418
  %34 = load i32, ptr %32, align 8, !dbg !1418
  %35 = trunc i32 %34 to i8, !dbg !1418
  %36 = load i32, ptr %12, align 4, !dbg !1418
  %37 = sext i32 %36 to i64, !dbg !1418
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1418
  store i8 %35, ptr %38, align 8, !dbg !1418
  br label %101, !dbg !1418

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1418
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1418
  store ptr %41, ptr %5, align 8, !dbg !1418
  %42 = load i32, ptr %40, align 8, !dbg !1418
  %43 = trunc i32 %42 to i8, !dbg !1418
  %44 = load i32, ptr %12, align 4, !dbg !1418
  %45 = sext i32 %44 to i64, !dbg !1418
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1418
  store i8 %43, ptr %46, align 8, !dbg !1418
  br label %101, !dbg !1418

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1418
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1418
  store ptr %49, ptr %5, align 8, !dbg !1418
  %50 = load i32, ptr %48, align 8, !dbg !1418
  %51 = trunc i32 %50 to i16, !dbg !1418
  %52 = load i32, ptr %12, align 4, !dbg !1418
  %53 = sext i32 %52 to i64, !dbg !1418
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1418
  store i16 %51, ptr %54, align 8, !dbg !1418
  br label %101, !dbg !1418

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1418
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1418
  store ptr %57, ptr %5, align 8, !dbg !1418
  %58 = load i32, ptr %56, align 8, !dbg !1418
  %59 = trunc i32 %58 to i16, !dbg !1418
  %60 = load i32, ptr %12, align 4, !dbg !1418
  %61 = sext i32 %60 to i64, !dbg !1418
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1418
  store i16 %59, ptr %62, align 8, !dbg !1418
  br label %101, !dbg !1418

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1418
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1418
  store ptr %65, ptr %5, align 8, !dbg !1418
  %66 = load i32, ptr %64, align 8, !dbg !1418
  %67 = load i32, ptr %12, align 4, !dbg !1418
  %68 = sext i32 %67 to i64, !dbg !1418
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1418
  store i32 %66, ptr %69, align 8, !dbg !1418
  br label %101, !dbg !1418

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1418
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1418
  store ptr %72, ptr %5, align 8, !dbg !1418
  %73 = load i32, ptr %71, align 8, !dbg !1418
  %74 = sext i32 %73 to i64, !dbg !1418
  %75 = load i32, ptr %12, align 4, !dbg !1418
  %76 = sext i32 %75 to i64, !dbg !1418
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1418
  store i64 %74, ptr %77, align 8, !dbg !1418
  br label %101, !dbg !1418

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1418
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1418
  store ptr %80, ptr %5, align 8, !dbg !1418
  %81 = load double, ptr %79, align 8, !dbg !1418
  %82 = fptrunc double %81 to float, !dbg !1418
  %83 = load i32, ptr %12, align 4, !dbg !1418
  %84 = sext i32 %83 to i64, !dbg !1418
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1418
  store float %82, ptr %85, align 8, !dbg !1418
  br label %101, !dbg !1418

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1418
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1418
  store ptr %88, ptr %5, align 8, !dbg !1418
  %89 = load double, ptr %87, align 8, !dbg !1418
  %90 = load i32, ptr %12, align 4, !dbg !1418
  %91 = sext i32 %90 to i64, !dbg !1418
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1418
  store double %89, ptr %92, align 8, !dbg !1418
  br label %101, !dbg !1418

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1418
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1418
  store ptr %95, ptr %5, align 8, !dbg !1418
  %96 = load ptr, ptr %94, align 8, !dbg !1418
  %97 = load i32, ptr %12, align 4, !dbg !1418
  %98 = sext i32 %97 to i64, !dbg !1418
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1418
  store ptr %96, ptr %99, align 8, !dbg !1418
  br label %101, !dbg !1418

100:                                              ; preds = %25
  br label %101, !dbg !1418

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1415

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1420
  %104 = add nsw i32 %103, 1, !dbg !1420
  store i32 %104, ptr %12, align 4, !dbg !1420
  br label %21, !dbg !1420, !llvm.loop !1421

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1405
  %107 = load ptr, ptr %106, align 8, !dbg !1405
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 125, !dbg !1405
  %109 = load ptr, ptr %108, align 8, !dbg !1405
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1405
  %111 = load ptr, ptr %6, align 8, !dbg !1405
  %112 = load ptr, ptr %7, align 8, !dbg !1405
  %113 = load ptr, ptr %8, align 8, !dbg !1405
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1405
  ret i16 %114, !dbg !1405
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1422 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1423, metadata !DIExpression()), !dbg !1424
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1425, metadata !DIExpression()), !dbg !1424
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1426, metadata !DIExpression()), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1427, metadata !DIExpression()), !dbg !1424
  call void @llvm.va_start(ptr %7), !dbg !1424
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1428, metadata !DIExpression()), !dbg !1424
  %9 = load ptr, ptr %7, align 8, !dbg !1424
  %10 = load ptr, ptr %4, align 8, !dbg !1424
  %11 = load ptr, ptr %5, align 8, !dbg !1424
  %12 = load ptr, ptr %6, align 8, !dbg !1424
  %13 = call i16 @JNI_CallShortMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1424
  store i16 %13, ptr %8, align 2, !dbg !1424
  call void @llvm.va_end(ptr %7), !dbg !1424
  %14 = load i16, ptr %8, align 2, !dbg !1424
  ret i16 %14, !dbg !1424
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1429 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1430, metadata !DIExpression()), !dbg !1431
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1432, metadata !DIExpression()), !dbg !1431
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1433, metadata !DIExpression()), !dbg !1431
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1434, metadata !DIExpression()), !dbg !1431
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1435, metadata !DIExpression()), !dbg !1431
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1436, metadata !DIExpression()), !dbg !1431
  %13 = load ptr, ptr %8, align 8, !dbg !1431
  %14 = load ptr, ptr %13, align 8, !dbg !1431
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1431
  %16 = load ptr, ptr %15, align 8, !dbg !1431
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1431
  %18 = load ptr, ptr %6, align 8, !dbg !1431
  %19 = load ptr, ptr %8, align 8, !dbg !1431
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1431
  store i32 %20, ptr %10, align 4, !dbg !1431
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1437, metadata !DIExpression()), !dbg !1431
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1438, metadata !DIExpression()), !dbg !1440
  store i32 0, ptr %12, align 4, !dbg !1440
  br label %21, !dbg !1440

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1440
  %23 = load i32, ptr %10, align 4, !dbg !1440
  %24 = icmp slt i32 %22, %23, !dbg !1440
  br i1 %24, label %25, label %105, !dbg !1440

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1441
  %27 = sext i32 %26 to i64, !dbg !1441
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1441
  %29 = load i8, ptr %28, align 1, !dbg !1441
  %30 = sext i8 %29 to i32, !dbg !1441
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1441

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1444
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1444
  store ptr %33, ptr %5, align 8, !dbg !1444
  %34 = load i32, ptr %32, align 8, !dbg !1444
  %35 = trunc i32 %34 to i8, !dbg !1444
  %36 = load i32, ptr %12, align 4, !dbg !1444
  %37 = sext i32 %36 to i64, !dbg !1444
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1444
  store i8 %35, ptr %38, align 8, !dbg !1444
  br label %101, !dbg !1444

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1444
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1444
  store ptr %41, ptr %5, align 8, !dbg !1444
  %42 = load i32, ptr %40, align 8, !dbg !1444
  %43 = trunc i32 %42 to i8, !dbg !1444
  %44 = load i32, ptr %12, align 4, !dbg !1444
  %45 = sext i32 %44 to i64, !dbg !1444
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1444
  store i8 %43, ptr %46, align 8, !dbg !1444
  br label %101, !dbg !1444

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1444
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1444
  store ptr %49, ptr %5, align 8, !dbg !1444
  %50 = load i32, ptr %48, align 8, !dbg !1444
  %51 = trunc i32 %50 to i16, !dbg !1444
  %52 = load i32, ptr %12, align 4, !dbg !1444
  %53 = sext i32 %52 to i64, !dbg !1444
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1444
  store i16 %51, ptr %54, align 8, !dbg !1444
  br label %101, !dbg !1444

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1444
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1444
  store ptr %57, ptr %5, align 8, !dbg !1444
  %58 = load i32, ptr %56, align 8, !dbg !1444
  %59 = trunc i32 %58 to i16, !dbg !1444
  %60 = load i32, ptr %12, align 4, !dbg !1444
  %61 = sext i32 %60 to i64, !dbg !1444
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1444
  store i16 %59, ptr %62, align 8, !dbg !1444
  br label %101, !dbg !1444

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1444
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1444
  store ptr %65, ptr %5, align 8, !dbg !1444
  %66 = load i32, ptr %64, align 8, !dbg !1444
  %67 = load i32, ptr %12, align 4, !dbg !1444
  %68 = sext i32 %67 to i64, !dbg !1444
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1444
  store i32 %66, ptr %69, align 8, !dbg !1444
  br label %101, !dbg !1444

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1444
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1444
  store ptr %72, ptr %5, align 8, !dbg !1444
  %73 = load i32, ptr %71, align 8, !dbg !1444
  %74 = sext i32 %73 to i64, !dbg !1444
  %75 = load i32, ptr %12, align 4, !dbg !1444
  %76 = sext i32 %75 to i64, !dbg !1444
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1444
  store i64 %74, ptr %77, align 8, !dbg !1444
  br label %101, !dbg !1444

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1444
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1444
  store ptr %80, ptr %5, align 8, !dbg !1444
  %81 = load double, ptr %79, align 8, !dbg !1444
  %82 = fptrunc double %81 to float, !dbg !1444
  %83 = load i32, ptr %12, align 4, !dbg !1444
  %84 = sext i32 %83 to i64, !dbg !1444
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1444
  store float %82, ptr %85, align 8, !dbg !1444
  br label %101, !dbg !1444

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1444
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1444
  store ptr %88, ptr %5, align 8, !dbg !1444
  %89 = load double, ptr %87, align 8, !dbg !1444
  %90 = load i32, ptr %12, align 4, !dbg !1444
  %91 = sext i32 %90 to i64, !dbg !1444
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1444
  store double %89, ptr %92, align 8, !dbg !1444
  br label %101, !dbg !1444

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1444
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1444
  store ptr %95, ptr %5, align 8, !dbg !1444
  %96 = load ptr, ptr %94, align 8, !dbg !1444
  %97 = load i32, ptr %12, align 4, !dbg !1444
  %98 = sext i32 %97 to i64, !dbg !1444
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1444
  store ptr %96, ptr %99, align 8, !dbg !1444
  br label %101, !dbg !1444

100:                                              ; preds = %25
  br label %101, !dbg !1444

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1441

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1446
  %104 = add nsw i32 %103, 1, !dbg !1446
  store i32 %104, ptr %12, align 4, !dbg !1446
  br label %21, !dbg !1446, !llvm.loop !1447

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1431
  %107 = load ptr, ptr %106, align 8, !dbg !1431
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 48, !dbg !1431
  %109 = load ptr, ptr %108, align 8, !dbg !1431
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1431
  %111 = load ptr, ptr %6, align 8, !dbg !1431
  %112 = load ptr, ptr %7, align 8, !dbg !1431
  %113 = load ptr, ptr %8, align 8, !dbg !1431
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1431
  ret i16 %114, !dbg !1431
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1448 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1449, metadata !DIExpression()), !dbg !1450
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1451, metadata !DIExpression()), !dbg !1450
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1452, metadata !DIExpression()), !dbg !1450
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1453, metadata !DIExpression()), !dbg !1450
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1454, metadata !DIExpression()), !dbg !1450
  call void @llvm.va_start(ptr %9), !dbg !1450
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1455, metadata !DIExpression()), !dbg !1450
  %11 = load ptr, ptr %9, align 8, !dbg !1450
  %12 = load ptr, ptr %5, align 8, !dbg !1450
  %13 = load ptr, ptr %6, align 8, !dbg !1450
  %14 = load ptr, ptr %7, align 8, !dbg !1450
  %15 = load ptr, ptr %8, align 8, !dbg !1450
  %16 = call i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1450
  store i16 %16, ptr %10, align 2, !dbg !1450
  call void @llvm.va_end(ptr %9), !dbg !1450
  %17 = load i16, ptr %10, align 2, !dbg !1450
  ret i16 %17, !dbg !1450
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1456 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1457, metadata !DIExpression()), !dbg !1458
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1459, metadata !DIExpression()), !dbg !1458
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1460, metadata !DIExpression()), !dbg !1458
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1461, metadata !DIExpression()), !dbg !1458
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1462, metadata !DIExpression()), !dbg !1458
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1463, metadata !DIExpression()), !dbg !1458
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1464, metadata !DIExpression()), !dbg !1458
  %15 = load ptr, ptr %10, align 8, !dbg !1458
  %16 = load ptr, ptr %15, align 8, !dbg !1458
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1458
  %18 = load ptr, ptr %17, align 8, !dbg !1458
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1458
  %20 = load ptr, ptr %7, align 8, !dbg !1458
  %21 = load ptr, ptr %10, align 8, !dbg !1458
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1458
  store i32 %22, ptr %12, align 4, !dbg !1458
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1465, metadata !DIExpression()), !dbg !1458
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1466, metadata !DIExpression()), !dbg !1468
  store i32 0, ptr %14, align 4, !dbg !1468
  br label %23, !dbg !1468

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1468
  %25 = load i32, ptr %12, align 4, !dbg !1468
  %26 = icmp slt i32 %24, %25, !dbg !1468
  br i1 %26, label %27, label %107, !dbg !1468

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1469
  %29 = sext i32 %28 to i64, !dbg !1469
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1469
  %31 = load i8, ptr %30, align 1, !dbg !1469
  %32 = sext i8 %31 to i32, !dbg !1469
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1469

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1472
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1472
  store ptr %35, ptr %6, align 8, !dbg !1472
  %36 = load i32, ptr %34, align 8, !dbg !1472
  %37 = trunc i32 %36 to i8, !dbg !1472
  %38 = load i32, ptr %14, align 4, !dbg !1472
  %39 = sext i32 %38 to i64, !dbg !1472
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1472
  store i8 %37, ptr %40, align 8, !dbg !1472
  br label %103, !dbg !1472

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1472
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1472
  store ptr %43, ptr %6, align 8, !dbg !1472
  %44 = load i32, ptr %42, align 8, !dbg !1472
  %45 = trunc i32 %44 to i8, !dbg !1472
  %46 = load i32, ptr %14, align 4, !dbg !1472
  %47 = sext i32 %46 to i64, !dbg !1472
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1472
  store i8 %45, ptr %48, align 8, !dbg !1472
  br label %103, !dbg !1472

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1472
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1472
  store ptr %51, ptr %6, align 8, !dbg !1472
  %52 = load i32, ptr %50, align 8, !dbg !1472
  %53 = trunc i32 %52 to i16, !dbg !1472
  %54 = load i32, ptr %14, align 4, !dbg !1472
  %55 = sext i32 %54 to i64, !dbg !1472
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1472
  store i16 %53, ptr %56, align 8, !dbg !1472
  br label %103, !dbg !1472

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1472
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1472
  store ptr %59, ptr %6, align 8, !dbg !1472
  %60 = load i32, ptr %58, align 8, !dbg !1472
  %61 = trunc i32 %60 to i16, !dbg !1472
  %62 = load i32, ptr %14, align 4, !dbg !1472
  %63 = sext i32 %62 to i64, !dbg !1472
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1472
  store i16 %61, ptr %64, align 8, !dbg !1472
  br label %103, !dbg !1472

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1472
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1472
  store ptr %67, ptr %6, align 8, !dbg !1472
  %68 = load i32, ptr %66, align 8, !dbg !1472
  %69 = load i32, ptr %14, align 4, !dbg !1472
  %70 = sext i32 %69 to i64, !dbg !1472
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1472
  store i32 %68, ptr %71, align 8, !dbg !1472
  br label %103, !dbg !1472

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1472
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1472
  store ptr %74, ptr %6, align 8, !dbg !1472
  %75 = load i32, ptr %73, align 8, !dbg !1472
  %76 = sext i32 %75 to i64, !dbg !1472
  %77 = load i32, ptr %14, align 4, !dbg !1472
  %78 = sext i32 %77 to i64, !dbg !1472
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1472
  store i64 %76, ptr %79, align 8, !dbg !1472
  br label %103, !dbg !1472

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1472
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1472
  store ptr %82, ptr %6, align 8, !dbg !1472
  %83 = load double, ptr %81, align 8, !dbg !1472
  %84 = fptrunc double %83 to float, !dbg !1472
  %85 = load i32, ptr %14, align 4, !dbg !1472
  %86 = sext i32 %85 to i64, !dbg !1472
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1472
  store float %84, ptr %87, align 8, !dbg !1472
  br label %103, !dbg !1472

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1472
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1472
  store ptr %90, ptr %6, align 8, !dbg !1472
  %91 = load double, ptr %89, align 8, !dbg !1472
  %92 = load i32, ptr %14, align 4, !dbg !1472
  %93 = sext i32 %92 to i64, !dbg !1472
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1472
  store double %91, ptr %94, align 8, !dbg !1472
  br label %103, !dbg !1472

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1472
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1472
  store ptr %97, ptr %6, align 8, !dbg !1472
  %98 = load ptr, ptr %96, align 8, !dbg !1472
  %99 = load i32, ptr %14, align 4, !dbg !1472
  %100 = sext i32 %99 to i64, !dbg !1472
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1472
  store ptr %98, ptr %101, align 8, !dbg !1472
  br label %103, !dbg !1472

102:                                              ; preds = %27
  br label %103, !dbg !1472

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1469

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1474
  %106 = add nsw i32 %105, 1, !dbg !1474
  store i32 %106, ptr %14, align 4, !dbg !1474
  br label %23, !dbg !1474, !llvm.loop !1475

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1458
  %109 = load ptr, ptr %108, align 8, !dbg !1458
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 78, !dbg !1458
  %111 = load ptr, ptr %110, align 8, !dbg !1458
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1458
  %113 = load ptr, ptr %7, align 8, !dbg !1458
  %114 = load ptr, ptr %8, align 8, !dbg !1458
  %115 = load ptr, ptr %9, align 8, !dbg !1458
  %116 = load ptr, ptr %10, align 8, !dbg !1458
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1458
  ret i16 %117, !dbg !1458
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1476 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1477, metadata !DIExpression()), !dbg !1478
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1479, metadata !DIExpression()), !dbg !1478
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1480, metadata !DIExpression()), !dbg !1478
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1481, metadata !DIExpression()), !dbg !1478
  call void @llvm.va_start(ptr %7), !dbg !1478
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1482, metadata !DIExpression()), !dbg !1478
  %9 = load ptr, ptr %7, align 8, !dbg !1478
  %10 = load ptr, ptr %4, align 8, !dbg !1478
  %11 = load ptr, ptr %5, align 8, !dbg !1478
  %12 = load ptr, ptr %6, align 8, !dbg !1478
  %13 = call i16 @JNI_CallStaticShortMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1478
  store i16 %13, ptr %8, align 2, !dbg !1478
  call void @llvm.va_end(ptr %7), !dbg !1478
  %14 = load i16, ptr %8, align 2, !dbg !1478
  ret i16 %14, !dbg !1478
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1483 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1484, metadata !DIExpression()), !dbg !1485
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1486, metadata !DIExpression()), !dbg !1485
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1487, metadata !DIExpression()), !dbg !1485
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1488, metadata !DIExpression()), !dbg !1485
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1489, metadata !DIExpression()), !dbg !1485
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1490, metadata !DIExpression()), !dbg !1485
  %13 = load ptr, ptr %8, align 8, !dbg !1485
  %14 = load ptr, ptr %13, align 8, !dbg !1485
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1485
  %16 = load ptr, ptr %15, align 8, !dbg !1485
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1485
  %18 = load ptr, ptr %6, align 8, !dbg !1485
  %19 = load ptr, ptr %8, align 8, !dbg !1485
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1485
  store i32 %20, ptr %10, align 4, !dbg !1485
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1491, metadata !DIExpression()), !dbg !1485
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1492, metadata !DIExpression()), !dbg !1494
  store i32 0, ptr %12, align 4, !dbg !1494
  br label %21, !dbg !1494

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1494
  %23 = load i32, ptr %10, align 4, !dbg !1494
  %24 = icmp slt i32 %22, %23, !dbg !1494
  br i1 %24, label %25, label %105, !dbg !1494

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1495
  %27 = sext i32 %26 to i64, !dbg !1495
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1495
  %29 = load i8, ptr %28, align 1, !dbg !1495
  %30 = sext i8 %29 to i32, !dbg !1495
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1495

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1498
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1498
  store ptr %33, ptr %5, align 8, !dbg !1498
  %34 = load i32, ptr %32, align 8, !dbg !1498
  %35 = trunc i32 %34 to i8, !dbg !1498
  %36 = load i32, ptr %12, align 4, !dbg !1498
  %37 = sext i32 %36 to i64, !dbg !1498
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1498
  store i8 %35, ptr %38, align 8, !dbg !1498
  br label %101, !dbg !1498

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1498
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1498
  store ptr %41, ptr %5, align 8, !dbg !1498
  %42 = load i32, ptr %40, align 8, !dbg !1498
  %43 = trunc i32 %42 to i8, !dbg !1498
  %44 = load i32, ptr %12, align 4, !dbg !1498
  %45 = sext i32 %44 to i64, !dbg !1498
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1498
  store i8 %43, ptr %46, align 8, !dbg !1498
  br label %101, !dbg !1498

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1498
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1498
  store ptr %49, ptr %5, align 8, !dbg !1498
  %50 = load i32, ptr %48, align 8, !dbg !1498
  %51 = trunc i32 %50 to i16, !dbg !1498
  %52 = load i32, ptr %12, align 4, !dbg !1498
  %53 = sext i32 %52 to i64, !dbg !1498
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1498
  store i16 %51, ptr %54, align 8, !dbg !1498
  br label %101, !dbg !1498

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1498
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1498
  store ptr %57, ptr %5, align 8, !dbg !1498
  %58 = load i32, ptr %56, align 8, !dbg !1498
  %59 = trunc i32 %58 to i16, !dbg !1498
  %60 = load i32, ptr %12, align 4, !dbg !1498
  %61 = sext i32 %60 to i64, !dbg !1498
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1498
  store i16 %59, ptr %62, align 8, !dbg !1498
  br label %101, !dbg !1498

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1498
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1498
  store ptr %65, ptr %5, align 8, !dbg !1498
  %66 = load i32, ptr %64, align 8, !dbg !1498
  %67 = load i32, ptr %12, align 4, !dbg !1498
  %68 = sext i32 %67 to i64, !dbg !1498
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1498
  store i32 %66, ptr %69, align 8, !dbg !1498
  br label %101, !dbg !1498

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1498
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1498
  store ptr %72, ptr %5, align 8, !dbg !1498
  %73 = load i32, ptr %71, align 8, !dbg !1498
  %74 = sext i32 %73 to i64, !dbg !1498
  %75 = load i32, ptr %12, align 4, !dbg !1498
  %76 = sext i32 %75 to i64, !dbg !1498
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1498
  store i64 %74, ptr %77, align 8, !dbg !1498
  br label %101, !dbg !1498

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1498
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1498
  store ptr %80, ptr %5, align 8, !dbg !1498
  %81 = load double, ptr %79, align 8, !dbg !1498
  %82 = fptrunc double %81 to float, !dbg !1498
  %83 = load i32, ptr %12, align 4, !dbg !1498
  %84 = sext i32 %83 to i64, !dbg !1498
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1498
  store float %82, ptr %85, align 8, !dbg !1498
  br label %101, !dbg !1498

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1498
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1498
  store ptr %88, ptr %5, align 8, !dbg !1498
  %89 = load double, ptr %87, align 8, !dbg !1498
  %90 = load i32, ptr %12, align 4, !dbg !1498
  %91 = sext i32 %90 to i64, !dbg !1498
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1498
  store double %89, ptr %92, align 8, !dbg !1498
  br label %101, !dbg !1498

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1498
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1498
  store ptr %95, ptr %5, align 8, !dbg !1498
  %96 = load ptr, ptr %94, align 8, !dbg !1498
  %97 = load i32, ptr %12, align 4, !dbg !1498
  %98 = sext i32 %97 to i64, !dbg !1498
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1498
  store ptr %96, ptr %99, align 8, !dbg !1498
  br label %101, !dbg !1498

100:                                              ; preds = %25
  br label %101, !dbg !1498

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1495

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1500
  %104 = add nsw i32 %103, 1, !dbg !1500
  store i32 %104, ptr %12, align 4, !dbg !1500
  br label %21, !dbg !1500, !llvm.loop !1501

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1485
  %107 = load ptr, ptr %106, align 8, !dbg !1485
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 128, !dbg !1485
  %109 = load ptr, ptr %108, align 8, !dbg !1485
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1485
  %111 = load ptr, ptr %6, align 8, !dbg !1485
  %112 = load ptr, ptr %7, align 8, !dbg !1485
  %113 = load ptr, ptr %8, align 8, !dbg !1485
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1485
  ret i16 %114, !dbg !1485
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1502 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1503, metadata !DIExpression()), !dbg !1504
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1505, metadata !DIExpression()), !dbg !1504
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1506, metadata !DIExpression()), !dbg !1504
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1507, metadata !DIExpression()), !dbg !1504
  call void @llvm.va_start(ptr %7), !dbg !1504
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1508, metadata !DIExpression()), !dbg !1504
  %9 = load ptr, ptr %7, align 8, !dbg !1504
  %10 = load ptr, ptr %4, align 8, !dbg !1504
  %11 = load ptr, ptr %5, align 8, !dbg !1504
  %12 = load ptr, ptr %6, align 8, !dbg !1504
  %13 = call i32 @JNI_CallIntMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1504
  store i32 %13, ptr %8, align 4, !dbg !1504
  call void @llvm.va_end(ptr %7), !dbg !1504
  %14 = load i32, ptr %8, align 4, !dbg !1504
  ret i32 %14, !dbg !1504
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1509 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1510, metadata !DIExpression()), !dbg !1511
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1512, metadata !DIExpression()), !dbg !1511
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1513, metadata !DIExpression()), !dbg !1511
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1514, metadata !DIExpression()), !dbg !1511
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1515, metadata !DIExpression()), !dbg !1511
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1516, metadata !DIExpression()), !dbg !1511
  %13 = load ptr, ptr %8, align 8, !dbg !1511
  %14 = load ptr, ptr %13, align 8, !dbg !1511
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1511
  %16 = load ptr, ptr %15, align 8, !dbg !1511
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1511
  %18 = load ptr, ptr %6, align 8, !dbg !1511
  %19 = load ptr, ptr %8, align 8, !dbg !1511
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1511
  store i32 %20, ptr %10, align 4, !dbg !1511
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1517, metadata !DIExpression()), !dbg !1511
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1518, metadata !DIExpression()), !dbg !1520
  store i32 0, ptr %12, align 4, !dbg !1520
  br label %21, !dbg !1520

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1520
  %23 = load i32, ptr %10, align 4, !dbg !1520
  %24 = icmp slt i32 %22, %23, !dbg !1520
  br i1 %24, label %25, label %105, !dbg !1520

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1521
  %27 = sext i32 %26 to i64, !dbg !1521
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1521
  %29 = load i8, ptr %28, align 1, !dbg !1521
  %30 = sext i8 %29 to i32, !dbg !1521
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1521

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1524
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1524
  store ptr %33, ptr %5, align 8, !dbg !1524
  %34 = load i32, ptr %32, align 8, !dbg !1524
  %35 = trunc i32 %34 to i8, !dbg !1524
  %36 = load i32, ptr %12, align 4, !dbg !1524
  %37 = sext i32 %36 to i64, !dbg !1524
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1524
  store i8 %35, ptr %38, align 8, !dbg !1524
  br label %101, !dbg !1524

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1524
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1524
  store ptr %41, ptr %5, align 8, !dbg !1524
  %42 = load i32, ptr %40, align 8, !dbg !1524
  %43 = trunc i32 %42 to i8, !dbg !1524
  %44 = load i32, ptr %12, align 4, !dbg !1524
  %45 = sext i32 %44 to i64, !dbg !1524
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1524
  store i8 %43, ptr %46, align 8, !dbg !1524
  br label %101, !dbg !1524

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1524
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1524
  store ptr %49, ptr %5, align 8, !dbg !1524
  %50 = load i32, ptr %48, align 8, !dbg !1524
  %51 = trunc i32 %50 to i16, !dbg !1524
  %52 = load i32, ptr %12, align 4, !dbg !1524
  %53 = sext i32 %52 to i64, !dbg !1524
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1524
  store i16 %51, ptr %54, align 8, !dbg !1524
  br label %101, !dbg !1524

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1524
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1524
  store ptr %57, ptr %5, align 8, !dbg !1524
  %58 = load i32, ptr %56, align 8, !dbg !1524
  %59 = trunc i32 %58 to i16, !dbg !1524
  %60 = load i32, ptr %12, align 4, !dbg !1524
  %61 = sext i32 %60 to i64, !dbg !1524
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1524
  store i16 %59, ptr %62, align 8, !dbg !1524
  br label %101, !dbg !1524

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1524
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1524
  store ptr %65, ptr %5, align 8, !dbg !1524
  %66 = load i32, ptr %64, align 8, !dbg !1524
  %67 = load i32, ptr %12, align 4, !dbg !1524
  %68 = sext i32 %67 to i64, !dbg !1524
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1524
  store i32 %66, ptr %69, align 8, !dbg !1524
  br label %101, !dbg !1524

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1524
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1524
  store ptr %72, ptr %5, align 8, !dbg !1524
  %73 = load i32, ptr %71, align 8, !dbg !1524
  %74 = sext i32 %73 to i64, !dbg !1524
  %75 = load i32, ptr %12, align 4, !dbg !1524
  %76 = sext i32 %75 to i64, !dbg !1524
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1524
  store i64 %74, ptr %77, align 8, !dbg !1524
  br label %101, !dbg !1524

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1524
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1524
  store ptr %80, ptr %5, align 8, !dbg !1524
  %81 = load double, ptr %79, align 8, !dbg !1524
  %82 = fptrunc double %81 to float, !dbg !1524
  %83 = load i32, ptr %12, align 4, !dbg !1524
  %84 = sext i32 %83 to i64, !dbg !1524
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1524
  store float %82, ptr %85, align 8, !dbg !1524
  br label %101, !dbg !1524

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1524
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1524
  store ptr %88, ptr %5, align 8, !dbg !1524
  %89 = load double, ptr %87, align 8, !dbg !1524
  %90 = load i32, ptr %12, align 4, !dbg !1524
  %91 = sext i32 %90 to i64, !dbg !1524
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1524
  store double %89, ptr %92, align 8, !dbg !1524
  br label %101, !dbg !1524

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1524
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1524
  store ptr %95, ptr %5, align 8, !dbg !1524
  %96 = load ptr, ptr %94, align 8, !dbg !1524
  %97 = load i32, ptr %12, align 4, !dbg !1524
  %98 = sext i32 %97 to i64, !dbg !1524
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1524
  store ptr %96, ptr %99, align 8, !dbg !1524
  br label %101, !dbg !1524

100:                                              ; preds = %25
  br label %101, !dbg !1524

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1521

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1526
  %104 = add nsw i32 %103, 1, !dbg !1526
  store i32 %104, ptr %12, align 4, !dbg !1526
  br label %21, !dbg !1526, !llvm.loop !1527

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1511
  %107 = load ptr, ptr %106, align 8, !dbg !1511
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 51, !dbg !1511
  %109 = load ptr, ptr %108, align 8, !dbg !1511
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1511
  %111 = load ptr, ptr %6, align 8, !dbg !1511
  %112 = load ptr, ptr %7, align 8, !dbg !1511
  %113 = load ptr, ptr %8, align 8, !dbg !1511
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1511
  ret i32 %114, !dbg !1511
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1528 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1529, metadata !DIExpression()), !dbg !1530
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1531, metadata !DIExpression()), !dbg !1530
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1532, metadata !DIExpression()), !dbg !1530
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1533, metadata !DIExpression()), !dbg !1530
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1534, metadata !DIExpression()), !dbg !1530
  call void @llvm.va_start(ptr %9), !dbg !1530
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1535, metadata !DIExpression()), !dbg !1530
  %11 = load ptr, ptr %9, align 8, !dbg !1530
  %12 = load ptr, ptr %5, align 8, !dbg !1530
  %13 = load ptr, ptr %6, align 8, !dbg !1530
  %14 = load ptr, ptr %7, align 8, !dbg !1530
  %15 = load ptr, ptr %8, align 8, !dbg !1530
  %16 = call i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1530
  store i32 %16, ptr %10, align 4, !dbg !1530
  call void @llvm.va_end(ptr %9), !dbg !1530
  %17 = load i32, ptr %10, align 4, !dbg !1530
  ret i32 %17, !dbg !1530
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1536 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1537, metadata !DIExpression()), !dbg !1538
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1539, metadata !DIExpression()), !dbg !1538
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1540, metadata !DIExpression()), !dbg !1538
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1541, metadata !DIExpression()), !dbg !1538
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1542, metadata !DIExpression()), !dbg !1538
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1543, metadata !DIExpression()), !dbg !1538
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1544, metadata !DIExpression()), !dbg !1538
  %15 = load ptr, ptr %10, align 8, !dbg !1538
  %16 = load ptr, ptr %15, align 8, !dbg !1538
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1538
  %18 = load ptr, ptr %17, align 8, !dbg !1538
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1538
  %20 = load ptr, ptr %7, align 8, !dbg !1538
  %21 = load ptr, ptr %10, align 8, !dbg !1538
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1538
  store i32 %22, ptr %12, align 4, !dbg !1538
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1545, metadata !DIExpression()), !dbg !1538
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1546, metadata !DIExpression()), !dbg !1548
  store i32 0, ptr %14, align 4, !dbg !1548
  br label %23, !dbg !1548

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1548
  %25 = load i32, ptr %12, align 4, !dbg !1548
  %26 = icmp slt i32 %24, %25, !dbg !1548
  br i1 %26, label %27, label %107, !dbg !1548

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1549
  %29 = sext i32 %28 to i64, !dbg !1549
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1549
  %31 = load i8, ptr %30, align 1, !dbg !1549
  %32 = sext i8 %31 to i32, !dbg !1549
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1549

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1552
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1552
  store ptr %35, ptr %6, align 8, !dbg !1552
  %36 = load i32, ptr %34, align 8, !dbg !1552
  %37 = trunc i32 %36 to i8, !dbg !1552
  %38 = load i32, ptr %14, align 4, !dbg !1552
  %39 = sext i32 %38 to i64, !dbg !1552
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1552
  store i8 %37, ptr %40, align 8, !dbg !1552
  br label %103, !dbg !1552

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1552
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1552
  store ptr %43, ptr %6, align 8, !dbg !1552
  %44 = load i32, ptr %42, align 8, !dbg !1552
  %45 = trunc i32 %44 to i8, !dbg !1552
  %46 = load i32, ptr %14, align 4, !dbg !1552
  %47 = sext i32 %46 to i64, !dbg !1552
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1552
  store i8 %45, ptr %48, align 8, !dbg !1552
  br label %103, !dbg !1552

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1552
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1552
  store ptr %51, ptr %6, align 8, !dbg !1552
  %52 = load i32, ptr %50, align 8, !dbg !1552
  %53 = trunc i32 %52 to i16, !dbg !1552
  %54 = load i32, ptr %14, align 4, !dbg !1552
  %55 = sext i32 %54 to i64, !dbg !1552
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1552
  store i16 %53, ptr %56, align 8, !dbg !1552
  br label %103, !dbg !1552

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1552
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1552
  store ptr %59, ptr %6, align 8, !dbg !1552
  %60 = load i32, ptr %58, align 8, !dbg !1552
  %61 = trunc i32 %60 to i16, !dbg !1552
  %62 = load i32, ptr %14, align 4, !dbg !1552
  %63 = sext i32 %62 to i64, !dbg !1552
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1552
  store i16 %61, ptr %64, align 8, !dbg !1552
  br label %103, !dbg !1552

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1552
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1552
  store ptr %67, ptr %6, align 8, !dbg !1552
  %68 = load i32, ptr %66, align 8, !dbg !1552
  %69 = load i32, ptr %14, align 4, !dbg !1552
  %70 = sext i32 %69 to i64, !dbg !1552
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1552
  store i32 %68, ptr %71, align 8, !dbg !1552
  br label %103, !dbg !1552

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1552
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1552
  store ptr %74, ptr %6, align 8, !dbg !1552
  %75 = load i32, ptr %73, align 8, !dbg !1552
  %76 = sext i32 %75 to i64, !dbg !1552
  %77 = load i32, ptr %14, align 4, !dbg !1552
  %78 = sext i32 %77 to i64, !dbg !1552
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1552
  store i64 %76, ptr %79, align 8, !dbg !1552
  br label %103, !dbg !1552

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1552
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1552
  store ptr %82, ptr %6, align 8, !dbg !1552
  %83 = load double, ptr %81, align 8, !dbg !1552
  %84 = fptrunc double %83 to float, !dbg !1552
  %85 = load i32, ptr %14, align 4, !dbg !1552
  %86 = sext i32 %85 to i64, !dbg !1552
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1552
  store float %84, ptr %87, align 8, !dbg !1552
  br label %103, !dbg !1552

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1552
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1552
  store ptr %90, ptr %6, align 8, !dbg !1552
  %91 = load double, ptr %89, align 8, !dbg !1552
  %92 = load i32, ptr %14, align 4, !dbg !1552
  %93 = sext i32 %92 to i64, !dbg !1552
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1552
  store double %91, ptr %94, align 8, !dbg !1552
  br label %103, !dbg !1552

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1552
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1552
  store ptr %97, ptr %6, align 8, !dbg !1552
  %98 = load ptr, ptr %96, align 8, !dbg !1552
  %99 = load i32, ptr %14, align 4, !dbg !1552
  %100 = sext i32 %99 to i64, !dbg !1552
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1552
  store ptr %98, ptr %101, align 8, !dbg !1552
  br label %103, !dbg !1552

102:                                              ; preds = %27
  br label %103, !dbg !1552

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1549

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1554
  %106 = add nsw i32 %105, 1, !dbg !1554
  store i32 %106, ptr %14, align 4, !dbg !1554
  br label %23, !dbg !1554, !llvm.loop !1555

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1538
  %109 = load ptr, ptr %108, align 8, !dbg !1538
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 81, !dbg !1538
  %111 = load ptr, ptr %110, align 8, !dbg !1538
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1538
  %113 = load ptr, ptr %7, align 8, !dbg !1538
  %114 = load ptr, ptr %8, align 8, !dbg !1538
  %115 = load ptr, ptr %9, align 8, !dbg !1538
  %116 = load ptr, ptr %10, align 8, !dbg !1538
  %117 = call i32 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1538
  ret i32 %117, !dbg !1538
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1556 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1557, metadata !DIExpression()), !dbg !1558
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1559, metadata !DIExpression()), !dbg !1558
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1560, metadata !DIExpression()), !dbg !1558
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1561, metadata !DIExpression()), !dbg !1558
  call void @llvm.va_start(ptr %7), !dbg !1558
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1562, metadata !DIExpression()), !dbg !1558
  %9 = load ptr, ptr %7, align 8, !dbg !1558
  %10 = load ptr, ptr %4, align 8, !dbg !1558
  %11 = load ptr, ptr %5, align 8, !dbg !1558
  %12 = load ptr, ptr %6, align 8, !dbg !1558
  %13 = call i32 @JNI_CallStaticIntMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1558
  store i32 %13, ptr %8, align 4, !dbg !1558
  call void @llvm.va_end(ptr %7), !dbg !1558
  %14 = load i32, ptr %8, align 4, !dbg !1558
  ret i32 %14, !dbg !1558
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1563 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1564, metadata !DIExpression()), !dbg !1565
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1566, metadata !DIExpression()), !dbg !1565
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1567, metadata !DIExpression()), !dbg !1565
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1568, metadata !DIExpression()), !dbg !1565
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1569, metadata !DIExpression()), !dbg !1565
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1570, metadata !DIExpression()), !dbg !1565
  %13 = load ptr, ptr %8, align 8, !dbg !1565
  %14 = load ptr, ptr %13, align 8, !dbg !1565
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1565
  %16 = load ptr, ptr %15, align 8, !dbg !1565
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1565
  %18 = load ptr, ptr %6, align 8, !dbg !1565
  %19 = load ptr, ptr %8, align 8, !dbg !1565
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1565
  store i32 %20, ptr %10, align 4, !dbg !1565
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1571, metadata !DIExpression()), !dbg !1565
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1572, metadata !DIExpression()), !dbg !1574
  store i32 0, ptr %12, align 4, !dbg !1574
  br label %21, !dbg !1574

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1574
  %23 = load i32, ptr %10, align 4, !dbg !1574
  %24 = icmp slt i32 %22, %23, !dbg !1574
  br i1 %24, label %25, label %105, !dbg !1574

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1575
  %27 = sext i32 %26 to i64, !dbg !1575
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1575
  %29 = load i8, ptr %28, align 1, !dbg !1575
  %30 = sext i8 %29 to i32, !dbg !1575
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1575

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1578
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1578
  store ptr %33, ptr %5, align 8, !dbg !1578
  %34 = load i32, ptr %32, align 8, !dbg !1578
  %35 = trunc i32 %34 to i8, !dbg !1578
  %36 = load i32, ptr %12, align 4, !dbg !1578
  %37 = sext i32 %36 to i64, !dbg !1578
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1578
  store i8 %35, ptr %38, align 8, !dbg !1578
  br label %101, !dbg !1578

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1578
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1578
  store ptr %41, ptr %5, align 8, !dbg !1578
  %42 = load i32, ptr %40, align 8, !dbg !1578
  %43 = trunc i32 %42 to i8, !dbg !1578
  %44 = load i32, ptr %12, align 4, !dbg !1578
  %45 = sext i32 %44 to i64, !dbg !1578
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1578
  store i8 %43, ptr %46, align 8, !dbg !1578
  br label %101, !dbg !1578

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1578
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1578
  store ptr %49, ptr %5, align 8, !dbg !1578
  %50 = load i32, ptr %48, align 8, !dbg !1578
  %51 = trunc i32 %50 to i16, !dbg !1578
  %52 = load i32, ptr %12, align 4, !dbg !1578
  %53 = sext i32 %52 to i64, !dbg !1578
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1578
  store i16 %51, ptr %54, align 8, !dbg !1578
  br label %101, !dbg !1578

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1578
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1578
  store ptr %57, ptr %5, align 8, !dbg !1578
  %58 = load i32, ptr %56, align 8, !dbg !1578
  %59 = trunc i32 %58 to i16, !dbg !1578
  %60 = load i32, ptr %12, align 4, !dbg !1578
  %61 = sext i32 %60 to i64, !dbg !1578
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1578
  store i16 %59, ptr %62, align 8, !dbg !1578
  br label %101, !dbg !1578

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1578
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1578
  store ptr %65, ptr %5, align 8, !dbg !1578
  %66 = load i32, ptr %64, align 8, !dbg !1578
  %67 = load i32, ptr %12, align 4, !dbg !1578
  %68 = sext i32 %67 to i64, !dbg !1578
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1578
  store i32 %66, ptr %69, align 8, !dbg !1578
  br label %101, !dbg !1578

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1578
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1578
  store ptr %72, ptr %5, align 8, !dbg !1578
  %73 = load i32, ptr %71, align 8, !dbg !1578
  %74 = sext i32 %73 to i64, !dbg !1578
  %75 = load i32, ptr %12, align 4, !dbg !1578
  %76 = sext i32 %75 to i64, !dbg !1578
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1578
  store i64 %74, ptr %77, align 8, !dbg !1578
  br label %101, !dbg !1578

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1578
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1578
  store ptr %80, ptr %5, align 8, !dbg !1578
  %81 = load double, ptr %79, align 8, !dbg !1578
  %82 = fptrunc double %81 to float, !dbg !1578
  %83 = load i32, ptr %12, align 4, !dbg !1578
  %84 = sext i32 %83 to i64, !dbg !1578
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1578
  store float %82, ptr %85, align 8, !dbg !1578
  br label %101, !dbg !1578

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1578
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1578
  store ptr %88, ptr %5, align 8, !dbg !1578
  %89 = load double, ptr %87, align 8, !dbg !1578
  %90 = load i32, ptr %12, align 4, !dbg !1578
  %91 = sext i32 %90 to i64, !dbg !1578
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1578
  store double %89, ptr %92, align 8, !dbg !1578
  br label %101, !dbg !1578

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1578
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1578
  store ptr %95, ptr %5, align 8, !dbg !1578
  %96 = load ptr, ptr %94, align 8, !dbg !1578
  %97 = load i32, ptr %12, align 4, !dbg !1578
  %98 = sext i32 %97 to i64, !dbg !1578
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1578
  store ptr %96, ptr %99, align 8, !dbg !1578
  br label %101, !dbg !1578

100:                                              ; preds = %25
  br label %101, !dbg !1578

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1575

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1580
  %104 = add nsw i32 %103, 1, !dbg !1580
  store i32 %104, ptr %12, align 4, !dbg !1580
  br label %21, !dbg !1580, !llvm.loop !1581

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1565
  %107 = load ptr, ptr %106, align 8, !dbg !1565
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 131, !dbg !1565
  %109 = load ptr, ptr %108, align 8, !dbg !1565
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1565
  %111 = load ptr, ptr %6, align 8, !dbg !1565
  %112 = load ptr, ptr %7, align 8, !dbg !1565
  %113 = load ptr, ptr %8, align 8, !dbg !1565
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1565
  ret i32 %114, !dbg !1565
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1582 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1583, metadata !DIExpression()), !dbg !1584
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1585, metadata !DIExpression()), !dbg !1584
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1586, metadata !DIExpression()), !dbg !1584
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1587, metadata !DIExpression()), !dbg !1584
  call void @llvm.va_start(ptr %7), !dbg !1584
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1588, metadata !DIExpression()), !dbg !1584
  %9 = load ptr, ptr %7, align 8, !dbg !1584
  %10 = load ptr, ptr %4, align 8, !dbg !1584
  %11 = load ptr, ptr %5, align 8, !dbg !1584
  %12 = load ptr, ptr %6, align 8, !dbg !1584
  %13 = call i64 @JNI_CallLongMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1584
  store i64 %13, ptr %8, align 8, !dbg !1584
  call void @llvm.va_end(ptr %7), !dbg !1584
  %14 = load i64, ptr %8, align 8, !dbg !1584
  ret i64 %14, !dbg !1584
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1589 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1590, metadata !DIExpression()), !dbg !1591
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1592, metadata !DIExpression()), !dbg !1591
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1593, metadata !DIExpression()), !dbg !1591
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1594, metadata !DIExpression()), !dbg !1591
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1595, metadata !DIExpression()), !dbg !1591
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1596, metadata !DIExpression()), !dbg !1591
  %13 = load ptr, ptr %8, align 8, !dbg !1591
  %14 = load ptr, ptr %13, align 8, !dbg !1591
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1591
  %16 = load ptr, ptr %15, align 8, !dbg !1591
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1591
  %18 = load ptr, ptr %6, align 8, !dbg !1591
  %19 = load ptr, ptr %8, align 8, !dbg !1591
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1591
  store i32 %20, ptr %10, align 4, !dbg !1591
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1597, metadata !DIExpression()), !dbg !1591
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1598, metadata !DIExpression()), !dbg !1600
  store i32 0, ptr %12, align 4, !dbg !1600
  br label %21, !dbg !1600

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1600
  %23 = load i32, ptr %10, align 4, !dbg !1600
  %24 = icmp slt i32 %22, %23, !dbg !1600
  br i1 %24, label %25, label %105, !dbg !1600

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1601
  %27 = sext i32 %26 to i64, !dbg !1601
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1601
  %29 = load i8, ptr %28, align 1, !dbg !1601
  %30 = sext i8 %29 to i32, !dbg !1601
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1601

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1604
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1604
  store ptr %33, ptr %5, align 8, !dbg !1604
  %34 = load i32, ptr %32, align 8, !dbg !1604
  %35 = trunc i32 %34 to i8, !dbg !1604
  %36 = load i32, ptr %12, align 4, !dbg !1604
  %37 = sext i32 %36 to i64, !dbg !1604
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1604
  store i8 %35, ptr %38, align 8, !dbg !1604
  br label %101, !dbg !1604

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1604
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1604
  store ptr %41, ptr %5, align 8, !dbg !1604
  %42 = load i32, ptr %40, align 8, !dbg !1604
  %43 = trunc i32 %42 to i8, !dbg !1604
  %44 = load i32, ptr %12, align 4, !dbg !1604
  %45 = sext i32 %44 to i64, !dbg !1604
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1604
  store i8 %43, ptr %46, align 8, !dbg !1604
  br label %101, !dbg !1604

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1604
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1604
  store ptr %49, ptr %5, align 8, !dbg !1604
  %50 = load i32, ptr %48, align 8, !dbg !1604
  %51 = trunc i32 %50 to i16, !dbg !1604
  %52 = load i32, ptr %12, align 4, !dbg !1604
  %53 = sext i32 %52 to i64, !dbg !1604
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1604
  store i16 %51, ptr %54, align 8, !dbg !1604
  br label %101, !dbg !1604

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1604
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1604
  store ptr %57, ptr %5, align 8, !dbg !1604
  %58 = load i32, ptr %56, align 8, !dbg !1604
  %59 = trunc i32 %58 to i16, !dbg !1604
  %60 = load i32, ptr %12, align 4, !dbg !1604
  %61 = sext i32 %60 to i64, !dbg !1604
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1604
  store i16 %59, ptr %62, align 8, !dbg !1604
  br label %101, !dbg !1604

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1604
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1604
  store ptr %65, ptr %5, align 8, !dbg !1604
  %66 = load i32, ptr %64, align 8, !dbg !1604
  %67 = load i32, ptr %12, align 4, !dbg !1604
  %68 = sext i32 %67 to i64, !dbg !1604
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1604
  store i32 %66, ptr %69, align 8, !dbg !1604
  br label %101, !dbg !1604

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1604
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1604
  store ptr %72, ptr %5, align 8, !dbg !1604
  %73 = load i32, ptr %71, align 8, !dbg !1604
  %74 = sext i32 %73 to i64, !dbg !1604
  %75 = load i32, ptr %12, align 4, !dbg !1604
  %76 = sext i32 %75 to i64, !dbg !1604
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1604
  store i64 %74, ptr %77, align 8, !dbg !1604
  br label %101, !dbg !1604

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1604
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1604
  store ptr %80, ptr %5, align 8, !dbg !1604
  %81 = load double, ptr %79, align 8, !dbg !1604
  %82 = fptrunc double %81 to float, !dbg !1604
  %83 = load i32, ptr %12, align 4, !dbg !1604
  %84 = sext i32 %83 to i64, !dbg !1604
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1604
  store float %82, ptr %85, align 8, !dbg !1604
  br label %101, !dbg !1604

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1604
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1604
  store ptr %88, ptr %5, align 8, !dbg !1604
  %89 = load double, ptr %87, align 8, !dbg !1604
  %90 = load i32, ptr %12, align 4, !dbg !1604
  %91 = sext i32 %90 to i64, !dbg !1604
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1604
  store double %89, ptr %92, align 8, !dbg !1604
  br label %101, !dbg !1604

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1604
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1604
  store ptr %95, ptr %5, align 8, !dbg !1604
  %96 = load ptr, ptr %94, align 8, !dbg !1604
  %97 = load i32, ptr %12, align 4, !dbg !1604
  %98 = sext i32 %97 to i64, !dbg !1604
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1604
  store ptr %96, ptr %99, align 8, !dbg !1604
  br label %101, !dbg !1604

100:                                              ; preds = %25
  br label %101, !dbg !1604

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1601

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1606
  %104 = add nsw i32 %103, 1, !dbg !1606
  store i32 %104, ptr %12, align 4, !dbg !1606
  br label %21, !dbg !1606, !llvm.loop !1607

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1591
  %107 = load ptr, ptr %106, align 8, !dbg !1591
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 54, !dbg !1591
  %109 = load ptr, ptr %108, align 8, !dbg !1591
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1591
  %111 = load ptr, ptr %6, align 8, !dbg !1591
  %112 = load ptr, ptr %7, align 8, !dbg !1591
  %113 = load ptr, ptr %8, align 8, !dbg !1591
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1591
  ret i64 %114, !dbg !1591
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1608 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca i64, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1609, metadata !DIExpression()), !dbg !1610
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1611, metadata !DIExpression()), !dbg !1610
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1612, metadata !DIExpression()), !dbg !1610
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1613, metadata !DIExpression()), !dbg !1610
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1614, metadata !DIExpression()), !dbg !1610
  call void @llvm.va_start(ptr %9), !dbg !1610
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1615, metadata !DIExpression()), !dbg !1610
  %11 = load ptr, ptr %9, align 8, !dbg !1610
  %12 = load ptr, ptr %5, align 8, !dbg !1610
  %13 = load ptr, ptr %6, align 8, !dbg !1610
  %14 = load ptr, ptr %7, align 8, !dbg !1610
  %15 = load ptr, ptr %8, align 8, !dbg !1610
  %16 = call i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1610
  store i64 %16, ptr %10, align 8, !dbg !1610
  call void @llvm.va_end(ptr %9), !dbg !1610
  %17 = load i64, ptr %10, align 8, !dbg !1610
  ret i64 %17, !dbg !1610
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1616 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1617, metadata !DIExpression()), !dbg !1618
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1619, metadata !DIExpression()), !dbg !1618
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1620, metadata !DIExpression()), !dbg !1618
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1621, metadata !DIExpression()), !dbg !1618
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1622, metadata !DIExpression()), !dbg !1618
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1623, metadata !DIExpression()), !dbg !1618
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1624, metadata !DIExpression()), !dbg !1618
  %15 = load ptr, ptr %10, align 8, !dbg !1618
  %16 = load ptr, ptr %15, align 8, !dbg !1618
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1618
  %18 = load ptr, ptr %17, align 8, !dbg !1618
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1618
  %20 = load ptr, ptr %7, align 8, !dbg !1618
  %21 = load ptr, ptr %10, align 8, !dbg !1618
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1618
  store i32 %22, ptr %12, align 4, !dbg !1618
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1625, metadata !DIExpression()), !dbg !1618
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1626, metadata !DIExpression()), !dbg !1628
  store i32 0, ptr %14, align 4, !dbg !1628
  br label %23, !dbg !1628

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1628
  %25 = load i32, ptr %12, align 4, !dbg !1628
  %26 = icmp slt i32 %24, %25, !dbg !1628
  br i1 %26, label %27, label %107, !dbg !1628

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1629
  %29 = sext i32 %28 to i64, !dbg !1629
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1629
  %31 = load i8, ptr %30, align 1, !dbg !1629
  %32 = sext i8 %31 to i32, !dbg !1629
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1629

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1632
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1632
  store ptr %35, ptr %6, align 8, !dbg !1632
  %36 = load i32, ptr %34, align 8, !dbg !1632
  %37 = trunc i32 %36 to i8, !dbg !1632
  %38 = load i32, ptr %14, align 4, !dbg !1632
  %39 = sext i32 %38 to i64, !dbg !1632
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1632
  store i8 %37, ptr %40, align 8, !dbg !1632
  br label %103, !dbg !1632

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1632
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1632
  store ptr %43, ptr %6, align 8, !dbg !1632
  %44 = load i32, ptr %42, align 8, !dbg !1632
  %45 = trunc i32 %44 to i8, !dbg !1632
  %46 = load i32, ptr %14, align 4, !dbg !1632
  %47 = sext i32 %46 to i64, !dbg !1632
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1632
  store i8 %45, ptr %48, align 8, !dbg !1632
  br label %103, !dbg !1632

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1632
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1632
  store ptr %51, ptr %6, align 8, !dbg !1632
  %52 = load i32, ptr %50, align 8, !dbg !1632
  %53 = trunc i32 %52 to i16, !dbg !1632
  %54 = load i32, ptr %14, align 4, !dbg !1632
  %55 = sext i32 %54 to i64, !dbg !1632
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1632
  store i16 %53, ptr %56, align 8, !dbg !1632
  br label %103, !dbg !1632

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1632
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1632
  store ptr %59, ptr %6, align 8, !dbg !1632
  %60 = load i32, ptr %58, align 8, !dbg !1632
  %61 = trunc i32 %60 to i16, !dbg !1632
  %62 = load i32, ptr %14, align 4, !dbg !1632
  %63 = sext i32 %62 to i64, !dbg !1632
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1632
  store i16 %61, ptr %64, align 8, !dbg !1632
  br label %103, !dbg !1632

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1632
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1632
  store ptr %67, ptr %6, align 8, !dbg !1632
  %68 = load i32, ptr %66, align 8, !dbg !1632
  %69 = load i32, ptr %14, align 4, !dbg !1632
  %70 = sext i32 %69 to i64, !dbg !1632
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1632
  store i32 %68, ptr %71, align 8, !dbg !1632
  br label %103, !dbg !1632

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1632
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1632
  store ptr %74, ptr %6, align 8, !dbg !1632
  %75 = load i32, ptr %73, align 8, !dbg !1632
  %76 = sext i32 %75 to i64, !dbg !1632
  %77 = load i32, ptr %14, align 4, !dbg !1632
  %78 = sext i32 %77 to i64, !dbg !1632
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1632
  store i64 %76, ptr %79, align 8, !dbg !1632
  br label %103, !dbg !1632

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1632
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1632
  store ptr %82, ptr %6, align 8, !dbg !1632
  %83 = load double, ptr %81, align 8, !dbg !1632
  %84 = fptrunc double %83 to float, !dbg !1632
  %85 = load i32, ptr %14, align 4, !dbg !1632
  %86 = sext i32 %85 to i64, !dbg !1632
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1632
  store float %84, ptr %87, align 8, !dbg !1632
  br label %103, !dbg !1632

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1632
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1632
  store ptr %90, ptr %6, align 8, !dbg !1632
  %91 = load double, ptr %89, align 8, !dbg !1632
  %92 = load i32, ptr %14, align 4, !dbg !1632
  %93 = sext i32 %92 to i64, !dbg !1632
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1632
  store double %91, ptr %94, align 8, !dbg !1632
  br label %103, !dbg !1632

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1632
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1632
  store ptr %97, ptr %6, align 8, !dbg !1632
  %98 = load ptr, ptr %96, align 8, !dbg !1632
  %99 = load i32, ptr %14, align 4, !dbg !1632
  %100 = sext i32 %99 to i64, !dbg !1632
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1632
  store ptr %98, ptr %101, align 8, !dbg !1632
  br label %103, !dbg !1632

102:                                              ; preds = %27
  br label %103, !dbg !1632

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1629

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1634
  %106 = add nsw i32 %105, 1, !dbg !1634
  store i32 %106, ptr %14, align 4, !dbg !1634
  br label %23, !dbg !1634, !llvm.loop !1635

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1618
  %109 = load ptr, ptr %108, align 8, !dbg !1618
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 84, !dbg !1618
  %111 = load ptr, ptr %110, align 8, !dbg !1618
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1618
  %113 = load ptr, ptr %7, align 8, !dbg !1618
  %114 = load ptr, ptr %8, align 8, !dbg !1618
  %115 = load ptr, ptr %9, align 8, !dbg !1618
  %116 = load ptr, ptr %10, align 8, !dbg !1618
  %117 = call i64 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1618
  ret i64 %117, !dbg !1618
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1636 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1637, metadata !DIExpression()), !dbg !1638
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1639, metadata !DIExpression()), !dbg !1638
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1640, metadata !DIExpression()), !dbg !1638
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1641, metadata !DIExpression()), !dbg !1638
  call void @llvm.va_start(ptr %7), !dbg !1638
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1642, metadata !DIExpression()), !dbg !1638
  %9 = load ptr, ptr %7, align 8, !dbg !1638
  %10 = load ptr, ptr %4, align 8, !dbg !1638
  %11 = load ptr, ptr %5, align 8, !dbg !1638
  %12 = load ptr, ptr %6, align 8, !dbg !1638
  %13 = call i64 @JNI_CallStaticLongMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1638
  store i64 %13, ptr %8, align 8, !dbg !1638
  call void @llvm.va_end(ptr %7), !dbg !1638
  %14 = load i64, ptr %8, align 8, !dbg !1638
  ret i64 %14, !dbg !1638
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1643 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1644, metadata !DIExpression()), !dbg !1645
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1646, metadata !DIExpression()), !dbg !1645
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1647, metadata !DIExpression()), !dbg !1645
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1648, metadata !DIExpression()), !dbg !1645
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1649, metadata !DIExpression()), !dbg !1645
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1650, metadata !DIExpression()), !dbg !1645
  %13 = load ptr, ptr %8, align 8, !dbg !1645
  %14 = load ptr, ptr %13, align 8, !dbg !1645
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1645
  %16 = load ptr, ptr %15, align 8, !dbg !1645
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1645
  %18 = load ptr, ptr %6, align 8, !dbg !1645
  %19 = load ptr, ptr %8, align 8, !dbg !1645
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1645
  store i32 %20, ptr %10, align 4, !dbg !1645
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1651, metadata !DIExpression()), !dbg !1645
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1652, metadata !DIExpression()), !dbg !1654
  store i32 0, ptr %12, align 4, !dbg !1654
  br label %21, !dbg !1654

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1654
  %23 = load i32, ptr %10, align 4, !dbg !1654
  %24 = icmp slt i32 %22, %23, !dbg !1654
  br i1 %24, label %25, label %105, !dbg !1654

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1655
  %27 = sext i32 %26 to i64, !dbg !1655
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1655
  %29 = load i8, ptr %28, align 1, !dbg !1655
  %30 = sext i8 %29 to i32, !dbg !1655
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1655

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1658
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1658
  store ptr %33, ptr %5, align 8, !dbg !1658
  %34 = load i32, ptr %32, align 8, !dbg !1658
  %35 = trunc i32 %34 to i8, !dbg !1658
  %36 = load i32, ptr %12, align 4, !dbg !1658
  %37 = sext i32 %36 to i64, !dbg !1658
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1658
  store i8 %35, ptr %38, align 8, !dbg !1658
  br label %101, !dbg !1658

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1658
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1658
  store ptr %41, ptr %5, align 8, !dbg !1658
  %42 = load i32, ptr %40, align 8, !dbg !1658
  %43 = trunc i32 %42 to i8, !dbg !1658
  %44 = load i32, ptr %12, align 4, !dbg !1658
  %45 = sext i32 %44 to i64, !dbg !1658
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1658
  store i8 %43, ptr %46, align 8, !dbg !1658
  br label %101, !dbg !1658

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1658
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1658
  store ptr %49, ptr %5, align 8, !dbg !1658
  %50 = load i32, ptr %48, align 8, !dbg !1658
  %51 = trunc i32 %50 to i16, !dbg !1658
  %52 = load i32, ptr %12, align 4, !dbg !1658
  %53 = sext i32 %52 to i64, !dbg !1658
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1658
  store i16 %51, ptr %54, align 8, !dbg !1658
  br label %101, !dbg !1658

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1658
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1658
  store ptr %57, ptr %5, align 8, !dbg !1658
  %58 = load i32, ptr %56, align 8, !dbg !1658
  %59 = trunc i32 %58 to i16, !dbg !1658
  %60 = load i32, ptr %12, align 4, !dbg !1658
  %61 = sext i32 %60 to i64, !dbg !1658
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1658
  store i16 %59, ptr %62, align 8, !dbg !1658
  br label %101, !dbg !1658

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1658
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1658
  store ptr %65, ptr %5, align 8, !dbg !1658
  %66 = load i32, ptr %64, align 8, !dbg !1658
  %67 = load i32, ptr %12, align 4, !dbg !1658
  %68 = sext i32 %67 to i64, !dbg !1658
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1658
  store i32 %66, ptr %69, align 8, !dbg !1658
  br label %101, !dbg !1658

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1658
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1658
  store ptr %72, ptr %5, align 8, !dbg !1658
  %73 = load i32, ptr %71, align 8, !dbg !1658
  %74 = sext i32 %73 to i64, !dbg !1658
  %75 = load i32, ptr %12, align 4, !dbg !1658
  %76 = sext i32 %75 to i64, !dbg !1658
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1658
  store i64 %74, ptr %77, align 8, !dbg !1658
  br label %101, !dbg !1658

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1658
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1658
  store ptr %80, ptr %5, align 8, !dbg !1658
  %81 = load double, ptr %79, align 8, !dbg !1658
  %82 = fptrunc double %81 to float, !dbg !1658
  %83 = load i32, ptr %12, align 4, !dbg !1658
  %84 = sext i32 %83 to i64, !dbg !1658
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1658
  store float %82, ptr %85, align 8, !dbg !1658
  br label %101, !dbg !1658

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1658
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1658
  store ptr %88, ptr %5, align 8, !dbg !1658
  %89 = load double, ptr %87, align 8, !dbg !1658
  %90 = load i32, ptr %12, align 4, !dbg !1658
  %91 = sext i32 %90 to i64, !dbg !1658
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1658
  store double %89, ptr %92, align 8, !dbg !1658
  br label %101, !dbg !1658

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1658
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1658
  store ptr %95, ptr %5, align 8, !dbg !1658
  %96 = load ptr, ptr %94, align 8, !dbg !1658
  %97 = load i32, ptr %12, align 4, !dbg !1658
  %98 = sext i32 %97 to i64, !dbg !1658
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1658
  store ptr %96, ptr %99, align 8, !dbg !1658
  br label %101, !dbg !1658

100:                                              ; preds = %25
  br label %101, !dbg !1658

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1655

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1660
  %104 = add nsw i32 %103, 1, !dbg !1660
  store i32 %104, ptr %12, align 4, !dbg !1660
  br label %21, !dbg !1660, !llvm.loop !1661

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1645
  %107 = load ptr, ptr %106, align 8, !dbg !1645
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 134, !dbg !1645
  %109 = load ptr, ptr %108, align 8, !dbg !1645
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1645
  %111 = load ptr, ptr %6, align 8, !dbg !1645
  %112 = load ptr, ptr %7, align 8, !dbg !1645
  %113 = load ptr, ptr %8, align 8, !dbg !1645
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1645
  ret i64 %114, !dbg !1645
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1662 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1663, metadata !DIExpression()), !dbg !1664
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1665, metadata !DIExpression()), !dbg !1664
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1666, metadata !DIExpression()), !dbg !1664
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1667, metadata !DIExpression()), !dbg !1664
  call void @llvm.va_start(ptr %7), !dbg !1664
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1668, metadata !DIExpression()), !dbg !1664
  %9 = load ptr, ptr %7, align 8, !dbg !1664
  %10 = load ptr, ptr %4, align 8, !dbg !1664
  %11 = load ptr, ptr %5, align 8, !dbg !1664
  %12 = load ptr, ptr %6, align 8, !dbg !1664
  %13 = call float @JNI_CallFloatMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1664
  store float %13, ptr %8, align 4, !dbg !1664
  call void @llvm.va_end(ptr %7), !dbg !1664
  %14 = load float, ptr %8, align 4, !dbg !1664
  ret float %14, !dbg !1664
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1669 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1670, metadata !DIExpression()), !dbg !1671
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1672, metadata !DIExpression()), !dbg !1671
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1673, metadata !DIExpression()), !dbg !1671
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1674, metadata !DIExpression()), !dbg !1671
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1675, metadata !DIExpression()), !dbg !1671
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1676, metadata !DIExpression()), !dbg !1671
  %13 = load ptr, ptr %8, align 8, !dbg !1671
  %14 = load ptr, ptr %13, align 8, !dbg !1671
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1671
  %16 = load ptr, ptr %15, align 8, !dbg !1671
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1671
  %18 = load ptr, ptr %6, align 8, !dbg !1671
  %19 = load ptr, ptr %8, align 8, !dbg !1671
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1671
  store i32 %20, ptr %10, align 4, !dbg !1671
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1677, metadata !DIExpression()), !dbg !1671
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1678, metadata !DIExpression()), !dbg !1680
  store i32 0, ptr %12, align 4, !dbg !1680
  br label %21, !dbg !1680

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1680
  %23 = load i32, ptr %10, align 4, !dbg !1680
  %24 = icmp slt i32 %22, %23, !dbg !1680
  br i1 %24, label %25, label %105, !dbg !1680

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1681
  %27 = sext i32 %26 to i64, !dbg !1681
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1681
  %29 = load i8, ptr %28, align 1, !dbg !1681
  %30 = sext i8 %29 to i32, !dbg !1681
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1681

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1684
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1684
  store ptr %33, ptr %5, align 8, !dbg !1684
  %34 = load i32, ptr %32, align 8, !dbg !1684
  %35 = trunc i32 %34 to i8, !dbg !1684
  %36 = load i32, ptr %12, align 4, !dbg !1684
  %37 = sext i32 %36 to i64, !dbg !1684
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1684
  store i8 %35, ptr %38, align 8, !dbg !1684
  br label %101, !dbg !1684

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1684
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1684
  store ptr %41, ptr %5, align 8, !dbg !1684
  %42 = load i32, ptr %40, align 8, !dbg !1684
  %43 = trunc i32 %42 to i8, !dbg !1684
  %44 = load i32, ptr %12, align 4, !dbg !1684
  %45 = sext i32 %44 to i64, !dbg !1684
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1684
  store i8 %43, ptr %46, align 8, !dbg !1684
  br label %101, !dbg !1684

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1684
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1684
  store ptr %49, ptr %5, align 8, !dbg !1684
  %50 = load i32, ptr %48, align 8, !dbg !1684
  %51 = trunc i32 %50 to i16, !dbg !1684
  %52 = load i32, ptr %12, align 4, !dbg !1684
  %53 = sext i32 %52 to i64, !dbg !1684
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1684
  store i16 %51, ptr %54, align 8, !dbg !1684
  br label %101, !dbg !1684

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1684
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1684
  store ptr %57, ptr %5, align 8, !dbg !1684
  %58 = load i32, ptr %56, align 8, !dbg !1684
  %59 = trunc i32 %58 to i16, !dbg !1684
  %60 = load i32, ptr %12, align 4, !dbg !1684
  %61 = sext i32 %60 to i64, !dbg !1684
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1684
  store i16 %59, ptr %62, align 8, !dbg !1684
  br label %101, !dbg !1684

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1684
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1684
  store ptr %65, ptr %5, align 8, !dbg !1684
  %66 = load i32, ptr %64, align 8, !dbg !1684
  %67 = load i32, ptr %12, align 4, !dbg !1684
  %68 = sext i32 %67 to i64, !dbg !1684
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1684
  store i32 %66, ptr %69, align 8, !dbg !1684
  br label %101, !dbg !1684

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1684
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1684
  store ptr %72, ptr %5, align 8, !dbg !1684
  %73 = load i32, ptr %71, align 8, !dbg !1684
  %74 = sext i32 %73 to i64, !dbg !1684
  %75 = load i32, ptr %12, align 4, !dbg !1684
  %76 = sext i32 %75 to i64, !dbg !1684
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1684
  store i64 %74, ptr %77, align 8, !dbg !1684
  br label %101, !dbg !1684

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1684
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1684
  store ptr %80, ptr %5, align 8, !dbg !1684
  %81 = load double, ptr %79, align 8, !dbg !1684
  %82 = fptrunc double %81 to float, !dbg !1684
  %83 = load i32, ptr %12, align 4, !dbg !1684
  %84 = sext i32 %83 to i64, !dbg !1684
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1684
  store float %82, ptr %85, align 8, !dbg !1684
  br label %101, !dbg !1684

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1684
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1684
  store ptr %88, ptr %5, align 8, !dbg !1684
  %89 = load double, ptr %87, align 8, !dbg !1684
  %90 = load i32, ptr %12, align 4, !dbg !1684
  %91 = sext i32 %90 to i64, !dbg !1684
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1684
  store double %89, ptr %92, align 8, !dbg !1684
  br label %101, !dbg !1684

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1684
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1684
  store ptr %95, ptr %5, align 8, !dbg !1684
  %96 = load ptr, ptr %94, align 8, !dbg !1684
  %97 = load i32, ptr %12, align 4, !dbg !1684
  %98 = sext i32 %97 to i64, !dbg !1684
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1684
  store ptr %96, ptr %99, align 8, !dbg !1684
  br label %101, !dbg !1684

100:                                              ; preds = %25
  br label %101, !dbg !1684

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1681

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1686
  %104 = add nsw i32 %103, 1, !dbg !1686
  store i32 %104, ptr %12, align 4, !dbg !1686
  br label %21, !dbg !1686, !llvm.loop !1687

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1671
  %107 = load ptr, ptr %106, align 8, !dbg !1671
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 57, !dbg !1671
  %109 = load ptr, ptr %108, align 8, !dbg !1671
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1671
  %111 = load ptr, ptr %6, align 8, !dbg !1671
  %112 = load ptr, ptr %7, align 8, !dbg !1671
  %113 = load ptr, ptr %8, align 8, !dbg !1671
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1671
  ret float %114, !dbg !1671
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1688 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca float, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1689, metadata !DIExpression()), !dbg !1690
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1691, metadata !DIExpression()), !dbg !1690
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1692, metadata !DIExpression()), !dbg !1690
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1693, metadata !DIExpression()), !dbg !1690
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1694, metadata !DIExpression()), !dbg !1690
  call void @llvm.va_start(ptr %9), !dbg !1690
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1695, metadata !DIExpression()), !dbg !1690
  %11 = load ptr, ptr %9, align 8, !dbg !1690
  %12 = load ptr, ptr %5, align 8, !dbg !1690
  %13 = load ptr, ptr %6, align 8, !dbg !1690
  %14 = load ptr, ptr %7, align 8, !dbg !1690
  %15 = load ptr, ptr %8, align 8, !dbg !1690
  %16 = call float @JNI_CallNonvirtualFloatMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1690
  store float %16, ptr %10, align 4, !dbg !1690
  call void @llvm.va_end(ptr %9), !dbg !1690
  %17 = load float, ptr %10, align 4, !dbg !1690
  ret float %17, !dbg !1690
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1696 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1697, metadata !DIExpression()), !dbg !1698
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1699, metadata !DIExpression()), !dbg !1698
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1700, metadata !DIExpression()), !dbg !1698
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1701, metadata !DIExpression()), !dbg !1698
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1702, metadata !DIExpression()), !dbg !1698
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1703, metadata !DIExpression()), !dbg !1698
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1704, metadata !DIExpression()), !dbg !1698
  %15 = load ptr, ptr %10, align 8, !dbg !1698
  %16 = load ptr, ptr %15, align 8, !dbg !1698
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1698
  %18 = load ptr, ptr %17, align 8, !dbg !1698
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1698
  %20 = load ptr, ptr %7, align 8, !dbg !1698
  %21 = load ptr, ptr %10, align 8, !dbg !1698
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1698
  store i32 %22, ptr %12, align 4, !dbg !1698
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1705, metadata !DIExpression()), !dbg !1698
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1706, metadata !DIExpression()), !dbg !1708
  store i32 0, ptr %14, align 4, !dbg !1708
  br label %23, !dbg !1708

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1708
  %25 = load i32, ptr %12, align 4, !dbg !1708
  %26 = icmp slt i32 %24, %25, !dbg !1708
  br i1 %26, label %27, label %107, !dbg !1708

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1709
  %29 = sext i32 %28 to i64, !dbg !1709
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1709
  %31 = load i8, ptr %30, align 1, !dbg !1709
  %32 = sext i8 %31 to i32, !dbg !1709
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1709

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1712
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1712
  store ptr %35, ptr %6, align 8, !dbg !1712
  %36 = load i32, ptr %34, align 8, !dbg !1712
  %37 = trunc i32 %36 to i8, !dbg !1712
  %38 = load i32, ptr %14, align 4, !dbg !1712
  %39 = sext i32 %38 to i64, !dbg !1712
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1712
  store i8 %37, ptr %40, align 8, !dbg !1712
  br label %103, !dbg !1712

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1712
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1712
  store ptr %43, ptr %6, align 8, !dbg !1712
  %44 = load i32, ptr %42, align 8, !dbg !1712
  %45 = trunc i32 %44 to i8, !dbg !1712
  %46 = load i32, ptr %14, align 4, !dbg !1712
  %47 = sext i32 %46 to i64, !dbg !1712
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1712
  store i8 %45, ptr %48, align 8, !dbg !1712
  br label %103, !dbg !1712

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1712
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1712
  store ptr %51, ptr %6, align 8, !dbg !1712
  %52 = load i32, ptr %50, align 8, !dbg !1712
  %53 = trunc i32 %52 to i16, !dbg !1712
  %54 = load i32, ptr %14, align 4, !dbg !1712
  %55 = sext i32 %54 to i64, !dbg !1712
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1712
  store i16 %53, ptr %56, align 8, !dbg !1712
  br label %103, !dbg !1712

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1712
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1712
  store ptr %59, ptr %6, align 8, !dbg !1712
  %60 = load i32, ptr %58, align 8, !dbg !1712
  %61 = trunc i32 %60 to i16, !dbg !1712
  %62 = load i32, ptr %14, align 4, !dbg !1712
  %63 = sext i32 %62 to i64, !dbg !1712
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1712
  store i16 %61, ptr %64, align 8, !dbg !1712
  br label %103, !dbg !1712

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1712
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1712
  store ptr %67, ptr %6, align 8, !dbg !1712
  %68 = load i32, ptr %66, align 8, !dbg !1712
  %69 = load i32, ptr %14, align 4, !dbg !1712
  %70 = sext i32 %69 to i64, !dbg !1712
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1712
  store i32 %68, ptr %71, align 8, !dbg !1712
  br label %103, !dbg !1712

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1712
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1712
  store ptr %74, ptr %6, align 8, !dbg !1712
  %75 = load i32, ptr %73, align 8, !dbg !1712
  %76 = sext i32 %75 to i64, !dbg !1712
  %77 = load i32, ptr %14, align 4, !dbg !1712
  %78 = sext i32 %77 to i64, !dbg !1712
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1712
  store i64 %76, ptr %79, align 8, !dbg !1712
  br label %103, !dbg !1712

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1712
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1712
  store ptr %82, ptr %6, align 8, !dbg !1712
  %83 = load double, ptr %81, align 8, !dbg !1712
  %84 = fptrunc double %83 to float, !dbg !1712
  %85 = load i32, ptr %14, align 4, !dbg !1712
  %86 = sext i32 %85 to i64, !dbg !1712
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1712
  store float %84, ptr %87, align 8, !dbg !1712
  br label %103, !dbg !1712

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1712
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1712
  store ptr %90, ptr %6, align 8, !dbg !1712
  %91 = load double, ptr %89, align 8, !dbg !1712
  %92 = load i32, ptr %14, align 4, !dbg !1712
  %93 = sext i32 %92 to i64, !dbg !1712
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1712
  store double %91, ptr %94, align 8, !dbg !1712
  br label %103, !dbg !1712

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1712
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1712
  store ptr %97, ptr %6, align 8, !dbg !1712
  %98 = load ptr, ptr %96, align 8, !dbg !1712
  %99 = load i32, ptr %14, align 4, !dbg !1712
  %100 = sext i32 %99 to i64, !dbg !1712
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1712
  store ptr %98, ptr %101, align 8, !dbg !1712
  br label %103, !dbg !1712

102:                                              ; preds = %27
  br label %103, !dbg !1712

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1709

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1714
  %106 = add nsw i32 %105, 1, !dbg !1714
  store i32 %106, ptr %14, align 4, !dbg !1714
  br label %23, !dbg !1714, !llvm.loop !1715

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1698
  %109 = load ptr, ptr %108, align 8, !dbg !1698
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 87, !dbg !1698
  %111 = load ptr, ptr %110, align 8, !dbg !1698
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1698
  %113 = load ptr, ptr %7, align 8, !dbg !1698
  %114 = load ptr, ptr %8, align 8, !dbg !1698
  %115 = load ptr, ptr %9, align 8, !dbg !1698
  %116 = load ptr, ptr %10, align 8, !dbg !1698
  %117 = call float %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1698
  ret float %117, !dbg !1698
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1716 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1717, metadata !DIExpression()), !dbg !1718
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1719, metadata !DIExpression()), !dbg !1718
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1720, metadata !DIExpression()), !dbg !1718
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1721, metadata !DIExpression()), !dbg !1718
  call void @llvm.va_start(ptr %7), !dbg !1718
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1722, metadata !DIExpression()), !dbg !1718
  %9 = load ptr, ptr %7, align 8, !dbg !1718
  %10 = load ptr, ptr %4, align 8, !dbg !1718
  %11 = load ptr, ptr %5, align 8, !dbg !1718
  %12 = load ptr, ptr %6, align 8, !dbg !1718
  %13 = call float @JNI_CallStaticFloatMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1718
  store float %13, ptr %8, align 4, !dbg !1718
  call void @llvm.va_end(ptr %7), !dbg !1718
  %14 = load float, ptr %8, align 4, !dbg !1718
  ret float %14, !dbg !1718
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1723 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1724, metadata !DIExpression()), !dbg !1725
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1726, metadata !DIExpression()), !dbg !1725
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1727, metadata !DIExpression()), !dbg !1725
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1728, metadata !DIExpression()), !dbg !1725
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1729, metadata !DIExpression()), !dbg !1725
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1730, metadata !DIExpression()), !dbg !1725
  %13 = load ptr, ptr %8, align 8, !dbg !1725
  %14 = load ptr, ptr %13, align 8, !dbg !1725
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1725
  %16 = load ptr, ptr %15, align 8, !dbg !1725
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1725
  %18 = load ptr, ptr %6, align 8, !dbg !1725
  %19 = load ptr, ptr %8, align 8, !dbg !1725
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1725
  store i32 %20, ptr %10, align 4, !dbg !1725
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1731, metadata !DIExpression()), !dbg !1725
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1732, metadata !DIExpression()), !dbg !1734
  store i32 0, ptr %12, align 4, !dbg !1734
  br label %21, !dbg !1734

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1734
  %23 = load i32, ptr %10, align 4, !dbg !1734
  %24 = icmp slt i32 %22, %23, !dbg !1734
  br i1 %24, label %25, label %105, !dbg !1734

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1735
  %27 = sext i32 %26 to i64, !dbg !1735
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1735
  %29 = load i8, ptr %28, align 1, !dbg !1735
  %30 = sext i8 %29 to i32, !dbg !1735
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1735

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1738
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1738
  store ptr %33, ptr %5, align 8, !dbg !1738
  %34 = load i32, ptr %32, align 8, !dbg !1738
  %35 = trunc i32 %34 to i8, !dbg !1738
  %36 = load i32, ptr %12, align 4, !dbg !1738
  %37 = sext i32 %36 to i64, !dbg !1738
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1738
  store i8 %35, ptr %38, align 8, !dbg !1738
  br label %101, !dbg !1738

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1738
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1738
  store ptr %41, ptr %5, align 8, !dbg !1738
  %42 = load i32, ptr %40, align 8, !dbg !1738
  %43 = trunc i32 %42 to i8, !dbg !1738
  %44 = load i32, ptr %12, align 4, !dbg !1738
  %45 = sext i32 %44 to i64, !dbg !1738
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1738
  store i8 %43, ptr %46, align 8, !dbg !1738
  br label %101, !dbg !1738

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1738
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1738
  store ptr %49, ptr %5, align 8, !dbg !1738
  %50 = load i32, ptr %48, align 8, !dbg !1738
  %51 = trunc i32 %50 to i16, !dbg !1738
  %52 = load i32, ptr %12, align 4, !dbg !1738
  %53 = sext i32 %52 to i64, !dbg !1738
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1738
  store i16 %51, ptr %54, align 8, !dbg !1738
  br label %101, !dbg !1738

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1738
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1738
  store ptr %57, ptr %5, align 8, !dbg !1738
  %58 = load i32, ptr %56, align 8, !dbg !1738
  %59 = trunc i32 %58 to i16, !dbg !1738
  %60 = load i32, ptr %12, align 4, !dbg !1738
  %61 = sext i32 %60 to i64, !dbg !1738
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1738
  store i16 %59, ptr %62, align 8, !dbg !1738
  br label %101, !dbg !1738

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1738
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1738
  store ptr %65, ptr %5, align 8, !dbg !1738
  %66 = load i32, ptr %64, align 8, !dbg !1738
  %67 = load i32, ptr %12, align 4, !dbg !1738
  %68 = sext i32 %67 to i64, !dbg !1738
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1738
  store i32 %66, ptr %69, align 8, !dbg !1738
  br label %101, !dbg !1738

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1738
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1738
  store ptr %72, ptr %5, align 8, !dbg !1738
  %73 = load i32, ptr %71, align 8, !dbg !1738
  %74 = sext i32 %73 to i64, !dbg !1738
  %75 = load i32, ptr %12, align 4, !dbg !1738
  %76 = sext i32 %75 to i64, !dbg !1738
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1738
  store i64 %74, ptr %77, align 8, !dbg !1738
  br label %101, !dbg !1738

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1738
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1738
  store ptr %80, ptr %5, align 8, !dbg !1738
  %81 = load double, ptr %79, align 8, !dbg !1738
  %82 = fptrunc double %81 to float, !dbg !1738
  %83 = load i32, ptr %12, align 4, !dbg !1738
  %84 = sext i32 %83 to i64, !dbg !1738
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1738
  store float %82, ptr %85, align 8, !dbg !1738
  br label %101, !dbg !1738

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1738
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1738
  store ptr %88, ptr %5, align 8, !dbg !1738
  %89 = load double, ptr %87, align 8, !dbg !1738
  %90 = load i32, ptr %12, align 4, !dbg !1738
  %91 = sext i32 %90 to i64, !dbg !1738
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1738
  store double %89, ptr %92, align 8, !dbg !1738
  br label %101, !dbg !1738

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1738
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1738
  store ptr %95, ptr %5, align 8, !dbg !1738
  %96 = load ptr, ptr %94, align 8, !dbg !1738
  %97 = load i32, ptr %12, align 4, !dbg !1738
  %98 = sext i32 %97 to i64, !dbg !1738
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1738
  store ptr %96, ptr %99, align 8, !dbg !1738
  br label %101, !dbg !1738

100:                                              ; preds = %25
  br label %101, !dbg !1738

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1735

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1740
  %104 = add nsw i32 %103, 1, !dbg !1740
  store i32 %104, ptr %12, align 4, !dbg !1740
  br label %21, !dbg !1740, !llvm.loop !1741

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1725
  %107 = load ptr, ptr %106, align 8, !dbg !1725
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 137, !dbg !1725
  %109 = load ptr, ptr %108, align 8, !dbg !1725
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1725
  %111 = load ptr, ptr %6, align 8, !dbg !1725
  %112 = load ptr, ptr %7, align 8, !dbg !1725
  %113 = load ptr, ptr %8, align 8, !dbg !1725
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1725
  ret float %114, !dbg !1725
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1742 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1743, metadata !DIExpression()), !dbg !1744
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1745, metadata !DIExpression()), !dbg !1744
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1746, metadata !DIExpression()), !dbg !1744
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1747, metadata !DIExpression()), !dbg !1744
  call void @llvm.va_start(ptr %7), !dbg !1744
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1748, metadata !DIExpression()), !dbg !1744
  %9 = load ptr, ptr %7, align 8, !dbg !1744
  %10 = load ptr, ptr %4, align 8, !dbg !1744
  %11 = load ptr, ptr %5, align 8, !dbg !1744
  %12 = load ptr, ptr %6, align 8, !dbg !1744
  %13 = call double @JNI_CallDoubleMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1744
  store double %13, ptr %8, align 8, !dbg !1744
  call void @llvm.va_end(ptr %7), !dbg !1744
  %14 = load double, ptr %8, align 8, !dbg !1744
  ret double %14, !dbg !1744
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1749 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1750, metadata !DIExpression()), !dbg !1751
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1752, metadata !DIExpression()), !dbg !1751
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1753, metadata !DIExpression()), !dbg !1751
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1754, metadata !DIExpression()), !dbg !1751
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1755, metadata !DIExpression()), !dbg !1751
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1756, metadata !DIExpression()), !dbg !1751
  %13 = load ptr, ptr %8, align 8, !dbg !1751
  %14 = load ptr, ptr %13, align 8, !dbg !1751
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1751
  %16 = load ptr, ptr %15, align 8, !dbg !1751
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1751
  %18 = load ptr, ptr %6, align 8, !dbg !1751
  %19 = load ptr, ptr %8, align 8, !dbg !1751
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1751
  store i32 %20, ptr %10, align 4, !dbg !1751
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1757, metadata !DIExpression()), !dbg !1751
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1758, metadata !DIExpression()), !dbg !1760
  store i32 0, ptr %12, align 4, !dbg !1760
  br label %21, !dbg !1760

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1760
  %23 = load i32, ptr %10, align 4, !dbg !1760
  %24 = icmp slt i32 %22, %23, !dbg !1760
  br i1 %24, label %25, label %105, !dbg !1760

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1761
  %27 = sext i32 %26 to i64, !dbg !1761
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1761
  %29 = load i8, ptr %28, align 1, !dbg !1761
  %30 = sext i8 %29 to i32, !dbg !1761
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1761

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1764
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1764
  store ptr %33, ptr %5, align 8, !dbg !1764
  %34 = load i32, ptr %32, align 8, !dbg !1764
  %35 = trunc i32 %34 to i8, !dbg !1764
  %36 = load i32, ptr %12, align 4, !dbg !1764
  %37 = sext i32 %36 to i64, !dbg !1764
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1764
  store i8 %35, ptr %38, align 8, !dbg !1764
  br label %101, !dbg !1764

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1764
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1764
  store ptr %41, ptr %5, align 8, !dbg !1764
  %42 = load i32, ptr %40, align 8, !dbg !1764
  %43 = trunc i32 %42 to i8, !dbg !1764
  %44 = load i32, ptr %12, align 4, !dbg !1764
  %45 = sext i32 %44 to i64, !dbg !1764
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1764
  store i8 %43, ptr %46, align 8, !dbg !1764
  br label %101, !dbg !1764

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1764
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1764
  store ptr %49, ptr %5, align 8, !dbg !1764
  %50 = load i32, ptr %48, align 8, !dbg !1764
  %51 = trunc i32 %50 to i16, !dbg !1764
  %52 = load i32, ptr %12, align 4, !dbg !1764
  %53 = sext i32 %52 to i64, !dbg !1764
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1764
  store i16 %51, ptr %54, align 8, !dbg !1764
  br label %101, !dbg !1764

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1764
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1764
  store ptr %57, ptr %5, align 8, !dbg !1764
  %58 = load i32, ptr %56, align 8, !dbg !1764
  %59 = trunc i32 %58 to i16, !dbg !1764
  %60 = load i32, ptr %12, align 4, !dbg !1764
  %61 = sext i32 %60 to i64, !dbg !1764
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1764
  store i16 %59, ptr %62, align 8, !dbg !1764
  br label %101, !dbg !1764

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1764
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1764
  store ptr %65, ptr %5, align 8, !dbg !1764
  %66 = load i32, ptr %64, align 8, !dbg !1764
  %67 = load i32, ptr %12, align 4, !dbg !1764
  %68 = sext i32 %67 to i64, !dbg !1764
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1764
  store i32 %66, ptr %69, align 8, !dbg !1764
  br label %101, !dbg !1764

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1764
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1764
  store ptr %72, ptr %5, align 8, !dbg !1764
  %73 = load i32, ptr %71, align 8, !dbg !1764
  %74 = sext i32 %73 to i64, !dbg !1764
  %75 = load i32, ptr %12, align 4, !dbg !1764
  %76 = sext i32 %75 to i64, !dbg !1764
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1764
  store i64 %74, ptr %77, align 8, !dbg !1764
  br label %101, !dbg !1764

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1764
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1764
  store ptr %80, ptr %5, align 8, !dbg !1764
  %81 = load double, ptr %79, align 8, !dbg !1764
  %82 = fptrunc double %81 to float, !dbg !1764
  %83 = load i32, ptr %12, align 4, !dbg !1764
  %84 = sext i32 %83 to i64, !dbg !1764
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1764
  store float %82, ptr %85, align 8, !dbg !1764
  br label %101, !dbg !1764

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1764
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1764
  store ptr %88, ptr %5, align 8, !dbg !1764
  %89 = load double, ptr %87, align 8, !dbg !1764
  %90 = load i32, ptr %12, align 4, !dbg !1764
  %91 = sext i32 %90 to i64, !dbg !1764
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1764
  store double %89, ptr %92, align 8, !dbg !1764
  br label %101, !dbg !1764

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1764
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1764
  store ptr %95, ptr %5, align 8, !dbg !1764
  %96 = load ptr, ptr %94, align 8, !dbg !1764
  %97 = load i32, ptr %12, align 4, !dbg !1764
  %98 = sext i32 %97 to i64, !dbg !1764
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1764
  store ptr %96, ptr %99, align 8, !dbg !1764
  br label %101, !dbg !1764

100:                                              ; preds = %25
  br label %101, !dbg !1764

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1761

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1766
  %104 = add nsw i32 %103, 1, !dbg !1766
  store i32 %104, ptr %12, align 4, !dbg !1766
  br label %21, !dbg !1766, !llvm.loop !1767

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1751
  %107 = load ptr, ptr %106, align 8, !dbg !1751
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 60, !dbg !1751
  %109 = load ptr, ptr %108, align 8, !dbg !1751
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1751
  %111 = load ptr, ptr %6, align 8, !dbg !1751
  %112 = load ptr, ptr %7, align 8, !dbg !1751
  %113 = load ptr, ptr %8, align 8, !dbg !1751
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1751
  ret double %114, !dbg !1751
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1768 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca double, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1769, metadata !DIExpression()), !dbg !1770
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1771, metadata !DIExpression()), !dbg !1770
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1772, metadata !DIExpression()), !dbg !1770
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1773, metadata !DIExpression()), !dbg !1770
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1774, metadata !DIExpression()), !dbg !1770
  call void @llvm.va_start(ptr %9), !dbg !1770
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1775, metadata !DIExpression()), !dbg !1770
  %11 = load ptr, ptr %9, align 8, !dbg !1770
  %12 = load ptr, ptr %5, align 8, !dbg !1770
  %13 = load ptr, ptr %6, align 8, !dbg !1770
  %14 = load ptr, ptr %7, align 8, !dbg !1770
  %15 = load ptr, ptr %8, align 8, !dbg !1770
  %16 = call double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11), !dbg !1770
  store double %16, ptr %10, align 8, !dbg !1770
  call void @llvm.va_end(ptr %9), !dbg !1770
  %17 = load double, ptr %10, align 8, !dbg !1770
  ret double %17, !dbg !1770
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1776 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1777, metadata !DIExpression()), !dbg !1778
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1779, metadata !DIExpression()), !dbg !1778
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1780, metadata !DIExpression()), !dbg !1778
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1781, metadata !DIExpression()), !dbg !1778
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1782, metadata !DIExpression()), !dbg !1778
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1783, metadata !DIExpression()), !dbg !1778
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1784, metadata !DIExpression()), !dbg !1778
  %15 = load ptr, ptr %10, align 8, !dbg !1778
  %16 = load ptr, ptr %15, align 8, !dbg !1778
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1778
  %18 = load ptr, ptr %17, align 8, !dbg !1778
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1778
  %20 = load ptr, ptr %7, align 8, !dbg !1778
  %21 = load ptr, ptr %10, align 8, !dbg !1778
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1778
  store i32 %22, ptr %12, align 4, !dbg !1778
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1785, metadata !DIExpression()), !dbg !1778
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1786, metadata !DIExpression()), !dbg !1788
  store i32 0, ptr %14, align 4, !dbg !1788
  br label %23, !dbg !1788

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1788
  %25 = load i32, ptr %12, align 4, !dbg !1788
  %26 = icmp slt i32 %24, %25, !dbg !1788
  br i1 %26, label %27, label %107, !dbg !1788

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1789
  %29 = sext i32 %28 to i64, !dbg !1789
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1789
  %31 = load i8, ptr %30, align 1, !dbg !1789
  %32 = sext i8 %31 to i32, !dbg !1789
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1789

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1792
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1792
  store ptr %35, ptr %6, align 8, !dbg !1792
  %36 = load i32, ptr %34, align 8, !dbg !1792
  %37 = trunc i32 %36 to i8, !dbg !1792
  %38 = load i32, ptr %14, align 4, !dbg !1792
  %39 = sext i32 %38 to i64, !dbg !1792
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1792
  store i8 %37, ptr %40, align 8, !dbg !1792
  br label %103, !dbg !1792

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1792
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1792
  store ptr %43, ptr %6, align 8, !dbg !1792
  %44 = load i32, ptr %42, align 8, !dbg !1792
  %45 = trunc i32 %44 to i8, !dbg !1792
  %46 = load i32, ptr %14, align 4, !dbg !1792
  %47 = sext i32 %46 to i64, !dbg !1792
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1792
  store i8 %45, ptr %48, align 8, !dbg !1792
  br label %103, !dbg !1792

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1792
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1792
  store ptr %51, ptr %6, align 8, !dbg !1792
  %52 = load i32, ptr %50, align 8, !dbg !1792
  %53 = trunc i32 %52 to i16, !dbg !1792
  %54 = load i32, ptr %14, align 4, !dbg !1792
  %55 = sext i32 %54 to i64, !dbg !1792
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1792
  store i16 %53, ptr %56, align 8, !dbg !1792
  br label %103, !dbg !1792

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1792
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1792
  store ptr %59, ptr %6, align 8, !dbg !1792
  %60 = load i32, ptr %58, align 8, !dbg !1792
  %61 = trunc i32 %60 to i16, !dbg !1792
  %62 = load i32, ptr %14, align 4, !dbg !1792
  %63 = sext i32 %62 to i64, !dbg !1792
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1792
  store i16 %61, ptr %64, align 8, !dbg !1792
  br label %103, !dbg !1792

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1792
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1792
  store ptr %67, ptr %6, align 8, !dbg !1792
  %68 = load i32, ptr %66, align 8, !dbg !1792
  %69 = load i32, ptr %14, align 4, !dbg !1792
  %70 = sext i32 %69 to i64, !dbg !1792
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1792
  store i32 %68, ptr %71, align 8, !dbg !1792
  br label %103, !dbg !1792

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1792
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1792
  store ptr %74, ptr %6, align 8, !dbg !1792
  %75 = load i32, ptr %73, align 8, !dbg !1792
  %76 = sext i32 %75 to i64, !dbg !1792
  %77 = load i32, ptr %14, align 4, !dbg !1792
  %78 = sext i32 %77 to i64, !dbg !1792
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1792
  store i64 %76, ptr %79, align 8, !dbg !1792
  br label %103, !dbg !1792

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1792
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1792
  store ptr %82, ptr %6, align 8, !dbg !1792
  %83 = load double, ptr %81, align 8, !dbg !1792
  %84 = fptrunc double %83 to float, !dbg !1792
  %85 = load i32, ptr %14, align 4, !dbg !1792
  %86 = sext i32 %85 to i64, !dbg !1792
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1792
  store float %84, ptr %87, align 8, !dbg !1792
  br label %103, !dbg !1792

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1792
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1792
  store ptr %90, ptr %6, align 8, !dbg !1792
  %91 = load double, ptr %89, align 8, !dbg !1792
  %92 = load i32, ptr %14, align 4, !dbg !1792
  %93 = sext i32 %92 to i64, !dbg !1792
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1792
  store double %91, ptr %94, align 8, !dbg !1792
  br label %103, !dbg !1792

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1792
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1792
  store ptr %97, ptr %6, align 8, !dbg !1792
  %98 = load ptr, ptr %96, align 8, !dbg !1792
  %99 = load i32, ptr %14, align 4, !dbg !1792
  %100 = sext i32 %99 to i64, !dbg !1792
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1792
  store ptr %98, ptr %101, align 8, !dbg !1792
  br label %103, !dbg !1792

102:                                              ; preds = %27
  br label %103, !dbg !1792

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1789

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1794
  %106 = add nsw i32 %105, 1, !dbg !1794
  store i32 %106, ptr %14, align 4, !dbg !1794
  br label %23, !dbg !1794, !llvm.loop !1795

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1778
  %109 = load ptr, ptr %108, align 8, !dbg !1778
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 90, !dbg !1778
  %111 = load ptr, ptr %110, align 8, !dbg !1778
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1778
  %113 = load ptr, ptr %7, align 8, !dbg !1778
  %114 = load ptr, ptr %8, align 8, !dbg !1778
  %115 = load ptr, ptr %9, align 8, !dbg !1778
  %116 = load ptr, ptr %10, align 8, !dbg !1778
  %117 = call double %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1778
  ret double %117, !dbg !1778
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1796 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1797, metadata !DIExpression()), !dbg !1798
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1799, metadata !DIExpression()), !dbg !1798
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1800, metadata !DIExpression()), !dbg !1798
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1801, metadata !DIExpression()), !dbg !1798
  call void @llvm.va_start(ptr %7), !dbg !1798
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1802, metadata !DIExpression()), !dbg !1798
  %9 = load ptr, ptr %7, align 8, !dbg !1798
  %10 = load ptr, ptr %4, align 8, !dbg !1798
  %11 = load ptr, ptr %5, align 8, !dbg !1798
  %12 = load ptr, ptr %6, align 8, !dbg !1798
  %13 = call double @JNI_CallStaticDoubleMethodV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1798
  store double %13, ptr %8, align 8, !dbg !1798
  call void @llvm.va_end(ptr %7), !dbg !1798
  %14 = load double, ptr %8, align 8, !dbg !1798
  ret double %14, !dbg !1798
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1803 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1804, metadata !DIExpression()), !dbg !1805
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1806, metadata !DIExpression()), !dbg !1805
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1807, metadata !DIExpression()), !dbg !1805
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1808, metadata !DIExpression()), !dbg !1805
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1809, metadata !DIExpression()), !dbg !1805
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1810, metadata !DIExpression()), !dbg !1805
  %13 = load ptr, ptr %8, align 8, !dbg !1805
  %14 = load ptr, ptr %13, align 8, !dbg !1805
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1805
  %16 = load ptr, ptr %15, align 8, !dbg !1805
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1805
  %18 = load ptr, ptr %6, align 8, !dbg !1805
  %19 = load ptr, ptr %8, align 8, !dbg !1805
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1805
  store i32 %20, ptr %10, align 4, !dbg !1805
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1811, metadata !DIExpression()), !dbg !1805
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1812, metadata !DIExpression()), !dbg !1814
  store i32 0, ptr %12, align 4, !dbg !1814
  br label %21, !dbg !1814

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1814
  %23 = load i32, ptr %10, align 4, !dbg !1814
  %24 = icmp slt i32 %22, %23, !dbg !1814
  br i1 %24, label %25, label %105, !dbg !1814

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1815
  %27 = sext i32 %26 to i64, !dbg !1815
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1815
  %29 = load i8, ptr %28, align 1, !dbg !1815
  %30 = sext i8 %29 to i32, !dbg !1815
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1815

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1818
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1818
  store ptr %33, ptr %5, align 8, !dbg !1818
  %34 = load i32, ptr %32, align 8, !dbg !1818
  %35 = trunc i32 %34 to i8, !dbg !1818
  %36 = load i32, ptr %12, align 4, !dbg !1818
  %37 = sext i32 %36 to i64, !dbg !1818
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1818
  store i8 %35, ptr %38, align 8, !dbg !1818
  br label %101, !dbg !1818

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1818
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1818
  store ptr %41, ptr %5, align 8, !dbg !1818
  %42 = load i32, ptr %40, align 8, !dbg !1818
  %43 = trunc i32 %42 to i8, !dbg !1818
  %44 = load i32, ptr %12, align 4, !dbg !1818
  %45 = sext i32 %44 to i64, !dbg !1818
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1818
  store i8 %43, ptr %46, align 8, !dbg !1818
  br label %101, !dbg !1818

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1818
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1818
  store ptr %49, ptr %5, align 8, !dbg !1818
  %50 = load i32, ptr %48, align 8, !dbg !1818
  %51 = trunc i32 %50 to i16, !dbg !1818
  %52 = load i32, ptr %12, align 4, !dbg !1818
  %53 = sext i32 %52 to i64, !dbg !1818
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1818
  store i16 %51, ptr %54, align 8, !dbg !1818
  br label %101, !dbg !1818

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1818
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1818
  store ptr %57, ptr %5, align 8, !dbg !1818
  %58 = load i32, ptr %56, align 8, !dbg !1818
  %59 = trunc i32 %58 to i16, !dbg !1818
  %60 = load i32, ptr %12, align 4, !dbg !1818
  %61 = sext i32 %60 to i64, !dbg !1818
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1818
  store i16 %59, ptr %62, align 8, !dbg !1818
  br label %101, !dbg !1818

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1818
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1818
  store ptr %65, ptr %5, align 8, !dbg !1818
  %66 = load i32, ptr %64, align 8, !dbg !1818
  %67 = load i32, ptr %12, align 4, !dbg !1818
  %68 = sext i32 %67 to i64, !dbg !1818
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1818
  store i32 %66, ptr %69, align 8, !dbg !1818
  br label %101, !dbg !1818

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1818
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1818
  store ptr %72, ptr %5, align 8, !dbg !1818
  %73 = load i32, ptr %71, align 8, !dbg !1818
  %74 = sext i32 %73 to i64, !dbg !1818
  %75 = load i32, ptr %12, align 4, !dbg !1818
  %76 = sext i32 %75 to i64, !dbg !1818
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1818
  store i64 %74, ptr %77, align 8, !dbg !1818
  br label %101, !dbg !1818

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1818
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1818
  store ptr %80, ptr %5, align 8, !dbg !1818
  %81 = load double, ptr %79, align 8, !dbg !1818
  %82 = fptrunc double %81 to float, !dbg !1818
  %83 = load i32, ptr %12, align 4, !dbg !1818
  %84 = sext i32 %83 to i64, !dbg !1818
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1818
  store float %82, ptr %85, align 8, !dbg !1818
  br label %101, !dbg !1818

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1818
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1818
  store ptr %88, ptr %5, align 8, !dbg !1818
  %89 = load double, ptr %87, align 8, !dbg !1818
  %90 = load i32, ptr %12, align 4, !dbg !1818
  %91 = sext i32 %90 to i64, !dbg !1818
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1818
  store double %89, ptr %92, align 8, !dbg !1818
  br label %101, !dbg !1818

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1818
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1818
  store ptr %95, ptr %5, align 8, !dbg !1818
  %96 = load ptr, ptr %94, align 8, !dbg !1818
  %97 = load i32, ptr %12, align 4, !dbg !1818
  %98 = sext i32 %97 to i64, !dbg !1818
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1818
  store ptr %96, ptr %99, align 8, !dbg !1818
  br label %101, !dbg !1818

100:                                              ; preds = %25
  br label %101, !dbg !1818

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1815

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1820
  %104 = add nsw i32 %103, 1, !dbg !1820
  store i32 %104, ptr %12, align 4, !dbg !1820
  br label %21, !dbg !1820, !llvm.loop !1821

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1805
  %107 = load ptr, ptr %106, align 8, !dbg !1805
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 140, !dbg !1805
  %109 = load ptr, ptr %108, align 8, !dbg !1805
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1805
  %111 = load ptr, ptr %6, align 8, !dbg !1805
  %112 = load ptr, ptr %7, align 8, !dbg !1805
  %113 = load ptr, ptr %8, align 8, !dbg !1805
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1805
  ret double %114, !dbg !1805
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1822 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1823, metadata !DIExpression()), !dbg !1824
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1825, metadata !DIExpression()), !dbg !1824
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1826, metadata !DIExpression()), !dbg !1824
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1827, metadata !DIExpression()), !dbg !1828
  call void @llvm.va_start(ptr %7), !dbg !1829
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1830, metadata !DIExpression()), !dbg !1831
  %9 = load ptr, ptr %7, align 8, !dbg !1831
  %10 = load ptr, ptr %4, align 8, !dbg !1831
  %11 = load ptr, ptr %5, align 8, !dbg !1831
  %12 = load ptr, ptr %6, align 8, !dbg !1831
  %13 = call ptr @JNI_NewObjectV(ptr noundef %12, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1831
  store ptr %13, ptr %8, align 8, !dbg !1831
  call void @llvm.va_end(ptr %7), !dbg !1832
  %14 = load ptr, ptr %8, align 8, !dbg !1833
  ret ptr %14, !dbg !1833
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1834 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1835, metadata !DIExpression()), !dbg !1836
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1837, metadata !DIExpression()), !dbg !1836
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1838, metadata !DIExpression()), !dbg !1836
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1839, metadata !DIExpression()), !dbg !1836
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1840, metadata !DIExpression()), !dbg !1841
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1842, metadata !DIExpression()), !dbg !1841
  %13 = load ptr, ptr %8, align 8, !dbg !1841
  %14 = load ptr, ptr %13, align 8, !dbg !1841
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1841
  %16 = load ptr, ptr %15, align 8, !dbg !1841
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1841
  %18 = load ptr, ptr %6, align 8, !dbg !1841
  %19 = load ptr, ptr %8, align 8, !dbg !1841
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1841
  store i32 %20, ptr %10, align 4, !dbg !1841
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1843, metadata !DIExpression()), !dbg !1841
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1844, metadata !DIExpression()), !dbg !1846
  store i32 0, ptr %12, align 4, !dbg !1846
  br label %21, !dbg !1846

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1846
  %23 = load i32, ptr %10, align 4, !dbg !1846
  %24 = icmp slt i32 %22, %23, !dbg !1846
  br i1 %24, label %25, label %105, !dbg !1846

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1847
  %27 = sext i32 %26 to i64, !dbg !1847
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1847
  %29 = load i8, ptr %28, align 1, !dbg !1847
  %30 = sext i8 %29 to i32, !dbg !1847
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1847

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1850
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1850
  store ptr %33, ptr %5, align 8, !dbg !1850
  %34 = load i32, ptr %32, align 8, !dbg !1850
  %35 = trunc i32 %34 to i8, !dbg !1850
  %36 = load i32, ptr %12, align 4, !dbg !1850
  %37 = sext i32 %36 to i64, !dbg !1850
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1850
  store i8 %35, ptr %38, align 8, !dbg !1850
  br label %101, !dbg !1850

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1850
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1850
  store ptr %41, ptr %5, align 8, !dbg !1850
  %42 = load i32, ptr %40, align 8, !dbg !1850
  %43 = trunc i32 %42 to i8, !dbg !1850
  %44 = load i32, ptr %12, align 4, !dbg !1850
  %45 = sext i32 %44 to i64, !dbg !1850
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1850
  store i8 %43, ptr %46, align 8, !dbg !1850
  br label %101, !dbg !1850

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1850
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1850
  store ptr %49, ptr %5, align 8, !dbg !1850
  %50 = load i32, ptr %48, align 8, !dbg !1850
  %51 = trunc i32 %50 to i16, !dbg !1850
  %52 = load i32, ptr %12, align 4, !dbg !1850
  %53 = sext i32 %52 to i64, !dbg !1850
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1850
  store i16 %51, ptr %54, align 8, !dbg !1850
  br label %101, !dbg !1850

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1850
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1850
  store ptr %57, ptr %5, align 8, !dbg !1850
  %58 = load i32, ptr %56, align 8, !dbg !1850
  %59 = trunc i32 %58 to i16, !dbg !1850
  %60 = load i32, ptr %12, align 4, !dbg !1850
  %61 = sext i32 %60 to i64, !dbg !1850
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1850
  store i16 %59, ptr %62, align 8, !dbg !1850
  br label %101, !dbg !1850

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1850
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1850
  store ptr %65, ptr %5, align 8, !dbg !1850
  %66 = load i32, ptr %64, align 8, !dbg !1850
  %67 = load i32, ptr %12, align 4, !dbg !1850
  %68 = sext i32 %67 to i64, !dbg !1850
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1850
  store i32 %66, ptr %69, align 8, !dbg !1850
  br label %101, !dbg !1850

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1850
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1850
  store ptr %72, ptr %5, align 8, !dbg !1850
  %73 = load i32, ptr %71, align 8, !dbg !1850
  %74 = sext i32 %73 to i64, !dbg !1850
  %75 = load i32, ptr %12, align 4, !dbg !1850
  %76 = sext i32 %75 to i64, !dbg !1850
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1850
  store i64 %74, ptr %77, align 8, !dbg !1850
  br label %101, !dbg !1850

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1850
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1850
  store ptr %80, ptr %5, align 8, !dbg !1850
  %81 = load double, ptr %79, align 8, !dbg !1850
  %82 = fptrunc double %81 to float, !dbg !1850
  %83 = load i32, ptr %12, align 4, !dbg !1850
  %84 = sext i32 %83 to i64, !dbg !1850
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1850
  store float %82, ptr %85, align 8, !dbg !1850
  br label %101, !dbg !1850

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1850
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1850
  store ptr %88, ptr %5, align 8, !dbg !1850
  %89 = load double, ptr %87, align 8, !dbg !1850
  %90 = load i32, ptr %12, align 4, !dbg !1850
  %91 = sext i32 %90 to i64, !dbg !1850
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1850
  store double %89, ptr %92, align 8, !dbg !1850
  br label %101, !dbg !1850

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1850
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1850
  store ptr %95, ptr %5, align 8, !dbg !1850
  %96 = load ptr, ptr %94, align 8, !dbg !1850
  %97 = load i32, ptr %12, align 4, !dbg !1850
  %98 = sext i32 %97 to i64, !dbg !1850
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1850
  store ptr %96, ptr %99, align 8, !dbg !1850
  br label %101, !dbg !1850

100:                                              ; preds = %25
  br label %101, !dbg !1850

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1847

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1852
  %104 = add nsw i32 %103, 1, !dbg !1852
  store i32 %104, ptr %12, align 4, !dbg !1852
  br label %21, !dbg !1852, !llvm.loop !1853

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1854
  %107 = load ptr, ptr %106, align 8, !dbg !1854
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 30, !dbg !1854
  %109 = load ptr, ptr %108, align 8, !dbg !1854
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1854
  %111 = load ptr, ptr %6, align 8, !dbg !1854
  %112 = load ptr, ptr %7, align 8, !dbg !1854
  %113 = load ptr, ptr %8, align 8, !dbg !1854
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1854
  ret ptr %114, !dbg !1854
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1855 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1856, metadata !DIExpression()), !dbg !1857
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1858, metadata !DIExpression()), !dbg !1857
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1859, metadata !DIExpression()), !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1860, metadata !DIExpression()), !dbg !1861
  call void @llvm.va_start(ptr %7), !dbg !1862
  %8 = load ptr, ptr %7, align 8, !dbg !1863
  %9 = load ptr, ptr %4, align 8, !dbg !1863
  %10 = load ptr, ptr %5, align 8, !dbg !1863
  %11 = load ptr, ptr %6, align 8, !dbg !1863
  call void @JNI_CallVoidMethodV(ptr noundef %11, ptr noundef %10, ptr noundef %9, ptr noundef %8), !dbg !1863
  call void @llvm.va_end(ptr %7), !dbg !1864
  ret void, !dbg !1865
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1866 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1867, metadata !DIExpression()), !dbg !1868
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1869, metadata !DIExpression()), !dbg !1868
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1870, metadata !DIExpression()), !dbg !1868
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1871, metadata !DIExpression()), !dbg !1868
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1872, metadata !DIExpression()), !dbg !1873
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1874, metadata !DIExpression()), !dbg !1873
  %13 = load ptr, ptr %8, align 8, !dbg !1873
  %14 = load ptr, ptr %13, align 8, !dbg !1873
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1873
  %16 = load ptr, ptr %15, align 8, !dbg !1873
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1873
  %18 = load ptr, ptr %6, align 8, !dbg !1873
  %19 = load ptr, ptr %8, align 8, !dbg !1873
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1873
  store i32 %20, ptr %10, align 4, !dbg !1873
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1875, metadata !DIExpression()), !dbg !1873
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1876, metadata !DIExpression()), !dbg !1878
  store i32 0, ptr %12, align 4, !dbg !1878
  br label %21, !dbg !1878

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1878
  %23 = load i32, ptr %10, align 4, !dbg !1878
  %24 = icmp slt i32 %22, %23, !dbg !1878
  br i1 %24, label %25, label %105, !dbg !1878

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1879
  %27 = sext i32 %26 to i64, !dbg !1879
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1879
  %29 = load i8, ptr %28, align 1, !dbg !1879
  %30 = sext i8 %29 to i32, !dbg !1879
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1879

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1882
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1882
  store ptr %33, ptr %5, align 8, !dbg !1882
  %34 = load i32, ptr %32, align 8, !dbg !1882
  %35 = trunc i32 %34 to i8, !dbg !1882
  %36 = load i32, ptr %12, align 4, !dbg !1882
  %37 = sext i32 %36 to i64, !dbg !1882
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1882
  store i8 %35, ptr %38, align 8, !dbg !1882
  br label %101, !dbg !1882

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1882
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1882
  store ptr %41, ptr %5, align 8, !dbg !1882
  %42 = load i32, ptr %40, align 8, !dbg !1882
  %43 = trunc i32 %42 to i8, !dbg !1882
  %44 = load i32, ptr %12, align 4, !dbg !1882
  %45 = sext i32 %44 to i64, !dbg !1882
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1882
  store i8 %43, ptr %46, align 8, !dbg !1882
  br label %101, !dbg !1882

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1882
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1882
  store ptr %49, ptr %5, align 8, !dbg !1882
  %50 = load i32, ptr %48, align 8, !dbg !1882
  %51 = trunc i32 %50 to i16, !dbg !1882
  %52 = load i32, ptr %12, align 4, !dbg !1882
  %53 = sext i32 %52 to i64, !dbg !1882
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1882
  store i16 %51, ptr %54, align 8, !dbg !1882
  br label %101, !dbg !1882

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1882
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1882
  store ptr %57, ptr %5, align 8, !dbg !1882
  %58 = load i32, ptr %56, align 8, !dbg !1882
  %59 = trunc i32 %58 to i16, !dbg !1882
  %60 = load i32, ptr %12, align 4, !dbg !1882
  %61 = sext i32 %60 to i64, !dbg !1882
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1882
  store i16 %59, ptr %62, align 8, !dbg !1882
  br label %101, !dbg !1882

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1882
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1882
  store ptr %65, ptr %5, align 8, !dbg !1882
  %66 = load i32, ptr %64, align 8, !dbg !1882
  %67 = load i32, ptr %12, align 4, !dbg !1882
  %68 = sext i32 %67 to i64, !dbg !1882
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1882
  store i32 %66, ptr %69, align 8, !dbg !1882
  br label %101, !dbg !1882

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1882
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1882
  store ptr %72, ptr %5, align 8, !dbg !1882
  %73 = load i32, ptr %71, align 8, !dbg !1882
  %74 = sext i32 %73 to i64, !dbg !1882
  %75 = load i32, ptr %12, align 4, !dbg !1882
  %76 = sext i32 %75 to i64, !dbg !1882
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1882
  store i64 %74, ptr %77, align 8, !dbg !1882
  br label %101, !dbg !1882

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1882
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1882
  store ptr %80, ptr %5, align 8, !dbg !1882
  %81 = load double, ptr %79, align 8, !dbg !1882
  %82 = fptrunc double %81 to float, !dbg !1882
  %83 = load i32, ptr %12, align 4, !dbg !1882
  %84 = sext i32 %83 to i64, !dbg !1882
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1882
  store float %82, ptr %85, align 8, !dbg !1882
  br label %101, !dbg !1882

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1882
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1882
  store ptr %88, ptr %5, align 8, !dbg !1882
  %89 = load double, ptr %87, align 8, !dbg !1882
  %90 = load i32, ptr %12, align 4, !dbg !1882
  %91 = sext i32 %90 to i64, !dbg !1882
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1882
  store double %89, ptr %92, align 8, !dbg !1882
  br label %101, !dbg !1882

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1882
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1882
  store ptr %95, ptr %5, align 8, !dbg !1882
  %96 = load ptr, ptr %94, align 8, !dbg !1882
  %97 = load i32, ptr %12, align 4, !dbg !1882
  %98 = sext i32 %97 to i64, !dbg !1882
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1882
  store ptr %96, ptr %99, align 8, !dbg !1882
  br label %101, !dbg !1882

100:                                              ; preds = %25
  br label %101, !dbg !1882

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1879

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1884
  %104 = add nsw i32 %103, 1, !dbg !1884
  store i32 %104, ptr %12, align 4, !dbg !1884
  br label %21, !dbg !1884, !llvm.loop !1885

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1886
  %107 = load ptr, ptr %106, align 8, !dbg !1886
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 63, !dbg !1886
  %109 = load ptr, ptr %108, align 8, !dbg !1886
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1886
  %111 = load ptr, ptr %6, align 8, !dbg !1886
  %112 = load ptr, ptr %7, align 8, !dbg !1886
  %113 = load ptr, ptr %8, align 8, !dbg !1886
  call void %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1886
  ret void, !dbg !1887
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1888 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1889, metadata !DIExpression()), !dbg !1890
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1891, metadata !DIExpression()), !dbg !1890
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1892, metadata !DIExpression()), !dbg !1890
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1893, metadata !DIExpression()), !dbg !1890
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1894, metadata !DIExpression()), !dbg !1895
  call void @llvm.va_start(ptr %9), !dbg !1896
  %10 = load ptr, ptr %9, align 8, !dbg !1897
  %11 = load ptr, ptr %5, align 8, !dbg !1897
  %12 = load ptr, ptr %6, align 8, !dbg !1897
  %13 = load ptr, ptr %7, align 8, !dbg !1897
  %14 = load ptr, ptr %8, align 8, !dbg !1897
  call void @JNI_CallNonvirtualVoidMethodV(ptr noundef %14, ptr noundef %13, ptr noundef %12, ptr noundef %11, ptr noundef %10), !dbg !1897
  call void @llvm.va_end(ptr %9), !dbg !1898
  ret void, !dbg !1899
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1900 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 16
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 16
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1901, metadata !DIExpression()), !dbg !1902
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1903, metadata !DIExpression()), !dbg !1902
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1904, metadata !DIExpression()), !dbg !1902
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1905, metadata !DIExpression()), !dbg !1902
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1906, metadata !DIExpression()), !dbg !1902
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1907, metadata !DIExpression()), !dbg !1908
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1909, metadata !DIExpression()), !dbg !1908
  %15 = load ptr, ptr %10, align 8, !dbg !1908
  %16 = load ptr, ptr %15, align 8, !dbg !1908
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1908
  %18 = load ptr, ptr %17, align 8, !dbg !1908
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1908
  %20 = load ptr, ptr %7, align 8, !dbg !1908
  %21 = load ptr, ptr %10, align 8, !dbg !1908
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1908
  store i32 %22, ptr %12, align 4, !dbg !1908
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1910, metadata !DIExpression()), !dbg !1908
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1911, metadata !DIExpression()), !dbg !1913
  store i32 0, ptr %14, align 4, !dbg !1913
  br label %23, !dbg !1913

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1913
  %25 = load i32, ptr %12, align 4, !dbg !1913
  %26 = icmp slt i32 %24, %25, !dbg !1913
  br i1 %26, label %27, label %107, !dbg !1913

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1914
  %29 = sext i32 %28 to i64, !dbg !1914
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1914
  %31 = load i8, ptr %30, align 1, !dbg !1914
  %32 = sext i8 %31 to i32, !dbg !1914
  switch i32 %32, label %102 [
    i32 90, label %33
    i32 66, label %41
    i32 67, label %49
    i32 83, label %57
    i32 73, label %65
    i32 74, label %72
    i32 70, label %80
    i32 68, label %88
    i32 76, label %95
  ], !dbg !1914

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1917
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1917
  store ptr %35, ptr %6, align 8, !dbg !1917
  %36 = load i32, ptr %34, align 8, !dbg !1917
  %37 = trunc i32 %36 to i8, !dbg !1917
  %38 = load i32, ptr %14, align 4, !dbg !1917
  %39 = sext i32 %38 to i64, !dbg !1917
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1917
  store i8 %37, ptr %40, align 8, !dbg !1917
  br label %103, !dbg !1917

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1917
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1917
  store ptr %43, ptr %6, align 8, !dbg !1917
  %44 = load i32, ptr %42, align 8, !dbg !1917
  %45 = trunc i32 %44 to i8, !dbg !1917
  %46 = load i32, ptr %14, align 4, !dbg !1917
  %47 = sext i32 %46 to i64, !dbg !1917
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1917
  store i8 %45, ptr %48, align 8, !dbg !1917
  br label %103, !dbg !1917

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1917
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1917
  store ptr %51, ptr %6, align 8, !dbg !1917
  %52 = load i32, ptr %50, align 8, !dbg !1917
  %53 = trunc i32 %52 to i16, !dbg !1917
  %54 = load i32, ptr %14, align 4, !dbg !1917
  %55 = sext i32 %54 to i64, !dbg !1917
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1917
  store i16 %53, ptr %56, align 8, !dbg !1917
  br label %103, !dbg !1917

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1917
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1917
  store ptr %59, ptr %6, align 8, !dbg !1917
  %60 = load i32, ptr %58, align 8, !dbg !1917
  %61 = trunc i32 %60 to i16, !dbg !1917
  %62 = load i32, ptr %14, align 4, !dbg !1917
  %63 = sext i32 %62 to i64, !dbg !1917
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1917
  store i16 %61, ptr %64, align 8, !dbg !1917
  br label %103, !dbg !1917

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1917
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1917
  store ptr %67, ptr %6, align 8, !dbg !1917
  %68 = load i32, ptr %66, align 8, !dbg !1917
  %69 = load i32, ptr %14, align 4, !dbg !1917
  %70 = sext i32 %69 to i64, !dbg !1917
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1917
  store i32 %68, ptr %71, align 8, !dbg !1917
  br label %103, !dbg !1917

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1917
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1917
  store ptr %74, ptr %6, align 8, !dbg !1917
  %75 = load i32, ptr %73, align 8, !dbg !1917
  %76 = sext i32 %75 to i64, !dbg !1917
  %77 = load i32, ptr %14, align 4, !dbg !1917
  %78 = sext i32 %77 to i64, !dbg !1917
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1917
  store i64 %76, ptr %79, align 8, !dbg !1917
  br label %103, !dbg !1917

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1917
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1917
  store ptr %82, ptr %6, align 8, !dbg !1917
  %83 = load double, ptr %81, align 8, !dbg !1917
  %84 = fptrunc double %83 to float, !dbg !1917
  %85 = load i32, ptr %14, align 4, !dbg !1917
  %86 = sext i32 %85 to i64, !dbg !1917
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1917
  store float %84, ptr %87, align 8, !dbg !1917
  br label %103, !dbg !1917

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1917
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1917
  store ptr %90, ptr %6, align 8, !dbg !1917
  %91 = load double, ptr %89, align 8, !dbg !1917
  %92 = load i32, ptr %14, align 4, !dbg !1917
  %93 = sext i32 %92 to i64, !dbg !1917
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1917
  store double %91, ptr %94, align 8, !dbg !1917
  br label %103, !dbg !1917

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1917
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1917
  store ptr %97, ptr %6, align 8, !dbg !1917
  %98 = load ptr, ptr %96, align 8, !dbg !1917
  %99 = load i32, ptr %14, align 4, !dbg !1917
  %100 = sext i32 %99 to i64, !dbg !1917
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1917
  store ptr %98, ptr %101, align 8, !dbg !1917
  br label %103, !dbg !1917

102:                                              ; preds = %27
  br label %103, !dbg !1917

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1914

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1919
  %106 = add nsw i32 %105, 1, !dbg !1919
  store i32 %106, ptr %14, align 4, !dbg !1919
  br label %23, !dbg !1919, !llvm.loop !1920

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1921
  %109 = load ptr, ptr %108, align 8, !dbg !1921
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 93, !dbg !1921
  %111 = load ptr, ptr %110, align 8, !dbg !1921
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1921
  %113 = load ptr, ptr %7, align 8, !dbg !1921
  %114 = load ptr, ptr %8, align 8, !dbg !1921
  %115 = load ptr, ptr %9, align 8, !dbg !1921
  %116 = load ptr, ptr %10, align 8, !dbg !1921
  call void %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1921
  ret void, !dbg !1922
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1923 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1924, metadata !DIExpression()), !dbg !1925
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1926, metadata !DIExpression()), !dbg !1925
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1927, metadata !DIExpression()), !dbg !1925
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1928, metadata !DIExpression()), !dbg !1929
  call void @llvm.va_start(ptr %7), !dbg !1930
  %8 = load ptr, ptr %7, align 8, !dbg !1931
  %9 = load ptr, ptr %4, align 8, !dbg !1931
  %10 = load ptr, ptr %5, align 8, !dbg !1931
  %11 = load ptr, ptr %6, align 8, !dbg !1931
  call void @JNI_CallStaticVoidMethodV(ptr noundef %11, ptr noundef %10, ptr noundef %9, ptr noundef %8), !dbg !1931
  call void @llvm.va_end(ptr %7), !dbg !1932
  ret void, !dbg !1933
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1934 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 16
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 16
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1935, metadata !DIExpression()), !dbg !1936
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1937, metadata !DIExpression()), !dbg !1936
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1938, metadata !DIExpression()), !dbg !1936
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1939, metadata !DIExpression()), !dbg !1936
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1940, metadata !DIExpression()), !dbg !1941
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1942, metadata !DIExpression()), !dbg !1941
  %13 = load ptr, ptr %8, align 8, !dbg !1941
  %14 = load ptr, ptr %13, align 8, !dbg !1941
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1941
  %16 = load ptr, ptr %15, align 8, !dbg !1941
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1941
  %18 = load ptr, ptr %6, align 8, !dbg !1941
  %19 = load ptr, ptr %8, align 8, !dbg !1941
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1941
  store i32 %20, ptr %10, align 4, !dbg !1941
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1943, metadata !DIExpression()), !dbg !1941
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1944, metadata !DIExpression()), !dbg !1946
  store i32 0, ptr %12, align 4, !dbg !1946
  br label %21, !dbg !1946

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1946
  %23 = load i32, ptr %10, align 4, !dbg !1946
  %24 = icmp slt i32 %22, %23, !dbg !1946
  br i1 %24, label %25, label %105, !dbg !1946

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1947
  %27 = sext i32 %26 to i64, !dbg !1947
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1947
  %29 = load i8, ptr %28, align 1, !dbg !1947
  %30 = sext i8 %29 to i32, !dbg !1947
  switch i32 %30, label %100 [
    i32 90, label %31
    i32 66, label %39
    i32 67, label %47
    i32 83, label %55
    i32 73, label %63
    i32 74, label %70
    i32 70, label %78
    i32 68, label %86
    i32 76, label %93
  ], !dbg !1947

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1950
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1950
  store ptr %33, ptr %5, align 8, !dbg !1950
  %34 = load i32, ptr %32, align 8, !dbg !1950
  %35 = trunc i32 %34 to i8, !dbg !1950
  %36 = load i32, ptr %12, align 4, !dbg !1950
  %37 = sext i32 %36 to i64, !dbg !1950
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1950
  store i8 %35, ptr %38, align 8, !dbg !1950
  br label %101, !dbg !1950

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1950
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1950
  store ptr %41, ptr %5, align 8, !dbg !1950
  %42 = load i32, ptr %40, align 8, !dbg !1950
  %43 = trunc i32 %42 to i8, !dbg !1950
  %44 = load i32, ptr %12, align 4, !dbg !1950
  %45 = sext i32 %44 to i64, !dbg !1950
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1950
  store i8 %43, ptr %46, align 8, !dbg !1950
  br label %101, !dbg !1950

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1950
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1950
  store ptr %49, ptr %5, align 8, !dbg !1950
  %50 = load i32, ptr %48, align 8, !dbg !1950
  %51 = trunc i32 %50 to i16, !dbg !1950
  %52 = load i32, ptr %12, align 4, !dbg !1950
  %53 = sext i32 %52 to i64, !dbg !1950
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1950
  store i16 %51, ptr %54, align 8, !dbg !1950
  br label %101, !dbg !1950

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1950
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1950
  store ptr %57, ptr %5, align 8, !dbg !1950
  %58 = load i32, ptr %56, align 8, !dbg !1950
  %59 = trunc i32 %58 to i16, !dbg !1950
  %60 = load i32, ptr %12, align 4, !dbg !1950
  %61 = sext i32 %60 to i64, !dbg !1950
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1950
  store i16 %59, ptr %62, align 8, !dbg !1950
  br label %101, !dbg !1950

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1950
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1950
  store ptr %65, ptr %5, align 8, !dbg !1950
  %66 = load i32, ptr %64, align 8, !dbg !1950
  %67 = load i32, ptr %12, align 4, !dbg !1950
  %68 = sext i32 %67 to i64, !dbg !1950
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1950
  store i32 %66, ptr %69, align 8, !dbg !1950
  br label %101, !dbg !1950

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1950
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1950
  store ptr %72, ptr %5, align 8, !dbg !1950
  %73 = load i32, ptr %71, align 8, !dbg !1950
  %74 = sext i32 %73 to i64, !dbg !1950
  %75 = load i32, ptr %12, align 4, !dbg !1950
  %76 = sext i32 %75 to i64, !dbg !1950
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1950
  store i64 %74, ptr %77, align 8, !dbg !1950
  br label %101, !dbg !1950

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1950
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1950
  store ptr %80, ptr %5, align 8, !dbg !1950
  %81 = load double, ptr %79, align 8, !dbg !1950
  %82 = fptrunc double %81 to float, !dbg !1950
  %83 = load i32, ptr %12, align 4, !dbg !1950
  %84 = sext i32 %83 to i64, !dbg !1950
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1950
  store float %82, ptr %85, align 8, !dbg !1950
  br label %101, !dbg !1950

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1950
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1950
  store ptr %88, ptr %5, align 8, !dbg !1950
  %89 = load double, ptr %87, align 8, !dbg !1950
  %90 = load i32, ptr %12, align 4, !dbg !1950
  %91 = sext i32 %90 to i64, !dbg !1950
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1950
  store double %89, ptr %92, align 8, !dbg !1950
  br label %101, !dbg !1950

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1950
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1950
  store ptr %95, ptr %5, align 8, !dbg !1950
  %96 = load ptr, ptr %94, align 8, !dbg !1950
  %97 = load i32, ptr %12, align 4, !dbg !1950
  %98 = sext i32 %97 to i64, !dbg !1950
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1950
  store ptr %96, ptr %99, align 8, !dbg !1950
  br label %101, !dbg !1950

100:                                              ; preds = %25
  br label %101, !dbg !1950

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1947

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1952
  %104 = add nsw i32 %103, 1, !dbg !1952
  store i32 %104, ptr %12, align 4, !dbg !1952
  br label %21, !dbg !1952, !llvm.loop !1953

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1954
  %107 = load ptr, ptr %106, align 8, !dbg !1954
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 143, !dbg !1954
  %109 = load ptr, ptr %108, align 8, !dbg !1954
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1954
  %111 = load ptr, ptr %6, align 8, !dbg !1954
  %112 = load ptr, ptr %7, align 8, !dbg !1954
  %113 = load ptr, ptr %8, align 8, !dbg !1954
  call void %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1954
  ret void, !dbg !1955
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !1956 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1972, metadata !DIExpression()), !dbg !1973
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1974, metadata !DIExpression()), !dbg !1975
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1976, metadata !DIExpression()), !dbg !1977
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1978, metadata !DIExpression()), !dbg !1979
  %9 = load ptr, ptr %5, align 8, !dbg !1980
  %10 = load ptr, ptr %6, align 8, !dbg !1980
  %11 = load ptr, ptr %7, align 8, !dbg !1980
  %12 = load ptr, ptr %8, align 8, !dbg !1980
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i64 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !1980
  ret i32 %13, !dbg !1980
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsnprintf_l(ptr noundef %0, i64 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat !dbg !1981 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i64, align 8
  %10 = alloca ptr, align 8
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1984, metadata !DIExpression()), !dbg !1985
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1986, metadata !DIExpression()), !dbg !1987
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1988, metadata !DIExpression()), !dbg !1989
  store i64 %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1990, metadata !DIExpression()), !dbg !1991
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1992, metadata !DIExpression()), !dbg !1993
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1994, metadata !DIExpression()), !dbg !1996
  %12 = load ptr, ptr %6, align 8, !dbg !1996
  %13 = load ptr, ptr %7, align 8, !dbg !1996
  %14 = load ptr, ptr %8, align 8, !dbg !1996
  %15 = load i64, ptr %9, align 8, !dbg !1996
  %16 = load ptr, ptr %10, align 8, !dbg !1996
  %17 = call ptr @__local_stdio_printf_options(), !dbg !1996
  %18 = load i64, ptr %17, align 8, !dbg !1996
  %19 = or i64 %18, 1, !dbg !1996
  %20 = call i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i64 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12), !dbg !1996
  store i32 %20, ptr %11, align 4, !dbg !1996
  %21 = load i32, ptr %11, align 4, !dbg !1997
  %22 = icmp slt i32 %21, 0, !dbg !1997
  br i1 %22, label %23, label %24, !dbg !1997

23:                                               ; preds = %5
  br label %26, !dbg !1997

24:                                               ; preds = %5
  %25 = load i32, ptr %11, align 4, !dbg !1997
  br label %26, !dbg !1997

26:                                               ; preds = %24, %23
  %27 = phi i32 [ -1, %23 ], [ %25, %24 ], !dbg !1997
  ret i32 %27, !dbg !1997
}

declare dso_local i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i64 noundef, ptr noundef, ptr noundef, ptr noundef) #3

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local ptr @__local_stdio_printf_options() #0 comdat !dbg !2 {
  ret ptr @__local_stdio_printf_options._OptionsStorage, !dbg !1998
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { nocallback nofree nosync nounwind readnone speculatable willreturn }
attributes #2 = { nocallback nofree nosync nounwind willreturn }
attributes #3 = { "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }

!llvm.dbg.cu = !{!8}
!llvm.module.flags = !{!1033, !1034, !1035, !1036, !1037}
!llvm.ident = !{!1038}

!0 = !DIGlobalVariableExpression(var: !1, expr: !DIExpression())
!1 = distinct !DIGlobalVariable(name: "_OptionsStorage", scope: !2, file: !3, line: 91, type: !7, isLocal: true, isDefinition: true)
!2 = distinct !DISubprogram(name: "__local_stdio_printf_options", scope: !3, file: !3, line: 89, type: !4, scopeLine: 90, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!3 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt_stdio_config.h", directory: "", checksumkind: CSK_MD5, checksum: "dacf907bda504afb0b64f53a242bdae6")
!4 = !DISubroutineType(types: !5)
!5 = !{!6}
!6 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !7, size: 64)
!7 = !DIBasicType(name: "unsigned long long", size: 64, encoding: DW_ATE_unsigned)
!8 = distinct !DICompileUnit(language: DW_LANG_C99, file: !9, producer: "clang version 15.0.2", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, enums: !10, retainedTypes: !19, globals: !1031, splitDebugInlining: false, nameTableKind: None)
!9 = !DIFile(filename: "jni.c", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "855ea059d27abb4e7997a3ea5375fa86")
!10 = !{!11}
!11 = !DICompositeType(tag: DW_TAG_enumeration_type, name: "_jobjectType", file: !12, line: 139, baseType: !13, size: 32, elements: !14)
!12 = !DIFile(filename: "..\\..\\..\\openjdk\\jdk\\src\\share\\javavm\\export\\jni.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "b2a82d1bb637ba88970c8bd412bf1b49")
!13 = !DIBasicType(name: "int", size: 32, encoding: DW_ATE_signed)
!14 = !{!15, !16, !17, !18}
!15 = !DIEnumerator(name: "JNIInvalidRefType", value: 0)
!16 = !DIEnumerator(name: "JNILocalRefType", value: 1)
!17 = !DIEnumerator(name: "JNIGlobalRefType", value: 2)
!18 = !DIEnumerator(name: "JNIWeakGlobalRefType", value: 3)
!19 = !{!20, !81, !56, !164, !167, !40, !171, !174, !177, !48, !1029}
!20 = !DIDerivedType(tag: DW_TAG_typedef, name: "GetMethodArgs_t", file: !21, line: 22, baseType: !22)
!21 = !DIFile(filename: "./jni.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "9a0a7e9906ffff4113f7afa62fff60a2")
!22 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !23, size: 64)
!23 = !DISubroutineType(types: !24)
!24 = !{!13, !25, !67, !151}
!25 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !26, size: 64)
!26 = !DIDerivedType(tag: DW_TAG_typedef, name: "JNIEnv", file: !12, line: 197, baseType: !27)
!27 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !28, size: 64)
!28 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !29)
!29 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNINativeInterface_", file: !12, line: 214, size: 14912, elements: !30)
!30 = !{!31, !33, !34, !35, !36, !43, !59, !63, !70, !77, !83, !87, !91, !95, !100, !104, !108, !112, !113, !117, !121, !125, !126, !130, !131, !135, !136, !137, !141, !145, !152, !180, !184, !188, !192, !196, !200, !204, !208, !212, !216, !220, !224, !228, !232, !236, !240, !244, !248, !252, !256, !260, !264, !268, !272, !276, !280, !284, !288, !292, !296, !300, !304, !308, !312, !316, !320, !324, !328, !332, !336, !340, !344, !348, !352, !356, !360, !364, !368, !372, !376, !380, !384, !388, !392, !396, !400, !404, !408, !412, !416, !420, !424, !428, !432, !436, !440, !444, !448, !452, !456, !460, !464, !468, !472, !476, !480, !484, !488, !492, !496, !500, !504, !508, !509, !510, !511, !512, !516, !520, !524, !528, !532, !536, !540, !544, !548, !552, !556, !560, !564, !568, !572, !576, !580, !584, !588, !592, !596, !600, !604, !608, !612, !616, !620, !621, !625, !629, !633, !637, !641, !645, !649, !653, !657, !661, !665, !669, !673, !677, !681, !685, !689, !693, !700, !704, !709, !713, !717, !718, !722, !726, !731, !736, !740, !744, !749, !754, !759, !764, !769, !774, !779, !784, !788, !793, !798, !803, !808, !813, !818, !823, !827, !831, !835, !839, !843, !847, !851, !855, !859, !863, !867, !871, !875, !879, !883, !887, !893, !897, !901, !907, !913, !919, !925, !931, !943, !947, !951, !952, !981, !985, !989, !993, !997, !998, !999, !1004, !1008, !1012, !1016, !1020, !1024}
!31 = !DIDerivedType(tag: DW_TAG_member, name: "reserved0", scope: !29, file: !12, line: 215, baseType: !32, size: 64)
!32 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: null, size: 64)
!33 = !DIDerivedType(tag: DW_TAG_member, name: "reserved1", scope: !29, file: !12, line: 216, baseType: !32, size: 64, offset: 64)
!34 = !DIDerivedType(tag: DW_TAG_member, name: "reserved2", scope: !29, file: !12, line: 217, baseType: !32, size: 64, offset: 128)
!35 = !DIDerivedType(tag: DW_TAG_member, name: "reserved3", scope: !29, file: !12, line: 219, baseType: !32, size: 64, offset: 192)
!36 = !DIDerivedType(tag: DW_TAG_member, name: "GetVersion", scope: !29, file: !12, line: 220, baseType: !37, size: 64, offset: 256)
!37 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !38, size: 64)
!38 = !DISubroutineType(types: !39)
!39 = !{!40, !25}
!40 = !DIDerivedType(tag: DW_TAG_typedef, name: "jint", file: !41, line: 33, baseType: !42)
!41 = !DIFile(filename: "..\\..\\..\\openjdk\\jdk\\src\\windows\\javavm\\export\\jni_md.h", directory: "C:\\dev\\ikvm\\src\\IKVM.Runtime\\LLIR", checksumkind: CSK_MD5, checksum: "1ea1808175ba5b9740cb94cde3a9f925")
!42 = !DIBasicType(name: "long", size: 32, encoding: DW_ATE_signed)
!43 = !DIDerivedType(tag: DW_TAG_member, name: "DefineClass", scope: !29, file: !12, line: 222, baseType: !44, size: 64, offset: 320)
!44 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !45, size: 64)
!45 = !DISubroutineType(types: !46)
!46 = !{!47, !25, !51, !48, !54, !58}
!47 = !DIDerivedType(tag: DW_TAG_typedef, name: "jclass", file: !12, line: 102, baseType: !48)
!48 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobject", file: !12, line: 101, baseType: !49)
!49 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !50, size: 64)
!50 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jobject", file: !12, line: 99, flags: DIFlagFwdDecl)
!51 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !52, size: 64)
!52 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !53)
!53 = !DIBasicType(name: "char", size: 8, encoding: DW_ATE_signed_char)
!54 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !55, size: 64)
!55 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !56)
!56 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbyte", file: !41, line: 35, baseType: !57)
!57 = !DIBasicType(name: "signed char", size: 8, encoding: DW_ATE_signed_char)
!58 = !DIDerivedType(tag: DW_TAG_typedef, name: "jsize", file: !12, line: 63, baseType: !40)
!59 = !DIDerivedType(tag: DW_TAG_member, name: "FindClass", scope: !29, file: !12, line: 225, baseType: !60, size: 64, offset: 384)
!60 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !61, size: 64)
!61 = !DISubroutineType(types: !62)
!62 = !{!47, !25, !51}
!63 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedMethod", scope: !29, file: !12, line: 228, baseType: !64, size: 64, offset: 448)
!64 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !65, size: 64)
!65 = !DISubroutineType(types: !66)
!66 = !{!67, !25, !48}
!67 = !DIDerivedType(tag: DW_TAG_typedef, name: "jmethodID", file: !12, line: 136, baseType: !68)
!68 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !69, size: 64)
!69 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jmethodID", file: !12, line: 135, flags: DIFlagFwdDecl)
!70 = !DIDerivedType(tag: DW_TAG_member, name: "FromReflectedField", scope: !29, file: !12, line: 230, baseType: !71, size: 64, offset: 512)
!71 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !72, size: 64)
!72 = !DISubroutineType(types: !73)
!73 = !{!74, !25, !48}
!74 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfieldID", file: !12, line: 133, baseType: !75)
!75 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !76, size: 64)
!76 = !DICompositeType(tag: DW_TAG_structure_type, name: "_jfieldID", file: !12, line: 132, flags: DIFlagFwdDecl)
!77 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedMethod", scope: !29, file: !12, line: 233, baseType: !78, size: 64, offset: 576)
!78 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !79, size: 64)
!79 = !DISubroutineType(types: !80)
!80 = !{!48, !25, !47, !67, !81}
!81 = !DIDerivedType(tag: DW_TAG_typedef, name: "jboolean", file: !12, line: 57, baseType: !82)
!82 = !DIBasicType(name: "unsigned char", size: 8, encoding: DW_ATE_unsigned_char)
!83 = !DIDerivedType(tag: DW_TAG_member, name: "GetSuperclass", scope: !29, file: !12, line: 236, baseType: !84, size: 64, offset: 640)
!84 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !85, size: 64)
!85 = !DISubroutineType(types: !86)
!86 = !{!47, !25, !47}
!87 = !DIDerivedType(tag: DW_TAG_member, name: "IsAssignableFrom", scope: !29, file: !12, line: 238, baseType: !88, size: 64, offset: 704)
!88 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !89, size: 64)
!89 = !DISubroutineType(types: !90)
!90 = !{!81, !25, !47, !47}
!91 = !DIDerivedType(tag: DW_TAG_member, name: "ToReflectedField", scope: !29, file: !12, line: 241, baseType: !92, size: 64, offset: 768)
!92 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !93, size: 64)
!93 = !DISubroutineType(types: !94)
!94 = !{!48, !25, !47, !74, !81}
!95 = !DIDerivedType(tag: DW_TAG_member, name: "Throw", scope: !29, file: !12, line: 244, baseType: !96, size: 64, offset: 832)
!96 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !97, size: 64)
!97 = !DISubroutineType(types: !98)
!98 = !{!40, !25, !99}
!99 = !DIDerivedType(tag: DW_TAG_typedef, name: "jthrowable", file: !12, line: 103, baseType: !48)
!100 = !DIDerivedType(tag: DW_TAG_member, name: "ThrowNew", scope: !29, file: !12, line: 246, baseType: !101, size: 64, offset: 896)
!101 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !102, size: 64)
!102 = !DISubroutineType(types: !103)
!103 = !{!40, !25, !47, !51}
!104 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionOccurred", scope: !29, file: !12, line: 248, baseType: !105, size: 64, offset: 960)
!105 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !106, size: 64)
!106 = !DISubroutineType(types: !107)
!107 = !{!99, !25}
!108 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionDescribe", scope: !29, file: !12, line: 250, baseType: !109, size: 64, offset: 1024)
!109 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !110, size: 64)
!110 = !DISubroutineType(types: !111)
!111 = !{null, !25}
!112 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionClear", scope: !29, file: !12, line: 252, baseType: !109, size: 64, offset: 1088)
!113 = !DIDerivedType(tag: DW_TAG_member, name: "FatalError", scope: !29, file: !12, line: 254, baseType: !114, size: 64, offset: 1152)
!114 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !115, size: 64)
!115 = !DISubroutineType(types: !116)
!116 = !{null, !25, !51}
!117 = !DIDerivedType(tag: DW_TAG_member, name: "PushLocalFrame", scope: !29, file: !12, line: 257, baseType: !118, size: 64, offset: 1216)
!118 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !119, size: 64)
!119 = !DISubroutineType(types: !120)
!120 = !{!40, !25, !40}
!121 = !DIDerivedType(tag: DW_TAG_member, name: "PopLocalFrame", scope: !29, file: !12, line: 259, baseType: !122, size: 64, offset: 1280)
!122 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !123, size: 64)
!123 = !DISubroutineType(types: !124)
!124 = !{!48, !25, !48}
!125 = !DIDerivedType(tag: DW_TAG_member, name: "NewGlobalRef", scope: !29, file: !12, line: 262, baseType: !122, size: 64, offset: 1344)
!126 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteGlobalRef", scope: !29, file: !12, line: 264, baseType: !127, size: 64, offset: 1408)
!127 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !128, size: 64)
!128 = !DISubroutineType(types: !129)
!129 = !{null, !25, !48}
!130 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteLocalRef", scope: !29, file: !12, line: 266, baseType: !127, size: 64, offset: 1472)
!131 = !DIDerivedType(tag: DW_TAG_member, name: "IsSameObject", scope: !29, file: !12, line: 268, baseType: !132, size: 64, offset: 1536)
!132 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !133, size: 64)
!133 = !DISubroutineType(types: !134)
!134 = !{!81, !25, !48, !48}
!135 = !DIDerivedType(tag: DW_TAG_member, name: "NewLocalRef", scope: !29, file: !12, line: 270, baseType: !122, size: 64, offset: 1600)
!136 = !DIDerivedType(tag: DW_TAG_member, name: "EnsureLocalCapacity", scope: !29, file: !12, line: 272, baseType: !118, size: 64, offset: 1664)
!137 = !DIDerivedType(tag: DW_TAG_member, name: "AllocObject", scope: !29, file: !12, line: 275, baseType: !138, size: 64, offset: 1728)
!138 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !139, size: 64)
!139 = !DISubroutineType(types: !140)
!140 = !{!48, !25, !47}
!141 = !DIDerivedType(tag: DW_TAG_member, name: "NewObject", scope: !29, file: !12, line: 277, baseType: !142, size: 64, offset: 1792)
!142 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !143, size: 64)
!143 = !DISubroutineType(types: !144)
!144 = !{!48, !25, !47, !67, null}
!145 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectV", scope: !29, file: !12, line: 279, baseType: !146, size: 64, offset: 1856)
!146 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !147, size: 64)
!147 = !DISubroutineType(types: !148)
!148 = !{!48, !25, !47, !67, !149}
!149 = !DIDerivedType(tag: DW_TAG_typedef, name: "va_list", file: !150, line: 72, baseType: !151)
!150 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vadefs.h", directory: "", checksumkind: CSK_MD5, checksum: "a4b8f96637d0704c82f39ecb6bde2ab4")
!151 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !53, size: 64)
!152 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectA", scope: !29, file: !12, line: 281, baseType: !153, size: 64, offset: 1920)
!153 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !154, size: 64)
!154 = !DISubroutineType(types: !155)
!155 = !{!48, !25, !47, !67, !156}
!156 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !157, size: 64)
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
!179 = !DIDerivedType(tag: DW_TAG_member, name: "l", scope: !159, file: !12, line: 129, baseType: !48, size: 64)
!180 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectClass", scope: !29, file: !12, line: 284, baseType: !181, size: 64, offset: 1984)
!181 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !182, size: 64)
!182 = !DISubroutineType(types: !183)
!183 = !{!47, !25, !48}
!184 = !DIDerivedType(tag: DW_TAG_member, name: "IsInstanceOf", scope: !29, file: !12, line: 286, baseType: !185, size: 64, offset: 2048)
!185 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !186, size: 64)
!186 = !DISubroutineType(types: !187)
!187 = !{!81, !25, !48, !47}
!188 = !DIDerivedType(tag: DW_TAG_member, name: "GetMethodID", scope: !29, file: !12, line: 289, baseType: !189, size: 64, offset: 2112)
!189 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !190, size: 64)
!190 = !DISubroutineType(types: !191)
!191 = !{!67, !25, !47, !51, !51}
!192 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethod", scope: !29, file: !12, line: 292, baseType: !193, size: 64, offset: 2176)
!193 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !194, size: 64)
!194 = !DISubroutineType(types: !195)
!195 = !{!48, !25, !48, !67, null}
!196 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodV", scope: !29, file: !12, line: 294, baseType: !197, size: 64, offset: 2240)
!197 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !198, size: 64)
!198 = !DISubroutineType(types: !199)
!199 = !{!48, !25, !48, !67, !149}
!200 = !DIDerivedType(tag: DW_TAG_member, name: "CallObjectMethodA", scope: !29, file: !12, line: 296, baseType: !201, size: 64, offset: 2304)
!201 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !202, size: 64)
!202 = !DISubroutineType(types: !203)
!203 = !{!48, !25, !48, !67, !156}
!204 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethod", scope: !29, file: !12, line: 299, baseType: !205, size: 64, offset: 2368)
!205 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !206, size: 64)
!206 = !DISubroutineType(types: !207)
!207 = !{!81, !25, !48, !67, null}
!208 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodV", scope: !29, file: !12, line: 301, baseType: !209, size: 64, offset: 2432)
!209 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !210, size: 64)
!210 = !DISubroutineType(types: !211)
!211 = !{!81, !25, !48, !67, !149}
!212 = !DIDerivedType(tag: DW_TAG_member, name: "CallBooleanMethodA", scope: !29, file: !12, line: 303, baseType: !213, size: 64, offset: 2496)
!213 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !214, size: 64)
!214 = !DISubroutineType(types: !215)
!215 = !{!81, !25, !48, !67, !156}
!216 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethod", scope: !29, file: !12, line: 306, baseType: !217, size: 64, offset: 2560)
!217 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !218, size: 64)
!218 = !DISubroutineType(types: !219)
!219 = !{!56, !25, !48, !67, null}
!220 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodV", scope: !29, file: !12, line: 308, baseType: !221, size: 64, offset: 2624)
!221 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !222, size: 64)
!222 = !DISubroutineType(types: !223)
!223 = !{!56, !25, !48, !67, !149}
!224 = !DIDerivedType(tag: DW_TAG_member, name: "CallByteMethodA", scope: !29, file: !12, line: 310, baseType: !225, size: 64, offset: 2688)
!225 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !226, size: 64)
!226 = !DISubroutineType(types: !227)
!227 = !{!56, !25, !48, !67, !156}
!228 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethod", scope: !29, file: !12, line: 313, baseType: !229, size: 64, offset: 2752)
!229 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !230, size: 64)
!230 = !DISubroutineType(types: !231)
!231 = !{!164, !25, !48, !67, null}
!232 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodV", scope: !29, file: !12, line: 315, baseType: !233, size: 64, offset: 2816)
!233 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !234, size: 64)
!234 = !DISubroutineType(types: !235)
!235 = !{!164, !25, !48, !67, !149}
!236 = !DIDerivedType(tag: DW_TAG_member, name: "CallCharMethodA", scope: !29, file: !12, line: 317, baseType: !237, size: 64, offset: 2880)
!237 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !238, size: 64)
!238 = !DISubroutineType(types: !239)
!239 = !{!164, !25, !48, !67, !156}
!240 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethod", scope: !29, file: !12, line: 320, baseType: !241, size: 64, offset: 2944)
!241 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !242, size: 64)
!242 = !DISubroutineType(types: !243)
!243 = !{!167, !25, !48, !67, null}
!244 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodV", scope: !29, file: !12, line: 322, baseType: !245, size: 64, offset: 3008)
!245 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !246, size: 64)
!246 = !DISubroutineType(types: !247)
!247 = !{!167, !25, !48, !67, !149}
!248 = !DIDerivedType(tag: DW_TAG_member, name: "CallShortMethodA", scope: !29, file: !12, line: 324, baseType: !249, size: 64, offset: 3072)
!249 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !250, size: 64)
!250 = !DISubroutineType(types: !251)
!251 = !{!167, !25, !48, !67, !156}
!252 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethod", scope: !29, file: !12, line: 327, baseType: !253, size: 64, offset: 3136)
!253 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !254, size: 64)
!254 = !DISubroutineType(types: !255)
!255 = !{!40, !25, !48, !67, null}
!256 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodV", scope: !29, file: !12, line: 329, baseType: !257, size: 64, offset: 3200)
!257 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !258, size: 64)
!258 = !DISubroutineType(types: !259)
!259 = !{!40, !25, !48, !67, !149}
!260 = !DIDerivedType(tag: DW_TAG_member, name: "CallIntMethodA", scope: !29, file: !12, line: 331, baseType: !261, size: 64, offset: 3264)
!261 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !262, size: 64)
!262 = !DISubroutineType(types: !263)
!263 = !{!40, !25, !48, !67, !156}
!264 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethod", scope: !29, file: !12, line: 334, baseType: !265, size: 64, offset: 3328)
!265 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !266, size: 64)
!266 = !DISubroutineType(types: !267)
!267 = !{!171, !25, !48, !67, null}
!268 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodV", scope: !29, file: !12, line: 336, baseType: !269, size: 64, offset: 3392)
!269 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !270, size: 64)
!270 = !DISubroutineType(types: !271)
!271 = !{!171, !25, !48, !67, !149}
!272 = !DIDerivedType(tag: DW_TAG_member, name: "CallLongMethodA", scope: !29, file: !12, line: 338, baseType: !273, size: 64, offset: 3456)
!273 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !274, size: 64)
!274 = !DISubroutineType(types: !275)
!275 = !{!171, !25, !48, !67, !156}
!276 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethod", scope: !29, file: !12, line: 341, baseType: !277, size: 64, offset: 3520)
!277 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !278, size: 64)
!278 = !DISubroutineType(types: !279)
!279 = !{!174, !25, !48, !67, null}
!280 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodV", scope: !29, file: !12, line: 343, baseType: !281, size: 64, offset: 3584)
!281 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !282, size: 64)
!282 = !DISubroutineType(types: !283)
!283 = !{!174, !25, !48, !67, !149}
!284 = !DIDerivedType(tag: DW_TAG_member, name: "CallFloatMethodA", scope: !29, file: !12, line: 345, baseType: !285, size: 64, offset: 3648)
!285 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !286, size: 64)
!286 = !DISubroutineType(types: !287)
!287 = !{!174, !25, !48, !67, !156}
!288 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethod", scope: !29, file: !12, line: 348, baseType: !289, size: 64, offset: 3712)
!289 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !290, size: 64)
!290 = !DISubroutineType(types: !291)
!291 = !{!177, !25, !48, !67, null}
!292 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodV", scope: !29, file: !12, line: 350, baseType: !293, size: 64, offset: 3776)
!293 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !294, size: 64)
!294 = !DISubroutineType(types: !295)
!295 = !{!177, !25, !48, !67, !149}
!296 = !DIDerivedType(tag: DW_TAG_member, name: "CallDoubleMethodA", scope: !29, file: !12, line: 352, baseType: !297, size: 64, offset: 3840)
!297 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !298, size: 64)
!298 = !DISubroutineType(types: !299)
!299 = !{!177, !25, !48, !67, !156}
!300 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethod", scope: !29, file: !12, line: 355, baseType: !301, size: 64, offset: 3904)
!301 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !302, size: 64)
!302 = !DISubroutineType(types: !303)
!303 = !{null, !25, !48, !67, null}
!304 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodV", scope: !29, file: !12, line: 357, baseType: !305, size: 64, offset: 3968)
!305 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !306, size: 64)
!306 = !DISubroutineType(types: !307)
!307 = !{null, !25, !48, !67, !149}
!308 = !DIDerivedType(tag: DW_TAG_member, name: "CallVoidMethodA", scope: !29, file: !12, line: 359, baseType: !309, size: 64, offset: 4032)
!309 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !310, size: 64)
!310 = !DISubroutineType(types: !311)
!311 = !{null, !25, !48, !67, !156}
!312 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethod", scope: !29, file: !12, line: 362, baseType: !313, size: 64, offset: 4096)
!313 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !314, size: 64)
!314 = !DISubroutineType(types: !315)
!315 = !{!48, !25, !48, !47, !67, null}
!316 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodV", scope: !29, file: !12, line: 364, baseType: !317, size: 64, offset: 4160)
!317 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !318, size: 64)
!318 = !DISubroutineType(types: !319)
!319 = !{!48, !25, !48, !47, !67, !149}
!320 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualObjectMethodA", scope: !29, file: !12, line: 367, baseType: !321, size: 64, offset: 4224)
!321 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !322, size: 64)
!322 = !DISubroutineType(types: !323)
!323 = !{!48, !25, !48, !47, !67, !156}
!324 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethod", scope: !29, file: !12, line: 371, baseType: !325, size: 64, offset: 4288)
!325 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !326, size: 64)
!326 = !DISubroutineType(types: !327)
!327 = !{!81, !25, !48, !47, !67, null}
!328 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodV", scope: !29, file: !12, line: 373, baseType: !329, size: 64, offset: 4352)
!329 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !330, size: 64)
!330 = !DISubroutineType(types: !331)
!331 = !{!81, !25, !48, !47, !67, !149}
!332 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualBooleanMethodA", scope: !29, file: !12, line: 376, baseType: !333, size: 64, offset: 4416)
!333 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !334, size: 64)
!334 = !DISubroutineType(types: !335)
!335 = !{!81, !25, !48, !47, !67, !156}
!336 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethod", scope: !29, file: !12, line: 380, baseType: !337, size: 64, offset: 4480)
!337 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !338, size: 64)
!338 = !DISubroutineType(types: !339)
!339 = !{!56, !25, !48, !47, !67, null}
!340 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodV", scope: !29, file: !12, line: 382, baseType: !341, size: 64, offset: 4544)
!341 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !342, size: 64)
!342 = !DISubroutineType(types: !343)
!343 = !{!56, !25, !48, !47, !67, !149}
!344 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualByteMethodA", scope: !29, file: !12, line: 385, baseType: !345, size: 64, offset: 4608)
!345 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !346, size: 64)
!346 = !DISubroutineType(types: !347)
!347 = !{!56, !25, !48, !47, !67, !156}
!348 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethod", scope: !29, file: !12, line: 389, baseType: !349, size: 64, offset: 4672)
!349 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !350, size: 64)
!350 = !DISubroutineType(types: !351)
!351 = !{!164, !25, !48, !47, !67, null}
!352 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodV", scope: !29, file: !12, line: 391, baseType: !353, size: 64, offset: 4736)
!353 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !354, size: 64)
!354 = !DISubroutineType(types: !355)
!355 = !{!164, !25, !48, !47, !67, !149}
!356 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualCharMethodA", scope: !29, file: !12, line: 394, baseType: !357, size: 64, offset: 4800)
!357 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !358, size: 64)
!358 = !DISubroutineType(types: !359)
!359 = !{!164, !25, !48, !47, !67, !156}
!360 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethod", scope: !29, file: !12, line: 398, baseType: !361, size: 64, offset: 4864)
!361 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !362, size: 64)
!362 = !DISubroutineType(types: !363)
!363 = !{!167, !25, !48, !47, !67, null}
!364 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodV", scope: !29, file: !12, line: 400, baseType: !365, size: 64, offset: 4928)
!365 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !366, size: 64)
!366 = !DISubroutineType(types: !367)
!367 = !{!167, !25, !48, !47, !67, !149}
!368 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualShortMethodA", scope: !29, file: !12, line: 403, baseType: !369, size: 64, offset: 4992)
!369 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !370, size: 64)
!370 = !DISubroutineType(types: !371)
!371 = !{!167, !25, !48, !47, !67, !156}
!372 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethod", scope: !29, file: !12, line: 407, baseType: !373, size: 64, offset: 5056)
!373 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !374, size: 64)
!374 = !DISubroutineType(types: !375)
!375 = !{!40, !25, !48, !47, !67, null}
!376 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodV", scope: !29, file: !12, line: 409, baseType: !377, size: 64, offset: 5120)
!377 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !378, size: 64)
!378 = !DISubroutineType(types: !379)
!379 = !{!40, !25, !48, !47, !67, !149}
!380 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualIntMethodA", scope: !29, file: !12, line: 412, baseType: !381, size: 64, offset: 5184)
!381 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !382, size: 64)
!382 = !DISubroutineType(types: !383)
!383 = !{!40, !25, !48, !47, !67, !156}
!384 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethod", scope: !29, file: !12, line: 416, baseType: !385, size: 64, offset: 5248)
!385 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !386, size: 64)
!386 = !DISubroutineType(types: !387)
!387 = !{!171, !25, !48, !47, !67, null}
!388 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodV", scope: !29, file: !12, line: 418, baseType: !389, size: 64, offset: 5312)
!389 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !390, size: 64)
!390 = !DISubroutineType(types: !391)
!391 = !{!171, !25, !48, !47, !67, !149}
!392 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualLongMethodA", scope: !29, file: !12, line: 421, baseType: !393, size: 64, offset: 5376)
!393 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !394, size: 64)
!394 = !DISubroutineType(types: !395)
!395 = !{!171, !25, !48, !47, !67, !156}
!396 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethod", scope: !29, file: !12, line: 425, baseType: !397, size: 64, offset: 5440)
!397 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !398, size: 64)
!398 = !DISubroutineType(types: !399)
!399 = !{!174, !25, !48, !47, !67, null}
!400 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodV", scope: !29, file: !12, line: 427, baseType: !401, size: 64, offset: 5504)
!401 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !402, size: 64)
!402 = !DISubroutineType(types: !403)
!403 = !{!174, !25, !48, !47, !67, !149}
!404 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualFloatMethodA", scope: !29, file: !12, line: 430, baseType: !405, size: 64, offset: 5568)
!405 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !406, size: 64)
!406 = !DISubroutineType(types: !407)
!407 = !{!174, !25, !48, !47, !67, !156}
!408 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethod", scope: !29, file: !12, line: 434, baseType: !409, size: 64, offset: 5632)
!409 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !410, size: 64)
!410 = !DISubroutineType(types: !411)
!411 = !{!177, !25, !48, !47, !67, null}
!412 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodV", scope: !29, file: !12, line: 436, baseType: !413, size: 64, offset: 5696)
!413 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !414, size: 64)
!414 = !DISubroutineType(types: !415)
!415 = !{!177, !25, !48, !47, !67, !149}
!416 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualDoubleMethodA", scope: !29, file: !12, line: 439, baseType: !417, size: 64, offset: 5760)
!417 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !418, size: 64)
!418 = !DISubroutineType(types: !419)
!419 = !{!177, !25, !48, !47, !67, !156}
!420 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethod", scope: !29, file: !12, line: 443, baseType: !421, size: 64, offset: 5824)
!421 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !422, size: 64)
!422 = !DISubroutineType(types: !423)
!423 = !{null, !25, !48, !47, !67, null}
!424 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodV", scope: !29, file: !12, line: 445, baseType: !425, size: 64, offset: 5888)
!425 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !426, size: 64)
!426 = !DISubroutineType(types: !427)
!427 = !{null, !25, !48, !47, !67, !149}
!428 = !DIDerivedType(tag: DW_TAG_member, name: "CallNonvirtualVoidMethodA", scope: !29, file: !12, line: 448, baseType: !429, size: 64, offset: 5952)
!429 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !430, size: 64)
!430 = !DISubroutineType(types: !431)
!431 = !{null, !25, !48, !47, !67, !156}
!432 = !DIDerivedType(tag: DW_TAG_member, name: "GetFieldID", scope: !29, file: !12, line: 452, baseType: !433, size: 64, offset: 6016)
!433 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !434, size: 64)
!434 = !DISubroutineType(types: !435)
!435 = !{!74, !25, !47, !51, !51}
!436 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectField", scope: !29, file: !12, line: 455, baseType: !437, size: 64, offset: 6080)
!437 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !438, size: 64)
!438 = !DISubroutineType(types: !439)
!439 = !{!48, !25, !48, !74}
!440 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanField", scope: !29, file: !12, line: 457, baseType: !441, size: 64, offset: 6144)
!441 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !442, size: 64)
!442 = !DISubroutineType(types: !443)
!443 = !{!81, !25, !48, !74}
!444 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteField", scope: !29, file: !12, line: 459, baseType: !445, size: 64, offset: 6208)
!445 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !446, size: 64)
!446 = !DISubroutineType(types: !447)
!447 = !{!56, !25, !48, !74}
!448 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharField", scope: !29, file: !12, line: 461, baseType: !449, size: 64, offset: 6272)
!449 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !450, size: 64)
!450 = !DISubroutineType(types: !451)
!451 = !{!164, !25, !48, !74}
!452 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortField", scope: !29, file: !12, line: 463, baseType: !453, size: 64, offset: 6336)
!453 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !454, size: 64)
!454 = !DISubroutineType(types: !455)
!455 = !{!167, !25, !48, !74}
!456 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntField", scope: !29, file: !12, line: 465, baseType: !457, size: 64, offset: 6400)
!457 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !458, size: 64)
!458 = !DISubroutineType(types: !459)
!459 = !{!40, !25, !48, !74}
!460 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongField", scope: !29, file: !12, line: 467, baseType: !461, size: 64, offset: 6464)
!461 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !462, size: 64)
!462 = !DISubroutineType(types: !463)
!463 = !{!171, !25, !48, !74}
!464 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatField", scope: !29, file: !12, line: 469, baseType: !465, size: 64, offset: 6528)
!465 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !466, size: 64)
!466 = !DISubroutineType(types: !467)
!467 = !{!174, !25, !48, !74}
!468 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleField", scope: !29, file: !12, line: 471, baseType: !469, size: 64, offset: 6592)
!469 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !470, size: 64)
!470 = !DISubroutineType(types: !471)
!471 = !{!177, !25, !48, !74}
!472 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectField", scope: !29, file: !12, line: 474, baseType: !473, size: 64, offset: 6656)
!473 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !474, size: 64)
!474 = !DISubroutineType(types: !475)
!475 = !{null, !25, !48, !74, !48}
!476 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanField", scope: !29, file: !12, line: 476, baseType: !477, size: 64, offset: 6720)
!477 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !478, size: 64)
!478 = !DISubroutineType(types: !479)
!479 = !{null, !25, !48, !74, !81}
!480 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteField", scope: !29, file: !12, line: 478, baseType: !481, size: 64, offset: 6784)
!481 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !482, size: 64)
!482 = !DISubroutineType(types: !483)
!483 = !{null, !25, !48, !74, !56}
!484 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharField", scope: !29, file: !12, line: 480, baseType: !485, size: 64, offset: 6848)
!485 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !486, size: 64)
!486 = !DISubroutineType(types: !487)
!487 = !{null, !25, !48, !74, !164}
!488 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortField", scope: !29, file: !12, line: 482, baseType: !489, size: 64, offset: 6912)
!489 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !490, size: 64)
!490 = !DISubroutineType(types: !491)
!491 = !{null, !25, !48, !74, !167}
!492 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntField", scope: !29, file: !12, line: 484, baseType: !493, size: 64, offset: 6976)
!493 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !494, size: 64)
!494 = !DISubroutineType(types: !495)
!495 = !{null, !25, !48, !74, !40}
!496 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongField", scope: !29, file: !12, line: 486, baseType: !497, size: 64, offset: 7040)
!497 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !498, size: 64)
!498 = !DISubroutineType(types: !499)
!499 = !{null, !25, !48, !74, !171}
!500 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatField", scope: !29, file: !12, line: 488, baseType: !501, size: 64, offset: 7104)
!501 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !502, size: 64)
!502 = !DISubroutineType(types: !503)
!503 = !{null, !25, !48, !74, !174}
!504 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleField", scope: !29, file: !12, line: 490, baseType: !505, size: 64, offset: 7168)
!505 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !506, size: 64)
!506 = !DISubroutineType(types: !507)
!507 = !{null, !25, !48, !74, !177}
!508 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticMethodID", scope: !29, file: !12, line: 493, baseType: !189, size: 64, offset: 7232)
!509 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethod", scope: !29, file: !12, line: 496, baseType: !142, size: 64, offset: 7296)
!510 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethodV", scope: !29, file: !12, line: 498, baseType: !146, size: 64, offset: 7360)
!511 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticObjectMethodA", scope: !29, file: !12, line: 500, baseType: !153, size: 64, offset: 7424)
!512 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethod", scope: !29, file: !12, line: 503, baseType: !513, size: 64, offset: 7488)
!513 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !514, size: 64)
!514 = !DISubroutineType(types: !515)
!515 = !{!81, !25, !47, !67, null}
!516 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethodV", scope: !29, file: !12, line: 505, baseType: !517, size: 64, offset: 7552)
!517 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !518, size: 64)
!518 = !DISubroutineType(types: !519)
!519 = !{!81, !25, !47, !67, !149}
!520 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticBooleanMethodA", scope: !29, file: !12, line: 507, baseType: !521, size: 64, offset: 7616)
!521 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !522, size: 64)
!522 = !DISubroutineType(types: !523)
!523 = !{!81, !25, !47, !67, !156}
!524 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethod", scope: !29, file: !12, line: 510, baseType: !525, size: 64, offset: 7680)
!525 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !526, size: 64)
!526 = !DISubroutineType(types: !527)
!527 = !{!56, !25, !47, !67, null}
!528 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodV", scope: !29, file: !12, line: 512, baseType: !529, size: 64, offset: 7744)
!529 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !530, size: 64)
!530 = !DISubroutineType(types: !531)
!531 = !{!56, !25, !47, !67, !149}
!532 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticByteMethodA", scope: !29, file: !12, line: 514, baseType: !533, size: 64, offset: 7808)
!533 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !534, size: 64)
!534 = !DISubroutineType(types: !535)
!535 = !{!56, !25, !47, !67, !156}
!536 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethod", scope: !29, file: !12, line: 517, baseType: !537, size: 64, offset: 7872)
!537 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !538, size: 64)
!538 = !DISubroutineType(types: !539)
!539 = !{!164, !25, !47, !67, null}
!540 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodV", scope: !29, file: !12, line: 519, baseType: !541, size: 64, offset: 7936)
!541 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !542, size: 64)
!542 = !DISubroutineType(types: !543)
!543 = !{!164, !25, !47, !67, !149}
!544 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticCharMethodA", scope: !29, file: !12, line: 521, baseType: !545, size: 64, offset: 8000)
!545 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !546, size: 64)
!546 = !DISubroutineType(types: !547)
!547 = !{!164, !25, !47, !67, !156}
!548 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethod", scope: !29, file: !12, line: 524, baseType: !549, size: 64, offset: 8064)
!549 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !550, size: 64)
!550 = !DISubroutineType(types: !551)
!551 = !{!167, !25, !47, !67, null}
!552 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodV", scope: !29, file: !12, line: 526, baseType: !553, size: 64, offset: 8128)
!553 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !554, size: 64)
!554 = !DISubroutineType(types: !555)
!555 = !{!167, !25, !47, !67, !149}
!556 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticShortMethodA", scope: !29, file: !12, line: 528, baseType: !557, size: 64, offset: 8192)
!557 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !558, size: 64)
!558 = !DISubroutineType(types: !559)
!559 = !{!167, !25, !47, !67, !156}
!560 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethod", scope: !29, file: !12, line: 531, baseType: !561, size: 64, offset: 8256)
!561 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !562, size: 64)
!562 = !DISubroutineType(types: !563)
!563 = !{!40, !25, !47, !67, null}
!564 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodV", scope: !29, file: !12, line: 533, baseType: !565, size: 64, offset: 8320)
!565 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !566, size: 64)
!566 = !DISubroutineType(types: !567)
!567 = !{!40, !25, !47, !67, !149}
!568 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticIntMethodA", scope: !29, file: !12, line: 535, baseType: !569, size: 64, offset: 8384)
!569 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !570, size: 64)
!570 = !DISubroutineType(types: !571)
!571 = !{!40, !25, !47, !67, !156}
!572 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethod", scope: !29, file: !12, line: 538, baseType: !573, size: 64, offset: 8448)
!573 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !574, size: 64)
!574 = !DISubroutineType(types: !575)
!575 = !{!171, !25, !47, !67, null}
!576 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodV", scope: !29, file: !12, line: 540, baseType: !577, size: 64, offset: 8512)
!577 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !578, size: 64)
!578 = !DISubroutineType(types: !579)
!579 = !{!171, !25, !47, !67, !149}
!580 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticLongMethodA", scope: !29, file: !12, line: 542, baseType: !581, size: 64, offset: 8576)
!581 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !582, size: 64)
!582 = !DISubroutineType(types: !583)
!583 = !{!171, !25, !47, !67, !156}
!584 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethod", scope: !29, file: !12, line: 545, baseType: !585, size: 64, offset: 8640)
!585 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !586, size: 64)
!586 = !DISubroutineType(types: !587)
!587 = !{!174, !25, !47, !67, null}
!588 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodV", scope: !29, file: !12, line: 547, baseType: !589, size: 64, offset: 8704)
!589 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !590, size: 64)
!590 = !DISubroutineType(types: !591)
!591 = !{!174, !25, !47, !67, !149}
!592 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticFloatMethodA", scope: !29, file: !12, line: 549, baseType: !593, size: 64, offset: 8768)
!593 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !594, size: 64)
!594 = !DISubroutineType(types: !595)
!595 = !{!174, !25, !47, !67, !156}
!596 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethod", scope: !29, file: !12, line: 552, baseType: !597, size: 64, offset: 8832)
!597 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !598, size: 64)
!598 = !DISubroutineType(types: !599)
!599 = !{!177, !25, !47, !67, null}
!600 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodV", scope: !29, file: !12, line: 554, baseType: !601, size: 64, offset: 8896)
!601 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !602, size: 64)
!602 = !DISubroutineType(types: !603)
!603 = !{!177, !25, !47, !67, !149}
!604 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticDoubleMethodA", scope: !29, file: !12, line: 556, baseType: !605, size: 64, offset: 8960)
!605 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !606, size: 64)
!606 = !DISubroutineType(types: !607)
!607 = !{!177, !25, !47, !67, !156}
!608 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethod", scope: !29, file: !12, line: 559, baseType: !609, size: 64, offset: 9024)
!609 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !610, size: 64)
!610 = !DISubroutineType(types: !611)
!611 = !{null, !25, !47, !67, null}
!612 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodV", scope: !29, file: !12, line: 561, baseType: !613, size: 64, offset: 9088)
!613 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !614, size: 64)
!614 = !DISubroutineType(types: !615)
!615 = !{null, !25, !47, !67, !149}
!616 = !DIDerivedType(tag: DW_TAG_member, name: "CallStaticVoidMethodA", scope: !29, file: !12, line: 563, baseType: !617, size: 64, offset: 9152)
!617 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !618, size: 64)
!618 = !DISubroutineType(types: !619)
!619 = !{null, !25, !47, !67, !156}
!620 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFieldID", scope: !29, file: !12, line: 566, baseType: !433, size: 64, offset: 9216)
!621 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticObjectField", scope: !29, file: !12, line: 568, baseType: !622, size: 64, offset: 9280)
!622 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !623, size: 64)
!623 = !DISubroutineType(types: !624)
!624 = !{!48, !25, !47, !74}
!625 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticBooleanField", scope: !29, file: !12, line: 570, baseType: !626, size: 64, offset: 9344)
!626 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !627, size: 64)
!627 = !DISubroutineType(types: !628)
!628 = !{!81, !25, !47, !74}
!629 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticByteField", scope: !29, file: !12, line: 572, baseType: !630, size: 64, offset: 9408)
!630 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !631, size: 64)
!631 = !DISubroutineType(types: !632)
!632 = !{!56, !25, !47, !74}
!633 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticCharField", scope: !29, file: !12, line: 574, baseType: !634, size: 64, offset: 9472)
!634 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !635, size: 64)
!635 = !DISubroutineType(types: !636)
!636 = !{!164, !25, !47, !74}
!637 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticShortField", scope: !29, file: !12, line: 576, baseType: !638, size: 64, offset: 9536)
!638 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !639, size: 64)
!639 = !DISubroutineType(types: !640)
!640 = !{!167, !25, !47, !74}
!641 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticIntField", scope: !29, file: !12, line: 578, baseType: !642, size: 64, offset: 9600)
!642 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !643, size: 64)
!643 = !DISubroutineType(types: !644)
!644 = !{!40, !25, !47, !74}
!645 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticLongField", scope: !29, file: !12, line: 580, baseType: !646, size: 64, offset: 9664)
!646 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !647, size: 64)
!647 = !DISubroutineType(types: !648)
!648 = !{!171, !25, !47, !74}
!649 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticFloatField", scope: !29, file: !12, line: 582, baseType: !650, size: 64, offset: 9728)
!650 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !651, size: 64)
!651 = !DISubroutineType(types: !652)
!652 = !{!174, !25, !47, !74}
!653 = !DIDerivedType(tag: DW_TAG_member, name: "GetStaticDoubleField", scope: !29, file: !12, line: 584, baseType: !654, size: 64, offset: 9792)
!654 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !655, size: 64)
!655 = !DISubroutineType(types: !656)
!656 = !{!177, !25, !47, !74}
!657 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticObjectField", scope: !29, file: !12, line: 587, baseType: !658, size: 64, offset: 9856)
!658 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !659, size: 64)
!659 = !DISubroutineType(types: !660)
!660 = !{null, !25, !47, !74, !48}
!661 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticBooleanField", scope: !29, file: !12, line: 589, baseType: !662, size: 64, offset: 9920)
!662 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !663, size: 64)
!663 = !DISubroutineType(types: !664)
!664 = !{null, !25, !47, !74, !81}
!665 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticByteField", scope: !29, file: !12, line: 591, baseType: !666, size: 64, offset: 9984)
!666 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !667, size: 64)
!667 = !DISubroutineType(types: !668)
!668 = !{null, !25, !47, !74, !56}
!669 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticCharField", scope: !29, file: !12, line: 593, baseType: !670, size: 64, offset: 10048)
!670 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !671, size: 64)
!671 = !DISubroutineType(types: !672)
!672 = !{null, !25, !47, !74, !164}
!673 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticShortField", scope: !29, file: !12, line: 595, baseType: !674, size: 64, offset: 10112)
!674 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !675, size: 64)
!675 = !DISubroutineType(types: !676)
!676 = !{null, !25, !47, !74, !167}
!677 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticIntField", scope: !29, file: !12, line: 597, baseType: !678, size: 64, offset: 10176)
!678 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !679, size: 64)
!679 = !DISubroutineType(types: !680)
!680 = !{null, !25, !47, !74, !40}
!681 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticLongField", scope: !29, file: !12, line: 599, baseType: !682, size: 64, offset: 10240)
!682 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !683, size: 64)
!683 = !DISubroutineType(types: !684)
!684 = !{null, !25, !47, !74, !171}
!685 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticFloatField", scope: !29, file: !12, line: 601, baseType: !686, size: 64, offset: 10304)
!686 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !687, size: 64)
!687 = !DISubroutineType(types: !688)
!688 = !{null, !25, !47, !74, !174}
!689 = !DIDerivedType(tag: DW_TAG_member, name: "SetStaticDoubleField", scope: !29, file: !12, line: 603, baseType: !690, size: 64, offset: 10368)
!690 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !691, size: 64)
!691 = !DISubroutineType(types: !692)
!692 = !{null, !25, !47, !74, !177}
!693 = !DIDerivedType(tag: DW_TAG_member, name: "NewString", scope: !29, file: !12, line: 606, baseType: !694, size: 64, offset: 10432)
!694 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !695, size: 64)
!695 = !DISubroutineType(types: !696)
!696 = !{!697, !25, !698, !58}
!697 = !DIDerivedType(tag: DW_TAG_typedef, name: "jstring", file: !12, line: 104, baseType: !48)
!698 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !699, size: 64)
!699 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !164)
!700 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringLength", scope: !29, file: !12, line: 608, baseType: !701, size: 64, offset: 10496)
!701 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !702, size: 64)
!702 = !DISubroutineType(types: !703)
!703 = !{!58, !25, !697}
!704 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringChars", scope: !29, file: !12, line: 610, baseType: !705, size: 64, offset: 10560)
!705 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !706, size: 64)
!706 = !DISubroutineType(types: !707)
!707 = !{!698, !25, !697, !708}
!708 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !81, size: 64)
!709 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringChars", scope: !29, file: !12, line: 612, baseType: !710, size: 64, offset: 10624)
!710 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !711, size: 64)
!711 = !DISubroutineType(types: !712)
!712 = !{null, !25, !697, !698}
!713 = !DIDerivedType(tag: DW_TAG_member, name: "NewStringUTF", scope: !29, file: !12, line: 615, baseType: !714, size: 64, offset: 10688)
!714 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !715, size: 64)
!715 = !DISubroutineType(types: !716)
!716 = !{!697, !25, !51}
!717 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFLength", scope: !29, file: !12, line: 617, baseType: !701, size: 64, offset: 10752)
!718 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFChars", scope: !29, file: !12, line: 619, baseType: !719, size: 64, offset: 10816)
!719 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !720, size: 64)
!720 = !DISubroutineType(types: !721)
!721 = !{!51, !25, !697, !708}
!722 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringUTFChars", scope: !29, file: !12, line: 621, baseType: !723, size: 64, offset: 10880)
!723 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !724, size: 64)
!724 = !DISubroutineType(types: !725)
!725 = !{null, !25, !697, !51}
!726 = !DIDerivedType(tag: DW_TAG_member, name: "GetArrayLength", scope: !29, file: !12, line: 625, baseType: !727, size: 64, offset: 10944)
!727 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !728, size: 64)
!728 = !DISubroutineType(types: !729)
!729 = !{!58, !25, !730}
!730 = !DIDerivedType(tag: DW_TAG_typedef, name: "jarray", file: !12, line: 105, baseType: !48)
!731 = !DIDerivedType(tag: DW_TAG_member, name: "NewObjectArray", scope: !29, file: !12, line: 628, baseType: !732, size: 64, offset: 11008)
!732 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !733, size: 64)
!733 = !DISubroutineType(types: !734)
!734 = !{!735, !25, !58, !47, !48}
!735 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectArray", file: !12, line: 114, baseType: !730)
!736 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectArrayElement", scope: !29, file: !12, line: 630, baseType: !737, size: 64, offset: 11072)
!737 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !738, size: 64)
!738 = !DISubroutineType(types: !739)
!739 = !{!48, !25, !735, !58}
!740 = !DIDerivedType(tag: DW_TAG_member, name: "SetObjectArrayElement", scope: !29, file: !12, line: 632, baseType: !741, size: 64, offset: 11136)
!741 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !742, size: 64)
!742 = !DISubroutineType(types: !743)
!743 = !{null, !25, !735, !58, !48}
!744 = !DIDerivedType(tag: DW_TAG_member, name: "NewBooleanArray", scope: !29, file: !12, line: 635, baseType: !745, size: 64, offset: 11200)
!745 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !746, size: 64)
!746 = !DISubroutineType(types: !747)
!747 = !{!748, !25, !58}
!748 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbooleanArray", file: !12, line: 106, baseType: !730)
!749 = !DIDerivedType(tag: DW_TAG_member, name: "NewByteArray", scope: !29, file: !12, line: 637, baseType: !750, size: 64, offset: 11264)
!750 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !751, size: 64)
!751 = !DISubroutineType(types: !752)
!752 = !{!753, !25, !58}
!753 = !DIDerivedType(tag: DW_TAG_typedef, name: "jbyteArray", file: !12, line: 107, baseType: !730)
!754 = !DIDerivedType(tag: DW_TAG_member, name: "NewCharArray", scope: !29, file: !12, line: 639, baseType: !755, size: 64, offset: 11328)
!755 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !756, size: 64)
!756 = !DISubroutineType(types: !757)
!757 = !{!758, !25, !58}
!758 = !DIDerivedType(tag: DW_TAG_typedef, name: "jcharArray", file: !12, line: 108, baseType: !730)
!759 = !DIDerivedType(tag: DW_TAG_member, name: "NewShortArray", scope: !29, file: !12, line: 641, baseType: !760, size: 64, offset: 11392)
!760 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !761, size: 64)
!761 = !DISubroutineType(types: !762)
!762 = !{!763, !25, !58}
!763 = !DIDerivedType(tag: DW_TAG_typedef, name: "jshortArray", file: !12, line: 109, baseType: !730)
!764 = !DIDerivedType(tag: DW_TAG_member, name: "NewIntArray", scope: !29, file: !12, line: 643, baseType: !765, size: 64, offset: 11456)
!765 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !766, size: 64)
!766 = !DISubroutineType(types: !767)
!767 = !{!768, !25, !58}
!768 = !DIDerivedType(tag: DW_TAG_typedef, name: "jintArray", file: !12, line: 110, baseType: !730)
!769 = !DIDerivedType(tag: DW_TAG_member, name: "NewLongArray", scope: !29, file: !12, line: 645, baseType: !770, size: 64, offset: 11520)
!770 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !771, size: 64)
!771 = !DISubroutineType(types: !772)
!772 = !{!773, !25, !58}
!773 = !DIDerivedType(tag: DW_TAG_typedef, name: "jlongArray", file: !12, line: 111, baseType: !730)
!774 = !DIDerivedType(tag: DW_TAG_member, name: "NewFloatArray", scope: !29, file: !12, line: 647, baseType: !775, size: 64, offset: 11584)
!775 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !776, size: 64)
!776 = !DISubroutineType(types: !777)
!777 = !{!778, !25, !58}
!778 = !DIDerivedType(tag: DW_TAG_typedef, name: "jfloatArray", file: !12, line: 112, baseType: !730)
!779 = !DIDerivedType(tag: DW_TAG_member, name: "NewDoubleArray", scope: !29, file: !12, line: 649, baseType: !780, size: 64, offset: 11648)
!780 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !781, size: 64)
!781 = !DISubroutineType(types: !782)
!782 = !{!783, !25, !58}
!783 = !DIDerivedType(tag: DW_TAG_typedef, name: "jdoubleArray", file: !12, line: 113, baseType: !730)
!784 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayElements", scope: !29, file: !12, line: 652, baseType: !785, size: 64, offset: 11712)
!785 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !786, size: 64)
!786 = !DISubroutineType(types: !787)
!787 = !{!708, !25, !748, !708}
!788 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayElements", scope: !29, file: !12, line: 654, baseType: !789, size: 64, offset: 11776)
!789 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !790, size: 64)
!790 = !DISubroutineType(types: !791)
!791 = !{!792, !25, !753, !708}
!792 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !56, size: 64)
!793 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayElements", scope: !29, file: !12, line: 656, baseType: !794, size: 64, offset: 11840)
!794 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !795, size: 64)
!795 = !DISubroutineType(types: !796)
!796 = !{!797, !25, !758, !708}
!797 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !164, size: 64)
!798 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayElements", scope: !29, file: !12, line: 658, baseType: !799, size: 64, offset: 11904)
!799 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !800, size: 64)
!800 = !DISubroutineType(types: !801)
!801 = !{!802, !25, !763, !708}
!802 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !167, size: 64)
!803 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayElements", scope: !29, file: !12, line: 660, baseType: !804, size: 64, offset: 11968)
!804 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !805, size: 64)
!805 = !DISubroutineType(types: !806)
!806 = !{!807, !25, !768, !708}
!807 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !40, size: 64)
!808 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayElements", scope: !29, file: !12, line: 662, baseType: !809, size: 64, offset: 12032)
!809 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !810, size: 64)
!810 = !DISubroutineType(types: !811)
!811 = !{!812, !25, !773, !708}
!812 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !171, size: 64)
!813 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayElements", scope: !29, file: !12, line: 664, baseType: !814, size: 64, offset: 12096)
!814 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !815, size: 64)
!815 = !DISubroutineType(types: !816)
!816 = !{!817, !25, !778, !708}
!817 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !174, size: 64)
!818 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayElements", scope: !29, file: !12, line: 666, baseType: !819, size: 64, offset: 12160)
!819 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !820, size: 64)
!820 = !DISubroutineType(types: !821)
!821 = !{!822, !25, !783, !708}
!822 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !177, size: 64)
!823 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseBooleanArrayElements", scope: !29, file: !12, line: 669, baseType: !824, size: 64, offset: 12224)
!824 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !825, size: 64)
!825 = !DISubroutineType(types: !826)
!826 = !{null, !25, !748, !708, !40}
!827 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseByteArrayElements", scope: !29, file: !12, line: 671, baseType: !828, size: 64, offset: 12288)
!828 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !829, size: 64)
!829 = !DISubroutineType(types: !830)
!830 = !{null, !25, !753, !792, !40}
!831 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseCharArrayElements", scope: !29, file: !12, line: 673, baseType: !832, size: 64, offset: 12352)
!832 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !833, size: 64)
!833 = !DISubroutineType(types: !834)
!834 = !{null, !25, !758, !797, !40}
!835 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseShortArrayElements", scope: !29, file: !12, line: 675, baseType: !836, size: 64, offset: 12416)
!836 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !837, size: 64)
!837 = !DISubroutineType(types: !838)
!838 = !{null, !25, !763, !802, !40}
!839 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseIntArrayElements", scope: !29, file: !12, line: 677, baseType: !840, size: 64, offset: 12480)
!840 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !841, size: 64)
!841 = !DISubroutineType(types: !842)
!842 = !{null, !25, !768, !807, !40}
!843 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseLongArrayElements", scope: !29, file: !12, line: 679, baseType: !844, size: 64, offset: 12544)
!844 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !845, size: 64)
!845 = !DISubroutineType(types: !846)
!846 = !{null, !25, !773, !812, !40}
!847 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseFloatArrayElements", scope: !29, file: !12, line: 681, baseType: !848, size: 64, offset: 12608)
!848 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !849, size: 64)
!849 = !DISubroutineType(types: !850)
!850 = !{null, !25, !778, !817, !40}
!851 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseDoubleArrayElements", scope: !29, file: !12, line: 683, baseType: !852, size: 64, offset: 12672)
!852 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !853, size: 64)
!853 = !DISubroutineType(types: !854)
!854 = !{null, !25, !783, !822, !40}
!855 = !DIDerivedType(tag: DW_TAG_member, name: "GetBooleanArrayRegion", scope: !29, file: !12, line: 686, baseType: !856, size: 64, offset: 12736)
!856 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !857, size: 64)
!857 = !DISubroutineType(types: !858)
!858 = !{null, !25, !748, !58, !58, !708}
!859 = !DIDerivedType(tag: DW_TAG_member, name: "GetByteArrayRegion", scope: !29, file: !12, line: 688, baseType: !860, size: 64, offset: 12800)
!860 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !861, size: 64)
!861 = !DISubroutineType(types: !862)
!862 = !{null, !25, !753, !58, !58, !792}
!863 = !DIDerivedType(tag: DW_TAG_member, name: "GetCharArrayRegion", scope: !29, file: !12, line: 690, baseType: !864, size: 64, offset: 12864)
!864 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !865, size: 64)
!865 = !DISubroutineType(types: !866)
!866 = !{null, !25, !758, !58, !58, !797}
!867 = !DIDerivedType(tag: DW_TAG_member, name: "GetShortArrayRegion", scope: !29, file: !12, line: 692, baseType: !868, size: 64, offset: 12928)
!868 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !869, size: 64)
!869 = !DISubroutineType(types: !870)
!870 = !{null, !25, !763, !58, !58, !802}
!871 = !DIDerivedType(tag: DW_TAG_member, name: "GetIntArrayRegion", scope: !29, file: !12, line: 694, baseType: !872, size: 64, offset: 12992)
!872 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !873, size: 64)
!873 = !DISubroutineType(types: !874)
!874 = !{null, !25, !768, !58, !58, !807}
!875 = !DIDerivedType(tag: DW_TAG_member, name: "GetLongArrayRegion", scope: !29, file: !12, line: 696, baseType: !876, size: 64, offset: 13056)
!876 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !877, size: 64)
!877 = !DISubroutineType(types: !878)
!878 = !{null, !25, !773, !58, !58, !812}
!879 = !DIDerivedType(tag: DW_TAG_member, name: "GetFloatArrayRegion", scope: !29, file: !12, line: 698, baseType: !880, size: 64, offset: 13120)
!880 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !881, size: 64)
!881 = !DISubroutineType(types: !882)
!882 = !{null, !25, !778, !58, !58, !817}
!883 = !DIDerivedType(tag: DW_TAG_member, name: "GetDoubleArrayRegion", scope: !29, file: !12, line: 700, baseType: !884, size: 64, offset: 13184)
!884 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !885, size: 64)
!885 = !DISubroutineType(types: !886)
!886 = !{null, !25, !783, !58, !58, !822}
!887 = !DIDerivedType(tag: DW_TAG_member, name: "SetBooleanArrayRegion", scope: !29, file: !12, line: 703, baseType: !888, size: 64, offset: 13248)
!888 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !889, size: 64)
!889 = !DISubroutineType(types: !890)
!890 = !{null, !25, !748, !58, !58, !891}
!891 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !892, size: 64)
!892 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !81)
!893 = !DIDerivedType(tag: DW_TAG_member, name: "SetByteArrayRegion", scope: !29, file: !12, line: 705, baseType: !894, size: 64, offset: 13312)
!894 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !895, size: 64)
!895 = !DISubroutineType(types: !896)
!896 = !{null, !25, !753, !58, !58, !54}
!897 = !DIDerivedType(tag: DW_TAG_member, name: "SetCharArrayRegion", scope: !29, file: !12, line: 707, baseType: !898, size: 64, offset: 13376)
!898 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !899, size: 64)
!899 = !DISubroutineType(types: !900)
!900 = !{null, !25, !758, !58, !58, !698}
!901 = !DIDerivedType(tag: DW_TAG_member, name: "SetShortArrayRegion", scope: !29, file: !12, line: 709, baseType: !902, size: 64, offset: 13440)
!902 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !903, size: 64)
!903 = !DISubroutineType(types: !904)
!904 = !{null, !25, !763, !58, !58, !905}
!905 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !906, size: 64)
!906 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !167)
!907 = !DIDerivedType(tag: DW_TAG_member, name: "SetIntArrayRegion", scope: !29, file: !12, line: 711, baseType: !908, size: 64, offset: 13504)
!908 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !909, size: 64)
!909 = !DISubroutineType(types: !910)
!910 = !{null, !25, !768, !58, !58, !911}
!911 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !912, size: 64)
!912 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !40)
!913 = !DIDerivedType(tag: DW_TAG_member, name: "SetLongArrayRegion", scope: !29, file: !12, line: 713, baseType: !914, size: 64, offset: 13568)
!914 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !915, size: 64)
!915 = !DISubroutineType(types: !916)
!916 = !{null, !25, !773, !58, !58, !917}
!917 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !918, size: 64)
!918 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !171)
!919 = !DIDerivedType(tag: DW_TAG_member, name: "SetFloatArrayRegion", scope: !29, file: !12, line: 715, baseType: !920, size: 64, offset: 13632)
!920 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !921, size: 64)
!921 = !DISubroutineType(types: !922)
!922 = !{null, !25, !778, !58, !58, !923}
!923 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !924, size: 64)
!924 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !174)
!925 = !DIDerivedType(tag: DW_TAG_member, name: "SetDoubleArrayRegion", scope: !29, file: !12, line: 717, baseType: !926, size: 64, offset: 13696)
!926 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !927, size: 64)
!927 = !DISubroutineType(types: !928)
!928 = !{null, !25, !783, !58, !58, !929}
!929 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !930, size: 64)
!930 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !177)
!931 = !DIDerivedType(tag: DW_TAG_member, name: "RegisterNatives", scope: !29, file: !12, line: 720, baseType: !932, size: 64, offset: 13760)
!932 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !933, size: 64)
!933 = !DISubroutineType(types: !934)
!934 = !{!40, !25, !47, !935, !40}
!935 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !936, size: 64)
!936 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !937)
!937 = !DIDerivedType(tag: DW_TAG_typedef, name: "JNINativeMethod", file: !12, line: 184, baseType: !938)
!938 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNINativeMethod", file: !12, line: 180, size: 192, elements: !939)
!939 = !{!940, !941, !942}
!940 = !DIDerivedType(tag: DW_TAG_member, name: "name", scope: !938, file: !12, line: 181, baseType: !151, size: 64)
!941 = !DIDerivedType(tag: DW_TAG_member, name: "signature", scope: !938, file: !12, line: 182, baseType: !151, size: 64, offset: 64)
!942 = !DIDerivedType(tag: DW_TAG_member, name: "fnPtr", scope: !938, file: !12, line: 183, baseType: !32, size: 64, offset: 128)
!943 = !DIDerivedType(tag: DW_TAG_member, name: "UnregisterNatives", scope: !29, file: !12, line: 723, baseType: !944, size: 64, offset: 13824)
!944 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !945, size: 64)
!945 = !DISubroutineType(types: !946)
!946 = !{!40, !25, !47}
!947 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorEnter", scope: !29, file: !12, line: 726, baseType: !948, size: 64, offset: 13888)
!948 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !949, size: 64)
!949 = !DISubroutineType(types: !950)
!950 = !{!40, !25, !48}
!951 = !DIDerivedType(tag: DW_TAG_member, name: "MonitorExit", scope: !29, file: !12, line: 728, baseType: !948, size: 64, offset: 13952)
!952 = !DIDerivedType(tag: DW_TAG_member, name: "GetJavaVM", scope: !29, file: !12, line: 731, baseType: !953, size: 64, offset: 14016)
!953 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !954, size: 64)
!954 = !DISubroutineType(types: !955)
!955 = !{!40, !25, !956}
!956 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !957, size: 64)
!957 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !958, size: 64)
!958 = !DIDerivedType(tag: DW_TAG_typedef, name: "JavaVM", file: !12, line: 211, baseType: !959)
!959 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !960, size: 64)
!960 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !961)
!961 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "JNIInvokeInterface_", file: !12, line: 1890, size: 512, elements: !962)
!962 = !{!963, !964, !965, !966, !970, !975, !976, !980}
!963 = !DIDerivedType(tag: DW_TAG_member, name: "reserved0", scope: !961, file: !12, line: 1891, baseType: !32, size: 64)
!964 = !DIDerivedType(tag: DW_TAG_member, name: "reserved1", scope: !961, file: !12, line: 1892, baseType: !32, size: 64, offset: 64)
!965 = !DIDerivedType(tag: DW_TAG_member, name: "reserved2", scope: !961, file: !12, line: 1893, baseType: !32, size: 64, offset: 128)
!966 = !DIDerivedType(tag: DW_TAG_member, name: "DestroyJavaVM", scope: !961, file: !12, line: 1895, baseType: !967, size: 64, offset: 192)
!967 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !968, size: 64)
!968 = !DISubroutineType(types: !969)
!969 = !{!40, !957}
!970 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThread", scope: !961, file: !12, line: 1897, baseType: !971, size: 64, offset: 256)
!971 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !972, size: 64)
!972 = !DISubroutineType(types: !973)
!973 = !{!40, !957, !974, !32}
!974 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !32, size: 64)
!975 = !DIDerivedType(tag: DW_TAG_member, name: "DetachCurrentThread", scope: !961, file: !12, line: 1899, baseType: !967, size: 64, offset: 320)
!976 = !DIDerivedType(tag: DW_TAG_member, name: "GetEnv", scope: !961, file: !12, line: 1901, baseType: !977, size: 64, offset: 384)
!977 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !978, size: 64)
!978 = !DISubroutineType(types: !979)
!979 = !{!40, !957, !974, !40}
!980 = !DIDerivedType(tag: DW_TAG_member, name: "AttachCurrentThreadAsDaemon", scope: !961, file: !12, line: 1903, baseType: !971, size: 64, offset: 448)
!981 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringRegion", scope: !29, file: !12, line: 734, baseType: !982, size: 64, offset: 14080)
!982 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !983, size: 64)
!983 = !DISubroutineType(types: !984)
!984 = !{null, !25, !697, !58, !58, !797}
!985 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringUTFRegion", scope: !29, file: !12, line: 736, baseType: !986, size: 64, offset: 14144)
!986 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !987, size: 64)
!987 = !DISubroutineType(types: !988)
!988 = !{null, !25, !697, !58, !58, !151}
!989 = !DIDerivedType(tag: DW_TAG_member, name: "GetPrimitiveArrayCritical", scope: !29, file: !12, line: 739, baseType: !990, size: 64, offset: 14208)
!990 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !991, size: 64)
!991 = !DISubroutineType(types: !992)
!992 = !{!32, !25, !730, !708}
!993 = !DIDerivedType(tag: DW_TAG_member, name: "ReleasePrimitiveArrayCritical", scope: !29, file: !12, line: 741, baseType: !994, size: 64, offset: 14272)
!994 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !995, size: 64)
!995 = !DISubroutineType(types: !996)
!996 = !{null, !25, !730, !32, !40}
!997 = !DIDerivedType(tag: DW_TAG_member, name: "GetStringCritical", scope: !29, file: !12, line: 744, baseType: !705, size: 64, offset: 14336)
!998 = !DIDerivedType(tag: DW_TAG_member, name: "ReleaseStringCritical", scope: !29, file: !12, line: 746, baseType: !710, size: 64, offset: 14400)
!999 = !DIDerivedType(tag: DW_TAG_member, name: "NewWeakGlobalRef", scope: !29, file: !12, line: 749, baseType: !1000, size: 64, offset: 14464)
!1000 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1001, size: 64)
!1001 = !DISubroutineType(types: !1002)
!1002 = !{!1003, !25, !48}
!1003 = !DIDerivedType(tag: DW_TAG_typedef, name: "jweak", file: !12, line: 118, baseType: !48)
!1004 = !DIDerivedType(tag: DW_TAG_member, name: "DeleteWeakGlobalRef", scope: !29, file: !12, line: 751, baseType: !1005, size: 64, offset: 14528)
!1005 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1006, size: 64)
!1006 = !DISubroutineType(types: !1007)
!1007 = !{null, !25, !1003}
!1008 = !DIDerivedType(tag: DW_TAG_member, name: "ExceptionCheck", scope: !29, file: !12, line: 754, baseType: !1009, size: 64, offset: 14592)
!1009 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1010, size: 64)
!1010 = !DISubroutineType(types: !1011)
!1011 = !{!81, !25}
!1012 = !DIDerivedType(tag: DW_TAG_member, name: "NewDirectByteBuffer", scope: !29, file: !12, line: 757, baseType: !1013, size: 64, offset: 14656)
!1013 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1014, size: 64)
!1014 = !DISubroutineType(types: !1015)
!1015 = !{!48, !25, !32, !171}
!1016 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferAddress", scope: !29, file: !12, line: 759, baseType: !1017, size: 64, offset: 14720)
!1017 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1018, size: 64)
!1018 = !DISubroutineType(types: !1019)
!1019 = !{!32, !25, !48}
!1020 = !DIDerivedType(tag: DW_TAG_member, name: "GetDirectBufferCapacity", scope: !29, file: !12, line: 761, baseType: !1021, size: 64, offset: 14784)
!1021 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1022, size: 64)
!1022 = !DISubroutineType(types: !1023)
!1023 = !{!171, !25, !48}
!1024 = !DIDerivedType(tag: DW_TAG_member, name: "GetObjectRefType", scope: !29, file: !12, line: 766, baseType: !1025, size: 64, offset: 14848)
!1025 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1026, size: 64)
!1026 = !DISubroutineType(types: !1027)
!1027 = !{!1028, !25, !48}
!1028 = !DIDerivedType(tag: DW_TAG_typedef, name: "jobjectRefType", file: !12, line: 144, baseType: !11)
!1029 = !DIDerivedType(tag: DW_TAG_typedef, name: "size_t", file: !1030, line: 193, baseType: !7)
!1030 = !DIFile(filename: "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\VC\\Tools\\MSVC\\14.34.31933\\include\\vcruntime.h", directory: "", checksumkind: CSK_MD5, checksum: "39da3a8c8438e40538f3964bd55ef6b8")
!1031 = !{!0}
!1032 = !{}
!1033 = !{i32 2, !"CodeView", i32 1}
!1034 = !{i32 2, !"Debug Info Version", i32 3}
!1035 = !{i32 1, !"wchar_size", i32 2}
!1036 = !{i32 7, !"PIC Level", i32 2}
!1037 = !{i32 7, !"uwtable", i32 2}
!1038 = !{!"clang version 15.0.2"}
!1039 = distinct !DISubprogram(name: "sprintf", scope: !1040, file: !1040, line: 1764, type: !1041, scopeLine: 1771, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1040 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\stdio.h", directory: "", checksumkind: CSK_MD5, checksum: "c1a1fbc43e7d45f0ea4ae539ddcffb19")
!1041 = !DISubroutineType(types: !1042)
!1042 = !{!13, !1043, !1044, null}
!1043 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !151)
!1044 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !51)
!1045 = !DILocalVariable(name: "_Format", arg: 2, scope: !1039, file: !1040, line: 1766, type: !1044)
!1046 = !DILocation(line: 1766, scope: !1039)
!1047 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1039, file: !1040, line: 1765, type: !1043)
!1048 = !DILocation(line: 1765, scope: !1039)
!1049 = !DILocalVariable(name: "_Result", scope: !1039, file: !1040, line: 1772, type: !13)
!1050 = !DILocation(line: 1772, scope: !1039)
!1051 = !DILocalVariable(name: "_ArgList", scope: !1039, file: !1040, line: 1773, type: !149)
!1052 = !DILocation(line: 1773, scope: !1039)
!1053 = !DILocation(line: 1774, scope: !1039)
!1054 = !DILocation(line: 1776, scope: !1039)
!1055 = !DILocation(line: 1778, scope: !1039)
!1056 = !DILocation(line: 1779, scope: !1039)
!1057 = distinct !DISubprogram(name: "vsprintf", scope: !1040, file: !1040, line: 1465, type: !1058, scopeLine: 1473, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1058 = !DISubroutineType(types: !1059)
!1059 = !{!13, !1043, !1044, !149}
!1060 = !DILocalVariable(name: "_ArgList", arg: 3, scope: !1057, file: !1040, line: 1468, type: !149)
!1061 = !DILocation(line: 1468, scope: !1057)
!1062 = !DILocalVariable(name: "_Format", arg: 2, scope: !1057, file: !1040, line: 1467, type: !1044)
!1063 = !DILocation(line: 1467, scope: !1057)
!1064 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1057, file: !1040, line: 1466, type: !1043)
!1065 = !DILocation(line: 1466, scope: !1057)
!1066 = !DILocation(line: 1474, scope: !1057)
!1067 = distinct !DISubprogram(name: "_snprintf", scope: !1040, file: !1040, line: 1939, type: !1068, scopeLine: 1947, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1068 = !DISubroutineType(types: !1069)
!1069 = !{!13, !1043, !1070, !1044, null}
!1070 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !1029)
!1071 = !DILocalVariable(name: "_Format", arg: 3, scope: !1067, file: !1040, line: 1942, type: !1044)
!1072 = !DILocation(line: 1942, scope: !1067)
!1073 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1067, file: !1040, line: 1941, type: !1070)
!1074 = !DILocation(line: 1941, scope: !1067)
!1075 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1067, file: !1040, line: 1940, type: !1043)
!1076 = !DILocation(line: 1940, scope: !1067)
!1077 = !DILocalVariable(name: "_Result", scope: !1067, file: !1040, line: 1948, type: !13)
!1078 = !DILocation(line: 1948, scope: !1067)
!1079 = !DILocalVariable(name: "_ArgList", scope: !1067, file: !1040, line: 1949, type: !149)
!1080 = !DILocation(line: 1949, scope: !1067)
!1081 = !DILocation(line: 1950, scope: !1067)
!1082 = !DILocation(line: 1951, scope: !1067)
!1083 = !DILocation(line: 1952, scope: !1067)
!1084 = !DILocation(line: 1953, scope: !1067)
!1085 = distinct !DISubprogram(name: "_vsnprintf", scope: !1040, file: !1040, line: 1402, type: !1086, scopeLine: 1411, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1086 = !DISubroutineType(types: !1087)
!1087 = !{!13, !1043, !1070, !1044, !149}
!1088 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !1085, file: !1040, line: 1406, type: !149)
!1089 = !DILocation(line: 1406, scope: !1085)
!1090 = !DILocalVariable(name: "_Format", arg: 3, scope: !1085, file: !1040, line: 1405, type: !1044)
!1091 = !DILocation(line: 1405, scope: !1085)
!1092 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1085, file: !1040, line: 1404, type: !1070)
!1093 = !DILocation(line: 1404, scope: !1085)
!1094 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1085, file: !1040, line: 1403, type: !1043)
!1095 = !DILocation(line: 1403, scope: !1085)
!1096 = !DILocation(line: 1412, scope: !1085)
!1097 = distinct !DISubprogram(name: "JNI_CallObjectMethod", scope: !9, file: !9, line: 3, type: !194, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1098 = !DILocalVariable(name: "methodID", arg: 3, scope: !1097, file: !9, line: 3, type: !67)
!1099 = !DILocation(line: 3, scope: !1097)
!1100 = !DILocalVariable(name: "obj", arg: 2, scope: !1097, file: !9, line: 3, type: !48)
!1101 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1097, file: !9, line: 3, type: !25)
!1102 = !DILocalVariable(name: "args", scope: !1097, file: !9, line: 3, type: !149)
!1103 = !DILocalVariable(name: "ret", scope: !1097, file: !9, line: 3, type: !48)
!1104 = distinct !DISubprogram(name: "JNI_CallObjectMethodV", scope: !9, file: !9, line: 3, type: !198, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1105 = !DILocalVariable(name: "args", arg: 4, scope: !1104, file: !9, line: 3, type: !149)
!1106 = !DILocation(line: 3, scope: !1104)
!1107 = !DILocalVariable(name: "methodID", arg: 3, scope: !1104, file: !9, line: 3, type: !67)
!1108 = !DILocalVariable(name: "obj", arg: 2, scope: !1104, file: !9, line: 3, type: !48)
!1109 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1104, file: !9, line: 3, type: !25)
!1110 = !DILocalVariable(name: "sig", scope: !1104, file: !9, line: 3, type: !1111)
!1111 = !DICompositeType(tag: DW_TAG_array_type, baseType: !53, size: 2048, elements: !1112)
!1112 = !{!1113}
!1113 = !DISubrange(count: 256)
!1114 = !DILocalVariable(name: "argc", scope: !1104, file: !9, line: 3, type: !13)
!1115 = !DILocalVariable(name: "argv", scope: !1104, file: !9, line: 3, type: !1116)
!1116 = !DICompositeType(tag: DW_TAG_array_type, baseType: !158, size: 16384, elements: !1112)
!1117 = !DILocalVariable(name: "i", scope: !1118, file: !9, line: 3, type: !13)
!1118 = distinct !DILexicalBlock(scope: !1104, file: !9, line: 3)
!1119 = !DILocation(line: 3, scope: !1118)
!1120 = !DILocation(line: 3, scope: !1121)
!1121 = distinct !DILexicalBlock(scope: !1122, file: !9, line: 3)
!1122 = distinct !DILexicalBlock(scope: !1118, file: !9, line: 3)
!1123 = !DILocation(line: 3, scope: !1124)
!1124 = distinct !DILexicalBlock(scope: !1121, file: !9, line: 3)
!1125 = !DILocation(line: 3, scope: !1122)
!1126 = distinct !{!1126, !1119, !1119, !1127}
!1127 = !{!"llvm.loop.mustprogress"}
!1128 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethod", scope: !9, file: !9, line: 3, type: !314, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1129 = !DILocalVariable(name: "methodID", arg: 4, scope: !1128, file: !9, line: 3, type: !67)
!1130 = !DILocation(line: 3, scope: !1128)
!1131 = !DILocalVariable(name: "clazz", arg: 3, scope: !1128, file: !9, line: 3, type: !47)
!1132 = !DILocalVariable(name: "obj", arg: 2, scope: !1128, file: !9, line: 3, type: !48)
!1133 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1128, file: !9, line: 3, type: !25)
!1134 = !DILocalVariable(name: "args", scope: !1128, file: !9, line: 3, type: !149)
!1135 = !DILocalVariable(name: "ret", scope: !1128, file: !9, line: 3, type: !48)
!1136 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethodV", scope: !9, file: !9, line: 3, type: !318, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1137 = !DILocalVariable(name: "args", arg: 5, scope: !1136, file: !9, line: 3, type: !149)
!1138 = !DILocation(line: 3, scope: !1136)
!1139 = !DILocalVariable(name: "methodID", arg: 4, scope: !1136, file: !9, line: 3, type: !67)
!1140 = !DILocalVariable(name: "clazz", arg: 3, scope: !1136, file: !9, line: 3, type: !47)
!1141 = !DILocalVariable(name: "obj", arg: 2, scope: !1136, file: !9, line: 3, type: !48)
!1142 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1136, file: !9, line: 3, type: !25)
!1143 = !DILocalVariable(name: "sig", scope: !1136, file: !9, line: 3, type: !1111)
!1144 = !DILocalVariable(name: "argc", scope: !1136, file: !9, line: 3, type: !13)
!1145 = !DILocalVariable(name: "argv", scope: !1136, file: !9, line: 3, type: !1116)
!1146 = !DILocalVariable(name: "i", scope: !1147, file: !9, line: 3, type: !13)
!1147 = distinct !DILexicalBlock(scope: !1136, file: !9, line: 3)
!1148 = !DILocation(line: 3, scope: !1147)
!1149 = !DILocation(line: 3, scope: !1150)
!1150 = distinct !DILexicalBlock(scope: !1151, file: !9, line: 3)
!1151 = distinct !DILexicalBlock(scope: !1147, file: !9, line: 3)
!1152 = !DILocation(line: 3, scope: !1153)
!1153 = distinct !DILexicalBlock(scope: !1150, file: !9, line: 3)
!1154 = !DILocation(line: 3, scope: !1151)
!1155 = distinct !{!1155, !1148, !1148, !1127}
!1156 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethod", scope: !9, file: !9, line: 3, type: !143, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1157 = !DILocalVariable(name: "methodID", arg: 3, scope: !1156, file: !9, line: 3, type: !67)
!1158 = !DILocation(line: 3, scope: !1156)
!1159 = !DILocalVariable(name: "clazz", arg: 2, scope: !1156, file: !9, line: 3, type: !47)
!1160 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1156, file: !9, line: 3, type: !25)
!1161 = !DILocalVariable(name: "args", scope: !1156, file: !9, line: 3, type: !149)
!1162 = !DILocalVariable(name: "ret", scope: !1156, file: !9, line: 3, type: !48)
!1163 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethodV", scope: !9, file: !9, line: 3, type: !147, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1164 = !DILocalVariable(name: "args", arg: 4, scope: !1163, file: !9, line: 3, type: !149)
!1165 = !DILocation(line: 3, scope: !1163)
!1166 = !DILocalVariable(name: "methodID", arg: 3, scope: !1163, file: !9, line: 3, type: !67)
!1167 = !DILocalVariable(name: "clazz", arg: 2, scope: !1163, file: !9, line: 3, type: !47)
!1168 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1163, file: !9, line: 3, type: !25)
!1169 = !DILocalVariable(name: "sig", scope: !1163, file: !9, line: 3, type: !1111)
!1170 = !DILocalVariable(name: "argc", scope: !1163, file: !9, line: 3, type: !13)
!1171 = !DILocalVariable(name: "argv", scope: !1163, file: !9, line: 3, type: !1116)
!1172 = !DILocalVariable(name: "i", scope: !1173, file: !9, line: 3, type: !13)
!1173 = distinct !DILexicalBlock(scope: !1163, file: !9, line: 3)
!1174 = !DILocation(line: 3, scope: !1173)
!1175 = !DILocation(line: 3, scope: !1176)
!1176 = distinct !DILexicalBlock(scope: !1177, file: !9, line: 3)
!1177 = distinct !DILexicalBlock(scope: !1173, file: !9, line: 3)
!1178 = !DILocation(line: 3, scope: !1179)
!1179 = distinct !DILexicalBlock(scope: !1176, file: !9, line: 3)
!1180 = !DILocation(line: 3, scope: !1177)
!1181 = distinct !{!1181, !1174, !1174, !1127}
!1182 = distinct !DISubprogram(name: "JNI_CallBooleanMethod", scope: !9, file: !9, line: 4, type: !206, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1183 = !DILocalVariable(name: "methodID", arg: 3, scope: !1182, file: !9, line: 4, type: !67)
!1184 = !DILocation(line: 4, scope: !1182)
!1185 = !DILocalVariable(name: "obj", arg: 2, scope: !1182, file: !9, line: 4, type: !48)
!1186 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1182, file: !9, line: 4, type: !25)
!1187 = !DILocalVariable(name: "args", scope: !1182, file: !9, line: 4, type: !149)
!1188 = !DILocalVariable(name: "ret", scope: !1182, file: !9, line: 4, type: !81)
!1189 = distinct !DISubprogram(name: "JNI_CallBooleanMethodV", scope: !9, file: !9, line: 4, type: !210, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1190 = !DILocalVariable(name: "args", arg: 4, scope: !1189, file: !9, line: 4, type: !149)
!1191 = !DILocation(line: 4, scope: !1189)
!1192 = !DILocalVariable(name: "methodID", arg: 3, scope: !1189, file: !9, line: 4, type: !67)
!1193 = !DILocalVariable(name: "obj", arg: 2, scope: !1189, file: !9, line: 4, type: !48)
!1194 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1189, file: !9, line: 4, type: !25)
!1195 = !DILocalVariable(name: "sig", scope: !1189, file: !9, line: 4, type: !1111)
!1196 = !DILocalVariable(name: "argc", scope: !1189, file: !9, line: 4, type: !13)
!1197 = !DILocalVariable(name: "argv", scope: !1189, file: !9, line: 4, type: !1116)
!1198 = !DILocalVariable(name: "i", scope: !1199, file: !9, line: 4, type: !13)
!1199 = distinct !DILexicalBlock(scope: !1189, file: !9, line: 4)
!1200 = !DILocation(line: 4, scope: !1199)
!1201 = !DILocation(line: 4, scope: !1202)
!1202 = distinct !DILexicalBlock(scope: !1203, file: !9, line: 4)
!1203 = distinct !DILexicalBlock(scope: !1199, file: !9, line: 4)
!1204 = !DILocation(line: 4, scope: !1205)
!1205 = distinct !DILexicalBlock(scope: !1202, file: !9, line: 4)
!1206 = !DILocation(line: 4, scope: !1203)
!1207 = distinct !{!1207, !1200, !1200, !1127}
!1208 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethod", scope: !9, file: !9, line: 4, type: !326, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1209 = !DILocalVariable(name: "methodID", arg: 4, scope: !1208, file: !9, line: 4, type: !67)
!1210 = !DILocation(line: 4, scope: !1208)
!1211 = !DILocalVariable(name: "clazz", arg: 3, scope: !1208, file: !9, line: 4, type: !47)
!1212 = !DILocalVariable(name: "obj", arg: 2, scope: !1208, file: !9, line: 4, type: !48)
!1213 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1208, file: !9, line: 4, type: !25)
!1214 = !DILocalVariable(name: "args", scope: !1208, file: !9, line: 4, type: !149)
!1215 = !DILocalVariable(name: "ret", scope: !1208, file: !9, line: 4, type: !81)
!1216 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethodV", scope: !9, file: !9, line: 4, type: !330, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1217 = !DILocalVariable(name: "args", arg: 5, scope: !1216, file: !9, line: 4, type: !149)
!1218 = !DILocation(line: 4, scope: !1216)
!1219 = !DILocalVariable(name: "methodID", arg: 4, scope: !1216, file: !9, line: 4, type: !67)
!1220 = !DILocalVariable(name: "clazz", arg: 3, scope: !1216, file: !9, line: 4, type: !47)
!1221 = !DILocalVariable(name: "obj", arg: 2, scope: !1216, file: !9, line: 4, type: !48)
!1222 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1216, file: !9, line: 4, type: !25)
!1223 = !DILocalVariable(name: "sig", scope: !1216, file: !9, line: 4, type: !1111)
!1224 = !DILocalVariable(name: "argc", scope: !1216, file: !9, line: 4, type: !13)
!1225 = !DILocalVariable(name: "argv", scope: !1216, file: !9, line: 4, type: !1116)
!1226 = !DILocalVariable(name: "i", scope: !1227, file: !9, line: 4, type: !13)
!1227 = distinct !DILexicalBlock(scope: !1216, file: !9, line: 4)
!1228 = !DILocation(line: 4, scope: !1227)
!1229 = !DILocation(line: 4, scope: !1230)
!1230 = distinct !DILexicalBlock(scope: !1231, file: !9, line: 4)
!1231 = distinct !DILexicalBlock(scope: !1227, file: !9, line: 4)
!1232 = !DILocation(line: 4, scope: !1233)
!1233 = distinct !DILexicalBlock(scope: !1230, file: !9, line: 4)
!1234 = !DILocation(line: 4, scope: !1231)
!1235 = distinct !{!1235, !1228, !1228, !1127}
!1236 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethod", scope: !9, file: !9, line: 4, type: !514, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1237 = !DILocalVariable(name: "methodID", arg: 3, scope: !1236, file: !9, line: 4, type: !67)
!1238 = !DILocation(line: 4, scope: !1236)
!1239 = !DILocalVariable(name: "clazz", arg: 2, scope: !1236, file: !9, line: 4, type: !47)
!1240 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1236, file: !9, line: 4, type: !25)
!1241 = !DILocalVariable(name: "args", scope: !1236, file: !9, line: 4, type: !149)
!1242 = !DILocalVariable(name: "ret", scope: !1236, file: !9, line: 4, type: !81)
!1243 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethodV", scope: !9, file: !9, line: 4, type: !518, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1244 = !DILocalVariable(name: "args", arg: 4, scope: !1243, file: !9, line: 4, type: !149)
!1245 = !DILocation(line: 4, scope: !1243)
!1246 = !DILocalVariable(name: "methodID", arg: 3, scope: !1243, file: !9, line: 4, type: !67)
!1247 = !DILocalVariable(name: "clazz", arg: 2, scope: !1243, file: !9, line: 4, type: !47)
!1248 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1243, file: !9, line: 4, type: !25)
!1249 = !DILocalVariable(name: "sig", scope: !1243, file: !9, line: 4, type: !1111)
!1250 = !DILocalVariable(name: "argc", scope: !1243, file: !9, line: 4, type: !13)
!1251 = !DILocalVariable(name: "argv", scope: !1243, file: !9, line: 4, type: !1116)
!1252 = !DILocalVariable(name: "i", scope: !1253, file: !9, line: 4, type: !13)
!1253 = distinct !DILexicalBlock(scope: !1243, file: !9, line: 4)
!1254 = !DILocation(line: 4, scope: !1253)
!1255 = !DILocation(line: 4, scope: !1256)
!1256 = distinct !DILexicalBlock(scope: !1257, file: !9, line: 4)
!1257 = distinct !DILexicalBlock(scope: !1253, file: !9, line: 4)
!1258 = !DILocation(line: 4, scope: !1259)
!1259 = distinct !DILexicalBlock(scope: !1256, file: !9, line: 4)
!1260 = !DILocation(line: 4, scope: !1257)
!1261 = distinct !{!1261, !1254, !1254, !1127}
!1262 = distinct !DISubprogram(name: "JNI_CallByteMethod", scope: !9, file: !9, line: 5, type: !218, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1263 = !DILocalVariable(name: "methodID", arg: 3, scope: !1262, file: !9, line: 5, type: !67)
!1264 = !DILocation(line: 5, scope: !1262)
!1265 = !DILocalVariable(name: "obj", arg: 2, scope: !1262, file: !9, line: 5, type: !48)
!1266 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1262, file: !9, line: 5, type: !25)
!1267 = !DILocalVariable(name: "args", scope: !1262, file: !9, line: 5, type: !149)
!1268 = !DILocalVariable(name: "ret", scope: !1262, file: !9, line: 5, type: !56)
!1269 = distinct !DISubprogram(name: "JNI_CallByteMethodV", scope: !9, file: !9, line: 5, type: !222, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1270 = !DILocalVariable(name: "args", arg: 4, scope: !1269, file: !9, line: 5, type: !149)
!1271 = !DILocation(line: 5, scope: !1269)
!1272 = !DILocalVariable(name: "methodID", arg: 3, scope: !1269, file: !9, line: 5, type: !67)
!1273 = !DILocalVariable(name: "obj", arg: 2, scope: !1269, file: !9, line: 5, type: !48)
!1274 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1269, file: !9, line: 5, type: !25)
!1275 = !DILocalVariable(name: "sig", scope: !1269, file: !9, line: 5, type: !1111)
!1276 = !DILocalVariable(name: "argc", scope: !1269, file: !9, line: 5, type: !13)
!1277 = !DILocalVariable(name: "argv", scope: !1269, file: !9, line: 5, type: !1116)
!1278 = !DILocalVariable(name: "i", scope: !1279, file: !9, line: 5, type: !13)
!1279 = distinct !DILexicalBlock(scope: !1269, file: !9, line: 5)
!1280 = !DILocation(line: 5, scope: !1279)
!1281 = !DILocation(line: 5, scope: !1282)
!1282 = distinct !DILexicalBlock(scope: !1283, file: !9, line: 5)
!1283 = distinct !DILexicalBlock(scope: !1279, file: !9, line: 5)
!1284 = !DILocation(line: 5, scope: !1285)
!1285 = distinct !DILexicalBlock(scope: !1282, file: !9, line: 5)
!1286 = !DILocation(line: 5, scope: !1283)
!1287 = distinct !{!1287, !1280, !1280, !1127}
!1288 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethod", scope: !9, file: !9, line: 5, type: !338, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1289 = !DILocalVariable(name: "methodID", arg: 4, scope: !1288, file: !9, line: 5, type: !67)
!1290 = !DILocation(line: 5, scope: !1288)
!1291 = !DILocalVariable(name: "clazz", arg: 3, scope: !1288, file: !9, line: 5, type: !47)
!1292 = !DILocalVariable(name: "obj", arg: 2, scope: !1288, file: !9, line: 5, type: !48)
!1293 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1288, file: !9, line: 5, type: !25)
!1294 = !DILocalVariable(name: "args", scope: !1288, file: !9, line: 5, type: !149)
!1295 = !DILocalVariable(name: "ret", scope: !1288, file: !9, line: 5, type: !56)
!1296 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethodV", scope: !9, file: !9, line: 5, type: !342, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1297 = !DILocalVariable(name: "args", arg: 5, scope: !1296, file: !9, line: 5, type: !149)
!1298 = !DILocation(line: 5, scope: !1296)
!1299 = !DILocalVariable(name: "methodID", arg: 4, scope: !1296, file: !9, line: 5, type: !67)
!1300 = !DILocalVariable(name: "clazz", arg: 3, scope: !1296, file: !9, line: 5, type: !47)
!1301 = !DILocalVariable(name: "obj", arg: 2, scope: !1296, file: !9, line: 5, type: !48)
!1302 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1296, file: !9, line: 5, type: !25)
!1303 = !DILocalVariable(name: "sig", scope: !1296, file: !9, line: 5, type: !1111)
!1304 = !DILocalVariable(name: "argc", scope: !1296, file: !9, line: 5, type: !13)
!1305 = !DILocalVariable(name: "argv", scope: !1296, file: !9, line: 5, type: !1116)
!1306 = !DILocalVariable(name: "i", scope: !1307, file: !9, line: 5, type: !13)
!1307 = distinct !DILexicalBlock(scope: !1296, file: !9, line: 5)
!1308 = !DILocation(line: 5, scope: !1307)
!1309 = !DILocation(line: 5, scope: !1310)
!1310 = distinct !DILexicalBlock(scope: !1311, file: !9, line: 5)
!1311 = distinct !DILexicalBlock(scope: !1307, file: !9, line: 5)
!1312 = !DILocation(line: 5, scope: !1313)
!1313 = distinct !DILexicalBlock(scope: !1310, file: !9, line: 5)
!1314 = !DILocation(line: 5, scope: !1311)
!1315 = distinct !{!1315, !1308, !1308, !1127}
!1316 = distinct !DISubprogram(name: "JNI_CallStaticByteMethod", scope: !9, file: !9, line: 5, type: !526, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1317 = !DILocalVariable(name: "methodID", arg: 3, scope: !1316, file: !9, line: 5, type: !67)
!1318 = !DILocation(line: 5, scope: !1316)
!1319 = !DILocalVariable(name: "clazz", arg: 2, scope: !1316, file: !9, line: 5, type: !47)
!1320 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1316, file: !9, line: 5, type: !25)
!1321 = !DILocalVariable(name: "args", scope: !1316, file: !9, line: 5, type: !149)
!1322 = !DILocalVariable(name: "ret", scope: !1316, file: !9, line: 5, type: !56)
!1323 = distinct !DISubprogram(name: "JNI_CallStaticByteMethodV", scope: !9, file: !9, line: 5, type: !530, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1324 = !DILocalVariable(name: "args", arg: 4, scope: !1323, file: !9, line: 5, type: !149)
!1325 = !DILocation(line: 5, scope: !1323)
!1326 = !DILocalVariable(name: "methodID", arg: 3, scope: !1323, file: !9, line: 5, type: !67)
!1327 = !DILocalVariable(name: "clazz", arg: 2, scope: !1323, file: !9, line: 5, type: !47)
!1328 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1323, file: !9, line: 5, type: !25)
!1329 = !DILocalVariable(name: "sig", scope: !1323, file: !9, line: 5, type: !1111)
!1330 = !DILocalVariable(name: "argc", scope: !1323, file: !9, line: 5, type: !13)
!1331 = !DILocalVariable(name: "argv", scope: !1323, file: !9, line: 5, type: !1116)
!1332 = !DILocalVariable(name: "i", scope: !1333, file: !9, line: 5, type: !13)
!1333 = distinct !DILexicalBlock(scope: !1323, file: !9, line: 5)
!1334 = !DILocation(line: 5, scope: !1333)
!1335 = !DILocation(line: 5, scope: !1336)
!1336 = distinct !DILexicalBlock(scope: !1337, file: !9, line: 5)
!1337 = distinct !DILexicalBlock(scope: !1333, file: !9, line: 5)
!1338 = !DILocation(line: 5, scope: !1339)
!1339 = distinct !DILexicalBlock(scope: !1336, file: !9, line: 5)
!1340 = !DILocation(line: 5, scope: !1337)
!1341 = distinct !{!1341, !1334, !1334, !1127}
!1342 = distinct !DISubprogram(name: "JNI_CallCharMethod", scope: !9, file: !9, line: 6, type: !230, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1343 = !DILocalVariable(name: "methodID", arg: 3, scope: !1342, file: !9, line: 6, type: !67)
!1344 = !DILocation(line: 6, scope: !1342)
!1345 = !DILocalVariable(name: "obj", arg: 2, scope: !1342, file: !9, line: 6, type: !48)
!1346 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1342, file: !9, line: 6, type: !25)
!1347 = !DILocalVariable(name: "args", scope: !1342, file: !9, line: 6, type: !149)
!1348 = !DILocalVariable(name: "ret", scope: !1342, file: !9, line: 6, type: !164)
!1349 = distinct !DISubprogram(name: "JNI_CallCharMethodV", scope: !9, file: !9, line: 6, type: !234, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1350 = !DILocalVariable(name: "args", arg: 4, scope: !1349, file: !9, line: 6, type: !149)
!1351 = !DILocation(line: 6, scope: !1349)
!1352 = !DILocalVariable(name: "methodID", arg: 3, scope: !1349, file: !9, line: 6, type: !67)
!1353 = !DILocalVariable(name: "obj", arg: 2, scope: !1349, file: !9, line: 6, type: !48)
!1354 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1349, file: !9, line: 6, type: !25)
!1355 = !DILocalVariable(name: "sig", scope: !1349, file: !9, line: 6, type: !1111)
!1356 = !DILocalVariable(name: "argc", scope: !1349, file: !9, line: 6, type: !13)
!1357 = !DILocalVariable(name: "argv", scope: !1349, file: !9, line: 6, type: !1116)
!1358 = !DILocalVariable(name: "i", scope: !1359, file: !9, line: 6, type: !13)
!1359 = distinct !DILexicalBlock(scope: !1349, file: !9, line: 6)
!1360 = !DILocation(line: 6, scope: !1359)
!1361 = !DILocation(line: 6, scope: !1362)
!1362 = distinct !DILexicalBlock(scope: !1363, file: !9, line: 6)
!1363 = distinct !DILexicalBlock(scope: !1359, file: !9, line: 6)
!1364 = !DILocation(line: 6, scope: !1365)
!1365 = distinct !DILexicalBlock(scope: !1362, file: !9, line: 6)
!1366 = !DILocation(line: 6, scope: !1363)
!1367 = distinct !{!1367, !1360, !1360, !1127}
!1368 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethod", scope: !9, file: !9, line: 6, type: !350, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1369 = !DILocalVariable(name: "methodID", arg: 4, scope: !1368, file: !9, line: 6, type: !67)
!1370 = !DILocation(line: 6, scope: !1368)
!1371 = !DILocalVariable(name: "clazz", arg: 3, scope: !1368, file: !9, line: 6, type: !47)
!1372 = !DILocalVariable(name: "obj", arg: 2, scope: !1368, file: !9, line: 6, type: !48)
!1373 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1368, file: !9, line: 6, type: !25)
!1374 = !DILocalVariable(name: "args", scope: !1368, file: !9, line: 6, type: !149)
!1375 = !DILocalVariable(name: "ret", scope: !1368, file: !9, line: 6, type: !164)
!1376 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethodV", scope: !9, file: !9, line: 6, type: !354, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1377 = !DILocalVariable(name: "args", arg: 5, scope: !1376, file: !9, line: 6, type: !149)
!1378 = !DILocation(line: 6, scope: !1376)
!1379 = !DILocalVariable(name: "methodID", arg: 4, scope: !1376, file: !9, line: 6, type: !67)
!1380 = !DILocalVariable(name: "clazz", arg: 3, scope: !1376, file: !9, line: 6, type: !47)
!1381 = !DILocalVariable(name: "obj", arg: 2, scope: !1376, file: !9, line: 6, type: !48)
!1382 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1376, file: !9, line: 6, type: !25)
!1383 = !DILocalVariable(name: "sig", scope: !1376, file: !9, line: 6, type: !1111)
!1384 = !DILocalVariable(name: "argc", scope: !1376, file: !9, line: 6, type: !13)
!1385 = !DILocalVariable(name: "argv", scope: !1376, file: !9, line: 6, type: !1116)
!1386 = !DILocalVariable(name: "i", scope: !1387, file: !9, line: 6, type: !13)
!1387 = distinct !DILexicalBlock(scope: !1376, file: !9, line: 6)
!1388 = !DILocation(line: 6, scope: !1387)
!1389 = !DILocation(line: 6, scope: !1390)
!1390 = distinct !DILexicalBlock(scope: !1391, file: !9, line: 6)
!1391 = distinct !DILexicalBlock(scope: !1387, file: !9, line: 6)
!1392 = !DILocation(line: 6, scope: !1393)
!1393 = distinct !DILexicalBlock(scope: !1390, file: !9, line: 6)
!1394 = !DILocation(line: 6, scope: !1391)
!1395 = distinct !{!1395, !1388, !1388, !1127}
!1396 = distinct !DISubprogram(name: "JNI_CallStaticCharMethod", scope: !9, file: !9, line: 6, type: !538, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1397 = !DILocalVariable(name: "methodID", arg: 3, scope: !1396, file: !9, line: 6, type: !67)
!1398 = !DILocation(line: 6, scope: !1396)
!1399 = !DILocalVariable(name: "clazz", arg: 2, scope: !1396, file: !9, line: 6, type: !47)
!1400 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1396, file: !9, line: 6, type: !25)
!1401 = !DILocalVariable(name: "args", scope: !1396, file: !9, line: 6, type: !149)
!1402 = !DILocalVariable(name: "ret", scope: !1396, file: !9, line: 6, type: !164)
!1403 = distinct !DISubprogram(name: "JNI_CallStaticCharMethodV", scope: !9, file: !9, line: 6, type: !542, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1404 = !DILocalVariable(name: "args", arg: 4, scope: !1403, file: !9, line: 6, type: !149)
!1405 = !DILocation(line: 6, scope: !1403)
!1406 = !DILocalVariable(name: "methodID", arg: 3, scope: !1403, file: !9, line: 6, type: !67)
!1407 = !DILocalVariable(name: "clazz", arg: 2, scope: !1403, file: !9, line: 6, type: !47)
!1408 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1403, file: !9, line: 6, type: !25)
!1409 = !DILocalVariable(name: "sig", scope: !1403, file: !9, line: 6, type: !1111)
!1410 = !DILocalVariable(name: "argc", scope: !1403, file: !9, line: 6, type: !13)
!1411 = !DILocalVariable(name: "argv", scope: !1403, file: !9, line: 6, type: !1116)
!1412 = !DILocalVariable(name: "i", scope: !1413, file: !9, line: 6, type: !13)
!1413 = distinct !DILexicalBlock(scope: !1403, file: !9, line: 6)
!1414 = !DILocation(line: 6, scope: !1413)
!1415 = !DILocation(line: 6, scope: !1416)
!1416 = distinct !DILexicalBlock(scope: !1417, file: !9, line: 6)
!1417 = distinct !DILexicalBlock(scope: !1413, file: !9, line: 6)
!1418 = !DILocation(line: 6, scope: !1419)
!1419 = distinct !DILexicalBlock(scope: !1416, file: !9, line: 6)
!1420 = !DILocation(line: 6, scope: !1417)
!1421 = distinct !{!1421, !1414, !1414, !1127}
!1422 = distinct !DISubprogram(name: "JNI_CallShortMethod", scope: !9, file: !9, line: 7, type: !242, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1423 = !DILocalVariable(name: "methodID", arg: 3, scope: !1422, file: !9, line: 7, type: !67)
!1424 = !DILocation(line: 7, scope: !1422)
!1425 = !DILocalVariable(name: "obj", arg: 2, scope: !1422, file: !9, line: 7, type: !48)
!1426 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1422, file: !9, line: 7, type: !25)
!1427 = !DILocalVariable(name: "args", scope: !1422, file: !9, line: 7, type: !149)
!1428 = !DILocalVariable(name: "ret", scope: !1422, file: !9, line: 7, type: !167)
!1429 = distinct !DISubprogram(name: "JNI_CallShortMethodV", scope: !9, file: !9, line: 7, type: !246, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1430 = !DILocalVariable(name: "args", arg: 4, scope: !1429, file: !9, line: 7, type: !149)
!1431 = !DILocation(line: 7, scope: !1429)
!1432 = !DILocalVariable(name: "methodID", arg: 3, scope: !1429, file: !9, line: 7, type: !67)
!1433 = !DILocalVariable(name: "obj", arg: 2, scope: !1429, file: !9, line: 7, type: !48)
!1434 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1429, file: !9, line: 7, type: !25)
!1435 = !DILocalVariable(name: "sig", scope: !1429, file: !9, line: 7, type: !1111)
!1436 = !DILocalVariable(name: "argc", scope: !1429, file: !9, line: 7, type: !13)
!1437 = !DILocalVariable(name: "argv", scope: !1429, file: !9, line: 7, type: !1116)
!1438 = !DILocalVariable(name: "i", scope: !1439, file: !9, line: 7, type: !13)
!1439 = distinct !DILexicalBlock(scope: !1429, file: !9, line: 7)
!1440 = !DILocation(line: 7, scope: !1439)
!1441 = !DILocation(line: 7, scope: !1442)
!1442 = distinct !DILexicalBlock(scope: !1443, file: !9, line: 7)
!1443 = distinct !DILexicalBlock(scope: !1439, file: !9, line: 7)
!1444 = !DILocation(line: 7, scope: !1445)
!1445 = distinct !DILexicalBlock(scope: !1442, file: !9, line: 7)
!1446 = !DILocation(line: 7, scope: !1443)
!1447 = distinct !{!1447, !1440, !1440, !1127}
!1448 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethod", scope: !9, file: !9, line: 7, type: !362, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1449 = !DILocalVariable(name: "methodID", arg: 4, scope: !1448, file: !9, line: 7, type: !67)
!1450 = !DILocation(line: 7, scope: !1448)
!1451 = !DILocalVariable(name: "clazz", arg: 3, scope: !1448, file: !9, line: 7, type: !47)
!1452 = !DILocalVariable(name: "obj", arg: 2, scope: !1448, file: !9, line: 7, type: !48)
!1453 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1448, file: !9, line: 7, type: !25)
!1454 = !DILocalVariable(name: "args", scope: !1448, file: !9, line: 7, type: !149)
!1455 = !DILocalVariable(name: "ret", scope: !1448, file: !9, line: 7, type: !167)
!1456 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethodV", scope: !9, file: !9, line: 7, type: !366, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1457 = !DILocalVariable(name: "args", arg: 5, scope: !1456, file: !9, line: 7, type: !149)
!1458 = !DILocation(line: 7, scope: !1456)
!1459 = !DILocalVariable(name: "methodID", arg: 4, scope: !1456, file: !9, line: 7, type: !67)
!1460 = !DILocalVariable(name: "clazz", arg: 3, scope: !1456, file: !9, line: 7, type: !47)
!1461 = !DILocalVariable(name: "obj", arg: 2, scope: !1456, file: !9, line: 7, type: !48)
!1462 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1456, file: !9, line: 7, type: !25)
!1463 = !DILocalVariable(name: "sig", scope: !1456, file: !9, line: 7, type: !1111)
!1464 = !DILocalVariable(name: "argc", scope: !1456, file: !9, line: 7, type: !13)
!1465 = !DILocalVariable(name: "argv", scope: !1456, file: !9, line: 7, type: !1116)
!1466 = !DILocalVariable(name: "i", scope: !1467, file: !9, line: 7, type: !13)
!1467 = distinct !DILexicalBlock(scope: !1456, file: !9, line: 7)
!1468 = !DILocation(line: 7, scope: !1467)
!1469 = !DILocation(line: 7, scope: !1470)
!1470 = distinct !DILexicalBlock(scope: !1471, file: !9, line: 7)
!1471 = distinct !DILexicalBlock(scope: !1467, file: !9, line: 7)
!1472 = !DILocation(line: 7, scope: !1473)
!1473 = distinct !DILexicalBlock(scope: !1470, file: !9, line: 7)
!1474 = !DILocation(line: 7, scope: !1471)
!1475 = distinct !{!1475, !1468, !1468, !1127}
!1476 = distinct !DISubprogram(name: "JNI_CallStaticShortMethod", scope: !9, file: !9, line: 7, type: !550, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1477 = !DILocalVariable(name: "methodID", arg: 3, scope: !1476, file: !9, line: 7, type: !67)
!1478 = !DILocation(line: 7, scope: !1476)
!1479 = !DILocalVariable(name: "clazz", arg: 2, scope: !1476, file: !9, line: 7, type: !47)
!1480 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1476, file: !9, line: 7, type: !25)
!1481 = !DILocalVariable(name: "args", scope: !1476, file: !9, line: 7, type: !149)
!1482 = !DILocalVariable(name: "ret", scope: !1476, file: !9, line: 7, type: !167)
!1483 = distinct !DISubprogram(name: "JNI_CallStaticShortMethodV", scope: !9, file: !9, line: 7, type: !554, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1484 = !DILocalVariable(name: "args", arg: 4, scope: !1483, file: !9, line: 7, type: !149)
!1485 = !DILocation(line: 7, scope: !1483)
!1486 = !DILocalVariable(name: "methodID", arg: 3, scope: !1483, file: !9, line: 7, type: !67)
!1487 = !DILocalVariable(name: "clazz", arg: 2, scope: !1483, file: !9, line: 7, type: !47)
!1488 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1483, file: !9, line: 7, type: !25)
!1489 = !DILocalVariable(name: "sig", scope: !1483, file: !9, line: 7, type: !1111)
!1490 = !DILocalVariable(name: "argc", scope: !1483, file: !9, line: 7, type: !13)
!1491 = !DILocalVariable(name: "argv", scope: !1483, file: !9, line: 7, type: !1116)
!1492 = !DILocalVariable(name: "i", scope: !1493, file: !9, line: 7, type: !13)
!1493 = distinct !DILexicalBlock(scope: !1483, file: !9, line: 7)
!1494 = !DILocation(line: 7, scope: !1493)
!1495 = !DILocation(line: 7, scope: !1496)
!1496 = distinct !DILexicalBlock(scope: !1497, file: !9, line: 7)
!1497 = distinct !DILexicalBlock(scope: !1493, file: !9, line: 7)
!1498 = !DILocation(line: 7, scope: !1499)
!1499 = distinct !DILexicalBlock(scope: !1496, file: !9, line: 7)
!1500 = !DILocation(line: 7, scope: !1497)
!1501 = distinct !{!1501, !1494, !1494, !1127}
!1502 = distinct !DISubprogram(name: "JNI_CallIntMethod", scope: !9, file: !9, line: 8, type: !254, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1503 = !DILocalVariable(name: "methodID", arg: 3, scope: !1502, file: !9, line: 8, type: !67)
!1504 = !DILocation(line: 8, scope: !1502)
!1505 = !DILocalVariable(name: "obj", arg: 2, scope: !1502, file: !9, line: 8, type: !48)
!1506 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1502, file: !9, line: 8, type: !25)
!1507 = !DILocalVariable(name: "args", scope: !1502, file: !9, line: 8, type: !149)
!1508 = !DILocalVariable(name: "ret", scope: !1502, file: !9, line: 8, type: !40)
!1509 = distinct !DISubprogram(name: "JNI_CallIntMethodV", scope: !9, file: !9, line: 8, type: !258, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1510 = !DILocalVariable(name: "args", arg: 4, scope: !1509, file: !9, line: 8, type: !149)
!1511 = !DILocation(line: 8, scope: !1509)
!1512 = !DILocalVariable(name: "methodID", arg: 3, scope: !1509, file: !9, line: 8, type: !67)
!1513 = !DILocalVariable(name: "obj", arg: 2, scope: !1509, file: !9, line: 8, type: !48)
!1514 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1509, file: !9, line: 8, type: !25)
!1515 = !DILocalVariable(name: "sig", scope: !1509, file: !9, line: 8, type: !1111)
!1516 = !DILocalVariable(name: "argc", scope: !1509, file: !9, line: 8, type: !13)
!1517 = !DILocalVariable(name: "argv", scope: !1509, file: !9, line: 8, type: !1116)
!1518 = !DILocalVariable(name: "i", scope: !1519, file: !9, line: 8, type: !13)
!1519 = distinct !DILexicalBlock(scope: !1509, file: !9, line: 8)
!1520 = !DILocation(line: 8, scope: !1519)
!1521 = !DILocation(line: 8, scope: !1522)
!1522 = distinct !DILexicalBlock(scope: !1523, file: !9, line: 8)
!1523 = distinct !DILexicalBlock(scope: !1519, file: !9, line: 8)
!1524 = !DILocation(line: 8, scope: !1525)
!1525 = distinct !DILexicalBlock(scope: !1522, file: !9, line: 8)
!1526 = !DILocation(line: 8, scope: !1523)
!1527 = distinct !{!1527, !1520, !1520, !1127}
!1528 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethod", scope: !9, file: !9, line: 8, type: !374, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1529 = !DILocalVariable(name: "methodID", arg: 4, scope: !1528, file: !9, line: 8, type: !67)
!1530 = !DILocation(line: 8, scope: !1528)
!1531 = !DILocalVariable(name: "clazz", arg: 3, scope: !1528, file: !9, line: 8, type: !47)
!1532 = !DILocalVariable(name: "obj", arg: 2, scope: !1528, file: !9, line: 8, type: !48)
!1533 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1528, file: !9, line: 8, type: !25)
!1534 = !DILocalVariable(name: "args", scope: !1528, file: !9, line: 8, type: !149)
!1535 = !DILocalVariable(name: "ret", scope: !1528, file: !9, line: 8, type: !40)
!1536 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethodV", scope: !9, file: !9, line: 8, type: !378, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1537 = !DILocalVariable(name: "args", arg: 5, scope: !1536, file: !9, line: 8, type: !149)
!1538 = !DILocation(line: 8, scope: !1536)
!1539 = !DILocalVariable(name: "methodID", arg: 4, scope: !1536, file: !9, line: 8, type: !67)
!1540 = !DILocalVariable(name: "clazz", arg: 3, scope: !1536, file: !9, line: 8, type: !47)
!1541 = !DILocalVariable(name: "obj", arg: 2, scope: !1536, file: !9, line: 8, type: !48)
!1542 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1536, file: !9, line: 8, type: !25)
!1543 = !DILocalVariable(name: "sig", scope: !1536, file: !9, line: 8, type: !1111)
!1544 = !DILocalVariable(name: "argc", scope: !1536, file: !9, line: 8, type: !13)
!1545 = !DILocalVariable(name: "argv", scope: !1536, file: !9, line: 8, type: !1116)
!1546 = !DILocalVariable(name: "i", scope: !1547, file: !9, line: 8, type: !13)
!1547 = distinct !DILexicalBlock(scope: !1536, file: !9, line: 8)
!1548 = !DILocation(line: 8, scope: !1547)
!1549 = !DILocation(line: 8, scope: !1550)
!1550 = distinct !DILexicalBlock(scope: !1551, file: !9, line: 8)
!1551 = distinct !DILexicalBlock(scope: !1547, file: !9, line: 8)
!1552 = !DILocation(line: 8, scope: !1553)
!1553 = distinct !DILexicalBlock(scope: !1550, file: !9, line: 8)
!1554 = !DILocation(line: 8, scope: !1551)
!1555 = distinct !{!1555, !1548, !1548, !1127}
!1556 = distinct !DISubprogram(name: "JNI_CallStaticIntMethod", scope: !9, file: !9, line: 8, type: !562, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1557 = !DILocalVariable(name: "methodID", arg: 3, scope: !1556, file: !9, line: 8, type: !67)
!1558 = !DILocation(line: 8, scope: !1556)
!1559 = !DILocalVariable(name: "clazz", arg: 2, scope: !1556, file: !9, line: 8, type: !47)
!1560 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1556, file: !9, line: 8, type: !25)
!1561 = !DILocalVariable(name: "args", scope: !1556, file: !9, line: 8, type: !149)
!1562 = !DILocalVariable(name: "ret", scope: !1556, file: !9, line: 8, type: !40)
!1563 = distinct !DISubprogram(name: "JNI_CallStaticIntMethodV", scope: !9, file: !9, line: 8, type: !566, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1564 = !DILocalVariable(name: "args", arg: 4, scope: !1563, file: !9, line: 8, type: !149)
!1565 = !DILocation(line: 8, scope: !1563)
!1566 = !DILocalVariable(name: "methodID", arg: 3, scope: !1563, file: !9, line: 8, type: !67)
!1567 = !DILocalVariable(name: "clazz", arg: 2, scope: !1563, file: !9, line: 8, type: !47)
!1568 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1563, file: !9, line: 8, type: !25)
!1569 = !DILocalVariable(name: "sig", scope: !1563, file: !9, line: 8, type: !1111)
!1570 = !DILocalVariable(name: "argc", scope: !1563, file: !9, line: 8, type: !13)
!1571 = !DILocalVariable(name: "argv", scope: !1563, file: !9, line: 8, type: !1116)
!1572 = !DILocalVariable(name: "i", scope: !1573, file: !9, line: 8, type: !13)
!1573 = distinct !DILexicalBlock(scope: !1563, file: !9, line: 8)
!1574 = !DILocation(line: 8, scope: !1573)
!1575 = !DILocation(line: 8, scope: !1576)
!1576 = distinct !DILexicalBlock(scope: !1577, file: !9, line: 8)
!1577 = distinct !DILexicalBlock(scope: !1573, file: !9, line: 8)
!1578 = !DILocation(line: 8, scope: !1579)
!1579 = distinct !DILexicalBlock(scope: !1576, file: !9, line: 8)
!1580 = !DILocation(line: 8, scope: !1577)
!1581 = distinct !{!1581, !1574, !1574, !1127}
!1582 = distinct !DISubprogram(name: "JNI_CallLongMethod", scope: !9, file: !9, line: 9, type: !266, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1583 = !DILocalVariable(name: "methodID", arg: 3, scope: !1582, file: !9, line: 9, type: !67)
!1584 = !DILocation(line: 9, scope: !1582)
!1585 = !DILocalVariable(name: "obj", arg: 2, scope: !1582, file: !9, line: 9, type: !48)
!1586 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1582, file: !9, line: 9, type: !25)
!1587 = !DILocalVariable(name: "args", scope: !1582, file: !9, line: 9, type: !149)
!1588 = !DILocalVariable(name: "ret", scope: !1582, file: !9, line: 9, type: !171)
!1589 = distinct !DISubprogram(name: "JNI_CallLongMethodV", scope: !9, file: !9, line: 9, type: !270, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1590 = !DILocalVariable(name: "args", arg: 4, scope: !1589, file: !9, line: 9, type: !149)
!1591 = !DILocation(line: 9, scope: !1589)
!1592 = !DILocalVariable(name: "methodID", arg: 3, scope: !1589, file: !9, line: 9, type: !67)
!1593 = !DILocalVariable(name: "obj", arg: 2, scope: !1589, file: !9, line: 9, type: !48)
!1594 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1589, file: !9, line: 9, type: !25)
!1595 = !DILocalVariable(name: "sig", scope: !1589, file: !9, line: 9, type: !1111)
!1596 = !DILocalVariable(name: "argc", scope: !1589, file: !9, line: 9, type: !13)
!1597 = !DILocalVariable(name: "argv", scope: !1589, file: !9, line: 9, type: !1116)
!1598 = !DILocalVariable(name: "i", scope: !1599, file: !9, line: 9, type: !13)
!1599 = distinct !DILexicalBlock(scope: !1589, file: !9, line: 9)
!1600 = !DILocation(line: 9, scope: !1599)
!1601 = !DILocation(line: 9, scope: !1602)
!1602 = distinct !DILexicalBlock(scope: !1603, file: !9, line: 9)
!1603 = distinct !DILexicalBlock(scope: !1599, file: !9, line: 9)
!1604 = !DILocation(line: 9, scope: !1605)
!1605 = distinct !DILexicalBlock(scope: !1602, file: !9, line: 9)
!1606 = !DILocation(line: 9, scope: !1603)
!1607 = distinct !{!1607, !1600, !1600, !1127}
!1608 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethod", scope: !9, file: !9, line: 9, type: !386, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1609 = !DILocalVariable(name: "methodID", arg: 4, scope: !1608, file: !9, line: 9, type: !67)
!1610 = !DILocation(line: 9, scope: !1608)
!1611 = !DILocalVariable(name: "clazz", arg: 3, scope: !1608, file: !9, line: 9, type: !47)
!1612 = !DILocalVariable(name: "obj", arg: 2, scope: !1608, file: !9, line: 9, type: !48)
!1613 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1608, file: !9, line: 9, type: !25)
!1614 = !DILocalVariable(name: "args", scope: !1608, file: !9, line: 9, type: !149)
!1615 = !DILocalVariable(name: "ret", scope: !1608, file: !9, line: 9, type: !171)
!1616 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethodV", scope: !9, file: !9, line: 9, type: !390, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1617 = !DILocalVariable(name: "args", arg: 5, scope: !1616, file: !9, line: 9, type: !149)
!1618 = !DILocation(line: 9, scope: !1616)
!1619 = !DILocalVariable(name: "methodID", arg: 4, scope: !1616, file: !9, line: 9, type: !67)
!1620 = !DILocalVariable(name: "clazz", arg: 3, scope: !1616, file: !9, line: 9, type: !47)
!1621 = !DILocalVariable(name: "obj", arg: 2, scope: !1616, file: !9, line: 9, type: !48)
!1622 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1616, file: !9, line: 9, type: !25)
!1623 = !DILocalVariable(name: "sig", scope: !1616, file: !9, line: 9, type: !1111)
!1624 = !DILocalVariable(name: "argc", scope: !1616, file: !9, line: 9, type: !13)
!1625 = !DILocalVariable(name: "argv", scope: !1616, file: !9, line: 9, type: !1116)
!1626 = !DILocalVariable(name: "i", scope: !1627, file: !9, line: 9, type: !13)
!1627 = distinct !DILexicalBlock(scope: !1616, file: !9, line: 9)
!1628 = !DILocation(line: 9, scope: !1627)
!1629 = !DILocation(line: 9, scope: !1630)
!1630 = distinct !DILexicalBlock(scope: !1631, file: !9, line: 9)
!1631 = distinct !DILexicalBlock(scope: !1627, file: !9, line: 9)
!1632 = !DILocation(line: 9, scope: !1633)
!1633 = distinct !DILexicalBlock(scope: !1630, file: !9, line: 9)
!1634 = !DILocation(line: 9, scope: !1631)
!1635 = distinct !{!1635, !1628, !1628, !1127}
!1636 = distinct !DISubprogram(name: "JNI_CallStaticLongMethod", scope: !9, file: !9, line: 9, type: !574, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1637 = !DILocalVariable(name: "methodID", arg: 3, scope: !1636, file: !9, line: 9, type: !67)
!1638 = !DILocation(line: 9, scope: !1636)
!1639 = !DILocalVariable(name: "clazz", arg: 2, scope: !1636, file: !9, line: 9, type: !47)
!1640 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1636, file: !9, line: 9, type: !25)
!1641 = !DILocalVariable(name: "args", scope: !1636, file: !9, line: 9, type: !149)
!1642 = !DILocalVariable(name: "ret", scope: !1636, file: !9, line: 9, type: !171)
!1643 = distinct !DISubprogram(name: "JNI_CallStaticLongMethodV", scope: !9, file: !9, line: 9, type: !578, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1644 = !DILocalVariable(name: "args", arg: 4, scope: !1643, file: !9, line: 9, type: !149)
!1645 = !DILocation(line: 9, scope: !1643)
!1646 = !DILocalVariable(name: "methodID", arg: 3, scope: !1643, file: !9, line: 9, type: !67)
!1647 = !DILocalVariable(name: "clazz", arg: 2, scope: !1643, file: !9, line: 9, type: !47)
!1648 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1643, file: !9, line: 9, type: !25)
!1649 = !DILocalVariable(name: "sig", scope: !1643, file: !9, line: 9, type: !1111)
!1650 = !DILocalVariable(name: "argc", scope: !1643, file: !9, line: 9, type: !13)
!1651 = !DILocalVariable(name: "argv", scope: !1643, file: !9, line: 9, type: !1116)
!1652 = !DILocalVariable(name: "i", scope: !1653, file: !9, line: 9, type: !13)
!1653 = distinct !DILexicalBlock(scope: !1643, file: !9, line: 9)
!1654 = !DILocation(line: 9, scope: !1653)
!1655 = !DILocation(line: 9, scope: !1656)
!1656 = distinct !DILexicalBlock(scope: !1657, file: !9, line: 9)
!1657 = distinct !DILexicalBlock(scope: !1653, file: !9, line: 9)
!1658 = !DILocation(line: 9, scope: !1659)
!1659 = distinct !DILexicalBlock(scope: !1656, file: !9, line: 9)
!1660 = !DILocation(line: 9, scope: !1657)
!1661 = distinct !{!1661, !1654, !1654, !1127}
!1662 = distinct !DISubprogram(name: "JNI_CallFloatMethod", scope: !9, file: !9, line: 10, type: !278, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1663 = !DILocalVariable(name: "methodID", arg: 3, scope: !1662, file: !9, line: 10, type: !67)
!1664 = !DILocation(line: 10, scope: !1662)
!1665 = !DILocalVariable(name: "obj", arg: 2, scope: !1662, file: !9, line: 10, type: !48)
!1666 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1662, file: !9, line: 10, type: !25)
!1667 = !DILocalVariable(name: "args", scope: !1662, file: !9, line: 10, type: !149)
!1668 = !DILocalVariable(name: "ret", scope: !1662, file: !9, line: 10, type: !174)
!1669 = distinct !DISubprogram(name: "JNI_CallFloatMethodV", scope: !9, file: !9, line: 10, type: !282, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1670 = !DILocalVariable(name: "args", arg: 4, scope: !1669, file: !9, line: 10, type: !149)
!1671 = !DILocation(line: 10, scope: !1669)
!1672 = !DILocalVariable(name: "methodID", arg: 3, scope: !1669, file: !9, line: 10, type: !67)
!1673 = !DILocalVariable(name: "obj", arg: 2, scope: !1669, file: !9, line: 10, type: !48)
!1674 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1669, file: !9, line: 10, type: !25)
!1675 = !DILocalVariable(name: "sig", scope: !1669, file: !9, line: 10, type: !1111)
!1676 = !DILocalVariable(name: "argc", scope: !1669, file: !9, line: 10, type: !13)
!1677 = !DILocalVariable(name: "argv", scope: !1669, file: !9, line: 10, type: !1116)
!1678 = !DILocalVariable(name: "i", scope: !1679, file: !9, line: 10, type: !13)
!1679 = distinct !DILexicalBlock(scope: !1669, file: !9, line: 10)
!1680 = !DILocation(line: 10, scope: !1679)
!1681 = !DILocation(line: 10, scope: !1682)
!1682 = distinct !DILexicalBlock(scope: !1683, file: !9, line: 10)
!1683 = distinct !DILexicalBlock(scope: !1679, file: !9, line: 10)
!1684 = !DILocation(line: 10, scope: !1685)
!1685 = distinct !DILexicalBlock(scope: !1682, file: !9, line: 10)
!1686 = !DILocation(line: 10, scope: !1683)
!1687 = distinct !{!1687, !1680, !1680, !1127}
!1688 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethod", scope: !9, file: !9, line: 10, type: !398, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1689 = !DILocalVariable(name: "methodID", arg: 4, scope: !1688, file: !9, line: 10, type: !67)
!1690 = !DILocation(line: 10, scope: !1688)
!1691 = !DILocalVariable(name: "clazz", arg: 3, scope: !1688, file: !9, line: 10, type: !47)
!1692 = !DILocalVariable(name: "obj", arg: 2, scope: !1688, file: !9, line: 10, type: !48)
!1693 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1688, file: !9, line: 10, type: !25)
!1694 = !DILocalVariable(name: "args", scope: !1688, file: !9, line: 10, type: !149)
!1695 = !DILocalVariable(name: "ret", scope: !1688, file: !9, line: 10, type: !174)
!1696 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethodV", scope: !9, file: !9, line: 10, type: !402, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1697 = !DILocalVariable(name: "args", arg: 5, scope: !1696, file: !9, line: 10, type: !149)
!1698 = !DILocation(line: 10, scope: !1696)
!1699 = !DILocalVariable(name: "methodID", arg: 4, scope: !1696, file: !9, line: 10, type: !67)
!1700 = !DILocalVariable(name: "clazz", arg: 3, scope: !1696, file: !9, line: 10, type: !47)
!1701 = !DILocalVariable(name: "obj", arg: 2, scope: !1696, file: !9, line: 10, type: !48)
!1702 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1696, file: !9, line: 10, type: !25)
!1703 = !DILocalVariable(name: "sig", scope: !1696, file: !9, line: 10, type: !1111)
!1704 = !DILocalVariable(name: "argc", scope: !1696, file: !9, line: 10, type: !13)
!1705 = !DILocalVariable(name: "argv", scope: !1696, file: !9, line: 10, type: !1116)
!1706 = !DILocalVariable(name: "i", scope: !1707, file: !9, line: 10, type: !13)
!1707 = distinct !DILexicalBlock(scope: !1696, file: !9, line: 10)
!1708 = !DILocation(line: 10, scope: !1707)
!1709 = !DILocation(line: 10, scope: !1710)
!1710 = distinct !DILexicalBlock(scope: !1711, file: !9, line: 10)
!1711 = distinct !DILexicalBlock(scope: !1707, file: !9, line: 10)
!1712 = !DILocation(line: 10, scope: !1713)
!1713 = distinct !DILexicalBlock(scope: !1710, file: !9, line: 10)
!1714 = !DILocation(line: 10, scope: !1711)
!1715 = distinct !{!1715, !1708, !1708, !1127}
!1716 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethod", scope: !9, file: !9, line: 10, type: !586, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1717 = !DILocalVariable(name: "methodID", arg: 3, scope: !1716, file: !9, line: 10, type: !67)
!1718 = !DILocation(line: 10, scope: !1716)
!1719 = !DILocalVariable(name: "clazz", arg: 2, scope: !1716, file: !9, line: 10, type: !47)
!1720 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1716, file: !9, line: 10, type: !25)
!1721 = !DILocalVariable(name: "args", scope: !1716, file: !9, line: 10, type: !149)
!1722 = !DILocalVariable(name: "ret", scope: !1716, file: !9, line: 10, type: !174)
!1723 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethodV", scope: !9, file: !9, line: 10, type: !590, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1724 = !DILocalVariable(name: "args", arg: 4, scope: !1723, file: !9, line: 10, type: !149)
!1725 = !DILocation(line: 10, scope: !1723)
!1726 = !DILocalVariable(name: "methodID", arg: 3, scope: !1723, file: !9, line: 10, type: !67)
!1727 = !DILocalVariable(name: "clazz", arg: 2, scope: !1723, file: !9, line: 10, type: !47)
!1728 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1723, file: !9, line: 10, type: !25)
!1729 = !DILocalVariable(name: "sig", scope: !1723, file: !9, line: 10, type: !1111)
!1730 = !DILocalVariable(name: "argc", scope: !1723, file: !9, line: 10, type: !13)
!1731 = !DILocalVariable(name: "argv", scope: !1723, file: !9, line: 10, type: !1116)
!1732 = !DILocalVariable(name: "i", scope: !1733, file: !9, line: 10, type: !13)
!1733 = distinct !DILexicalBlock(scope: !1723, file: !9, line: 10)
!1734 = !DILocation(line: 10, scope: !1733)
!1735 = !DILocation(line: 10, scope: !1736)
!1736 = distinct !DILexicalBlock(scope: !1737, file: !9, line: 10)
!1737 = distinct !DILexicalBlock(scope: !1733, file: !9, line: 10)
!1738 = !DILocation(line: 10, scope: !1739)
!1739 = distinct !DILexicalBlock(scope: !1736, file: !9, line: 10)
!1740 = !DILocation(line: 10, scope: !1737)
!1741 = distinct !{!1741, !1734, !1734, !1127}
!1742 = distinct !DISubprogram(name: "JNI_CallDoubleMethod", scope: !9, file: !9, line: 11, type: !290, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1743 = !DILocalVariable(name: "methodID", arg: 3, scope: !1742, file: !9, line: 11, type: !67)
!1744 = !DILocation(line: 11, scope: !1742)
!1745 = !DILocalVariable(name: "obj", arg: 2, scope: !1742, file: !9, line: 11, type: !48)
!1746 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1742, file: !9, line: 11, type: !25)
!1747 = !DILocalVariable(name: "args", scope: !1742, file: !9, line: 11, type: !149)
!1748 = !DILocalVariable(name: "ret", scope: !1742, file: !9, line: 11, type: !177)
!1749 = distinct !DISubprogram(name: "JNI_CallDoubleMethodV", scope: !9, file: !9, line: 11, type: !294, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1750 = !DILocalVariable(name: "args", arg: 4, scope: !1749, file: !9, line: 11, type: !149)
!1751 = !DILocation(line: 11, scope: !1749)
!1752 = !DILocalVariable(name: "methodID", arg: 3, scope: !1749, file: !9, line: 11, type: !67)
!1753 = !DILocalVariable(name: "obj", arg: 2, scope: !1749, file: !9, line: 11, type: !48)
!1754 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1749, file: !9, line: 11, type: !25)
!1755 = !DILocalVariable(name: "sig", scope: !1749, file: !9, line: 11, type: !1111)
!1756 = !DILocalVariable(name: "argc", scope: !1749, file: !9, line: 11, type: !13)
!1757 = !DILocalVariable(name: "argv", scope: !1749, file: !9, line: 11, type: !1116)
!1758 = !DILocalVariable(name: "i", scope: !1759, file: !9, line: 11, type: !13)
!1759 = distinct !DILexicalBlock(scope: !1749, file: !9, line: 11)
!1760 = !DILocation(line: 11, scope: !1759)
!1761 = !DILocation(line: 11, scope: !1762)
!1762 = distinct !DILexicalBlock(scope: !1763, file: !9, line: 11)
!1763 = distinct !DILexicalBlock(scope: !1759, file: !9, line: 11)
!1764 = !DILocation(line: 11, scope: !1765)
!1765 = distinct !DILexicalBlock(scope: !1762, file: !9, line: 11)
!1766 = !DILocation(line: 11, scope: !1763)
!1767 = distinct !{!1767, !1760, !1760, !1127}
!1768 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethod", scope: !9, file: !9, line: 11, type: !410, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1769 = !DILocalVariable(name: "methodID", arg: 4, scope: !1768, file: !9, line: 11, type: !67)
!1770 = !DILocation(line: 11, scope: !1768)
!1771 = !DILocalVariable(name: "clazz", arg: 3, scope: !1768, file: !9, line: 11, type: !47)
!1772 = !DILocalVariable(name: "obj", arg: 2, scope: !1768, file: !9, line: 11, type: !48)
!1773 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1768, file: !9, line: 11, type: !25)
!1774 = !DILocalVariable(name: "args", scope: !1768, file: !9, line: 11, type: !149)
!1775 = !DILocalVariable(name: "ret", scope: !1768, file: !9, line: 11, type: !177)
!1776 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethodV", scope: !9, file: !9, line: 11, type: !414, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1777 = !DILocalVariable(name: "args", arg: 5, scope: !1776, file: !9, line: 11, type: !149)
!1778 = !DILocation(line: 11, scope: !1776)
!1779 = !DILocalVariable(name: "methodID", arg: 4, scope: !1776, file: !9, line: 11, type: !67)
!1780 = !DILocalVariable(name: "clazz", arg: 3, scope: !1776, file: !9, line: 11, type: !47)
!1781 = !DILocalVariable(name: "obj", arg: 2, scope: !1776, file: !9, line: 11, type: !48)
!1782 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1776, file: !9, line: 11, type: !25)
!1783 = !DILocalVariable(name: "sig", scope: !1776, file: !9, line: 11, type: !1111)
!1784 = !DILocalVariable(name: "argc", scope: !1776, file: !9, line: 11, type: !13)
!1785 = !DILocalVariable(name: "argv", scope: !1776, file: !9, line: 11, type: !1116)
!1786 = !DILocalVariable(name: "i", scope: !1787, file: !9, line: 11, type: !13)
!1787 = distinct !DILexicalBlock(scope: !1776, file: !9, line: 11)
!1788 = !DILocation(line: 11, scope: !1787)
!1789 = !DILocation(line: 11, scope: !1790)
!1790 = distinct !DILexicalBlock(scope: !1791, file: !9, line: 11)
!1791 = distinct !DILexicalBlock(scope: !1787, file: !9, line: 11)
!1792 = !DILocation(line: 11, scope: !1793)
!1793 = distinct !DILexicalBlock(scope: !1790, file: !9, line: 11)
!1794 = !DILocation(line: 11, scope: !1791)
!1795 = distinct !{!1795, !1788, !1788, !1127}
!1796 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethod", scope: !9, file: !9, line: 11, type: !598, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1797 = !DILocalVariable(name: "methodID", arg: 3, scope: !1796, file: !9, line: 11, type: !67)
!1798 = !DILocation(line: 11, scope: !1796)
!1799 = !DILocalVariable(name: "clazz", arg: 2, scope: !1796, file: !9, line: 11, type: !47)
!1800 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1796, file: !9, line: 11, type: !25)
!1801 = !DILocalVariable(name: "args", scope: !1796, file: !9, line: 11, type: !149)
!1802 = !DILocalVariable(name: "ret", scope: !1796, file: !9, line: 11, type: !177)
!1803 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethodV", scope: !9, file: !9, line: 11, type: !602, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1804 = !DILocalVariable(name: "args", arg: 4, scope: !1803, file: !9, line: 11, type: !149)
!1805 = !DILocation(line: 11, scope: !1803)
!1806 = !DILocalVariable(name: "methodID", arg: 3, scope: !1803, file: !9, line: 11, type: !67)
!1807 = !DILocalVariable(name: "clazz", arg: 2, scope: !1803, file: !9, line: 11, type: !47)
!1808 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1803, file: !9, line: 11, type: !25)
!1809 = !DILocalVariable(name: "sig", scope: !1803, file: !9, line: 11, type: !1111)
!1810 = !DILocalVariable(name: "argc", scope: !1803, file: !9, line: 11, type: !13)
!1811 = !DILocalVariable(name: "argv", scope: !1803, file: !9, line: 11, type: !1116)
!1812 = !DILocalVariable(name: "i", scope: !1813, file: !9, line: 11, type: !13)
!1813 = distinct !DILexicalBlock(scope: !1803, file: !9, line: 11)
!1814 = !DILocation(line: 11, scope: !1813)
!1815 = !DILocation(line: 11, scope: !1816)
!1816 = distinct !DILexicalBlock(scope: !1817, file: !9, line: 11)
!1817 = distinct !DILexicalBlock(scope: !1813, file: !9, line: 11)
!1818 = !DILocation(line: 11, scope: !1819)
!1819 = distinct !DILexicalBlock(scope: !1816, file: !9, line: 11)
!1820 = !DILocation(line: 11, scope: !1817)
!1821 = distinct !{!1821, !1814, !1814, !1127}
!1822 = distinct !DISubprogram(name: "JNI_NewObject", scope: !9, file: !9, line: 13, type: !143, scopeLine: 14, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1823 = !DILocalVariable(name: "methodID", arg: 3, scope: !1822, file: !9, line: 13, type: !67)
!1824 = !DILocation(line: 13, scope: !1822)
!1825 = !DILocalVariable(name: "clazz", arg: 2, scope: !1822, file: !9, line: 13, type: !47)
!1826 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1822, file: !9, line: 13, type: !25)
!1827 = !DILocalVariable(name: "args", scope: !1822, file: !9, line: 15, type: !149)
!1828 = !DILocation(line: 15, scope: !1822)
!1829 = !DILocation(line: 16, scope: !1822)
!1830 = !DILocalVariable(name: "o", scope: !1822, file: !9, line: 17, type: !48)
!1831 = !DILocation(line: 17, scope: !1822)
!1832 = !DILocation(line: 18, scope: !1822)
!1833 = !DILocation(line: 19, scope: !1822)
!1834 = distinct !DISubprogram(name: "JNI_NewObjectV", scope: !9, file: !9, line: 22, type: !147, scopeLine: 23, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1835 = !DILocalVariable(name: "args", arg: 4, scope: !1834, file: !9, line: 22, type: !149)
!1836 = !DILocation(line: 22, scope: !1834)
!1837 = !DILocalVariable(name: "methodID", arg: 3, scope: !1834, file: !9, line: 22, type: !67)
!1838 = !DILocalVariable(name: "clazz", arg: 2, scope: !1834, file: !9, line: 22, type: !47)
!1839 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1834, file: !9, line: 22, type: !25)
!1840 = !DILocalVariable(name: "sig", scope: !1834, file: !9, line: 24, type: !1111)
!1841 = !DILocation(line: 24, scope: !1834)
!1842 = !DILocalVariable(name: "argc", scope: !1834, file: !9, line: 24, type: !13)
!1843 = !DILocalVariable(name: "argv", scope: !1834, file: !9, line: 24, type: !1116)
!1844 = !DILocalVariable(name: "i", scope: !1845, file: !9, line: 24, type: !13)
!1845 = distinct !DILexicalBlock(scope: !1834, file: !9, line: 24)
!1846 = !DILocation(line: 24, scope: !1845)
!1847 = !DILocation(line: 24, scope: !1848)
!1848 = distinct !DILexicalBlock(scope: !1849, file: !9, line: 24)
!1849 = distinct !DILexicalBlock(scope: !1845, file: !9, line: 24)
!1850 = !DILocation(line: 24, scope: !1851)
!1851 = distinct !DILexicalBlock(scope: !1848, file: !9, line: 24)
!1852 = !DILocation(line: 24, scope: !1849)
!1853 = distinct !{!1853, !1846, !1846, !1127}
!1854 = !DILocation(line: 25, scope: !1834)
!1855 = distinct !DISubprogram(name: "JNI_CallVoidMethod", scope: !9, file: !9, line: 28, type: !302, scopeLine: 29, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1856 = !DILocalVariable(name: "methodID", arg: 3, scope: !1855, file: !9, line: 28, type: !67)
!1857 = !DILocation(line: 28, scope: !1855)
!1858 = !DILocalVariable(name: "obj", arg: 2, scope: !1855, file: !9, line: 28, type: !48)
!1859 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1855, file: !9, line: 28, type: !25)
!1860 = !DILocalVariable(name: "args", scope: !1855, file: !9, line: 30, type: !149)
!1861 = !DILocation(line: 30, scope: !1855)
!1862 = !DILocation(line: 31, scope: !1855)
!1863 = !DILocation(line: 32, scope: !1855)
!1864 = !DILocation(line: 33, scope: !1855)
!1865 = !DILocation(line: 34, scope: !1855)
!1866 = distinct !DISubprogram(name: "JNI_CallVoidMethodV", scope: !9, file: !9, line: 36, type: !306, scopeLine: 37, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1867 = !DILocalVariable(name: "args", arg: 4, scope: !1866, file: !9, line: 36, type: !149)
!1868 = !DILocation(line: 36, scope: !1866)
!1869 = !DILocalVariable(name: "methodID", arg: 3, scope: !1866, file: !9, line: 36, type: !67)
!1870 = !DILocalVariable(name: "obj", arg: 2, scope: !1866, file: !9, line: 36, type: !48)
!1871 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1866, file: !9, line: 36, type: !25)
!1872 = !DILocalVariable(name: "sig", scope: !1866, file: !9, line: 38, type: !1111)
!1873 = !DILocation(line: 38, scope: !1866)
!1874 = !DILocalVariable(name: "argc", scope: !1866, file: !9, line: 38, type: !13)
!1875 = !DILocalVariable(name: "argv", scope: !1866, file: !9, line: 38, type: !1116)
!1876 = !DILocalVariable(name: "i", scope: !1877, file: !9, line: 38, type: !13)
!1877 = distinct !DILexicalBlock(scope: !1866, file: !9, line: 38)
!1878 = !DILocation(line: 38, scope: !1877)
!1879 = !DILocation(line: 38, scope: !1880)
!1880 = distinct !DILexicalBlock(scope: !1881, file: !9, line: 38)
!1881 = distinct !DILexicalBlock(scope: !1877, file: !9, line: 38)
!1882 = !DILocation(line: 38, scope: !1883)
!1883 = distinct !DILexicalBlock(scope: !1880, file: !9, line: 38)
!1884 = !DILocation(line: 38, scope: !1881)
!1885 = distinct !{!1885, !1878, !1878, !1127}
!1886 = !DILocation(line: 39, scope: !1866)
!1887 = !DILocation(line: 40, scope: !1866)
!1888 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethod", scope: !9, file: !9, line: 42, type: !422, scopeLine: 43, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1889 = !DILocalVariable(name: "methodID", arg: 4, scope: !1888, file: !9, line: 42, type: !67)
!1890 = !DILocation(line: 42, scope: !1888)
!1891 = !DILocalVariable(name: "clazz", arg: 3, scope: !1888, file: !9, line: 42, type: !47)
!1892 = !DILocalVariable(name: "obj", arg: 2, scope: !1888, file: !9, line: 42, type: !48)
!1893 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1888, file: !9, line: 42, type: !25)
!1894 = !DILocalVariable(name: "args", scope: !1888, file: !9, line: 44, type: !149)
!1895 = !DILocation(line: 44, scope: !1888)
!1896 = !DILocation(line: 45, scope: !1888)
!1897 = !DILocation(line: 46, scope: !1888)
!1898 = !DILocation(line: 47, scope: !1888)
!1899 = !DILocation(line: 48, scope: !1888)
!1900 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethodV", scope: !9, file: !9, line: 50, type: !426, scopeLine: 51, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1901 = !DILocalVariable(name: "args", arg: 5, scope: !1900, file: !9, line: 50, type: !149)
!1902 = !DILocation(line: 50, scope: !1900)
!1903 = !DILocalVariable(name: "methodID", arg: 4, scope: !1900, file: !9, line: 50, type: !67)
!1904 = !DILocalVariable(name: "clazz", arg: 3, scope: !1900, file: !9, line: 50, type: !47)
!1905 = !DILocalVariable(name: "obj", arg: 2, scope: !1900, file: !9, line: 50, type: !48)
!1906 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1900, file: !9, line: 50, type: !25)
!1907 = !DILocalVariable(name: "sig", scope: !1900, file: !9, line: 52, type: !1111)
!1908 = !DILocation(line: 52, scope: !1900)
!1909 = !DILocalVariable(name: "argc", scope: !1900, file: !9, line: 52, type: !13)
!1910 = !DILocalVariable(name: "argv", scope: !1900, file: !9, line: 52, type: !1116)
!1911 = !DILocalVariable(name: "i", scope: !1912, file: !9, line: 52, type: !13)
!1912 = distinct !DILexicalBlock(scope: !1900, file: !9, line: 52)
!1913 = !DILocation(line: 52, scope: !1912)
!1914 = !DILocation(line: 52, scope: !1915)
!1915 = distinct !DILexicalBlock(scope: !1916, file: !9, line: 52)
!1916 = distinct !DILexicalBlock(scope: !1912, file: !9, line: 52)
!1917 = !DILocation(line: 52, scope: !1918)
!1918 = distinct !DILexicalBlock(scope: !1915, file: !9, line: 52)
!1919 = !DILocation(line: 52, scope: !1916)
!1920 = distinct !{!1920, !1913, !1913, !1127}
!1921 = !DILocation(line: 53, scope: !1900)
!1922 = !DILocation(line: 54, scope: !1900)
!1923 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethod", scope: !9, file: !9, line: 56, type: !610, scopeLine: 57, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1924 = !DILocalVariable(name: "methodID", arg: 3, scope: !1923, file: !9, line: 56, type: !67)
!1925 = !DILocation(line: 56, scope: !1923)
!1926 = !DILocalVariable(name: "clazz", arg: 2, scope: !1923, file: !9, line: 56, type: !47)
!1927 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1923, file: !9, line: 56, type: !25)
!1928 = !DILocalVariable(name: "args", scope: !1923, file: !9, line: 58, type: !149)
!1929 = !DILocation(line: 58, scope: !1923)
!1930 = !DILocation(line: 59, scope: !1923)
!1931 = !DILocation(line: 60, scope: !1923)
!1932 = !DILocation(line: 61, scope: !1923)
!1933 = !DILocation(line: 62, scope: !1923)
!1934 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethodV", scope: !9, file: !9, line: 64, type: !614, scopeLine: 65, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1935 = !DILocalVariable(name: "args", arg: 4, scope: !1934, file: !9, line: 64, type: !149)
!1936 = !DILocation(line: 64, scope: !1934)
!1937 = !DILocalVariable(name: "methodID", arg: 3, scope: !1934, file: !9, line: 64, type: !67)
!1938 = !DILocalVariable(name: "clazz", arg: 2, scope: !1934, file: !9, line: 64, type: !47)
!1939 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1934, file: !9, line: 64, type: !25)
!1940 = !DILocalVariable(name: "sig", scope: !1934, file: !9, line: 66, type: !1111)
!1941 = !DILocation(line: 66, scope: !1934)
!1942 = !DILocalVariable(name: "argc", scope: !1934, file: !9, line: 66, type: !13)
!1943 = !DILocalVariable(name: "argv", scope: !1934, file: !9, line: 66, type: !1116)
!1944 = !DILocalVariable(name: "i", scope: !1945, file: !9, line: 66, type: !13)
!1945 = distinct !DILexicalBlock(scope: !1934, file: !9, line: 66)
!1946 = !DILocation(line: 66, scope: !1945)
!1947 = !DILocation(line: 66, scope: !1948)
!1948 = distinct !DILexicalBlock(scope: !1949, file: !9, line: 66)
!1949 = distinct !DILexicalBlock(scope: !1945, file: !9, line: 66)
!1950 = !DILocation(line: 66, scope: !1951)
!1951 = distinct !DILexicalBlock(scope: !1948, file: !9, line: 66)
!1952 = !DILocation(line: 66, scope: !1949)
!1953 = distinct !{!1953, !1946, !1946, !1127}
!1954 = !DILocation(line: 67, scope: !1934)
!1955 = !DILocation(line: 68, scope: !1934)
!1956 = distinct !DISubprogram(name: "_vsprintf_l", scope: !1040, file: !1040, line: 1449, type: !1957, scopeLine: 1458, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1957 = !DISubroutineType(types: !1958)
!1958 = !{!13, !1043, !1044, !1959, !149}
!1959 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !1960)
!1960 = !DIDerivedType(tag: DW_TAG_typedef, name: "_locale_t", file: !1961, line: 623, baseType: !1962)
!1961 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt.h", directory: "", checksumkind: CSK_MD5, checksum: "4ce81db8e96f94c79f8dce86dd46b97f")
!1962 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1963, size: 64)
!1963 = !DIDerivedType(tag: DW_TAG_typedef, name: "__crt_locale_pointers", file: !1961, line: 621, baseType: !1964)
!1964 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_pointers", file: !1961, line: 617, size: 128, elements: !1965)
!1965 = !{!1966, !1969}
!1966 = !DIDerivedType(tag: DW_TAG_member, name: "locinfo", scope: !1964, file: !1961, line: 619, baseType: !1967, size: 64)
!1967 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1968, size: 64)
!1968 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_data", file: !1961, line: 619, flags: DIFlagFwdDecl)
!1969 = !DIDerivedType(tag: DW_TAG_member, name: "mbcinfo", scope: !1964, file: !1961, line: 620, baseType: !1970, size: 64, offset: 64)
!1970 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !1971, size: 64)
!1971 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_multibyte_data", file: !1961, line: 620, flags: DIFlagFwdDecl)
!1972 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !1956, file: !1040, line: 1453, type: !149)
!1973 = !DILocation(line: 1453, scope: !1956)
!1974 = !DILocalVariable(name: "_Locale", arg: 3, scope: !1956, file: !1040, line: 1452, type: !1959)
!1975 = !DILocation(line: 1452, scope: !1956)
!1976 = !DILocalVariable(name: "_Format", arg: 2, scope: !1956, file: !1040, line: 1451, type: !1044)
!1977 = !DILocation(line: 1451, scope: !1956)
!1978 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1956, file: !1040, line: 1450, type: !1043)
!1979 = !DILocation(line: 1450, scope: !1956)
!1980 = !DILocation(line: 1459, scope: !1956)
!1981 = distinct !DISubprogram(name: "_vsnprintf_l", scope: !1040, file: !1040, line: 1381, type: !1982, scopeLine: 1391, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1982 = !DISubroutineType(types: !1983)
!1983 = !{!13, !1043, !1070, !1044, !1959, !149}
!1984 = !DILocalVariable(name: "_ArgList", arg: 5, scope: !1981, file: !1040, line: 1386, type: !149)
!1985 = !DILocation(line: 1386, scope: !1981)
!1986 = !DILocalVariable(name: "_Locale", arg: 4, scope: !1981, file: !1040, line: 1385, type: !1959)
!1987 = !DILocation(line: 1385, scope: !1981)
!1988 = !DILocalVariable(name: "_Format", arg: 3, scope: !1981, file: !1040, line: 1384, type: !1044)
!1989 = !DILocation(line: 1384, scope: !1981)
!1990 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !1981, file: !1040, line: 1383, type: !1070)
!1991 = !DILocation(line: 1383, scope: !1981)
!1992 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !1981, file: !1040, line: 1382, type: !1043)
!1993 = !DILocation(line: 1382, scope: !1981)
!1994 = !DILocalVariable(name: "_Result", scope: !1981, file: !1040, line: 1392, type: !1995)
!1995 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !13)
!1996 = !DILocation(line: 1392, scope: !1981)
!1997 = !DILocation(line: 1396, scope: !1981)
!1998 = !DILocation(line: 92, scope: !2)
