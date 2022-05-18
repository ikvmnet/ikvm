using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Tests
{

    [TestClass]
    public class FirstTest
    {

        [TestMethod]
        public void Foo()
        {
            ikvmc.IkvmcCompiler.Main(new string[] { @"-out:openjdk.dll", @"@C:\dev\ikvm\ikvm\rebuild\IKVM.OpenJDK\obj\Debug\netcoreapp3.1\ikvmc.args.txt" });
        }

    }

}
