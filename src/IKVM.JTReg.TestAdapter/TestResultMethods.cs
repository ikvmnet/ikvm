using System;
using System.Collections.Generic;
using System.Security.Claims;

using com.sun.org.glassfish.gmbal;

using java.text;
using java.util;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Provides various information for working against 'com.sun.javatest.TestResult' instances.
    /// </summary>
    static class TestResultMethods
    {

        /// <summary>
        /// Creates a new <see cref="TestCase"/> from the given test result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static TestCase ToTestCase(string source, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            var name = TestResultMethods.GetFullyQualifiedName(testResult);
            var description = testResult.getDescription();

            var testCase = new TestCase(name, new Uri("executor://IkvmJTRegTestAdapter/v1"), source);
            testCase.CodeFilePath = ((java.io.File)description.getFile())?.toString();
            return testCase;
        }

        /// <summary>
        /// Creates a new <see cref="TestResult"/> from the given test result. Optionally specifies an existing <see cref="TestCase"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testResult"></param>
        /// <param name="testCase"></param>
        /// <returns></returns>
        public static TestResult ToTestResult(string source, dynamic testResult, TestCase testCase)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));
            if (testCase is null)
                throw new ArgumentNullException(nameof(testCase));

            var r = new TestResult(testCase)
            {
                DisplayName = TestResultMethods.GetDisplayName(testResult),
                ComputerName = testResult.getProperty("hostname"),
                Duration = ParseElapsed(testResult.getProperty("elapsed")),
                StartTime = ParseLogDate(testResult.getProperty("start")),
                EndTime = ParseLogDate(testResult.getProperty("end")),
                Outcome = ToTestOutcome(testResult.getStatus()),
                ErrorMessage = testResult.getProperty("execStatus"),
            };

            for (int i = 0; i < testResult.getSectionCount(); i++)
            {
                var s = testResult.getSection(i);
                foreach (var j in s.getOutputNames())
                {
                    var o = s.getOutput(j);
                    var m = new TestResultMessage($"{s.getTitle()} - {j}", o);
                    r.Messages.Add(m);
                }
            }

            return r;
        }

        /// <summary>
        /// Gets the display name for a test result.
        /// </summary>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetDisplayName(dynamic testResult)
        {
            var description = testResult.getDescription();
            return (string)description.getName();
        }

        /// <summary>
        /// Gets the fully qualified name for the given test.
        /// </summary>
        /// <param name="testResult"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFullyQualifiedName(dynamic testResult)
        {
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            // get and format test name
            var name = testResult.getTestName();
            if (name.EndsWith(".java"))
                name = name.Substring(0, name.Length - 5);
            name = name.Replace("/", ".");

            // repeat last segment, since we need ns.class.method
            var spl = new List<string>(name.Split('.'));
            spl.Add(spl[spl.Count - 1]);
            name = string.Join(".", spl);

            return name;
        }

        static readonly SimpleDateFormat logFormat = new SimpleDateFormat("EEE MMM dd HH:mm:ss z yyyy", Locale.US);

        /// <summary>
        /// Parses the common date time format used by jtreg.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        static DateTimeOffset ParseLogDate(string v)
        {
            try
            {
                if (v != null)
                {
                    var p = logFormat.parse(v);
                    var d = DateTimeOffset.FromUnixTimeMilliseconds(p.getTime()).ToOffset(new TimeSpan(0, -p.getTimezoneOffset(), 0));
                    return d;
                }
            }
            catch (java.text.ParseException)
            {
                // ignore
            }

            return DateTimeOffset.MinValue;
        }

        static TimeSpan ParseElapsed(string v)
        {
            return v != null && int.TryParse(v.Split(' ')[0], out var s) ? TimeSpan.FromMilliseconds(s) : TimeSpan.MinValue;
        }

        /// <summary>
        /// Maps a <see cref="Status"/> to a <see cref="TestOutcome"/>.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static TestOutcome ToTestOutcome(dynamic status)
        {
            if (status.isPassed())
                return TestOutcome.Passed;
            else if (status.isFailed())
                return TestOutcome.Failed;
            else if (status.isError())
                return TestOutcome.Failed;
            else if (status.isNotRun())
                return TestOutcome.Skipped;
            else
                return TestOutcome.None;
        }

    }

}
