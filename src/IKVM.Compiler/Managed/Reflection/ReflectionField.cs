using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="ManagedField"/> by reflecting against an existing .NET field.
    /// </summary>
    internal sealed class ReflectionField : ReflectionMember, ManagedField
    {

        readonly FieldInfo field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="field"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionField(ReflectionType declaringType, FieldInfo field) :
            base(declaringType, field)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
        }

    }

}
