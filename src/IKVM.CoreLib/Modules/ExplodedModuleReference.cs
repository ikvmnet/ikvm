using System;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Represents a reference to a modular JAR file.
    /// </summary>
    class ExplodedModuleReference : ModuleReference
    {

        readonly ModuleDescriptor _descriptor;
        readonly RuntimeVersion _runtimeVersion;
        readonly string _dir;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="dir"></param>
        public ExplodedModuleReference(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string dir)
        {
            _descriptor = descriptor;
            _runtimeVersion = runtimeVersion;
            _dir = dir ?? throw new ArgumentNullException(nameof(dir));
            _dir = Path.GetFullPath(_dir);
        }

        /// <inheritdoc />
        public override ModuleDescriptor Descriptor => _descriptor;

        /// <inheritdoc />
        public override string? Location => _dir;

        /// <inheritdoc />
        public override ModuleReader Open() => new ExplodedModuleReader(_dir, _runtimeVersion);

    }

}
