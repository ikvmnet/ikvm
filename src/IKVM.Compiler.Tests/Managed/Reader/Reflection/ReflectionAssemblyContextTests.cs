using System;

using IKVM.Compiler.Managed.Reader.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed.Reader.Reflection
{

    [TestClass]
    public class ReflectionAssemblyContextTests
    {

        [TestMethod]
        public void CanResolveAssembly()
        {
            var r = new ReflectionReaderContext(new ReflectionAssemblyResolver());
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

        [TestMethod]
        public void Can()
        {

        }

    }

}
