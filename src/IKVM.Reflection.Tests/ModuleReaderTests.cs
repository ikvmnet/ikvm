using System;
using System.Collections.Generic;

using FluentAssertions;

using IKVM.Tests.Util;

using Xunit;

namespace IKVM.Reflection.Tests
{

    public class ModuleReaderTests
    {

        /// <summary>
        /// Initializes the variables requires to execute tests.
        /// </summary
        /// <param name="framework"></param>
        /// <param name="universe"></param>
        /// <param name="resolver"></param>
        bool Init(FrameworkSpec framework, out Universe universe, out TestAssemblyResolver resolver)
        {
            universe = null;
            resolver = null;

            // initialize primary classes
            universe = new Universe(DotNetSdkUtil.GetCoreLibName(framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion));
            resolver = new TestAssemblyResolver(universe, framework.Tfm, framework.TargetFrameworkIdentifier, framework.TargetFrameworkVersion);

            return true;
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanMakeGenericType(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver) == false)
                return;

            var t = universe.Import(typeof(IEnumerable<>));
            t.IsGenericType.Should().BeTrue();
            t.IsGenericTypeDefinition.Should().BeTrue();
            t.IsConstructedGenericType.Should().BeFalse();
            var g = t.MakeGenericType(universe.Import(typeof(object)));
            g.IsGenericTypeDefinition.Should().BeFalse();
            g.IsConstructedGenericType.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void CanGetGenericPropertyOfGenericType(FrameworkSpec framework)
        {
            if (Init(framework, out var universe, out var resolver) == false)
                return;

            // simple generic with a single property
            var nullableType = universe.Import(typeof(Nullable<>));
            nullableType.IsGenericType.Should().BeTrue();
            nullableType.IsGenericTypeDefinition.Should().BeTrue();
            nullableType.IsConstructedGenericType.Should().BeFalse();
            nullableType.GetGenericArguments().Should().HaveCount(1);
            nullableType.GetGenericArguments()[0].Should().NotBeNull();

            // make generic instance
            var nullableOfObjectType = nullableType.MakeGenericType(universe.Import(typeof(object)));
            nullableOfObjectType.IsGenericTypeDefinition.Should().BeFalse();
            nullableOfObjectType.IsConstructedGenericType.Should().BeTrue();

            // check for expected property
            var valueProperty = nullableOfObjectType.GetProperty("Value");
            valueProperty.PropertyType.Should().Be(universe.Import(typeof(object)));
            var valueGetter = valueProperty.GetGetMethod();
            valueGetter.ReturnType.Should().Be(universe.Import(typeof(object)));
            valueGetter.GetParameters().Should().BeEmpty();
        }

    }

}
