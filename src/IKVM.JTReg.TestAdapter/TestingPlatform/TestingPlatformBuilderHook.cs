using System.Reflection;

using Microsoft.Testing.Platform.Builder;

namespace IKVM.JTReg.TestAdapter.TestingPlatform
{

    public static class TestingPlatformBuilderHook
    {

        public static void AddExtensions(ITestApplicationBuilder testApplicationBuilder, string[] arguments) => testApplicationBuilder.AddJTReg(() => new[] { Assembly.GetEntryAssembly() });

    }

}
