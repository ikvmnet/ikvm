using FluentAssertions;

using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util
{

    [TestClass]
    public class LocaleTests
    {

        [TestMethod]
        public void Can_get_default_locale()
        {
            var l = Locale.getDefault();
            l.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_us_locale()
        {
            var l = Locale.US;
            l.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_english_locale()
        {
            var l = Locale.ENGLISH;
            l.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_uk_locale()
        {
            var l = Locale.UK;
            l.Should().NotBeNull();
        }

    }

}
