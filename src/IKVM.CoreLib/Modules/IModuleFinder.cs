using System.Collections.Immutable;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// A finder of modules. A <see cref="ModuleFinder"/> is used to find modules during resolution or service binding.
    /// </summary>
    internal interface IModuleFinder
    {

        /// <summary>
        /// Finds a reference to a module of a given name.
        /// 
        /// A  provides a consistent view of the modules that it locates. If <see
        /// cref="Find(string)"/> is invoked several times to locate the same module (by name) then it will return the
        /// same result each time. If a module is located then it is guaranteed to be a member of the set of modules
        /// returned by the <see cref="FindAll"/> method.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="FindException" />
        ModuleReference? Find(string name);

        /// <summary>
        /// Returns the set of all module references that this finder can locate.
        /// 
        /// A <see cref="IModuleFinder"/> provides a consistent view of the modules that it locates. If <see cref="FindAll"/> is
        /// invoked several times then it will return the same (equals) result each time. For each <see cref="ModuleReference"/>
        /// element in the returned set then it is guaranteed that find will locate the <see cref="ModuleReference"/>
        /// if invoked to find that module.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FindException" />
        ImmutableHashSet<ModuleReference> FindAll();

    }

}
