using System.Collections.Immutable;

using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Emit
{

    public abstract class AssemblySymbolBuilderTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols: SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        protected abstract AssemblySymbolBuilder DefineAssembly(AssemblyIdentity identity, ImmutableArray<CustomAttribute> customAttributes);

        [TestMethod]
        public void CanDefineModule()
        {
            var a = DefineAssembly(new AssemblyIdentity("Test"), []);
            a.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var m = a.DefineModule("Test", "Test.dll");
            m.Name.Should().Be("Test");
            m.FileName.Should().Be("Test.dll");
        }

    }

}
