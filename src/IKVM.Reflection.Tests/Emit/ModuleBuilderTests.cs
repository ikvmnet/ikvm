using System.IO;

using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    public class ModuleBuilderTests
    {

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanDefineManifestResource(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            module.DefineManifestResource("Resource1", new MemoryStream([0x01, 0x02, 0x03, 0x04, 0x05]), ResourceAttributes.Public);
            module.DefineManifestResource("Resource2", new MemoryStream([0x06, 0x07, 0x08, 0x09, 0x0a]), ResourceAttributes.Public);
            module.DefineManifestResource("Resource3", new MemoryStream([0x0b, 0x0c, 0x0d, 0x0e, 0x0f]), ResourceAttributes.Public);
            type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            a.GetManifestResourceNames().Should().BeEquivalentTo("Resource1", "Resource2", "Resource3");
            ReadAll(a.GetManifestResourceStream("Resource1")!).Should().Equal(0x01, 0x02, 0x03, 0x04, 0x05);
            ReadAll(a.GetManifestResourceStream("Resource2")!).Should().Equal(0x06, 0x07, 0x08, 0x09, 0x0a);
            ReadAll(a.GetManifestResourceStream("Resource3")!).Should().Equal(0x0b, 0x0c, 0x0d, 0x0e, 0x0f);
        }

        static byte[] ReadAll(Stream stream)
        {
            using var m = new MemoryStream();
            stream.CopyTo(m);
            return m.ToArray();
        }

    }

}
