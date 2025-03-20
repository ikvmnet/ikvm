using System;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Represents a reference to a modular JAR file.
    /// </summary>
    class JarModuleReference : ModuleReference
    {

        readonly ModuleDescriptor _descriptor;
        readonly RuntimeVersion _runtimeVersion;
        readonly string _file;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="file"></param>
        public JarModuleReference(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string file)
        {
            _descriptor = descriptor;
            _runtimeVersion = runtimeVersion;
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _file = Path.GetFullPath(_file);
        }

        /// <inheritdoc />
        public override ModuleDescriptor Descriptor => _descriptor;

        /// <inheritdoc />
        public override string? Location => _file;

        /// <inheritdoc />
        public override ModuleReader Open() => new JarModuleReader(_file, _runtimeVersion);

    }

}
