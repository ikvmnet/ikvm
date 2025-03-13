using System.Collections.Immutable;

namespace IKVM.CoreLib.Loaders
{

    /// <summary>
    /// Represents a classpath.
    /// </summary>
    public class LoaderClassPath
    {

        readonly ImmutableArray<LoaderClassPathEntry> _entries;

    }

}
