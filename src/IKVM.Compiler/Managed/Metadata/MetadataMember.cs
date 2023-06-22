using System;
using System.Collections.Generic;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents a member from metadata.
    /// </summary>
    internal abstract class MetadataMember : MetadataBase, IManagedMember
    {

        internal readonly MetadataType declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataMember(MetadataType declaringType) :
            base(declaringType.Context)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <inheritdoc />
        public IManagedType DeclaringType => declaringType;

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract IReadOnlyList<IManagedCustomAttribute> CustomAttributes { get; }

    }

}
