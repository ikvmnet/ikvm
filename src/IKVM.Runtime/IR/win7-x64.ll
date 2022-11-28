; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc19.34.31933"

%union.jvalue = type { i64 }
%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !11

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 36
  %70 = load ptr, ptr %69, align 8, !tbaa !13
  %71 = call ptr %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret ptr %71
}

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !11

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 36
  %68 = load ptr, ptr %67, align 8, !tbaa !13
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret ptr %69
}

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !14

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 66
  %71 = load ptr, ptr %70, align 8, !tbaa !15
  %72 = call ptr %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret ptr %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !14

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 66
  %69 = load ptr, ptr %68, align 8, !tbaa !15
  %70 = call ptr %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret ptr %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !16

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 116
  %70 = load ptr, ptr %69, align 8, !tbaa !17
  %71 = call ptr %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret ptr %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !16

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 116
  %68 = load ptr, ptr %67, align 8, !tbaa !17
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !18

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 39
  %70 = load ptr, ptr %69, align 8, !tbaa !19
  %71 = call i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !18

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 39
  %68 = load ptr, ptr %67, align 8, !tbaa !19
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !20

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 69
  %71 = load ptr, ptr %70, align 8, !tbaa !21
  %72 = call i8 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i8 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !20

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 69
  %69 = load ptr, ptr %68, align 8, !tbaa !21
  %70 = call i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !22

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 119
  %70 = load ptr, ptr %69, align 8, !tbaa !23
  %71 = call i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !22

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 119
  %68 = load ptr, ptr %67, align 8, !tbaa !23
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !24

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 42
  %70 = load ptr, ptr %69, align 8, !tbaa !25
  %71 = call i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !24

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 42
  %68 = load ptr, ptr %67, align 8, !tbaa !25
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !26

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 72
  %71 = load ptr, ptr %70, align 8, !tbaa !27
  %72 = call i8 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i8 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !26

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 72
  %69 = load ptr, ptr %68, align 8, !tbaa !27
  %70 = call i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !28

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 122
  %70 = load ptr, ptr %69, align 8, !tbaa !29
  %71 = call i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !28

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 122
  %68 = load ptr, ptr %67, align 8, !tbaa !29
  %69 = call i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !30

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 45
  %70 = load ptr, ptr %69, align 8, !tbaa !31
  %71 = call i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !30

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 45
  %68 = load ptr, ptr %67, align 8, !tbaa !31
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !32

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 75
  %71 = load ptr, ptr %70, align 8, !tbaa !33
  %72 = call i16 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i16 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !32

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 75
  %69 = load ptr, ptr %68, align 8, !tbaa !33
  %70 = call i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !34

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 125
  %70 = load ptr, ptr %69, align 8, !tbaa !35
  %71 = call i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !34

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 125
  %68 = load ptr, ptr %67, align 8, !tbaa !35
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !36

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 48
  %70 = load ptr, ptr %69, align 8, !tbaa !37
  %71 = call i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !36

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 48
  %68 = load ptr, ptr %67, align 8, !tbaa !37
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !38

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 78
  %71 = load ptr, ptr %70, align 8, !tbaa !39
  %72 = call i16 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i16 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !38

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 78
  %69 = load ptr, ptr %68, align 8, !tbaa !39
  %70 = call i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !40

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 128
  %70 = load ptr, ptr %69, align 8, !tbaa !41
  %71 = call i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !40

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 128
  %68 = load ptr, ptr %67, align 8, !tbaa !41
  %69 = call i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !42

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 51
  %70 = load ptr, ptr %69, align 8, !tbaa !43
  %71 = call i32 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i32 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !42

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 51
  %68 = load ptr, ptr %67, align 8, !tbaa !43
  %69 = call i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !44

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 81
  %71 = load ptr, ptr %70, align 8, !tbaa !45
  %72 = call i32 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i32 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !44

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 81
  %69 = load ptr, ptr %68, align 8, !tbaa !45
  %70 = call i32 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i32 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !46

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 131
  %70 = load ptr, ptr %69, align 8, !tbaa !47
  %71 = call i32 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i32 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !46

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 131
  %68 = load ptr, ptr %67, align 8, !tbaa !47
  %69 = call i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !48

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 54
  %70 = load ptr, ptr %69, align 8, !tbaa !49
  %71 = call i64 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i64 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !48

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 54
  %68 = load ptr, ptr %67, align 8, !tbaa !49
  %69 = call i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !50

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 84
  %71 = load ptr, ptr %70, align 8, !tbaa !51
  %72 = call i64 %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret i64 %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !50

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 84
  %69 = load ptr, ptr %68, align 8, !tbaa !51
  %70 = call i64 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret i64 %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !52

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 134
  %70 = load ptr, ptr %69, align 8, !tbaa !53
  %71 = call i64 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret i64 %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !52

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 134
  %68 = load ptr, ptr %67, align 8, !tbaa !53
  %69 = call i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !54

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 57
  %70 = load ptr, ptr %69, align 8, !tbaa !55
  %71 = call float %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret float %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !54

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 57
  %68 = load ptr, ptr %67, align 8, !tbaa !55
  %69 = call float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret float %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !56

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 87
  %71 = load ptr, ptr %70, align 8, !tbaa !57
  %72 = call float %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret float %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !56

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 87
  %69 = load ptr, ptr %68, align 8, !tbaa !57
  %70 = call float %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret float %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !58

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 137
  %70 = load ptr, ptr %69, align 8, !tbaa !59
  %71 = call float %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret float %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !58

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 137
  %68 = load ptr, ptr %67, align 8, !tbaa !59
  %69 = call float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret float %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !60

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 60
  %70 = load ptr, ptr %69, align 8, !tbaa !61
  %71 = call double %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret double %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !60

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 60
  %68 = load ptr, ptr %67, align 8, !tbaa !61
  %69 = call double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret double %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !62

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 90
  %71 = load ptr, ptr %70, align 8, !tbaa !63
  %72 = call double %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret double %72
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !62

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 90
  %69 = load ptr, ptr %68, align 8, !tbaa !63
  %70 = call double %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret double %70
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !64

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 140
  %70 = load ptr, ptr %69, align 8, !tbaa !65
  %71 = call double %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret double %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !64

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 140
  %68 = load ptr, ptr %67, align 8, !tbaa !65
  %69 = call double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret double %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !66

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 30
  %70 = load ptr, ptr %69, align 8, !tbaa !67
  %71 = call ptr %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret ptr %71
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !66

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 30
  %68 = load ptr, ptr %67, align 8, !tbaa !67
  %69 = call ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !68

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 63
  %70 = load ptr, ptr %69, align 8, !tbaa !69
  call void %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !68

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 63
  %68 = load ptr, ptr %67, align 8, !tbaa !69
  call void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  %6 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 8, !tbaa !4
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 8, !tbaa !4
  %10 = load ptr, ptr %9, align 8, !tbaa !8
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = zext i32 %11 to i64
  %13 = shl nuw nsw i64 %12, 3
  %14 = alloca i8, i64 %13, align 16
  %15 = icmp sgt i32 %11, 0
  br i1 %15, label %16, label %68

