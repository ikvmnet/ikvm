using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Maintains a context for a specific assembly file.
    /// </summary>
    internal class MetadataContext
    {

        readonly MetadataContextResolver resolver;
        readonly MetadataReader reader;
        readonly MetadataAssemblyInfo assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="reader"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MetadataContext(MetadataContextResolver context, MetadataReader reader)
        {
            this.resolver = context ?? throw new ArgumentNullException(nameof(context));
            this.reader = reader ?? throw new ArgumentNullException(nameof(reader));

            assembly = new MetadataAssemblyInfo(this, reader.GetAssemblyDefinition());
        }

        /// <summary>
        /// Gets the <see cref="MetadataReader"/> associated with the context.
        /// </summary>
        public MetadataReader Reader => reader;

        /// <summary>
        /// Gets the assembly that defines this context.
        /// </summary>
        public MetadataAssemblyInfo Assembly => assembly;

        /// <summary>
        /// Resolves a context based on the specified assembly name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MetadataContext Resolve(AssemblyName name) => assembly.Name == name ? this : resolver.Resolve(name);

    }

}
