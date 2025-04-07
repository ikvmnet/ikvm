namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A reference to a module's content.
    /// </summary>
    internal abstract class ModuleReference
    {

        /// <summary>
        /// Creates a <see cref="ModuleReference"/> to a possibly-patched module in a modular JAR.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ModuleReference CreateJarModule(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string path)
        {
            return new JarModuleReference(descriptor, runtimeVersion, path);
        }

        /// <summary>
        /// Creates a <see cref="ModuleReference"/> to a possibly-patched module in a modular JAR.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="runtimeVersion"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ModuleReference CreateExplodedModule(ModuleDescriptor descriptor, RuntimeVersion runtimeVersion, string path)
        {
            return new ExplodedModuleReference(descriptor, runtimeVersion, path);
        }

        readonly ModuleDescriptor _descriptor;
        readonly string? _location;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="location"></param>
        public ModuleReference(ModuleDescriptor descriptor, string? location)
        {
            _descriptor = descriptor;
            _location = location;
        }

        /// <summary>
        /// Returns the module descriptor.
        /// </summary>
        public ModuleDescriptor Descriptor => _descriptor;

        /// <summary>
        /// Returns the location of this module's content, if known.
        /// </summary>
        public string? Location => _location;

        /// <summary>
        /// Opens the module content for reading.
        /// </summary>
        /// <returns></returns>
        public abstract ModuleReader Open();

    }

}
