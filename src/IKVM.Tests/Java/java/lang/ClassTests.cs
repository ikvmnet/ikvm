using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using IKVM.Java.Tests.Util;
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
            var all = bcp.SelectMany(EnumerateClassFiles).ToList();

            if (all.Count < 100)
                throw new Exception("Expected more classes.");

            foreach (var file in all.Take(256))
            {
                if (file.StartsWith("WrapperGenerator"))
                    continue;

                var c = global::java.lang.Class.forName(file.Replace(global::java.io.File.separator, ".").Substring(0, file.Length - 6), false, null);
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
        /// <param name="root"></param>
        /// <returns></returns>
        IEnumerable<string> EnumerateClassFiles(string root)
        {
            return EnumerateClassFiles(root, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<string> EnumerateClassFiles(string root, string path)
        {
            foreach (var file in new global::java.io.File(JoinPath(root, path)).listFiles())
            {
                if (file.isFile())
                    yield return JoinPath(path, file.getName());
                else if (file.isDirectory())
                    foreach (var i in EnumerateClassFiles(root, JoinPath(path, file.getName())))
                        yield return i;
            }
        }

        /// <summary>
        /// Combines two paths.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        string JoinPath(string path1, string path2)
        {
            if (path1 != null && path2 != null)
                return path1 + global::java.io.File.separator + path2;
            else if (path1 != null)
                return path1;
            else
                return path2;
        }

    }

}