16:                                               ; preds = %4, %64
  %17 = phi i64 [ %66, %64 ], [ 0, %4 ]
  %18 = phi ptr [ %65, %64 ], [ %7, %4 ]
  %19 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %17
  %20 = load i8, ptr %19, align 1, !tbaa !10
  %21 = sext i8 %20 to i32
  switch i32 %21, label %64 [
    i32 90, label %22
    i32 66, label %27
    i32 83, label %32
    i32 67, label %37
    i32 73, label %42
    i32 74, label %46
    i32 68, label %51
    i32 70, label %55
    i32 76, label %60
  ]

22:                                               ; preds = %16
  %23 = getelementptr inbounds i8, ptr %18, i64 8
  %24 = load i32, ptr %18, align 8
  %25 = trunc i32 %24 to i8
  %26 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %25, ptr %26, align 8, !tbaa !10
  br label %64

27:                                               ; preds = %16
  %28 = getelementptr inbounds i8, ptr %18, i64 8
  %29 = load i32, ptr %18, align 8
  %30 = trunc i32 %29 to i8
  %31 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i8 %30, ptr %31, align 8, !tbaa !10
  br label %64

32:                                               ; preds = %16
  %33 = getelementptr inbounds i8, ptr %18, i64 8
  %34 = load i32, ptr %18, align 8
  %35 = trunc i32 %34 to i16
  %36 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i16 %35, ptr %36, align 8, !tbaa !10
  br label %64

