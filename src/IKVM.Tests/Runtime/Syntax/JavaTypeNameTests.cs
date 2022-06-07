using FluentAssertions;

using IKVM.Runtime.Syntax;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Syntax
{

    [TestClass]
    public class JavaTypeNameTests
    {

        [TestMethod]
        public void Can_parse_type_without_package()
        {
            var t = new JavaTypeName("TypeName");
            t.IsInnerClass.Should().BeFalse();
            t.IsNestedClass.Should().BeFalse();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("");
            t.PackageName.IsChildOf("").Should().BeFalse();
            t.UnqualifiedName.ToString().Should().Be("TypeName");
            t.SimpleName.ToString().Should().Be("TypeName");
            t.Parent.ToString().Should().Be("");
            t.IsMemberOf("").Should().BeTrue();
            t.IsMemberOf("com").Should().BeFalse();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_type_with_package()
        {
            var t = new JavaTypeName("com.TypeName");
            t.IsInnerClass.Should().BeFalse();
            t.IsNestedClass.Should().BeFalse();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("com");
            t.PackageName.IsChildOf("").Should().BeTrue();
            t.UnqualifiedName.ToString().Should().Be("TypeName");
            t.SimpleName.ToString().Should().Be("TypeName");
            t.Parent.ToString().Should().Be("");
            t.IsMemberOf("").Should().BeFalse();
            t.IsMemberOf("com").Should().BeTrue();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_type_with_multilevel_package()
        {
            var t = new JavaTypeName("com.foo.TypeName");
            t.IsInnerClass.Should().BeFalse();
            t.IsNestedClass.Should().BeFalse();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("com.foo");
            t.PackageName.IsChildOf("").Should().BeTrue();
            t.PackageName.IsChildOf("com").Should().BeTrue();
            t.PackageName.IsChildOf("com.foo").Should().BeFalse();
            t.PackageName.IsChildOf("com.bar").Should().BeFalse();
            t.PackageName.IsChildOf("bar").Should().BeFalse();
            t.UnqualifiedName.ToString().Should().Be("TypeName");
            t.SimpleName.ToString().Should().Be("TypeName");
            t.Parent.ToString().Should().Be("");
            t.IsMemberOf("").Should().BeFalse();
            t.IsMemberOf("com").Should().BeFalse();
            t.IsMemberOf("com.foo").Should().BeTrue();
            t.IsMemberOf("com.bar").Should().BeFalse();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_nested_type_without_package()
        {
            var t = new JavaTypeName("TypeName$Nested");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeTrue();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("");
            t.UnqualifiedName.ToString().Should().Be("TypeName$Nested");
            t.SimpleName.ToString().Should().Be("Nested");
            t.Parent.ToString().Should().Be("TypeName");
            t.IsMemberOf("").Should().BeTrue();
            t.IsMemberOf("com").Should().BeFalse();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_nested_type_with_package()
        {
            var t = new JavaTypeName("com.TypeName$Nested");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeTrue();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("com");
            t.PackageName.IsChildOf("").Should().BeTrue();
            t.UnqualifiedName.ToString().Should().Be("TypeName$Nested");
            t.SimpleName.ToString().Should().Be("Nested");
            t.Parent.ToString().Should().Be("com.TypeName");
            t.IsMemberOf("").Should().BeFalse();
            t.IsMemberOf("com").Should().BeTrue();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_multiple_nested_type_without_package()
        {
            var t = new JavaTypeName("TypeName$Nested$NestedMore");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeTrue();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("");
            t.PackageName.IsChildOf("").Should().BeFalse();
            t.UnqualifiedName.ToString().Should().Be("TypeName$Nested$NestedMore");
            t.SimpleName.ToString().Should().Be("NestedMore");
            t.Parent.ToString().Should().Be("TypeName$Nested");
            t.IsMemberOf("").Should().BeTrue();
            t.IsMemberOf("com").Should().BeFalse();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_multiple_nested_type_with_package()
        {
            var t = new JavaTypeName("com.TypeName$Nested$NestedMore");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeTrue();
            t.IsAnonymousClass.Should().BeFalse();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("com");
            t.PackageName.IsChildOf("").Should().BeTrue();
            t.UnqualifiedName.ToString().Should().Be("TypeName$Nested$NestedMore");
            t.SimpleName.ToString().Should().Be("NestedMore");
            t.Parent.ToString().Should().Be("com.TypeName$Nested");
            t.IsMemberOf("").Should().BeFalse();
            t.IsMemberOf("com").Should().BeTrue();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_anonymous_class_without_package()
        {
            var t = new JavaTypeName("TypeName$123");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeFalse();
            t.IsAnonymousClass.Should().BeTrue();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("");
            t.PackageName.IsChildOf("").Should().BeFalse();
            t.UnqualifiedName.ToString().Should().Be("TypeName$123");
            t.SimpleName.ToString().Should().Be("");
            t.Parent.ToString().Should().Be("TypeName");
            t.IsMemberOf("").Should().BeTrue();
            t.IsMemberOf("com").Should().BeFalse();
            t.IsMemberOf("bar").Should().BeFalse();
        }

        [TestMethod]
        public void Can_parse_anonymous_class_with_package()
        {
            var t = new JavaTypeName("com.TypeName$123");
            t.IsInnerClass.Should().BeTrue();
            t.IsNestedClass.Should().BeFalse();
            t.IsAnonymousClass.Should().BeTrue();
            t.IsLocalClass.Should().BeFalse();
            t.PackageName.ToString().Should().Be("com");
            t.PackageName.IsChildOf("").Should().BeTrue();
            t.UnqualifiedName.ToString().Should().Be("TypeName$123");
            t.SimpleName.ToString().Should().Be("");
            t.Parent.ToString().Should().Be("com.TypeName");
            t.IsMemberOf("").Should().BeFalse();
            t.IsMemberOf("com").Should().BeTrue();
            t.IsMemberOf("bar").Should().BeFalse();
        }

    }

}
