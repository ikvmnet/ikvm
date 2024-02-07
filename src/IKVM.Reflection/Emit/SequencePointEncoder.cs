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
        public void LocalSignature(StandaloneSignatureHandle localSignature)
        {
            if (localSignatureEncoded)
                throw new InvalidOperationException("Local Signature already encoded.");
            if (sequencePointEncoded)
                throw new InvalidOperationException("Sequence point already encoded.");

            // write local signature
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(localSignature));

            localSignatureEncoded = true;
        }

        /// <summary>
        /// Encodes a new sequence point. <paramref name="previousDocument"/> is used to determine whether a new document record is required to be written first.
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
            if (localSignatureEncoded == false)
                throw new InvalidOperationException("Local signature not already encoded.");

            // document passed, and different from previous, write new document-record
            if (document.IsNil == false && document != previousDocument)
            {
                // this isn't the initial header document, but a document-record, which requires a zero IL offset
                if (sequencePointEncoded)
                    writer.WriteCompressedInteger(0);

                writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(document));
                previousDocument = document;
            }

            // IL offset or delta IL offset
            writer.WriteCompressedInteger(prevOffset > -1 ? offset - prevOffset : offset);
            prevOffset = offset;

            if (startLine == System.Reflection.Metadata.SequencePoint.HiddenLine)
            {
                writer.WriteInt16(0);
            }
            else
            {
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

            sequencePointEncoded = true;
        }

        void SerializeDeltaLinesAndColumns(int startLine, int startColumn, int endLine, int endColumn)
        {
            var deltaLines = endLine - startLine;
            var deltaColumns = endColumn - startColumn;
            Debug.Assert(deltaLines != 0 || deltaColumns != 0 || startLine == System.Reflection.Metadata.SequencePoint.HiddenLine);

            writer.WriteCompressedInteger(deltaLines);

            if (deltaLines == 0)
                writer.WriteCompressedInteger(deltaColumns);
            else
                writer.WriteCompressedSignedInteger(deltaColumns);
        }

    }

}
