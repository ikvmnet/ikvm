using System.Collections.Immutable;

namespace IKVM.CoreLib.Loaders
{

    /// <summary>
    /// Describes a source of class or module items.
    /// </summary>
    public class Loader
    {

        readonly ImmutableArray<LoaderClassPathEntry> _classPath;
        readonly ImmutableArray<LoaderModulePathEntry> _modulePath;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="classPath"></param>
        /// <param name="modulePath"></param>
        public Loader(ImmutableArray<LoaderClassPathEntry> classPath, ImmutableArray<LoaderModulePathEntry> modulePath)
        {
            _classPath = classPath;
            _modulePath = modulePath;
        }

    }

}
