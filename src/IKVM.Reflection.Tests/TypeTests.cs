using System.Collections.Generic;

using FluentAssertions;

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

    }

}
