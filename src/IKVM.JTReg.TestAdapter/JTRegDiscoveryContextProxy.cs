using System;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// Proxies the <see cref="IJTRegDiscoveryContext"/> interface to the Visual Studio test adapter.
    /// </summary>
    class JTRegDiscoveryContextProxy :
#if NETFRAMEWORK
        MarshalByRefObject,
#endif
        IJTRegDiscoveryContext
    {

        readonly IDiscoveryContext discoveryContext;
        readonly IMessageLogger logger;
        readonly ITestCaseDiscoverySink discoverySink;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="discoveryContext"></param>
        /// <param name="logger"></param>
        /// <param name="discoverySink"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JTRegDiscoveryContextProxy(IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            this.discoveryContext = discoveryContext ?? throw new ArgumentNullException(nameof(discoveryContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.discoverySink = discoverySink ?? throw new ArgumentNullException(nameof(discoverySink));
        }

        public void SendMessage(JTRegTestMessageLevel informational, string message)
        {
            logger.SendMessage(Convert(informational), message);
        }

        public void SendTestCase(JTRegTestCase testCase)
        {
            discoverySink.SendTestCase(Convert(testCase));
        }

        /// <summary>
        /// Converts a <see cref="JTRegTestCase"/> to a <see cref="TestCase"/>.
        /// </summary>
        /// <param name="testCase"></param>
        /// <returns></returns>
        TestCase Convert(JTRegTestCase testCase)
        {
            var t = new TestCase(testCase.FullyQualifiedName, new Uri(JTRegTestManager.URI), testCase.Source) { CodeFilePath = testCase.CodeFilePath };
            t.SetPropertyValue(JTRegTestProperties.TestSuiteRootProperty, testCase.TestSuiteRoot);
            t.SetPropertyValue(JTRegTestProperties.TestSuiteNameProperty, testCase.TestSuiteName);
            t.SetPropertyValue(JTRegTestProperties.TestPathNameProperty, testCase.TestPathName);
            t.SetPropertyValue(JTRegTestProperties.TestIdProperty, testCase.TestId);
            t.SetPropertyValue(JTRegTestProperties.TestTitleProperty, testCase.TestTitle);
            t.SetPropertyValue(JTRegTestProperties.TestAuthorProperty, testCase.TestAuthor);
            t.SetPropertyValue(JTRegTestProperties.TestCategoryProperty, testCase.TestCategory);
            t.SetPropertyValue(JTRegTestProperties.TestPartitionProperty, testCase.TestPartition);

            foreach (var kvp in testCase.Traits)
                t.Traits.Add(kvp.Key, kvp.Value);

            return t;
        }

        TestMessageLevel Convert(JTRegTestMessageLevel messageLevel) => messageLevel switch
        {
            JTRegTestMessageLevel.Informational => TestMessageLevel.Informational,
            JTRegTestMessageLevel.Warning => TestMessageLevel.Warning,
            JTRegTestMessageLevel.Error => TestMessageLevel.Error,
            _ => throw new NotImplementedException(),
        };

#if NETFRAMEWORK

        public override object InitializeLifetimeService()
        {
            return null;
        }

#endif

    }

}
