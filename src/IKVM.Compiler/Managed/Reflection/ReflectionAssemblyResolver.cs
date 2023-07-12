using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
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
