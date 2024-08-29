using System.Collections.Generic;

using FluentAssertions;

using IKVM.Tools.Runner;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static IKVM.MSBuild.Tasks.IkvmToolTaskDiagnosticWriter;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmToolTaskDiagnosticWriterTests
    {

        public record struct ParseEventTestCase(IkvmToolDiagnosticEventLevel Level, string Code, string Message);

        public static IEnumerable<object[]> ParseEventTestCases => [
            [
                IkvmToolDiagnosticEventLevel.Warning,
                "ErrorIKVM0000: Message",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Error, "IKVM0000", "Message")
            ], [
                IkvmToolDiagnosticEventLevel.Warning,
                "WarningIKVM0000: Message",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Warning, "IKVM0000", "Message")
            ], [
                IkvmToolDiagnosticEventLevel.Warning,
                "    (at Some:File)",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Warning, null, "    (at Some:File)")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "    (at Some:File)",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Debug, null, "    (at Some:File)")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "Error IKVM0000: Message",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Error, "IKVM0000", "Message")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "Trace IKVM0000: Message",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Debug, "IKVM0000", "Message")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "Info IKVM0000: Message",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Information, "IKVM0000", "Message")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "Error: Unspecified Error",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Error, "", "Unspecified Error")
            ], [
                IkvmToolDiagnosticEventLevel.Debug,
                "Warning: Unspecified Warning",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Warning, "", "Unspecified Warning")
            ], [
                IkvmToolDiagnosticEventLevel.Warning,
                "Info: Unspecified Information",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Information, "", "Unspecified Information")
            ], [
                IkvmToolDiagnosticEventLevel.Warning,
                "Trace: Unspecified Trace",
                new ParseEventTestCase(IkvmToolDiagnosticEventLevel.Debug, "", "Unspecified Trace")
            ]
        ];

        [TestMethod, DynamicData(nameof(ParseEventTestCases))]
        public void VerifyStructuredLog(IkvmToolDiagnosticEventLevel eventLevel, string eventMessage, ParseEventTestCase testCase)
        {
            ParseEvent(new(eventLevel, eventMessage)).Should().Be(new ParsedEvent(testCase.Level, testCase.Code, testCase.Message));
        }

    }

}
