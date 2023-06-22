using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Represents a typed argument on a custom attribute.
    /// </summary>
    internal sealed class ReflectionCustomAttributeTypedArgument : ReflectionBase, IManagedCustomAttributeTypedArgument
    {

        readonly CustomAttributeTypedArgument argument;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="argument"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionCustomAttributeTypedArgument(ReflectionBase parent, CustomAttributeTypedArgument argument) :
            base(parent.Context)
        {
            this.argument = argument;
        }

        /// <inhericdoc />
        public IManagedType ArgumentType => Context.Resolve(argument.ArgumentType);

        /// <inhericdoc />
        public object? Value => argument.Value;

    }

}
