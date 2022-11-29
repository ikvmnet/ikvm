; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:x-p:32:32-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32-a:0:32-S32"
target triple = "i686-pc-windows-msvc19.34.31933"

%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }
%union.jvalue = type { i64 }

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 35
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
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

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !11

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 36
  %67 = load ptr, ptr %66, align 4, !tbaa !13
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 65
  %8 = load ptr, ptr %7, align 4, !tbaa !14
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc ptr %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret ptr %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !15

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 66
  %68 = load ptr, ptr %67, align 4, !tbaa !16
  %69 = call x86_stdcallcc ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 115
  %7 = load ptr, ptr %6, align 4, !tbaa !17
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !18

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 116
  %67 = load ptr, ptr %66, align 4, !tbaa !19
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 38
  %7 = load ptr, ptr %6, align 4, !tbaa !20
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc zeroext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !21

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 39
  %67 = load ptr, ptr %66, align 4, !tbaa !22
  %68 = call x86_stdcallcc zeroext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 68
  %8 = load ptr, ptr %7, align 4, !tbaa !23
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc zeroext i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !24

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 69
  %68 = load ptr, ptr %67, align 4, !tbaa !25
  %69 = call x86_stdcallcc zeroext i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 118
  %7 = load ptr, ptr %6, align 4, !tbaa !26
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc zeroext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !27

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 119
  %67 = load ptr, ptr %66, align 4, !tbaa !28
  %68 = call x86_stdcallcc zeroext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 41
  %7 = load ptr, ptr %6, align 4, !tbaa !29
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc signext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !30

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 42
  %67 = load ptr, ptr %66, align 4, !tbaa !31
  %68 = call x86_stdcallcc signext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 71
  %8 = load ptr, ptr %7, align 4, !tbaa !32
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc signext i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !33

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 72
  %68 = load ptr, ptr %67, align 4, !tbaa !34
  %69 = call x86_stdcallcc signext i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 121
  %7 = load ptr, ptr %6, align 4, !tbaa !35
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc signext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !36

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 122
  %67 = load ptr, ptr %66, align 4, !tbaa !37
  %68 = call x86_stdcallcc signext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 44
  %7 = load ptr, ptr %6, align 4, !tbaa !38
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc zeroext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !39

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 45
  %67 = load ptr, ptr %66, align 4, !tbaa !40
  %68 = call x86_stdcallcc zeroext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 74
  %8 = load ptr, ptr %7, align 4, !tbaa !41
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc zeroext i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !42

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 75
  %68 = load ptr, ptr %67, align 4, !tbaa !43
  %69 = call x86_stdcallcc zeroext i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 124
  %7 = load ptr, ptr %6, align 4, !tbaa !44
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc zeroext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !45

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 125
  %67 = load ptr, ptr %66, align 4, !tbaa !46
  %68 = call x86_stdcallcc zeroext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 47
  %7 = load ptr, ptr %6, align 4, !tbaa !47
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc signext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !48

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 48
  %67 = load ptr, ptr %66, align 4, !tbaa !49
  %68 = call x86_stdcallcc signext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 77
  %8 = load ptr, ptr %7, align 4, !tbaa !50
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc signext i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !51

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 78
  %68 = load ptr, ptr %67, align 4, !tbaa !52
  %69 = call x86_stdcallcc signext i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 127
  %7 = load ptr, ptr %6, align 4, !tbaa !53
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc signext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !54

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 128
  %67 = load ptr, ptr %66, align 4, !tbaa !55
  %68 = call x86_stdcallcc signext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 50
  %7 = load ptr, ptr %6, align 4, !tbaa !56
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !57

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 51
  %67 = load ptr, ptr %66, align 4, !tbaa !58
  %68 = call x86_stdcallcc i32 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i32 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 80
  %8 = load ptr, ptr %7, align 4, !tbaa !59
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc i32 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i32 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !60

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 81
  %68 = load ptr, ptr %67, align 4, !tbaa !61
  %69 = call x86_stdcallcc i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 130
  %7 = load ptr, ptr %6, align 4, !tbaa !62
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !63

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 131
  %67 = load ptr, ptr %66, align 4, !tbaa !64
  %68 = call x86_stdcallcc i32 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i32 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 53
  %7 = load ptr, ptr %6, align 4, !tbaa !65
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !66

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 54
  %67 = load ptr, ptr %66, align 4, !tbaa !67
  %68 = call x86_stdcallcc i64 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i64 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 83
  %8 = load ptr, ptr %7, align 4, !tbaa !68
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc i64 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i64 %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !69

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 84
  %68 = load ptr, ptr %67, align 4, !tbaa !70
  %69 = call x86_stdcallcc i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 133
  %7 = load ptr, ptr %6, align 4, !tbaa !71
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !72

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 134
  %67 = load ptr, ptr %66, align 4, !tbaa !73
  %68 = call x86_stdcallcc i64 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i64 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 56
  %7 = load ptr, ptr %6, align 4, !tbaa !74
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !75

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 57
  %67 = load ptr, ptr %66, align 4, !tbaa !76
  %68 = call x86_stdcallcc float %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret float %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 86
  %8 = load ptr, ptr %7, align 4, !tbaa !77
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc float %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret float %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !78

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 87
  %68 = load ptr, ptr %67, align 4, !tbaa !79
  %69 = call x86_stdcallcc float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret float %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 136
  %7 = load ptr, ptr %6, align 4, !tbaa !80
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !81

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 137
  %67 = load ptr, ptr %66, align 4, !tbaa !82
  %68 = call x86_stdcallcc float %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret float %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 59
  %7 = load ptr, ptr %6, align 4, !tbaa !83
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !84

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 60
  %67 = load ptr, ptr %66, align 4, !tbaa !85
  %68 = call x86_stdcallcc double %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret double %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 89
  %8 = load ptr, ptr %7, align 4, !tbaa !86
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  %10 = call x86_stdcallcc double %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret double %10
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !87

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 90
  %68 = load ptr, ptr %67, align 4, !tbaa !88
  %69 = call x86_stdcallcc double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret double %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 139
  %7 = load ptr, ptr %6, align 4, !tbaa !89
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !90

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 140
  %67 = load ptr, ptr %66, align 4, !tbaa !91
  %68 = call x86_stdcallcc double %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret double %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 29
  %7 = load ptr, ptr %6, align 4, !tbaa !92
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  %9 = call x86_stdcallcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !93

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 30
  %67 = load ptr, ptr %66, align 4, !tbaa !94
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 62
  %7 = load ptr, ptr %6, align 4, !tbaa !95
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  call x86_stdcallcc void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !96

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 63
  %67 = load ptr, ptr %66, align 4, !tbaa !97
  call x86_stdcallcc void %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 92
  %8 = load ptr, ptr %7, align 4, !tbaa !98
  %9 = load ptr, ptr %5, align 4, !tbaa !3
  call x86_stdcallcc void %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !10
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
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !99

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 93
  %68 = load ptr, ptr %67, align 4, !tbaa !100
  call x86_stdcallcc void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !3
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 142
  %7 = load ptr, ptr %6, align 4, !tbaa !101
  %8 = load ptr, ptr %4, align 4, !tbaa !3
  call x86_stdcallcc void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !10
  %17 = sext i8 %16 to i32
  switch i32 %17, label %60 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %51
    i32 76, label %56
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !10
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !10
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !10
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !10
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !10
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !10
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !10
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !10
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !102

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 143
  %67 = load ptr, ptr %66, align 4, !tbaa !103
  call x86_stdcallcc void %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret void
}

