using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Base adapter class.
    /// </summary>
    public abstract class IkvmJTRegTestAdapter
    {

        protected const int PARTITION_COUNT = 16;

        protected const string BASEDIR_PREFIX = "ikvm-jtreg-";
        protected const string TEST_ROOT_FILE_NAME = "TEST.ROOT";
        protected const string TEST_PROBLEM_LIST_FILE_NAME = "ProblemList.txt";
        protected const string TEST_EXCLUDE_LIST_FILE_NAME = "ExcludeList.txt";
        protected const string TEST_INCLUDE_LIST_FILE_NAME = "IncludeList.txt";
        protected const string DEFAULT_OUT_FILE_NAME = "jtreg.out";
        protected const string DEFAULT_LOG_FILE_NAME = "jtreg.log";
        protected const string DEFAULT_WORK_DIR_NAME = "JTwork";
        protected const string DEFAULT_REPORT_DIR_NAME = "JTreport";
        protected const string DEFAULT_PARAM_TAG = "regtest";
        protected const string ENV_PREFIX = "JTREG_";

        protected static readonly MD5 MD5 = MD5.Create();

        protected static readonly string[] DEFAULT_WINDOWS_ENV_VARS = { "PATH", "SystemDrive", "SystemRoot", "windir", "TMP", "TEMP", "TZ" };
        protected static readonly string[] DEFAULT_UNIX_ENV_VARS = { "PATH", "DISPLAY", "GNOME_DESKTOP_SESSION_ID", "HOME", "LANG", "LC_ALL", "LC_CTYPE", "LPDEST", "PRINTER", "TZ", "XMODIFIERS" };

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmJTRegTestAdapter()
        {
            // executable permissions may not have made it onto the JRE binaries so attempt to set them
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var javaHome = java.lang.System.getProperty("java.home");
                foreach (var exec in new[] { "java", "javac", "jar", "jarsigner", "javadoc", "javah", "javap", "jdeps", "keytool", "policytool", "rmic", "wsgen", "wsimport" })
                {
                    var execPath = Path.Combine(javaHome, "bin", exec);
                    if (File.Exists(execPath))
                    {
                        try
                        {
                            var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(execPath);
                            if (psx.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.UserExecute) == false)
                                psx.FileAccessPermissions |= Mono.Unix.FileAccessPermissions.UserExecute;
                            if (psx.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.GroupExecute) == false)
                                psx.FileAccessPermissions |= Mono.Unix.FileAccessPermissions.GroupExecute;
                            if (psx.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.OtherExecute) == false)
                                psx.FileAccessPermissions |= Mono.Unix.FileAccessPermissions.OtherExecute;
                        }
                        catch
                        {

                        }
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
        protected static string GetSourceHash(string source)
        {
            var b = MD5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var s = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                s.Append(b[i].ToString("x2"));
            return s.ToString();
        }

        protected readonly Dictionary<string, TestProperty> properties = new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected IkvmJTRegTestAdapter()
        {
            properties.Add(IkvmJTRegTestProperties.TestSuiteRootProperty.Label, IkvmJTRegTestProperties.TestSuiteRootProperty);
            properties.Add(IkvmJTRegTestProperties.TestSuiteNameProperty.Label, IkvmJTRegTestProperties.TestSuiteNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestPathNameProperty.Label, IkvmJTRegTestProperties.TestPathNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestIdProperty.Label, IkvmJTRegTestProperties.TestIdProperty);
            properties.Add(IkvmJTRegTestProperties.TestNameProperty.Label, IkvmJTRegTestProperties.TestNameProperty);
            properties.Add(IkvmJTRegTestProperties.TestTitleProperty.Label, IkvmJTRegTestProperties.TestTitleProperty);
            properties.Add(IkvmJTRegTestProperties.TestAuthorProperty.Label, IkvmJTRegTestProperties.TestAuthorProperty);
            properties.Add(IkvmJTRegTestProperties.TestPartitionProperty.Label, IkvmJTRegTestProperties.TestPartitionProperty);
            properties.Add(IkvmJTRegTestProperties.TestCategoryProperty.Label, IkvmJTRegTestProperties.TestCategoryProperty);
        }

        /// <summary>
        /// Creates a new TestManager.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="baseDir"></param>
        /// <param name="output"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        protected dynamic CreateTestManager(IMessageLogger logger, string baseDir, java.io.PrintWriter output, TextWriter errors)
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
        protected dynamic CreateParameters(string source, string baseDir, dynamic testManager, dynamic testSuite, IEnumerable<TestCase> tests)
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
                var testSuiteName = Util.GetTestSuiteName(source, testSuite);

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
            rp.setCompileJDK(JTRegTypes.JDK.Of(new java.io.File(java.lang.System.getProperty("java.home"))));
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
            var testNGPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "testng.jar");
            rp.setTestNGPath(JTRegTypes.SearchPath.New(testNGPath));

            // locate and configure JUnit
            var junitPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "junit.jar");
            rp.setJUnitPath(JTRegTypes.SearchPath.New(junitPath));

            // locate and configure JUnit
            var asmtoolsPath = Path.Combine(Path.GetDirectoryName(typeof(IkvmJTRegTestAdapter).Assembly.Location), "jtreg", "asmtools.jar");
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
        protected IEnumerable<dynamic> GetTestResults(string source, dynamic testSuite, dynamic parameters)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (testSuite is null)
                throw new ArgumentNullException(nameof(testSuite));
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
                return ((IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() })).OrderBy(i => Util.GetTestPathName(source, testSuite, i));
            else if (tests.Length == 0)
                return (IEnumerable<dynamic>)Array.CreateInstance(ikvm.runtime.Util.getInstanceTypeFromClass(JTRegTypes.TestResult.Class), 0);
            else
                return ((IEnumerable<dynamic>)rtltrmethod.Invoke(null, new[] { (java.util.Iterator)trt.getIterator() })).OrderBy(i => Util.GetTestPathName(source, testSuite, i));
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
