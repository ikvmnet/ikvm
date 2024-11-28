using System.Collections.Immutable;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Tests.Symbols.Emit;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.CoreLib.Tests.Symbols.Reflection.Emit
{

    [TestClass]
    public class ReflectionAssemblyBuilderTests : AssemblySymbolBuilderTests<ReflectionSymbolTestInit, ReflectionSymbolContext>
    {

        protected override AssemblySymbolBuilder DefineAssembly(AssemblyIdentity identity, ImmutableArray<CustomAttribute> customAttributes)
        {
            return Init.Symbols.DefineAssembly(identity, customAttributes);
        }

        [TestMethod]
        public void Nothing()
        {

        }

    }

}
