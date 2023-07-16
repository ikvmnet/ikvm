using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Writer.Reflection
{

    /// <summary>
    /// Implements the <see cref="IManagedModuleWriter"/> interface through System.Reflection.Emit.
    /// </summary>
    internal class ReflectionModuleWriter : IManagedModuleWriter
    {

        public IManagedMethodWriter DefineMethod(string name, MethodAttributes attributes, IManagedTypeWriter types)
        {
            throw new NotImplementedException();
        }

    }

}
