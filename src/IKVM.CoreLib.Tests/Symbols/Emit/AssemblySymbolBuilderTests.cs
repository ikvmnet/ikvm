using System;
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

        [TestMethod]
        public void ThrowsOnFreeze()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            a.Freeze();
            a.Invoking(_ => _.DefineModule("Test.dll", "Test.dll")).Should().ThrowExactly<InvalidOperationException>();
        }

        [TestMethod]
        public void CanDefineModule()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            a.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var m = a.DefineModule("Test.dll", "Test.dll");
            m.Name.Should().Be("Test.dll");
            m.ScopeName.Should().Be("Test.dll");
        }

    }

}
