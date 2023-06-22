using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Represents a named argument on a custom attribute.
    /// </summary>
    internal sealed class ReflectionCustomAttributeNamedArgument : ReflectionBase, IManagedCustomAttributeNamedArgument
    {

        readonly CustomAttributeNamedArgument argument;
        readonly ReflectionCustomAttributeTypedArgument typedArgument;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="argument"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionCustomAttributeNamedArgument(ReflectionBase parent, CustomAttributeNamedArgument argument) :
            base(parent.Context)
        {
            this.argument = argument;

            typedArgument = new ReflectionCustomAttributeTypedArgument(this, argument.TypedValue);
        }

        public bool IsField => argument.IsField;

        public IManagedMember Member => Context.Resolve(argument.MemberInfo);

        public string MemberName => argument.MemberName;

        public IManagedCustomAttributeTypedArgument TypedValue => typedArgument;

    }

}
