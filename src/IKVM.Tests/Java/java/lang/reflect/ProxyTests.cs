using FluentAssertions;

using java.lang;
using java.lang.reflect;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.reflect
{

    [TestClass]
    public class ProxyTests
    {

        class PublicInterfaceHandler : InvocationHandler
        {

            public object invoke(object obj, Method meth, object[] args)
            {
                if (meth.getName() == "Run")
                    return "TEST";

                throw new System.Exception();
            }

        }

        public interface PublicInterface
        {

            string Run();

        }

        [TestMethod]
        public void CanProxyPublicInterface()
        {
            var c = (Class)typeof(PublicInterface);
            var p = (PublicInterface)Proxy.newProxyInstance(c.getClassLoader(), new[] { c }, new PublicInterfaceHandler());
            p.Run().Should().Be("TEST");
        }

    }

}
