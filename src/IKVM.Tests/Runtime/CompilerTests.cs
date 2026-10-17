using System;
using System.Linq;
using System.Reflection;
using System.Text;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.Java.Tests.Util;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests for the bytecode to IL compiler. The snapshot tests compare the IL the runtime emits for a method against a
    /// recorded listing, so that changes to how the compiler works can be checked to not change what it produces.
    /// </summary>
    [TestClass]
    public class CompilerTests
    {

        const BindingFlags AllDeclared = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        const string SnapshotClassName = "CompilerSnapshotTest";
        const string FinalizerClassName = "CompilerSnapshotFinalizer";

        const string SnapshotSource = """
            public class CompilerSnapshotTest {

                static int counter;

                int field;

                long longField;

                public CompilerSnapshotTest() { }

                static void f() { }

                static int g(int x) { return x; }

                static void h(Throwable t) { }

                static int arithmetic(int a, int b) { return ((a + b) * (a - b) / b % 7) << 2 >> 1 >>> 1 ^ a | b & ~a; }

                static long longs(long a, long b, int s) { return (a * b + a / b - a % b) << s >> s >>> s ^ (a | b & ~a); }

                static double doubles(double a, double b) { return a * b / 3.0 + a % b - -a; }

                static float floats(float a, float b) { return a * b / 3.0f + a % b - -a; }

                static int conversions(double d, long l, float f, int i) { return (int) d + (int) l + (int) f + (byte) i + (char) i + (short) i + (int) (long) d + (int) (float) l + (int) (double) f; }

                static int compares(long a, long b, double c, double d, float e, float f) { int r = 0; if (a < b) r++; if (c > d) r += 2; if (c <= d) r += 4; if (e < f) r += 8; if (e >= f) r += 16; return r; }

                static int loop(int n) { int s = 0; for (int i = 0; i < n; i++) s += i; return s; }

                static int whileTrueBreak(int n) { int i = 0; while (true) { if (i > n) break; i += 2; } return i; }

                static int tableSwitch(int x) { switch (x) { case 0: return 10; case 1: return 11; case 2: return 12; default: return -1; } }

                static int lookupSwitch(int x) { switch (x) { case 1: return 1; case 1000: return 2; case -5: return 3; default: return 0; } }

                static int stringSwitch(String s) { switch (s) { case "a": return 1; case "b": return 2; default: return 0; } }

                static int tryCatch(int x) { try { f(); return x; } catch (RuntimeException e) { h(e); return -1; } }

                static int tryCatchUnused(int x) { try { f(); return x; } catch (RuntimeException e) { return -1; } }

                static int tryCatchThrowable(int x) { try { f(); return x; } catch (Throwable t) { return -1; } }

                static int tryFinally(int x) { try { f(); return x; } finally { counter++; } }

                static int tryCatchFinally(int x) { try { f(); } catch (IllegalStateException e) { return 1; } catch (RuntimeException e) { return 2; } finally { counter++; } return x; }

                static int nestedTry(int x) { try { try { f(); } finally { counter++; } } catch (Exception e) { return -1; } finally { counter--; } return x; }

                static int breakContinueInTry(int n) { int i = 0; while (true) { try { if (i++ > n) break; if (i == 2) continue; f(); } catch (RuntimeException e) { continue; } finally { counter++; } } return i; }

                @SuppressWarnings("finally")
                static int returnInFinally(int x) { try { f(); return x; } finally { if (x > 0) return -x; } }

                static void sync(Object o) { synchronized (o) { f(); } }

                static int syncReturn(Object o, int x) { synchronized (o) { if (x > 0) return x; f(); } return 0; }

                static synchronized int staticSync(int x) { f(); return x; }

                synchronized int instanceSync() { return field++; }

                static Object newWithBranch(boolean b) { return new StringBuilder(b ? "a" : "b"); }

                static Object arrays(int n) { int[] a = new int[n]; long[] l = new long[n]; Object[] o = new String[n]; int[][] m = new int[n][n]; a[0] = m.length; l[0] = a[0]; o[0] = "x"; m[0][0] = a.length; return o; }

                static int arrayOps(byte[] b, char[] c, short[] s, boolean[] z, float[] f, double[] d) { b[0] = 1; c[0] = 'c'; s[0] = 2; z[0] = true; f[0] = 1.5f; d[0] = 2.5; return b[0] + c[0] + s[0] + (z[0] ? 1 : 0) + (int) f[0] + (int) d[0]; }

                static boolean instanceOf(Object o) { return o instanceof String && ((String) o).length() > 0; }

                static void throwNew() { throw new IllegalStateException("x"); }

                static void rethrow(RuntimeException e) { throw e; }

                static String concat(String a, int b, long c, Object d) { return a + b + c + d; }

                int instanceField(CompilerSnapshotTest other) { this.field = other.field + 1; return field; }

                static int staticField() { counter += 2; return counter; }

                static int ternaryInCall(boolean b, int x) { return g(b ? x : -x) + g(x); }

                static int dupArray(int[] a, int i) { return a[i]++ + a[i]--; }

                static long dupLongArray(long[] a, int i) { return a[i]++ + --a[i]; }

                static int dupStaticField() { return counter++; }

                long dupLongField() { return longField++; }

                static void interfaceCall(java.util.List<String> l) { l.add("x"); l.size(); }

                static int virtualCall(Object o) { return o.hashCode() + o.toString().length(); }

                static int returnFromLoopWithFinally(int n) { for (int i = 0; i < n; i++) { try { if (i == 3) return i; } finally { counter++; } } return -1; }

                static int manyArgsWithTry(int a, int b, int c, int d, int e) { try { e++; d = g(e); } catch (RuntimeException ex) { a = -1; } return a + b + c + d + e; }

                static long wideLocals(long a, double b) { long c = a; double d = b; c += 1; d *= 2; return c + (long) d; }

            }

            class CompilerSnapshotFinalizer {

                CompilerSnapshotFinalizer(int x) { if (x > 0) throw new IllegalArgumentException(); }

                protected void finalize() { }

            }
            """;

        static LoadedClass snapshotClass;
        static Type snapshotType;
        static LoadedClass finalizerClass;
        static Type finalizerType;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var compiler = new InMemoryCompiler([new InMemoryCodeUnit(SnapshotClassName, SnapshotSource)]);
            compiler.Compile();

            snapshotClass = LoadedClass.Load(SnapshotClassName, compiler.GetClassBytes(SnapshotClassName));
            snapshotType = snapshotClass.GetRuntimeType();
            finalizerClass = LoadedClass.Load(FinalizerClassName, compiler.GetClassBytes(FinalizerClassName));
            finalizerType = finalizerClass.GetRuntimeType();
        }

        static void SkipIfEmittingSymbols()
        {
            // emitting symbols changes how locals are allocated, which changes the IL
            if (JVM.EmitSymbols)
                Assert.Inconclusive("Compiler snapshots are recorded without symbols.");
        }

        /// <summary>
        /// Formats every compiled method with the given name.
        /// </summary>
        static string FormatMethods(LoadedClass clazz, Type type, string name)
        {
            var methods = type.GetMethods(AllDeclared).Cast<MethodBase>().Concat(type.GetConstructors(AllDeclared))
                .Where(i => i.Name == name)
                .Select(i => ILFormatter.Format(i, type, offset => clazz.Type.GetSourceLineNumber(i, offset)))
                .OrderBy(i => i, StringComparer.Ordinal)
                .ToArray();

            methods.Should().NotBeEmpty("a method named {0} should have been compiled", name);
            return string.Join("\n", methods);
        }

        [DataTestMethod]
        [DataRow("arithmetic")]
        [DataRow("longs")]
        [DataRow("doubles")]
        [DataRow("floats")]
        [DataRow("conversions")]
        [DataRow("compares")]
        [DataRow("loop")]
        [DataRow("whileTrueBreak")]
        [DataRow("tableSwitch")]
        [DataRow("lookupSwitch")]
        [DataRow("stringSwitch")]
        [DataRow("tryCatch")]
        [DataRow("tryCatchUnused")]
        [DataRow("tryCatchThrowable")]
        [DataRow("tryFinally")]
        [DataRow("tryCatchFinally")]
        [DataRow("nestedTry")]
        [DataRow("breakContinueInTry")]
        [DataRow("returnInFinally")]
        [DataRow("sync")]
        [DataRow("syncReturn")]
        [DataRow("staticSync")]
        [DataRow("instanceSync")]
        [DataRow("newWithBranch")]
        [DataRow("arrays")]
        [DataRow("arrayOps")]
        [DataRow("instanceOf")]
        [DataRow("throwNew")]
        [DataRow("rethrow")]
        [DataRow("concat")]
        [DataRow("instanceField")]
        [DataRow("staticField")]
        [DataRow("ternaryInCall")]
        [DataRow("dupArray")]
        [DataRow("dupLongArray")]
        [DataRow("dupStaticField")]
        [DataRow("dupLongField")]
        [DataRow("interfaceCall")]
        [DataRow("virtualCall")]
        [DataRow("returnFromLoopWithFinally")]
        [DataRow("manyArgsWithTry")]
        [DataRow("wideLocals")]
        public void JavacMethodSnapshot(string name)
        {
            SkipIfEmittingSymbols();
            SnapshotAssert.Match($"{SnapshotClassName}.{name}", FormatMethods(snapshotClass, snapshotType, name));
        }

        [TestMethod]
        public void ConstructorOfClassWithFinalizerSnapshot()
        {
            SkipIfEmittingSymbols();
            SnapshotAssert.Match($"{FinalizerClassName}..ctor", FormatMethods(finalizerClass, finalizerType, ".ctor"));
        }

        static object Invoke(Type type, string name, params object[] args)
        {
            try
            {
                return type.GetMethod(name, AllDeclared).Invoke(null, args);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        [TestMethod]
        public void BackwardBranchWithValueOnStack()
        {
            // the loop body is only reachable by a backward branch, with the running total on the stack, which the
            // CLR does not allow, so the compiler must move the stack into locals
            var c = new TestClassBuilder("CompilerBackwardStack");
            c.AddMethod("m", "(I)I", 2, 1, (code, handlers) =>
            {
                var body = code.DefineLabel();
                var test = code.DefineLabel();
                code.Iconst0();
                code.Goto(test);
                code.MarkLabel(body);
                code.Iload0();
                code.Iadd();
                code.Iinc(0, -1);
                code.MarkLabel(test);
                code.Iload0();
                code.Ifne(body);
                code.Ireturn();
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m", 0).Should().Be(0);
            Invoke(type, "m", 4).Should().Be(10);

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerBackwardStack.m", FormatMethods(clazz, type, "m"));
        }

        [TestMethod]
        public void UninitializedObjectStoredInLocal()
        {
            var c = new TestClassBuilder("CompilerUninitializedLocal");
            c.AddMethod("m", "()Ljava/lang/Object;", 2, 1, (code, handlers) =>
            {
                code.New(c.Class("java/lang/Object"));
                code.Dup();
                code.Astore0();
                code.InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/Object", "<init>", "()V"));
                code.Aload0();
                code.Areturn();
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m").Should().NotBeNull();

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerUninitializedLocal.m", FormatMethods(clazz, type, "m"));
        }

        [TestMethod]
        public void BranchIntoMiddleOfTryBlock()
        {
            var c = new TestClassBuilder("CompilerBranchIntoTry");
            c.AddMethod("thrower", "(I)V", 2, 1, (code, handlers) =>
            {
                var ok = code.DefineLabel();
                code.Iload0();
                code.Ifeq(ok);
                code.New(c.Class("java/lang/IllegalStateException"));
                code.Dup();
                code.InvokeSpecial(c.Constants.GetOrAddMethodref("java/lang/IllegalStateException", "<init>", "()V"));
                code.Athrow();
                code.MarkLabel(ok);
                code.Return();
            });
            c.AddMethod("m", "(II)I", 1, 2, (code, handlers) =>
            {
                var target = code.DefineLabel();
                code.Iload0();
                code.Ifeq(target);
                var start = code.Offset;
                code.Iconst0();
                code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "thrower", "(I)V"));
                code.MarkLabel(target);
                code.Iload1();
                code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "thrower", "(I)V"));
                var end = code.Offset;
                code.Iconst1();
                code.Ireturn();
                var handler = code.Offset;
                code.Pop();
                code.IconstM1();
                code.Ireturn();
                handlers.Add((start, end, handler, c.Class("java/lang/IllegalStateException")));
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m", 0, 0).Should().Be(1);
            Invoke(type, "m", 1, 0).Should().Be(1);
            Invoke(type, "m", 0, 1).Should().Be(-1);
            Invoke(type, "m", 1, 1).Should().Be(-1);

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerBranchIntoTry.m", FormatMethods(clazz, type, "m"));
        }

        [TestMethod]
        public void ReturnWithExtraValuesOnStack()
        {
            var c = new TestClassBuilder("CompilerReturnJunk");
            c.AddMethod("m", "()I", 3, 0, (code, handlers) => code.Iconst5().Iconst4().Iconst1().Ireturn());
            c.AddMethod("v", "()V", 2, 0, (code, handlers) => code.Iconst5().Iconst4().Return());
            c.AddMethod("t", "()I", 3, 0, (code, handlers) =>
            {
                var start = code.Offset;
                code.Iconst5();
                code.InvokeStatic(c.F);
                code.Iconst1();
                code.Ireturn();
                var end = code.Offset;
                code.Pop();
                code.IconstM1();
                code.Ireturn();
                handlers.Add((start, end, end, c.Class("java/lang/Exception")));
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m").Should().Be(1);
            Invoke(type, "v").Should().BeNull();
            Invoke(type, "t").Should().Be(1);

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerReturnJunk.m", FormatMethods(clazz, type, "m"));
            SnapshotAssert.Match("CompilerReturnJunk.v", FormatMethods(clazz, type, "v"));
            SnapshotAssert.Match("CompilerReturnJunk.t", FormatMethods(clazz, type, "t"));
        }

        [TestMethod]
        public void ValueOnStackWhenEnteringTryBlock()
        {
            // javac never leaves values on the stack when entering a try block, but the JVM allows it
            var c = new TestClassBuilder("CompilerStackIntoTry");
            c.AddMethod("m", "(I)I", 2, 1, (code, handlers) =>
            {
                code.Iconst5();
                var start = code.Offset;
                code.InvokeStatic(c.F);
                code.Iload0();
                code.Iadd();
                var end = code.Offset;
                code.Ireturn();
                var handler = code.Offset;
                code.Pop();
                code.IconstM1();
                code.Ireturn();
                handlers.Add((start, end, handler, c.Class("java/lang/Exception")));
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m", 3).Should().Be(8);

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerStackIntoTry.m", FormatMethods(clazz, type, "m"));
        }

        [TestMethod]
        public void UnreachableCode()
        {
            var c = new TestClassBuilder("CompilerUnreachable");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var target = code.DefineLabel();
                code.Goto(target);
                code.Iconst5();
                code.Ireturn();
                code.MarkLabel(target);
                code.Iload0();
                code.Ireturn();
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m", 7).Should().Be(7);

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerUnreachable.m", FormatMethods(clazz, type, "m"));
        }

        [TestMethod]
        public void VerifyErrorIsThrownWhenMethodIsCalled()
        {
            var c = new TestClassBuilder("CompilerVerifyError");
            c.AddMethod("m", "()V", 1, 0, (code, handlers) => code.Pop().Return());

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            FluentActions.Invoking(() => Invoke(type, "m")).Should().Throw<java.lang.VerifyError>();
        }

        [TestMethod]
        public void MissingMethodIsThrownWhenCalled()
        {
            var c = new TestClassBuilder("CompilerMissingMethod");
            c.AddMethod("m", "(I)I", 1, 1, (code, handlers) =>
            {
                var call = code.DefineLabel();
                code.Iload0();
                code.Ifne(call);
                code.Iconst1();
                code.Ireturn();
                code.MarkLabel(call);
                code.InvokeStatic(c.Constants.GetOrAddMethodref(c.Name, "missing", "()V"));
                code.Iconst2();
                code.Ireturn();
            });

            var clazz = c.Load();
            var type = clazz.GetRuntimeType();
            Invoke(type, "m", 0).Should().Be(1);
            FluentActions.Invoking(() => Invoke(type, "m", 1)).Should().Throw<java.lang.NoSuchMethodError>();

            SkipIfEmittingSymbols();
            SnapshotAssert.Match("CompilerMissingMethod.m", FormatMethods(clazz, type, "m"));
        }

    }

}
