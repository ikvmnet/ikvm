using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Tests.Symbols.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection.Emit
{

    [TestClass]
    public class IkvmReflectionAssemblyBuilderTests : AssemblySymbolBuilderTests<IkvmReflectionSymbolTestInit, IkvmReflectionSymbolContext>
    {

        [TestMethod]
        public void CanDeclareAssembly()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Declared);
            a.FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Name.Should().Be("Test");
            a.GetName().FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Version.Should().Be(new System.Version("0.0.0.0"));
            a.GetName().CultureName.Should().Be("");
            a.GetName().GetPublicKeyToken().Should().BeEmpty();
        }

        [TestMethod]
        public void CanFinishAssembly()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Finished);
            a.FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Name.Should().Be("Test");
            a.GetName().FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Version.Should().Be(new System.Version("0.0.0.0"));
            a.GetName().CultureName.Should().Be("");
            a.GetName().GetPublicKeyToken().Should().BeEmpty();
        }

        [TestMethod]
        public void ShouldDeclareModuleOnFinish()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            b.DefineModule("Test.dll", "Test.dll");
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Finished);
            var m = a.GetModule("Test.dll");
            m.Assembly.Should().BeSameAs(a);
            m.Name.Should().Be("Test.dll");
        }

    }

}
