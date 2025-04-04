using System;
using System.Collections.Generic;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Provides access to the content of a module.
    /// </summary>
    abstract class ModuleReader : IDisposable
    {

        /// <summary>
        /// Opens a resource, returning an input stream to read the resource in the module.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract Stream? Open(string name);

        /// <summary>
        /// Lists the contents of the module, returning a stream of elements that are the names of all resources in the module.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<string> List();

        /// <inheritdoc />
        public abstract void Dispose();

    }

}
