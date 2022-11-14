using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.text
{

    [TestClass]
    public class NumberFormatTests
    {

        [TestMethod]
        public void Can_format_usd_as_english()
        {
            var usd = global::java.util.Currency.getInstance("USD");
            var fmt = global::java.text.NumberFormat.getCurrencyInstance(global::java.util.Locale.ENGLISH);
            fmt.setCurrency(usd);
            fmt.format(10).Should().Be("USD10.00");
        }

        [TestMethod]
        public void Can_format_usd_as_chinese()
        {
            var usd = global::java.util.Currency.getInstance("USD");
            var fmt = global::java.text.NumberFormat.getCurrencyInstance(global::java.util.Locale.TRADITIONAL_CHINESE);
            fmt.setCurrency(usd);
            fmt.format(10).Should().Be("USD10.00");
        }

    }

}
