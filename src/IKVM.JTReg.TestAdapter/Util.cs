using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using java.lang;
using java.text;
using java.util;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Provides various information for working against 'com.sun.javatest.TestResult' instances.
    /// </summary>
    static class Util
    {

        /// <summary>
        /// Discovers the test suite directories specified by the given source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTestSuiteDirectories(string source, IMessageLogger logger)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            // normalize source path
            source = Path.GetFullPath(source);
            logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Scanning for test suites for '{source}'.");

            var assembly = Assembly.LoadFrom(source);
            foreach (var testAttr in assembly.GetCustomAttributes<JTRegTestSuiteAttribute>())
            {
                // relative root is relative to test assembly
                var testPath = Path.IsPathRooted(testAttr.Path) ? testAttr.Path : Path.Combine(Path.GetDirectoryName(assembly.Location), testAttr.Path);
                var rootFile = Path.Combine(testPath, IkvmJTRegTestAdapter.TEST_ROOT_FILE_NAME);
                if (File.Exists(rootFile) == false)
                {
                    logger.SendMessage(TestMessageLevel.Error, "JTReg: " + $"Missing test root file: {rootFile}");
                    continue;
                }

                logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Found test suite: {testPath}");
                yield return testPath;
            }
        }

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

            var testCase = new TestCase(Util.GetFullyQualifiedTestName(source, testSuite, testResult), new Uri("executor://ikvmjtregtestadapter/v1"), source);
            testCase.CodeFilePath = ((java.io.File)testResult.getDescription().getFile())?.toPath().toAbsolutePath().toString();
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestSuiteRootProperty, ((java.io.File)testSuite.getRootDir()).toPath().toAbsolutePath().toString());
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestSuiteNameProperty, GetTestSuiteName(source, testSuite));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestPathNameProperty, GetTestPathName(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestIdProperty, GetTestId(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestTitleProperty, GetTestTitle(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestAuthorProperty, GetTestAuthor(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestCategoryProperty, GetTestKeywords(source, testSuite, testResult));
            testCase.SetPropertyValue(IkvmJTRegTestProperties.TestPartitionProperty, partition);
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
                DisplayName = Util.GetTestDisplayName(testResult),
                ComputerName = testResult.getProperty("hostname"),
                Duration = ParseElapsed(testResult.getProperty("elapsed")),
                StartTime = ParseLogDate(testResult.getProperty("start")),
                EndTime = ParseLogDate(testResult.getProperty("end")),
                Outcome = ToTestOutcome(testResult.getStatus()),
                ErrorMessage = testResult.getProperty("execStatus"),
            };

            // create an attachment set for our results
            var attachments = new AttachmentSet(new Uri("executor://ikvmjtregtestadapter/v1"), "IkvmJTRegTestAdapter");
            r.Attachments.Add(attachments);

            // if a JTR file is available, add it as an attachment
            var jtrFile = (java.io.File)testResult.getFile();
            if (jtrFile != null && jtrFile.exists())
                attachments.Attachments.Add(new UriDataAttachment(new Uri(jtrFile.getAbsolutePath()), jtrFile.getName()));

            // jtreg outputs sections, each with multiple output, translate to messages
            for (int i = 0; i < testResult.getSectionCount(); i++)
                if (testResult.getSection(i) != null)
                    foreach (var message in GetTestResultSectionMessages(testResult.getSection(i)))
                        r.Messages.Add(message);

            return r;
        }

        /// <summary>
        /// For a given section, returns a new test result message that document's it's completion.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        static IEnumerable<TestResultMessage> GetTestResultSectionMessages(dynamic section)
        {
            // since vstest splits output by category, append a common header to each to make them distinguishable
            var header = new System.Text.StringBuilder();
            header.AppendLine($"ACTION: {(string)section.getTitle()}");
            header.AppendLine($"STATUS: {section.getStatus()}");
            var m = (string)section.getOutput("messages");
            if (m != null)
                header.AppendLine(m.Trim());

            // each section contains one or more outputs
            foreach (var outputName in section.getOutputNames())
            {
                var messageCategory = outputName switch
                {
                    "System.out" => TestResultMessage.StandardOutCategory,
                    "System.err" => TestResultMessage.StandardErrorCategory,
                    _ => TestResultMessage.AdditionalInfoCategory,
                };

                var output = (string)section.getOutput(outputName);
                var text = new System.Text.StringBuilder();
                text.AppendLine(header.ToString());
                text.AppendLine(output.Trim());
                text.AppendLine("----");
                yield return new TestResultMessage(messageCategory, text.ToString());
            }
        }

        /// <summary>
        /// Find the reason that the action was run. This method takes advantage of the fact that the reason string begins with a known value and ends with a line seperator.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        static string GetTestResultSectionReason(dynamic section)
        {
            const string SEARCH = "reason: ";

            var o = (string)section.getOutput("messages");
            var posBeg = o.IndexOf(SEARCH) + SEARCH.Length;
            var posEnd = o.IndexOf(Environment.NewLine, posBeg);
            return o.Substring(posBeg, posEnd);
        }

        /// <summary>
        /// Finds the elapsed time for the action. This method takes advantage of the fact that the string containing the elapsed time begins with a known value and ends with a line seperator.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        static string GetElapsedTime(dynamic section)
        {
            const string SEARCH = "elased time (seconds): ";

            var o = (string)section.getOutput("messages");
            var posBeg = o.IndexOf(SEARCH) + SEARCH.Length;
            var posEnd = o.IndexOf(Environment.NewLine, posBeg);
            return o.Substring(posBeg, posEnd);
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
            return v != null && int.TryParse(v.Split(' ')[0], out var s) ? TimeSpan.FromMilliseconds(s) : TimeSpan.Zero;
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
