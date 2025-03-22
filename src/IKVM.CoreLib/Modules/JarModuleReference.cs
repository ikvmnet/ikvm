using System;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Represents a reference to a modular JAR file.
    /// </summary>
    class JarModuleReference : ModuleReference
    {

        readonly RuntimeVersion _runtimeVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="file"></param>
        public JarModuleReference(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string file) :
            base(descriptor, Path.GetFullPath(file))
        {
            _runtimeVersion = runtimeVersion;
        }

        /// <inheritdoc />
        public override ModuleReader Open() => new JarModuleReader(Location ?? throw new InvalidOperationException(), _runtimeVersion);

    }

}
