using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKVM.CoreLib.Modules
{

    public interface IModule
    {

        /// <summary>
        /// Gets the descriptor of the module.
        /// </summary>
        public ModuleDescriptor Descriptor { get; }

        /// <summary>
        /// Gets the set of packages that make up the module.
        /// </summary>
        //public IEnumerable<IPackage> Packages { get; }

    }

}
