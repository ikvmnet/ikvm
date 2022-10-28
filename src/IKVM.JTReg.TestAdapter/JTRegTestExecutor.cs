using System.Collections.Generic;
using System.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace IKVM.JTReg.TestAdapter
{

    [ExtensionUri(JTRegTestManager.URI)]
    public class JTRegTestExecutor : ITestExecutor
    {

        readonly IJTRegTestManager manager = JTRegTestIsolationHost.CreateManager();

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            manager.RunTests(sources.ToList(), new JTRegExecutionContextProxy(runContext, frameworkHandle));
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            manager.RunTests(JTRegProxyUtil.Convert(tests).ToList(), new JTRegExecutionContextProxy(runContext, frameworkHandle));
        }

        public void Cancel()
        {
            manager.Cancel();
        }

    }

}
