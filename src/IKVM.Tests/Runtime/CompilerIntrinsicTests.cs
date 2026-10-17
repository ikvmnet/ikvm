using System;
using System.Reflection;

using FluentAssertions;

using IKVM.Java.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Runs intrinsic patterns through the bytecode to IL compiler. The outer finally handlers are compiled once per
    /// exception table entry that targets them, so intrinsics in them are emitted more than once (issue #128).
    /// </summary>
    [TestClass]
    public class CompilerIntrinsicTests
    {

        const string ClassName = "CompilerIntrinsicTest";

        const string Source = """
            import java.lang.reflect.Field;

            public class CompilerIntrinsicTest {

                static Object o = new Object();

                static void f(int mode) {
                    if (mode == 1) throw new IllegalStateException("ise");
                }

                static String getClassEqualsClassLiteral(Object v, int mode) {
                    StringBuilder sb = new StringBuilder();
                    try {
                        try { f(mode); sb.append("a"); } finally { sb.append("b"); }
                    } catch (RuntimeException e) {
                        sb.append("c");
                    } finally {
                        if (v.getClass() == Object.class) sb.append("o");
                        if (v.getClass() != String.class) sb.append("n");
                    }
                    return sb.toString();
                }

                static void getClassEqualsClassLiteralWithoutCatch() {
                    try { try { System.out.println("body"); } finally { System.out.println("inner"); } }
                    finally { if (o.getClass() == Object.class) System.out.println("fin"); }
                }

                static String getClassEqualsGetClass(String a, Object b, int mode) {
                    StringBuilder sb = new StringBuilder();
                    try {
                        try { f(mode); sb.append("a"); } finally { sb.append("b"); }
                    } catch (RuntimeException e) {
                        sb.append("c");
                    } finally {
                        if (a.getClass() == b.getClass()) sb.append("s");
                        if (a.getClass() != b.getClass()) sb.append("d");
                    }
                    return sb.toString();
                }

                static String unsafeGetObjectWithCheckcast(String[] a, int mode) throws Exception {
                    Field field = sun.misc.Unsafe.class.getDeclaredField("theUnsafe");
                    field.setAccessible(true);
                    sun.misc.Unsafe unsafe = (sun.misc.Unsafe) field.get(null);
                    long offset = unsafe.arrayBaseOffset(String[].class);
                    StringBuilder sb = new StringBuilder();
                    try {
                        try { f(mode); sb.append("a"); } finally { sb.append("b"); }
                    } catch (RuntimeException e) {
                        sb.append("c");
                    } finally {
                        String s = (String) unsafe.getObject(a, offset);
                        sb.append(s);
                        s = (String) unsafe.getObjectVolatile(a, offset);
                        sb.append(s);
                    }
                    return sb.toString();
                }

                @SuppressWarnings("finally")
                static String throwNewInFinally(int mode) {
                    StringBuilder sb = new StringBuilder();
                    try {
                        try {
                            try { f(mode); sb.append("a"); } finally { sb.append("b"); }
                        } catch (IllegalStateException e) {
                            sb.append("c");
                        } finally {
                            throw new IllegalArgumentException("iae");
                        }
                    } catch (IllegalArgumentException e) {
                        sb.append(e.getMessage());
                    }
                    return sb.toString();
                }

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

        [TestMethod]
        public void GetClassEqualsClassLiteralInNestedFinally()
        {
            Invoke("getClassEqualsClassLiteralWithoutCatch");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void GetClassEqualsClassLiteralInNestedFinallyWithCatch(int mode)
        {
            var prefix = mode == 0 ? "ab" : "bc";
            Invoke("getClassEqualsClassLiteral", new object(), mode).Should().Be(prefix + "on");
            Invoke("getClassEqualsClassLiteral", "s", mode).Should().Be(prefix);
            Invoke("getClassEqualsClassLiteral", new java.util.ArrayList(), mode).Should().Be(prefix + "n");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void GetClassEqualsGetClassInNestedFinally(int mode)
        {
            var prefix = mode == 0 ? "ab" : "bc";
            Invoke("getClassEqualsGetClass", "x", "y", mode).Should().Be(prefix + "s");
            Invoke("getClassEqualsGetClass", "x", new java.lang.StringBuilder("y"), mode).Should().Be(prefix + "d");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void UnsafeGetObjectWithCheckcastInNestedFinally(int mode)
        {
            var prefix = mode == 0 ? "ab" : "bc";
            Invoke("unsafeGetObjectWithCheckcast", new[] { "x" }, mode).Should().Be(prefix + "xx");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void ThrowNewInNestedFinally(int mode)
        {
            var prefix = mode == 0 ? "ab" : "bc";
            Invoke("throwNewInFinally", mode).Should().Be(prefix + "iae");
        }

    }

}
