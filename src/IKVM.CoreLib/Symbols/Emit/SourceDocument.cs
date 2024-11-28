using System;

namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Represents a reference to a source document.
    /// </summary>
    public class SourceDocument
    {

        readonly SymbolContext _context;
        readonly ModuleSymbolBuilder _module;
        readonly string _url;
        readonly Guid _language;
        readonly Guid _languageVendor;
        readonly Guid _documentType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <param name="languageVendor"></param>
        /// <param name="documentType"></param>
        public SourceDocument(SymbolContext context, ModuleSymbolBuilder module, string url, Guid language, Guid languageVendor, Guid documentType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _url = url;
            _language = language;
            _languageVendor = languageVendor;
            _documentType = documentType;
        }

        /// <summary>
        /// The URL for the document.
        /// </summary>
        public string Url => _url;

        /// <summary>
        /// The GUID that identifies the document language. This can be <see cref="Guid.Empty"/>.
        /// </summary>
        public Guid Language => _language;

        /// <summary>
        /// The GUID that identifies the document language vendor. This can be <see cref="Guid.Empty"/>.
        /// </summary>
        public Guid LanguageVendor => _languageVendor;

        /// <summary>
        /// The GUID that identifies the document type. This can be <see cref="Guid.Empty"/>.
        /// </summary>
        public Guid DocumentType => _documentType;

    }

}
