; ModuleID = 'jni.c'
source_filename = "jni.c"
target datalayout = "e-m:w-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "thumbv7-pc-windows-msvc19.20.0"

%struct.JNINativeInterface_ = type { ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr, ptr }
%union.jvalue = type { i64 }

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 35
  %7 = load ptr, ptr %6, align 4, !tbaa !9
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
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

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !13

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 36
  %75 = load ptr, ptr %74, align 4, !tbaa !15
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 65
  %8 = load ptr, ptr %7, align 4, !tbaa !16
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc ptr %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret ptr %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallNonvirtualObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !17

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 66
  %76 = load ptr, ptr %75, align 4, !tbaa !18
  %77 = call arm_aapcs_vfpcc ptr %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret ptr %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 115
  %7 = load ptr, ptr %6, align 4, !tbaa !19
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_CallStaticObjectMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !20

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 116
  %75 = load ptr, ptr %74, align 4, !tbaa !21
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 38
  %7 = load ptr, ptr %6, align 4, !tbaa !22
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc zeroext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !23

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 39
  %75 = load ptr, ptr %74, align 4, !tbaa !24
  %76 = call arm_aapcs_vfpcc zeroext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 68
  %8 = load ptr, ptr %7, align 4, !tbaa !25
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc zeroext i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallNonvirtualBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !26

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 69
  %76 = load ptr, ptr %75, align 4, !tbaa !27
  %77 = call arm_aapcs_vfpcc zeroext i8 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i8 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 118
  %7 = load ptr, ptr %6, align 4, !tbaa !28
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc zeroext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i8 @JNI_CallStaticBooleanMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !29

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 119
  %75 = load ptr, ptr %74, align 4, !tbaa !30
  %76 = call arm_aapcs_vfpcc zeroext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 41
  %7 = load ptr, ptr %6, align 4, !tbaa !31
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc signext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !32

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 42
  %75 = load ptr, ptr %74, align 4, !tbaa !33
  %76 = call arm_aapcs_vfpcc signext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 71
  %8 = load ptr, ptr %7, align 4, !tbaa !34
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc signext i8 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i8 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallNonvirtualByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !35

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 72
  %76 = load ptr, ptr %75, align 4, !tbaa !36
  %77 = call arm_aapcs_vfpcc signext i8 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i8 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 121
  %7 = load ptr, ptr %6, align 4, !tbaa !37
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc signext i8 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i8 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i8 @JNI_CallStaticByteMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !38

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 122
  %75 = load ptr, ptr %74, align 4, !tbaa !39
  %76 = call arm_aapcs_vfpcc signext i8 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i8 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 44
  %7 = load ptr, ptr %6, align 4, !tbaa !40
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc zeroext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !41

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 45
  %75 = load ptr, ptr %74, align 4, !tbaa !42
  %76 = call arm_aapcs_vfpcc zeroext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 74
  %8 = load ptr, ptr %7, align 4, !tbaa !43
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc zeroext i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallNonvirtualCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !44

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 75
  %76 = load ptr, ptr %75, align 4, !tbaa !45
  %77 = call arm_aapcs_vfpcc zeroext i16 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i16 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 124
  %7 = load ptr, ptr %6, align 4, !tbaa !46
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc zeroext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc zeroext i16 @JNI_CallStaticCharMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !47

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 125
  %75 = load ptr, ptr %74, align 4, !tbaa !48
  %76 = call arm_aapcs_vfpcc zeroext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 47
  %7 = load ptr, ptr %6, align 4, !tbaa !49
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc signext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !50

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 48
  %75 = load ptr, ptr %74, align 4, !tbaa !51
  %76 = call arm_aapcs_vfpcc signext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 77
  %8 = load ptr, ptr %7, align 4, !tbaa !52
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc signext i16 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i16 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallNonvirtualShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !53

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 78
  %76 = load ptr, ptr %75, align 4, !tbaa !54
  %77 = call arm_aapcs_vfpcc signext i16 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i16 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 127
  %7 = load ptr, ptr %6, align 4, !tbaa !55
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc signext i16 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i16 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc signext i16 @JNI_CallStaticShortMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !56

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 128
  %75 = load ptr, ptr %74, align 4, !tbaa !57
  %76 = call arm_aapcs_vfpcc signext i16 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i16 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 50
  %7 = load ptr, ptr %6, align 4, !tbaa !58
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !59

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 51
  %75 = load ptr, ptr %74, align 4, !tbaa !60
  %76 = call arm_aapcs_vfpcc i32 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i32 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 80
  %8 = load ptr, ptr %7, align 4, !tbaa !61
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i32 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallNonvirtualIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !62

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 81
  %76 = load ptr, ptr %75, align 4, !tbaa !63
  %77 = call arm_aapcs_vfpcc i32 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i32 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 130
  %7 = load ptr, ptr %6, align 4, !tbaa !64
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i32 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i32 @JNI_CallStaticIntMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !65

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 131
  %75 = load ptr, ptr %74, align 4, !tbaa !66
  %76 = call arm_aapcs_vfpcc i32 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i32 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 53
  %7 = load ptr, ptr %6, align 4, !tbaa !67
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !68

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 54
  %75 = load ptr, ptr %74, align 4, !tbaa !69
  %76 = call arm_aapcs_vfpcc i64 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i64 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 83
  %8 = load ptr, ptr %7, align 4, !tbaa !70
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc i64 %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret i64 %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallNonvirtualLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !71

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 84
  %76 = load ptr, ptr %75, align 4, !tbaa !72
  %77 = call arm_aapcs_vfpcc i64 %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret i64 %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 133
  %7 = load ptr, ptr %6, align 4, !tbaa !73
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc i64 %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret i64 %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc i64 @JNI_CallStaticLongMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !74

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 134
  %75 = load ptr, ptr %74, align 4, !tbaa !75
  %76 = call arm_aapcs_vfpcc i64 %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret i64 %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 56
  %7 = load ptr, ptr %6, align 4, !tbaa !76
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !77

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 57
  %75 = load ptr, ptr %74, align 4, !tbaa !78
  %76 = call arm_aapcs_vfpcc float %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret float %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 86
  %8 = load ptr, ptr %7, align 4, !tbaa !79
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc float %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret float %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallNonvirtualFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !80

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 87
  %76 = load ptr, ptr %75, align 4, !tbaa !81
  %77 = call arm_aapcs_vfpcc float %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret float %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 136
  %7 = load ptr, ptr %6, align 4, !tbaa !82
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc float %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret float %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc float @JNI_CallStaticFloatMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !83

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 137
  %75 = load ptr, ptr %74, align 4, !tbaa !84
  %76 = call arm_aapcs_vfpcc float %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret float %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 59
  %7 = load ptr, ptr %6, align 4, !tbaa !85
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !86

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 60
  %75 = load ptr, ptr %74, align 4, !tbaa !87
  %76 = call arm_aapcs_vfpcc double %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret double %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 89
  %8 = load ptr, ptr %7, align 4, !tbaa !88
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  %10 = call arm_aapcs_vfpcc double %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret double %10
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallNonvirtualDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !89

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 90
  %76 = load ptr, ptr %75, align 4, !tbaa !90
  %77 = call arm_aapcs_vfpcc double %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret double %77
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 139
  %7 = load ptr, ptr %6, align 4, !tbaa !91
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc double %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret double %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc double @JNI_CallStaticDoubleMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !92

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 140
  %75 = load ptr, ptr %74, align 4, !tbaa !93
  %76 = call arm_aapcs_vfpcc double %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret double %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObject(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 29
  %7 = load ptr, ptr %6, align 4, !tbaa !94
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  %9 = call arm_aapcs_vfpcc ptr %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret ptr %9
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc ptr @JNI_NewObjectV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !95

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 30
  %75 = load ptr, ptr %74, align 4, !tbaa !96
  %76 = call arm_aapcs_vfpcc ptr %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret ptr %76
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 62
  %7 = load ptr, ptr %6, align 4, !tbaa !97
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  call arm_aapcs_vfpcc void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !98

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 63
  %75 = load ptr, ptr %74, align 4, !tbaa !99
  call arm_aapcs_vfpcc void %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ...) local_unnamed_addr #0 {
  %5 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %5) #3
  call void @llvm.va_start(ptr nonnull %5)
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = getelementptr inbounds %struct.JNINativeInterface_, ptr %6, i32 0, i32 92
  %8 = load ptr, ptr %7, align 4, !tbaa !100
  %9 = load ptr, ptr %5, align 4, !tbaa !5
  call arm_aapcs_vfpcc void %8(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %9) #3
  call void @llvm.va_end(ptr nonnull %5)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %5) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallNonvirtualVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef %4) local_unnamed_addr #0 {
  %6 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %6) #3
  %7 = load ptr, ptr %0, align 4, !tbaa !5
  %8 = load ptr, ptr %7, align 4, !tbaa !11
  %9 = call arm_aapcs_vfpcc i32 %8(ptr noundef nonnull %0, ptr noundef %3, ptr noundef nonnull %6) #3
  %10 = shl i32 %9, 3
  %11 = alloca i8, i32 %10, align 8
  %12 = icmp sgt i32 %9, 0
  br i1 %12, label %13, label %73

