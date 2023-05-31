using FluentAssertions;

using IKVM.Runtime.Accessors;
using IKVM.Runtime.Accessors.Ikvm.Internal;
using IKVM.Runtime.Accessors.Java.Lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime.Accessors.Java.Lang
{

    [TestClass]
    public class ClassAccessorTests
    {

        [TestMethod]
        public void CanInvokeGetMethod()
        {
            AccessorRef<CallerIDAccessor> callerIDAccessorRef = new();
            AccessorRef<ClassAccessor> classAccessorRef = new();

            var callerID = callerIDAccessorRef.Value.InvokeCreate(typeof(ClassAccessorTests).TypeHandle);

            var c = classAccessorRef.Value.InvokeForName("java.lang.Object", callerID);
            c.Should().NotBeNull();

            var m = classAccessorRef.Value.InvokeGetMethod(c, "toString", classAccessorRef.Value.InitArray(0), callerID);
            m.Should().NotBeNull();
        }

    }

}
