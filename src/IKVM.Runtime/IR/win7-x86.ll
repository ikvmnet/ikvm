; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:x-p:32:32-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32-a:0:32-S32"
target triple = "i686-pc-windows-msvc19.34.31933"

%union.jvalue = type { i64 }
%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !10

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 36
  %69 = load ptr, ptr %68, align 4, !tbaa !12
  %70 = call x86_stdcallcc ptr %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %70
}

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !10

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 36
  %67 = load ptr, ptr %66, align 4, !tbaa !12
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %68
}

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !13

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 66
  %70 = load ptr, ptr %69, align 4, !tbaa !14
  %71 = call x86_stdcallcc ptr %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret ptr %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !13

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 66
  %68 = load ptr, ptr %67, align 4, !tbaa !14
  %69 = call x86_stdcallcc ptr %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret ptr %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !15

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 116
  %69 = load ptr, ptr %68, align 4, !tbaa !16
  %70 = call x86_stdcallcc ptr %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !15

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 116
  %67 = load ptr, ptr %66, align 4, !tbaa !16
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !17

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 39
  %69 = load ptr, ptr %68, align 4, !tbaa !18
  %70 = call x86_stdcallcc zeroext i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !17

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 39
  %67 = load ptr, ptr %66, align 4, !tbaa !18
  %68 = call x86_stdcallcc zeroext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !19

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 69
  %70 = load ptr, ptr %69, align 4, !tbaa !20
  %71 = call x86_stdcallcc zeroext i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !19

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 69
  %68 = load ptr, ptr %67, align 4, !tbaa !20
  %69 = call x86_stdcallcc zeroext i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !21

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 119
  %69 = load ptr, ptr %68, align 4, !tbaa !22
  %70 = call x86_stdcallcc zeroext i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !21

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 119
  %67 = load ptr, ptr %66, align 4, !tbaa !22
  %68 = call x86_stdcallcc zeroext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !23

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 42
  %69 = load ptr, ptr %68, align 4, !tbaa !24
  %70 = call x86_stdcallcc signext i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !23

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 42
  %67 = load ptr, ptr %66, align 4, !tbaa !24
  %68 = call x86_stdcallcc signext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !25

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 72
  %70 = load ptr, ptr %69, align 4, !tbaa !26
  %71 = call x86_stdcallcc signext i8 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i8 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !25

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 72
  %68 = load ptr, ptr %67, align 4, !tbaa !26
  %69 = call x86_stdcallcc signext i8 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i8 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !27

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 122
  %69 = load ptr, ptr %68, align 4, !tbaa !28
  %70 = call x86_stdcallcc signext i8 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !27

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 122
  %67 = load ptr, ptr %66, align 4, !tbaa !28
  %68 = call x86_stdcallcc signext i8 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !29

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 45
  %69 = load ptr, ptr %68, align 4, !tbaa !30
  %70 = call x86_stdcallcc zeroext i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !29

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 45
  %67 = load ptr, ptr %66, align 4, !tbaa !30
  %68 = call x86_stdcallcc zeroext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !31

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 75
  %70 = load ptr, ptr %69, align 4, !tbaa !32
  %71 = call x86_stdcallcc zeroext i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !31

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 75
  %68 = load ptr, ptr %67, align 4, !tbaa !32
  %69 = call x86_stdcallcc zeroext i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !33

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 125
  %69 = load ptr, ptr %68, align 4, !tbaa !34
  %70 = call x86_stdcallcc zeroext i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !33

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 125
  %67 = load ptr, ptr %66, align 4, !tbaa !34
  %68 = call x86_stdcallcc zeroext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !35

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 48
  %69 = load ptr, ptr %68, align 4, !tbaa !36
  %70 = call x86_stdcallcc signext i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !35

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 48
  %67 = load ptr, ptr %66, align 4, !tbaa !36
  %68 = call x86_stdcallcc signext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !37

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 78
  %70 = load ptr, ptr %69, align 4, !tbaa !38
  %71 = call x86_stdcallcc signext i16 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i16 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !37

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 78
  %68 = load ptr, ptr %67, align 4, !tbaa !38
  %69 = call x86_stdcallcc signext i16 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i16 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !39

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 128
  %69 = load ptr, ptr %68, align 4, !tbaa !40
  %70 = call x86_stdcallcc signext i16 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !39

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 128
  %67 = load ptr, ptr %66, align 4, !tbaa !40
  %68 = call x86_stdcallcc signext i16 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !41

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 51
  %69 = load ptr, ptr %68, align 4, !tbaa !42
  %70 = call x86_stdcallcc i32 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i32 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !41

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 51
  %67 = load ptr, ptr %66, align 4, !tbaa !42
  %68 = call x86_stdcallcc i32 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i32 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !43

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 81
  %70 = load ptr, ptr %69, align 4, !tbaa !44
  %71 = call x86_stdcallcc i32 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i32 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !43

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 81
  %68 = load ptr, ptr %67, align 4, !tbaa !44
  %69 = call x86_stdcallcc i32 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i32 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !45

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 131
  %69 = load ptr, ptr %68, align 4, !tbaa !46
  %70 = call x86_stdcallcc i32 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i32 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !45

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 131
  %67 = load ptr, ptr %66, align 4, !tbaa !46
  %68 = call x86_stdcallcc i32 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i32 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !47

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 54
  %69 = load ptr, ptr %68, align 4, !tbaa !48
  %70 = call x86_stdcallcc i64 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i64 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !47

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 54
  %67 = load ptr, ptr %66, align 4, !tbaa !48
  %68 = call x86_stdcallcc i64 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i64 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !49

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 84
  %70 = load ptr, ptr %69, align 4, !tbaa !50
  %71 = call x86_stdcallcc i64 %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i64 %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !49

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 84
  %68 = load ptr, ptr %67, align 4, !tbaa !50
  %69 = call x86_stdcallcc i64 %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i64 %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !51

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 134
  %69 = load ptr, ptr %68, align 4, !tbaa !52
  %70 = call x86_stdcallcc i64 %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i64 %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !51

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 134
  %67 = load ptr, ptr %66, align 4, !tbaa !52
  %68 = call x86_stdcallcc i64 %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i64 %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !53

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 57
  %69 = load ptr, ptr %68, align 4, !tbaa !54
  %70 = call x86_stdcallcc float %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret float %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !53

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 57
  %67 = load ptr, ptr %66, align 4, !tbaa !54
  %68 = call x86_stdcallcc float %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret float %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !55

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 87
  %70 = load ptr, ptr %69, align 4, !tbaa !56
  %71 = call x86_stdcallcc float %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret float %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !55

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 87
  %68 = load ptr, ptr %67, align 4, !tbaa !56
  %69 = call x86_stdcallcc float %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret float %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !57

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 137
  %69 = load ptr, ptr %68, align 4, !tbaa !58
  %70 = call x86_stdcallcc float %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret float %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !57

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 137
  %67 = load ptr, ptr %66, align 4, !tbaa !58
  %68 = call x86_stdcallcc float %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret float %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !59

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 60
  %69 = load ptr, ptr %68, align 4, !tbaa !60
  %70 = call x86_stdcallcc double %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret double %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !59

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 60
  %67 = load ptr, ptr %66, align 4, !tbaa !60
  %68 = call x86_stdcallcc double %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret double %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !61

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 90
  %70 = load ptr, ptr %69, align 4, !tbaa !62
  %71 = call x86_stdcallcc double %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret double %71
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !61

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 90
  %68 = load ptr, ptr %67, align 4, !tbaa !62
  %69 = call x86_stdcallcc double %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret double %69
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !63

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 140
  %69 = load ptr, ptr %68, align 4, !tbaa !64
  %70 = call x86_stdcallcc double %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret double %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !63

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 140
  %67 = load ptr, ptr %66, align 4, !tbaa !64
  %68 = call x86_stdcallcc double %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret double %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !65

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 30
  %69 = load ptr, ptr %68, align 4, !tbaa !66
  %70 = call x86_stdcallcc ptr %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %70
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !65

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 30
  %67 = load ptr, ptr %66, align 4, !tbaa !66
  %68 = call x86_stdcallcc ptr %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %68
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !67

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 63
  %69 = load ptr, ptr %68, align 4, !tbaa !68
  call x86_stdcallcc void %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !67

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 63
  %67 = load ptr, ptr %66, align 4, !tbaa !68
  call x86_stdcallcc void %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !3
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !3
  %10 = load ptr, ptr %9, align 4, !tbaa !7
  %11 = call i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 16
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %67

