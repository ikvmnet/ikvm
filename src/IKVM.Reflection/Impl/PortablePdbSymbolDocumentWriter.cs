using System;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Impl
{

    class PortablePdbSymbolDocumentWriter : ISymbolDocumentWriter
    {

        readonly string url;
        readonly Guid language;
        readonly Guid languageVendor;
        readonly Guid documentType;
        Guid hashAlgorithmId;
        byte[] hash;
        byte[] source;

        DocumentHandle handle;

        /// <summary>
        /// initializes a new instance.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <param name="languageVendor"></param>
        /// <param name="documentType"></param>
        public PortablePdbSymbolDocumentWriter(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            this.url = url;
            this.language = language;
            this.languageVendor = languageVendor;
            this.documentType = documentType;
        }

        /// <summary>
        /// Gets the handle value. Only available after the document has been written.
        /// </summary>
        internal DocumentHandle Handle => handle;

        /// <inheritdoc />
        public void SetCheckSum(Guid algorithmId, byte[] checkSum)
        {
            this.hashAlgorithmId = algorithmId;
            this.hash = checkSum;
        }

        /// <inheritdoc />
        public void SetSource(byte[] source)
        {
            this.source = source;
        }

        /// <summary>
        /// Writes the document information to the given module's metadata.
        /// </summary>
        /// <param name="module"></param>
        internal void WriteMetadata(ModuleBuilder module)
        {
            var rec = new DocumentTable.Record();
            rec.Name = module.GetOrAddBlobUTF8(url ?? "");
            rec.Language = module.Metadata.GetOrAddGuid(language);
            rec.HashAlgorithm = module.Metadata.GetOrAddGuid(hashAlgorithmId);
            rec.Hash = module.GetOrAddBlob(hash);
            handle = MetadataTokens.DocumentHandle(module.DocumentTable.AddRecord(rec));
        }

    }

}
