using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IKVM.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{


    [TestClass]
    public class ClassTests
    {

        [TestMethod]
        public void Can_get_method_and_invoke()
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

    }

}
