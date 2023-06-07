using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedFieldInfo"/> by reflecting against an existing .NET field.
    /// </summary>
    internal sealed class ReflectionFieldInfo : ReflectionMemberInfo, IManagedFieldInfo
    {

        readonly FieldInfo field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionFieldInfo(ReflectionTypeInfo declaringType, FieldInfo field) :
            base(declaringType, field)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

    }

}
