using FluentAssertions;

using IKVM.CoreLib.Symbols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols
{

    public abstract class AssemblySymbolTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols : SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        [TestMethod]
        public void SystemObjectShouldNotBeNull()
        {
            Init.Symbols.ResolveCoreType("System.Object").Should().NotBeNull();
        }

    }

}
