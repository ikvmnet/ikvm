; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "thumbv7-pc-windows-msvc19.34.31933"

%union.jvalue = type { i64 }
%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !12

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 36
  %77 = load ptr, ptr %76, align 4, !tbaa !14
  %78 = call arm_aapcs_vfpcc ptr %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %78
}

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_start(ptr) #2

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !12

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 36
  %75 = load ptr, ptr %74, align 4, !tbaa !14
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %76
}

; Function Attrs: mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.va_end(ptr) #2

; Function Attrs: argmemonly mustprogress nocallback nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0(i64 immarg, ptr nocapture) #1

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !15

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 66
  %78 = load ptr, ptr %77, align 4, !tbaa !16
  %79 = call arm_aapcs_vfpcc ptr %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret ptr %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !15

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 66
  %76 = load ptr, ptr %75, align 4, !tbaa !16
  %77 = call arm_aapcs_vfpcc ptr %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret ptr %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !17

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 116
  %77 = load ptr, ptr %76, align 4, !tbaa !18
  %78 = call arm_aapcs_vfpcc ptr %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !17

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 116
  %75 = load ptr, ptr %74, align 4, !tbaa !18
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !19

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 39
  %77 = load ptr, ptr %76, align 4, !tbaa !20
  %78 = call arm_aapcs_vfpcc zeroext i8 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !19

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 39
  %75 = load ptr, ptr %74, align 4, !tbaa !20
  %76 = call arm_aapcs_vfpcc zeroext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !21

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 69
  %78 = load ptr, ptr %77, align 4, !tbaa !22
  %79 = call arm_aapcs_vfpcc zeroext i8 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i8 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !21

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 69
  %76 = load ptr, ptr %75, align 4, !tbaa !22
  %77 = call arm_aapcs_vfpcc zeroext i8 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i8 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !23

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 119
  %77 = load ptr, ptr %76, align 4, !tbaa !24
  %78 = call arm_aapcs_vfpcc zeroext i8 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !23

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 119
  %75 = load ptr, ptr %74, align 4, !tbaa !24
  %76 = call arm_aapcs_vfpcc zeroext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !25

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 42
  %77 = load ptr, ptr %76, align 4, !tbaa !26
  %78 = call arm_aapcs_vfpcc signext i8 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !25

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 42
  %75 = load ptr, ptr %74, align 4, !tbaa !26
  %76 = call arm_aapcs_vfpcc signext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !27

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 72
  %78 = load ptr, ptr %77, align 4, !tbaa !28
  %79 = call arm_aapcs_vfpcc signext i8 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i8 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !27

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 72
  %76 = load ptr, ptr %75, align 4, !tbaa !28
  %77 = call arm_aapcs_vfpcc signext i8 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i8 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !29

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 122
  %77 = load ptr, ptr %76, align 4, !tbaa !30
  %78 = call arm_aapcs_vfpcc signext i8 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i8 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !29

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 122
  %75 = load ptr, ptr %74, align 4, !tbaa !30
  %76 = call arm_aapcs_vfpcc signext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !31

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 45
  %77 = load ptr, ptr %76, align 4, !tbaa !32
  %78 = call arm_aapcs_vfpcc zeroext i16 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !31

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 45
  %75 = load ptr, ptr %74, align 4, !tbaa !32
  %76 = call arm_aapcs_vfpcc zeroext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !33

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 75
  %78 = load ptr, ptr %77, align 4, !tbaa !34
  %79 = call arm_aapcs_vfpcc zeroext i16 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i16 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !33

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 75
  %76 = load ptr, ptr %75, align 4, !tbaa !34
  %77 = call arm_aapcs_vfpcc zeroext i16 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i16 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !35

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 125
  %77 = load ptr, ptr %76, align 4, !tbaa !36
  %78 = call arm_aapcs_vfpcc zeroext i16 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !35

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 125
  %75 = load ptr, ptr %74, align 4, !tbaa !36
  %76 = call arm_aapcs_vfpcc zeroext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !37

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 48
  %77 = load ptr, ptr %76, align 4, !tbaa !38
  %78 = call arm_aapcs_vfpcc signext i16 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !37

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 48
  %75 = load ptr, ptr %74, align 4, !tbaa !38
  %76 = call arm_aapcs_vfpcc signext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !39

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 78
  %78 = load ptr, ptr %77, align 4, !tbaa !40
  %79 = call arm_aapcs_vfpcc signext i16 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i16 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !39

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 78
  %76 = load ptr, ptr %75, align 4, !tbaa !40
  %77 = call arm_aapcs_vfpcc signext i16 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i16 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !41

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 128
  %77 = load ptr, ptr %76, align 4, !tbaa !42
  %78 = call arm_aapcs_vfpcc signext i16 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i16 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !41

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 128
  %75 = load ptr, ptr %74, align 4, !tbaa !42
  %76 = call arm_aapcs_vfpcc signext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !43

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 51
  %77 = load ptr, ptr %76, align 4, !tbaa !44
  %78 = call arm_aapcs_vfpcc i32 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i32 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !43

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 51
  %75 = load ptr, ptr %74, align 4, !tbaa !44
  %76 = call arm_aapcs_vfpcc i32 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i32 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !45

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 81
  %78 = load ptr, ptr %77, align 4, !tbaa !46
  %79 = call arm_aapcs_vfpcc i32 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i32 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !45

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 81
  %76 = load ptr, ptr %75, align 4, !tbaa !46
  %77 = call arm_aapcs_vfpcc i32 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i32 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !47

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 131
  %77 = load ptr, ptr %76, align 4, !tbaa !48
  %78 = call arm_aapcs_vfpcc i32 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i32 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !47

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 131
  %75 = load ptr, ptr %74, align 4, !tbaa !48
  %76 = call arm_aapcs_vfpcc i32 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i32 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !49

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 54
  %77 = load ptr, ptr %76, align 4, !tbaa !50
  %78 = call arm_aapcs_vfpcc i64 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i64 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !49

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 54
  %75 = load ptr, ptr %74, align 4, !tbaa !50
  %76 = call arm_aapcs_vfpcc i64 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i64 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !51

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 84
  %78 = load ptr, ptr %77, align 4, !tbaa !52
  %79 = call arm_aapcs_vfpcc i64 %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret i64 %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !51

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 84
  %76 = load ptr, ptr %75, align 4, !tbaa !52
  %77 = call arm_aapcs_vfpcc i64 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret i64 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !53

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 134
  %77 = load ptr, ptr %76, align 4, !tbaa !54
  %78 = call arm_aapcs_vfpcc i64 %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret i64 %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !53

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 134
  %75 = load ptr, ptr %74, align 4, !tbaa !54
  %76 = call arm_aapcs_vfpcc i64 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret i64 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !55

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 57
  %77 = load ptr, ptr %76, align 4, !tbaa !56
  %78 = call arm_aapcs_vfpcc float %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret float %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !55

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 57
  %75 = load ptr, ptr %74, align 4, !tbaa !56
  %76 = call arm_aapcs_vfpcc float %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret float %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !57

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 87
  %78 = load ptr, ptr %77, align 4, !tbaa !58
  %79 = call arm_aapcs_vfpcc float %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret float %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !57

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 87
  %76 = load ptr, ptr %75, align 4, !tbaa !58
  %77 = call arm_aapcs_vfpcc float %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret float %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !59

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 137
  %77 = load ptr, ptr %76, align 4, !tbaa !60
  %78 = call arm_aapcs_vfpcc float %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret float %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !59

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 137
  %75 = load ptr, ptr %74, align 4, !tbaa !60
  %76 = call arm_aapcs_vfpcc float %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret float %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !61

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 60
  %77 = load ptr, ptr %76, align 4, !tbaa !62
  %78 = call arm_aapcs_vfpcc double %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret double %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !61

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 60
  %75 = load ptr, ptr %74, align 4, !tbaa !62
  %76 = call arm_aapcs_vfpcc double %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret double %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !63

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 90
  %78 = load ptr, ptr %77, align 4, !tbaa !64
  %79 = call arm_aapcs_vfpcc double %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret double %79
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !63

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 90
  %76 = load ptr, ptr %75, align 4, !tbaa !64
  %77 = call arm_aapcs_vfpcc double %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret double %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !65

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 140
  %77 = load ptr, ptr %76, align 4, !tbaa !66
  %78 = call arm_aapcs_vfpcc double %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret double %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !65

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 140
  %75 = load ptr, ptr %74, align 4, !tbaa !66
  %76 = call arm_aapcs_vfpcc double %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret double %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !67

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 30
  %77 = load ptr, ptr %76, align 4, !tbaa !68
  %78 = call arm_aapcs_vfpcc ptr %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret ptr %78
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !67

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 30
  %75 = load ptr, ptr %74, align 4, !tbaa !68
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret ptr %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !69

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 63
  %77 = load ptr, ptr %76, align 4, !tbaa !70
  call arm_aapcs_vfpcc void %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !69

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 63
  %75 = load ptr, ptr %74, align 4, !tbaa !70
  call arm_aapcs_vfpcc void %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  %6 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %6) #4
  call void @llvm.va_start(ptr nonnull %6)
  %7 = load ptr, ptr %6, align 4, !tbaa !5
  %8 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %9 = load ptr, ptr %0, align 4, !tbaa !5
  %10 = load ptr, ptr %9, align 4, !tbaa !9
  %11 = call arm_aapcs_vfpcc i32 %10(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %5) #4
  %12 = shl i32 %11, 3
  %13 = alloca i8, i32 %12, align 8
  %14 = icmp sgt i32 %11, 0
  br i1 %14, label %15, label %75

