using System.IO;

using FluentAssertions;

using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.IkvmReflection
{

    [TestClass]
    public class IkvmReflectionSymbolTests
    {

        class Foo<T>
        {

            T? field;

            bool Method(int p1) => true;

        }

        Universe? universe;
        Assembly? coreAssembly;
        Assembly? thisAssembly;

        [TestInitialize]
        public void Setup()
        {
            universe = new Universe(typeof(object).Assembly.GetName().Name);
            universe.AssemblyResolve += Universe_AssemblyResolve;
            coreAssembly = universe.LoadFile(typeof(object).Assembly.GetAssemblyLocation());
            thisAssembly = universe.LoadFile(typeof(IkvmReflectionSymbolTests).Assembly.GetAssemblyLocation());
        }

        /// <summary>
        /// Attempt to load assembly from system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Assembly? Universe_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var asm = System.Reflection.Assembly.Load(args.Name);
                if (asm != null && File.Exists(asm.Location))
                    return universe!.LoadFile(asm.Location);
            }
            catch
            {

            }

            return null;
        }

        [TestMethod]
        public void SameTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(universe!.GetBuiltInType("System", "Object"));
            var s2 = c.GetOrCreateTypeSymbol(universe!.GetBuiltInType("System", "Object"));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeDefinitionShouldBeSame()
        {
            var t = thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1");
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeShouldBeSame()
        {
            var t = thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1").MakeGenericType(universe!.GetBuiltInType("System", "Int32"));
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void ArrayTypeShouldBeSame()
        {
            var t = universe!.GetBuiltInType("System", "Object").MakeArrayType(2);
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void SZArrayTypeShouldBeSame()
        {
            var t = universe!.GetBuiltInType("System", "Object").MakeArrayType();
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void PointerTypeShouldBeSame()
        {
            var t = universe!.GetBuiltInType("System", "Int32").MakePointerType();
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void ByRefTypeShouldBeSame()
        {
            var t = universe!.GetBuiltInType("System", "Int32").MakeByRefType();
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void EnumTypeShouldBeSame()
        {
            var a = universe!.Load(typeof(System.AttributeTargets).Assembly.FullName);
            var t = a.GetType("System.AttributeTargets");
            var c = new IkvmReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(t);
            var s2 = c.GetOrCreateTypeSymbol(t);
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void CanGetType()
        {
            var t = universe!.GetBuiltInType("System", "Object");
            var c = new IkvmReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(t);
            s.Name.Should().Be("Object");
            s.FullName.Should().Be("System.Object");
        }

        [TestMethod]
        public void CanGetFieldOfGenericTypeDefinition()
        {
            var t = thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1");
            var c = new IkvmReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(t);
            s.IsGenericType.Should().BeTrue();
            s.IsGenericTypeDefinition.Should().BeTrue();
            var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            f.Name.Should().Be("field");
            f.FieldType.IsGenericType.Should().BeFalse();
            f.FieldType.IsGenericParameter.Should().BeTrue();
        }

        [TestMethod]
        public void CanGetFieldOfGenericType()
        {
            var t = thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1").MakeGenericType(universe!.GetBuiltInType("System", "Int32"));
            var c = new IkvmReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(t);
            s.IsGenericType.Should().BeTrue();
            s.IsGenericTypeDefinition.Should().BeFalse();
            var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            f.Name.Should().Be("field");
            f.FieldType.IsGenericType.Should().BeFalse();
            f.FieldType.IsGenericParameter.Should().BeFalse();
            f.FieldType.Should().BeSameAs(c.GetOrCreateTypeSymbol(universe!.GetBuiltInType("System", "Int32")));
        }

        [TestMethod]
        public void CanGetMethod()
        {
            var t = universe!.GetBuiltInType("System", "Object");
            var c = new IkvmReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(t);
            var m = s.GetMethod("ToString");
            m.Name.Should().Be("ToString");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(universe!.GetBuiltInType("System", "String")));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(universe!.GetBuiltInType("System", "String")));
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
        }

        [System.AttributeUsage(System.AttributeTargets.Class)]
        class AttributeWithType : System.Attribute
        {

            public AttributeWithType(System.Type type)
            {
                Type = type;
            }

            public System.Type Type { get; }

        }

        [AttributeWithType(typeof(object))]
        class ClassWithAttributeWithType
        {



        }

        [TestMethod]
        public void CanReadCustomAttributes()
        {
            var c = new IkvmReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+ClassWithAttributeWithType"));
            var a = s.GetCustomAttribute(c.GetOrCreateTypeSymbol(thisAssembly!.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+AttributeWithType")));
            var v = a.Value.ConstructorArguments[0].Value;
            v.Should().BeOfType<IkvmReflectionTypeSymbol>();
        }

    }

}
