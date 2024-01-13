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
            var d = Path.Combine(Path.GetTempPath(), "IKVM.Reflection.Tests", "ModuleWriterTests");
            if (Directory.Exists(d))
                Directory.Delete(d, true);
            Directory.CreateDirectory(d);

            var u = new Universe("mscorlib");
            u.AssemblyResolve += U_AssemblyResolve;
            var a = u.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, d);
            var m = a.DefineDynamicModule("Test", "Test.dll", false);
            var t = m.DefineType("Type");
            var z = t.DefineMethod("Exec", MethodAttributes.Public, u.Import(typeof(object)), new[] { u.Import(typeof(string[])) });
            z.DefineParameter(0, ParameterAttributes.None, "args");
            a.Save("Test.dll");
            TestContext.WriteLine(d);
        }

        Assembly U_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var d = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8.1";
            var p = Path.Combine(d, args.Name + ".dll");
            if (File.Exists(p))
                return ((Universe)sender).LoadFile(p);

            return null;
        }

        [TestMethod]
        public void CanWriteNetCoreModule()
        {
            var d = Path.Combine(TestContext.TestRunResultsDirectory, "IKVM.Reflection.Tests", "ModuleWriterTests");
            if (Directory.Exists(d))
                Directory.Delete(d, true);
            Directory.CreateDirectory(d);

            var u = new Universe("System.Runtime");
            var a = u.DefineDynamicAssembly(new AssemblyName("Test"), AssemblyBuilderAccess.Save, d);
            var m = a.DefineDynamicModule("Test", "Test.dll", false);
            m.DefineType("Type");
            a.Save("Test.dll");
        }

    }

}
