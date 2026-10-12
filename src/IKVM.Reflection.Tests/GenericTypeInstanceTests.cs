using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Tests for <see cref="GenericTypeInstance"/>.
    /// </summary>
    public class GenericTypeInstanceTests
    {

        /// <summary>
        /// Binding the type parameters of a generic instance keeps the custom modifiers of its type arguments.
        /// </summary>
        /// <param name="tfm">The target framework of the fixture and of the universe's reference assemblies.</param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void BindingKeepsTypeArgumentCustomModifiers(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var definition = u.Universe.LoadFile(Fixture.GetAssemblyPath(tfm)).GetType("IKVM.Reflection.Tests.Fixture.GenericClass`1", true);
            var parameter = definition.GetGenericArguments()[0];
            var mods = CustomModifiers.FromReqOpt([u.Import(typeof(System.Runtime.CompilerServices.IsVolatile))], null);

            // GenericClass<T modreq(IsVolatile)> bound against GenericClass<int> is GenericClass<int modreq(IsVolatile)>
            var open = definition.__MakeGenericType([parameter], [mods]);
            var bound = open.BindTypeParameters(definition.MakeGenericType(u.Import(typeof(int))));

            bound.GetGenericArguments().Should().Equal(u.Import(typeof(int)));
            bound.__GetGenericArgumentsCustomModifiers()[0].Equals(mods).Should().BeTrue();
        }

    }

}
