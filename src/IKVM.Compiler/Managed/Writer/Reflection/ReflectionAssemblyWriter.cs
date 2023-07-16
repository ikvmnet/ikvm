using System;

namespace IKVM.Compiler.Managed.Writer.Reflection
{
    
    /// <summary>
    /// Implements the <see cref="IManagedAssemblyWriter"/> interface through System.Reflection.Emit.
    /// </summary>
    internal class ReflectionAssemblyWriter : IManagedAssemblyWriter
    {

        public IManagedModuleWriter DefineModule(string name)
        {
            throw new NotImplementedException();
        }

    }

}
