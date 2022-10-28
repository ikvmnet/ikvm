using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Proxies the <see cref="IJTRegExecutionContext"/> interface to the Visual Studio test adapter.
    /// </summary>
    class JTRegExecutionContextProxy :
#if NETFRAMEWORK
        MarshalByRefObject,
#endif
        IJTRegExecutionContext
    {

        static readonly Dictionary<string, TestProperty> properties = new Dictionary<string, TestProperty>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        static JTRegExecutionContextProxy()
        {
            properties.Add(JTRegTestProperties.TestSuiteRootProperty.Label, JTRegTestProperties.TestSuiteRootProperty);
            properties.Add(JTRegTestProperties.TestSuiteNameProperty.Label, JTRegTestProperties.TestSuiteNameProperty);
            properties.Add(JTRegTestProperties.TestPathNameProperty.Label, JTRegTestProperties.TestPathNameProperty);
            properties.Add(JTRegTestProperties.TestIdProperty.Label, JTRegTestProperties.TestIdProperty);
            properties.Add(JTRegTestProperties.TestNameProperty.Label, JTRegTestProperties.TestNameProperty);
            properties.Add(JTRegTestProperties.TestTitleProperty.Label, JTRegTestProperties.TestTitleProperty);
            properties.Add(JTRegTestProperties.TestAuthorProperty.Label, JTRegTestProperties.TestAuthorProperty);
            properties.Add(JTRegTestProperties.TestPartitionProperty.Label, JTRegTestProperties.TestPartitionProperty);
            properties.Add(JTRegTestProperties.TestCategoryProperty.Label, JTRegTestProperties.TestCategoryProperty);
        }

        readonly IRunContext runContext;
        readonly IFrameworkHandle frameworkHandle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JTRegExecutionContextProxy(IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            this.runContext = runContext ?? throw new ArgumentNullException(nameof(runContext));
            this.frameworkHandle = frameworkHandle ?? throw new ArgumentNullException(nameof(frameworkHandle));
        }

        public string TestRunDirectory => runContext.TestRunDirectory;

        public bool CanAttachDebuggerToProcess => frameworkHandle is IFrameworkHandle2;

        public bool AttachDebuggerToProcess(int pid)
        {
            return frameworkHandle is IFrameworkHandle2 h && h.AttachDebuggerToProcess(pid);
        }

        bool MatchTestCase(ITestCaseFilterExpression filter, JTRegTestCase testCase)
        {
            var t = JTRegProxyUtil.Convert(testCase);
            return filter.MatchTestCase(t, s => properties.TryGetValue(s, out var v) ? t.GetPropertyValue(v) : null);
        }

        public List<JTRegTestCase> FilterTestCases(List<JTRegTestCase> tests)
        {
            var filter = runContext.GetTestCaseFilter(properties.Keys, s => properties.TryGetValue(s, out var v) ? v : null);
            if (filter != null)
                tests = tests.Where(i => MatchTestCase(filter, i)).ToList();

            return tests;
        }

        public void RecordEnd(JTRegTestCase test, JTRegTestOutcome outcome)
        {
            frameworkHandle.RecordEnd(JTRegProxyUtil.Convert(test), JTRegProxyUtil.Convert(outcome));
        }

        public void RecordResult(JTRegTestResult rslt)
        {
            frameworkHandle.RecordResult(JTRegProxyUtil.Convert(rslt));
        }

        public void RecordStart(JTRegTestCase test)
        {
            frameworkHandle.RecordStart(JTRegProxyUtil.Convert(test));
        }

        public void SendMessage(JTRegTestMessageLevel level, string message)
        {
            frameworkHandle.SendMessage(JTRegProxyUtil.Convert(level), message);
        }

#if NETFRAMEWORK

        public override object InitializeLifetimeService()
        {
            return null;
        }

#endif

    }

}