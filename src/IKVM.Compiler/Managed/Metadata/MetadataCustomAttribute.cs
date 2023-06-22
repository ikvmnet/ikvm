using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataCustomAttribute : MetadataBase, IManagedCustomAttribute
    {

        readonly MetadataBase parent;
        readonly CustomAttributeHandle handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="handle"></param>
        internal MetadataCustomAttribute(MetadataBase parent, CustomAttributeHandle handle) :
            base(parent.Context)
        {
            this.parent = parent;
            this.handle = handle;
        }

        CustomAttribute Data => Context.Reader.GetCustomAttribute(handle);

        public IManagedMethod Constructor => throw new System.NotImplementedException();

        public IReadOnlyList<IManagedCustomAttributeTypedArgument> ConstructorArguments => throw new System.NotImplementedException();

        public IReadOnlyList<IManagedCustomAttributeNamedArgument> NamedArguments => throw new System.NotImplementedException();

    }

}
