using System;
using System.Reflection;
using System.Reflection.Emit;

using FluentAssertions;

using IKVM.CoreLib.Symbols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Emit
{

    public abstract class TypeSymbolBuilderTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols: SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        [TestMethod]
        public void ThrowsOnFreeze()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Test");
            t.Freeze();
            t.Invoking(_ => _.SetParent(null)).Should().ThrowExactly<InvalidOperationException>();
        }

        [TestMethod]
        public void CanDefineMethod()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Test");
            var f = t.DefineMethod("TestMethod", MethodAttributes.Public | MethodAttributes.Static, null, []);
            f.Name.Should().Be("TestMethod");
            f.Module.Should().Be(m);
            f.Assembly.Should().Be(a);
            f.Attributes.Should().HaveFlag(MethodAttributes.Public);
            f.Attributes.Should().HaveFlag(MethodAttributes.Static);
            var il = f.GetILGenerator();
            il.Emit(OpCodes.Ret);
        }

        [TestMethod]
        public void CanDefineNestedType()
        {
            var a = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = a.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType", TypeAttributes.Public);
            var n = t.DefineNestedType("NestedType");
            n.Assembly.Should().Be(a);
            n.Module.Should().Be(m);
            n.Name.Should().Be("NestedType");
            n.FullName.Should().Be("Namespace.TestType+NestedType");
            n.Attributes.Should().HaveFlag(TypeAttributes.Public);
            n.Attributes.Should().HaveFlag(TypeAttributes.Class);
        }

    }

}
