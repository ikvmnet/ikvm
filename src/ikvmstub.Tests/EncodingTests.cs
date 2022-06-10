using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ikvmstub.Tests
{
    public class EncodingTests
    {
        [TestMethod]
        public void Should_load_encoding_Windows1252()
        {
            try
            {
                Encoding.GetEncoding("Windows-1252");
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        [TestMethod]
        public void Should_load_encoding_GB2312()
        {
            try
            {
                Encoding.GetEncoding("gb2312");
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }
    }
}
