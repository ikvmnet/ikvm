using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Encodes the sequence points blob of a method in a Portable PDB: the local signature header followed by
    /// document records and sequence point records with delta-compressed offsets, lines and columns.
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
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer">The blob builder the sequence points are written to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is <see langword="null"/>.</exception>
        public SequencePointEncoder(BlobBuilder writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <summary>
        /// Encodes the local signature header. This must be called once, before any sequence point is encoded.
        /// </summary>
        /// <param name="localSignature">The stand-alone signature of the method's locals.</param>
        /// <exception cref="InvalidOperationException">The local signature or a sequence point has already been encoded.</exception>
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
        /// Encodes a new sequence point. <paramref name="previousDocument"/> is used to determine whether a new
        /// document record is required to be written first.
        /// </summary>
        /// <param name="document">The document containing the sequence point, or a nil handle to keep the current document.</param>
        /// <param name="offset">The IL offset of the sequence point; must be greater than that of the previous sequence point.</param>
        /// <param name="startLine">The start line, or <see cref="System.Reflection.Metadata.SequencePoint.HiddenLine"/> for a hidden sequence point.</param>
        /// <param name="startColumn">The start column.</param>
        /// <param name="endLine">The end line.</param>
        /// <param name="endColumn">The end column.</param>
        /// <param name="previousDocument">The document of the previously written record; updated to <paramref name="document"/> when a new document record is written.</param>
        /// <exception cref="InvalidOperationException">The local signature has not been encoded yet.</exception>
        /// <exception cref="ArgumentException"><paramref name="offset"/> is not greater than the offset of the previous sequence point.</exception>
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
