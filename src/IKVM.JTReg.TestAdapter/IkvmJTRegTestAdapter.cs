using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("executor://IkvmJTRegTestAdapter/v1")]
    [ExtensionUri("executor://IkvmJTRegTestAdapter/v1")]
    public class IkvmJTRegTestAdapter : ITestDiscoverer, ITestExecutor2
    {

        const string BASEDIR_PREFIX = "ikvm-jtreg-";
        const string TEST_ROOT_FILE_NAME = "TEST.ROOT";
        const string TEST_PROBLEM_LIST_FILE_NAME = "ProblemList.txt";
        const string TEST_EXCLUDE_LIST_FILE_NAME = "ExcludeList.txt";
        const string TEST_INCLUDE_LIST_FILE_NAME = "IncludeList.txt";
        const string DEFAULT_OUT_FILE_NAME = "jtreg.out";
        const string DEFAULT_LOG_FILE_NAME = "jtreg.log";
        const string DEFAULT_WORK_DIR_NAME = "JTwork";
        const string DEFAULT_REPORT_DIR_NAME = "JTreport";
        const string DEFAULT_PARAM_TAG = "regtest";
        const string ENV_PREFIX = "JTREG_";

        static readonly MD5 MD5 = MD5.Create();
        static readonly string IKVM_JDK_DIR = GetIkvmJdkDir();

        /// <summary>
        /// Returns the appropriate IKVM JDK directory based on the platform.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        static string GetIkvmJdkDir()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "ikvm", "win7-x64");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "ikvm", "linux-x64");

            throw new PlatformNotSupportedException("No IKVM JDK installation for platform.");
        }

        static readonly string[] DEFAULT_WINDOWS_ENV_VARS = { "PATH", "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ" };
        static readonly string[] DEFAULT_UNIX_ENV_VARS = { "PATH", "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG", "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS" };

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmJTRegTestAdapter()
        {
            // need to do some static configuration on the harness
            if (JTRegTypes.Harness.GetClassDirMethod.invoke(null) == null)
                JTRegTypes.Harness.SetClassDirMethod.invoke(null, JTRegTypes.ProductInfo.GetJavaTestClassDirMethod.invoke(null));
        }

        /// <summary>
        /// Creates a short hash of the given string to uniquely identify it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static string GetSourceHash(string source)
        {
            var b = MD5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var s = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                s.Append(b[i].ToString("x2"));
            return s.ToString();
        }

        readonly Dictionary<string, TestProperty> properties = new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase);
        CancellationTokenSource cts;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmJTRegTestAdapter()
        {
            properties.Add(IkvmJTRegTestProperties.TestSuiteRootProperty.Label, IkvmJTRegTestProperties.TestSuiteRootProperty);
            properties.Add(IkvmJTRegTestProperties.TestSuiteNameProperty.Label, IkvmJTRegTestProperties.TestSuiteNameProperty);
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
            if (sources is null)
                throw new ArgumentNullException(nameof(sources));
            if (discoveryContext is null)
                throw new ArgumentNullException(nameof(discoveryContext));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (discoverySink is null)
                throw new ArgumentNullException(nameof(discoverySink));

            try
            {
                foreach (var source in sources)
                    DiscoverTests(source, discoveryContext, logger, discoverySink);
            }
            catch (Exception e)
            {
                logger.SendMessage(TestMessageLevel.Error, "JTReg: " + $"An exception occurred discovering tests.\n\n{e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// Discovers the available tests.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        internal void DiscoverTests(string source, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (discoveryContext is null)
                throw new ArgumentNullException(nameof(discoveryContext));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (discoverySink is null)
                throw new ArgumentNullException(nameof(discoverySink));

            try
            {
                logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Scanning for test roots for '{source}'.");

                // discover all root test directories
                var testDirs = new java.util.ArrayList();
                foreach (var rootFile in Directory.GetFiles(Path.GetDirectoryName(source), TEST_ROOT_FILE_NAME, SearchOption.AllDirectories))
                {
                    logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Found test root file: {rootFile}");
                    testDirs.add(new java.io.File(Path.GetDirectoryName(rootFile)));
                }

                // output path for jtreg state
                var id = GetSourceHash(source);
                var baseDir = Path.Combine(Path.GetTempPath(), BASEDIR_PREFIX + id);

                // attempt to create a temporary directory as scratch space for this test
                logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Using run directory: {baseDir}");
                Directory.CreateDirectory(baseDir);

                using var output = new java.io.PrintWriter(Path.Combine(baseDir, DEFAULT_OUT_FILE_NAME));
                using var errors = new StreamWriter(File.OpenWrite(Path.Combine(baseDir, DEFAULT_LOG_FILE_NAME)));

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(logger, baseDir, output, errors);
                testManager.addTestFiles(testDirs, false);

                // track metrics related to tests
                int testCount = 0;
                var testWatch = new Stopwatch();
                testWatch.Start();

                // for each suite, get the results and transform a test case
                foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                {
                    logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered test suite: {testSuite.getName()}");

                    foreach (var testResult in GetResults(CreateParameters(source, baseDir, testManager, testSuite, null)))
                    {
                        testCount++;
                        var testCase = (TestCase)TestResultMethods.ToTestCase(source, testSuite, testResult);
                        logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered test: {testCase.FullyQualifiedName}");
                        discoverySink.SendTestCase(testCase);
                    }
                }

                testWatch.Stop();
                logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered {testCount} tests for '{source}' in {testWatch.Elapsed.TotalSeconds} seconds.");
            }
            catch (Exception e)
            {
                logger.SendMessage(TestMessageLevel.Error, "JTReg: " + $"An exception occurred discovering tests for '{source}'.\n\n{e.Message}\n{e.StackTrace}");
            }
        }

        public bool ShouldAttachToTestHost(IEnumerable<string> sources, IRunContext runContext)
        {
            return true;
        }

        public bool ShouldAttachToTestHost(IEnumerable<TestCase> tests, IRunContext runContext)
        {
            return true;
        }

        /// <summary>
        /// Runs the given test cases.
        /// </summary>
        /// <param name="tests"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            if (tests is null)
                throw new ArgumentNullException(nameof(tests));
            if (runContext is null)
                throw new ArgumentNullException(nameof(runContext));
            if (frameworkHandle is null)
                throw new ArgumentNullException(nameof(frameworkHandle));

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
            if (sources is null)
                throw new ArgumentNullException(nameof(sources));
            if (runContext is null)
                throw new ArgumentNullException(nameof(runContext));
            if (frameworkHandle is null)
                throw new ArgumentNullException(nameof(frameworkHandle));

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
        internal void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests = null)
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
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, "JTReg: " + $"Exception occurred running tests.\n\n{e.Message}\n{e.StackTrace}");
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
        /// <param name="cancellationToken"></param>
        internal void RunTestForSource(string source, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (runContext is null)
                throw new ArgumentNullException(nameof(runContext));
            if (frameworkHandle is null)
                throw new ArgumentNullException(nameof(frameworkHandle));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // discover all root test directories
                var testDirs = new java.util.ArrayList();
                foreach (var rootFile in Directory.GetFiles(Path.GetDirectoryName(source), TEST_ROOT_FILE_NAME, SearchOption.AllDirectories))
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Found test root file: {rootFile}");
                    testDirs.add(new java.io.File(Path.GetDirectoryName(rootFile)));
                }

                // output path for jtreg state
                var id = GetSourceHash(source);
                var baseDir = Path.Combine(Path.GetTempPath(), BASEDIR_PREFIX + id);
                Directory.CreateDirectory(baseDir);

                // setup output logs to rundir
                using var output = new java.io.PrintWriter(Path.Combine(baseDir, DEFAULT_OUT_FILE_NAME));
                using var errors = new StreamWriter(File.OpenWrite(Path.Combine(baseDir, DEFAULT_LOG_FILE_NAME)));

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(frameworkHandle, baseDir, output, errors);
                testManager.addTestFiles(testDirs, false);

                // we will need a full list of tests to apply any filters to
                if (tests == null)
                {
                    var l = new List<TestCase>();

                    // discover the full set of tests
                    foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                        foreach (var testResult in GetResults(CreateParameters(source, baseDir, testManager, testSuite, null)))
                            l.Add(TestResultMethods.ToTestCase(source, testSuite, testResult));

                    tests = l;
                }

                // filter the tests
                var filter = runContext.GetTestCaseFilter(properties.Keys, s => properties.TryGetValue(s, out var v) ? v : null);
                if (filter != null)
                    tests = tests.Where(i => filter.MatchTestCase(i, s => properties.TryGetValue(s, out var v) ? i.GetPropertyValue(v) : null));

                // invoke each test suite
                foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Running test suite: {testSuite.getName()}");
                    RunTests(source, testSuite, runContext, frameworkHandle, tests, CreateParameters(source, baseDir, testManager, testSuite, tests), cancellationToken);
                }
            }
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, "JTReg: " + $"Exception occurred running tests for '{source}'.\n\n{e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary>
        /// Executes the tests for the given parameters.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTests(string source, dynamic testSuite, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, dynamic parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (runContext is null)
                throw new ArgumentNullException(nameof(runContext));
            if (frameworkHandle is null)
                throw new ArgumentNullException(nameof(frameworkHandle));
            if (tests is null)
                throw new ArgumentNullException(nameof(tests));
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            cancellationToken.ThrowIfCancellationRequested();

            // only continue if there are in fact tests in the suite to execute
            var firstTestResult = ((IEnumerable<object>)GetResults(parameters)).FirstOrDefault();
            if (firstTestResult is null)
                return;

            try
            {
                // observe harness for test results
                var harness = JTRegTypes.Harness.New();
                var observer = HarnessObserverInterceptor.Create(new HarnessObserverImplementation(source, testSuite, runContext, frameworkHandle, tests));
                harness.addObserver(observer);

                // begin harness execution asynchronously, thus allowing potential cancellation
                harness.start(parameters);
                cancellationToken.Register(() => harness.stop());

                // wait until canceled or finished and cleanup
                harness.waitUntilDone();
                harness.stop();
                harness.removeObserver(observer);
            }
            catch (java.lang.InterruptedException)
            {
                // ignore
            }
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, "JTReg: " + e.ToString());
            }
        }

        /// <summary>
        /// Initiates a cancellation of the test discovery or execution.
        /// </summary>
        public void Cancel()
        {
            cts?.Cancel();
            cts = null;
        }

        /// <summary>
        /// Creates a new TestManager.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="baseDir"></param>
        /// <param name="output"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        dynamic CreateTestManager(IMessageLogger logger, string baseDir, java.io.PrintWriter output, TextWriter errors)
        {
            if (string.IsNullOrEmpty(baseDir))
                throw new ArgumentException($"'{nameof(baseDir)}' cannot be null or empty.", nameof(baseDir));
            if (output is null)
                throw new ArgumentNullException(nameof(output));
            if (errors is null)
                throw new ArgumentNullException(nameof(errors));

            var errorHandler = ErrorHandlerInterceptor.Create(new ErrorHandlerImplementation(errors));
            var testManager = JTRegTypes.TestManager.New(output, new java.io.File(Environment.CurrentDirectory), errorHandler);

            var workDirectory = Path.Combine(baseDir, DEFAULT_WORK_DIR_NAME);
            logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Using work directory: '{workDirectory}'.");
            Directory.CreateDirectory(workDirectory);
            testManager.setWorkDirectory(new java.io.File(workDirectory));

            var reportDirectory = Path.Combine(baseDir, DEFAULT_REPORT_DIR_NAME);
            logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Using report directory: '{reportDirectory}'.");
            Directory.CreateDirectory(reportDirectory);
            testManager.setReportDirectory(new java.io.File(reportDirectory));
            return testManager;
        }

        /// <summary>
        /// Creates the parameters for a given suite.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="baseDir"></param>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <param name="tests"></param>
        /// <returns></returns>
        dynamic CreateParameters(string source, string baseDir, dynamic testManager, dynamic testSuite, IEnumerable<TestCase> tests)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (baseDir is null)
                throw new ArgumentNullException(nameof(baseDir));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // invoke new RegressionParameters(string, RegressionTestSuite)
            var rp = JTRegTypes.RegressionParameters.New(DEFAULT_PARAM_TAG, testSuite);

            // configure work directory
            var wd = testManager.getWorkDirectory(testSuite);
            rp.setWorkDirectory(wd);

            var rd = testManager.getReportDirectory(testSuite);
            rp.setReportDir(rd);

            // if a ProblemList.txt or ExcludeList.txt file exists in the root, add them as exclude files
            var excludeFileList = new List<java.io.File>();
            foreach (var n in new[] { TEST_PROBLEM_LIST_FILE_NAME, TEST_EXCLUDE_LIST_FILE_NAME })
                if (Path.Combine(((java.io.File)testSuite.getRootDir()).toString(), n) is string f && File.Exists(f))
                    excludeFileList.Add(new java.io.File(new java.io.File(f).getAbsoluteFile().toURI().normalize()));

            // if a IncludeList.txt file exists in the root, add it as include files
            var includeFileList = new List<java.io.File>();
            foreach (var n in new[] { TEST_INCLUDE_LIST_FILE_NAME })
                if (Path.Combine(((java.io.File)testSuite.getRootDir()).toString(), n) is string f && File.Exists(f))
                    includeFileList.Add(new java.io.File(new java.io.File(f).getAbsoluteFile().toURI().normalize()));

            // passed in an explicit set of tests, add to an include file
            if (tests != null)
            {
                // name of the current suite
                var testSuiteName = TestResultMethods.GetTestSuiteName(source, testSuite);

                // fill in include list containing tests located within the suite
                var tf = Path.Combine(baseDir, testSuiteName + "-IncludeList.txt");
                using (var ts = File.CreateText(tf))
                    foreach (var test in tests)
                        if (string.Equals(test.GetPropertyValue(IkvmJTRegTestProperties.TestSuiteNameProperty), testSuiteName))
                            ts.WriteLine(test.GetPropertyValue(IkvmJTRegTestProperties.TestPathNameProperty) + " generic-all");

                includeFileList.Add(new java.io.File(new java.io.File(tf).getAbsoluteFile().toURI().normalize()));
            }

            rp.setTests((java.util.Set)testManager.getTests(testSuite));
            rp.setExecMode(testSuite.getDefaultExecMode() ?? JTRegTypes.ExecMode.OTHERVM);
            rp.setCheck(false);
            rp.setCompileJDK(JTRegTypes.JDK.Of(new java.io.File(IKVM_JDK_DIR)));
            rp.setTestJDK(rp.getCompileJDK());
            rp.setTestCompilerOptions(java.util.Collections.emptyList());
            rp.setFile((java.io.File)wd.getFile("config.jti"));
            rp.setEnvVars(GetEnvVars());
            rp.setConcurrency(Environment.ProcessorCount);
            rp.setTimeLimit(15000);
            rp.setRetainArgs(java.util.Collections.singletonList("all"));
            rp.setExcludeLists(excludeFileList.ToArray());
            rp.setMatchLists(includeFileList.ToArray());
            rp.setIgnoreKind(JTRegTypes.IgnoreKind.QUIET);
            rp.setPriorStatusValues(null);
            rp.setUseWindowsSubsystemForLinux(true);

            // configure keywords to filter based on
            string keywordsExpr = null;
            keywordsExpr = combineKeywords(keywordsExpr, "!ignore");
            keywordsExpr = combineKeywords(keywordsExpr, "!manual");
            if (string.IsNullOrWhiteSpace(keywordsExpr) == false)
                rp.setKeywordsExpr(keywordsExpr);

            // locate and configure TestNG
            var testNGPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "lib", "testng.jar");
            rp.setTestNGPath(JTRegTypes.SearchPath.New(testNGPath));

            // locate and configure JUnit
            var junitPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "lib", "junit.jar");
            rp.setJUnitPath(JTRegTypes.SearchPath.New(junitPath));

            // locate and configure JUnit
            var asmtoolsPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "lib", "asmtools.jar");
            rp.setAsmToolsPath(JTRegTypes.SearchPath.New(asmtoolsPath));

            // final initialization
            rp.initExprContext();
            if (rp.isValid() == false)
                throw new Exception();

            return rp;
        }

        /// <summary>
        /// Gets the set of environment variables to include with the tests by default.
        /// </summary>
        /// <returns></returns>
        java.util.Map GetEnvVars()
        {
            var envVars = new java.util.TreeMap();

            // import existing variables based on the current OS
            foreach (var var in ((string)JTRegTypes.OS.Current().family) == "windows" ? DEFAULT_WINDOWS_ENV_VARS : DEFAULT_UNIX_ENV_VARS)
                if (Environment.GetEnvironmentVariable(var) is string val)
                    envVars.put(var, val);

            // import any variables prefixed with JTREG, as done by the existing tool
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                if (((string)entry.Key).StartsWith(ENV_PREFIX))
                    envVars.put((string)entry.Key, (string)entry.Value);

            return envVars;
        }

        /// <summary>
        /// Gets the results of an interview.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IEnumerable<dynamic> GetResults(dynamic parameters)
        {
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            // resolve RemainingToList generic method
            var trtype = ikvm.runtime.Util.getInstanceTypeFromClass(JTRegTypes.TestResult.Class);
            var rtltrmethod = typeof(java.util.IteratorExtensions).GetMethod(nameof(java.util.IteratorExtensions.RemainingToList)).MakeGenericMethod(trtype);

            // wait until result is initialized
            var trt = parameters.getWorkDirectory().getTestResultTable();
            trt.waitUntilReady();

            // find tests
            var tests = parameters.getTests();
            if (tests == null)
                return (IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() });
            else if (tests.Length == 0)
                return (IEnumerable<dynamic>)Array.CreateInstance(ikvm.runtime.Util.getInstanceTypeFromClass(JTRegTypes.TestResult.Class), 0);
            else
                return (IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() });
        }

        /// <summary>
        /// Combines the two keywords.
        /// </summary>
        /// <param name="kw1"></param>
        /// <param name="kw2"></param>
        /// <returns></returns>
        string combineKeywords(string kw1, string kw2)
        {
            return kw1 == null ? kw2 : kw1 + " & " + kw2;
        }

    }

}
