using System.Linq;

using FluentAssertions;

using IKVM.CoreLib.Symbols.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

    [TestClass]
    public class ReflectionAssemblySymbolTests : AssemblySymbolTests<ReflectionSymbolTestInit, ReflectionSymbolContext>
    {

        [TestMethod]
        public void ResolvedAssemblyShouldBeSame()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            var s1 = Init.Symbols.ResolveAssemblySymbol(a);
            s.Should().BeSameAs(s1);
        }

        [TestMethod]
        public void AssemblyPropertiesShouldMatch()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            s.FullName.Should().Be(a.FullName);
            s.Location.Should().Be(a.Location);
            s.IsMissing.Should().BeFalse();
        }

        [TestMethod]
        public void AssemblyIdentityShouldMatch()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            s.Identity.Name.Should().Be(a.GetName().Name);
            s.Identity.Version.Should().Be(a.GetName().Version);
            s.Identity.CultureName.Should().Be(a.GetName().CultureName);
            s.Identity.PublicKeyToken.Should().BeEquivalentTo(a.GetName().GetPublicKeyToken());
        }

        [TestMethod]
        public void CanGetAssemblyModules()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            var l = s.GetModules();
            l.Length.Should().Be(1);
            l[0].Should().NotBeNull();
            l[0].Name.Should().Be("Microsoft.VisualStudio.TestPlatform.TestFramework.dll");
        }

        [TestMethod]
        public void CanGetTypes()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            var l = s.GetTypes();
        }

        [TestMethod]
        public void CanGetCustomAttributes()
        {
            var a = typeof(TestClassAttribute).Assembly;
            var s = Init.Symbols.ResolveAssemblySymbol(a);
            var l = s.GetCustomAttributes(true);
            l.Should().HaveCountGreaterThan(5);

            var companyAttributeType = Init.Symbols.ResolveCoreType("System.Reflection.AssemblyCompanyAttribute");
            var companyAttribute = l.Single(e => e.AttributeType == companyAttributeType);
            companyAttribute.NamedArguments.Should().HaveCount(0);
            companyAttribute.ConstructorArguments.Should().HaveCount(1);
            companyAttribute.ConstructorArguments[0].ArgumentType.Should().Be(Init.Symbols.ResolveCoreType("System.String"));
            ((string?)companyAttribute.ConstructorArguments[0].Value).Should().Be("Microsoft Corporation");
        }

    }

}
