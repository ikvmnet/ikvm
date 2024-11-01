using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// Generates an implementation of 'com.sun.javatest.Harness$Observer'.
    /// </summary>
    public class HarnessObserverInterceptor : DispatchProxy
    {

        static readonly MethodInfo CreateMethodInfo = typeof(DispatchProxy).GetMethods()
            .Where(i => i.Name == "Create")
            .Where(i => i.GetGenericArguments().Length == 2)
            .First();

        /// <summary>
        /// Creates a new implementation of 'com.sun.javatest.Harness$Observer'.
        /// </summary>
        /// <returns></returns>
        public static dynamic Create(string source, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests)
        {
            var proxy = (HarnessObserverInterceptor)CreateMethodInfo.MakeGenericMethod(JTRegTypes.Harness.Observer.Type, typeof(HarnessObserverInterceptor)).Invoke(null, []);
            proxy.Init(source, testSuite, context, tests);
            return proxy;
        }

        string source;
        dynamic testSuite;
        IJTRegExecutionContext context;
        ConcurrentDictionary<string, JTRegTestCase> tests;

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="testSuite"></param>
        /// <param name="context"></param>
        /// <param name="tests"></param>
        /// <exception cref="ArgumentNullException"></exception>
        void Init(string source, dynamic testSuite, IJTRegExecutionContext context, IEnumerable<JTRegTestCase> tests)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.testSuite = testSuite ?? throw new ArgumentNullException(nameof(testSuite));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.tests = new ConcurrentDictionary<string, JTRegTestCase>(tests?.Where(i => i.Source == source).ToDictionary(i => i.FullyQualifiedName, i => i) ?? new Dictionary<string, JTRegTestCase>());
        }

        /// <inheritdoc />
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            switch (targetMethod.Name)
            {
                case "startingTestRun":
                    startingTestRun(args[0]);
                    break;
                case "startingTest":
                    startingTest(args[0]);
                    break;
                case "error":
                    error((string)args[0]);
                    break;
                case "stoppingTestRun":
                    stoppingTestRun();
                    break;
                case "finishedTest":
                    finishedTest(args[0]);
                    break;
                case "finishedTesting":
                    finishedTesting();
                    break;
                case "finishedTestRun":
                    finishedTestRun((bool)args[0]);
                    break;
            }

            return null;
        }

        void startingTestRun(dynamic parameters)
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: starting test run");
        }

        void startingTest(dynamic testResult)
        {
            var desc = testResult.getDescription();
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, desc);
            var test = tests.GetOrAdd(name, _ => Util.ToTestCase(source, testSuite, desc, context.Options.PartitionCount));

            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: starting test '{test.FullyQualifiedName}'");
            context.RecordStart(test);
        }

        void error(string message)
        {
            context.SendMessage(JTRegTestMessageLevel.Error, $"JTReg: error: '{message}'");
        }

        void stoppingTestRun()
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: stopping test run");
        }

        void finishedTest(dynamic testResult)
        {
            var desc = testResult.getDescription();
            var name = (string)Util.GetFullyQualifiedTestName(source, testSuite, desc);
            var test = tests.GetOrAdd(name, _ => Util.ToTestCase(source, testSuite, desc, context.Options.PartitionCount));
            var rslt = (JTRegTestResult)Util.ToTestResult(source, testSuite, testResult, test, context.Options.PartitionCount);

            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: finished test '{test.FullyQualifiedName}'. [{rslt.Outcome}]");
            context.RecordEnd(test, rslt.Outcome);
            context.RecordResult(rslt);
        }

        void finishedTesting()
        {
            context.SendMessage(JTRegTestMessageLevel.Informational, $"JTReg: finished testing");
        }

        void finishedTestRun(bool success)
        {
            context.SendMessage(success ? JTRegTestMessageLevel.Informational : JTRegTestMessageLevel.Warning, $"JTReg: finished test run with overall result '{success}'");
        }

    }

}
