using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Testing.Platform.Extensions;

namespace IKVM.JTReg.TestAdapter.TestingPlatform
{

    sealed class JTRegExtension : IExtension
    {

        static string GetExtensionUid()
        {
            foreach (var attr in Assembly.GetEntryAssembly()?.GetCustomAttributes<AssemblyMetadataAttribute>())
                if (attr.Key == "JTReg.Extension.Uid")
                    return attr.Value;

            return nameof(JTRegExtension);
        }

        public string Uid { get; } = GetExtensionUid();

        public string DisplayName => "MSTest";

        public string Version => "1.0.0";

        public string Description => "JTReg for Microsoft Testing Platform";

        public Task<bool> IsEnabledAsync() => Task.FromResult(true);

    }

}