13:                                               ; preds = %5, %69
  %14 = phi i32 [ %71, %69 ], [ 0, %5 ]
  %15 = phi ptr [ %70, %69 ], [ %4, %5 ]
  %16 = getelementptr inbounds [257 x i8], ptr %6, i32 0, i32 %14
  %17 = load i8, ptr %16, align 1, !tbaa !12
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
  store i8 %22, ptr %23, align 8, !tbaa !12
  br label %69

24:                                               ; preds = %13
  %25 = getelementptr inbounds i8, ptr %15, i32 4
  %26 = load i32, ptr %15, align 4
  %27 = trunc i32 %26 to i8
  %28 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i8 %27, ptr %28, align 8, !tbaa !12
  br label %69

29:                                               ; preds = %13
  %30 = getelementptr inbounds i8, ptr %15, i32 4
  %31 = load i32, ptr %15, align 4
  %32 = trunc i32 %31 to i16
  %33 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i16 %32, ptr %33, align 8, !tbaa !12
  br label %69

34:                                               ; preds = %13
  %35 = getelementptr inbounds i8, ptr %15, i32 4
  %36 = load i32, ptr %15, align 4
  %37 = and i32 %36, 65535
  %38 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %37, ptr %38, align 8, !tbaa !12
  br label %69

39:                                               ; preds = %13
  %40 = getelementptr inbounds i8, ptr %15, i32 4
  %41 = load i32, ptr %15, align 4
  %42 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i32 %41, ptr %42, align 8, !tbaa !12
  br label %69

