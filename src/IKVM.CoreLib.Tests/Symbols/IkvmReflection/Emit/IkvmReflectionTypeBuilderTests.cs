using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Tests.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection.Emit
{

    [TestClass]
    public class IkvmReflectionTypeBuilderTests : TypeSymbolBuilderTests<IkvmReflectionSymbolTestInit, IkvmReflectionSymbolContext>
    {

        [TestMethod]
        public void ShouldReturnTypeBuilderOnFinish()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var z = Init.Symbols.ResolveType((TypeSymbol)t, IkvmReflectionSymbolState.Finished);
            z.Should().BeAssignableTo<TypeBuilder>();
            z.Namespace.Should().Be("Namespace");
            z.Name.Should().Be("TestType");
            z.FullName.Should().Be("Namespace.TestType");
        }

        [TestMethod]
        public void ShouldReturnTypeOnComplete()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var z = Init.Symbols.ResolveType((TypeSymbol)t, IkvmReflectionSymbolState.Completed);
            z.Should().BeAssignableTo<Type>();
            z.Namespace.Should().Be("Namespace");
            z.Name.Should().Be("TestType");
            z.FullName.Should().Be("Namespace.TestType");
        }

        [TestMethod]
        public void ShouldReturnMethodBuilderOnDeclare()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var j = t.DefineMethod("TestMethod", System.Reflection.MethodAttributes.Public);
            var z = Init.Symbols.ResolveMethod((MethodSymbol)j, IkvmReflectionSymbolState.Declared);
            z.Should().BeOfType<MethodBuilder>();
            z.Name.Should().Be("TestMethod");
        }

        [TestMethod]
        public void ShouldReturnMethodBuilderOnFinish()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var j = t.DefineMethod("TestMethod", System.Reflection.MethodAttributes.Public);
            var z = Init.Symbols.ResolveMethod((MethodSymbol)j, IkvmReflectionSymbolState.Finished);
            z.Should().BeOfType<MethodBuilder>();
            z.Name.Should().Be("TestMethod");
        }

        [TestMethod]
        public void ShouldReturnMethodInfoOnComplete()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var j = t.DefineMethod("TestMethod", System.Reflection.MethodAttributes.Public);
            var z = Init.Symbols.ResolveMethod((MethodSymbol)j, IkvmReflectionSymbolState.Completed);
            z.Should().BeAssignableTo<MethodInfo>();
            z.Should().NotBeSameAs(j);
            z.Name.Should().Be("TestMethod");
        }

        [TestMethod]
        public void ShouldDeclareFieldsInOrder()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var m1 = t.DefineField("TestField2", Init.Symbols.ResolveCoreType("System.Int32"), System.Reflection.FieldAttributes.Public);
            var m2 = t.DefineField("TestField3", Init.Symbols.ResolveCoreType("System.Int32"), System.Reflection.FieldAttributes.Public);
            var m3 = t.DefineField("TestField1", Init.Symbols.ResolveCoreType("System.Int32"), System.Reflection.FieldAttributes.Public);
            var m4 = t.DefineField("TestField4", Init.Symbols.ResolveCoreType("System.Int32"), System.Reflection.FieldAttributes.Public);
            var z = Init.Symbols.ResolveType(t, IkvmReflectionSymbolState.Finished);
            var l = z.__GetDeclaredFields();
            l.Should().Satisfy(
                i => i.Name == "TestField2",
                i => i.Name == "TestField3",
                i => i.Name == "TestField1",
                i => i.Name == "TestField4");
        }

        [TestMethod]
        public void ShouldDeclareMethodsInOrder()
        {
            var b = Init.Symbols.DefineAssembly(new AssemblyIdentity("Test"), []);
            var m = b.DefineModule("Test.dll", "Test.dll");
            var t = m.DefineType("Namespace.TestType");
            var m1 = t.DefineMethod("TestMethod2", System.Reflection.MethodAttributes.Public);
            var m2 = t.DefineMethod("TestMethod3", System.Reflection.MethodAttributes.Public);
            var m3 = t.DefineMethod("TestMethod1", System.Reflection.MethodAttributes.Public);
            var m4 = t.DefineMethod("TestMethod4", System.Reflection.MethodAttributes.Public);
            var z = Init.Symbols.ResolveType(t, IkvmReflectionSymbolState.Finished);
            var l = z.__GetDeclaredMethods();
            l.Should().Satisfy(
                i => i.Name == "TestMethod2",
                i => i.Name == "TestMethod3",
                i => i.Name == "TestMethod1",
                i => i.Name == "TestMethod4");
        }

    }

}
