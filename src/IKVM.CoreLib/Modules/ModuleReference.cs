using System.IO;

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

        /// <summary>
        /// Returns the module descriptor.
        /// </summary>
        public abstract ModuleDescriptor Descriptor { get; }

        /// <summary>
        /// Returns the location of this module's content, if known.
        /// </summary>
        public abstract string? Location { get; }

        /// <summary>
        /// Opens the module content for reading.
        /// </summary>
        /// <returns></returns>
        public abstract ModuleReader Open();

    }

}
