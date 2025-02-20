using FluentAssertions;

using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class ClassFileTests
    {

        [TestMethod]
        public void IsValidFieldDescriptor()
        {
            ClassFile.IsValidFieldDescriptor("").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("L").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("L;").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("Lcom;").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Lcom;A").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("B").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Z").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("C").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("S").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("I").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("J").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("F").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("D").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Q").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("B ").Should().BeFalse();
        }

        [TestMethod]
        public void IsValidMethodDescriptor()
        {
            ClassFile.IsValidMethodDescriptor("").Should().BeFalse();
            ClassFile.IsValidMethodDescriptor("()V").Should().BeTrue();
        }

    }

}
