using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    [ExtensionUri("executor://ikvmjtregtestadapter/v1")]
    public class IkvmJTRegTestExecutor : IkvmJTRegTestAdapter, ITestExecutor
    {

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

                // normalize source path
                source = Path.GetFullPath(source);
                frameworkHandle.SendMessage(TestMessageLevel.Informational, "JTReg: " + $"Scanning for test roots for '{source}'.");

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
                    var testCount = 0;

                    var l = new List<TestCase>();

                    // discover the full set of tests
                    foreach (dynamic testSuite in Util.GetTestSuites(source, testManager))
                        foreach (var testResult in GetTestResults(source, testSuite, CreateParameters(source, baseDir, testManager, testSuite, null)))
                            l.Add(Util.ToTestCase(source, testSuite, testResult, testCount++ % PARTITION_COUNT));

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
            var firstTestResult = ((IEnumerable<object>)GetTestResults(source, testSuite, parameters)).FirstOrDefault();
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

    }

}
