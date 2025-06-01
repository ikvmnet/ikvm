using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Tests.Symbols.Emit;
using IKVM.Reflection.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection.Emit
{

    [TestClass]
    public class IkvmReflectionAssemblyBuilderTests : AssemblySymbolBuilderTests<IkvmReflectionSymbolTestInit, IkvmReflectionSymbolContext>
    {

        [TestMethod]
        public void CanResolveDefined()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Defined);
            a.Should().BeOfType<AssemblyBuilder>();
            a.FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Name.Should().Be("Test");
            a.GetName().FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Version.Should().Be(new System.Version("0.0.0.0"));
            a.GetName().CultureName.Should().Be("");
            a.GetName().GetPublicKeyToken().Should().BeEmpty();
        }

        [TestMethod]
        public void CanResolveEmitted()
        {
            var s = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var a = Init.Symbols.ResolveAssembly(s, IkvmReflectionSymbolState.Emitted);
            a.Should().BeOfType<AssemblyBuilder>();
            a.FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Name.Should().Be("Test");
            a.GetName().FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Version.Should().Be(new System.Version("0.0.0.0"));
            a.GetName().CultureName.Should().Be("");
            a.GetName().GetPublicKeyToken().Should().BeEmpty();
        }

        [TestMethod]
        public void CanResolveFinished()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Emitted);
            a.Should().BeOfType<AssemblyBuilder>();
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
            var a = Init.Symbols.ResolveAssembly(b, IkvmReflectionSymbolState.Emitted);
            var m = a.GetModule("Test.dll");
            m.Assembly.Should().BeSameAs(a);
            m.Name.Should().Be("Test.dll");
        }

    }

}