15:                                               ; preds = %4, %71
  %16 = phi i32 [ %73, %71 ], [ 0, %4 ]
  %17 = phi ptr [ %72, %71 ], [ %7, %4 ]
  %18 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %16
  %19 = load i8, ptr %18, align 1, !tbaa !11
  %20 = sext i8 %19 to i32
  switch i32 %20, label %71 [
    i32 90, label %21
    i32 66, label %26
    i32 83, label %31
    i32 67, label %36
    i32 73, label %41
    i32 74, label %45
    i32 68, label %50
    i32 70, label %58
    i32 76, label %67
  ]

21:                                               ; preds = %15
  %22 = getelementptr inbounds i8, ptr %17, i32 4
  %23 = load i32, ptr %17, align 4
  %24 = trunc i32 %23 to i8
  %25 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %24, ptr %25, align 8, !tbaa !11
  br label %71

26:                                               ; preds = %15
  %27 = getelementptr inbounds i8, ptr %17, i32 4
  %28 = load i32, ptr %17, align 4
  %29 = trunc i32 %28 to i8
  %30 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i8 %29, ptr %30, align 8, !tbaa !11
  br label %71

31:                                               ; preds = %15
  %32 = getelementptr inbounds i8, ptr %17, i32 4
  %33 = load i32, ptr %17, align 4
  %34 = trunc i32 %33 to i16
  %35 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i16 %34, ptr %35, align 8, !tbaa !11
  br label %71

