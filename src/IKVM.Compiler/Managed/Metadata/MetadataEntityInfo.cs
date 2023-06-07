using System;

namespace IKVM.Compiler.Managed.Metadata
{

    internal abstract class MetadataEntityInfo
    {

        readonly MetadataContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataEntityInfo(MetadataContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets a reference to the metadata reader.
        /// </summary>
        public MetadataContext Context => context;

    }

}
