using FluentAssertions;

using java.lang;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class StrictMathTests
    {

        [TestMethod]
        public void AbsDouble()
        {
            StrictMath.abs(-1.5d).Should().Be(1.5d);
        }

    }

}
