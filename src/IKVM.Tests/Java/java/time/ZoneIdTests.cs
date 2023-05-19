using FluentAssertions;

using java.time;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.time
{

    [TestClass]
    public class ZoneIdTests
    {

        [TestMethod]
        public void SystemDefaultZoneIdShouldMatchTimeZone()
        {
            var test1 = ZoneId.systemDefault();
            var test2 = TimeZone.getDefault();
            test1.getId().Should().Be(test2.getID());
        }

    }

}