36:                                               ; preds = %15
  %37 = getelementptr inbounds i8, ptr %17, i32 4
  %38 = load i32, ptr %17, align 4
  %39 = and i32 %38, 65535
  %40 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %39, ptr %40, align 8, !tbaa !11
  br label %71

41:                                               ; preds = %15
  %42 = getelementptr inbounds i8, ptr %17, i32 4
  %43 = load i32, ptr %17, align 4
  %44 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i32 %43, ptr %44, align 8, !tbaa !11
  br label %71

45:                                               ; preds = %15
  %46 = getelementptr inbounds i8, ptr %17, i32 4
  %47 = load i32, ptr %17, align 4
  %48 = sext i32 %47 to i64
  %49 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store i64 %48, ptr %49, align 8, !tbaa !11
  br label %71

50:                                               ; preds = %15
  %51 = ptrtoint ptr %17 to i32
  %52 = add i32 %51, 7
  %53 = and i32 %52, -8
  %54 = inttoptr i32 %53 to ptr
  %55 = getelementptr inbounds i8, ptr %54, i32 8
  %56 = load double, ptr %54, align 8
  %57 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store double %56, ptr %57, align 8, !tbaa !11
  br label %71

58:                                               ; preds = %15
  %59 = ptrtoint ptr %17 to i32
  %60 = add i32 %59, 7
  %61 = and i32 %60, -8
  %62 = inttoptr i32 %61 to ptr
  %63 = getelementptr inbounds i8, ptr %62, i32 8
  %64 = load double, ptr %62, align 8
  %65 = fptrunc double %64 to float
  %66 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store float %65, ptr %66, align 8, !tbaa !11
  br label %71

