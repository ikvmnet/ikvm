using System;
using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedMethod"/> by reflecting against an existing .NET method.
    /// </summary>
    internal sealed class ReflectionMethod : ReflectionMember, IManagedMethod
    {

        readonly MethodInfo method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionMethod(ReflectionType declaringType, MethodInfo method) :
            base(declaringType, method)
        {
            this.method = method;
        }

        public ManagedTypeSignature ReturnType => throw new NotImplementedException();

        public IReadOnlyList<IManagedParameter> Parameters => throw new NotImplementedException();

    }

}
