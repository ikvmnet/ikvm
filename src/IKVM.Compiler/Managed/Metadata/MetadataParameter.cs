using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataParameter : MetadataBase, IManagedParameter
    {

        readonly MetadataMethod method;
        readonly ParameterHandle handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="handle"></param>
        internal MetadataParameter(MetadataMethod method, ParameterHandle handle) :
            base(method.Context)
        {
            this.method = method;
            this.handle = handle;
        }

        Parameter Data => Context.Reader.GetParameter(handle);

        /// <inheritdoc />
        public string Name => Context.Reader.GetString(Data.Name);

        /// <inheritdoc />
        public ManagedTypeSignature ParameterType => throw new NotImplementedException();

    }

}
