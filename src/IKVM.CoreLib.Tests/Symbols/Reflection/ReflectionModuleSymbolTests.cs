using FluentAssertions;

using IKVM.CoreLib.Symbols.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

    [TestClass]
    public class ReflectionModuleSymbolTests
    {

        [TestMethod]
        public void CanResolve()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = c.ResolveModuleSymbol(typeof(object).Module);
            m.Name.Should().Be(typeof(object).Module.Name);
        }

        [TestMethod]
        public void ShouldBeSame()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var s = c.ResolveModuleSymbol(typeof(object).Module);
            var s1 = c.ResolveModuleSymbol(typeof(object).Module);
            s.Should().BeSameAs(s1);
        }

        [TestMethod]
        public void PropertiesShouldMatch()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = typeof(object).Module;
            var s = c.ResolveModuleSymbol(m);
            s.Name.Should().Be(m.Name);
            s.IsMissing.Should().BeFalse();
            s.ContainsMissing.Should().BeFalse();
            s.IsComplete.Should().BeTrue();
            s.FullyQualifiedName.Should().Be(m.FullyQualifiedName);
        }

        [TestMethod]
        public void CanGetFields()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = typeof(object).Module;
            var s = c.ResolveModuleSymbol(m);
            var l = s.GetFields();
        }

        [TestMethod]
        public void CanGetMethods()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = typeof(object).Module;
            var s = c.ResolveModuleSymbol(m);
            var l = s.GetMethods();
        }

        [TestMethod]
        public void CanGetTypes()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = typeof(object).Module;
            var s = c.ResolveModuleSymbol(m);
            var l = s.GetTypes();
        }

        [TestMethod]
        public void CanGetCustomAttributes()
        {
            var c = new ReflectionSymbolContext(typeof(object).Assembly);
            var m = typeof(object).Module;
            var s = c.ResolveModuleSymbol(m);
            var l = s.GetCustomAttributes(true);
        }

    }

}
