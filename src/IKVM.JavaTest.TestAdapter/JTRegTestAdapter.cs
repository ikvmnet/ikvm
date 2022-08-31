using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using com.sun.javatest;
using com.sun.javatest.regtest.config;
using com.sun.javatest.regtest.report;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

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
        internal static void Initialize()
        {
            var cld = ((global::java.lang.Class)typeof(Harness)).getClassLoader();
            var cls = cld.getResource("com/sun/javatest/Harness.class").getFile();
            var dir = new global::java.io.File(cls).getParentFile().getParentFile().getParentFile().getParentFile();
            if (Harness.getClassDir() == null)
                Harness.setClassDir(dir);
        }

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
            // discover all root test directories
            var testDirs = new global::java.util.ArrayList();
            foreach (var rootDir in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
                testDirs.add(new global::java.io.File(Path.GetDirectoryName(rootDir)));

            // initialize the test manager with the discovered roots
            var testManager = CreateTestManager(null);
            testManager.addTestFiles(testDirs, false);

            // for each suite, get the results and transform a test case
            foreach (RegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
            {
                for (var iter = GetResultsIter(CreateParameters(testManager, testSuite)); iter.hasNext();)
                {
                    var testResult = (com.sun.javatest.TestResult)iter.next();
                    var description = testResult.getDescription();
                    var name = GetFullyQualifiedName(testResult);

                    var testCase = new TestCase(name, new Uri("executor://JTRegAdapter/v1"), source);
                    testCase.CodeFilePath = description.getFile()?.toString();
                    discoverySink.SendTestCase(testCase);
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
        /// <exception cref="NotImplementedException"></exception>
        internal void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
        {
            foreach (var source in sources)
                RunTests(source, runContext, frameworkHandle, tests?.Where(i => i.Source == source));
        }

        /// <summary>
        /// Runs the tests in the given source, optionally filtered by specific test case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        internal void RunTests(string source, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
        {
            // either add all tests, or tests specified
            var testFiles = new global::java.util.ArrayList();
            if (tests == null)
                foreach (var rootDir in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
                    testFiles.add(new global::java.io.File(Path.GetDirectoryName(rootDir)));
            else
                foreach (var testFile in tests.Select(i => i.CodeFilePath))
                    testFiles.add(new global::java.io.File(testFile));

            // initialize the test manager with the discovered roots
            var testManager = CreateTestManager(Path.Combine(runContext.TestRunDirectory, "jtreg"));
            testManager.addTestFiles(testFiles, false);

            var testStats = new TestStats();
            foreach (RegressionTestSuite testSuite in (IEnumerable)testManager.getTestSuites())
                testStats.addAll(BatchHarness(CreateParameters(testManager, testSuite)));
        }

        internal TestStats BatchHarness(RegressionParameters parameters)
        {
            var stats = new TestStats();
            var h = new Harness();
            stats.register(h);

            var tests = parameters.getTests();
            var ok = (tests != null && tests.Length == 0) || h.batch(parameters);
            return stats;
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new <see cref="TestManager"/>.
        /// </summary>
        /// <param name="runDir"></param>
        /// <returns></returns>
        TestManager CreateTestManager(string runDir)
        {

            // default to temp output
            runDir ??= Path.Combine(runDir ?? Path.GetTempPath(), "jtreg");
            if (Directory.Exists(runDir))
                Directory.Delete(runDir, true);
            if (Directory.Exists(runDir) == false)
                Directory.CreateDirectory(runDir);

            // initialize new test manager
            using var pw = new global::java.io.PrintWriter(Path.Combine(runDir, "jtreg.out"));
            using var fw = new StreamWriter(File.OpenWrite(Path.Combine(runDir, "jtreg.log")));
            var tm = new TestManager(pw, new global::java.io.File(Environment.CurrentDirectory), new DelegateErrorHandler(s => fw.WriteLine(s)));
            tm.setReportDirectory(new global::java.io.File(Path.Combine(runDir, "report")));
            tm.setWorkDirectory(new global::java.io.File(Path.Combine(runDir, "work")));
            return tm;
        }

        /// <summary>
        /// Creates the parameters for a given suite.
        /// </summary>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        RegressionParameters CreateParameters(TestManager testManager, RegressionTestSuite testSuite)
        {
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            var rp = new RegressionParameters("regtest", testSuite, new DelegateConsumer<string>(s => Console.WriteLine(s)));

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
            rp.setExcludeLists(new global::java.io.File[0]);
            rp.setMatchLists(new global::java.io.File[0]);
            rp.setEnvVars(new global::java.util.HashMap());

            rp.setTests(testManager.getTests(testSuite));

            return rp;
        }

        /// <summary>
        /// Gets the results of an interview.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        global::java.util.Iterator GetResultsIter(InterviewParameters parameters)
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
