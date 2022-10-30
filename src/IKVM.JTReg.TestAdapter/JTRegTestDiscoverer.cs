using System.Collections.Generic;
using System.Threading;

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

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            foreach (var source in sources)
            {
                // setup isolation host for source
                using var host = new JTRegTestIsolationHost(source);
                var proxy = host.CreateManager();

                // discover tests from source
                proxy.DiscoverTests(source, new JTRegDiscoveryContextProxy(discoveryContext, logger, discoverySink), new CancellationTokenProxy(CancellationToken.None));
            }
        }

    }

}
