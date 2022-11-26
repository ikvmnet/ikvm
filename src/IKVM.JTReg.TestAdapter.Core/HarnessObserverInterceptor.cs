using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.Harness$Observer'.
    /// </summary>
    class HarnessObserverInterceptor : IInterceptor
    {

        static readonly ProxyGenerator DefaultProxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.Harness$Observer'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(string source, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests)
        {
            return DefaultProxyGenerator.CreateInterfaceProxyWithoutTarget(JTRegTypes.Harness.Observer.Type, new HarnessObserverInterceptor(source, testSuite, context, tests));
        }

        readonly string source;
        readonly dynamic testSuite;
        readonly IJTRegExecutionContext context;
        readonly ConcurrentDictionary<string, JTRegTestCase> tests;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="context"></param>
        /// <param name="tests"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HarnessObserverInterceptor(string source, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.testSuite = testSuite ?? throw new ArgumentNullException(nameof(testSuite));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.tests = new ConcurrentDictionary<string, JTRegTestCase>(tests?.Where(i => i.Source == source).ToDictionary(i => i.FullyQualifiedName, i => i) ?? new Dictionary<string, JTRegTestCase>());
        }

        /// <summary>
        /// Invoked for any operation on the original type.
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "startingTestRun":
                    startingTestRun(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "startingTest":
                    startingTest(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "error":
                    error(invocation.Proxy, (string)invocation.GetArgumentValue(0));
                    break;
                case "stoppingTestRun":
                    stoppingTestRun(invocation.Proxy);
                    break;
                case "finishedTest":
                    finishedTest(invocation.Proxy, invocation.GetArgumentValue(0));
                    break;
                case "finishedTesting":
                    finishedTesting(invocation.Proxy);
                    break;
                case "finishedTestRun":
                    finishedTestRun(invocation.Proxy, (bool)invocation.GetArgumentValue(0));
                    break;
                default:
                    invocation.Proceed();
                    break;
            }
        }

        public void startingTestRun(dynamic self, dynamic parameters)
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: starting test run");
        }

        public void startingTest(dynamic self, dynamic testResult)
        {
            var desc = testResult.getDescription();
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, desc);
            var test = tests.GetOrAdd(name, _ => Util.ToTestCase(source, testSuite, desc));

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
            var desc = testResult.getDescription();
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, desc);
            var test = tests.GetOrAdd(name, _ => Util.ToTestCase(source, testSuite, desc));
            var rslt = (JTRegTestResult)Util.ToTestResult(source, testSuite, testResult, test);

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
