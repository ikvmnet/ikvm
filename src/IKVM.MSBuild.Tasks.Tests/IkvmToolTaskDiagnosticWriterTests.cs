using FluentAssertions;
using IKVM.Tools.Runner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using StructuredLog = IKVM.MSBuild.Tasks.IkvmToolTaskDiagnosticWriter.ParsedEvent;

namespace IKVM.MSBuild.Tasks.Tests;

[TestClass]
public class IkvmToolTaskDiagnosticWriterTests
{
    [TestMethod, DynamicData(nameof(StructuredLogTestCases))]
    public void VerifyStructuredLog(IkvmToolDiagnosticEventLevel eventLevel, string eventMessage, StructuredLogTestCase expectedData)
    {
        IkvmToolTaskDiagnosticWriter.ParseEvent(
            new(eventLevel, eventMessage)
        ).Should().Be(new StructuredLog(
            expectedData.Level,
            expectedData.Code,
            expectedData.Message));
    }

    public static IEnumerable<object[]> StructuredLogTestCases => [
        [
            IkvmToolDiagnosticEventLevel.Warning,
            "ErrorIKVM0000: Message",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Error, "IKVM0000", "Message")
        ], [
            IkvmToolDiagnosticEventLevel.Warning,
            "WarningIKVM0000: Message",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Warning, "IKVM0000", "Message")
        ], [
            IkvmToolDiagnosticEventLevel.Warning,
            "    (at Some:File)",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Warning, null, "    (at Some:File)")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "    (at Some:File)",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Debug, null, "    (at Some:File)")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "Error IKVM0000: Message",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Error, "IKVM0000", "Message")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "Trace IKVM0000: Message",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Debug, "IKVM0000", "Message")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "Info IKVM0000: Message",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Information, "IKVM0000", "Message")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "Error: Unspecified Error",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Error, "", "Unspecified Error")
        ], [
            IkvmToolDiagnosticEventLevel.Debug,
            "Warning: Unspecified Warning",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Warning, "", "Unspecified Warning")
        ], [
            IkvmToolDiagnosticEventLevel.Warning,
            "Info: Unspecified Information",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Information, "", "Unspecified Information")
        ], [
            IkvmToolDiagnosticEventLevel.Warning,
            "Trace: Unspecified Trace",
            new StructuredLogTestCase(IkvmToolDiagnosticEventLevel.Debug, "", "Unspecified Trace")
        ]
    ];

    public record struct StructuredLogTestCase(
        IkvmToolDiagnosticEventLevel Level,
        string Code,
        string Message
    );
}