attributes #0 = { alwaysinline nounwind "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nounwind }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"NumRegisterParameters", i32 0}
!1 = !{i32 1, !"wchar_size", i32 2}
!2 = !{!"clang version 15.0.2"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C/C++ TBAA"}
!7 = !{!8, !4, i64 140}
!8 = !{!"JNINativeInterface_", !4, i64 0, !4, i64 4, !4, i64 8, !4, i64 12, !4, i64 16, !4, i64 20, !4, i64 24, !4, i64 28, !4, i64 32, !4, i64 36, !4, i64 40, !4, i64 44, !4, i64 48, !4, i64 52, !4, i64 56, !4, i64 60, !4, i64 64, !4, i64 68, !4, i64 72, !4, i64 76, !4, i64 80, !4, i64 84, !4, i64 88, !4, i64 92, !4, i64 96, !4, i64 100, !4, i64 104, !4, i64 108, !4, i64 112, !4, i64 116, !4, i64 120, !4, i64 124, !4, i64 128, !4, i64 132, !4, i64 136, !4, i64 140, !4, i64 144, !4, i64 148, !4, i64 152, !4, i64 156, !4, i64 160, !4, i64 164, !4, i64 168, !4, i64 172, !4, i64 176, !4, i64 180, !4, i64 184, !4, i64 188, !4, i64 192, !4, i64 196, !4, i64 200, !4, i64 204, !4, i64 208, !4, i64 212, !4, i64 216, !4, i64 220, !4, i64 224, !4, i64 228, !4, i64 232, !4, i64 236, !4, i64 240, !4, i64 244, !4, i64 248, !4, i64 252, !4, i64 256, !4, i64 260, !4, i64 264, !4, i64 268, !4, i64 272, !4, i64 276, !4, i64 280, !4, i64 284, !4, i64 288, !4, i64 292, !4, i64 296, !4, i64 300, !4, i64 304, !4, i64 308, !4, i64 312, !4, i64 316, !4, i64 320, !4, i64 324, !4, i64 328, !4, i64 332, !4, i64 336, !4, i64 340, !4, i64 344, !4, i64 348, !4, i64 352, !4, i64 356, !4, i64 360, !4, i64 364, !4, i64 368, !4, i64 372, !4, i64 376, !4, i64 380, !4, i64 384, !4, i64 388, !4, i64 392, !4, i64 396, !4, i64 400, !4, i64 404, !4, i64 408, !4, i64 412, !4, i64 416, !4, i64 420, !4, i64 424, !4, i64 428, !4, i64 432, !4, i64 436, !4, i64 440, !4, i64 444, !4, i64 448, !4, i64 452, !4, i64 456, !4, i64 460, !4, i64 464, !4, i64 468, !4, i64 472, !4, i64 476, !4, i64 480, !4, i64 484, !4, i64 488, !4, i64 492, !4, i64 496, !4, i64 500, !4, i64 504, !4, i64 508, !4, i64 512, !4, i64 516, !4, i64 520, !4, i64 524, !4, i64 528, !4, i64 532, !4, i64 536, !4, i64 540, !4, i64 544, !4, i64 548, !4, i64 552, !4, i64 556, !4, i64 560, !4, i64 564, !4, i64 568, !4, i64 572, !4, i64 576, !4, i64 580, !4, i64 584, !4, i64 588, !4, i64 592, !4, i64 596, !4, i64 600, !4, i64 604, !4, i64 608, !4, i64 612, !4, i64 616, !4, i64 620, !4, i64 624, !4, i64 628, !4, i64 632, !4, i64 636, !4, i64 640, !4, i64 644, !4, i64 648, !4, i64 652, !4, i64 656, !4, i64 660, !4, i64 664, !4, i64 668, !4, i64 672, !4, i64 676, !4, i64 680, !4, i64 684, !4, i64 688, !4, i64 692, !4, i64 696, !4, i64 700, !4, i64 704, !4, i64 708, !4, i64 712, !4, i64 716, !4, i64 720, !4, i64 724, !4, i64 728, !4, i64 732, !4, i64 736, !4, i64 740, !4, i64 744, !4, i64 748, !4, i64 752, !4, i64 756, !4, i64 760, !4, i64 764, !4, i64 768, !4, i64 772, !4, i64 776, !4, i64 780, !4, i64 784, !4, i64 788, !4, i64 792, !4, i64 796, !4, i64 800, !4, i64 804, !4, i64 808, !4, i64 812, !4, i64 816, !4, i64 820, !4, i64 824, !4, i64 828, !4, i64 832, !4, i64 836, !4, i64 840, !4, i64 844, !4, i64 848, !4, i64 852, !4, i64 856, !4, i64 860, !4, i64 864, !4, i64 868, !4, i64 872, !4, i64 876, !4, i64 880, !4, i64 884, !4, i64 888, !4, i64 892, !4, i64 896, !4, i64 900, !4, i64 904, !4, i64 908, !4, i64 912, !4, i64 916, !4, i64 920, !4, i64 924, !4, i64 928}
!9 = !{!8, !4, i64 0}
!10 = !{!5, !5, i64 0}
!11 = distinct !{!11, !12}
!12 = !{!"llvm.loop.mustprogress"}
!13 = !{!8, !4, i64 144}
!14 = !{!8, !4, i64 260}
!15 = distinct !{!15, !12}
!16 = !{!8, !4, i64 264}
!17 = !{!8, !4, i64 460}
!18 = distinct !{!18, !12}
!19 = !{!8, !4, i64 464}
!20 = !{!8, !4, i64 152}
!21 = distinct !{!21, !12}
!22 = !{!8, !4, i64 156}
!23 = !{!8, !4, i64 272}
!24 = distinct !{!24, !12}
!25 = !{!8, !4, i64 276}
!26 = !{!8, !4, i64 472}
!27 = distinct !{!27, !12}
!28 = !{!8, !4, i64 476}
!29 = !{!8, !4, i64 164}
!30 = distinct !{!30, !12}
!31 = !{!8, !4, i64 168}
!32 = !{!8, !4, i64 284}
!33 = distinct !{!33, !12}
!34 = !{!8, !4, i64 288}
!35 = !{!8, !4, i64 484}
!36 = distinct !{!36, !12}
!37 = !{!8, !4, i64 488}
!38 = !{!8, !4, i64 176}
!39 = distinct !{!39, !12}
!40 = !{!8, !4, i64 180}
!41 = !{!8, !4, i64 296}
!42 = distinct !{!42, !12}
!43 = !{!8, !4, i64 300}
!44 = !{!8, !4, i64 496}
!45 = distinct !{!45, !12}
!46 = !{!8, !4, i64 500}
!47 = !{!8, !4, i64 188}
!48 = distinct !{!48, !12}
!49 = !{!8, !4, i64 192}
!50 = !{!8, !4, i64 308}
!51 = distinct !{!51, !12}
!52 = !{!8, !4, i64 312}
!53 = !{!8, !4, i64 508}
!54 = distinct !{!54, !12}
!55 = !{!8, !4, i64 512}
!56 = !{!8, !4, i64 200}
!57 = distinct !{!57, !12}
!58 = !{!8, !4, i64 204}
!59 = !{!8, !4, i64 320}
!60 = distinct !{!60, !12}
!61 = !{!8, !4, i64 324}
!62 = !{!8, !4, i64 520}
!63 = distinct !{!63, !12}
!64 = !{!8, !4, i64 524}
!65 = !{!8, !4, i64 212}
!66 = distinct !{!66, !12}
!67 = !{!8, !4, i64 216}
!68 = !{!8, !4, i64 332}
!69 = distinct !{!69, !12}
!70 = !{!8, !4, i64 336}
!71 = !{!8, !4, i64 532}
!72 = distinct !{!72, !12}
!73 = !{!8, !4, i64 536}
!74 = !{!8, !4, i64 224}
!75 = distinct !{!75, !12}
!76 = !{!8, !4, i64 228}
!77 = !{!8, !4, i64 344}
!78 = distinct !{!78, !12}
!79 = !{!8, !4, i64 348}
!80 = !{!8, !4, i64 544}
!81 = distinct !{!81, !12}
!82 = !{!8, !4, i64 548}
!83 = !{!8, !4, i64 236}
!84 = distinct !{!84, !12}
!85 = !{!8, !4, i64 240}
!86 = !{!8, !4, i64 356}
!87 = distinct !{!87, !12}
!88 = !{!8, !4, i64 360}
!89 = !{!8, !4, i64 556}
!90 = distinct !{!90, !12}
!91 = !{!8, !4, i64 560}
!92 = !{!8, !4, i64 116}
!93 = distinct !{!93, !12}
!94 = !{!8, !4, i64 120}
!95 = !{!8, !4, i64 248}
!96 = distinct !{!96, !12}
!97 = !{!8, !4, i64 252}
!98 = !{!8, !4, i64 368}
!99 = distinct !{!99, !12}
!100 = !{!8, !4, i64 372}
!101 = !{!8, !4, i64 568}
!102 = distinct !{!102, !12}
!103 = !{!8, !4, i64 572}
