using FluentAssertions;

using javax.script;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.javax.script
{

    [TestClass]
    public class NashornAccessTests
    {

        class SharedObject
        {

            public string publicString = "PublicString";

        }

        static ScriptEngine e;
        static SharedObject o;

        [TestInitialize]
        public void Initialize()
        {
            var m = new ScriptEngineManager();
            e = m.getEngineByName("nashorn");
            o = new SharedObject();
            e.put("o", o);
            e.eval("var SharedObject = Packages.cli.IKVM.Tests.Java.javax.script.NashornAccessTests$SharedObject;");
        }

    }

}