43:                                               ; preds = %13
  %44 = getelementptr inbounds i8, ptr %15, i32 4
  %45 = load i32, ptr %15, align 4
  %46 = sext i32 %45 to i64
  %47 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store i64 %46, ptr %47, align 8, !tbaa !12
  br label %69

48:                                               ; preds = %13
  %49 = ptrtoint ptr %15 to i32
  %50 = add i32 %49, 7
  %51 = and i32 %50, -8
  %52 = inttoptr i32 %51 to ptr
  %53 = getelementptr inbounds i8, ptr %52, i32 8
  %54 = load double, ptr %52, align 8
  %55 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store double %54, ptr %55, align 8, !tbaa !12
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
  store float %63, ptr %64, align 8, !tbaa !12
  br label %69

65:                                               ; preds = %13
  %66 = getelementptr inbounds i8, ptr %15, i32 4
  %67 = load ptr, ptr %15, align 4
  %68 = getelementptr inbounds %union.jvalue, ptr %11, i32 %14
  store ptr %67, ptr %68, align 8, !tbaa !12
  br label %69

69:                                               ; preds = %19, %24, %29, %34, %39, %43, %48, %56, %65, %13
  %70 = phi ptr [ %15, %13 ], [ %66, %65 ], [ %61, %56 ], [ %53, %48 ], [ %44, %43 ], [ %40, %39 ], [ %35, %34 ], [ %30, %29 ], [ %25, %24 ], [ %20, %19 ]
  %71 = add nuw nsw i32 %14, 1
  %72 = icmp eq i32 %71, %9
  br i1 %72, label %73, label %13, !llvm.loop !101

