using System;
using System.Reflection;
using System.Threading;

using FluentAssertions;

using IKVM.Java.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Runs javac compiled code through the bytecode to IL compiler and checks the results.
    /// </summary>
    [TestClass]
    public class CompilerExecutionTests
    {

        const string ClassName = "CompilerExecutionTest";

        const string Source = """
            public class CompilerExecutionTest {

                static int counter;

                int field;

                public CompilerExecutionTest() { }

                static void f(int mode) {
                    if (mode == 1) throw new IllegalStateException("ise");
                    if (mode == 2) throw new IllegalArgumentException("iae");
                    if (mode == 3) throw new Error("error");
                }

                static int tryFinally(int mode) {
                    int r = 0;
                    try { f(mode); r = 1; } catch (RuntimeException e) { r = 2; } finally { r += 10; }
                    return r;
                }

                static String tryCatchFinally(int mode) {
                    StringBuilder sb = new StringBuilder();
                    try { f(mode); sb.append("t"); }
                    catch (IllegalStateException e) { sb.append("ise"); }
                    catch (RuntimeException e) { sb.append("rte"); }
                    finally { sb.append("f"); }
                    return sb.toString();
                }

                static String finallyRunsWhenExceptionEscapes(int mode) {
                    StringBuilder sb = new StringBuilder();
                    try {
                        try { f(mode); sb.append("t"); } finally { sb.append("f"); }
                    } catch (Throwable t) { sb.append("c"); }
                    return sb.toString();
                }

                @SuppressWarnings("finally")
                static int returnInFinally(int mode) {
                    try { f(mode); return 1; } finally { if (mode != 0) return 2; }
                }

                static int returnInTryRunsFinally(int mode) {
                    counter = 0;
                    try { if (mode == 0) return 1; f(mode); return 2; } catch (RuntimeException e) { return 3; } finally { counter++; }
                }

                static String nestedFinally(int mode) {
                    StringBuilder sb = new StringBuilder();
                    try {
                        try { f(mode); sb.append("a"); } finally { sb.append("b"); }
                        sb.append("c");
                    } catch (RuntimeException e) { sb.append("d"); } finally { sb.append("e"); }
                    return sb.toString();
                }

                static int loopWithBreakAndContinue(int n) {
                    int s = 0;
                    counter = 0;
                    for (int i = 0; i < n; i++) {
                        try {
                            if (i % 3 == 0) continue;
                            if (i > 7) break;
                            f(i == 5 ? 1 : 0);
                            s += i;
                        } catch (IllegalStateException e) {
                            s += 100;
                        } finally {
                            counter++;
                        }
                    }
                    return s * 1000 + counter;
                }

                static int returnFromNestedLoopTry(int n) {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            try { if (i * j == 6) return i * 10 + j; } finally { counter++; }
                    return -1;
                }

                static int exceptionInHandler(int mode) {
                    try {
                        try { f(mode); return 0; } catch (IllegalStateException e) { f(2); return 1; }
                    } catch (IllegalArgumentException e) { return 2; }
                }

                static synchronized int staticSync(int x) { return x + 1; }

                synchronized int instanceSync(int x) { field += x; return field; }

                static int syncBlock(Object o, int mode) {
                    synchronized (o) { try { f(mode); return 1; } catch (RuntimeException e) { return 2; } }
                }

                static int syncThrows(Object o) {
                    try { synchronized (o) { f(1); } } catch (IllegalStateException e) { return 1; }
                    return 0;
                }

                static String newWithBranch(boolean b) { return new StringBuilder(b ? "x" : "y").append(b ? 1 : 2).toString(); }

                static int switches(int x) {
                    int r;
                    switch (x) { case 0: r = 10; break; case 1: r = 11; case 2: r = 12; break; case 100: r = 100; break; default: r = -1; }
                    return r;
                }

                static int stringSwitch(String s) {
                    switch (s) { case "a": return 1; case "b": return 2; case "Aa": return 3; case "BB": return 4; default: return 0; }
                }

                static int intDivRem(int a, int b) { return a / b + a % b; }

                static long longDivRem(long a, long b) { return a / b + a % b; }

                static int divByZero(int a, long b) {
                    int r = 0;
                    try { r += 1 / a; } catch (ArithmeticException e) { r += 10; }
                    try { r += (int) (1L / b); } catch (ArithmeticException e) { r += 100; }
                    try { r += 1 % a; } catch (ArithmeticException e) { r += 1000; }
                    return r;
                }

                static int shifts(int a, int s) { return (a << s) ^ (a >> s) ^ (a >>> s); }

                static long longShifts(long a, int s) { return (a << s) ^ (a >> s) ^ (a >>> s); }

                static int d2i(double d) { return (int) d; }

                static long d2l(double d) { return (long) d; }

                static int f2i(float f) { return (int) f; }

                static long f2l(float f) { return (long) f; }

                static int compare(double a, double b) { return a < b ? -1 : a > b ? 1 : a == b ? 0 : 2; }

                static int narrowing(int i) { return (byte) i * 1000000 + (char) i + (short) i; }

                static int dupArray(int[] a, int i) { return a[i]++ + a[i]--; }

                static long dupLongArray(long[] a, int i) { return a[i]++ + ++a[i]; }

                static int dupStaticField() { counter = 5; return counter++ + ++counter; }

                static int ternaryInArgs(boolean b, int x) { return Math.max(b ? x : -x, b ? -x : x); }

                static String exceptionMessage(int mode) { try { f(mode); return "none"; } catch (Throwable t) { return t.getMessage(); } }

                static int catchThrowNew() { try { throw new IllegalStateException("abc"); } catch (IllegalStateException e) { return e.getMessage().length(); } }

                static boolean thrownExceptionHasStackTrace() {
                    try { f(1); return false; } catch (IllegalStateException e) { return e.getStackTrace().length > 0; }
                }

                static int arrays(int n) {
                    int[][] m = new int[n][n + 1];
                    m[n - 1][n] = 7;
                    Object[] o = new String[n];
                    o[0] = "s";
                    return m.length * 100 + m[0].length * 10 + m[n - 1][n] + ((String) o[0]).length();
                }

                static boolean arrayStoreChecked() {
                    Object[] o = new String[1];
                    try { o[0] = Integer.valueOf(1); return false; } catch (ArrayStoreException e) { return true; }
                }

                static boolean instanceOf(Object o) { return o instanceof CharSequence; }

                static int interfaceCall(java.util.List<Integer> l) { l.add(1); l.add(2); return l.size(); }

                int instanceMethod(int x) { field += x; return field; }

                static int manyArgsWithTry(int a, int b, int c, int d, int e) {
                    try { e++; d = e * 2; f(a); } catch (RuntimeException ex) { a = -1; }
                    return a + b + c + d + e;
                }

                static int nullCheck(String s) { try { return s.length(); } catch (NullPointerException e) { return -1; } }

            }
            """;

        static Type type;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var compiler = new InMemoryCompiler([new InMemoryCodeUnit(ClassName, Source)]);
            compiler.Compile();
            type = LoadedClass.Load(ClassName, compiler.GetClassBytes(ClassName)).GetRuntimeType();
        }

        static object Invoke(string name, params object[] args)
        {
            try
            {
                return type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, args);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        static object InvokeInstance(object instance, string name, params object[] args)
        {
            return type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, args);
        }

        [DataTestMethod]
        [DataRow(0, 11)]
        [DataRow(1, 12)]
        [DataRow(2, 12)]
        public void TryFinally(int mode, int expected)
        {
            Invoke("tryFinally", mode).Should().Be(expected);
        }

        [TestMethod]
        public void TryFinallyLetsErrorEscape()
        {
            FluentActions.Invoking(() => Invoke("tryFinally", 3)).Should().Throw<java.lang.Error>().WithMessage("error");
        }

        [DataTestMethod]
        [DataRow(0, "tf")]
        [DataRow(1, "isef")]
        [DataRow(2, "rtef")]
        public void TryCatchFinally(int mode, string expected)
        {
            Invoke("tryCatchFinally", mode).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(0, "tf")]
        [DataRow(1, "fc")]
        [DataRow(3, "fc")]
        public void FinallyRunsWhenExceptionEscapes(int mode, string expected)
        {
            Invoke("finallyRunsWhenExceptionEscapes", mode).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 2)]
        [DataRow(3, 2)]
        public void ReturnInFinally(int mode, int expected)
        {
            Invoke("returnInFinally", mode).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(4, 2)]
        [DataRow(1, 3)]
        public void ReturnInTryRunsFinally(int mode, int expected)
        {
            Invoke("returnInTryRunsFinally", mode).Should().Be(expected);
            type.GetField("counter", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).GetValue(null).Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(0, "abce")]
        [DataRow(1, "bde")]
        public void NestedFinally(int mode, string expected)
        {
            Invoke("nestedFinally", mode).Should().Be(expected);
        }

        [TestMethod]
        public void NestedFinallyLetsErrorEscape()
        {
            FluentActions.Invoking(() => Invoke("nestedFinally", 3)).Should().Throw<java.lang.Error>();
        }

        [TestMethod]
        public void LoopWithBreakAndContinueInTry()
        {
            // i = 1, 2, 4, 7 are added, i = 5 throws (+100), i = 0, 3, 6 continue and i = 8 breaks; finally runs for i = 0..8
            Invoke("loopWithBreakAndContinue", 20).Should().Be(114 * 1000 + 9);
        }

        [TestMethod]
        public void ReturnFromNestedLoopTry()
        {
            Invoke("returnFromNestedLoopTry", 5).Should().Be(23);
            Invoke("returnFromNestedLoopTry", 2).Should().Be(-1);
        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        public void ExceptionInHandler(int mode, int expected)
        {
            Invoke("exceptionInHandler", mode).Should().Be(expected);
        }

        [TestMethod]
        public void StaticSynchronizedMethod()
        {
            Invoke("staticSync", 1).Should().Be(2);
        }

        [TestMethod]
        public void InstanceSynchronizedMethod()
        {
            var o = Activator.CreateInstance(type);
            InvokeInstance(o, "instanceSync", 3).Should().Be(3);
            InvokeInstance(o, "instanceSync", 4).Should().Be(7);
            Monitor.IsEntered(o).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 2)]
        public void SynchronizedBlockReleasesMonitor(int mode, int expected)
        {
            var o = new object();
            Invoke("syncBlock", o, mode).Should().Be(expected);
            Monitor.IsEntered(o).Should().BeFalse();
        }

        [TestMethod]
        public void SynchronizedBlockReleasesMonitorWhenExceptionEscapes()
        {
            var o = new object();
            Invoke("syncThrows", o).Should().Be(1);
            Monitor.IsEntered(o).Should().BeFalse();
        }

        [TestMethod]
        public void NewWithBranchInConstructorArguments()
        {
            Invoke("newWithBranch", true).Should().Be("x1");
            Invoke("newWithBranch", false).Should().Be("y2");
        }

        [DataTestMethod]
        [DataRow(0, 10)]
        [DataRow(1, 12)]
        [DataRow(2, 12)]
        [DataRow(100, 100)]
        [DataRow(5, -1)]
        [DataRow(-1, -1)]
        public void Switch(int x, int expected)
        {
            Invoke("switches", x).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("a", 1)]
        [DataRow("b", 2)]
        [DataRow("Aa", 3)]
        [DataRow("BB", 4)]
        [DataRow("c", 0)]
        public void StringSwitch(string s, int expected)
        {
            // "Aa" and "BB" have the same hash code
            Invoke("stringSwitch", s).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(7, 2, 4)]
        [DataRow(-7, 2, -4)]
        [DataRow(int.MinValue, -1, int.MinValue)]
        public void IntegerDivisionAndRemainder(int a, int b, int expected)
        {
            Invoke("intDivRem", a, b).Should().Be(expected);
        }

        [TestMethod]
        public void IntegerDivisionByZeroIsArithmeticException()
        {
            Invoke("divByZero", 0, 0L).Should().Be(1110);
            Invoke("divByZero", 1, 1L).Should().Be(2);
        }

        [DataTestMethod]
        [DataRow(7L, 2L, 4L)]
        [DataRow(long.MinValue, -1L, long.MinValue)]
        public void LongDivisionAndRemainder(long a, long b, long expected)
        {
            Invoke("longDivRem", a, b).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(-12345, 3)]
        [DataRow(-12345, 35)]
        [DataRow(int.MinValue, 31)]
        public void Shifts(int a, int s)
        {
            var e = s & 31;
            Invoke("shifts", a, s).Should().Be((a << e) ^ (a >> e) ^ (int)((uint)a >> e));
        }

        [DataTestMethod]
        [DataRow(-1234567890123L, 3)]
        [DataRow(-1234567890123L, 67)]
        [DataRow(long.MinValue, 63)]
        public void LongShifts(long a, int s)
        {
            var e = s & 63;
            Invoke("longShifts", a, s).Should().Be((a << e) ^ (a >> e) ^ (long)((ulong)a >> e));
        }

        [TestMethod]
        public void FloatingPointToIntegerConversions()
        {
            Invoke("d2i", double.NaN).Should().Be(0);
            Invoke("d2i", 1e20).Should().Be(int.MaxValue);
            Invoke("d2i", -1e20).Should().Be(int.MinValue);
            Invoke("d2i", -1.9).Should().Be(-1);
            Invoke("d2l", double.NaN).Should().Be(0L);
            Invoke("d2l", 1e30).Should().Be(long.MaxValue);
            Invoke("d2l", -1e30).Should().Be(long.MinValue);
            Invoke("f2i", float.NaN).Should().Be(0);
            Invoke("f2i", float.PositiveInfinity).Should().Be(int.MaxValue);
            Invoke("f2l", float.NegativeInfinity).Should().Be(long.MinValue);
        }

        [TestMethod]
        public void FloatingPointComparisons()
        {
            Invoke("compare", 1.0, 2.0).Should().Be(-1);
            Invoke("compare", 2.0, 1.0).Should().Be(1);
            Invoke("compare", 1.0, 1.0).Should().Be(0);
            Invoke("compare", double.NaN, 1.0).Should().Be(2);
            Invoke("compare", 1.0, double.NaN).Should().Be(2);
        }

        [TestMethod]
        public void NarrowingConversions()
        {
            Invoke("narrowing", 0x12345678).Should().Be((sbyte)0x78 * 1000000 + (char)0x5678 + (short)0x5678);
            Invoke("narrowing", -1).Should().Be(-1 * 1000000 + 0xFFFF + -1);
        }

        [TestMethod]
        public void DupForms()
        {
            Invoke("dupArray", new[] { 5 }, 0).Should().Be(11);
            Invoke("dupLongArray", new[] { 5L }, 0).Should().Be(12L);
            Invoke("dupStaticField").Should().Be(12);
        }

        [TestMethod]
        public void TernaryInArguments()
        {
            Invoke("ternaryInArgs", true, 3).Should().Be(3);
            Invoke("ternaryInArgs", false, 3).Should().Be(3);
            Invoke("ternaryInArgs", false, -3).Should().Be(3);
        }

        [DataTestMethod]
        [DataRow(0, "none")]
        [DataRow(1, "ise")]
        [DataRow(2, "iae")]
        [DataRow(3, "error")]
        public void ExceptionMessage(int mode, string expected)
        {
            Invoke("exceptionMessage", mode).Should().Be(expected);
        }

        [TestMethod]
        public void CatchThrowNew()
        {
            Invoke("catchThrowNew").Should().Be(3);
        }

        [TestMethod]
        public void ThrownExceptionHasStackTrace()
        {
            Invoke("thrownExceptionHasStackTrace").Should().Be(true);
        }

        [TestMethod]
        public void Arrays()
        {
            Invoke("arrays", 2).Should().Be(238);
            Invoke("arrayStoreChecked").Should().Be(true);
        }

        [TestMethod]
        public void InstanceOf()
        {
            Invoke("instanceOf", "s").Should().Be(true);
            Invoke("instanceOf", new object()).Should().Be(false);
            Invoke("instanceOf", new object[] { null }).Should().Be(false);
        }

        [TestMethod]
        public void InterfaceCall()
        {
            Invoke("interfaceCall", new java.util.ArrayList()).Should().Be(2);
        }

        [TestMethod]
        public void InstanceMethod()
        {
            var o = Activator.CreateInstance(type);
            InvokeInstance(o, "instanceMethod", 2).Should().Be(2);
            InvokeInstance(o, "instanceMethod", 5).Should().Be(7);
        }

        [TestMethod]
        public void ManyArgumentsWithTry()
        {
            // e = 6, d = 12
            Invoke("manyArgsWithTry", 0, 2, 3, 4, 5).Should().Be(0 + 2 + 3 + 12 + 6);
            Invoke("manyArgsWithTry", 1, 2, 3, 4, 5).Should().Be(-1 + 2 + 3 + 12 + 6);
        }

        [TestMethod]
        public void NullPointerExceptionIsCaught()
        {
            Invoke("nullCheck", "abc").Should().Be(3);
            Invoke("nullCheck", new object[] { null }).Should().Be(-1);
        }

    }

}
