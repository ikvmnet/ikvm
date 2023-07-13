using System;

using FluentAssertions;

using IKVM.Compiler.Managed.Reader.Metadata;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed.Reader.Metadata
{

    [TestClass]
    public class MetadataAssemblyResolverTests
    {

        [TestMethod]
        public void ShouldReturnNullOnMissingAssemblyResolve()
        {
            var r = new MetadataAssemblyContext(new MetadataPathReaderResolver(new[] { typeof(object).Assembly.Location }), typeof(void).Assembly.GetName().Name);
            r.ResolveAssembly("Missing.Exception").Should().BeNull();
        }

        [TestMethod]
        public void ShouldReturnNullOnMissingTypeResolve()
        {
            var r = new MetadataAssemblyContext(new MetadataPathReaderResolver(new[] { typeof(object).Assembly.Location }), typeof(void).Assembly.GetName().Name);
            var a = r.ResolveAssembly(typeof(object).Assembly.GetName().Name);
            var t = a.ResolveType("Missing.Type").Should().BeNull();
        }

        [TestMethod]
        public void CanResolveAssembly()
        {
            var l = typeof(object).Assembly.Location;
            var r = new MetadataAssemblyContext(new MetadataPathReaderResolver(new[] { typeof(object).Assembly.Location }), typeof(void).Assembly.GetName().Name);
            var a = r.ResolveAssembly(typeof(object).Assembly.GetName().Name);

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
