using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Implements the <see cref="ITestDiscoverer"/> portion of the adapter.
    /// </summary>
    [DefaultExecutorUri(JTRegTestManager.URI)]
    [FileExtension(".dll")]
    [FileExtension(".exe")]
    public class JTRegTestDiscoverer : ITestDiscoverer
    {

        /// <summary>
        /// Discovers the tests available from the provided sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            DiscoverTests(sources, discoveryContext, logger, discoverySink, CancellationToken.None);
        }

        /// <summary>
        /// Discovers the tests available from the provided sources.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        /// <param name="cancellationToken"></param>
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink, CancellationToken cancellationToken)
        {
            foreach (var source in sources)
            {
                if (Path.GetExtension(source) is not ".exe" and not ".dll")
                    continue;

                try
                {
                    JTRegTestManager.Instance.DiscoverTests(source, new JTRegDiscoveryContextProxy(discoveryContext, logger, discoverySink), cancellationToken);
                }
                catch (Exception e)
                {
                    logger.SendMessage(TestMessageLevel.Error, $"JTReg: An exception occurred discovering tests for '{source}': {e.Message}.\n{e}");
                }
            }
        }

    }

}
