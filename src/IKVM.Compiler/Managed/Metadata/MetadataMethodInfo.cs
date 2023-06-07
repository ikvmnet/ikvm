using System;
using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Metadata
{

    internal sealed class MetadataMethodInfo : MetadataMemberInfo, IManagedMethodInfo
    {

        readonly MetadataTypeInfo declaringType;
        readonly MethodDefinition method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal MetadataMethodInfo(MetadataTypeInfo declaringType, MethodDefinition method) :
            base(declaringType)
        {
            this.declaringType = this.declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            this.method = method;
        }

        public override string Name => Context.Reader.GetString(method.Name);

    }

}
