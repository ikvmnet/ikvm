using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Tests.Symbols.Emit;
using IKVM.Reflection.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection.Emit
{

    [TestClass]
    public class IkvmReflectionModuleBuilderTests : ModuleSymbolBuilderTests<IkvmReflectionSymbolTestInit, IkvmReflectionSymbolContext>
    {

        [TestMethod]
        public void ShouldReturnTypeBuilderOnFinish()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var z = Init.Symbols.ResolveType((TypeSymbol)t, IkvmReflectionSymbolState.Finished);
            z.Should().BeOfType<TypeBuilder>();
            z.Namespace.Should().Be("Namespace");
            z.Name.Should().Be("TestType");
            z.FullName.Should().Be("Namespace.TestType");
        }

    }

}
