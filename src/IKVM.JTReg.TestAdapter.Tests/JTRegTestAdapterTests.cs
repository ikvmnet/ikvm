using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.JavaTest.TestAdapter.Tests
{

    [TestClass]
    public class JTRegTestAdapterTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Can_locate_test()
        {
            var discoveryContext = new Mock<IDiscoveryContext>();
            var messageLogger = new Mock<IMessageLogger>();
            var testCaseDiscoverySink = new Mock<ITestCaseDiscoverySink>();

            var testCases = new List<TestCase>();
            testCaseDiscoverySink.Setup(x => x.SendTestCase(It.IsAny<TestCase>())).Callback((TestCase x) => testCases.Add(x));

            var adp = new JTRegTestAdapter();
            adp.DiscoverTests(typeof(JTRegTestAdapterTests).Assembly.Location, discoveryContext.Object, messageLogger.Object, testCaseDiscoverySink.Object);
            testCases.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [TestMethod]
        public void Can_execute_test()
        {
            var runContext = new Mock<IRunContext>();
            runContext.Setup(x => x.TestRunDirectory).Returns(TestContext.TestRunDirectory);

            var frameworkHandle = new Mock<IFrameworkHandle2>();

            var sources = new[] { typeof(JTRegTestAdapterTests).Assembly.Location };

            // execute tests
            JTRegTestAdapter.Initialize();
            var adp = new JTRegTestAdapter();
            adp.RunTests(sources, runContext.Object, frameworkHandle.Object);
        }

    }

}
