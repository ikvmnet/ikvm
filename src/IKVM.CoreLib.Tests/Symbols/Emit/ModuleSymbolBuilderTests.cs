using System;
using System.Reflection;
using System.Reflection.Emit;

using FluentAssertions;

using IKVM.CoreLib.Symbols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Emit
{

    public abstract class ModuleSymbolBuilderTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols: SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        [TestMethod]
        public void ThrowsOnFreeze()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            m.Freeze();
            m.Invoking(_ => _.DefineType("Namespace.TestType", TypeAttributes.Public)).Should().ThrowExactly<InvalidOperationException>();
        }

        [TestMethod]
        public void CanDefineGlobalMethod()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            var f = m.DefineGlobalMethod("TestMethod", MethodAttributes.Public | MethodAttributes.Static, null, []);
            f.Name.Should().Be("TestMethod");
            f.Module.Should().Be(m);
            f.Assembly.Should().Be(a);
            f.Attributes.Should().HaveFlag(MethodAttributes.Public);
            f.Attributes.Should().HaveFlag(MethodAttributes.Static);
            var il = f.GetILGenerator();
            il.Emit(OpCodes.Ret);
        }

        [TestMethod]
        public void CanDefineType()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType", TypeAttributes.Public);
            t.Assembly.Should().Be(a);
            t.Module.Should().Be(m);
            t.Name.Should().Be("TestType");
            t.FullName.Should().Be("Namespace.TestType");
            t.Attributes.Should().HaveFlag(TypeAttributes.Public);
            t.Attributes.Should().HaveFlag(TypeAttributes.Class);
        }

    }

}
