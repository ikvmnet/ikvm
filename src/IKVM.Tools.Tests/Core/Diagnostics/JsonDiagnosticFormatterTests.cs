using System;
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
            wrt.Write(DiagnosticEvent.GenericClassLoadingError("ERROR", location: new DiagnosticLocation("file.txt", 1, 2, 3, 4)));
            wrt.Dispose();

            var obj = JsonObject.Parse(Encoding.UTF8.GetString(mem.ToArray()));
            ((int)obj["id"]).Should().Be(4019);
            ((string)obj["level"]).Should().Be("error");
            ((string)obj["message"]).Should().Be("{0}");
            ((JsonArray)obj["args"]).Count.Should().Be(1);
            ((string)((JsonArray)obj["args"])[0]).Should().Be("ERROR");
            ((string)((JsonObject)obj["location"])["path"]).Should().Be("file.txt");
            ((int)((JsonObject)obj["location"])["position"][0]).Should().Be(1);
            ((int)((JsonObject)obj["location"])["position"][1]).Should().Be(2);
            ((int)((JsonObject)obj["location"])["position"][2]).Should().Be(3);
            ((int)((JsonObject)obj["location"])["position"][3]).Should().Be(4);
        }

        [TestMethod]
        public void CanFormatConsoleEncodingEvent()
        {
            var enc = Console.OutputEncoding;

            var mem = new MemoryStream();
            var opt = new JsonDiagnosticFormatterOptions(new StreamDiagnosticChannel(mem, enc, false));
            var wrt = new JsonDiagnosticFormatter(opt);
            wrt.Write(DiagnosticEvent.GenericClassLoadingError("ERROR", location: new DiagnosticLocation("file.txt", 1, 2, 3, 4)));
            wrt.Dispose();

            var obj = JsonObject.Parse(enc.GetString(mem.ToArray()));
            ((int)obj["id"]).Should().Be(4019);
            ((string)obj["level"]).Should().Be("error");
            ((string)obj["message"]).Should().Be("{0}");
            ((JsonArray)obj["args"]).Count.Should().Be(1);
            ((string)((JsonArray)obj["args"])[0]).Should().Be("ERROR");
            ((string)((JsonObject)obj["location"])["path"]).Should().Be("file.txt");
            ((int)((JsonObject)obj["location"])["position"][0]).Should().Be(1);
            ((int)((JsonObject)obj["location"])["position"][1]).Should().Be(2);
            ((int)((JsonObject)obj["location"])["position"][2]).Should().Be(3);
            ((int)((JsonObject)obj["location"])["position"][3]).Should().Be(4);
        }

    }

}
