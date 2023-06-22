using System;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="IManagedField"/> by reflecting against an existing .NET field.
    /// </summary>
    internal sealed class ReflectionCustomAttribute : ReflectionBase, IManagedCustomAttribute
    {

        readonly ReflectionBase parent;
        readonly CustomAttributeData customAttribute;

        readonly ReadOnlyListMap<CustomAttributeTypedArgument, IManagedCustomAttributeTypedArgument> constructorArguments;
        readonly ReadOnlyListMap<CustomAttributeNamedArgument, IManagedCustomAttributeNamedArgument> namedArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="customAttribute"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionCustomAttribute(ReflectionBase parent, CustomAttributeData customAttribute) :
            base(parent.Context)
        {
            this.parent = parent;
            this.customAttribute = customAttribute;

            constructorArguments = customAttribute.ConstructorArguments.Map((a, i) => new ReflectionCustomAttributeTypedArgument(this, a));
            namedArguments = customAttribute.NamedArguments.Map((a, i) => new ReflectionCustomAttributeNamedArgument(this, a));
        }

        public IManagedType AttributeType => Context.Resolve(customAttribute.AttributeType);

        public IManagedMethod Constructor => Context.Resolve(customAttribute.Constructor);

        public IReadOnlyList<IManagedCustomAttributeTypedArgument> ConstructorArguments => throw new NotImplementedException();

        public IReadOnlyList<IManagedCustomAttributeNamedArgument> NamedArguments => throw new NotImplementedException();

    }

}
