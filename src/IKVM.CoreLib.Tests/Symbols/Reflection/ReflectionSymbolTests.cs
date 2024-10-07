using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using FluentAssertions;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

    [TestClass]
    public class ReflectionSymbolTests
    {

        class Foo<T>
        {
            T? field;
        }

        [TestMethod]
        public void SameTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(object));
            var s2 = c.GetOrCreateTypeSymbol(typeof(object));
            s1.Should().BeOfType<ReflectionTypeSymbol>();
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeDefinitionShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(Foo<>));
            var s2 = c.GetOrCreateTypeSymbol(typeof(Foo<>));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GenericTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
            var s2 = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void ArrayTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(object[,]));
            var s2 = c.GetOrCreateTypeSymbol(typeof(object[,]));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void SZArrayTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(object[]));
            var s2 = c.GetOrCreateTypeSymbol(typeof(object[]));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void PointerTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(int*));
            var s2 = c.GetOrCreateTypeSymbol(typeof(int*));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public unsafe void ByRefTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(int).MakeByRefType());
            var s2 = c.GetOrCreateTypeSymbol(typeof(int).MakeByRefType());
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void EnumTypeShouldBeSame()
        {
            var c = new ReflectionSymbolContext();
            var s1 = c.GetOrCreateTypeSymbol(typeof(AttributeTargets));
            var s2 = c.GetOrCreateTypeSymbol(typeof(AttributeTargets));
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void CanResolveType()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(object));
            s.Name.Should().Be("Object");
            s.FullName.Should().Be("System.Object");
        }

        [TestMethod]
        public void CanResolveFieldOfGenericTypeDefinition()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(Foo<>));
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
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(Foo<int>));
            s.IsGenericType.Should().BeTrue();
            s.IsGenericTypeDefinition.Should().BeFalse();
            var f = s.GetField("field", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            f.Name.Should().Be("field");
            f.FieldType.IsGenericType.Should().BeFalse();
            f.FieldType.IsGenericParameter.Should().BeFalse();
            f.FieldType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(int)));
        }

        [TestMethod]
        public void CanResolveMethod()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(object));
            var m = s.GetMethod("ToString");
            m.Name.Should().Be("ToString");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(string)));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(string)));
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
        }

        [TestMethod]
        public void CanResolveGenericMethod()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(ValueTuple));
            var m = s.GetMethods().FirstOrDefault(i => i.Name == "Create" && i.GetGenericArguments().Length == 1);
            m.Name.Should().Be("Create");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(ValueTuple<>)).MakeGenericType(m.GetGenericArguments()[0]));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(ValueTuple<>)).MakeGenericType(m.GetGenericArguments()[0]));
            m.IsGenericMethod.Should().BeTrue();
            m.IsGenericMethodDefinition.Should().BeTrue();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
        }

        [TestMethod]
        public void CanResolveParameters()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(object));
            var m = s.GetMethod("ReferenceEquals");
            m.Name.Should().Be("ReferenceEquals");
            m.ReturnType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(bool)));
            m.ReturnParameter.ParameterType.Should().BeSameAs(c.GetOrCreateTypeSymbol(typeof(bool)));
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsPrivate.Should().BeFalse();
            var p = m.GetParameters();
            p.Length.Should().Be(2);
            p[0].Name.Should().Be("objA");
            p[0].ParameterType.Should().Be(c.GetOrCreateTypeSymbol(typeof(object)));
            p[1].Name.Should().Be("objB");
            p[1].ParameterType.Should().Be(c.GetOrCreateTypeSymbol(typeof(object)));
        }

        [AttributeUsage(AttributeTargets.Class)]
        class AttributeWithType : Attribute
        {

            public AttributeWithType(Type type)
            {
                Type = type;
            }

            public Type Type { get; }

        }

        [AttributeWithType(typeof(object))]
        class ClassWithAttributeWithType
        {



        }

        [TestMethod]
        public void CanReadCustomAttributes()
        {
            var c = new ReflectionSymbolContext();
            var s = c.GetOrCreateTypeSymbol(typeof(ClassWithAttributeWithType));
            var a = s.GetCustomAttribute(c.GetOrCreateTypeSymbol(typeof(AttributeWithType)));
            var v = a.Value.ConstructorArguments[0].Value;
            v.Should().BeOfType<ReflectionTypeSymbol>();
        }

        [TestMethod]
        public void CanResolveAssemblyBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var assemblySymbol = c.GetOrCreateAssemblySymbol(a);
            assemblySymbol.Should().BeOfType<ReflectionAssemblySymbolBuilder>();
            assemblySymbol.Should().BeSameAs(c.GetOrCreateAssemblySymbol(a));
        }

        [TestMethod]
        public void CanResolveModuleBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var module = a.DefineDynamicModule("Test");
            var moduleSymbol = c.GetOrCreateModuleSymbol(module);
            moduleSymbol.Should().BeOfType<ReflectionModuleSymbolBuilder>();
            moduleSymbol.Should().BeSameAs(c.GetOrCreateModuleSymbol(module));
        }

        [TestMethod]
        public void CanResolveTypeBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");

            var type1 = m.DefineType("DynamicType1");
            var type1Symbol = c.GetOrCreateTypeSymbol(type1);
            type1Symbol.Should().BeOfType<ReflectionTypeSymbolBuilder>();
            type1Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type1));
        }

        [TestMethod]
        public void CanResolveMultipleTypeBuilders()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");

            var type1 = m.DefineType("DynamicType1");
            var type1Symbol = c.GetOrCreateTypeSymbol(type1);
            type1Symbol.Should().BeOfType<ReflectionTypeSymbolBuilder>();
            type1Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type1));
            type1.CreateType();

            var type2 = m.DefineType("DynamicType2");
            var type2Symbol = c.GetOrCreateTypeSymbol(type2);
            type2Symbol.Should().BeOfType<ReflectionTypeSymbolBuilder>();
            type2Symbol.Should().BeSameAs(c.GetOrCreateTypeSymbol(type2));
        }

        [TestMethod]
        public void CanResolveMethodBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var method1 = t.DefineMethod("DynamicMethod1", MethodAttributes.Public | MethodAttributes.Static);
            var method1Symbol = c.GetOrCreateMethodSymbol(method1);
            method1Symbol.Should().BeOfType<ReflectionMethodSymbolBuilder>();
            method1Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method1));
        }

        [TestMethod]
        public void CanResolveMultipleMethodBuilders()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var method1 = t.DefineMethod("DynamicMethod1", MethodAttributes.Public | MethodAttributes.Static);
            var method1Symbol = c.GetOrCreateMethodSymbol(method1);
            method1Symbol.Should().BeOfType<ReflectionMethodSymbolBuilder>();
            method1Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method1));

            var method2 = t.DefineMethod("DynamicMethod2", MethodAttributes.Public | MethodAttributes.Static);
            var method2Symbol = c.GetOrCreateMethodSymbol(method2);
            method2Symbol.Should().BeOfType<ReflectionMethodSymbolBuilder>();
            method2Symbol.Should().BeSameAs(c.GetOrCreateMethodSymbol(method2));
        }

        [TestMethod]
        public void CanResolveFieldBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var field = t.DefineField("dynamicField", typeof(object), FieldAttributes.Public);
            var fieldSymbol = c.GetOrCreateFieldSymbol(field);
            fieldSymbol.Should().BeOfType<ReflectionFieldSymbolBuilder>();
            fieldSymbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field));
        }

        [TestMethod]
        public void CanResolveMultipleFieldBuilders()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var field1 = t.DefineField("dynamicField1", typeof(object), FieldAttributes.Public);
            var field1Symbol = c.GetOrCreateFieldSymbol(field1);
            field1Symbol.Should().BeOfType<ReflectionFieldSymbolBuilder>();
            field1Symbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field1));

            var field2 = t.DefineField("dynamicField2", typeof(object), FieldAttributes.Public);
            var field2Symbol = c.GetOrCreateFieldSymbol(field2);
            field2Symbol.Should().BeOfType<ReflectionFieldSymbolBuilder>();
            field2Symbol.Should().BeSameAs(c.GetOrCreateFieldSymbol(field2));
        }

        [TestMethod]
        public void CanResolvePropertyBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var property = t.DefineProperty("DynamicProperty", PropertyAttributes.None, typeof(object), []);
            var propertySymbol = c.GetOrCreatePropertySymbol(property);
            propertySymbol.Should().BeOfType<ReflectionPropertySymbolBuilder>();
            propertySymbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property));
        }

        [TestMethod]
        public void CanResolveMultiplePropertyBuilders()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var property1 = t.DefineProperty("DynamicProperty1", PropertyAttributes.None, typeof(object), []);
            var property1Symbol = c.GetOrCreatePropertySymbol(property1);
            property1Symbol.Should().BeOfType<ReflectionPropertySymbolBuilder>();
            property1Symbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property1));

            var property2 = t.DefineProperty("DynamicProperty2", PropertyAttributes.None, typeof(object), []);
            var property2Symbol = c.GetOrCreatePropertySymbol(property2);
            property2Symbol.Should().BeOfType<ReflectionPropertySymbolBuilder>();
            property2Symbol.Should().BeSameAs(c.GetOrCreatePropertySymbol(property2));
        }

        [TestMethod]
        public void CanResolveEventBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var event1 = t.DefineEvent("DynamicEvent", EventAttributes.None, typeof(EventHandler));
            var event1Symbol = c.GetOrCreateEventSymbol(event1);
            event1Symbol.Should().BeOfType<ReflectionEventSymbolBuilder>();
            event1Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event1));
        }

        [TestMethod]
        public void CanResolveMultipleEventBuilders()
        {
            var c = new ReflectionSymbolContext();
            var a = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect);
            var m = a.DefineDynamicModule("DynamicModule");
            var t = m.DefineType("DynamicType");

            var event1 = t.DefineEvent("DynamicEvent", EventAttributes.None, typeof(EventHandler));
            var event1Symbol = c.GetOrCreateEventSymbol(event1);
            event1Symbol.Should().BeOfType<ReflectionEventSymbolBuilder>();
            event1Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event1));

            var event2 = t.DefineEvent("DynamicEvent", EventAttributes.None, typeof(EventHandler));
            var event2Symbol = c.GetOrCreateEventSymbol(event2);
            event2Symbol.Should().BeOfType<ReflectionEventSymbolBuilder>();
            event2Symbol.Should().BeSameAs(c.GetOrCreateEventSymbol(event2));
        }

        [TestMethod]
        public void CanCompleteTypeBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = c.GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicAssembly"), AssemblyBuilderAccess.RunAndCollect));

            var moduleBuilder = a.DefineModule("DynamicModule");
            var moduleSymbol = (IModuleSymbol)moduleBuilder;

            var type1Builder = moduleBuilder.DefineType("DynamicType1");
            var type1Symbol = (ITypeSymbol)type1Builder;
            type1Symbol.Should().BeOfType<ReflectionTypeSymbolBuilder>();

            type1Builder.Complete();
            var type1Again = moduleBuilder.GetType("DynamicType1");
            type1Again.Should().BeSameAs(type1Symbol);
            type1Again.Name.Should().Be("DynamicType1");

            // check that we can relookup type
            moduleSymbol.GetType("DynamicType1").Should().BeSameAs(type1Symbol);
        }

        [TestMethod]
        public void CanGetMethodsFromTypeBuilder()
        {
            var c = new ReflectionSymbolContext();
            var a = c.DefineAssembly(new AssemblyIdentity("DynamicAssembly"), false, false);
            var m = a.DefineModule("DynamicModule");
            var type = m.DefineType("DynamicType");

            var method = type.DefineMethod("DynamicMethod1", System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.Static);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ret);

            // before complete
            type.GetMethods().Should().HaveCount(5);
            var incompleteMethod = type.GetMethod("DynamicMethod1");

            // complete the type
            type.Complete();

            // after complete
            type.GetMethods().Should().HaveCount(5);
            var completeMethod = type.GetMethod("DynamicMethod1");

            // all the methods should be the same
            incompleteMethod.Should().BeSameAs(completeMethod);
            incompleteMethod.Should().BeSameAs(method);
        }

    }

}
