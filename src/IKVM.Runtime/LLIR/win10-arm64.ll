; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:64:64-i32:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-pc-windows-msvc19.34.31933"

%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }
%union.jvalue = type { i64 }

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 35
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !12

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 36
  %68 = load ptr, ptr %67, align 8, !tbaa !14
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 65
  %8 = load ptr, ptr %7, align 8, !tbaa !15
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call ptr %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret ptr %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !16

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 66
  %69 = load ptr, ptr %68, align 8, !tbaa !17
  %70 = call ptr %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret ptr %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 115
  %7 = load ptr, ptr %6, align 8, !tbaa !18
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !19

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 116
  %68 = load ptr, ptr %67, align 8, !tbaa !20
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 38
  %7 = load ptr, ptr %6, align 8, !tbaa !21
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !22

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 39
  %68 = load ptr, ptr %67, align 8, !tbaa !23
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 68
  %8 = load ptr, ptr %7, align 8, !tbaa !24
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !25

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 69
  %69 = load ptr, ptr %68, align 8, !tbaa !26
  %70 = call i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 118
  %7 = load ptr, ptr %6, align 8, !tbaa !27
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !28

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 119
  %68 = load ptr, ptr %67, align 8, !tbaa !29
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 41
  %7 = load ptr, ptr %6, align 8, !tbaa !30
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !31

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 42
  %68 = load ptr, ptr %67, align 8, !tbaa !32
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 71
  %8 = load ptr, ptr %7, align 8, !tbaa !33
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !34

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 72
  %69 = load ptr, ptr %68, align 8, !tbaa !35
  %70 = call i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 121
  %7 = load ptr, ptr %6, align 8, !tbaa !36
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !37

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 122
  %68 = load ptr, ptr %67, align 8, !tbaa !38
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 44
  %7 = load ptr, ptr %6, align 8, !tbaa !39
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !40

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 45
  %68 = load ptr, ptr %67, align 8, !tbaa !41
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 74
  %8 = load ptr, ptr %7, align 8, !tbaa !42
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !43

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 75
  %69 = load ptr, ptr %68, align 8, !tbaa !44
  %70 = call i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 124
  %7 = load ptr, ptr %6, align 8, !tbaa !45
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !46

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 125
  %68 = load ptr, ptr %67, align 8, !tbaa !47
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 47
  %7 = load ptr, ptr %6, align 8, !tbaa !48
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !49

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 48
  %68 = load ptr, ptr %67, align 8, !tbaa !50
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 77
  %8 = load ptr, ptr %7, align 8, !tbaa !51
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !52

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 78
  %69 = load ptr, ptr %68, align 8, !tbaa !53
  %70 = call i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 127
  %7 = load ptr, ptr %6, align 8, !tbaa !54
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !55

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 128
  %68 = load ptr, ptr %67, align 8, !tbaa !56
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 50
  %7 = load ptr, ptr %6, align 8, !tbaa !57
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !58

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 51
  %68 = load ptr, ptr %67, align 8, !tbaa !59
  %69 = call i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 80
  %8 = load ptr, ptr %7, align 8, !tbaa !60
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i32 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i32 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !61

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 81
  %69 = load ptr, ptr %68, align 8, !tbaa !62
  %70 = call i32 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i32 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 130
  %7 = load ptr, ptr %6, align 8, !tbaa !63
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !64

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 131
  %68 = load ptr, ptr %67, align 8, !tbaa !65
  %69 = call i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 53
  %7 = load ptr, ptr %6, align 8, !tbaa !66
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !67

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 54
  %68 = load ptr, ptr %67, align 8, !tbaa !68
  %69 = call i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 83
  %8 = load ptr, ptr %7, align 8, !tbaa !69
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call i64 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret i64 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !70

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 84
  %69 = load ptr, ptr %68, align 8, !tbaa !71
  %70 = call i64 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret i64 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 133
  %7 = load ptr, ptr %6, align 8, !tbaa !72
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !73

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 134
  %68 = load ptr, ptr %67, align 8, !tbaa !74
  %69 = call i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 56
  %7 = load ptr, ptr %6, align 8, !tbaa !75
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !76

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 57
  %68 = load ptr, ptr %67, align 8, !tbaa !77
  %69 = call float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret float %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 86
  %8 = load ptr, ptr %7, align 8, !tbaa !78
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call float %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret float %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !79

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 87
  %69 = load ptr, ptr %68, align 8, !tbaa !80
  %70 = call float %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret float %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 136
  %7 = load ptr, ptr %6, align 8, !tbaa !81
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !82

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 137
  %68 = load ptr, ptr %67, align 8, !tbaa !83
  %69 = call float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret float %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 59
  %7 = load ptr, ptr %6, align 8, !tbaa !84
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !85

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 60
  %68 = load ptr, ptr %67, align 8, !tbaa !86
  %69 = call double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret double %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 89
  %8 = load ptr, ptr %7, align 8, !tbaa !87
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  %10 = call double %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret double %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !88

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 90
  %69 = load ptr, ptr %68, align 8, !tbaa !89
  %70 = call double %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret double %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 139
  %7 = load ptr, ptr %6, align 8, !tbaa !90
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !91

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 140
  %68 = load ptr, ptr %67, align 8, !tbaa !92
  %69 = call double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret double %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 29
  %7 = load ptr, ptr %6, align 8, !tbaa !93
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  %9 = call ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !94

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 30
  %68 = load ptr, ptr %67, align 8, !tbaa !95
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 62
  %7 = load ptr, ptr %6, align 8, !tbaa !96
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  call void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !97

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 63
  %68 = load ptr, ptr %67, align 8, !tbaa !98
  call void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i64 0, i32 92
  %8 = load ptr, ptr %7, align 8, !tbaa !99
  %9 = load ptr, ptr %5, align 8, !tbaa !4
  call void %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !10
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %62 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %53
    i32 76, label %58
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i64 8
  %22 = load i32, ptr %16, align 8
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !11
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !11
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !11
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !100

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 93
  %69 = load ptr, ptr %68, align 8, !tbaa !101
  call void %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 8, !tbaa !4
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i64 0, i32 142
  %7 = load ptr, ptr %6, align 8, !tbaa !102
  %8 = load ptr, ptr %4, align 8, !tbaa !4
  call void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !10
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %61 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %52
    i32 76, label %57
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !11
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !11
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !11
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !103

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 143
  %68 = load ptr, ptr %67, align 8, !tbaa !104
  call void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #3
  ret void
}

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+v8a" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 7, !"uwtable", i32 2}
!3 = !{!"clang version 15.0.2"}
!4 = !{!5, !5, i64 0}
!5 = !{!"any pointer", !6, i64 0}
!6 = !{!"omnipotent char", !7, i64 0}
!7 = !{!"Simple C/C++ TBAA"}
!8 = !{!9, !5, i64 280}
!9 = !{!"JNINativeInterface_", !5, i64 0, !5, i64 8, !5, i64 16, !5, i64 24, !5, i64 32, !5, i64 40, !5, i64 48, !5, i64 56, !5, i64 64, !5, i64 72, !5, i64 80, !5, i64 88, !5, i64 96, !5, i64 104, !5, i64 112, !5, i64 120, !5, i64 128, !5, i64 136, !5, i64 144, !5, i64 152, !5, i64 160, !5, i64 168, !5, i64 176, !5, i64 184, !5, i64 192, !5, i64 200, !5, i64 208, !5, i64 216, !5, i64 224, !5, i64 232, !5, i64 240, !5, i64 248, !5, i64 256, !5, i64 264, !5, i64 272, !5, i64 280, !5, i64 288, !5, i64 296, !5, i64 304, !5, i64 312, !5, i64 320, !5, i64 328, !5, i64 336, !5, i64 344, !5, i64 352, !5, i64 360, !5, i64 368, !5, i64 376, !5, i64 384, !5, i64 392, !5, i64 400, !5, i64 408, !5, i64 416, !5, i64 424, !5, i64 432, !5, i64 440, !5, i64 448, !5, i64 456, !5, i64 464, !5, i64 472, !5, i64 480, !5, i64 488, !5, i64 496, !5, i64 504, !5, i64 512, !5, i64 520, !5, i64 528, !5, i64 536, !5, i64 544, !5, i64 552, !5, i64 560, !5, i64 568, !5, i64 576, !5, i64 584, !5, i64 592, !5, i64 600, !5, i64 608, !5, i64 616, !5, i64 624, !5, i64 632, !5, i64 640, !5, i64 648, !5, i64 656, !5, i64 664, !5, i64 672, !5, i64 680, !5, i64 688, !5, i64 696, !5, i64 704, !5, i64 712, !5, i64 720, !5, i64 728, !5, i64 736, !5, i64 744, !5, i64 752, !5, i64 760, !5, i64 768, !5, i64 776, !5, i64 784, !5, i64 792, !5, i64 800, !5, i64 808, !5, i64 816, !5, i64 824, !5, i64 832, !5, i64 840, !5, i64 848, !5, i64 856, !5, i64 864, !5, i64 872, !5, i64 880, !5, i64 888, !5, i64 896, !5, i64 904, !5, i64 912, !5, i64 920, !5, i64 928, !5, i64 936, !5, i64 944, !5, i64 952, !5, i64 960, !5, i64 968, !5, i64 976, !5, i64 984, !5, i64 992, !5, i64 1000, !5, i64 1008, !5, i64 1016, !5, i64 1024, !5, i64 1032, !5, i64 1040, !5, i64 1048, !5, i64 1056, !5, i64 1064, !5, i64 1072, !5, i64 1080, !5, i64 1088, !5, i64 1096, !5, i64 1104, !5, i64 1112, !5, i64 1120, !5, i64 1128, !5, i64 1136, !5, i64 1144, !5, i64 1152, !5, i64 1160, !5, i64 1168, !5, i64 1176, !5, i64 1184, !5, i64 1192, !5, i64 1200, !5, i64 1208, !5, i64 1216, !5, i64 1224, !5, i64 1232, !5, i64 1240, !5, i64 1248, !5, i64 1256, !5, i64 1264, !5, i64 1272, !5, i64 1280, !5, i64 1288, !5, i64 1296, !5, i64 1304, !5, i64 1312, !5, i64 1320, !5, i64 1328, !5, i64 1336, !5, i64 1344, !5, i64 1352, !5, i64 1360, !5, i64 1368, !5, i64 1376, !5, i64 1384, !5, i64 1392, !5, i64 1400, !5, i64 1408, !5, i64 1416, !5, i64 1424, !5, i64 1432, !5, i64 1440, !5, i64 1448, !5, i64 1456, !5, i64 1464, !5, i64 1472, !5, i64 1480, !5, i64 1488, !5, i64 1496, !5, i64 1504, !5, i64 1512, !5, i64 1520, !5, i64 1528, !5, i64 1536, !5, i64 1544, !5, i64 1552, !5, i64 1560, !5, i64 1568, !5, i64 1576, !5, i64 1584, !5, i64 1592, !5, i64 1600, !5, i64 1608, !5, i64 1616, !5, i64 1624, !5, i64 1632, !5, i64 1640, !5, i64 1648, !5, i64 1656, !5, i64 1664, !5, i64 1672, !5, i64 1680, !5, i64 1688, !5, i64 1696, !5, i64 1704, !5, i64 1712, !5, i64 1720, !5, i64 1728, !5, i64 1736, !5, i64 1744, !5, i64 1752, !5, i64 1760, !5, i64 1768, !5, i64 1776, !5, i64 1784, !5, i64 1792, !5, i64 1800, !5, i64 1808, !5, i64 1816, !5, i64 1824, !5, i64 1832, !5, i64 1840, !5, i64 1848, !5, i64 1856}
!10 = !{!9, !5, i64 0}
!11 = !{!6, !6, i64 0}
!12 = distinct !{!12, !13}
!13 = !{!"llvm.loop.mustprogress"}
!14 = !{!9, !5, i64 288}
!15 = !{!9, !5, i64 520}
!16 = distinct !{!16, !13}
!17 = !{!9, !5, i64 528}
!18 = !{!9, !5, i64 920}
!19 = distinct !{!19, !13}
!20 = !{!9, !5, i64 928}
!21 = !{!9, !5, i64 304}
!22 = distinct !{!22, !13}
!23 = !{!9, !5, i64 312}
!24 = !{!9, !5, i64 544}
!25 = distinct !{!25, !13}
!26 = !{!9, !5, i64 552}
!27 = !{!9, !5, i64 944}
!28 = distinct !{!28, !13}
!29 = !{!9, !5, i64 952}
!30 = !{!9, !5, i64 328}
!31 = distinct !{!31, !13}
!32 = !{!9, !5, i64 336}
!33 = !{!9, !5, i64 568}
!34 = distinct !{!34, !13}
!35 = !{!9, !5, i64 576}
!36 = !{!9, !5, i64 968}
!37 = distinct !{!37, !13}
!38 = !{!9, !5, i64 976}
!39 = !{!9, !5, i64 352}
!40 = distinct !{!40, !13}
!41 = !{!9, !5, i64 360}
!42 = !{!9, !5, i64 592}
!43 = distinct !{!43, !13}
!44 = !{!9, !5, i64 600}
!45 = !{!9, !5, i64 992}
!46 = distinct !{!46, !13}
!47 = !{!9, !5, i64 1000}
!48 = !{!9, !5, i64 376}
!49 = distinct !{!49, !13}
!50 = !{!9, !5, i64 384}
!51 = !{!9, !5, i64 616}
!52 = distinct !{!52, !13}
!53 = !{!9, !5, i64 624}
!54 = !{!9, !5, i64 1016}
!55 = distinct !{!55, !13}
!56 = !{!9, !5, i64 1024}
!57 = !{!9, !5, i64 400}
!58 = distinct !{!58, !13}
!59 = !{!9, !5, i64 408}
!60 = !{!9, !5, i64 640}
!61 = distinct !{!61, !13}
!62 = !{!9, !5, i64 648}
!63 = !{!9, !5, i64 1040}
!64 = distinct !{!64, !13}
!65 = !{!9, !5, i64 1048}
!66 = !{!9, !5, i64 424}
!67 = distinct !{!67, !13}
!68 = !{!9, !5, i64 432}
!69 = !{!9, !5, i64 664}
!70 = distinct !{!70, !13}
!71 = !{!9, !5, i64 672}
!72 = !{!9, !5, i64 1064}
!73 = distinct !{!73, !13}
!74 = !{!9, !5, i64 1072}
!75 = !{!9, !5, i64 448}
!76 = distinct !{!76, !13}
!77 = !{!9, !5, i64 456}
!78 = !{!9, !5, i64 688}
!79 = distinct !{!79, !13}
!80 = !{!9, !5, i64 696}
!81 = !{!9, !5, i64 1088}
!82 = distinct !{!82, !13}
!83 = !{!9, !5, i64 1096}
!84 = !{!9, !5, i64 472}
!85 = distinct !{!85, !13}
!86 = !{!9, !5, i64 480}
!87 = !{!9, !5, i64 712}
!88 = distinct !{!88, !13}
!89 = !{!9, !5, i64 720}
!90 = !{!9, !5, i64 1112}
!91 = distinct !{!91, !13}
!92 = !{!9, !5, i64 1120}
!93 = !{!9, !5, i64 232}
!94 = distinct !{!94, !13}
!95 = !{!9, !5, i64 240}
!96 = !{!9, !5, i64 496}
!97 = distinct !{!97, !13}
!98 = !{!9, !5, i64 504}
!99 = !{!9, !5, i64 736}
!100 = distinct !{!100, !13}
!101 = !{!9, !5, i64 744}
!102 = !{!9, !5, i64 1136}
!103 = distinct !{!103, !13}
!104 = !{!9, !5, i64 1144}
