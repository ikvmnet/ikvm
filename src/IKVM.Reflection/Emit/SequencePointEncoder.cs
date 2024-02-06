using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Helper to encode sequence points.
    /// </summary>
    struct SequencePointEncoder
    {

        readonly BlobBuilder writer;

        bool headerEncoded = false;
        bool sequencePointEncoded = false;
        int prevOffset = -1;
        int prevNonHiddenStartLine = -1;
        int prevNonHiddenStartColumn = -1;

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public SequencePointEncoder(BlobBuilder writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Encodes the local signature.
        /// </summary>
        /// <param name="localSignature"></param>
        /// <param name="initialDocument"></param>
        /// <param name="previousDocument".
        public void Header(StandaloneSignatureHandle localSignature, DocumentHandle initialDocument, ref DocumentHandle previousDocument)
        {
            if (headerEncoded)
                throw new InvalidOperationException("Local Signature already encoded.");
            if (sequencePointEncoded)
                throw new InvalidOperationException("Sequence point already encoded.");

            headerEncoded = true;

            // write local signature
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(localSignature));

            // write optional initial document and set as previous
            if (initialDocument.IsNil == false && initialDocument != previousDocument)
            {
                writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(initialDocument));
                previousDocument = initialDocument;
            }
        }

        /// <summary>
        /// Encodes a new sequence point.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="offset"></param>
        /// <param name="startLine"></param>
        /// <param name="startColumn"></param>
        /// <param name="endLine"></param>
        /// <param name="endColumn"></param>
        /// <param name="previousDocument"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SequencePoint(DocumentHandle document, int offset, int startLine, int startColumn, int endLine, int endColumn, ref DocumentHandle previousDocument)
        {
            if (headerEncoded == false)
                throw new InvalidOperationException("Header not already encoded.");

            sequencePointEncoded = true;

            // document passed, and different from previous, write new document-record
            if (document.IsNil == false && document != previousDocument)
            {
                writer.WriteCompressedInteger(0);
                writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(document));
                previousDocument = document;
            }

            // delta IL offset
            writer.WriteCompressedInteger(prevOffset > -1 ? offset - prevOffset : offset);
            prevOffset = offset;

            if (startLine == System.Reflection.Metadata.SequencePoint.HiddenLine)
            {
                writer.WriteInt16(0);
                return;
            }

            // Delta Lines & Columns:
            SerializeDeltaLinesAndColumns(startLine, startColumn, endLine, endColumn);

            // delta Start Lines & Columns:
            if (prevNonHiddenStartLine < 0)
            {
                Debug.Assert(prevNonHiddenStartColumn < 0);
                writer.WriteCompressedInteger(startLine);
                writer.WriteCompressedInteger(startColumn);
            }
            else
            {
                writer.WriteCompressedSignedInteger(startLine - prevNonHiddenStartLine);
                writer.WriteCompressedSignedInteger(startColumn - prevNonHiddenStartColumn);
            }

            prevNonHiddenStartLine = startLine;
            prevNonHiddenStartColumn = startColumn;
        }

        void SerializeDeltaLinesAndColumns(int startLine, int startColumn, int endLine, int endColumn)
        {
            int deltaLines = endLine - startLine;
            int deltaColumns = endColumn - startColumn;

            // only hidden sequence points have zero width
            Debug.Assert(deltaLines != 0 || deltaColumns != 0 || startLine == System.Reflection.Metadata.SequencePoint.HiddenLine);

            writer.WriteCompressedInteger(deltaLines);

            if (deltaLines == 0)
                writer.WriteCompressedInteger(deltaColumns);
            else
                writer.WriteCompressedSignedInteger(deltaColumns);
        }

    }

}
