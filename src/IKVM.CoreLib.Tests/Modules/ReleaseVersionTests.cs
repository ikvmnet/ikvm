using IKVM.CoreLib.Modules;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Modules
{

    [TestClass]
    public class ReleaseVersionTests
    {

        [TestMethod]
        public void Foo()
        {
            var v = RuntimeVersion.Parse("1");
        }

    }

}
