using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    struct ImportsEncoder
    {

        readonly BlobBuilder writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportsEncoder(BlobBuilder writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void ImportNamespace(BlobHandle namespaceName)
        {
            // <import> ::= ImportNamespace <target-namespace>
            writer.WriteByte((byte)ImportDefinitionKind.ImportNamespace);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

    }

}
