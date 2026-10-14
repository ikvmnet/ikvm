using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Encodes the imports blob of a portable PDB import scope.
    /// </summary>
    struct ImportsEncoder
    {

        readonly BlobBuilder _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer">The blob builder the imports are written to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public ImportsEncoder(BlobBuilder writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Writes an import of a namespace.
        /// </summary>
        /// <param name="namespaceName">The blob heap handle of the UTF-8 encoded namespace name.</param>
        public void ImportNamespace(BlobHandle namespaceName)
        {
            // <import> ::= ImportNamespace <target-namespace>
            _writer.WriteByte((byte)ImportDefinitionKind.ImportNamespace);
            _writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

    }

}
