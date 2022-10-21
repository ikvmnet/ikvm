using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    [ExtensionUri(URI)]
    public class IkvmJTRegTestExecutor : IkvmJTRegTestAdapter, ITestExecutor
    {

        protected static readonly string[] DEFAULT_WINDOWS_ENV_VARS = { "PATH", "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ" };
        protected static readonly string[] DEFAULT_UNIX_ENV_VARS = { "PATH", "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG", "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS" };

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmJTRegTestExecutor()
        {
#if NETCOREAPP
            // executable permissions may not have made it onto the JRE binaries so attempt to set them
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var javaHome = java.lang.System.getProperty("java.home");
                foreach (var exec in new[] { "java", "javac", "jar", "jarsigner", "javadoc", "javah", "javap", "jdeps", "keytool", "policytool", "rmic", "wsgen", "wsimport" })
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

        CancellationTokenSource cts;

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
        internal void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
        {
            try
            {
                cts = new CancellationTokenSource();
                IkvmTraceServer debug = null;

                try
                {
                    // if we have the capability of attaching to child process, start debug server to listen for child processes
                    if (frameworkHandle is IFrameworkHandle2 fh2 && Debugger.IsAttached)
                    {
                        debug = new IkvmTraceServer(fh2);
                        debug.Start();
                    }

                    foreach (var source in sources)
                        RunTestForSource(source, runContext, frameworkHandle, tests?.Where(i => i.Source == source), debug?.Uri, cts.Token);
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
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: Exception occurred running tests.\n{e}");
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
        /// <param name="debugUri"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTestForSource(string source, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, Uri debugUri, CancellationToken cancellationToken)
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

                // discover test suites from test assembly
                var testDirs = new java.util.ArrayList();
                foreach (var testRoot in Util.GetTestSuiteDirectories(source, frameworkHandle))
                    testDirs.add(new java.io.File(testRoot));
                if (testDirs.size() == 0)
                    return;

                // output path for jtreg state
                var id = GetSourceHash(source);
                var baseDir = Path.Combine(runContext.TestRunDirectory, BASEDIR_PREFIX + id);
                Directory.CreateDirectory(baseDir);

                // output to framework
                using var output = new MessageLoggerWriter(frameworkHandle, TestMessageLevel.Informational);

                // initialize the test manager with the discovered roots
                var testManager = CreateTestManager(frameworkHandle, baseDir, new java.io.PrintWriter(output));
                testManager.addTestFiles(testDirs, false);

                // we will need a full list of tests to apply any filters to
                if (tests == null)
                {
                    var l = new List<TestCase>();

                    // discover the full set of tests
                    foreach (dynamic testSuite in Util.GetTestSuites(source, testManager))
                        foreach (var testCase in (IEnumerable<TestCase>)Util.GetTestCases(source, testManager, testSuite))
                            l.Add(testCase);

                    tests = l;
                }

                // filter the tests
                var filter = runContext.GetTestCaseFilter(properties.Keys, s => properties.TryGetValue(s, out var v) ? v : null);
                if (filter != null)
                    tests = tests.Where(i => filter.MatchTestCase(i, s => properties.TryGetValue(s, out var v) ? i.GetPropertyValue(v) : null));

                // invoke each test suite
                foreach (dynamic testSuite in (IEnumerable)testManager.getTestSuites())
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: Running test suite: {testSuite.getName()}");
                    RunTests(source, testManager, testSuite, runContext, frameworkHandle, tests, output, CreateParameters(source, testManager, testSuite, tests, debugUri), cancellationToken);
                }
            }
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
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
        dynamic CreateParameters(string source,  dynamic testManager, dynamic testSuite, IEnumerable<TestCase> tests, Uri debugUri)
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
                        if (string.Equals(test.GetPropertyValue(IkvmJTRegTestProperties.TestSuiteNameProperty), testSuiteName))
                            includeList.WriteLine(test.GetPropertyValue(IkvmJTRegTestProperties.TestPathNameProperty) + " generic-all");

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
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        /// <param name="output"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        internal void RunTests(string source, dynamic testManager, dynamic testSuite, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests, java.io.Writer output, dynamic parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            if (testManager is null)
                throw new ArgumentNullException(nameof(testManager));
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
                var observer = HarnessObserverInterceptor.Create(new HarnessObserverImplementation(source, testSuite, runContext, frameworkHandle, tests));
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
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
            }

            // shutdown pool
            pool.flush();

            // show result stats
            stats.showResultStats(new java.io.PrintWriter(output));

            // generate reports after execution
            var reporter = JTRegTypes.RegressionReporter.New(new java.io.PrintWriter(output));
            reporter.report(testManager);
        }

        /// <summary>
        /// Initiates a cancellation of the test discovery or execution.
        /// </summary>
        public void Cancel()
        {
            cts?.Cancel();
            cts = null;
        }

    }

}
