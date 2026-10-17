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

        public static readonly JTRegTestManager Instance = new();

        public const string URI = "executor://jtregtestadapter/v1";

        internal static readonly string JTREG_LIB = Path.Combine(Path.GetDirectoryName(typeof(JTRegTestManager).Assembly.Location), "jtreg");

        protected static readonly string[] DEFAULT_WINDOWS_ENV_VARS = { "PATH", "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ" };
        protected static readonly string[] DEFAULT_UNIX_ENV_VARS = { "PATH", "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG", "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS" };

        internal const string BASEDIR_PREFIX = "ikvm-jtreg-";
        internal const string TEST_ROOT_FILE_NAME = "TEST.ROOT";
        internal const string DEFAULT_WORK_DIR_NAME = "work";
        internal const string DEFAULT_REPORT_DIR_NAME = "report";
        internal const string DEFAULT_PARAM_TAG = "regtest";
        internal const string ENV_PREFIX = "JTREG_";

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JTRegTestManager()
        {
            // executable permissions may not have made it onto the JRE binaries so attempt to set them
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var javaHome = java.lang.System.getProperty("java.home");
                foreach (var exec in new[] { "java", "javac", "jar", "jarsigner", "javadoc", "javah", "javap", "jdeps", "keytool", "native2ascii", "orbd", "policytool", "rmic", "schemagen", "wsgen", "wsimport", "xjc" })
                {
                    var execPath = Path.Combine(javaHome, "bin", exec);
                    if (File.Exists(execPath))
                    {
#if NET7_0_OR_GREATER
                        var mod = File.GetUnixFileMode(execPath);
                        var prm = mod | UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
                        if (mod != prm)
                            File.SetUnixFileMode(execPath, prm);
#else
                        var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(execPath);
                        var mod = psx.FileAccessPermissions;
                        var prm = mod | Mono.Unix.FileAccessPermissions.UserExecute | Mono.Unix.FileAccessPermissions.GroupExecute | Mono.Unix.FileAccessPermissions.OtherExecute;
                        if (mod != prm)
                            psx.FileAccessPermissions = prm;
#endif
                    }
                }
            }

            // need to do some static configuration on the harness
            if (JTRegTypes.Harness.GetClassDirMethod.invoke(null) == null)
                JTRegTypes.Harness.SetClassDirMethod.invoke(null, JTRegTypes.ProductInfo.GetJavaTestClassDirMethod.invoke(null));
        }

        /// <summary>
        /// Creates a short hash of the given string to uniquely identify it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static string GetSourceId(string source)
        {
            using var md5 = MD5.Create();
            var h = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

            var s = new StringBuilder(16);
            for (int i = 0; i < 8; i++)
                s.Append(h[i].ToString("x2"));

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

            var testManager = JTRegTypes.TestManager.New(output, new java.io.File(Environment.CurrentDirectory), ErrorHandlerInterceptor.Create(logger));

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
        /// <param name="options"></param>
        /// <param name="testSuite"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public List<java.io.File> GetExcludeListFiles(JTRegTestOptions options, dynamic testSuite)
        {
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // if a ProblemList.txt or ExcludeList.txt file exists in the root, add them as exclude files
            var excludeFileList = new List<java.io.File>();
            foreach (var n in options.ExcludeListFiles.Concat(options.AdditionalExcludeListFiles))
                if (Path.Combine(((java.io.File)testSuite.getRootDir()).toString(), n) is string f && File.Exists(f))
                    excludeFileList.Add(new java.io.File(new java.io.File(f).getAbsoluteFile().toURI().normalize()));

            return excludeFileList;
        }

        /// <summary>
        /// Gets the set of files that represent the inclusions for a suite.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<java.io.File> GetIncludeListFiles(JTRegTestOptions options, dynamic testSuite)
        {
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // if a IncludeList.txt file exists in the root, add it as include files
            var includeFileList = new List<java.io.File>();
            foreach (var n in options.IncludeListFiles.Concat(options.AdditionalIncludeListFiles))
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

            source = Path.GetFullPath(source);
            DiscoverTestsImpl(source, Util.GetTestSuiteDirectories(source, context).ToArray(), context, cancellationToken);
        }

        /// <summary>
        /// Discovers the available tests in the given root directories.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testDirs"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        internal void DiscoverTestsImpl(string source, string[] testDirs, IJTRegDiscoveryContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testDirs is null)
                throw new ArgumentNullException(nameof(testDirs));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                // discover test suites from test assembly
                var testRoots = new java.util.ArrayList();
                foreach (var testDir in testDirs)
                    testRoots.add(new java.io.File(testDir));
                if (testRoots.size() == 0)
                    return;

                // output path for jtreg state
                var id = GetSourceId(source);
                var baseDir = Path.Combine(Path.GetTempPath(), BASEDIR_PREFIX + id);

                // attempt to create a temporary directory as scratch space for this test
                context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Using discover directory: {baseDir}");
                Directory.CreateDirectory(baseDir);

                // output to framework
                using var output = new MessageLoggerWriter(context, JTRegTestMessageLevel.Informational);

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(context, baseDir, new java.io.PrintWriter(output));
                testManager.addTestFiles(testRoots, false);

                // track metrics related to tests
                int testCount = 0;
                var testWatch = new Stopwatch();
                testWatch.Start();

                // for each suite, get the results and transform a test case
                foreach (dynamic testSuite in (IEnumerable<dynamic>)Util.GetTestSuites(source, testManager))
                {
                    foreach (var testCase in (IEnumerable<JTRegTestCase>)Util.GetTestCases(source, testManager, testSuite, context.Options.PartitionCount))
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return;

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

        /// <summary>
        /// Runs the specified test cases, if provided, in the sources.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tests"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        public void RunTests(string source, List<JTRegTestCase> tests, IJTRegExecutionContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                IkvmTraceServer debug = null;

                try
                {
                    // if we have the capability of attaching to child process, start debug server to listen for child processes
                    if (context.CanAttachDebuggerToProcess && Debugger.IsAttached)
                    {
                        debug = new IkvmTraceServer(context);
                        debug.Start(cancellationToken);
                    }

                    source = Path.GetFullPath(source);
                    RunTestsImpl(source, Util.GetTestSuiteDirectories(source, context).ToArray(), tests?.Where(i => i.Source == source).ToList(), context, debug?.Uri, cancellationToken);
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
        /// Runs the tests in the given sources, optionally filtered by specific test case.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testDirs"></param>
        /// <param name="tests"></param>
        /// <param name="context"></param>
        /// <param name="debugUri"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTestsImpl(string source, string[] testDirs, List<JTRegTestCase> tests, IJTRegExecutionContext context, Uri debugUri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // discover test suites from test assembly
                var testRoots = new java.util.ArrayList();
                foreach (var testDir in testDirs)
                    testRoots.add(new java.io.File(testDir));
                if (testRoots.size() == 0)
                    return;

                // output path for jtreg state
                var id = GetSourceId(source);
                var baseDir = Path.Combine(context.TestRunDirectory, BASEDIR_PREFIX + id);

                // attempt to create a temporary directory as scratch space for this test
                context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Using run directory: {baseDir}");
                Directory.CreateDirectory(baseDir);

                // output to framework
                using var output = new MessageLoggerWriter(context, JTRegTestMessageLevel.Informational);

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(context, baseDir, new java.io.PrintWriter(output));
                testManager.addTestFiles(testRoots, false);

                // load the set of suites
                var testSuites = ((IEnumerable<dynamic>)Util.GetTestSuites(source, testManager)).ToList();

                // invoke each test suite
                foreach (dynamic testSuite in testSuites)
                {
                    bool FilterByList(dynamic td) => tests == null || tests.Any(i => i.TestPathName == (string)Util.GetTestPathName(source, testSuite, td));
                    bool FilterByContext(dynamic td) => context.FilterTestCase(Util.ToTestCase(source, testSuite, td, context.Options.PartitionCount));
                    bool Filter(dynamic td) => FilterByList(td) && FilterByContext(td);

                    context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: Running test suite: {(string)testSuite.getName()}");
                    RunTestsImpl(source, testManager, testSuite, context, tests, output, CreateParameters(testManager, testSuite, (Func<dynamic, bool>)Filter, context.Options, debugUri), cancellationToken);
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
        /// <param name="testManager"></param>
        /// <param name="testSuite"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <param name="debugUri"></param>
        /// <returns></returns>
        dynamic CreateParameters(dynamic testManager, dynamic testSuite, Func<dynamic, bool> filter, JTRegTestOptions options, Uri debugUri)
        {
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));

            // invoke new RegressionParameters(string, RegressionTestSuite)
            var rp = RegressionParametersInterceptor.Create(DEFAULT_PARAM_TAG, testSuite, filter);

            // configure work directory
            var wd = testManager.getWorkDirectory(testSuite);
            rp.setWorkDirectory(wd);

            // configure report directory
            var rd = testManager.getReportDirectory(testSuite);
            rp.setReportDir(rd);

            // if a ProblemList.txt or ExcludeList.txt file exists in the root, add them as exclude files
            var excludeFileList = (List<java.io.File>)GetExcludeListFiles(options, testSuite);

            // if a IncludeList.txt file exists in the root, add it as include files
            var includeFileList = (List<java.io.File>)GetIncludeListFiles(options, testSuite);

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
            rp.setConcurrency(options.Concurrency > 0 ? options.Concurrency : Environment.ProcessorCount);
            rp.setTimeoutFactor(options.TimeoutFactor);
            rp.setRetainArgs(java.util.Collections.singletonList("all"));
            rp.setExcludeLists(excludeFileList.ToArray());
            rp.setMatchLists(includeFileList.ToArray());
            rp.setIgnoreKind(JTRegTypes.IgnoreKind.QUIET);
            rp.setPriorStatusValues(null);
            rp.setUseWindowsSubsystemForLinux(((string)JTRegTypes.OS.Current().family) == "windows");
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
        internal void RunTestsImpl(string source, dynamic testManager, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests, java.io.Writer output, dynamic parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (parameters is null)
                throw new ArgumentNullException(nameof(parameters));

            cancellationToken.ThrowIfCancellationRequested();

            // only continue if there are in fact tests in the suite to execute
            var firstTestResult = ((IEnumerable<object>)Util.GetTestResults(source, testManager, testSuite)).FirstOrDefault();
            if (firstTestResult is null)
                return;

            // generate a policy file for the test run
            var policyFile = (java.io.File)parameters.getWorkDirectory().getFile("jtreg.policy");
            using (var policyFileStream = new StreamWriter(File.Create(policyFile.toString())))
                foreach (var jarName in new[] { "jtreg.jar", "javatest.jar" })
                    policyFileStream.WriteLine($@"grant codebase ""{java.nio.file.Paths.get(Path.Combine(JTREG_LIB, jarName)).toUri().toURL()}"" {{ permission java.security.AllPermission; }};");

            // set parameters on pool
            var opts = context.Options;
            var pool = JTRegTypes.Agent.Pool.Instance(parameters);
            pool.setTimeoutFactor(opts.TimeoutFactor);
            pool.setSecurityPolicy(policyFile);

            // before we install our own security manager (which will restrict access to the system properties) take a copy of the system properties
            JTRegTypes.TestEnvironment.AddDefaultPropTable("(system properties)", java.lang.System.getProperties());

            // collect events from the harness
            var stats = JTRegTypes.TestStats.New();

            try
            {
                // observe harness for test results
                var harness = JTRegTypes.Harness.New();
                var observer = HarnessObserverInterceptor.Create(source, testSuite, context, tests);
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
