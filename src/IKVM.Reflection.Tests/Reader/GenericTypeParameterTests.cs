using FluentAssertions;

using Xunit;

namespace IKVM.Reflection.Tests.Reader
{

    /// <summary>
    /// Tests for <see cref="IKVM.Reflection.Reader.GenericTypeParameter"/>.
    /// </summary>
    public class GenericTypeParameterTests
    {

        /// <summary>
        /// The type parameters of a generic type and of a generic method report the namespace the runtime reports.
        /// </summary>
        [Fact]
        public void NamespaceMatchesRuntime()
        {
            var tfm = Fixture.HostTargetFramework;
            using var u = TestUniverse.Create(tfm);
            var runtime = System.Reflection.Assembly.LoadFrom(Fixture.GetAssemblyPath(tfm)).GetType("IKVM.Reflection.Tests.Fixture.GenericClass`1", true);
            var type = u.Universe.LoadFile(Fixture.GetAssemblyPath(tfm)).GetType("IKVM.Reflection.Tests.Fixture.GenericClass`1", true);

            type.GetGenericArguments()[0].Namespace.Should().Be(runtime.GetGenericArguments()[0].Namespace);
            type.GetMethod("GenericMethod").GetGenericArguments()[0].Namespace.Should().Be(runtime.GetMethod("GenericMethod").GetGenericArguments()[0].Namespace);
        }

    }

}
