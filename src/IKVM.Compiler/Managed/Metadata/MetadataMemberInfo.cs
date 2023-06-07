using System;

namespace IKVM.Compiler.Managed.Metadata
{

    internal abstract class MetadataMemberInfo : MetadataEntityInfo, IManagedMemberInfo
    {

        readonly MetadataTypeInfo declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataMemberInfo(MetadataTypeInfo declaringType) :
            base(declaringType.Context)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        public IManagedTypeInfo DeclaringType => declaringType;

        public abstract string Name { get; }

    }

}
