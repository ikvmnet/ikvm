using System;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests that .NET members involving ByRef-like types (ref structs) are hidden from Java, since they cannot be
    /// represented (they cannot be boxed or used as array elements). See https://github.com/ikvmnet/ikvm/issues/343.
    /// </summary>
    [TestClass]
    public class ByRefLikeTypeTests
    {

        public class TypeWithByRefLikeMembers
        {

            public TypeWithByRefLikeMembers()
            {

            }

            public TypeWithByRefLikeMembers(in ReadOnlySpan<char> value)
            {

            }

            public void InReadOnlySpan(in ReadOnlySpan<char> value)
            {

            }

            public void RefSpan(ref Span<char> value)
            {

            }

            public void ReadOnlySpan(ReadOnlySpan<char> value)
            {

            }

            public ReadOnlySpan<char> ReturnsReadOnlySpan()
            {
                return default;
            }

            public void Supported(string value)
            {

            }

        }

        public delegate void ByRefLikeDelegate(ReadOnlySpan<char> value);

        static global::java.lang.Class GetClass(Type type) => global::ikvm.runtime.Util.getClassFromTypeHandle(type.TypeHandle);

        [TestMethod]
        public void MethodsWithByRefLikeTypesAreHidden()
        {
            var c = GetClass(typeof(TypeWithByRefLikeMembers));
            var names = c.getMethods().Select(i => i.getName()).ToList();
            names.Should().Contain(nameof(TypeWithByRefLikeMembers.Supported));
            names.Should().NotContain(nameof(TypeWithByRefLikeMembers.InReadOnlySpan));
            names.Should().NotContain(nameof(TypeWithByRefLikeMembers.RefSpan));
            names.Should().NotContain(nameof(TypeWithByRefLikeMembers.ReadOnlySpan));
            names.Should().NotContain(nameof(TypeWithByRefLikeMembers.ReturnsReadOnlySpan));
        }

        [TestMethod]
        public void DeclaredMethodsWithByRefLikeTypesAreHidden()
        {
            var c = GetClass(typeof(TypeWithByRefLikeMembers));
            var names = c.getDeclaredMethods().Select(i => i.getName()).ToList();
            names.Should().BeEquivalentTo(new[] { nameof(TypeWithByRefLikeMembers.Supported) });
        }

        [TestMethod]
        public void ConstructorsWithByRefLikeTypesAreHidden()
        {
            var c = GetClass(typeof(TypeWithByRefLikeMembers));
            var ctors = c.getConstructors();
            ctors.Should().HaveCount(1);
            ctors[0].getParameterTypes().Should().BeEmpty();
            c.getConstructor().Should().NotBeNull();
        }

        [TestMethod]
        public void DelegateWithByRefLikeParameterHasNoMethodInterface()
        {
            var c = GetClass(typeof(ByRefLikeDelegate));
            c.getDeclaredClasses().Select(i => i.getSimpleName()).Should().NotContain("Method");
        }

    }

}
