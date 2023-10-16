using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using FluentAssertions;

using IKVM.JTReg.TestAdapter.Core;

using javax.xml.bind;

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
            var testCases = new List<JTRegTestCase>();
            var messages = new ConcurrentBag<(JTRegTestMessageLevel, string)>();

            var discoveryContext = new Mock<IJTRegDiscoveryContext>();
            discoveryContext.Setup(x => x.Options).Returns(new JTRegTestOptions());
            discoveryContext.Setup(x => x.SendMessage(It.IsAny<JTRegTestMessageLevel>(), It.IsAny<string>())).Callback((JTRegTestMessageLevel l, string m) => messages.Add((l, m)));
            discoveryContext.Setup(x => x.SendTestCase(It.IsAny<JTRegTestCase>())).Callback((JTRegTestCase x) => testCases.Add(x));

            var testManager = new JTRegTestManager();
            var baseDir = Path.GetDirectoryName(typeof(JTRegTestAdapterTests).Assembly.Location);
            var testDir = Path.Combine(baseDir, "root");
            testManager.DiscoverTestsImpl(Path.Combine(baseDir, "fake"), new[] { testDir }, discoveryContext.Object, CancellationToken.None);
            testCases.Should().HaveCountGreaterThanOrEqualTo(1);
        }

        [TestMethod]
        public void CanExecuteTests()
        {
            var testResults = new ConcurrentBag<JTRegTestResult>();
            var messages = new ConcurrentBag<(JTRegTestMessageLevel, string)>();

            var executionContext = new Mock<IJTRegExecutionContext>();
            executionContext.Setup(x => x.Options).Returns(new JTRegTestOptions());
            executionContext.Setup(x => x.SendMessage(It.IsAny<JTRegTestMessageLevel>(), It.IsAny<string>())).Callback((JTRegTestMessageLevel l, string m) => messages.Add((l, m)));
            executionContext.Setup(x => x.TestRunDirectory).Returns(TestContext.TestRunDirectory);
            executionContext.Setup(x => x.FilterTestCase(It.IsAny<JTRegTestCase>())).Returns(true);
            executionContext.Setup(x => x.RecordResult(It.IsAny<JTRegTestResult>())).Callback((JTRegTestResult x) => testResults.Add(x));

            var testManager = new JTRegTestManager();
            var baseDir = Path.GetDirectoryName(typeof(JTRegTestAdapterTests).Assembly.Location);
            var testDir = Path.Combine(baseDir, "root");
            testManager.RunTestsImpl(Path.Combine(baseDir, "fake"), new[] { testDir }, null, executionContext.Object, null, CancellationToken.None);
            testResults.Should().HaveCountGreaterThanOrEqualTo(1);
        }

    }

}
