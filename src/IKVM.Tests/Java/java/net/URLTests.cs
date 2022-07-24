using System.Text;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLTests
    {

        [TestMethod]
        public void Can_fetch_http_url()
        {
            var stm = new global::java.net.URL("https://httpbin.org/anything?text=ECHO").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, 8192);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            var json = JObject.Parse(str);
            json["args"]["text"].Value<string>().Should().Be("ECHO");
        }

    }

}
