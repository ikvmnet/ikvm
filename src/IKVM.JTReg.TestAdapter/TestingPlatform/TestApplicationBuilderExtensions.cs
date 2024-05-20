using System.Collections.Generic;
using System;
using System.Reflection;

using Microsoft.Testing.Extensions.VSTestBridge.Capabilities;
using Microsoft.Testing.Extensions.VSTestBridge.Helpers;
using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Capabilities.TestFramework;

namespace IKVM.JTReg.TestAdapter.TestingPlatform
{

    public static class TestApplicationBuilderExtensions
    {

        public static void AddJTReg(this ITestApplicationBuilder testApplicationBuilder, Func<IEnumerable<Assembly>> getTestAssemblies)
        {
            var extension = new JTRegExtension();
            testApplicationBuilder.AddRunSettingsService(extension);
            testApplicationBuilder.AddTestCaseFilterService(extension);
            testApplicationBuilder.RegisterTestFramework(_ => new TestFrameworkCapabilities(new VSTestBridgeExtensionBaseCapabilities()), (capabilities, serviceProvider) => new JTRegBridgedTestFramework(extension, getTestAssemblies, serviceProvider, capabilities));
        }

    }

}
