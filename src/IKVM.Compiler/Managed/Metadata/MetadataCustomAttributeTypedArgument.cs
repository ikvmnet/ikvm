using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents a typed argument on a custom attribute.
    /// </summary>
    internal sealed class MetadataCustomAttributeTypedArgument : MetadataBase, IManagedCustomAttributeTypedArgument
    {

        readonly CustomAttributeTypedArgument<MetadataType> argument;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="argument"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MetadataCustomAttributeTypedArgument(MetadataBase parent, CustomAttributeTypedArgument<MetadataType> argument) :
            base(parent.Context)
        {
            this.argument = argument;
        }

        /// <inhericdoc />
        public IManagedType ArgumentType => Context.ResolveType(argument.Type);

        /// <inhericdoc />
        public object? Value => argument.Value;

    }

}
