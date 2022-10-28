using System.Collections.Generic;
using System.Linq;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    [DefaultExecutorUri(JTRegTestManager.URI)]
    [FileExtension(".dll")]
    [FileExtension(".exe")]
    public class JTRegTestDiscoverer : ITestDiscoverer
    {

        readonly IJTRegTestManager manager = JTRegTestIsolationHost.CreateManager();

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            manager.DiscoverTests(sources.ToList(), new JTRegDiscoveryContextProxy(discoveryContext, logger, discoverySink));
        }

    }

}
