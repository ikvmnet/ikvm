; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:64:64-i32:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-pc-windows-msvc19.34.31933"

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
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1098, metadata !DIExpression()), !dbg !1099
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1100, metadata !DIExpression()), !dbg !1099
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1101, metadata !DIExpression()), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1102, metadata !DIExpression()), !dbg !1099
  call void @llvm.va_start(ptr %7), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1103, metadata !DIExpression()), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1107, metadata !DIExpression()), !dbg !1099
  %13 = load ptr, ptr %6, align 8, !dbg !1099
  %14 = load ptr, ptr %13, align 8, !dbg !1099
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1099
  %16 = load ptr, ptr %15, align 8, !dbg !1099
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1099
  %18 = load ptr, ptr %4, align 8, !dbg !1099
  %19 = load ptr, ptr %6, align 8, !dbg !1099
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1099
  store i32 %20, ptr %9, align 4, !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1108, metadata !DIExpression()), !dbg !1099
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1110, metadata !DIExpression()), !dbg !1112
  store i32 0, ptr %11, align 4, !dbg !1112
  br label %21, !dbg !1112

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1112
  %23 = load i32, ptr %9, align 4, !dbg !1112
  %24 = icmp slt i32 %22, %23, !dbg !1112
  br i1 %24, label %25, label %105, !dbg !1112

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1113
  %27 = sext i32 %26 to i64, !dbg !1113
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1113
  %29 = load i8, ptr %28, align 1, !dbg !1113
  %30 = sext i8 %29 to i32, !dbg !1113
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
  ], !dbg !1113

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1116
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1116
  store ptr %33, ptr %7, align 8, !dbg !1116
  %34 = load i32, ptr %32, align 8, !dbg !1116
  %35 = trunc i32 %34 to i8, !dbg !1116
  %36 = load i32, ptr %11, align 4, !dbg !1116
  %37 = sext i32 %36 to i64, !dbg !1116
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1116
  store i8 %35, ptr %38, align 8, !dbg !1116
  br label %101, !dbg !1116

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1116
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1116
  store ptr %41, ptr %7, align 8, !dbg !1116
  %42 = load i32, ptr %40, align 8, !dbg !1116
  %43 = trunc i32 %42 to i8, !dbg !1116
  %44 = load i32, ptr %11, align 4, !dbg !1116
  %45 = sext i32 %44 to i64, !dbg !1116
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1116
  store i8 %43, ptr %46, align 8, !dbg !1116
  br label %101, !dbg !1116

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1116
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1116
  store ptr %49, ptr %7, align 8, !dbg !1116
  %50 = load i32, ptr %48, align 8, !dbg !1116
  %51 = trunc i32 %50 to i16, !dbg !1116
  %52 = load i32, ptr %11, align 4, !dbg !1116
  %53 = sext i32 %52 to i64, !dbg !1116
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1116
  store i16 %51, ptr %54, align 8, !dbg !1116
  br label %101, !dbg !1116

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1116
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1116
  store ptr %57, ptr %7, align 8, !dbg !1116
  %58 = load i32, ptr %56, align 8, !dbg !1116
  %59 = trunc i32 %58 to i16, !dbg !1116
  %60 = load i32, ptr %11, align 4, !dbg !1116
  %61 = sext i32 %60 to i64, !dbg !1116
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1116
  store i16 %59, ptr %62, align 8, !dbg !1116
  br label %101, !dbg !1116

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1116
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1116
  store ptr %65, ptr %7, align 8, !dbg !1116
  %66 = load i32, ptr %64, align 8, !dbg !1116
  %67 = load i32, ptr %11, align 4, !dbg !1116
  %68 = sext i32 %67 to i64, !dbg !1116
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1116
  store i32 %66, ptr %69, align 8, !dbg !1116
  br label %101, !dbg !1116

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1116
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1116
  store ptr %72, ptr %7, align 8, !dbg !1116
  %73 = load i32, ptr %71, align 8, !dbg !1116
  %74 = sext i32 %73 to i64, !dbg !1116
  %75 = load i32, ptr %11, align 4, !dbg !1116
  %76 = sext i32 %75 to i64, !dbg !1116
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1116
  store i64 %74, ptr %77, align 8, !dbg !1116
  br label %101, !dbg !1116

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1116
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1116
  store ptr %80, ptr %7, align 8, !dbg !1116
  %81 = load double, ptr %79, align 8, !dbg !1116
  %82 = fptrunc double %81 to float, !dbg !1116
  %83 = load i32, ptr %11, align 4, !dbg !1116
  %84 = sext i32 %83 to i64, !dbg !1116
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1116
  store float %82, ptr %85, align 8, !dbg !1116
  br label %101, !dbg !1116

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1116
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1116
  store ptr %88, ptr %7, align 8, !dbg !1116
  %89 = load double, ptr %87, align 8, !dbg !1116
  %90 = load i32, ptr %11, align 4, !dbg !1116
  %91 = sext i32 %90 to i64, !dbg !1116
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1116
  store double %89, ptr %92, align 8, !dbg !1116
  br label %101, !dbg !1116

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1116
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1116
  store ptr %95, ptr %7, align 8, !dbg !1116
  %96 = load ptr, ptr %94, align 8, !dbg !1116
  %97 = load i32, ptr %11, align 4, !dbg !1116
  %98 = sext i32 %97 to i64, !dbg !1116
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1116
  store ptr %96, ptr %99, align 8, !dbg !1116
  br label %101, !dbg !1116

100:                                              ; preds = %25
  br label %101, !dbg !1116

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1113

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1118
  %104 = add nsw i32 %103, 1, !dbg !1118
  store i32 %104, ptr %11, align 4, !dbg !1118
  br label %21, !dbg !1118, !llvm.loop !1119

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1121, metadata !DIExpression()), !dbg !1099
  %106 = load ptr, ptr %6, align 8, !dbg !1099
  %107 = load ptr, ptr %106, align 8, !dbg !1099
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 36, !dbg !1099
  %109 = load ptr, ptr %108, align 8, !dbg !1099
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1099
  %111 = load ptr, ptr %4, align 8, !dbg !1099
  %112 = load ptr, ptr %5, align 8, !dbg !1099
  %113 = load ptr, ptr %6, align 8, !dbg !1099
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1099
  store ptr %114, ptr %12, align 8, !dbg !1099
  call void @llvm.va_end(ptr %7), !dbg !1099
  %115 = load ptr, ptr %12, align 8, !dbg !1099
  ret ptr %115, !dbg !1099
}

; Function Attrs: nocallback nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1122 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1123, metadata !DIExpression()), !dbg !1124
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1125, metadata !DIExpression()), !dbg !1124
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1126, metadata !DIExpression()), !dbg !1124
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1127, metadata !DIExpression()), !dbg !1124
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1128, metadata !DIExpression()), !dbg !1124
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1129, metadata !DIExpression()), !dbg !1124
  %13 = load ptr, ptr %8, align 8, !dbg !1124
  %14 = load ptr, ptr %13, align 8, !dbg !1124
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1124
  %16 = load ptr, ptr %15, align 8, !dbg !1124
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1124
  %18 = load ptr, ptr %6, align 8, !dbg !1124
  %19 = load ptr, ptr %8, align 8, !dbg !1124
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1124
  store i32 %20, ptr %10, align 4, !dbg !1124
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1130, metadata !DIExpression()), !dbg !1124
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1131, metadata !DIExpression()), !dbg !1133
  store i32 0, ptr %12, align 4, !dbg !1133
  br label %21, !dbg !1133

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1133
  %23 = load i32, ptr %10, align 4, !dbg !1133
  %24 = icmp slt i32 %22, %23, !dbg !1133
  br i1 %24, label %25, label %105, !dbg !1133

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1134
  %27 = sext i32 %26 to i64, !dbg !1134
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1134
  %29 = load i8, ptr %28, align 1, !dbg !1134
  %30 = sext i8 %29 to i32, !dbg !1134
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
  ], !dbg !1134

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1137
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1137
  store ptr %33, ptr %5, align 8, !dbg !1137
  %34 = load i32, ptr %32, align 8, !dbg !1137
  %35 = trunc i32 %34 to i8, !dbg !1137
  %36 = load i32, ptr %12, align 4, !dbg !1137
  %37 = sext i32 %36 to i64, !dbg !1137
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1137
  store i8 %35, ptr %38, align 8, !dbg !1137
  br label %101, !dbg !1137

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1137
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1137
  store ptr %41, ptr %5, align 8, !dbg !1137
  %42 = load i32, ptr %40, align 8, !dbg !1137
  %43 = trunc i32 %42 to i8, !dbg !1137
  %44 = load i32, ptr %12, align 4, !dbg !1137
  %45 = sext i32 %44 to i64, !dbg !1137
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1137
  store i8 %43, ptr %46, align 8, !dbg !1137
  br label %101, !dbg !1137

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1137
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1137
  store ptr %49, ptr %5, align 8, !dbg !1137
  %50 = load i32, ptr %48, align 8, !dbg !1137
  %51 = trunc i32 %50 to i16, !dbg !1137
  %52 = load i32, ptr %12, align 4, !dbg !1137
  %53 = sext i32 %52 to i64, !dbg !1137
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1137
  store i16 %51, ptr %54, align 8, !dbg !1137
  br label %101, !dbg !1137

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1137
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1137
  store ptr %57, ptr %5, align 8, !dbg !1137
  %58 = load i32, ptr %56, align 8, !dbg !1137
  %59 = trunc i32 %58 to i16, !dbg !1137
  %60 = load i32, ptr %12, align 4, !dbg !1137
  %61 = sext i32 %60 to i64, !dbg !1137
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1137
  store i16 %59, ptr %62, align 8, !dbg !1137
  br label %101, !dbg !1137

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1137
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1137
  store ptr %65, ptr %5, align 8, !dbg !1137
  %66 = load i32, ptr %64, align 8, !dbg !1137
  %67 = load i32, ptr %12, align 4, !dbg !1137
  %68 = sext i32 %67 to i64, !dbg !1137
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1137
  store i32 %66, ptr %69, align 8, !dbg !1137
  br label %101, !dbg !1137

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1137
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1137
  store ptr %72, ptr %5, align 8, !dbg !1137
  %73 = load i32, ptr %71, align 8, !dbg !1137
  %74 = sext i32 %73 to i64, !dbg !1137
  %75 = load i32, ptr %12, align 4, !dbg !1137
  %76 = sext i32 %75 to i64, !dbg !1137
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1137
  store i64 %74, ptr %77, align 8, !dbg !1137
  br label %101, !dbg !1137

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1137
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1137
  store ptr %80, ptr %5, align 8, !dbg !1137
  %81 = load double, ptr %79, align 8, !dbg !1137
  %82 = fptrunc double %81 to float, !dbg !1137
  %83 = load i32, ptr %12, align 4, !dbg !1137
  %84 = sext i32 %83 to i64, !dbg !1137
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1137
  store float %82, ptr %85, align 8, !dbg !1137
  br label %101, !dbg !1137

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1137
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1137
  store ptr %88, ptr %5, align 8, !dbg !1137
  %89 = load double, ptr %87, align 8, !dbg !1137
  %90 = load i32, ptr %12, align 4, !dbg !1137
  %91 = sext i32 %90 to i64, !dbg !1137
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1137
  store double %89, ptr %92, align 8, !dbg !1137
  br label %101, !dbg !1137

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1137
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1137
  store ptr %95, ptr %5, align 8, !dbg !1137
  %96 = load ptr, ptr %94, align 8, !dbg !1137
  %97 = load i32, ptr %12, align 4, !dbg !1137
  %98 = sext i32 %97 to i64, !dbg !1137
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1137
  store ptr %96, ptr %99, align 8, !dbg !1137
  br label %101, !dbg !1137

100:                                              ; preds = %25
  br label %101, !dbg !1137

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1134

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1139
  %104 = add nsw i32 %103, 1, !dbg !1139
  store i32 %104, ptr %12, align 4, !dbg !1139
  br label %21, !dbg !1139, !llvm.loop !1140

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1124
  %107 = load ptr, ptr %106, align 8, !dbg !1124
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 36, !dbg !1124
  %109 = load ptr, ptr %108, align 8, !dbg !1124
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1124
  %111 = load ptr, ptr %6, align 8, !dbg !1124
  %112 = load ptr, ptr %7, align 8, !dbg !1124
  %113 = load ptr, ptr %8, align 8, !dbg !1124
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1124
  ret ptr %114, !dbg !1124
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1141 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1142, metadata !DIExpression()), !dbg !1143
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1144, metadata !DIExpression()), !dbg !1143
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1145, metadata !DIExpression()), !dbg !1143
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1146, metadata !DIExpression()), !dbg !1143
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1147, metadata !DIExpression()), !dbg !1143
  call void @llvm.va_start(ptr %9), !dbg !1143
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1148, metadata !DIExpression()), !dbg !1143
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1149, metadata !DIExpression()), !dbg !1143
  %15 = load ptr, ptr %8, align 8, !dbg !1143
  %16 = load ptr, ptr %15, align 8, !dbg !1143
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1143
  %18 = load ptr, ptr %17, align 8, !dbg !1143
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1143
  %20 = load ptr, ptr %5, align 8, !dbg !1143
  %21 = load ptr, ptr %8, align 8, !dbg !1143
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1143
  store i32 %22, ptr %11, align 4, !dbg !1143
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1150, metadata !DIExpression()), !dbg !1143
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1151, metadata !DIExpression()), !dbg !1153
  store i32 0, ptr %13, align 4, !dbg !1153
  br label %23, !dbg !1153

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1153
  %25 = load i32, ptr %11, align 4, !dbg !1153
  %26 = icmp slt i32 %24, %25, !dbg !1153
  br i1 %26, label %27, label %107, !dbg !1153

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1154
  %29 = sext i32 %28 to i64, !dbg !1154
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1154
  %31 = load i8, ptr %30, align 1, !dbg !1154
  %32 = sext i8 %31 to i32, !dbg !1154
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
  ], !dbg !1154

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1157
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1157
  store ptr %35, ptr %9, align 8, !dbg !1157
  %36 = load i32, ptr %34, align 8, !dbg !1157
  %37 = trunc i32 %36 to i8, !dbg !1157
  %38 = load i32, ptr %13, align 4, !dbg !1157
  %39 = sext i32 %38 to i64, !dbg !1157
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1157
  store i8 %37, ptr %40, align 8, !dbg !1157
  br label %103, !dbg !1157

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1157
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1157
  store ptr %43, ptr %9, align 8, !dbg !1157
  %44 = load i32, ptr %42, align 8, !dbg !1157
  %45 = trunc i32 %44 to i8, !dbg !1157
  %46 = load i32, ptr %13, align 4, !dbg !1157
  %47 = sext i32 %46 to i64, !dbg !1157
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1157
  store i8 %45, ptr %48, align 8, !dbg !1157
  br label %103, !dbg !1157

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1157
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1157
  store ptr %51, ptr %9, align 8, !dbg !1157
  %52 = load i32, ptr %50, align 8, !dbg !1157
  %53 = trunc i32 %52 to i16, !dbg !1157
  %54 = load i32, ptr %13, align 4, !dbg !1157
  %55 = sext i32 %54 to i64, !dbg !1157
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1157
  store i16 %53, ptr %56, align 8, !dbg !1157
  br label %103, !dbg !1157

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1157
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1157
  store ptr %59, ptr %9, align 8, !dbg !1157
  %60 = load i32, ptr %58, align 8, !dbg !1157
  %61 = trunc i32 %60 to i16, !dbg !1157
  %62 = load i32, ptr %13, align 4, !dbg !1157
  %63 = sext i32 %62 to i64, !dbg !1157
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1157
  store i16 %61, ptr %64, align 8, !dbg !1157
  br label %103, !dbg !1157

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1157
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1157
  store ptr %67, ptr %9, align 8, !dbg !1157
  %68 = load i32, ptr %66, align 8, !dbg !1157
  %69 = load i32, ptr %13, align 4, !dbg !1157
  %70 = sext i32 %69 to i64, !dbg !1157
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1157
  store i32 %68, ptr %71, align 8, !dbg !1157
  br label %103, !dbg !1157

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1157
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1157
  store ptr %74, ptr %9, align 8, !dbg !1157
  %75 = load i32, ptr %73, align 8, !dbg !1157
  %76 = sext i32 %75 to i64, !dbg !1157
  %77 = load i32, ptr %13, align 4, !dbg !1157
  %78 = sext i32 %77 to i64, !dbg !1157
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1157
  store i64 %76, ptr %79, align 8, !dbg !1157
  br label %103, !dbg !1157

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1157
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1157
  store ptr %82, ptr %9, align 8, !dbg !1157
  %83 = load double, ptr %81, align 8, !dbg !1157
  %84 = fptrunc double %83 to float, !dbg !1157
  %85 = load i32, ptr %13, align 4, !dbg !1157
  %86 = sext i32 %85 to i64, !dbg !1157
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1157
  store float %84, ptr %87, align 8, !dbg !1157
  br label %103, !dbg !1157

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1157
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1157
  store ptr %90, ptr %9, align 8, !dbg !1157
  %91 = load double, ptr %89, align 8, !dbg !1157
  %92 = load i32, ptr %13, align 4, !dbg !1157
  %93 = sext i32 %92 to i64, !dbg !1157
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1157
  store double %91, ptr %94, align 8, !dbg !1157
  br label %103, !dbg !1157

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1157
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1157
  store ptr %97, ptr %9, align 8, !dbg !1157
  %98 = load ptr, ptr %96, align 8, !dbg !1157
  %99 = load i32, ptr %13, align 4, !dbg !1157
  %100 = sext i32 %99 to i64, !dbg !1157
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1157
  store ptr %98, ptr %101, align 8, !dbg !1157
  br label %103, !dbg !1157

102:                                              ; preds = %27
  br label %103, !dbg !1157

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1154

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1159
  %106 = add nsw i32 %105, 1, !dbg !1159
  store i32 %106, ptr %13, align 4, !dbg !1159
  br label %23, !dbg !1159, !llvm.loop !1160

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1161, metadata !DIExpression()), !dbg !1143
  %108 = load ptr, ptr %8, align 8, !dbg !1143
  %109 = load ptr, ptr %108, align 8, !dbg !1143
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 66, !dbg !1143
  %111 = load ptr, ptr %110, align 8, !dbg !1143
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1143
  %113 = load ptr, ptr %5, align 8, !dbg !1143
  %114 = load ptr, ptr %6, align 8, !dbg !1143
  %115 = load ptr, ptr %7, align 8, !dbg !1143
  %116 = load ptr, ptr %8, align 8, !dbg !1143
  %117 = call ptr %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1143
  store ptr %117, ptr %14, align 8, !dbg !1143
  call void @llvm.va_end(ptr %9), !dbg !1143
  %118 = load ptr, ptr %14, align 8, !dbg !1143
  ret ptr %118, !dbg !1143
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1162 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1163, metadata !DIExpression()), !dbg !1164
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1165, metadata !DIExpression()), !dbg !1164
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1166, metadata !DIExpression()), !dbg !1164
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1167, metadata !DIExpression()), !dbg !1164
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1168, metadata !DIExpression()), !dbg !1164
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1169, metadata !DIExpression()), !dbg !1164
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1170, metadata !DIExpression()), !dbg !1164
  %15 = load ptr, ptr %10, align 8, !dbg !1164
  %16 = load ptr, ptr %15, align 8, !dbg !1164
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1164
  %18 = load ptr, ptr %17, align 8, !dbg !1164
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1164
  %20 = load ptr, ptr %7, align 8, !dbg !1164
  %21 = load ptr, ptr %10, align 8, !dbg !1164
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1164
  store i32 %22, ptr %12, align 4, !dbg !1164
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1171, metadata !DIExpression()), !dbg !1164
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1172, metadata !DIExpression()), !dbg !1174
  store i32 0, ptr %14, align 4, !dbg !1174
  br label %23, !dbg !1174

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1174
  %25 = load i32, ptr %12, align 4, !dbg !1174
  %26 = icmp slt i32 %24, %25, !dbg !1174
  br i1 %26, label %27, label %107, !dbg !1174

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1175
  %29 = sext i32 %28 to i64, !dbg !1175
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1175
  %31 = load i8, ptr %30, align 1, !dbg !1175
  %32 = sext i8 %31 to i32, !dbg !1175
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
  ], !dbg !1175

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1178
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1178
  store ptr %35, ptr %6, align 8, !dbg !1178
  %36 = load i32, ptr %34, align 8, !dbg !1178
  %37 = trunc i32 %36 to i8, !dbg !1178
  %38 = load i32, ptr %14, align 4, !dbg !1178
  %39 = sext i32 %38 to i64, !dbg !1178
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1178
  store i8 %37, ptr %40, align 8, !dbg !1178
  br label %103, !dbg !1178

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1178
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1178
  store ptr %43, ptr %6, align 8, !dbg !1178
  %44 = load i32, ptr %42, align 8, !dbg !1178
  %45 = trunc i32 %44 to i8, !dbg !1178
  %46 = load i32, ptr %14, align 4, !dbg !1178
  %47 = sext i32 %46 to i64, !dbg !1178
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1178
  store i8 %45, ptr %48, align 8, !dbg !1178
  br label %103, !dbg !1178

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1178
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1178
  store ptr %51, ptr %6, align 8, !dbg !1178
  %52 = load i32, ptr %50, align 8, !dbg !1178
  %53 = trunc i32 %52 to i16, !dbg !1178
  %54 = load i32, ptr %14, align 4, !dbg !1178
  %55 = sext i32 %54 to i64, !dbg !1178
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1178
  store i16 %53, ptr %56, align 8, !dbg !1178
  br label %103, !dbg !1178

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1178
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1178
  store ptr %59, ptr %6, align 8, !dbg !1178
  %60 = load i32, ptr %58, align 8, !dbg !1178
  %61 = trunc i32 %60 to i16, !dbg !1178
  %62 = load i32, ptr %14, align 4, !dbg !1178
  %63 = sext i32 %62 to i64, !dbg !1178
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1178
  store i16 %61, ptr %64, align 8, !dbg !1178
  br label %103, !dbg !1178

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1178
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1178
  store ptr %67, ptr %6, align 8, !dbg !1178
  %68 = load i32, ptr %66, align 8, !dbg !1178
  %69 = load i32, ptr %14, align 4, !dbg !1178
  %70 = sext i32 %69 to i64, !dbg !1178
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1178
  store i32 %68, ptr %71, align 8, !dbg !1178
  br label %103, !dbg !1178

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1178
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1178
  store ptr %74, ptr %6, align 8, !dbg !1178
  %75 = load i32, ptr %73, align 8, !dbg !1178
  %76 = sext i32 %75 to i64, !dbg !1178
  %77 = load i32, ptr %14, align 4, !dbg !1178
  %78 = sext i32 %77 to i64, !dbg !1178
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1178
  store i64 %76, ptr %79, align 8, !dbg !1178
  br label %103, !dbg !1178

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1178
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1178
  store ptr %82, ptr %6, align 8, !dbg !1178
  %83 = load double, ptr %81, align 8, !dbg !1178
  %84 = fptrunc double %83 to float, !dbg !1178
  %85 = load i32, ptr %14, align 4, !dbg !1178
  %86 = sext i32 %85 to i64, !dbg !1178
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1178
  store float %84, ptr %87, align 8, !dbg !1178
  br label %103, !dbg !1178

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1178
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1178
  store ptr %90, ptr %6, align 8, !dbg !1178
  %91 = load double, ptr %89, align 8, !dbg !1178
  %92 = load i32, ptr %14, align 4, !dbg !1178
  %93 = sext i32 %92 to i64, !dbg !1178
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1178
  store double %91, ptr %94, align 8, !dbg !1178
  br label %103, !dbg !1178

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1178
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1178
  store ptr %97, ptr %6, align 8, !dbg !1178
  %98 = load ptr, ptr %96, align 8, !dbg !1178
  %99 = load i32, ptr %14, align 4, !dbg !1178
  %100 = sext i32 %99 to i64, !dbg !1178
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1178
  store ptr %98, ptr %101, align 8, !dbg !1178
  br label %103, !dbg !1178

102:                                              ; preds = %27
  br label %103, !dbg !1178

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1175

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1180
  %106 = add nsw i32 %105, 1, !dbg !1180
  store i32 %106, ptr %14, align 4, !dbg !1180
  br label %23, !dbg !1180, !llvm.loop !1181

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1164
  %109 = load ptr, ptr %108, align 8, !dbg !1164
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 66, !dbg !1164
  %111 = load ptr, ptr %110, align 8, !dbg !1164
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1164
  %113 = load ptr, ptr %7, align 8, !dbg !1164
  %114 = load ptr, ptr %8, align 8, !dbg !1164
  %115 = load ptr, ptr %9, align 8, !dbg !1164
  %116 = load ptr, ptr %10, align 8, !dbg !1164
  %117 = call ptr %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1164
  ret ptr %117, !dbg !1164
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1182 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1183, metadata !DIExpression()), !dbg !1184
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1185, metadata !DIExpression()), !dbg !1184
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1186, metadata !DIExpression()), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1187, metadata !DIExpression()), !dbg !1184
  call void @llvm.va_start(ptr %7), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1188, metadata !DIExpression()), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1189, metadata !DIExpression()), !dbg !1184
  %13 = load ptr, ptr %6, align 8, !dbg !1184
  %14 = load ptr, ptr %13, align 8, !dbg !1184
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1184
  %16 = load ptr, ptr %15, align 8, !dbg !1184
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1184
  %18 = load ptr, ptr %4, align 8, !dbg !1184
  %19 = load ptr, ptr %6, align 8, !dbg !1184
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1184
  store i32 %20, ptr %9, align 4, !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1190, metadata !DIExpression()), !dbg !1184
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1191, metadata !DIExpression()), !dbg !1193
  store i32 0, ptr %11, align 4, !dbg !1193
  br label %21, !dbg !1193

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1193
  %23 = load i32, ptr %9, align 4, !dbg !1193
  %24 = icmp slt i32 %22, %23, !dbg !1193
  br i1 %24, label %25, label %105, !dbg !1193

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1194
  %27 = sext i32 %26 to i64, !dbg !1194
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1194
  %29 = load i8, ptr %28, align 1, !dbg !1194
  %30 = sext i8 %29 to i32, !dbg !1194
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
  ], !dbg !1194

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1197
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1197
  store ptr %33, ptr %7, align 8, !dbg !1197
  %34 = load i32, ptr %32, align 8, !dbg !1197
  %35 = trunc i32 %34 to i8, !dbg !1197
  %36 = load i32, ptr %11, align 4, !dbg !1197
  %37 = sext i32 %36 to i64, !dbg !1197
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1197
  store i8 %35, ptr %38, align 8, !dbg !1197
  br label %101, !dbg !1197

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1197
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1197
  store ptr %41, ptr %7, align 8, !dbg !1197
  %42 = load i32, ptr %40, align 8, !dbg !1197
  %43 = trunc i32 %42 to i8, !dbg !1197
  %44 = load i32, ptr %11, align 4, !dbg !1197
  %45 = sext i32 %44 to i64, !dbg !1197
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1197
  store i8 %43, ptr %46, align 8, !dbg !1197
  br label %101, !dbg !1197

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1197
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1197
  store ptr %49, ptr %7, align 8, !dbg !1197
  %50 = load i32, ptr %48, align 8, !dbg !1197
  %51 = trunc i32 %50 to i16, !dbg !1197
  %52 = load i32, ptr %11, align 4, !dbg !1197
  %53 = sext i32 %52 to i64, !dbg !1197
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1197
  store i16 %51, ptr %54, align 8, !dbg !1197
  br label %101, !dbg !1197

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1197
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1197
  store ptr %57, ptr %7, align 8, !dbg !1197
  %58 = load i32, ptr %56, align 8, !dbg !1197
  %59 = trunc i32 %58 to i16, !dbg !1197
  %60 = load i32, ptr %11, align 4, !dbg !1197
  %61 = sext i32 %60 to i64, !dbg !1197
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1197
  store i16 %59, ptr %62, align 8, !dbg !1197
  br label %101, !dbg !1197

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1197
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1197
  store ptr %65, ptr %7, align 8, !dbg !1197
  %66 = load i32, ptr %64, align 8, !dbg !1197
  %67 = load i32, ptr %11, align 4, !dbg !1197
  %68 = sext i32 %67 to i64, !dbg !1197
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1197
  store i32 %66, ptr %69, align 8, !dbg !1197
  br label %101, !dbg !1197

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1197
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1197
  store ptr %72, ptr %7, align 8, !dbg !1197
  %73 = load i32, ptr %71, align 8, !dbg !1197
  %74 = sext i32 %73 to i64, !dbg !1197
  %75 = load i32, ptr %11, align 4, !dbg !1197
  %76 = sext i32 %75 to i64, !dbg !1197
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1197
  store i64 %74, ptr %77, align 8, !dbg !1197
  br label %101, !dbg !1197

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1197
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1197
  store ptr %80, ptr %7, align 8, !dbg !1197
  %81 = load double, ptr %79, align 8, !dbg !1197
  %82 = fptrunc double %81 to float, !dbg !1197
  %83 = load i32, ptr %11, align 4, !dbg !1197
  %84 = sext i32 %83 to i64, !dbg !1197
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1197
  store float %82, ptr %85, align 8, !dbg !1197
  br label %101, !dbg !1197

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1197
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1197
  store ptr %88, ptr %7, align 8, !dbg !1197
  %89 = load double, ptr %87, align 8, !dbg !1197
  %90 = load i32, ptr %11, align 4, !dbg !1197
  %91 = sext i32 %90 to i64, !dbg !1197
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1197
  store double %89, ptr %92, align 8, !dbg !1197
  br label %101, !dbg !1197

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1197
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1197
  store ptr %95, ptr %7, align 8, !dbg !1197
  %96 = load ptr, ptr %94, align 8, !dbg !1197
  %97 = load i32, ptr %11, align 4, !dbg !1197
  %98 = sext i32 %97 to i64, !dbg !1197
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1197
  store ptr %96, ptr %99, align 8, !dbg !1197
  br label %101, !dbg !1197

100:                                              ; preds = %25
  br label %101, !dbg !1197

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1194

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1199
  %104 = add nsw i32 %103, 1, !dbg !1199
  store i32 %104, ptr %11, align 4, !dbg !1199
  br label %21, !dbg !1199, !llvm.loop !1200

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1201, metadata !DIExpression()), !dbg !1184
  %106 = load ptr, ptr %6, align 8, !dbg !1184
  %107 = load ptr, ptr %106, align 8, !dbg !1184
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 116, !dbg !1184
  %109 = load ptr, ptr %108, align 8, !dbg !1184
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1184
  %111 = load ptr, ptr %4, align 8, !dbg !1184
  %112 = load ptr, ptr %5, align 8, !dbg !1184
  %113 = load ptr, ptr %6, align 8, !dbg !1184
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1184
  store ptr %114, ptr %12, align 8, !dbg !1184
  call void @llvm.va_end(ptr %7), !dbg !1184
  %115 = load ptr, ptr %12, align 8, !dbg !1184
  ret ptr %115, !dbg !1184
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1202 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1203, metadata !DIExpression()), !dbg !1204
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1205, metadata !DIExpression()), !dbg !1204
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1206, metadata !DIExpression()), !dbg !1204
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1207, metadata !DIExpression()), !dbg !1204
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1208, metadata !DIExpression()), !dbg !1204
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1209, metadata !DIExpression()), !dbg !1204
  %13 = load ptr, ptr %8, align 8, !dbg !1204
  %14 = load ptr, ptr %13, align 8, !dbg !1204
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1204
  %16 = load ptr, ptr %15, align 8, !dbg !1204
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1204
  %18 = load ptr, ptr %6, align 8, !dbg !1204
  %19 = load ptr, ptr %8, align 8, !dbg !1204
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1204
  store i32 %20, ptr %10, align 4, !dbg !1204
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1210, metadata !DIExpression()), !dbg !1204
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1211, metadata !DIExpression()), !dbg !1213
  store i32 0, ptr %12, align 4, !dbg !1213
  br label %21, !dbg !1213

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1213
  %23 = load i32, ptr %10, align 4, !dbg !1213
  %24 = icmp slt i32 %22, %23, !dbg !1213
  br i1 %24, label %25, label %105, !dbg !1213

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1214
  %27 = sext i32 %26 to i64, !dbg !1214
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1214
  %29 = load i8, ptr %28, align 1, !dbg !1214
  %30 = sext i8 %29 to i32, !dbg !1214
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
  ], !dbg !1214

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1217
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1217
  store ptr %33, ptr %5, align 8, !dbg !1217
  %34 = load i32, ptr %32, align 8, !dbg !1217
  %35 = trunc i32 %34 to i8, !dbg !1217
  %36 = load i32, ptr %12, align 4, !dbg !1217
  %37 = sext i32 %36 to i64, !dbg !1217
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1217
  store i8 %35, ptr %38, align 8, !dbg !1217
  br label %101, !dbg !1217

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1217
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1217
  store ptr %41, ptr %5, align 8, !dbg !1217
  %42 = load i32, ptr %40, align 8, !dbg !1217
  %43 = trunc i32 %42 to i8, !dbg !1217
  %44 = load i32, ptr %12, align 4, !dbg !1217
  %45 = sext i32 %44 to i64, !dbg !1217
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1217
  store i8 %43, ptr %46, align 8, !dbg !1217
  br label %101, !dbg !1217

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1217
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1217
  store ptr %49, ptr %5, align 8, !dbg !1217
  %50 = load i32, ptr %48, align 8, !dbg !1217
  %51 = trunc i32 %50 to i16, !dbg !1217
  %52 = load i32, ptr %12, align 4, !dbg !1217
  %53 = sext i32 %52 to i64, !dbg !1217
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1217
  store i16 %51, ptr %54, align 8, !dbg !1217
  br label %101, !dbg !1217

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1217
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1217
  store ptr %57, ptr %5, align 8, !dbg !1217
  %58 = load i32, ptr %56, align 8, !dbg !1217
  %59 = trunc i32 %58 to i16, !dbg !1217
  %60 = load i32, ptr %12, align 4, !dbg !1217
  %61 = sext i32 %60 to i64, !dbg !1217
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1217
  store i16 %59, ptr %62, align 8, !dbg !1217
  br label %101, !dbg !1217

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1217
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1217
  store ptr %65, ptr %5, align 8, !dbg !1217
  %66 = load i32, ptr %64, align 8, !dbg !1217
  %67 = load i32, ptr %12, align 4, !dbg !1217
  %68 = sext i32 %67 to i64, !dbg !1217
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1217
  store i32 %66, ptr %69, align 8, !dbg !1217
  br label %101, !dbg !1217

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1217
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1217
  store ptr %72, ptr %5, align 8, !dbg !1217
  %73 = load i32, ptr %71, align 8, !dbg !1217
  %74 = sext i32 %73 to i64, !dbg !1217
  %75 = load i32, ptr %12, align 4, !dbg !1217
  %76 = sext i32 %75 to i64, !dbg !1217
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1217
  store i64 %74, ptr %77, align 8, !dbg !1217
  br label %101, !dbg !1217

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1217
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1217
  store ptr %80, ptr %5, align 8, !dbg !1217
  %81 = load double, ptr %79, align 8, !dbg !1217
  %82 = fptrunc double %81 to float, !dbg !1217
  %83 = load i32, ptr %12, align 4, !dbg !1217
  %84 = sext i32 %83 to i64, !dbg !1217
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1217
  store float %82, ptr %85, align 8, !dbg !1217
  br label %101, !dbg !1217

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1217
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1217
  store ptr %88, ptr %5, align 8, !dbg !1217
  %89 = load double, ptr %87, align 8, !dbg !1217
  %90 = load i32, ptr %12, align 4, !dbg !1217
  %91 = sext i32 %90 to i64, !dbg !1217
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1217
  store double %89, ptr %92, align 8, !dbg !1217
  br label %101, !dbg !1217

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1217
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1217
  store ptr %95, ptr %5, align 8, !dbg !1217
  %96 = load ptr, ptr %94, align 8, !dbg !1217
  %97 = load i32, ptr %12, align 4, !dbg !1217
  %98 = sext i32 %97 to i64, !dbg !1217
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1217
  store ptr %96, ptr %99, align 8, !dbg !1217
  br label %101, !dbg !1217

100:                                              ; preds = %25
  br label %101, !dbg !1217

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1214

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1219
  %104 = add nsw i32 %103, 1, !dbg !1219
  store i32 %104, ptr %12, align 4, !dbg !1219
  br label %21, !dbg !1219, !llvm.loop !1220

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1204
  %107 = load ptr, ptr %106, align 8, !dbg !1204
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 116, !dbg !1204
  %109 = load ptr, ptr %108, align 8, !dbg !1204
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1204
  %111 = load ptr, ptr %6, align 8, !dbg !1204
  %112 = load ptr, ptr %7, align 8, !dbg !1204
  %113 = load ptr, ptr %8, align 8, !dbg !1204
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1204
  ret ptr %114, !dbg !1204
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1221 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1222, metadata !DIExpression()), !dbg !1223
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1224, metadata !DIExpression()), !dbg !1223
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1225, metadata !DIExpression()), !dbg !1223
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1226, metadata !DIExpression()), !dbg !1223
  call void @llvm.va_start(ptr %7), !dbg !1223
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1227, metadata !DIExpression()), !dbg !1223
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1228, metadata !DIExpression()), !dbg !1223
  %13 = load ptr, ptr %6, align 8, !dbg !1223
  %14 = load ptr, ptr %13, align 8, !dbg !1223
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1223
  %16 = load ptr, ptr %15, align 8, !dbg !1223
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1223
  %18 = load ptr, ptr %4, align 8, !dbg !1223
  %19 = load ptr, ptr %6, align 8, !dbg !1223
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1223
  store i32 %20, ptr %9, align 4, !dbg !1223
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1229, metadata !DIExpression()), !dbg !1223
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1230, metadata !DIExpression()), !dbg !1232
  store i32 0, ptr %11, align 4, !dbg !1232
  br label %21, !dbg !1232

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1232
  %23 = load i32, ptr %9, align 4, !dbg !1232
  %24 = icmp slt i32 %22, %23, !dbg !1232
  br i1 %24, label %25, label %105, !dbg !1232

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1233
  %27 = sext i32 %26 to i64, !dbg !1233
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1233
  %29 = load i8, ptr %28, align 1, !dbg !1233
  %30 = sext i8 %29 to i32, !dbg !1233
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
  ], !dbg !1233

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1236
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1236
  store ptr %33, ptr %7, align 8, !dbg !1236
  %34 = load i32, ptr %32, align 8, !dbg !1236
  %35 = trunc i32 %34 to i8, !dbg !1236
  %36 = load i32, ptr %11, align 4, !dbg !1236
  %37 = sext i32 %36 to i64, !dbg !1236
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1236
  store i8 %35, ptr %38, align 8, !dbg !1236
  br label %101, !dbg !1236

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1236
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1236
  store ptr %41, ptr %7, align 8, !dbg !1236
  %42 = load i32, ptr %40, align 8, !dbg !1236
  %43 = trunc i32 %42 to i8, !dbg !1236
  %44 = load i32, ptr %11, align 4, !dbg !1236
  %45 = sext i32 %44 to i64, !dbg !1236
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1236
  store i8 %43, ptr %46, align 8, !dbg !1236
  br label %101, !dbg !1236

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1236
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1236
  store ptr %49, ptr %7, align 8, !dbg !1236
  %50 = load i32, ptr %48, align 8, !dbg !1236
  %51 = trunc i32 %50 to i16, !dbg !1236
  %52 = load i32, ptr %11, align 4, !dbg !1236
  %53 = sext i32 %52 to i64, !dbg !1236
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1236
  store i16 %51, ptr %54, align 8, !dbg !1236
  br label %101, !dbg !1236

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1236
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1236
  store ptr %57, ptr %7, align 8, !dbg !1236
  %58 = load i32, ptr %56, align 8, !dbg !1236
  %59 = trunc i32 %58 to i16, !dbg !1236
  %60 = load i32, ptr %11, align 4, !dbg !1236
  %61 = sext i32 %60 to i64, !dbg !1236
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1236
  store i16 %59, ptr %62, align 8, !dbg !1236
  br label %101, !dbg !1236

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1236
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1236
  store ptr %65, ptr %7, align 8, !dbg !1236
  %66 = load i32, ptr %64, align 8, !dbg !1236
  %67 = load i32, ptr %11, align 4, !dbg !1236
  %68 = sext i32 %67 to i64, !dbg !1236
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1236
  store i32 %66, ptr %69, align 8, !dbg !1236
  br label %101, !dbg !1236

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1236
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1236
  store ptr %72, ptr %7, align 8, !dbg !1236
  %73 = load i32, ptr %71, align 8, !dbg !1236
  %74 = sext i32 %73 to i64, !dbg !1236
  %75 = load i32, ptr %11, align 4, !dbg !1236
  %76 = sext i32 %75 to i64, !dbg !1236
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1236
  store i64 %74, ptr %77, align 8, !dbg !1236
  br label %101, !dbg !1236

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1236
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1236
  store ptr %80, ptr %7, align 8, !dbg !1236
  %81 = load double, ptr %79, align 8, !dbg !1236
  %82 = fptrunc double %81 to float, !dbg !1236
  %83 = load i32, ptr %11, align 4, !dbg !1236
  %84 = sext i32 %83 to i64, !dbg !1236
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1236
  store float %82, ptr %85, align 8, !dbg !1236
  br label %101, !dbg !1236

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1236
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1236
  store ptr %88, ptr %7, align 8, !dbg !1236
  %89 = load double, ptr %87, align 8, !dbg !1236
  %90 = load i32, ptr %11, align 4, !dbg !1236
  %91 = sext i32 %90 to i64, !dbg !1236
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1236
  store double %89, ptr %92, align 8, !dbg !1236
  br label %101, !dbg !1236

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1236
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1236
  store ptr %95, ptr %7, align 8, !dbg !1236
  %96 = load ptr, ptr %94, align 8, !dbg !1236
  %97 = load i32, ptr %11, align 4, !dbg !1236
  %98 = sext i32 %97 to i64, !dbg !1236
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1236
  store ptr %96, ptr %99, align 8, !dbg !1236
  br label %101, !dbg !1236

100:                                              ; preds = %25
  br label %101, !dbg !1236

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1233

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1238
  %104 = add nsw i32 %103, 1, !dbg !1238
  store i32 %104, ptr %11, align 4, !dbg !1238
  br label %21, !dbg !1238, !llvm.loop !1239

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1240, metadata !DIExpression()), !dbg !1223
  %106 = load ptr, ptr %6, align 8, !dbg !1223
  %107 = load ptr, ptr %106, align 8, !dbg !1223
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 39, !dbg !1223
  %109 = load ptr, ptr %108, align 8, !dbg !1223
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1223
  %111 = load ptr, ptr %4, align 8, !dbg !1223
  %112 = load ptr, ptr %5, align 8, !dbg !1223
  %113 = load ptr, ptr %6, align 8, !dbg !1223
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1223
  store i8 %114, ptr %12, align 1, !dbg !1223
  call void @llvm.va_end(ptr %7), !dbg !1223
  %115 = load i8, ptr %12, align 1, !dbg !1223
  ret i8 %115, !dbg !1223
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1241 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1242, metadata !DIExpression()), !dbg !1243
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1244, metadata !DIExpression()), !dbg !1243
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1245, metadata !DIExpression()), !dbg !1243
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1246, metadata !DIExpression()), !dbg !1243
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1247, metadata !DIExpression()), !dbg !1243
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1248, metadata !DIExpression()), !dbg !1243
  %13 = load ptr, ptr %8, align 8, !dbg !1243
  %14 = load ptr, ptr %13, align 8, !dbg !1243
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1243
  %16 = load ptr, ptr %15, align 8, !dbg !1243
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1243
  %18 = load ptr, ptr %6, align 8, !dbg !1243
  %19 = load ptr, ptr %8, align 8, !dbg !1243
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1243
  store i32 %20, ptr %10, align 4, !dbg !1243
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1249, metadata !DIExpression()), !dbg !1243
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1250, metadata !DIExpression()), !dbg !1252
  store i32 0, ptr %12, align 4, !dbg !1252
  br label %21, !dbg !1252

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1252
  %23 = load i32, ptr %10, align 4, !dbg !1252
  %24 = icmp slt i32 %22, %23, !dbg !1252
  br i1 %24, label %25, label %105, !dbg !1252

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1253
  %27 = sext i32 %26 to i64, !dbg !1253
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1253
  %29 = load i8, ptr %28, align 1, !dbg !1253
  %30 = sext i8 %29 to i32, !dbg !1253
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
  ], !dbg !1253

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1256
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1256
  store ptr %33, ptr %5, align 8, !dbg !1256
  %34 = load i32, ptr %32, align 8, !dbg !1256
  %35 = trunc i32 %34 to i8, !dbg !1256
  %36 = load i32, ptr %12, align 4, !dbg !1256
  %37 = sext i32 %36 to i64, !dbg !1256
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1256
  store i8 %35, ptr %38, align 8, !dbg !1256
  br label %101, !dbg !1256

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1256
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1256
  store ptr %41, ptr %5, align 8, !dbg !1256
  %42 = load i32, ptr %40, align 8, !dbg !1256
  %43 = trunc i32 %42 to i8, !dbg !1256
  %44 = load i32, ptr %12, align 4, !dbg !1256
  %45 = sext i32 %44 to i64, !dbg !1256
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1256
  store i8 %43, ptr %46, align 8, !dbg !1256
  br label %101, !dbg !1256

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1256
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1256
  store ptr %49, ptr %5, align 8, !dbg !1256
  %50 = load i32, ptr %48, align 8, !dbg !1256
  %51 = trunc i32 %50 to i16, !dbg !1256
  %52 = load i32, ptr %12, align 4, !dbg !1256
  %53 = sext i32 %52 to i64, !dbg !1256
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1256
  store i16 %51, ptr %54, align 8, !dbg !1256
  br label %101, !dbg !1256

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1256
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1256
  store ptr %57, ptr %5, align 8, !dbg !1256
  %58 = load i32, ptr %56, align 8, !dbg !1256
  %59 = trunc i32 %58 to i16, !dbg !1256
  %60 = load i32, ptr %12, align 4, !dbg !1256
  %61 = sext i32 %60 to i64, !dbg !1256
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1256
  store i16 %59, ptr %62, align 8, !dbg !1256
  br label %101, !dbg !1256

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1256
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1256
  store ptr %65, ptr %5, align 8, !dbg !1256
  %66 = load i32, ptr %64, align 8, !dbg !1256
  %67 = load i32, ptr %12, align 4, !dbg !1256
  %68 = sext i32 %67 to i64, !dbg !1256
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1256
  store i32 %66, ptr %69, align 8, !dbg !1256
  br label %101, !dbg !1256

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1256
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1256
  store ptr %72, ptr %5, align 8, !dbg !1256
  %73 = load i32, ptr %71, align 8, !dbg !1256
  %74 = sext i32 %73 to i64, !dbg !1256
  %75 = load i32, ptr %12, align 4, !dbg !1256
  %76 = sext i32 %75 to i64, !dbg !1256
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1256
  store i64 %74, ptr %77, align 8, !dbg !1256
  br label %101, !dbg !1256

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1256
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1256
  store ptr %80, ptr %5, align 8, !dbg !1256
  %81 = load double, ptr %79, align 8, !dbg !1256
  %82 = fptrunc double %81 to float, !dbg !1256
  %83 = load i32, ptr %12, align 4, !dbg !1256
  %84 = sext i32 %83 to i64, !dbg !1256
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1256
  store float %82, ptr %85, align 8, !dbg !1256
  br label %101, !dbg !1256

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1256
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1256
  store ptr %88, ptr %5, align 8, !dbg !1256
  %89 = load double, ptr %87, align 8, !dbg !1256
  %90 = load i32, ptr %12, align 4, !dbg !1256
  %91 = sext i32 %90 to i64, !dbg !1256
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1256
  store double %89, ptr %92, align 8, !dbg !1256
  br label %101, !dbg !1256

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1256
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1256
  store ptr %95, ptr %5, align 8, !dbg !1256
  %96 = load ptr, ptr %94, align 8, !dbg !1256
  %97 = load i32, ptr %12, align 4, !dbg !1256
  %98 = sext i32 %97 to i64, !dbg !1256
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1256
  store ptr %96, ptr %99, align 8, !dbg !1256
  br label %101, !dbg !1256

100:                                              ; preds = %25
  br label %101, !dbg !1256

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1253

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1258
  %104 = add nsw i32 %103, 1, !dbg !1258
  store i32 %104, ptr %12, align 4, !dbg !1258
  br label %21, !dbg !1258, !llvm.loop !1259

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1243
  %107 = load ptr, ptr %106, align 8, !dbg !1243
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 39, !dbg !1243
  %109 = load ptr, ptr %108, align 8, !dbg !1243
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1243
  %111 = load ptr, ptr %6, align 8, !dbg !1243
  %112 = load ptr, ptr %7, align 8, !dbg !1243
  %113 = load ptr, ptr %8, align 8, !dbg !1243
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1243
  ret i8 %114, !dbg !1243
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1260 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1261, metadata !DIExpression()), !dbg !1262
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1263, metadata !DIExpression()), !dbg !1262
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1264, metadata !DIExpression()), !dbg !1262
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1265, metadata !DIExpression()), !dbg !1262
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1266, metadata !DIExpression()), !dbg !1262
  call void @llvm.va_start(ptr %9), !dbg !1262
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1267, metadata !DIExpression()), !dbg !1262
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1268, metadata !DIExpression()), !dbg !1262
  %15 = load ptr, ptr %8, align 8, !dbg !1262
  %16 = load ptr, ptr %15, align 8, !dbg !1262
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1262
  %18 = load ptr, ptr %17, align 8, !dbg !1262
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1262
  %20 = load ptr, ptr %5, align 8, !dbg !1262
  %21 = load ptr, ptr %8, align 8, !dbg !1262
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1262
  store i32 %22, ptr %11, align 4, !dbg !1262
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1269, metadata !DIExpression()), !dbg !1262
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1270, metadata !DIExpression()), !dbg !1272
  store i32 0, ptr %13, align 4, !dbg !1272
  br label %23, !dbg !1272

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1272
  %25 = load i32, ptr %11, align 4, !dbg !1272
  %26 = icmp slt i32 %24, %25, !dbg !1272
  br i1 %26, label %27, label %107, !dbg !1272

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1273
  %29 = sext i32 %28 to i64, !dbg !1273
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1273
  %31 = load i8, ptr %30, align 1, !dbg !1273
  %32 = sext i8 %31 to i32, !dbg !1273
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
  ], !dbg !1273

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1276
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1276
  store ptr %35, ptr %9, align 8, !dbg !1276
  %36 = load i32, ptr %34, align 8, !dbg !1276
  %37 = trunc i32 %36 to i8, !dbg !1276
  %38 = load i32, ptr %13, align 4, !dbg !1276
  %39 = sext i32 %38 to i64, !dbg !1276
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1276
  store i8 %37, ptr %40, align 8, !dbg !1276
  br label %103, !dbg !1276

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1276
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1276
  store ptr %43, ptr %9, align 8, !dbg !1276
  %44 = load i32, ptr %42, align 8, !dbg !1276
  %45 = trunc i32 %44 to i8, !dbg !1276
  %46 = load i32, ptr %13, align 4, !dbg !1276
  %47 = sext i32 %46 to i64, !dbg !1276
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1276
  store i8 %45, ptr %48, align 8, !dbg !1276
  br label %103, !dbg !1276

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1276
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1276
  store ptr %51, ptr %9, align 8, !dbg !1276
  %52 = load i32, ptr %50, align 8, !dbg !1276
  %53 = trunc i32 %52 to i16, !dbg !1276
  %54 = load i32, ptr %13, align 4, !dbg !1276
  %55 = sext i32 %54 to i64, !dbg !1276
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1276
  store i16 %53, ptr %56, align 8, !dbg !1276
  br label %103, !dbg !1276

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1276
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1276
  store ptr %59, ptr %9, align 8, !dbg !1276
  %60 = load i32, ptr %58, align 8, !dbg !1276
  %61 = trunc i32 %60 to i16, !dbg !1276
  %62 = load i32, ptr %13, align 4, !dbg !1276
  %63 = sext i32 %62 to i64, !dbg !1276
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1276
  store i16 %61, ptr %64, align 8, !dbg !1276
  br label %103, !dbg !1276

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1276
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1276
  store ptr %67, ptr %9, align 8, !dbg !1276
  %68 = load i32, ptr %66, align 8, !dbg !1276
  %69 = load i32, ptr %13, align 4, !dbg !1276
  %70 = sext i32 %69 to i64, !dbg !1276
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1276
  store i32 %68, ptr %71, align 8, !dbg !1276
  br label %103, !dbg !1276

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1276
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1276
  store ptr %74, ptr %9, align 8, !dbg !1276
  %75 = load i32, ptr %73, align 8, !dbg !1276
  %76 = sext i32 %75 to i64, !dbg !1276
  %77 = load i32, ptr %13, align 4, !dbg !1276
  %78 = sext i32 %77 to i64, !dbg !1276
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1276
  store i64 %76, ptr %79, align 8, !dbg !1276
  br label %103, !dbg !1276

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1276
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1276
  store ptr %82, ptr %9, align 8, !dbg !1276
  %83 = load double, ptr %81, align 8, !dbg !1276
  %84 = fptrunc double %83 to float, !dbg !1276
  %85 = load i32, ptr %13, align 4, !dbg !1276
  %86 = sext i32 %85 to i64, !dbg !1276
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1276
  store float %84, ptr %87, align 8, !dbg !1276
  br label %103, !dbg !1276

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1276
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1276
  store ptr %90, ptr %9, align 8, !dbg !1276
  %91 = load double, ptr %89, align 8, !dbg !1276
  %92 = load i32, ptr %13, align 4, !dbg !1276
  %93 = sext i32 %92 to i64, !dbg !1276
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1276
  store double %91, ptr %94, align 8, !dbg !1276
  br label %103, !dbg !1276

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1276
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1276
  store ptr %97, ptr %9, align 8, !dbg !1276
  %98 = load ptr, ptr %96, align 8, !dbg !1276
  %99 = load i32, ptr %13, align 4, !dbg !1276
  %100 = sext i32 %99 to i64, !dbg !1276
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1276
  store ptr %98, ptr %101, align 8, !dbg !1276
  br label %103, !dbg !1276

102:                                              ; preds = %27
  br label %103, !dbg !1276

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1273

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1278
  %106 = add nsw i32 %105, 1, !dbg !1278
  store i32 %106, ptr %13, align 4, !dbg !1278
  br label %23, !dbg !1278, !llvm.loop !1279

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1280, metadata !DIExpression()), !dbg !1262
  %108 = load ptr, ptr %8, align 8, !dbg !1262
  %109 = load ptr, ptr %108, align 8, !dbg !1262
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 69, !dbg !1262
  %111 = load ptr, ptr %110, align 8, !dbg !1262
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1262
  %113 = load ptr, ptr %5, align 8, !dbg !1262
  %114 = load ptr, ptr %6, align 8, !dbg !1262
  %115 = load ptr, ptr %7, align 8, !dbg !1262
  %116 = load ptr, ptr %8, align 8, !dbg !1262
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1262
  store i8 %117, ptr %14, align 1, !dbg !1262
  call void @llvm.va_end(ptr %9), !dbg !1262
  %118 = load i8, ptr %14, align 1, !dbg !1262
  ret i8 %118, !dbg !1262
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1281 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1282, metadata !DIExpression()), !dbg !1283
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1284, metadata !DIExpression()), !dbg !1283
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1285, metadata !DIExpression()), !dbg !1283
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1286, metadata !DIExpression()), !dbg !1283
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1287, metadata !DIExpression()), !dbg !1283
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1288, metadata !DIExpression()), !dbg !1283
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1289, metadata !DIExpression()), !dbg !1283
  %15 = load ptr, ptr %10, align 8, !dbg !1283
  %16 = load ptr, ptr %15, align 8, !dbg !1283
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1283
  %18 = load ptr, ptr %17, align 8, !dbg !1283
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1283
  %20 = load ptr, ptr %7, align 8, !dbg !1283
  %21 = load ptr, ptr %10, align 8, !dbg !1283
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1283
  store i32 %22, ptr %12, align 4, !dbg !1283
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1290, metadata !DIExpression()), !dbg !1283
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1291, metadata !DIExpression()), !dbg !1293
  store i32 0, ptr %14, align 4, !dbg !1293
  br label %23, !dbg !1293

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1293
  %25 = load i32, ptr %12, align 4, !dbg !1293
  %26 = icmp slt i32 %24, %25, !dbg !1293
  br i1 %26, label %27, label %107, !dbg !1293

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1294
  %29 = sext i32 %28 to i64, !dbg !1294
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1294
  %31 = load i8, ptr %30, align 1, !dbg !1294
  %32 = sext i8 %31 to i32, !dbg !1294
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
  ], !dbg !1294

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1297
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1297
  store ptr %35, ptr %6, align 8, !dbg !1297
  %36 = load i32, ptr %34, align 8, !dbg !1297
  %37 = trunc i32 %36 to i8, !dbg !1297
  %38 = load i32, ptr %14, align 4, !dbg !1297
  %39 = sext i32 %38 to i64, !dbg !1297
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1297
  store i8 %37, ptr %40, align 8, !dbg !1297
  br label %103, !dbg !1297

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1297
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1297
  store ptr %43, ptr %6, align 8, !dbg !1297
  %44 = load i32, ptr %42, align 8, !dbg !1297
  %45 = trunc i32 %44 to i8, !dbg !1297
  %46 = load i32, ptr %14, align 4, !dbg !1297
  %47 = sext i32 %46 to i64, !dbg !1297
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1297
  store i8 %45, ptr %48, align 8, !dbg !1297
  br label %103, !dbg !1297

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1297
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1297
  store ptr %51, ptr %6, align 8, !dbg !1297
  %52 = load i32, ptr %50, align 8, !dbg !1297
  %53 = trunc i32 %52 to i16, !dbg !1297
  %54 = load i32, ptr %14, align 4, !dbg !1297
  %55 = sext i32 %54 to i64, !dbg !1297
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1297
  store i16 %53, ptr %56, align 8, !dbg !1297
  br label %103, !dbg !1297

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1297
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1297
  store ptr %59, ptr %6, align 8, !dbg !1297
  %60 = load i32, ptr %58, align 8, !dbg !1297
  %61 = trunc i32 %60 to i16, !dbg !1297
  %62 = load i32, ptr %14, align 4, !dbg !1297
  %63 = sext i32 %62 to i64, !dbg !1297
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1297
  store i16 %61, ptr %64, align 8, !dbg !1297
  br label %103, !dbg !1297

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1297
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1297
  store ptr %67, ptr %6, align 8, !dbg !1297
  %68 = load i32, ptr %66, align 8, !dbg !1297
  %69 = load i32, ptr %14, align 4, !dbg !1297
  %70 = sext i32 %69 to i64, !dbg !1297
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1297
  store i32 %68, ptr %71, align 8, !dbg !1297
  br label %103, !dbg !1297

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1297
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1297
  store ptr %74, ptr %6, align 8, !dbg !1297
  %75 = load i32, ptr %73, align 8, !dbg !1297
  %76 = sext i32 %75 to i64, !dbg !1297
  %77 = load i32, ptr %14, align 4, !dbg !1297
  %78 = sext i32 %77 to i64, !dbg !1297
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1297
  store i64 %76, ptr %79, align 8, !dbg !1297
  br label %103, !dbg !1297

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1297
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1297
  store ptr %82, ptr %6, align 8, !dbg !1297
  %83 = load double, ptr %81, align 8, !dbg !1297
  %84 = fptrunc double %83 to float, !dbg !1297
  %85 = load i32, ptr %14, align 4, !dbg !1297
  %86 = sext i32 %85 to i64, !dbg !1297
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1297
  store float %84, ptr %87, align 8, !dbg !1297
  br label %103, !dbg !1297

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1297
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1297
  store ptr %90, ptr %6, align 8, !dbg !1297
  %91 = load double, ptr %89, align 8, !dbg !1297
  %92 = load i32, ptr %14, align 4, !dbg !1297
  %93 = sext i32 %92 to i64, !dbg !1297
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1297
  store double %91, ptr %94, align 8, !dbg !1297
  br label %103, !dbg !1297

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1297
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1297
  store ptr %97, ptr %6, align 8, !dbg !1297
  %98 = load ptr, ptr %96, align 8, !dbg !1297
  %99 = load i32, ptr %14, align 4, !dbg !1297
  %100 = sext i32 %99 to i64, !dbg !1297
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1297
  store ptr %98, ptr %101, align 8, !dbg !1297
  br label %103, !dbg !1297

102:                                              ; preds = %27
  br label %103, !dbg !1297

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1294

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1299
  %106 = add nsw i32 %105, 1, !dbg !1299
  store i32 %106, ptr %14, align 4, !dbg !1299
  br label %23, !dbg !1299, !llvm.loop !1300

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1283
  %109 = load ptr, ptr %108, align 8, !dbg !1283
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 69, !dbg !1283
  %111 = load ptr, ptr %110, align 8, !dbg !1283
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1283
  %113 = load ptr, ptr %7, align 8, !dbg !1283
  %114 = load ptr, ptr %8, align 8, !dbg !1283
  %115 = load ptr, ptr %9, align 8, !dbg !1283
  %116 = load ptr, ptr %10, align 8, !dbg !1283
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1283
  ret i8 %117, !dbg !1283
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1301 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1302, metadata !DIExpression()), !dbg !1303
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1304, metadata !DIExpression()), !dbg !1303
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1305, metadata !DIExpression()), !dbg !1303
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1306, metadata !DIExpression()), !dbg !1303
  call void @llvm.va_start(ptr %7), !dbg !1303
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1307, metadata !DIExpression()), !dbg !1303
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1308, metadata !DIExpression()), !dbg !1303
  %13 = load ptr, ptr %6, align 8, !dbg !1303
  %14 = load ptr, ptr %13, align 8, !dbg !1303
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1303
  %16 = load ptr, ptr %15, align 8, !dbg !1303
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1303
  %18 = load ptr, ptr %4, align 8, !dbg !1303
  %19 = load ptr, ptr %6, align 8, !dbg !1303
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1303
  store i32 %20, ptr %9, align 4, !dbg !1303
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1309, metadata !DIExpression()), !dbg !1303
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1310, metadata !DIExpression()), !dbg !1312
  store i32 0, ptr %11, align 4, !dbg !1312
  br label %21, !dbg !1312

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1312
  %23 = load i32, ptr %9, align 4, !dbg !1312
  %24 = icmp slt i32 %22, %23, !dbg !1312
  br i1 %24, label %25, label %105, !dbg !1312

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1313
  %27 = sext i32 %26 to i64, !dbg !1313
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1313
  %29 = load i8, ptr %28, align 1, !dbg !1313
  %30 = sext i8 %29 to i32, !dbg !1313
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
  ], !dbg !1313

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1316
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1316
  store ptr %33, ptr %7, align 8, !dbg !1316
  %34 = load i32, ptr %32, align 8, !dbg !1316
  %35 = trunc i32 %34 to i8, !dbg !1316
  %36 = load i32, ptr %11, align 4, !dbg !1316
  %37 = sext i32 %36 to i64, !dbg !1316
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1316
  store i8 %35, ptr %38, align 8, !dbg !1316
  br label %101, !dbg !1316

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1316
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1316
  store ptr %41, ptr %7, align 8, !dbg !1316
  %42 = load i32, ptr %40, align 8, !dbg !1316
  %43 = trunc i32 %42 to i8, !dbg !1316
  %44 = load i32, ptr %11, align 4, !dbg !1316
  %45 = sext i32 %44 to i64, !dbg !1316
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1316
  store i8 %43, ptr %46, align 8, !dbg !1316
  br label %101, !dbg !1316

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1316
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1316
  store ptr %49, ptr %7, align 8, !dbg !1316
  %50 = load i32, ptr %48, align 8, !dbg !1316
  %51 = trunc i32 %50 to i16, !dbg !1316
  %52 = load i32, ptr %11, align 4, !dbg !1316
  %53 = sext i32 %52 to i64, !dbg !1316
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1316
  store i16 %51, ptr %54, align 8, !dbg !1316
  br label %101, !dbg !1316

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1316
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1316
  store ptr %57, ptr %7, align 8, !dbg !1316
  %58 = load i32, ptr %56, align 8, !dbg !1316
  %59 = trunc i32 %58 to i16, !dbg !1316
  %60 = load i32, ptr %11, align 4, !dbg !1316
  %61 = sext i32 %60 to i64, !dbg !1316
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1316
  store i16 %59, ptr %62, align 8, !dbg !1316
  br label %101, !dbg !1316

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1316
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1316
  store ptr %65, ptr %7, align 8, !dbg !1316
  %66 = load i32, ptr %64, align 8, !dbg !1316
  %67 = load i32, ptr %11, align 4, !dbg !1316
  %68 = sext i32 %67 to i64, !dbg !1316
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1316
  store i32 %66, ptr %69, align 8, !dbg !1316
  br label %101, !dbg !1316

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1316
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1316
  store ptr %72, ptr %7, align 8, !dbg !1316
  %73 = load i32, ptr %71, align 8, !dbg !1316
  %74 = sext i32 %73 to i64, !dbg !1316
  %75 = load i32, ptr %11, align 4, !dbg !1316
  %76 = sext i32 %75 to i64, !dbg !1316
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1316
  store i64 %74, ptr %77, align 8, !dbg !1316
  br label %101, !dbg !1316

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1316
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1316
  store ptr %80, ptr %7, align 8, !dbg !1316
  %81 = load double, ptr %79, align 8, !dbg !1316
  %82 = fptrunc double %81 to float, !dbg !1316
  %83 = load i32, ptr %11, align 4, !dbg !1316
  %84 = sext i32 %83 to i64, !dbg !1316
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1316
  store float %82, ptr %85, align 8, !dbg !1316
  br label %101, !dbg !1316

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1316
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1316
  store ptr %88, ptr %7, align 8, !dbg !1316
  %89 = load double, ptr %87, align 8, !dbg !1316
  %90 = load i32, ptr %11, align 4, !dbg !1316
  %91 = sext i32 %90 to i64, !dbg !1316
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1316
  store double %89, ptr %92, align 8, !dbg !1316
  br label %101, !dbg !1316

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1316
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1316
  store ptr %95, ptr %7, align 8, !dbg !1316
  %96 = load ptr, ptr %94, align 8, !dbg !1316
  %97 = load i32, ptr %11, align 4, !dbg !1316
  %98 = sext i32 %97 to i64, !dbg !1316
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1316
  store ptr %96, ptr %99, align 8, !dbg !1316
  br label %101, !dbg !1316

100:                                              ; preds = %25
  br label %101, !dbg !1316

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1313

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1318
  %104 = add nsw i32 %103, 1, !dbg !1318
  store i32 %104, ptr %11, align 4, !dbg !1318
  br label %21, !dbg !1318, !llvm.loop !1319

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1320, metadata !DIExpression()), !dbg !1303
  %106 = load ptr, ptr %6, align 8, !dbg !1303
  %107 = load ptr, ptr %106, align 8, !dbg !1303
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 119, !dbg !1303
  %109 = load ptr, ptr %108, align 8, !dbg !1303
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1303
  %111 = load ptr, ptr %4, align 8, !dbg !1303
  %112 = load ptr, ptr %5, align 8, !dbg !1303
  %113 = load ptr, ptr %6, align 8, !dbg !1303
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1303
  store i8 %114, ptr %12, align 1, !dbg !1303
  call void @llvm.va_end(ptr %7), !dbg !1303
  %115 = load i8, ptr %12, align 1, !dbg !1303
  ret i8 %115, !dbg !1303
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1321 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1322, metadata !DIExpression()), !dbg !1323
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1324, metadata !DIExpression()), !dbg !1323
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1325, metadata !DIExpression()), !dbg !1323
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1326, metadata !DIExpression()), !dbg !1323
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1327, metadata !DIExpression()), !dbg !1323
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1328, metadata !DIExpression()), !dbg !1323
  %13 = load ptr, ptr %8, align 8, !dbg !1323
  %14 = load ptr, ptr %13, align 8, !dbg !1323
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1323
  %16 = load ptr, ptr %15, align 8, !dbg !1323
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1323
  %18 = load ptr, ptr %6, align 8, !dbg !1323
  %19 = load ptr, ptr %8, align 8, !dbg !1323
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1323
  store i32 %20, ptr %10, align 4, !dbg !1323
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1329, metadata !DIExpression()), !dbg !1323
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1330, metadata !DIExpression()), !dbg !1332
  store i32 0, ptr %12, align 4, !dbg !1332
  br label %21, !dbg !1332

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1332
  %23 = load i32, ptr %10, align 4, !dbg !1332
  %24 = icmp slt i32 %22, %23, !dbg !1332
  br i1 %24, label %25, label %105, !dbg !1332

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1333
  %27 = sext i32 %26 to i64, !dbg !1333
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1333
  %29 = load i8, ptr %28, align 1, !dbg !1333
  %30 = sext i8 %29 to i32, !dbg !1333
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
  ], !dbg !1333

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1336
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1336
  store ptr %33, ptr %5, align 8, !dbg !1336
  %34 = load i32, ptr %32, align 8, !dbg !1336
  %35 = trunc i32 %34 to i8, !dbg !1336
  %36 = load i32, ptr %12, align 4, !dbg !1336
  %37 = sext i32 %36 to i64, !dbg !1336
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1336
  store i8 %35, ptr %38, align 8, !dbg !1336
  br label %101, !dbg !1336

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1336
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1336
  store ptr %41, ptr %5, align 8, !dbg !1336
  %42 = load i32, ptr %40, align 8, !dbg !1336
  %43 = trunc i32 %42 to i8, !dbg !1336
  %44 = load i32, ptr %12, align 4, !dbg !1336
  %45 = sext i32 %44 to i64, !dbg !1336
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1336
  store i8 %43, ptr %46, align 8, !dbg !1336
  br label %101, !dbg !1336

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1336
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1336
  store ptr %49, ptr %5, align 8, !dbg !1336
  %50 = load i32, ptr %48, align 8, !dbg !1336
  %51 = trunc i32 %50 to i16, !dbg !1336
  %52 = load i32, ptr %12, align 4, !dbg !1336
  %53 = sext i32 %52 to i64, !dbg !1336
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1336
  store i16 %51, ptr %54, align 8, !dbg !1336
  br label %101, !dbg !1336

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1336
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1336
  store ptr %57, ptr %5, align 8, !dbg !1336
  %58 = load i32, ptr %56, align 8, !dbg !1336
  %59 = trunc i32 %58 to i16, !dbg !1336
  %60 = load i32, ptr %12, align 4, !dbg !1336
  %61 = sext i32 %60 to i64, !dbg !1336
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1336
  store i16 %59, ptr %62, align 8, !dbg !1336
  br label %101, !dbg !1336

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1336
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1336
  store ptr %65, ptr %5, align 8, !dbg !1336
  %66 = load i32, ptr %64, align 8, !dbg !1336
  %67 = load i32, ptr %12, align 4, !dbg !1336
  %68 = sext i32 %67 to i64, !dbg !1336
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1336
  store i32 %66, ptr %69, align 8, !dbg !1336
  br label %101, !dbg !1336

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1336
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1336
  store ptr %72, ptr %5, align 8, !dbg !1336
  %73 = load i32, ptr %71, align 8, !dbg !1336
  %74 = sext i32 %73 to i64, !dbg !1336
  %75 = load i32, ptr %12, align 4, !dbg !1336
  %76 = sext i32 %75 to i64, !dbg !1336
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1336
  store i64 %74, ptr %77, align 8, !dbg !1336
  br label %101, !dbg !1336

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1336
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1336
  store ptr %80, ptr %5, align 8, !dbg !1336
  %81 = load double, ptr %79, align 8, !dbg !1336
  %82 = fptrunc double %81 to float, !dbg !1336
  %83 = load i32, ptr %12, align 4, !dbg !1336
  %84 = sext i32 %83 to i64, !dbg !1336
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1336
  store float %82, ptr %85, align 8, !dbg !1336
  br label %101, !dbg !1336

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1336
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1336
  store ptr %88, ptr %5, align 8, !dbg !1336
  %89 = load double, ptr %87, align 8, !dbg !1336
  %90 = load i32, ptr %12, align 4, !dbg !1336
  %91 = sext i32 %90 to i64, !dbg !1336
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1336
  store double %89, ptr %92, align 8, !dbg !1336
  br label %101, !dbg !1336

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1336
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1336
  store ptr %95, ptr %5, align 8, !dbg !1336
  %96 = load ptr, ptr %94, align 8, !dbg !1336
  %97 = load i32, ptr %12, align 4, !dbg !1336
  %98 = sext i32 %97 to i64, !dbg !1336
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1336
  store ptr %96, ptr %99, align 8, !dbg !1336
  br label %101, !dbg !1336

100:                                              ; preds = %25
  br label %101, !dbg !1336

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1333

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1338
  %104 = add nsw i32 %103, 1, !dbg !1338
  store i32 %104, ptr %12, align 4, !dbg !1338
  br label %21, !dbg !1338, !llvm.loop !1339

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1323
  %107 = load ptr, ptr %106, align 8, !dbg !1323
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 119, !dbg !1323
  %109 = load ptr, ptr %108, align 8, !dbg !1323
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1323
  %111 = load ptr, ptr %6, align 8, !dbg !1323
  %112 = load ptr, ptr %7, align 8, !dbg !1323
  %113 = load ptr, ptr %8, align 8, !dbg !1323
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1323
  ret i8 %114, !dbg !1323
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1340 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1341, metadata !DIExpression()), !dbg !1342
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1343, metadata !DIExpression()), !dbg !1342
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1344, metadata !DIExpression()), !dbg !1342
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1345, metadata !DIExpression()), !dbg !1342
  call void @llvm.va_start(ptr %7), !dbg !1342
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1346, metadata !DIExpression()), !dbg !1342
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1347, metadata !DIExpression()), !dbg !1342
  %13 = load ptr, ptr %6, align 8, !dbg !1342
  %14 = load ptr, ptr %13, align 8, !dbg !1342
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1342
  %16 = load ptr, ptr %15, align 8, !dbg !1342
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1342
  %18 = load ptr, ptr %4, align 8, !dbg !1342
  %19 = load ptr, ptr %6, align 8, !dbg !1342
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1342
  store i32 %20, ptr %9, align 4, !dbg !1342
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1348, metadata !DIExpression()), !dbg !1342
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1349, metadata !DIExpression()), !dbg !1351
  store i32 0, ptr %11, align 4, !dbg !1351
  br label %21, !dbg !1351

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1351
  %23 = load i32, ptr %9, align 4, !dbg !1351
  %24 = icmp slt i32 %22, %23, !dbg !1351
  br i1 %24, label %25, label %105, !dbg !1351

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1352
  %27 = sext i32 %26 to i64, !dbg !1352
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1352
  %29 = load i8, ptr %28, align 1, !dbg !1352
  %30 = sext i8 %29 to i32, !dbg !1352
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
  ], !dbg !1352

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1355
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1355
  store ptr %33, ptr %7, align 8, !dbg !1355
  %34 = load i32, ptr %32, align 8, !dbg !1355
  %35 = trunc i32 %34 to i8, !dbg !1355
  %36 = load i32, ptr %11, align 4, !dbg !1355
  %37 = sext i32 %36 to i64, !dbg !1355
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1355
  store i8 %35, ptr %38, align 8, !dbg !1355
  br label %101, !dbg !1355

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1355
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1355
  store ptr %41, ptr %7, align 8, !dbg !1355
  %42 = load i32, ptr %40, align 8, !dbg !1355
  %43 = trunc i32 %42 to i8, !dbg !1355
  %44 = load i32, ptr %11, align 4, !dbg !1355
  %45 = sext i32 %44 to i64, !dbg !1355
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1355
  store i8 %43, ptr %46, align 8, !dbg !1355
  br label %101, !dbg !1355

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1355
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1355
  store ptr %49, ptr %7, align 8, !dbg !1355
  %50 = load i32, ptr %48, align 8, !dbg !1355
  %51 = trunc i32 %50 to i16, !dbg !1355
  %52 = load i32, ptr %11, align 4, !dbg !1355
  %53 = sext i32 %52 to i64, !dbg !1355
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1355
  store i16 %51, ptr %54, align 8, !dbg !1355
  br label %101, !dbg !1355

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1355
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1355
  store ptr %57, ptr %7, align 8, !dbg !1355
  %58 = load i32, ptr %56, align 8, !dbg !1355
  %59 = trunc i32 %58 to i16, !dbg !1355
  %60 = load i32, ptr %11, align 4, !dbg !1355
  %61 = sext i32 %60 to i64, !dbg !1355
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1355
  store i16 %59, ptr %62, align 8, !dbg !1355
  br label %101, !dbg !1355

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1355
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1355
  store ptr %65, ptr %7, align 8, !dbg !1355
  %66 = load i32, ptr %64, align 8, !dbg !1355
  %67 = load i32, ptr %11, align 4, !dbg !1355
  %68 = sext i32 %67 to i64, !dbg !1355
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1355
  store i32 %66, ptr %69, align 8, !dbg !1355
  br label %101, !dbg !1355

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1355
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1355
  store ptr %72, ptr %7, align 8, !dbg !1355
  %73 = load i32, ptr %71, align 8, !dbg !1355
  %74 = sext i32 %73 to i64, !dbg !1355
  %75 = load i32, ptr %11, align 4, !dbg !1355
  %76 = sext i32 %75 to i64, !dbg !1355
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1355
  store i64 %74, ptr %77, align 8, !dbg !1355
  br label %101, !dbg !1355

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1355
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1355
  store ptr %80, ptr %7, align 8, !dbg !1355
  %81 = load double, ptr %79, align 8, !dbg !1355
  %82 = fptrunc double %81 to float, !dbg !1355
  %83 = load i32, ptr %11, align 4, !dbg !1355
  %84 = sext i32 %83 to i64, !dbg !1355
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1355
  store float %82, ptr %85, align 8, !dbg !1355
  br label %101, !dbg !1355

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1355
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1355
  store ptr %88, ptr %7, align 8, !dbg !1355
  %89 = load double, ptr %87, align 8, !dbg !1355
  %90 = load i32, ptr %11, align 4, !dbg !1355
  %91 = sext i32 %90 to i64, !dbg !1355
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1355
  store double %89, ptr %92, align 8, !dbg !1355
  br label %101, !dbg !1355

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1355
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1355
  store ptr %95, ptr %7, align 8, !dbg !1355
  %96 = load ptr, ptr %94, align 8, !dbg !1355
  %97 = load i32, ptr %11, align 4, !dbg !1355
  %98 = sext i32 %97 to i64, !dbg !1355
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1355
  store ptr %96, ptr %99, align 8, !dbg !1355
  br label %101, !dbg !1355

100:                                              ; preds = %25
  br label %101, !dbg !1355

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1352

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1357
  %104 = add nsw i32 %103, 1, !dbg !1357
  store i32 %104, ptr %11, align 4, !dbg !1357
  br label %21, !dbg !1357, !llvm.loop !1358

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1359, metadata !DIExpression()), !dbg !1342
  %106 = load ptr, ptr %6, align 8, !dbg !1342
  %107 = load ptr, ptr %106, align 8, !dbg !1342
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 42, !dbg !1342
  %109 = load ptr, ptr %108, align 8, !dbg !1342
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1342
  %111 = load ptr, ptr %4, align 8, !dbg !1342
  %112 = load ptr, ptr %5, align 8, !dbg !1342
  %113 = load ptr, ptr %6, align 8, !dbg !1342
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1342
  store i8 %114, ptr %12, align 1, !dbg !1342
  call void @llvm.va_end(ptr %7), !dbg !1342
  %115 = load i8, ptr %12, align 1, !dbg !1342
  ret i8 %115, !dbg !1342
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1360 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1361, metadata !DIExpression()), !dbg !1362
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1363, metadata !DIExpression()), !dbg !1362
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1364, metadata !DIExpression()), !dbg !1362
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1365, metadata !DIExpression()), !dbg !1362
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1366, metadata !DIExpression()), !dbg !1362
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1367, metadata !DIExpression()), !dbg !1362
  %13 = load ptr, ptr %8, align 8, !dbg !1362
  %14 = load ptr, ptr %13, align 8, !dbg !1362
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1362
  %16 = load ptr, ptr %15, align 8, !dbg !1362
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1362
  %18 = load ptr, ptr %6, align 8, !dbg !1362
  %19 = load ptr, ptr %8, align 8, !dbg !1362
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1362
  store i32 %20, ptr %10, align 4, !dbg !1362
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1368, metadata !DIExpression()), !dbg !1362
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1369, metadata !DIExpression()), !dbg !1371
  store i32 0, ptr %12, align 4, !dbg !1371
  br label %21, !dbg !1371

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1371
  %23 = load i32, ptr %10, align 4, !dbg !1371
  %24 = icmp slt i32 %22, %23, !dbg !1371
  br i1 %24, label %25, label %105, !dbg !1371

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1372
  %27 = sext i32 %26 to i64, !dbg !1372
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1372
  %29 = load i8, ptr %28, align 1, !dbg !1372
  %30 = sext i8 %29 to i32, !dbg !1372
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
  ], !dbg !1372

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1375
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1375
  store ptr %33, ptr %5, align 8, !dbg !1375
  %34 = load i32, ptr %32, align 8, !dbg !1375
  %35 = trunc i32 %34 to i8, !dbg !1375
  %36 = load i32, ptr %12, align 4, !dbg !1375
  %37 = sext i32 %36 to i64, !dbg !1375
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1375
  store i8 %35, ptr %38, align 8, !dbg !1375
  br label %101, !dbg !1375

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1375
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1375
  store ptr %41, ptr %5, align 8, !dbg !1375
  %42 = load i32, ptr %40, align 8, !dbg !1375
  %43 = trunc i32 %42 to i8, !dbg !1375
  %44 = load i32, ptr %12, align 4, !dbg !1375
  %45 = sext i32 %44 to i64, !dbg !1375
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1375
  store i8 %43, ptr %46, align 8, !dbg !1375
  br label %101, !dbg !1375

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1375
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1375
  store ptr %49, ptr %5, align 8, !dbg !1375
  %50 = load i32, ptr %48, align 8, !dbg !1375
  %51 = trunc i32 %50 to i16, !dbg !1375
  %52 = load i32, ptr %12, align 4, !dbg !1375
  %53 = sext i32 %52 to i64, !dbg !1375
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1375
  store i16 %51, ptr %54, align 8, !dbg !1375
  br label %101, !dbg !1375

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1375
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1375
  store ptr %57, ptr %5, align 8, !dbg !1375
  %58 = load i32, ptr %56, align 8, !dbg !1375
  %59 = trunc i32 %58 to i16, !dbg !1375
  %60 = load i32, ptr %12, align 4, !dbg !1375
  %61 = sext i32 %60 to i64, !dbg !1375
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1375
  store i16 %59, ptr %62, align 8, !dbg !1375
  br label %101, !dbg !1375

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1375
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1375
  store ptr %65, ptr %5, align 8, !dbg !1375
  %66 = load i32, ptr %64, align 8, !dbg !1375
  %67 = load i32, ptr %12, align 4, !dbg !1375
  %68 = sext i32 %67 to i64, !dbg !1375
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1375
  store i32 %66, ptr %69, align 8, !dbg !1375
  br label %101, !dbg !1375

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1375
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1375
  store ptr %72, ptr %5, align 8, !dbg !1375
  %73 = load i32, ptr %71, align 8, !dbg !1375
  %74 = sext i32 %73 to i64, !dbg !1375
  %75 = load i32, ptr %12, align 4, !dbg !1375
  %76 = sext i32 %75 to i64, !dbg !1375
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1375
  store i64 %74, ptr %77, align 8, !dbg !1375
  br label %101, !dbg !1375

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1375
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1375
  store ptr %80, ptr %5, align 8, !dbg !1375
  %81 = load double, ptr %79, align 8, !dbg !1375
  %82 = fptrunc double %81 to float, !dbg !1375
  %83 = load i32, ptr %12, align 4, !dbg !1375
  %84 = sext i32 %83 to i64, !dbg !1375
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1375
  store float %82, ptr %85, align 8, !dbg !1375
  br label %101, !dbg !1375

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1375
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1375
  store ptr %88, ptr %5, align 8, !dbg !1375
  %89 = load double, ptr %87, align 8, !dbg !1375
  %90 = load i32, ptr %12, align 4, !dbg !1375
  %91 = sext i32 %90 to i64, !dbg !1375
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1375
  store double %89, ptr %92, align 8, !dbg !1375
  br label %101, !dbg !1375

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1375
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1375
  store ptr %95, ptr %5, align 8, !dbg !1375
  %96 = load ptr, ptr %94, align 8, !dbg !1375
  %97 = load i32, ptr %12, align 4, !dbg !1375
  %98 = sext i32 %97 to i64, !dbg !1375
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1375
  store ptr %96, ptr %99, align 8, !dbg !1375
  br label %101, !dbg !1375

100:                                              ; preds = %25
  br label %101, !dbg !1375

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1372

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1377
  %104 = add nsw i32 %103, 1, !dbg !1377
  store i32 %104, ptr %12, align 4, !dbg !1377
  br label %21, !dbg !1377, !llvm.loop !1378

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1362
  %107 = load ptr, ptr %106, align 8, !dbg !1362
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 42, !dbg !1362
  %109 = load ptr, ptr %108, align 8, !dbg !1362
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1362
  %111 = load ptr, ptr %6, align 8, !dbg !1362
  %112 = load ptr, ptr %7, align 8, !dbg !1362
  %113 = load ptr, ptr %8, align 8, !dbg !1362
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1362
  ret i8 %114, !dbg !1362
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1379 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i8, align 1
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1380, metadata !DIExpression()), !dbg !1381
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1382, metadata !DIExpression()), !dbg !1381
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1383, metadata !DIExpression()), !dbg !1381
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1384, metadata !DIExpression()), !dbg !1381
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1385, metadata !DIExpression()), !dbg !1381
  call void @llvm.va_start(ptr %9), !dbg !1381
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1386, metadata !DIExpression()), !dbg !1381
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1387, metadata !DIExpression()), !dbg !1381
  %15 = load ptr, ptr %8, align 8, !dbg !1381
  %16 = load ptr, ptr %15, align 8, !dbg !1381
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1381
  %18 = load ptr, ptr %17, align 8, !dbg !1381
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1381
  %20 = load ptr, ptr %5, align 8, !dbg !1381
  %21 = load ptr, ptr %8, align 8, !dbg !1381
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1381
  store i32 %22, ptr %11, align 4, !dbg !1381
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1388, metadata !DIExpression()), !dbg !1381
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1389, metadata !DIExpression()), !dbg !1391
  store i32 0, ptr %13, align 4, !dbg !1391
  br label %23, !dbg !1391

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1391
  %25 = load i32, ptr %11, align 4, !dbg !1391
  %26 = icmp slt i32 %24, %25, !dbg !1391
  br i1 %26, label %27, label %107, !dbg !1391

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1392
  %29 = sext i32 %28 to i64, !dbg !1392
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1392
  %31 = load i8, ptr %30, align 1, !dbg !1392
  %32 = sext i8 %31 to i32, !dbg !1392
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
  ], !dbg !1392

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1395
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1395
  store ptr %35, ptr %9, align 8, !dbg !1395
  %36 = load i32, ptr %34, align 8, !dbg !1395
  %37 = trunc i32 %36 to i8, !dbg !1395
  %38 = load i32, ptr %13, align 4, !dbg !1395
  %39 = sext i32 %38 to i64, !dbg !1395
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1395
  store i8 %37, ptr %40, align 8, !dbg !1395
  br label %103, !dbg !1395

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1395
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1395
  store ptr %43, ptr %9, align 8, !dbg !1395
  %44 = load i32, ptr %42, align 8, !dbg !1395
  %45 = trunc i32 %44 to i8, !dbg !1395
  %46 = load i32, ptr %13, align 4, !dbg !1395
  %47 = sext i32 %46 to i64, !dbg !1395
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1395
  store i8 %45, ptr %48, align 8, !dbg !1395
  br label %103, !dbg !1395

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1395
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1395
  store ptr %51, ptr %9, align 8, !dbg !1395
  %52 = load i32, ptr %50, align 8, !dbg !1395
  %53 = trunc i32 %52 to i16, !dbg !1395
  %54 = load i32, ptr %13, align 4, !dbg !1395
  %55 = sext i32 %54 to i64, !dbg !1395
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1395
  store i16 %53, ptr %56, align 8, !dbg !1395
  br label %103, !dbg !1395

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1395
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1395
  store ptr %59, ptr %9, align 8, !dbg !1395
  %60 = load i32, ptr %58, align 8, !dbg !1395
  %61 = trunc i32 %60 to i16, !dbg !1395
  %62 = load i32, ptr %13, align 4, !dbg !1395
  %63 = sext i32 %62 to i64, !dbg !1395
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1395
  store i16 %61, ptr %64, align 8, !dbg !1395
  br label %103, !dbg !1395

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1395
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1395
  store ptr %67, ptr %9, align 8, !dbg !1395
  %68 = load i32, ptr %66, align 8, !dbg !1395
  %69 = load i32, ptr %13, align 4, !dbg !1395
  %70 = sext i32 %69 to i64, !dbg !1395
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1395
  store i32 %68, ptr %71, align 8, !dbg !1395
  br label %103, !dbg !1395

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1395
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1395
  store ptr %74, ptr %9, align 8, !dbg !1395
  %75 = load i32, ptr %73, align 8, !dbg !1395
  %76 = sext i32 %75 to i64, !dbg !1395
  %77 = load i32, ptr %13, align 4, !dbg !1395
  %78 = sext i32 %77 to i64, !dbg !1395
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1395
  store i64 %76, ptr %79, align 8, !dbg !1395
  br label %103, !dbg !1395

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1395
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1395
  store ptr %82, ptr %9, align 8, !dbg !1395
  %83 = load double, ptr %81, align 8, !dbg !1395
  %84 = fptrunc double %83 to float, !dbg !1395
  %85 = load i32, ptr %13, align 4, !dbg !1395
  %86 = sext i32 %85 to i64, !dbg !1395
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1395
  store float %84, ptr %87, align 8, !dbg !1395
  br label %103, !dbg !1395

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1395
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1395
  store ptr %90, ptr %9, align 8, !dbg !1395
  %91 = load double, ptr %89, align 8, !dbg !1395
  %92 = load i32, ptr %13, align 4, !dbg !1395
  %93 = sext i32 %92 to i64, !dbg !1395
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1395
  store double %91, ptr %94, align 8, !dbg !1395
  br label %103, !dbg !1395

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1395
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1395
  store ptr %97, ptr %9, align 8, !dbg !1395
  %98 = load ptr, ptr %96, align 8, !dbg !1395
  %99 = load i32, ptr %13, align 4, !dbg !1395
  %100 = sext i32 %99 to i64, !dbg !1395
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1395
  store ptr %98, ptr %101, align 8, !dbg !1395
  br label %103, !dbg !1395

102:                                              ; preds = %27
  br label %103, !dbg !1395

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1392

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1397
  %106 = add nsw i32 %105, 1, !dbg !1397
  store i32 %106, ptr %13, align 4, !dbg !1397
  br label %23, !dbg !1397, !llvm.loop !1398

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1399, metadata !DIExpression()), !dbg !1381
  %108 = load ptr, ptr %8, align 8, !dbg !1381
  %109 = load ptr, ptr %108, align 8, !dbg !1381
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 72, !dbg !1381
  %111 = load ptr, ptr %110, align 8, !dbg !1381
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1381
  %113 = load ptr, ptr %5, align 8, !dbg !1381
  %114 = load ptr, ptr %6, align 8, !dbg !1381
  %115 = load ptr, ptr %7, align 8, !dbg !1381
  %116 = load ptr, ptr %8, align 8, !dbg !1381
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1381
  store i8 %117, ptr %14, align 1, !dbg !1381
  call void @llvm.va_end(ptr %9), !dbg !1381
  %118 = load i8, ptr %14, align 1, !dbg !1381
  ret i8 %118, !dbg !1381
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1400 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1401, metadata !DIExpression()), !dbg !1402
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1403, metadata !DIExpression()), !dbg !1402
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1404, metadata !DIExpression()), !dbg !1402
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1405, metadata !DIExpression()), !dbg !1402
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1406, metadata !DIExpression()), !dbg !1402
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1407, metadata !DIExpression()), !dbg !1402
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1408, metadata !DIExpression()), !dbg !1402
  %15 = load ptr, ptr %10, align 8, !dbg !1402
  %16 = load ptr, ptr %15, align 8, !dbg !1402
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1402
  %18 = load ptr, ptr %17, align 8, !dbg !1402
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1402
  %20 = load ptr, ptr %7, align 8, !dbg !1402
  %21 = load ptr, ptr %10, align 8, !dbg !1402
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1402
  store i32 %22, ptr %12, align 4, !dbg !1402
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1409, metadata !DIExpression()), !dbg !1402
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1410, metadata !DIExpression()), !dbg !1412
  store i32 0, ptr %14, align 4, !dbg !1412
  br label %23, !dbg !1412

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1412
  %25 = load i32, ptr %12, align 4, !dbg !1412
  %26 = icmp slt i32 %24, %25, !dbg !1412
  br i1 %26, label %27, label %107, !dbg !1412

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1413
  %29 = sext i32 %28 to i64, !dbg !1413
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1413
  %31 = load i8, ptr %30, align 1, !dbg !1413
  %32 = sext i8 %31 to i32, !dbg !1413
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
  ], !dbg !1413

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1416
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1416
  store ptr %35, ptr %6, align 8, !dbg !1416
  %36 = load i32, ptr %34, align 8, !dbg !1416
  %37 = trunc i32 %36 to i8, !dbg !1416
  %38 = load i32, ptr %14, align 4, !dbg !1416
  %39 = sext i32 %38 to i64, !dbg !1416
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1416
  store i8 %37, ptr %40, align 8, !dbg !1416
  br label %103, !dbg !1416

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1416
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1416
  store ptr %43, ptr %6, align 8, !dbg !1416
  %44 = load i32, ptr %42, align 8, !dbg !1416
  %45 = trunc i32 %44 to i8, !dbg !1416
  %46 = load i32, ptr %14, align 4, !dbg !1416
  %47 = sext i32 %46 to i64, !dbg !1416
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1416
  store i8 %45, ptr %48, align 8, !dbg !1416
  br label %103, !dbg !1416

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1416
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1416
  store ptr %51, ptr %6, align 8, !dbg !1416
  %52 = load i32, ptr %50, align 8, !dbg !1416
  %53 = trunc i32 %52 to i16, !dbg !1416
  %54 = load i32, ptr %14, align 4, !dbg !1416
  %55 = sext i32 %54 to i64, !dbg !1416
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1416
  store i16 %53, ptr %56, align 8, !dbg !1416
  br label %103, !dbg !1416

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1416
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1416
  store ptr %59, ptr %6, align 8, !dbg !1416
  %60 = load i32, ptr %58, align 8, !dbg !1416
  %61 = trunc i32 %60 to i16, !dbg !1416
  %62 = load i32, ptr %14, align 4, !dbg !1416
  %63 = sext i32 %62 to i64, !dbg !1416
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1416
  store i16 %61, ptr %64, align 8, !dbg !1416
  br label %103, !dbg !1416

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1416
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1416
  store ptr %67, ptr %6, align 8, !dbg !1416
  %68 = load i32, ptr %66, align 8, !dbg !1416
  %69 = load i32, ptr %14, align 4, !dbg !1416
  %70 = sext i32 %69 to i64, !dbg !1416
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1416
  store i32 %68, ptr %71, align 8, !dbg !1416
  br label %103, !dbg !1416

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1416
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1416
  store ptr %74, ptr %6, align 8, !dbg !1416
  %75 = load i32, ptr %73, align 8, !dbg !1416
  %76 = sext i32 %75 to i64, !dbg !1416
  %77 = load i32, ptr %14, align 4, !dbg !1416
  %78 = sext i32 %77 to i64, !dbg !1416
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1416
  store i64 %76, ptr %79, align 8, !dbg !1416
  br label %103, !dbg !1416

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1416
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1416
  store ptr %82, ptr %6, align 8, !dbg !1416
  %83 = load double, ptr %81, align 8, !dbg !1416
  %84 = fptrunc double %83 to float, !dbg !1416
  %85 = load i32, ptr %14, align 4, !dbg !1416
  %86 = sext i32 %85 to i64, !dbg !1416
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1416
  store float %84, ptr %87, align 8, !dbg !1416
  br label %103, !dbg !1416

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1416
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1416
  store ptr %90, ptr %6, align 8, !dbg !1416
  %91 = load double, ptr %89, align 8, !dbg !1416
  %92 = load i32, ptr %14, align 4, !dbg !1416
  %93 = sext i32 %92 to i64, !dbg !1416
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1416
  store double %91, ptr %94, align 8, !dbg !1416
  br label %103, !dbg !1416

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1416
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1416
  store ptr %97, ptr %6, align 8, !dbg !1416
  %98 = load ptr, ptr %96, align 8, !dbg !1416
  %99 = load i32, ptr %14, align 4, !dbg !1416
  %100 = sext i32 %99 to i64, !dbg !1416
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1416
  store ptr %98, ptr %101, align 8, !dbg !1416
  br label %103, !dbg !1416

102:                                              ; preds = %27
  br label %103, !dbg !1416

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1413

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1418
  %106 = add nsw i32 %105, 1, !dbg !1418
  store i32 %106, ptr %14, align 4, !dbg !1418
  br label %23, !dbg !1418, !llvm.loop !1419

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1402
  %109 = load ptr, ptr %108, align 8, !dbg !1402
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 72, !dbg !1402
  %111 = load ptr, ptr %110, align 8, !dbg !1402
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1402
  %113 = load ptr, ptr %7, align 8, !dbg !1402
  %114 = load ptr, ptr %8, align 8, !dbg !1402
  %115 = load ptr, ptr %9, align 8, !dbg !1402
  %116 = load ptr, ptr %10, align 8, !dbg !1402
  %117 = call i8 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1402
  ret i8 %117, !dbg !1402
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1420 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i8, align 1
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1421, metadata !DIExpression()), !dbg !1422
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1423, metadata !DIExpression()), !dbg !1422
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1424, metadata !DIExpression()), !dbg !1422
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1425, metadata !DIExpression()), !dbg !1422
  call void @llvm.va_start(ptr %7), !dbg !1422
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1426, metadata !DIExpression()), !dbg !1422
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1427, metadata !DIExpression()), !dbg !1422
  %13 = load ptr, ptr %6, align 8, !dbg !1422
  %14 = load ptr, ptr %13, align 8, !dbg !1422
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1422
  %16 = load ptr, ptr %15, align 8, !dbg !1422
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1422
  %18 = load ptr, ptr %4, align 8, !dbg !1422
  %19 = load ptr, ptr %6, align 8, !dbg !1422
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1422
  store i32 %20, ptr %9, align 4, !dbg !1422
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1428, metadata !DIExpression()), !dbg !1422
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1429, metadata !DIExpression()), !dbg !1431
  store i32 0, ptr %11, align 4, !dbg !1431
  br label %21, !dbg !1431

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1431
  %23 = load i32, ptr %9, align 4, !dbg !1431
  %24 = icmp slt i32 %22, %23, !dbg !1431
  br i1 %24, label %25, label %105, !dbg !1431

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1432
  %27 = sext i32 %26 to i64, !dbg !1432
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1432
  %29 = load i8, ptr %28, align 1, !dbg !1432
  %30 = sext i8 %29 to i32, !dbg !1432
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
  ], !dbg !1432

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1435
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1435
  store ptr %33, ptr %7, align 8, !dbg !1435
  %34 = load i32, ptr %32, align 8, !dbg !1435
  %35 = trunc i32 %34 to i8, !dbg !1435
  %36 = load i32, ptr %11, align 4, !dbg !1435
  %37 = sext i32 %36 to i64, !dbg !1435
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1435
  store i8 %35, ptr %38, align 8, !dbg !1435
  br label %101, !dbg !1435

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1435
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1435
  store ptr %41, ptr %7, align 8, !dbg !1435
  %42 = load i32, ptr %40, align 8, !dbg !1435
  %43 = trunc i32 %42 to i8, !dbg !1435
  %44 = load i32, ptr %11, align 4, !dbg !1435
  %45 = sext i32 %44 to i64, !dbg !1435
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1435
  store i8 %43, ptr %46, align 8, !dbg !1435
  br label %101, !dbg !1435

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1435
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1435
  store ptr %49, ptr %7, align 8, !dbg !1435
  %50 = load i32, ptr %48, align 8, !dbg !1435
  %51 = trunc i32 %50 to i16, !dbg !1435
  %52 = load i32, ptr %11, align 4, !dbg !1435
  %53 = sext i32 %52 to i64, !dbg !1435
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1435
  store i16 %51, ptr %54, align 8, !dbg !1435
  br label %101, !dbg !1435

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1435
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1435
  store ptr %57, ptr %7, align 8, !dbg !1435
  %58 = load i32, ptr %56, align 8, !dbg !1435
  %59 = trunc i32 %58 to i16, !dbg !1435
  %60 = load i32, ptr %11, align 4, !dbg !1435
  %61 = sext i32 %60 to i64, !dbg !1435
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1435
  store i16 %59, ptr %62, align 8, !dbg !1435
  br label %101, !dbg !1435

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1435
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1435
  store ptr %65, ptr %7, align 8, !dbg !1435
  %66 = load i32, ptr %64, align 8, !dbg !1435
  %67 = load i32, ptr %11, align 4, !dbg !1435
  %68 = sext i32 %67 to i64, !dbg !1435
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1435
  store i32 %66, ptr %69, align 8, !dbg !1435
  br label %101, !dbg !1435

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1435
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1435
  store ptr %72, ptr %7, align 8, !dbg !1435
  %73 = load i32, ptr %71, align 8, !dbg !1435
  %74 = sext i32 %73 to i64, !dbg !1435
  %75 = load i32, ptr %11, align 4, !dbg !1435
  %76 = sext i32 %75 to i64, !dbg !1435
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1435
  store i64 %74, ptr %77, align 8, !dbg !1435
  br label %101, !dbg !1435

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1435
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1435
  store ptr %80, ptr %7, align 8, !dbg !1435
  %81 = load double, ptr %79, align 8, !dbg !1435
  %82 = fptrunc double %81 to float, !dbg !1435
  %83 = load i32, ptr %11, align 4, !dbg !1435
  %84 = sext i32 %83 to i64, !dbg !1435
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1435
  store float %82, ptr %85, align 8, !dbg !1435
  br label %101, !dbg !1435

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1435
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1435
  store ptr %88, ptr %7, align 8, !dbg !1435
  %89 = load double, ptr %87, align 8, !dbg !1435
  %90 = load i32, ptr %11, align 4, !dbg !1435
  %91 = sext i32 %90 to i64, !dbg !1435
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1435
  store double %89, ptr %92, align 8, !dbg !1435
  br label %101, !dbg !1435

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1435
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1435
  store ptr %95, ptr %7, align 8, !dbg !1435
  %96 = load ptr, ptr %94, align 8, !dbg !1435
  %97 = load i32, ptr %11, align 4, !dbg !1435
  %98 = sext i32 %97 to i64, !dbg !1435
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1435
  store ptr %96, ptr %99, align 8, !dbg !1435
  br label %101, !dbg !1435

100:                                              ; preds = %25
  br label %101, !dbg !1435

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1432

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1437
  %104 = add nsw i32 %103, 1, !dbg !1437
  store i32 %104, ptr %11, align 4, !dbg !1437
  br label %21, !dbg !1437, !llvm.loop !1438

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1439, metadata !DIExpression()), !dbg !1422
  %106 = load ptr, ptr %6, align 8, !dbg !1422
  %107 = load ptr, ptr %106, align 8, !dbg !1422
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 122, !dbg !1422
  %109 = load ptr, ptr %108, align 8, !dbg !1422
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1422
  %111 = load ptr, ptr %4, align 8, !dbg !1422
  %112 = load ptr, ptr %5, align 8, !dbg !1422
  %113 = load ptr, ptr %6, align 8, !dbg !1422
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1422
  store i8 %114, ptr %12, align 1, !dbg !1422
  call void @llvm.va_end(ptr %7), !dbg !1422
  %115 = load i8, ptr %12, align 1, !dbg !1422
  ret i8 %115, !dbg !1422
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1440 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1441, metadata !DIExpression()), !dbg !1442
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1443, metadata !DIExpression()), !dbg !1442
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1444, metadata !DIExpression()), !dbg !1442
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1445, metadata !DIExpression()), !dbg !1442
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1446, metadata !DIExpression()), !dbg !1442
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1447, metadata !DIExpression()), !dbg !1442
  %13 = load ptr, ptr %8, align 8, !dbg !1442
  %14 = load ptr, ptr %13, align 8, !dbg !1442
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1442
  %16 = load ptr, ptr %15, align 8, !dbg !1442
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1442
  %18 = load ptr, ptr %6, align 8, !dbg !1442
  %19 = load ptr, ptr %8, align 8, !dbg !1442
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1442
  store i32 %20, ptr %10, align 4, !dbg !1442
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1448, metadata !DIExpression()), !dbg !1442
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1449, metadata !DIExpression()), !dbg !1451
  store i32 0, ptr %12, align 4, !dbg !1451
  br label %21, !dbg !1451

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1451
  %23 = load i32, ptr %10, align 4, !dbg !1451
  %24 = icmp slt i32 %22, %23, !dbg !1451
  br i1 %24, label %25, label %105, !dbg !1451

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1452
  %27 = sext i32 %26 to i64, !dbg !1452
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1452
  %29 = load i8, ptr %28, align 1, !dbg !1452
  %30 = sext i8 %29 to i32, !dbg !1452
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
  ], !dbg !1452

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1455
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1455
  store ptr %33, ptr %5, align 8, !dbg !1455
  %34 = load i32, ptr %32, align 8, !dbg !1455
  %35 = trunc i32 %34 to i8, !dbg !1455
  %36 = load i32, ptr %12, align 4, !dbg !1455
  %37 = sext i32 %36 to i64, !dbg !1455
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1455
  store i8 %35, ptr %38, align 8, !dbg !1455
  br label %101, !dbg !1455

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1455
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1455
  store ptr %41, ptr %5, align 8, !dbg !1455
  %42 = load i32, ptr %40, align 8, !dbg !1455
  %43 = trunc i32 %42 to i8, !dbg !1455
  %44 = load i32, ptr %12, align 4, !dbg !1455
  %45 = sext i32 %44 to i64, !dbg !1455
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1455
  store i8 %43, ptr %46, align 8, !dbg !1455
  br label %101, !dbg !1455

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1455
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1455
  store ptr %49, ptr %5, align 8, !dbg !1455
  %50 = load i32, ptr %48, align 8, !dbg !1455
  %51 = trunc i32 %50 to i16, !dbg !1455
  %52 = load i32, ptr %12, align 4, !dbg !1455
  %53 = sext i32 %52 to i64, !dbg !1455
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1455
  store i16 %51, ptr %54, align 8, !dbg !1455
  br label %101, !dbg !1455

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1455
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1455
  store ptr %57, ptr %5, align 8, !dbg !1455
  %58 = load i32, ptr %56, align 8, !dbg !1455
  %59 = trunc i32 %58 to i16, !dbg !1455
  %60 = load i32, ptr %12, align 4, !dbg !1455
  %61 = sext i32 %60 to i64, !dbg !1455
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1455
  store i16 %59, ptr %62, align 8, !dbg !1455
  br label %101, !dbg !1455

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1455
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1455
  store ptr %65, ptr %5, align 8, !dbg !1455
  %66 = load i32, ptr %64, align 8, !dbg !1455
  %67 = load i32, ptr %12, align 4, !dbg !1455
  %68 = sext i32 %67 to i64, !dbg !1455
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1455
  store i32 %66, ptr %69, align 8, !dbg !1455
  br label %101, !dbg !1455

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1455
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1455
  store ptr %72, ptr %5, align 8, !dbg !1455
  %73 = load i32, ptr %71, align 8, !dbg !1455
  %74 = sext i32 %73 to i64, !dbg !1455
  %75 = load i32, ptr %12, align 4, !dbg !1455
  %76 = sext i32 %75 to i64, !dbg !1455
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1455
  store i64 %74, ptr %77, align 8, !dbg !1455
  br label %101, !dbg !1455

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1455
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1455
  store ptr %80, ptr %5, align 8, !dbg !1455
  %81 = load double, ptr %79, align 8, !dbg !1455
  %82 = fptrunc double %81 to float, !dbg !1455
  %83 = load i32, ptr %12, align 4, !dbg !1455
  %84 = sext i32 %83 to i64, !dbg !1455
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1455
  store float %82, ptr %85, align 8, !dbg !1455
  br label %101, !dbg !1455

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1455
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1455
  store ptr %88, ptr %5, align 8, !dbg !1455
  %89 = load double, ptr %87, align 8, !dbg !1455
  %90 = load i32, ptr %12, align 4, !dbg !1455
  %91 = sext i32 %90 to i64, !dbg !1455
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1455
  store double %89, ptr %92, align 8, !dbg !1455
  br label %101, !dbg !1455

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1455
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1455
  store ptr %95, ptr %5, align 8, !dbg !1455
  %96 = load ptr, ptr %94, align 8, !dbg !1455
  %97 = load i32, ptr %12, align 4, !dbg !1455
  %98 = sext i32 %97 to i64, !dbg !1455
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1455
  store ptr %96, ptr %99, align 8, !dbg !1455
  br label %101, !dbg !1455

100:                                              ; preds = %25
  br label %101, !dbg !1455

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1452

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1457
  %104 = add nsw i32 %103, 1, !dbg !1457
  store i32 %104, ptr %12, align 4, !dbg !1457
  br label %21, !dbg !1457, !llvm.loop !1458

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1442
  %107 = load ptr, ptr %106, align 8, !dbg !1442
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 122, !dbg !1442
  %109 = load ptr, ptr %108, align 8, !dbg !1442
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1442
  %111 = load ptr, ptr %6, align 8, !dbg !1442
  %112 = load ptr, ptr %7, align 8, !dbg !1442
  %113 = load ptr, ptr %8, align 8, !dbg !1442
  %114 = call i8 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1442
  ret i8 %114, !dbg !1442
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1459 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1460, metadata !DIExpression()), !dbg !1461
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1462, metadata !DIExpression()), !dbg !1461
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1463, metadata !DIExpression()), !dbg !1461
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1464, metadata !DIExpression()), !dbg !1461
  call void @llvm.va_start(ptr %7), !dbg !1461
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1465, metadata !DIExpression()), !dbg !1461
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1466, metadata !DIExpression()), !dbg !1461
  %13 = load ptr, ptr %6, align 8, !dbg !1461
  %14 = load ptr, ptr %13, align 8, !dbg !1461
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1461
  %16 = load ptr, ptr %15, align 8, !dbg !1461
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1461
  %18 = load ptr, ptr %4, align 8, !dbg !1461
  %19 = load ptr, ptr %6, align 8, !dbg !1461
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1461
  store i32 %20, ptr %9, align 4, !dbg !1461
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1467, metadata !DIExpression()), !dbg !1461
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1468, metadata !DIExpression()), !dbg !1470
  store i32 0, ptr %11, align 4, !dbg !1470
  br label %21, !dbg !1470

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1470
  %23 = load i32, ptr %9, align 4, !dbg !1470
  %24 = icmp slt i32 %22, %23, !dbg !1470
  br i1 %24, label %25, label %105, !dbg !1470

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1471
  %27 = sext i32 %26 to i64, !dbg !1471
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1471
  %29 = load i8, ptr %28, align 1, !dbg !1471
  %30 = sext i8 %29 to i32, !dbg !1471
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
  ], !dbg !1471

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1474
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1474
  store ptr %33, ptr %7, align 8, !dbg !1474
  %34 = load i32, ptr %32, align 8, !dbg !1474
  %35 = trunc i32 %34 to i8, !dbg !1474
  %36 = load i32, ptr %11, align 4, !dbg !1474
  %37 = sext i32 %36 to i64, !dbg !1474
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1474
  store i8 %35, ptr %38, align 8, !dbg !1474
  br label %101, !dbg !1474

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1474
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1474
  store ptr %41, ptr %7, align 8, !dbg !1474
  %42 = load i32, ptr %40, align 8, !dbg !1474
  %43 = trunc i32 %42 to i8, !dbg !1474
  %44 = load i32, ptr %11, align 4, !dbg !1474
  %45 = sext i32 %44 to i64, !dbg !1474
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1474
  store i8 %43, ptr %46, align 8, !dbg !1474
  br label %101, !dbg !1474

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1474
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1474
  store ptr %49, ptr %7, align 8, !dbg !1474
  %50 = load i32, ptr %48, align 8, !dbg !1474
  %51 = trunc i32 %50 to i16, !dbg !1474
  %52 = load i32, ptr %11, align 4, !dbg !1474
  %53 = sext i32 %52 to i64, !dbg !1474
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1474
  store i16 %51, ptr %54, align 8, !dbg !1474
  br label %101, !dbg !1474

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1474
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1474
  store ptr %57, ptr %7, align 8, !dbg !1474
  %58 = load i32, ptr %56, align 8, !dbg !1474
  %59 = trunc i32 %58 to i16, !dbg !1474
  %60 = load i32, ptr %11, align 4, !dbg !1474
  %61 = sext i32 %60 to i64, !dbg !1474
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1474
  store i16 %59, ptr %62, align 8, !dbg !1474
  br label %101, !dbg !1474

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1474
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1474
  store ptr %65, ptr %7, align 8, !dbg !1474
  %66 = load i32, ptr %64, align 8, !dbg !1474
  %67 = load i32, ptr %11, align 4, !dbg !1474
  %68 = sext i32 %67 to i64, !dbg !1474
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1474
  store i32 %66, ptr %69, align 8, !dbg !1474
  br label %101, !dbg !1474

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1474
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1474
  store ptr %72, ptr %7, align 8, !dbg !1474
  %73 = load i32, ptr %71, align 8, !dbg !1474
  %74 = sext i32 %73 to i64, !dbg !1474
  %75 = load i32, ptr %11, align 4, !dbg !1474
  %76 = sext i32 %75 to i64, !dbg !1474
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1474
  store i64 %74, ptr %77, align 8, !dbg !1474
  br label %101, !dbg !1474

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1474
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1474
  store ptr %80, ptr %7, align 8, !dbg !1474
  %81 = load double, ptr %79, align 8, !dbg !1474
  %82 = fptrunc double %81 to float, !dbg !1474
  %83 = load i32, ptr %11, align 4, !dbg !1474
  %84 = sext i32 %83 to i64, !dbg !1474
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1474
  store float %82, ptr %85, align 8, !dbg !1474
  br label %101, !dbg !1474

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1474
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1474
  store ptr %88, ptr %7, align 8, !dbg !1474
  %89 = load double, ptr %87, align 8, !dbg !1474
  %90 = load i32, ptr %11, align 4, !dbg !1474
  %91 = sext i32 %90 to i64, !dbg !1474
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1474
  store double %89, ptr %92, align 8, !dbg !1474
  br label %101, !dbg !1474

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1474
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1474
  store ptr %95, ptr %7, align 8, !dbg !1474
  %96 = load ptr, ptr %94, align 8, !dbg !1474
  %97 = load i32, ptr %11, align 4, !dbg !1474
  %98 = sext i32 %97 to i64, !dbg !1474
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1474
  store ptr %96, ptr %99, align 8, !dbg !1474
  br label %101, !dbg !1474

100:                                              ; preds = %25
  br label %101, !dbg !1474

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1471

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1476
  %104 = add nsw i32 %103, 1, !dbg !1476
  store i32 %104, ptr %11, align 4, !dbg !1476
  br label %21, !dbg !1476, !llvm.loop !1477

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1478, metadata !DIExpression()), !dbg !1461
  %106 = load ptr, ptr %6, align 8, !dbg !1461
  %107 = load ptr, ptr %106, align 8, !dbg !1461
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 45, !dbg !1461
  %109 = load ptr, ptr %108, align 8, !dbg !1461
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1461
  %111 = load ptr, ptr %4, align 8, !dbg !1461
  %112 = load ptr, ptr %5, align 8, !dbg !1461
  %113 = load ptr, ptr %6, align 8, !dbg !1461
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1461
  store i16 %114, ptr %12, align 2, !dbg !1461
  call void @llvm.va_end(ptr %7), !dbg !1461
  %115 = load i16, ptr %12, align 2, !dbg !1461
  ret i16 %115, !dbg !1461
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1479 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1480, metadata !DIExpression()), !dbg !1481
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1482, metadata !DIExpression()), !dbg !1481
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1483, metadata !DIExpression()), !dbg !1481
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1484, metadata !DIExpression()), !dbg !1481
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1485, metadata !DIExpression()), !dbg !1481
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1486, metadata !DIExpression()), !dbg !1481
  %13 = load ptr, ptr %8, align 8, !dbg !1481
  %14 = load ptr, ptr %13, align 8, !dbg !1481
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1481
  %16 = load ptr, ptr %15, align 8, !dbg !1481
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1481
  %18 = load ptr, ptr %6, align 8, !dbg !1481
  %19 = load ptr, ptr %8, align 8, !dbg !1481
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1481
  store i32 %20, ptr %10, align 4, !dbg !1481
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1487, metadata !DIExpression()), !dbg !1481
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1488, metadata !DIExpression()), !dbg !1490
  store i32 0, ptr %12, align 4, !dbg !1490
  br label %21, !dbg !1490

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1490
  %23 = load i32, ptr %10, align 4, !dbg !1490
  %24 = icmp slt i32 %22, %23, !dbg !1490
  br i1 %24, label %25, label %105, !dbg !1490

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1491
  %27 = sext i32 %26 to i64, !dbg !1491
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1491
  %29 = load i8, ptr %28, align 1, !dbg !1491
  %30 = sext i8 %29 to i32, !dbg !1491
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
  ], !dbg !1491

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1494
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1494
  store ptr %33, ptr %5, align 8, !dbg !1494
  %34 = load i32, ptr %32, align 8, !dbg !1494
  %35 = trunc i32 %34 to i8, !dbg !1494
  %36 = load i32, ptr %12, align 4, !dbg !1494
  %37 = sext i32 %36 to i64, !dbg !1494
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1494
  store i8 %35, ptr %38, align 8, !dbg !1494
  br label %101, !dbg !1494

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1494
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1494
  store ptr %41, ptr %5, align 8, !dbg !1494
  %42 = load i32, ptr %40, align 8, !dbg !1494
  %43 = trunc i32 %42 to i8, !dbg !1494
  %44 = load i32, ptr %12, align 4, !dbg !1494
  %45 = sext i32 %44 to i64, !dbg !1494
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1494
  store i8 %43, ptr %46, align 8, !dbg !1494
  br label %101, !dbg !1494

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1494
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1494
  store ptr %49, ptr %5, align 8, !dbg !1494
  %50 = load i32, ptr %48, align 8, !dbg !1494
  %51 = trunc i32 %50 to i16, !dbg !1494
  %52 = load i32, ptr %12, align 4, !dbg !1494
  %53 = sext i32 %52 to i64, !dbg !1494
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1494
  store i16 %51, ptr %54, align 8, !dbg !1494
  br label %101, !dbg !1494

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1494
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1494
  store ptr %57, ptr %5, align 8, !dbg !1494
  %58 = load i32, ptr %56, align 8, !dbg !1494
  %59 = trunc i32 %58 to i16, !dbg !1494
  %60 = load i32, ptr %12, align 4, !dbg !1494
  %61 = sext i32 %60 to i64, !dbg !1494
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1494
  store i16 %59, ptr %62, align 8, !dbg !1494
  br label %101, !dbg !1494

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1494
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1494
  store ptr %65, ptr %5, align 8, !dbg !1494
  %66 = load i32, ptr %64, align 8, !dbg !1494
  %67 = load i32, ptr %12, align 4, !dbg !1494
  %68 = sext i32 %67 to i64, !dbg !1494
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1494
  store i32 %66, ptr %69, align 8, !dbg !1494
  br label %101, !dbg !1494

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1494
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1494
  store ptr %72, ptr %5, align 8, !dbg !1494
  %73 = load i32, ptr %71, align 8, !dbg !1494
  %74 = sext i32 %73 to i64, !dbg !1494
  %75 = load i32, ptr %12, align 4, !dbg !1494
  %76 = sext i32 %75 to i64, !dbg !1494
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1494
  store i64 %74, ptr %77, align 8, !dbg !1494
  br label %101, !dbg !1494

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1494
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1494
  store ptr %80, ptr %5, align 8, !dbg !1494
  %81 = load double, ptr %79, align 8, !dbg !1494
  %82 = fptrunc double %81 to float, !dbg !1494
  %83 = load i32, ptr %12, align 4, !dbg !1494
  %84 = sext i32 %83 to i64, !dbg !1494
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1494
  store float %82, ptr %85, align 8, !dbg !1494
  br label %101, !dbg !1494

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1494
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1494
  store ptr %88, ptr %5, align 8, !dbg !1494
  %89 = load double, ptr %87, align 8, !dbg !1494
  %90 = load i32, ptr %12, align 4, !dbg !1494
  %91 = sext i32 %90 to i64, !dbg !1494
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1494
  store double %89, ptr %92, align 8, !dbg !1494
  br label %101, !dbg !1494

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1494
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1494
  store ptr %95, ptr %5, align 8, !dbg !1494
  %96 = load ptr, ptr %94, align 8, !dbg !1494
  %97 = load i32, ptr %12, align 4, !dbg !1494
  %98 = sext i32 %97 to i64, !dbg !1494
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1494
  store ptr %96, ptr %99, align 8, !dbg !1494
  br label %101, !dbg !1494

100:                                              ; preds = %25
  br label %101, !dbg !1494

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1491

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1496
  %104 = add nsw i32 %103, 1, !dbg !1496
  store i32 %104, ptr %12, align 4, !dbg !1496
  br label %21, !dbg !1496, !llvm.loop !1497

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1481
  %107 = load ptr, ptr %106, align 8, !dbg !1481
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 45, !dbg !1481
  %109 = load ptr, ptr %108, align 8, !dbg !1481
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1481
  %111 = load ptr, ptr %6, align 8, !dbg !1481
  %112 = load ptr, ptr %7, align 8, !dbg !1481
  %113 = load ptr, ptr %8, align 8, !dbg !1481
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1481
  ret i16 %114, !dbg !1481
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1498 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1499, metadata !DIExpression()), !dbg !1500
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1501, metadata !DIExpression()), !dbg !1500
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1502, metadata !DIExpression()), !dbg !1500
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1503, metadata !DIExpression()), !dbg !1500
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1504, metadata !DIExpression()), !dbg !1500
  call void @llvm.va_start(ptr %9), !dbg !1500
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1505, metadata !DIExpression()), !dbg !1500
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1506, metadata !DIExpression()), !dbg !1500
  %15 = load ptr, ptr %8, align 8, !dbg !1500
  %16 = load ptr, ptr %15, align 8, !dbg !1500
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1500
  %18 = load ptr, ptr %17, align 8, !dbg !1500
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1500
  %20 = load ptr, ptr %5, align 8, !dbg !1500
  %21 = load ptr, ptr %8, align 8, !dbg !1500
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1500
  store i32 %22, ptr %11, align 4, !dbg !1500
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1507, metadata !DIExpression()), !dbg !1500
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1508, metadata !DIExpression()), !dbg !1510
  store i32 0, ptr %13, align 4, !dbg !1510
  br label %23, !dbg !1510

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1510
  %25 = load i32, ptr %11, align 4, !dbg !1510
  %26 = icmp slt i32 %24, %25, !dbg !1510
  br i1 %26, label %27, label %107, !dbg !1510

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1511
  %29 = sext i32 %28 to i64, !dbg !1511
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1511
  %31 = load i8, ptr %30, align 1, !dbg !1511
  %32 = sext i8 %31 to i32, !dbg !1511
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
  ], !dbg !1511

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1514
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1514
  store ptr %35, ptr %9, align 8, !dbg !1514
  %36 = load i32, ptr %34, align 8, !dbg !1514
  %37 = trunc i32 %36 to i8, !dbg !1514
  %38 = load i32, ptr %13, align 4, !dbg !1514
  %39 = sext i32 %38 to i64, !dbg !1514
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1514
  store i8 %37, ptr %40, align 8, !dbg !1514
  br label %103, !dbg !1514

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1514
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1514
  store ptr %43, ptr %9, align 8, !dbg !1514
  %44 = load i32, ptr %42, align 8, !dbg !1514
  %45 = trunc i32 %44 to i8, !dbg !1514
  %46 = load i32, ptr %13, align 4, !dbg !1514
  %47 = sext i32 %46 to i64, !dbg !1514
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1514
  store i8 %45, ptr %48, align 8, !dbg !1514
  br label %103, !dbg !1514

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1514
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1514
  store ptr %51, ptr %9, align 8, !dbg !1514
  %52 = load i32, ptr %50, align 8, !dbg !1514
  %53 = trunc i32 %52 to i16, !dbg !1514
  %54 = load i32, ptr %13, align 4, !dbg !1514
  %55 = sext i32 %54 to i64, !dbg !1514
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1514
  store i16 %53, ptr %56, align 8, !dbg !1514
  br label %103, !dbg !1514

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1514
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1514
  store ptr %59, ptr %9, align 8, !dbg !1514
  %60 = load i32, ptr %58, align 8, !dbg !1514
  %61 = trunc i32 %60 to i16, !dbg !1514
  %62 = load i32, ptr %13, align 4, !dbg !1514
  %63 = sext i32 %62 to i64, !dbg !1514
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1514
  store i16 %61, ptr %64, align 8, !dbg !1514
  br label %103, !dbg !1514

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1514
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1514
  store ptr %67, ptr %9, align 8, !dbg !1514
  %68 = load i32, ptr %66, align 8, !dbg !1514
  %69 = load i32, ptr %13, align 4, !dbg !1514
  %70 = sext i32 %69 to i64, !dbg !1514
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1514
  store i32 %68, ptr %71, align 8, !dbg !1514
  br label %103, !dbg !1514

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1514
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1514
  store ptr %74, ptr %9, align 8, !dbg !1514
  %75 = load i32, ptr %73, align 8, !dbg !1514
  %76 = sext i32 %75 to i64, !dbg !1514
  %77 = load i32, ptr %13, align 4, !dbg !1514
  %78 = sext i32 %77 to i64, !dbg !1514
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1514
  store i64 %76, ptr %79, align 8, !dbg !1514
  br label %103, !dbg !1514

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1514
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1514
  store ptr %82, ptr %9, align 8, !dbg !1514
  %83 = load double, ptr %81, align 8, !dbg !1514
  %84 = fptrunc double %83 to float, !dbg !1514
  %85 = load i32, ptr %13, align 4, !dbg !1514
  %86 = sext i32 %85 to i64, !dbg !1514
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1514
  store float %84, ptr %87, align 8, !dbg !1514
  br label %103, !dbg !1514

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1514
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1514
  store ptr %90, ptr %9, align 8, !dbg !1514
  %91 = load double, ptr %89, align 8, !dbg !1514
  %92 = load i32, ptr %13, align 4, !dbg !1514
  %93 = sext i32 %92 to i64, !dbg !1514
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1514
  store double %91, ptr %94, align 8, !dbg !1514
  br label %103, !dbg !1514

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1514
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1514
  store ptr %97, ptr %9, align 8, !dbg !1514
  %98 = load ptr, ptr %96, align 8, !dbg !1514
  %99 = load i32, ptr %13, align 4, !dbg !1514
  %100 = sext i32 %99 to i64, !dbg !1514
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1514
  store ptr %98, ptr %101, align 8, !dbg !1514
  br label %103, !dbg !1514

102:                                              ; preds = %27
  br label %103, !dbg !1514

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1511

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1516
  %106 = add nsw i32 %105, 1, !dbg !1516
  store i32 %106, ptr %13, align 4, !dbg !1516
  br label %23, !dbg !1516, !llvm.loop !1517

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1518, metadata !DIExpression()), !dbg !1500
  %108 = load ptr, ptr %8, align 8, !dbg !1500
  %109 = load ptr, ptr %108, align 8, !dbg !1500
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 75, !dbg !1500
  %111 = load ptr, ptr %110, align 8, !dbg !1500
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1500
  %113 = load ptr, ptr %5, align 8, !dbg !1500
  %114 = load ptr, ptr %6, align 8, !dbg !1500
  %115 = load ptr, ptr %7, align 8, !dbg !1500
  %116 = load ptr, ptr %8, align 8, !dbg !1500
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1500
  store i16 %117, ptr %14, align 2, !dbg !1500
  call void @llvm.va_end(ptr %9), !dbg !1500
  %118 = load i16, ptr %14, align 2, !dbg !1500
  ret i16 %118, !dbg !1500
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1519 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1520, metadata !DIExpression()), !dbg !1521
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1522, metadata !DIExpression()), !dbg !1521
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1523, metadata !DIExpression()), !dbg !1521
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1524, metadata !DIExpression()), !dbg !1521
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1525, metadata !DIExpression()), !dbg !1521
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1526, metadata !DIExpression()), !dbg !1521
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1527, metadata !DIExpression()), !dbg !1521
  %15 = load ptr, ptr %10, align 8, !dbg !1521
  %16 = load ptr, ptr %15, align 8, !dbg !1521
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1521
  %18 = load ptr, ptr %17, align 8, !dbg !1521
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1521
  %20 = load ptr, ptr %7, align 8, !dbg !1521
  %21 = load ptr, ptr %10, align 8, !dbg !1521
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1521
  store i32 %22, ptr %12, align 4, !dbg !1521
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1528, metadata !DIExpression()), !dbg !1521
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1529, metadata !DIExpression()), !dbg !1531
  store i32 0, ptr %14, align 4, !dbg !1531
  br label %23, !dbg !1531

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1531
  %25 = load i32, ptr %12, align 4, !dbg !1531
  %26 = icmp slt i32 %24, %25, !dbg !1531
  br i1 %26, label %27, label %107, !dbg !1531

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1532
  %29 = sext i32 %28 to i64, !dbg !1532
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1532
  %31 = load i8, ptr %30, align 1, !dbg !1532
  %32 = sext i8 %31 to i32, !dbg !1532
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
  ], !dbg !1532

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1535
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1535
  store ptr %35, ptr %6, align 8, !dbg !1535
  %36 = load i32, ptr %34, align 8, !dbg !1535
  %37 = trunc i32 %36 to i8, !dbg !1535
  %38 = load i32, ptr %14, align 4, !dbg !1535
  %39 = sext i32 %38 to i64, !dbg !1535
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1535
  store i8 %37, ptr %40, align 8, !dbg !1535
  br label %103, !dbg !1535

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1535
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1535
  store ptr %43, ptr %6, align 8, !dbg !1535
  %44 = load i32, ptr %42, align 8, !dbg !1535
  %45 = trunc i32 %44 to i8, !dbg !1535
  %46 = load i32, ptr %14, align 4, !dbg !1535
  %47 = sext i32 %46 to i64, !dbg !1535
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1535
  store i8 %45, ptr %48, align 8, !dbg !1535
  br label %103, !dbg !1535

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1535
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1535
  store ptr %51, ptr %6, align 8, !dbg !1535
  %52 = load i32, ptr %50, align 8, !dbg !1535
  %53 = trunc i32 %52 to i16, !dbg !1535
  %54 = load i32, ptr %14, align 4, !dbg !1535
  %55 = sext i32 %54 to i64, !dbg !1535
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1535
  store i16 %53, ptr %56, align 8, !dbg !1535
  br label %103, !dbg !1535

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1535
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1535
  store ptr %59, ptr %6, align 8, !dbg !1535
  %60 = load i32, ptr %58, align 8, !dbg !1535
  %61 = trunc i32 %60 to i16, !dbg !1535
  %62 = load i32, ptr %14, align 4, !dbg !1535
  %63 = sext i32 %62 to i64, !dbg !1535
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1535
  store i16 %61, ptr %64, align 8, !dbg !1535
  br label %103, !dbg !1535

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1535
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1535
  store ptr %67, ptr %6, align 8, !dbg !1535
  %68 = load i32, ptr %66, align 8, !dbg !1535
  %69 = load i32, ptr %14, align 4, !dbg !1535
  %70 = sext i32 %69 to i64, !dbg !1535
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1535
  store i32 %68, ptr %71, align 8, !dbg !1535
  br label %103, !dbg !1535

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1535
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1535
  store ptr %74, ptr %6, align 8, !dbg !1535
  %75 = load i32, ptr %73, align 8, !dbg !1535
  %76 = sext i32 %75 to i64, !dbg !1535
  %77 = load i32, ptr %14, align 4, !dbg !1535
  %78 = sext i32 %77 to i64, !dbg !1535
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1535
  store i64 %76, ptr %79, align 8, !dbg !1535
  br label %103, !dbg !1535

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1535
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1535
  store ptr %82, ptr %6, align 8, !dbg !1535
  %83 = load double, ptr %81, align 8, !dbg !1535
  %84 = fptrunc double %83 to float, !dbg !1535
  %85 = load i32, ptr %14, align 4, !dbg !1535
  %86 = sext i32 %85 to i64, !dbg !1535
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1535
  store float %84, ptr %87, align 8, !dbg !1535
  br label %103, !dbg !1535

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1535
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1535
  store ptr %90, ptr %6, align 8, !dbg !1535
  %91 = load double, ptr %89, align 8, !dbg !1535
  %92 = load i32, ptr %14, align 4, !dbg !1535
  %93 = sext i32 %92 to i64, !dbg !1535
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1535
  store double %91, ptr %94, align 8, !dbg !1535
  br label %103, !dbg !1535

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1535
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1535
  store ptr %97, ptr %6, align 8, !dbg !1535
  %98 = load ptr, ptr %96, align 8, !dbg !1535
  %99 = load i32, ptr %14, align 4, !dbg !1535
  %100 = sext i32 %99 to i64, !dbg !1535
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1535
  store ptr %98, ptr %101, align 8, !dbg !1535
  br label %103, !dbg !1535

102:                                              ; preds = %27
  br label %103, !dbg !1535

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1532

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1537
  %106 = add nsw i32 %105, 1, !dbg !1537
  store i32 %106, ptr %14, align 4, !dbg !1537
  br label %23, !dbg !1537, !llvm.loop !1538

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1521
  %109 = load ptr, ptr %108, align 8, !dbg !1521
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 75, !dbg !1521
  %111 = load ptr, ptr %110, align 8, !dbg !1521
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1521
  %113 = load ptr, ptr %7, align 8, !dbg !1521
  %114 = load ptr, ptr %8, align 8, !dbg !1521
  %115 = load ptr, ptr %9, align 8, !dbg !1521
  %116 = load ptr, ptr %10, align 8, !dbg !1521
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1521
  ret i16 %117, !dbg !1521
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1539 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1540, metadata !DIExpression()), !dbg !1541
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1542, metadata !DIExpression()), !dbg !1541
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1543, metadata !DIExpression()), !dbg !1541
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1544, metadata !DIExpression()), !dbg !1541
  call void @llvm.va_start(ptr %7), !dbg !1541
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1545, metadata !DIExpression()), !dbg !1541
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1546, metadata !DIExpression()), !dbg !1541
  %13 = load ptr, ptr %6, align 8, !dbg !1541
  %14 = load ptr, ptr %13, align 8, !dbg !1541
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1541
  %16 = load ptr, ptr %15, align 8, !dbg !1541
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1541
  %18 = load ptr, ptr %4, align 8, !dbg !1541
  %19 = load ptr, ptr %6, align 8, !dbg !1541
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1541
  store i32 %20, ptr %9, align 4, !dbg !1541
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1547, metadata !DIExpression()), !dbg !1541
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1548, metadata !DIExpression()), !dbg !1550
  store i32 0, ptr %11, align 4, !dbg !1550
  br label %21, !dbg !1550

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1550
  %23 = load i32, ptr %9, align 4, !dbg !1550
  %24 = icmp slt i32 %22, %23, !dbg !1550
  br i1 %24, label %25, label %105, !dbg !1550

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1551
  %27 = sext i32 %26 to i64, !dbg !1551
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1551
  %29 = load i8, ptr %28, align 1, !dbg !1551
  %30 = sext i8 %29 to i32, !dbg !1551
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
  ], !dbg !1551

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1554
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1554
  store ptr %33, ptr %7, align 8, !dbg !1554
  %34 = load i32, ptr %32, align 8, !dbg !1554
  %35 = trunc i32 %34 to i8, !dbg !1554
  %36 = load i32, ptr %11, align 4, !dbg !1554
  %37 = sext i32 %36 to i64, !dbg !1554
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1554
  store i8 %35, ptr %38, align 8, !dbg !1554
  br label %101, !dbg !1554

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1554
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1554
  store ptr %41, ptr %7, align 8, !dbg !1554
  %42 = load i32, ptr %40, align 8, !dbg !1554
  %43 = trunc i32 %42 to i8, !dbg !1554
  %44 = load i32, ptr %11, align 4, !dbg !1554
  %45 = sext i32 %44 to i64, !dbg !1554
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1554
  store i8 %43, ptr %46, align 8, !dbg !1554
  br label %101, !dbg !1554

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1554
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1554
  store ptr %49, ptr %7, align 8, !dbg !1554
  %50 = load i32, ptr %48, align 8, !dbg !1554
  %51 = trunc i32 %50 to i16, !dbg !1554
  %52 = load i32, ptr %11, align 4, !dbg !1554
  %53 = sext i32 %52 to i64, !dbg !1554
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1554
  store i16 %51, ptr %54, align 8, !dbg !1554
  br label %101, !dbg !1554

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1554
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1554
  store ptr %57, ptr %7, align 8, !dbg !1554
  %58 = load i32, ptr %56, align 8, !dbg !1554
  %59 = trunc i32 %58 to i16, !dbg !1554
  %60 = load i32, ptr %11, align 4, !dbg !1554
  %61 = sext i32 %60 to i64, !dbg !1554
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1554
  store i16 %59, ptr %62, align 8, !dbg !1554
  br label %101, !dbg !1554

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1554
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1554
  store ptr %65, ptr %7, align 8, !dbg !1554
  %66 = load i32, ptr %64, align 8, !dbg !1554
  %67 = load i32, ptr %11, align 4, !dbg !1554
  %68 = sext i32 %67 to i64, !dbg !1554
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1554
  store i32 %66, ptr %69, align 8, !dbg !1554
  br label %101, !dbg !1554

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1554
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1554
  store ptr %72, ptr %7, align 8, !dbg !1554
  %73 = load i32, ptr %71, align 8, !dbg !1554
  %74 = sext i32 %73 to i64, !dbg !1554
  %75 = load i32, ptr %11, align 4, !dbg !1554
  %76 = sext i32 %75 to i64, !dbg !1554
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1554
  store i64 %74, ptr %77, align 8, !dbg !1554
  br label %101, !dbg !1554

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1554
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1554
  store ptr %80, ptr %7, align 8, !dbg !1554
  %81 = load double, ptr %79, align 8, !dbg !1554
  %82 = fptrunc double %81 to float, !dbg !1554
  %83 = load i32, ptr %11, align 4, !dbg !1554
  %84 = sext i32 %83 to i64, !dbg !1554
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1554
  store float %82, ptr %85, align 8, !dbg !1554
  br label %101, !dbg !1554

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1554
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1554
  store ptr %88, ptr %7, align 8, !dbg !1554
  %89 = load double, ptr %87, align 8, !dbg !1554
  %90 = load i32, ptr %11, align 4, !dbg !1554
  %91 = sext i32 %90 to i64, !dbg !1554
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1554
  store double %89, ptr %92, align 8, !dbg !1554
  br label %101, !dbg !1554

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1554
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1554
  store ptr %95, ptr %7, align 8, !dbg !1554
  %96 = load ptr, ptr %94, align 8, !dbg !1554
  %97 = load i32, ptr %11, align 4, !dbg !1554
  %98 = sext i32 %97 to i64, !dbg !1554
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1554
  store ptr %96, ptr %99, align 8, !dbg !1554
  br label %101, !dbg !1554

100:                                              ; preds = %25
  br label %101, !dbg !1554

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1551

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1556
  %104 = add nsw i32 %103, 1, !dbg !1556
  store i32 %104, ptr %11, align 4, !dbg !1556
  br label %21, !dbg !1556, !llvm.loop !1557

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1558, metadata !DIExpression()), !dbg !1541
  %106 = load ptr, ptr %6, align 8, !dbg !1541
  %107 = load ptr, ptr %106, align 8, !dbg !1541
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 125, !dbg !1541
  %109 = load ptr, ptr %108, align 8, !dbg !1541
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1541
  %111 = load ptr, ptr %4, align 8, !dbg !1541
  %112 = load ptr, ptr %5, align 8, !dbg !1541
  %113 = load ptr, ptr %6, align 8, !dbg !1541
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1541
  store i16 %114, ptr %12, align 2, !dbg !1541
  call void @llvm.va_end(ptr %7), !dbg !1541
  %115 = load i16, ptr %12, align 2, !dbg !1541
  ret i16 %115, !dbg !1541
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1559 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1560, metadata !DIExpression()), !dbg !1561
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1562, metadata !DIExpression()), !dbg !1561
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1563, metadata !DIExpression()), !dbg !1561
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1564, metadata !DIExpression()), !dbg !1561
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1565, metadata !DIExpression()), !dbg !1561
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1566, metadata !DIExpression()), !dbg !1561
  %13 = load ptr, ptr %8, align 8, !dbg !1561
  %14 = load ptr, ptr %13, align 8, !dbg !1561
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1561
  %16 = load ptr, ptr %15, align 8, !dbg !1561
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1561
  %18 = load ptr, ptr %6, align 8, !dbg !1561
  %19 = load ptr, ptr %8, align 8, !dbg !1561
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1561
  store i32 %20, ptr %10, align 4, !dbg !1561
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1567, metadata !DIExpression()), !dbg !1561
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1568, metadata !DIExpression()), !dbg !1570
  store i32 0, ptr %12, align 4, !dbg !1570
  br label %21, !dbg !1570

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1570
  %23 = load i32, ptr %10, align 4, !dbg !1570
  %24 = icmp slt i32 %22, %23, !dbg !1570
  br i1 %24, label %25, label %105, !dbg !1570

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1571
  %27 = sext i32 %26 to i64, !dbg !1571
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1571
  %29 = load i8, ptr %28, align 1, !dbg !1571
  %30 = sext i8 %29 to i32, !dbg !1571
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
  ], !dbg !1571

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1574
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1574
  store ptr %33, ptr %5, align 8, !dbg !1574
  %34 = load i32, ptr %32, align 8, !dbg !1574
  %35 = trunc i32 %34 to i8, !dbg !1574
  %36 = load i32, ptr %12, align 4, !dbg !1574
  %37 = sext i32 %36 to i64, !dbg !1574
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1574
  store i8 %35, ptr %38, align 8, !dbg !1574
  br label %101, !dbg !1574

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1574
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1574
  store ptr %41, ptr %5, align 8, !dbg !1574
  %42 = load i32, ptr %40, align 8, !dbg !1574
  %43 = trunc i32 %42 to i8, !dbg !1574
  %44 = load i32, ptr %12, align 4, !dbg !1574
  %45 = sext i32 %44 to i64, !dbg !1574
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1574
  store i8 %43, ptr %46, align 8, !dbg !1574
  br label %101, !dbg !1574

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1574
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1574
  store ptr %49, ptr %5, align 8, !dbg !1574
  %50 = load i32, ptr %48, align 8, !dbg !1574
  %51 = trunc i32 %50 to i16, !dbg !1574
  %52 = load i32, ptr %12, align 4, !dbg !1574
  %53 = sext i32 %52 to i64, !dbg !1574
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1574
  store i16 %51, ptr %54, align 8, !dbg !1574
  br label %101, !dbg !1574

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1574
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1574
  store ptr %57, ptr %5, align 8, !dbg !1574
  %58 = load i32, ptr %56, align 8, !dbg !1574
  %59 = trunc i32 %58 to i16, !dbg !1574
  %60 = load i32, ptr %12, align 4, !dbg !1574
  %61 = sext i32 %60 to i64, !dbg !1574
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1574
  store i16 %59, ptr %62, align 8, !dbg !1574
  br label %101, !dbg !1574

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1574
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1574
  store ptr %65, ptr %5, align 8, !dbg !1574
  %66 = load i32, ptr %64, align 8, !dbg !1574
  %67 = load i32, ptr %12, align 4, !dbg !1574
  %68 = sext i32 %67 to i64, !dbg !1574
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1574
  store i32 %66, ptr %69, align 8, !dbg !1574
  br label %101, !dbg !1574

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1574
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1574
  store ptr %72, ptr %5, align 8, !dbg !1574
  %73 = load i32, ptr %71, align 8, !dbg !1574
  %74 = sext i32 %73 to i64, !dbg !1574
  %75 = load i32, ptr %12, align 4, !dbg !1574
  %76 = sext i32 %75 to i64, !dbg !1574
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1574
  store i64 %74, ptr %77, align 8, !dbg !1574
  br label %101, !dbg !1574

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1574
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1574
  store ptr %80, ptr %5, align 8, !dbg !1574
  %81 = load double, ptr %79, align 8, !dbg !1574
  %82 = fptrunc double %81 to float, !dbg !1574
  %83 = load i32, ptr %12, align 4, !dbg !1574
  %84 = sext i32 %83 to i64, !dbg !1574
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1574
  store float %82, ptr %85, align 8, !dbg !1574
  br label %101, !dbg !1574

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1574
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1574
  store ptr %88, ptr %5, align 8, !dbg !1574
  %89 = load double, ptr %87, align 8, !dbg !1574
  %90 = load i32, ptr %12, align 4, !dbg !1574
  %91 = sext i32 %90 to i64, !dbg !1574
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1574
  store double %89, ptr %92, align 8, !dbg !1574
  br label %101, !dbg !1574

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1574
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1574
  store ptr %95, ptr %5, align 8, !dbg !1574
  %96 = load ptr, ptr %94, align 8, !dbg !1574
  %97 = load i32, ptr %12, align 4, !dbg !1574
  %98 = sext i32 %97 to i64, !dbg !1574
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1574
  store ptr %96, ptr %99, align 8, !dbg !1574
  br label %101, !dbg !1574

100:                                              ; preds = %25
  br label %101, !dbg !1574

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1571

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1576
  %104 = add nsw i32 %103, 1, !dbg !1576
  store i32 %104, ptr %12, align 4, !dbg !1576
  br label %21, !dbg !1576, !llvm.loop !1577

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1561
  %107 = load ptr, ptr %106, align 8, !dbg !1561
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 125, !dbg !1561
  %109 = load ptr, ptr %108, align 8, !dbg !1561
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1561
  %111 = load ptr, ptr %6, align 8, !dbg !1561
  %112 = load ptr, ptr %7, align 8, !dbg !1561
  %113 = load ptr, ptr %8, align 8, !dbg !1561
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1561
  ret i16 %114, !dbg !1561
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1578 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1579, metadata !DIExpression()), !dbg !1580
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1581, metadata !DIExpression()), !dbg !1580
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1582, metadata !DIExpression()), !dbg !1580
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1583, metadata !DIExpression()), !dbg !1580
  call void @llvm.va_start(ptr %7), !dbg !1580
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1584, metadata !DIExpression()), !dbg !1580
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1585, metadata !DIExpression()), !dbg !1580
  %13 = load ptr, ptr %6, align 8, !dbg !1580
  %14 = load ptr, ptr %13, align 8, !dbg !1580
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1580
  %16 = load ptr, ptr %15, align 8, !dbg !1580
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1580
  %18 = load ptr, ptr %4, align 8, !dbg !1580
  %19 = load ptr, ptr %6, align 8, !dbg !1580
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1580
  store i32 %20, ptr %9, align 4, !dbg !1580
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1586, metadata !DIExpression()), !dbg !1580
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1587, metadata !DIExpression()), !dbg !1589
  store i32 0, ptr %11, align 4, !dbg !1589
  br label %21, !dbg !1589

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1589
  %23 = load i32, ptr %9, align 4, !dbg !1589
  %24 = icmp slt i32 %22, %23, !dbg !1589
  br i1 %24, label %25, label %105, !dbg !1589

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1590
  %27 = sext i32 %26 to i64, !dbg !1590
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1590
  %29 = load i8, ptr %28, align 1, !dbg !1590
  %30 = sext i8 %29 to i32, !dbg !1590
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
  ], !dbg !1590

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1593
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1593
  store ptr %33, ptr %7, align 8, !dbg !1593
  %34 = load i32, ptr %32, align 8, !dbg !1593
  %35 = trunc i32 %34 to i8, !dbg !1593
  %36 = load i32, ptr %11, align 4, !dbg !1593
  %37 = sext i32 %36 to i64, !dbg !1593
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1593
  store i8 %35, ptr %38, align 8, !dbg !1593
  br label %101, !dbg !1593

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1593
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1593
  store ptr %41, ptr %7, align 8, !dbg !1593
  %42 = load i32, ptr %40, align 8, !dbg !1593
  %43 = trunc i32 %42 to i8, !dbg !1593
  %44 = load i32, ptr %11, align 4, !dbg !1593
  %45 = sext i32 %44 to i64, !dbg !1593
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1593
  store i8 %43, ptr %46, align 8, !dbg !1593
  br label %101, !dbg !1593

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1593
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1593
  store ptr %49, ptr %7, align 8, !dbg !1593
  %50 = load i32, ptr %48, align 8, !dbg !1593
  %51 = trunc i32 %50 to i16, !dbg !1593
  %52 = load i32, ptr %11, align 4, !dbg !1593
  %53 = sext i32 %52 to i64, !dbg !1593
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1593
  store i16 %51, ptr %54, align 8, !dbg !1593
  br label %101, !dbg !1593

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1593
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1593
  store ptr %57, ptr %7, align 8, !dbg !1593
  %58 = load i32, ptr %56, align 8, !dbg !1593
  %59 = trunc i32 %58 to i16, !dbg !1593
  %60 = load i32, ptr %11, align 4, !dbg !1593
  %61 = sext i32 %60 to i64, !dbg !1593
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1593
  store i16 %59, ptr %62, align 8, !dbg !1593
  br label %101, !dbg !1593

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1593
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1593
  store ptr %65, ptr %7, align 8, !dbg !1593
  %66 = load i32, ptr %64, align 8, !dbg !1593
  %67 = load i32, ptr %11, align 4, !dbg !1593
  %68 = sext i32 %67 to i64, !dbg !1593
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1593
  store i32 %66, ptr %69, align 8, !dbg !1593
  br label %101, !dbg !1593

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1593
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1593
  store ptr %72, ptr %7, align 8, !dbg !1593
  %73 = load i32, ptr %71, align 8, !dbg !1593
  %74 = sext i32 %73 to i64, !dbg !1593
  %75 = load i32, ptr %11, align 4, !dbg !1593
  %76 = sext i32 %75 to i64, !dbg !1593
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1593
  store i64 %74, ptr %77, align 8, !dbg !1593
  br label %101, !dbg !1593

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1593
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1593
  store ptr %80, ptr %7, align 8, !dbg !1593
  %81 = load double, ptr %79, align 8, !dbg !1593
  %82 = fptrunc double %81 to float, !dbg !1593
  %83 = load i32, ptr %11, align 4, !dbg !1593
  %84 = sext i32 %83 to i64, !dbg !1593
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1593
  store float %82, ptr %85, align 8, !dbg !1593
  br label %101, !dbg !1593

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1593
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1593
  store ptr %88, ptr %7, align 8, !dbg !1593
  %89 = load double, ptr %87, align 8, !dbg !1593
  %90 = load i32, ptr %11, align 4, !dbg !1593
  %91 = sext i32 %90 to i64, !dbg !1593
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1593
  store double %89, ptr %92, align 8, !dbg !1593
  br label %101, !dbg !1593

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1593
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1593
  store ptr %95, ptr %7, align 8, !dbg !1593
  %96 = load ptr, ptr %94, align 8, !dbg !1593
  %97 = load i32, ptr %11, align 4, !dbg !1593
  %98 = sext i32 %97 to i64, !dbg !1593
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1593
  store ptr %96, ptr %99, align 8, !dbg !1593
  br label %101, !dbg !1593

100:                                              ; preds = %25
  br label %101, !dbg !1593

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1590

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1595
  %104 = add nsw i32 %103, 1, !dbg !1595
  store i32 %104, ptr %11, align 4, !dbg !1595
  br label %21, !dbg !1595, !llvm.loop !1596

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1597, metadata !DIExpression()), !dbg !1580
  %106 = load ptr, ptr %6, align 8, !dbg !1580
  %107 = load ptr, ptr %106, align 8, !dbg !1580
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 48, !dbg !1580
  %109 = load ptr, ptr %108, align 8, !dbg !1580
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1580
  %111 = load ptr, ptr %4, align 8, !dbg !1580
  %112 = load ptr, ptr %5, align 8, !dbg !1580
  %113 = load ptr, ptr %6, align 8, !dbg !1580
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1580
  store i16 %114, ptr %12, align 2, !dbg !1580
  call void @llvm.va_end(ptr %7), !dbg !1580
  %115 = load i16, ptr %12, align 2, !dbg !1580
  ret i16 %115, !dbg !1580
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1598 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1599, metadata !DIExpression()), !dbg !1600
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1601, metadata !DIExpression()), !dbg !1600
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1602, metadata !DIExpression()), !dbg !1600
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1603, metadata !DIExpression()), !dbg !1600
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1604, metadata !DIExpression()), !dbg !1600
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1605, metadata !DIExpression()), !dbg !1600
  %13 = load ptr, ptr %8, align 8, !dbg !1600
  %14 = load ptr, ptr %13, align 8, !dbg !1600
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1600
  %16 = load ptr, ptr %15, align 8, !dbg !1600
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1600
  %18 = load ptr, ptr %6, align 8, !dbg !1600
  %19 = load ptr, ptr %8, align 8, !dbg !1600
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1600
  store i32 %20, ptr %10, align 4, !dbg !1600
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1606, metadata !DIExpression()), !dbg !1600
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1607, metadata !DIExpression()), !dbg !1609
  store i32 0, ptr %12, align 4, !dbg !1609
  br label %21, !dbg !1609

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1609
  %23 = load i32, ptr %10, align 4, !dbg !1609
  %24 = icmp slt i32 %22, %23, !dbg !1609
  br i1 %24, label %25, label %105, !dbg !1609

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1610
  %27 = sext i32 %26 to i64, !dbg !1610
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1610
  %29 = load i8, ptr %28, align 1, !dbg !1610
  %30 = sext i8 %29 to i32, !dbg !1610
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
  ], !dbg !1610

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1613
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1613
  store ptr %33, ptr %5, align 8, !dbg !1613
  %34 = load i32, ptr %32, align 8, !dbg !1613
  %35 = trunc i32 %34 to i8, !dbg !1613
  %36 = load i32, ptr %12, align 4, !dbg !1613
  %37 = sext i32 %36 to i64, !dbg !1613
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1613
  store i8 %35, ptr %38, align 8, !dbg !1613
  br label %101, !dbg !1613

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1613
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1613
  store ptr %41, ptr %5, align 8, !dbg !1613
  %42 = load i32, ptr %40, align 8, !dbg !1613
  %43 = trunc i32 %42 to i8, !dbg !1613
  %44 = load i32, ptr %12, align 4, !dbg !1613
  %45 = sext i32 %44 to i64, !dbg !1613
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1613
  store i8 %43, ptr %46, align 8, !dbg !1613
  br label %101, !dbg !1613

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1613
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1613
  store ptr %49, ptr %5, align 8, !dbg !1613
  %50 = load i32, ptr %48, align 8, !dbg !1613
  %51 = trunc i32 %50 to i16, !dbg !1613
  %52 = load i32, ptr %12, align 4, !dbg !1613
  %53 = sext i32 %52 to i64, !dbg !1613
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1613
  store i16 %51, ptr %54, align 8, !dbg !1613
  br label %101, !dbg !1613

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1613
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1613
  store ptr %57, ptr %5, align 8, !dbg !1613
  %58 = load i32, ptr %56, align 8, !dbg !1613
  %59 = trunc i32 %58 to i16, !dbg !1613
  %60 = load i32, ptr %12, align 4, !dbg !1613
  %61 = sext i32 %60 to i64, !dbg !1613
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1613
  store i16 %59, ptr %62, align 8, !dbg !1613
  br label %101, !dbg !1613

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1613
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1613
  store ptr %65, ptr %5, align 8, !dbg !1613
  %66 = load i32, ptr %64, align 8, !dbg !1613
  %67 = load i32, ptr %12, align 4, !dbg !1613
  %68 = sext i32 %67 to i64, !dbg !1613
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1613
  store i32 %66, ptr %69, align 8, !dbg !1613
  br label %101, !dbg !1613

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1613
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1613
  store ptr %72, ptr %5, align 8, !dbg !1613
  %73 = load i32, ptr %71, align 8, !dbg !1613
  %74 = sext i32 %73 to i64, !dbg !1613
  %75 = load i32, ptr %12, align 4, !dbg !1613
  %76 = sext i32 %75 to i64, !dbg !1613
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1613
  store i64 %74, ptr %77, align 8, !dbg !1613
  br label %101, !dbg !1613

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1613
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1613
  store ptr %80, ptr %5, align 8, !dbg !1613
  %81 = load double, ptr %79, align 8, !dbg !1613
  %82 = fptrunc double %81 to float, !dbg !1613
  %83 = load i32, ptr %12, align 4, !dbg !1613
  %84 = sext i32 %83 to i64, !dbg !1613
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1613
  store float %82, ptr %85, align 8, !dbg !1613
  br label %101, !dbg !1613

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1613
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1613
  store ptr %88, ptr %5, align 8, !dbg !1613
  %89 = load double, ptr %87, align 8, !dbg !1613
  %90 = load i32, ptr %12, align 4, !dbg !1613
  %91 = sext i32 %90 to i64, !dbg !1613
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1613
  store double %89, ptr %92, align 8, !dbg !1613
  br label %101, !dbg !1613

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1613
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1613
  store ptr %95, ptr %5, align 8, !dbg !1613
  %96 = load ptr, ptr %94, align 8, !dbg !1613
  %97 = load i32, ptr %12, align 4, !dbg !1613
  %98 = sext i32 %97 to i64, !dbg !1613
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1613
  store ptr %96, ptr %99, align 8, !dbg !1613
  br label %101, !dbg !1613

100:                                              ; preds = %25
  br label %101, !dbg !1613

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1610

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1615
  %104 = add nsw i32 %103, 1, !dbg !1615
  store i32 %104, ptr %12, align 4, !dbg !1615
  br label %21, !dbg !1615, !llvm.loop !1616

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1600
  %107 = load ptr, ptr %106, align 8, !dbg !1600
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 48, !dbg !1600
  %109 = load ptr, ptr %108, align 8, !dbg !1600
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1600
  %111 = load ptr, ptr %6, align 8, !dbg !1600
  %112 = load ptr, ptr %7, align 8, !dbg !1600
  %113 = load ptr, ptr %8, align 8, !dbg !1600
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1600
  ret i16 %114, !dbg !1600
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1617 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i16, align 2
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1618, metadata !DIExpression()), !dbg !1619
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1620, metadata !DIExpression()), !dbg !1619
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1621, metadata !DIExpression()), !dbg !1619
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1622, metadata !DIExpression()), !dbg !1619
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1623, metadata !DIExpression()), !dbg !1619
  call void @llvm.va_start(ptr %9), !dbg !1619
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1624, metadata !DIExpression()), !dbg !1619
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1625, metadata !DIExpression()), !dbg !1619
  %15 = load ptr, ptr %8, align 8, !dbg !1619
  %16 = load ptr, ptr %15, align 8, !dbg !1619
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1619
  %18 = load ptr, ptr %17, align 8, !dbg !1619
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1619
  %20 = load ptr, ptr %5, align 8, !dbg !1619
  %21 = load ptr, ptr %8, align 8, !dbg !1619
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1619
  store i32 %22, ptr %11, align 4, !dbg !1619
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1626, metadata !DIExpression()), !dbg !1619
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1627, metadata !DIExpression()), !dbg !1629
  store i32 0, ptr %13, align 4, !dbg !1629
  br label %23, !dbg !1629

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1629
  %25 = load i32, ptr %11, align 4, !dbg !1629
  %26 = icmp slt i32 %24, %25, !dbg !1629
  br i1 %26, label %27, label %107, !dbg !1629

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1630
  %29 = sext i32 %28 to i64, !dbg !1630
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1630
  %31 = load i8, ptr %30, align 1, !dbg !1630
  %32 = sext i8 %31 to i32, !dbg !1630
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
  ], !dbg !1630

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1633
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1633
  store ptr %35, ptr %9, align 8, !dbg !1633
  %36 = load i32, ptr %34, align 8, !dbg !1633
  %37 = trunc i32 %36 to i8, !dbg !1633
  %38 = load i32, ptr %13, align 4, !dbg !1633
  %39 = sext i32 %38 to i64, !dbg !1633
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1633
  store i8 %37, ptr %40, align 8, !dbg !1633
  br label %103, !dbg !1633

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1633
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1633
  store ptr %43, ptr %9, align 8, !dbg !1633
  %44 = load i32, ptr %42, align 8, !dbg !1633
  %45 = trunc i32 %44 to i8, !dbg !1633
  %46 = load i32, ptr %13, align 4, !dbg !1633
  %47 = sext i32 %46 to i64, !dbg !1633
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1633
  store i8 %45, ptr %48, align 8, !dbg !1633
  br label %103, !dbg !1633

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1633
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1633
  store ptr %51, ptr %9, align 8, !dbg !1633
  %52 = load i32, ptr %50, align 8, !dbg !1633
  %53 = trunc i32 %52 to i16, !dbg !1633
  %54 = load i32, ptr %13, align 4, !dbg !1633
  %55 = sext i32 %54 to i64, !dbg !1633
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1633
  store i16 %53, ptr %56, align 8, !dbg !1633
  br label %103, !dbg !1633

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1633
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1633
  store ptr %59, ptr %9, align 8, !dbg !1633
  %60 = load i32, ptr %58, align 8, !dbg !1633
  %61 = trunc i32 %60 to i16, !dbg !1633
  %62 = load i32, ptr %13, align 4, !dbg !1633
  %63 = sext i32 %62 to i64, !dbg !1633
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1633
  store i16 %61, ptr %64, align 8, !dbg !1633
  br label %103, !dbg !1633

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1633
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1633
  store ptr %67, ptr %9, align 8, !dbg !1633
  %68 = load i32, ptr %66, align 8, !dbg !1633
  %69 = load i32, ptr %13, align 4, !dbg !1633
  %70 = sext i32 %69 to i64, !dbg !1633
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1633
  store i32 %68, ptr %71, align 8, !dbg !1633
  br label %103, !dbg !1633

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1633
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1633
  store ptr %74, ptr %9, align 8, !dbg !1633
  %75 = load i32, ptr %73, align 8, !dbg !1633
  %76 = sext i32 %75 to i64, !dbg !1633
  %77 = load i32, ptr %13, align 4, !dbg !1633
  %78 = sext i32 %77 to i64, !dbg !1633
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1633
  store i64 %76, ptr %79, align 8, !dbg !1633
  br label %103, !dbg !1633

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1633
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1633
  store ptr %82, ptr %9, align 8, !dbg !1633
  %83 = load double, ptr %81, align 8, !dbg !1633
  %84 = fptrunc double %83 to float, !dbg !1633
  %85 = load i32, ptr %13, align 4, !dbg !1633
  %86 = sext i32 %85 to i64, !dbg !1633
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1633
  store float %84, ptr %87, align 8, !dbg !1633
  br label %103, !dbg !1633

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1633
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1633
  store ptr %90, ptr %9, align 8, !dbg !1633
  %91 = load double, ptr %89, align 8, !dbg !1633
  %92 = load i32, ptr %13, align 4, !dbg !1633
  %93 = sext i32 %92 to i64, !dbg !1633
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1633
  store double %91, ptr %94, align 8, !dbg !1633
  br label %103, !dbg !1633

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1633
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1633
  store ptr %97, ptr %9, align 8, !dbg !1633
  %98 = load ptr, ptr %96, align 8, !dbg !1633
  %99 = load i32, ptr %13, align 4, !dbg !1633
  %100 = sext i32 %99 to i64, !dbg !1633
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1633
  store ptr %98, ptr %101, align 8, !dbg !1633
  br label %103, !dbg !1633

102:                                              ; preds = %27
  br label %103, !dbg !1633

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1630

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1635
  %106 = add nsw i32 %105, 1, !dbg !1635
  store i32 %106, ptr %13, align 4, !dbg !1635
  br label %23, !dbg !1635, !llvm.loop !1636

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1637, metadata !DIExpression()), !dbg !1619
  %108 = load ptr, ptr %8, align 8, !dbg !1619
  %109 = load ptr, ptr %108, align 8, !dbg !1619
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 78, !dbg !1619
  %111 = load ptr, ptr %110, align 8, !dbg !1619
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1619
  %113 = load ptr, ptr %5, align 8, !dbg !1619
  %114 = load ptr, ptr %6, align 8, !dbg !1619
  %115 = load ptr, ptr %7, align 8, !dbg !1619
  %116 = load ptr, ptr %8, align 8, !dbg !1619
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1619
  store i16 %117, ptr %14, align 2, !dbg !1619
  call void @llvm.va_end(ptr %9), !dbg !1619
  %118 = load i16, ptr %14, align 2, !dbg !1619
  ret i16 %118, !dbg !1619
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1638 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1639, metadata !DIExpression()), !dbg !1640
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1641, metadata !DIExpression()), !dbg !1640
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1642, metadata !DIExpression()), !dbg !1640
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1643, metadata !DIExpression()), !dbg !1640
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1644, metadata !DIExpression()), !dbg !1640
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1645, metadata !DIExpression()), !dbg !1640
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1646, metadata !DIExpression()), !dbg !1640
  %15 = load ptr, ptr %10, align 8, !dbg !1640
  %16 = load ptr, ptr %15, align 8, !dbg !1640
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1640
  %18 = load ptr, ptr %17, align 8, !dbg !1640
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1640
  %20 = load ptr, ptr %7, align 8, !dbg !1640
  %21 = load ptr, ptr %10, align 8, !dbg !1640
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1640
  store i32 %22, ptr %12, align 4, !dbg !1640
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1647, metadata !DIExpression()), !dbg !1640
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1648, metadata !DIExpression()), !dbg !1650
  store i32 0, ptr %14, align 4, !dbg !1650
  br label %23, !dbg !1650

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1650
  %25 = load i32, ptr %12, align 4, !dbg !1650
  %26 = icmp slt i32 %24, %25, !dbg !1650
  br i1 %26, label %27, label %107, !dbg !1650

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1651
  %29 = sext i32 %28 to i64, !dbg !1651
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1651
  %31 = load i8, ptr %30, align 1, !dbg !1651
  %32 = sext i8 %31 to i32, !dbg !1651
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
  ], !dbg !1651

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1654
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1654
  store ptr %35, ptr %6, align 8, !dbg !1654
  %36 = load i32, ptr %34, align 8, !dbg !1654
  %37 = trunc i32 %36 to i8, !dbg !1654
  %38 = load i32, ptr %14, align 4, !dbg !1654
  %39 = sext i32 %38 to i64, !dbg !1654
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1654
  store i8 %37, ptr %40, align 8, !dbg !1654
  br label %103, !dbg !1654

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1654
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1654
  store ptr %43, ptr %6, align 8, !dbg !1654
  %44 = load i32, ptr %42, align 8, !dbg !1654
  %45 = trunc i32 %44 to i8, !dbg !1654
  %46 = load i32, ptr %14, align 4, !dbg !1654
  %47 = sext i32 %46 to i64, !dbg !1654
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1654
  store i8 %45, ptr %48, align 8, !dbg !1654
  br label %103, !dbg !1654

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1654
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1654
  store ptr %51, ptr %6, align 8, !dbg !1654
  %52 = load i32, ptr %50, align 8, !dbg !1654
  %53 = trunc i32 %52 to i16, !dbg !1654
  %54 = load i32, ptr %14, align 4, !dbg !1654
  %55 = sext i32 %54 to i64, !dbg !1654
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1654
  store i16 %53, ptr %56, align 8, !dbg !1654
  br label %103, !dbg !1654

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1654
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1654
  store ptr %59, ptr %6, align 8, !dbg !1654
  %60 = load i32, ptr %58, align 8, !dbg !1654
  %61 = trunc i32 %60 to i16, !dbg !1654
  %62 = load i32, ptr %14, align 4, !dbg !1654
  %63 = sext i32 %62 to i64, !dbg !1654
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1654
  store i16 %61, ptr %64, align 8, !dbg !1654
  br label %103, !dbg !1654

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1654
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1654
  store ptr %67, ptr %6, align 8, !dbg !1654
  %68 = load i32, ptr %66, align 8, !dbg !1654
  %69 = load i32, ptr %14, align 4, !dbg !1654
  %70 = sext i32 %69 to i64, !dbg !1654
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1654
  store i32 %68, ptr %71, align 8, !dbg !1654
  br label %103, !dbg !1654

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1654
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1654
  store ptr %74, ptr %6, align 8, !dbg !1654
  %75 = load i32, ptr %73, align 8, !dbg !1654
  %76 = sext i32 %75 to i64, !dbg !1654
  %77 = load i32, ptr %14, align 4, !dbg !1654
  %78 = sext i32 %77 to i64, !dbg !1654
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1654
  store i64 %76, ptr %79, align 8, !dbg !1654
  br label %103, !dbg !1654

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1654
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1654
  store ptr %82, ptr %6, align 8, !dbg !1654
  %83 = load double, ptr %81, align 8, !dbg !1654
  %84 = fptrunc double %83 to float, !dbg !1654
  %85 = load i32, ptr %14, align 4, !dbg !1654
  %86 = sext i32 %85 to i64, !dbg !1654
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1654
  store float %84, ptr %87, align 8, !dbg !1654
  br label %103, !dbg !1654

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1654
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1654
  store ptr %90, ptr %6, align 8, !dbg !1654
  %91 = load double, ptr %89, align 8, !dbg !1654
  %92 = load i32, ptr %14, align 4, !dbg !1654
  %93 = sext i32 %92 to i64, !dbg !1654
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1654
  store double %91, ptr %94, align 8, !dbg !1654
  br label %103, !dbg !1654

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1654
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1654
  store ptr %97, ptr %6, align 8, !dbg !1654
  %98 = load ptr, ptr %96, align 8, !dbg !1654
  %99 = load i32, ptr %14, align 4, !dbg !1654
  %100 = sext i32 %99 to i64, !dbg !1654
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1654
  store ptr %98, ptr %101, align 8, !dbg !1654
  br label %103, !dbg !1654

102:                                              ; preds = %27
  br label %103, !dbg !1654

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1651

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1656
  %106 = add nsw i32 %105, 1, !dbg !1656
  store i32 %106, ptr %14, align 4, !dbg !1656
  br label %23, !dbg !1656, !llvm.loop !1657

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1640
  %109 = load ptr, ptr %108, align 8, !dbg !1640
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 78, !dbg !1640
  %111 = load ptr, ptr %110, align 8, !dbg !1640
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1640
  %113 = load ptr, ptr %7, align 8, !dbg !1640
  %114 = load ptr, ptr %8, align 8, !dbg !1640
  %115 = load ptr, ptr %9, align 8, !dbg !1640
  %116 = load ptr, ptr %10, align 8, !dbg !1640
  %117 = call i16 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1640
  ret i16 %117, !dbg !1640
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1658 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i16, align 2
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1659, metadata !DIExpression()), !dbg !1660
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1661, metadata !DIExpression()), !dbg !1660
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1662, metadata !DIExpression()), !dbg !1660
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1663, metadata !DIExpression()), !dbg !1660
  call void @llvm.va_start(ptr %7), !dbg !1660
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1664, metadata !DIExpression()), !dbg !1660
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1665, metadata !DIExpression()), !dbg !1660
  %13 = load ptr, ptr %6, align 8, !dbg !1660
  %14 = load ptr, ptr %13, align 8, !dbg !1660
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1660
  %16 = load ptr, ptr %15, align 8, !dbg !1660
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1660
  %18 = load ptr, ptr %4, align 8, !dbg !1660
  %19 = load ptr, ptr %6, align 8, !dbg !1660
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1660
  store i32 %20, ptr %9, align 4, !dbg !1660
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1666, metadata !DIExpression()), !dbg !1660
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1667, metadata !DIExpression()), !dbg !1669
  store i32 0, ptr %11, align 4, !dbg !1669
  br label %21, !dbg !1669

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1669
  %23 = load i32, ptr %9, align 4, !dbg !1669
  %24 = icmp slt i32 %22, %23, !dbg !1669
  br i1 %24, label %25, label %105, !dbg !1669

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1670
  %27 = sext i32 %26 to i64, !dbg !1670
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1670
  %29 = load i8, ptr %28, align 1, !dbg !1670
  %30 = sext i8 %29 to i32, !dbg !1670
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
  ], !dbg !1670

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1673
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1673
  store ptr %33, ptr %7, align 8, !dbg !1673
  %34 = load i32, ptr %32, align 8, !dbg !1673
  %35 = trunc i32 %34 to i8, !dbg !1673
  %36 = load i32, ptr %11, align 4, !dbg !1673
  %37 = sext i32 %36 to i64, !dbg !1673
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1673
  store i8 %35, ptr %38, align 8, !dbg !1673
  br label %101, !dbg !1673

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1673
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1673
  store ptr %41, ptr %7, align 8, !dbg !1673
  %42 = load i32, ptr %40, align 8, !dbg !1673
  %43 = trunc i32 %42 to i8, !dbg !1673
  %44 = load i32, ptr %11, align 4, !dbg !1673
  %45 = sext i32 %44 to i64, !dbg !1673
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1673
  store i8 %43, ptr %46, align 8, !dbg !1673
  br label %101, !dbg !1673

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1673
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1673
  store ptr %49, ptr %7, align 8, !dbg !1673
  %50 = load i32, ptr %48, align 8, !dbg !1673
  %51 = trunc i32 %50 to i16, !dbg !1673
  %52 = load i32, ptr %11, align 4, !dbg !1673
  %53 = sext i32 %52 to i64, !dbg !1673
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1673
  store i16 %51, ptr %54, align 8, !dbg !1673
  br label %101, !dbg !1673

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1673
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1673
  store ptr %57, ptr %7, align 8, !dbg !1673
  %58 = load i32, ptr %56, align 8, !dbg !1673
  %59 = trunc i32 %58 to i16, !dbg !1673
  %60 = load i32, ptr %11, align 4, !dbg !1673
  %61 = sext i32 %60 to i64, !dbg !1673
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1673
  store i16 %59, ptr %62, align 8, !dbg !1673
  br label %101, !dbg !1673

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1673
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1673
  store ptr %65, ptr %7, align 8, !dbg !1673
  %66 = load i32, ptr %64, align 8, !dbg !1673
  %67 = load i32, ptr %11, align 4, !dbg !1673
  %68 = sext i32 %67 to i64, !dbg !1673
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1673
  store i32 %66, ptr %69, align 8, !dbg !1673
  br label %101, !dbg !1673

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1673
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1673
  store ptr %72, ptr %7, align 8, !dbg !1673
  %73 = load i32, ptr %71, align 8, !dbg !1673
  %74 = sext i32 %73 to i64, !dbg !1673
  %75 = load i32, ptr %11, align 4, !dbg !1673
  %76 = sext i32 %75 to i64, !dbg !1673
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1673
  store i64 %74, ptr %77, align 8, !dbg !1673
  br label %101, !dbg !1673

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1673
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1673
  store ptr %80, ptr %7, align 8, !dbg !1673
  %81 = load double, ptr %79, align 8, !dbg !1673
  %82 = fptrunc double %81 to float, !dbg !1673
  %83 = load i32, ptr %11, align 4, !dbg !1673
  %84 = sext i32 %83 to i64, !dbg !1673
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1673
  store float %82, ptr %85, align 8, !dbg !1673
  br label %101, !dbg !1673

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1673
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1673
  store ptr %88, ptr %7, align 8, !dbg !1673
  %89 = load double, ptr %87, align 8, !dbg !1673
  %90 = load i32, ptr %11, align 4, !dbg !1673
  %91 = sext i32 %90 to i64, !dbg !1673
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1673
  store double %89, ptr %92, align 8, !dbg !1673
  br label %101, !dbg !1673

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1673
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1673
  store ptr %95, ptr %7, align 8, !dbg !1673
  %96 = load ptr, ptr %94, align 8, !dbg !1673
  %97 = load i32, ptr %11, align 4, !dbg !1673
  %98 = sext i32 %97 to i64, !dbg !1673
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1673
  store ptr %96, ptr %99, align 8, !dbg !1673
  br label %101, !dbg !1673

100:                                              ; preds = %25
  br label %101, !dbg !1673

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1670

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1675
  %104 = add nsw i32 %103, 1, !dbg !1675
  store i32 %104, ptr %11, align 4, !dbg !1675
  br label %21, !dbg !1675, !llvm.loop !1676

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1677, metadata !DIExpression()), !dbg !1660
  %106 = load ptr, ptr %6, align 8, !dbg !1660
  %107 = load ptr, ptr %106, align 8, !dbg !1660
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 128, !dbg !1660
  %109 = load ptr, ptr %108, align 8, !dbg !1660
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1660
  %111 = load ptr, ptr %4, align 8, !dbg !1660
  %112 = load ptr, ptr %5, align 8, !dbg !1660
  %113 = load ptr, ptr %6, align 8, !dbg !1660
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1660
  store i16 %114, ptr %12, align 2, !dbg !1660
  call void @llvm.va_end(ptr %7), !dbg !1660
  %115 = load i16, ptr %12, align 2, !dbg !1660
  ret i16 %115, !dbg !1660
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1678 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1679, metadata !DIExpression()), !dbg !1680
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1681, metadata !DIExpression()), !dbg !1680
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1682, metadata !DIExpression()), !dbg !1680
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1683, metadata !DIExpression()), !dbg !1680
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1684, metadata !DIExpression()), !dbg !1680
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1685, metadata !DIExpression()), !dbg !1680
  %13 = load ptr, ptr %8, align 8, !dbg !1680
  %14 = load ptr, ptr %13, align 8, !dbg !1680
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1680
  %16 = load ptr, ptr %15, align 8, !dbg !1680
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1680
  %18 = load ptr, ptr %6, align 8, !dbg !1680
  %19 = load ptr, ptr %8, align 8, !dbg !1680
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1680
  store i32 %20, ptr %10, align 4, !dbg !1680
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1686, metadata !DIExpression()), !dbg !1680
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1687, metadata !DIExpression()), !dbg !1689
  store i32 0, ptr %12, align 4, !dbg !1689
  br label %21, !dbg !1689

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1689
  %23 = load i32, ptr %10, align 4, !dbg !1689
  %24 = icmp slt i32 %22, %23, !dbg !1689
  br i1 %24, label %25, label %105, !dbg !1689

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1690
  %27 = sext i32 %26 to i64, !dbg !1690
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1690
  %29 = load i8, ptr %28, align 1, !dbg !1690
  %30 = sext i8 %29 to i32, !dbg !1690
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
  ], !dbg !1690

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1693
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1693
  store ptr %33, ptr %5, align 8, !dbg !1693
  %34 = load i32, ptr %32, align 8, !dbg !1693
  %35 = trunc i32 %34 to i8, !dbg !1693
  %36 = load i32, ptr %12, align 4, !dbg !1693
  %37 = sext i32 %36 to i64, !dbg !1693
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1693
  store i8 %35, ptr %38, align 8, !dbg !1693
  br label %101, !dbg !1693

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1693
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1693
  store ptr %41, ptr %5, align 8, !dbg !1693
  %42 = load i32, ptr %40, align 8, !dbg !1693
  %43 = trunc i32 %42 to i8, !dbg !1693
  %44 = load i32, ptr %12, align 4, !dbg !1693
  %45 = sext i32 %44 to i64, !dbg !1693
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1693
  store i8 %43, ptr %46, align 8, !dbg !1693
  br label %101, !dbg !1693

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1693
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1693
  store ptr %49, ptr %5, align 8, !dbg !1693
  %50 = load i32, ptr %48, align 8, !dbg !1693
  %51 = trunc i32 %50 to i16, !dbg !1693
  %52 = load i32, ptr %12, align 4, !dbg !1693
  %53 = sext i32 %52 to i64, !dbg !1693
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1693
  store i16 %51, ptr %54, align 8, !dbg !1693
  br label %101, !dbg !1693

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1693
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1693
  store ptr %57, ptr %5, align 8, !dbg !1693
  %58 = load i32, ptr %56, align 8, !dbg !1693
  %59 = trunc i32 %58 to i16, !dbg !1693
  %60 = load i32, ptr %12, align 4, !dbg !1693
  %61 = sext i32 %60 to i64, !dbg !1693
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1693
  store i16 %59, ptr %62, align 8, !dbg !1693
  br label %101, !dbg !1693

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1693
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1693
  store ptr %65, ptr %5, align 8, !dbg !1693
  %66 = load i32, ptr %64, align 8, !dbg !1693
  %67 = load i32, ptr %12, align 4, !dbg !1693
  %68 = sext i32 %67 to i64, !dbg !1693
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1693
  store i32 %66, ptr %69, align 8, !dbg !1693
  br label %101, !dbg !1693

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1693
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1693
  store ptr %72, ptr %5, align 8, !dbg !1693
  %73 = load i32, ptr %71, align 8, !dbg !1693
  %74 = sext i32 %73 to i64, !dbg !1693
  %75 = load i32, ptr %12, align 4, !dbg !1693
  %76 = sext i32 %75 to i64, !dbg !1693
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1693
  store i64 %74, ptr %77, align 8, !dbg !1693
  br label %101, !dbg !1693

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1693
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1693
  store ptr %80, ptr %5, align 8, !dbg !1693
  %81 = load double, ptr %79, align 8, !dbg !1693
  %82 = fptrunc double %81 to float, !dbg !1693
  %83 = load i32, ptr %12, align 4, !dbg !1693
  %84 = sext i32 %83 to i64, !dbg !1693
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1693
  store float %82, ptr %85, align 8, !dbg !1693
  br label %101, !dbg !1693

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1693
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1693
  store ptr %88, ptr %5, align 8, !dbg !1693
  %89 = load double, ptr %87, align 8, !dbg !1693
  %90 = load i32, ptr %12, align 4, !dbg !1693
  %91 = sext i32 %90 to i64, !dbg !1693
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1693
  store double %89, ptr %92, align 8, !dbg !1693
  br label %101, !dbg !1693

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1693
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1693
  store ptr %95, ptr %5, align 8, !dbg !1693
  %96 = load ptr, ptr %94, align 8, !dbg !1693
  %97 = load i32, ptr %12, align 4, !dbg !1693
  %98 = sext i32 %97 to i64, !dbg !1693
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1693
  store ptr %96, ptr %99, align 8, !dbg !1693
  br label %101, !dbg !1693

100:                                              ; preds = %25
  br label %101, !dbg !1693

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1690

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1695
  %104 = add nsw i32 %103, 1, !dbg !1695
  store i32 %104, ptr %12, align 4, !dbg !1695
  br label %21, !dbg !1695, !llvm.loop !1696

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1680
  %107 = load ptr, ptr %106, align 8, !dbg !1680
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 128, !dbg !1680
  %109 = load ptr, ptr %108, align 8, !dbg !1680
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1680
  %111 = load ptr, ptr %6, align 8, !dbg !1680
  %112 = load ptr, ptr %7, align 8, !dbg !1680
  %113 = load ptr, ptr %8, align 8, !dbg !1680
  %114 = call i16 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1680
  ret i16 %114, !dbg !1680
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1697 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1698, metadata !DIExpression()), !dbg !1699
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1700, metadata !DIExpression()), !dbg !1699
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1701, metadata !DIExpression()), !dbg !1699
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1702, metadata !DIExpression()), !dbg !1699
  call void @llvm.va_start(ptr %7), !dbg !1699
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1703, metadata !DIExpression()), !dbg !1699
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1704, metadata !DIExpression()), !dbg !1699
  %13 = load ptr, ptr %6, align 8, !dbg !1699
  %14 = load ptr, ptr %13, align 8, !dbg !1699
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1699
  %16 = load ptr, ptr %15, align 8, !dbg !1699
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1699
  %18 = load ptr, ptr %4, align 8, !dbg !1699
  %19 = load ptr, ptr %6, align 8, !dbg !1699
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1699
  store i32 %20, ptr %9, align 4, !dbg !1699
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1705, metadata !DIExpression()), !dbg !1699
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1706, metadata !DIExpression()), !dbg !1708
  store i32 0, ptr %11, align 4, !dbg !1708
  br label %21, !dbg !1708

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1708
  %23 = load i32, ptr %9, align 4, !dbg !1708
  %24 = icmp slt i32 %22, %23, !dbg !1708
  br i1 %24, label %25, label %105, !dbg !1708

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1709
  %27 = sext i32 %26 to i64, !dbg !1709
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1709
  %29 = load i8, ptr %28, align 1, !dbg !1709
  %30 = sext i8 %29 to i32, !dbg !1709
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
  ], !dbg !1709

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1712
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1712
  store ptr %33, ptr %7, align 8, !dbg !1712
  %34 = load i32, ptr %32, align 8, !dbg !1712
  %35 = trunc i32 %34 to i8, !dbg !1712
  %36 = load i32, ptr %11, align 4, !dbg !1712
  %37 = sext i32 %36 to i64, !dbg !1712
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1712
  store i8 %35, ptr %38, align 8, !dbg !1712
  br label %101, !dbg !1712

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1712
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1712
  store ptr %41, ptr %7, align 8, !dbg !1712
  %42 = load i32, ptr %40, align 8, !dbg !1712
  %43 = trunc i32 %42 to i8, !dbg !1712
  %44 = load i32, ptr %11, align 4, !dbg !1712
  %45 = sext i32 %44 to i64, !dbg !1712
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1712
  store i8 %43, ptr %46, align 8, !dbg !1712
  br label %101, !dbg !1712

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1712
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1712
  store ptr %49, ptr %7, align 8, !dbg !1712
  %50 = load i32, ptr %48, align 8, !dbg !1712
  %51 = trunc i32 %50 to i16, !dbg !1712
  %52 = load i32, ptr %11, align 4, !dbg !1712
  %53 = sext i32 %52 to i64, !dbg !1712
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1712
  store i16 %51, ptr %54, align 8, !dbg !1712
  br label %101, !dbg !1712

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1712
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1712
  store ptr %57, ptr %7, align 8, !dbg !1712
  %58 = load i32, ptr %56, align 8, !dbg !1712
  %59 = trunc i32 %58 to i16, !dbg !1712
  %60 = load i32, ptr %11, align 4, !dbg !1712
  %61 = sext i32 %60 to i64, !dbg !1712
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1712
  store i16 %59, ptr %62, align 8, !dbg !1712
  br label %101, !dbg !1712

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1712
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1712
  store ptr %65, ptr %7, align 8, !dbg !1712
  %66 = load i32, ptr %64, align 8, !dbg !1712
  %67 = load i32, ptr %11, align 4, !dbg !1712
  %68 = sext i32 %67 to i64, !dbg !1712
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1712
  store i32 %66, ptr %69, align 8, !dbg !1712
  br label %101, !dbg !1712

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1712
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1712
  store ptr %72, ptr %7, align 8, !dbg !1712
  %73 = load i32, ptr %71, align 8, !dbg !1712
  %74 = sext i32 %73 to i64, !dbg !1712
  %75 = load i32, ptr %11, align 4, !dbg !1712
  %76 = sext i32 %75 to i64, !dbg !1712
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1712
  store i64 %74, ptr %77, align 8, !dbg !1712
  br label %101, !dbg !1712

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1712
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1712
  store ptr %80, ptr %7, align 8, !dbg !1712
  %81 = load double, ptr %79, align 8, !dbg !1712
  %82 = fptrunc double %81 to float, !dbg !1712
  %83 = load i32, ptr %11, align 4, !dbg !1712
  %84 = sext i32 %83 to i64, !dbg !1712
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1712
  store float %82, ptr %85, align 8, !dbg !1712
  br label %101, !dbg !1712

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1712
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1712
  store ptr %88, ptr %7, align 8, !dbg !1712
  %89 = load double, ptr %87, align 8, !dbg !1712
  %90 = load i32, ptr %11, align 4, !dbg !1712
  %91 = sext i32 %90 to i64, !dbg !1712
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1712
  store double %89, ptr %92, align 8, !dbg !1712
  br label %101, !dbg !1712

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1712
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1712
  store ptr %95, ptr %7, align 8, !dbg !1712
  %96 = load ptr, ptr %94, align 8, !dbg !1712
  %97 = load i32, ptr %11, align 4, !dbg !1712
  %98 = sext i32 %97 to i64, !dbg !1712
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1712
  store ptr %96, ptr %99, align 8, !dbg !1712
  br label %101, !dbg !1712

100:                                              ; preds = %25
  br label %101, !dbg !1712

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1709

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1714
  %104 = add nsw i32 %103, 1, !dbg !1714
  store i32 %104, ptr %11, align 4, !dbg !1714
  br label %21, !dbg !1714, !llvm.loop !1715

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1716, metadata !DIExpression()), !dbg !1699
  %106 = load ptr, ptr %6, align 8, !dbg !1699
  %107 = load ptr, ptr %106, align 8, !dbg !1699
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 51, !dbg !1699
  %109 = load ptr, ptr %108, align 8, !dbg !1699
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1699
  %111 = load ptr, ptr %4, align 8, !dbg !1699
  %112 = load ptr, ptr %5, align 8, !dbg !1699
  %113 = load ptr, ptr %6, align 8, !dbg !1699
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1699
  store i32 %114, ptr %12, align 4, !dbg !1699
  call void @llvm.va_end(ptr %7), !dbg !1699
  %115 = load i32, ptr %12, align 4, !dbg !1699
  ret i32 %115, !dbg !1699
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1717 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1718, metadata !DIExpression()), !dbg !1719
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1720, metadata !DIExpression()), !dbg !1719
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1721, metadata !DIExpression()), !dbg !1719
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1722, metadata !DIExpression()), !dbg !1719
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1723, metadata !DIExpression()), !dbg !1719
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1724, metadata !DIExpression()), !dbg !1719
  %13 = load ptr, ptr %8, align 8, !dbg !1719
  %14 = load ptr, ptr %13, align 8, !dbg !1719
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1719
  %16 = load ptr, ptr %15, align 8, !dbg !1719
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1719
  %18 = load ptr, ptr %6, align 8, !dbg !1719
  %19 = load ptr, ptr %8, align 8, !dbg !1719
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1719
  store i32 %20, ptr %10, align 4, !dbg !1719
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1725, metadata !DIExpression()), !dbg !1719
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1726, metadata !DIExpression()), !dbg !1728
  store i32 0, ptr %12, align 4, !dbg !1728
  br label %21, !dbg !1728

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1728
  %23 = load i32, ptr %10, align 4, !dbg !1728
  %24 = icmp slt i32 %22, %23, !dbg !1728
  br i1 %24, label %25, label %105, !dbg !1728

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1729
  %27 = sext i32 %26 to i64, !dbg !1729
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1729
  %29 = load i8, ptr %28, align 1, !dbg !1729
  %30 = sext i8 %29 to i32, !dbg !1729
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
  ], !dbg !1729

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1732
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1732
  store ptr %33, ptr %5, align 8, !dbg !1732
  %34 = load i32, ptr %32, align 8, !dbg !1732
  %35 = trunc i32 %34 to i8, !dbg !1732
  %36 = load i32, ptr %12, align 4, !dbg !1732
  %37 = sext i32 %36 to i64, !dbg !1732
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1732
  store i8 %35, ptr %38, align 8, !dbg !1732
  br label %101, !dbg !1732

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1732
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1732
  store ptr %41, ptr %5, align 8, !dbg !1732
  %42 = load i32, ptr %40, align 8, !dbg !1732
  %43 = trunc i32 %42 to i8, !dbg !1732
  %44 = load i32, ptr %12, align 4, !dbg !1732
  %45 = sext i32 %44 to i64, !dbg !1732
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1732
  store i8 %43, ptr %46, align 8, !dbg !1732
  br label %101, !dbg !1732

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1732
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1732
  store ptr %49, ptr %5, align 8, !dbg !1732
  %50 = load i32, ptr %48, align 8, !dbg !1732
  %51 = trunc i32 %50 to i16, !dbg !1732
  %52 = load i32, ptr %12, align 4, !dbg !1732
  %53 = sext i32 %52 to i64, !dbg !1732
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1732
  store i16 %51, ptr %54, align 8, !dbg !1732
  br label %101, !dbg !1732

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1732
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1732
  store ptr %57, ptr %5, align 8, !dbg !1732
  %58 = load i32, ptr %56, align 8, !dbg !1732
  %59 = trunc i32 %58 to i16, !dbg !1732
  %60 = load i32, ptr %12, align 4, !dbg !1732
  %61 = sext i32 %60 to i64, !dbg !1732
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1732
  store i16 %59, ptr %62, align 8, !dbg !1732
  br label %101, !dbg !1732

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1732
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1732
  store ptr %65, ptr %5, align 8, !dbg !1732
  %66 = load i32, ptr %64, align 8, !dbg !1732
  %67 = load i32, ptr %12, align 4, !dbg !1732
  %68 = sext i32 %67 to i64, !dbg !1732
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1732
  store i32 %66, ptr %69, align 8, !dbg !1732
  br label %101, !dbg !1732

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1732
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1732
  store ptr %72, ptr %5, align 8, !dbg !1732
  %73 = load i32, ptr %71, align 8, !dbg !1732
  %74 = sext i32 %73 to i64, !dbg !1732
  %75 = load i32, ptr %12, align 4, !dbg !1732
  %76 = sext i32 %75 to i64, !dbg !1732
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1732
  store i64 %74, ptr %77, align 8, !dbg !1732
  br label %101, !dbg !1732

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1732
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1732
  store ptr %80, ptr %5, align 8, !dbg !1732
  %81 = load double, ptr %79, align 8, !dbg !1732
  %82 = fptrunc double %81 to float, !dbg !1732
  %83 = load i32, ptr %12, align 4, !dbg !1732
  %84 = sext i32 %83 to i64, !dbg !1732
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1732
  store float %82, ptr %85, align 8, !dbg !1732
  br label %101, !dbg !1732

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1732
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1732
  store ptr %88, ptr %5, align 8, !dbg !1732
  %89 = load double, ptr %87, align 8, !dbg !1732
  %90 = load i32, ptr %12, align 4, !dbg !1732
  %91 = sext i32 %90 to i64, !dbg !1732
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1732
  store double %89, ptr %92, align 8, !dbg !1732
  br label %101, !dbg !1732

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1732
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1732
  store ptr %95, ptr %5, align 8, !dbg !1732
  %96 = load ptr, ptr %94, align 8, !dbg !1732
  %97 = load i32, ptr %12, align 4, !dbg !1732
  %98 = sext i32 %97 to i64, !dbg !1732
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1732
  store ptr %96, ptr %99, align 8, !dbg !1732
  br label %101, !dbg !1732

100:                                              ; preds = %25
  br label %101, !dbg !1732

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1729

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1734
  %104 = add nsw i32 %103, 1, !dbg !1734
  store i32 %104, ptr %12, align 4, !dbg !1734
  br label %21, !dbg !1734, !llvm.loop !1735

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1719
  %107 = load ptr, ptr %106, align 8, !dbg !1719
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 51, !dbg !1719
  %109 = load ptr, ptr %108, align 8, !dbg !1719
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1719
  %111 = load ptr, ptr %6, align 8, !dbg !1719
  %112 = load ptr, ptr %7, align 8, !dbg !1719
  %113 = load ptr, ptr %8, align 8, !dbg !1719
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1719
  ret i32 %114, !dbg !1719
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1736 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1737, metadata !DIExpression()), !dbg !1738
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1739, metadata !DIExpression()), !dbg !1738
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1740, metadata !DIExpression()), !dbg !1738
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1741, metadata !DIExpression()), !dbg !1738
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1742, metadata !DIExpression()), !dbg !1738
  call void @llvm.va_start(ptr %9), !dbg !1738
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1743, metadata !DIExpression()), !dbg !1738
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1744, metadata !DIExpression()), !dbg !1738
  %15 = load ptr, ptr %8, align 8, !dbg !1738
  %16 = load ptr, ptr %15, align 8, !dbg !1738
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1738
  %18 = load ptr, ptr %17, align 8, !dbg !1738
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1738
  %20 = load ptr, ptr %5, align 8, !dbg !1738
  %21 = load ptr, ptr %8, align 8, !dbg !1738
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1738
  store i32 %22, ptr %11, align 4, !dbg !1738
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1745, metadata !DIExpression()), !dbg !1738
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1746, metadata !DIExpression()), !dbg !1748
  store i32 0, ptr %13, align 4, !dbg !1748
  br label %23, !dbg !1748

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1748
  %25 = load i32, ptr %11, align 4, !dbg !1748
  %26 = icmp slt i32 %24, %25, !dbg !1748
  br i1 %26, label %27, label %107, !dbg !1748

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1749
  %29 = sext i32 %28 to i64, !dbg !1749
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1749
  %31 = load i8, ptr %30, align 1, !dbg !1749
  %32 = sext i8 %31 to i32, !dbg !1749
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
  ], !dbg !1749

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1752
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1752
  store ptr %35, ptr %9, align 8, !dbg !1752
  %36 = load i32, ptr %34, align 8, !dbg !1752
  %37 = trunc i32 %36 to i8, !dbg !1752
  %38 = load i32, ptr %13, align 4, !dbg !1752
  %39 = sext i32 %38 to i64, !dbg !1752
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1752
  store i8 %37, ptr %40, align 8, !dbg !1752
  br label %103, !dbg !1752

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1752
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1752
  store ptr %43, ptr %9, align 8, !dbg !1752
  %44 = load i32, ptr %42, align 8, !dbg !1752
  %45 = trunc i32 %44 to i8, !dbg !1752
  %46 = load i32, ptr %13, align 4, !dbg !1752
  %47 = sext i32 %46 to i64, !dbg !1752
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1752
  store i8 %45, ptr %48, align 8, !dbg !1752
  br label %103, !dbg !1752

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1752
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1752
  store ptr %51, ptr %9, align 8, !dbg !1752
  %52 = load i32, ptr %50, align 8, !dbg !1752
  %53 = trunc i32 %52 to i16, !dbg !1752
  %54 = load i32, ptr %13, align 4, !dbg !1752
  %55 = sext i32 %54 to i64, !dbg !1752
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1752
  store i16 %53, ptr %56, align 8, !dbg !1752
  br label %103, !dbg !1752

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1752
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1752
  store ptr %59, ptr %9, align 8, !dbg !1752
  %60 = load i32, ptr %58, align 8, !dbg !1752
  %61 = trunc i32 %60 to i16, !dbg !1752
  %62 = load i32, ptr %13, align 4, !dbg !1752
  %63 = sext i32 %62 to i64, !dbg !1752
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1752
  store i16 %61, ptr %64, align 8, !dbg !1752
  br label %103, !dbg !1752

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1752
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1752
  store ptr %67, ptr %9, align 8, !dbg !1752
  %68 = load i32, ptr %66, align 8, !dbg !1752
  %69 = load i32, ptr %13, align 4, !dbg !1752
  %70 = sext i32 %69 to i64, !dbg !1752
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1752
  store i32 %68, ptr %71, align 8, !dbg !1752
  br label %103, !dbg !1752

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1752
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1752
  store ptr %74, ptr %9, align 8, !dbg !1752
  %75 = load i32, ptr %73, align 8, !dbg !1752
  %76 = sext i32 %75 to i64, !dbg !1752
  %77 = load i32, ptr %13, align 4, !dbg !1752
  %78 = sext i32 %77 to i64, !dbg !1752
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1752
  store i64 %76, ptr %79, align 8, !dbg !1752
  br label %103, !dbg !1752

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1752
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1752
  store ptr %82, ptr %9, align 8, !dbg !1752
  %83 = load double, ptr %81, align 8, !dbg !1752
  %84 = fptrunc double %83 to float, !dbg !1752
  %85 = load i32, ptr %13, align 4, !dbg !1752
  %86 = sext i32 %85 to i64, !dbg !1752
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1752
  store float %84, ptr %87, align 8, !dbg !1752
  br label %103, !dbg !1752

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1752
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1752
  store ptr %90, ptr %9, align 8, !dbg !1752
  %91 = load double, ptr %89, align 8, !dbg !1752
  %92 = load i32, ptr %13, align 4, !dbg !1752
  %93 = sext i32 %92 to i64, !dbg !1752
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1752
  store double %91, ptr %94, align 8, !dbg !1752
  br label %103, !dbg !1752

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1752
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1752
  store ptr %97, ptr %9, align 8, !dbg !1752
  %98 = load ptr, ptr %96, align 8, !dbg !1752
  %99 = load i32, ptr %13, align 4, !dbg !1752
  %100 = sext i32 %99 to i64, !dbg !1752
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1752
  store ptr %98, ptr %101, align 8, !dbg !1752
  br label %103, !dbg !1752

102:                                              ; preds = %27
  br label %103, !dbg !1752

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1749

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1754
  %106 = add nsw i32 %105, 1, !dbg !1754
  store i32 %106, ptr %13, align 4, !dbg !1754
  br label %23, !dbg !1754, !llvm.loop !1755

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1756, metadata !DIExpression()), !dbg !1738
  %108 = load ptr, ptr %8, align 8, !dbg !1738
  %109 = load ptr, ptr %108, align 8, !dbg !1738
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 81, !dbg !1738
  %111 = load ptr, ptr %110, align 8, !dbg !1738
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1738
  %113 = load ptr, ptr %5, align 8, !dbg !1738
  %114 = load ptr, ptr %6, align 8, !dbg !1738
  %115 = load ptr, ptr %7, align 8, !dbg !1738
  %116 = load ptr, ptr %8, align 8, !dbg !1738
  %117 = call i32 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1738
  store i32 %117, ptr %14, align 4, !dbg !1738
  call void @llvm.va_end(ptr %9), !dbg !1738
  %118 = load i32, ptr %14, align 4, !dbg !1738
  ret i32 %118, !dbg !1738
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1757 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1758, metadata !DIExpression()), !dbg !1759
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1760, metadata !DIExpression()), !dbg !1759
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1761, metadata !DIExpression()), !dbg !1759
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1762, metadata !DIExpression()), !dbg !1759
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1763, metadata !DIExpression()), !dbg !1759
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1764, metadata !DIExpression()), !dbg !1759
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1765, metadata !DIExpression()), !dbg !1759
  %15 = load ptr, ptr %10, align 8, !dbg !1759
  %16 = load ptr, ptr %15, align 8, !dbg !1759
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1759
  %18 = load ptr, ptr %17, align 8, !dbg !1759
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1759
  %20 = load ptr, ptr %7, align 8, !dbg !1759
  %21 = load ptr, ptr %10, align 8, !dbg !1759
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1759
  store i32 %22, ptr %12, align 4, !dbg !1759
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1766, metadata !DIExpression()), !dbg !1759
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1767, metadata !DIExpression()), !dbg !1769
  store i32 0, ptr %14, align 4, !dbg !1769
  br label %23, !dbg !1769

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1769
  %25 = load i32, ptr %12, align 4, !dbg !1769
  %26 = icmp slt i32 %24, %25, !dbg !1769
  br i1 %26, label %27, label %107, !dbg !1769

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1770
  %29 = sext i32 %28 to i64, !dbg !1770
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1770
  %31 = load i8, ptr %30, align 1, !dbg !1770
  %32 = sext i8 %31 to i32, !dbg !1770
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
  ], !dbg !1770

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1773
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1773
  store ptr %35, ptr %6, align 8, !dbg !1773
  %36 = load i32, ptr %34, align 8, !dbg !1773
  %37 = trunc i32 %36 to i8, !dbg !1773
  %38 = load i32, ptr %14, align 4, !dbg !1773
  %39 = sext i32 %38 to i64, !dbg !1773
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1773
  store i8 %37, ptr %40, align 8, !dbg !1773
  br label %103, !dbg !1773

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1773
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1773
  store ptr %43, ptr %6, align 8, !dbg !1773
  %44 = load i32, ptr %42, align 8, !dbg !1773
  %45 = trunc i32 %44 to i8, !dbg !1773
  %46 = load i32, ptr %14, align 4, !dbg !1773
  %47 = sext i32 %46 to i64, !dbg !1773
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1773
  store i8 %45, ptr %48, align 8, !dbg !1773
  br label %103, !dbg !1773

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1773
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1773
  store ptr %51, ptr %6, align 8, !dbg !1773
  %52 = load i32, ptr %50, align 8, !dbg !1773
  %53 = trunc i32 %52 to i16, !dbg !1773
  %54 = load i32, ptr %14, align 4, !dbg !1773
  %55 = sext i32 %54 to i64, !dbg !1773
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1773
  store i16 %53, ptr %56, align 8, !dbg !1773
  br label %103, !dbg !1773

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1773
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1773
  store ptr %59, ptr %6, align 8, !dbg !1773
  %60 = load i32, ptr %58, align 8, !dbg !1773
  %61 = trunc i32 %60 to i16, !dbg !1773
  %62 = load i32, ptr %14, align 4, !dbg !1773
  %63 = sext i32 %62 to i64, !dbg !1773
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1773
  store i16 %61, ptr %64, align 8, !dbg !1773
  br label %103, !dbg !1773

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1773
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1773
  store ptr %67, ptr %6, align 8, !dbg !1773
  %68 = load i32, ptr %66, align 8, !dbg !1773
  %69 = load i32, ptr %14, align 4, !dbg !1773
  %70 = sext i32 %69 to i64, !dbg !1773
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1773
  store i32 %68, ptr %71, align 8, !dbg !1773
  br label %103, !dbg !1773

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1773
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1773
  store ptr %74, ptr %6, align 8, !dbg !1773
  %75 = load i32, ptr %73, align 8, !dbg !1773
  %76 = sext i32 %75 to i64, !dbg !1773
  %77 = load i32, ptr %14, align 4, !dbg !1773
  %78 = sext i32 %77 to i64, !dbg !1773
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1773
  store i64 %76, ptr %79, align 8, !dbg !1773
  br label %103, !dbg !1773

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1773
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1773
  store ptr %82, ptr %6, align 8, !dbg !1773
  %83 = load double, ptr %81, align 8, !dbg !1773
  %84 = fptrunc double %83 to float, !dbg !1773
  %85 = load i32, ptr %14, align 4, !dbg !1773
  %86 = sext i32 %85 to i64, !dbg !1773
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1773
  store float %84, ptr %87, align 8, !dbg !1773
  br label %103, !dbg !1773

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1773
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1773
  store ptr %90, ptr %6, align 8, !dbg !1773
  %91 = load double, ptr %89, align 8, !dbg !1773
  %92 = load i32, ptr %14, align 4, !dbg !1773
  %93 = sext i32 %92 to i64, !dbg !1773
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1773
  store double %91, ptr %94, align 8, !dbg !1773
  br label %103, !dbg !1773

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1773
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1773
  store ptr %97, ptr %6, align 8, !dbg !1773
  %98 = load ptr, ptr %96, align 8, !dbg !1773
  %99 = load i32, ptr %14, align 4, !dbg !1773
  %100 = sext i32 %99 to i64, !dbg !1773
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1773
  store ptr %98, ptr %101, align 8, !dbg !1773
  br label %103, !dbg !1773

102:                                              ; preds = %27
  br label %103, !dbg !1773

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1770

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1775
  %106 = add nsw i32 %105, 1, !dbg !1775
  store i32 %106, ptr %14, align 4, !dbg !1775
  br label %23, !dbg !1775, !llvm.loop !1776

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1759
  %109 = load ptr, ptr %108, align 8, !dbg !1759
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 81, !dbg !1759
  %111 = load ptr, ptr %110, align 8, !dbg !1759
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1759
  %113 = load ptr, ptr %7, align 8, !dbg !1759
  %114 = load ptr, ptr %8, align 8, !dbg !1759
  %115 = load ptr, ptr %9, align 8, !dbg !1759
  %116 = load ptr, ptr %10, align 8, !dbg !1759
  %117 = call i32 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1759
  ret i32 %117, !dbg !1759
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1777 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1778, metadata !DIExpression()), !dbg !1779
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1780, metadata !DIExpression()), !dbg !1779
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1781, metadata !DIExpression()), !dbg !1779
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1782, metadata !DIExpression()), !dbg !1779
  call void @llvm.va_start(ptr %7), !dbg !1779
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1783, metadata !DIExpression()), !dbg !1779
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1784, metadata !DIExpression()), !dbg !1779
  %13 = load ptr, ptr %6, align 8, !dbg !1779
  %14 = load ptr, ptr %13, align 8, !dbg !1779
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1779
  %16 = load ptr, ptr %15, align 8, !dbg !1779
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1779
  %18 = load ptr, ptr %4, align 8, !dbg !1779
  %19 = load ptr, ptr %6, align 8, !dbg !1779
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1779
  store i32 %20, ptr %9, align 4, !dbg !1779
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1785, metadata !DIExpression()), !dbg !1779
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1786, metadata !DIExpression()), !dbg !1788
  store i32 0, ptr %11, align 4, !dbg !1788
  br label %21, !dbg !1788

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1788
  %23 = load i32, ptr %9, align 4, !dbg !1788
  %24 = icmp slt i32 %22, %23, !dbg !1788
  br i1 %24, label %25, label %105, !dbg !1788

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1789
  %27 = sext i32 %26 to i64, !dbg !1789
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1789
  %29 = load i8, ptr %28, align 1, !dbg !1789
  %30 = sext i8 %29 to i32, !dbg !1789
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
  ], !dbg !1789

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1792
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1792
  store ptr %33, ptr %7, align 8, !dbg !1792
  %34 = load i32, ptr %32, align 8, !dbg !1792
  %35 = trunc i32 %34 to i8, !dbg !1792
  %36 = load i32, ptr %11, align 4, !dbg !1792
  %37 = sext i32 %36 to i64, !dbg !1792
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1792
  store i8 %35, ptr %38, align 8, !dbg !1792
  br label %101, !dbg !1792

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1792
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1792
  store ptr %41, ptr %7, align 8, !dbg !1792
  %42 = load i32, ptr %40, align 8, !dbg !1792
  %43 = trunc i32 %42 to i8, !dbg !1792
  %44 = load i32, ptr %11, align 4, !dbg !1792
  %45 = sext i32 %44 to i64, !dbg !1792
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1792
  store i8 %43, ptr %46, align 8, !dbg !1792
  br label %101, !dbg !1792

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1792
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1792
  store ptr %49, ptr %7, align 8, !dbg !1792
  %50 = load i32, ptr %48, align 8, !dbg !1792
  %51 = trunc i32 %50 to i16, !dbg !1792
  %52 = load i32, ptr %11, align 4, !dbg !1792
  %53 = sext i32 %52 to i64, !dbg !1792
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1792
  store i16 %51, ptr %54, align 8, !dbg !1792
  br label %101, !dbg !1792

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1792
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1792
  store ptr %57, ptr %7, align 8, !dbg !1792
  %58 = load i32, ptr %56, align 8, !dbg !1792
  %59 = trunc i32 %58 to i16, !dbg !1792
  %60 = load i32, ptr %11, align 4, !dbg !1792
  %61 = sext i32 %60 to i64, !dbg !1792
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1792
  store i16 %59, ptr %62, align 8, !dbg !1792
  br label %101, !dbg !1792

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1792
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1792
  store ptr %65, ptr %7, align 8, !dbg !1792
  %66 = load i32, ptr %64, align 8, !dbg !1792
  %67 = load i32, ptr %11, align 4, !dbg !1792
  %68 = sext i32 %67 to i64, !dbg !1792
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1792
  store i32 %66, ptr %69, align 8, !dbg !1792
  br label %101, !dbg !1792

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1792
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1792
  store ptr %72, ptr %7, align 8, !dbg !1792
  %73 = load i32, ptr %71, align 8, !dbg !1792
  %74 = sext i32 %73 to i64, !dbg !1792
  %75 = load i32, ptr %11, align 4, !dbg !1792
  %76 = sext i32 %75 to i64, !dbg !1792
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1792
  store i64 %74, ptr %77, align 8, !dbg !1792
  br label %101, !dbg !1792

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1792
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1792
  store ptr %80, ptr %7, align 8, !dbg !1792
  %81 = load double, ptr %79, align 8, !dbg !1792
  %82 = fptrunc double %81 to float, !dbg !1792
  %83 = load i32, ptr %11, align 4, !dbg !1792
  %84 = sext i32 %83 to i64, !dbg !1792
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1792
  store float %82, ptr %85, align 8, !dbg !1792
  br label %101, !dbg !1792

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1792
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1792
  store ptr %88, ptr %7, align 8, !dbg !1792
  %89 = load double, ptr %87, align 8, !dbg !1792
  %90 = load i32, ptr %11, align 4, !dbg !1792
  %91 = sext i32 %90 to i64, !dbg !1792
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1792
  store double %89, ptr %92, align 8, !dbg !1792
  br label %101, !dbg !1792

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1792
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1792
  store ptr %95, ptr %7, align 8, !dbg !1792
  %96 = load ptr, ptr %94, align 8, !dbg !1792
  %97 = load i32, ptr %11, align 4, !dbg !1792
  %98 = sext i32 %97 to i64, !dbg !1792
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1792
  store ptr %96, ptr %99, align 8, !dbg !1792
  br label %101, !dbg !1792

100:                                              ; preds = %25
  br label %101, !dbg !1792

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1789

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1794
  %104 = add nsw i32 %103, 1, !dbg !1794
  store i32 %104, ptr %11, align 4, !dbg !1794
  br label %21, !dbg !1794, !llvm.loop !1795

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1796, metadata !DIExpression()), !dbg !1779
  %106 = load ptr, ptr %6, align 8, !dbg !1779
  %107 = load ptr, ptr %106, align 8, !dbg !1779
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 131, !dbg !1779
  %109 = load ptr, ptr %108, align 8, !dbg !1779
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1779
  %111 = load ptr, ptr %4, align 8, !dbg !1779
  %112 = load ptr, ptr %5, align 8, !dbg !1779
  %113 = load ptr, ptr %6, align 8, !dbg !1779
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1779
  store i32 %114, ptr %12, align 4, !dbg !1779
  call void @llvm.va_end(ptr %7), !dbg !1779
  %115 = load i32, ptr %12, align 4, !dbg !1779
  ret i32 %115, !dbg !1779
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1797 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1798, metadata !DIExpression()), !dbg !1799
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1800, metadata !DIExpression()), !dbg !1799
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1801, metadata !DIExpression()), !dbg !1799
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1802, metadata !DIExpression()), !dbg !1799
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1803, metadata !DIExpression()), !dbg !1799
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1804, metadata !DIExpression()), !dbg !1799
  %13 = load ptr, ptr %8, align 8, !dbg !1799
  %14 = load ptr, ptr %13, align 8, !dbg !1799
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1799
  %16 = load ptr, ptr %15, align 8, !dbg !1799
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1799
  %18 = load ptr, ptr %6, align 8, !dbg !1799
  %19 = load ptr, ptr %8, align 8, !dbg !1799
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1799
  store i32 %20, ptr %10, align 4, !dbg !1799
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1805, metadata !DIExpression()), !dbg !1799
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1806, metadata !DIExpression()), !dbg !1808
  store i32 0, ptr %12, align 4, !dbg !1808
  br label %21, !dbg !1808

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1808
  %23 = load i32, ptr %10, align 4, !dbg !1808
  %24 = icmp slt i32 %22, %23, !dbg !1808
  br i1 %24, label %25, label %105, !dbg !1808

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1809
  %27 = sext i32 %26 to i64, !dbg !1809
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1809
  %29 = load i8, ptr %28, align 1, !dbg !1809
  %30 = sext i8 %29 to i32, !dbg !1809
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
  ], !dbg !1809

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1812
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1812
  store ptr %33, ptr %5, align 8, !dbg !1812
  %34 = load i32, ptr %32, align 8, !dbg !1812
  %35 = trunc i32 %34 to i8, !dbg !1812
  %36 = load i32, ptr %12, align 4, !dbg !1812
  %37 = sext i32 %36 to i64, !dbg !1812
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1812
  store i8 %35, ptr %38, align 8, !dbg !1812
  br label %101, !dbg !1812

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1812
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1812
  store ptr %41, ptr %5, align 8, !dbg !1812
  %42 = load i32, ptr %40, align 8, !dbg !1812
  %43 = trunc i32 %42 to i8, !dbg !1812
  %44 = load i32, ptr %12, align 4, !dbg !1812
  %45 = sext i32 %44 to i64, !dbg !1812
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1812
  store i8 %43, ptr %46, align 8, !dbg !1812
  br label %101, !dbg !1812

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1812
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1812
  store ptr %49, ptr %5, align 8, !dbg !1812
  %50 = load i32, ptr %48, align 8, !dbg !1812
  %51 = trunc i32 %50 to i16, !dbg !1812
  %52 = load i32, ptr %12, align 4, !dbg !1812
  %53 = sext i32 %52 to i64, !dbg !1812
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1812
  store i16 %51, ptr %54, align 8, !dbg !1812
  br label %101, !dbg !1812

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1812
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1812
  store ptr %57, ptr %5, align 8, !dbg !1812
  %58 = load i32, ptr %56, align 8, !dbg !1812
  %59 = trunc i32 %58 to i16, !dbg !1812
  %60 = load i32, ptr %12, align 4, !dbg !1812
  %61 = sext i32 %60 to i64, !dbg !1812
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1812
  store i16 %59, ptr %62, align 8, !dbg !1812
  br label %101, !dbg !1812

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1812
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1812
  store ptr %65, ptr %5, align 8, !dbg !1812
  %66 = load i32, ptr %64, align 8, !dbg !1812
  %67 = load i32, ptr %12, align 4, !dbg !1812
  %68 = sext i32 %67 to i64, !dbg !1812
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1812
  store i32 %66, ptr %69, align 8, !dbg !1812
  br label %101, !dbg !1812

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1812
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1812
  store ptr %72, ptr %5, align 8, !dbg !1812
  %73 = load i32, ptr %71, align 8, !dbg !1812
  %74 = sext i32 %73 to i64, !dbg !1812
  %75 = load i32, ptr %12, align 4, !dbg !1812
  %76 = sext i32 %75 to i64, !dbg !1812
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1812
  store i64 %74, ptr %77, align 8, !dbg !1812
  br label %101, !dbg !1812

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1812
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1812
  store ptr %80, ptr %5, align 8, !dbg !1812
  %81 = load double, ptr %79, align 8, !dbg !1812
  %82 = fptrunc double %81 to float, !dbg !1812
  %83 = load i32, ptr %12, align 4, !dbg !1812
  %84 = sext i32 %83 to i64, !dbg !1812
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1812
  store float %82, ptr %85, align 8, !dbg !1812
  br label %101, !dbg !1812

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1812
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1812
  store ptr %88, ptr %5, align 8, !dbg !1812
  %89 = load double, ptr %87, align 8, !dbg !1812
  %90 = load i32, ptr %12, align 4, !dbg !1812
  %91 = sext i32 %90 to i64, !dbg !1812
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1812
  store double %89, ptr %92, align 8, !dbg !1812
  br label %101, !dbg !1812

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1812
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1812
  store ptr %95, ptr %5, align 8, !dbg !1812
  %96 = load ptr, ptr %94, align 8, !dbg !1812
  %97 = load i32, ptr %12, align 4, !dbg !1812
  %98 = sext i32 %97 to i64, !dbg !1812
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1812
  store ptr %96, ptr %99, align 8, !dbg !1812
  br label %101, !dbg !1812

100:                                              ; preds = %25
  br label %101, !dbg !1812

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1809

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1814
  %104 = add nsw i32 %103, 1, !dbg !1814
  store i32 %104, ptr %12, align 4, !dbg !1814
  br label %21, !dbg !1814, !llvm.loop !1815

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1799
  %107 = load ptr, ptr %106, align 8, !dbg !1799
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 131, !dbg !1799
  %109 = load ptr, ptr %108, align 8, !dbg !1799
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1799
  %111 = load ptr, ptr %6, align 8, !dbg !1799
  %112 = load ptr, ptr %7, align 8, !dbg !1799
  %113 = load ptr, ptr %8, align 8, !dbg !1799
  %114 = call i32 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1799
  ret i32 %114, !dbg !1799
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1816 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1817, metadata !DIExpression()), !dbg !1818
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1819, metadata !DIExpression()), !dbg !1818
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1820, metadata !DIExpression()), !dbg !1818
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1821, metadata !DIExpression()), !dbg !1818
  call void @llvm.va_start(ptr %7), !dbg !1818
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1822, metadata !DIExpression()), !dbg !1818
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1823, metadata !DIExpression()), !dbg !1818
  %13 = load ptr, ptr %6, align 8, !dbg !1818
  %14 = load ptr, ptr %13, align 8, !dbg !1818
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1818
  %16 = load ptr, ptr %15, align 8, !dbg !1818
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1818
  %18 = load ptr, ptr %4, align 8, !dbg !1818
  %19 = load ptr, ptr %6, align 8, !dbg !1818
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1818
  store i32 %20, ptr %9, align 4, !dbg !1818
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1824, metadata !DIExpression()), !dbg !1818
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1825, metadata !DIExpression()), !dbg !1827
  store i32 0, ptr %11, align 4, !dbg !1827
  br label %21, !dbg !1827

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1827
  %23 = load i32, ptr %9, align 4, !dbg !1827
  %24 = icmp slt i32 %22, %23, !dbg !1827
  br i1 %24, label %25, label %105, !dbg !1827

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1828
  %27 = sext i32 %26 to i64, !dbg !1828
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1828
  %29 = load i8, ptr %28, align 1, !dbg !1828
  %30 = sext i8 %29 to i32, !dbg !1828
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
  ], !dbg !1828

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1831
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1831
  store ptr %33, ptr %7, align 8, !dbg !1831
  %34 = load i32, ptr %32, align 8, !dbg !1831
  %35 = trunc i32 %34 to i8, !dbg !1831
  %36 = load i32, ptr %11, align 4, !dbg !1831
  %37 = sext i32 %36 to i64, !dbg !1831
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1831
  store i8 %35, ptr %38, align 8, !dbg !1831
  br label %101, !dbg !1831

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1831
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1831
  store ptr %41, ptr %7, align 8, !dbg !1831
  %42 = load i32, ptr %40, align 8, !dbg !1831
  %43 = trunc i32 %42 to i8, !dbg !1831
  %44 = load i32, ptr %11, align 4, !dbg !1831
  %45 = sext i32 %44 to i64, !dbg !1831
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1831
  store i8 %43, ptr %46, align 8, !dbg !1831
  br label %101, !dbg !1831

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1831
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1831
  store ptr %49, ptr %7, align 8, !dbg !1831
  %50 = load i32, ptr %48, align 8, !dbg !1831
  %51 = trunc i32 %50 to i16, !dbg !1831
  %52 = load i32, ptr %11, align 4, !dbg !1831
  %53 = sext i32 %52 to i64, !dbg !1831
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1831
  store i16 %51, ptr %54, align 8, !dbg !1831
  br label %101, !dbg !1831

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1831
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1831
  store ptr %57, ptr %7, align 8, !dbg !1831
  %58 = load i32, ptr %56, align 8, !dbg !1831
  %59 = trunc i32 %58 to i16, !dbg !1831
  %60 = load i32, ptr %11, align 4, !dbg !1831
  %61 = sext i32 %60 to i64, !dbg !1831
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1831
  store i16 %59, ptr %62, align 8, !dbg !1831
  br label %101, !dbg !1831

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1831
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1831
  store ptr %65, ptr %7, align 8, !dbg !1831
  %66 = load i32, ptr %64, align 8, !dbg !1831
  %67 = load i32, ptr %11, align 4, !dbg !1831
  %68 = sext i32 %67 to i64, !dbg !1831
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1831
  store i32 %66, ptr %69, align 8, !dbg !1831
  br label %101, !dbg !1831

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1831
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1831
  store ptr %72, ptr %7, align 8, !dbg !1831
  %73 = load i32, ptr %71, align 8, !dbg !1831
  %74 = sext i32 %73 to i64, !dbg !1831
  %75 = load i32, ptr %11, align 4, !dbg !1831
  %76 = sext i32 %75 to i64, !dbg !1831
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1831
  store i64 %74, ptr %77, align 8, !dbg !1831
  br label %101, !dbg !1831

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1831
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1831
  store ptr %80, ptr %7, align 8, !dbg !1831
  %81 = load double, ptr %79, align 8, !dbg !1831
  %82 = fptrunc double %81 to float, !dbg !1831
  %83 = load i32, ptr %11, align 4, !dbg !1831
  %84 = sext i32 %83 to i64, !dbg !1831
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1831
  store float %82, ptr %85, align 8, !dbg !1831
  br label %101, !dbg !1831

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1831
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1831
  store ptr %88, ptr %7, align 8, !dbg !1831
  %89 = load double, ptr %87, align 8, !dbg !1831
  %90 = load i32, ptr %11, align 4, !dbg !1831
  %91 = sext i32 %90 to i64, !dbg !1831
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1831
  store double %89, ptr %92, align 8, !dbg !1831
  br label %101, !dbg !1831

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1831
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1831
  store ptr %95, ptr %7, align 8, !dbg !1831
  %96 = load ptr, ptr %94, align 8, !dbg !1831
  %97 = load i32, ptr %11, align 4, !dbg !1831
  %98 = sext i32 %97 to i64, !dbg !1831
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1831
  store ptr %96, ptr %99, align 8, !dbg !1831
  br label %101, !dbg !1831

100:                                              ; preds = %25
  br label %101, !dbg !1831

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1828

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1833
  %104 = add nsw i32 %103, 1, !dbg !1833
  store i32 %104, ptr %11, align 4, !dbg !1833
  br label %21, !dbg !1833, !llvm.loop !1834

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1835, metadata !DIExpression()), !dbg !1818
  %106 = load ptr, ptr %6, align 8, !dbg !1818
  %107 = load ptr, ptr %106, align 8, !dbg !1818
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 54, !dbg !1818
  %109 = load ptr, ptr %108, align 8, !dbg !1818
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1818
  %111 = load ptr, ptr %4, align 8, !dbg !1818
  %112 = load ptr, ptr %5, align 8, !dbg !1818
  %113 = load ptr, ptr %6, align 8, !dbg !1818
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1818
  store i64 %114, ptr %12, align 8, !dbg !1818
  call void @llvm.va_end(ptr %7), !dbg !1818
  %115 = load i64, ptr %12, align 8, !dbg !1818
  ret i64 %115, !dbg !1818
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1836 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1837, metadata !DIExpression()), !dbg !1838
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1839, metadata !DIExpression()), !dbg !1838
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1840, metadata !DIExpression()), !dbg !1838
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1841, metadata !DIExpression()), !dbg !1838
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1842, metadata !DIExpression()), !dbg !1838
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1843, metadata !DIExpression()), !dbg !1838
  %13 = load ptr, ptr %8, align 8, !dbg !1838
  %14 = load ptr, ptr %13, align 8, !dbg !1838
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1838
  %16 = load ptr, ptr %15, align 8, !dbg !1838
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1838
  %18 = load ptr, ptr %6, align 8, !dbg !1838
  %19 = load ptr, ptr %8, align 8, !dbg !1838
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1838
  store i32 %20, ptr %10, align 4, !dbg !1838
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1844, metadata !DIExpression()), !dbg !1838
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1845, metadata !DIExpression()), !dbg !1847
  store i32 0, ptr %12, align 4, !dbg !1847
  br label %21, !dbg !1847

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1847
  %23 = load i32, ptr %10, align 4, !dbg !1847
  %24 = icmp slt i32 %22, %23, !dbg !1847
  br i1 %24, label %25, label %105, !dbg !1847

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1848
  %27 = sext i32 %26 to i64, !dbg !1848
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1848
  %29 = load i8, ptr %28, align 1, !dbg !1848
  %30 = sext i8 %29 to i32, !dbg !1848
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
  ], !dbg !1848

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1851
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1851
  store ptr %33, ptr %5, align 8, !dbg !1851
  %34 = load i32, ptr %32, align 8, !dbg !1851
  %35 = trunc i32 %34 to i8, !dbg !1851
  %36 = load i32, ptr %12, align 4, !dbg !1851
  %37 = sext i32 %36 to i64, !dbg !1851
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1851
  store i8 %35, ptr %38, align 8, !dbg !1851
  br label %101, !dbg !1851

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1851
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1851
  store ptr %41, ptr %5, align 8, !dbg !1851
  %42 = load i32, ptr %40, align 8, !dbg !1851
  %43 = trunc i32 %42 to i8, !dbg !1851
  %44 = load i32, ptr %12, align 4, !dbg !1851
  %45 = sext i32 %44 to i64, !dbg !1851
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1851
  store i8 %43, ptr %46, align 8, !dbg !1851
  br label %101, !dbg !1851

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1851
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1851
  store ptr %49, ptr %5, align 8, !dbg !1851
  %50 = load i32, ptr %48, align 8, !dbg !1851
  %51 = trunc i32 %50 to i16, !dbg !1851
  %52 = load i32, ptr %12, align 4, !dbg !1851
  %53 = sext i32 %52 to i64, !dbg !1851
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1851
  store i16 %51, ptr %54, align 8, !dbg !1851
  br label %101, !dbg !1851

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1851
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1851
  store ptr %57, ptr %5, align 8, !dbg !1851
  %58 = load i32, ptr %56, align 8, !dbg !1851
  %59 = trunc i32 %58 to i16, !dbg !1851
  %60 = load i32, ptr %12, align 4, !dbg !1851
  %61 = sext i32 %60 to i64, !dbg !1851
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1851
  store i16 %59, ptr %62, align 8, !dbg !1851
  br label %101, !dbg !1851

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1851
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1851
  store ptr %65, ptr %5, align 8, !dbg !1851
  %66 = load i32, ptr %64, align 8, !dbg !1851
  %67 = load i32, ptr %12, align 4, !dbg !1851
  %68 = sext i32 %67 to i64, !dbg !1851
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1851
  store i32 %66, ptr %69, align 8, !dbg !1851
  br label %101, !dbg !1851

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1851
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1851
  store ptr %72, ptr %5, align 8, !dbg !1851
  %73 = load i32, ptr %71, align 8, !dbg !1851
  %74 = sext i32 %73 to i64, !dbg !1851
  %75 = load i32, ptr %12, align 4, !dbg !1851
  %76 = sext i32 %75 to i64, !dbg !1851
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1851
  store i64 %74, ptr %77, align 8, !dbg !1851
  br label %101, !dbg !1851

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1851
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1851
  store ptr %80, ptr %5, align 8, !dbg !1851
  %81 = load double, ptr %79, align 8, !dbg !1851
  %82 = fptrunc double %81 to float, !dbg !1851
  %83 = load i32, ptr %12, align 4, !dbg !1851
  %84 = sext i32 %83 to i64, !dbg !1851
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1851
  store float %82, ptr %85, align 8, !dbg !1851
  br label %101, !dbg !1851

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1851
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1851
  store ptr %88, ptr %5, align 8, !dbg !1851
  %89 = load double, ptr %87, align 8, !dbg !1851
  %90 = load i32, ptr %12, align 4, !dbg !1851
  %91 = sext i32 %90 to i64, !dbg !1851
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1851
  store double %89, ptr %92, align 8, !dbg !1851
  br label %101, !dbg !1851

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1851
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1851
  store ptr %95, ptr %5, align 8, !dbg !1851
  %96 = load ptr, ptr %94, align 8, !dbg !1851
  %97 = load i32, ptr %12, align 4, !dbg !1851
  %98 = sext i32 %97 to i64, !dbg !1851
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1851
  store ptr %96, ptr %99, align 8, !dbg !1851
  br label %101, !dbg !1851

100:                                              ; preds = %25
  br label %101, !dbg !1851

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1848

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1853
  %104 = add nsw i32 %103, 1, !dbg !1853
  store i32 %104, ptr %12, align 4, !dbg !1853
  br label %21, !dbg !1853, !llvm.loop !1854

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1838
  %107 = load ptr, ptr %106, align 8, !dbg !1838
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 54, !dbg !1838
  %109 = load ptr, ptr %108, align 8, !dbg !1838
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1838
  %111 = load ptr, ptr %6, align 8, !dbg !1838
  %112 = load ptr, ptr %7, align 8, !dbg !1838
  %113 = load ptr, ptr %8, align 8, !dbg !1838
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1838
  ret i64 %114, !dbg !1838
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1855 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca i64, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1856, metadata !DIExpression()), !dbg !1857
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1858, metadata !DIExpression()), !dbg !1857
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1859, metadata !DIExpression()), !dbg !1857
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1860, metadata !DIExpression()), !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1861, metadata !DIExpression()), !dbg !1857
  call void @llvm.va_start(ptr %9), !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1862, metadata !DIExpression()), !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1863, metadata !DIExpression()), !dbg !1857
  %15 = load ptr, ptr %8, align 8, !dbg !1857
  %16 = load ptr, ptr %15, align 8, !dbg !1857
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1857
  %18 = load ptr, ptr %17, align 8, !dbg !1857
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1857
  %20 = load ptr, ptr %5, align 8, !dbg !1857
  %21 = load ptr, ptr %8, align 8, !dbg !1857
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1857
  store i32 %22, ptr %11, align 4, !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1864, metadata !DIExpression()), !dbg !1857
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1865, metadata !DIExpression()), !dbg !1867
  store i32 0, ptr %13, align 4, !dbg !1867
  br label %23, !dbg !1867

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1867
  %25 = load i32, ptr %11, align 4, !dbg !1867
  %26 = icmp slt i32 %24, %25, !dbg !1867
  br i1 %26, label %27, label %107, !dbg !1867

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1868
  %29 = sext i32 %28 to i64, !dbg !1868
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1868
  %31 = load i8, ptr %30, align 1, !dbg !1868
  %32 = sext i8 %31 to i32, !dbg !1868
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
  ], !dbg !1868

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1871
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1871
  store ptr %35, ptr %9, align 8, !dbg !1871
  %36 = load i32, ptr %34, align 8, !dbg !1871
  %37 = trunc i32 %36 to i8, !dbg !1871
  %38 = load i32, ptr %13, align 4, !dbg !1871
  %39 = sext i32 %38 to i64, !dbg !1871
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1871
  store i8 %37, ptr %40, align 8, !dbg !1871
  br label %103, !dbg !1871

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1871
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1871
  store ptr %43, ptr %9, align 8, !dbg !1871
  %44 = load i32, ptr %42, align 8, !dbg !1871
  %45 = trunc i32 %44 to i8, !dbg !1871
  %46 = load i32, ptr %13, align 4, !dbg !1871
  %47 = sext i32 %46 to i64, !dbg !1871
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1871
  store i8 %45, ptr %48, align 8, !dbg !1871
  br label %103, !dbg !1871

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1871
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1871
  store ptr %51, ptr %9, align 8, !dbg !1871
  %52 = load i32, ptr %50, align 8, !dbg !1871
  %53 = trunc i32 %52 to i16, !dbg !1871
  %54 = load i32, ptr %13, align 4, !dbg !1871
  %55 = sext i32 %54 to i64, !dbg !1871
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1871
  store i16 %53, ptr %56, align 8, !dbg !1871
  br label %103, !dbg !1871

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1871
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1871
  store ptr %59, ptr %9, align 8, !dbg !1871
  %60 = load i32, ptr %58, align 8, !dbg !1871
  %61 = trunc i32 %60 to i16, !dbg !1871
  %62 = load i32, ptr %13, align 4, !dbg !1871
  %63 = sext i32 %62 to i64, !dbg !1871
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1871
  store i16 %61, ptr %64, align 8, !dbg !1871
  br label %103, !dbg !1871

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1871
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1871
  store ptr %67, ptr %9, align 8, !dbg !1871
  %68 = load i32, ptr %66, align 8, !dbg !1871
  %69 = load i32, ptr %13, align 4, !dbg !1871
  %70 = sext i32 %69 to i64, !dbg !1871
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1871
  store i32 %68, ptr %71, align 8, !dbg !1871
  br label %103, !dbg !1871

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1871
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1871
  store ptr %74, ptr %9, align 8, !dbg !1871
  %75 = load i32, ptr %73, align 8, !dbg !1871
  %76 = sext i32 %75 to i64, !dbg !1871
  %77 = load i32, ptr %13, align 4, !dbg !1871
  %78 = sext i32 %77 to i64, !dbg !1871
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1871
  store i64 %76, ptr %79, align 8, !dbg !1871
  br label %103, !dbg !1871

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1871
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1871
  store ptr %82, ptr %9, align 8, !dbg !1871
  %83 = load double, ptr %81, align 8, !dbg !1871
  %84 = fptrunc double %83 to float, !dbg !1871
  %85 = load i32, ptr %13, align 4, !dbg !1871
  %86 = sext i32 %85 to i64, !dbg !1871
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1871
  store float %84, ptr %87, align 8, !dbg !1871
  br label %103, !dbg !1871

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1871
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1871
  store ptr %90, ptr %9, align 8, !dbg !1871
  %91 = load double, ptr %89, align 8, !dbg !1871
  %92 = load i32, ptr %13, align 4, !dbg !1871
  %93 = sext i32 %92 to i64, !dbg !1871
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1871
  store double %91, ptr %94, align 8, !dbg !1871
  br label %103, !dbg !1871

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1871
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1871
  store ptr %97, ptr %9, align 8, !dbg !1871
  %98 = load ptr, ptr %96, align 8, !dbg !1871
  %99 = load i32, ptr %13, align 4, !dbg !1871
  %100 = sext i32 %99 to i64, !dbg !1871
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1871
  store ptr %98, ptr %101, align 8, !dbg !1871
  br label %103, !dbg !1871

102:                                              ; preds = %27
  br label %103, !dbg !1871

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1868

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1873
  %106 = add nsw i32 %105, 1, !dbg !1873
  store i32 %106, ptr %13, align 4, !dbg !1873
  br label %23, !dbg !1873, !llvm.loop !1874

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1875, metadata !DIExpression()), !dbg !1857
  %108 = load ptr, ptr %8, align 8, !dbg !1857
  %109 = load ptr, ptr %108, align 8, !dbg !1857
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 84, !dbg !1857
  %111 = load ptr, ptr %110, align 8, !dbg !1857
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1857
  %113 = load ptr, ptr %5, align 8, !dbg !1857
  %114 = load ptr, ptr %6, align 8, !dbg !1857
  %115 = load ptr, ptr %7, align 8, !dbg !1857
  %116 = load ptr, ptr %8, align 8, !dbg !1857
  %117 = call i64 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1857
  store i64 %117, ptr %14, align 8, !dbg !1857
  call void @llvm.va_end(ptr %9), !dbg !1857
  %118 = load i64, ptr %14, align 8, !dbg !1857
  ret i64 %118, !dbg !1857
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1876 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1877, metadata !DIExpression()), !dbg !1878
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1879, metadata !DIExpression()), !dbg !1878
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1880, metadata !DIExpression()), !dbg !1878
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1881, metadata !DIExpression()), !dbg !1878
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1882, metadata !DIExpression()), !dbg !1878
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1883, metadata !DIExpression()), !dbg !1878
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1884, metadata !DIExpression()), !dbg !1878
  %15 = load ptr, ptr %10, align 8, !dbg !1878
  %16 = load ptr, ptr %15, align 8, !dbg !1878
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1878
  %18 = load ptr, ptr %17, align 8, !dbg !1878
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1878
  %20 = load ptr, ptr %7, align 8, !dbg !1878
  %21 = load ptr, ptr %10, align 8, !dbg !1878
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1878
  store i32 %22, ptr %12, align 4, !dbg !1878
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1885, metadata !DIExpression()), !dbg !1878
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1886, metadata !DIExpression()), !dbg !1888
  store i32 0, ptr %14, align 4, !dbg !1888
  br label %23, !dbg !1888

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !1888
  %25 = load i32, ptr %12, align 4, !dbg !1888
  %26 = icmp slt i32 %24, %25, !dbg !1888
  br i1 %26, label %27, label %107, !dbg !1888

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !1889
  %29 = sext i32 %28 to i64, !dbg !1889
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !1889
  %31 = load i8, ptr %30, align 1, !dbg !1889
  %32 = sext i8 %31 to i32, !dbg !1889
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
  ], !dbg !1889

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !1892
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1892
  store ptr %35, ptr %6, align 8, !dbg !1892
  %36 = load i32, ptr %34, align 8, !dbg !1892
  %37 = trunc i32 %36 to i8, !dbg !1892
  %38 = load i32, ptr %14, align 4, !dbg !1892
  %39 = sext i32 %38 to i64, !dbg !1892
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !1892
  store i8 %37, ptr %40, align 8, !dbg !1892
  br label %103, !dbg !1892

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !1892
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1892
  store ptr %43, ptr %6, align 8, !dbg !1892
  %44 = load i32, ptr %42, align 8, !dbg !1892
  %45 = trunc i32 %44 to i8, !dbg !1892
  %46 = load i32, ptr %14, align 4, !dbg !1892
  %47 = sext i32 %46 to i64, !dbg !1892
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !1892
  store i8 %45, ptr %48, align 8, !dbg !1892
  br label %103, !dbg !1892

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !1892
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1892
  store ptr %51, ptr %6, align 8, !dbg !1892
  %52 = load i32, ptr %50, align 8, !dbg !1892
  %53 = trunc i32 %52 to i16, !dbg !1892
  %54 = load i32, ptr %14, align 4, !dbg !1892
  %55 = sext i32 %54 to i64, !dbg !1892
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !1892
  store i16 %53, ptr %56, align 8, !dbg !1892
  br label %103, !dbg !1892

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !1892
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1892
  store ptr %59, ptr %6, align 8, !dbg !1892
  %60 = load i32, ptr %58, align 8, !dbg !1892
  %61 = trunc i32 %60 to i16, !dbg !1892
  %62 = load i32, ptr %14, align 4, !dbg !1892
  %63 = sext i32 %62 to i64, !dbg !1892
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !1892
  store i16 %61, ptr %64, align 8, !dbg !1892
  br label %103, !dbg !1892

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !1892
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1892
  store ptr %67, ptr %6, align 8, !dbg !1892
  %68 = load i32, ptr %66, align 8, !dbg !1892
  %69 = load i32, ptr %14, align 4, !dbg !1892
  %70 = sext i32 %69 to i64, !dbg !1892
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !1892
  store i32 %68, ptr %71, align 8, !dbg !1892
  br label %103, !dbg !1892

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !1892
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1892
  store ptr %74, ptr %6, align 8, !dbg !1892
  %75 = load i32, ptr %73, align 8, !dbg !1892
  %76 = sext i32 %75 to i64, !dbg !1892
  %77 = load i32, ptr %14, align 4, !dbg !1892
  %78 = sext i32 %77 to i64, !dbg !1892
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !1892
  store i64 %76, ptr %79, align 8, !dbg !1892
  br label %103, !dbg !1892

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !1892
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1892
  store ptr %82, ptr %6, align 8, !dbg !1892
  %83 = load double, ptr %81, align 8, !dbg !1892
  %84 = fptrunc double %83 to float, !dbg !1892
  %85 = load i32, ptr %14, align 4, !dbg !1892
  %86 = sext i32 %85 to i64, !dbg !1892
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !1892
  store float %84, ptr %87, align 8, !dbg !1892
  br label %103, !dbg !1892

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !1892
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1892
  store ptr %90, ptr %6, align 8, !dbg !1892
  %91 = load double, ptr %89, align 8, !dbg !1892
  %92 = load i32, ptr %14, align 4, !dbg !1892
  %93 = sext i32 %92 to i64, !dbg !1892
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !1892
  store double %91, ptr %94, align 8, !dbg !1892
  br label %103, !dbg !1892

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !1892
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1892
  store ptr %97, ptr %6, align 8, !dbg !1892
  %98 = load ptr, ptr %96, align 8, !dbg !1892
  %99 = load i32, ptr %14, align 4, !dbg !1892
  %100 = sext i32 %99 to i64, !dbg !1892
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !1892
  store ptr %98, ptr %101, align 8, !dbg !1892
  br label %103, !dbg !1892

102:                                              ; preds = %27
  br label %103, !dbg !1892

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1889

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !1894
  %106 = add nsw i32 %105, 1, !dbg !1894
  store i32 %106, ptr %14, align 4, !dbg !1894
  br label %23, !dbg !1894, !llvm.loop !1895

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1878
  %109 = load ptr, ptr %108, align 8, !dbg !1878
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 84, !dbg !1878
  %111 = load ptr, ptr %110, align 8, !dbg !1878
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1878
  %113 = load ptr, ptr %7, align 8, !dbg !1878
  %114 = load ptr, ptr %8, align 8, !dbg !1878
  %115 = load ptr, ptr %9, align 8, !dbg !1878
  %116 = load ptr, ptr %10, align 8, !dbg !1878
  %117 = call i64 %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1878
  ret i64 %117, !dbg !1878
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1896 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca i64, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1897, metadata !DIExpression()), !dbg !1898
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1899, metadata !DIExpression()), !dbg !1898
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1900, metadata !DIExpression()), !dbg !1898
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1901, metadata !DIExpression()), !dbg !1898
  call void @llvm.va_start(ptr %7), !dbg !1898
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1902, metadata !DIExpression()), !dbg !1898
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1903, metadata !DIExpression()), !dbg !1898
  %13 = load ptr, ptr %6, align 8, !dbg !1898
  %14 = load ptr, ptr %13, align 8, !dbg !1898
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1898
  %16 = load ptr, ptr %15, align 8, !dbg !1898
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1898
  %18 = load ptr, ptr %4, align 8, !dbg !1898
  %19 = load ptr, ptr %6, align 8, !dbg !1898
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1898
  store i32 %20, ptr %9, align 4, !dbg !1898
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1904, metadata !DIExpression()), !dbg !1898
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1905, metadata !DIExpression()), !dbg !1907
  store i32 0, ptr %11, align 4, !dbg !1907
  br label %21, !dbg !1907

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1907
  %23 = load i32, ptr %9, align 4, !dbg !1907
  %24 = icmp slt i32 %22, %23, !dbg !1907
  br i1 %24, label %25, label %105, !dbg !1907

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1908
  %27 = sext i32 %26 to i64, !dbg !1908
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1908
  %29 = load i8, ptr %28, align 1, !dbg !1908
  %30 = sext i8 %29 to i32, !dbg !1908
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
  ], !dbg !1908

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !1911
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1911
  store ptr %33, ptr %7, align 8, !dbg !1911
  %34 = load i32, ptr %32, align 8, !dbg !1911
  %35 = trunc i32 %34 to i8, !dbg !1911
  %36 = load i32, ptr %11, align 4, !dbg !1911
  %37 = sext i32 %36 to i64, !dbg !1911
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1911
  store i8 %35, ptr %38, align 8, !dbg !1911
  br label %101, !dbg !1911

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1911
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1911
  store ptr %41, ptr %7, align 8, !dbg !1911
  %42 = load i32, ptr %40, align 8, !dbg !1911
  %43 = trunc i32 %42 to i8, !dbg !1911
  %44 = load i32, ptr %11, align 4, !dbg !1911
  %45 = sext i32 %44 to i64, !dbg !1911
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1911
  store i8 %43, ptr %46, align 8, !dbg !1911
  br label %101, !dbg !1911

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1911
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1911
  store ptr %49, ptr %7, align 8, !dbg !1911
  %50 = load i32, ptr %48, align 8, !dbg !1911
  %51 = trunc i32 %50 to i16, !dbg !1911
  %52 = load i32, ptr %11, align 4, !dbg !1911
  %53 = sext i32 %52 to i64, !dbg !1911
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1911
  store i16 %51, ptr %54, align 8, !dbg !1911
  br label %101, !dbg !1911

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1911
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1911
  store ptr %57, ptr %7, align 8, !dbg !1911
  %58 = load i32, ptr %56, align 8, !dbg !1911
  %59 = trunc i32 %58 to i16, !dbg !1911
  %60 = load i32, ptr %11, align 4, !dbg !1911
  %61 = sext i32 %60 to i64, !dbg !1911
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1911
  store i16 %59, ptr %62, align 8, !dbg !1911
  br label %101, !dbg !1911

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1911
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1911
  store ptr %65, ptr %7, align 8, !dbg !1911
  %66 = load i32, ptr %64, align 8, !dbg !1911
  %67 = load i32, ptr %11, align 4, !dbg !1911
  %68 = sext i32 %67 to i64, !dbg !1911
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1911
  store i32 %66, ptr %69, align 8, !dbg !1911
  br label %101, !dbg !1911

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1911
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1911
  store ptr %72, ptr %7, align 8, !dbg !1911
  %73 = load i32, ptr %71, align 8, !dbg !1911
  %74 = sext i32 %73 to i64, !dbg !1911
  %75 = load i32, ptr %11, align 4, !dbg !1911
  %76 = sext i32 %75 to i64, !dbg !1911
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1911
  store i64 %74, ptr %77, align 8, !dbg !1911
  br label %101, !dbg !1911

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1911
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1911
  store ptr %80, ptr %7, align 8, !dbg !1911
  %81 = load double, ptr %79, align 8, !dbg !1911
  %82 = fptrunc double %81 to float, !dbg !1911
  %83 = load i32, ptr %11, align 4, !dbg !1911
  %84 = sext i32 %83 to i64, !dbg !1911
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1911
  store float %82, ptr %85, align 8, !dbg !1911
  br label %101, !dbg !1911

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1911
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1911
  store ptr %88, ptr %7, align 8, !dbg !1911
  %89 = load double, ptr %87, align 8, !dbg !1911
  %90 = load i32, ptr %11, align 4, !dbg !1911
  %91 = sext i32 %90 to i64, !dbg !1911
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1911
  store double %89, ptr %92, align 8, !dbg !1911
  br label %101, !dbg !1911

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1911
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1911
  store ptr %95, ptr %7, align 8, !dbg !1911
  %96 = load ptr, ptr %94, align 8, !dbg !1911
  %97 = load i32, ptr %11, align 4, !dbg !1911
  %98 = sext i32 %97 to i64, !dbg !1911
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1911
  store ptr %96, ptr %99, align 8, !dbg !1911
  br label %101, !dbg !1911

100:                                              ; preds = %25
  br label %101, !dbg !1911

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1908

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1913
  %104 = add nsw i32 %103, 1, !dbg !1913
  store i32 %104, ptr %11, align 4, !dbg !1913
  br label %21, !dbg !1913, !llvm.loop !1914

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1915, metadata !DIExpression()), !dbg !1898
  %106 = load ptr, ptr %6, align 8, !dbg !1898
  %107 = load ptr, ptr %106, align 8, !dbg !1898
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 134, !dbg !1898
  %109 = load ptr, ptr %108, align 8, !dbg !1898
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1898
  %111 = load ptr, ptr %4, align 8, !dbg !1898
  %112 = load ptr, ptr %5, align 8, !dbg !1898
  %113 = load ptr, ptr %6, align 8, !dbg !1898
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1898
  store i64 %114, ptr %12, align 8, !dbg !1898
  call void @llvm.va_end(ptr %7), !dbg !1898
  %115 = load i64, ptr %12, align 8, !dbg !1898
  ret i64 %115, !dbg !1898
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1916 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1917, metadata !DIExpression()), !dbg !1918
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1919, metadata !DIExpression()), !dbg !1918
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1920, metadata !DIExpression()), !dbg !1918
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1921, metadata !DIExpression()), !dbg !1918
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1922, metadata !DIExpression()), !dbg !1918
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1923, metadata !DIExpression()), !dbg !1918
  %13 = load ptr, ptr %8, align 8, !dbg !1918
  %14 = load ptr, ptr %13, align 8, !dbg !1918
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1918
  %16 = load ptr, ptr %15, align 8, !dbg !1918
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1918
  %18 = load ptr, ptr %6, align 8, !dbg !1918
  %19 = load ptr, ptr %8, align 8, !dbg !1918
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1918
  store i32 %20, ptr %10, align 4, !dbg !1918
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1924, metadata !DIExpression()), !dbg !1918
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1925, metadata !DIExpression()), !dbg !1927
  store i32 0, ptr %12, align 4, !dbg !1927
  br label %21, !dbg !1927

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1927
  %23 = load i32, ptr %10, align 4, !dbg !1927
  %24 = icmp slt i32 %22, %23, !dbg !1927
  br i1 %24, label %25, label %105, !dbg !1927

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1928
  %27 = sext i32 %26 to i64, !dbg !1928
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1928
  %29 = load i8, ptr %28, align 1, !dbg !1928
  %30 = sext i8 %29 to i32, !dbg !1928
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
  ], !dbg !1928

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1931
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1931
  store ptr %33, ptr %5, align 8, !dbg !1931
  %34 = load i32, ptr %32, align 8, !dbg !1931
  %35 = trunc i32 %34 to i8, !dbg !1931
  %36 = load i32, ptr %12, align 4, !dbg !1931
  %37 = sext i32 %36 to i64, !dbg !1931
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1931
  store i8 %35, ptr %38, align 8, !dbg !1931
  br label %101, !dbg !1931

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1931
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1931
  store ptr %41, ptr %5, align 8, !dbg !1931
  %42 = load i32, ptr %40, align 8, !dbg !1931
  %43 = trunc i32 %42 to i8, !dbg !1931
  %44 = load i32, ptr %12, align 4, !dbg !1931
  %45 = sext i32 %44 to i64, !dbg !1931
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1931
  store i8 %43, ptr %46, align 8, !dbg !1931
  br label %101, !dbg !1931

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1931
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1931
  store ptr %49, ptr %5, align 8, !dbg !1931
  %50 = load i32, ptr %48, align 8, !dbg !1931
  %51 = trunc i32 %50 to i16, !dbg !1931
  %52 = load i32, ptr %12, align 4, !dbg !1931
  %53 = sext i32 %52 to i64, !dbg !1931
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1931
  store i16 %51, ptr %54, align 8, !dbg !1931
  br label %101, !dbg !1931

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1931
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1931
  store ptr %57, ptr %5, align 8, !dbg !1931
  %58 = load i32, ptr %56, align 8, !dbg !1931
  %59 = trunc i32 %58 to i16, !dbg !1931
  %60 = load i32, ptr %12, align 4, !dbg !1931
  %61 = sext i32 %60 to i64, !dbg !1931
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1931
  store i16 %59, ptr %62, align 8, !dbg !1931
  br label %101, !dbg !1931

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1931
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1931
  store ptr %65, ptr %5, align 8, !dbg !1931
  %66 = load i32, ptr %64, align 8, !dbg !1931
  %67 = load i32, ptr %12, align 4, !dbg !1931
  %68 = sext i32 %67 to i64, !dbg !1931
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1931
  store i32 %66, ptr %69, align 8, !dbg !1931
  br label %101, !dbg !1931

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1931
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1931
  store ptr %72, ptr %5, align 8, !dbg !1931
  %73 = load i32, ptr %71, align 8, !dbg !1931
  %74 = sext i32 %73 to i64, !dbg !1931
  %75 = load i32, ptr %12, align 4, !dbg !1931
  %76 = sext i32 %75 to i64, !dbg !1931
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1931
  store i64 %74, ptr %77, align 8, !dbg !1931
  br label %101, !dbg !1931

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1931
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1931
  store ptr %80, ptr %5, align 8, !dbg !1931
  %81 = load double, ptr %79, align 8, !dbg !1931
  %82 = fptrunc double %81 to float, !dbg !1931
  %83 = load i32, ptr %12, align 4, !dbg !1931
  %84 = sext i32 %83 to i64, !dbg !1931
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1931
  store float %82, ptr %85, align 8, !dbg !1931
  br label %101, !dbg !1931

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1931
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1931
  store ptr %88, ptr %5, align 8, !dbg !1931
  %89 = load double, ptr %87, align 8, !dbg !1931
  %90 = load i32, ptr %12, align 4, !dbg !1931
  %91 = sext i32 %90 to i64, !dbg !1931
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1931
  store double %89, ptr %92, align 8, !dbg !1931
  br label %101, !dbg !1931

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1931
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1931
  store ptr %95, ptr %5, align 8, !dbg !1931
  %96 = load ptr, ptr %94, align 8, !dbg !1931
  %97 = load i32, ptr %12, align 4, !dbg !1931
  %98 = sext i32 %97 to i64, !dbg !1931
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1931
  store ptr %96, ptr %99, align 8, !dbg !1931
  br label %101, !dbg !1931

100:                                              ; preds = %25
  br label %101, !dbg !1931

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1928

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1933
  %104 = add nsw i32 %103, 1, !dbg !1933
  store i32 %104, ptr %12, align 4, !dbg !1933
  br label %21, !dbg !1933, !llvm.loop !1934

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1918
  %107 = load ptr, ptr %106, align 8, !dbg !1918
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 134, !dbg !1918
  %109 = load ptr, ptr %108, align 8, !dbg !1918
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1918
  %111 = load ptr, ptr %6, align 8, !dbg !1918
  %112 = load ptr, ptr %7, align 8, !dbg !1918
  %113 = load ptr, ptr %8, align 8, !dbg !1918
  %114 = call i64 %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1918
  ret i64 %114, !dbg !1918
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !1935 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !1936, metadata !DIExpression()), !dbg !1937
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1938, metadata !DIExpression()), !dbg !1937
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1939, metadata !DIExpression()), !dbg !1937
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1940, metadata !DIExpression()), !dbg !1937
  call void @llvm.va_start(ptr %7), !dbg !1937
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1941, metadata !DIExpression()), !dbg !1937
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1942, metadata !DIExpression()), !dbg !1937
  %13 = load ptr, ptr %6, align 8, !dbg !1937
  %14 = load ptr, ptr %13, align 8, !dbg !1937
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1937
  %16 = load ptr, ptr %15, align 8, !dbg !1937
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !1937
  %18 = load ptr, ptr %4, align 8, !dbg !1937
  %19 = load ptr, ptr %6, align 8, !dbg !1937
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1937
  store i32 %20, ptr %9, align 4, !dbg !1937
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1943, metadata !DIExpression()), !dbg !1937
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1944, metadata !DIExpression()), !dbg !1946
  store i32 0, ptr %11, align 4, !dbg !1946
  br label %21, !dbg !1946

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !1946
  %23 = load i32, ptr %9, align 4, !dbg !1946
  %24 = icmp slt i32 %22, %23, !dbg !1946
  br i1 %24, label %25, label %105, !dbg !1946

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !1947
  %27 = sext i32 %26 to i64, !dbg !1947
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !1947
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
  %32 = load ptr, ptr %7, align 8, !dbg !1950
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1950
  store ptr %33, ptr %7, align 8, !dbg !1950
  %34 = load i32, ptr %32, align 8, !dbg !1950
  %35 = trunc i32 %34 to i8, !dbg !1950
  %36 = load i32, ptr %11, align 4, !dbg !1950
  %37 = sext i32 %36 to i64, !dbg !1950
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !1950
  store i8 %35, ptr %38, align 8, !dbg !1950
  br label %101, !dbg !1950

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !1950
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1950
  store ptr %41, ptr %7, align 8, !dbg !1950
  %42 = load i32, ptr %40, align 8, !dbg !1950
  %43 = trunc i32 %42 to i8, !dbg !1950
  %44 = load i32, ptr %11, align 4, !dbg !1950
  %45 = sext i32 %44 to i64, !dbg !1950
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !1950
  store i8 %43, ptr %46, align 8, !dbg !1950
  br label %101, !dbg !1950

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !1950
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1950
  store ptr %49, ptr %7, align 8, !dbg !1950
  %50 = load i32, ptr %48, align 8, !dbg !1950
  %51 = trunc i32 %50 to i16, !dbg !1950
  %52 = load i32, ptr %11, align 4, !dbg !1950
  %53 = sext i32 %52 to i64, !dbg !1950
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !1950
  store i16 %51, ptr %54, align 8, !dbg !1950
  br label %101, !dbg !1950

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !1950
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1950
  store ptr %57, ptr %7, align 8, !dbg !1950
  %58 = load i32, ptr %56, align 8, !dbg !1950
  %59 = trunc i32 %58 to i16, !dbg !1950
  %60 = load i32, ptr %11, align 4, !dbg !1950
  %61 = sext i32 %60 to i64, !dbg !1950
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !1950
  store i16 %59, ptr %62, align 8, !dbg !1950
  br label %101, !dbg !1950

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !1950
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1950
  store ptr %65, ptr %7, align 8, !dbg !1950
  %66 = load i32, ptr %64, align 8, !dbg !1950
  %67 = load i32, ptr %11, align 4, !dbg !1950
  %68 = sext i32 %67 to i64, !dbg !1950
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !1950
  store i32 %66, ptr %69, align 8, !dbg !1950
  br label %101, !dbg !1950

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !1950
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1950
  store ptr %72, ptr %7, align 8, !dbg !1950
  %73 = load i32, ptr %71, align 8, !dbg !1950
  %74 = sext i32 %73 to i64, !dbg !1950
  %75 = load i32, ptr %11, align 4, !dbg !1950
  %76 = sext i32 %75 to i64, !dbg !1950
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !1950
  store i64 %74, ptr %77, align 8, !dbg !1950
  br label %101, !dbg !1950

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !1950
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1950
  store ptr %80, ptr %7, align 8, !dbg !1950
  %81 = load double, ptr %79, align 8, !dbg !1950
  %82 = fptrunc double %81 to float, !dbg !1950
  %83 = load i32, ptr %11, align 4, !dbg !1950
  %84 = sext i32 %83 to i64, !dbg !1950
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !1950
  store float %82, ptr %85, align 8, !dbg !1950
  br label %101, !dbg !1950

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !1950
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1950
  store ptr %88, ptr %7, align 8, !dbg !1950
  %89 = load double, ptr %87, align 8, !dbg !1950
  %90 = load i32, ptr %11, align 4, !dbg !1950
  %91 = sext i32 %90 to i64, !dbg !1950
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !1950
  store double %89, ptr %92, align 8, !dbg !1950
  br label %101, !dbg !1950

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !1950
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1950
  store ptr %95, ptr %7, align 8, !dbg !1950
  %96 = load ptr, ptr %94, align 8, !dbg !1950
  %97 = load i32, ptr %11, align 4, !dbg !1950
  %98 = sext i32 %97 to i64, !dbg !1950
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !1950
  store ptr %96, ptr %99, align 8, !dbg !1950
  br label %101, !dbg !1950

100:                                              ; preds = %25
  br label %101, !dbg !1950

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1947

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !1952
  %104 = add nsw i32 %103, 1, !dbg !1952
  store i32 %104, ptr %11, align 4, !dbg !1952
  br label %21, !dbg !1952, !llvm.loop !1953

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1954, metadata !DIExpression()), !dbg !1937
  %106 = load ptr, ptr %6, align 8, !dbg !1937
  %107 = load ptr, ptr %106, align 8, !dbg !1937
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 57, !dbg !1937
  %109 = load ptr, ptr %108, align 8, !dbg !1937
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !1937
  %111 = load ptr, ptr %4, align 8, !dbg !1937
  %112 = load ptr, ptr %5, align 8, !dbg !1937
  %113 = load ptr, ptr %6, align 8, !dbg !1937
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1937
  store float %114, ptr %12, align 4, !dbg !1937
  call void @llvm.va_end(ptr %7), !dbg !1937
  %115 = load float, ptr %12, align 4, !dbg !1937
  ret float %115, !dbg !1937
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !1955 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1956, metadata !DIExpression()), !dbg !1957
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1958, metadata !DIExpression()), !dbg !1957
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1959, metadata !DIExpression()), !dbg !1957
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1960, metadata !DIExpression()), !dbg !1957
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1961, metadata !DIExpression()), !dbg !1957
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1962, metadata !DIExpression()), !dbg !1957
  %13 = load ptr, ptr %8, align 8, !dbg !1957
  %14 = load ptr, ptr %13, align 8, !dbg !1957
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !1957
  %16 = load ptr, ptr %15, align 8, !dbg !1957
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !1957
  %18 = load ptr, ptr %6, align 8, !dbg !1957
  %19 = load ptr, ptr %8, align 8, !dbg !1957
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !1957
  store i32 %20, ptr %10, align 4, !dbg !1957
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1963, metadata !DIExpression()), !dbg !1957
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1964, metadata !DIExpression()), !dbg !1966
  store i32 0, ptr %12, align 4, !dbg !1966
  br label %21, !dbg !1966

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !1966
  %23 = load i32, ptr %10, align 4, !dbg !1966
  %24 = icmp slt i32 %22, %23, !dbg !1966
  br i1 %24, label %25, label %105, !dbg !1966

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !1967
  %27 = sext i32 %26 to i64, !dbg !1967
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !1967
  %29 = load i8, ptr %28, align 1, !dbg !1967
  %30 = sext i8 %29 to i32, !dbg !1967
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
  ], !dbg !1967

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !1970
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !1970
  store ptr %33, ptr %5, align 8, !dbg !1970
  %34 = load i32, ptr %32, align 8, !dbg !1970
  %35 = trunc i32 %34 to i8, !dbg !1970
  %36 = load i32, ptr %12, align 4, !dbg !1970
  %37 = sext i32 %36 to i64, !dbg !1970
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !1970
  store i8 %35, ptr %38, align 8, !dbg !1970
  br label %101, !dbg !1970

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !1970
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !1970
  store ptr %41, ptr %5, align 8, !dbg !1970
  %42 = load i32, ptr %40, align 8, !dbg !1970
  %43 = trunc i32 %42 to i8, !dbg !1970
  %44 = load i32, ptr %12, align 4, !dbg !1970
  %45 = sext i32 %44 to i64, !dbg !1970
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !1970
  store i8 %43, ptr %46, align 8, !dbg !1970
  br label %101, !dbg !1970

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !1970
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !1970
  store ptr %49, ptr %5, align 8, !dbg !1970
  %50 = load i32, ptr %48, align 8, !dbg !1970
  %51 = trunc i32 %50 to i16, !dbg !1970
  %52 = load i32, ptr %12, align 4, !dbg !1970
  %53 = sext i32 %52 to i64, !dbg !1970
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !1970
  store i16 %51, ptr %54, align 8, !dbg !1970
  br label %101, !dbg !1970

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !1970
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !1970
  store ptr %57, ptr %5, align 8, !dbg !1970
  %58 = load i32, ptr %56, align 8, !dbg !1970
  %59 = trunc i32 %58 to i16, !dbg !1970
  %60 = load i32, ptr %12, align 4, !dbg !1970
  %61 = sext i32 %60 to i64, !dbg !1970
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !1970
  store i16 %59, ptr %62, align 8, !dbg !1970
  br label %101, !dbg !1970

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !1970
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !1970
  store ptr %65, ptr %5, align 8, !dbg !1970
  %66 = load i32, ptr %64, align 8, !dbg !1970
  %67 = load i32, ptr %12, align 4, !dbg !1970
  %68 = sext i32 %67 to i64, !dbg !1970
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !1970
  store i32 %66, ptr %69, align 8, !dbg !1970
  br label %101, !dbg !1970

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !1970
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !1970
  store ptr %72, ptr %5, align 8, !dbg !1970
  %73 = load i32, ptr %71, align 8, !dbg !1970
  %74 = sext i32 %73 to i64, !dbg !1970
  %75 = load i32, ptr %12, align 4, !dbg !1970
  %76 = sext i32 %75 to i64, !dbg !1970
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !1970
  store i64 %74, ptr %77, align 8, !dbg !1970
  br label %101, !dbg !1970

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !1970
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !1970
  store ptr %80, ptr %5, align 8, !dbg !1970
  %81 = load double, ptr %79, align 8, !dbg !1970
  %82 = fptrunc double %81 to float, !dbg !1970
  %83 = load i32, ptr %12, align 4, !dbg !1970
  %84 = sext i32 %83 to i64, !dbg !1970
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !1970
  store float %82, ptr %85, align 8, !dbg !1970
  br label %101, !dbg !1970

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !1970
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !1970
  store ptr %88, ptr %5, align 8, !dbg !1970
  %89 = load double, ptr %87, align 8, !dbg !1970
  %90 = load i32, ptr %12, align 4, !dbg !1970
  %91 = sext i32 %90 to i64, !dbg !1970
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !1970
  store double %89, ptr %92, align 8, !dbg !1970
  br label %101, !dbg !1970

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !1970
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !1970
  store ptr %95, ptr %5, align 8, !dbg !1970
  %96 = load ptr, ptr %94, align 8, !dbg !1970
  %97 = load i32, ptr %12, align 4, !dbg !1970
  %98 = sext i32 %97 to i64, !dbg !1970
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !1970
  store ptr %96, ptr %99, align 8, !dbg !1970
  br label %101, !dbg !1970

100:                                              ; preds = %25
  br label %101, !dbg !1970

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !1967

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !1972
  %104 = add nsw i32 %103, 1, !dbg !1972
  store i32 %104, ptr %12, align 4, !dbg !1972
  br label %21, !dbg !1972, !llvm.loop !1973

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !1957
  %107 = load ptr, ptr %106, align 8, !dbg !1957
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 57, !dbg !1957
  %109 = load ptr, ptr %108, align 8, !dbg !1957
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !1957
  %111 = load ptr, ptr %6, align 8, !dbg !1957
  %112 = load ptr, ptr %7, align 8, !dbg !1957
  %113 = load ptr, ptr %8, align 8, !dbg !1957
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !1957
  ret float %114, !dbg !1957
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !1974 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca float, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !1975, metadata !DIExpression()), !dbg !1976
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1977, metadata !DIExpression()), !dbg !1976
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1978, metadata !DIExpression()), !dbg !1976
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1979, metadata !DIExpression()), !dbg !1976
  call void @llvm.dbg.declare(metadata ptr %9, metadata !1980, metadata !DIExpression()), !dbg !1976
  call void @llvm.va_start(ptr %9), !dbg !1976
  call void @llvm.dbg.declare(metadata ptr %10, metadata !1981, metadata !DIExpression()), !dbg !1976
  call void @llvm.dbg.declare(metadata ptr %11, metadata !1982, metadata !DIExpression()), !dbg !1976
  %15 = load ptr, ptr %8, align 8, !dbg !1976
  %16 = load ptr, ptr %15, align 8, !dbg !1976
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1976
  %18 = load ptr, ptr %17, align 8, !dbg !1976
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !1976
  %20 = load ptr, ptr %5, align 8, !dbg !1976
  %21 = load ptr, ptr %8, align 8, !dbg !1976
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1976
  store i32 %22, ptr %11, align 4, !dbg !1976
  call void @llvm.dbg.declare(metadata ptr %12, metadata !1983, metadata !DIExpression()), !dbg !1976
  call void @llvm.dbg.declare(metadata ptr %13, metadata !1984, metadata !DIExpression()), !dbg !1986
  store i32 0, ptr %13, align 4, !dbg !1986
  br label %23, !dbg !1986

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !1986
  %25 = load i32, ptr %11, align 4, !dbg !1986
  %26 = icmp slt i32 %24, %25, !dbg !1986
  br i1 %26, label %27, label %107, !dbg !1986

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !1987
  %29 = sext i32 %28 to i64, !dbg !1987
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !1987
  %31 = load i8, ptr %30, align 1, !dbg !1987
  %32 = sext i8 %31 to i32, !dbg !1987
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
  ], !dbg !1987

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !1990
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !1990
  store ptr %35, ptr %9, align 8, !dbg !1990
  %36 = load i32, ptr %34, align 8, !dbg !1990
  %37 = trunc i32 %36 to i8, !dbg !1990
  %38 = load i32, ptr %13, align 4, !dbg !1990
  %39 = sext i32 %38 to i64, !dbg !1990
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !1990
  store i8 %37, ptr %40, align 8, !dbg !1990
  br label %103, !dbg !1990

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !1990
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !1990
  store ptr %43, ptr %9, align 8, !dbg !1990
  %44 = load i32, ptr %42, align 8, !dbg !1990
  %45 = trunc i32 %44 to i8, !dbg !1990
  %46 = load i32, ptr %13, align 4, !dbg !1990
  %47 = sext i32 %46 to i64, !dbg !1990
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !1990
  store i8 %45, ptr %48, align 8, !dbg !1990
  br label %103, !dbg !1990

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !1990
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !1990
  store ptr %51, ptr %9, align 8, !dbg !1990
  %52 = load i32, ptr %50, align 8, !dbg !1990
  %53 = trunc i32 %52 to i16, !dbg !1990
  %54 = load i32, ptr %13, align 4, !dbg !1990
  %55 = sext i32 %54 to i64, !dbg !1990
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !1990
  store i16 %53, ptr %56, align 8, !dbg !1990
  br label %103, !dbg !1990

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !1990
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !1990
  store ptr %59, ptr %9, align 8, !dbg !1990
  %60 = load i32, ptr %58, align 8, !dbg !1990
  %61 = trunc i32 %60 to i16, !dbg !1990
  %62 = load i32, ptr %13, align 4, !dbg !1990
  %63 = sext i32 %62 to i64, !dbg !1990
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !1990
  store i16 %61, ptr %64, align 8, !dbg !1990
  br label %103, !dbg !1990

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !1990
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !1990
  store ptr %67, ptr %9, align 8, !dbg !1990
  %68 = load i32, ptr %66, align 8, !dbg !1990
  %69 = load i32, ptr %13, align 4, !dbg !1990
  %70 = sext i32 %69 to i64, !dbg !1990
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !1990
  store i32 %68, ptr %71, align 8, !dbg !1990
  br label %103, !dbg !1990

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !1990
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !1990
  store ptr %74, ptr %9, align 8, !dbg !1990
  %75 = load i32, ptr %73, align 8, !dbg !1990
  %76 = sext i32 %75 to i64, !dbg !1990
  %77 = load i32, ptr %13, align 4, !dbg !1990
  %78 = sext i32 %77 to i64, !dbg !1990
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !1990
  store i64 %76, ptr %79, align 8, !dbg !1990
  br label %103, !dbg !1990

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !1990
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !1990
  store ptr %82, ptr %9, align 8, !dbg !1990
  %83 = load double, ptr %81, align 8, !dbg !1990
  %84 = fptrunc double %83 to float, !dbg !1990
  %85 = load i32, ptr %13, align 4, !dbg !1990
  %86 = sext i32 %85 to i64, !dbg !1990
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !1990
  store float %84, ptr %87, align 8, !dbg !1990
  br label %103, !dbg !1990

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !1990
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !1990
  store ptr %90, ptr %9, align 8, !dbg !1990
  %91 = load double, ptr %89, align 8, !dbg !1990
  %92 = load i32, ptr %13, align 4, !dbg !1990
  %93 = sext i32 %92 to i64, !dbg !1990
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !1990
  store double %91, ptr %94, align 8, !dbg !1990
  br label %103, !dbg !1990

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !1990
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !1990
  store ptr %97, ptr %9, align 8, !dbg !1990
  %98 = load ptr, ptr %96, align 8, !dbg !1990
  %99 = load i32, ptr %13, align 4, !dbg !1990
  %100 = sext i32 %99 to i64, !dbg !1990
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !1990
  store ptr %98, ptr %101, align 8, !dbg !1990
  br label %103, !dbg !1990

102:                                              ; preds = %27
  br label %103, !dbg !1990

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !1987

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !1992
  %106 = add nsw i32 %105, 1, !dbg !1992
  store i32 %106, ptr %13, align 4, !dbg !1992
  br label %23, !dbg !1992, !llvm.loop !1993

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !1994, metadata !DIExpression()), !dbg !1976
  %108 = load ptr, ptr %8, align 8, !dbg !1976
  %109 = load ptr, ptr %108, align 8, !dbg !1976
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 87, !dbg !1976
  %111 = load ptr, ptr %110, align 8, !dbg !1976
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !1976
  %113 = load ptr, ptr %5, align 8, !dbg !1976
  %114 = load ptr, ptr %6, align 8, !dbg !1976
  %115 = load ptr, ptr %7, align 8, !dbg !1976
  %116 = load ptr, ptr %8, align 8, !dbg !1976
  %117 = call float %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1976
  store float %117, ptr %14, align 4, !dbg !1976
  call void @llvm.va_end(ptr %9), !dbg !1976
  %118 = load float, ptr %14, align 4, !dbg !1976
  ret float %118, !dbg !1976
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !1995 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !1996, metadata !DIExpression()), !dbg !1997
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !1998, metadata !DIExpression()), !dbg !1997
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !1999, metadata !DIExpression()), !dbg !1997
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2000, metadata !DIExpression()), !dbg !1997
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2001, metadata !DIExpression()), !dbg !1997
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2002, metadata !DIExpression()), !dbg !1997
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2003, metadata !DIExpression()), !dbg !1997
  %15 = load ptr, ptr %10, align 8, !dbg !1997
  %16 = load ptr, ptr %15, align 8, !dbg !1997
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !1997
  %18 = load ptr, ptr %17, align 8, !dbg !1997
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !1997
  %20 = load ptr, ptr %7, align 8, !dbg !1997
  %21 = load ptr, ptr %10, align 8, !dbg !1997
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !1997
  store i32 %22, ptr %12, align 4, !dbg !1997
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2004, metadata !DIExpression()), !dbg !1997
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2005, metadata !DIExpression()), !dbg !2007
  store i32 0, ptr %14, align 4, !dbg !2007
  br label %23, !dbg !2007

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !2007
  %25 = load i32, ptr %12, align 4, !dbg !2007
  %26 = icmp slt i32 %24, %25, !dbg !2007
  br i1 %26, label %27, label %107, !dbg !2007

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2008
  %29 = sext i32 %28 to i64, !dbg !2008
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !2008
  %31 = load i8, ptr %30, align 1, !dbg !2008
  %32 = sext i8 %31 to i32, !dbg !2008
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
  ], !dbg !2008

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !2011
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !2011
  store ptr %35, ptr %6, align 8, !dbg !2011
  %36 = load i32, ptr %34, align 8, !dbg !2011
  %37 = trunc i32 %36 to i8, !dbg !2011
  %38 = load i32, ptr %14, align 4, !dbg !2011
  %39 = sext i32 %38 to i64, !dbg !2011
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !2011
  store i8 %37, ptr %40, align 8, !dbg !2011
  br label %103, !dbg !2011

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !2011
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !2011
  store ptr %43, ptr %6, align 8, !dbg !2011
  %44 = load i32, ptr %42, align 8, !dbg !2011
  %45 = trunc i32 %44 to i8, !dbg !2011
  %46 = load i32, ptr %14, align 4, !dbg !2011
  %47 = sext i32 %46 to i64, !dbg !2011
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !2011
  store i8 %45, ptr %48, align 8, !dbg !2011
  br label %103, !dbg !2011

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !2011
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !2011
  store ptr %51, ptr %6, align 8, !dbg !2011
  %52 = load i32, ptr %50, align 8, !dbg !2011
  %53 = trunc i32 %52 to i16, !dbg !2011
  %54 = load i32, ptr %14, align 4, !dbg !2011
  %55 = sext i32 %54 to i64, !dbg !2011
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !2011
  store i16 %53, ptr %56, align 8, !dbg !2011
  br label %103, !dbg !2011

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !2011
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !2011
  store ptr %59, ptr %6, align 8, !dbg !2011
  %60 = load i32, ptr %58, align 8, !dbg !2011
  %61 = trunc i32 %60 to i16, !dbg !2011
  %62 = load i32, ptr %14, align 4, !dbg !2011
  %63 = sext i32 %62 to i64, !dbg !2011
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !2011
  store i16 %61, ptr %64, align 8, !dbg !2011
  br label %103, !dbg !2011

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !2011
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !2011
  store ptr %67, ptr %6, align 8, !dbg !2011
  %68 = load i32, ptr %66, align 8, !dbg !2011
  %69 = load i32, ptr %14, align 4, !dbg !2011
  %70 = sext i32 %69 to i64, !dbg !2011
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !2011
  store i32 %68, ptr %71, align 8, !dbg !2011
  br label %103, !dbg !2011

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !2011
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !2011
  store ptr %74, ptr %6, align 8, !dbg !2011
  %75 = load i32, ptr %73, align 8, !dbg !2011
  %76 = sext i32 %75 to i64, !dbg !2011
  %77 = load i32, ptr %14, align 4, !dbg !2011
  %78 = sext i32 %77 to i64, !dbg !2011
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !2011
  store i64 %76, ptr %79, align 8, !dbg !2011
  br label %103, !dbg !2011

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !2011
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !2011
  store ptr %82, ptr %6, align 8, !dbg !2011
  %83 = load double, ptr %81, align 8, !dbg !2011
  %84 = fptrunc double %83 to float, !dbg !2011
  %85 = load i32, ptr %14, align 4, !dbg !2011
  %86 = sext i32 %85 to i64, !dbg !2011
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !2011
  store float %84, ptr %87, align 8, !dbg !2011
  br label %103, !dbg !2011

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !2011
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !2011
  store ptr %90, ptr %6, align 8, !dbg !2011
  %91 = load double, ptr %89, align 8, !dbg !2011
  %92 = load i32, ptr %14, align 4, !dbg !2011
  %93 = sext i32 %92 to i64, !dbg !2011
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !2011
  store double %91, ptr %94, align 8, !dbg !2011
  br label %103, !dbg !2011

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !2011
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !2011
  store ptr %97, ptr %6, align 8, !dbg !2011
  %98 = load ptr, ptr %96, align 8, !dbg !2011
  %99 = load i32, ptr %14, align 4, !dbg !2011
  %100 = sext i32 %99 to i64, !dbg !2011
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !2011
  store ptr %98, ptr %101, align 8, !dbg !2011
  br label %103, !dbg !2011

102:                                              ; preds = %27
  br label %103, !dbg !2011

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !2008

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !2013
  %106 = add nsw i32 %105, 1, !dbg !2013
  store i32 %106, ptr %14, align 4, !dbg !2013
  br label %23, !dbg !2013, !llvm.loop !2014

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !1997
  %109 = load ptr, ptr %108, align 8, !dbg !1997
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 87, !dbg !1997
  %111 = load ptr, ptr %110, align 8, !dbg !1997
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !1997
  %113 = load ptr, ptr %7, align 8, !dbg !1997
  %114 = load ptr, ptr %8, align 8, !dbg !1997
  %115 = load ptr, ptr %9, align 8, !dbg !1997
  %116 = load ptr, ptr %10, align 8, !dbg !1997
  %117 = call float %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !1997
  ret float %117, !dbg !1997
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2015 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca float, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2016, metadata !DIExpression()), !dbg !2017
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2018, metadata !DIExpression()), !dbg !2017
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2019, metadata !DIExpression()), !dbg !2017
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2020, metadata !DIExpression()), !dbg !2017
  call void @llvm.va_start(ptr %7), !dbg !2017
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2021, metadata !DIExpression()), !dbg !2017
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2022, metadata !DIExpression()), !dbg !2017
  %13 = load ptr, ptr %6, align 8, !dbg !2017
  %14 = load ptr, ptr %13, align 8, !dbg !2017
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2017
  %16 = load ptr, ptr %15, align 8, !dbg !2017
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2017
  %18 = load ptr, ptr %4, align 8, !dbg !2017
  %19 = load ptr, ptr %6, align 8, !dbg !2017
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2017
  store i32 %20, ptr %9, align 4, !dbg !2017
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2023, metadata !DIExpression()), !dbg !2017
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2024, metadata !DIExpression()), !dbg !2026
  store i32 0, ptr %11, align 4, !dbg !2026
  br label %21, !dbg !2026

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !2026
  %23 = load i32, ptr %9, align 4, !dbg !2026
  %24 = icmp slt i32 %22, %23, !dbg !2026
  br i1 %24, label %25, label %105, !dbg !2026

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2027
  %27 = sext i32 %26 to i64, !dbg !2027
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !2027
  %29 = load i8, ptr %28, align 1, !dbg !2027
  %30 = sext i8 %29 to i32, !dbg !2027
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
  ], !dbg !2027

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !2030
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2030
  store ptr %33, ptr %7, align 8, !dbg !2030
  %34 = load i32, ptr %32, align 8, !dbg !2030
  %35 = trunc i32 %34 to i8, !dbg !2030
  %36 = load i32, ptr %11, align 4, !dbg !2030
  %37 = sext i32 %36 to i64, !dbg !2030
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !2030
  store i8 %35, ptr %38, align 8, !dbg !2030
  br label %101, !dbg !2030

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !2030
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2030
  store ptr %41, ptr %7, align 8, !dbg !2030
  %42 = load i32, ptr %40, align 8, !dbg !2030
  %43 = trunc i32 %42 to i8, !dbg !2030
  %44 = load i32, ptr %11, align 4, !dbg !2030
  %45 = sext i32 %44 to i64, !dbg !2030
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !2030
  store i8 %43, ptr %46, align 8, !dbg !2030
  br label %101, !dbg !2030

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !2030
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2030
  store ptr %49, ptr %7, align 8, !dbg !2030
  %50 = load i32, ptr %48, align 8, !dbg !2030
  %51 = trunc i32 %50 to i16, !dbg !2030
  %52 = load i32, ptr %11, align 4, !dbg !2030
  %53 = sext i32 %52 to i64, !dbg !2030
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !2030
  store i16 %51, ptr %54, align 8, !dbg !2030
  br label %101, !dbg !2030

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !2030
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2030
  store ptr %57, ptr %7, align 8, !dbg !2030
  %58 = load i32, ptr %56, align 8, !dbg !2030
  %59 = trunc i32 %58 to i16, !dbg !2030
  %60 = load i32, ptr %11, align 4, !dbg !2030
  %61 = sext i32 %60 to i64, !dbg !2030
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !2030
  store i16 %59, ptr %62, align 8, !dbg !2030
  br label %101, !dbg !2030

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !2030
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2030
  store ptr %65, ptr %7, align 8, !dbg !2030
  %66 = load i32, ptr %64, align 8, !dbg !2030
  %67 = load i32, ptr %11, align 4, !dbg !2030
  %68 = sext i32 %67 to i64, !dbg !2030
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !2030
  store i32 %66, ptr %69, align 8, !dbg !2030
  br label %101, !dbg !2030

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !2030
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2030
  store ptr %72, ptr %7, align 8, !dbg !2030
  %73 = load i32, ptr %71, align 8, !dbg !2030
  %74 = sext i32 %73 to i64, !dbg !2030
  %75 = load i32, ptr %11, align 4, !dbg !2030
  %76 = sext i32 %75 to i64, !dbg !2030
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !2030
  store i64 %74, ptr %77, align 8, !dbg !2030
  br label %101, !dbg !2030

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !2030
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2030
  store ptr %80, ptr %7, align 8, !dbg !2030
  %81 = load double, ptr %79, align 8, !dbg !2030
  %82 = fptrunc double %81 to float, !dbg !2030
  %83 = load i32, ptr %11, align 4, !dbg !2030
  %84 = sext i32 %83 to i64, !dbg !2030
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !2030
  store float %82, ptr %85, align 8, !dbg !2030
  br label %101, !dbg !2030

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !2030
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2030
  store ptr %88, ptr %7, align 8, !dbg !2030
  %89 = load double, ptr %87, align 8, !dbg !2030
  %90 = load i32, ptr %11, align 4, !dbg !2030
  %91 = sext i32 %90 to i64, !dbg !2030
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !2030
  store double %89, ptr %92, align 8, !dbg !2030
  br label %101, !dbg !2030

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !2030
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2030
  store ptr %95, ptr %7, align 8, !dbg !2030
  %96 = load ptr, ptr %94, align 8, !dbg !2030
  %97 = load i32, ptr %11, align 4, !dbg !2030
  %98 = sext i32 %97 to i64, !dbg !2030
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !2030
  store ptr %96, ptr %99, align 8, !dbg !2030
  br label %101, !dbg !2030

100:                                              ; preds = %25
  br label %101, !dbg !2030

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2027

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !2032
  %104 = add nsw i32 %103, 1, !dbg !2032
  store i32 %104, ptr %11, align 4, !dbg !2032
  br label %21, !dbg !2032, !llvm.loop !2033

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2034, metadata !DIExpression()), !dbg !2017
  %106 = load ptr, ptr %6, align 8, !dbg !2017
  %107 = load ptr, ptr %106, align 8, !dbg !2017
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 137, !dbg !2017
  %109 = load ptr, ptr %108, align 8, !dbg !2017
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2017
  %111 = load ptr, ptr %4, align 8, !dbg !2017
  %112 = load ptr, ptr %5, align 8, !dbg !2017
  %113 = load ptr, ptr %6, align 8, !dbg !2017
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2017
  store float %114, ptr %12, align 4, !dbg !2017
  call void @llvm.va_end(ptr %7), !dbg !2017
  %115 = load float, ptr %12, align 4, !dbg !2017
  ret float %115, !dbg !2017
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2035 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2036, metadata !DIExpression()), !dbg !2037
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2038, metadata !DIExpression()), !dbg !2037
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2039, metadata !DIExpression()), !dbg !2037
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2040, metadata !DIExpression()), !dbg !2037
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2041, metadata !DIExpression()), !dbg !2037
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2042, metadata !DIExpression()), !dbg !2037
  %13 = load ptr, ptr %8, align 8, !dbg !2037
  %14 = load ptr, ptr %13, align 8, !dbg !2037
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2037
  %16 = load ptr, ptr %15, align 8, !dbg !2037
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2037
  %18 = load ptr, ptr %6, align 8, !dbg !2037
  %19 = load ptr, ptr %8, align 8, !dbg !2037
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2037
  store i32 %20, ptr %10, align 4, !dbg !2037
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2043, metadata !DIExpression()), !dbg !2037
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2044, metadata !DIExpression()), !dbg !2046
  store i32 0, ptr %12, align 4, !dbg !2046
  br label %21, !dbg !2046

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2046
  %23 = load i32, ptr %10, align 4, !dbg !2046
  %24 = icmp slt i32 %22, %23, !dbg !2046
  br i1 %24, label %25, label %105, !dbg !2046

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2047
  %27 = sext i32 %26 to i64, !dbg !2047
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2047
  %29 = load i8, ptr %28, align 1, !dbg !2047
  %30 = sext i8 %29 to i32, !dbg !2047
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
  ], !dbg !2047

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2050
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2050
  store ptr %33, ptr %5, align 8, !dbg !2050
  %34 = load i32, ptr %32, align 8, !dbg !2050
  %35 = trunc i32 %34 to i8, !dbg !2050
  %36 = load i32, ptr %12, align 4, !dbg !2050
  %37 = sext i32 %36 to i64, !dbg !2050
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2050
  store i8 %35, ptr %38, align 8, !dbg !2050
  br label %101, !dbg !2050

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2050
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2050
  store ptr %41, ptr %5, align 8, !dbg !2050
  %42 = load i32, ptr %40, align 8, !dbg !2050
  %43 = trunc i32 %42 to i8, !dbg !2050
  %44 = load i32, ptr %12, align 4, !dbg !2050
  %45 = sext i32 %44 to i64, !dbg !2050
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2050
  store i8 %43, ptr %46, align 8, !dbg !2050
  br label %101, !dbg !2050

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2050
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2050
  store ptr %49, ptr %5, align 8, !dbg !2050
  %50 = load i32, ptr %48, align 8, !dbg !2050
  %51 = trunc i32 %50 to i16, !dbg !2050
  %52 = load i32, ptr %12, align 4, !dbg !2050
  %53 = sext i32 %52 to i64, !dbg !2050
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2050
  store i16 %51, ptr %54, align 8, !dbg !2050
  br label %101, !dbg !2050

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2050
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2050
  store ptr %57, ptr %5, align 8, !dbg !2050
  %58 = load i32, ptr %56, align 8, !dbg !2050
  %59 = trunc i32 %58 to i16, !dbg !2050
  %60 = load i32, ptr %12, align 4, !dbg !2050
  %61 = sext i32 %60 to i64, !dbg !2050
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2050
  store i16 %59, ptr %62, align 8, !dbg !2050
  br label %101, !dbg !2050

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2050
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2050
  store ptr %65, ptr %5, align 8, !dbg !2050
  %66 = load i32, ptr %64, align 8, !dbg !2050
  %67 = load i32, ptr %12, align 4, !dbg !2050
  %68 = sext i32 %67 to i64, !dbg !2050
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2050
  store i32 %66, ptr %69, align 8, !dbg !2050
  br label %101, !dbg !2050

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2050
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2050
  store ptr %72, ptr %5, align 8, !dbg !2050
  %73 = load i32, ptr %71, align 8, !dbg !2050
  %74 = sext i32 %73 to i64, !dbg !2050
  %75 = load i32, ptr %12, align 4, !dbg !2050
  %76 = sext i32 %75 to i64, !dbg !2050
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2050
  store i64 %74, ptr %77, align 8, !dbg !2050
  br label %101, !dbg !2050

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2050
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2050
  store ptr %80, ptr %5, align 8, !dbg !2050
  %81 = load double, ptr %79, align 8, !dbg !2050
  %82 = fptrunc double %81 to float, !dbg !2050
  %83 = load i32, ptr %12, align 4, !dbg !2050
  %84 = sext i32 %83 to i64, !dbg !2050
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2050
  store float %82, ptr %85, align 8, !dbg !2050
  br label %101, !dbg !2050

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2050
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2050
  store ptr %88, ptr %5, align 8, !dbg !2050
  %89 = load double, ptr %87, align 8, !dbg !2050
  %90 = load i32, ptr %12, align 4, !dbg !2050
  %91 = sext i32 %90 to i64, !dbg !2050
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2050
  store double %89, ptr %92, align 8, !dbg !2050
  br label %101, !dbg !2050

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2050
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2050
  store ptr %95, ptr %5, align 8, !dbg !2050
  %96 = load ptr, ptr %94, align 8, !dbg !2050
  %97 = load i32, ptr %12, align 4, !dbg !2050
  %98 = sext i32 %97 to i64, !dbg !2050
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2050
  store ptr %96, ptr %99, align 8, !dbg !2050
  br label %101, !dbg !2050

100:                                              ; preds = %25
  br label %101, !dbg !2050

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2047

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2052
  %104 = add nsw i32 %103, 1, !dbg !2052
  store i32 %104, ptr %12, align 4, !dbg !2052
  br label %21, !dbg !2052, !llvm.loop !2053

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2037
  %107 = load ptr, ptr %106, align 8, !dbg !2037
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 137, !dbg !2037
  %109 = load ptr, ptr %108, align 8, !dbg !2037
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2037
  %111 = load ptr, ptr %6, align 8, !dbg !2037
  %112 = load ptr, ptr %7, align 8, !dbg !2037
  %113 = load ptr, ptr %8, align 8, !dbg !2037
  %114 = call float %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2037
  ret float %114, !dbg !2037
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2054 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2055, metadata !DIExpression()), !dbg !2056
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2057, metadata !DIExpression()), !dbg !2056
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2058, metadata !DIExpression()), !dbg !2056
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2059, metadata !DIExpression()), !dbg !2056
  call void @llvm.va_start(ptr %7), !dbg !2056
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2060, metadata !DIExpression()), !dbg !2056
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2061, metadata !DIExpression()), !dbg !2056
  %13 = load ptr, ptr %6, align 8, !dbg !2056
  %14 = load ptr, ptr %13, align 8, !dbg !2056
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2056
  %16 = load ptr, ptr %15, align 8, !dbg !2056
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2056
  %18 = load ptr, ptr %4, align 8, !dbg !2056
  %19 = load ptr, ptr %6, align 8, !dbg !2056
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2056
  store i32 %20, ptr %9, align 4, !dbg !2056
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2062, metadata !DIExpression()), !dbg !2056
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2063, metadata !DIExpression()), !dbg !2065
  store i32 0, ptr %11, align 4, !dbg !2065
  br label %21, !dbg !2065

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !2065
  %23 = load i32, ptr %9, align 4, !dbg !2065
  %24 = icmp slt i32 %22, %23, !dbg !2065
  br i1 %24, label %25, label %105, !dbg !2065

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2066
  %27 = sext i32 %26 to i64, !dbg !2066
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !2066
  %29 = load i8, ptr %28, align 1, !dbg !2066
  %30 = sext i8 %29 to i32, !dbg !2066
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
  ], !dbg !2066

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !2069
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2069
  store ptr %33, ptr %7, align 8, !dbg !2069
  %34 = load i32, ptr %32, align 8, !dbg !2069
  %35 = trunc i32 %34 to i8, !dbg !2069
  %36 = load i32, ptr %11, align 4, !dbg !2069
  %37 = sext i32 %36 to i64, !dbg !2069
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !2069
  store i8 %35, ptr %38, align 8, !dbg !2069
  br label %101, !dbg !2069

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !2069
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2069
  store ptr %41, ptr %7, align 8, !dbg !2069
  %42 = load i32, ptr %40, align 8, !dbg !2069
  %43 = trunc i32 %42 to i8, !dbg !2069
  %44 = load i32, ptr %11, align 4, !dbg !2069
  %45 = sext i32 %44 to i64, !dbg !2069
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !2069
  store i8 %43, ptr %46, align 8, !dbg !2069
  br label %101, !dbg !2069

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !2069
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2069
  store ptr %49, ptr %7, align 8, !dbg !2069
  %50 = load i32, ptr %48, align 8, !dbg !2069
  %51 = trunc i32 %50 to i16, !dbg !2069
  %52 = load i32, ptr %11, align 4, !dbg !2069
  %53 = sext i32 %52 to i64, !dbg !2069
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !2069
  store i16 %51, ptr %54, align 8, !dbg !2069
  br label %101, !dbg !2069

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !2069
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2069
  store ptr %57, ptr %7, align 8, !dbg !2069
  %58 = load i32, ptr %56, align 8, !dbg !2069
  %59 = trunc i32 %58 to i16, !dbg !2069
  %60 = load i32, ptr %11, align 4, !dbg !2069
  %61 = sext i32 %60 to i64, !dbg !2069
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !2069
  store i16 %59, ptr %62, align 8, !dbg !2069
  br label %101, !dbg !2069

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !2069
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2069
  store ptr %65, ptr %7, align 8, !dbg !2069
  %66 = load i32, ptr %64, align 8, !dbg !2069
  %67 = load i32, ptr %11, align 4, !dbg !2069
  %68 = sext i32 %67 to i64, !dbg !2069
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !2069
  store i32 %66, ptr %69, align 8, !dbg !2069
  br label %101, !dbg !2069

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !2069
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2069
  store ptr %72, ptr %7, align 8, !dbg !2069
  %73 = load i32, ptr %71, align 8, !dbg !2069
  %74 = sext i32 %73 to i64, !dbg !2069
  %75 = load i32, ptr %11, align 4, !dbg !2069
  %76 = sext i32 %75 to i64, !dbg !2069
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !2069
  store i64 %74, ptr %77, align 8, !dbg !2069
  br label %101, !dbg !2069

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !2069
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2069
  store ptr %80, ptr %7, align 8, !dbg !2069
  %81 = load double, ptr %79, align 8, !dbg !2069
  %82 = fptrunc double %81 to float, !dbg !2069
  %83 = load i32, ptr %11, align 4, !dbg !2069
  %84 = sext i32 %83 to i64, !dbg !2069
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !2069
  store float %82, ptr %85, align 8, !dbg !2069
  br label %101, !dbg !2069

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !2069
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2069
  store ptr %88, ptr %7, align 8, !dbg !2069
  %89 = load double, ptr %87, align 8, !dbg !2069
  %90 = load i32, ptr %11, align 4, !dbg !2069
  %91 = sext i32 %90 to i64, !dbg !2069
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !2069
  store double %89, ptr %92, align 8, !dbg !2069
  br label %101, !dbg !2069

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !2069
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2069
  store ptr %95, ptr %7, align 8, !dbg !2069
  %96 = load ptr, ptr %94, align 8, !dbg !2069
  %97 = load i32, ptr %11, align 4, !dbg !2069
  %98 = sext i32 %97 to i64, !dbg !2069
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !2069
  store ptr %96, ptr %99, align 8, !dbg !2069
  br label %101, !dbg !2069

100:                                              ; preds = %25
  br label %101, !dbg !2069

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2066

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !2071
  %104 = add nsw i32 %103, 1, !dbg !2071
  store i32 %104, ptr %11, align 4, !dbg !2071
  br label %21, !dbg !2071, !llvm.loop !2072

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2073, metadata !DIExpression()), !dbg !2056
  %106 = load ptr, ptr %6, align 8, !dbg !2056
  %107 = load ptr, ptr %106, align 8, !dbg !2056
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 60, !dbg !2056
  %109 = load ptr, ptr %108, align 8, !dbg !2056
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2056
  %111 = load ptr, ptr %4, align 8, !dbg !2056
  %112 = load ptr, ptr %5, align 8, !dbg !2056
  %113 = load ptr, ptr %6, align 8, !dbg !2056
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2056
  store double %114, ptr %12, align 8, !dbg !2056
  call void @llvm.va_end(ptr %7), !dbg !2056
  %115 = load double, ptr %12, align 8, !dbg !2056
  ret double %115, !dbg !2056
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2074 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2075, metadata !DIExpression()), !dbg !2076
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2077, metadata !DIExpression()), !dbg !2076
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2078, metadata !DIExpression()), !dbg !2076
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2079, metadata !DIExpression()), !dbg !2076
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2080, metadata !DIExpression()), !dbg !2076
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2081, metadata !DIExpression()), !dbg !2076
  %13 = load ptr, ptr %8, align 8, !dbg !2076
  %14 = load ptr, ptr %13, align 8, !dbg !2076
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2076
  %16 = load ptr, ptr %15, align 8, !dbg !2076
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2076
  %18 = load ptr, ptr %6, align 8, !dbg !2076
  %19 = load ptr, ptr %8, align 8, !dbg !2076
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2076
  store i32 %20, ptr %10, align 4, !dbg !2076
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2082, metadata !DIExpression()), !dbg !2076
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2083, metadata !DIExpression()), !dbg !2085
  store i32 0, ptr %12, align 4, !dbg !2085
  br label %21, !dbg !2085

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2085
  %23 = load i32, ptr %10, align 4, !dbg !2085
  %24 = icmp slt i32 %22, %23, !dbg !2085
  br i1 %24, label %25, label %105, !dbg !2085

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2086
  %27 = sext i32 %26 to i64, !dbg !2086
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2086
  %29 = load i8, ptr %28, align 1, !dbg !2086
  %30 = sext i8 %29 to i32, !dbg !2086
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
  ], !dbg !2086

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2089
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2089
  store ptr %33, ptr %5, align 8, !dbg !2089
  %34 = load i32, ptr %32, align 8, !dbg !2089
  %35 = trunc i32 %34 to i8, !dbg !2089
  %36 = load i32, ptr %12, align 4, !dbg !2089
  %37 = sext i32 %36 to i64, !dbg !2089
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2089
  store i8 %35, ptr %38, align 8, !dbg !2089
  br label %101, !dbg !2089

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2089
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2089
  store ptr %41, ptr %5, align 8, !dbg !2089
  %42 = load i32, ptr %40, align 8, !dbg !2089
  %43 = trunc i32 %42 to i8, !dbg !2089
  %44 = load i32, ptr %12, align 4, !dbg !2089
  %45 = sext i32 %44 to i64, !dbg !2089
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2089
  store i8 %43, ptr %46, align 8, !dbg !2089
  br label %101, !dbg !2089

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2089
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2089
  store ptr %49, ptr %5, align 8, !dbg !2089
  %50 = load i32, ptr %48, align 8, !dbg !2089
  %51 = trunc i32 %50 to i16, !dbg !2089
  %52 = load i32, ptr %12, align 4, !dbg !2089
  %53 = sext i32 %52 to i64, !dbg !2089
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2089
  store i16 %51, ptr %54, align 8, !dbg !2089
  br label %101, !dbg !2089

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2089
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2089
  store ptr %57, ptr %5, align 8, !dbg !2089
  %58 = load i32, ptr %56, align 8, !dbg !2089
  %59 = trunc i32 %58 to i16, !dbg !2089
  %60 = load i32, ptr %12, align 4, !dbg !2089
  %61 = sext i32 %60 to i64, !dbg !2089
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2089
  store i16 %59, ptr %62, align 8, !dbg !2089
  br label %101, !dbg !2089

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2089
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2089
  store ptr %65, ptr %5, align 8, !dbg !2089
  %66 = load i32, ptr %64, align 8, !dbg !2089
  %67 = load i32, ptr %12, align 4, !dbg !2089
  %68 = sext i32 %67 to i64, !dbg !2089
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2089
  store i32 %66, ptr %69, align 8, !dbg !2089
  br label %101, !dbg !2089

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2089
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2089
  store ptr %72, ptr %5, align 8, !dbg !2089
  %73 = load i32, ptr %71, align 8, !dbg !2089
  %74 = sext i32 %73 to i64, !dbg !2089
  %75 = load i32, ptr %12, align 4, !dbg !2089
  %76 = sext i32 %75 to i64, !dbg !2089
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2089
  store i64 %74, ptr %77, align 8, !dbg !2089
  br label %101, !dbg !2089

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2089
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2089
  store ptr %80, ptr %5, align 8, !dbg !2089
  %81 = load double, ptr %79, align 8, !dbg !2089
  %82 = fptrunc double %81 to float, !dbg !2089
  %83 = load i32, ptr %12, align 4, !dbg !2089
  %84 = sext i32 %83 to i64, !dbg !2089
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2089
  store float %82, ptr %85, align 8, !dbg !2089
  br label %101, !dbg !2089

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2089
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2089
  store ptr %88, ptr %5, align 8, !dbg !2089
  %89 = load double, ptr %87, align 8, !dbg !2089
  %90 = load i32, ptr %12, align 4, !dbg !2089
  %91 = sext i32 %90 to i64, !dbg !2089
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2089
  store double %89, ptr %92, align 8, !dbg !2089
  br label %101, !dbg !2089

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2089
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2089
  store ptr %95, ptr %5, align 8, !dbg !2089
  %96 = load ptr, ptr %94, align 8, !dbg !2089
  %97 = load i32, ptr %12, align 4, !dbg !2089
  %98 = sext i32 %97 to i64, !dbg !2089
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2089
  store ptr %96, ptr %99, align 8, !dbg !2089
  br label %101, !dbg !2089

100:                                              ; preds = %25
  br label %101, !dbg !2089

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2086

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2091
  %104 = add nsw i32 %103, 1, !dbg !2091
  store i32 %104, ptr %12, align 4, !dbg !2091
  br label %21, !dbg !2091, !llvm.loop !2092

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2076
  %107 = load ptr, ptr %106, align 8, !dbg !2076
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 60, !dbg !2076
  %109 = load ptr, ptr %108, align 8, !dbg !2076
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2076
  %111 = load ptr, ptr %6, align 8, !dbg !2076
  %112 = load ptr, ptr %7, align 8, !dbg !2076
  %113 = load ptr, ptr %8, align 8, !dbg !2076
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2076
  ret double %114, !dbg !2076
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2093 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  %14 = alloca double, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2094, metadata !DIExpression()), !dbg !2095
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2096, metadata !DIExpression()), !dbg !2095
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2097, metadata !DIExpression()), !dbg !2095
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2098, metadata !DIExpression()), !dbg !2095
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2099, metadata !DIExpression()), !dbg !2095
  call void @llvm.va_start(ptr %9), !dbg !2095
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2100, metadata !DIExpression()), !dbg !2095
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2101, metadata !DIExpression()), !dbg !2095
  %15 = load ptr, ptr %8, align 8, !dbg !2095
  %16 = load ptr, ptr %15, align 8, !dbg !2095
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2095
  %18 = load ptr, ptr %17, align 8, !dbg !2095
  %19 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !2095
  %20 = load ptr, ptr %5, align 8, !dbg !2095
  %21 = load ptr, ptr %8, align 8, !dbg !2095
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2095
  store i32 %22, ptr %11, align 4, !dbg !2095
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2102, metadata !DIExpression()), !dbg !2095
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2103, metadata !DIExpression()), !dbg !2105
  store i32 0, ptr %13, align 4, !dbg !2105
  br label %23, !dbg !2105

23:                                               ; preds = %104, %4
  %24 = load i32, ptr %13, align 4, !dbg !2105
  %25 = load i32, ptr %11, align 4, !dbg !2105
  %26 = icmp slt i32 %24, %25, !dbg !2105
  br i1 %26, label %27, label %107, !dbg !2105

27:                                               ; preds = %23
  %28 = load i32, ptr %13, align 4, !dbg !2106
  %29 = sext i32 %28 to i64, !dbg !2106
  %30 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %29, !dbg !2106
  %31 = load i8, ptr %30, align 1, !dbg !2106
  %32 = sext i8 %31 to i32, !dbg !2106
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
  ], !dbg !2106

33:                                               ; preds = %27
  %34 = load ptr, ptr %9, align 8, !dbg !2109
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !2109
  store ptr %35, ptr %9, align 8, !dbg !2109
  %36 = load i32, ptr %34, align 8, !dbg !2109
  %37 = trunc i32 %36 to i8, !dbg !2109
  %38 = load i32, ptr %13, align 4, !dbg !2109
  %39 = sext i32 %38 to i64, !dbg !2109
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %39, !dbg !2109
  store i8 %37, ptr %40, align 8, !dbg !2109
  br label %103, !dbg !2109

41:                                               ; preds = %27
  %42 = load ptr, ptr %9, align 8, !dbg !2109
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !2109
  store ptr %43, ptr %9, align 8, !dbg !2109
  %44 = load i32, ptr %42, align 8, !dbg !2109
  %45 = trunc i32 %44 to i8, !dbg !2109
  %46 = load i32, ptr %13, align 4, !dbg !2109
  %47 = sext i32 %46 to i64, !dbg !2109
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %47, !dbg !2109
  store i8 %45, ptr %48, align 8, !dbg !2109
  br label %103, !dbg !2109

49:                                               ; preds = %27
  %50 = load ptr, ptr %9, align 8, !dbg !2109
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !2109
  store ptr %51, ptr %9, align 8, !dbg !2109
  %52 = load i32, ptr %50, align 8, !dbg !2109
  %53 = trunc i32 %52 to i16, !dbg !2109
  %54 = load i32, ptr %13, align 4, !dbg !2109
  %55 = sext i32 %54 to i64, !dbg !2109
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %55, !dbg !2109
  store i16 %53, ptr %56, align 8, !dbg !2109
  br label %103, !dbg !2109

57:                                               ; preds = %27
  %58 = load ptr, ptr %9, align 8, !dbg !2109
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !2109
  store ptr %59, ptr %9, align 8, !dbg !2109
  %60 = load i32, ptr %58, align 8, !dbg !2109
  %61 = trunc i32 %60 to i16, !dbg !2109
  %62 = load i32, ptr %13, align 4, !dbg !2109
  %63 = sext i32 %62 to i64, !dbg !2109
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %63, !dbg !2109
  store i16 %61, ptr %64, align 8, !dbg !2109
  br label %103, !dbg !2109

65:                                               ; preds = %27
  %66 = load ptr, ptr %9, align 8, !dbg !2109
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !2109
  store ptr %67, ptr %9, align 8, !dbg !2109
  %68 = load i32, ptr %66, align 8, !dbg !2109
  %69 = load i32, ptr %13, align 4, !dbg !2109
  %70 = sext i32 %69 to i64, !dbg !2109
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %70, !dbg !2109
  store i32 %68, ptr %71, align 8, !dbg !2109
  br label %103, !dbg !2109

72:                                               ; preds = %27
  %73 = load ptr, ptr %9, align 8, !dbg !2109
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !2109
  store ptr %74, ptr %9, align 8, !dbg !2109
  %75 = load i32, ptr %73, align 8, !dbg !2109
  %76 = sext i32 %75 to i64, !dbg !2109
  %77 = load i32, ptr %13, align 4, !dbg !2109
  %78 = sext i32 %77 to i64, !dbg !2109
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %78, !dbg !2109
  store i64 %76, ptr %79, align 8, !dbg !2109
  br label %103, !dbg !2109

80:                                               ; preds = %27
  %81 = load ptr, ptr %9, align 8, !dbg !2109
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !2109
  store ptr %82, ptr %9, align 8, !dbg !2109
  %83 = load double, ptr %81, align 8, !dbg !2109
  %84 = fptrunc double %83 to float, !dbg !2109
  %85 = load i32, ptr %13, align 4, !dbg !2109
  %86 = sext i32 %85 to i64, !dbg !2109
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %86, !dbg !2109
  store float %84, ptr %87, align 8, !dbg !2109
  br label %103, !dbg !2109

88:                                               ; preds = %27
  %89 = load ptr, ptr %9, align 8, !dbg !2109
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !2109
  store ptr %90, ptr %9, align 8, !dbg !2109
  %91 = load double, ptr %89, align 8, !dbg !2109
  %92 = load i32, ptr %13, align 4, !dbg !2109
  %93 = sext i32 %92 to i64, !dbg !2109
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %93, !dbg !2109
  store double %91, ptr %94, align 8, !dbg !2109
  br label %103, !dbg !2109

95:                                               ; preds = %27
  %96 = load ptr, ptr %9, align 8, !dbg !2109
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !2109
  store ptr %97, ptr %9, align 8, !dbg !2109
  %98 = load ptr, ptr %96, align 8, !dbg !2109
  %99 = load i32, ptr %13, align 4, !dbg !2109
  %100 = sext i32 %99 to i64, !dbg !2109
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %100, !dbg !2109
  store ptr %98, ptr %101, align 8, !dbg !2109
  br label %103, !dbg !2109

102:                                              ; preds = %27
  br label %103, !dbg !2109

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !2106

104:                                              ; preds = %103
  %105 = load i32, ptr %13, align 4, !dbg !2111
  %106 = add nsw i32 %105, 1, !dbg !2111
  store i32 %106, ptr %13, align 4, !dbg !2111
  br label %23, !dbg !2111, !llvm.loop !2112

107:                                              ; preds = %23
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2113, metadata !DIExpression()), !dbg !2095
  %108 = load ptr, ptr %8, align 8, !dbg !2095
  %109 = load ptr, ptr %108, align 8, !dbg !2095
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 90, !dbg !2095
  %111 = load ptr, ptr %110, align 8, !dbg !2095
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !2095
  %113 = load ptr, ptr %5, align 8, !dbg !2095
  %114 = load ptr, ptr %6, align 8, !dbg !2095
  %115 = load ptr, ptr %7, align 8, !dbg !2095
  %116 = load ptr, ptr %8, align 8, !dbg !2095
  %117 = call double %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !2095
  store double %117, ptr %14, align 8, !dbg !2095
  call void @llvm.va_end(ptr %9), !dbg !2095
  %118 = load double, ptr %14, align 8, !dbg !2095
  ret double %118, !dbg !2095
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2114 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2115, metadata !DIExpression()), !dbg !2116
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2117, metadata !DIExpression()), !dbg !2116
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2118, metadata !DIExpression()), !dbg !2116
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2119, metadata !DIExpression()), !dbg !2116
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2120, metadata !DIExpression()), !dbg !2116
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2121, metadata !DIExpression()), !dbg !2116
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2122, metadata !DIExpression()), !dbg !2116
  %15 = load ptr, ptr %10, align 8, !dbg !2116
  %16 = load ptr, ptr %15, align 8, !dbg !2116
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2116
  %18 = load ptr, ptr %17, align 8, !dbg !2116
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !2116
  %20 = load ptr, ptr %7, align 8, !dbg !2116
  %21 = load ptr, ptr %10, align 8, !dbg !2116
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2116
  store i32 %22, ptr %12, align 4, !dbg !2116
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2123, metadata !DIExpression()), !dbg !2116
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2124, metadata !DIExpression()), !dbg !2126
  store i32 0, ptr %14, align 4, !dbg !2126
  br label %23, !dbg !2126

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !2126
  %25 = load i32, ptr %12, align 4, !dbg !2126
  %26 = icmp slt i32 %24, %25, !dbg !2126
  br i1 %26, label %27, label %107, !dbg !2126

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2127
  %29 = sext i32 %28 to i64, !dbg !2127
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !2127
  %31 = load i8, ptr %30, align 1, !dbg !2127
  %32 = sext i8 %31 to i32, !dbg !2127
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
  ], !dbg !2127

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !2130
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !2130
  store ptr %35, ptr %6, align 8, !dbg !2130
  %36 = load i32, ptr %34, align 8, !dbg !2130
  %37 = trunc i32 %36 to i8, !dbg !2130
  %38 = load i32, ptr %14, align 4, !dbg !2130
  %39 = sext i32 %38 to i64, !dbg !2130
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !2130
  store i8 %37, ptr %40, align 8, !dbg !2130
  br label %103, !dbg !2130

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !2130
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !2130
  store ptr %43, ptr %6, align 8, !dbg !2130
  %44 = load i32, ptr %42, align 8, !dbg !2130
  %45 = trunc i32 %44 to i8, !dbg !2130
  %46 = load i32, ptr %14, align 4, !dbg !2130
  %47 = sext i32 %46 to i64, !dbg !2130
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !2130
  store i8 %45, ptr %48, align 8, !dbg !2130
  br label %103, !dbg !2130

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !2130
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !2130
  store ptr %51, ptr %6, align 8, !dbg !2130
  %52 = load i32, ptr %50, align 8, !dbg !2130
  %53 = trunc i32 %52 to i16, !dbg !2130
  %54 = load i32, ptr %14, align 4, !dbg !2130
  %55 = sext i32 %54 to i64, !dbg !2130
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !2130
  store i16 %53, ptr %56, align 8, !dbg !2130
  br label %103, !dbg !2130

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !2130
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !2130
  store ptr %59, ptr %6, align 8, !dbg !2130
  %60 = load i32, ptr %58, align 8, !dbg !2130
  %61 = trunc i32 %60 to i16, !dbg !2130
  %62 = load i32, ptr %14, align 4, !dbg !2130
  %63 = sext i32 %62 to i64, !dbg !2130
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !2130
  store i16 %61, ptr %64, align 8, !dbg !2130
  br label %103, !dbg !2130

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !2130
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !2130
  store ptr %67, ptr %6, align 8, !dbg !2130
  %68 = load i32, ptr %66, align 8, !dbg !2130
  %69 = load i32, ptr %14, align 4, !dbg !2130
  %70 = sext i32 %69 to i64, !dbg !2130
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !2130
  store i32 %68, ptr %71, align 8, !dbg !2130
  br label %103, !dbg !2130

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !2130
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !2130
  store ptr %74, ptr %6, align 8, !dbg !2130
  %75 = load i32, ptr %73, align 8, !dbg !2130
  %76 = sext i32 %75 to i64, !dbg !2130
  %77 = load i32, ptr %14, align 4, !dbg !2130
  %78 = sext i32 %77 to i64, !dbg !2130
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !2130
  store i64 %76, ptr %79, align 8, !dbg !2130
  br label %103, !dbg !2130

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !2130
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !2130
  store ptr %82, ptr %6, align 8, !dbg !2130
  %83 = load double, ptr %81, align 8, !dbg !2130
  %84 = fptrunc double %83 to float, !dbg !2130
  %85 = load i32, ptr %14, align 4, !dbg !2130
  %86 = sext i32 %85 to i64, !dbg !2130
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !2130
  store float %84, ptr %87, align 8, !dbg !2130
  br label %103, !dbg !2130

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !2130
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !2130
  store ptr %90, ptr %6, align 8, !dbg !2130
  %91 = load double, ptr %89, align 8, !dbg !2130
  %92 = load i32, ptr %14, align 4, !dbg !2130
  %93 = sext i32 %92 to i64, !dbg !2130
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !2130
  store double %91, ptr %94, align 8, !dbg !2130
  br label %103, !dbg !2130

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !2130
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !2130
  store ptr %97, ptr %6, align 8, !dbg !2130
  %98 = load ptr, ptr %96, align 8, !dbg !2130
  %99 = load i32, ptr %14, align 4, !dbg !2130
  %100 = sext i32 %99 to i64, !dbg !2130
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !2130
  store ptr %98, ptr %101, align 8, !dbg !2130
  br label %103, !dbg !2130

102:                                              ; preds = %27
  br label %103, !dbg !2130

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !2127

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !2132
  %106 = add nsw i32 %105, 1, !dbg !2132
  store i32 %106, ptr %14, align 4, !dbg !2132
  br label %23, !dbg !2132, !llvm.loop !2133

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !2116
  %109 = load ptr, ptr %108, align 8, !dbg !2116
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 90, !dbg !2116
  %111 = load ptr, ptr %110, align 8, !dbg !2116
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !2116
  %113 = load ptr, ptr %7, align 8, !dbg !2116
  %114 = load ptr, ptr %8, align 8, !dbg !2116
  %115 = load ptr, ptr %9, align 8, !dbg !2116
  %116 = load ptr, ptr %10, align 8, !dbg !2116
  %117 = call double %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !2116
  ret double %117, !dbg !2116
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2134 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca double, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2135, metadata !DIExpression()), !dbg !2136
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2137, metadata !DIExpression()), !dbg !2136
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2138, metadata !DIExpression()), !dbg !2136
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2139, metadata !DIExpression()), !dbg !2136
  call void @llvm.va_start(ptr %7), !dbg !2136
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2140, metadata !DIExpression()), !dbg !2136
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2141, metadata !DIExpression()), !dbg !2136
  %13 = load ptr, ptr %6, align 8, !dbg !2136
  %14 = load ptr, ptr %13, align 8, !dbg !2136
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2136
  %16 = load ptr, ptr %15, align 8, !dbg !2136
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2136
  %18 = load ptr, ptr %4, align 8, !dbg !2136
  %19 = load ptr, ptr %6, align 8, !dbg !2136
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2136
  store i32 %20, ptr %9, align 4, !dbg !2136
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2142, metadata !DIExpression()), !dbg !2136
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2143, metadata !DIExpression()), !dbg !2145
  store i32 0, ptr %11, align 4, !dbg !2145
  br label %21, !dbg !2145

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !2145
  %23 = load i32, ptr %9, align 4, !dbg !2145
  %24 = icmp slt i32 %22, %23, !dbg !2145
  br i1 %24, label %25, label %105, !dbg !2145

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2146
  %27 = sext i32 %26 to i64, !dbg !2146
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !2146
  %29 = load i8, ptr %28, align 1, !dbg !2146
  %30 = sext i8 %29 to i32, !dbg !2146
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
  ], !dbg !2146

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !2149
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2149
  store ptr %33, ptr %7, align 8, !dbg !2149
  %34 = load i32, ptr %32, align 8, !dbg !2149
  %35 = trunc i32 %34 to i8, !dbg !2149
  %36 = load i32, ptr %11, align 4, !dbg !2149
  %37 = sext i32 %36 to i64, !dbg !2149
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !2149
  store i8 %35, ptr %38, align 8, !dbg !2149
  br label %101, !dbg !2149

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !2149
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2149
  store ptr %41, ptr %7, align 8, !dbg !2149
  %42 = load i32, ptr %40, align 8, !dbg !2149
  %43 = trunc i32 %42 to i8, !dbg !2149
  %44 = load i32, ptr %11, align 4, !dbg !2149
  %45 = sext i32 %44 to i64, !dbg !2149
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !2149
  store i8 %43, ptr %46, align 8, !dbg !2149
  br label %101, !dbg !2149

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !2149
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2149
  store ptr %49, ptr %7, align 8, !dbg !2149
  %50 = load i32, ptr %48, align 8, !dbg !2149
  %51 = trunc i32 %50 to i16, !dbg !2149
  %52 = load i32, ptr %11, align 4, !dbg !2149
  %53 = sext i32 %52 to i64, !dbg !2149
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !2149
  store i16 %51, ptr %54, align 8, !dbg !2149
  br label %101, !dbg !2149

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !2149
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2149
  store ptr %57, ptr %7, align 8, !dbg !2149
  %58 = load i32, ptr %56, align 8, !dbg !2149
  %59 = trunc i32 %58 to i16, !dbg !2149
  %60 = load i32, ptr %11, align 4, !dbg !2149
  %61 = sext i32 %60 to i64, !dbg !2149
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !2149
  store i16 %59, ptr %62, align 8, !dbg !2149
  br label %101, !dbg !2149

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !2149
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2149
  store ptr %65, ptr %7, align 8, !dbg !2149
  %66 = load i32, ptr %64, align 8, !dbg !2149
  %67 = load i32, ptr %11, align 4, !dbg !2149
  %68 = sext i32 %67 to i64, !dbg !2149
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !2149
  store i32 %66, ptr %69, align 8, !dbg !2149
  br label %101, !dbg !2149

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !2149
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2149
  store ptr %72, ptr %7, align 8, !dbg !2149
  %73 = load i32, ptr %71, align 8, !dbg !2149
  %74 = sext i32 %73 to i64, !dbg !2149
  %75 = load i32, ptr %11, align 4, !dbg !2149
  %76 = sext i32 %75 to i64, !dbg !2149
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !2149
  store i64 %74, ptr %77, align 8, !dbg !2149
  br label %101, !dbg !2149

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !2149
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2149
  store ptr %80, ptr %7, align 8, !dbg !2149
  %81 = load double, ptr %79, align 8, !dbg !2149
  %82 = fptrunc double %81 to float, !dbg !2149
  %83 = load i32, ptr %11, align 4, !dbg !2149
  %84 = sext i32 %83 to i64, !dbg !2149
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !2149
  store float %82, ptr %85, align 8, !dbg !2149
  br label %101, !dbg !2149

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !2149
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2149
  store ptr %88, ptr %7, align 8, !dbg !2149
  %89 = load double, ptr %87, align 8, !dbg !2149
  %90 = load i32, ptr %11, align 4, !dbg !2149
  %91 = sext i32 %90 to i64, !dbg !2149
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !2149
  store double %89, ptr %92, align 8, !dbg !2149
  br label %101, !dbg !2149

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !2149
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2149
  store ptr %95, ptr %7, align 8, !dbg !2149
  %96 = load ptr, ptr %94, align 8, !dbg !2149
  %97 = load i32, ptr %11, align 4, !dbg !2149
  %98 = sext i32 %97 to i64, !dbg !2149
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !2149
  store ptr %96, ptr %99, align 8, !dbg !2149
  br label %101, !dbg !2149

100:                                              ; preds = %25
  br label %101, !dbg !2149

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2146

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !2151
  %104 = add nsw i32 %103, 1, !dbg !2151
  store i32 %104, ptr %11, align 4, !dbg !2151
  br label %21, !dbg !2151, !llvm.loop !2152

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2153, metadata !DIExpression()), !dbg !2136
  %106 = load ptr, ptr %6, align 8, !dbg !2136
  %107 = load ptr, ptr %106, align 8, !dbg !2136
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 140, !dbg !2136
  %109 = load ptr, ptr %108, align 8, !dbg !2136
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2136
  %111 = load ptr, ptr %4, align 8, !dbg !2136
  %112 = load ptr, ptr %5, align 8, !dbg !2136
  %113 = load ptr, ptr %6, align 8, !dbg !2136
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2136
  store double %114, ptr %12, align 8, !dbg !2136
  call void @llvm.va_end(ptr %7), !dbg !2136
  %115 = load double, ptr %12, align 8, !dbg !2136
  ret double %115, !dbg !2136
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2154 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2155, metadata !DIExpression()), !dbg !2156
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2157, metadata !DIExpression()), !dbg !2156
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2158, metadata !DIExpression()), !dbg !2156
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2159, metadata !DIExpression()), !dbg !2156
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2160, metadata !DIExpression()), !dbg !2156
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2161, metadata !DIExpression()), !dbg !2156
  %13 = load ptr, ptr %8, align 8, !dbg !2156
  %14 = load ptr, ptr %13, align 8, !dbg !2156
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2156
  %16 = load ptr, ptr %15, align 8, !dbg !2156
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2156
  %18 = load ptr, ptr %6, align 8, !dbg !2156
  %19 = load ptr, ptr %8, align 8, !dbg !2156
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2156
  store i32 %20, ptr %10, align 4, !dbg !2156
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2162, metadata !DIExpression()), !dbg !2156
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2163, metadata !DIExpression()), !dbg !2165
  store i32 0, ptr %12, align 4, !dbg !2165
  br label %21, !dbg !2165

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2165
  %23 = load i32, ptr %10, align 4, !dbg !2165
  %24 = icmp slt i32 %22, %23, !dbg !2165
  br i1 %24, label %25, label %105, !dbg !2165

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2166
  %27 = sext i32 %26 to i64, !dbg !2166
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2166
  %29 = load i8, ptr %28, align 1, !dbg !2166
  %30 = sext i8 %29 to i32, !dbg !2166
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
  ], !dbg !2166

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2169
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2169
  store ptr %33, ptr %5, align 8, !dbg !2169
  %34 = load i32, ptr %32, align 8, !dbg !2169
  %35 = trunc i32 %34 to i8, !dbg !2169
  %36 = load i32, ptr %12, align 4, !dbg !2169
  %37 = sext i32 %36 to i64, !dbg !2169
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2169
  store i8 %35, ptr %38, align 8, !dbg !2169
  br label %101, !dbg !2169

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2169
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2169
  store ptr %41, ptr %5, align 8, !dbg !2169
  %42 = load i32, ptr %40, align 8, !dbg !2169
  %43 = trunc i32 %42 to i8, !dbg !2169
  %44 = load i32, ptr %12, align 4, !dbg !2169
  %45 = sext i32 %44 to i64, !dbg !2169
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2169
  store i8 %43, ptr %46, align 8, !dbg !2169
  br label %101, !dbg !2169

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2169
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2169
  store ptr %49, ptr %5, align 8, !dbg !2169
  %50 = load i32, ptr %48, align 8, !dbg !2169
  %51 = trunc i32 %50 to i16, !dbg !2169
  %52 = load i32, ptr %12, align 4, !dbg !2169
  %53 = sext i32 %52 to i64, !dbg !2169
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2169
  store i16 %51, ptr %54, align 8, !dbg !2169
  br label %101, !dbg !2169

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2169
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2169
  store ptr %57, ptr %5, align 8, !dbg !2169
  %58 = load i32, ptr %56, align 8, !dbg !2169
  %59 = trunc i32 %58 to i16, !dbg !2169
  %60 = load i32, ptr %12, align 4, !dbg !2169
  %61 = sext i32 %60 to i64, !dbg !2169
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2169
  store i16 %59, ptr %62, align 8, !dbg !2169
  br label %101, !dbg !2169

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2169
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2169
  store ptr %65, ptr %5, align 8, !dbg !2169
  %66 = load i32, ptr %64, align 8, !dbg !2169
  %67 = load i32, ptr %12, align 4, !dbg !2169
  %68 = sext i32 %67 to i64, !dbg !2169
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2169
  store i32 %66, ptr %69, align 8, !dbg !2169
  br label %101, !dbg !2169

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2169
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2169
  store ptr %72, ptr %5, align 8, !dbg !2169
  %73 = load i32, ptr %71, align 8, !dbg !2169
  %74 = sext i32 %73 to i64, !dbg !2169
  %75 = load i32, ptr %12, align 4, !dbg !2169
  %76 = sext i32 %75 to i64, !dbg !2169
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2169
  store i64 %74, ptr %77, align 8, !dbg !2169
  br label %101, !dbg !2169

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2169
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2169
  store ptr %80, ptr %5, align 8, !dbg !2169
  %81 = load double, ptr %79, align 8, !dbg !2169
  %82 = fptrunc double %81 to float, !dbg !2169
  %83 = load i32, ptr %12, align 4, !dbg !2169
  %84 = sext i32 %83 to i64, !dbg !2169
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2169
  store float %82, ptr %85, align 8, !dbg !2169
  br label %101, !dbg !2169

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2169
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2169
  store ptr %88, ptr %5, align 8, !dbg !2169
  %89 = load double, ptr %87, align 8, !dbg !2169
  %90 = load i32, ptr %12, align 4, !dbg !2169
  %91 = sext i32 %90 to i64, !dbg !2169
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2169
  store double %89, ptr %92, align 8, !dbg !2169
  br label %101, !dbg !2169

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2169
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2169
  store ptr %95, ptr %5, align 8, !dbg !2169
  %96 = load ptr, ptr %94, align 8, !dbg !2169
  %97 = load i32, ptr %12, align 4, !dbg !2169
  %98 = sext i32 %97 to i64, !dbg !2169
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2169
  store ptr %96, ptr %99, align 8, !dbg !2169
  br label %101, !dbg !2169

100:                                              ; preds = %25
  br label %101, !dbg !2169

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2166

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2171
  %104 = add nsw i32 %103, 1, !dbg !2171
  store i32 %104, ptr %12, align 4, !dbg !2171
  br label %21, !dbg !2171, !llvm.loop !2172

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2156
  %107 = load ptr, ptr %106, align 8, !dbg !2156
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 140, !dbg !2156
  %109 = load ptr, ptr %108, align 8, !dbg !2156
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2156
  %111 = load ptr, ptr %6, align 8, !dbg !2156
  %112 = load ptr, ptr %7, align 8, !dbg !2156
  %113 = load ptr, ptr %8, align 8, !dbg !2156
  %114 = call double %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2156
  ret double %114, !dbg !2156
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2173 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  %12 = alloca ptr, align 8
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2174, metadata !DIExpression()), !dbg !2175
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2176, metadata !DIExpression()), !dbg !2175
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2177, metadata !DIExpression()), !dbg !2175
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2178, metadata !DIExpression()), !dbg !2179
  call void @llvm.va_start(ptr %7), !dbg !2180
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2181, metadata !DIExpression()), !dbg !2182
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2183, metadata !DIExpression()), !dbg !2182
  %13 = load ptr, ptr %6, align 8, !dbg !2182
  %14 = load ptr, ptr %13, align 8, !dbg !2182
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2182
  %16 = load ptr, ptr %15, align 8, !dbg !2182
  %17 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2182
  %18 = load ptr, ptr %4, align 8, !dbg !2182
  %19 = load ptr, ptr %6, align 8, !dbg !2182
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2182
  store i32 %20, ptr %9, align 4, !dbg !2182
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2184, metadata !DIExpression()), !dbg !2182
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2185, metadata !DIExpression()), !dbg !2187
  store i32 0, ptr %11, align 4, !dbg !2187
  br label %21, !dbg !2187

21:                                               ; preds = %102, %3
  %22 = load i32, ptr %11, align 4, !dbg !2187
  %23 = load i32, ptr %9, align 4, !dbg !2187
  %24 = icmp slt i32 %22, %23, !dbg !2187
  br i1 %24, label %25, label %105, !dbg !2187

25:                                               ; preds = %21
  %26 = load i32, ptr %11, align 4, !dbg !2188
  %27 = sext i32 %26 to i64, !dbg !2188
  %28 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %27, !dbg !2188
  %29 = load i8, ptr %28, align 1, !dbg !2188
  %30 = sext i8 %29 to i32, !dbg !2188
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
  ], !dbg !2188

31:                                               ; preds = %25
  %32 = load ptr, ptr %7, align 8, !dbg !2191
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2191
  store ptr %33, ptr %7, align 8, !dbg !2191
  %34 = load i32, ptr %32, align 8, !dbg !2191
  %35 = trunc i32 %34 to i8, !dbg !2191
  %36 = load i32, ptr %11, align 4, !dbg !2191
  %37 = sext i32 %36 to i64, !dbg !2191
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %37, !dbg !2191
  store i8 %35, ptr %38, align 8, !dbg !2191
  br label %101, !dbg !2191

39:                                               ; preds = %25
  %40 = load ptr, ptr %7, align 8, !dbg !2191
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2191
  store ptr %41, ptr %7, align 8, !dbg !2191
  %42 = load i32, ptr %40, align 8, !dbg !2191
  %43 = trunc i32 %42 to i8, !dbg !2191
  %44 = load i32, ptr %11, align 4, !dbg !2191
  %45 = sext i32 %44 to i64, !dbg !2191
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %45, !dbg !2191
  store i8 %43, ptr %46, align 8, !dbg !2191
  br label %101, !dbg !2191

47:                                               ; preds = %25
  %48 = load ptr, ptr %7, align 8, !dbg !2191
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2191
  store ptr %49, ptr %7, align 8, !dbg !2191
  %50 = load i32, ptr %48, align 8, !dbg !2191
  %51 = trunc i32 %50 to i16, !dbg !2191
  %52 = load i32, ptr %11, align 4, !dbg !2191
  %53 = sext i32 %52 to i64, !dbg !2191
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %53, !dbg !2191
  store i16 %51, ptr %54, align 8, !dbg !2191
  br label %101, !dbg !2191

55:                                               ; preds = %25
  %56 = load ptr, ptr %7, align 8, !dbg !2191
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2191
  store ptr %57, ptr %7, align 8, !dbg !2191
  %58 = load i32, ptr %56, align 8, !dbg !2191
  %59 = trunc i32 %58 to i16, !dbg !2191
  %60 = load i32, ptr %11, align 4, !dbg !2191
  %61 = sext i32 %60 to i64, !dbg !2191
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %61, !dbg !2191
  store i16 %59, ptr %62, align 8, !dbg !2191
  br label %101, !dbg !2191

63:                                               ; preds = %25
  %64 = load ptr, ptr %7, align 8, !dbg !2191
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2191
  store ptr %65, ptr %7, align 8, !dbg !2191
  %66 = load i32, ptr %64, align 8, !dbg !2191
  %67 = load i32, ptr %11, align 4, !dbg !2191
  %68 = sext i32 %67 to i64, !dbg !2191
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %68, !dbg !2191
  store i32 %66, ptr %69, align 8, !dbg !2191
  br label %101, !dbg !2191

70:                                               ; preds = %25
  %71 = load ptr, ptr %7, align 8, !dbg !2191
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2191
  store ptr %72, ptr %7, align 8, !dbg !2191
  %73 = load i32, ptr %71, align 8, !dbg !2191
  %74 = sext i32 %73 to i64, !dbg !2191
  %75 = load i32, ptr %11, align 4, !dbg !2191
  %76 = sext i32 %75 to i64, !dbg !2191
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %76, !dbg !2191
  store i64 %74, ptr %77, align 8, !dbg !2191
  br label %101, !dbg !2191

78:                                               ; preds = %25
  %79 = load ptr, ptr %7, align 8, !dbg !2191
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2191
  store ptr %80, ptr %7, align 8, !dbg !2191
  %81 = load double, ptr %79, align 8, !dbg !2191
  %82 = fptrunc double %81 to float, !dbg !2191
  %83 = load i32, ptr %11, align 4, !dbg !2191
  %84 = sext i32 %83 to i64, !dbg !2191
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %84, !dbg !2191
  store float %82, ptr %85, align 8, !dbg !2191
  br label %101, !dbg !2191

86:                                               ; preds = %25
  %87 = load ptr, ptr %7, align 8, !dbg !2191
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2191
  store ptr %88, ptr %7, align 8, !dbg !2191
  %89 = load double, ptr %87, align 8, !dbg !2191
  %90 = load i32, ptr %11, align 4, !dbg !2191
  %91 = sext i32 %90 to i64, !dbg !2191
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %91, !dbg !2191
  store double %89, ptr %92, align 8, !dbg !2191
  br label %101, !dbg !2191

93:                                               ; preds = %25
  %94 = load ptr, ptr %7, align 8, !dbg !2191
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2191
  store ptr %95, ptr %7, align 8, !dbg !2191
  %96 = load ptr, ptr %94, align 8, !dbg !2191
  %97 = load i32, ptr %11, align 4, !dbg !2191
  %98 = sext i32 %97 to i64, !dbg !2191
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %98, !dbg !2191
  store ptr %96, ptr %99, align 8, !dbg !2191
  br label %101, !dbg !2191

100:                                              ; preds = %25
  br label %101, !dbg !2191

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2188

102:                                              ; preds = %101
  %103 = load i32, ptr %11, align 4, !dbg !2193
  %104 = add nsw i32 %103, 1, !dbg !2193
  store i32 %104, ptr %11, align 4, !dbg !2193
  br label %21, !dbg !2193, !llvm.loop !2194

105:                                              ; preds = %21
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2195, metadata !DIExpression()), !dbg !2196
  %106 = load ptr, ptr %6, align 8, !dbg !2196
  %107 = load ptr, ptr %106, align 8, !dbg !2196
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 30, !dbg !2196
  %109 = load ptr, ptr %108, align 8, !dbg !2196
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2196
  %111 = load ptr, ptr %4, align 8, !dbg !2196
  %112 = load ptr, ptr %5, align 8, !dbg !2196
  %113 = load ptr, ptr %6, align 8, !dbg !2196
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2196
  store ptr %114, ptr %12, align 8, !dbg !2196
  call void @llvm.va_end(ptr %7), !dbg !2197
  %115 = load ptr, ptr %12, align 8, !dbg !2198
  ret ptr %115, !dbg !2198
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2199 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2200, metadata !DIExpression()), !dbg !2201
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2202, metadata !DIExpression()), !dbg !2201
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2203, metadata !DIExpression()), !dbg !2201
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2204, metadata !DIExpression()), !dbg !2201
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2205, metadata !DIExpression()), !dbg !2206
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2207, metadata !DIExpression()), !dbg !2206
  %13 = load ptr, ptr %8, align 8, !dbg !2206
  %14 = load ptr, ptr %13, align 8, !dbg !2206
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2206
  %16 = load ptr, ptr %15, align 8, !dbg !2206
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2206
  %18 = load ptr, ptr %6, align 8, !dbg !2206
  %19 = load ptr, ptr %8, align 8, !dbg !2206
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2206
  store i32 %20, ptr %10, align 4, !dbg !2206
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2208, metadata !DIExpression()), !dbg !2206
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2209, metadata !DIExpression()), !dbg !2211
  store i32 0, ptr %12, align 4, !dbg !2211
  br label %21, !dbg !2211

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2211
  %23 = load i32, ptr %10, align 4, !dbg !2211
  %24 = icmp slt i32 %22, %23, !dbg !2211
  br i1 %24, label %25, label %105, !dbg !2211

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2212
  %27 = sext i32 %26 to i64, !dbg !2212
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2212
  %29 = load i8, ptr %28, align 1, !dbg !2212
  %30 = sext i8 %29 to i32, !dbg !2212
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
  ], !dbg !2212

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2215
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2215
  store ptr %33, ptr %5, align 8, !dbg !2215
  %34 = load i32, ptr %32, align 8, !dbg !2215
  %35 = trunc i32 %34 to i8, !dbg !2215
  %36 = load i32, ptr %12, align 4, !dbg !2215
  %37 = sext i32 %36 to i64, !dbg !2215
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2215
  store i8 %35, ptr %38, align 8, !dbg !2215
  br label %101, !dbg !2215

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2215
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2215
  store ptr %41, ptr %5, align 8, !dbg !2215
  %42 = load i32, ptr %40, align 8, !dbg !2215
  %43 = trunc i32 %42 to i8, !dbg !2215
  %44 = load i32, ptr %12, align 4, !dbg !2215
  %45 = sext i32 %44 to i64, !dbg !2215
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2215
  store i8 %43, ptr %46, align 8, !dbg !2215
  br label %101, !dbg !2215

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2215
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2215
  store ptr %49, ptr %5, align 8, !dbg !2215
  %50 = load i32, ptr %48, align 8, !dbg !2215
  %51 = trunc i32 %50 to i16, !dbg !2215
  %52 = load i32, ptr %12, align 4, !dbg !2215
  %53 = sext i32 %52 to i64, !dbg !2215
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2215
  store i16 %51, ptr %54, align 8, !dbg !2215
  br label %101, !dbg !2215

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2215
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2215
  store ptr %57, ptr %5, align 8, !dbg !2215
  %58 = load i32, ptr %56, align 8, !dbg !2215
  %59 = trunc i32 %58 to i16, !dbg !2215
  %60 = load i32, ptr %12, align 4, !dbg !2215
  %61 = sext i32 %60 to i64, !dbg !2215
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2215
  store i16 %59, ptr %62, align 8, !dbg !2215
  br label %101, !dbg !2215

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2215
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2215
  store ptr %65, ptr %5, align 8, !dbg !2215
  %66 = load i32, ptr %64, align 8, !dbg !2215
  %67 = load i32, ptr %12, align 4, !dbg !2215
  %68 = sext i32 %67 to i64, !dbg !2215
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2215
  store i32 %66, ptr %69, align 8, !dbg !2215
  br label %101, !dbg !2215

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2215
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2215
  store ptr %72, ptr %5, align 8, !dbg !2215
  %73 = load i32, ptr %71, align 8, !dbg !2215
  %74 = sext i32 %73 to i64, !dbg !2215
  %75 = load i32, ptr %12, align 4, !dbg !2215
  %76 = sext i32 %75 to i64, !dbg !2215
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2215
  store i64 %74, ptr %77, align 8, !dbg !2215
  br label %101, !dbg !2215

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2215
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2215
  store ptr %80, ptr %5, align 8, !dbg !2215
  %81 = load double, ptr %79, align 8, !dbg !2215
  %82 = fptrunc double %81 to float, !dbg !2215
  %83 = load i32, ptr %12, align 4, !dbg !2215
  %84 = sext i32 %83 to i64, !dbg !2215
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2215
  store float %82, ptr %85, align 8, !dbg !2215
  br label %101, !dbg !2215

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2215
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2215
  store ptr %88, ptr %5, align 8, !dbg !2215
  %89 = load double, ptr %87, align 8, !dbg !2215
  %90 = load i32, ptr %12, align 4, !dbg !2215
  %91 = sext i32 %90 to i64, !dbg !2215
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2215
  store double %89, ptr %92, align 8, !dbg !2215
  br label %101, !dbg !2215

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2215
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2215
  store ptr %95, ptr %5, align 8, !dbg !2215
  %96 = load ptr, ptr %94, align 8, !dbg !2215
  %97 = load i32, ptr %12, align 4, !dbg !2215
  %98 = sext i32 %97 to i64, !dbg !2215
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2215
  store ptr %96, ptr %99, align 8, !dbg !2215
  br label %101, !dbg !2215

100:                                              ; preds = %25
  br label %101, !dbg !2215

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2212

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2217
  %104 = add nsw i32 %103, 1, !dbg !2217
  store i32 %104, ptr %12, align 4, !dbg !2217
  br label %21, !dbg !2217, !llvm.loop !2218

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2219
  %107 = load ptr, ptr %106, align 8, !dbg !2219
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 30, !dbg !2219
  %109 = load ptr, ptr %108, align 8, !dbg !2219
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2219
  %111 = load ptr, ptr %6, align 8, !dbg !2219
  %112 = load ptr, ptr %7, align 8, !dbg !2219
  %113 = load ptr, ptr %8, align 8, !dbg !2219
  %114 = call ptr %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2219
  ret ptr %114, !dbg !2219
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2220 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2221, metadata !DIExpression()), !dbg !2222
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2223, metadata !DIExpression()), !dbg !2222
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2224, metadata !DIExpression()), !dbg !2222
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2225, metadata !DIExpression()), !dbg !2226
  call void @llvm.va_start(ptr %7), !dbg !2227
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2228, metadata !DIExpression()), !dbg !2229
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2230, metadata !DIExpression()), !dbg !2229
  %12 = load ptr, ptr %6, align 8, !dbg !2229
  %13 = load ptr, ptr %12, align 8, !dbg !2229
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2229
  %15 = load ptr, ptr %14, align 8, !dbg !2229
  %16 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2229
  %17 = load ptr, ptr %4, align 8, !dbg !2229
  %18 = load ptr, ptr %6, align 8, !dbg !2229
  %19 = call i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2229
  store i32 %19, ptr %9, align 4, !dbg !2229
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2231, metadata !DIExpression()), !dbg !2229
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2232, metadata !DIExpression()), !dbg !2234
  store i32 0, ptr %11, align 4, !dbg !2234
  br label %20, !dbg !2234

20:                                               ; preds = %101, %3
  %21 = load i32, ptr %11, align 4, !dbg !2234
  %22 = load i32, ptr %9, align 4, !dbg !2234
  %23 = icmp slt i32 %21, %22, !dbg !2234
  br i1 %23, label %24, label %104, !dbg !2234

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2235
  %26 = sext i32 %25 to i64, !dbg !2235
  %27 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %26, !dbg !2235
  %28 = load i8, ptr %27, align 1, !dbg !2235
  %29 = sext i8 %28 to i32, !dbg !2235
  switch i32 %29, label %99 [
    i32 90, label %30
    i32 66, label %38
    i32 67, label %46
    i32 83, label %54
    i32 73, label %62
    i32 74, label %69
    i32 70, label %77
    i32 68, label %85
    i32 76, label %92
  ], !dbg !2235

30:                                               ; preds = %24
  %31 = load ptr, ptr %7, align 8, !dbg !2238
  %32 = getelementptr inbounds i8, ptr %31, i64 8, !dbg !2238
  store ptr %32, ptr %7, align 8, !dbg !2238
  %33 = load i32, ptr %31, align 8, !dbg !2238
  %34 = trunc i32 %33 to i8, !dbg !2238
  %35 = load i32, ptr %11, align 4, !dbg !2238
  %36 = sext i32 %35 to i64, !dbg !2238
  %37 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %36, !dbg !2238
  store i8 %34, ptr %37, align 8, !dbg !2238
  br label %100, !dbg !2238

38:                                               ; preds = %24
  %39 = load ptr, ptr %7, align 8, !dbg !2238
  %40 = getelementptr inbounds i8, ptr %39, i64 8, !dbg !2238
  store ptr %40, ptr %7, align 8, !dbg !2238
  %41 = load i32, ptr %39, align 8, !dbg !2238
  %42 = trunc i32 %41 to i8, !dbg !2238
  %43 = load i32, ptr %11, align 4, !dbg !2238
  %44 = sext i32 %43 to i64, !dbg !2238
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %44, !dbg !2238
  store i8 %42, ptr %45, align 8, !dbg !2238
  br label %100, !dbg !2238

46:                                               ; preds = %24
  %47 = load ptr, ptr %7, align 8, !dbg !2238
  %48 = getelementptr inbounds i8, ptr %47, i64 8, !dbg !2238
  store ptr %48, ptr %7, align 8, !dbg !2238
  %49 = load i32, ptr %47, align 8, !dbg !2238
  %50 = trunc i32 %49 to i16, !dbg !2238
  %51 = load i32, ptr %11, align 4, !dbg !2238
  %52 = sext i32 %51 to i64, !dbg !2238
  %53 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %52, !dbg !2238
  store i16 %50, ptr %53, align 8, !dbg !2238
  br label %100, !dbg !2238

54:                                               ; preds = %24
  %55 = load ptr, ptr %7, align 8, !dbg !2238
  %56 = getelementptr inbounds i8, ptr %55, i64 8, !dbg !2238
  store ptr %56, ptr %7, align 8, !dbg !2238
  %57 = load i32, ptr %55, align 8, !dbg !2238
  %58 = trunc i32 %57 to i16, !dbg !2238
  %59 = load i32, ptr %11, align 4, !dbg !2238
  %60 = sext i32 %59 to i64, !dbg !2238
  %61 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %60, !dbg !2238
  store i16 %58, ptr %61, align 8, !dbg !2238
  br label %100, !dbg !2238

62:                                               ; preds = %24
  %63 = load ptr, ptr %7, align 8, !dbg !2238
  %64 = getelementptr inbounds i8, ptr %63, i64 8, !dbg !2238
  store ptr %64, ptr %7, align 8, !dbg !2238
  %65 = load i32, ptr %63, align 8, !dbg !2238
  %66 = load i32, ptr %11, align 4, !dbg !2238
  %67 = sext i32 %66 to i64, !dbg !2238
  %68 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %67, !dbg !2238
  store i32 %65, ptr %68, align 8, !dbg !2238
  br label %100, !dbg !2238

69:                                               ; preds = %24
  %70 = load ptr, ptr %7, align 8, !dbg !2238
  %71 = getelementptr inbounds i8, ptr %70, i64 8, !dbg !2238
  store ptr %71, ptr %7, align 8, !dbg !2238
  %72 = load i32, ptr %70, align 8, !dbg !2238
  %73 = sext i32 %72 to i64, !dbg !2238
  %74 = load i32, ptr %11, align 4, !dbg !2238
  %75 = sext i32 %74 to i64, !dbg !2238
  %76 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %75, !dbg !2238
  store i64 %73, ptr %76, align 8, !dbg !2238
  br label %100, !dbg !2238

77:                                               ; preds = %24
  %78 = load ptr, ptr %7, align 8, !dbg !2238
  %79 = getelementptr inbounds i8, ptr %78, i64 8, !dbg !2238
  store ptr %79, ptr %7, align 8, !dbg !2238
  %80 = load double, ptr %78, align 8, !dbg !2238
  %81 = fptrunc double %80 to float, !dbg !2238
  %82 = load i32, ptr %11, align 4, !dbg !2238
  %83 = sext i32 %82 to i64, !dbg !2238
  %84 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %83, !dbg !2238
  store float %81, ptr %84, align 8, !dbg !2238
  br label %100, !dbg !2238

85:                                               ; preds = %24
  %86 = load ptr, ptr %7, align 8, !dbg !2238
  %87 = getelementptr inbounds i8, ptr %86, i64 8, !dbg !2238
  store ptr %87, ptr %7, align 8, !dbg !2238
  %88 = load double, ptr %86, align 8, !dbg !2238
  %89 = load i32, ptr %11, align 4, !dbg !2238
  %90 = sext i32 %89 to i64, !dbg !2238
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %90, !dbg !2238
  store double %88, ptr %91, align 8, !dbg !2238
  br label %100, !dbg !2238

92:                                               ; preds = %24
  %93 = load ptr, ptr %7, align 8, !dbg !2238
  %94 = getelementptr inbounds i8, ptr %93, i64 8, !dbg !2238
  store ptr %94, ptr %7, align 8, !dbg !2238
  %95 = load ptr, ptr %93, align 8, !dbg !2238
  %96 = load i32, ptr %11, align 4, !dbg !2238
  %97 = sext i32 %96 to i64, !dbg !2238
  %98 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %97, !dbg !2238
  store ptr %95, ptr %98, align 8, !dbg !2238
  br label %100, !dbg !2238

99:                                               ; preds = %24
  br label %100, !dbg !2238

100:                                              ; preds = %99, %92, %85, %77, %69, %62, %54, %46, %38, %30
  br label %101, !dbg !2235

101:                                              ; preds = %100
  %102 = load i32, ptr %11, align 4, !dbg !2240
  %103 = add nsw i32 %102, 1, !dbg !2240
  store i32 %103, ptr %11, align 4, !dbg !2240
  br label %20, !dbg !2240, !llvm.loop !2241

104:                                              ; preds = %20
  %105 = load ptr, ptr %6, align 8, !dbg !2242
  %106 = load ptr, ptr %105, align 8, !dbg !2242
  %107 = getelementptr inbounds %struct.JNINativeInterface_, ptr %106, i32 0, i32 63, !dbg !2242
  %108 = load ptr, ptr %107, align 8, !dbg !2242
  %109 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2242
  %110 = load ptr, ptr %4, align 8, !dbg !2242
  %111 = load ptr, ptr %5, align 8, !dbg !2242
  %112 = load ptr, ptr %6, align 8, !dbg !2242
  call void %108(ptr noundef %112, ptr noundef %111, ptr noundef %110, ptr noundef %109), !dbg !2242
  call void @llvm.va_end(ptr %7), !dbg !2243
  ret void, !dbg !2244
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2245 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2246, metadata !DIExpression()), !dbg !2247
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2248, metadata !DIExpression()), !dbg !2247
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2249, metadata !DIExpression()), !dbg !2247
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2250, metadata !DIExpression()), !dbg !2247
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2251, metadata !DIExpression()), !dbg !2252
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2253, metadata !DIExpression()), !dbg !2252
  %13 = load ptr, ptr %8, align 8, !dbg !2252
  %14 = load ptr, ptr %13, align 8, !dbg !2252
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2252
  %16 = load ptr, ptr %15, align 8, !dbg !2252
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2252
  %18 = load ptr, ptr %6, align 8, !dbg !2252
  %19 = load ptr, ptr %8, align 8, !dbg !2252
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2252
  store i32 %20, ptr %10, align 4, !dbg !2252
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2254, metadata !DIExpression()), !dbg !2252
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2255, metadata !DIExpression()), !dbg !2257
  store i32 0, ptr %12, align 4, !dbg !2257
  br label %21, !dbg !2257

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2257
  %23 = load i32, ptr %10, align 4, !dbg !2257
  %24 = icmp slt i32 %22, %23, !dbg !2257
  br i1 %24, label %25, label %105, !dbg !2257

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2258
  %27 = sext i32 %26 to i64, !dbg !2258
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2258
  %29 = load i8, ptr %28, align 1, !dbg !2258
  %30 = sext i8 %29 to i32, !dbg !2258
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
  ], !dbg !2258

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2261
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2261
  store ptr %33, ptr %5, align 8, !dbg !2261
  %34 = load i32, ptr %32, align 8, !dbg !2261
  %35 = trunc i32 %34 to i8, !dbg !2261
  %36 = load i32, ptr %12, align 4, !dbg !2261
  %37 = sext i32 %36 to i64, !dbg !2261
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2261
  store i8 %35, ptr %38, align 8, !dbg !2261
  br label %101, !dbg !2261

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2261
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2261
  store ptr %41, ptr %5, align 8, !dbg !2261
  %42 = load i32, ptr %40, align 8, !dbg !2261
  %43 = trunc i32 %42 to i8, !dbg !2261
  %44 = load i32, ptr %12, align 4, !dbg !2261
  %45 = sext i32 %44 to i64, !dbg !2261
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2261
  store i8 %43, ptr %46, align 8, !dbg !2261
  br label %101, !dbg !2261

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2261
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2261
  store ptr %49, ptr %5, align 8, !dbg !2261
  %50 = load i32, ptr %48, align 8, !dbg !2261
  %51 = trunc i32 %50 to i16, !dbg !2261
  %52 = load i32, ptr %12, align 4, !dbg !2261
  %53 = sext i32 %52 to i64, !dbg !2261
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2261
  store i16 %51, ptr %54, align 8, !dbg !2261
  br label %101, !dbg !2261

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2261
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2261
  store ptr %57, ptr %5, align 8, !dbg !2261
  %58 = load i32, ptr %56, align 8, !dbg !2261
  %59 = trunc i32 %58 to i16, !dbg !2261
  %60 = load i32, ptr %12, align 4, !dbg !2261
  %61 = sext i32 %60 to i64, !dbg !2261
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2261
  store i16 %59, ptr %62, align 8, !dbg !2261
  br label %101, !dbg !2261

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2261
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2261
  store ptr %65, ptr %5, align 8, !dbg !2261
  %66 = load i32, ptr %64, align 8, !dbg !2261
  %67 = load i32, ptr %12, align 4, !dbg !2261
  %68 = sext i32 %67 to i64, !dbg !2261
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2261
  store i32 %66, ptr %69, align 8, !dbg !2261
  br label %101, !dbg !2261

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2261
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2261
  store ptr %72, ptr %5, align 8, !dbg !2261
  %73 = load i32, ptr %71, align 8, !dbg !2261
  %74 = sext i32 %73 to i64, !dbg !2261
  %75 = load i32, ptr %12, align 4, !dbg !2261
  %76 = sext i32 %75 to i64, !dbg !2261
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2261
  store i64 %74, ptr %77, align 8, !dbg !2261
  br label %101, !dbg !2261

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2261
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2261
  store ptr %80, ptr %5, align 8, !dbg !2261
  %81 = load double, ptr %79, align 8, !dbg !2261
  %82 = fptrunc double %81 to float, !dbg !2261
  %83 = load i32, ptr %12, align 4, !dbg !2261
  %84 = sext i32 %83 to i64, !dbg !2261
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2261
  store float %82, ptr %85, align 8, !dbg !2261
  br label %101, !dbg !2261

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2261
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2261
  store ptr %88, ptr %5, align 8, !dbg !2261
  %89 = load double, ptr %87, align 8, !dbg !2261
  %90 = load i32, ptr %12, align 4, !dbg !2261
  %91 = sext i32 %90 to i64, !dbg !2261
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2261
  store double %89, ptr %92, align 8, !dbg !2261
  br label %101, !dbg !2261

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2261
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2261
  store ptr %95, ptr %5, align 8, !dbg !2261
  %96 = load ptr, ptr %94, align 8, !dbg !2261
  %97 = load i32, ptr %12, align 4, !dbg !2261
  %98 = sext i32 %97 to i64, !dbg !2261
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2261
  store ptr %96, ptr %99, align 8, !dbg !2261
  br label %101, !dbg !2261

100:                                              ; preds = %25
  br label %101, !dbg !2261

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2258

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2263
  %104 = add nsw i32 %103, 1, !dbg !2263
  store i32 %104, ptr %12, align 4, !dbg !2263
  br label %21, !dbg !2263, !llvm.loop !2264

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2265
  %107 = load ptr, ptr %106, align 8, !dbg !2265
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 63, !dbg !2265
  %109 = load ptr, ptr %108, align 8, !dbg !2265
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2265
  %111 = load ptr, ptr %6, align 8, !dbg !2265
  %112 = load ptr, ptr %7, align 8, !dbg !2265
  %113 = load ptr, ptr %8, align 8, !dbg !2265
  call void %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2265
  ret void, !dbg !2266
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) #0 !dbg !2267 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca [256 x i8], align 1
  %11 = alloca i32, align 4
  %12 = alloca [256 x %union.jvalue], align 8
  %13 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2268, metadata !DIExpression()), !dbg !2269
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2270, metadata !DIExpression()), !dbg !2269
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2271, metadata !DIExpression()), !dbg !2269
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2272, metadata !DIExpression()), !dbg !2269
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2273, metadata !DIExpression()), !dbg !2274
  call void @llvm.va_start(ptr %9), !dbg !2275
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2276, metadata !DIExpression()), !dbg !2277
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2278, metadata !DIExpression()), !dbg !2277
  %14 = load ptr, ptr %8, align 8, !dbg !2277
  %15 = load ptr, ptr %14, align 8, !dbg !2277
  %16 = getelementptr inbounds %struct.JNINativeInterface_, ptr %15, i32 0, i32 0, !dbg !2277
  %17 = load ptr, ptr %16, align 8, !dbg !2277
  %18 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 0, !dbg !2277
  %19 = load ptr, ptr %5, align 8, !dbg !2277
  %20 = load ptr, ptr %8, align 8, !dbg !2277
  %21 = call i32 %17(ptr noundef %20, ptr noundef %19, ptr noundef %18), !dbg !2277
  store i32 %21, ptr %11, align 4, !dbg !2277
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2279, metadata !DIExpression()), !dbg !2277
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2280, metadata !DIExpression()), !dbg !2282
  store i32 0, ptr %13, align 4, !dbg !2282
  br label %22, !dbg !2282

22:                                               ; preds = %103, %4
  %23 = load i32, ptr %13, align 4, !dbg !2282
  %24 = load i32, ptr %11, align 4, !dbg !2282
  %25 = icmp slt i32 %23, %24, !dbg !2282
  br i1 %25, label %26, label %106, !dbg !2282

26:                                               ; preds = %22
  %27 = load i32, ptr %13, align 4, !dbg !2283
  %28 = sext i32 %27 to i64, !dbg !2283
  %29 = getelementptr inbounds [256 x i8], ptr %10, i64 0, i64 %28, !dbg !2283
  %30 = load i8, ptr %29, align 1, !dbg !2283
  %31 = sext i8 %30 to i32, !dbg !2283
  switch i32 %31, label %101 [
    i32 90, label %32
    i32 66, label %40
    i32 67, label %48
    i32 83, label %56
    i32 73, label %64
    i32 74, label %71
    i32 70, label %79
    i32 68, label %87
    i32 76, label %94
  ], !dbg !2283

32:                                               ; preds = %26
  %33 = load ptr, ptr %9, align 8, !dbg !2286
  %34 = getelementptr inbounds i8, ptr %33, i64 8, !dbg !2286
  store ptr %34, ptr %9, align 8, !dbg !2286
  %35 = load i32, ptr %33, align 8, !dbg !2286
  %36 = trunc i32 %35 to i8, !dbg !2286
  %37 = load i32, ptr %13, align 4, !dbg !2286
  %38 = sext i32 %37 to i64, !dbg !2286
  %39 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %38, !dbg !2286
  store i8 %36, ptr %39, align 8, !dbg !2286
  br label %102, !dbg !2286

40:                                               ; preds = %26
  %41 = load ptr, ptr %9, align 8, !dbg !2286
  %42 = getelementptr inbounds i8, ptr %41, i64 8, !dbg !2286
  store ptr %42, ptr %9, align 8, !dbg !2286
  %43 = load i32, ptr %41, align 8, !dbg !2286
  %44 = trunc i32 %43 to i8, !dbg !2286
  %45 = load i32, ptr %13, align 4, !dbg !2286
  %46 = sext i32 %45 to i64, !dbg !2286
  %47 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %46, !dbg !2286
  store i8 %44, ptr %47, align 8, !dbg !2286
  br label %102, !dbg !2286

48:                                               ; preds = %26
  %49 = load ptr, ptr %9, align 8, !dbg !2286
  %50 = getelementptr inbounds i8, ptr %49, i64 8, !dbg !2286
  store ptr %50, ptr %9, align 8, !dbg !2286
  %51 = load i32, ptr %49, align 8, !dbg !2286
  %52 = trunc i32 %51 to i16, !dbg !2286
  %53 = load i32, ptr %13, align 4, !dbg !2286
  %54 = sext i32 %53 to i64, !dbg !2286
  %55 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %54, !dbg !2286
  store i16 %52, ptr %55, align 8, !dbg !2286
  br label %102, !dbg !2286

56:                                               ; preds = %26
  %57 = load ptr, ptr %9, align 8, !dbg !2286
  %58 = getelementptr inbounds i8, ptr %57, i64 8, !dbg !2286
  store ptr %58, ptr %9, align 8, !dbg !2286
  %59 = load i32, ptr %57, align 8, !dbg !2286
  %60 = trunc i32 %59 to i16, !dbg !2286
  %61 = load i32, ptr %13, align 4, !dbg !2286
  %62 = sext i32 %61 to i64, !dbg !2286
  %63 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %62, !dbg !2286
  store i16 %60, ptr %63, align 8, !dbg !2286
  br label %102, !dbg !2286

64:                                               ; preds = %26
  %65 = load ptr, ptr %9, align 8, !dbg !2286
  %66 = getelementptr inbounds i8, ptr %65, i64 8, !dbg !2286
  store ptr %66, ptr %9, align 8, !dbg !2286
  %67 = load i32, ptr %65, align 8, !dbg !2286
  %68 = load i32, ptr %13, align 4, !dbg !2286
  %69 = sext i32 %68 to i64, !dbg !2286
  %70 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %69, !dbg !2286
  store i32 %67, ptr %70, align 8, !dbg !2286
  br label %102, !dbg !2286

71:                                               ; preds = %26
  %72 = load ptr, ptr %9, align 8, !dbg !2286
  %73 = getelementptr inbounds i8, ptr %72, i64 8, !dbg !2286
  store ptr %73, ptr %9, align 8, !dbg !2286
  %74 = load i32, ptr %72, align 8, !dbg !2286
  %75 = sext i32 %74 to i64, !dbg !2286
  %76 = load i32, ptr %13, align 4, !dbg !2286
  %77 = sext i32 %76 to i64, !dbg !2286
  %78 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %77, !dbg !2286
  store i64 %75, ptr %78, align 8, !dbg !2286
  br label %102, !dbg !2286

79:                                               ; preds = %26
  %80 = load ptr, ptr %9, align 8, !dbg !2286
  %81 = getelementptr inbounds i8, ptr %80, i64 8, !dbg !2286
  store ptr %81, ptr %9, align 8, !dbg !2286
  %82 = load double, ptr %80, align 8, !dbg !2286
  %83 = fptrunc double %82 to float, !dbg !2286
  %84 = load i32, ptr %13, align 4, !dbg !2286
  %85 = sext i32 %84 to i64, !dbg !2286
  %86 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %85, !dbg !2286
  store float %83, ptr %86, align 8, !dbg !2286
  br label %102, !dbg !2286

87:                                               ; preds = %26
  %88 = load ptr, ptr %9, align 8, !dbg !2286
  %89 = getelementptr inbounds i8, ptr %88, i64 8, !dbg !2286
  store ptr %89, ptr %9, align 8, !dbg !2286
  %90 = load double, ptr %88, align 8, !dbg !2286
  %91 = load i32, ptr %13, align 4, !dbg !2286
  %92 = sext i32 %91 to i64, !dbg !2286
  %93 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %92, !dbg !2286
  store double %90, ptr %93, align 8, !dbg !2286
  br label %102, !dbg !2286

94:                                               ; preds = %26
  %95 = load ptr, ptr %9, align 8, !dbg !2286
  %96 = getelementptr inbounds i8, ptr %95, i64 8, !dbg !2286
  store ptr %96, ptr %9, align 8, !dbg !2286
  %97 = load ptr, ptr %95, align 8, !dbg !2286
  %98 = load i32, ptr %13, align 4, !dbg !2286
  %99 = sext i32 %98 to i64, !dbg !2286
  %100 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 %99, !dbg !2286
  store ptr %97, ptr %100, align 8, !dbg !2286
  br label %102, !dbg !2286

101:                                              ; preds = %26
  br label %102, !dbg !2286

102:                                              ; preds = %101, %94, %87, %79, %71, %64, %56, %48, %40, %32
  br label %103, !dbg !2283

103:                                              ; preds = %102
  %104 = load i32, ptr %13, align 4, !dbg !2288
  %105 = add nsw i32 %104, 1, !dbg !2288
  store i32 %105, ptr %13, align 4, !dbg !2288
  br label %22, !dbg !2288, !llvm.loop !2289

106:                                              ; preds = %22
  %107 = load ptr, ptr %8, align 8, !dbg !2290
  %108 = load ptr, ptr %107, align 8, !dbg !2290
  %109 = getelementptr inbounds %struct.JNINativeInterface_, ptr %108, i32 0, i32 93, !dbg !2290
  %110 = load ptr, ptr %109, align 8, !dbg !2290
  %111 = getelementptr inbounds [256 x %union.jvalue], ptr %12, i64 0, i64 0, !dbg !2290
  %112 = load ptr, ptr %5, align 8, !dbg !2290
  %113 = load ptr, ptr %6, align 8, !dbg !2290
  %114 = load ptr, ptr %7, align 8, !dbg !2290
  %115 = load ptr, ptr %8, align 8, !dbg !2290
  call void %110(ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112, ptr noundef %111), !dbg !2290
  call void @llvm.va_end(ptr %9), !dbg !2291
  ret void, !dbg !2292
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 !dbg !2293 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca ptr, align 8
  %10 = alloca ptr, align 8
  %11 = alloca [256 x i8], align 1
  %12 = alloca i32, align 4
  %13 = alloca [256 x %union.jvalue], align 8
  %14 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2294, metadata !DIExpression()), !dbg !2295
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2296, metadata !DIExpression()), !dbg !2295
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2297, metadata !DIExpression()), !dbg !2295
  store ptr %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2298, metadata !DIExpression()), !dbg !2295
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2299, metadata !DIExpression()), !dbg !2295
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2300, metadata !DIExpression()), !dbg !2301
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2302, metadata !DIExpression()), !dbg !2301
  %15 = load ptr, ptr %10, align 8, !dbg !2301
  %16 = load ptr, ptr %15, align 8, !dbg !2301
  %17 = getelementptr inbounds %struct.JNINativeInterface_, ptr %16, i32 0, i32 0, !dbg !2301
  %18 = load ptr, ptr %17, align 8, !dbg !2301
  %19 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 0, !dbg !2301
  %20 = load ptr, ptr %7, align 8, !dbg !2301
  %21 = load ptr, ptr %10, align 8, !dbg !2301
  %22 = call i32 %18(ptr noundef %21, ptr noundef %20, ptr noundef %19), !dbg !2301
  store i32 %22, ptr %12, align 4, !dbg !2301
  call void @llvm.dbg.declare(metadata ptr %13, metadata !2303, metadata !DIExpression()), !dbg !2301
  call void @llvm.dbg.declare(metadata ptr %14, metadata !2304, metadata !DIExpression()), !dbg !2306
  store i32 0, ptr %14, align 4, !dbg !2306
  br label %23, !dbg !2306

23:                                               ; preds = %104, %5
  %24 = load i32, ptr %14, align 4, !dbg !2306
  %25 = load i32, ptr %12, align 4, !dbg !2306
  %26 = icmp slt i32 %24, %25, !dbg !2306
  br i1 %26, label %27, label %107, !dbg !2306

27:                                               ; preds = %23
  %28 = load i32, ptr %14, align 4, !dbg !2307
  %29 = sext i32 %28 to i64, !dbg !2307
  %30 = getelementptr inbounds [256 x i8], ptr %11, i64 0, i64 %29, !dbg !2307
  %31 = load i8, ptr %30, align 1, !dbg !2307
  %32 = sext i8 %31 to i32, !dbg !2307
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
  ], !dbg !2307

33:                                               ; preds = %27
  %34 = load ptr, ptr %6, align 8, !dbg !2310
  %35 = getelementptr inbounds i8, ptr %34, i64 8, !dbg !2310
  store ptr %35, ptr %6, align 8, !dbg !2310
  %36 = load i32, ptr %34, align 8, !dbg !2310
  %37 = trunc i32 %36 to i8, !dbg !2310
  %38 = load i32, ptr %14, align 4, !dbg !2310
  %39 = sext i32 %38 to i64, !dbg !2310
  %40 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %39, !dbg !2310
  store i8 %37, ptr %40, align 8, !dbg !2310
  br label %103, !dbg !2310

41:                                               ; preds = %27
  %42 = load ptr, ptr %6, align 8, !dbg !2310
  %43 = getelementptr inbounds i8, ptr %42, i64 8, !dbg !2310
  store ptr %43, ptr %6, align 8, !dbg !2310
  %44 = load i32, ptr %42, align 8, !dbg !2310
  %45 = trunc i32 %44 to i8, !dbg !2310
  %46 = load i32, ptr %14, align 4, !dbg !2310
  %47 = sext i32 %46 to i64, !dbg !2310
  %48 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %47, !dbg !2310
  store i8 %45, ptr %48, align 8, !dbg !2310
  br label %103, !dbg !2310

49:                                               ; preds = %27
  %50 = load ptr, ptr %6, align 8, !dbg !2310
  %51 = getelementptr inbounds i8, ptr %50, i64 8, !dbg !2310
  store ptr %51, ptr %6, align 8, !dbg !2310
  %52 = load i32, ptr %50, align 8, !dbg !2310
  %53 = trunc i32 %52 to i16, !dbg !2310
  %54 = load i32, ptr %14, align 4, !dbg !2310
  %55 = sext i32 %54 to i64, !dbg !2310
  %56 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %55, !dbg !2310
  store i16 %53, ptr %56, align 8, !dbg !2310
  br label %103, !dbg !2310

57:                                               ; preds = %27
  %58 = load ptr, ptr %6, align 8, !dbg !2310
  %59 = getelementptr inbounds i8, ptr %58, i64 8, !dbg !2310
  store ptr %59, ptr %6, align 8, !dbg !2310
  %60 = load i32, ptr %58, align 8, !dbg !2310
  %61 = trunc i32 %60 to i16, !dbg !2310
  %62 = load i32, ptr %14, align 4, !dbg !2310
  %63 = sext i32 %62 to i64, !dbg !2310
  %64 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %63, !dbg !2310
  store i16 %61, ptr %64, align 8, !dbg !2310
  br label %103, !dbg !2310

65:                                               ; preds = %27
  %66 = load ptr, ptr %6, align 8, !dbg !2310
  %67 = getelementptr inbounds i8, ptr %66, i64 8, !dbg !2310
  store ptr %67, ptr %6, align 8, !dbg !2310
  %68 = load i32, ptr %66, align 8, !dbg !2310
  %69 = load i32, ptr %14, align 4, !dbg !2310
  %70 = sext i32 %69 to i64, !dbg !2310
  %71 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %70, !dbg !2310
  store i32 %68, ptr %71, align 8, !dbg !2310
  br label %103, !dbg !2310

72:                                               ; preds = %27
  %73 = load ptr, ptr %6, align 8, !dbg !2310
  %74 = getelementptr inbounds i8, ptr %73, i64 8, !dbg !2310
  store ptr %74, ptr %6, align 8, !dbg !2310
  %75 = load i32, ptr %73, align 8, !dbg !2310
  %76 = sext i32 %75 to i64, !dbg !2310
  %77 = load i32, ptr %14, align 4, !dbg !2310
  %78 = sext i32 %77 to i64, !dbg !2310
  %79 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %78, !dbg !2310
  store i64 %76, ptr %79, align 8, !dbg !2310
  br label %103, !dbg !2310

80:                                               ; preds = %27
  %81 = load ptr, ptr %6, align 8, !dbg !2310
  %82 = getelementptr inbounds i8, ptr %81, i64 8, !dbg !2310
  store ptr %82, ptr %6, align 8, !dbg !2310
  %83 = load double, ptr %81, align 8, !dbg !2310
  %84 = fptrunc double %83 to float, !dbg !2310
  %85 = load i32, ptr %14, align 4, !dbg !2310
  %86 = sext i32 %85 to i64, !dbg !2310
  %87 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %86, !dbg !2310
  store float %84, ptr %87, align 8, !dbg !2310
  br label %103, !dbg !2310

88:                                               ; preds = %27
  %89 = load ptr, ptr %6, align 8, !dbg !2310
  %90 = getelementptr inbounds i8, ptr %89, i64 8, !dbg !2310
  store ptr %90, ptr %6, align 8, !dbg !2310
  %91 = load double, ptr %89, align 8, !dbg !2310
  %92 = load i32, ptr %14, align 4, !dbg !2310
  %93 = sext i32 %92 to i64, !dbg !2310
  %94 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %93, !dbg !2310
  store double %91, ptr %94, align 8, !dbg !2310
  br label %103, !dbg !2310

95:                                               ; preds = %27
  %96 = load ptr, ptr %6, align 8, !dbg !2310
  %97 = getelementptr inbounds i8, ptr %96, i64 8, !dbg !2310
  store ptr %97, ptr %6, align 8, !dbg !2310
  %98 = load ptr, ptr %96, align 8, !dbg !2310
  %99 = load i32, ptr %14, align 4, !dbg !2310
  %100 = sext i32 %99 to i64, !dbg !2310
  %101 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 %100, !dbg !2310
  store ptr %98, ptr %101, align 8, !dbg !2310
  br label %103, !dbg !2310

102:                                              ; preds = %27
  br label %103, !dbg !2310

103:                                              ; preds = %102, %95, %88, %80, %72, %65, %57, %49, %41, %33
  br label %104, !dbg !2307

104:                                              ; preds = %103
  %105 = load i32, ptr %14, align 4, !dbg !2312
  %106 = add nsw i32 %105, 1, !dbg !2312
  store i32 %106, ptr %14, align 4, !dbg !2312
  br label %23, !dbg !2312, !llvm.loop !2313

107:                                              ; preds = %23
  %108 = load ptr, ptr %10, align 8, !dbg !2314
  %109 = load ptr, ptr %108, align 8, !dbg !2314
  %110 = getelementptr inbounds %struct.JNINativeInterface_, ptr %109, i32 0, i32 93, !dbg !2314
  %111 = load ptr, ptr %110, align 8, !dbg !2314
  %112 = getelementptr inbounds [256 x %union.jvalue], ptr %13, i64 0, i64 0, !dbg !2314
  %113 = load ptr, ptr %7, align 8, !dbg !2314
  %114 = load ptr, ptr %8, align 8, !dbg !2314
  %115 = load ptr, ptr %9, align 8, !dbg !2314
  %116 = load ptr, ptr %10, align 8, !dbg !2314
  call void %111(ptr noundef %116, ptr noundef %115, ptr noundef %114, ptr noundef %113, ptr noundef %112), !dbg !2314
  ret void, !dbg !2315
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) #0 !dbg !2316 {
  %4 = alloca ptr, align 8
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca [256 x i8], align 1
  %9 = alloca i32, align 4
  %10 = alloca [256 x %union.jvalue], align 8
  %11 = alloca i32, align 4
  store ptr %2, ptr %4, align 8
  call void @llvm.dbg.declare(metadata ptr %4, metadata !2317, metadata !DIExpression()), !dbg !2318
  store ptr %1, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2319, metadata !DIExpression()), !dbg !2318
  store ptr %0, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2320, metadata !DIExpression()), !dbg !2318
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2321, metadata !DIExpression()), !dbg !2322
  call void @llvm.va_start(ptr %7), !dbg !2323
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2324, metadata !DIExpression()), !dbg !2325
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2326, metadata !DIExpression()), !dbg !2325
  %12 = load ptr, ptr %6, align 8, !dbg !2325
  %13 = load ptr, ptr %12, align 8, !dbg !2325
  %14 = getelementptr inbounds %struct.JNINativeInterface_, ptr %13, i32 0, i32 0, !dbg !2325
  %15 = load ptr, ptr %14, align 8, !dbg !2325
  %16 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 0, !dbg !2325
  %17 = load ptr, ptr %4, align 8, !dbg !2325
  %18 = load ptr, ptr %6, align 8, !dbg !2325
  %19 = call i32 %15(ptr noundef %18, ptr noundef %17, ptr noundef %16), !dbg !2325
  store i32 %19, ptr %9, align 4, !dbg !2325
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2327, metadata !DIExpression()), !dbg !2325
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2328, metadata !DIExpression()), !dbg !2330
  store i32 0, ptr %11, align 4, !dbg !2330
  br label %20, !dbg !2330

20:                                               ; preds = %101, %3
  %21 = load i32, ptr %11, align 4, !dbg !2330
  %22 = load i32, ptr %9, align 4, !dbg !2330
  %23 = icmp slt i32 %21, %22, !dbg !2330
  br i1 %23, label %24, label %104, !dbg !2330

24:                                               ; preds = %20
  %25 = load i32, ptr %11, align 4, !dbg !2331
  %26 = sext i32 %25 to i64, !dbg !2331
  %27 = getelementptr inbounds [256 x i8], ptr %8, i64 0, i64 %26, !dbg !2331
  %28 = load i8, ptr %27, align 1, !dbg !2331
  %29 = sext i8 %28 to i32, !dbg !2331
  switch i32 %29, label %99 [
    i32 90, label %30
    i32 66, label %38
    i32 67, label %46
    i32 83, label %54
    i32 73, label %62
    i32 74, label %69
    i32 70, label %77
    i32 68, label %85
    i32 76, label %92
  ], !dbg !2331

30:                                               ; preds = %24
  %31 = load ptr, ptr %7, align 8, !dbg !2334
  %32 = getelementptr inbounds i8, ptr %31, i64 8, !dbg !2334
  store ptr %32, ptr %7, align 8, !dbg !2334
  %33 = load i32, ptr %31, align 8, !dbg !2334
  %34 = trunc i32 %33 to i8, !dbg !2334
  %35 = load i32, ptr %11, align 4, !dbg !2334
  %36 = sext i32 %35 to i64, !dbg !2334
  %37 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %36, !dbg !2334
  store i8 %34, ptr %37, align 8, !dbg !2334
  br label %100, !dbg !2334

38:                                               ; preds = %24
  %39 = load ptr, ptr %7, align 8, !dbg !2334
  %40 = getelementptr inbounds i8, ptr %39, i64 8, !dbg !2334
  store ptr %40, ptr %7, align 8, !dbg !2334
  %41 = load i32, ptr %39, align 8, !dbg !2334
  %42 = trunc i32 %41 to i8, !dbg !2334
  %43 = load i32, ptr %11, align 4, !dbg !2334
  %44 = sext i32 %43 to i64, !dbg !2334
  %45 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %44, !dbg !2334
  store i8 %42, ptr %45, align 8, !dbg !2334
  br label %100, !dbg !2334

46:                                               ; preds = %24
  %47 = load ptr, ptr %7, align 8, !dbg !2334
  %48 = getelementptr inbounds i8, ptr %47, i64 8, !dbg !2334
  store ptr %48, ptr %7, align 8, !dbg !2334
  %49 = load i32, ptr %47, align 8, !dbg !2334
  %50 = trunc i32 %49 to i16, !dbg !2334
  %51 = load i32, ptr %11, align 4, !dbg !2334
  %52 = sext i32 %51 to i64, !dbg !2334
  %53 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %52, !dbg !2334
  store i16 %50, ptr %53, align 8, !dbg !2334
  br label %100, !dbg !2334

54:                                               ; preds = %24
  %55 = load ptr, ptr %7, align 8, !dbg !2334
  %56 = getelementptr inbounds i8, ptr %55, i64 8, !dbg !2334
  store ptr %56, ptr %7, align 8, !dbg !2334
  %57 = load i32, ptr %55, align 8, !dbg !2334
  %58 = trunc i32 %57 to i16, !dbg !2334
  %59 = load i32, ptr %11, align 4, !dbg !2334
  %60 = sext i32 %59 to i64, !dbg !2334
  %61 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %60, !dbg !2334
  store i16 %58, ptr %61, align 8, !dbg !2334
  br label %100, !dbg !2334

62:                                               ; preds = %24
  %63 = load ptr, ptr %7, align 8, !dbg !2334
  %64 = getelementptr inbounds i8, ptr %63, i64 8, !dbg !2334
  store ptr %64, ptr %7, align 8, !dbg !2334
  %65 = load i32, ptr %63, align 8, !dbg !2334
  %66 = load i32, ptr %11, align 4, !dbg !2334
  %67 = sext i32 %66 to i64, !dbg !2334
  %68 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %67, !dbg !2334
  store i32 %65, ptr %68, align 8, !dbg !2334
  br label %100, !dbg !2334

69:                                               ; preds = %24
  %70 = load ptr, ptr %7, align 8, !dbg !2334
  %71 = getelementptr inbounds i8, ptr %70, i64 8, !dbg !2334
  store ptr %71, ptr %7, align 8, !dbg !2334
  %72 = load i32, ptr %70, align 8, !dbg !2334
  %73 = sext i32 %72 to i64, !dbg !2334
  %74 = load i32, ptr %11, align 4, !dbg !2334
  %75 = sext i32 %74 to i64, !dbg !2334
  %76 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %75, !dbg !2334
  store i64 %73, ptr %76, align 8, !dbg !2334
  br label %100, !dbg !2334

77:                                               ; preds = %24
  %78 = load ptr, ptr %7, align 8, !dbg !2334
  %79 = getelementptr inbounds i8, ptr %78, i64 8, !dbg !2334
  store ptr %79, ptr %7, align 8, !dbg !2334
  %80 = load double, ptr %78, align 8, !dbg !2334
  %81 = fptrunc double %80 to float, !dbg !2334
  %82 = load i32, ptr %11, align 4, !dbg !2334
  %83 = sext i32 %82 to i64, !dbg !2334
  %84 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %83, !dbg !2334
  store float %81, ptr %84, align 8, !dbg !2334
  br label %100, !dbg !2334

85:                                               ; preds = %24
  %86 = load ptr, ptr %7, align 8, !dbg !2334
  %87 = getelementptr inbounds i8, ptr %86, i64 8, !dbg !2334
  store ptr %87, ptr %7, align 8, !dbg !2334
  %88 = load double, ptr %86, align 8, !dbg !2334
  %89 = load i32, ptr %11, align 4, !dbg !2334
  %90 = sext i32 %89 to i64, !dbg !2334
  %91 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %90, !dbg !2334
  store double %88, ptr %91, align 8, !dbg !2334
  br label %100, !dbg !2334

92:                                               ; preds = %24
  %93 = load ptr, ptr %7, align 8, !dbg !2334
  %94 = getelementptr inbounds i8, ptr %93, i64 8, !dbg !2334
  store ptr %94, ptr %7, align 8, !dbg !2334
  %95 = load ptr, ptr %93, align 8, !dbg !2334
  %96 = load i32, ptr %11, align 4, !dbg !2334
  %97 = sext i32 %96 to i64, !dbg !2334
  %98 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 %97, !dbg !2334
  store ptr %95, ptr %98, align 8, !dbg !2334
  br label %100, !dbg !2334

99:                                               ; preds = %24
  br label %100, !dbg !2334

100:                                              ; preds = %99, %92, %85, %77, %69, %62, %54, %46, %38, %30
  br label %101, !dbg !2331

101:                                              ; preds = %100
  %102 = load i32, ptr %11, align 4, !dbg !2336
  %103 = add nsw i32 %102, 1, !dbg !2336
  store i32 %103, ptr %11, align 4, !dbg !2336
  br label %20, !dbg !2336, !llvm.loop !2337

104:                                              ; preds = %20
  %105 = load ptr, ptr %6, align 8, !dbg !2338
  %106 = load ptr, ptr %105, align 8, !dbg !2338
  %107 = getelementptr inbounds %struct.JNINativeInterface_, ptr %106, i32 0, i32 143, !dbg !2338
  %108 = load ptr, ptr %107, align 8, !dbg !2338
  %109 = getelementptr inbounds [256 x %union.jvalue], ptr %10, i64 0, i64 0, !dbg !2338
  %110 = load ptr, ptr %4, align 8, !dbg !2338
  %111 = load ptr, ptr %5, align 8, !dbg !2338
  %112 = load ptr, ptr %6, align 8, !dbg !2338
  call void %108(ptr noundef %112, ptr noundef %111, ptr noundef %110, ptr noundef %109), !dbg !2338
  call void @llvm.va_end(ptr %7), !dbg !2339
  ret void, !dbg !2340
}

; Function Attrs: noinline nounwind optnone uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 !dbg !2341 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca [256 x i8], align 1
  %10 = alloca i32, align 4
  %11 = alloca [256 x %union.jvalue], align 8
  %12 = alloca i32, align 4
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2342, metadata !DIExpression()), !dbg !2343
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2344, metadata !DIExpression()), !dbg !2343
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2345, metadata !DIExpression()), !dbg !2343
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2346, metadata !DIExpression()), !dbg !2343
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2347, metadata !DIExpression()), !dbg !2348
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2349, metadata !DIExpression()), !dbg !2348
  %13 = load ptr, ptr %8, align 8, !dbg !2348
  %14 = load ptr, ptr %13, align 8, !dbg !2348
  %15 = getelementptr inbounds %struct.JNINativeInterface_, ptr %14, i32 0, i32 0, !dbg !2348
  %16 = load ptr, ptr %15, align 8, !dbg !2348
  %17 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 0, !dbg !2348
  %18 = load ptr, ptr %6, align 8, !dbg !2348
  %19 = load ptr, ptr %8, align 8, !dbg !2348
  %20 = call i32 %16(ptr noundef %19, ptr noundef %18, ptr noundef %17), !dbg !2348
  store i32 %20, ptr %10, align 4, !dbg !2348
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2350, metadata !DIExpression()), !dbg !2348
  call void @llvm.dbg.declare(metadata ptr %12, metadata !2351, metadata !DIExpression()), !dbg !2353
  store i32 0, ptr %12, align 4, !dbg !2353
  br label %21, !dbg !2353

21:                                               ; preds = %102, %4
  %22 = load i32, ptr %12, align 4, !dbg !2353
  %23 = load i32, ptr %10, align 4, !dbg !2353
  %24 = icmp slt i32 %22, %23, !dbg !2353
  br i1 %24, label %25, label %105, !dbg !2353

25:                                               ; preds = %21
  %26 = load i32, ptr %12, align 4, !dbg !2354
  %27 = sext i32 %26 to i64, !dbg !2354
  %28 = getelementptr inbounds [256 x i8], ptr %9, i64 0, i64 %27, !dbg !2354
  %29 = load i8, ptr %28, align 1, !dbg !2354
  %30 = sext i8 %29 to i32, !dbg !2354
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
  ], !dbg !2354

31:                                               ; preds = %25
  %32 = load ptr, ptr %5, align 8, !dbg !2357
  %33 = getelementptr inbounds i8, ptr %32, i64 8, !dbg !2357
  store ptr %33, ptr %5, align 8, !dbg !2357
  %34 = load i32, ptr %32, align 8, !dbg !2357
  %35 = trunc i32 %34 to i8, !dbg !2357
  %36 = load i32, ptr %12, align 4, !dbg !2357
  %37 = sext i32 %36 to i64, !dbg !2357
  %38 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %37, !dbg !2357
  store i8 %35, ptr %38, align 8, !dbg !2357
  br label %101, !dbg !2357

39:                                               ; preds = %25
  %40 = load ptr, ptr %5, align 8, !dbg !2357
  %41 = getelementptr inbounds i8, ptr %40, i64 8, !dbg !2357
  store ptr %41, ptr %5, align 8, !dbg !2357
  %42 = load i32, ptr %40, align 8, !dbg !2357
  %43 = trunc i32 %42 to i8, !dbg !2357
  %44 = load i32, ptr %12, align 4, !dbg !2357
  %45 = sext i32 %44 to i64, !dbg !2357
  %46 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %45, !dbg !2357
  store i8 %43, ptr %46, align 8, !dbg !2357
  br label %101, !dbg !2357

47:                                               ; preds = %25
  %48 = load ptr, ptr %5, align 8, !dbg !2357
  %49 = getelementptr inbounds i8, ptr %48, i64 8, !dbg !2357
  store ptr %49, ptr %5, align 8, !dbg !2357
  %50 = load i32, ptr %48, align 8, !dbg !2357
  %51 = trunc i32 %50 to i16, !dbg !2357
  %52 = load i32, ptr %12, align 4, !dbg !2357
  %53 = sext i32 %52 to i64, !dbg !2357
  %54 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %53, !dbg !2357
  store i16 %51, ptr %54, align 8, !dbg !2357
  br label %101, !dbg !2357

55:                                               ; preds = %25
  %56 = load ptr, ptr %5, align 8, !dbg !2357
  %57 = getelementptr inbounds i8, ptr %56, i64 8, !dbg !2357
  store ptr %57, ptr %5, align 8, !dbg !2357
  %58 = load i32, ptr %56, align 8, !dbg !2357
  %59 = trunc i32 %58 to i16, !dbg !2357
  %60 = load i32, ptr %12, align 4, !dbg !2357
  %61 = sext i32 %60 to i64, !dbg !2357
  %62 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %61, !dbg !2357
  store i16 %59, ptr %62, align 8, !dbg !2357
  br label %101, !dbg !2357

63:                                               ; preds = %25
  %64 = load ptr, ptr %5, align 8, !dbg !2357
  %65 = getelementptr inbounds i8, ptr %64, i64 8, !dbg !2357
  store ptr %65, ptr %5, align 8, !dbg !2357
  %66 = load i32, ptr %64, align 8, !dbg !2357
  %67 = load i32, ptr %12, align 4, !dbg !2357
  %68 = sext i32 %67 to i64, !dbg !2357
  %69 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %68, !dbg !2357
  store i32 %66, ptr %69, align 8, !dbg !2357
  br label %101, !dbg !2357

70:                                               ; preds = %25
  %71 = load ptr, ptr %5, align 8, !dbg !2357
  %72 = getelementptr inbounds i8, ptr %71, i64 8, !dbg !2357
  store ptr %72, ptr %5, align 8, !dbg !2357
  %73 = load i32, ptr %71, align 8, !dbg !2357
  %74 = sext i32 %73 to i64, !dbg !2357
  %75 = load i32, ptr %12, align 4, !dbg !2357
  %76 = sext i32 %75 to i64, !dbg !2357
  %77 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %76, !dbg !2357
  store i64 %74, ptr %77, align 8, !dbg !2357
  br label %101, !dbg !2357

78:                                               ; preds = %25
  %79 = load ptr, ptr %5, align 8, !dbg !2357
  %80 = getelementptr inbounds i8, ptr %79, i64 8, !dbg !2357
  store ptr %80, ptr %5, align 8, !dbg !2357
  %81 = load double, ptr %79, align 8, !dbg !2357
  %82 = fptrunc double %81 to float, !dbg !2357
  %83 = load i32, ptr %12, align 4, !dbg !2357
  %84 = sext i32 %83 to i64, !dbg !2357
  %85 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %84, !dbg !2357
  store float %82, ptr %85, align 8, !dbg !2357
  br label %101, !dbg !2357

86:                                               ; preds = %25
  %87 = load ptr, ptr %5, align 8, !dbg !2357
  %88 = getelementptr inbounds i8, ptr %87, i64 8, !dbg !2357
  store ptr %88, ptr %5, align 8, !dbg !2357
  %89 = load double, ptr %87, align 8, !dbg !2357
  %90 = load i32, ptr %12, align 4, !dbg !2357
  %91 = sext i32 %90 to i64, !dbg !2357
  %92 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %91, !dbg !2357
  store double %89, ptr %92, align 8, !dbg !2357
  br label %101, !dbg !2357

93:                                               ; preds = %25
  %94 = load ptr, ptr %5, align 8, !dbg !2357
  %95 = getelementptr inbounds i8, ptr %94, i64 8, !dbg !2357
  store ptr %95, ptr %5, align 8, !dbg !2357
  %96 = load ptr, ptr %94, align 8, !dbg !2357
  %97 = load i32, ptr %12, align 4, !dbg !2357
  %98 = sext i32 %97 to i64, !dbg !2357
  %99 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 %98, !dbg !2357
  store ptr %96, ptr %99, align 8, !dbg !2357
  br label %101, !dbg !2357

100:                                              ; preds = %25
  br label %101, !dbg !2357

101:                                              ; preds = %100, %93, %86, %78, %70, %63, %55, %47, %39, %31
  br label %102, !dbg !2354

102:                                              ; preds = %101
  %103 = load i32, ptr %12, align 4, !dbg !2359
  %104 = add nsw i32 %103, 1, !dbg !2359
  store i32 %104, ptr %12, align 4, !dbg !2359
  br label %21, !dbg !2359, !llvm.loop !2360

105:                                              ; preds = %21
  %106 = load ptr, ptr %8, align 8, !dbg !2361
  %107 = load ptr, ptr %106, align 8, !dbg !2361
  %108 = getelementptr inbounds %struct.JNINativeInterface_, ptr %107, i32 0, i32 143, !dbg !2361
  %109 = load ptr, ptr %108, align 8, !dbg !2361
  %110 = getelementptr inbounds [256 x %union.jvalue], ptr %11, i64 0, i64 0, !dbg !2361
  %111 = load ptr, ptr %6, align 8, !dbg !2361
  %112 = load ptr, ptr %7, align 8, !dbg !2361
  %113 = load ptr, ptr %8, align 8, !dbg !2361
  call void %109(ptr noundef %113, ptr noundef %112, ptr noundef %111, ptr noundef %110), !dbg !2361
  ret void, !dbg !2362
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsprintf_l(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) #0 comdat !dbg !2363 {
  %5 = alloca ptr, align 8
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  store ptr %3, ptr %5, align 8
  call void @llvm.dbg.declare(metadata ptr %5, metadata !2379, metadata !DIExpression()), !dbg !2380
  store ptr %2, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2381, metadata !DIExpression()), !dbg !2382
  store ptr %1, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2383, metadata !DIExpression()), !dbg !2384
  store ptr %0, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2385, metadata !DIExpression()), !dbg !2386
  %9 = load ptr, ptr %5, align 8, !dbg !2387
  %10 = load ptr, ptr %6, align 8, !dbg !2387
  %11 = load ptr, ptr %7, align 8, !dbg !2387
  %12 = load ptr, ptr %8, align 8, !dbg !2387
  %13 = call i32 @_vsnprintf_l(ptr noundef %12, i64 noundef -1, ptr noundef %11, ptr noundef %10, ptr noundef %9), !dbg !2387
  ret i32 %13, !dbg !2387
}

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local i32 @_vsnprintf_l(ptr noundef %0, i64 noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) #0 comdat !dbg !2388 {
  %6 = alloca ptr, align 8
  %7 = alloca ptr, align 8
  %8 = alloca ptr, align 8
  %9 = alloca i64, align 8
  %10 = alloca ptr, align 8
  %11 = alloca i32, align 4
  store ptr %4, ptr %6, align 8
  call void @llvm.dbg.declare(metadata ptr %6, metadata !2391, metadata !DIExpression()), !dbg !2392
  store ptr %3, ptr %7, align 8
  call void @llvm.dbg.declare(metadata ptr %7, metadata !2393, metadata !DIExpression()), !dbg !2394
  store ptr %2, ptr %8, align 8
  call void @llvm.dbg.declare(metadata ptr %8, metadata !2395, metadata !DIExpression()), !dbg !2396
  store i64 %1, ptr %9, align 8
  call void @llvm.dbg.declare(metadata ptr %9, metadata !2397, metadata !DIExpression()), !dbg !2398
  store ptr %0, ptr %10, align 8
  call void @llvm.dbg.declare(metadata ptr %10, metadata !2399, metadata !DIExpression()), !dbg !2400
  call void @llvm.dbg.declare(metadata ptr %11, metadata !2401, metadata !DIExpression()), !dbg !2403
  %12 = load ptr, ptr %6, align 8, !dbg !2403
  %13 = load ptr, ptr %7, align 8, !dbg !2403
  %14 = load ptr, ptr %8, align 8, !dbg !2403
  %15 = load i64, ptr %9, align 8, !dbg !2403
  %16 = load ptr, ptr %10, align 8, !dbg !2403
  %17 = call ptr @__local_stdio_printf_options(), !dbg !2403
  %18 = load i64, ptr %17, align 8, !dbg !2403
  %19 = or i64 %18, 1, !dbg !2403
  %20 = call i32 @__stdio_common_vsprintf(i64 noundef %19, ptr noundef %16, i64 noundef %15, ptr noundef %14, ptr noundef %13, ptr noundef %12), !dbg !2403
  store i32 %20, ptr %11, align 4, !dbg !2403
  %21 = load i32, ptr %11, align 4, !dbg !2404
  %22 = icmp slt i32 %21, 0, !dbg !2404
  br i1 %22, label %23, label %24, !dbg !2404

23:                                               ; preds = %5
  br label %26, !dbg !2404

24:                                               ; preds = %5
  %25 = load i32, ptr %11, align 4, !dbg !2404
  br label %26, !dbg !2404

26:                                               ; preds = %24, %23
  %27 = phi i32 [ -1, %23 ], [ %25, %24 ], !dbg !2404
  ret i32 %27, !dbg !2404
}

declare dso_local i32 @__stdio_common_vsprintf(i64 noundef, ptr noundef, i64 noundef, ptr noundef, ptr noundef, ptr noundef) #3

; Function Attrs: noinline nounwind optnone uwtable
define linkonce_odr dso_local ptr @__local_stdio_printf_options() #0 comdat !dbg !2 {
  ret ptr @__local_stdio_printf_options._OptionsStorage, !dbg !2405
}

attributes #0 = { noinline nounwind optnone uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+v8a" }
attributes #1 = { nocallback nofree nosync nounwind readnone speculatable willreturn }
attributes #2 = { nocallback nofree nosync nounwind willreturn }
attributes #3 = { "frame-pointer"="none" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+v8a" }

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
!1103 = !DILocalVariable(name: "sig", scope: !1097, file: !9, line: 3, type: !1104)
!1104 = !DICompositeType(tag: DW_TAG_array_type, baseType: !53, size: 2048, elements: !1105)
!1105 = !{!1106}
!1106 = !DISubrange(count: 256)
!1107 = !DILocalVariable(name: "argc", scope: !1097, file: !9, line: 3, type: !13)
!1108 = !DILocalVariable(name: "argv", scope: !1097, file: !9, line: 3, type: !1109)
!1109 = !DICompositeType(tag: DW_TAG_array_type, baseType: !158, size: 16384, elements: !1105)
!1110 = !DILocalVariable(name: "i", scope: !1111, file: !9, line: 3, type: !13)
!1111 = distinct !DILexicalBlock(scope: !1097, file: !9, line: 3)
!1112 = !DILocation(line: 3, scope: !1111)
!1113 = !DILocation(line: 3, scope: !1114)
!1114 = distinct !DILexicalBlock(scope: !1115, file: !9, line: 3)
!1115 = distinct !DILexicalBlock(scope: !1111, file: !9, line: 3)
!1116 = !DILocation(line: 3, scope: !1117)
!1117 = distinct !DILexicalBlock(scope: !1114, file: !9, line: 3)
!1118 = !DILocation(line: 3, scope: !1115)
!1119 = distinct !{!1119, !1112, !1112, !1120}
!1120 = !{!"llvm.loop.mustprogress"}
!1121 = !DILocalVariable(name: "ret", scope: !1097, file: !9, line: 3, type: !48)
!1122 = distinct !DISubprogram(name: "JNI_CallObjectMethodV", scope: !9, file: !9, line: 3, type: !198, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1123 = !DILocalVariable(name: "args", arg: 4, scope: !1122, file: !9, line: 3, type: !149)
!1124 = !DILocation(line: 3, scope: !1122)
!1125 = !DILocalVariable(name: "methodID", arg: 3, scope: !1122, file: !9, line: 3, type: !67)
!1126 = !DILocalVariable(name: "obj", arg: 2, scope: !1122, file: !9, line: 3, type: !48)
!1127 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1122, file: !9, line: 3, type: !25)
!1128 = !DILocalVariable(name: "sig", scope: !1122, file: !9, line: 3, type: !1104)
!1129 = !DILocalVariable(name: "argc", scope: !1122, file: !9, line: 3, type: !13)
!1130 = !DILocalVariable(name: "argv", scope: !1122, file: !9, line: 3, type: !1109)
!1131 = !DILocalVariable(name: "i", scope: !1132, file: !9, line: 3, type: !13)
!1132 = distinct !DILexicalBlock(scope: !1122, file: !9, line: 3)
!1133 = !DILocation(line: 3, scope: !1132)
!1134 = !DILocation(line: 3, scope: !1135)
!1135 = distinct !DILexicalBlock(scope: !1136, file: !9, line: 3)
!1136 = distinct !DILexicalBlock(scope: !1132, file: !9, line: 3)
!1137 = !DILocation(line: 3, scope: !1138)
!1138 = distinct !DILexicalBlock(scope: !1135, file: !9, line: 3)
!1139 = !DILocation(line: 3, scope: !1136)
!1140 = distinct !{!1140, !1133, !1133, !1120}
!1141 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethod", scope: !9, file: !9, line: 3, type: !314, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1142 = !DILocalVariable(name: "methodID", arg: 4, scope: !1141, file: !9, line: 3, type: !67)
!1143 = !DILocation(line: 3, scope: !1141)
!1144 = !DILocalVariable(name: "clazz", arg: 3, scope: !1141, file: !9, line: 3, type: !47)
!1145 = !DILocalVariable(name: "obj", arg: 2, scope: !1141, file: !9, line: 3, type: !48)
!1146 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1141, file: !9, line: 3, type: !25)
!1147 = !DILocalVariable(name: "args", scope: !1141, file: !9, line: 3, type: !149)
!1148 = !DILocalVariable(name: "sig", scope: !1141, file: !9, line: 3, type: !1104)
!1149 = !DILocalVariable(name: "argc", scope: !1141, file: !9, line: 3, type: !13)
!1150 = !DILocalVariable(name: "argv", scope: !1141, file: !9, line: 3, type: !1109)
!1151 = !DILocalVariable(name: "i", scope: !1152, file: !9, line: 3, type: !13)
!1152 = distinct !DILexicalBlock(scope: !1141, file: !9, line: 3)
!1153 = !DILocation(line: 3, scope: !1152)
!1154 = !DILocation(line: 3, scope: !1155)
!1155 = distinct !DILexicalBlock(scope: !1156, file: !9, line: 3)
!1156 = distinct !DILexicalBlock(scope: !1152, file: !9, line: 3)
!1157 = !DILocation(line: 3, scope: !1158)
!1158 = distinct !DILexicalBlock(scope: !1155, file: !9, line: 3)
!1159 = !DILocation(line: 3, scope: !1156)
!1160 = distinct !{!1160, !1153, !1153, !1120}
!1161 = !DILocalVariable(name: "ret", scope: !1141, file: !9, line: 3, type: !48)
!1162 = distinct !DISubprogram(name: "JNI_CallNonvirtualObjectMethodV", scope: !9, file: !9, line: 3, type: !318, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1163 = !DILocalVariable(name: "args", arg: 5, scope: !1162, file: !9, line: 3, type: !149)
!1164 = !DILocation(line: 3, scope: !1162)
!1165 = !DILocalVariable(name: "methodID", arg: 4, scope: !1162, file: !9, line: 3, type: !67)
!1166 = !DILocalVariable(name: "clazz", arg: 3, scope: !1162, file: !9, line: 3, type: !47)
!1167 = !DILocalVariable(name: "obj", arg: 2, scope: !1162, file: !9, line: 3, type: !48)
!1168 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1162, file: !9, line: 3, type: !25)
!1169 = !DILocalVariable(name: "sig", scope: !1162, file: !9, line: 3, type: !1104)
!1170 = !DILocalVariable(name: "argc", scope: !1162, file: !9, line: 3, type: !13)
!1171 = !DILocalVariable(name: "argv", scope: !1162, file: !9, line: 3, type: !1109)
!1172 = !DILocalVariable(name: "i", scope: !1173, file: !9, line: 3, type: !13)
!1173 = distinct !DILexicalBlock(scope: !1162, file: !9, line: 3)
!1174 = !DILocation(line: 3, scope: !1173)
!1175 = !DILocation(line: 3, scope: !1176)
!1176 = distinct !DILexicalBlock(scope: !1177, file: !9, line: 3)
!1177 = distinct !DILexicalBlock(scope: !1173, file: !9, line: 3)
!1178 = !DILocation(line: 3, scope: !1179)
!1179 = distinct !DILexicalBlock(scope: !1176, file: !9, line: 3)
!1180 = !DILocation(line: 3, scope: !1177)
!1181 = distinct !{!1181, !1174, !1174, !1120}
!1182 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethod", scope: !9, file: !9, line: 3, type: !143, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1183 = !DILocalVariable(name: "methodID", arg: 3, scope: !1182, file: !9, line: 3, type: !67)
!1184 = !DILocation(line: 3, scope: !1182)
!1185 = !DILocalVariable(name: "clazz", arg: 2, scope: !1182, file: !9, line: 3, type: !47)
!1186 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1182, file: !9, line: 3, type: !25)
!1187 = !DILocalVariable(name: "args", scope: !1182, file: !9, line: 3, type: !149)
!1188 = !DILocalVariable(name: "sig", scope: !1182, file: !9, line: 3, type: !1104)
!1189 = !DILocalVariable(name: "argc", scope: !1182, file: !9, line: 3, type: !13)
!1190 = !DILocalVariable(name: "argv", scope: !1182, file: !9, line: 3, type: !1109)
!1191 = !DILocalVariable(name: "i", scope: !1192, file: !9, line: 3, type: !13)
!1192 = distinct !DILexicalBlock(scope: !1182, file: !9, line: 3)
!1193 = !DILocation(line: 3, scope: !1192)
!1194 = !DILocation(line: 3, scope: !1195)
!1195 = distinct !DILexicalBlock(scope: !1196, file: !9, line: 3)
!1196 = distinct !DILexicalBlock(scope: !1192, file: !9, line: 3)
!1197 = !DILocation(line: 3, scope: !1198)
!1198 = distinct !DILexicalBlock(scope: !1195, file: !9, line: 3)
!1199 = !DILocation(line: 3, scope: !1196)
!1200 = distinct !{!1200, !1193, !1193, !1120}
!1201 = !DILocalVariable(name: "ret", scope: !1182, file: !9, line: 3, type: !48)
!1202 = distinct !DISubprogram(name: "JNI_CallStaticObjectMethodV", scope: !9, file: !9, line: 3, type: !147, scopeLine: 3, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1203 = !DILocalVariable(name: "args", arg: 4, scope: !1202, file: !9, line: 3, type: !149)
!1204 = !DILocation(line: 3, scope: !1202)
!1205 = !DILocalVariable(name: "methodID", arg: 3, scope: !1202, file: !9, line: 3, type: !67)
!1206 = !DILocalVariable(name: "clazz", arg: 2, scope: !1202, file: !9, line: 3, type: !47)
!1207 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1202, file: !9, line: 3, type: !25)
!1208 = !DILocalVariable(name: "sig", scope: !1202, file: !9, line: 3, type: !1104)
!1209 = !DILocalVariable(name: "argc", scope: !1202, file: !9, line: 3, type: !13)
!1210 = !DILocalVariable(name: "argv", scope: !1202, file: !9, line: 3, type: !1109)
!1211 = !DILocalVariable(name: "i", scope: !1212, file: !9, line: 3, type: !13)
!1212 = distinct !DILexicalBlock(scope: !1202, file: !9, line: 3)
!1213 = !DILocation(line: 3, scope: !1212)
!1214 = !DILocation(line: 3, scope: !1215)
!1215 = distinct !DILexicalBlock(scope: !1216, file: !9, line: 3)
!1216 = distinct !DILexicalBlock(scope: !1212, file: !9, line: 3)
!1217 = !DILocation(line: 3, scope: !1218)
!1218 = distinct !DILexicalBlock(scope: !1215, file: !9, line: 3)
!1219 = !DILocation(line: 3, scope: !1216)
!1220 = distinct !{!1220, !1213, !1213, !1120}
!1221 = distinct !DISubprogram(name: "JNI_CallBooleanMethod", scope: !9, file: !9, line: 4, type: !206, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1222 = !DILocalVariable(name: "methodID", arg: 3, scope: !1221, file: !9, line: 4, type: !67)
!1223 = !DILocation(line: 4, scope: !1221)
!1224 = !DILocalVariable(name: "obj", arg: 2, scope: !1221, file: !9, line: 4, type: !48)
!1225 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1221, file: !9, line: 4, type: !25)
!1226 = !DILocalVariable(name: "args", scope: !1221, file: !9, line: 4, type: !149)
!1227 = !DILocalVariable(name: "sig", scope: !1221, file: !9, line: 4, type: !1104)
!1228 = !DILocalVariable(name: "argc", scope: !1221, file: !9, line: 4, type: !13)
!1229 = !DILocalVariable(name: "argv", scope: !1221, file: !9, line: 4, type: !1109)
!1230 = !DILocalVariable(name: "i", scope: !1231, file: !9, line: 4, type: !13)
!1231 = distinct !DILexicalBlock(scope: !1221, file: !9, line: 4)
!1232 = !DILocation(line: 4, scope: !1231)
!1233 = !DILocation(line: 4, scope: !1234)
!1234 = distinct !DILexicalBlock(scope: !1235, file: !9, line: 4)
!1235 = distinct !DILexicalBlock(scope: !1231, file: !9, line: 4)
!1236 = !DILocation(line: 4, scope: !1237)
!1237 = distinct !DILexicalBlock(scope: !1234, file: !9, line: 4)
!1238 = !DILocation(line: 4, scope: !1235)
!1239 = distinct !{!1239, !1232, !1232, !1120}
!1240 = !DILocalVariable(name: "ret", scope: !1221, file: !9, line: 4, type: !81)
!1241 = distinct !DISubprogram(name: "JNI_CallBooleanMethodV", scope: !9, file: !9, line: 4, type: !210, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1242 = !DILocalVariable(name: "args", arg: 4, scope: !1241, file: !9, line: 4, type: !149)
!1243 = !DILocation(line: 4, scope: !1241)
!1244 = !DILocalVariable(name: "methodID", arg: 3, scope: !1241, file: !9, line: 4, type: !67)
!1245 = !DILocalVariable(name: "obj", arg: 2, scope: !1241, file: !9, line: 4, type: !48)
!1246 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1241, file: !9, line: 4, type: !25)
!1247 = !DILocalVariable(name: "sig", scope: !1241, file: !9, line: 4, type: !1104)
!1248 = !DILocalVariable(name: "argc", scope: !1241, file: !9, line: 4, type: !13)
!1249 = !DILocalVariable(name: "argv", scope: !1241, file: !9, line: 4, type: !1109)
!1250 = !DILocalVariable(name: "i", scope: !1251, file: !9, line: 4, type: !13)
!1251 = distinct !DILexicalBlock(scope: !1241, file: !9, line: 4)
!1252 = !DILocation(line: 4, scope: !1251)
!1253 = !DILocation(line: 4, scope: !1254)
!1254 = distinct !DILexicalBlock(scope: !1255, file: !9, line: 4)
!1255 = distinct !DILexicalBlock(scope: !1251, file: !9, line: 4)
!1256 = !DILocation(line: 4, scope: !1257)
!1257 = distinct !DILexicalBlock(scope: !1254, file: !9, line: 4)
!1258 = !DILocation(line: 4, scope: !1255)
!1259 = distinct !{!1259, !1252, !1252, !1120}
!1260 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethod", scope: !9, file: !9, line: 4, type: !326, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1261 = !DILocalVariable(name: "methodID", arg: 4, scope: !1260, file: !9, line: 4, type: !67)
!1262 = !DILocation(line: 4, scope: !1260)
!1263 = !DILocalVariable(name: "clazz", arg: 3, scope: !1260, file: !9, line: 4, type: !47)
!1264 = !DILocalVariable(name: "obj", arg: 2, scope: !1260, file: !9, line: 4, type: !48)
!1265 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1260, file: !9, line: 4, type: !25)
!1266 = !DILocalVariable(name: "args", scope: !1260, file: !9, line: 4, type: !149)
!1267 = !DILocalVariable(name: "sig", scope: !1260, file: !9, line: 4, type: !1104)
!1268 = !DILocalVariable(name: "argc", scope: !1260, file: !9, line: 4, type: !13)
!1269 = !DILocalVariable(name: "argv", scope: !1260, file: !9, line: 4, type: !1109)
!1270 = !DILocalVariable(name: "i", scope: !1271, file: !9, line: 4, type: !13)
!1271 = distinct !DILexicalBlock(scope: !1260, file: !9, line: 4)
!1272 = !DILocation(line: 4, scope: !1271)
!1273 = !DILocation(line: 4, scope: !1274)
!1274 = distinct !DILexicalBlock(scope: !1275, file: !9, line: 4)
!1275 = distinct !DILexicalBlock(scope: !1271, file: !9, line: 4)
!1276 = !DILocation(line: 4, scope: !1277)
!1277 = distinct !DILexicalBlock(scope: !1274, file: !9, line: 4)
!1278 = !DILocation(line: 4, scope: !1275)
!1279 = distinct !{!1279, !1272, !1272, !1120}
!1280 = !DILocalVariable(name: "ret", scope: !1260, file: !9, line: 4, type: !81)
!1281 = distinct !DISubprogram(name: "JNI_CallNonvirtualBooleanMethodV", scope: !9, file: !9, line: 4, type: !330, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1282 = !DILocalVariable(name: "args", arg: 5, scope: !1281, file: !9, line: 4, type: !149)
!1283 = !DILocation(line: 4, scope: !1281)
!1284 = !DILocalVariable(name: "methodID", arg: 4, scope: !1281, file: !9, line: 4, type: !67)
!1285 = !DILocalVariable(name: "clazz", arg: 3, scope: !1281, file: !9, line: 4, type: !47)
!1286 = !DILocalVariable(name: "obj", arg: 2, scope: !1281, file: !9, line: 4, type: !48)
!1287 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1281, file: !9, line: 4, type: !25)
!1288 = !DILocalVariable(name: "sig", scope: !1281, file: !9, line: 4, type: !1104)
!1289 = !DILocalVariable(name: "argc", scope: !1281, file: !9, line: 4, type: !13)
!1290 = !DILocalVariable(name: "argv", scope: !1281, file: !9, line: 4, type: !1109)
!1291 = !DILocalVariable(name: "i", scope: !1292, file: !9, line: 4, type: !13)
!1292 = distinct !DILexicalBlock(scope: !1281, file: !9, line: 4)
!1293 = !DILocation(line: 4, scope: !1292)
!1294 = !DILocation(line: 4, scope: !1295)
!1295 = distinct !DILexicalBlock(scope: !1296, file: !9, line: 4)
!1296 = distinct !DILexicalBlock(scope: !1292, file: !9, line: 4)
!1297 = !DILocation(line: 4, scope: !1298)
!1298 = distinct !DILexicalBlock(scope: !1295, file: !9, line: 4)
!1299 = !DILocation(line: 4, scope: !1296)
!1300 = distinct !{!1300, !1293, !1293, !1120}
!1301 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethod", scope: !9, file: !9, line: 4, type: !514, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1302 = !DILocalVariable(name: "methodID", arg: 3, scope: !1301, file: !9, line: 4, type: !67)
!1303 = !DILocation(line: 4, scope: !1301)
!1304 = !DILocalVariable(name: "clazz", arg: 2, scope: !1301, file: !9, line: 4, type: !47)
!1305 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1301, file: !9, line: 4, type: !25)
!1306 = !DILocalVariable(name: "args", scope: !1301, file: !9, line: 4, type: !149)
!1307 = !DILocalVariable(name: "sig", scope: !1301, file: !9, line: 4, type: !1104)
!1308 = !DILocalVariable(name: "argc", scope: !1301, file: !9, line: 4, type: !13)
!1309 = !DILocalVariable(name: "argv", scope: !1301, file: !9, line: 4, type: !1109)
!1310 = !DILocalVariable(name: "i", scope: !1311, file: !9, line: 4, type: !13)
!1311 = distinct !DILexicalBlock(scope: !1301, file: !9, line: 4)
!1312 = !DILocation(line: 4, scope: !1311)
!1313 = !DILocation(line: 4, scope: !1314)
!1314 = distinct !DILexicalBlock(scope: !1315, file: !9, line: 4)
!1315 = distinct !DILexicalBlock(scope: !1311, file: !9, line: 4)
!1316 = !DILocation(line: 4, scope: !1317)
!1317 = distinct !DILexicalBlock(scope: !1314, file: !9, line: 4)
!1318 = !DILocation(line: 4, scope: !1315)
!1319 = distinct !{!1319, !1312, !1312, !1120}
!1320 = !DILocalVariable(name: "ret", scope: !1301, file: !9, line: 4, type: !81)
!1321 = distinct !DISubprogram(name: "JNI_CallStaticBooleanMethodV", scope: !9, file: !9, line: 4, type: !518, scopeLine: 4, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1322 = !DILocalVariable(name: "args", arg: 4, scope: !1321, file: !9, line: 4, type: !149)
!1323 = !DILocation(line: 4, scope: !1321)
!1324 = !DILocalVariable(name: "methodID", arg: 3, scope: !1321, file: !9, line: 4, type: !67)
!1325 = !DILocalVariable(name: "clazz", arg: 2, scope: !1321, file: !9, line: 4, type: !47)
!1326 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1321, file: !9, line: 4, type: !25)
!1327 = !DILocalVariable(name: "sig", scope: !1321, file: !9, line: 4, type: !1104)
!1328 = !DILocalVariable(name: "argc", scope: !1321, file: !9, line: 4, type: !13)
!1329 = !DILocalVariable(name: "argv", scope: !1321, file: !9, line: 4, type: !1109)
!1330 = !DILocalVariable(name: "i", scope: !1331, file: !9, line: 4, type: !13)
!1331 = distinct !DILexicalBlock(scope: !1321, file: !9, line: 4)
!1332 = !DILocation(line: 4, scope: !1331)
!1333 = !DILocation(line: 4, scope: !1334)
!1334 = distinct !DILexicalBlock(scope: !1335, file: !9, line: 4)
!1335 = distinct !DILexicalBlock(scope: !1331, file: !9, line: 4)
!1336 = !DILocation(line: 4, scope: !1337)
!1337 = distinct !DILexicalBlock(scope: !1334, file: !9, line: 4)
!1338 = !DILocation(line: 4, scope: !1335)
!1339 = distinct !{!1339, !1332, !1332, !1120}
!1340 = distinct !DISubprogram(name: "JNI_CallByteMethod", scope: !9, file: !9, line: 5, type: !218, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1341 = !DILocalVariable(name: "methodID", arg: 3, scope: !1340, file: !9, line: 5, type: !67)
!1342 = !DILocation(line: 5, scope: !1340)
!1343 = !DILocalVariable(name: "obj", arg: 2, scope: !1340, file: !9, line: 5, type: !48)
!1344 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1340, file: !9, line: 5, type: !25)
!1345 = !DILocalVariable(name: "args", scope: !1340, file: !9, line: 5, type: !149)
!1346 = !DILocalVariable(name: "sig", scope: !1340, file: !9, line: 5, type: !1104)
!1347 = !DILocalVariable(name: "argc", scope: !1340, file: !9, line: 5, type: !13)
!1348 = !DILocalVariable(name: "argv", scope: !1340, file: !9, line: 5, type: !1109)
!1349 = !DILocalVariable(name: "i", scope: !1350, file: !9, line: 5, type: !13)
!1350 = distinct !DILexicalBlock(scope: !1340, file: !9, line: 5)
!1351 = !DILocation(line: 5, scope: !1350)
!1352 = !DILocation(line: 5, scope: !1353)
!1353 = distinct !DILexicalBlock(scope: !1354, file: !9, line: 5)
!1354 = distinct !DILexicalBlock(scope: !1350, file: !9, line: 5)
!1355 = !DILocation(line: 5, scope: !1356)
!1356 = distinct !DILexicalBlock(scope: !1353, file: !9, line: 5)
!1357 = !DILocation(line: 5, scope: !1354)
!1358 = distinct !{!1358, !1351, !1351, !1120}
!1359 = !DILocalVariable(name: "ret", scope: !1340, file: !9, line: 5, type: !56)
!1360 = distinct !DISubprogram(name: "JNI_CallByteMethodV", scope: !9, file: !9, line: 5, type: !222, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1361 = !DILocalVariable(name: "args", arg: 4, scope: !1360, file: !9, line: 5, type: !149)
!1362 = !DILocation(line: 5, scope: !1360)
!1363 = !DILocalVariable(name: "methodID", arg: 3, scope: !1360, file: !9, line: 5, type: !67)
!1364 = !DILocalVariable(name: "obj", arg: 2, scope: !1360, file: !9, line: 5, type: !48)
!1365 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1360, file: !9, line: 5, type: !25)
!1366 = !DILocalVariable(name: "sig", scope: !1360, file: !9, line: 5, type: !1104)
!1367 = !DILocalVariable(name: "argc", scope: !1360, file: !9, line: 5, type: !13)
!1368 = !DILocalVariable(name: "argv", scope: !1360, file: !9, line: 5, type: !1109)
!1369 = !DILocalVariable(name: "i", scope: !1370, file: !9, line: 5, type: !13)
!1370 = distinct !DILexicalBlock(scope: !1360, file: !9, line: 5)
!1371 = !DILocation(line: 5, scope: !1370)
!1372 = !DILocation(line: 5, scope: !1373)
!1373 = distinct !DILexicalBlock(scope: !1374, file: !9, line: 5)
!1374 = distinct !DILexicalBlock(scope: !1370, file: !9, line: 5)
!1375 = !DILocation(line: 5, scope: !1376)
!1376 = distinct !DILexicalBlock(scope: !1373, file: !9, line: 5)
!1377 = !DILocation(line: 5, scope: !1374)
!1378 = distinct !{!1378, !1371, !1371, !1120}
!1379 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethod", scope: !9, file: !9, line: 5, type: !338, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1380 = !DILocalVariable(name: "methodID", arg: 4, scope: !1379, file: !9, line: 5, type: !67)
!1381 = !DILocation(line: 5, scope: !1379)
!1382 = !DILocalVariable(name: "clazz", arg: 3, scope: !1379, file: !9, line: 5, type: !47)
!1383 = !DILocalVariable(name: "obj", arg: 2, scope: !1379, file: !9, line: 5, type: !48)
!1384 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1379, file: !9, line: 5, type: !25)
!1385 = !DILocalVariable(name: "args", scope: !1379, file: !9, line: 5, type: !149)
!1386 = !DILocalVariable(name: "sig", scope: !1379, file: !9, line: 5, type: !1104)
!1387 = !DILocalVariable(name: "argc", scope: !1379, file: !9, line: 5, type: !13)
!1388 = !DILocalVariable(name: "argv", scope: !1379, file: !9, line: 5, type: !1109)
!1389 = !DILocalVariable(name: "i", scope: !1390, file: !9, line: 5, type: !13)
!1390 = distinct !DILexicalBlock(scope: !1379, file: !9, line: 5)
!1391 = !DILocation(line: 5, scope: !1390)
!1392 = !DILocation(line: 5, scope: !1393)
!1393 = distinct !DILexicalBlock(scope: !1394, file: !9, line: 5)
!1394 = distinct !DILexicalBlock(scope: !1390, file: !9, line: 5)
!1395 = !DILocation(line: 5, scope: !1396)
!1396 = distinct !DILexicalBlock(scope: !1393, file: !9, line: 5)
!1397 = !DILocation(line: 5, scope: !1394)
!1398 = distinct !{!1398, !1391, !1391, !1120}
!1399 = !DILocalVariable(name: "ret", scope: !1379, file: !9, line: 5, type: !56)
!1400 = distinct !DISubprogram(name: "JNI_CallNonvirtualByteMethodV", scope: !9, file: !9, line: 5, type: !342, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1401 = !DILocalVariable(name: "args", arg: 5, scope: !1400, file: !9, line: 5, type: !149)
!1402 = !DILocation(line: 5, scope: !1400)
!1403 = !DILocalVariable(name: "methodID", arg: 4, scope: !1400, file: !9, line: 5, type: !67)
!1404 = !DILocalVariable(name: "clazz", arg: 3, scope: !1400, file: !9, line: 5, type: !47)
!1405 = !DILocalVariable(name: "obj", arg: 2, scope: !1400, file: !9, line: 5, type: !48)
!1406 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1400, file: !9, line: 5, type: !25)
!1407 = !DILocalVariable(name: "sig", scope: !1400, file: !9, line: 5, type: !1104)
!1408 = !DILocalVariable(name: "argc", scope: !1400, file: !9, line: 5, type: !13)
!1409 = !DILocalVariable(name: "argv", scope: !1400, file: !9, line: 5, type: !1109)
!1410 = !DILocalVariable(name: "i", scope: !1411, file: !9, line: 5, type: !13)
!1411 = distinct !DILexicalBlock(scope: !1400, file: !9, line: 5)
!1412 = !DILocation(line: 5, scope: !1411)
!1413 = !DILocation(line: 5, scope: !1414)
!1414 = distinct !DILexicalBlock(scope: !1415, file: !9, line: 5)
!1415 = distinct !DILexicalBlock(scope: !1411, file: !9, line: 5)
!1416 = !DILocation(line: 5, scope: !1417)
!1417 = distinct !DILexicalBlock(scope: !1414, file: !9, line: 5)
!1418 = !DILocation(line: 5, scope: !1415)
!1419 = distinct !{!1419, !1412, !1412, !1120}
!1420 = distinct !DISubprogram(name: "JNI_CallStaticByteMethod", scope: !9, file: !9, line: 5, type: !526, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1421 = !DILocalVariable(name: "methodID", arg: 3, scope: !1420, file: !9, line: 5, type: !67)
!1422 = !DILocation(line: 5, scope: !1420)
!1423 = !DILocalVariable(name: "clazz", arg: 2, scope: !1420, file: !9, line: 5, type: !47)
!1424 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1420, file: !9, line: 5, type: !25)
!1425 = !DILocalVariable(name: "args", scope: !1420, file: !9, line: 5, type: !149)
!1426 = !DILocalVariable(name: "sig", scope: !1420, file: !9, line: 5, type: !1104)
!1427 = !DILocalVariable(name: "argc", scope: !1420, file: !9, line: 5, type: !13)
!1428 = !DILocalVariable(name: "argv", scope: !1420, file: !9, line: 5, type: !1109)
!1429 = !DILocalVariable(name: "i", scope: !1430, file: !9, line: 5, type: !13)
!1430 = distinct !DILexicalBlock(scope: !1420, file: !9, line: 5)
!1431 = !DILocation(line: 5, scope: !1430)
!1432 = !DILocation(line: 5, scope: !1433)
!1433 = distinct !DILexicalBlock(scope: !1434, file: !9, line: 5)
!1434 = distinct !DILexicalBlock(scope: !1430, file: !9, line: 5)
!1435 = !DILocation(line: 5, scope: !1436)
!1436 = distinct !DILexicalBlock(scope: !1433, file: !9, line: 5)
!1437 = !DILocation(line: 5, scope: !1434)
!1438 = distinct !{!1438, !1431, !1431, !1120}
!1439 = !DILocalVariable(name: "ret", scope: !1420, file: !9, line: 5, type: !56)
!1440 = distinct !DISubprogram(name: "JNI_CallStaticByteMethodV", scope: !9, file: !9, line: 5, type: !530, scopeLine: 5, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1441 = !DILocalVariable(name: "args", arg: 4, scope: !1440, file: !9, line: 5, type: !149)
!1442 = !DILocation(line: 5, scope: !1440)
!1443 = !DILocalVariable(name: "methodID", arg: 3, scope: !1440, file: !9, line: 5, type: !67)
!1444 = !DILocalVariable(name: "clazz", arg: 2, scope: !1440, file: !9, line: 5, type: !47)
!1445 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1440, file: !9, line: 5, type: !25)
!1446 = !DILocalVariable(name: "sig", scope: !1440, file: !9, line: 5, type: !1104)
!1447 = !DILocalVariable(name: "argc", scope: !1440, file: !9, line: 5, type: !13)
!1448 = !DILocalVariable(name: "argv", scope: !1440, file: !9, line: 5, type: !1109)
!1449 = !DILocalVariable(name: "i", scope: !1450, file: !9, line: 5, type: !13)
!1450 = distinct !DILexicalBlock(scope: !1440, file: !9, line: 5)
!1451 = !DILocation(line: 5, scope: !1450)
!1452 = !DILocation(line: 5, scope: !1453)
!1453 = distinct !DILexicalBlock(scope: !1454, file: !9, line: 5)
!1454 = distinct !DILexicalBlock(scope: !1450, file: !9, line: 5)
!1455 = !DILocation(line: 5, scope: !1456)
!1456 = distinct !DILexicalBlock(scope: !1453, file: !9, line: 5)
!1457 = !DILocation(line: 5, scope: !1454)
!1458 = distinct !{!1458, !1451, !1451, !1120}
!1459 = distinct !DISubprogram(name: "JNI_CallCharMethod", scope: !9, file: !9, line: 6, type: !230, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1460 = !DILocalVariable(name: "methodID", arg: 3, scope: !1459, file: !9, line: 6, type: !67)
!1461 = !DILocation(line: 6, scope: !1459)
!1462 = !DILocalVariable(name: "obj", arg: 2, scope: !1459, file: !9, line: 6, type: !48)
!1463 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1459, file: !9, line: 6, type: !25)
!1464 = !DILocalVariable(name: "args", scope: !1459, file: !9, line: 6, type: !149)
!1465 = !DILocalVariable(name: "sig", scope: !1459, file: !9, line: 6, type: !1104)
!1466 = !DILocalVariable(name: "argc", scope: !1459, file: !9, line: 6, type: !13)
!1467 = !DILocalVariable(name: "argv", scope: !1459, file: !9, line: 6, type: !1109)
!1468 = !DILocalVariable(name: "i", scope: !1469, file: !9, line: 6, type: !13)
!1469 = distinct !DILexicalBlock(scope: !1459, file: !9, line: 6)
!1470 = !DILocation(line: 6, scope: !1469)
!1471 = !DILocation(line: 6, scope: !1472)
!1472 = distinct !DILexicalBlock(scope: !1473, file: !9, line: 6)
!1473 = distinct !DILexicalBlock(scope: !1469, file: !9, line: 6)
!1474 = !DILocation(line: 6, scope: !1475)
!1475 = distinct !DILexicalBlock(scope: !1472, file: !9, line: 6)
!1476 = !DILocation(line: 6, scope: !1473)
!1477 = distinct !{!1477, !1470, !1470, !1120}
!1478 = !DILocalVariable(name: "ret", scope: !1459, file: !9, line: 6, type: !164)
!1479 = distinct !DISubprogram(name: "JNI_CallCharMethodV", scope: !9, file: !9, line: 6, type: !234, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1480 = !DILocalVariable(name: "args", arg: 4, scope: !1479, file: !9, line: 6, type: !149)
!1481 = !DILocation(line: 6, scope: !1479)
!1482 = !DILocalVariable(name: "methodID", arg: 3, scope: !1479, file: !9, line: 6, type: !67)
!1483 = !DILocalVariable(name: "obj", arg: 2, scope: !1479, file: !9, line: 6, type: !48)
!1484 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1479, file: !9, line: 6, type: !25)
!1485 = !DILocalVariable(name: "sig", scope: !1479, file: !9, line: 6, type: !1104)
!1486 = !DILocalVariable(name: "argc", scope: !1479, file: !9, line: 6, type: !13)
!1487 = !DILocalVariable(name: "argv", scope: !1479, file: !9, line: 6, type: !1109)
!1488 = !DILocalVariable(name: "i", scope: !1489, file: !9, line: 6, type: !13)
!1489 = distinct !DILexicalBlock(scope: !1479, file: !9, line: 6)
!1490 = !DILocation(line: 6, scope: !1489)
!1491 = !DILocation(line: 6, scope: !1492)
!1492 = distinct !DILexicalBlock(scope: !1493, file: !9, line: 6)
!1493 = distinct !DILexicalBlock(scope: !1489, file: !9, line: 6)
!1494 = !DILocation(line: 6, scope: !1495)
!1495 = distinct !DILexicalBlock(scope: !1492, file: !9, line: 6)
!1496 = !DILocation(line: 6, scope: !1493)
!1497 = distinct !{!1497, !1490, !1490, !1120}
!1498 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethod", scope: !9, file: !9, line: 6, type: !350, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1499 = !DILocalVariable(name: "methodID", arg: 4, scope: !1498, file: !9, line: 6, type: !67)
!1500 = !DILocation(line: 6, scope: !1498)
!1501 = !DILocalVariable(name: "clazz", arg: 3, scope: !1498, file: !9, line: 6, type: !47)
!1502 = !DILocalVariable(name: "obj", arg: 2, scope: !1498, file: !9, line: 6, type: !48)
!1503 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1498, file: !9, line: 6, type: !25)
!1504 = !DILocalVariable(name: "args", scope: !1498, file: !9, line: 6, type: !149)
!1505 = !DILocalVariable(name: "sig", scope: !1498, file: !9, line: 6, type: !1104)
!1506 = !DILocalVariable(name: "argc", scope: !1498, file: !9, line: 6, type: !13)
!1507 = !DILocalVariable(name: "argv", scope: !1498, file: !9, line: 6, type: !1109)
!1508 = !DILocalVariable(name: "i", scope: !1509, file: !9, line: 6, type: !13)
!1509 = distinct !DILexicalBlock(scope: !1498, file: !9, line: 6)
!1510 = !DILocation(line: 6, scope: !1509)
!1511 = !DILocation(line: 6, scope: !1512)
!1512 = distinct !DILexicalBlock(scope: !1513, file: !9, line: 6)
!1513 = distinct !DILexicalBlock(scope: !1509, file: !9, line: 6)
!1514 = !DILocation(line: 6, scope: !1515)
!1515 = distinct !DILexicalBlock(scope: !1512, file: !9, line: 6)
!1516 = !DILocation(line: 6, scope: !1513)
!1517 = distinct !{!1517, !1510, !1510, !1120}
!1518 = !DILocalVariable(name: "ret", scope: !1498, file: !9, line: 6, type: !164)
!1519 = distinct !DISubprogram(name: "JNI_CallNonvirtualCharMethodV", scope: !9, file: !9, line: 6, type: !354, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1520 = !DILocalVariable(name: "args", arg: 5, scope: !1519, file: !9, line: 6, type: !149)
!1521 = !DILocation(line: 6, scope: !1519)
!1522 = !DILocalVariable(name: "methodID", arg: 4, scope: !1519, file: !9, line: 6, type: !67)
!1523 = !DILocalVariable(name: "clazz", arg: 3, scope: !1519, file: !9, line: 6, type: !47)
!1524 = !DILocalVariable(name: "obj", arg: 2, scope: !1519, file: !9, line: 6, type: !48)
!1525 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1519, file: !9, line: 6, type: !25)
!1526 = !DILocalVariable(name: "sig", scope: !1519, file: !9, line: 6, type: !1104)
!1527 = !DILocalVariable(name: "argc", scope: !1519, file: !9, line: 6, type: !13)
!1528 = !DILocalVariable(name: "argv", scope: !1519, file: !9, line: 6, type: !1109)
!1529 = !DILocalVariable(name: "i", scope: !1530, file: !9, line: 6, type: !13)
!1530 = distinct !DILexicalBlock(scope: !1519, file: !9, line: 6)
!1531 = !DILocation(line: 6, scope: !1530)
!1532 = !DILocation(line: 6, scope: !1533)
!1533 = distinct !DILexicalBlock(scope: !1534, file: !9, line: 6)
!1534 = distinct !DILexicalBlock(scope: !1530, file: !9, line: 6)
!1535 = !DILocation(line: 6, scope: !1536)
!1536 = distinct !DILexicalBlock(scope: !1533, file: !9, line: 6)
!1537 = !DILocation(line: 6, scope: !1534)
!1538 = distinct !{!1538, !1531, !1531, !1120}
!1539 = distinct !DISubprogram(name: "JNI_CallStaticCharMethod", scope: !9, file: !9, line: 6, type: !538, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1540 = !DILocalVariable(name: "methodID", arg: 3, scope: !1539, file: !9, line: 6, type: !67)
!1541 = !DILocation(line: 6, scope: !1539)
!1542 = !DILocalVariable(name: "clazz", arg: 2, scope: !1539, file: !9, line: 6, type: !47)
!1543 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1539, file: !9, line: 6, type: !25)
!1544 = !DILocalVariable(name: "args", scope: !1539, file: !9, line: 6, type: !149)
!1545 = !DILocalVariable(name: "sig", scope: !1539, file: !9, line: 6, type: !1104)
!1546 = !DILocalVariable(name: "argc", scope: !1539, file: !9, line: 6, type: !13)
!1547 = !DILocalVariable(name: "argv", scope: !1539, file: !9, line: 6, type: !1109)
!1548 = !DILocalVariable(name: "i", scope: !1549, file: !9, line: 6, type: !13)
!1549 = distinct !DILexicalBlock(scope: !1539, file: !9, line: 6)
!1550 = !DILocation(line: 6, scope: !1549)
!1551 = !DILocation(line: 6, scope: !1552)
!1552 = distinct !DILexicalBlock(scope: !1553, file: !9, line: 6)
!1553 = distinct !DILexicalBlock(scope: !1549, file: !9, line: 6)
!1554 = !DILocation(line: 6, scope: !1555)
!1555 = distinct !DILexicalBlock(scope: !1552, file: !9, line: 6)
!1556 = !DILocation(line: 6, scope: !1553)
!1557 = distinct !{!1557, !1550, !1550, !1120}
!1558 = !DILocalVariable(name: "ret", scope: !1539, file: !9, line: 6, type: !164)
!1559 = distinct !DISubprogram(name: "JNI_CallStaticCharMethodV", scope: !9, file: !9, line: 6, type: !542, scopeLine: 6, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1560 = !DILocalVariable(name: "args", arg: 4, scope: !1559, file: !9, line: 6, type: !149)
!1561 = !DILocation(line: 6, scope: !1559)
!1562 = !DILocalVariable(name: "methodID", arg: 3, scope: !1559, file: !9, line: 6, type: !67)
!1563 = !DILocalVariable(name: "clazz", arg: 2, scope: !1559, file: !9, line: 6, type: !47)
!1564 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1559, file: !9, line: 6, type: !25)
!1565 = !DILocalVariable(name: "sig", scope: !1559, file: !9, line: 6, type: !1104)
!1566 = !DILocalVariable(name: "argc", scope: !1559, file: !9, line: 6, type: !13)
!1567 = !DILocalVariable(name: "argv", scope: !1559, file: !9, line: 6, type: !1109)
!1568 = !DILocalVariable(name: "i", scope: !1569, file: !9, line: 6, type: !13)
!1569 = distinct !DILexicalBlock(scope: !1559, file: !9, line: 6)
!1570 = !DILocation(line: 6, scope: !1569)
!1571 = !DILocation(line: 6, scope: !1572)
!1572 = distinct !DILexicalBlock(scope: !1573, file: !9, line: 6)
!1573 = distinct !DILexicalBlock(scope: !1569, file: !9, line: 6)
!1574 = !DILocation(line: 6, scope: !1575)
!1575 = distinct !DILexicalBlock(scope: !1572, file: !9, line: 6)
!1576 = !DILocation(line: 6, scope: !1573)
!1577 = distinct !{!1577, !1570, !1570, !1120}
!1578 = distinct !DISubprogram(name: "JNI_CallShortMethod", scope: !9, file: !9, line: 7, type: !242, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1579 = !DILocalVariable(name: "methodID", arg: 3, scope: !1578, file: !9, line: 7, type: !67)
!1580 = !DILocation(line: 7, scope: !1578)
!1581 = !DILocalVariable(name: "obj", arg: 2, scope: !1578, file: !9, line: 7, type: !48)
!1582 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1578, file: !9, line: 7, type: !25)
!1583 = !DILocalVariable(name: "args", scope: !1578, file: !9, line: 7, type: !149)
!1584 = !DILocalVariable(name: "sig", scope: !1578, file: !9, line: 7, type: !1104)
!1585 = !DILocalVariable(name: "argc", scope: !1578, file: !9, line: 7, type: !13)
!1586 = !DILocalVariable(name: "argv", scope: !1578, file: !9, line: 7, type: !1109)
!1587 = !DILocalVariable(name: "i", scope: !1588, file: !9, line: 7, type: !13)
!1588 = distinct !DILexicalBlock(scope: !1578, file: !9, line: 7)
!1589 = !DILocation(line: 7, scope: !1588)
!1590 = !DILocation(line: 7, scope: !1591)
!1591 = distinct !DILexicalBlock(scope: !1592, file: !9, line: 7)
!1592 = distinct !DILexicalBlock(scope: !1588, file: !9, line: 7)
!1593 = !DILocation(line: 7, scope: !1594)
!1594 = distinct !DILexicalBlock(scope: !1591, file: !9, line: 7)
!1595 = !DILocation(line: 7, scope: !1592)
!1596 = distinct !{!1596, !1589, !1589, !1120}
!1597 = !DILocalVariable(name: "ret", scope: !1578, file: !9, line: 7, type: !167)
!1598 = distinct !DISubprogram(name: "JNI_CallShortMethodV", scope: !9, file: !9, line: 7, type: !246, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1599 = !DILocalVariable(name: "args", arg: 4, scope: !1598, file: !9, line: 7, type: !149)
!1600 = !DILocation(line: 7, scope: !1598)
!1601 = !DILocalVariable(name: "methodID", arg: 3, scope: !1598, file: !9, line: 7, type: !67)
!1602 = !DILocalVariable(name: "obj", arg: 2, scope: !1598, file: !9, line: 7, type: !48)
!1603 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1598, file: !9, line: 7, type: !25)
!1604 = !DILocalVariable(name: "sig", scope: !1598, file: !9, line: 7, type: !1104)
!1605 = !DILocalVariable(name: "argc", scope: !1598, file: !9, line: 7, type: !13)
!1606 = !DILocalVariable(name: "argv", scope: !1598, file: !9, line: 7, type: !1109)
!1607 = !DILocalVariable(name: "i", scope: !1608, file: !9, line: 7, type: !13)
!1608 = distinct !DILexicalBlock(scope: !1598, file: !9, line: 7)
!1609 = !DILocation(line: 7, scope: !1608)
!1610 = !DILocation(line: 7, scope: !1611)
!1611 = distinct !DILexicalBlock(scope: !1612, file: !9, line: 7)
!1612 = distinct !DILexicalBlock(scope: !1608, file: !9, line: 7)
!1613 = !DILocation(line: 7, scope: !1614)
!1614 = distinct !DILexicalBlock(scope: !1611, file: !9, line: 7)
!1615 = !DILocation(line: 7, scope: !1612)
!1616 = distinct !{!1616, !1609, !1609, !1120}
!1617 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethod", scope: !9, file: !9, line: 7, type: !362, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1618 = !DILocalVariable(name: "methodID", arg: 4, scope: !1617, file: !9, line: 7, type: !67)
!1619 = !DILocation(line: 7, scope: !1617)
!1620 = !DILocalVariable(name: "clazz", arg: 3, scope: !1617, file: !9, line: 7, type: !47)
!1621 = !DILocalVariable(name: "obj", arg: 2, scope: !1617, file: !9, line: 7, type: !48)
!1622 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1617, file: !9, line: 7, type: !25)
!1623 = !DILocalVariable(name: "args", scope: !1617, file: !9, line: 7, type: !149)
!1624 = !DILocalVariable(name: "sig", scope: !1617, file: !9, line: 7, type: !1104)
!1625 = !DILocalVariable(name: "argc", scope: !1617, file: !9, line: 7, type: !13)
!1626 = !DILocalVariable(name: "argv", scope: !1617, file: !9, line: 7, type: !1109)
!1627 = !DILocalVariable(name: "i", scope: !1628, file: !9, line: 7, type: !13)
!1628 = distinct !DILexicalBlock(scope: !1617, file: !9, line: 7)
!1629 = !DILocation(line: 7, scope: !1628)
!1630 = !DILocation(line: 7, scope: !1631)
!1631 = distinct !DILexicalBlock(scope: !1632, file: !9, line: 7)
!1632 = distinct !DILexicalBlock(scope: !1628, file: !9, line: 7)
!1633 = !DILocation(line: 7, scope: !1634)
!1634 = distinct !DILexicalBlock(scope: !1631, file: !9, line: 7)
!1635 = !DILocation(line: 7, scope: !1632)
!1636 = distinct !{!1636, !1629, !1629, !1120}
!1637 = !DILocalVariable(name: "ret", scope: !1617, file: !9, line: 7, type: !167)
!1638 = distinct !DISubprogram(name: "JNI_CallNonvirtualShortMethodV", scope: !9, file: !9, line: 7, type: !366, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1639 = !DILocalVariable(name: "args", arg: 5, scope: !1638, file: !9, line: 7, type: !149)
!1640 = !DILocation(line: 7, scope: !1638)
!1641 = !DILocalVariable(name: "methodID", arg: 4, scope: !1638, file: !9, line: 7, type: !67)
!1642 = !DILocalVariable(name: "clazz", arg: 3, scope: !1638, file: !9, line: 7, type: !47)
!1643 = !DILocalVariable(name: "obj", arg: 2, scope: !1638, file: !9, line: 7, type: !48)
!1644 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1638, file: !9, line: 7, type: !25)
!1645 = !DILocalVariable(name: "sig", scope: !1638, file: !9, line: 7, type: !1104)
!1646 = !DILocalVariable(name: "argc", scope: !1638, file: !9, line: 7, type: !13)
!1647 = !DILocalVariable(name: "argv", scope: !1638, file: !9, line: 7, type: !1109)
!1648 = !DILocalVariable(name: "i", scope: !1649, file: !9, line: 7, type: !13)
!1649 = distinct !DILexicalBlock(scope: !1638, file: !9, line: 7)
!1650 = !DILocation(line: 7, scope: !1649)
!1651 = !DILocation(line: 7, scope: !1652)
!1652 = distinct !DILexicalBlock(scope: !1653, file: !9, line: 7)
!1653 = distinct !DILexicalBlock(scope: !1649, file: !9, line: 7)
!1654 = !DILocation(line: 7, scope: !1655)
!1655 = distinct !DILexicalBlock(scope: !1652, file: !9, line: 7)
!1656 = !DILocation(line: 7, scope: !1653)
!1657 = distinct !{!1657, !1650, !1650, !1120}
!1658 = distinct !DISubprogram(name: "JNI_CallStaticShortMethod", scope: !9, file: !9, line: 7, type: !550, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1659 = !DILocalVariable(name: "methodID", arg: 3, scope: !1658, file: !9, line: 7, type: !67)
!1660 = !DILocation(line: 7, scope: !1658)
!1661 = !DILocalVariable(name: "clazz", arg: 2, scope: !1658, file: !9, line: 7, type: !47)
!1662 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1658, file: !9, line: 7, type: !25)
!1663 = !DILocalVariable(name: "args", scope: !1658, file: !9, line: 7, type: !149)
!1664 = !DILocalVariable(name: "sig", scope: !1658, file: !9, line: 7, type: !1104)
!1665 = !DILocalVariable(name: "argc", scope: !1658, file: !9, line: 7, type: !13)
!1666 = !DILocalVariable(name: "argv", scope: !1658, file: !9, line: 7, type: !1109)
!1667 = !DILocalVariable(name: "i", scope: !1668, file: !9, line: 7, type: !13)
!1668 = distinct !DILexicalBlock(scope: !1658, file: !9, line: 7)
!1669 = !DILocation(line: 7, scope: !1668)
!1670 = !DILocation(line: 7, scope: !1671)
!1671 = distinct !DILexicalBlock(scope: !1672, file: !9, line: 7)
!1672 = distinct !DILexicalBlock(scope: !1668, file: !9, line: 7)
!1673 = !DILocation(line: 7, scope: !1674)
!1674 = distinct !DILexicalBlock(scope: !1671, file: !9, line: 7)
!1675 = !DILocation(line: 7, scope: !1672)
!1676 = distinct !{!1676, !1669, !1669, !1120}
!1677 = !DILocalVariable(name: "ret", scope: !1658, file: !9, line: 7, type: !167)
!1678 = distinct !DISubprogram(name: "JNI_CallStaticShortMethodV", scope: !9, file: !9, line: 7, type: !554, scopeLine: 7, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1679 = !DILocalVariable(name: "args", arg: 4, scope: !1678, file: !9, line: 7, type: !149)
!1680 = !DILocation(line: 7, scope: !1678)
!1681 = !DILocalVariable(name: "methodID", arg: 3, scope: !1678, file: !9, line: 7, type: !67)
!1682 = !DILocalVariable(name: "clazz", arg: 2, scope: !1678, file: !9, line: 7, type: !47)
!1683 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1678, file: !9, line: 7, type: !25)
!1684 = !DILocalVariable(name: "sig", scope: !1678, file: !9, line: 7, type: !1104)
!1685 = !DILocalVariable(name: "argc", scope: !1678, file: !9, line: 7, type: !13)
!1686 = !DILocalVariable(name: "argv", scope: !1678, file: !9, line: 7, type: !1109)
!1687 = !DILocalVariable(name: "i", scope: !1688, file: !9, line: 7, type: !13)
!1688 = distinct !DILexicalBlock(scope: !1678, file: !9, line: 7)
!1689 = !DILocation(line: 7, scope: !1688)
!1690 = !DILocation(line: 7, scope: !1691)
!1691 = distinct !DILexicalBlock(scope: !1692, file: !9, line: 7)
!1692 = distinct !DILexicalBlock(scope: !1688, file: !9, line: 7)
!1693 = !DILocation(line: 7, scope: !1694)
!1694 = distinct !DILexicalBlock(scope: !1691, file: !9, line: 7)
!1695 = !DILocation(line: 7, scope: !1692)
!1696 = distinct !{!1696, !1689, !1689, !1120}
!1697 = distinct !DISubprogram(name: "JNI_CallIntMethod", scope: !9, file: !9, line: 8, type: !254, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1698 = !DILocalVariable(name: "methodID", arg: 3, scope: !1697, file: !9, line: 8, type: !67)
!1699 = !DILocation(line: 8, scope: !1697)
!1700 = !DILocalVariable(name: "obj", arg: 2, scope: !1697, file: !9, line: 8, type: !48)
!1701 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1697, file: !9, line: 8, type: !25)
!1702 = !DILocalVariable(name: "args", scope: !1697, file: !9, line: 8, type: !149)
!1703 = !DILocalVariable(name: "sig", scope: !1697, file: !9, line: 8, type: !1104)
!1704 = !DILocalVariable(name: "argc", scope: !1697, file: !9, line: 8, type: !13)
!1705 = !DILocalVariable(name: "argv", scope: !1697, file: !9, line: 8, type: !1109)
!1706 = !DILocalVariable(name: "i", scope: !1707, file: !9, line: 8, type: !13)
!1707 = distinct !DILexicalBlock(scope: !1697, file: !9, line: 8)
!1708 = !DILocation(line: 8, scope: !1707)
!1709 = !DILocation(line: 8, scope: !1710)
!1710 = distinct !DILexicalBlock(scope: !1711, file: !9, line: 8)
!1711 = distinct !DILexicalBlock(scope: !1707, file: !9, line: 8)
!1712 = !DILocation(line: 8, scope: !1713)
!1713 = distinct !DILexicalBlock(scope: !1710, file: !9, line: 8)
!1714 = !DILocation(line: 8, scope: !1711)
!1715 = distinct !{!1715, !1708, !1708, !1120}
!1716 = !DILocalVariable(name: "ret", scope: !1697, file: !9, line: 8, type: !40)
!1717 = distinct !DISubprogram(name: "JNI_CallIntMethodV", scope: !9, file: !9, line: 8, type: !258, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1718 = !DILocalVariable(name: "args", arg: 4, scope: !1717, file: !9, line: 8, type: !149)
!1719 = !DILocation(line: 8, scope: !1717)
!1720 = !DILocalVariable(name: "methodID", arg: 3, scope: !1717, file: !9, line: 8, type: !67)
!1721 = !DILocalVariable(name: "obj", arg: 2, scope: !1717, file: !9, line: 8, type: !48)
!1722 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1717, file: !9, line: 8, type: !25)
!1723 = !DILocalVariable(name: "sig", scope: !1717, file: !9, line: 8, type: !1104)
!1724 = !DILocalVariable(name: "argc", scope: !1717, file: !9, line: 8, type: !13)
!1725 = !DILocalVariable(name: "argv", scope: !1717, file: !9, line: 8, type: !1109)
!1726 = !DILocalVariable(name: "i", scope: !1727, file: !9, line: 8, type: !13)
!1727 = distinct !DILexicalBlock(scope: !1717, file: !9, line: 8)
!1728 = !DILocation(line: 8, scope: !1727)
!1729 = !DILocation(line: 8, scope: !1730)
!1730 = distinct !DILexicalBlock(scope: !1731, file: !9, line: 8)
!1731 = distinct !DILexicalBlock(scope: !1727, file: !9, line: 8)
!1732 = !DILocation(line: 8, scope: !1733)
!1733 = distinct !DILexicalBlock(scope: !1730, file: !9, line: 8)
!1734 = !DILocation(line: 8, scope: !1731)
!1735 = distinct !{!1735, !1728, !1728, !1120}
!1736 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethod", scope: !9, file: !9, line: 8, type: !374, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1737 = !DILocalVariable(name: "methodID", arg: 4, scope: !1736, file: !9, line: 8, type: !67)
!1738 = !DILocation(line: 8, scope: !1736)
!1739 = !DILocalVariable(name: "clazz", arg: 3, scope: !1736, file: !9, line: 8, type: !47)
!1740 = !DILocalVariable(name: "obj", arg: 2, scope: !1736, file: !9, line: 8, type: !48)
!1741 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1736, file: !9, line: 8, type: !25)
!1742 = !DILocalVariable(name: "args", scope: !1736, file: !9, line: 8, type: !149)
!1743 = !DILocalVariable(name: "sig", scope: !1736, file: !9, line: 8, type: !1104)
!1744 = !DILocalVariable(name: "argc", scope: !1736, file: !9, line: 8, type: !13)
!1745 = !DILocalVariable(name: "argv", scope: !1736, file: !9, line: 8, type: !1109)
!1746 = !DILocalVariable(name: "i", scope: !1747, file: !9, line: 8, type: !13)
!1747 = distinct !DILexicalBlock(scope: !1736, file: !9, line: 8)
!1748 = !DILocation(line: 8, scope: !1747)
!1749 = !DILocation(line: 8, scope: !1750)
!1750 = distinct !DILexicalBlock(scope: !1751, file: !9, line: 8)
!1751 = distinct !DILexicalBlock(scope: !1747, file: !9, line: 8)
!1752 = !DILocation(line: 8, scope: !1753)
!1753 = distinct !DILexicalBlock(scope: !1750, file: !9, line: 8)
!1754 = !DILocation(line: 8, scope: !1751)
!1755 = distinct !{!1755, !1748, !1748, !1120}
!1756 = !DILocalVariable(name: "ret", scope: !1736, file: !9, line: 8, type: !40)
!1757 = distinct !DISubprogram(name: "JNI_CallNonvirtualIntMethodV", scope: !9, file: !9, line: 8, type: !378, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1758 = !DILocalVariable(name: "args", arg: 5, scope: !1757, file: !9, line: 8, type: !149)
!1759 = !DILocation(line: 8, scope: !1757)
!1760 = !DILocalVariable(name: "methodID", arg: 4, scope: !1757, file: !9, line: 8, type: !67)
!1761 = !DILocalVariable(name: "clazz", arg: 3, scope: !1757, file: !9, line: 8, type: !47)
!1762 = !DILocalVariable(name: "obj", arg: 2, scope: !1757, file: !9, line: 8, type: !48)
!1763 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1757, file: !9, line: 8, type: !25)
!1764 = !DILocalVariable(name: "sig", scope: !1757, file: !9, line: 8, type: !1104)
!1765 = !DILocalVariable(name: "argc", scope: !1757, file: !9, line: 8, type: !13)
!1766 = !DILocalVariable(name: "argv", scope: !1757, file: !9, line: 8, type: !1109)
!1767 = !DILocalVariable(name: "i", scope: !1768, file: !9, line: 8, type: !13)
!1768 = distinct !DILexicalBlock(scope: !1757, file: !9, line: 8)
!1769 = !DILocation(line: 8, scope: !1768)
!1770 = !DILocation(line: 8, scope: !1771)
!1771 = distinct !DILexicalBlock(scope: !1772, file: !9, line: 8)
!1772 = distinct !DILexicalBlock(scope: !1768, file: !9, line: 8)
!1773 = !DILocation(line: 8, scope: !1774)
!1774 = distinct !DILexicalBlock(scope: !1771, file: !9, line: 8)
!1775 = !DILocation(line: 8, scope: !1772)
!1776 = distinct !{!1776, !1769, !1769, !1120}
!1777 = distinct !DISubprogram(name: "JNI_CallStaticIntMethod", scope: !9, file: !9, line: 8, type: !562, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1778 = !DILocalVariable(name: "methodID", arg: 3, scope: !1777, file: !9, line: 8, type: !67)
!1779 = !DILocation(line: 8, scope: !1777)
!1780 = !DILocalVariable(name: "clazz", arg: 2, scope: !1777, file: !9, line: 8, type: !47)
!1781 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1777, file: !9, line: 8, type: !25)
!1782 = !DILocalVariable(name: "args", scope: !1777, file: !9, line: 8, type: !149)
!1783 = !DILocalVariable(name: "sig", scope: !1777, file: !9, line: 8, type: !1104)
!1784 = !DILocalVariable(name: "argc", scope: !1777, file: !9, line: 8, type: !13)
!1785 = !DILocalVariable(name: "argv", scope: !1777, file: !9, line: 8, type: !1109)
!1786 = !DILocalVariable(name: "i", scope: !1787, file: !9, line: 8, type: !13)
!1787 = distinct !DILexicalBlock(scope: !1777, file: !9, line: 8)
!1788 = !DILocation(line: 8, scope: !1787)
!1789 = !DILocation(line: 8, scope: !1790)
!1790 = distinct !DILexicalBlock(scope: !1791, file: !9, line: 8)
!1791 = distinct !DILexicalBlock(scope: !1787, file: !9, line: 8)
!1792 = !DILocation(line: 8, scope: !1793)
!1793 = distinct !DILexicalBlock(scope: !1790, file: !9, line: 8)
!1794 = !DILocation(line: 8, scope: !1791)
!1795 = distinct !{!1795, !1788, !1788, !1120}
!1796 = !DILocalVariable(name: "ret", scope: !1777, file: !9, line: 8, type: !40)
!1797 = distinct !DISubprogram(name: "JNI_CallStaticIntMethodV", scope: !9, file: !9, line: 8, type: !566, scopeLine: 8, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1798 = !DILocalVariable(name: "args", arg: 4, scope: !1797, file: !9, line: 8, type: !149)
!1799 = !DILocation(line: 8, scope: !1797)
!1800 = !DILocalVariable(name: "methodID", arg: 3, scope: !1797, file: !9, line: 8, type: !67)
!1801 = !DILocalVariable(name: "clazz", arg: 2, scope: !1797, file: !9, line: 8, type: !47)
!1802 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1797, file: !9, line: 8, type: !25)
!1803 = !DILocalVariable(name: "sig", scope: !1797, file: !9, line: 8, type: !1104)
!1804 = !DILocalVariable(name: "argc", scope: !1797, file: !9, line: 8, type: !13)
!1805 = !DILocalVariable(name: "argv", scope: !1797, file: !9, line: 8, type: !1109)
!1806 = !DILocalVariable(name: "i", scope: !1807, file: !9, line: 8, type: !13)
!1807 = distinct !DILexicalBlock(scope: !1797, file: !9, line: 8)
!1808 = !DILocation(line: 8, scope: !1807)
!1809 = !DILocation(line: 8, scope: !1810)
!1810 = distinct !DILexicalBlock(scope: !1811, file: !9, line: 8)
!1811 = distinct !DILexicalBlock(scope: !1807, file: !9, line: 8)
!1812 = !DILocation(line: 8, scope: !1813)
!1813 = distinct !DILexicalBlock(scope: !1810, file: !9, line: 8)
!1814 = !DILocation(line: 8, scope: !1811)
!1815 = distinct !{!1815, !1808, !1808, !1120}
!1816 = distinct !DISubprogram(name: "JNI_CallLongMethod", scope: !9, file: !9, line: 9, type: !266, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1817 = !DILocalVariable(name: "methodID", arg: 3, scope: !1816, file: !9, line: 9, type: !67)
!1818 = !DILocation(line: 9, scope: !1816)
!1819 = !DILocalVariable(name: "obj", arg: 2, scope: !1816, file: !9, line: 9, type: !48)
!1820 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1816, file: !9, line: 9, type: !25)
!1821 = !DILocalVariable(name: "args", scope: !1816, file: !9, line: 9, type: !149)
!1822 = !DILocalVariable(name: "sig", scope: !1816, file: !9, line: 9, type: !1104)
!1823 = !DILocalVariable(name: "argc", scope: !1816, file: !9, line: 9, type: !13)
!1824 = !DILocalVariable(name: "argv", scope: !1816, file: !9, line: 9, type: !1109)
!1825 = !DILocalVariable(name: "i", scope: !1826, file: !9, line: 9, type: !13)
!1826 = distinct !DILexicalBlock(scope: !1816, file: !9, line: 9)
!1827 = !DILocation(line: 9, scope: !1826)
!1828 = !DILocation(line: 9, scope: !1829)
!1829 = distinct !DILexicalBlock(scope: !1830, file: !9, line: 9)
!1830 = distinct !DILexicalBlock(scope: !1826, file: !9, line: 9)
!1831 = !DILocation(line: 9, scope: !1832)
!1832 = distinct !DILexicalBlock(scope: !1829, file: !9, line: 9)
!1833 = !DILocation(line: 9, scope: !1830)
!1834 = distinct !{!1834, !1827, !1827, !1120}
!1835 = !DILocalVariable(name: "ret", scope: !1816, file: !9, line: 9, type: !171)
!1836 = distinct !DISubprogram(name: "JNI_CallLongMethodV", scope: !9, file: !9, line: 9, type: !270, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1837 = !DILocalVariable(name: "args", arg: 4, scope: !1836, file: !9, line: 9, type: !149)
!1838 = !DILocation(line: 9, scope: !1836)
!1839 = !DILocalVariable(name: "methodID", arg: 3, scope: !1836, file: !9, line: 9, type: !67)
!1840 = !DILocalVariable(name: "obj", arg: 2, scope: !1836, file: !9, line: 9, type: !48)
!1841 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1836, file: !9, line: 9, type: !25)
!1842 = !DILocalVariable(name: "sig", scope: !1836, file: !9, line: 9, type: !1104)
!1843 = !DILocalVariable(name: "argc", scope: !1836, file: !9, line: 9, type: !13)
!1844 = !DILocalVariable(name: "argv", scope: !1836, file: !9, line: 9, type: !1109)
!1845 = !DILocalVariable(name: "i", scope: !1846, file: !9, line: 9, type: !13)
!1846 = distinct !DILexicalBlock(scope: !1836, file: !9, line: 9)
!1847 = !DILocation(line: 9, scope: !1846)
!1848 = !DILocation(line: 9, scope: !1849)
!1849 = distinct !DILexicalBlock(scope: !1850, file: !9, line: 9)
!1850 = distinct !DILexicalBlock(scope: !1846, file: !9, line: 9)
!1851 = !DILocation(line: 9, scope: !1852)
!1852 = distinct !DILexicalBlock(scope: !1849, file: !9, line: 9)
!1853 = !DILocation(line: 9, scope: !1850)
!1854 = distinct !{!1854, !1847, !1847, !1120}
!1855 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethod", scope: !9, file: !9, line: 9, type: !386, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1856 = !DILocalVariable(name: "methodID", arg: 4, scope: !1855, file: !9, line: 9, type: !67)
!1857 = !DILocation(line: 9, scope: !1855)
!1858 = !DILocalVariable(name: "clazz", arg: 3, scope: !1855, file: !9, line: 9, type: !47)
!1859 = !DILocalVariable(name: "obj", arg: 2, scope: !1855, file: !9, line: 9, type: !48)
!1860 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1855, file: !9, line: 9, type: !25)
!1861 = !DILocalVariable(name: "args", scope: !1855, file: !9, line: 9, type: !149)
!1862 = !DILocalVariable(name: "sig", scope: !1855, file: !9, line: 9, type: !1104)
!1863 = !DILocalVariable(name: "argc", scope: !1855, file: !9, line: 9, type: !13)
!1864 = !DILocalVariable(name: "argv", scope: !1855, file: !9, line: 9, type: !1109)
!1865 = !DILocalVariable(name: "i", scope: !1866, file: !9, line: 9, type: !13)
!1866 = distinct !DILexicalBlock(scope: !1855, file: !9, line: 9)
!1867 = !DILocation(line: 9, scope: !1866)
!1868 = !DILocation(line: 9, scope: !1869)
!1869 = distinct !DILexicalBlock(scope: !1870, file: !9, line: 9)
!1870 = distinct !DILexicalBlock(scope: !1866, file: !9, line: 9)
!1871 = !DILocation(line: 9, scope: !1872)
!1872 = distinct !DILexicalBlock(scope: !1869, file: !9, line: 9)
!1873 = !DILocation(line: 9, scope: !1870)
!1874 = distinct !{!1874, !1867, !1867, !1120}
!1875 = !DILocalVariable(name: "ret", scope: !1855, file: !9, line: 9, type: !171)
!1876 = distinct !DISubprogram(name: "JNI_CallNonvirtualLongMethodV", scope: !9, file: !9, line: 9, type: !390, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1877 = !DILocalVariable(name: "args", arg: 5, scope: !1876, file: !9, line: 9, type: !149)
!1878 = !DILocation(line: 9, scope: !1876)
!1879 = !DILocalVariable(name: "methodID", arg: 4, scope: !1876, file: !9, line: 9, type: !67)
!1880 = !DILocalVariable(name: "clazz", arg: 3, scope: !1876, file: !9, line: 9, type: !47)
!1881 = !DILocalVariable(name: "obj", arg: 2, scope: !1876, file: !9, line: 9, type: !48)
!1882 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1876, file: !9, line: 9, type: !25)
!1883 = !DILocalVariable(name: "sig", scope: !1876, file: !9, line: 9, type: !1104)
!1884 = !DILocalVariable(name: "argc", scope: !1876, file: !9, line: 9, type: !13)
!1885 = !DILocalVariable(name: "argv", scope: !1876, file: !9, line: 9, type: !1109)
!1886 = !DILocalVariable(name: "i", scope: !1887, file: !9, line: 9, type: !13)
!1887 = distinct !DILexicalBlock(scope: !1876, file: !9, line: 9)
!1888 = !DILocation(line: 9, scope: !1887)
!1889 = !DILocation(line: 9, scope: !1890)
!1890 = distinct !DILexicalBlock(scope: !1891, file: !9, line: 9)
!1891 = distinct !DILexicalBlock(scope: !1887, file: !9, line: 9)
!1892 = !DILocation(line: 9, scope: !1893)
!1893 = distinct !DILexicalBlock(scope: !1890, file: !9, line: 9)
!1894 = !DILocation(line: 9, scope: !1891)
!1895 = distinct !{!1895, !1888, !1888, !1120}
!1896 = distinct !DISubprogram(name: "JNI_CallStaticLongMethod", scope: !9, file: !9, line: 9, type: !574, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1897 = !DILocalVariable(name: "methodID", arg: 3, scope: !1896, file: !9, line: 9, type: !67)
!1898 = !DILocation(line: 9, scope: !1896)
!1899 = !DILocalVariable(name: "clazz", arg: 2, scope: !1896, file: !9, line: 9, type: !47)
!1900 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1896, file: !9, line: 9, type: !25)
!1901 = !DILocalVariable(name: "args", scope: !1896, file: !9, line: 9, type: !149)
!1902 = !DILocalVariable(name: "sig", scope: !1896, file: !9, line: 9, type: !1104)
!1903 = !DILocalVariable(name: "argc", scope: !1896, file: !9, line: 9, type: !13)
!1904 = !DILocalVariable(name: "argv", scope: !1896, file: !9, line: 9, type: !1109)
!1905 = !DILocalVariable(name: "i", scope: !1906, file: !9, line: 9, type: !13)
!1906 = distinct !DILexicalBlock(scope: !1896, file: !9, line: 9)
!1907 = !DILocation(line: 9, scope: !1906)
!1908 = !DILocation(line: 9, scope: !1909)
!1909 = distinct !DILexicalBlock(scope: !1910, file: !9, line: 9)
!1910 = distinct !DILexicalBlock(scope: !1906, file: !9, line: 9)
!1911 = !DILocation(line: 9, scope: !1912)
!1912 = distinct !DILexicalBlock(scope: !1909, file: !9, line: 9)
!1913 = !DILocation(line: 9, scope: !1910)
!1914 = distinct !{!1914, !1907, !1907, !1120}
!1915 = !DILocalVariable(name: "ret", scope: !1896, file: !9, line: 9, type: !171)
!1916 = distinct !DISubprogram(name: "JNI_CallStaticLongMethodV", scope: !9, file: !9, line: 9, type: !578, scopeLine: 9, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1917 = !DILocalVariable(name: "args", arg: 4, scope: !1916, file: !9, line: 9, type: !149)
!1918 = !DILocation(line: 9, scope: !1916)
!1919 = !DILocalVariable(name: "methodID", arg: 3, scope: !1916, file: !9, line: 9, type: !67)
!1920 = !DILocalVariable(name: "clazz", arg: 2, scope: !1916, file: !9, line: 9, type: !47)
!1921 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1916, file: !9, line: 9, type: !25)
!1922 = !DILocalVariable(name: "sig", scope: !1916, file: !9, line: 9, type: !1104)
!1923 = !DILocalVariable(name: "argc", scope: !1916, file: !9, line: 9, type: !13)
!1924 = !DILocalVariable(name: "argv", scope: !1916, file: !9, line: 9, type: !1109)
!1925 = !DILocalVariable(name: "i", scope: !1926, file: !9, line: 9, type: !13)
!1926 = distinct !DILexicalBlock(scope: !1916, file: !9, line: 9)
!1927 = !DILocation(line: 9, scope: !1926)
!1928 = !DILocation(line: 9, scope: !1929)
!1929 = distinct !DILexicalBlock(scope: !1930, file: !9, line: 9)
!1930 = distinct !DILexicalBlock(scope: !1926, file: !9, line: 9)
!1931 = !DILocation(line: 9, scope: !1932)
!1932 = distinct !DILexicalBlock(scope: !1929, file: !9, line: 9)
!1933 = !DILocation(line: 9, scope: !1930)
!1934 = distinct !{!1934, !1927, !1927, !1120}
!1935 = distinct !DISubprogram(name: "JNI_CallFloatMethod", scope: !9, file: !9, line: 10, type: !278, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1936 = !DILocalVariable(name: "methodID", arg: 3, scope: !1935, file: !9, line: 10, type: !67)
!1937 = !DILocation(line: 10, scope: !1935)
!1938 = !DILocalVariable(name: "obj", arg: 2, scope: !1935, file: !9, line: 10, type: !48)
!1939 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1935, file: !9, line: 10, type: !25)
!1940 = !DILocalVariable(name: "args", scope: !1935, file: !9, line: 10, type: !149)
!1941 = !DILocalVariable(name: "sig", scope: !1935, file: !9, line: 10, type: !1104)
!1942 = !DILocalVariable(name: "argc", scope: !1935, file: !9, line: 10, type: !13)
!1943 = !DILocalVariable(name: "argv", scope: !1935, file: !9, line: 10, type: !1109)
!1944 = !DILocalVariable(name: "i", scope: !1945, file: !9, line: 10, type: !13)
!1945 = distinct !DILexicalBlock(scope: !1935, file: !9, line: 10)
!1946 = !DILocation(line: 10, scope: !1945)
!1947 = !DILocation(line: 10, scope: !1948)
!1948 = distinct !DILexicalBlock(scope: !1949, file: !9, line: 10)
!1949 = distinct !DILexicalBlock(scope: !1945, file: !9, line: 10)
!1950 = !DILocation(line: 10, scope: !1951)
!1951 = distinct !DILexicalBlock(scope: !1948, file: !9, line: 10)
!1952 = !DILocation(line: 10, scope: !1949)
!1953 = distinct !{!1953, !1946, !1946, !1120}
!1954 = !DILocalVariable(name: "ret", scope: !1935, file: !9, line: 10, type: !174)
!1955 = distinct !DISubprogram(name: "JNI_CallFloatMethodV", scope: !9, file: !9, line: 10, type: !282, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1956 = !DILocalVariable(name: "args", arg: 4, scope: !1955, file: !9, line: 10, type: !149)
!1957 = !DILocation(line: 10, scope: !1955)
!1958 = !DILocalVariable(name: "methodID", arg: 3, scope: !1955, file: !9, line: 10, type: !67)
!1959 = !DILocalVariable(name: "obj", arg: 2, scope: !1955, file: !9, line: 10, type: !48)
!1960 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1955, file: !9, line: 10, type: !25)
!1961 = !DILocalVariable(name: "sig", scope: !1955, file: !9, line: 10, type: !1104)
!1962 = !DILocalVariable(name: "argc", scope: !1955, file: !9, line: 10, type: !13)
!1963 = !DILocalVariable(name: "argv", scope: !1955, file: !9, line: 10, type: !1109)
!1964 = !DILocalVariable(name: "i", scope: !1965, file: !9, line: 10, type: !13)
!1965 = distinct !DILexicalBlock(scope: !1955, file: !9, line: 10)
!1966 = !DILocation(line: 10, scope: !1965)
!1967 = !DILocation(line: 10, scope: !1968)
!1968 = distinct !DILexicalBlock(scope: !1969, file: !9, line: 10)
!1969 = distinct !DILexicalBlock(scope: !1965, file: !9, line: 10)
!1970 = !DILocation(line: 10, scope: !1971)
!1971 = distinct !DILexicalBlock(scope: !1968, file: !9, line: 10)
!1972 = !DILocation(line: 10, scope: !1969)
!1973 = distinct !{!1973, !1966, !1966, !1120}
!1974 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethod", scope: !9, file: !9, line: 10, type: !398, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1975 = !DILocalVariable(name: "methodID", arg: 4, scope: !1974, file: !9, line: 10, type: !67)
!1976 = !DILocation(line: 10, scope: !1974)
!1977 = !DILocalVariable(name: "clazz", arg: 3, scope: !1974, file: !9, line: 10, type: !47)
!1978 = !DILocalVariable(name: "obj", arg: 2, scope: !1974, file: !9, line: 10, type: !48)
!1979 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1974, file: !9, line: 10, type: !25)
!1980 = !DILocalVariable(name: "args", scope: !1974, file: !9, line: 10, type: !149)
!1981 = !DILocalVariable(name: "sig", scope: !1974, file: !9, line: 10, type: !1104)
!1982 = !DILocalVariable(name: "argc", scope: !1974, file: !9, line: 10, type: !13)
!1983 = !DILocalVariable(name: "argv", scope: !1974, file: !9, line: 10, type: !1109)
!1984 = !DILocalVariable(name: "i", scope: !1985, file: !9, line: 10, type: !13)
!1985 = distinct !DILexicalBlock(scope: !1974, file: !9, line: 10)
!1986 = !DILocation(line: 10, scope: !1985)
!1987 = !DILocation(line: 10, scope: !1988)
!1988 = distinct !DILexicalBlock(scope: !1989, file: !9, line: 10)
!1989 = distinct !DILexicalBlock(scope: !1985, file: !9, line: 10)
!1990 = !DILocation(line: 10, scope: !1991)
!1991 = distinct !DILexicalBlock(scope: !1988, file: !9, line: 10)
!1992 = !DILocation(line: 10, scope: !1989)
!1993 = distinct !{!1993, !1986, !1986, !1120}
!1994 = !DILocalVariable(name: "ret", scope: !1974, file: !9, line: 10, type: !174)
!1995 = distinct !DISubprogram(name: "JNI_CallNonvirtualFloatMethodV", scope: !9, file: !9, line: 10, type: !402, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!1996 = !DILocalVariable(name: "args", arg: 5, scope: !1995, file: !9, line: 10, type: !149)
!1997 = !DILocation(line: 10, scope: !1995)
!1998 = !DILocalVariable(name: "methodID", arg: 4, scope: !1995, file: !9, line: 10, type: !67)
!1999 = !DILocalVariable(name: "clazz", arg: 3, scope: !1995, file: !9, line: 10, type: !47)
!2000 = !DILocalVariable(name: "obj", arg: 2, scope: !1995, file: !9, line: 10, type: !48)
!2001 = !DILocalVariable(name: "pEnv", arg: 1, scope: !1995, file: !9, line: 10, type: !25)
!2002 = !DILocalVariable(name: "sig", scope: !1995, file: !9, line: 10, type: !1104)
!2003 = !DILocalVariable(name: "argc", scope: !1995, file: !9, line: 10, type: !13)
!2004 = !DILocalVariable(name: "argv", scope: !1995, file: !9, line: 10, type: !1109)
!2005 = !DILocalVariable(name: "i", scope: !2006, file: !9, line: 10, type: !13)
!2006 = distinct !DILexicalBlock(scope: !1995, file: !9, line: 10)
!2007 = !DILocation(line: 10, scope: !2006)
!2008 = !DILocation(line: 10, scope: !2009)
!2009 = distinct !DILexicalBlock(scope: !2010, file: !9, line: 10)
!2010 = distinct !DILexicalBlock(scope: !2006, file: !9, line: 10)
!2011 = !DILocation(line: 10, scope: !2012)
!2012 = distinct !DILexicalBlock(scope: !2009, file: !9, line: 10)
!2013 = !DILocation(line: 10, scope: !2010)
!2014 = distinct !{!2014, !2007, !2007, !1120}
!2015 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethod", scope: !9, file: !9, line: 10, type: !586, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2016 = !DILocalVariable(name: "methodID", arg: 3, scope: !2015, file: !9, line: 10, type: !67)
!2017 = !DILocation(line: 10, scope: !2015)
!2018 = !DILocalVariable(name: "clazz", arg: 2, scope: !2015, file: !9, line: 10, type: !47)
!2019 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2015, file: !9, line: 10, type: !25)
!2020 = !DILocalVariable(name: "args", scope: !2015, file: !9, line: 10, type: !149)
!2021 = !DILocalVariable(name: "sig", scope: !2015, file: !9, line: 10, type: !1104)
!2022 = !DILocalVariable(name: "argc", scope: !2015, file: !9, line: 10, type: !13)
!2023 = !DILocalVariable(name: "argv", scope: !2015, file: !9, line: 10, type: !1109)
!2024 = !DILocalVariable(name: "i", scope: !2025, file: !9, line: 10, type: !13)
!2025 = distinct !DILexicalBlock(scope: !2015, file: !9, line: 10)
!2026 = !DILocation(line: 10, scope: !2025)
!2027 = !DILocation(line: 10, scope: !2028)
!2028 = distinct !DILexicalBlock(scope: !2029, file: !9, line: 10)
!2029 = distinct !DILexicalBlock(scope: !2025, file: !9, line: 10)
!2030 = !DILocation(line: 10, scope: !2031)
!2031 = distinct !DILexicalBlock(scope: !2028, file: !9, line: 10)
!2032 = !DILocation(line: 10, scope: !2029)
!2033 = distinct !{!2033, !2026, !2026, !1120}
!2034 = !DILocalVariable(name: "ret", scope: !2015, file: !9, line: 10, type: !174)
!2035 = distinct !DISubprogram(name: "JNI_CallStaticFloatMethodV", scope: !9, file: !9, line: 10, type: !590, scopeLine: 10, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2036 = !DILocalVariable(name: "args", arg: 4, scope: !2035, file: !9, line: 10, type: !149)
!2037 = !DILocation(line: 10, scope: !2035)
!2038 = !DILocalVariable(name: "methodID", arg: 3, scope: !2035, file: !9, line: 10, type: !67)
!2039 = !DILocalVariable(name: "clazz", arg: 2, scope: !2035, file: !9, line: 10, type: !47)
!2040 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2035, file: !9, line: 10, type: !25)
!2041 = !DILocalVariable(name: "sig", scope: !2035, file: !9, line: 10, type: !1104)
!2042 = !DILocalVariable(name: "argc", scope: !2035, file: !9, line: 10, type: !13)
!2043 = !DILocalVariable(name: "argv", scope: !2035, file: !9, line: 10, type: !1109)
!2044 = !DILocalVariable(name: "i", scope: !2045, file: !9, line: 10, type: !13)
!2045 = distinct !DILexicalBlock(scope: !2035, file: !9, line: 10)
!2046 = !DILocation(line: 10, scope: !2045)
!2047 = !DILocation(line: 10, scope: !2048)
!2048 = distinct !DILexicalBlock(scope: !2049, file: !9, line: 10)
!2049 = distinct !DILexicalBlock(scope: !2045, file: !9, line: 10)
!2050 = !DILocation(line: 10, scope: !2051)
!2051 = distinct !DILexicalBlock(scope: !2048, file: !9, line: 10)
!2052 = !DILocation(line: 10, scope: !2049)
!2053 = distinct !{!2053, !2046, !2046, !1120}
!2054 = distinct !DISubprogram(name: "JNI_CallDoubleMethod", scope: !9, file: !9, line: 11, type: !290, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2055 = !DILocalVariable(name: "methodID", arg: 3, scope: !2054, file: !9, line: 11, type: !67)
!2056 = !DILocation(line: 11, scope: !2054)
!2057 = !DILocalVariable(name: "obj", arg: 2, scope: !2054, file: !9, line: 11, type: !48)
!2058 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2054, file: !9, line: 11, type: !25)
!2059 = !DILocalVariable(name: "args", scope: !2054, file: !9, line: 11, type: !149)
!2060 = !DILocalVariable(name: "sig", scope: !2054, file: !9, line: 11, type: !1104)
!2061 = !DILocalVariable(name: "argc", scope: !2054, file: !9, line: 11, type: !13)
!2062 = !DILocalVariable(name: "argv", scope: !2054, file: !9, line: 11, type: !1109)
!2063 = !DILocalVariable(name: "i", scope: !2064, file: !9, line: 11, type: !13)
!2064 = distinct !DILexicalBlock(scope: !2054, file: !9, line: 11)
!2065 = !DILocation(line: 11, scope: !2064)
!2066 = !DILocation(line: 11, scope: !2067)
!2067 = distinct !DILexicalBlock(scope: !2068, file: !9, line: 11)
!2068 = distinct !DILexicalBlock(scope: !2064, file: !9, line: 11)
!2069 = !DILocation(line: 11, scope: !2070)
!2070 = distinct !DILexicalBlock(scope: !2067, file: !9, line: 11)
!2071 = !DILocation(line: 11, scope: !2068)
!2072 = distinct !{!2072, !2065, !2065, !1120}
!2073 = !DILocalVariable(name: "ret", scope: !2054, file: !9, line: 11, type: !177)
!2074 = distinct !DISubprogram(name: "JNI_CallDoubleMethodV", scope: !9, file: !9, line: 11, type: !294, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2075 = !DILocalVariable(name: "args", arg: 4, scope: !2074, file: !9, line: 11, type: !149)
!2076 = !DILocation(line: 11, scope: !2074)
!2077 = !DILocalVariable(name: "methodID", arg: 3, scope: !2074, file: !9, line: 11, type: !67)
!2078 = !DILocalVariable(name: "obj", arg: 2, scope: !2074, file: !9, line: 11, type: !48)
!2079 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2074, file: !9, line: 11, type: !25)
!2080 = !DILocalVariable(name: "sig", scope: !2074, file: !9, line: 11, type: !1104)
!2081 = !DILocalVariable(name: "argc", scope: !2074, file: !9, line: 11, type: !13)
!2082 = !DILocalVariable(name: "argv", scope: !2074, file: !9, line: 11, type: !1109)
!2083 = !DILocalVariable(name: "i", scope: !2084, file: !9, line: 11, type: !13)
!2084 = distinct !DILexicalBlock(scope: !2074, file: !9, line: 11)
!2085 = !DILocation(line: 11, scope: !2084)
!2086 = !DILocation(line: 11, scope: !2087)
!2087 = distinct !DILexicalBlock(scope: !2088, file: !9, line: 11)
!2088 = distinct !DILexicalBlock(scope: !2084, file: !9, line: 11)
!2089 = !DILocation(line: 11, scope: !2090)
!2090 = distinct !DILexicalBlock(scope: !2087, file: !9, line: 11)
!2091 = !DILocation(line: 11, scope: !2088)
!2092 = distinct !{!2092, !2085, !2085, !1120}
!2093 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethod", scope: !9, file: !9, line: 11, type: !410, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2094 = !DILocalVariable(name: "methodID", arg: 4, scope: !2093, file: !9, line: 11, type: !67)
!2095 = !DILocation(line: 11, scope: !2093)
!2096 = !DILocalVariable(name: "clazz", arg: 3, scope: !2093, file: !9, line: 11, type: !47)
!2097 = !DILocalVariable(name: "obj", arg: 2, scope: !2093, file: !9, line: 11, type: !48)
!2098 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2093, file: !9, line: 11, type: !25)
!2099 = !DILocalVariable(name: "args", scope: !2093, file: !9, line: 11, type: !149)
!2100 = !DILocalVariable(name: "sig", scope: !2093, file: !9, line: 11, type: !1104)
!2101 = !DILocalVariable(name: "argc", scope: !2093, file: !9, line: 11, type: !13)
!2102 = !DILocalVariable(name: "argv", scope: !2093, file: !9, line: 11, type: !1109)
!2103 = !DILocalVariable(name: "i", scope: !2104, file: !9, line: 11, type: !13)
!2104 = distinct !DILexicalBlock(scope: !2093, file: !9, line: 11)
!2105 = !DILocation(line: 11, scope: !2104)
!2106 = !DILocation(line: 11, scope: !2107)
!2107 = distinct !DILexicalBlock(scope: !2108, file: !9, line: 11)
!2108 = distinct !DILexicalBlock(scope: !2104, file: !9, line: 11)
!2109 = !DILocation(line: 11, scope: !2110)
!2110 = distinct !DILexicalBlock(scope: !2107, file: !9, line: 11)
!2111 = !DILocation(line: 11, scope: !2108)
!2112 = distinct !{!2112, !2105, !2105, !1120}
!2113 = !DILocalVariable(name: "ret", scope: !2093, file: !9, line: 11, type: !177)
!2114 = distinct !DISubprogram(name: "JNI_CallNonvirtualDoubleMethodV", scope: !9, file: !9, line: 11, type: !414, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2115 = !DILocalVariable(name: "args", arg: 5, scope: !2114, file: !9, line: 11, type: !149)
!2116 = !DILocation(line: 11, scope: !2114)
!2117 = !DILocalVariable(name: "methodID", arg: 4, scope: !2114, file: !9, line: 11, type: !67)
!2118 = !DILocalVariable(name: "clazz", arg: 3, scope: !2114, file: !9, line: 11, type: !47)
!2119 = !DILocalVariable(name: "obj", arg: 2, scope: !2114, file: !9, line: 11, type: !48)
!2120 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2114, file: !9, line: 11, type: !25)
!2121 = !DILocalVariable(name: "sig", scope: !2114, file: !9, line: 11, type: !1104)
!2122 = !DILocalVariable(name: "argc", scope: !2114, file: !9, line: 11, type: !13)
!2123 = !DILocalVariable(name: "argv", scope: !2114, file: !9, line: 11, type: !1109)
!2124 = !DILocalVariable(name: "i", scope: !2125, file: !9, line: 11, type: !13)
!2125 = distinct !DILexicalBlock(scope: !2114, file: !9, line: 11)
!2126 = !DILocation(line: 11, scope: !2125)
!2127 = !DILocation(line: 11, scope: !2128)
!2128 = distinct !DILexicalBlock(scope: !2129, file: !9, line: 11)
!2129 = distinct !DILexicalBlock(scope: !2125, file: !9, line: 11)
!2130 = !DILocation(line: 11, scope: !2131)
!2131 = distinct !DILexicalBlock(scope: !2128, file: !9, line: 11)
!2132 = !DILocation(line: 11, scope: !2129)
!2133 = distinct !{!2133, !2126, !2126, !1120}
!2134 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethod", scope: !9, file: !9, line: 11, type: !598, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2135 = !DILocalVariable(name: "methodID", arg: 3, scope: !2134, file: !9, line: 11, type: !67)
!2136 = !DILocation(line: 11, scope: !2134)
!2137 = !DILocalVariable(name: "clazz", arg: 2, scope: !2134, file: !9, line: 11, type: !47)
!2138 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2134, file: !9, line: 11, type: !25)
!2139 = !DILocalVariable(name: "args", scope: !2134, file: !9, line: 11, type: !149)
!2140 = !DILocalVariable(name: "sig", scope: !2134, file: !9, line: 11, type: !1104)
!2141 = !DILocalVariable(name: "argc", scope: !2134, file: !9, line: 11, type: !13)
!2142 = !DILocalVariable(name: "argv", scope: !2134, file: !9, line: 11, type: !1109)
!2143 = !DILocalVariable(name: "i", scope: !2144, file: !9, line: 11, type: !13)
!2144 = distinct !DILexicalBlock(scope: !2134, file: !9, line: 11)
!2145 = !DILocation(line: 11, scope: !2144)
!2146 = !DILocation(line: 11, scope: !2147)
!2147 = distinct !DILexicalBlock(scope: !2148, file: !9, line: 11)
!2148 = distinct !DILexicalBlock(scope: !2144, file: !9, line: 11)
!2149 = !DILocation(line: 11, scope: !2150)
!2150 = distinct !DILexicalBlock(scope: !2147, file: !9, line: 11)
!2151 = !DILocation(line: 11, scope: !2148)
!2152 = distinct !{!2152, !2145, !2145, !1120}
!2153 = !DILocalVariable(name: "ret", scope: !2134, file: !9, line: 11, type: !177)
!2154 = distinct !DISubprogram(name: "JNI_CallStaticDoubleMethodV", scope: !9, file: !9, line: 11, type: !602, scopeLine: 11, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2155 = !DILocalVariable(name: "args", arg: 4, scope: !2154, file: !9, line: 11, type: !149)
!2156 = !DILocation(line: 11, scope: !2154)
!2157 = !DILocalVariable(name: "methodID", arg: 3, scope: !2154, file: !9, line: 11, type: !67)
!2158 = !DILocalVariable(name: "clazz", arg: 2, scope: !2154, file: !9, line: 11, type: !47)
!2159 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2154, file: !9, line: 11, type: !25)
!2160 = !DILocalVariable(name: "sig", scope: !2154, file: !9, line: 11, type: !1104)
!2161 = !DILocalVariable(name: "argc", scope: !2154, file: !9, line: 11, type: !13)
!2162 = !DILocalVariable(name: "argv", scope: !2154, file: !9, line: 11, type: !1109)
!2163 = !DILocalVariable(name: "i", scope: !2164, file: !9, line: 11, type: !13)
!2164 = distinct !DILexicalBlock(scope: !2154, file: !9, line: 11)
!2165 = !DILocation(line: 11, scope: !2164)
!2166 = !DILocation(line: 11, scope: !2167)
!2167 = distinct !DILexicalBlock(scope: !2168, file: !9, line: 11)
!2168 = distinct !DILexicalBlock(scope: !2164, file: !9, line: 11)
!2169 = !DILocation(line: 11, scope: !2170)
!2170 = distinct !DILexicalBlock(scope: !2167, file: !9, line: 11)
!2171 = !DILocation(line: 11, scope: !2168)
!2172 = distinct !{!2172, !2165, !2165, !1120}
!2173 = distinct !DISubprogram(name: "JNI_NewObject", scope: !9, file: !9, line: 13, type: !143, scopeLine: 14, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2174 = !DILocalVariable(name: "methodID", arg: 3, scope: !2173, file: !9, line: 13, type: !67)
!2175 = !DILocation(line: 13, scope: !2173)
!2176 = !DILocalVariable(name: "clazz", arg: 2, scope: !2173, file: !9, line: 13, type: !47)
!2177 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2173, file: !9, line: 13, type: !25)
!2178 = !DILocalVariable(name: "args", scope: !2173, file: !9, line: 15, type: !149)
!2179 = !DILocation(line: 15, scope: !2173)
!2180 = !DILocation(line: 16, scope: !2173)
!2181 = !DILocalVariable(name: "sig", scope: !2173, file: !9, line: 17, type: !1104)
!2182 = !DILocation(line: 17, scope: !2173)
!2183 = !DILocalVariable(name: "argc", scope: !2173, file: !9, line: 17, type: !13)
!2184 = !DILocalVariable(name: "argv", scope: !2173, file: !9, line: 17, type: !1109)
!2185 = !DILocalVariable(name: "i", scope: !2186, file: !9, line: 17, type: !13)
!2186 = distinct !DILexicalBlock(scope: !2173, file: !9, line: 17)
!2187 = !DILocation(line: 17, scope: !2186)
!2188 = !DILocation(line: 17, scope: !2189)
!2189 = distinct !DILexicalBlock(scope: !2190, file: !9, line: 17)
!2190 = distinct !DILexicalBlock(scope: !2186, file: !9, line: 17)
!2191 = !DILocation(line: 17, scope: !2192)
!2192 = distinct !DILexicalBlock(scope: !2189, file: !9, line: 17)
!2193 = !DILocation(line: 17, scope: !2190)
!2194 = distinct !{!2194, !2187, !2187, !1120}
!2195 = !DILocalVariable(name: "o", scope: !2173, file: !9, line: 18, type: !48)
!2196 = !DILocation(line: 18, scope: !2173)
!2197 = !DILocation(line: 19, scope: !2173)
!2198 = !DILocation(line: 20, scope: !2173)
!2199 = distinct !DISubprogram(name: "JNI_NewObjectV", scope: !9, file: !9, line: 23, type: !147, scopeLine: 24, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2200 = !DILocalVariable(name: "args", arg: 4, scope: !2199, file: !9, line: 23, type: !149)
!2201 = !DILocation(line: 23, scope: !2199)
!2202 = !DILocalVariable(name: "methodID", arg: 3, scope: !2199, file: !9, line: 23, type: !67)
!2203 = !DILocalVariable(name: "clazz", arg: 2, scope: !2199, file: !9, line: 23, type: !47)
!2204 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2199, file: !9, line: 23, type: !25)
!2205 = !DILocalVariable(name: "sig", scope: !2199, file: !9, line: 25, type: !1104)
!2206 = !DILocation(line: 25, scope: !2199)
!2207 = !DILocalVariable(name: "argc", scope: !2199, file: !9, line: 25, type: !13)
!2208 = !DILocalVariable(name: "argv", scope: !2199, file: !9, line: 25, type: !1109)
!2209 = !DILocalVariable(name: "i", scope: !2210, file: !9, line: 25, type: !13)
!2210 = distinct !DILexicalBlock(scope: !2199, file: !9, line: 25)
!2211 = !DILocation(line: 25, scope: !2210)
!2212 = !DILocation(line: 25, scope: !2213)
!2213 = distinct !DILexicalBlock(scope: !2214, file: !9, line: 25)
!2214 = distinct !DILexicalBlock(scope: !2210, file: !9, line: 25)
!2215 = !DILocation(line: 25, scope: !2216)
!2216 = distinct !DILexicalBlock(scope: !2213, file: !9, line: 25)
!2217 = !DILocation(line: 25, scope: !2214)
!2218 = distinct !{!2218, !2211, !2211, !1120}
!2219 = !DILocation(line: 26, scope: !2199)
!2220 = distinct !DISubprogram(name: "JNI_CallVoidMethod", scope: !9, file: !9, line: 29, type: !302, scopeLine: 30, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2221 = !DILocalVariable(name: "methodID", arg: 3, scope: !2220, file: !9, line: 29, type: !67)
!2222 = !DILocation(line: 29, scope: !2220)
!2223 = !DILocalVariable(name: "obj", arg: 2, scope: !2220, file: !9, line: 29, type: !48)
!2224 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2220, file: !9, line: 29, type: !25)
!2225 = !DILocalVariable(name: "args", scope: !2220, file: !9, line: 31, type: !149)
!2226 = !DILocation(line: 31, scope: !2220)
!2227 = !DILocation(line: 32, scope: !2220)
!2228 = !DILocalVariable(name: "sig", scope: !2220, file: !9, line: 33, type: !1104)
!2229 = !DILocation(line: 33, scope: !2220)
!2230 = !DILocalVariable(name: "argc", scope: !2220, file: !9, line: 33, type: !13)
!2231 = !DILocalVariable(name: "argv", scope: !2220, file: !9, line: 33, type: !1109)
!2232 = !DILocalVariable(name: "i", scope: !2233, file: !9, line: 33, type: !13)
!2233 = distinct !DILexicalBlock(scope: !2220, file: !9, line: 33)
!2234 = !DILocation(line: 33, scope: !2233)
!2235 = !DILocation(line: 33, scope: !2236)
!2236 = distinct !DILexicalBlock(scope: !2237, file: !9, line: 33)
!2237 = distinct !DILexicalBlock(scope: !2233, file: !9, line: 33)
!2238 = !DILocation(line: 33, scope: !2239)
!2239 = distinct !DILexicalBlock(scope: !2236, file: !9, line: 33)
!2240 = !DILocation(line: 33, scope: !2237)
!2241 = distinct !{!2241, !2234, !2234, !1120}
!2242 = !DILocation(line: 34, scope: !2220)
!2243 = !DILocation(line: 35, scope: !2220)
!2244 = !DILocation(line: 36, scope: !2220)
!2245 = distinct !DISubprogram(name: "JNI_CallVoidMethodV", scope: !9, file: !9, line: 38, type: !306, scopeLine: 39, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2246 = !DILocalVariable(name: "args", arg: 4, scope: !2245, file: !9, line: 38, type: !149)
!2247 = !DILocation(line: 38, scope: !2245)
!2248 = !DILocalVariable(name: "methodID", arg: 3, scope: !2245, file: !9, line: 38, type: !67)
!2249 = !DILocalVariable(name: "obj", arg: 2, scope: !2245, file: !9, line: 38, type: !48)
!2250 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2245, file: !9, line: 38, type: !25)
!2251 = !DILocalVariable(name: "sig", scope: !2245, file: !9, line: 40, type: !1104)
!2252 = !DILocation(line: 40, scope: !2245)
!2253 = !DILocalVariable(name: "argc", scope: !2245, file: !9, line: 40, type: !13)
!2254 = !DILocalVariable(name: "argv", scope: !2245, file: !9, line: 40, type: !1109)
!2255 = !DILocalVariable(name: "i", scope: !2256, file: !9, line: 40, type: !13)
!2256 = distinct !DILexicalBlock(scope: !2245, file: !9, line: 40)
!2257 = !DILocation(line: 40, scope: !2256)
!2258 = !DILocation(line: 40, scope: !2259)
!2259 = distinct !DILexicalBlock(scope: !2260, file: !9, line: 40)
!2260 = distinct !DILexicalBlock(scope: !2256, file: !9, line: 40)
!2261 = !DILocation(line: 40, scope: !2262)
!2262 = distinct !DILexicalBlock(scope: !2259, file: !9, line: 40)
!2263 = !DILocation(line: 40, scope: !2260)
!2264 = distinct !{!2264, !2257, !2257, !1120}
!2265 = !DILocation(line: 41, scope: !2245)
!2266 = !DILocation(line: 42, scope: !2245)
!2267 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethod", scope: !9, file: !9, line: 44, type: !422, scopeLine: 45, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2268 = !DILocalVariable(name: "methodID", arg: 4, scope: !2267, file: !9, line: 44, type: !67)
!2269 = !DILocation(line: 44, scope: !2267)
!2270 = !DILocalVariable(name: "clazz", arg: 3, scope: !2267, file: !9, line: 44, type: !47)
!2271 = !DILocalVariable(name: "obj", arg: 2, scope: !2267, file: !9, line: 44, type: !48)
!2272 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2267, file: !9, line: 44, type: !25)
!2273 = !DILocalVariable(name: "args", scope: !2267, file: !9, line: 46, type: !149)
!2274 = !DILocation(line: 46, scope: !2267)
!2275 = !DILocation(line: 47, scope: !2267)
!2276 = !DILocalVariable(name: "sig", scope: !2267, file: !9, line: 48, type: !1104)
!2277 = !DILocation(line: 48, scope: !2267)
!2278 = !DILocalVariable(name: "argc", scope: !2267, file: !9, line: 48, type: !13)
!2279 = !DILocalVariable(name: "argv", scope: !2267, file: !9, line: 48, type: !1109)
!2280 = !DILocalVariable(name: "i", scope: !2281, file: !9, line: 48, type: !13)
!2281 = distinct !DILexicalBlock(scope: !2267, file: !9, line: 48)
!2282 = !DILocation(line: 48, scope: !2281)
!2283 = !DILocation(line: 48, scope: !2284)
!2284 = distinct !DILexicalBlock(scope: !2285, file: !9, line: 48)
!2285 = distinct !DILexicalBlock(scope: !2281, file: !9, line: 48)
!2286 = !DILocation(line: 48, scope: !2287)
!2287 = distinct !DILexicalBlock(scope: !2284, file: !9, line: 48)
!2288 = !DILocation(line: 48, scope: !2285)
!2289 = distinct !{!2289, !2282, !2282, !1120}
!2290 = !DILocation(line: 49, scope: !2267)
!2291 = !DILocation(line: 50, scope: !2267)
!2292 = !DILocation(line: 51, scope: !2267)
!2293 = distinct !DISubprogram(name: "JNI_CallNonvirtualVoidMethodV", scope: !9, file: !9, line: 53, type: !426, scopeLine: 54, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2294 = !DILocalVariable(name: "args", arg: 5, scope: !2293, file: !9, line: 53, type: !149)
!2295 = !DILocation(line: 53, scope: !2293)
!2296 = !DILocalVariable(name: "methodID", arg: 4, scope: !2293, file: !9, line: 53, type: !67)
!2297 = !DILocalVariable(name: "clazz", arg: 3, scope: !2293, file: !9, line: 53, type: !47)
!2298 = !DILocalVariable(name: "obj", arg: 2, scope: !2293, file: !9, line: 53, type: !48)
!2299 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2293, file: !9, line: 53, type: !25)
!2300 = !DILocalVariable(name: "sig", scope: !2293, file: !9, line: 55, type: !1104)
!2301 = !DILocation(line: 55, scope: !2293)
!2302 = !DILocalVariable(name: "argc", scope: !2293, file: !9, line: 55, type: !13)
!2303 = !DILocalVariable(name: "argv", scope: !2293, file: !9, line: 55, type: !1109)
!2304 = !DILocalVariable(name: "i", scope: !2305, file: !9, line: 55, type: !13)
!2305 = distinct !DILexicalBlock(scope: !2293, file: !9, line: 55)
!2306 = !DILocation(line: 55, scope: !2305)
!2307 = !DILocation(line: 55, scope: !2308)
!2308 = distinct !DILexicalBlock(scope: !2309, file: !9, line: 55)
!2309 = distinct !DILexicalBlock(scope: !2305, file: !9, line: 55)
!2310 = !DILocation(line: 55, scope: !2311)
!2311 = distinct !DILexicalBlock(scope: !2308, file: !9, line: 55)
!2312 = !DILocation(line: 55, scope: !2309)
!2313 = distinct !{!2313, !2306, !2306, !1120}
!2314 = !DILocation(line: 56, scope: !2293)
!2315 = !DILocation(line: 57, scope: !2293)
!2316 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethod", scope: !9, file: !9, line: 59, type: !610, scopeLine: 60, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2317 = !DILocalVariable(name: "methodID", arg: 3, scope: !2316, file: !9, line: 59, type: !67)
!2318 = !DILocation(line: 59, scope: !2316)
!2319 = !DILocalVariable(name: "clazz", arg: 2, scope: !2316, file: !9, line: 59, type: !47)
!2320 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2316, file: !9, line: 59, type: !25)
!2321 = !DILocalVariable(name: "args", scope: !2316, file: !9, line: 61, type: !149)
!2322 = !DILocation(line: 61, scope: !2316)
!2323 = !DILocation(line: 62, scope: !2316)
!2324 = !DILocalVariable(name: "sig", scope: !2316, file: !9, line: 63, type: !1104)
!2325 = !DILocation(line: 63, scope: !2316)
!2326 = !DILocalVariable(name: "argc", scope: !2316, file: !9, line: 63, type: !13)
!2327 = !DILocalVariable(name: "argv", scope: !2316, file: !9, line: 63, type: !1109)
!2328 = !DILocalVariable(name: "i", scope: !2329, file: !9, line: 63, type: !13)
!2329 = distinct !DILexicalBlock(scope: !2316, file: !9, line: 63)
!2330 = !DILocation(line: 63, scope: !2329)
!2331 = !DILocation(line: 63, scope: !2332)
!2332 = distinct !DILexicalBlock(scope: !2333, file: !9, line: 63)
!2333 = distinct !DILexicalBlock(scope: !2329, file: !9, line: 63)
!2334 = !DILocation(line: 63, scope: !2335)
!2335 = distinct !DILexicalBlock(scope: !2332, file: !9, line: 63)
!2336 = !DILocation(line: 63, scope: !2333)
!2337 = distinct !{!2337, !2330, !2330, !1120}
!2338 = !DILocation(line: 64, scope: !2316)
!2339 = !DILocation(line: 65, scope: !2316)
!2340 = !DILocation(line: 66, scope: !2316)
!2341 = distinct !DISubprogram(name: "JNI_CallStaticVoidMethodV", scope: !9, file: !9, line: 68, type: !614, scopeLine: 69, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2342 = !DILocalVariable(name: "args", arg: 4, scope: !2341, file: !9, line: 68, type: !149)
!2343 = !DILocation(line: 68, scope: !2341)
!2344 = !DILocalVariable(name: "methodID", arg: 3, scope: !2341, file: !9, line: 68, type: !67)
!2345 = !DILocalVariable(name: "clazz", arg: 2, scope: !2341, file: !9, line: 68, type: !47)
!2346 = !DILocalVariable(name: "pEnv", arg: 1, scope: !2341, file: !9, line: 68, type: !25)
!2347 = !DILocalVariable(name: "sig", scope: !2341, file: !9, line: 70, type: !1104)
!2348 = !DILocation(line: 70, scope: !2341)
!2349 = !DILocalVariable(name: "argc", scope: !2341, file: !9, line: 70, type: !13)
!2350 = !DILocalVariable(name: "argv", scope: !2341, file: !9, line: 70, type: !1109)
!2351 = !DILocalVariable(name: "i", scope: !2352, file: !9, line: 70, type: !13)
!2352 = distinct !DILexicalBlock(scope: !2341, file: !9, line: 70)
!2353 = !DILocation(line: 70, scope: !2352)
!2354 = !DILocation(line: 70, scope: !2355)
!2355 = distinct !DILexicalBlock(scope: !2356, file: !9, line: 70)
!2356 = distinct !DILexicalBlock(scope: !2352, file: !9, line: 70)
!2357 = !DILocation(line: 70, scope: !2358)
!2358 = distinct !DILexicalBlock(scope: !2355, file: !9, line: 70)
!2359 = !DILocation(line: 70, scope: !2356)
!2360 = distinct !{!2360, !2353, !2353, !1120}
!2361 = !DILocation(line: 71, scope: !2341)
!2362 = !DILocation(line: 72, scope: !2341)
!2363 = distinct !DISubprogram(name: "_vsprintf_l", scope: !1040, file: !1040, line: 1449, type: !2364, scopeLine: 1458, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2364 = !DISubroutineType(types: !2365)
!2365 = !{!13, !1043, !1044, !2366, !149}
!2366 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !2367)
!2367 = !DIDerivedType(tag: DW_TAG_typedef, name: "_locale_t", file: !2368, line: 623, baseType: !2369)
!2368 = !DIFile(filename: "C:\\Program Files (x86)\\Windows Kits\\10\\include\\10.0.22621.0\\ucrt\\corecrt.h", directory: "", checksumkind: CSK_MD5, checksum: "4ce81db8e96f94c79f8dce86dd46b97f")
!2369 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2370, size: 64)
!2370 = !DIDerivedType(tag: DW_TAG_typedef, name: "__crt_locale_pointers", file: !2368, line: 621, baseType: !2371)
!2371 = distinct !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_pointers", file: !2368, line: 617, size: 128, elements: !2372)
!2372 = !{!2373, !2376}
!2373 = !DIDerivedType(tag: DW_TAG_member, name: "locinfo", scope: !2371, file: !2368, line: 619, baseType: !2374, size: 64)
!2374 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2375, size: 64)
!2375 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_locale_data", file: !2368, line: 619, flags: DIFlagFwdDecl)
!2376 = !DIDerivedType(tag: DW_TAG_member, name: "mbcinfo", scope: !2371, file: !2368, line: 620, baseType: !2377, size: 64, offset: 64)
!2377 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !2378, size: 64)
!2378 = !DICompositeType(tag: DW_TAG_structure_type, name: "__crt_multibyte_data", file: !2368, line: 620, flags: DIFlagFwdDecl)
!2379 = !DILocalVariable(name: "_ArgList", arg: 4, scope: !2363, file: !1040, line: 1453, type: !149)
!2380 = !DILocation(line: 1453, scope: !2363)
!2381 = !DILocalVariable(name: "_Locale", arg: 3, scope: !2363, file: !1040, line: 1452, type: !2366)
!2382 = !DILocation(line: 1452, scope: !2363)
!2383 = !DILocalVariable(name: "_Format", arg: 2, scope: !2363, file: !1040, line: 1451, type: !1044)
!2384 = !DILocation(line: 1451, scope: !2363)
!2385 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2363, file: !1040, line: 1450, type: !1043)
!2386 = !DILocation(line: 1450, scope: !2363)
!2387 = !DILocation(line: 1459, scope: !2363)
!2388 = distinct !DISubprogram(name: "_vsnprintf_l", scope: !1040, file: !1040, line: 1381, type: !2389, scopeLine: 1391, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !8, retainedNodes: !1032)
!2389 = !DISubroutineType(types: !2390)
!2390 = !{!13, !1043, !1070, !1044, !2366, !149}
!2391 = !DILocalVariable(name: "_ArgList", arg: 5, scope: !2388, file: !1040, line: 1386, type: !149)
!2392 = !DILocation(line: 1386, scope: !2388)
!2393 = !DILocalVariable(name: "_Locale", arg: 4, scope: !2388, file: !1040, line: 1385, type: !2366)
!2394 = !DILocation(line: 1385, scope: !2388)
!2395 = !DILocalVariable(name: "_Format", arg: 3, scope: !2388, file: !1040, line: 1384, type: !1044)
!2396 = !DILocation(line: 1384, scope: !2388)
!2397 = !DILocalVariable(name: "_BufferCount", arg: 2, scope: !2388, file: !1040, line: 1383, type: !1070)
!2398 = !DILocation(line: 1383, scope: !2388)
!2399 = !DILocalVariable(name: "_Buffer", arg: 1, scope: !2388, file: !1040, line: 1382, type: !1043)
!2400 = !DILocation(line: 1382, scope: !2388)
!2401 = !DILocalVariable(name: "_Result", scope: !2388, file: !1040, line: 1392, type: !2402)
!2402 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !13)
!2403 = !DILocation(line: 1392, scope: !2388)
!2404 = !DILocation(line: 1396, scope: !2388)
!2405 = !DILocation(line: 92, scope: !2)
