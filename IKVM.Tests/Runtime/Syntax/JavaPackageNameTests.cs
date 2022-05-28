using FluentAssertions;

using IKVM.Runtime.Syntax;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Syntax
{

    [TestClass]
    public class JavaPackageNameTests
    {

        [TestMethod]
        public void Can_test_for_equality()
        {
            var a = new JavaPackageName("com");
            var b = new JavaPackageName("com");
            (a == b).Should().BeTrue();
        }

        [TestMethod]
        public void Can_test_for_inequality()
        {
            var a = new JavaPackageName("com");
            var b = new JavaPackageName("org");
            (a != b).Should().BeTrue();
        }

        [TestMethod]
        public void Can_determine_child()
        {
            var a = new JavaPackageName("com");
            var b = new JavaPackageName("com.foo");
            b.IsChildOf(a).Should().BeTrue();
            a.IsChildOf(b).Should().BeFalse();

            var c = new JavaPackageName("com.foo.blah.bar");
            var d = new JavaPackageName("com.foo.bar.blah");
            c.IsChildOf(d).Should().BeFalse();
            d.IsChildOf(d).Should().BeFalse();
        }

        [TestMethod]
        public void Can_determine_child_of_empty_package()
        {
            var a = new JavaPackageName("");
            var b = new JavaPackageName("com");
            var c = new JavaPackageName("com.foo");
            c.IsChildOf(a).Should().BeTrue();
            b.IsChildOf(a).Should().BeTrue();
            a.IsChildOf(b).Should().BeFalse();
        }

        [TestMethod]
        public void Empty_package_should_not_be_child_of_empty_package()
        {
            var a = new JavaPackageName("");
            var b = new JavaPackageName("");
            a.IsChildOf(b).Should().BeFalse();
        }

    }

}
