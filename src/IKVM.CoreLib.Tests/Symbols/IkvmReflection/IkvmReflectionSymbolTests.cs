using System;
using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

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
        Assembly? Universe_AssemblyResolve(object sender, IKVM.Reflection.ResolveEventArgs args)
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
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object"));
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object"));
            s1.Should().BeOfType<IkvmReflectionTypeSymbol>();
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeDefinitionShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1"));
            var s2 = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1"));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1").MakeGenericType(coreAssembly.GetType("System.Int32")));
            var s2 = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1").MakeGenericType(coreAssembly.GetType("System.Int32")));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void ArrayTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object").MakeArrayType(2));
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object").MakeArrayType(2));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void SZArrayTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakeArrayType());
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakeArrayType());
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void PointerTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakePointerType());
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakePointerType());
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void ByRefTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakeByRefType());
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32").MakeByRefType());
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void EnumTypeShouldBeSame()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s1 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.AttributeTargets"));
            var s2 = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.AttributeTargets"));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void CanResolveType()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object"));
            s.Name.Should().Be("Object");
            s.FullName.Should().Be("System.Object");
        }

        [TestMethod]
        public void CanResolveFieldOfGenericTypeDefinition()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1"));
            s.IsGenericType.Should().BeTrue();
            s.IsGenericTypeDefinition.Should().BeTrue();
            var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            f.Name.Should().Be("field");
            f.FieldType.IsGenericType.Should().BeFalse();
            f.FieldType.IsGenericParameter.Should().BeTrue();
        }

        [TestMethod]
        public void CanResolveFieldOfGenericType()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+Foo`1").MakeGenericType(coreAssembly.GetType("System.Int32")));
            s.IsGenericType.Should().BeTrue();
            s.IsGenericTypeDefinition.Should().BeFalse();
            var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            f.Name.Should().Be("field");
            f.FieldType.IsGenericType.Should().BeFalse();
            f.FieldType.IsGenericParameter.Should().BeFalse();
            f.FieldType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Int32")));
        }

        [TestMethod]
        public void CanResolveMethod()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object"));
            var m = s.GetMethod("ToString");
            m.Name.Should().Be("ToString");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.String")));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.String")));
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
        }

        [TestMethod]
        public void CanResolveGenericMethod()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.ValueTuple"));
            var m = s.GetMethods().FirstOrDefault(i => i.Name == "Create" && i.GetGenericArguments().Length == 1);
            m.Name.Should().Be("Create");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.ValueTuple`1")).MakeGenericType(m.GetGenericArguments()[0]));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.ValueTuple`1")).MakeGenericType(m.GetGenericArguments()[0]));
            m.IsGenericMethod.Should().BeTrue();
            m.IsGenericMethodDefinition.Should().BeTrue();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
        }

        [TestMethod]
        public void CanResolveParameters()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object"));
            var m = s.GetMethod("ReferenceEquals");
            m.Name.Should().Be("ReferenceEquals");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Boolean")));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Boolean")));
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
            var p = m.GetParameters();
            p.Length.Should().Be(2);
            p[0].Name.Should().Be("objA");
            p[0].ParameterType.Should().Be(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object")));
            p[1].Name.Should().Be("objB");
            p[1].ParameterType.Should().Be(c.GetOrCreateTypeSymbol(coreAssembly.GetType("System.Object")));
        }

        [AttributeUsage(AttributeTargets.Class)]
        class AttributeWithType : Attribute
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
            var c = new IkvmReflectionSymbolContext(universe);
            var s = c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+ClassWithAttributeWithType"));
            var a = s.GetCustomAttribute(c.GetOrCreateTypeSymbol(thisAssembly.GetType("IKVM.CoreLib.Tests.Symbols.IkvmReflection.IkvmReflectionSymbolTests+AttributeWithType")));
            var v = a.Value.ConstructorArguments[0].Value;
            v.Should().BeOfType<IkvmReflectionTypeSymbol>();
        }

        [TestMethod]
        public void CanResolveAssemblyBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var assemblySymbol = c.GetOrCreateAssemblySymbol(a);
            assemblySymbol.Should().BeOfType<IkvmReflectionAssemblySymbolBuilder>();
            assemblySymbol.Should().BeSameAs(c.GetOrCreateAssemblySymbol(a));
        }

        [TestMethod]
        public void CanResolveModuleBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var moduleSymbol = c.GetOrCreateModuleSymbol(m);
            moduleSymbol.Should().BeOfType<IkvmReflectionModuleSymbolBuilder>();
            moduleSymbol.Should().BeSameAs(c.GetOrCreateModuleSymbol(m));
        }

        [TestMethod]
        public void CanResolveTypeBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");

            var type1 = m.DefineType("DynamicType1");
            var type1Symbol = c.GetOrCreateTypeSymbol(type1);
            type1Symbol.Should().BeOfType<IkvmReflectionTypeSymbolBuilder>();
            type1Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type1));
        }

        [TestMethod]
        public void CanResolveMultipleTypeBuilders()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");

            var type1 = m.DefineType("DynamicType1");
            var type1Symbol = c.GetOrCreateTypeSymbol(type1);
            type1Symbol.Should().BeOfType<IkvmReflectionTypeSymbolBuilder>();
            type1Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type1));
            type1.CreateType();

            var type2 = m.DefineType("DynamicType2");
            var type2Symbol = c.GetOrCreateTypeSymbol(type2);
            type2Symbol.Should().BeOfType<IkvmReflectionTypeSymbolBuilder>();
            type2Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type2));
        }

        [TestMethod]
        public void CanResolveMethodBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var method1 = t.DefineMethod("DynamicMethod1", MethodAttributes.Public | MethodAttributes.Static);
            var method1Symbol = c.GetOrCreateMethodSymbol(method1);
            method1Symbol.Should().BeOfType<IkvmReflectionMethodSymbolBuilder>();
            method1Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method1));
        }

        [TestMethod]
        public void CanResolveMultipleMethodBuilders()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var method1 = t.DefineMethod("DynamicMethod1", MethodAttributes.Public | MethodAttributes.Static);
            var method1Symbol = c.GetOrCreateMethodSymbol(method1);
            method1Symbol.Should().BeOfType<IkvmReflectionMethodSymbolBuilder>();
            method1Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method1));

            var method2 = t.DefineMethod("DynamicMethod2", MethodAttributes.Public | MethodAttributes.Static);
            var method2Symbol = c.GetOrCreateMethodSymbol(method2);
            method2Symbol.Should().BeOfType<IkvmReflectionMethodSymbolBuilder>();
            method2Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method2));
        }

        [TestMethod]
        public void CanResolveFieldBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var field = t.DefineField("dynamicField", coreAssembly.GetType("System.Object"), FieldAttributes.Public);
            var fieldSymbol = c.GetOrCreateFieldSymbol(field);
            fieldSymbol.Should().BeOfType<IkvmReflectionFieldSymbolBuilder>();
            fieldSymbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field));
        }

        [TestMethod]
        public void CanResolveMultipleFieldBuilders()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var field1 = t.DefineField("dynamicField1", coreAssembly.GetType("System.Object"), FieldAttributes.Public);
            var field1Symbol = c.GetOrCreateFieldSymbol(field1);
            field1Symbol.Should().BeOfType<IkvmReflectionFieldSymbolBuilder>();
            field1Symbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field1));

            var field2 = t.DefineField("dynamicField2", coreAssembly.GetType("System.Object"), FieldAttributes.Public);
            var field2Symbol = c.GetOrCreateFieldSymbol(field2);
            field2Symbol.Should().BeOfType<IkvmReflectionFieldSymbolBuilder>();
            field2Symbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field2));
        }

        [TestMethod]
        public void CanResolvePropertyBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var property = t.DefineProperty("DynamicProperty", PropertyAttributes.None, coreAssembly.GetType("System.Object"), []);
            var propertySymbol = c.GetOrCreatePropertySymbol(property);
            propertySymbol.Should().BeOfType<IkvmReflectionPropertySymbolBuilder>();
            propertySymbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property));
        }

        [TestMethod]
        public void CanResolveMultiplePropertyBuilders()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var property1 = t.DefineProperty("DynamicProperty1", PropertyAttributes.None, coreAssembly.GetType("System.Object"), []);
            var property1Symbol = c.GetOrCreatePropertySymbol(property1);
            property1Symbol.Should().BeOfType<IkvmReflectionPropertySymbolBuilder>();
            property1Symbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property1));

            var property2 = t.DefineProperty("DynamicProperty2", PropertyAttributes.None, coreAssembly.GetType("System.Object"), []);
            var property2Symbol = c.GetOrCreatePropertySymbol(property2);
            property2Symbol.Should().BeOfType<IkvmReflectionPropertySymbolBuilder>();
            property2Symbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property2));
        }

        [TestMethod]
        public void CanResolveEventBuilder()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var event1 = t.DefineEvent("DynamicEvent", EventAttributes.None, coreAssembly.GetType("System.EventHandler"));
            var event1Symbol = c.GetOrCreateEventSymbol(event1);
            event1Symbol.Should().BeOfType<IkvmReflectionEventSymbolBuilder>();
            event1Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event1));
        }

        [TestMethod]
        public void CanResolveMultipleEventBuilders()
        {
            var c = new IkvmReflectionSymbolContext(universe);
            var a = universe.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.Save);
            var m = a.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            var t = m.DefineType("DynamicType");

            var event1 = t.DefineEvent("DynamicEvent", EventAttributes.None, coreAssembly.GetType("System.EventHandler"));
            var event1Symbol = c.GetOrCreateEventSymbol(event1);
            event1Symbol.Should().BeOfType<IkvmReflectionEventSymbolBuilder>();
            event1Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event1));

            var event2 = t.DefineEvent("DynamicEvent", EventAttributes.None, coreAssembly.GetType("System.EventHandler"));
            var event2Symbol = c.GetOrCreateEventSymbol(event2);
            event2Symbol.Should().BeOfType<IkvmReflectionEventSymbolBuilder>();
            event2Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event2));
        }

    }

}
