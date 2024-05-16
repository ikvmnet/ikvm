using System.Collections.Generic;
using System.Linq;
using System.Threading;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Implements the <see cref="ITestExecutor"/> portion of the adapter.
    /// </summary>
    [ExtensionUri(JTRegTestManager.URI)]
    public class JTRegTestExecutor : ITestExecutor
    {

        CancellationTokenSource cts;

        /// <summary>
        /// Executes the given test cases.
        /// </summary>
        /// <param name="tests"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            cts = new CancellationTokenSource();
            RunTests(tests, runContext, frameworkHandle, cts.Token);
        }

        /// <summary>
        /// Executes the given test cases.
        /// </summary>
        /// <param name="tests"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle, CancellationToken cancellationToken)
        {
            foreach (var group in tests.GroupBy(i => i.Source))
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                JTRegTestManager.Instance.RunTests(group.Key, JTRegProxyUtil.Convert(group).ToList(), new JTRegExecutionContextProxy(runContext, frameworkHandle), cancellationToken);
            }
        }

        /// <summary>
        /// Executes the test cases in the given sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            cts = new CancellationTokenSource();
            RunTests(sources, runContext, frameworkHandle, cts.Token);
        }

        /// <summary>
        /// Executes the test cases in the given sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="runContext"></param>
        /// <param name="frameworkHandle"></param>
        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle, CancellationToken cancellationToken)
        {
            foreach (var source in sources)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                JTRegTestManager.Instance.RunTests(source, null, new JTRegExecutionContextProxy(runContext, frameworkHandle), cancellationToken);
            }
        }

        /// <summary>
        /// Signals cancellation of any running tests.
        /// </summary>
        public void Cancel()
        {
            cts?.Cancel();
        }

    }

}
