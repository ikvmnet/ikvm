using System;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Encoding;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using IKVM.CoreLib.Linking;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests that pin down the verifier state <see cref="MethodAnalyzer"/> computes on entry to each instruction. Each frame
    /// is written as <c>stack|locals</c>, with the stack from bottom to top. Types are written as <c>I</c>, <c>J</c>,
    /// <c>F</c> and <c>D</c> for primitives, with a trailing <c>!</c> for extended precision, <c>top</c> for an unusable
    /// local, <c>null</c>, <c>uninitThis</c>, <c>new@n:T</c> for the result of the 'new' at instruction n, <c>this:T</c>,
    /// <c>fault@n</c> for the exception of a fault block with its handler at instruction n, or the type name. The last
    /// frame belongs to the pseudo instruction that follows the code.
    /// </summary>
    [TestClass]
    public class MethodAnalyzerFrameTests
    {

        const byte T_BOOLEAN = 4;
        const byte T_CHAR = 5;
        const byte T_FLOAT = 6;
        const byte T_DOUBLE = 7;
        const byte T_BYTE = 8;
        const byte T_SHORT = 9;
        const byte T_INT = 10;
        const byte T_LONG = 11;

        static AnalyzedMethod Analyze(TestClassBuilder builder, string name, string signature)
        {
            return builder.Load().GetMethod(name, signature).Analyze();
        }

        static string[] Frames(TestClassBuilder builder, string name, string signature)
        {
            return Analyze(builder, name, signature).Frames();
        }

        static void ShouldFailVerification(TestClassBuilder builder, string name, string signature)
        {
            var m = builder.Load().GetMethod(name, signature);
            m.Invoking(i => i.CreateAnalyzer()).Should().Throw<IKVM.Runtime.VerifyError>();
        }

        /// <summary>
        /// Builds 'static Object m(int)' that runs <paramref name="a"/> or <paramref name="b"/> depending on the argument
        /// and returns what they leave on the stack, and returns the frame at the join.
        /// </summary>
        static string MergeAtJoin(string className, Action<TestClassBuilder, CodeBuilder> a, Action<TestClassBuilder, CodeBuilder> b)
        {
            var c = new TestClassBuilder(className);
            int join = 0;
            c.AddMethod("m", "(I)Ljava/lang/Object;", 2, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                a(c, code);
                code.Goto(end);
                code.MarkLabel(other);
                b(c, code);
                code.MarkLabel(end);
                code.Areturn();
            });

            var m = Analyze(c, "m", "(I)Ljava/lang/Object;");
            for (int i = 0; i < m.Instructions.Length; i++)
                if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.Areturn)
                    join = i;

            return m.Frames()[join];
        }

        static Action<TestClassBuilder, CodeBuilder> Ref(string className)
        {
            return (c, code) => code.AconstNull().Checkcast(c.Class(className));
        }

        static readonly Action<TestClassBuilder, CodeBuilder> Null = (c, code) => code.AconstNull();

        #region Constants

        [TestMethod]
        public void ConstantsPushTheirTypes()
        {
            var c = new TestClassBuilder("FrameConstants");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code
                .AconstNull().Pop()
                .IconstM1().Pop()
                .Lconst0().Pop2()
                .Fconst1().Pop()
                .Dconst1().Pop2()
                .Bipush(5).Pop()
                .Sipush(500).Pop()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|", "null|", "|", "I|", "|", "J|", "|", "F|", "|", "D|", "|", "I|", "|", "I|", "|", "-");
        }

        [TestMethod]
        public void LdcPushesTheTypeOfTheConstant()
        {
            var c = new TestClassBuilder("FrameLdc");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code
                .Ldc(c.Constants.GetOrAddInteger(100000)).Pop()
                .Ldc(c.Constants.GetOrAddFloat(1.5f)).Pop()
                .Ldc(c.Constants.GetOrAddString("s")).Pop()
                .Ldc(c.Class("java/lang/String")).Pop()
                .Ldc2W(c.Constants.GetOrAddLong(1L << 40)).Pop2()
                .Ldc2W(c.Constants.GetOrAddDouble(2.5)).Pop2()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|", "I|", "|", "F|", "|", "java.lang.String|", "|", "java.lang.Class|", "|", "J|", "|", "D|", "|", "-");
        }

        #endregion

        #region Arguments and locals

        [TestMethod]
        public void StaticArgumentsStartInLocals()
        {
            var c = new TestClassBuilder("FrameStaticArgs");
            c.AddMethod("m", "(ZBCSIJFDLjava/lang/String;[I)V", 0, 12, (code, handlers) => code.Return());

            Frames(c, "m", "(ZBCSIJFDLjava/lang/String;[I)V").Should().Equal(
                "|I,I,I,I,I,J,top,F,D,top,java.lang.String,[I", "-");
        }

        [TestMethod]
        public void InstanceMethodHasThisInFirstLocal()
        {
            var c = new TestClassBuilder("FrameInstance");
            c.AddMethod(AccessFlag.Public, "m", "(I)V", 0, 3, (code, handlers) => code.Return());

            Frames(c, "m", "(I)V").Should().Equal("|this:FrameInstance,I,top", "-");
        }

        [TestMethod]
        public void StoringThisDropsTheThisMarker()
        {
            var c = new TestClassBuilder("FrameStoreThis");
            c.AddMethod(AccessFlag.Public, "m", "()Ljava/lang/Object;", 1, 2, (code, handlers) => code
                .Aload0()
                .Astore1()
                .Aload1()
                .Areturn());

            Frames(c, "m", "()Ljava/lang/Object;").Should().Equal(
                "|this:FrameStoreThis,top",
                "this:FrameStoreThis|this:FrameStoreThis,top",
                "|this:FrameStoreThis,FrameStoreThis",
                "FrameStoreThis|this:FrameStoreThis,FrameStoreThis",
                "-");
        }

        [TestMethod]
        public void StoringIntoSecondHalfOfLongInvalidatesTheLong()
        {
            var c = new TestClassBuilder("FrameSplitLong");
            c.AddMethod("m", "(J)V", 1, 3, (code, handlers) => code
                .Iconst0()
                .Istore1()
                .Iload1()
                .Pop()
                .Return());

            Frames(c, "m", "(J)V").Should().Equal(
                "|J,top,top",
                "I|J,top,top",
                "|top,I,top",
                "I|top,I,top",
                "|top,I,top",
                "-");
        }

        [TestMethod]
        public void StoringLongInvalidatesTheFollowingLocal()
        {
            var c = new TestClassBuilder("FrameStoreLong");
            c.AddMethod("m", "(II)V", 2, 3, (code, handlers) => code
                .Lconst1()
                .Lstore0()
                .Return());

            Frames(c, "m", "(II)V").Should().Equal(
                "|I,I,top",
                "J|I,I,top",
                "|J,top,top",
                "-");
        }

        [TestMethod]
        public void StoringOverFirstHalfOfLongLeavesSecondHalfUnusable()
        {
            var c = new TestClassBuilder("FrameOverwriteLong");
            c.AddMethod("m", "(J)V", 1, 2, (code, handlers) => code
                .AconstNull()
                .Astore0()
                .Return());

            Frames(c, "m", "(J)V").Should().Equal(
                "|J,top",
                "null|J,top",
                "|null,top",
                "-");
        }

        [TestMethod]
        public void LocalsCanChangeTypeAlongStraightLineCode()
        {
            var c = new TestClassBuilder("FrameRetypeLocal");
            c.AddMethod("m", "()V", 2, 1, (code, handlers) => code
                .Iconst1().Istore0()
                .Fconst1().Fstore0()
                .Ldc(c.Constants.GetOrAddString("s")).Astore0()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|top", "I|top", "|I", "F|I", "|F", "java.lang.String|F", "|java.lang.String", "-");
        }

        [TestMethod]
        public void IincKeepsTheLocalAnInt()
        {
            var c = new TestClassBuilder("FrameIinc");
            c.AddMethod("m", "(I)V", 0, 1, (code, handlers) => code.Iinc(0, 1).Return());

            Frames(c, "m", "(I)V").Should().Equal("|I", "|I", "-");
        }

        #endregion

        #region Arithmetic and conversions

        [TestMethod]
        public void FloatArithmeticProducesExtendedPrecision()
        {
            var c = new TestClassBuilder("FrameExtended");
            c.AddMethod("m", "(FFDD)V", 4, 6, (code, handlers) => code
                .Fload0().Fload1().Fadd().Fneg().Pop()
                .Fload0().Fneg().Pop()
                .Dload2().Dload(4).Dmul().Dneg().Pop2()
                .Dload2().Dneg().Pop2()
                .Return());

            const string L = "F,F,D,top,D,top";
            Frames(c, "m", "(FFDD)V").Should().Equal(
                "|" + L,
                "F|" + L,
                "F,F|" + L,
                "F!|" + L,
                "F!|" + L,
                "|" + L,
                "F|" + L,
                "F|" + L,
                "|" + L,
                "D|" + L,
                "D,D|" + L,
                "D!|" + L,
                "D!|" + L,
                "|" + L,
                "D|" + L,
                "D|" + L,
                "|" + L,
                "-");
        }

        [TestMethod]
        public void InvokeReturningFloatOrDoubleProducesExtendedPrecision()
        {
            var c = new TestClassBuilder("FrameInvokeExtended");
            c.AddMethod("fl", "()F", 1, 0, (code, handlers) => code.Fconst1().Freturn());
            c.AddMethod("db", "()D", 2, 0, (code, handlers) => code.Dconst1().Dreturn());
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code
                .InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "fl", "()F")).Pop()
                .InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "db", "()D")).Pop2()
                .Return());

            Frames(c, "m", "()V").Should().Equal("|", "F!|", "|", "D!|", "|", "-");
        }

        [TestMethod]
        public void IntegerArithmeticKeepsTypes()
        {
            var c = new TestClassBuilder("FrameIntArith");
            c.AddMethod("m", "(IJ)V", 4, 3, (code, handlers) => code
                .Iload0().Iload0().Imul().Ineg().Pop()
                .Lload1().Iload0().Lshl().Lload1().Ladd().Pop2()
                .Lload1().Lload1().Lcmp().Pop()
                .Return());

            const string L = "I,J,top";
            Frames(c, "m", "(IJ)V").Should().Equal(
                "|" + L, "I|" + L, "I,I|" + L, "I|" + L, "I|" + L,
                "|" + L, "J|" + L, "J,I|" + L, "J|" + L, "J,J|" + L, "J|" + L,
                "|" + L, "J|" + L, "J,J|" + L, "I|" + L,
                "|" + L,
                "-");
        }

        [TestMethod]
        public void ConversionsProduceTheirTargetTypes()
        {
            var c = new TestClassBuilder("FrameConversions");
            c.AddMethod("m", "(IJFD)V", 2, 6, (code, handlers) => code
                .Iload0().I2l().Pop2()
                .Iload0().I2f().Pop()
                .Iload0().I2d().Pop2()
                .Lload1().L2i().Pop()
                .Lload1().L2f().Pop()
                .Lload1().L2d().Pop2()
                .Fload3().F2i().Pop()
                .Fload3().F2l().Pop2()
                .Fload3().F2d().Pop2()
                .Dload(4).D2i().Pop()
                .Dload(4).D2l().Pop2()
                .Dload(4).D2f().Pop()
                .Iload0().I2b().Pop()
                .Iload0().I2c().Pop()
                .Iload0().I2s().Pop()
                .Return());

            var frames = Frames(c, "m", "(IJFD)V");
            frames.Should().HaveCount(15 * 3 + 2);
            for (int i = 0; i < 15; i++)
                frames[i * 3].Should().Be("|I,J,top,F,D,top");

            var results = new string[15];
            for (int i = 0; i < 15; i++)
                results[i] = frames[i * 3 + 2].Split('|')[0];

            results.Should().Equal("J", "F", "D", "I", "F", "D", "I", "J", "D", "I", "J", "F", "I", "I", "I");
        }

        #endregion

        #region Stack manipulation

        [TestMethod]
        public void Swap()
        {
            var c = new TestClassBuilder("FrameSwap");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.Iconst1().Fconst1().Swap().Pop().Pop().Return());

            Frames(c, "m", "()V").Should().Equal("|", "I|", "I,F|", "F,I|", "F|", "|", "-");
        }

        [TestMethod]
        public void Dup()
        {
            var c = new TestClassBuilder("FrameDup");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.Fconst1().Dup().Pop().Pop().Return());

            Frames(c, "m", "()V").Should().Equal("|", "F|", "F,F|", "F|", "|", "-");
        }

        [TestMethod]
        public void DupX1()
        {
            var c = new TestClassBuilder("FrameDupX1");
            c.AddMethod("m", "()V", 3, 0, (code, handlers) => code.Iconst1().Fconst1().DupX1().Pop().Pop().Pop().Return());

            Frames(c, "m", "()V").Should().Equal("|", "I|", "I,F|", "F,I,F|", "F,I|", "F|", "|", "-");
        }

        [TestMethod]
        public void DupX2WithThreeSingleWordValues()
        {
            var c = new TestClassBuilder("FrameDupX2a");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Iconst1().Fconst1().AconstNull().DupX2().Pop().Pop().Pop().Pop().Return());

            Frames(c, "m", "()V").Should().Equal(
                "|", "I|", "I,F|", "I,F,null|", "null,I,F,null|", "null,I,F|", "null,I|", "null|", "|", "-");
        }

        [TestMethod]
        public void DupX2OverLong()
        {
            var c = new TestClassBuilder("FrameDupX2b");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Lconst0().Iconst1().DupX2().Pop().Pop2().Pop().Return());

            Frames(c, "m", "()V").Should().Equal("|", "J|", "J,I|", "I,J,I|", "I,J|", "I|", "|", "-");
        }

        [TestMethod]
        public void Dup2WithTwoSingleWordValues()
        {
            var c = new TestClassBuilder("FrameDup2a");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Iconst1().Fconst1().Dup2().Pop2().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "I|", "I,F|", "I,F,I,F|", "I,F|", "|", "-");
        }

        [TestMethod]
        public void Dup2OfLong()
        {
            var c = new TestClassBuilder("FrameDup2b");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Lconst0().Dup2().Pop2().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "J|", "J,J|", "J|", "|", "-");
        }

        [TestMethod]
        public void Dup2X1WithThreeSingleWordValues()
        {
            var c = new TestClassBuilder("FrameDup2X1a");
            c.AddMethod("m", "()V", 5, 0, (code, handlers) => code.AconstNull().Iconst1().Fconst1().Dup2X1().Pop2().Pop().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal(
                "|", "null|", "null,I|", "null,I,F|", "I,F,null,I,F|", "I,F,null|", "I,F|", "|", "-");
        }

        [TestMethod]
        public void Dup2X1OfLongOverSingleWordValue()
        {
            var c = new TestClassBuilder("FrameDup2X1b");
            c.AddMethod("m", "()V", 5, 0, (code, handlers) => code.Iconst1().Lconst0().Dup2X1().Pop2().Pop().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "I|", "I,J|", "J,I,J|", "J,I|", "J|", "|", "-");
        }

        [TestMethod]
        public void Dup2X2WithFourSingleWordValues()
        {
            var c = new TestClassBuilder("FrameDup2X2a");
            c.AddMethod("m", "()V", 6, 0, (code, handlers) => code
                .Ldc(c.Constants.GetOrAddString("s")).AconstNull().Iconst1().Fconst1()
                .Dup2X2()
                .Pop2().Pop2().Pop2()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|",
                "java.lang.String|",
                "java.lang.String,null|",
                "java.lang.String,null,I|",
                "java.lang.String,null,I,F|",
                "I,F,java.lang.String,null,I,F|",
                "I,F,java.lang.String,null|",
                "I,F|",
                "|",
                "-");
        }

        [TestMethod]
        public void Dup2X2OfLongOverTwoSingleWordValues()
        {
            var c = new TestClassBuilder("FrameDup2X2b");
            c.AddMethod("m", "()V", 6, 0, (code, handlers) => code.Iconst1().Fconst1().Lconst0().Dup2X2().Pop2().Pop2().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "I|", "I,F|", "I,F,J|", "J,I,F,J|", "J,I,F|", "J|", "|", "-");
        }

        [TestMethod]
        public void Dup2X2OfTwoSingleWordValuesOverLong()
        {
            var c = new TestClassBuilder("FrameDup2X2c");
            c.AddMethod("m", "()V", 6, 0, (code, handlers) => code.Lconst0().Iconst1().Fconst1().Dup2X2().Pop2().Pop2().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "J|", "J,I|", "J,I,F|", "I,F,J,I,F|", "I,F,J|", "I,F|", "|", "-");
        }

        [TestMethod]
        public void Dup2X2OfLongOverDouble()
        {
            var c = new TestClassBuilder("FrameDup2X2d");
            c.AddMethod("m", "()V", 6, 0, (code, handlers) => code.Dconst0().Lconst0().Dup2X2().Pop2().Pop2().Pop2().Return());

            Frames(c, "m", "()V").Should().Equal("|", "D|", "D,J|", "J,D,J|", "J,D|", "J|", "|", "-");
        }

        #endregion

        #region Objects

        [TestMethod]
        public void NewIsUninitializedUntilConstructorIsCalled()
        {
            var c = new TestClassBuilder("FrameNew");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 0, (code, handlers) => code
                .New(c.Class("java/lang/Object"))
                .Dup()
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Areturn());

            Frames(c, "m", "()Ljava/lang/Object;").Should().Equal(
                "|",
                "new@0:java.lang.Object|",
                "new@0:java.lang.Object,new@0:java.lang.Object|",
                "java.lang.Object|",
                "-");
        }

        [TestMethod]
        public void ConstructorCallInitializesCopiesInLocals()
        {
            var c = new TestClassBuilder("FrameNewLocal");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 1, (code, handlers) => code
                .New(c.Class("java/lang/Object"))
                .Dup()
                .Astore0()
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Aload0()
                .Areturn());

            Frames(c, "m", "()Ljava/lang/Object;").Should().Equal(
                "|top",
                "new@0:java.lang.Object|top",
                "new@0:java.lang.Object,new@0:java.lang.Object|top",
                "new@0:java.lang.Object|new@0:java.lang.Object",
                "|java.lang.Object",
                "java.lang.Object|java.lang.Object",
                "-");
        }

        [TestMethod]
        public void NewAtDifferentInstructionsAreDistinct()
        {
            var c = new TestClassBuilder("FrameTwoNews");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code
                .New(c.Class("java/lang/Object"))
                .New(c.Class("java/lang/Object"))
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Pop()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|",
                "new@0:java.lang.Object|",
                "new@0:java.lang.Object,new@1:java.lang.Object|",
                "new@0:java.lang.Object|",
                "|",
                "-");
        }

        [TestMethod]
        public void ConstructorStartsWithUninitializedThis()
        {
            var c = new TestClassBuilder("FrameCtor");
            c.AddMethod(AccessFlag.Public, "<init>", "()V", 1, 1, (code, handlers) => code
                .Aload0()
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Aload0()
                .Pop()
                .Return());

            Frames(c, "<init>", "()V").Should().Equal(
                "|uninitThis",
                "uninitThis|uninitThis",
                "|this:FrameCtor",
                "this:FrameCtor|this:FrameCtor",
                "|this:FrameCtor",
                "-");
        }

        [TestMethod]
        public void ConstructorMayStoreOwnFieldBeforeCallingSuper()
        {
            var c = new TestClassBuilder("FrameCtorField");
            c.AddField(AccessFlag.Private, "x", "I");
            c.AddMethod(AccessFlag.Public, "<init>", "()V", 2, 1, (code, handlers) => code
                .Aload0()
                .Iconst1()
                .PutField(c.Constants.GetOrAddFieldref(c.Name, "x", "I"))
                .Aload0()
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Return());

            Frames(c, "<init>", "()V").Should().Equal(
                "|uninitThis",
                "uninitThis|uninitThis",
                "uninitThis,I|uninitThis",
                "|uninitThis",
                "uninitThis|uninitThis",
                "|this:FrameCtorField",
                "-");
        }

        [TestMethod]
        public void FieldAccessPushesFieldType()
        {
            var c = new TestClassBuilder("FrameFields");
            c.AddField(AccessFlag.Private, "s", "Ljava/lang/String;");
            c.AddField(AccessFlag.Private | AccessFlag.Static, "j", "J");
            c.AddMethod(AccessFlag.Public, "m", "()V", 2, 1, (code, handlers) => code
                .Aload0()
                .GetField(c.Constants.GetOrAddFieldref(c.Name, "s", "Ljava/lang/String;"))
                .Pop()
                .GetStatic(c.Constants.GetOrAddFieldref(c.Name, "j", "J"))
                .Pop2()
                .Return());

            const string L = "this:FrameFields";
            Frames(c, "m", "()V").Should().Equal(
                "|" + L, "this:FrameFields|" + L, "java.lang.String|" + L, "|" + L, "J|" + L, "|" + L, "-");
        }

        [TestMethod]
        public void CheckcastAndInstanceOf()
        {
            var c = new TestClassBuilder("FrameCasts");
            c.AddMethod("m", "(Ljava/lang/Object;)V", 1, 1, (code, handlers) => code
                .Aload0().Checkcast(c.Class("java/lang/String")).Pop()
                .Aload0().InstanceOf(c.Class("java/lang/String")).Pop()
                .Return());

            const string L = "java.lang.Object";
            Frames(c, "m", "(Ljava/lang/Object;)V").Should().Equal(
                "|" + L, "java.lang.Object|" + L, "java.lang.String|" + L,
                "|" + L, "java.lang.Object|" + L, "I|" + L,
                "|" + L,
                "-");
        }

        [TestMethod]
        public void InvokePushesReturnType()
        {
            var c = new TestClassBuilder("FrameInvoke");
            c.AddMethod(AccessFlag.Public, "m", "()V", 2, 1, (code, handlers) => code
                .Aload0()
                .InvokeVirtual(c.Constants.GetOrAddMethodref("java/lang/Object", "toString", "()Ljava/lang/String;"))
                .InvokeVirtual(c.Constants.GetOrAddMethodref("java/lang/String", "length", "()I"))
                .I2l()
                .InvokeStatic(c.Constants.GetOrAddMethodref("java/lang/Long", "valueOf", "(J)Ljava/lang/Long;"))
                .Pop()
                .Return());

            const string L = "this:FrameInvoke";
            Frames(c, "m", "()V").Should().Equal(
                "|" + L,
                "this:FrameInvoke|" + L,
                "java.lang.String|" + L,
                "I|" + L,
                "J|" + L,
                "java.lang.Long|" + L,
                "|" + L,
                "-");
        }

        #endregion

        #region Arrays

        [TestMethod]
        public void NewarrayCreatesPrimitiveArrays()
        {
            var c = new TestClassBuilder("FrameNewarray");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) => code
                .Iconst1().Newarray(T_BOOLEAN).Pop()
                .Iconst1().Newarray(T_CHAR).Pop()
                .Iconst1().Newarray(T_FLOAT).Pop()
                .Iconst1().Newarray(T_DOUBLE).Pop()
                .Iconst1().Newarray(T_BYTE).Pop()
                .Iconst1().Newarray(T_SHORT).Pop()
                .Iconst1().Newarray(T_INT).Pop()
                .Iconst1().Newarray(T_LONG).Pop()
                .Return());

            var frames = Frames(c, "m", "()V");
            var results = new string[8];
            for (int i = 0; i < 8; i++)
                results[i] = frames[i * 3 + 2];

            results.Should().Equal("[Z|", "[C|", "[F|", "[D|", "[B|", "[S|", "[I|", "[J|");
        }

        [TestMethod]
        public void ReferenceArrays()
        {
            var c = new TestClassBuilder("FrameRefArrays");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code
                .Iconst1().Anewarray(c.Class("java/lang/String")).Pop()
                .Iconst1().Anewarray(c.Class("[Ljava/lang/String;")).Pop()
                .Iconst1().Iconst1().Multianewarray(c.Class("[[I"), 2).Pop()
                .Return());

            Frames(c, "m", "()V").Should().Equal(
                "|", "I|", "[Ljava.lang.String;|", "|",
                "I|", "[[Ljava.lang.String;|", "|",
                "I|", "I,I|", "[[I|", "|",
                "-");
        }

        [TestMethod]
        public void ArrayLoadsPushElementType()
        {
            var c = new TestClassBuilder("FrameArrayLoads");
            c.AddMethod("m", "([Z[B[C[S[I[J[F[D[Ljava/lang/String;[[I)V", 3, 10, (code, handlers) => code
                .Aload0().Iconst0().Baload().Pop()
                .Aload1().Iconst0().Baload().Pop()
                .Aload2().Iconst0().Caload().Pop()
                .Aload3().Iconst0().Saload().Pop()
                .Aload(4).Iconst0().Iaload().Pop()
                .Aload(5).Iconst0().Laload().Pop2()
                .Aload(6).Iconst0().Faload().Pop()
                .Aload(7).Iconst0().Daload().Pop2()
                .Aload(8).Iconst0().Aaload().Pop()
                .Aload(9).Iconst0().Aaload().Pop()
                .Aload(9).Arraylength().Pop()
                .Return());

            var frames = Frames(c, "m", "([Z[B[C[S[I[J[F[D[Ljava/lang/String;[[I)V");
            var results = new string[10];
            for (int i = 0; i < 10; i++)
                results[i] = frames[i * 4 + 3].Split('|')[0];

            results.Should().Equal("I", "I", "I", "I", "I", "J", "F", "D", "java.lang.String", "[I");
            frames[10 * 4 + 2].Split('|')[0].Should().Be("I");
        }

        [TestMethod]
        public void AaloadOfNullArrayPushesNull()
        {
            var c = new TestClassBuilder("FrameAaloadNull");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.AconstNull().Iconst0().Aaload().Pop().Return());

            Frames(c, "m", "()V").Should().Equal("|", "null|", "null,I|", "null|", "|", "-");
        }

        #endregion

        #region Merges

        [TestMethod]
        public void MergeOfIdenticalTypesKeepsTheType()
        {
            MergeAtJoin("FrameMergeSame", Ref("java/lang/String"), Ref("java/lang/String")).Should().StartWith("java.lang.String|");
        }

        [TestMethod]
        public void MergeOfUnrelatedClassesIsTheirCommonSuperclass()
        {
            MergeAtJoin("FrameMergeNumber", Ref("java/lang/Integer"), Ref("java/lang/Long")).Should().StartWith("java.lang.Number|");
            MergeAtJoin("FrameMergeList", Ref("java/util/ArrayList"), Ref("java/util/LinkedList")).Should().StartWith("java.util.AbstractList|");
            MergeAtJoin("FrameMergeThrowable", Ref("java/lang/RuntimeException"), Ref("java/lang/Error")).Should().StartWith("java.lang.Throwable|");
            MergeAtJoin("FrameMergeObject", Ref("java/lang/String"), Ref("java/lang/Integer")).Should().StartWith("java.lang.Object|");
        }

        [TestMethod]
        public void MergeOfSubclassAndSuperclassIsTheSuperclass()
        {
            MergeAtJoin("FrameMergeSub", Ref("java/lang/RuntimeException"), Ref("java/lang/Exception")).Should().StartWith("java.lang.Exception|");
            MergeAtJoin("FrameMergeSuper", Ref("java/lang/Exception"), Ref("java/lang/RuntimeException")).Should().StartWith("java.lang.Exception|");
        }

        [TestMethod]
        public void MergeWithNullIsTheOtherType()
        {
            MergeAtJoin("FrameMergeNullA", Ref("java/lang/String"), Null).Should().StartWith("java.lang.String|");
            MergeAtJoin("FrameMergeNullB", Null, Ref("java/lang/String")).Should().StartWith("java.lang.String|");
            MergeAtJoin("FrameMergeNullC", Null, Null).Should().StartWith("null|");
            MergeAtJoin("FrameMergeNullD", Null, Ref("[I")).Should().StartWith("[I|");
        }

        [TestMethod]
        public void MergeWithObjectIsObject()
        {
            MergeAtJoin("FrameMergeObjA", Ref("java/lang/Object"), Ref("java/util/List")).Should().StartWith("java.lang.Object|");
            MergeAtJoin("FrameMergeObjB", Ref("[Ljava/lang/String;"), Ref("java/lang/Object")).Should().StartWith("java.lang.Object|");
        }

        [TestMethod]
        public void MergeWithInterfaceIsTheInterfaceOnlyIfImplemented()
        {
            MergeAtJoin("FrameMergeIfaceA", Ref("java/util/List"), Ref("java/util/ArrayList")).Should().StartWith("java.util.List|");
            MergeAtJoin("FrameMergeIfaceB", Ref("java/util/ArrayList"), Ref("java/util/List")).Should().StartWith("java.util.List|");
            MergeAtJoin("FrameMergeIfaceC", Ref("java/util/List"), Ref("java/lang/String")).Should().StartWith("java.lang.Object|");
        }

        [TestMethod]
        public void MergeOfTwoInterfacesIsObject()
        {
            MergeAtJoin("FrameMergeIfaces", Ref("java/util/List"), Ref("java/util/Set")).Should().StartWith("java.lang.Object|");
            MergeAtJoin("FrameMergeIfaceSame", Ref("java/util/List"), Ref("java/util/List")).Should().StartWith("java.util.List|");
        }

        [TestMethod]
        public void MergeOfReferenceArraysMergesElementTypes()
        {
            MergeAtJoin("FrameMergeArrA", Ref("[Ljava/lang/Integer;"), Ref("[Ljava/lang/Long;")).Should().StartWith("[Ljava.lang.Number;|");
            MergeAtJoin("FrameMergeArrB", Ref("[Ljava/util/ArrayList;"), Ref("[Ljava/util/LinkedList;")).Should().StartWith("[Ljava.util.AbstractList;|");
            MergeAtJoin("FrameMergeArrC", Ref("[[Ljava/lang/Integer;"), Ref("[[Ljava/lang/Long;")).Should().StartWith("[[Ljava.lang.Number;|");
        }

        [TestMethod]
        public void MergeOfArraysOfDifferentRankIsObjectArrayOfTheSmallerRank()
        {
            MergeAtJoin("FrameMergeRankA", Ref("[Ljava/lang/String;"), Ref("[[Ljava/lang/String;")).Should().StartWith("[Ljava.lang.Object;|");
            MergeAtJoin("FrameMergeRankB", Ref("[[[Ljava/lang/String;"), Ref("[[Ljava/lang/String;")).Should().StartWith("[[Ljava.lang.Object;|");
        }

        [TestMethod]
        public void MergeOfPrimitiveArraysIsObjectAtTheirRank()
        {
            MergeAtJoin("FrameMergePrimArrA", Ref("[I"), Ref("[J")).Should().StartWith("java.lang.Object|");
            MergeAtJoin("FrameMergePrimArrB", Ref("[[I"), Ref("[[J")).Should().StartWith("[Ljava.lang.Object;|");
            MergeAtJoin("FrameMergePrimArrC", Ref("[I"), Ref("[Ljava/lang/Object;")).Should().StartWith("java.lang.Object|");
            MergeAtJoin("FrameMergePrimArrD", Ref("[[I"), Ref("[Ljava/lang/String;")).Should().StartWith("[Ljava.lang.Object;|");
        }

        [TestMethod]
        public void MergeOfArrayAndNonArrayIsObject()
        {
            MergeAtJoin("FrameMergeArrObjA", Ref("[Ljava/lang/String;"), Ref("java/lang/String")).Should().StartWith("java.lang.Object|");
            MergeAtJoin("FrameMergeArrObjB", Ref("java/lang/Integer"), Ref("[I")).Should().StartWith("java.lang.Object|");
        }

        [TestMethod]
        public void MergeOfThisAndItsClassIsTheClass()
        {
            var c = new TestClassBuilder("FrameMergeThis");
            int join = 0;
            c.AddMethod(AccessFlag.Public, "m", "(I)Ljava/lang/Object;", 1, 2, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload1();
                code.Ifeq(other);
                code.Aload0();
                code.Goto(end);
                code.MarkLabel(other);
                code.AconstNull().Checkcast(c.Class(c.Name));
                code.MarkLabel(end);
                code.Areturn();
            });

            var m = Analyze(c, "m", "(I)Ljava/lang/Object;");
            for (int i = 0; i < m.Instructions.Length; i++)
                if (m.Instructions[i].NormalizedOpCode == NormalizedOpCode.Areturn)
                    join = i;

            m.Frames()[join].Should().Be("FrameMergeThis|this:FrameMergeThis,I");
        }

        [TestMethod]
        public void MergeOfIncompatibleLocalsMakesTheLocalUnusable()
        {
            var c = new TestClassBuilder("FrameMergeLocals");
            c.AddMethod("m", "(I)V", 1, 4, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Iconst1().Istore1();
                code.Fconst1().Fstore2();
                code.Ldc(c.Constants.GetOrAddString("s")).Astore3();
                code.Goto(end);
                code.MarkLabel(other);
                code.Ldc(c.Constants.GetOrAddString("s")).Astore1();
                code.Fconst1().Fstore2();
                code.Ldc(c.Class("java/lang/Integer")).Pop().AconstNull().Astore3();
                code.MarkLabel(end);
                code.Return();
            });

            var frames = Frames(c, "m", "(I)V");
            frames[frames.Length - 2].Should().Be("|I,top,F,java.lang.String");
        }

        [TestMethod]
        public void MergeOfExtendedAndPlainFloatIsExtended()
        {
            var c = new TestClassBuilder("FrameMergeExtended");
            c.AddMethod("m", "(IF)F", 2, 2, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Fload1().Fload1().Fadd();
                code.Goto(end);
                code.MarkLabel(other);
                code.Fload1();
                code.MarkLabel(end);
                code.Freturn();
            });

            var frames = Frames(c, "m", "(IF)F");
            frames[frames.Length - 2].Should().Be("F!|I,F");
        }

        [TestMethod]
        public void MergeOfExtendedAndPlainDoubleIsExtended()
        {
            var c = new TestClassBuilder("FrameMergeExtendedD");
            c.AddMethod("m", "(ID)D", 4, 3, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Dload1();
                code.Goto(end);
                code.MarkLabel(other);
                code.Dload1().Dload1().Dmul();
                code.MarkLabel(end);
                code.Dreturn();
            });

            var frames = Frames(c, "m", "(ID)D");
            frames[frames.Length - 2].Should().Be("D!|I,D,top");
        }

        [TestMethod]
        public void LoopBackEdgeWidensTypes()
        {
            var c = new TestClassBuilder("FrameLoop");
            c.AddMethod("m", "(I)V", 1, 2, (code, handlers) =>
            {
                var head = code.DefineLabel();
                var end = code.DefineLabel();
                code.Ldc(c.Constants.GetOrAddString("s")).Astore1();
                code.MarkLabel(head);
                code.Iload0().Ifeq(end);
                code.Ldc(c.Class("java/lang/Integer")).Pop();
                code.AconstNull().Checkcast(c.Class("java/lang/Integer")).Astore1();
                code.Iinc(0, -1);
                code.Goto(head);
                code.MarkLabel(end);
                code.Return();
            });

            var m = Analyze(c, "m", "(I)V");
            var frames = m.Frames();
            frames[2].Should().Be("|I,java.lang.Object");
            frames[frames.Length - 2].Should().Be("|I,java.lang.Object");
        }

        #endregion

        #region Exception handlers

        [TestMethod]
        public void TypedHandlerStartsWithCaughtException()
        {
            var c = new TestClassBuilder("FrameCatch");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/RuntimeException")));
            });

            Frames(c, "m", "(I)V").Should().Equal(
                "|I", "|I", "java.lang.RuntimeException|I", "|I", "-");
        }

        [TestMethod]
        public void CatchAnyHandlerThatUsesExceptionGetsThrowable()
        {
            var c = new TestClassBuilder("FrameCatchAny");
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

            Frames(c, "m", "()V").Should().Equal("|", "|", "java.lang.Throwable|", "|", "-");
        }

        [TestMethod]
        public void CatchAnyHandlerThatRethrowsKeepsFaultBlockException()
        {
            var c = new TestClassBuilder("FrameFault");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Astore0();
                code.InvokeStatic(c.F);
                code.Aload0();
                code.Athrow();
                handlers.Add((start, end, handler, default(ClassConstantHandle)));
            });

            Frames(c, "m", "()V").Should().Equal(
                "|top", "|top", "fault@2|top", "|fault@2", "|fault@2", "fault@2|fault@2", "-");
        }

        [TestMethod]
        public void HandlerLocalsAreMergedFromTheTryBlock()
        {
            var c = new TestClassBuilder("FrameHandlerLocals");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst1();
                code.Istore0();
                code.InvokeStatic(c.F);
                code.Ldc(c.Constants.GetOrAddString("s"));
                code.Astore0();
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Return();
                var handler = code.Offset;
                code.Pop();
                code.Return();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var frames = Frames(c, "m", "()V");
            frames[7].Should().Be("java.lang.Exception|top");
        }

        #endregion

        #region Verification errors

        [TestMethod]
        public void IntArithmeticOnFloatIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrIadd");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.Iconst1().Fconst1().Iadd().Pop().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void PopOfLongIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrPopLong");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.Lconst0().Pop().Pop().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void DupOfLongIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrDupLong");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Lconst0().Dup().Pop2().Pop2().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void LoadOfLocalWithWrongTypeIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrAloadInt");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) => code.Aload0().Pop().Return());
            ShouldFailVerification(c, "m", "(I)V");

            var d = new TestClassBuilder("FrameErrLloadInt");
            d.AddMethod("m", "(II)V", 2, 2, (code, handlers) => code.Lload0().Pop2().Return());
            ShouldFailVerification(d, "m", "(II)V");
        }

        [TestMethod]
        public void StoreBeyondMaxLocalsIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMaxLocals");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) => code.Iconst0().Istore1().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void LongStoreIntoLastLocalIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrLongLast");
            c.AddMethod("m", "()V", 2, 1, (code, handlers) => code.Lconst0().Lstore0().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void ReturnOfWrongKindIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrReturn");
            c.AddMethod("m", "()I", 1, 0, (code, handlers) => code.Fconst0().Ireturn());
            ShouldFailVerification(c, "m", "()I");

            var d = new TestClassBuilder("FrameErrReturnVoid");
            d.AddMethod("m", "()I", 0, 0, (code, handlers) => code.Return());
            ShouldFailVerification(d, "m", "()I");
        }

        [TestMethod]
        public void ReturnOfIncompatibleReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrReturnRef");
            c.AddMethod("m", "()Ljava/lang/String;", 1, 0, (code, handlers) => code.AconstNull().Checkcast(c.Class("java/lang/Integer")).Areturn());
            ShouldFailVerification(c, "m", "()Ljava/lang/String;");
        }

        [TestMethod]
        public void UseOfUninitializedObjectIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrUseNew");
            c.AddMethod("m", "()Ljava/lang/String;", 2, 0, (code, handlers) => code
                .New(c.Class("java/lang/Object"))
                .InvokeVirtual(c.Constants.GetOrAddMethodref("java/lang/Object", "toString", "()Ljava/lang/String;"))
                .Areturn());
            ShouldFailVerification(c, "m", "()Ljava/lang/String;");
        }

        [TestMethod]
        public void ReturningUninitializedObjectIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrReturnNew");
            c.AddMethod("m", "()Ljava/lang/Object;", 1, 0, (code, handlers) => code.New(c.Class("java/lang/Object")).Areturn());
            ShouldFailVerification(c, "m", "()Ljava/lang/Object;");
        }

        [TestMethod]
        public void MergeOfDifferentUninitializedObjectsIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMergeNew");
            c.AddMethod("m", "(I)V", 2, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.New(c.Class("java/lang/Object"));
                code.Goto(end);
                code.MarkLabel(other);
                code.New(c.Class("java/lang/Object"));
                code.MarkLabel(end);
                code.Pop();
                code.Return();
            });
            ShouldFailVerification(c, "m", "(I)V");
        }

        [TestMethod]
        public void MergeOfIntAndFloatOnStackIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMergePrim");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Iconst0();
                code.Goto(end);
                code.MarkLabel(other);
                code.Fconst0();
                code.MarkLabel(end);
                code.Pop();
                code.Return();
            });
            ShouldFailVerification(c, "m", "(I)V");
        }

        [TestMethod]
        public void MergeOfIntAndReferenceOnStackIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMergeIntRef");
            c.AddMethod("m", "(I)V", 1, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Iconst0();
                code.Goto(end);
                code.MarkLabel(other);
                code.AconstNull();
                code.MarkLabel(end);
                code.Pop();
                code.Return();
            });
            ShouldFailVerification(c, "m", "(I)V");
        }

        [TestMethod]
        public void ConstructorReturningBeforeSuperIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrCtorReturn");
            c.AddMethod(AccessFlag.Public, "<init>", "()V", 0, 1, (code, handlers) => code.Return());
            ShouldFailVerification(c, "<init>", "()V");
        }

        [TestMethod]
        public void ConstructorUsingThisBeforeSuperIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrCtorUse");
            c.AddMethod(AccessFlag.Public, "<init>", "()V", 1, 1, (code, handlers) => code
                .Aload0()
                .InvokeVirtual(c.Constants.GetOrAddMethodref("java/lang/Object", "hashCode", "()I"))
                .Pop()
                .Aload0()
                .InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"))
                .Return());
            ShouldFailVerification(c, "<init>", "()V");
        }

        [TestMethod]
        public void ConstructorCallingSuperOnlyOnOnePathIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrCtorPath");
            c.AddMethod(AccessFlag.Public, "<init>", "(I)V", 1, 2, (code, handlers) =>
            {
                var skip = code.DefineLabel();
                code.Iload1();
                code.Ifeq(skip);
                code.Aload0();
                code.InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"));
                code.MarkLabel(skip);
                code.Return();
            });
            ShouldFailVerification(c, "<init>", "(I)V");
        }

        [TestMethod]
        public void ArrayLoadFromNonArrayIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrArray");
            c.AddMethod("m", "(Ljava/lang/String;)V", 2, 1, (code, handlers) => code.Aload0().Iconst0().Aaload().Pop().Return());
            ShouldFailVerification(c, "m", "(Ljava/lang/String;)V");
        }

        [TestMethod]
        public void WrongPrimitiveArrayLoadIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrIaloadLong");
            c.AddMethod("m", "([J)V", 2, 1, (code, handlers) => code.Aload0().Iconst0().Iaload().Pop().Return());
            ShouldFailVerification(c, "m", "([J)V");
        }

        [TestMethod]
        public void PassingWrongArgumentTypeIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrArg");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) => code.Fconst0().InvokeStatic(c.H).Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void StoringExtendedFloatAsReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrAstoreExtFloat");
            c.AddMethod("m", "()V", 2, 1, (code, handlers) => code.Fconst1().Fconst1().Fadd().Astore0().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void StoringExtendedFloatFromCallAsReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrAstoreCallFloat");
            c.AddMethod("fl", "()F", 1, 0, (code, handlers) => code.Fconst0().Freturn());
            c.AddMethod("m", "()V", 1, 1, (code, handlers) => code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "fl", "()F")).Astore0().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void ReturningExtendedFloatAsReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrAreturnExtFloat");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 0, (code, handlers) => code.Fconst1().Fconst1().Fmul().Areturn());
            ShouldFailVerification(c, "m", "()Ljava/lang/Object;");
        }

        [TestMethod]
        public void PassingExtendedFloatAsReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrArgExtFloat");
            c.AddMethod("m", "()Ljava/lang/String;", 2, 0, (code, handlers) => code
                .Fconst1().Fconst1().Fdiv()
                .InvokeStatic(c.Constants.GetOrAddMethodref("java/lang/String", "valueOf", "(Ljava/lang/Object;)Ljava/lang/String;"))
                .Areturn());
            ShouldFailVerification(c, "m", "()Ljava/lang/String;");
        }

        [TestMethod]
        public void MergingExtendedFloatWithReferenceIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMergeExtFloat");
            c.AddMethod("m", "(I)V", 2, 1, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var end = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Fconst1().Fconst1().Fadd();
                code.Goto(end);
                code.MarkLabel(other);
                code.AconstNull();
                code.MarkLabel(end);
                code.Pop();
                code.Return();
            });
            ShouldFailVerification(c, "m", "(I)V");
        }

        [TestMethod]
        public void MonitorOnExtendedFloatIsVerifyError()
        {
            var c = new TestClassBuilder("FrameErrMonitorExtFloat");
            c.AddMethod("m", "()V", 2, 0, (code, handlers) => code.Fconst1().Fconst1().Fsub().MonitorEnter().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        [TestMethod]
        public void ExtendedDoubleIsStillACategory2Value()
        {
            var c = new TestClassBuilder("FrameErrPopExtDouble");
            c.AddMethod("m", "()V", 4, 0, (code, handlers) => code.Dconst1().Dconst1().Dadd().Pop().Return());
            ShouldFailVerification(c, "m", "()V");
        }

        #endregion

    }

}
