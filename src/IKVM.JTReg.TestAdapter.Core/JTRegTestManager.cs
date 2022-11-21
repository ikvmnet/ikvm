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

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Manages access to JTReg tests.
    /// </summary>
    public class JTRegTestManager
    {

        public const string URI = "executor://ikvmjtregtestadapter/v1";

        internal static readonly string JTREG_LIB = Path.Combine(Path.GetDirectoryName(typeof(JTRegTestManager).Assembly.Location), "jtreg");

        internal const string BASEDIR_PREFIX = "ikvm-jtreg-";
        internal const string TEST_ROOT_FILE_NAME = "TEST.ROOT";
        internal const string TEST_PROBLEM_LIST_FILE_NAME = "ProblemList.txt";
        internal const string TEST_EXCLUDE_LIST_FILE_NAME = "ExcludeList.txt";
        internal const string TEST_INCLUDE_LIST_FILE_NAME = "IncludeList.txt";
        internal const string DEFAULT_WORK_DIR_NAME = "work";
        internal const string DEFAULT_REPORT_DIR_NAME = "report";
        internal const string DEFAULT_PARAM_TAG = "regtest";
        internal const string ENV_PREFIX = "JTREG_";

        protected static readonly MD5 MD5 = MD5.Create();

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JTRegTestManager()
        {
#if NETCOREAPP
            // executable permissions may not have made it onto the JRE binaries so attempt to set them
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var javaHome = java.lang.System.getProperty("java.home");
                foreach (var exec in new[] { "java", "javac", "jar", "jarsigner", "javadoc", "javah", "javap", "jdeps", "keytool", "native2ascii", "policytool", "rmic", "wsgen", "wsimport" })
                {
                    var execPath = Path.Combine(javaHome, "bin", exec);
                    if (File.Exists(execPath))
                    {
                        var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(execPath);
                        var prm = psx.FileAccessPermissions;
                        prm |= Mono.Unix.FileAccessPermissions.UserExecute;
                        prm |= Mono.Unix.FileAccessPermissions.GroupExecute;
                        prm |= Mono.Unix.FileAccessPermissions.OtherExecute;
                        if (prm != psx.FileAccessPermissions)
                            psx.FileAccessPermissions = prm;
                    }
                }
            }
#endif

            // need to do some static configuration on the harness
            if (JTRegTypes.Harness.GetClassDirMethod.invoke(null) == null)
                JTRegTypes.Harness.SetClassDirMethod.invoke(null, JTRegTypes.ProductInfo.GetJavaTestClassDirMethod.invoke(null));
        }

        /// <summary>
        /// Creates a short hash of the given string to uniquely identify it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static string GetSourceHash(string source)
        {
            var b = MD5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var s = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
                s.Append(b[i].ToString("x2"));
            return s.ToString();
        }


        /// <summary>
        /// Creates a new TestManager.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="baseDir"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        protected dynamic CreateTestManager(IJTRegLoggerContext logger, string baseDir, java.io.PrintWriter output)
        {
            if (string.IsNullOrEmpty(baseDir))
                throw new ArgumentException($"'{nameof(baseDir)}' cannot be null or empty.", nameof(baseDir));
            if (output is null)
                throw new ArgumentNullException(nameof(output));

            var errorHandler = ErrorHandlerInterceptor.Create(new ErrorHandlerImplementation(logger));
            var testManager = JTRegTypes.TestManager.New(output, new java.io.File(Environment.CurrentDirectory), errorHandler);

            var workDirectory = Path.Combine(baseDir, DEFAULT_WORK_DIR_NAME);
            logger.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Using work directory: '{workDirectory}'.");
            Directory.CreateDirectory(workDirectory);
            testManager.setWorkDirectory(new java.io.File(workDirectory));

            var reportDirectory = Path.Combine(baseDir, DEFAULT_REPORT_DIR_NAME);
            logger.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Using report directory: '{reportDirectory}'.");
            Directory.CreateDirectory(reportDirectory);
            testManager.setReportDirectory(new java.io.File(reportDirectory));
            return testManager;
        }

        /// <summary>
        /// Gets the set of files that represent the exclusions for a suite.
        /// </summary>
        /// <param name="testSuite"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public List<java.io.File> GetExcludeListFiles(dynamic testSuite)
        {
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // if a ProblemList.txt or ExcludeList.txt file exists in the root, add them as exclude files
            var excludeFileList = new List<java.io.File>();
            foreach (var n in new[] { TEST_PROBLEM_LIST_FILE_NAME, TEST_EXCLUDE_LIST_FILE_NAME })
                if (Path.Combine(((java.io.File)testSuite.getRootDir()).toString(), n) is string f && File.Exists(f))
                    excludeFileList.Add(new java.io.File(new java.io.File(f).getAbsoluteFile().toURI().normalize()));

            return excludeFileList;
        }

        /// <summary>
        /// Gets the set of files that represent the inclusions for a suite.
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<java.io.File> GetIncludeListFiles(dynamic testSuite)
        {
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // if a IncludeList.txt file exists in the root, add it as include files
            var includeFileList = new List<java.io.File>();
            foreach (var n in new[] { TEST_INCLUDE_LIST_FILE_NAME })
                if (Path.Combine(((java.io.File)testSuite.getRootDir()).toString(), n) is string f && File.Exists(f))
                    includeFileList.Add(new java.io.File(new java.io.File(f).getAbsoluteFile().toURI().normalize()));

            return includeFileList;
        }

        /// <summary>
        /// Discovers the available tests.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        public void DiscoverTests(string source, IJTRegDiscoveryContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                // discover test suites from test assembly
                var testDirs = new java.util.ArrayList();
                foreach (var testRoot in Util.GetTestSuiteDirectories(source, context))
                    testDirs.add(new java.io.File(testRoot));
                if (testDirs.size() == 0)
                    return;

                // output path for jtreg state
                var id = GetSourceHash(source);
                var baseDir = Path.Combine(Path.GetTempPath(), BASEDIR_PREFIX + id);

                // attempt to create a temporary directory as scratch space for this test
                context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Using run directory: {baseDir}");
                Directory.CreateDirectory(baseDir);

                // output to framework
                using var output = new MessageLoggerWriter(context, JTRegTestMessageLevel.Informational);

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(context, baseDir, new java.io.PrintWriter(output));
                testManager.addTestFiles(testDirs, false);

                // track metrics related to tests
                int testCount = 0;
                var testWatch = new Stopwatch();
                testWatch.Start();

                // for each suite, get the results and transform a test case
                foreach (dynamic testSuite in Util.GetTestSuites(source, testManager))
                {
                    foreach (var testCase in (IEnumerable<JTRegTestCase>)Util.GetTestCases(source, testManager, testSuite))
                    {
                        context.SendTestCase(testCase);
                        testCount++;
                    }
                }

                testWatch.Stop();
                context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Discovered {testCount} tests for '{source}' in {testWatch.Elapsed.TotalSeconds} seconds.");
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: An exception occurred discovering tests for '{source}'.\n{e}");
            }
        }

        protected static readonly string[] DEFAULT_WINDOWS_ENV_VARS = { "PATH", "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ" };
        protected static readonly string[] DEFAULT_UNIX_ENV_VARS = { "PATH", "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG", "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS" };

        /// <summary>
        /// Runs the specified test cases, if provided, in the sources.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tests"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        public void RunTests(string source, List<JTRegTestCase> tests, IJTRegExecutionContext context, CancellationToken cancellationToken)
        {
            try
            {
                IkvmTraceServer debug = null;

                try
                {
                    // if we have the capability of attaching to child process, start debug server to listen for child processes
                    if (context.CanAttachDebuggerToProcess && Debugger.IsAttached)
                    {
                        debug = new IkvmTraceServer(context);
                        debug.Start();
                    }

                    RunTestForSource(source, context, tests?.Where(i => i.Source == source).ToList(), debug?.Uri, cancellationToken);
                }
                finally
                {
                    if (debug != null)
                    {
                        debug.Stop();
                        debug = null;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: Exception occurred running tests.\n{e}");
            }
        }

        /// <summary>
        /// Runs the tests in the given source, optionally filtered by specific test case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <param name="tests"></param>
        /// <param name="debugUri"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTestForSource(string source, IJTRegExecutionContext context, List<JTRegTestCase> tests, Uri debugUri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // discover test suites from test assembly
                var testDirs = new java.util.ArrayList();
                foreach (var testRoot in Util.GetTestSuiteDirectories(source, context))
                    testDirs.add(new java.io.File(testRoot));
                if (testDirs.size() == 0)
                    return;

                // output path for jtreg state
                var id = GetSourceHash(source);
                var baseDir = Path.Combine(context.TestRunDirectory, BASEDIR_PREFIX + id);
                Directory.CreateDirectory(baseDir);

                // output to framework
                using var output = new MessageLoggerWriter(context, JTRegTestMessageLevel.Informational);

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(context, baseDir, new java.io.PrintWriter(output));
                testManager.addTestFiles(testDirs, false);

                // load the set of suites
                var testSuites = Util.GetTestSuites(source, testManager).ToList();

                // we will need a full list of tests to apply any filters to
                if (tests == null)
                {
                    var l = new List<JTRegTestCase>(512);
                    
                    // discover the full set of tests
                    foreach (dynamic testSuite in testSuites)
                        foreach (var testCase in (IEnumerable<JTRegTestCase>)Util.GetTestCases(source, testManager, testSuite))
                            l.Add(testCase);

                    tests = l;
                }

                // filter the tests
                tests = context.FilterTestCases(tests);

                // invoke each test suite
                foreach (dynamic testSuite in testSuites)
                {
                    context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Running test suite: {(string)testSuite.getName()}");
                    RunTests(source, testManager, testSuite, context, tests, output, CreateParameters(source, testManager, testSuite, tests, debugUri), cancellationToken);
                }
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
            }
        }

        /// <summary>
        /// Creates the parameters for a given suite.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <param name="tests"></param>
        /// <param name="debugUri"></param>
        /// <returns></returns>
        dynamic CreateParameters(string source, dynamic testManager, dynamic testSuite, IEnumerable<JTRegTestCase> tests, Uri debugUri)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // invoke new RegressionParameters(string, RegressionTestSuite)
            var rp = JTRegTypes.RegressionParameters.New(DEFAULT_PARAM_TAG, testSuite);

            // configure work directory
            var wd = testManager.getWorkDirectory(testSuite);
            rp.setWorkDirectory(wd);

            // configure report directory
            var rd = testManager.getReportDirectory(testSuite);
            rp.setReportDir(rd);

            // if a ProblemList.txt or ExcludeList.txt file exists in the root, add them as exclude files
            var excludeFileList = (List<java.io.File>)GetExcludeListFiles(testSuite);

            // if a IncludeList.txt file exists in the root, add it as include files
            var includeFileList = (List<java.io.File>)GetIncludeListFiles(testSuite);

            // explicit tests specified, replace include list
            if (tests != null)
            {
                includeFileList.Clear();

                // name of the current suite
                var testSuiteName = Util.GetTestSuiteName(source, testSuite);

                // fill in include list containing tests located within the suite
                var includeListFile = (java.io.File)wd.getFile("IncludeList.txt");
                using (var includeList = File.CreateText(includeListFile.toString()))
                    foreach (var test in tests)
                        if (string.Equals(test.TestSuiteName, testSuiteName))
                            includeList.WriteLine(test.TestPathName + " generic-all");

                includeFileList.Add(new java.io.File(includeListFile.getAbsoluteFile().toURI().normalize()));
            }

            rp.setTests((java.util.Set)testManager.getTests(testSuite));
            rp.setExecMode(testSuite.getDefaultExecMode() ?? JTRegTypes.ExecMode.AGENTVM);
            rp.setCheck(false);
            rp.setCompileJDK(JTRegTypes.JDK.Of(new java.io.File(java.lang.System.getProperty("java.home"))));
            rp.setTestJDK(rp.getCompileJDK());
            rp.setTestVMOptions(java.util.Collections.emptyList());
            rp.setTestCompilerOptions(java.util.Collections.emptyList());
            rp.setTestJavaOptions(java.util.Collections.emptyList());
            rp.setFile((java.io.File)wd.getFile("config.jti"));
            rp.setEnvVars(GetEnvVars(debugUri));
            rp.setConcurrency(Environment.ProcessorCount);
            rp.setTimeLimit(15000);
            rp.setRetainArgs(java.util.Collections.singletonList("all"));
            rp.setExcludeLists(excludeFileList.ToArray());
            rp.setMatchLists(includeFileList.ToArray());
            rp.setIgnoreKind(JTRegTypes.IgnoreKind.QUIET);
            rp.setPriorStatusValues(null);
            rp.setUseWindowsSubsystemForLinux(true);
            rp.setTestNGPath(JTRegTypes.SearchPath.New(Path.Combine(JTREG_LIB, "testng.jar")));
            rp.setJUnitPath(JTRegTypes.SearchPath.New(Path.Combine(JTREG_LIB, "junit.jar")));
            rp.setAsmToolsPath(JTRegTypes.SearchPath.New(Path.Combine(JTREG_LIB, "asmtools.jar")));

            // configure keywords to filter based on
            string keywordsExpr = null;
            keywordsExpr = CombineKeywords(keywordsExpr, "!ignore");
            keywordsExpr = CombineKeywords(keywordsExpr, "!manual");
            if (string.IsNullOrWhiteSpace(keywordsExpr) == false)
                rp.setKeywordsExpr(keywordsExpr);

            // final initialization
            rp.initExprContext();
            if ((bool)rp.isValid() == false)
                throw new Exception();

            return rp;
        }

        /// <summary>
        /// Gets the set of environment variables to include with the tests by default.
        /// </summary>
        /// <param name="debugUri"></param>
        /// <returns></returns>
        java.util.Map GetEnvVars(Uri debugUri)
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

            // instruct child processes to signal debug attach
            if (debugUri != null)
            {
                envVars.put("IKVM_DEBUG_URI", debugUri.ToString());
                envVars.put("IKVM_DEBUG_WAIT", "60");
            }

            return envVars;
        }

        /// <summary>
        /// Combines the two keywords.
        /// </summary>
        /// <param name="kw1"></param>
        /// <param name="kw2"></param>
        /// <returns></returns>
        string CombineKeywords(string kw1, string kw2)
        {
            return kw1 == null ? kw2 : kw1 + " & " + kw2;
        }

        /// <summary>
        /// Executes the tests for the given parameters.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <param name="context"></param>
        /// <param name="tests"></param>
        /// <param name="output"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTests(string source, dynamic testManager, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests, java.io.Writer output, dynamic parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (tests is null)
                throw new ArgumentNullException(nameof(tests));
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            cancellationToken.ThrowIfCancellationRequested();

            // only continue if there are in fact tests in the suite to execute
            var firstTestResult = ((IEnumerable<object>)Util.GetTestResults(source, testManager, testSuite)).FirstOrDefault();
            if (firstTestResult is null)
                return;

            // generate a policy file for the test run
            var policyFile = (java.io.File)parameters.getWorkDirectory().getFile("jtreg.policy");
            using (var policyFileStream = new StreamWriter(File.OpenWrite(policyFile.toString())))
                foreach (var jarName in new[] { "jtreg.jar", "javatest.jar" })
                    policyFileStream.WriteLine($@"grant codebase ""{java.nio.file.Paths.get(Path.Combine(JTREG_LIB, jarName)).toUri().toURL()}"" {{ permission java.security.AllPermission; }};");

            // set parameters on pool
            var pool = JTRegTypes.Agent.Pool.Instance();
            pool.setSecurityPolicy(policyFile);

            // before we install our own security manager (which will restrict access to the system properties) take a copy of the system properties
            JTRegTypes.TestEnvironment.AddDefaultPropTable("(system properties)", java.lang.System.getProperties());

            // collect events from the harness
            var stats = JTRegTypes.TestStats.New();

            try
            {
                // observe harness for test results
                var harness = JTRegTypes.Harness.New();
                var observer = HarnessObserverInterceptor.Create(new HarnessObserverImplementation(source, testSuite, context, tests));
                harness.addObserver(observer);
                stats.register(harness);

                // begin harness execution asynchronously, thus allowing potential cancellation
                cancellationToken.Register(() => harness.stop());
                harness.batch(parameters);
            }
            catch (java.lang.InterruptedException)
            {
                // ignore
            }
            catch (Exception e)
            {
                context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
            }

            // shutdown pool
            pool.flush();

            // show result stats
            stats.showResultStats(new java.io.PrintWriter(output));

            // generate reports after execution
            var reporter = JTRegTypes.RegressionReporter.New(new java.io.PrintWriter(output));
            reporter.report(testManager);
        }

    }

}
