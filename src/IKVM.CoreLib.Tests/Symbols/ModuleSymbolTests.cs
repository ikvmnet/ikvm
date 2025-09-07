using FluentAssertions;

using IKVM.CoreLib.Symbols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols
{

    [TestClass]
    public abstract class ModuleSymbolTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols : SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        [TestMethod]
        public void CanResolve()
        {
            var m = Init.Symbols.ResolveCoreType("System.Object").Module;
            m.Name.Should().Be(typeof(object).Module.Name);
        }

        [TestMethod]
        public void ShouldBeSame()
        {
            var s1 = Init.Symbols.ResolveCoreType("System.Object").Module;
            var s2 = Init.Symbols.ResolveCoreType("System.Object").Module;
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void PropertiesShouldMatch()
        {
            var m = typeof(object).Module;
            var s = Init.Symbols.ResolveCoreType("System.Object").Module;
            s.Name.Should().Be(m.Name);
            s.IsMissing.Should().BeFalse();
            s.FullyQualifiedName.Should().Be(m.FullyQualifiedName);
        }

        [TestMethod]
        public void CanGetFields()
        {
            var m = typeof(object).Module;
            var s = Init.Symbols.ResolveCoreType("System.Object").Module;
            var l = s.GetFields();
        }

        [TestMethod]
        public void CanGetMethods()
        {
            var m = typeof(object).Module;
            var s = Init.Symbols.ResolveCoreType("System.Object").Module;
            var l = s.GetMethods();
        }

        [TestMethod]
        public void CanGetTypes()
        {
            var m = typeof(object).Module;
            var s = Init.Symbols.ResolveCoreType("System.Object").Module;
            var l = s.GetTypes();
        }

        [TestMethod]
        public void CanGetCustomAttributes()
        {
            var m = typeof(object).Module;
            var s = Init.Symbols.ResolveCoreType("System.Object").Module;
            var l = s.GetCustomAttributes(true);
        }

    }

}
