using System.Collections.Generic;

using FluentAssertions;

using IKVM.JavaTest;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

using Moq;

namespace IKVM.JTReg.TestAdapter.Console
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            Can_locate_test();
        }

        public static void Can_locate_test()
        {
            var discoveryContext = new Mock<IDiscoveryContext>();
            var messageLogger = new Mock<IMessageLogger>();
            var testCaseDiscoverySink = new Mock<ITestCaseDiscoverySink>();

            var testCases = new List<TestCase>();
            testCaseDiscoverySink.Setup(x => x.SendTestCase(It.IsAny<TestCase>())).Callback((TestCase x) => testCases.Add(x));

            var adp = new JTRegTestAdapter();
            adp.DiscoverTests(new[] { typeof(Program).Assembly.Location }, discoveryContext.Object, messageLogger.Object, testCaseDiscoverySink.Object);
            testCases.Should().HaveCountGreaterThanOrEqualTo(1);
        }

    }

}