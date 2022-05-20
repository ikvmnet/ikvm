using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests
{

    [TestClass]
    public class IKVMTest
    {

        [TestMethod]
        public void Can_resolve_object_type()
        {
            var o = (object)java.lang.Class.forName("java.lang.Object");
            o.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_resolve_string_type()
        {
            var o = (object)java.lang.Class.forName("java.lang.String");
            o.Should().NotBeNull();
        }

    }

}
