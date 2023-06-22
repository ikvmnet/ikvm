using System;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Represents the base class of a definition of an item from System.Reflection.Metadata.
    /// </summary>
    internal abstract class MetadataBase
    {

        readonly MetadataContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataBase(MetadataContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets a reference to the metadata reader that owns this metadata.
        /// </summary>
        public MetadataContext Context => context;

    }

}
