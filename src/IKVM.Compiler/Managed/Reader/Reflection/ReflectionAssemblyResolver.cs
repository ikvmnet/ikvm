using System.Reflection;

namespace IKVM.Compiler.Managed.Reader.Reflection
{

    internal class ReflectionAssemblyResolver : IReflectionAssemblyResolver
    {

        public Assembly? Resolve(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                return null;
            }
        }

    }

}
