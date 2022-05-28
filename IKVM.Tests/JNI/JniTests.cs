using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.JNI
{

    [TestClass]
    public class JniTests
    {

        class JavaSourceFromString : global::javax.tools.SimpleJavaFileObject
        {

            readonly string code;

            public JavaSourceFromString(string name, string code) :
                base(global::java.net.URI.create("string:///" + name.Replace(".", "/") + global::javax.tools.JavaFileObject.Kind.SOURCE.extension), global::javax.tools.JavaFileObject.Kind.SOURCE)
            {
                this.code = code;
            }

            public override global::java.lang.CharSequence getCharContent(bool ignoreEncodingErrors)
            {
                return code;
            }

        }

        [TestMethod]
        public void Can_get_javac_instance()
        {
            var s = new StreamReader(typeof(JniTests).Assembly.GetManifestResourceStream("IKVM.Tests.JNI.JniTests.java")).ReadToEnd();
            var f = global::java.util.Arrays.asList(new JavaSourceFromString("ikvm.tests.JNI.JniTests", s));
            var c = global::javax.tools.ToolProvider.getSystemJavaCompiler();
            var m = c.getStandardFileManager(null, null, null);
            m.setLocation(global::javax.tools.StandardLocation.CLASS_OUTPUT, global::java.util.Arrays.asList(new global::java.io.File(@"C:\foo")));
            var w = new global::java.io.StringWriter();
            var t = c.getTask(w, m, null, null, null, f).call().booleanValue();
            if (t == false)
                throw new Exception(w.toString());
        }

    }

}