37:                                               ; preds = %16
  %38 = getelementptr inbounds i8, ptr %18, i64 8
  %39 = load i32, ptr %18, align 8
  %40 = and i32 %39, 65535
  %41 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %40, ptr %41, align 8, !tbaa !10
  br label %64

42:                                               ; preds = %16
  %43 = getelementptr inbounds i8, ptr %18, i64 8
  %44 = load i32, ptr %18, align 8
  %45 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i32 %44, ptr %45, align 8, !tbaa !10
  br label %64

46:                                               ; preds = %16
  %47 = getelementptr inbounds i8, ptr %18, i64 8
  %48 = load i32, ptr %18, align 8
  %49 = sext i32 %48 to i64
  %50 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store i64 %49, ptr %50, align 8, !tbaa !10
  br label %64

51:                                               ; preds = %16
  %52 = getelementptr inbounds i8, ptr %18, i64 8
  %53 = load double, ptr %18, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store double %53, ptr %54, align 8, !tbaa !10
  br label %64

55:                                               ; preds = %16
  %56 = getelementptr inbounds i8, ptr %18, i64 8
  %57 = load double, ptr %18, align 8
  %58 = fptrunc double %57 to float
  %59 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store float %58, ptr %59, align 8, !tbaa !10
  br label %64

60:                                               ; preds = %16
  %61 = getelementptr inbounds i8, ptr %18, i64 8
  %62 = load ptr, ptr %18, align 8
  %63 = getelementptr inbounds %union.jvalue, ptr %14, i64 %17
  store ptr %62, ptr %63, align 8, !tbaa !10
  br label %64

64:                                               ; preds = %60, %55, %51, %46, %42, %37, %32, %27, %22, %16
  %65 = phi ptr [ %18, %16 ], [ %61, %60 ], [ %56, %55 ], [ %52, %51 ], [ %47, %46 ], [ %43, %42 ], [ %38, %37 ], [ %33, %32 ], [ %28, %27 ], [ %23, %22 ]
  %66 = add nuw nsw i64 %17, 1
  %67 = icmp eq i64 %66, %12
  br i1 %67, label %68, label %16, !llvm.loop !70

68:                                               ; preds = %64, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %69 = load ptr, ptr %0, align 8, !tbaa !4
  %70 = getelementptr inbounds %struct.JNINativeInterface_, ptr %69, i64 0, i32 93
  %71 = load ptr, ptr %70, align 8, !tbaa !71
  call void %71(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %14) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 8, !tbaa !4
  %8 = load ptr, ptr %7, align 8, !tbaa !8
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = zext i32 %9 to i64
  %11 = shl nuw nsw i64 %10, 3
  %12 = alloca i8, i64 %11, align 16
  %13 = icmp sgt i32 %9, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %5, %62
  %15 = phi i64 [ %64, %62 ], [ 0, %5 ]
  %16 = phi ptr [ %63, %62 ], [ %4, %5 ]
  %17 = getelementptr inbounds [257 x i8], ptr %6, i64 0, i64 %15
  %18 = load i8, ptr %17, align 1, !tbaa !10
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
  store i8 %23, ptr %24, align 8, !tbaa !10
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i64 8
  %27 = load i32, ptr %16, align 8
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i8 %28, ptr %29, align 8, !tbaa !10
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i64 8
  %32 = load i32, ptr %16, align 8
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i16 %33, ptr %34, align 8, !tbaa !10
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i64 8
  %37 = load i32, ptr %16, align 8
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %38, ptr %39, align 8, !tbaa !10
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i64 8
  %42 = load i32, ptr %16, align 8
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i32 %42, ptr %43, align 8, !tbaa !10
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i64 8
  %46 = load i32, ptr %16, align 8
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store i64 %47, ptr %48, align 8, !tbaa !10
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i64 8
  %51 = load double, ptr %16, align 8
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store double %51, ptr %52, align 8, !tbaa !10
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i64 8
  %55 = load double, ptr %16, align 8
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store float %56, ptr %57, align 8, !tbaa !10
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i64 8
  %60 = load ptr, ptr %16, align 8
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i64 %15
  store ptr %60, ptr %61, align 8, !tbaa !10
  br label %62