73:                                               ; preds = %69, %5
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %6) #3
  %74 = load ptr, ptr %0, align 4, !tbaa !5
  %75 = getelementptr inbounds %struct.JNINativeInterface_, ptr %74, i32 0, i32 93
  %76 = load ptr, ptr %75, align 4, !tbaa !102
  call arm_aapcs_vfpcc void %76(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %3, ptr noundef nonnull %11) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethod(ptr noundef %0, ptr noundef %1, ptr noundef %2, ...) local_unnamed_addr #0 {
  %4 = alloca ptr, align 4
  call void @llvm.lifetime.start.p0(i64 4, ptr nonnull %4) #3
  call void @llvm.va_start(ptr nonnull %4)
  %5 = load ptr, ptr %0, align 4, !tbaa !5
  %6 = getelementptr inbounds %struct.JNINativeInterface_, ptr %5, i32 0, i32 142
  %7 = load ptr, ptr %6, align 4, !tbaa !103
  %8 = load ptr, ptr %4, align 4, !tbaa !5
  call arm_aapcs_vfpcc void %7(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef %8) #3
  call void @llvm.va_end(ptr nonnull %4)
  call void @llvm.lifetime.end.p0(i64 4, ptr nonnull %4) #3
  ret void
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local dllexport arm_aapcs_vfpcc void @JNI_CallStaticVoidMethodV(ptr noundef %0, ptr noundef %1, ptr noundef %2, ptr noundef %3) local_unnamed_addr #0 {
  %5 = alloca [257 x i8], align 1
  call void @llvm.lifetime.start.p0(i64 257, ptr nonnull %5) #3
  %6 = load ptr, ptr %0, align 4, !tbaa !5
  %7 = load ptr, ptr %6, align 4, !tbaa !11
  %8 = call arm_aapcs_vfpcc i32 %7(ptr noundef nonnull %0, ptr noundef %2, ptr noundef nonnull %5) #3
  %9 = shl i32 %8, 3
  %10 = alloca i8, i32 %9, align 8
  %11 = icmp sgt i32 %8, 0
  br i1 %11, label %12, label %72

12:                                               ; preds = %4, %68
  %13 = phi i32 [ %70, %68 ], [ 0, %4 ]
  %14 = phi ptr [ %69, %68 ], [ %3, %4 ]
  %15 = getelementptr inbounds [257 x i8], ptr %5, i32 0, i32 %13
  %16 = load i8, ptr %15, align 1, !tbaa !12
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
  store i8 %21, ptr %22, align 8, !tbaa !12
  br label %68

23:                                               ; preds = %12
  %24 = getelementptr inbounds i8, ptr %14, i32 4
  %25 = load i32, ptr %14, align 4
  %26 = trunc i32 %25 to i8
  %27 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i8 %26, ptr %27, align 8, !tbaa !12
  br label %68

28:                                               ; preds = %12
  %29 = getelementptr inbounds i8, ptr %14, i32 4
  %30 = load i32, ptr %14, align 4
  %31 = trunc i32 %30 to i16
  %32 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i16 %31, ptr %32, align 8, !tbaa !12
  br label %68

33:                                               ; preds = %12
  %34 = getelementptr inbounds i8, ptr %14, i32 4
  %35 = load i32, ptr %14, align 4
  %36 = and i32 %35, 65535
  %37 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %36, ptr %37, align 8, !tbaa !12
  br label %68

