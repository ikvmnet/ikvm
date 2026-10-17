using System.Text;
using System.Text.Json;

using FluentAssertions;

using IKVM.Tools.Runner.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Runner.Diagnostics
{

    [TestClass]
    public class IkvmToolDiagnosticEventTests
    {

        [TestMethod]
        public void CanReadFullEvent()
        {
            var r = new Utf8JsonReader("""
            {
              "id": 10,
              "level": "warning",
              "message": "Message {0}",
              "args": [ "hi" ],
              "location": {
                "path": "file.txt",
                "position": [ 1, 2, 3, 4 ]
              }
            }
            """u8);

            var e = IkvmToolDiagnosticEvent.ReadJson(ref r);
            e.Id.Should().Be(10);
            e.Level.Should().Be(IkvmToolDiagnosticEventLevel.Warning);
            e.Message.Should().Be("Message {0}");
            e.Args.Should().HaveCount(1);
            e.Args[0].Should().Be("hi");
            e.Location.Path.Should().Be("file.txt");
            e.Location.StartLine.Should().Be(1);
            e.Location.StartColumn.Should().Be(2);
            e.Location.EndLine.Should().Be(3);
            e.Location.EndColumn.Should().Be(4);
        }

        [TestMethod]
        public void CanReadFullEventWithPadding()
        {
            var r = new Utf8JsonReader("""
            {
              "id": 10,
              "level": "warning",
              "message": "Message {0}",
              "args": [ "hi" ],
              "location": {
                "path": "file.txt",
                "position": [ 1, 2, 3, 4 ]
              }
            }
            """u8);

            var e = IkvmToolDiagnosticEvent.ReadJson(ref r);
            e.Id.Should().Be(10);
            e.Level.Should().Be(IkvmToolDiagnosticEventLevel.Warning);
            e.Message.Should().Be("Message {0}");
            e.Args.Should().HaveCount(1);
            e.Args[0].Should().Be("hi");
            e.Location.Path.Should().Be("file.txt");
            e.Location.StartLine.Should().Be(1);
            e.Location.StartColumn.Should().Be(2);
            e.Location.EndLine.Should().Be(3);
            e.Location.EndColumn.Should().Be(4);
        }

        [TestMethod]
        [DataRow("\"trace\"", IkvmToolDiagnosticEventLevel.Trace)]
        [DataRow("\"info\"", IkvmToolDiagnosticEventLevel.Info)]
        [DataRow("\"warning\"", IkvmToolDiagnosticEventLevel.Warning)]
        [DataRow("\"error\"", IkvmToolDiagnosticEventLevel.Error)]
        [DataRow("\"fatal\"", IkvmToolDiagnosticEventLevel.Fatal)]
        public void CanParseLevel(string text, IkvmToolDiagnosticEventLevel level)
        {
            var r = new Utf8JsonReader(Encoding.UTF8.GetBytes(text));
            r.Read();
            IkvmToolDiagnosticEvent.ParseLevel(ref r).Should().Be(level);
        }

    }

}
