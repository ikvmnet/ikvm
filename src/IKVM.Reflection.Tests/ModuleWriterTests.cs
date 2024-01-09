using System.IO;

using IKVM.Reflection.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Reflection.Tests
{

    [TestClass]
    public class ModuleWriterTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanWriteNetFxModule()
        {
            var d = Path.Combine(TestContext.TestRunResultsDirectory, "IKVK.Reflection.Tests", "ModuleWriterTests");
            if (Directory.Exists(d))
                Directory.Delete(d, true);
            Directory.CreateDirectory(d);

            var u = new Universe("mscorlib");
            var a = u.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, d);
            var m = a.DefineDynamicModule("Test", "Test.dll", false);
            a.Save("Test.dll");
        }

    }

}
