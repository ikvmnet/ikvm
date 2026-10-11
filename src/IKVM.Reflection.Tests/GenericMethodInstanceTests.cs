using System.Linq;

using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests
{

    public class GenericMethodInstanceTests
    {

        /// <summary>
        /// The parameter types of a method on a constructed generic type are bound to the type's arguments, and agree
        /// with the types its parameters report.
        /// </summary>
        /// <param name="tfm">The target framework of the fixture and of the universe's reference assemblies.</param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void ParameterTypesOfMethodOnConstructedTypeAreBound(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var int32 = u.Import(typeof(int));
            var type = LoadGenericClass(u, tfm).MakeGenericType(int32);
            var method = type.GetMethod("Method");

            method.Should().BeOfType<GenericMethodInstance>();
            method.GetParameterTypes().Should().Equal(int32);
            method.GetParameterTypes().Should().Equal(method.GetParameters().Select(p => p.ParameterType));
        }

        /// <summary>
        /// The parameter types of a constructor of a constructed generic type are bound to the type's arguments. The
        /// custom attribute writer encodes constructor arguments by these types.
        /// </summary>
        /// <param name="tfm">The target framework of the fixture and of the universe's reference assemblies.</param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void ParameterTypesOfConstructorOnConstructedTypeAreBound(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var int32 = u.Import(typeof(int));
            var type = LoadGenericClass(u, tfm).MakeGenericType(int32);
            var constructor = type.GetConstructor([int32]);

            constructor.Should().NotBeNull();
            constructor.GetParameterTypes().Should().Equal(int32);
        }

        /// <summary>
        /// The parameter types of a generic method instantiated on a constructed generic type are bound to both the
        /// type's and the method's arguments.
        /// </summary>
        /// <param name="tfm">The target framework of the fixture and of the universe's reference assemblies.</param>
        [Theory]
        [MemberData(nameof(Fixture.GetTargetFrameworkTestData), MemberType = typeof(Fixture))]
        public void ParameterTypesOfInstantiatedGenericMethodAreBound(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var int32 = u.Import(typeof(int));
            var str = u.Import(typeof(string));
            var method = LoadGenericClass(u, tfm).MakeGenericType(int32).GetMethod("GenericMethod").MakeGenericMethod(str);

            method.GetParameterTypes().Should().Equal(int32, str);
            method.GetParameterTypes().Should().Equal(method.GetParameters().Select(p => p.ParameterType));
        }

        /// <summary>
        /// Loads the fixture built for <paramref name="tfm"/> and returns its <c>GenericClass&lt;T&gt;</c>.
        /// </summary>
        /// <param name="u">The universe to load the fixture into.</param>
        /// <param name="tfm">The target framework of the fixture.</param>
        /// <returns>The generic type definition.</returns>
        static Type LoadGenericClass(TestUniverse u, string tfm)
        {
            return u.Universe.LoadFile(Fixture.GetAssemblyPath(tfm)).GetType("IKVM.Reflection.Tests.Fixture.GenericClass`1", true);
        }

    }

}
