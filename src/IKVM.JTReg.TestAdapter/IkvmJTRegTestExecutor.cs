using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
                    var testCount = 0;

                    var l = new List<TestCase>();

                    // discover the full set of tests
                    foreach (dynamic testSuite in Util.GetTestSuites(source, testManager))
                        foreach (var testResult in GetTestResults(source, testSuite, CreateParameters(source, baseDir, testManager, testSuite, null, debugUri)))
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
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: Running test suite: {testSuite.getName()}");
                    RunTests(source, testManager, testSuite, runContext, frameworkHandle, tests, output, CreateParameters(source, baseDir, testManager, testSuite, tests, debugUri), cancellationToken);
                }
            }
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
            }
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
            var firstTestResult = ((IEnumerable<object>)GetTestResults(source, testSuite, parameters)).FirstOrDefault();
            if (firstTestResult is null)
                return;

            try
            {

                // observe harness for test results
                var harness = JTRegTypes.Harness.New();
                var observer = HarnessObserverInterceptor.Create(new HarnessObserverImplementation(source, testSuite, runContext, frameworkHandle, tests));
                harness.addObserver(observer);

                // collect test stats
                var stats = JTRegTypes.TestStats.New();
                stats.register(harness);

                // required for reporting
                var elapsedTimeHandler = JTRegTypes.ElapsedTimeHandler.New();
                elapsedTimeHandler.register(harness);

                // begin harness execution asynchronously, thus allowing potential cancellation
                harness.start(parameters);
                cancellationToken.Register(() => harness.stop());

                // wait until canceled or finished and cleanup
                harness.waitUntilDone();
                harness.stop();
                harness.removeObserver(observer);

                // show result stats
                stats.showResultStats(new java.io.PrintWriter(output));

                // generate reports after execution
                var reporter = JTRegTypes.RegressionReporter.New(new java.io.PrintWriter(output));
                reporter.report(testManager);
            }
            catch (java.lang.InterruptedException)
            {
                // ignore
            }
            catch (Exception e)
            {
                frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: Exception occurred running tests for '{source}'.\n{e}");
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
