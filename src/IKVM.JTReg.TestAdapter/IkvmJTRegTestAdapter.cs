using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using sun.misc;

using static com.sun.tools.javac.util.Name;

namespace IKVM.JTReg.TestAdapter
{

    [FileExtension(".dll")]
    [FileExtension(".exe")]
    [DefaultExecutorUri("executor://IkvmJTRegTestAdapter/v1")]
    [ExtensionUri("executor://IkvmJTRegTestAdapter/v1")]
    public class IkvmJTRegTestAdapter : ITestDiscoverer, ITestExecutor2
    {

        internal static readonly java.net.URLClassLoader ClassLoader = new java.net.URLClassLoader(Directory.GetFiles(Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "lib"), "*.jar").Select(i => new java.io.File(i).toURI().toURL()).ToArray());
        internal static readonly MD5 MD5 = MD5.Create();

        static readonly string[] DEFAULT_UNIX_ENV_VARS = {
            "PATH",
            "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG",
            "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS"
        };

        static readonly string[] DEFAULT_WINDOWS_ENV_VARS = {
            "PATH",
            "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ"
        };

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmJTRegTestAdapter()
        {
            // need to do some static configuration on the harness
            var jarnessClazz = java.lang.Class.forName("com.sun.javatest.Harness", true, ClassLoader);
            var productClazz = java.lang.Class.forName("com.sun.javatest.ProductInfo", true, ClassLoader);
            if (jarnessClazz.getMethod("getClassDir").invoke(null) == null)
                jarnessClazz.getMethod("setClassDir", typeof(java.io.File)).invoke(null, productClazz.getMethod("getJavaTestClassDir").invoke(null));
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
            try
            {
                logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Scanning for test roots for '{source}'.");

                // discover all root test directories
                var testDirs = new java.util.ArrayList();
                foreach (var rootFile in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
                {
                    logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Found test root file: {rootFile}");
                    testDirs.add(new java.io.File(Path.GetDirectoryName(rootFile)));
                }

                // output path for jtreg state
                var id = GetSourceHash(source);
                var runDir = Path.Combine(Path.GetTempPath(), "ikvm-jtreg-" + id);

                try
                {
                    // attempt to create a temporary directory as scratch space for this test
                    logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Using run directory: {runDir}");
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
                    foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                    {
                        logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered test suite: {testSuite.getName()}");

                        foreach (var testResult in GetResults(CreateParameters(testManager, testSuite)))
                        {
                            testCount++;
                            var testCase = (TestCase)TestResultMethods.ToTestCase(source, testResult);
                            logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered test: {testCase.FullyQualifiedName}");
                            discoverySink.SendTestCase(testCase);
                        }
                    }

                    testWatch.Stop();
                    logger.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Discovered {testCount} tests for '{source}' in {testWatch.Elapsed.TotalSeconds} seconds.");
                }
                finally
                {
                    //if (Directory.Exists(runDir))
                    //{
                    //    try
                    //    {
                    //        Directory.Delete(runDir, true);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        logger.SendMessage(TestMessageLevel.Error, "JTReg: " + $"An exception occurred clearing run directory '{runDir}'.\n\n{e.Message}\n{e.StackTrace}");
                    //    }
                    //}
                }
            }
            catch (Exception e)
            {
                logger.SendMessage(TestMessageLevel.Error, "JTReg: " + $"An exception occurred discovering tests for '{source}'.\n\n{e.Message}\n{e.StackTrace}");
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
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // either add all tests, or tests specified
                var testFiles = new java.util.ArrayList();
                if (tests == null)
                {
                    tests = Array.Empty<TestCase>();
                    foreach (var rootFile in Directory.GetFiles(Path.GetDirectoryName(source), "TEST.ROOT", SearchOption.AllDirectories))
                    {
                        frameworkHandle.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Found test root file: {rootFile}");
                        testFiles.add(new java.io.File(Path.GetDirectoryName(rootFile)));
                    }
                }
                else
                    foreach (var testFile in tests.Select(i => i.CodeFilePath))
                        testFiles.add(new java.io.File(testFile));

                // output path for jtreg state
                var id = GetSourceHash(source);
                var runDir = Path.Combine(runContext.TestRunDirectory, "ikvm-jtreg-" + id);
                Directory.CreateDirectory(runDir);

                // setup output logs to rundir
                using var output = new java.io.PrintWriter(Path.Combine(runDir, "jtreg.out"));
                using var errors = new StreamWriter(File.OpenWrite(Path.Combine(runDir, "jtreg.log")));

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(runDir, output, errors);
                testManager.addTestFiles(testFiles, false);

                // invoke each test suite
                foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Running test suite: {testSuite.getName()}");
                    RunTests(source, runContext, frameworkHandle, tests, CreateParameters(testManager, testSuite), cancellationToken);
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
        internal void RunTests(string source, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, dynamic parameters, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // only continue if there are in fact tests in the suite to execute
            var testResults = ((IEnumerable<object>)GetResults(parameters)).ToList<dynamic>();
            if (testResults != null && testResults.Count > 0)
            {
                try
                {
                    // observe harness for test results
                    var h = (dynamic)java.lang.Class.forName("com.sun.javatest.Harness", true, ClassLoader).newInstance();
                    var o = HarnessObserverInterceptor.Create(new HarnessObserverImplementation(source, runContext, frameworkHandle, tests));
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
                    frameworkHandle.SendMessage(TestMessageLevel.Error, "JTReg: " + e.ToString());
                }
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
        dynamic CreateTestManager(string runDir, java.io.PrintWriter output, TextWriter errors)
        {
            var tmclazz = java.lang.Class.forName("com.sun.javatest.regtest.config.TestManager", true, ClassLoader);
            var ehclazz = java.lang.Class.forName("com.sun.javatest.TestFinder$ErrorHandler", true, ClassLoader);
            var eh = ErrorHandlerInterceptor.Create(new ErrorHandlerImplementation(errors));
            var tm = tmclazz.getConstructor(typeof(java.io.PrintWriter), typeof(java.io.File), ehclazz).newInstance(output, new java.io.File(Environment.CurrentDirectory), eh);

            var wd = Path.Combine(runDir, "JTwork");
            Directory.CreateDirectory(wd);
            tm.setWorkDirectory(new java.io.File(wd));

            var rd = Path.Combine(runDir, "JTreport");
            Directory.CreateDirectory(rd);
            tm.setReportDirectory(new java.io.File(rd));

            return tm;
        }

        /// <summary>
        /// Creates the parameters for a given suite.
        /// </summary>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        dynamic CreateParameters(dynamic testManager, dynamic testSuite)
        {
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // invoke new RegressionParameters(string, RegressionTestSuite)
            var rpclazz = java.lang.Class.forName("com.sun.javatest.regtest.config.RegressionParameters", true, ClassLoader);
            var tsclazz = java.lang.Class.forName("com.sun.javatest.regtest.config.RegressionTestSuite", true, ClassLoader);
            var rp = rpclazz.getConstructor(typeof(string), tsclazz).newInstance("regtest", testSuite);

            // configure work directory
            var wd = testManager.getWorkDirectory(testSuite);
            rp.setWorkDirectory(wd);

            var rd = testManager.getReportDirectory(testSuite);
            rp.setReportDir(rd);

            // invoke JDK.of(File)
            var jdkclazz = java.lang.Class.forName("com.sun.javatest.regtest.config.JDK", true, ClassLoader);
            var jdkofmethod = jdkclazz.getMethod("of", typeof(java.io.File));
            var jdk = (dynamic)jdkofmethod.invoke(null, new java.io.File(Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location)), "ikvm"));
            var execmodeclazz = java.lang.Class.forName("com.sun.javatest.regtest.config.ExecMode", true, ClassLoader);

            rp.setTests((java.util.Set)testManager.getTests(testSuite));
            rp.setExecMode(testSuite.getDefaultExecMode() ?? java.lang.Enum.valueOf(execmodeclazz, "OTHERVM"));
            rp.setCheck(false);
            rp.setCompileJDK(jdk);
            rp.setTestCompilerOptions(java.util.Collections.singletonList("-verbose"));
            rp.setTestJDK(jdk);
            rp.setFile((java.io.File)wd.getFile("config.jti"));
            rp.setEnvVars(GetEnvVars());
            rp.setConcurrency(Environment.ProcessorCount);
            rp.setTimeLimit(15000);
            rp.setRetainArgs(java.util.Collections.singletonList("all"));
            rp.setExcludeLists(new java.io.File[0]);
            rp.setMatchLists(new java.io.File[0]);
            rp.setPriorStatusValues(null);
            rp.setUseWindowsSubsystemForLinux(false);
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
            var os = (dynamic)java.lang.Class.forName("com.sun.javatest.regtest.config.OS", true, ClassLoader).getMethod("current").invoke(null);
            foreach (var var in ((string)os.family) == "windows" ? DEFAULT_WINDOWS_ENV_VARS : DEFAULT_UNIX_ENV_VARS)
                if (Environment.GetEnvironmentVariable(var) is string val)
                    envVars.put(var, val);

            // import any variables prefixed with JTREG, as done by the existing tool
            foreach (DictionaryEntry entry in Environment.GetEnvironmentVariables())
                if (((string)entry.Key).StartsWith("JTREG_"))
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
            var trclazz = java.lang.Class.forName("com.sun.javatest.TestResult", true, ClassLoader);
            var trtype = ikvm.runtime.Util.getInstanceTypeFromClass(trclazz);
            var rtltrmethod = typeof(java.util.IteratorExtensions).GetMethod(nameof(java.util.IteratorExtensions.RemainingToList)).MakeGenericMethod(trtype);

            // wait until result is initialized
            var trt = parameters.getWorkDirectory().getTestResultTable();
            trt.waitUntilReady();

            // find tests
            var tests = parameters.getTests();
            if (tests == null)
                return (IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() });
            else if (tests.Length == 0)
                return (IEnumerable<dynamic>)Array.CreateInstance(ikvm.runtime.Util.getInstanceTypeFromClass(trclazz), 0);
            else
                return (IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() });
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
