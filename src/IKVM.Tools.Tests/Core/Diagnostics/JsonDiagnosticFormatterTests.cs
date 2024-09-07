using System.IO;
using System.Text;
using System.Text.Json.Nodes;

using FluentAssertions;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Core.Diagnostics
{

    [TestClass]
    public class JsonDiagnosticFormatterTests
    {

        [TestMethod]
        public void CanFormatEvent()
        {
            var mem = new MemoryStream();
            var opt = new JsonDiagnosticFormatterOptions(new StreamDiagnosticChannel(mem, Encoding.UTF8, false));
            var wrt = new JsonDiagnosticFormatter(opt);
            wrt.Write(DiagnosticEvent.GenericClassLoadingError("ERROR"));
            wrt.Dispose();

            var obj = JsonObject.Parse(Encoding.UTF8.GetString(mem.ToArray()));
            ((int)obj["id"]).Should().Be(4019);
            ((string)obj["name"]).Should().Be("GenericClassLoadingError");
            ((string)obj["level"]).Should().Be("error");
            ((string)obj["message"]).Should().Be("{0}");
            ((JsonArray)obj["args"]).Count.Should().Be(1);
            ((string)((JsonArray)obj["args"])[0]).Should().Be("ERROR");
        }

    }
}
