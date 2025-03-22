using System;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Represents a reference to a modular JAR file.
    /// </summary>
    class ExplodedModuleReference : ModuleReference
    {

        readonly RuntimeVersion _runtimeVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="dir"></param>
        public ExplodedModuleReference(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string dir) :
            base(descriptor, Path.GetFullPath(dir))
        {
            _runtimeVersion = runtimeVersion;
        }

        /// <inheritdoc />
        public override ModuleReader Open() => new ExplodedModuleReader(Location ?? throw new InvalidOperationException(), _runtimeVersion);

    }

}