67:                                               ; preds = %15
  %68 = getelementptr inbounds i8, ptr %17, i32 4
  %69 = load ptr, ptr %17, align 4
  %70 = getelementptr inbounds %union.jvalue, ptr %13, i32 %16
  store ptr %69, ptr %70, align 8, !tbaa !11
  br label %71

71:                                               ; preds = %67, %58, %50, %45, %41, %36, %31, %26, %21, %15
  %72 = phi ptr [ %17, %15 ], [ %68, %67 ], [ %63, %58 ], [ %55, %50 ], [ %46, %45 ], [ %42, %41 ], [ %37, %36 ], [ %32, %31 ], [ %27, %26 ], [ %22, %21 ]
  %73 = add nuw nsw i32 %16, 1
  %74 = icmp eq i32 %73, %11
  br i1 %74, label %75, label %15, !llvm.loop !71

75:                                               ; preds = %71, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %76 = load ptr, ptr %0, align 4, !tbaa !5
  %77 = getelementptr inbounds %struct.JNINativeInterface_, ptr %76, i32 0, i32 93
  %78 = load ptr, ptr %77, align 4, !tbaa !72
  call arm_aapcs_vfpcc void %78(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %13) #4
  call void @llvm.stackrestore(ptr %8)
  call void @llvm.va_end(ptr nonnull %6)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %6) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #4
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !9
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #4
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !11
  %18 = sext i8 %17 to i32
  switch i32 %18, label %69 [
    i32 90, label %19
    i32 66, label %24
    i32 83, label %29
    i32 67, label %34
    i32 73, label %39
    i32 74, label %43
    i32 68, label %48
    i32 70, label %56
    i32 76, label %65
  ]

19:                                               ; preds = %13
  %20 = getelementptr inbounds i8, ptr %15, i32 4
  %21 = load i32, ptr %15, align 4
  %22 = trunc i32 %21 to i8
  %23 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %22, ptr %23, align 8, !tbaa !11
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !11
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !11
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !11
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !11
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !11
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !11
  br label %69

56:                                               ; preds = %13
  %57 = ptrtoint ptr %15 to i32
  %58 = add i32 %57, 7
  %59 = and i32 %58, -8
  %60 = inttoptr i32 %59 to ptr
  %61 = getelementptr inbounds i8, ptr %60, i32 8
  %62 = load double, ptr %60, align 8
  %63 = fptrunc double %62 to float
  %64 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store float %63, ptr %64, align 8, !tbaa !11
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !11
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !71

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #4
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 93
  %76 = load ptr, ptr %75, align 4, !tbaa !72
  call arm_aapcs_vfpcc void %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca [257 x i8], align 1
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #4
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %5, align 4, !tbaa !5
  %7 = call ptr @llvm.stacksave()
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %4) #4
  %8 = load ptr, ptr %0, align 4, !tbaa !5
  %9 = load ptr, ptr %8, align 4, !tbaa !9
  %10 = call arm_aapcs_vfpcc i32 %9(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %4) #4
  %11 = shl i32 %10, 3
  %12 = alloca i8, i32 %11, align 8
  %13 = icmp sgt i32 %10, 0
  br i1 %13, label %14, label %74

14:                                               ; preds = %3, %70
  %15 = phi i32 [ %72, %70 ], [ 0, %3 ]
  %16 = phi ptr [ %71, %70 ], [ %6, %3 ]
  %17 = getelementptr inbounds [257 x i8], ptr %4, i32 0, i32 %15
  %18 = load i8, ptr %17, align 1, !tbaa !11
  %19 = sext i8 %18 to i32
  switch i32 %19, label %70 [
    i32 90, label %20
    i32 66, label %25
    i32 83, label %30
    i32 67, label %35
    i32 73, label %40
    i32 74, label %44
    i32 68, label %49
    i32 70, label %57
    i32 76, label %66
  ]

20:                                               ; preds = %14
  %21 = getelementptr inbounds i8, ptr %16, i32 4
  %22 = load i32, ptr %16, align 4
  %23 = trunc i32 %22 to i8
  %24 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %23, ptr %24, align 8, !tbaa !11
  br label %70

25:                                               ; preds = %14
  %26 = getelementptr inbounds i8, ptr %16, i32 4
  %27 = load i32, ptr %16, align 4
  %28 = trunc i32 %27 to i8
  %29 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i8 %28, ptr %29, align 8, !tbaa !11
  br label %70

