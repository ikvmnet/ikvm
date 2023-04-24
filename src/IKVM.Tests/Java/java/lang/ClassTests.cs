using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using IKVM.Tests.Util;

using java.nio.file;
using java.util;
using java.util.function;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{


    [TestClass]
    public class ClassTests
    {

        [TestMethod]
        public void CanGetMethodAndInvoke()
        {
            var s = new StreamReader(typeof(ClassTests).Assembly.GetManifestResourceStream("IKVM.Tests.Java.java.lang.ClassTests.java")).ReadToEnd();
            var f = new InMemoryCodeUnit("ikvm.tests.java.lang.ClassTests", s);
            var c = new InMemoryCompiler(new[] { f });
            c.Compile();
            var z = c.GetClass("ikvm.tests.java.lang.ClassTests");
            if (z == null)
                throw new Exception();

            var m = z.getMethod("main", new global::java.lang.Class[] { typeof(string[]) });
            m.invoke(null, new object[] { new string[] { "TEST" } });
        }

        [TestMethod]
        public void CanSetFieldSetAccessibleOnBase()
        {
            var bcp = global::java.lang.System.getProperty("sun.boot.class.path").Split(global::java.io.File.pathSeparatorChar);
            var all = bcp.SelectMany(i => EnumerateClassFiles(Paths.get(i))).ToList();

            if (all.Count < 100)
                throw new Exception("Expected more classes.");

            foreach (var file in all)
            {
                if (file.StartsWith("WrapperGenerator"))
                    continue;

                var c = global::java.lang.Class.forName(file.Replace('/', '.').Substring(0, file.Length - 6), false, null);
                if (c == null)
                    throw new Exception("Could not load BCP class by name.");

                foreach (var f in c.getDeclaredFields())
                {
                    f.setAccessible(false);
                    f.setAccessible(true);
                }
            }
        }

        /// <summary>
        /// Returns an enumerable over all of the class files within the specified root path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IEnumerable<string> EnumerateClassFiles(global::java.nio.file.Path path)
        {
            var file = path.toFile();
            if (file.canRead() == false || file.isDirectory() == false)
                return Enumerable.Empty<string>();

            return Files.walk(path)
                .filter(new DelegatePredicate<global::java.nio.file.Path>(p => p.getFileName().toString().EndsWith(".class")))
                .map(new DelegateFunction<global::java.nio.file.Path, global::java.nio.file.Path>(p => path.relativize(p)))
                .map(new DelegateFunction<global::java.nio.file.Path, string>(p => p.toString().Replace(global::java.io.File.separatorChar, '/')))
                .AsEnumerable<string>();
        }

    }

}