38:                                               ; preds = %12
  %39 = getelementptr inbounds i8, ptr %14, i32 4
  %40 = load i32, ptr %14, align 4
  %41 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i32 %40, ptr %41, align 8, !tbaa !12
  br label %68

42:                                               ; preds = %12
  %43 = getelementptr inbounds i8, ptr %14, i32 4
  %44 = load i32, ptr %14, align 4
  %45 = sext i32 %44 to i64
  %46 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store i64 %45, ptr %46, align 8, !tbaa !12
  br label %68

47:                                               ; preds = %12
  %48 = ptrtoint ptr %14 to i32
  %49 = add i32 %48, 7
  %50 = and i32 %49, -8
  %51 = inttoptr i32 %50 to ptr
  %52 = getelementptr inbounds i8, ptr %51, i32 8
  %53 = load double, ptr %51, align 8
  %54 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store double %53, ptr %54, align 8, !tbaa !12
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
  store float %62, ptr %63, align 8, !tbaa !12
  br label %68

64:                                               ; preds = %12
  %65 = getelementptr inbounds i8, ptr %14, i32 4
  %66 = load ptr, ptr %14, align 4
  %67 = getelementptr inbounds %union.jvalue, ptr %10, i32 %13
  store ptr %66, ptr %67, align 8, !tbaa !12
  br label %68

68:                                               ; preds = %18, %23, %28, %33, %38, %42, %47, %55, %64, %12
  %69 = phi ptr [ %14, %12 ], [ %65, %64 ], [ %60, %55 ], [ %52, %47 ], [ %43, %42 ], [ %39, %38 ], [ %34, %33 ], [ %29, %28 ], [ %24, %23 ], [ %19, %18 ]
  %70 = add nuw nsw i32 %13, 1
  %71 = icmp eq i32 %70, %8
  br i1 %71, label %72, label %12, !llvm.loop !104

72:                                               ; preds = %68, %4
  call void @llvm.lifetime.end.p0(i64 257, ptr nonnull %5) #3
  %73 = load ptr, ptr %0, align 4, !tbaa !5
  %74 = getelementptr inbounds %struct.JNINativeInterface_, ptr %73, i32 0, i32 143
  %75 = load ptr, ptr %74, align 4, !tbaa !105
  call arm_aapcs_vfpcc void %75(ptr noundef nonnull %0, ptr noundef %1, ptr noundef %2, ptr noundef nonnull %10) #3
  ret void
}

