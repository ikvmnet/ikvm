using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Compiler.Managed;
using IKVM.Compiler.Managed.Metadata;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed.Metadata
{

    [TestClass]
    public class MetadataAssemblyResolverTests
    {

        [TestMethod]
        public async Task CanResolveAssembly()
        {

            var d = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var r = new MetadataAssemblyResolver(new MetadataAssemblyPathLoader(new[] { d }));
            var a = await r.ResolveAsync(typeof(object).Assembly.GetName());
            var t = a.ResolveType("System.Object");
            //var m = t.Methods.Resolve("Equals");
            //m.Name.Should().Be("Equals");

            foreach (var tt in a.ResolveTypes())
                Console.WriteLine(tt);
        }

    }

}
