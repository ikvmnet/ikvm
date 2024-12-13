using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ObjectTests
    {

        [TestMethod]
        public void CanCreateObject()
        {
            var o = new global::java.lang.Object();
            o.Should().BeOfType<global::java.lang.Object>();
        }

    }

}
