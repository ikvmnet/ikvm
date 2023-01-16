using System.IO;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.ByteCode.Reading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests
{

    [TestClass]
    public class ClassReaderTests
    {

        [TestMethod]
        public async Task CanLoadClassAsync()
        {
            using var file = File.OpenRead(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            var clazz = await ClassReader.ReadAsync(file);
            clazz.Should().NotBeNull();
            clazz.Name.Should().Be("0");
            clazz.Methods.Should().HaveCount(2);
            clazz.Fields.Should().HaveCount(0);

            clazz.Methods[1].Attributes.Get<CodeAttributeReader>().Code.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLoadClass()
        {
            using var file = File.OpenRead(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            var clazz = ClassReader.Read(file);
            clazz.Should().NotBeNull();
            clazz.Name.Should().Be("0");
            clazz.Methods.Should().HaveCount(2);
            clazz.Fields.Should().HaveCount(0);

            clazz.Methods[0].Attributes.Get<CodeAttributeReader>().Code.Should().NotBeNull();
            clazz.Methods[1].Attributes.Get<CodeAttributeReader>().Code.Should().NotBeNull();
        }

    }

}
