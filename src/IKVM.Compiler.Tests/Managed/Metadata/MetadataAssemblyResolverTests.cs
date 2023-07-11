using System;

using IKVM.Compiler.Managed.Metadata;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed.Metadata
{

    [TestClass]
    public class MetadataAssemblyResolverTests
    {

        [TestMethod]
        public void CanResolveAssembly()
        {

            var l = typeof(object).Assembly.Location;
            var r = new MetadataAssemblyContext(new MetadataPathReaderResolver(new[] { l }));
            var a = r.ResolveAssembly(typeof(object).Assembly.GetName());

            foreach (var t in a.ResolveTypes())
            {
                Console.WriteLine(t.Name);
                foreach (var m in t.Methods)
                {
                    Console.Write("   ");
                    Console.Write(m.Name);
                    Console.Write(": ");
                    Console.Write(m.ReturnType);
                    Console.WriteLine();
                }
            }
        }

    }

}