62:                                               ; preds = %20, %25, %30, %35, %40, %44, %49, %53, %58, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i64 %15, 1
  %65 = icmp eq i64 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !70

66:                                               ; preds = %62, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %67 = load ptr, ptr %0, align 8, !tbaa !4
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i64 0, i32 93
  %69 = load ptr, ptr %68, align 8, !tbaa !71
  call void %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %12) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 16
  %5 = alloca ptr, align 8
  call void @llvm.lifetime.start.p0(i64 8, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 8, !tbaa !4
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 8, !tbaa !4
  %9 = load ptr, ptr %8, align 8, !tbaa !8
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = zext i32 %10 to i64
  %12 = shl nuw nsw i64 %11, 3
  %13 = alloca i8, i64 %12, align 16
  %14 = icmp sgt i32 %10, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %3, %63
  %16 = phi i64 [ %65, %63 ], [ 0, %3 ]
  %17 = phi ptr [ %64, %63 ], [ %6, %3 ]
  %18 = getelementptr inbounds [257 x i8], ptr %4, i64 0, i64 %16
  %19 = load i8, ptr %18, align 1, !tbaa !10
  %20 = sext i8 %19 to i32
  switch i32 %20, label %63 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %54
    i32 76, label %59
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i64 8
  %23 = load i32, ptr %17, align 8
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %24, ptr %25, align 8, !tbaa !10
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i64 8
  %28 = load i32, ptr %17, align 8
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i8 %29, ptr %30, align 8, !tbaa !10
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i64 8
  %33 = load i32, ptr %17, align 8
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i16 %34, ptr %35, align 8, !tbaa !10
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i64 8
  %38 = load i32, ptr %17, align 8
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %39, ptr %40, align 8, !tbaa !10
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i64 8
  %43 = load i32, ptr %17, align 8
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i32 %43, ptr %44, align 8, !tbaa !10
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i64 8
  %47 = load i32, ptr %17, align 8
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store i64 %48, ptr %49, align 8, !tbaa !10
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i64 8
  %52 = load double, ptr %17, align 8
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store double %52, ptr %53, align 8, !tbaa !10
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i64 8
  %56 = load double, ptr %17, align 8
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store float %57, ptr %58, align 8, !tbaa !10
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i64 8
  %61 = load ptr, ptr %17, align 8
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i64 %16
  store ptr %61, ptr %62, align 8, !tbaa !10
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i64 %16, 1
  %66 = icmp eq i64 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !72

67:                                               ; preds = %63, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %68 = load ptr, ptr %0, align 8, !tbaa !4
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i64 0, i32 143
  %70 = load ptr, ptr %69, align 8, !tbaa !73
  call void %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 8, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 16
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 8, !tbaa !4
  %7 = load ptr, ptr %6, align 8, !tbaa !8
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = zext i32 %8 to i64
  %10 = shl nuw nsw i64 %9, 3
  %11 = alloca i8, i64 %10, align 16
  %12 = icmp sgt i32 %8, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %4, %61
  %14 = phi i64 [ %63, %61 ], [ 0, %4 ]
  %15 = phi ptr [ %62, %61 ], [ %3, %4 ]
  %16 = getelementptr inbounds [257 x i8], ptr %5, i64 0, i64 %14
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
  %20 = getelementptr inbounds i8, ptr %15, i64 8
  %21 = load i32, ptr %15, align 8
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %22, ptr %23, align 8, !tbaa !10
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i64 8
  %26 = load i32, ptr %15, align 8
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i8 %27, ptr %28, align 8, !tbaa !10
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i64 8
  %31 = load i32, ptr %15, align 8
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i16 %32, ptr %33, align 8, !tbaa !10
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i64 8
  %36 = load i32, ptr %15, align 8
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %37, ptr %38, align 8, !tbaa !10
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i64 8
  %41 = load i32, ptr %15, align 8
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i32 %41, ptr %42, align 8, !tbaa !10
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i64 8
  %45 = load i32, ptr %15, align 8
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store i64 %46, ptr %47, align 8, !tbaa !10
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i64 8
  %50 = load double, ptr %15, align 8
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store double %50, ptr %51, align 8, !tbaa !10
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i64 8
  %54 = load double, ptr %15, align 8
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store float %55, ptr %56, align 8, !tbaa !10
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i64 8
  %59 = load ptr, ptr %15, align 8
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i64 %14
  store ptr %59, ptr %60, align 8, !tbaa !10
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i64 %14, 1
  %64 = icmp eq i64 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !72

65:                                               ; preds = %61, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %66 = load ptr, ptr %0, align 8, !tbaa !4
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i64 0, i32 143
  %68 = load ptr, ptr %67, align 8, !tbaa !73
  call void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %11) #4
  ret void
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare ptr @llvm.stacksave() #3

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.stackrestore(ptr) #3

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nocallback nofree nosync nounwind willreturn }
attributes #4 = { nounwind }

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
!8 = !{!9, !5, i64 0}
!9 = !{!"JNINativeInterface_", !5, i64 0, !5, i64 8, !5, i64 16, !5, i64 24, !5, i64 32, !5, i64 40, !5, i64 48, !5, i64 56, !5, i64 64, !5, i64 72, !5, i64 80, !5, i64 88, !5, i64 96, !5, i64 104, !5, i64 112, !5, i64 120, !5, i64 128, !5, i64 136, !5, i64 144, !5, i64 152, !5, i64 160, !5, i64 168, !5, i64 176, !5, i64 184, !5, i64 192, !5, i64 200, !5, i64 208, !5, i64 216, !5, i64 224, !5, i64 232, !5, i64 240, !5, i64 248, !5, i64 256, !5, i64 264, !5, i64 272, !5, i64 280, !5, i64 288, !5, i64 296, !5, i64 304, !5, i64 312, !5, i64 320, !5, i64 328, !5, i64 336, !5, i64 344, !5, i64 352, !5, i64 360, !5, i64 368, !5, i64 376, !5, i64 384, !5, i64 392, !5, i64 400, !5, i64 408, !5, i64 416, !5, i64 424, !5, i64 432, !5, i64 440, !5, i64 448, !5, i64 456, !5, i64 464, !5, i64 472, !5, i64 480, !5, i64 488, !5, i64 496, !5, i64 504, !5, i64 512, !5, i64 520, !5, i64 528, !5, i64 536, !5, i64 544, !5, i64 552, !5, i64 560, !5, i64 568, !5, i64 576, !5, i64 584, !5, i64 592, !5, i64 600, !5, i64 608, !5, i64 616, !5, i64 624, !5, i64 632, !5, i64 640, !5, i64 648, !5, i64 656, !5, i64 664, !5, i64 672, !5, i64 680, !5, i64 688, !5, i64 696, !5, i64 704, !5, i64 712, !5, i64 720, !5, i64 728, !5, i64 736, !5, i64 744, !5, i64 752, !5, i64 760, !5, i64 768, !5, i64 776, !5, i64 784, !5, i64 792, !5, i64 800, !5, i64 808, !5, i64 816, !5, i64 824, !5, i64 832, !5, i64 840, !5, i64 848, !5, i64 856, !5, i64 864, !5, i64 872, !5, i64 880, !5, i64 888, !5, i64 896, !5, i64 904, !5, i64 912, !5, i64 920, !5, i64 928, !5, i64 936, !5, i64 944, !5, i64 952, !5, i64 960, !5, i64 968, !5, i64 976, !5, i64 984, !5, i64 992, !5, i64 1000, !5, i64 1008, !5, i64 1016, !5, i64 1024, !5, i64 1032, !5, i64 1040, !5, i64 1048, !5, i64 1056, !5, i64 1064, !5, i64 1072, !5, i64 1080, !5, i64 1088, !5, i64 1096, !5, i64 1104, !5, i64 1112, !5, i64 1120, !5, i64 1128, !5, i64 1136, !5, i64 1144, !5, i64 1152, !5, i64 1160, !5, i64 1168, !5, i64 1176, !5, i64 1184, !5, i64 1192, !5, i64 1200, !5, i64 1208, !5, i64 1216, !5, i64 1224, !5, i64 1232, !5, i64 1240, !5, i64 1248, !5, i64 1256, !5, i64 1264, !5, i64 1272, !5, i64 1280, !5, i64 1288, !5, i64 1296, !5, i64 1304, !5, i64 1312, !5, i64 1320, !5, i64 1328, !5, i64 1336, !5, i64 1344, !5, i64 1352, !5, i64 1360, !5, i64 1368, !5, i64 1376, !5, i64 1384, !5, i64 1392, !5, i64 1400, !5, i64 1408, !5, i64 1416, !5, i64 1424, !5, i64 1432, !5, i64 1440, !5, i64 1448, !5, i64 1456, !5, i64 1464, !5, i64 1472, !5, i64 1480, !5, i64 1488, !5, i64 1496, !5, i64 1504, !5, i64 1512, !5, i64 1520, !5, i64 1528, !5, i64 1536, !5, i64 1544, !5, i64 1552, !5, i64 1560, !5, i64 1568, !5, i64 1576, !5, i64 1584, !5, i64 1592, !5, i64 1600, !5, i64 1608, !5, i64 1616, !5, i64 1624, !5, i64 1632, !5, i64 1640, !5, i64 1648, !5, i64 1656, !5, i64 1664, !5, i64 1672, !5, i64 1680, !5, i64 1688, !5, i64 1696, !5, i64 1704, !5, i64 1712, !5, i64 1720, !5, i64 1728, !5, i64 1736, !5, i64 1744, !5, i64 1752, !5, i64 1760, !5, i64 1768, !5, i64 1776, !5, i64 1784, !5, i64 1792, !5, i64 1800, !5, i64 1808, !5, i64 1816, !5, i64 1824, !5, i64 1832, !5, i64 1840, !5, i64 1848, !5, i64 1856}
!10 = !{!6, !6, i64 0}
!11 = distinct !{!11, !12}
!12 = !{!"llvm.loop.mustprogress"}
!13 = !{!9, !5, i64 288}
!14 = distinct !{!14, !12}
!15 = !{!9, !5, i64 528}
!16 = distinct !{!16, !12}
!17 = !{!9, !5, i64 928}
!18 = distinct !{!18, !12}
!19 = !{!9, !5, i64 312}
!20 = distinct !{!20, !12}
!21 = !{!9, !5, i64 552}
!22 = distinct !{!22, !12}
!23 = !{!9, !5, i64 952}
!24 = distinct !{!24, !12}
!25 = !{!9, !5, i64 336}
!26 = distinct !{!26, !12}
!27 = !{!9, !5, i64 576}
!28 = distinct !{!28, !12}
!29 = !{!9, !5, i64 976}
!30 = distinct !{!30, !12}
!31 = !{!9, !5, i64 360}
!32 = distinct !{!32, !12}
!33 = !{!9, !5, i64 600}
!34 = distinct !{!34, !12}
!35 = !{!9, !5, i64 1000}
!36 = distinct !{!36, !12}
!37 = !{!9, !5, i64 384}
!38 = distinct !{!38, !12}
!39 = !{!9, !5, i64 624}
!40 = distinct !{!40, !12}
!41 = !{!9, !5, i64 1024}
!42 = distinct !{!42, !12}
!43 = !{!9, !5, i64 408}
!44 = distinct !{!44, !12}
!45 = !{!9, !5, i64 648}
!46 = distinct !{!46, !12}
!47 = !{!9, !5, i64 1048}
!48 = distinct !{!48, !12}
!49 = !{!9, !5, i64 432}
!50 = distinct !{!50, !12}
!51 = !{!9, !5, i64 672}
!52 = distinct !{!52, !12}
!53 = !{!9, !5, i64 1072}
!54 = distinct !{!54, !12}
!55 = !{!9, !5, i64 456}
!56 = distinct !{!56, !12}
!57 = !{!9, !5, i64 696}
!58 = distinct !{!58, !12}
!59 = !{!9, !5, i64 1096}
!60 = distinct !{!60, !12}
!61 = !{!9, !5, i64 480}
!62 = distinct !{!62, !12}
!63 = !{!9, !5, i64 720}
!64 = distinct !{!64, !12}
!65 = !{!9, !5, i64 1120}
!66 = distinct !{!66, !12}
!67 = !{!9, !5, i64 240}
!68 = distinct !{!68, !12}
!69 = !{!9, !5, i64 504}
!70 = distinct !{!70, !12}
!71 = !{!9, !5, i64 744}
!72 = distinct !{!72, !12}
!73 = !{!9, !5, i64 1144}
