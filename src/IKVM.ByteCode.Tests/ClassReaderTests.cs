using System.IO;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests
{

    [TestClass]
    public class ClassReaderTests
    {

        [TestMethod]
        public async Task CanLoadClass()
        {
            using var f = File.OpenRead(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            using var s = new ClassReader(f);
            var c = await s.ReadAsync();
            c.Should().NotBeNull();
            c.Name.Should().Be("0");
            var a = c.Methods[0].Attributes.Get<CodeAttributeData>();
            var z = a.Code;
            var l = a.Attributes;
        }

    }

}
