using System.Linq;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests for <see cref="LocalVarInfo"/>, which maps each load and store of a Java local to the .NET local the
    /// compiler declares for it.
    /// </summary>
    [TestClass]
    public class LocalVarInfoTests
    {

        static (AnalyzedMethod Method, LocalVarInfo Locals) Analyze(TestClassBuilder builder, string name, string signature)
        {
            var m = builder.Load().GetMethod(name, signature).Analyze();
            return (m, m.AnalyzeLocals());
        }

        [TestMethod]
        public void StoreAndLoadShareLocal()
        {
            var c = new TestClassBuilder("LocalSimple");
            c.AddMethod("m", "()I", 1, 1, (code, handlers) => code.Iconst1().Istore0().Iload0().Ireturn());

            var (m, locals) = Analyze(c, "m", "()I");
            var v = locals.GetLocalVar(1);
            v.Should().NotBeNull();
            v.local.Should().Be(0);
            v.isArg.Should().BeFalse();
            v.type.Should().BeSameAs(JVM.Context.PrimitiveJavaTypeFactory.INT);
            locals.GetLocalVar(2).Should().BeSameAs(v);
            locals.GetAllLocalVars().Should().Equal(v);
        }

        [TestMethod]
        public void LoadOfArgumentIsArgument()
        {
            var c = new TestClassBuilder("LocalArg");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) => code.Iload0().Ireturn());

            var (m, locals) = Analyze(c, "m", "(I)I");
            var v = locals.GetLocalVar(0);
            v.local.Should().Be(0);
            v.isArg.Should().BeTrue();
            v.type.Should().BeSameAs(JVM.Context.PrimitiveJavaTypeFactory.INT);
        }

        [TestMethod]
        public void StoreToArgumentThatIsLoadedLaterIsArgument()
        {
            var c = new TestClassBuilder("LocalArgStore");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var join = code.DefineLabel();
                code.Iload0();
                code.Ifne(join);
                code.Iconst5();
                code.Istore0();
                code.MarkLabel(join);
                code.Iload0();
                code.Ireturn();
            });

            // the load at 4 sees both the incoming argument and the store at 3, so they must be the same local
            var (m, locals) = Analyze(c, "m", "(I)I");
            var v = locals.GetLocalVar(4);
            v.isArg.Should().BeTrue();
            locals.GetLocalVar(3).Should().BeSameAs(v);
        }

        [TestMethod]
        public void SlotReusedWithDifferentTypesGetsSeparateLocals()
        {
            var c = new TestClassBuilder("LocalReuse");
            c.AddMethod("m", "()Ljava/lang/Object;", 1, 1, (code, handlers) =>
            {
                code.Iconst1();
                code.Istore0();
                code.Iload0();
                code.Pop();
                code.Ldc(c.Constants.GetOrAddString("s"));
                code.Astore0();
                code.Aload0();
                code.Areturn();
            });

            var (m, locals) = Analyze(c, "m", "()Ljava/lang/Object;");
            var i = locals.GetLocalVar(1);
            var s = locals.GetLocalVar(5);
            i.Should().NotBeSameAs(s);
            i.type.Should().BeSameAs(JVM.Context.PrimitiveJavaTypeFactory.INT);
            s.type.Name.Should().Be("java.lang.String");
            locals.GetLocalVar(2).Should().BeSameAs(i);
            locals.GetLocalVar(6).Should().BeSameAs(s);
            locals.GetAllLocalVars().Should().HaveCount(2);
        }

        [TestMethod]
        public void SlotReusedWithSameTypeIsMergedUnlessEmittingSymbols()
        {
            var c = new TestClassBuilder("LocalReuseSame");
            c.AddMethod("m", "()I", 1, 1, (code, handlers) =>
            {
                code.Iconst1();
                code.Istore0();
                code.Iload0();
                code.Pop();
                code.Iconst2();
                code.Istore0();
                code.Iload0();
                code.Ireturn();
            });

            var (m, locals) = Analyze(c, "m", "()I");
            if (m.Class.Type.ClassLoader.EmitSymbols)
            {
                locals.GetLocalVar(1).Should().NotBeSameAs(locals.GetLocalVar(5));
                locals.GetAllLocalVars().Should().HaveCount(2);
            }
            else
            {
                locals.GetLocalVar(1).Should().BeSameAs(locals.GetLocalVar(5));
                locals.GetAllLocalVars().Should().HaveCount(1);
            }
        }

        [TestMethod]
        public void StoresOnBothBranchesShareLocalWithLoadAtJoin()
        {
            var c = new TestClassBuilder("LocalJoin");
            c.AddMethod("m", "(I)I", 1, 2, (code, handlers) =>
            {
                var other = code.DefineLabel();
                var join = code.DefineLabel();
                code.Iload0();
                code.Ifeq(other);
                code.Iconst1();
                code.Istore1();
                code.Goto(join);
                code.MarkLabel(other);
                code.Iconst2();
                code.Istore1();
                code.MarkLabel(join);
                code.Iload1();
                code.Ireturn();
            });

            var (m, locals) = Analyze(c, "m", "(I)I");
            var v = locals.GetLocalVar(7);
            v.local.Should().Be(1);
            locals.GetLocalVar(3).Should().BeSameAs(v);
            locals.GetLocalVar(6).Should().BeSameAs(v);
        }

        [TestMethod]
        public void StoreThatIsNeverLoadedIsDeadUnlessEmittingSymbols()
        {
            var c = new TestClassBuilder("LocalDead");
            c.AddMethod("m", "()V", 1, 1, (code, handlers) => code.Iconst1().Istore0().Return());

            var (m, locals) = Analyze(c, "m", "()V");
            if (m.Class.Type.ClassLoader.EmitSymbols)
                locals.GetLocalVar(1).Should().NotBeNull();
            else
                locals.GetLocalVar(1).Should().BeNull();
        }

        [TestMethod]
        public void StoresInsideTryBlockAreVisibleInHandler()
        {
            var c = new TestClassBuilder("LocalHandler");
            c.AddMethod("m", "()I", 1, 1, (code, handlers) =>
            {
                code.Iconst0();
                code.Istore0();
                var start = code.Offset;
                code.Iconst1();
                code.Istore0();
                code.InvokeStatic(c.F);
                var end = code.Offset;
                code.Iload0();
                code.Ireturn();
                var handler = code.Offset;
                code.Pop();
                code.Iload0();
                code.Ireturn();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var (m, locals) = Analyze(c, "m", "()I");
            var v = locals.GetLocalVar(8);
            locals.GetLocalVar(1).Should().BeSameAs(v);
            locals.GetLocalVar(3).Should().BeSameAs(v);
            locals.GetLocalVar(5).Should().BeSameAs(v);
        }

        [TestMethod]
        public void StoreAtEndOfTryBlockIsVisibleInHandler()
        {
            var c = new TestClassBuilder("LocalTryEnd");
            c.AddMethod("m", "()I", 1, 1, (code, handlers) =>
            {
                code.Iconst0();
                code.Istore0();
                var start = code.Offset;
                code.InvokeStatic(c.F);
                code.Iconst1();
                code.Istore0();
                var end = code.Offset;
                code.Iload0();
                code.Ireturn();
                var handler = code.Offset;
                code.Pop();
                code.Iload0();
                code.Ireturn();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var (m, locals) = Analyze(c, "m", "()I");
            var v = locals.GetLocalVar(8);
            locals.GetLocalVar(1).Should().BeSameAs(v);
            locals.GetLocalVar(4).Should().BeSameAs(v);
        }

        [TestMethod]
        public void LocalsHoldingUninitializedObjectAreReportedForInvokeSpecial()
        {
            var c = new TestClassBuilder("LocalNew");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 1, (code, handlers) =>
            {
                code.New(c.Class("java/lang/Object"));
                code.Dup();
                code.Astore0();
                code.InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"));
                code.Aload0();
                code.Areturn();
            });

            var (m, locals) = Analyze(c, "m", "()Ljava/lang/Object;");
            var initialized = locals.GetLocalVarsForInvokeSpecial(3);
            initialized.Should().HaveCount(1);
            initialized[0].Should().NotBeNull();
            initialized[0].Should().BeSameAs(locals.GetLocalVar(4));
            initialized[0].type.Should().BeSameAs(JVM.Context.JavaBase.TypeOfJavaLangObject);
        }

        [TestMethod]
        public void InvokeSpecialWithoutLocalsReportsNone()
        {
            var c = new TestClassBuilder("LocalNoNew");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 0, (code, handlers) =>
            {
                code.New(c.Class("java/lang/Object"));
                code.Dup();
                code.InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"));
                code.Areturn();
            });

            var (m, locals) = Analyze(c, "m", "()Ljava/lang/Object;");
            locals.GetLocalVarsForInvokeSpecial(2).Should().BeEmpty();
            locals.GetAllLocalVars().Should().BeEmpty();
        }

        [TestMethod]
        public void ThisIsArgument()
        {
            var c = new TestClassBuilder("LocalThis");
            c.AddMethod(AccessFlag.Public, "m", "()Ljava/lang/Object;", 1, 1, (code, handlers) => code.Aload0().Areturn());

            var (m, locals) = Analyze(c, "m", "()Ljava/lang/Object;");
            var v = locals.GetLocalVar(0);
            v.isArg.Should().BeTrue();
            v.local.Should().Be(0);
            v.type.Should().BeSameAs(m.Class.Type);
        }

        [TestMethod]
        public void WideLocalsAreTracked()
        {
            var c = new TestClassBuilder("LocalWide");
            c.AddMethod("m", "(JD)D", 4, 6, (code, handlers) =>
            {
                code.Lload0();
                code.L2d();
                code.Dload2();
                code.Dadd();
                code.Dstore(4);
                code.Dload(4);
                code.Dreturn();
            });

            var (m, locals) = Analyze(c, "m", "(JD)D");
            locals.GetLocalVar(0).isArg.Should().BeTrue();
            locals.GetLocalVar(0).type.Should().BeSameAs(JVM.Context.PrimitiveJavaTypeFactory.LONG);
            locals.GetLocalVar(2).isArg.Should().BeTrue();
            locals.GetLocalVar(2).local.Should().Be(2);
            locals.GetLocalVar(2).type.Should().BeSameAs(JVM.Context.PrimitiveJavaTypeFactory.DOUBLE);
            locals.GetLocalVar(4).Should().BeSameAs(locals.GetLocalVar(5));
            locals.GetLocalVar(5).local.Should().Be(4);
            locals.GetLocalVar(5).isArg.Should().BeFalse();
        }

        [TestMethod]
        public void IincIsLoadAndStore()
        {
            var c = new TestClassBuilder("LocalIinc");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) => code.Iinc(0, 1).Iload0().Ireturn());

            var (m, locals) = Analyze(c, "m", "(I)I");
            var v = locals.GetLocalVar(0);
            v.isArg.Should().BeTrue();
            locals.GetLocalVar(1).local.Should().Be(0);

            // the load after the iinc reads the iinc's store site, which only becomes the same local through the
            // merge of same-typed locals that happens when symbols are not emitted
            if (m.Class.Type.ClassLoader.EmitSymbols == false)
                locals.GetLocalVar(1).Should().BeSameAs(v);
        }

        [TestMethod]
        public void LoopCarriedLocalIsSingleLocal()
        {
            var c = new TestClassBuilder("LocalLoop");
            c.AddMethod("m", "(I)I", 2, 2, (code, handlers) =>
            {
                var body = code.DefineLabel();
                var test = code.DefineLabel();
                code.Iconst0();
                code.Istore1();
                code.Goto(test);
                code.MarkLabel(body);
                code.Iload1();
                code.Iload0();
                code.Iadd();
                code.Istore1();
                code.Iinc(0, -1);
                code.MarkLabel(test);
                code.Iload0();
                code.Ifne(body);
                code.Iload1();
                code.Ireturn();
            });

            var (m, locals) = Analyze(c, "m", "(I)I");
            var sum = locals.GetLocalVar(1);
            new[] { 3, 6, 10 }.Select(i => locals.GetLocalVar(i)).Should().AllSatisfy(i => i.Should().BeSameAs(sum));

            var n = locals.GetLocalVar(4);
            n.isArg.Should().BeTrue();
            new[] { 7, 8 }.Select(i => locals.GetLocalVar(i)).Should().AllSatisfy(i => i.Should().BeSameAs(n));
        }

    }

}
