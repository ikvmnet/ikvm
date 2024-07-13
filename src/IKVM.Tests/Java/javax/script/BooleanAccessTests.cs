using FluentAssertions;

using javax.script;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.script
{

    [TestClass]
    public class BooleanAccessTests
    {

        static ScriptEngine e;
        static SharedObject o;

        [TestInitialize]
        public void Initialize()
        {
            var m = new ScriptEngineManager();
            e = m.getEngineByName("nashorn");
            o = new SharedObject();
            e.put("o", o);
            e.eval("var SharedObject = Packages.cli.IKVM.Tests.Java.javax.script.SharedObject;");
        }

        [TestCleanup]
        public void Cleanup()
        {
            e = null;
            o = null;
        }

        [TestMethod]
        public void AccessFieldBoolean()
        {
            e.eval("var p_boolean = o.publicBoolean;");
            o.publicBoolean.Should().Be(((global::java.lang.Boolean)e.get("p_boolean")).booleanValue());
            e.eval("typeof p_boolean;").Should().Be("boolean");
            e.eval("o.publicBoolean = false;");
            o.publicBoolean.Should().Be(false);
        }

    }

}
