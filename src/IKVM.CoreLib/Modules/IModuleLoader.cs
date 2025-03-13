using System.Collections.Generic;

using IKVM.CoreLib.Modules;

namespace IKVM.CoreLib.Loaders
{

    /// <summary>
    /// Provides an interface for loading modules.
    /// </summary>
    public interface IModuleLoader
    {

        /// <summary>
        /// Loads all of the modules available.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IModule> Load();

        /// <summary>
        /// Loads the named module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<IModule> Load(string name);

    }

}
