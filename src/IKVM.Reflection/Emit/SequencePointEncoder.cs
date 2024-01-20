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

        bool localSignatureEncoded = false;
        DocumentHandle prevDocument = default;
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
        /// <param name="handle"></param>
        public void LocalSignature(StandaloneSignatureHandle handle)
        {
            if (localSignatureEncoded)
                throw new InvalidOperationException("Local Signature already encoded.");

            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(handle));
            localSignatureEncoded = true;
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
        /// <exception cref="InvalidOperationException"></exception>
        public void SequencePoint(DocumentHandle document, int offset, int startLine, int startColumn, int endLine, int endColumn)
        {
            if (localSignatureEncoded == false)
                throw new InvalidOperationException("Local Signature not already encoded.");

            if (document != prevDocument)
            {
                // optional document in header or document record
                if (prevDocument.IsNil == false)
                    writer.WriteCompressedInteger(0);

                writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(document));
                prevDocument = document;
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