30:                                               ; preds = %14
  %31 = getelementptr inbounds i8, ptr %16, i32 4
  %32 = load i32, ptr %16, align 4
  %33 = trunc i32 %32 to i16
  %34 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i16 %33, ptr %34, align 8, !tbaa !11
  br label %70

35:                                               ; preds = %14
  %36 = getelementptr inbounds i8, ptr %16, i32 4
  %37 = load i32, ptr %16, align 4
  %38 = and i32 %37, 65535
  %39 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %38, ptr %39, align 8, !tbaa !11
  br label %70

40:                                               ; preds = %14
  %41 = getelementptr inbounds i8, ptr %16, i32 4
  %42 = load i32, ptr %16, align 4
  %43 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i32 %42, ptr %43, align 8, !tbaa !11
  br label %70

44:                                               ; preds = %14
  %45 = getelementptr inbounds i8, ptr %16, i32 4
  %46 = load i32, ptr %16, align 4
  %47 = sext i32 %46 to i64
  %48 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store i64 %47, ptr %48, align 8, !tbaa !11
  br label %70

49:                                               ; preds = %14
  %50 = ptrtoint ptr %16 to i32
  %51 = add i32 %50, 7
  %52 = and i32 %51, -8
  %53 = inttoptr i32 %52 to ptr
  %54 = getelementptr inbounds i8, ptr %53, i32 8
  %55 = load double, ptr %53, align 8
  %56 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store double %55, ptr %56, align 8, !tbaa !11
  br label %70

57:                                               ; preds = %14
  %58 = ptrtoint ptr %16 to i32
  %59 = add i32 %58, 7
  %60 = and i32 %59, -8
  %61 = inttoptr i32 %60 to ptr
  %62 = getelementptr inbounds i8, ptr %61, i32 8
  %63 = load double, ptr %61, align 8
  %64 = fptrunc double %63 to float
  %65 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store float %64, ptr %65, align 8, !tbaa !11
  br label %70

66:                                               ; preds = %14
  %67 = getelementptr inbounds i8, ptr %16, i32 4
  %68 = load ptr, ptr %16, align 4
  %69 = getelementptr inbounds %union.jvalue, ptr %12, i32 %15
  store ptr %68, ptr %69, align 8, !tbaa !11
  br label %70

70:                                               ; preds = %66, %57, %49, %44, %40, %35, %30, %25, %20, %14
  %71 = phi ptr [ %16, %14 ], [ %67, %66 ], [ %62, %57 ], [ %54, %49 ], [ %45, %44 ], [ %41, %40 ], [ %36, %35 ], [ %31, %30 ], [ %26, %25 ], [ %21, %20 ]
  %72 = add nuw nsw i32 %15, 1
  %73 = icmp eq i32 %72, %10
  br i1 %73, label %74, label %14, !llvm.loop !73

74:                                               ; preds = %70, %3
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %4) #4
  %75 = load ptr, ptr %0, align 4, !tbaa !5
  %76 = getelementptr inbounds %struct.JNINativeInterface_, ptr %75, i32 0, i32 143
  %77 = load ptr, ptr %76, align 4, !tbaa !74
  call arm_aapcs_vfpcc void %77(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %12) #4
  call void @llvm.stackrestore(ptr %7)
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #4
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #4
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #4
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !11
  %17 = sext i8 %16 to i32
  switch i32 %17, label %68 [
    i32 90, label %18
    i32 66, label %23
    i32 83, label %28
    i32 67, label %33
    i32 73, label %38
    i32 74, label %42
    i32 68, label %47
    i32 70, label %55
    i32 76, label %64
  ]

18:                                               ; preds = %12
  %19 = getelementptr inbounds i8, ptr %14, i32 4
  %20 = load i32, ptr %14, align 4
  %21 = trunc i32 %20 to i8
  %22 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %21, ptr %22, align 8, !tbaa !11
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !11
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !11
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !11
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !11
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !11
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !11
  br label %68