15:                                               ; preds = %4, %63
  %16 = phi i32 [ %65, %63 ], [ 0, %4 ]
  %17 = phi ptr [ %64, %63 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !9
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
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !9
  br label %63

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !9
  br label %63

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !9
  br label %63

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !9
  br label %63

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !9
  br label %63

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !9
  br label %63

50:                                               ; preds = %15
  %51 = getelementptr inbounds i8, ptr %17, i32 8
  %52 = load double, ptr %17, align 4
  %53 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %52, ptr %53, align 8, !tbaa !9
  br label %63

54:                                               ; preds = %15
  %55 = getelementptr inbounds i8, ptr %17, i32 8
  %56 = load double, ptr %17, align 4
  %57 = fptrunc double %56 to float
  %58 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %57, ptr %58, align 8, !tbaa !9
  br label %63

59:                                               ; preds = %15
  %60 = getelementptr inbounds i8, ptr %17, i32 4
  %61 = load ptr, ptr %17, align 4
  %62 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %61, ptr %62, align 8, !tbaa !9
  br label %63

63:                                               ; preds = %59, %54, %50, %45, %41, %36, %31, %26, %21, %15
  %64 = phi ptr [ %17, %15 ], [ %60, %59 ], [ %55, %54 ], [ %51, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %65 = add nuw nsw i32 %16, 1
  %66 = icmp eq i32 %65, %11
  br i1 %66, label %67, label %15, !llvm.loop !69

67:                                               ; preds = %63, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %68 = load ptr, ptr %0, align 4, !tbaa !3
  %69 = getelementptr inbounds %struct.JNINativeInterface_, ptr %68, i32 0, i32 93
  %70 = load ptr, ptr %69, align 4, !tbaa !70
  call x86_stdcallcc void %70(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr nocapture noundef readonly %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !3
  %8 = load ptr, ptr %7, align 4, !tbaa !7
  %9 = call i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 16
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %65

13:                                               ; preds = %5, %61
  %14 = phi i32 [ %63, %61 ], [ 0, %5 ]
  %15 = phi ptr [ %62, %61 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !9
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
  store i8 %22, ptr %23, align 8, !tbaa !9
  br label %61

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !9
  br label %61

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !9
  br label %61

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !9
  br label %61

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !9
  br label %61

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !9
  br label %61

48:                                               ; preds = %13
  %49 = getelementptr inbounds i8, ptr %15, i32 8
  %50 = load double, ptr %15, align 4
  %51 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %50, ptr %51, align 8, !tbaa !9
  br label %61

52:                                               ; preds = %13
  %53 = getelementptr inbounds i8, ptr %15, i32 8
  %54 = load double, ptr %15, align 4
  %55 = fptrunc double %54 to float
  %56 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %55, ptr %56, align 8, !tbaa !9
  br label %61

57:                                               ; preds = %13
  %58 = getelementptr inbounds i8, ptr %15, i32 4
  %59 = load ptr, ptr %15, align 4
  %60 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %59, ptr %60, align 8, !tbaa !9
  br label %61

61:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %52, %57, %13
  %62 = phi ptr [ %15, %13 ], [ %58, %57 ], [ %53, %52 ], [ %49, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %63 = add nuw nsw i32 %14, 1
  %64 = icmp eq i32 %63, %9
  br i1 %64, label %65, label %13, !llvm.loop !69

65:                                               ; preds = %61, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %66 = load ptr, ptr %0, align 4, !tbaa !3
  %67 = getelementptr inbounds %struct.JNINativeInterface_, ptr %66, i32 0, i32 93
  %68 = load ptr, ptr %67, align 4, !tbaa !70
  call x86_stdcallcc void %68(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !3
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !3
  %9 = load ptr, ptr %8, align 4, !tbaa !7
  %10 = call i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 16
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %66

14:                                               ; preds = %3, %62
  %15 = phi i32 [ %64, %62 ], [ 0, %3 ]
  %16 = phi ptr [ %63, %62 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !9
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
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !9
  br label %62

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !9
  br label %62

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !9
  br label %62

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !9
  br label %62

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !9
  br label %62

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !9
  br label %62

49:                                               ; preds = %14
  %50 = getelementptr inbounds i8, ptr %16, i32 8
  %51 = load double, ptr %16, align 4
  %52 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %51, ptr %52, align 8, !tbaa !9
  br label %62

53:                                               ; preds = %14
  %54 = getelementptr inbounds i8, ptr %16, i32 8
  %55 = load double, ptr %16, align 4
  %56 = fptrunc double %55 to float
  %57 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %56, ptr %57, align 8, !tbaa !9
  br label %62

58:                                               ; preds = %14
  %59 = getelementptr inbounds i8, ptr %16, i32 4
  %60 = load ptr, ptr %16, align 4
  %61 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %60, ptr %61, align 8, !tbaa !9
  br label %62

62:                                               ; preds = %58, %53, %49, %44, %40, %35, %30, %25, %20, %14
  %63 = phi ptr [ %16, %14 ], [ %59, %58 ], [ %54, %53 ], [ %50, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %64 = add nuw nsw i32 %15, 1
  %65 = icmp eq i32 %64, %10
  br i1 %65, label %66, label %14, !llvm.loop !71

66:                                               ; preds = %62, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %67 = load ptr, ptr %0, align 4, !tbaa !3
  %68 = getelementptr inbounds %struct.JNINativeInterface_, ptr %67, i32 0, i32 143
  %69 = load ptr, ptr %68, align 4, !tbaa !72
  call x86_stdcallcc void %69(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind
define dso_local dllexport void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr nocapture noundef readonly %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !3
  %7 = load ptr, ptr %6, align 4, !tbaa !7
  %8 = call i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 16
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %64

12:                                               ; preds = %4, %60
  %13 = phi i32 [ %62, %60 ], [ 0, %4 ]
  %14 = phi ptr [ %61, %60 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !9
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
  store i8 %21, ptr %22, align 8, !tbaa !9
  br label %60

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !9
  br label %60

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !9
  br label %60

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !9
  br label %60

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !9
  br label %60

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !9
  br label %60

47:                                               ; preds = %12
  %48 = getelementptr inbounds i8, ptr %14, i32 8
  %49 = load double, ptr %14, align 4
  %50 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %49, ptr %50, align 8, !tbaa !9
  br label %60

51:                                               ; preds = %12
  %52 = getelementptr inbounds i8, ptr %14, i32 8
  %53 = load double, ptr %14, align 4
  %54 = fptrunc double %53 to float
  %55 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %54, ptr %55, align 8, !tbaa !9
  br label %60

56:                                               ; preds = %12
  %57 = getelementptr inbounds i8, ptr %14, i32 4
  %58 = load ptr, ptr %14, align 4
  %59 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %58, ptr %59, align 8, !tbaa !9
  br label %60

60:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %51, %56, %12
  %61 = phi ptr [ %14, %12 ], [ %57, %56 ], [ %52, %51 ], [ %48, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %62 = add nuw nsw i32 %13, 1
  %63 = icmp eq i32 %62, %8
  br i1 %63, label %64, label %12, !llvm.loop !71

64:                                               ; preds = %60, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %65 = load ptr, ptr %0, align 4, !tbaa !3
  %66 = getelementptr inbounds %struct.JNINativeInterface_, ptr %65, i32 0, i32 143
  %67 = load ptr, ptr %66, align 4, !tbaa !72
  call x86_stdcallcc void %67(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret void
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare ptr @llvm.stacksave() #3

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.stackrestore(ptr) #3

attributes #0 = { alwaysinline nounwind "frame-pointer"="none" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="pentium4" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nocallback nofree nosync nounwind willreturn }
attributes #4 = { nounwind }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}

!0 = !{i32 1, !"NumRegisterParameters", i32 0}
!1 = !{i32 1, !"wchar_size", i32 2}
!2 = !{!"clang version 15.0.2"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C/C++ TBAA"}
!7 = !{!8, !4, i64 0}
!8 = !{!"JNINativeInterface_", !4, i64 0, !4, i64 4, !4, i64 8, !4, i64 12, !4, i64 16, !4, i64 20, !4, i64 24, !4, i64 28, !4, i64 32, !4, i64 36, !4, i64 40, !4, i64 44, !4, i64 48, !4, i64 52, !4, i64 56, !4, i64 60, !4, i64 64, !4, i64 68, !4, i64 72, !4, i64 76, !4, i64 80, !4, i64 84, !4, i64 88, !4, i64 92, !4, i64 96, !4, i64 100, !4, i64 104, !4, i64 108, !4, i64 112, !4, i64 116, !4, i64 120, !4, i64 124, !4, i64 128, !4, i64 132, !4, i64 136, !4, i64 140, !4, i64 144, !4, i64 148, !4, i64 152, !4, i64 156, !4, i64 160, !4, i64 164, !4, i64 168, !4, i64 172, !4, i64 176, !4, i64 180, !4, i64 184, !4, i64 188, !4, i64 192, !4, i64 196, !4, i64 200, !4, i64 204, !4, i64 208, !4, i64 212, !4, i64 216, !4, i64 220, !4, i64 224, !4, i64 228, !4, i64 232, !4, i64 236, !4, i64 240, !4, i64 244, !4, i64 248, !4, i64 252, !4, i64 256, !4, i64 260, !4, i64 264, !4, i64 268, !4, i64 272, !4, i64 276, !4, i64 280, !4, i64 284, !4, i64 288, !4, i64 292, !4, i64 296, !4, i64 300, !4, i64 304, !4, i64 308, !4, i64 312, !4, i64 316, !4, i64 320, !4, i64 324, !4, i64 328, !4, i64 332, !4, i64 336, !4, i64 340, !4, i64 344, !4, i64 348, !4, i64 352, !4, i64 356, !4, i64 360, !4, i64 364, !4, i64 368, !4, i64 372, !4, i64 376, !4, i64 380, !4, i64 384, !4, i64 388, !4, i64 392, !4, i64 396, !4, i64 400, !4, i64 404, !4, i64 408, !4, i64 412, !4, i64 416, !4, i64 420, !4, i64 424, !4, i64 428, !4, i64 432, !4, i64 436, !4, i64 440, !4, i64 444, !4, i64 448, !4, i64 452, !4, i64 456, !4, i64 460, !4, i64 464, !4, i64 468, !4, i64 472, !4, i64 476, !4, i64 480, !4, i64 484, !4, i64 488, !4, i64 492, !4, i64 496, !4, i64 500, !4, i64 504, !4, i64 508, !4, i64 512, !4, i64 516, !4, i64 520, !4, i64 524, !4, i64 528, !4, i64 532, !4, i64 536, !4, i64 540, !4, i64 544, !4, i64 548, !4, i64 552, !4, i64 556, !4, i64 560, !4, i64 564, !4, i64 568, !4, i64 572, !4, i64 576, !4, i64 580, !4, i64 584, !4, i64 588, !4, i64 592, !4, i64 596, !4, i64 600, !4, i64 604, !4, i64 608, !4, i64 612, !4, i64 616, !4, i64 620, !4, i64 624, !4, i64 628, !4, i64 632, !4, i64 636, !4, i64 640, !4, i64 644, !4, i64 648, !4, i64 652, !4, i64 656, !4, i64 660, !4, i64 664, !4, i64 668, !4, i64 672, !4, i64 676, !4, i64 680, !4, i64 684, !4, i64 688, !4, i64 692, !4, i64 696, !4, i64 700, !4, i64 704, !4, i64 708, !4, i64 712, !4, i64 716, !4, i64 720, !4, i64 724, !4, i64 728, !4, i64 732, !4, i64 736, !4, i64 740, !4, i64 744, !4, i64 748, !4, i64 752, !4, i64 756, !4, i64 760, !4, i64 764, !4, i64 768, !4, i64 772, !4, i64 776, !4, i64 780, !4, i64 784, !4, i64 788, !4, i64 792, !4, i64 796, !4, i64 800, !4, i64 804, !4, i64 808, !4, i64 812, !4, i64 816, !4, i64 820, !4, i64 824, !4, i64 828, !4, i64 832, !4, i64 836, !4, i64 840, !4, i64 844, !4, i64 848, !4, i64 852, !4, i64 856, !4, i64 860, !4, i64 864, !4, i64 868, !4, i64 872, !4, i64 876, !4, i64 880, !4, i64 884, !4, i64 888, !4, i64 892, !4, i64 896, !4, i64 900, !4, i64 904, !4, i64 908, !4, i64 912, !4, i64 916, !4, i64 920, !4, i64 924, !4, i64 928}
!9 = !{!5, !5, i64 0}
!10 = distinct !{!10, !11}
!11 = !{!"llvm.loop.mustprogress"}
!12 = !{!8, !4, i64 144}
!13 = distinct !{!13, !11}
!14 = !{!8, !4, i64 264}
!15 = distinct !{!15, !11}
!16 = !{!8, !4, i64 464}
!17 = distinct !{!17, !11}
!18 = !{!8, !4, i64 156}
!19 = distinct !{!19, !11}
!20 = !{!8, !4, i64 276}
!21 = distinct !{!21, !11}
!22 = !{!8, !4, i64 476}
!23 = distinct !{!23, !11}
!24 = !{!8, !4, i64 168}
!25 = distinct !{!25, !11}
!26 = !{!8, !4, i64 288}
!27 = distinct !{!27, !11}
!28 = !{!8, !4, i64 488}
!29 = distinct !{!29, !11}
!30 = !{!8, !4, i64 180}
!31 = distinct !{!31, !11}
!32 = !{!8, !4, i64 300}
!33 = distinct !{!33, !11}
!34 = !{!8, !4, i64 500}
!35 = distinct !{!35, !11}
!36 = !{!8, !4, i64 192}
!37 = distinct !{!37, !11}
!38 = !{!8, !4, i64 312}
!39 = distinct !{!39, !11}
!40 = !{!8, !4, i64 512}
!41 = distinct !{!41, !11}
!42 = !{!8, !4, i64 204}
!43 = distinct !{!43, !11}
!44 = !{!8, !4, i64 324}
!45 = distinct !{!45, !11}
!46 = !{!8, !4, i64 524}
!47 = distinct !{!47, !11}
!48 = !{!8, !4, i64 216}
!49 = distinct !{!49, !11}
!50 = !{!8, !4, i64 336}
!51 = distinct !{!51, !11}
!52 = !{!8, !4, i64 536}
!53 = distinct !{!53, !11}
!54 = !{!8, !4, i64 228}
!55 = distinct !{!55, !11}
!56 = !{!8, !4, i64 348}
!57 = distinct !{!57, !11}
!58 = !{!8, !4, i64 548}
!59 = distinct !{!59, !11}
!60 = !{!8, !4, i64 240}
!61 = distinct !{!61, !11}
!62 = !{!8, !4, i64 360}
!63 = distinct !{!63, !11}
!64 = !{!8, !4, i64 560}
!65 = distinct !{!65, !11}
!66 = !{!8, !4, i64 120}
!67 = distinct !{!67, !11}
!68 = !{!8, !4, i64 252}
!69 = distinct !{!69, !11}
!70 = !{!8, !4, i64 372}
!71 = distinct !{!71, !11}
!72 = !{!8, !4, i64 572}
