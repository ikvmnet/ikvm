using System.Linq;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Encoding;
using IKVM.Java.Tests.Util;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using InstructionFlags = IKVM.Runtime.ClassFile.Method.InstructionFlags;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests for <see cref="MethodAnalyzer"/>. Each test builds a class file, defines it in the runtime to obtain its
    /// <see cref="RuntimeJavaType"/>, and then runs the analyzer over a separately parsed copy of the same class file,
    /// in the same order as the compiler does.
    /// </summary>
    [TestClass]
    public class MethodAnalyzerTests
    {

        const InstructionFlags U = 0;
        const InstructionFlags R = InstructionFlags.Reachable | InstructionFlags.Processed;
        const InstructionFlags B = InstructionFlags.BranchTarget;

        static AnalyzedMethod Analyze(TestClassBuilder builder, string name, string signature)
        {
            return builder.Load().GetMethod(name, signature).Analyze();
        }

        #region Reachability

        [TestMethod]
        public void StraightLineCodeIsReachable()
        {
            var c = new TestClassBuilder("StraightLine");
            c.AddMethod("m", "(I)I", 2, 1, (code, handlers) => code.Iload0().Iconst1().Iadd().Ireturn());

            var m = Analyze(c, "m", "(I)I");
            m.Reachability().Should().Equal(R, R, R, R, U);
        }

        [TestMethod]
        public void CodeAfterGotoIsUnreachable()
        {
            var c = new TestClassBuilder("DeadCode");
            c.AddMethod("m", "(I)V", 0, 1, (code, handlers) =>
            {
                var target = code.DefineLabel();
                code.Goto(target);
                code.Iinc(0, 1);
                code.Return();
                code.MarkLabel(target);
                code.Return();
            });

            var m = Analyze(c, "m", "(I)V");
            m.Reachability().Should().Equal(R, U, U, R | B, U);
            m.CodeInfo.HasState(1).Should().BeFalse();
            m.CodeInfo.HasState(3).Should().BeTrue();
        }

        [TestMethod]
        public void ConditionalBranchReachesBothSuccessors()
        {
            var c = new TestClassBuilder("CondBranch");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var target = code.DefineLabel();
                code.Iload0();
                code.Ifeq(target);
                code.Iconst1();
                code.Ireturn();
                code.MarkLabel(target);
                code.Iconst0();
                code.Ireturn();
            });

            var m = Analyze(c, "m", "(I)I");
            m.Reachability().Should().Equal(R, R, R, R, R | B, R, U);
        }

        [TestMethod]
        public void BranchesLaidOutInReverseOrderAreReachable()
        {
            // each instruction is only reached from an instruction that comes after it
            var c = new TestClassBuilder("ReverseChain");
            c.AddMethod("m", "()V", 0, 0, (code, handlers) =>
            {
                var l1 = code.DefineLabel();
                var l2 = code.DefineLabel();
                var l3 = code.DefineLabel();
                code.Goto(l3);
                code.MarkLabel(l1);
                code.Return();
                code.MarkLabel(l2);
                code.Goto(l1);
                code.MarkLabel(l3);
                code.Goto(l2);
            });

            var m = Analyze(c, "m", "()V");
            m.Reachability().Should().Equal(R, R | B, R | B, R | B, U);
        }

        [TestMethod]
        public void LoopBodyReachedOnlyByBackwardBranchIsReachable()
        {
            var c = new TestClassBuilder("Loop");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var body = code.DefineLabel();
                var test = code.DefineLabel();
                code.Goto(test);
                code.MarkLabel(body);
                code.Iinc(0, -1);
                code.MarkLabel(test);
                code.Iload0();
                code.Ifne(body);
                code.Return();
            });

            var m = Analyze(c, "m", "(I)V");
            m.Reachability().Should().Equal(R, R | B, R | B, R, R, U);
        }

        [TestMethod]
        public void TableSwitchTargetsAreBranchTargets()
        {
            var c = new TestClassBuilder("TableSwitch");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var l0 = code.DefineLabel();
                var l1 = code.DefineLabel();
                var ld = code.DefineLabel();
                code.Iload0();
                code.TableSwitch(ld, 0, [l0, l1]);
                code.MarkLabel(l0);
                code.Iconst0();
                code.Ireturn();
                code.MarkLabel(l1);
                code.Iconst1();
                code.Ireturn();
                code.MarkLabel(ld);
                code.IconstM1();
                code.Ireturn();
            });

            var m = Analyze(c, "m", "(I)I");
            m.Reachability().Should().Equal(R, R, R | B, R, R | B, R, R | B, R, U);
        }

        [TestMethod]
        public void LookupSwitchTargetsAreBranchTargets()
        {
            var c = new TestClassBuilder("LookupSwitch");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var l0 = code.DefineLabel();
                var l1 = code.DefineLabel();
                var ld = code.DefineLabel();
                code.Iload0();
                code.LookupSwitch(ld, [(10, l0), (20, l1)]);
                code.MarkLabel(l0);
                code.Iconst0();
                code.Ireturn();
                code.MarkLabel(l1);
                code.Iconst1();
                code.Ireturn();
                code.MarkLabel(ld);
                code.IconstM1();
                code.Ireturn();
            });

            var m = Analyze(c, "m", "(I)I");
            m.Reachability().Should().Equal(R, R, R | B, R, R | B, R, R | B, R, U);
        }

        [TestMethod]
        public void ExceptionHandlerIsReachableFromTryBlock()
        {
            var c = new TestClassBuilder("Handler");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.Reachability().Should().Equal(R, R, R | B, R, U);
            m.CodeInfo.GetStackHeight(2).Should().Be(1);
            m.CodeInfo.GetRawStackTypeWrapper(2, 0).Name.Should().Be("java.lang.Exception");
        }

        [TestMethod]
        public void ReachabilityCanStartAtHandler()
        {
            var c = new TestClassBuilder("FromHandler");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.Reachability(2).Should().Equal(U, U, R, R, U);
        }

        [TestMethod]
        public void HandlerOfUnreachableTryBlockIsUnreachable()
        {
            var c = new TestClassBuilder("UnreachableTry");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                code.Return();
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.Exceptions.Length.Should().Be(1);
            m.Reachability().Should().Equal(R, U, U, U, U, U);
        }

        [TestMethod]
        public void CatchAnyHandlerThatRethrowsIsFaultBlock()
        {
            var c = new TestClassBuilder("Fault");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Athrow();
                handlers.Add((start, end, handler, default(ClassConstantHandle)));
            });

            var m = Analyze(c, "m", "()V");
            RuntimeVerifierJavaType.IsFaultBlockException(m.CodeInfo.GetRawStackTypeWrapper(2, 0)).Should().BeTrue();
            m.Exceptions.Length.Should().Be(1);
            m.Exceptions[0].isFinally.Should().BeFalse();
            m.Reachability(0, false).Should().Equal(R, R, R | B, U);
            m.Reachability(0, true).Should().Equal(R, R, U, U);
        }

        [TestMethod]
        public void CatchAnyHandlerThatUsesExceptionIsNotFaultBlock()
        {
            var c = new TestClassBuilder("NotFaultUse");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.InvokeStatic(c.H);
                code.Return();
                handlers.Add((start, end, handler, default(ClassConstantHandle)));
            });

            var m = Analyze(c, "m", "()V");
            RuntimeVerifierJavaType.IsFaultBlockException(m.CodeInfo.GetRawStackTypeWrapper(2, 0)).Should().BeFalse();
            m.Reachability(0, true).Should().Equal(R, R, R | B, R, U);
        }

        [TestMethod]
        public void CatchAnyHandlerThatReturnsIsNotFaultBlock()
        {
            var c = new TestClassBuilder("NotFaultReturn");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, default(ClassConstantHandle)));
            });

            var m = Analyze(c, "m", "()V");
            RuntimeVerifierJavaType.IsFaultBlockException(m.CodeInfo.GetRawStackTypeWrapper(2, 0)).Should().BeFalse();
            m.Reachability(0, true).Should().Equal(R, R, R | B, R, U);
        }

        #endregion

        #region Exception table untangling

        [TestMethod]
        public void TryBlockWithoutThrowingInstructionsIsRemoved()
        {
            var c = new TestClassBuilder("NoThrow");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst1();
                code.Istore0();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.Method.ExceptionTable.Length.Should().Be(1);
            m.Exceptions.Length.Should().Be(0);
        }

        [TestMethod]
        public void TryBlockThatCanCatchThreadDeathIsKeptForNonTrivialInstructions()
        {
            // ThreadDeath can be thrown asynchronously, so only blocks of loads, stores and similar are removed
            var c = new TestClassBuilder("ThreadDeathKept");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst1();
                code.Istore0();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Throwable")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 2, 3, 0));
        }

        [TestMethod]
        public void TryBlockThatCanCatchThreadDeathIsRemovedForLoadsAndStores()
        {
            var c = new TestClassBuilder("ThreadDeathRemoved");
            c.AddMethod("m", "(I)V", 1, 2, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iload0();
                code.Istore1();
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Throwable")));
            });

            var m = Analyze(c, "m", "(I)V");
            m.Exceptions.Length.Should().Be(0);
        }

        [TestMethod]
        public void TryBlockWithThrowingInstructionIsKept()
        {
            var c = new TestClassBuilder("Throws");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 1, 2, 0));
        }

        [TestMethod]
        public void TryBlocksSplitAroundReturnAreMerged()
        {
            // javac splits try blocks around return statements
            var c = new TestClassBuilder("MergeReturn");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var s0 = code.Offset;
                code.InvokeStatic(c.F);
                var e0 = code.Offset;
                code.Return();
                var s1 = code.Offset;
                code.InvokeStatic(c.F);
                var e1 = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((s0, e0, handler, c.Class("java/lang/Exception")));
                handlers.Add((s1, e1, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 3, 4, 0));
        }

        [TestMethod]
        public void TryBlockIsSplitAtBranchTargetFromOutside()
        {
            var c = new TestClassBuilder("SplitBranch");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var target = code.DefineLabel();
                code.Iload0();
                code.Ifeq(target);
                var start = code.Offset;
                code.InvokeStatic(c.F);
                code.MarkLabel(target);
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "(I)V");
            m.ExceptionEntries().Should().Equal((2, 3, 5, 0), (3, 4, 5, 0));
        }

        [TestMethod]
        public void SplittingTryBlockCanMakeForwardBranchInsideItCrossParts()
        {
            // the branch from outside splits the block at 5, which puts the branch from 4 to 7 in a different part than
            // its target, so the block must also be split at 7
            var c = new TestClassBuilder("SplitCascadeForward");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var outer = code.DefineLabel();
                var inner = code.DefineLabel();
                code.Iload0();
                code.Ifeq(outer);
                var start = code.Offset;
                code.InvokeStatic(c.F);
                code.Iload0();
                code.Ifne(inner);
                code.MarkLabel(outer);
                code.InvokeStatic(c.F);
                code.InvokeStatic(c.F);
                code.MarkLabel(inner);
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "(I)V");
            m.ExceptionEntries().Should().Equal((2, 5, 9, 0), (5, 7, 9, 0), (7, 8, 9, 0));
        }

        [TestMethod]
        public void SplittingTryBlockCanMakeBackwardBranchInsideItCrossParts()
        {
            // the branch from outside splits the block at 4, which puts the loop branch from 6 back to 3 in a different
            // part than its target, so the block must also be split at 3
            var c = new TestClassBuilder("SplitCascadeBackward");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var loop = code.DefineLabel();
                var outer = code.DefineLabel();
                code.Iload0();
                code.Ifeq(outer);
                var start = code.Offset;
                code.InvokeStatic(c.F);
                code.MarkLabel(loop);
                code.InvokeStatic(c.F);
                code.MarkLabel(outer);
                code.InvokeStatic(c.F);
                code.Iload0();
                code.Ifne(loop);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "(I)V");
            m.ExceptionEntries().Should().Equal((2, 3, 8, 0), (3, 4, 8, 0), (4, 7, 8, 0));
        }

        [TestMethod]
        public void PartiallyOverlappingTryBlocksAreSplit()
        {
            var c = new TestClassBuilder("PartialOverlap");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var a0 = code.Offset;
                code.InvokeStatic(c.F);
                var b0 = code.Offset;
                code.InvokeStatic(c.F);
                var a1 = code.Offset;
                code.InvokeStatic(c.F);
                var b1 = code.Offset;
                code.Return();
                var h1 = code.Offset;
                code.Pop();
                code.Return();
                var h2 = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((a0, a1, h1, c.Class("java/lang/Exception")));
                handlers.Add((b0, b1, h2, c.Class("java/lang/RuntimeException")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 1, 4, 0), (1, 2, 6, 1), (1, 2, 4, 0), (2, 3, 6, 1));
        }

        [TestMethod]
        public void InnerTryBlockListedAfterOuterTryBlockIsSplit()
        {
            // the inner block has lower priority than the outer block, so it cannot simply be nested inside it
            var c = new TestClassBuilder("InnerAfterOuter");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var a0 = code.Offset;
                code.InvokeStatic(c.F);
                var b0 = code.Offset;
                code.InvokeStatic(c.F);
                var b1 = code.Offset;
                code.InvokeStatic(c.F);
                var a1 = code.Offset;
                code.Return();
                var h1 = code.Offset;
                code.Pop();
                code.Return();
                var h2 = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((a0, a1, h1, c.Class("java/lang/Exception")));
                handlers.Add((b0, b1, h2, c.Class("java/lang/RuntimeException")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 1, 4, 0), (1, 2, 6, 1), (1, 2, 4, 0), (2, 3, 4, 0));
        }

        [TestMethod]
        public void InnerTryBlockListedBeforeOuterTryBlockIsNested()
        {
            var c = new TestClassBuilder("InnerBeforeOuter");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var a0 = code.Offset;
                code.InvokeStatic(c.F);
                var b0 = code.Offset;
                code.InvokeStatic(c.F);
                var b1 = code.Offset;
                code.InvokeStatic(c.F);
                var a1 = code.Offset;
                code.Return();
                var h1 = code.Offset;
                code.Pop();
                code.Return();
                var h2 = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((b0, b1, h2, c.Class("java/lang/RuntimeException")));
                handlers.Add((a0, a1, h1, c.Class("java/lang/Exception")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 3, 4, 1), (1, 2, 6, 0));
        }

        [TestMethod]
        public void TryBlockContainingHandlerIsSplitAtHandler()
        {
            var c = new TestClassBuilder("SplitHandler");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                var a0 = code.Offset;
                code.InvokeStatic(c.F);
                var b0 = code.Offset;
                code.InvokeStatic(c.F);
                var b1 = code.Offset;
                code.Return();
                var hb = code.Offset;
                code.Pop();
                code.InvokeStatic(c.F);
                code.Return();
                var ha = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((b0, b1, hb, c.Class("java/lang/Exception")));
                handlers.Add((a0, ha, ha, c.Class("java/lang/RuntimeException")));
            });

            var m = Analyze(c, "m", "()V");
            m.ExceptionEntries().Should().Equal((0, 3, 6, 1), (1, 2, 3, 0), (3, 6, 6, 1));
        }

        #endregion

        #region Type flow

        [TestMethod]
        public void StackHeightsAndTypesAreTracked()
        {
            var c = new TestClassBuilder("Stack");
            c.AddMethod("m", "(I)I", 2, 1, (code, handlers) => code.Iload0().Iconst2().Iadd().Ireturn());

            var m = Analyze(c, "m", "(I)I");
            var INT = JVM.Context.PrimitiveJavaTypeFactory.INT;
            m.CodeInfo.GetStackHeight(0).Should().Be(0);
            m.CodeInfo.GetStackHeight(1).Should().Be(1);
            m.CodeInfo.GetStackHeight(2).Should().Be(2);
            m.CodeInfo.GetStackHeight(3).Should().Be(1);
            m.CodeInfo.GetRawStackTypeWrapper(2, 0).Should().BeSameAs(INT);
            m.CodeInfo.GetRawStackTypeWrapper(2, 1).Should().BeSameAs(INT);
            m.CodeInfo.GetLocalTypeWrapper(0, 0).Should().BeSameAs(INT);
        }

        [TestMethod]
        public void TypesAreMergedAtJoinPoint()
        {
            var c = new TestClassBuilder("Merge");
            c.AddMethod("m", "(I)Ljava/lang/Object;", 1, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var join = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Ldc(c.Constants.GetOrAddString("s"));
                code.Goto(join);
                code.MarkLabel(other);
                code.GetStatic(c.Constants.GetOrAddFieldref("java/lang/Boolean", "TRUE", "Ljava/lang/Boolean;"));
                code.MarkLabel(join);
                code.Areturn();
            });

            var m = Analyze(c, "m", "(I)Ljava/lang/Object;");
            m.CodeInfo.GetRawStackTypeWrapper(3, 0).Name.Should().Be("java.lang.String");
            m.CodeInfo.GetRawStackTypeWrapper(5, 0).Should().BeSameAs(JVM.Context.JavaBase.TypeOfJavaLangObject);
        }

        [TestMethod]
        public void ThisReferenceIsTracked()
        {
            var c = new TestClassBuilder("This");
            c.AddMethod(AccessFlag.Public, "m", "()V", 1, 1, (code, handlers) => code.Aload0().Pop().Return());

            var m = Analyze(c, "m", "()V");
            RuntimeVerifierJavaType.IsThis(m.CodeInfo.GetLocalTypeWrapper(0, 0)).Should().BeTrue();
            RuntimeVerifierJavaType.IsThis(m.CodeInfo.GetRawStackTypeWrapper(1, 0)).Should().BeTrue();
            m.CodeInfo.GetStackTypeWrapper(1, 0).Should().BeSameAs(m.Class.Type);
        }

        [TestMethod]
        public void LdcOfConstantThatCannotThrowIsPatched()
        {
            var c = new TestClassBuilder("Ldc");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                code.Ldc(c.Constants.GetOrAddString("s"));
                code.Pop();
                code.Ldc(c.Class("java/lang/String"));
                code.Pop();
                code.Return();
            });

            var m = Analyze(c, "m", "()V");
            m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__ldc_nothrow);
            m.Instructions[2].NormalizedOpCode.Should().Be(NormalizedByteCode.__ldc);
            m.CodeInfo.GetRawStackTypeWrapper(3, 0).Should().BeSameAs(JVM.Context.JavaBase.TypeOfJavaLangClass);
        }

        #endregion

        #region Verification errors

        [TestMethod]
        public void StackUnderflowIsVerifyError()
        {
            var c = new TestClassBuilder("Underflow");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) => code.Pop().Return());

            var m = c.Load().GetMethod("m", "()V");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        [TestMethod]
        public void StackOverflowIsVerifyError()
        {
            var c = new TestClassBuilder("Overflow");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) => code.Iconst0().Iconst0().Pop2().Return());

            var m = c.Load().GetMethod("m", "()V");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        [TestMethod]
        public void ReturnTypeMismatchIsVerifyError()
        {
            var c = new TestClassBuilder("ReturnMismatch");
            c.AddMethod("m", "()Ljava/lang/Object;", 1, 0, (code, handlers) => code.Iconst0().Areturn());

            var m = c.Load().GetMethod("m", "()Ljava/lang/Object;");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        [TestMethod]
        public void InconsistentStackHeightAtJoinIsVerifyError()
        {
            var c = new TestClassBuilder("InconsistentStack");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var join = code.DefineLabel();
                code.Iload0();
                code.Ifeq(join);
                code.Iconst1();
                code.MarkLabel(join);
                code.Return();
            });

            var m = c.Load().GetMethod("m", "(I)V");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        [TestMethod]
        public void ReadOfUnassignedLocalIsVerifyError()
        {
            var c = new TestClassBuilder("UnassignedLocal");
            c.AddMethod("m", "()V", 1, 2, (code, handlers) => code.Iload1().Pop().Return());

            var m = c.Load().GetMethod("m", "()V");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        [TestMethod]
        public void FallingOffEndOfCodeIsVerifyError()
        {
            var c = new TestClassBuilder("FallOff");
            c.AddMethod("m", "()V", 0, 0, (code, handlers) => code.Nop());

            var m = c.Load().GetMethod("m", "()V");
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        #endregion

        #region Hard errors

        [TestMethod]
        public void InvokeOfMissingMethodIsPatchedToNoSuchMethodError()
        {
            var c = new TestClassBuilder("MissingMethod");
            c.AddMethod("m", "()V", 0, 0, (code, handlers) =>
            {
                code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "missing", "()V"));
                code.Return();
            });

            var m = Analyze(c, "m", "()V");
            m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__static_error);
            m.Instructions[0].HardError.Should().Be(HardError.NoSuchMethodError);
            m.Errors[m.Instructions[0].HardErrorMessageId].Should().Contain("missing");
        }

        [TestMethod]
        public void StaticInvokeOfInstanceMethodIsPatchedToIncompatibleClassChangeError()
        {
            var c = new TestClassBuilder("StaticInstance");
            c.AddMethod(AccessFlag.Public, "instance", "()V", 0, 1, (code, handlers) => code.Return());
            c.AddMethod("m", "()V", 0, 0, (code, handlers) =>
            {
                code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "instance", "()V"));
                code.Return();
            });

            var m = Analyze(c, "m", "()V");
            m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__static_error);
            m.Instructions[0].HardError.Should().Be(HardError.IncompatibleClassChangeError);
        }

        [TestMethod]
        public void AccessOfMissingFieldIsPatchedToNoSuchFieldError()
        {
            var c = new TestClassBuilder("MissingField");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) =>
            {
                code.GetStatic(c.Constants.GetOrAddFieldref(c.Name, "missing", "I"));
                code.Pop();
                code.Return();
            });

            var m = Analyze(c, "m", "()V");
            m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__static_error);
            m.Instructions[0].HardError.Should().Be(HardError.NoSuchFieldError);
            m.Errors[m.Instructions[0].HardErrorMessageId].Should().Contain("missing");
        }

        [TestMethod]
        public void InvokeOfMethodOnMissingClassIsPatched()
        {
            var c = new TestClassBuilder("MissingClass");
            c.AddMethod("m", "()V", 0, 0, (code, handlers) =>
            {
                code.InvokeStatic(c.Constants.GetOrAddMethodref("does/not/Exist", "f", "()V"));
                code.Return();
            });

            var m = Analyze(c, "m", "()V");
            if (m.Class.Type.ClassLoader.DisableDynamicBinding)
            {
                m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__static_error);
                m.Instructions[0].HardError.Should().Be(HardError.NoClassDefFoundError);
            }
            else
            {
                m.Instructions[0].NormalizedOpCode.Should().Be(NormalizedByteCode.__dynamic_invokestatic);
            }
        }

        #endregion

        #region javac output

        const string JavacClassName = "MethodAnalyzerJavacTest";

        const string JavacSource = """
            public class MethodAnalyzerJavacTest {

                static void f() { }

                static void g() { }

                static void h(Throwable t) { }

                static void tryFinally() {
                    try { f(); } finally { g(); }
                }

                static void tryCatch() {
                    try { f(); } catch (RuntimeException e) { h(e); }
                }

                static void tryCatchFinally() {
                    try { f(); } catch (RuntimeException e) { h(e); } finally { g(); }
                }

                static void nestedFinally() {
                    try { try { f(); } finally { g(); } } finally { g(); }
                }

                static void sync(Object o) {
                    synchronized (o) { f(); }
                }

                static int loopFinally(int n) {
                    int s = 0;
                    for (int i = 0; i < n; i++) {
                        try { s += i; f(); } finally { g(); }
                    }
                    return s;
                }

            }
            """;

        static byte[] javacClassBytes;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var compiler = new InMemoryCompiler([new InMemoryCodeUnit(JavacClassName, JavacSource)]);
            compiler.Compile();
            javacClassBytes = compiler.GetClassBytes(JavacClassName);
        }

        static AnalyzedMethod AnalyzeJavac(string name, string signature)
        {
            return LoadedClass.Load(JavacClassName, javacClassBytes).GetMethod(name, signature).Analyze();
        }

        static int[] GotoFinallyIndexes(AnalyzedMethod m)
        {
            return Enumerable.Range(0, m.Instructions.Length).Where(i => m.Instructions[i].NormalizedOpCode == NormalizedByteCode.__goto_finally).ToArray();
        }

        [TestMethod]
        public void JavacTryFinallyIsConvertedToFinallyBlock()
        {
            var m = AnalyzeJavac("tryFinally", "()V");
            m.Exceptions.Length.Should().Be(1);
            m.Exceptions[0].isFinally.Should().BeTrue();

            var handler = m.Exceptions[0].handlerIndex;
            RuntimeVerifierJavaType.IsFaultBlockException(m.CodeInfo.GetRawStackTypeWrapper(handler, 0)).Should().BeTrue();

            var gotos = GotoFinallyIndexes(m);
            gotos.Should().HaveCount(1);
            m.Instructions[gotos[0]].HandlerIndex.Should().Be(handler);

            m.Reachability(0, false)[handler].Should().Be(R | B);
            m.Reachability(0, true)[handler].Should().Be(U);
        }

        [TestMethod]
        public void JavacTryCatchIsNotConvertedToFinallyBlock()
        {
            var m = AnalyzeJavac("tryCatch", "()V");
            m.Exceptions.Length.Should().Be(1);
            m.Exceptions[0].isFinally.Should().BeFalse();
            m.Exceptions[0].catchType.IsNil.Should().BeFalse();
            m.CodeInfo.GetRawStackTypeWrapper(m.Exceptions[0].handlerIndex, 0).Name.Should().Be("java.lang.RuntimeException");
            GotoFinallyIndexes(m).Should().BeEmpty();
        }

        [TestMethod]
        public void JavacTryCatchFinallyKeepsCatchAndConvertsFinally()
        {
            var m = AnalyzeJavac("tryCatchFinally", "()V");
            var entries = Enumerable.Range(0, m.Exceptions.Length).Select(i => m.Exceptions[i]).ToArray();
            entries.Where(i => i.catchType.IsNil == false).Should().ContainSingle().Which.isFinally.Should().BeFalse();
            entries.Should().Contain(i => i.catchType.IsNil && i.isFinally);
            GotoFinallyIndexes(m).Should().NotBeEmpty();
        }

        [TestMethod]
        public void JavacNestedTryFinallyIsNotConvertedToFinallyBlocks()
        {
            // the outer finally block covers the inner finally handler, so the inner try block has two handlers, and
            // neither block has the single exit that conversion requires
            var m = AnalyzeJavac("nestedFinally", "()V");
            m.ExceptionEntries().Should().Equal((0, 1, 9, 1), (0, 1, 3, 0), (1, 3, 9, 1), (3, 7, 9, 1));
            Enumerable.Range(0, m.Exceptions.Length).Select(i => m.Exceptions[i]).Should().OnlyContain(i => i.catchType.IsNil && i.isFinally == false);
            GotoFinallyIndexes(m).Should().BeEmpty();
        }

        [TestMethod]
        public void JavacSynchronizedBlockAsyncGuardIsRemovedAndConvertedToFinallyBlock()
        {
            var m = AnalyzeJavac("sync", "(Ljava/lang/Object;)V");

            // javac protects the monitorexit in the handler with a second handler that covers itself
            m.Method.ExceptionTable.Length.Should().Be(2);
            m.Exceptions.Length.Should().Be(1);
            m.Exceptions[0].isFinally.Should().BeTrue();

            var gotos = GotoFinallyIndexes(m);
            gotos.Should().HaveCount(1);
            m.Instructions[gotos[0]].HandlerIndex.Should().Be(m.Exceptions[0].handlerIndex);
        }

        [TestMethod]
        public void JavacTryFinallyInLoopIsConvertedToFinallyBlock()
        {
            var m = AnalyzeJavac("loopFinally", "(I)I");
            m.Exceptions.Length.Should().Be(1);
            m.Exceptions[0].isFinally.Should().BeTrue();
            GotoFinallyIndexes(m).Should().HaveCount(1);

            // everything except the finally handler is reachable without fault blocks
            var full = m.Reachability(0, false);
            var skip = m.Reachability(0, true);
            var handler = m.Exceptions[0].handlerIndex;
            full[handler].Should().Be(R | B);
            skip[handler].Should().Be(U);
            for (int i = 0; i < handler; i++)
                skip[i].Should().Be(full[i], "instruction {0} is outside the finally handler", i);
        }

        #endregion

    }

}