55:                                               ; preds = %12
  %56 = ptrtoint ptr %14 to i32
  %57 = add i32 %56, 7
  %58 = and i32 %57, -8
  %59 = inttoptr i32 %58 to ptr
  %60 = getelementptr inbounds i8, ptr %59, i32 8
  %61 = load double, ptr %59, align 8
  %62 = fptrunc double %61 to float
  %63 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store float %62, ptr %63, align 8, !tbaa !11
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !11
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !73

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #4
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 143
  %75 = load ptr, ptr %74, align 4, !tbaa !74
  call arm_aapcs_vfpcc void %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #4
  ret void
}

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare ptr @llvm.stacksave() #3

; Function Attrs: nocallback nofree nosync nounwind willreturn
declare void @llvm.stackrestore(ptr) #3

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nocallback nofree nosync nounwind willreturn }
attributes #4 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3}
!llvm.ident = !{!4}

!0 = !{i32 1, !"wchar_size", i32 2}
!1 = !{i32 1, !"min_enum_size", i32 4}
!2 = !{i32 7, !"uwtable", i32 2}
!3 = !{i32 7, !"frame-pointer", i32 2}
!4 = !{!"clang version 15.0.2"}
!5 = !{!6, !6, i64 0}
!6 = !{!"any pointer", !7, i64 0}
!7 = !{!"omnipotent char", !8, i64 0}
!8 = !{!"Simple C/C++ TBAA"}
!9 = !{!10, !6, i64 0}
!10 = !{!"JNINativeInterface_", !6, i64 0, !6, i64 4, !6, i64 8, !6, i64 12, !6, i64 16, !6, i64 20, !6, i64 24, !6, i64 28, !6, i64 32, !6, i64 36, !6, i64 40, !6, i64 44, !6, i64 48, !6, i64 52, !6, i64 56, !6, i64 60, !6, i64 64, !6, i64 68, !6, i64 72, !6, i64 76, !6, i64 80, !6, i64 84, !6, i64 88, !6, i64 92, !6, i64 96, !6, i64 100, !6, i64 104, !6, i64 108, !6, i64 112, !6, i64 116, !6, i64 120, !6, i64 124, !6, i64 128, !6, i64 132, !6, i64 136, !6, i64 140, !6, i64 144, !6, i64 148, !6, i64 152, !6, i64 156, !6, i64 160, !6, i64 164, !6, i64 168, !6, i64 172, !6, i64 176, !6, i64 180, !6, i64 184, !6, i64 188, !6, i64 192, !6, i64 196, !6, i64 200, !6, i64 204, !6, i64 208, !6, i64 212, !6, i64 216, !6, i64 220, !6, i64 224, !6, i64 228, !6, i64 232, !6, i64 236, !6, i64 240, !6, i64 244, !6, i64 248, !6, i64 252, !6, i64 256, !6, i64 260, !6, i64 264, !6, i64 268, !6, i64 272, !6, i64 276, !6, i64 280, !6, i64 284, !6, i64 288, !6, i64 292, !6, i64 296, !6, i64 300, !6, i64 304, !6, i64 308, !6, i64 312, !6, i64 316, !6, i64 320, !6, i64 324, !6, i64 328, !6, i64 332, !6, i64 336, !6, i64 340, !6, i64 344, !6, i64 348, !6, i64 352, !6, i64 356, !6, i64 360, !6, i64 364, !6, i64 368, !6, i64 372, !6, i64 376, !6, i64 380, !6, i64 384, !6, i64 388, !6, i64 392, !6, i64 396, !6, i64 400, !6, i64 404, !6, i64 408, !6, i64 412, !6, i64 416, !6, i64 420, !6, i64 424, !6, i64 428, !6, i64 432, !6, i64 436, !6, i64 440, !6, i64 444, !6, i64 448, !6, i64 452, !6, i64 456, !6, i64 460, !6, i64 464, !6, i64 468, !6, i64 472, !6, i64 476, !6, i64 480, !6, i64 484, !6, i64 488, !6, i64 492, !6, i64 496, !6, i64 500, !6, i64 504, !6, i64 508, !6, i64 512, !6, i64 516, !6, i64 520, !6, i64 524, !6, i64 528, !6, i64 532, !6, i64 536, !6, i64 540, !6, i64 544, !6, i64 548, !6, i64 552, !6, i64 556, !6, i64 560, !6, i64 564, !6, i64 568, !6, i64 572, !6, i64 576, !6, i64 580, !6, i64 584, !6, i64 588, !6, i64 592, !6, i64 596, !6, i64 600, !6, i64 604, !6, i64 608, !6, i64 612, !6, i64 616, !6, i64 620, !6, i64 624, !6, i64 628, !6, i64 632, !6, i64 636, !6, i64 640, !6, i64 644, !6, i64 648, !6, i64 652, !6, i64 656, !6, i64 660, !6, i64 664, !6, i64 668, !6, i64 672, !6, i64 676, !6, i64 680, !6, i64 684, !6, i64 688, !6, i64 692, !6, i64 696, !6, i64 700, !6, i64 704, !6, i64 708, !6, i64 712, !6, i64 716, !6, i64 720, !6, i64 724, !6, i64 728, !6, i64 732, !6, i64 736, !6, i64 740, !6, i64 744, !6, i64 748, !6, i64 752, !6, i64 756, !6, i64 760, !6, i64 764, !6, i64 768, !6, i64 772, !6, i64 776, !6, i64 780, !6, i64 784, !6, i64 788, !6, i64 792, !6, i64 796, !6, i64 800, !6, i64 804, !6, i64 808, !6, i64 812, !6, i64 816, !6, i64 820, !6, i64 824, !6, i64 828, !6, i64 832, !6, i64 836, !6, i64 840, !6, i64 844, !6, i64 848, !6, i64 852, !6, i64 856, !6, i64 860, !6, i64 864, !6, i64 868, !6, i64 872, !6, i64 876, !6, i64 880, !6, i64 884, !6, i64 888, !6, i64 892, !6, i64 896, !6, i64 900, !6, i64 904, !6, i64 908, !6, i64 912, !6, i64 916, !6, i64 920, !6, i64 924, !6, i64 928}
!11 = !{!7, !7, i64 0}
!12 = distinct !{!12, !13}
!13 = !{!"llvm.loop.mustprogress"}
!14 = !{!10, !6, i64 144}
!15 = distinct !{!15, !13}
!16 = !{!10, !6, i64 264}
!17 = distinct !{!17, !13}
!18 = !{!10, !6, i64 464}
!19 = distinct !{!19, !13}
!20 = !{!10, !6, i64 156}
!21 = distinct !{!21, !13}
!22 = !{!10, !6, i64 276}
!23 = distinct !{!23, !13}
!24 = !{!10, !6, i64 476}
!25 = distinct !{!25, !13}
!26 = !{!10, !6, i64 168}
!27 = distinct !{!27, !13}
!28 = !{!10, !6, i64 288}
!29 = distinct !{!29, !13}
!30 = !{!10, !6, i64 488}
!31 = distinct !{!31, !13}
!32 = !{!10, !6, i64 180}
!33 = distinct !{!33, !13}
!34 = !{!10, !6, i64 300}
!35 = distinct !{!35, !13}
!36 = !{!10, !6, i64 500}
!37 = distinct !{!37, !13}
!38 = !{!10, !6, i64 192}
!39 = distinct !{!39, !13}
!40 = !{!10, !6, i64 312}
!41 = distinct !{!41, !13}
!42 = !{!10, !6, i64 512}
!43 = distinct !{!43, !13}
!44 = !{!10, !6, i64 204}
!45 = distinct !{!45, !13}
!46 = !{!10, !6, i64 324}
!47 = distinct !{!47, !13}
!48 = !{!10, !6, i64 524}
!49 = distinct !{!49, !13}
!50 = !{!10, !6, i64 216}
!51 = distinct !{!51, !13}
!52 = !{!10, !6, i64 336}
!53 = distinct !{!53, !13}
!54 = !{!10, !6, i64 536}
!55 = distinct !{!55, !13}
!56 = !{!10, !6, i64 228}
!57 = distinct !{!57, !13}
!58 = !{!10, !6, i64 348}
!59 = distinct !{!59, !13}
!60 = !{!10, !6, i64 548}
!61 = distinct !{!61, !13}
!62 = !{!10, !6, i64 240}
!63 = distinct !{!63, !13}
!64 = !{!10, !6, i64 360}
!65 = distinct !{!65, !13}
!66 = !{!10, !6, i64 560}
!67 = distinct !{!67, !13}
!68 = !{!10, !6, i64 120}
!69 = distinct !{!69, !13}
!70 = !{!10, !6, i64 252}
!71 = distinct !{!71, !13}
!72 = !{!10, !6, i64 372}
!73 = distinct !{!73, !13}
!74 = !{!10, !6, i64 572}
