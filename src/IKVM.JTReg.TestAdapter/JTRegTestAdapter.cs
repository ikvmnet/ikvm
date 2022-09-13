using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

using com.sun.javatest;
using com.sun.javatest.regtest.config;
using com.sun.javatest.regtest.report;

using IKVM.JTReg.TestAdapter;

using java.text;
using java.util;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JavaTest
{

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("executor://JTRegAdapter/v1")]
    [ExtensionUri("executor://JTRegAdapter/v1")]
    public class JTRegTestAdapter : ITestDiscoverer, ITestExecutor2
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JTRegTestAdapter()
        {
            // need to do some static configuration on the harness
            // preload class to ensure class loader is happy
            var cld = ((java.lang.Class)typeof(Harness)).getClassLoader();
            var cls = cld.getResource("com/sun/javatest/Harness.class").getFile();
            var dir = new java.io.File(cls).getParentFile().getParentFile().getParentFile().getParentFile();
            if (Harness.getClassDir() == null)
                Harness.setClassDir(dir);
        }

        readonly Guid id = Guid.NewGuid();
        CancellationTokenSource cts;

        /// <summary>
        /// Discovers the available OpenJDK tests.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            foreach (var source in sources)
                DiscoverTests(source, discoveryContext, logger, discoverySink);
        }

        /// <summary>
        /// Discovers the available OpenJDK tests.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        internal void DiscoverTests(string source, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            logger.SendMessage(TestMessageLevel.Informational, $"Scanning jtreg test directories for '{source}'.");

            // discover all root test directories
            var testDirs = new java.util.ArrayList();
            foreach (var rootDir in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
            {
                logger.SendMessage(TestMessageLevel.Informational, $"Found jtreg test directory: {rootDir}");
                testDirs.add(new java.io.File(Path.GetDirectoryName(rootDir)));
            }

            // temporary path for discovery
            var runDir = Path.Combine("jtreg-", Path.GetTempPath(), id.ToString("n"));

            try
            {
                // attempt to create a temporary directory as scratch space for this test
                logger.SendMessage(TestMessageLevel.Informational, $"Using jtreg run directory: {runDir}");
                Directory.CreateDirectory(runDir);

                using var output = new java.io.PrintWriter(Path.Combine(runDir, "jtreg.out"));
                using var errors = new StreamWriter(File.OpenWrite(Path.Combine(runDir, "jtreg.log")));

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(runDir, output, errors);
                testManager.addTestFiles(testDirs, false);

                // track metrics related to tests
                int testCount = 0;
                var testWatch = new Stopwatch();
                testWatch.Start();

                // for each suite, get the results and transform a test case
                foreach (IkvmRegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
                {
                    logger.SendMessage(TestMessageLevel.Informational, $"Discovered jtreg test suite: {testSuite.getName()}");

                    foreach (var testResult in GetResults(CreateParameters(testManager, testSuite)))
                    {
                        var id = testResult.getDescription().getId();
                        var description = testResult.getDescription();
                        var name = GetFullyQualifiedName(testResult);

                        testCount++;
                        logger.SendMessage(TestMessageLevel.Informational, $"Discovered jtreg test: {name}");
                        var testCase = new TestCase(name, new Uri("executor://JTRegAdapter/v1"), source);
                        testCase.DisplayName = description.getName();
                        testCase.CodeFilePath = description.getFile()?.toString();
                        discoverySink.SendTestCase(testCase);
                    }
                }

                testWatch.Stop();
                logger.SendMessage(TestMessageLevel.Informational, $"Discovered {testCount} jtreg tests for '{source}' in {testWatch.Elapsed.TotalSeconds} seconds.");
            }
            finally
            {
                if (Directory.Exists(runDir))
                {
                    try
                    {
                        Directory.Delete(runDir, true);
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }
            }
        }

        /// <summary>
        /// Runs the given test cases.
        /// </summary>
        /// <param name="tests"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            RunTests(tests.Select(i => i.Source).Distinct(), runContext, frameworkHandle, tests);
        }

        /// <summary>
        /// Runs all the test cases in the sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            RunTests(sources, runContext, frameworkHandle, null);

        }

        /// <summary>
        /// Runs the specified test cases, if provided, in the sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
        {
            try
            {
                cts = new CancellationTokenSource();

                foreach (var source in sources)
                    RunTestForSource(source, runContext, frameworkHandle, tests?.Where(i => i.Source == source), cts.Token);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            finally
            {
                cts = null;
            }
        }

        /// <summary>
        /// Runs the tests in the given source, optionally filtered by specific test case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        internal void RunTestForSource(string source, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // either add all tests, or tests specified
            var testFiles = new java.util.ArrayList();
            if (tests == null)
                foreach (var rootDir in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
                    testFiles.add(new java.io.File(Path.GetDirectoryName(rootDir)));
            else
                foreach (var testFile in tests.Select(i => i.CodeFilePath))
                    testFiles.add(new java.io.File(testFile));

            // temporary path for discovery
            var runDir = Path.Combine(runContext.TestRunDirectory, "jtreg");
            Directory.Delete(runDir, true);
            Directory.CreateDirectory(runDir);
            using var output = new java.io.PrintWriter(Path.Combine(runDir, "jtreg.out"));
            using var errors = new StreamWriter(File.OpenWrite(Path.Combine(runDir, "jtreg.log")));

            // initialize the test manager with the discovered roots
            var testManager = CreateTestManager(Path.Combine(runContext.TestRunDirectory, "jtreg"), output, errors);
            testManager.addTestFiles(testFiles, false);

            // invoke each test suite
            foreach (RegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
                RunTests(runContext, frameworkHandle, tests, CreateParameters(testManager, testSuite), cancellationToken);
        }

        /// <summary>
        /// Executes the tests for the given parameters.
        /// </summary>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTests(IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, IkvmRegressionParameters parameters, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // only continue if there are in fact tests in the suite to execute
            if (parameters.getTests() is string[] t && t.Length > 0)
            {
                try
                {
                    // observe harness for test results
                    var o = new TestObserver(runContext, frameworkHandle, tests);
                    var h = new Harness();
                    h.addObserver(o);

                    // begin harness execution asynchronously, thus allowing potential cancellation
                    h.start(parameters);
                    cancellationToken.Register(() => h.stop());

                    // wait until canceled or finished and cleanup
                    h.waitUntilDone();
                    h.stop();
                    h.removeObserver(o);
                }
                catch (java.lang.InterruptedException)
                {
                    // ignore
                }
                catch (Exception e)
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Error, e.ToString());
                }
            }
        }

        /// <summary>
        /// Traps events from the harness and emits them to the test framework.
        /// </summary>
        class TestObserver : Harness.Observer
        {

            readonly IRunContext runContext;
            readonly IFrameworkHandle frameworkHandle;
            readonly IEnumerable<TestCase> tests;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="runContext"></param>
            /// <param name="frameworkHandle"></param>
            /// <param name="tests"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public TestObserver(IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
            {
                this.runContext = runContext ?? throw new ArgumentNullException(nameof(runContext));
                this.frameworkHandle = frameworkHandle ?? throw new ArgumentNullException(nameof(frameworkHandle));
                this.tests = tests ?? throw new ArgumentNullException(nameof(tests));
            }

            public void startingTestRun(Parameters p)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: starting test run.");
            }

            public void startingTest(com.sun.javatest.TestResult tr)
            {
                var name = GetFullyQualifiedName(tr);
                var test = tests.FirstOrDefault(i => i.FullyQualifiedName == name);
                if (test == null)
                    return;

                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: starting test '{name}'.");
                frameworkHandle.RecordStart(test);
            }

            public void error(string str)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg Harness: ERROR: '{str}'.");
            }

            public void stoppingTestRun()
            {
                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: stopping test run.");
            }

            public void finishedTest(com.sun.javatest.TestResult tr)
            {
                var name = GetFullyQualifiedName(tr);
                var test = tests.FirstOrDefault(i => i.FullyQualifiedName == name);
                if (test == null)
                    return;

                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: finished test '{tr.getTestName()}'.");
                frameworkHandle.RecordResult(ToTestResult(test, tr));
            }

            public void finishedTesting()
            {
                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: finished testing.");
            }

            public void finishedTestRun(bool b)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg Harness: finished test run with overall result '{b}'.");
            }

            /// <summary>
            /// Maps a <see cref="com.sun.javatest.TestResult"/> to a <see cref="Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult"/>.
            /// </summary>
            /// <param name="test"></param>
            /// <param name="tr"></param>
            /// <returns></returns>
            Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult ToTestResult(TestCase test, com.sun.javatest.TestResult tr)
            {
                var testResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(test)
                {
                    DisplayName = test.DisplayName,
                    ComputerName = tr.getProperty("hostname"),
                    Duration = ParseElapsed(tr.getProperty("elapsed")),
                    StartTime = ParseLogDate(tr.getProperty("start")),
                    EndTime = ParseLogDate(tr.getProperty("end")),
                    Outcome = ToTestOutcome(tr.getStatus()),
                    ErrorMessage = tr.getProperty("execStatus"),
                };

                for (int i = 0; i < tr.getSectionCount(); i++)
                {
                    var s = tr.getSection(i);
                    foreach (var j in s.getOutputNames())
                    {
                        var o = s.getOutput(j);
                        var m = new TestResultMessage($"{s.getTitle()} - {j}", o);
                        testResult.Messages.Add(m);
                    }
                }

                return testResult;
            }

            static readonly SimpleDateFormat logFormat = new SimpleDateFormat("EEE MMM dd HH:mm:ss z yyyy", Locale.US);

            /// <summary>
            /// Parses the common date time format used by jtreg.
            /// </summary>
            /// <param name="v"></param>
            /// <returns></returns>
            DateTimeOffset ParseLogDate(string v)
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

            TimeSpan ParseElapsed(string v)
            {
                return int.TryParse(v.Split(' ')[0], out var s) ? TimeSpan.FromMilliseconds(s) : TimeSpan.MinValue;
            }

            /// <summary>
            /// Maps a <see cref="Status"/> to a <see cref="TestOutcome"/>.
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            TestOutcome ToTestOutcome(Status status)
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

        public void Cancel()
        {
            cts?.Cancel();
            cts = null;
        }

        /// <summary>
        /// Creates a new <see cref="TestManager"/>.
        /// </summary>
        /// <param name="runDir"></param>
        /// <returns></returns>
        IkvmTestManager CreateTestManager(string runDir, java.io.PrintWriter output, TextWriter errors)
        {
            var tm = new IkvmTestManager(output, new java.io.File(Environment.CurrentDirectory), new DelegateErrorHandler(s => errors.WriteLine(s)));
            tm.setReportDirectory(new java.io.File(Path.Combine(runDir, "report")));
            tm.setWorkDirectory(new java.io.File(Path.Combine(runDir, "work")));
            return tm;
        }

        /// <summary>
        /// Creates the parameters for a given suite.
        /// </summary>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        IkvmRegressionParameters CreateParameters(TestManager testManager, RegressionTestSuite testSuite)
        {
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            var rp = new IkvmRegressionParameters("regtest", testSuite);

            // configure work directory
            var wd = testManager.getWorkDirectory(testSuite);
            if (Directory.Exists(wd.getPath()) == false)
                Directory.CreateDirectory(wd.getPath());
            rp.setWorkDirectory(wd);
            var jd = wd.getJTData();
            if (Directory.Exists(jd.toString()) == false)
                Directory.CreateDirectory(jd.toString());

            // configure report directory
            var rd = testManager.getReportDirectory(testSuite);
            if (Directory.Exists(rd.toString()) == false)
                Directory.CreateDirectory(rd.toString());
            rp.setReportDir(rd);
            if (rp.isValid() == false)
                throw new Exception();

            rp.setCompileJDK(new IkvmJDK(new java.io.File(Path.Combine(Path.GetDirectoryName(typeof(JTRegTestAdapter).Assembly.Location)), "ikvm")));
            rp.setTestJDK(rp.getCompileJDK());
            rp.setExecMode(ExecMode.OTHERVM);
            rp.setFile(wd.getFile("config.jti"));
            rp.setPriorStatusValues(null);
            rp.setConcurrency(Environment.ProcessorCount);
            rp.setTimeLimit(15000);
            rp.setRetainArgs(global::java.util.Collections.singletonList("all"));
            rp.setExcludeLists(new java.io.File[0]);
            rp.setMatchLists(new java.io.File[0]);
            rp.setEnvVars(new java.util.HashMap());
            rp.setTests(testManager.getTests(testSuite));

            return rp;
        }

        /// <summary>
        /// Gets the results of an interview.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IReadOnlyList<com.sun.javatest.TestResult> GetResults(InterviewParameters parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            // wait until result is initialized
            var trt = parameters.getWorkDirectory().getTestResultTable();
            trt.waitUntilReady();

            // find tests
            var tests = parameters.getTests();
            if (tests == null)
                return trt.getIterator().RemainingToList<com.sun.javatest.TestResult>();
            else if (tests.Length == 0)
                return Array.Empty<com.sun.javatest.TestResult>();
            else
                return trt.getIterator(tests).RemainingToList<com.sun.javatest.TestResult>();
        }

        /// <summary>
        /// Gets the fully qualified name for the given test.
        /// </summary>
        /// <param name="testResult"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        static string GetFullyQualifiedName(com.sun.javatest.TestResult testResult)
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

        public bool ShouldAttachToTestHost(IEnumerable<string> sources, IRunContext runContext)
        {
            return true;
        }

        public bool ShouldAttachToTestHost(IEnumerable<TestCase> tests, IRunContext runContext)
        {
            return true;
        }

    }

}
