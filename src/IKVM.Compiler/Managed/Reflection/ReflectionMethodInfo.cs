using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedMethodInfo"/> by reflecting against an existing .NET method.
    /// </summary>
    internal sealed class ReflectionMethodInfo : ReflectionMemberInfo, IManagedMethodInfo
    {

        readonly MethodInfo method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionMethodInfo(ReflectionTypeInfo declaringType, MethodInfo method) :
            base(declaringType, method)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
        }

    }

}
