using System.Collections.Generic;

using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests
{

    public class TypeTests
    {

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
