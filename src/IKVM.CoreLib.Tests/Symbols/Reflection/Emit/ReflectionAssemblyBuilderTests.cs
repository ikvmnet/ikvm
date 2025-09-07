using System.Reflection.Emit;

using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Tests.Symbols.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection.Emit
{

    [TestClass]
    public class ReflectionAssemblyBuilderTests : AssemblySymbolBuilderTests<ReflectionSymbolTestInit, ReflectionSymbolContext>
    {

        [TestMethod]
        public void CanResolveDefined()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            b.FullName.Should().Be("Test, Version=0.0.0.0, PublicKeyToken=null");
            var a = Init.Symbols.ResolveAssembly(b, ReflectionSymbolState.Defined);
            a.Should().BeAssignableTo<AssemblyBuilder>();
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
            var a = Init.Symbols.ResolveAssembly(s, ReflectionSymbolState.Emitted);
            a.Should().BeAssignableTo<AssemblyBuilder>();
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
            var a = Init.Symbols.ResolveAssembly(b, ReflectionSymbolState.Emitted);
            a.Should().BeAssignableTo<AssemblyBuilder>();
            a.FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Name.Should().Be("Test");
            a.GetName().FullName.Should().Be("Test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            a.GetName().Version.Should().Be(new System.Version("0.0.0.0"));
            a.GetName().CultureName.Should().Be("");
            a.GetName().GetPublicKeyToken().Should().BeEmpty();
        }

    }

}
