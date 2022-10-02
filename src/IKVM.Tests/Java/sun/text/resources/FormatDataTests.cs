using FluentAssertions;
using java.util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.sun.text.resources
{

    [TestClass]
    public class FormatDataTests
    {

        [TestMethod]
        public void Can_get_default_FormatData_bundle()
        {
            var b = ResourceBundle.getBundle("sun.text.resources.FormatData", Locale.getDefault());
            b.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_us_FormatData_bundle()
        {
            var b = ResourceBundle.getBundle("sun.text.resources.FormatData", Locale.US);
            b.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_english_FormatData_bundle()
        {
            var b = ResourceBundle.getBundle("sun.text.resources.FormatData", Locale.ENGLISH);
            b.Should().NotBeNull();
        }

        [TestMethod]
        public void Can_get_uk_FormatData_bundle()
        {
            var b = ResourceBundle.getBundle("sun.text.resources.FormatData", Locale.UK);
            b.Should().NotBeNull();
        }

    }

}
