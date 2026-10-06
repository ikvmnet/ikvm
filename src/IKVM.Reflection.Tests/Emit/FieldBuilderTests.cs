using System.Linq;

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

        /// <summary>
        /// The constant and marshalling descriptor of a field builder can be read back from the created type, before and
        /// after saving.
        /// </summary>
        /// <param name="tfm"></param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanReadBackConstantAndMarshal(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var marshalAs = u.Import(typeof(System.Runtime.InteropServices.MarshalAsAttribute));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test", TypeAttributes.Public);
            type.DefineField("constant", u.Import(typeof(string)), FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal).SetConstant("value");
            type.DefineField("marshalled", u.Import(typeof(string)), FieldAttributes.Public).SetCustomAttribute(new CustomAttributeBuilder(marshalAs.GetConstructor([u.Import(typeof(System.Runtime.InteropServices.UnmanagedType))]), [System.Runtime.InteropServices.UnmanagedType.LPWStr]));
            var created = type.CreateType();

            void Check()
            {
                created.GetField("constant")!.GetRawConstantValue().Should().Be("value");
                created.GetField("marshalled")!.__TryGetFieldMarshal(out var marshal).Should().BeTrue();
                marshal.UnmanagedType.Should().Be(System.Runtime.InteropServices.UnmanagedType.LPWStr);
                created.GetField("constant")!.__TryGetFieldMarshal(out _).Should().BeFalse();
            }

            Check();
            assembly.Save("Test.dll");
            Check();

            var t = u.VerifyAndLoad("Test.dll").GetType("Test")!;
            t.GetField("constant")!.GetRawConstantValue().Should().Be("value");
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanSetOffset(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var intType = u.Import(typeof(int));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Test", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.ExplicitLayout, u.Import(typeof(System.ValueType)));
            type.DefineField("a", intType, FieldAttributes.Public).SetOffset(0);
            type.DefineField("b", intType, FieldAttributes.Public).SetOffset(4);
            type.DefineField("c", intType, FieldAttributes.Public).SetOffset(4);
            type.CreateType();
            assembly.Save("Test.dll");

            var t = u.VerifyAndLoad("Test.dll").GetType("Test")!;
            t.IsExplicitLayout.Should().BeTrue();
            t.GetFields().Select(i => (i.Name, Offset: (int)i.GetCustomAttributesData().Single(a => a.AttributeType.Name == "FieldOffsetAttribute").ConstructorArguments[0].Value!)).Should().Equal(("a", 0), ("b", 4), ("c", 4));
        }

    }

}