attributes #0 = { alwaysinline nounwind uwtable "frame-pointer"="all" "min-legal-vector-width"="0" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="cortex-a9" "target-features"="+armv7-a,+d32,+dsp,+fp16,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { argmemonly mustprogress nocallback nofree nosync nounwind willreturn }
attributes #2 = { mustprogress nocallback nofree nosync nounwind willreturn }
attributes #3 = { nounwind }

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
!9 = !{!10, !6, i64 140}
!10 = !{!"JNINativeInterface_", !6, i64 0, !6, i64 4, !6, i64 8, !6, i64 12, !6, i64 16, !6, i64 20, !6, i64 24, !6, i64 28, !6, i64 32, !6, i64 36, !6, i64 40, !6, i64 44, !6, i64 48, !6, i64 52, !6, i64 56, !6, i64 60, !6, i64 64, !6, i64 68, !6, i64 72, !6, i64 76, !6, i64 80, !6, i64 84, !6, i64 88, !6, i64 92, !6, i64 96, !6, i64 100, !6, i64 104, !6, i64 108, !6, i64 112, !6, i64 116, !6, i64 120, !6, i64 124, !6, i64 128, !6, i64 132, !6, i64 136, !6, i64 140, !6, i64 144, !6, i64 148, !6, i64 152, !6, i64 156, !6, i64 160, !6, i64 164, !6, i64 168, !6, i64 172, !6, i64 176, !6, i64 180, !6, i64 184, !6, i64 188, !6, i64 192, !6, i64 196, !6, i64 200, !6, i64 204, !6, i64 208, !6, i64 212, !6, i64 216, !6, i64 220, !6, i64 224, !6, i64 228, !6, i64 232, !6, i64 236, !6, i64 240, !6, i64 244, !6, i64 248, !6, i64 252, !6, i64 256, !6, i64 260, !6, i64 264, !6, i64 268, !6, i64 272, !6, i64 276, !6, i64 280, !6, i64 284, !6, i64 288, !6, i64 292, !6, i64 296, !6, i64 300, !6, i64 304, !6, i64 308, !6, i64 312, !6, i64 316, !6, i64 320, !6, i64 324, !6, i64 328, !6, i64 332, !6, i64 336, !6, i64 340, !6, i64 344, !6, i64 348, !6, i64 352, !6, i64 356, !6, i64 360, !6, i64 364, !6, i64 368, !6, i64 372, !6, i64 376, !6, i64 380, !6, i64 384, !6, i64 388, !6, i64 392, !6, i64 396, !6, i64 400, !6, i64 404, !6, i64 408, !6, i64 412, !6, i64 416, !6, i64 420, !6, i64 424, !6, i64 428, !6, i64 432, !6, i64 436, !6, i64 440, !6, i64 444, !6, i64 448, !6, i64 452, !6, i64 456, !6, i64 460, !6, i64 464, !6, i64 468, !6, i64 472, !6, i64 476, !6, i64 480, !6, i64 484, !6, i64 488, !6, i64 492, !6, i64 496, !6, i64 500, !6, i64 504, !6, i64 508, !6, i64 512, !6, i64 516, !6, i64 520, !6, i64 524, !6, i64 528, !6, i64 532, !6, i64 536, !6, i64 540, !6, i64 544, !6, i64 548, !6, i64 552, !6, i64 556, !6, i64 560, !6, i64 564, !6, i64 568, !6, i64 572, !6, i64 576, !6, i64 580, !6, i64 584, !6, i64 588, !6, i64 592, !6, i64 596, !6, i64 600, !6, i64 604, !6, i64 608, !6, i64 612, !6, i64 616, !6, i64 620, !6, i64 624, !6, i64 628, !6, i64 632, !6, i64 636, !6, i64 640, !6, i64 644, !6, i64 648, !6, i64 652, !6, i64 656, !6, i64 660, !6, i64 664, !6, i64 668, !6, i64 672, !6, i64 676, !6, i64 680, !6, i64 684, !6, i64 688, !6, i64 692, !6, i64 696, !6, i64 700, !6, i64 704, !6, i64 708, !6, i64 712, !6, i64 716, !6, i64 720, !6, i64 724, !6, i64 728, !6, i64 732, !6, i64 736, !6, i64 740, !6, i64 744, !6, i64 748, !6, i64 752, !6, i64 756, !6, i64 760, !6, i64 764, !6, i64 768, !6, i64 772, !6, i64 776, !6, i64 780, !6, i64 784, !6, i64 788, !6, i64 792, !6, i64 796, !6, i64 800, !6, i64 804, !6, i64 808, !6, i64 812, !6, i64 816, !6, i64 820, !6, i64 824, !6, i64 828, !6, i64 832, !6, i64 836, !6, i64 840, !6, i64 844, !6, i64 848, !6, i64 852, !6, i64 856, !6, i64 860, !6, i64 864, !6, i64 868, !6, i64 872, !6, i64 876, !6, i64 880, !6, i64 884, !6, i64 888, !6, i64 892, !6, i64 896, !6, i64 900, !6, i64 904, !6, i64 908, !6, i64 912, !6, i64 916, !6, i64 920, !6, i64 924, !6, i64 928}
!11 = !{!10, !6, i64 0}
!12 = !{!7, !7, i64 0}
!13 = distinct !{!13, !14}
!14 = !{!"llvm.loop.mustprogress"}
!15 = !{!10, !6, i64 144}
!16 = !{!10, !6, i64 260}
!17 = distinct !{!17, !14}
!18 = !{!10, !6, i64 264}
!19 = !{!10, !6, i64 460}
!20 = distinct !{!20, !14}
!21 = !{!10, !6, i64 464}
!22 = !{!10, !6, i64 152}
!23 = distinct !{!23, !14}
!24 = !{!10, !6, i64 156}
!25 = !{!10, !6, i64 272}
!26 = distinct !{!26, !14}
!27 = !{!10, !6, i64 276}
!28 = !{!10, !6, i64 472}
!29 = distinct !{!29, !14}
!30 = !{!10, !6, i64 476}
!31 = !{!10, !6, i64 164}
!32 = distinct !{!32, !14}
!33 = !{!10, !6, i64 168}
!34 = !{!10, !6, i64 284}
!35 = distinct !{!35, !14}
!36 = !{!10, !6, i64 288}
!37 = !{!10, !6, i64 484}
!38 = distinct !{!38, !14}
!39 = !{!10, !6, i64 488}
!40 = !{!10, !6, i64 176}
!41 = distinct !{!41, !14}
!42 = !{!10, !6, i64 180}
!43 = !{!10, !6, i64 296}
!44 = distinct !{!44, !14}
!45 = !{!10, !6, i64 300}
!46 = !{!10, !6, i64 496}
!47 = distinct !{!47, !14}
!48 = !{!10, !6, i64 500}
!49 = !{!10, !6, i64 188}
!50 = distinct !{!50, !14}
!51 = !{!10, !6, i64 192}
!52 = !{!10, !6, i64 308}
!53 = distinct !{!53, !14}
!54 = !{!10, !6, i64 312}
!55 = !{!10, !6, i64 508}
!56 = distinct !{!56, !14}
!57 = !{!10, !6, i64 512}
!58 = !{!10, !6, i64 200}
!59 = distinct !{!59, !14}
!60 = !{!10, !6, i64 204}
!61 = !{!10, !6, i64 320}
!62 = distinct !{!62, !14}
!63 = !{!10, !6, i64 324}
!64 = !{!10, !6, i64 520}
!65 = distinct !{!65, !14}
!66 = !{!10, !6, i64 524}
!67 = !{!10, !6, i64 212}
!68 = distinct !{!68, !14}
!69 = !{!10, !6, i64 216}
!70 = !{!10, !6, i64 332}
!71 = distinct !{!71, !14}
!72 = !{!10, !6, i64 336}
!73 = !{!10, !6, i64 532}
!74 = distinct !{!74, !14}
!75 = !{!10, !6, i64 536}
!76 = !{!10, !6, i64 224}
!77 = distinct !{!77, !14}
!78 = !{!10, !6, i64 228}
!79 = !{!10, !6, i64 344}
!80 = distinct !{!80, !14}
!81 = !{!10, !6, i64 348}
!82 = !{!10, !6, i64 544}
!83 = distinct !{!83, !14}
!84 = !{!10, !6, i64 548}
!85 = !{!10, !6, i64 236}
!86 = distinct !{!86, !14}
!87 = !{!10, !6, i64 240}
!88 = !{!10, !6, i64 356}
!89 = distinct !{!89, !14}
!90 = !{!10, !6, i64 360}
!91 = !{!10, !6, i64 556}
!92 = distinct !{!92, !14}
!93 = !{!10, !6, i64 560}
!94 = !{!10, !6, i64 116}
!95 = distinct !{!95, !14}
!96 = !{!10, !6, i64 120}
!97 = !{!10, !6, i64 248}
!98 = distinct !{!98, !14}
!99 = !{!10, !6, i64 252}
!100 = !{!10, !6, i64 368}
!101 = distinct !{!101, !14}
!102 = !{!10, !6, i64 372}
!103 = !{!10, !6, i64 568}
!104 = distinct !{!104, !14}
!105 = !{!10, !6, i64 572}
