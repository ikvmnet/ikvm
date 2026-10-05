using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    public class FieldBuilderTests
    {

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSetConstant(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test");
            var field = type.DefineField("value", u.Import(typeof(int)), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
            field.SetConstant(128);
            type.CreateType();
            assembly.Save("Test.dll");

            var a = u.VerifyAndLoad("Test.dll");
            var f = a.GetType("Test")!.GetField("value")!;
            f.FieldType.Should().Be(u.LoadContextType("System.Int32"));
            f.GetRawConstantValue().Should().Be(128);
        }

    }

}
