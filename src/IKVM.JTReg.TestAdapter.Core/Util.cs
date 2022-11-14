using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using java.text;
using java.util;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Provides various information for working against 'com.sun.javatest.TestResult' instances.
    /// </summary>
    static class Util
    {

        internal const int PARTITION_COUNT = 8;

        static readonly SimpleDateFormat TestResultDateFormat = new SimpleDateFormat("EEE MMM dd HH:mm:ss z yyyy", Locale.US);
        static readonly MethodInfo RemainingToListOfTestResultMethod = typeof(IteratorExtensions).GetMethod(nameof(IteratorExtensions.RemainingToList)).MakeGenericMethod(JTRegTypes.TestResult.Type);

        /// <summary>
        /// Discovers the test suite directories specified by the given source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTestSuiteDirectories(string source, IJTRegLoggerContext logger)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            // normalize source path
            source = Path.GetFullPath(source);
            logger.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Scanning for test suites for '{source}'.");

            var assembly = Assembly.LoadFrom(source);
            foreach (var testAttr in assembly.GetCustomAttributes<JTRegTestSuiteAttribute>())
            {
                // relative root is relative to test assembly
                var testPath = Path.IsPathRooted(testAttr.Path) ? testAttr.Path : Path.Combine(Path.GetDirectoryName(assembly.Location), testAttr.Path);
                var rootFile = Path.Combine(testPath, JTRegTestManager.TEST_ROOT_FILE_NAME);
                if (File.Exists(rootFile) == false)
                {
                    logger.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: Missing test root file: {rootFile}");
                    continue;
                }

                logger.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Found test suite: {testPath}");
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
        /// Gets the results of an interview.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<dynamic> GetTestResults(string source, dynamic testManager, dynamic testSuite)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // wait until result is initialized
            var trt = testManager.getWorkDirectory(testSuite).getTestResultTable();
            trt.waitUntilReady();
            return RemainingToListOfTestResult((Iterator)trt.getIterator());
        }

        /// <summary>
        /// Invokes the RemainingToList generic method for an iterator of TestResult objects.
        /// </summary>
        /// <param name="iter"></param>
        /// <returns></returns>
        static IEnumerable<dynamic> RemainingToListOfTestResult(Iterator iter)
        {
            if (iter is null)
                throw new ArgumentNullException(nameof(iter));

            return (IEnumerable<dynamic>)RemainingToListOfTestResultMethod.Invoke(null, new[] { iter });
        }

        /// <summary>
        /// Returns an enumerable of the test cases within the given suite with parameters.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        public static IEnumerable<JTRegTestCase> GetTestCases(string source, dynamic testManager, dynamic testSuite)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            var c = 0;

            // obtain enumerable of the test results within the suite
            var testResults = (IEnumerable<dynamic>)GetTestResults(source, testManager, testSuite);
            testResults = testResults.OrderBy(i => GetTestPathName(source, testSuite, i));

            // yield converted results
            foreach (var testResult in testResults)
                yield return ToTestCase(source, testSuite, testResult, c++ % PARTITION_COUNT);
        }

        /// <summary>
        /// Creates a new <see cref="TestCase"/> from the given test result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="testResult"></param>
        /// <param name="partition"></param>
        /// <returns></returns>
        public static JTRegTestCase ToTestCase(string source, dynamic testSuite, dynamic testResult, int partition)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));

            var testCase = new JTRegTestCase((string)Util.GetFullyQualifiedTestName(source, testSuite, testResult), new Uri(JTRegTestManager.URI), source);
            testCase.CodeFilePath = ((java.io.File)testResult.getDescription().getFile())?.toPath().toAbsolutePath().toString();
            testCase.TestSuiteRoot = ((java.io.File)testSuite.getRootDir()).toPath().toAbsolutePath().toString();
            testCase.TestSuiteName = GetTestSuiteName(source, testSuite);
            testCase.TestPathName = GetTestPathName(source, testSuite, testResult);
            testCase.TestId = GetTestId(source, testSuite, testResult);
            testCase.TestTitle = GetTestTitle(source, testSuite, testResult);
            testCase.TestAuthor = GetTestAuthor(source, testSuite, testResult);
            testCase.TestCategory = (string[])GetTestKeywords(source, testSuite, testResult);
            testCase.TestPartition = partition;
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
        public static JTRegTestResult ToTestResult(string source, dynamic testResult, JTRegTestCase testCase)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testResult is null)
                throw new ArgumentNullException(nameof(testResult));
            if (testCase is null)
                throw new ArgumentNullException(nameof(testCase));

            var r = new JTRegTestResult(testCase)
            {
                DisplayName = Util.GetTestDisplayName(testResult),
                ComputerName = testResult.getProperty("hostname"),
                Duration = ParseTestResultTimeSpan(testResult.getProperty("elapsed")),
                StartTime = ParseTestResultDate(testResult.getProperty("start")),
                EndTime = ParseTestResultDate(testResult.getProperty("end")),
                Outcome = ToTestOutcome(testResult.getStatus()),
                ErrorMessage = testResult.getProperty("execStatus"),
            };

            // jtreg outputs sections, each with multiple output, translate to messages
            for (int i = 0; i < testResult.getSectionCount(); i++)
                if (testResult.getSection(i) != null)
                    foreach (var message in GetTestResultSectionMessages(testResult.getSection(i)))
                        r.Messages.Add(message);

            // create an attachment set for our results
            var attachments = new JTRegAttachmentSet(new Uri(JTRegTestManager.URI), "IkvmJTRegTestAdapter");
            r.Attachments.Add(attachments);

            // if a JTR file is available, add it as an attachment
            var jtrFile = (java.io.File)testResult.getFile();
            if (jtrFile != null && jtrFile.exists())
                attachments.Attachments.Add(new JTRegUriDataAttachment(new Uri(jtrFile.getAbsolutePath()), jtrFile.getName()));

            return r;
        }

        /// <summary>
        /// For a given section, returns a new test result message that document's it's completion.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        static IEnumerable<JTRegTestResultMessage> GetTestResultSectionMessages(dynamic section)
        {
            if (section is null)
                throw new ArgumentNullException(nameof(section));

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
                    "System.out" => JTRegTestResultMessageCategory.StandardOut,
                    "System.err" => JTRegTestResultMessageCategory.StandardError,
                    _ => JTRegTestResultMessageCategory.AdditionalInfo,
                };

                var output = (string)section.getOutput(outputName);
                var text = new System.Text.StringBuilder();
                text.AppendLine(header.ToString());
                text.AppendLine(output.Trim());
                text.AppendLine("----");
                yield return new JTRegTestResultMessage(messageCategory, text.ToString());
            }
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

            return (string)testResult.getDescription().getId();
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

            return (string)testResult.getDescription().getName();
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
            var testName = testResult.getDescription().getName().Replace('.', '_').Replace('\\', '/');
            return $"{GetTestSuiteName(source, testSuite)}.{pathName}.{testName}";
        }

        /// <summary>
        /// Parses the common date time format used by jtreg.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        static DateTimeOffset ParseTestResultDate(string v)
        {
            try
            {
                if (v != null)
                {
                    var p = TestResultDateFormat.parse(v);
                    var d = DateTimeOffset.FromUnixTimeMilliseconds(p.getTime()).ToOffset(new TimeSpan(0, -p.getTimezoneOffset(), 0));
                    return d;
                }
            }
            catch (ParseException)
            {
                // ignore
            }

            return DateTimeOffset.MinValue;
        }

        static TimeSpan ParseTestResultTimeSpan(string v)
        {
            return v != null && int.TryParse(v.Split(' ')[0], out var s) ? TimeSpan.FromMilliseconds(s) : TimeSpan.Zero;
        }

        /// <summary>
        /// Maps a <see cref="Status"/> to a <see cref="TestOutcome"/>.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static JTRegTestOutcome ToTestOutcome(dynamic status)
        {
            if (status.isPassed())
                return JTRegTestOutcome.Passed;
            else if (status.isFailed())
                return JTRegTestOutcome.Failed;
            else if (status.isError())
                return JTRegTestOutcome.Failed;
            else if (status.isNotRun())
                return JTRegTestOutcome.Skipped;
            else
                return JTRegTestOutcome.None;
        }

    }

}
