using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using com.sun.org.glassfish.gmbal;

using java.nio.file;
using java.text;
using java.util;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Provides various information for working against 'com.sun.javatest.TestResult' instances.
    /// </summary>
    static class DynamicObjectMethods
    {

        /// <summary>
        /// Gets the test suites within the given test manager, properly sorted.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testManager"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> GetTestSuites(string source, dynamic testManager)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));

            return ((IEnumerable)testManager.getTestSuites()).Cast<object>().OrderBy(i => GetTestSuiteName(source, i)).Cast<dynamic>();
        }

        /// <summary>
        /// Creates a new <see cref="TestCase"/> from the given test result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static TestCase ToTestCase(string source, dynamic testSuite, dynamic testResult, int partition)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            var testCase = new TestCase(DynamicObjectMethods.GetFullyQualifiedTestName(source, testSuite, testResult), new Uri("executor://IkvmJTRegTestAdapter/v1"), source);
            testCase.CodeFilePath = ((java.io.File)testResult.getDescription().getFile())?.toPath().toAbsolutePath().toString();
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestSuiteRootProperty, ((java.io.File)testSuite.getRootDir()).toPath().toAbsolutePath().toString());
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestSuiteNameProperty, GetTestSuiteName(source, testSuite));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestPathNameProperty, GetTestPathName(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestIdProperty, GetTestId(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestTitleProperty, GetTestTitle(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestAuthorProperty, GetTestAuthor(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestCategoryProperty, GetTestKeywords(source, testSuite, testResult));
            testCase.Traits.Add("Partition", partition.ToString());
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
                DisplayName = DynamicObjectMethods.GetTestDisplayName(testResult),
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
        public static string GetTestDisplayName(dynamic testResult)
        {
            var description = testResult.getDescription();
            return (string)description.getName();
        }

        /// <summary>
        /// Gets the local name of the test suite.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetTestSuiteName(string source, dynamic testSuite)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // root path
            var rootPath = ((java.io.File)testSuite.getRootDir()).toPath();
            var rootName = new java.io.File(source).getParentFile().toPath().relativize(rootPath).toString().Replace('.', '_').Replace('\\', '/');

            return rootName;
        }

        /// <summary>
        /// Gets the path name of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetTestPathName(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return (string)testResult.getDescription().getRootRelativePath();
        }

        /// <summary>
        /// Gets the id of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetTestId(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return testResult.getDescription().getId();
        }

        /// <summary>
        /// Gets the name of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetTestName(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return testResult.getDescription().getName();
        }

        /// <summary>
        /// Gets the title of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetTestTitle(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return (string)testResult.getDescription().getTitle();
        }

        /// <summary>
        /// Gets the author of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string GetTestAuthor(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return (string)testResult.getDescription().getParameter("author");
        }

        /// <summary>
        /// Gets the title of the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public static string[] GetTestKeywords(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            return ((Set)testResult.getDescription().getKeywordTable()).AsSet<string>().ToArray();
        }

        /// <summary>
        /// Gets the fully qualified name for the given test.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFullyQualifiedTestName(string source, dynamic testSuite, dynamic testResult)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            var rootPath = ((java.io.File)testSuite.getRootDir()).toPath();
            var testPath = ((java.io.File)testResult.getDescription().getDir()).toPath();
            var pathName = rootPath.relativize(testPath).toString().Replace('.', '_').Replace('\\', '/');

            // test name
            var testName = testResult.getDescription().getName().Replace('.', '_').Replace('\\', '/');

            return $"{GetTestSuiteName(source, testSuite)}.{pathName}.{testName}";
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
