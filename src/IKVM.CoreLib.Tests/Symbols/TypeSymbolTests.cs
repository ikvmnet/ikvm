using System.Collections.Generic;
using System.Reflection;

using FluentAssertions;

using IKVM.CoreLib.Symbols;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols
{

    /// <summary>
    /// Base class for some tests that interact with <see cref="TypeSymbol"/>.
    /// </summary>
    public abstract class TypeSymbolTests<TInit, TSymbols>
        where TInit : SymbolTestInit<TSymbols>, new()
        where TSymbols : SymbolContext
    {

        protected TInit Init { get; } = new TInit();

        [TestMethod]
        public void SystemObjectPropertiesShouldMatch()
        {
            var s = Init.Symbols.ResolveCoreType("System.Object");
            s.AssemblyQualifiedName.Should().Be(typeof(object).AssemblyQualifiedName);
            s.Name.Should().Be(typeof(object).Name);
            s.Namespace.Should().Be(typeof(object).Namespace);
            s.FullName.Should().Be(typeof(object).FullName);
            s.BaseType.Should().BeNull();
            s.HasElementType.Should().BeFalse();
            s.IsAbstract.Should().BeFalse();
            s.IsArray.Should().BeFalse();
            s.IsAutoLayout.Should().BeTrue();
            s.IsByRef.Should().BeFalse();
            s.IsClass.Should().BeTrue();
            s.IsConstructedGenericType.Should().BeFalse();
            s.IsEnum.Should().BeFalse();
            s.IsExplicitLayout.Should().BeFalse();
            s.IsFunctionPointer.Should().BeFalse();
            s.IsGenericMethodParameter.Should().BeFalse();
            s.IsGenericParameter.Should().BeFalse();
            s.IsGenericType.Should().BeFalse();
            s.IsGenericTypeDefinition.Should().BeFalse();
            s.IsGenericTypeParameter.Should().BeFalse();
            s.IsInterface.Should().BeFalse();
            s.IsLayoutSequential.Should().BeFalse();
            s.IsMissing.Should().BeFalse();
            s.IsNested.Should().BeFalse();
            s.IsNestedAssembly.Should().BeFalse();
            s.IsNestedFamANDAssem.Should().BeFalse();
            s.IsNestedFamily.Should().BeFalse();
            s.IsNestedFamORAssem.Should().BeFalse();
            s.IsNestedPrivate.Should().BeFalse();
            s.IsNestedPublic.Should().BeFalse();
            s.IsNotPublic.Should().BeFalse();
            s.IsPointer.Should().BeFalse();
            s.IsPrimitive.Should().BeFalse();
            s.IsPublic.Should().BeTrue();
            s.IsSealed.Should().BeFalse();
            s.IsSerializable.Should().BeTrue();
            s.IsSZArray.Should().BeFalse();
            s.IsTypeDefinition.Should().BeTrue();
            s.IsUnmanagedFunctionPointer.Should().BeFalse();
            s.IsValueType.Should().BeFalse();
            s.IsVisible.Should().BeTrue();
            s.ToString().Should().Be(typeof(object).ToString());
            s.GetCustomAttributes(true);
        }

        [TestMethod]
        public void ShouldInt32PropertiesShouldMatch()
        {
            var s = Init.Symbols.ResolveCoreType("System.Int32");
            s.AssemblyQualifiedName.Should().Be(typeof(int).AssemblyQualifiedName);
            s.Name.Should().Be(typeof(int).Name);
            s.Namespace.Should().Be(typeof(int).Namespace);
            s.FullName.Should().Be(typeof(int).FullName);
            s.BaseType.Should().Be(Init.Symbols.ResolveCoreType("System.ValueType"));
            s.HasElementType.Should().BeFalse();
            s.IsAbstract.Should().BeFalse();
            s.IsArray.Should().BeFalse();
            s.IsAutoLayout.Should().BeFalse();
            s.IsByRef.Should().BeFalse();
            s.IsClass.Should().BeFalse();
            s.IsConstructedGenericType.Should().BeFalse();
            s.IsEnum.Should().BeFalse();
            s.IsExplicitLayout.Should().BeFalse();
            s.IsFunctionPointer.Should().BeFalse();
            s.IsGenericMethodParameter.Should().BeFalse();
            s.IsGenericParameter.Should().BeFalse();
            s.IsGenericType.Should().BeFalse();
            s.IsGenericTypeDefinition.Should().BeFalse();
            s.IsGenericTypeParameter.Should().BeFalse();
            s.IsInterface.Should().BeFalse();
            s.IsLayoutSequential.Should().BeTrue();
            s.IsMissing.Should().BeFalse();
            s.IsNested.Should().BeFalse();
            s.IsNestedAssembly.Should().BeFalse();
            s.IsNestedFamANDAssem.Should().BeFalse();
            s.IsNestedFamily.Should().BeFalse();
            s.IsNestedFamORAssem.Should().BeFalse();
            s.IsNestedPrivate.Should().BeFalse();
            s.IsNestedPublic.Should().BeFalse();
            s.IsNotPublic.Should().BeFalse();
            s.IsPointer.Should().BeFalse();
            s.IsPrimitive.Should().BeTrue();
            s.IsPublic.Should().BeTrue();
            s.IsSealed.Should().BeTrue();
            s.IsSerializable.Should().BeTrue();
            s.IsSZArray.Should().BeFalse();
            s.IsTypeDefinition.Should().BeTrue();
            s.IsUnmanagedFunctionPointer.Should().BeFalse();
            s.IsValueType.Should().BeTrue();
            s.IsVisible.Should().BeTrue();
            s.ToString().Should().Be(typeof(int).ToString());
            s.GetCustomAttributes(true);
        }

        [TestMethod]
        public void SystemInt32ShouldReturnSameInstance()
        {
            var s1 = Init.Symbols.ResolveCoreType("System.Int32");
            var s2 = Init.Symbols.ResolveCoreType("System.Int32");
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void GetFieldShouldNotReturnInternalField()
        {
            var s = Init.Symbols.ResolveCoreType("System.Nullable`1");
            var f = s.GetField("value");
            f.Should().BeNull();
        }

        [TestMethod]
        public void CanMakeArrayType()
        {
            var s = Init.Symbols.ResolveCoreType("System.Int32");
            var p = s.MakeArrayType();
            p.AssemblyQualifiedName.Should().Be(typeof(int[]).AssemblyQualifiedName);
            p.Name.Should().Be(typeof(int[]).Name);
            p.Namespace.Should().Be(typeof(int[]).Namespace);
            p.FullName.Should().Be(typeof(int[]).FullName);
            p.BaseType.Should().Be(Init.Symbols.ResolveCoreType("System.Array"));
            p.HasElementType.Should().BeTrue();
            p.IsAbstract.Should().BeFalse();
            p.IsArray.Should().BeTrue();
            p.IsAutoLayout.Should().BeTrue();
            p.IsByRef.Should().BeFalse();
            p.IsClass.Should().BeTrue();
            p.IsConstructedGenericType.Should().BeFalse();
            p.IsEnum.Should().BeFalse();
            p.IsExplicitLayout.Should().BeFalse();
            p.IsFunctionPointer.Should().BeFalse();
            p.IsGenericMethodParameter.Should().BeFalse();
            p.IsGenericParameter.Should().BeFalse();
            p.IsGenericType.Should().BeFalse();
            p.IsGenericTypeDefinition.Should().BeFalse();
            p.IsGenericTypeParameter.Should().BeFalse();
            p.IsInterface.Should().BeFalse();
            p.IsLayoutSequential.Should().BeFalse();
            p.IsMissing.Should().BeFalse();
            p.IsNested.Should().BeFalse();
            p.IsNestedAssembly.Should().BeFalse();
            p.IsNestedFamANDAssem.Should().BeFalse();
            p.IsNestedFamily.Should().BeFalse();
            p.IsNestedFamORAssem.Should().BeFalse();
            p.IsNestedPrivate.Should().BeFalse();
            p.IsNestedPublic.Should().BeFalse();
            p.IsNotPublic.Should().BeFalse();
            p.IsPointer.Should().BeFalse();
            p.IsPrimitive.Should().BeFalse();
            p.IsPublic.Should().BeTrue();
            p.IsSealed.Should().BeTrue();
            p.IsSerializable.Should().BeTrue();
            p.IsSZArray.Should().BeTrue();
            p.IsTypeDefinition.Should().BeFalse();
            p.IsUnmanagedFunctionPointer.Should().BeFalse();
            p.IsValueType.Should().BeFalse();
            p.IsVisible.Should().BeTrue();
            p.ToString().Should().Be(typeof(int[]).ToString());
            p.GetCustomAttributes(true);
        }

        [TestMethod]
        public void SystemInt32ArrayShouldReturnSameInstance()
        {
            var s1 = Init.Symbols.ResolveCoreType("System.Int32").MakeArrayType();
            var s2 = Init.Symbols.ResolveCoreType("System.Int32").MakeArrayType();
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void CanMakePointerType()
        {
            var s = Init.Symbols.ResolveCoreType("System.Int32");
            var p = s.MakePointerType();
            p.AssemblyQualifiedName.Should().Be(typeof(int*).AssemblyQualifiedName);
            p.Name.Should().Be(typeof(int*).Name);
            p.Namespace.Should().Be(typeof(int*).Namespace);
            p.FullName.Should().Be(typeof(int*).FullName);
            p.BaseType.Should().BeNull();
            p.HasElementType.Should().BeTrue();
            p.IsAbstract.Should().BeFalse();
            p.IsArray.Should().BeFalse();
            p.IsAutoLayout.Should().BeTrue();
            p.IsByRef.Should().BeFalse();
            p.IsClass.Should().BeTrue();
            p.IsConstructedGenericType.Should().BeFalse();
            p.IsEnum.Should().BeFalse();
            p.IsExplicitLayout.Should().BeFalse();
            p.IsFunctionPointer.Should().BeFalse();
            p.IsGenericMethodParameter.Should().BeFalse();
            p.IsGenericParameter.Should().BeFalse();
            p.IsGenericType.Should().BeFalse();
            p.IsGenericTypeDefinition.Should().BeFalse();
            p.IsGenericTypeParameter.Should().BeFalse();
            p.IsInterface.Should().BeFalse();
            p.IsLayoutSequential.Should().BeFalse();
            p.IsMissing.Should().BeFalse();
            p.IsNested.Should().BeFalse();
            p.IsNestedAssembly.Should().BeFalse();
            p.IsNestedFamANDAssem.Should().BeFalse();
            p.IsNestedFamily.Should().BeFalse();
            p.IsNestedFamORAssem.Should().BeFalse();
            p.IsNestedPrivate.Should().BeFalse();
            p.IsNestedPublic.Should().BeFalse();
            p.IsNotPublic.Should().BeFalse();
            p.IsPointer.Should().BeTrue();
            p.IsPrimitive.Should().BeFalse();
            p.IsPublic.Should().BeTrue();
            p.IsSealed.Should().BeFalse();
            p.IsSerializable.Should().BeFalse();
            p.IsSZArray.Should().BeFalse();
            p.IsTypeDefinition.Should().BeFalse();
            p.IsUnmanagedFunctionPointer.Should().BeFalse();
            p.IsValueType.Should().BeFalse();
            p.IsVisible.Should().BeTrue();
            p.ToString().Should().Be(typeof(int*).ToString());
            p.GetCustomAttributes(true);
        }

        [TestMethod]
        public void SystemInt32PointerShouldReturnSameInstance()
        {
            var s1 = Init.Symbols.ResolveCoreType("System.Int32").MakePointerType();
            var s2 = Init.Symbols.ResolveCoreType("System.Int32").MakePointerType();
            s1.Should().BeSameAs(s2);
        }

        [TestMethod]
        public void CanMakeGenericType()
        {
            var s = Init.Symbols.ResolveCoreType("System.Collections.Generic.List`1");
            var t = s.MakeGenericType([Init.Symbols.ResolveCoreType("System.Int32")]);
            t.AssemblyQualifiedName.Should().Be(typeof(List<int>).AssemblyQualifiedName);
            t.Name.Should().Be(typeof(List<int>).Name);
            t.Namespace.Should().Be(typeof(List<int>).Namespace);
            t.FullName.Should().Be(typeof(List<int>).FullName);
            t.BaseType.Should().Be(Init.Symbols.ResolveCoreType("System.Object"));
            t.HasElementType.Should().BeFalse();
            t.IsAbstract.Should().BeFalse();
            t.IsArray.Should().BeFalse();
            t.IsAutoLayout.Should().BeTrue();
            t.IsByRef.Should().BeFalse();
            t.IsClass.Should().BeTrue();
            t.IsConstructedGenericType.Should().BeTrue();
            t.IsEnum.Should().BeFalse();
            t.IsExplicitLayout.Should().BeFalse();
            t.IsFunctionPointer.Should().BeFalse();
            t.IsGenericMethodParameter.Should().BeFalse();
            t.IsGenericParameter.Should().BeFalse();
            t.IsGenericType.Should().BeTrue();
            t.IsGenericTypeDefinition.Should().BeFalse();
            t.IsGenericTypeParameter.Should().BeFalse();
            t.IsInterface.Should().BeFalse();
            t.IsLayoutSequential.Should().BeFalse();
            t.IsMissing.Should().BeFalse();
            t.IsNested.Should().BeFalse();
            t.IsNestedAssembly.Should().BeFalse();
            t.IsNestedFamANDAssem.Should().BeFalse();
            t.IsNestedFamily.Should().BeFalse();
            t.IsNestedFamORAssem.Should().BeFalse();
            t.IsNestedPrivate.Should().BeFalse();
            t.IsNestedPublic.Should().BeFalse();
            t.IsNotPublic.Should().BeFalse();
            t.IsPointer.Should().BeFalse();
            t.IsPrimitive.Should().BeFalse();
            t.IsPublic.Should().BeTrue();
            t.IsSealed.Should().BeFalse();
            t.IsSerializable.Should().BeTrue();
            t.IsSZArray.Should().BeFalse();
            t.IsTypeDefinition.Should().BeFalse();
            t.IsUnmanagedFunctionPointer.Should().BeFalse();
            t.IsValueType.Should().BeFalse();
            t.IsVisible.Should().BeTrue();
            t.ToString().Should().Be(typeof(List<int>).ToString());
            t.GetCustomAttributes(true);
        }

        [TestMethod]
        public void CanGetFieldFromConstructedType()
        {
            var s = Init.Symbols.ResolveCoreType("System.Nullable`1");
            var t = s.MakeGenericType([Init.Symbols.ResolveCoreType("System.Int32")]);
            var f = t.GetField("value", BindingFlags.NonPublic | BindingFlags.Instance);
            f.Should().BeOfType(typeof(ConstructedGenericFieldSymbol));
            f.DeclaringType.Should().BeSameAs(t);
            f.Name.Should().Be("value");
            f.IsAssembly.Should().BeTrue();
            f.IsFamily.Should().BeFalse();
            f.IsFamilyAndAssembly.Should().BeFalse();
            f.IsFamilyOrAssembly.Should().BeFalse();
            f.IsInitOnly.Should().BeFalse();
            f.IsLiteral.Should().BeFalse();
            f.IsMissing.Should().BeFalse();
            f.IsNotSerialized.Should().BeFalse();
            f.IsPrivate.Should().BeFalse();
            f.IsPublic.Should().BeFalse();
            f.IsSpecialName.Should().BeFalse();
            f.IsStatic.Should().BeFalse();
            f.GetCustomAttributes(true);

            var ft = f.FieldType;
            ft.Should().BeSameAs(Init.Symbols.ResolveCoreType("System.Int32"));
        }

        [TestMethod]
        public void CanGetMethodFromConstructedType()
        {
            var s = Init.Symbols.ResolveCoreType("System.Collections.Generic.List`1");
            var t = s.MakeGenericType([Init.Symbols.ResolveCoreType("System.Int32")]);
            var m = t.GetMethod("Add");
            m.Should().BeOfType(typeof(ConstructedGenericMethodSymbol));
            m.DeclaringType.Should().BeSameAs(t);
            m.Name.Should().Be("Add");
            m.ContainsGenericParameters.Should().BeFalse();
            m.IsAbstract.Should().BeFalse();
            m.IsAssembly.Should().BeFalse();
            m.IsConstructor.Should().BeFalse();
            m.IsFamily.Should().BeFalse();
            m.IsFamilyAndAssembly.Should().BeFalse();
            m.IsFamilyOrAssembly.Should().BeFalse();
            m.IsFinal.Should().BeTrue();
            m.IsGenericMethod.Should().BeFalse();
            m.IsGenericMethodDefinition.Should().BeFalse();
            m.IsHideBySig.Should().BeTrue();
            m.IsMissing.Should().BeFalse();
            m.IsPrivate.Should().BeFalse();
            m.IsPublic.Should().BeTrue();
            m.IsSpecialName.Should().BeFalse();
            m.IsStatic.Should().BeFalse();
            m.IsVirtual.Should().BeTrue();
            m.MethodImplementationFlags.Should().Be(typeof(List<int>).GetMethod("Add")!.MethodImplementationFlags);
            m.MemberType.Should().Be(MemberTypes.Method);
            m.GetCustomAttributes(true);

            var pl = m.Parameters;
            pl.Should().HaveCount(1);
            var p0 = pl[0];
            p0.Name.Should().Be("item");
            p0.ParameterType.Should().BeSameAs(Init.Symbols.ResolveCoreType("System.Int32"));
        }

        [TestMethod]
        public void CanGetConstructor()
        {
            var s = Init.Symbols.ResolveCoreType("System.Object");
            var c = s.GetConstructor([]);
            c.Should().NotBeNull();
            c.Name.Should().Be(ConstructorInfo.ConstructorName);
            c.IsConstructor.Should().BeTrue();
            c.Attributes.Should().HaveFlag(MethodAttributes.SpecialName);
            c.Attributes.Should().HaveFlag(MethodAttributes.RTSpecialName);
        }

        [TestMethod]
        public void CanGetStaticConstructor()
        {
            var s = Init.Symbols.ResolveCoreType("System.Reflection.Module");
            var c = s.TypeInitializer;
            c.Should().NotBeNull();
            c.Name.Should().Be(ConstructorInfo.TypeConstructorName);
            c.IsConstructor.Should().BeTrue();
            c.Attributes.Should().HaveFlag(MethodAttributes.SpecialName);
            c.Attributes.Should().HaveFlag(MethodAttributes.RTSpecialName);
        }

        [TestMethod]
        public void CanGetGenericMethodFromGenericType()
        {
            var typeOfFunc2 = Init.Symbols.ResolveCoreType("System.Func`2");
            var typeOfTask = Init.Symbols.ResolveCoreType("System.Threading.Tasks.Task");
            var typeOfTask1 = Init.Symbols.ResolveCoreType("System.Threading.Tasks.Task`1");

            var m = typeOfTask1.GetMethod("ContinueWith", 1, [
                TypeSymbolSelector.Predicate(t =>
                    t.GenericTypeDefinition == typeOfFunc2 &&
                    t.GenericArguments is [var arg1, { IsGenericMethodParameter: true, GenericParameterPosition: 0 }] && arg1 == typeOfTask),
                Init.Symbols.ResolveCoreType("System.Threading.CancellationToken")],
                default);

            var m2 = m.MakeGenericMethod([Init.Symbols.ResolveCoreType("System.Int32")]);

            var p = m2.ParameterTypes;
        }

    }

}
