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

        readonly BlobBuilder _writer;

        bool _localSignatureEncoded = false;
        bool _sequencePointEncoded = false;
        int _prevOffset = -1;
        int _prevNonHiddenStartLine = -1;
        int _prevNonHiddenStartColumn = -1;

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        public SequencePointEncoder(BlobBuilder writer)
        {
            this._writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Encodes the local signature.
        /// </summary>
        /// <param name="localSignature"></param>
        /// <param name="initialDocument"></param>
        /// <param name="previousDocument".
        public void LocalSignature(StandaloneSignatureHandle localSignature)
        {
            if (_localSignatureEncoded)
                throw new InvalidOperationException("Local Signature already encoded.");
            if (_sequencePointEncoded)
                throw new InvalidOperationException("Sequence point already encoded.");

            // write local signature
            _writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(localSignature));

            _localSignatureEncoded = true;
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
            if (_localSignatureEncoded == false)
                throw new InvalidOperationException("Local signature not already encoded.");
            if (offset <= _prevOffset)
                throw new ArgumentException("Subsequent sequence points must appear at an offset greater than the previous sequence point.");

            // document passed, and different from previous, write new document-record
            if (document.IsNil == false && document != previousDocument)
            {
                // this isn't the initial header document, but a document-record, which requires a zero IL offset
                if (_sequencePointEncoded)
                    _writer.WriteCompressedInteger(0);

                _writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(document));
                previousDocument = document;
            }

            // IL offset or delta IL offset
            if (_prevOffset == -1)
                _writer.WriteCompressedInteger(offset);
            else
                _writer.WriteCompressedInteger(offset - _prevOffset);

            // hidden or non-hidden sequence point
            if (startLine == System.Reflection.Metadata.SequencePoint.HiddenLine)
            {
                _writer.WriteCompressedInteger(0);
                _writer.WriteCompressedInteger(0);
            }
            else
            {
                var deltaLines = endLine - startLine;
                var deltaColumns = endColumn - startColumn;
                Debug.Assert(startLine == System.Reflection.Metadata.SequencePoint.HiddenLine || deltaLines > 0 || deltaColumns > 0);

                // write change in lines; then change in columns (signed if change in lines)
                _writer.WriteCompressedInteger(deltaLines);
                if (deltaLines == 0)
                    _writer.WriteCompressedInteger(deltaColumns);
                else
                    _writer.WriteCompressedSignedInteger(deltaColumns);

                // delta start lines & columns
                if (_prevNonHiddenStartLine == -1)
                {
                    Debug.Assert(_prevNonHiddenStartColumn == -1);
                    _writer.WriteCompressedInteger(startLine);
                    _writer.WriteCompressedInteger(startColumn);
                }
                else
                {
                    _writer.WriteCompressedSignedInteger(startLine - _prevNonHiddenStartLine);
                    _writer.WriteCompressedSignedInteger(startColumn - _prevNonHiddenStartColumn);
                }

                _prevNonHiddenStartLine = startLine;
                _prevNonHiddenStartColumn = startColumn;
            }

            _prevOffset = offset;
            _sequencePointEncoded = true;
        }

    }

}
