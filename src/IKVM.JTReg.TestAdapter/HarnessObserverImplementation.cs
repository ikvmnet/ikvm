using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Proxied implementation of 'com.sun.javatest.Harness$Observer'.
    /// </summary>
    class HarnessObserverImplementation
    {

        readonly string source;
        readonly dynamic testSuite;
        readonly IRunContext runContext;
        readonly IFrameworkHandle frameworkHandle;
        readonly IEnumerable<TestCase> tests;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <param name="tests"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HarnessObserverImplementation(string source, dynamic testSuite, IRunContext runContext, IFrameworkHandle frameworkHandle, IEnumerable<TestCase> tests)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.testSuite = testSuite ?? throw new ArgumentNullException(nameof(testSuite));
            this.runContext = runContext ?? throw new ArgumentNullException(nameof(runContext));
            this.frameworkHandle = frameworkHandle ?? throw new ArgumentNullException(nameof(frameworkHandle));
            this.tests = tests ?? throw new ArgumentNullException(nameof(tests));
        }

        public void startingTestRun(dynamic self, dynamic parameters)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: starting test run");
        }

        public void startingTest(dynamic self, dynamic testResult)
        {
            var name = (string)TestResultMethods.GetFullyQualifiedName(source, testSuite, testResult);
            var test = tests.FirstOrDefault(i => i.FullyQualifiedName == name) ?? (TestCase)TestResultMethods.ToTestCase(source, testSuite, testResult);

            frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: starting test '{test.FullyQualifiedName}'");
            frameworkHandle.RecordStart(test);
        }

        public void error(dynamic self, string message)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Error, $"JTReg: error: '{message}'");
        }

        public void stoppingTestRun(dynamic self)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: stopping test run");
        }

        public void finishedTest(dynamic self, dynamic testResult)
        {
            var name = (string)TestResultMethods.GetFullyQualifiedName(source, testSuite, testResult);
            var test = tests.FirstOrDefault(i => i.Source == source && i.FullyQualifiedName == name) ?? (TestCase)TestResultMethods.ToTestCase(source, testSuite, testResult);
            var rslt = (TestResult)TestResultMethods.ToTestResult(source, testResult, test);

            frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: finished test '{test.FullyQualifiedName}'. [{rslt.Outcome}]");
            frameworkHandle.RecordEnd(test, rslt.Outcome);
            frameworkHandle.RecordResult(rslt);
        }

        public void finishedTesting(dynamic self)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Informational, $"JTReg: finished testing");
        }

        public void finishedTestRun(dynamic self, bool success)
        {
            frameworkHandle.SendMessage(success ? TestMessageLevel.Informational : TestMessageLevel.Error, $"JTReg: finished test run with overall result '{success}'");
        }

    }

}
