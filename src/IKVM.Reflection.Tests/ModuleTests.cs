using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests
{

    public class ModuleTests
    {

        const string Ns = "IKVM.Reflection.Tests.Fixture.";

        /// <summary>
        /// Plain names are looked up without the type name parser, so these cover the names that take that path and the
        /// ones that must still be left to the parser, which decides the expected results.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expected">The full name of the type that is found, or null for none.</param>
        [Theory]
        [InlineData(Ns + "AttributedClass", Ns + "AttributedClass")]
        [InlineData(Ns + "GenericClass`1", Ns + "GenericClass`1")]
        [InlineData(Ns + "GenericClass`1+NestedInGeneric", Ns + "GenericClass`1+NestedInGeneric")]
        [InlineData(Ns + "AttributedClass[]", Ns + "AttributedClass[]")]
        [InlineData(" " + Ns + "AttributedClass", Ns + "AttributedClass")]
        [InlineData(Ns + "NoSuchType", null)]
        [InlineData(Ns + "NoSuchType+Nested", null)]
        [InlineData(Ns + "GenericClass`1+NoSuchNested", null)]
        [InlineData("+" + Ns + "AttributedClass", null)]
        [InlineData(Ns + "AttributedClass+", null)]
        [InlineData(Ns + "GenericClass`1++NestedInGeneric", null)]
        [InlineData("System.Object", null)]
        [InlineData("", null)]
        public void GetTypeFindsTypesOfModule(string name, string expected)
        {
            using var u = TestUniverse.Create(Fixture.HostTargetFramework);
            var module = u.Universe.LoadFile(Fixture.GetAssemblyPath(Fixture.HostTargetFramework)).ManifestModule;

            module.GetType(name, false, false)?.FullName.Should().Be(expected);
            if (expected == null)
                module.GetType(name, false, false).Should().BeNull();
        }

    }

}
