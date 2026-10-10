using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Tests for <see cref="Type"/>.
    /// </summary>
    public class TypeTests
    {

        /// <summary>
        /// Checks that a generic type definition imported from the framework reports itself as one, and that
        /// <see cref="Type.MakeGenericType(Type[])"/> produces a constructed generic type.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanMakeGenericType(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var t = u.Import(typeof(IEnumerable<>));
            t.IsGenericType.Should().BeTrue();
            t.IsGenericTypeDefinition.Should().BeTrue();
            t.IsConstructedGenericType.Should().BeFalse();

            var g = t.MakeGenericType(u.Import(typeof(object)));
            g.IsGenericTypeDefinition.Should().BeFalse();
            g.IsConstructedGenericType.Should().BeTrue();
        }

        /// <summary>
        /// Checks that a property of a constructed generic type, and its getter, have the type argument substituted for
        /// the generic parameter.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies are loaded.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanGetGenericPropertyOfGenericType(string tfm)
        {
            using var u = TestUniverse.Create(tfm);

            var nullableType = u.Import(typeof(System.Nullable<>));
            nullableType.IsGenericType.Should().BeTrue();
            nullableType.IsGenericTypeDefinition.Should().BeTrue();
            nullableType.IsConstructedGenericType.Should().BeFalse();
            nullableType.GetGenericArguments().Should().ContainSingle().Which.Should().NotBeNull();

            var nullableOfObjectType = nullableType.MakeGenericType(u.Import(typeof(object)));
            nullableOfObjectType.IsGenericTypeDefinition.Should().BeFalse();
            nullableOfObjectType.IsConstructedGenericType.Should().BeTrue();

            var valueProperty = nullableOfObjectType.GetProperty("Value");
            valueProperty.PropertyType.Should().Be(u.Import(typeof(object)));
            var valueGetter = valueProperty.GetGetMethod();
            valueGetter.ReturnType.Should().Be(u.Import(typeof(object)));
            valueGetter.GetParameters().Should().BeEmpty();
        }

    

        /// <summary>
        /// A constructor lookup that finds no exact match falls back to the binder over the type's own constructors
        /// only, and never returns a constructor of a base class, as the runtime does.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void GetConstructorDoesNotReturnBaseClassConstructors(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            System.Type[] runtimeTypes = [typeof(string), typeof(string), typeof(Exception)];

            // ArgumentException has this constructor and ArgumentNullException does not
            typeof(ArgumentNullException).GetConstructor(runtimeTypes).Should().BeNull();
            u.Import(typeof(ArgumentException)).GetConstructor(runtimeTypes.Select(u.Import).ToArray()).Should().NotBeNull();
            u.Import(typeof(ArgumentNullException)).GetConstructor(runtimeTypes.Select(u.Import).ToArray()).Should().BeNull();
        }

        /// <summary>
        /// A derived indexer hides an inherited one only when their parameter types match as well as their name and
        /// type, so an indexer with different parameters is inherited alongside it.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void PropertiesAreHiddenByNameAndSignature(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var int32 = u.Import(typeof(int));
            var str = u.Import(typeof(string));

            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);

            // defines a public indexer of type int with the given parameter type
            void DefineIndexer(TypeBuilder type, Type parameterType)
            {
                var getter = type.DefineMethod("get_Item", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName, int32, [parameterType]);
                var il = getter.GetILGenerator();
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ret);
                type.DefineProperty("Item", PropertyAttributes.None, int32, [parameterType]).SetGetMethod(getter);
            }

            var baseType = module.DefineType("Base", TypeAttributes.Public);
            baseType.DefineDefaultConstructor(MethodAttributes.Public);
            DefineIndexer(baseType, int32);
            DefineIndexer(baseType, u.Import(typeof(long)));
            baseType.CreateType();

            var derivedType = module.DefineType("Derived", TypeAttributes.Public, baseType);
            derivedType.DefineDefaultConstructor(MethodAttributes.Public);
            DefineIndexer(derivedType, int32);
            DefineIndexer(derivedType, str);

            derivedType.CreateType();
            assembly.Save("Test.dll");

            // the derived int indexer hides the base one; the base long indexer is inherited
            var expected = u.VerifyAndLoad("Test.dll").GetType("Derived")!.GetProperties().Select(i => i.GetIndexParameters().Single().ParameterType.Name).OrderBy(i => i).ToArray();
            expected.Should().Equal("Int32", "Int64", "String");

            using var reader = TestUniverse.Create(tfm);
            var derived = reader.Universe.LoadFile(Path.Combine(u.TempPath, "Test.dll")).GetType("Derived", true);
            derived.GetProperties().Select(i => i.GetIndexParameters().Single().ParameterType.Name).OrderBy(i => i).Should().Equal(expected);
        }

    }

}
