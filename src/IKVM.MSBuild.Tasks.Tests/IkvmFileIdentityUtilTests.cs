using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmFileIdentityUtilTests
    {

        [TestMethod]
        public async Task CanSaveState()
        {
            var u = new IkvmFileIdentityUtil(new IkvmAssemblyInfoUtil());

            var f = Path.GetTempFileName();
            File.WriteAllText(f, "TEST");
            var i = await u.GetIdentityForFileAsync(f, CancellationToken.None);

            var x = new XElement("Test");
            await u.SaveStateXmlAsync(x);

            x.Should().HaveElement("File").Which.Should().HaveAttribute("Path", f).And.HaveAttribute("Identity", i);
        }

    }

}
