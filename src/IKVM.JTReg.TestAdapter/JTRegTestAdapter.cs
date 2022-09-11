using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using com.sun.javatest;
using com.sun.javatest.regtest.config;
using System.Threading;
using com.sun.javatest.regtest.report;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using com.sun.javadoc;
using IKVM.JTReg.TestAdapter;

namespace IKVM.JavaTest
{

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("executor://JTRegAdapter/v1")]
    [ExtensionUri("executor://JTRegAdapter/v1")]
    public class JTRegTestAdapter : ITestDiscoverer, ITestExecutor
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JTRegTestAdapter()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static void Initialize()
        {
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
            logger.SendMessage(TestMessageLevel.Informational, $"Using jtreg run directory: {runDir}");
            Directory.CreateDirectory(runDir);
            using var output = new java.io.PrintWriter(Path.Combine(runDir, "jtreg.out"));
            using var errors = new StreamWriter(File.OpenWrite(Path.Combine(runDir, "jtreg.log")));

            // initialize the test manager with the discovered roots
            var testManager = CreateTestManager(runDir, output, errors);
            testManager.addTestFiles(testDirs, false);

            int testCount = 0;

            // for each suite, get the results and transform a test case
            foreach (IkvmRegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
            {
                logger.SendMessage(TestMessageLevel.Informational, $"Discovered jtreg test suite: {testSuite.getName()}");

                for (var iter = GetResultsIter(CreateParameters(testManager, testSuite)); iter.hasNext();)
                {
                    var testResult = (com.sun.javatest.TestResult)iter.next();
                    var description = testResult.getDescription();
                    var name = GetFullyQualifiedName(testResult);

                    testCount++;
                    logger.SendMessage(TestMessageLevel.Informational, $"Discovered jtreg test: {name}");
                    var testCase = new TestCase(name, new Uri("executor://JTRegAdapter/v1"), source);
                    testCase.CodeFilePath = description.getFile()?.toString();
                    discoverySink.SendTestCase(testCase);
                }
            }

            logger.SendMessage(TestMessageLevel.Informational, $"Discovered {testCount} jtreg tests for '{source}'.");
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

            var testStats = new TestStats();
            foreach (RegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
                testStats.addAll(RunTestSuite(testManager, testSuite, cancellationToken));
        }

        internal TestStats RunTestSuite(TestManager testManager, RegressionTestSuite testSuite, CancellationToken cancellationToken)
        {
            return RunHarness(CreateParameters(testManager, testSuite), cancellationToken);
        }

        internal TestStats RunHarness(IkvmRegressionParameters parameters, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stats = new TestStats();
            var h = new Harness();
            stats.register(h);

            var tests = parameters.getTests();
            if (tests != null && tests.Length > 0)
            {
                try
                {
                    var ok = h.batch(parameters);
                    //h.start(parameters);
                    //cancellationToken.Register(() => h.stop());
                    //h.waitUntilDone();
                }
                catch (java.lang.InterruptedException)
                {
                    // ignore
                }
            }

            return stats;
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
        TestManager CreateTestManager(string runDir, java.io.PrintWriter output, TextWriter errors)
        {
            var tm = new TestManager(output, new java.io.File(Environment.CurrentDirectory), new DelegateErrorHandler(s => errors.WriteLine(s)));
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
        java.util.Iterator GetResultsIter(InterviewParameters parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            // wait until result is initialized
            var trt = parameters.getWorkDirectory().getTestResultTable();
            trt.waitUntilReady();

            // find tests
            var tests = parameters.getTests();
            if (tests == null)
                return trt.getIterator();
            else if (tests.Length == 0)
                return global::java.util.Collections.emptySet().iterator();
            else
                return trt.getIterator(tests);
        }

        /// <summary>
        /// Gets the fully qualified name for the given test.
        /// </summary>
        /// <param name="testResult"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        string GetFullyQualifiedName(com.sun.javatest.TestResult testResult)
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

    }

}
