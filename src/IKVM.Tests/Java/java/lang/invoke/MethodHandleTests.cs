using IKVM.Runtime;

using java.lang;
using java.lang.invoke;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.invoke
{

    [TestClass]
    public class MethodHandleTests
    {

        class TestClass
        {

            public int Int32Method()
            {
                return 1;
            }

        }

        [TestMethod]
        public void CanInvokeExactForVirtualWithReturn()
        {
            var lookup = MethodHandles.lookup();
            var mt = MethodType.methodType(JVM.Context.PrimitiveJavaTypeFactory.INT.ClassObject);
            var mh = lookup.findVirtual((Class)ClassLiteral<TestClass>.Value, "Int32Method", mt);
            var d = ByteCodeHelper.GetDelegateForInvokeExact<MH<TestClass, int>>(mh);
            var s = d.Invoke(new TestClass());
        }

    }

}
