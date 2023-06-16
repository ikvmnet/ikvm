using System;

namespace IKVM.Compiler.Managed.Metadata
{

    internal abstract class MetadataMemberDefinition : MetadataEntityDefinition, IManagedMemberDefinition
    {

        readonly MetadataTypeDefinition declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataMemberDefinition(MetadataTypeDefinition declaringType) :
            base(declaringType.Context)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        public IManagedTypeDefinition DeclaringType => declaringType;

        public abstract string Name { get; }

    }

}
