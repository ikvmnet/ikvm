using System.Linq;

using FluentAssertions;

using IKVM.CoreLib.Symbols.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

    [TestClass]
    public class ReflectionAssemblySymbolTests
    {

        [TestMethod]
        public void SystemObjectShouldNotBeNull()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            c.ResolveCoreType("System.Object").Should().NotBeNull();
        }

        [TestMethod]
        public void ResolvedAssemblyShouldBeSame()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            var s1 = c.ResolveAssemblySymbol(a);
            s.Should().BeSameAs(s1);
        }

        [TestMethod]
        public void AssemblyPropertiesShouldMatch()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            s.FullName.Should().Be(a.FullName);
            s.Location.Should().Be(a.Location);
            s.IsMissing.Should().BeFalse();
            s.ContainsMissing.Should().BeFalse();
            s.IsComplete.Should().BeTrue();
        }

        [TestMethod]
        public void AssemblyIdentityShouldMatch()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            s.GetIdentity().Name.Should().Be(a.GetName().Name);
            s.GetIdentity().Version.Should().Be(a.GetName().Version);
            s.GetIdentity().CultureName.Should().Be(a.GetName().CultureName);
            s.GetIdentity().PublicKeyToken.Should().BeEquivalentTo(a.GetName().GetPublicKeyToken());
        }

        [TestMethod]
        public void CanGetAssemblyModules()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            var l = s.GetModules();
            l.Length.Should().Be(1);
            l[0].Should().NotBeNull();
            l[0].Name.Should().Be("Microsoft.VisualStudio.TestPlatform.TestFramework.dll");
        }

        [TestMethod]
        public void CanGetTypes()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            var l = s.GetTypes();
        }

        [TestMethod]
        public void CanGetCustomAttributes()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var a = typeof(TestClassAttribute).Assembly;
            var s = c.ResolveAssemblySymbol(a);
            var l = s.GetCustomAttributes(true);
            l.Should().HaveCountGreaterThan(5);

            var companyAttributeType = c.ResolveCoreType("System.Reflection.AssemblyCompanyAttribute");
            var companyAttribute = l.Single(e => e.AttributeType == companyAttributeType);
            companyAttribute.NamedArguments.Should().HaveCount(0);
            companyAttribute.ConstructorArguments.Should().HaveCount(1);
            companyAttribute.ConstructorArguments[0].ArgumentType.Should().Be(c.ResolveCoreType("System.String"));
            ((string?)companyAttribute.ConstructorArguments[0].Value).Should().Be("Microsoft Corporation");
        }

    }

}
