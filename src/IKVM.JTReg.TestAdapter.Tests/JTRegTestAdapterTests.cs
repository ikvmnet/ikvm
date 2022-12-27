﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.JTReg.TestAdapter.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.JTReg.TestAdapter.Tests
{

    [TestClass]
    public class JTRegTestAdapterTests
    {

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanDiscoverTests()
        {
            var discoveryContext = new Mock<IDiscoveryContext>();
            var messageLogger = new Mock<IMessageLogger>();
            var testCaseDiscoverySink = new Mock<ITestCaseDiscoverySink>();
            var testCases = new List<TestCase>();
            testCaseDiscoverySink.Setup(x => x.SendTestCase(It.IsAny<TestCase>())).Callback((TestCase x) => testCases.Add(x));

            var adp = new JTRegTestDiscoverer();
            adp.DiscoverTests(new[] { typeof(JTRegTestAdapterTests).Assembly.Location }, discoveryContext.Object, messageLogger.Object, testCaseDiscoverySink.Object);
            testCases.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [TestMethod]
        public void CanExecuteTests()
        {
            var runContext = new Mock<IRunContext>();
            runContext.Setup(x => x.TestRunDirectory).Returns(TestContext.TestRunDirectory);
            var frameworkHandle = new Mock<IFrameworkHandle>();
            var testResults = new ConcurrentBag<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult>();
            frameworkHandle.Setup(x => x.RecordResult(It.IsAny<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult>())).Callback((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult x) => testResults.Add(x));

            var adp = new JTRegTestExecutor();
            adp.RunTests(new[] { typeof(JTRegTestAdapterTests).Assembly.Location }, runContext.Object, frameworkHandle.Object);
            testResults.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [TestMethod]
        public void CurrentOSFamilyShouldBeCorrect()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ((string)JTRegTypes.OS.Current().family).Should().Be("windows");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                ((string)JTRegTypes.OS.Current().family).Should().Be("linux");
        }

    }

}
