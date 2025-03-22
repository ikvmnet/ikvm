using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IKVM.CoreLib.Modules;

namespace IKVM.CoreLib.Tests.Modules
{

    internal static class ModuleTestUtil
    {

        public static IModuleFinder FinderOf(IEnumerable<ModuleDescriptor> descriptors)
        {
            return new InMemoryModuleFinder(descriptors.Select(i => new InMemoryModuleReference(i, null)).ToImmutableArray<ModuleReference>());
        }

    }

}
