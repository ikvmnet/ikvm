using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Testing.Extensions.VSTestBridge;
using Microsoft.Testing.Extensions.VSTestBridge.Requests;
using Microsoft.Testing.Platform.Capabilities.TestFramework;
using Microsoft.Testing.Platform.Messages;

namespace IKVM.JTReg.TestAdapter.TestingPlatform
{

    sealed class JTRegBridgedTestFramework : SynchronizedSingleSessionVSTestBridgedTestFramework
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="getTestAssemblies"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="capabilities"></param>
        public JTRegBridgedTestFramework(JTRegExtension extension, Func<IEnumerable<Assembly>> getTestAssemblies, IServiceProvider serviceProvider, ITestFrameworkCapabilities capabilities) :
            base(extension, getTestAssemblies, serviceProvider, capabilities)
        {

        }

        /// <inheritdoc />
        protected override Task SynchronizedDiscoverTestsAsync(VSTestDiscoverTestExecutionRequest request, IMessageBus messageBus, CancellationToken cancellationToken)
        {
            if (Environment.GetEnvironmentVariable("JTREG_DEBUG_DISCOVERTESTS") == "1" && !Debugger.IsAttached)
                Debugger.Launch();

            new JTRegTestDiscoverer().DiscoverTests(request.AssemblyPaths, request.DiscoveryContext, request.MessageLogger, request.DiscoverySink);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override Task SynchronizedRunTestsAsync(VSTestRunTestExecutionRequest request, IMessageBus messageBus, CancellationToken cancellationToken)
        {
            if (Environment.GetEnvironmentVariable("JTREG_DEBUG_RUNTESTS") == "1" && !Debugger.IsAttached)
                Debugger.Launch();

            if (request.VSTestFilter.TestCases is { } testCases)
                new JTRegTestExecutor().RunTests(testCases, request.RunContext, request.FrameworkHandle, cancellationToken);
            else
                new JTRegTestExecutor().RunTests(request.AssemblyPaths, request.RunContext, request.FrameworkHandle, cancellationToken);

            return Task.CompletedTask;
        }

    }

}
