using System;
using System.IO;

using IKVM.Java.Tests.Util;

namespace IKVM.Tests.JNI
{

    /// <summary>
    /// Compiles the Java side of the JNI tests. They are compiled together so that they share a class loader, because
    /// a native library can only be loaded by one class loader.
    /// </summary>
    static class JniTestClasses
    {

        /// <summary>
        /// A class that only exists to be defined with DefineClass.
        /// </summary>
        public const string DefinedByJniClassName = "ikvm.tests.jni.DefinedByJni";

        const string DefinedByJniSource = """
            package ikvm.tests.jni;

            public class DefinedByJni
            {

                public int value()
                {
                    return 42;
                }

            }
            """;

        static readonly Lazy<InMemoryCompiler> compiler = new(Compile);

        /// <summary>
        /// Gets the compiler holding the compiled classes.
        /// </summary>
        public static InMemoryCompiler Compiler => compiler.Value;

        static InMemoryCompiler Compile()
        {
            var compiler = new InMemoryCompiler([
                new InMemoryCodeUnit("ikvm.tests.jni.JniTests", ReadSource("IKVM.Tests.JNI.JniTests.java")),
                new InMemoryCodeUnit("ikvm.tests.jni.JniEnvTests", ReadSource("IKVM.Tests.JNI.JniEnvTests.java")),
                new InMemoryCodeUnit(DefinedByJniClassName, DefinedByJniSource),
            ]);

            compiler.Compile();
            return compiler;
        }

        static string ReadSource(string resourceName)
        {
            using var stream = typeof(JniTestClasses).Assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }

}
