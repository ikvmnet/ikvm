using System;
using System.Collections.Generic;
using System.Linq;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Proxied implementation of 'com.sun.javatest.Harness$Observer'.
    /// </summary>
    class HarnessObserverImplementation
    {

        readonly string source;
        readonly dynamic testSuite;
        readonly IJTRegExecutionContext context;
        readonly IDictionary<string, JTRegTestCase> tests;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="context"></param>
        /// <param name="tests"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HarnessObserverImplementation(string source, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.testSuite = testSuite ?? throw new ArgumentNullException(nameof(testSuite));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.tests = tests.Where(i => i.Source == source).ToDictionary(i => i.FullyQualifiedName, i => i) ?? throw new ArgumentNullException(nameof(tests));
        }

        public void startingTestRun(dynamic self, dynamic parameters)
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: starting test run");
        }

        public void startingTest(dynamic self, dynamic testResult)
        {
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, testResult);
            var test = tests.TryGetValue(name, out var t) ? t : null;

            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: starting test '{test.FullyQualifiedName}'");
            context.RecordStart(test);
        }

        public void error(dynamic self, string message)
        {
            context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: error: '{message}'");
        }

        public void stoppingTestRun(dynamic self)
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: stopping test run");
        }

        public void finishedTest(dynamic self, dynamic testResult)
        {
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, testResult);
            var test = tests.TryGetValue(name, out var t) ? t : null;
            var rslt = (JTRegTestResult)Util.ToTestResult(source, testResult, test);

            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: finished test '{test.FullyQualifiedName}'. [{rslt.Outcome}]");
            context.RecordEnd(test, rslt.Outcome);
            context.RecordResult(rslt);
        }

        public void finishedTesting(dynamic self)
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: finished testing");
        }

        public void finishedTestRun(dynamic self, bool success)
        {
            context.SendMessage(success ? JTRegTestMessageLevel.Informational : JTRegTestMessageLevel.Warning, $"JTReg: finished test run with overall result '{success}'");
        }

    }

}
