using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    struct ImportsEncoder
    {

        readonly BlobBuilder _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportsEncoder(BlobBuilder writer)
        {
            this._writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void ImportNamespace(BlobHandle namespaceName)
        {
            // <import> ::= ImportNamespace <target-namespace>
            _writer.WriteByte((byte)ImportDefinitionKind.ImportNamespace);
            _writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

    }

}
