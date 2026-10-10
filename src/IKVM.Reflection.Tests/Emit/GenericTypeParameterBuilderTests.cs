using System.Linq;

using FluentAssertions;

using IKVM.Reflection.Emit;

using Xunit;

namespace IKVM.Reflection.Tests.Emit
{

    /// <summary>
    /// Tests for <see cref="GenericTypeParameterBuilder"/>.
    /// </summary>
    public class GenericTypeParameterBuilderTests
    {

        /// <summary>
        /// A null base type constraint means the parameter has none, as in System.Reflection.Emit, so the saved
        /// parameter has no constraints.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void NullBaseTypeConstraintAddsNoConstraint(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Generic", TypeAttributes.Public);
            type.DefineGenericParameters("T")[0].SetBaseTypeConstraint(null);
            type.CreateType();
            assembly.Save("Test.dll");

            u.VerifyAndLoad("Test.dll").GetType("Generic")!.GetGenericArguments().Single().GetGenericParameterConstraints().Should().BeEmpty();
        }

        /// <summary>
        /// The type parameters of a generic type and of a generic method report the namespace of the type that
        /// declares them.
        /// </summary>
        /// <param name="tfm">The target framework whose reference assemblies the universe loads.</param>
        [Theory]
        [MemberData(nameof(FrameworkSpec.GetFrameworkTestData), MemberType = typeof(FrameworkSpec))]
        public void NamespaceIsThatOfTheDeclaringType(string tfm)
        {
            using var u = TestUniverse.Create(tfm);
            var assembly = u.DefineAssembly();
            var module = assembly.DefineDynamicModule("Test", "Test.dll", false);
            var type = module.DefineType("Some.Namespace.Generic", TypeAttributes.Public);
            var typeParameter = type.DefineGenericParameters("T")[0];
            var method = type.DefineMethod("Method", MethodAttributes.Public | MethodAttributes.Static, u.Import(typeof(void)), Type.EmptyTypes);
            var methodParameter = method.DefineGenericParameters("TMethod")[0];

            typeParameter.Namespace.Should().Be("Some.Namespace");
            methodParameter.Namespace.Should().Be("Some.Namespace");
        }

    }

}
