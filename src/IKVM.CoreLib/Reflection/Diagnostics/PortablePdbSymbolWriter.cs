using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Diagnostics
{

    /// <summary>
    /// Symbol writer base implementation that outputs symbols for module builders. It records documents, sequence
    /// points, scopes, local variables and namespace imports as they are defined, and writes them as Portable PDB
    /// tables when <see cref="WriteTo"/> is called.
    /// </summary>
    class PortablePdbSymbolWriter : IMetadataSymbolWriter
    {

        /// <summary>
        /// Represents a source document defined through <see cref="DefineDocument"/>.
        /// </summary>
        protected sealed class Document : ISymbolDocumentWriter
        {

            readonly string _url;
            readonly Guid _language;
            readonly Guid _languageVendor;
            readonly Guid _documentType;

            Guid _algorithmId;
            byte[] _checksum;
            byte[] _source;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="url">The URL that identifies the document.</param>
            /// <param name="language">The document language.</param>
            /// <param name="languageVendor">The identity of the vendor for the document language.</param>
            /// <param name="documentType">The type of the document.</param>
            internal Document(string url, Guid language, Guid languageVendor, Guid documentType)
            {
                _url = url;
                _language = language;
                _languageVendor = languageVendor;
                _documentType = documentType;
            }

            /// <summary>
            /// Sets checksum information.
            /// </summary>
            /// <param name="algorithmId">The GUID representing the algorithm ID.</param>
            /// <param name="checksum">The checksum.</param>
            public void SetCheckSum(Guid algorithmId, byte[] checksum)
            {
                _algorithmId = algorithmId;
                _checksum = checksum;
            }

            /// <summary>
            /// Stores the raw source for a document in the symbol store. The source is recorded but not currently
            /// written to the PDB.
            /// </summary>
            /// <param name="source">The document source represented as unsigned bytes.</param>
            public void SetSource(byte[] source) => _source = source;

            /// <summary>
            /// Gets the URL of the document.
            /// </summary>
            public string Url => _url;

            /// <summary>
            /// Gets the language of the document.
            /// </summary>
            public Guid Language => _language;

            /// <summary>
            /// Gets the checksum algorithm ID.
            /// </summary>
            public Guid AlgorithmId => _algorithmId;

            /// <summary>
            /// Gets the checksum.
            /// </summary>
            public byte[] Checksum => _checksum;

        }

        /// <summary>
        /// Track local variable information.
        /// </summary>
        protected readonly struct LocalVar
        {

            readonly System.Reflection.FieldAttributes _attributes;
            readonly byte[] _signature;
            readonly SymAddressKind _addrKind;
            readonly int _addr1;
            readonly int _addr2;
            readonly int _addr3;
            readonly int _startOffset;
            readonly int _endOffset;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="attributes">The local variable attributes.</param>
            /// <param name="signature">The local variable signature.</param>
            /// <param name="addrKind">The address types for <paramref name="addr1"/>, <paramref name="addr2"/> and <paramref name="addr3"/>.</param>
            /// <param name="addr1">The first address for the local variable specification; for an IL offset address this is the slot index of the local.</param>
            /// <param name="addr2">The second address for the local variable specification.</param>
            /// <param name="addr3">The third address for the local variable specification.</param>
            /// <param name="startOffset">The start offset for the variable, or 0 if it is scoped to the whole method.</param>
            /// <param name="endOffset">The end offset for the variable, or 0 if it is scoped to the whole method.</param>
            internal LocalVar(System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
            {
                _attributes = attributes;
                _signature = signature;
                _addrKind = addrKind;
                _addr1 = addr1;
                _addr2 = addr2;
                _addr3 = addr3;
                _startOffset = startOffset;
                _endOffset = endOffset;
            }

            /// <summary>
            /// Gets the first address, which is written as the slot index of the local variable.
            /// </summary>
            public int Address1 => _addr1;

        }

        /// <summary>
        /// Tracks method scope information.
        /// </summary>
        protected sealed class Scope
        {

            readonly int _startOffset;

            /// <summary>
            /// The IL offset at which the scope ends, set when the scope is closed.
            /// </summary>
            internal int endOffset;

            /// <summary>
            /// The nested scopes, or <see langword="null"/> if there are none.
            /// </summary>
            internal List<Scope> scopes;

            /// <summary>
            /// The local variables declared in the scope, by name, or <see langword="null"/> if there are none.
            /// </summary>
            internal Dictionary<string, LocalVar> locals;

            /// <summary>
            /// The namespaces imported in the scope, or <see langword="null"/> if there are none.
            /// </summary>
            internal List<string> namespaces;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="startOffset">The IL offset at which the scope begins.</param>
            internal Scope(int startOffset)
            {
                _startOffset = startOffset;
            }

            /// <summary>
            /// Gets the start offset.
            /// </summary>
            public int StartOffset => _startOffset;

            /// <summary>
            /// Gets the end offset.
            /// </summary>
            public int EndOffset => endOffset;

            /// <summary>
            /// Gets the nested scopes.
            /// </summary>
            public IReadOnlyList<Scope> Scopes => (IReadOnlyList<Scope>)scopes ?? Array.Empty<Scope>();

            /// <summary>
            /// Gets the local variables.
            /// </summary>
            public IReadOnlyDictionary<string, LocalVar> Locals =>
                (IReadOnlyDictionary<string, LocalVar>)locals ?? ImmutableDictionary<string, LocalVar>.Empty;

            /// <summary>
            /// Gets the namespaces.
            /// </summary>
            public IReadOnlyList<string> Namespaces => (IReadOnlyList<string>)namespaces ?? Array.Empty<string>();

        }

        /// <summary>
        /// Tracks method information.
        /// </summary>
        protected sealed class Method
        {

            readonly int _token;
            readonly StandaloneSignatureHandle _localSignatureHandle;

            /// <summary>
            /// The sequence points defined for the method, or <see langword="null"/> if there are none.
            /// </summary>
            internal List<SequencePoint> sequencePoints;

            /// <summary>
            /// The outermost scopes of the method, or <see langword="null"/> if there are none.
            /// </summary>
            internal List<Scope> scopes;

            /// <summary>
            /// The scopes that are currently open, innermost on top, or <see langword="null"/> if none has been opened.
            /// </summary>
            internal Stack<Scope> ScopeStack;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="token">The <c>MethodDef</c> metadata token of the method.</param>
            /// <param name="localSignatureHandle">The local variable signature of the method body, or a nil handle if it has no locals.</param>
            internal Method(int token, StandaloneSignatureHandle localSignatureHandle)
            {
                _token = token;
                _localSignatureHandle = localSignatureHandle;
            }

            /// <summary>
            /// Gets the token of the method.
            /// </summary>
            public int Token => _token;

            /// <summary>
            /// Gets the local variable signature.
            /// </summary>
            public StandaloneSignatureHandle LocalSignatureHandle => _localSignatureHandle;

            /// <summary>
            /// Gets the sequence points.
            /// </summary>
            public IReadOnlyList<SequencePoint> SequencePoints => (IReadOnlyList<SequencePoint>)sequencePoints ?? Array.Empty<SequencePoint>();

            /// <summary>
            /// Gets the scopes.
            /// </summary>
            public IReadOnlyList<Scope> Scopes => (IReadOnlyList<Scope>)scopes ?? Array.Empty<Scope>();

        }

        /// <summary>
        /// Describes a set of sequence points in one document, as passed to a single call of
        /// <see cref="DefineSequencePoints"/>. The arrays are parallel, with one element per sequence point.
        /// </summary>
        protected readonly struct SequencePoint
        {

            readonly Document _document;
            readonly int[] _offsets;
            readonly int[] _lines;
            readonly int[] _columns;
            readonly int[] _endLines;
            readonly int[] _endColumns;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="document">The document for which the sequence points are being defined.</param>
            /// <param name="offsets">The sequence point offsets measured from the beginning of methods.</param>
            /// <param name="lines">The starting lines for the sequence points.</param>
            /// <param name="columns">The starting columns for the sequence points.</param>
            /// <param name="endLines">The ending lines for the sequence points.</param>
            /// <param name="endColumns">The ending columns for the sequence points.</param>
            internal SequencePoint(Document document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
            {
                _document = document;
                _offsets = offsets;
                _lines = lines;
                _columns = columns;
                _endLines = endLines;
                _endColumns = endColumns;
            }

            /// <summary>
            /// Gets the document.
            /// </summary>
            public Document Document => _document;

            /// <summary>
            /// Gets the offsets.
            /// </summary>
            public int[] Offsets => _offsets;

            /// <summary>
            /// Gets the starting lines.
            /// </summary>
            public int[] Lines => _lines;

            /// <summary>
            /// Gets the starting columns.
            /// </summary>
            public int[] Columns => _columns;

            /// <summary>
            /// Gets the ending lines.
            /// </summary>
            public int[] EndLines => _endLines;

            /// <summary>
            /// Gets the ending columns.
            /// </summary>
            public int[] EndColumns => _endColumns;

        }

        readonly ModuleBuilder _module;

        /// <summary>
        /// The documents defined so far, by URL, or <see langword="null"/> if none has been defined.
        /// </summary>
        Dictionary<string, Document> _documents;

        /// <summary>
        /// The methods that have been closed, in the order they were closed, or <see langword="null"/> if there are none.
        /// </summary>
        List<Method> _methods;

        /// <summary>
        /// The method that is currently open, or <see langword="null"/> if no method is open.
        /// </summary>
        Method _currentMethod;

        /// <summary>
        /// The metadata token of the user entry point, or 0 if none has been set.
        /// </summary>
        int _userEntryPoint;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module the symbols are written for.</param>
        /// <exception cref="ArgumentNullException"><paramref name="module"/> is <see langword="null"/>.</exception>
        public PortablePdbSymbolWriter(ModuleBuilder module)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="emitter">The metadata emitter interface.</param>
        /// <param name="filename">The file name associated with the debug information.</param>
        /// <param name="fFullBuild"><see langword="true"/> if this is a full rebuild; <see langword="false"/> if this is an incremental compilation.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public virtual void Initialize(IntPtr emitter, string filename, bool fFullBuild) => throw new NotImplementedException();

        /// <summary>
        /// Defines a source document. A document with the same URL as one defined earlier is returned again rather than
        /// defined twice.
        /// </summary>
        /// <param name="url">The URL that identifies the document.</param>
        /// <param name="language">The document language.</param>
        /// <param name="languageVendor">The identity of the vendor for the document language.</param>
        /// <param name="documentType">The type of the document.</param>
        /// <returns>The object that represents the document.</returns>
        public virtual ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            _documents ??= [];
            if (_documents.TryGetValue(url, out var doc) == false)
                _documents.Add(url, doc = new Document(url, language, languageVendor, documentType));

            return doc;
        }

        /// <summary>
        /// Not supported; use <see cref="UsingNamespace"/> to record namespace imports in a scope.
        /// </summary>
        /// <param name="name">The name of the new namespace.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public virtual void OpenNamespace(string name) => throw new NotImplementedException();

        /// <summary>
        /// Opens a method to place symbol information into, without a local variable signature.
        /// </summary>
        /// <param name="method">The metadata token for the method to be opened.</param>
        public virtual void OpenMethod(SymbolToken method)
        {
            Debug.Assert(ModuleBuilder.IsPseudoToken(method.GetToken()) == false);
            _currentMethod = new Method(method.GetToken(), default);
        }

        /// <summary>
        /// Opens a method to place symbol information into.
        /// </summary>
        /// <param name="method">The metadata token for the method to be opened. Must be a real token, not a pseudo token.</param>
        /// <param name="localSignatureHandle">The local variable signature of the method body, or a nil handle if it has no locals.</param>
        public virtual void OpenMethod(SymbolToken method, StandaloneSignatureHandle localSignatureHandle)
        {
            Debug.Assert(ModuleBuilder.IsPseudoToken(method.GetToken()) == false);
            _currentMethod = new Method(method.GetToken(), localSignatureHandle);
        }

        /// <summary>
        /// Defines a group of sequence points within the current method.
        /// </summary>
        /// <param name="document">The document object for which the sequence points are being defined.</param>
        /// <param name="offsets">The sequence point offsets measured from the beginning of methods.</param>
        /// <param name="lines">The document lines for the sequence points.</param>
        /// <param name="columns">The document positions for the sequence points.</param>
        /// <param name="endLines">The document end lines for the sequence points.</param>
        /// <param name="endColumns">The document end positions for the sequence points.</param>
        /// <exception cref="InvalidOperationException">No method is open, or <paramref name="document"/> was not obtained from this symbol writer.</exception>
        public virtual void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            if (document is not Document doc)
                throw new InvalidOperationException("Document not obtained from symbol writer.");

            var sequencePoint = new SequencePoint(doc, offsets, lines, columns, endLines, endColumns);
            _currentMethod.sequencePoints ??= [];
            _currentMethod.sequencePoints.Add(sequencePoint);
        }

        /// <summary>
        /// Opens a new lexical scope in the current method, nested in the innermost open scope if there is one.
        /// </summary>
        /// <param name="startOffset">The offset, in bytes, from the beginning of the method to the first instruction in the lexical scope.</param>
        /// <returns>Always 0; scope identifiers are not used by this writer.</returns>
        /// <exception cref="InvalidOperationException">No method is open.</exception>
        public virtual int OpenScope(int startOffset)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = new Scope(startOffset);

            if (_currentMethod.ScopeStack == null || _currentMethod.ScopeStack.Count == 0)
            {
                _currentMethod.scopes ??= [];
                _currentMethod.scopes.Add(scope);
            }
            else
            {
                var thisScope = _currentMethod.ScopeStack?.Peek();
                thisScope.scopes ??= [];
                thisScope.scopes.Add(scope);
            }

            _currentMethod.ScopeStack ??= new();
            _currentMethod.ScopeStack.Push(scope);

            return 0;
        }

        /// <summary>
        /// Specifies that the given, fully qualified namespace name is used within the open lexical scope.
        /// </summary>
        /// <param name="fullName">The fully qualified name of the namespace.</param>
        /// <exception cref="InvalidOperationException">No method or no scope is open.</exception>
        public virtual void UsingNamespace(string fullName)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Peek()) ?? throw new InvalidOperationException("No open scope.");

            scope.namespaces ??= [];
            scope.namespaces.Add(fullName);
        }

        /// <summary>
        /// Defines a single variable in the current lexical scope. A later definition with the same name in the same
        /// scope replaces the earlier one.
        /// </summary>
        /// <param name="name">The local variable name.</param>
        /// <param name="attributes">A bitwise combination of the <see cref="System.Reflection.FieldAttributes"/> values.</param>
        /// <param name="signature">The local variable signature.</param>
        /// <param name="addrKind">The address types for <paramref name="addr1"/>, <paramref name="addr2"/>, and <paramref name="addr3"/>.</param>
        /// <param name="addr1">The first address for the local variable specification; written as the slot index of the local.</param>
        /// <param name="addr2">The second address for the local variable specification.</param>
        /// <param name="addr3">The third address for the local variable specification.</param>
        /// <param name="startOffset">The start offset for the variable. If this parameter is zero, it is ignored and the variable is defined throughout the entire scope.</param>
        /// <param name="endOffset">The end offset for the variable. If this parameter is zero, it is ignored and the variable is defined throughout the entire scope.</param>
        /// <exception cref="InvalidOperationException">No method or no scope is open.</exception>
        public virtual void DefineLocalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Peek()) ?? throw new InvalidOperationException("No open scope.");

            scope.locals ??= [];
            scope.locals[name] = new LocalVar(attributes, signature, addrKind, addr1, addr2, addr3, startOffset, endOffset);
        }

        /// <summary>
        /// Closes the current lexical scope.
        /// </summary>
        /// <param name="endOffset">The points past the last instruction in the scope.</param>
        /// <exception cref="InvalidOperationException">No method or no scope is open.</exception>
        public virtual void CloseScope(int endOffset)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Pop()) ?? throw new InvalidOperationException("No open scope.");

            scope.endOffset = endOffset;
        }

        /// <summary>
        /// Closes the current method and keeps its symbol information to be written by <see cref="WriteTo"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">No method is open.</exception>
        public virtual void CloseMethod()
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            _methods ??= [];
            _methods.Add(_currentMethod);
            _currentMethod = null;
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public virtual void CloseNamespace() => throw new NotImplementedException();

        /// <summary>
        /// Closes the symbol writer. This implementation has nothing to release.
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// Writes the recorded symbol information to the Portable PDB metadata. The methods must have been closed in
        /// <c>MethodDef</c> row order, because each one becomes the <c>MethodDebugInformation</c> row with the same row
        /// number.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="userEntryPoint">Set to the metadata token of the user entry point, or 0 if none was set.</param>
        public virtual void WriteTo(MetadataBuilder metadata, out int userEntryPoint)
        {
            metadata.GetOrAddString("");
            metadata.GetOrAddUserString("");

            if (_methods != null)
            {
                var documentCache = new Dictionary<Document, DocumentHandle>();
                foreach (var method in _methods)
                    WriteMethod(metadata, method, documentCache);
            }

            userEntryPoint = _userEntryPoint;
        }

        /// <summary>
        /// Writes the sequence points and scopes of the method and its <c>MethodDebugInformation</c> row.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="method">The method to write.</param>
        /// <param name="documentCache">The handles of the documents written so far.</param>
        void WriteMethod(MetadataBuilder metadata, Method method, Dictionary<Document, DocumentHandle> documentCache)
        {
            // if all sequence points are in one document, it goes directly on the method debug information
            var initialDocument = GetSingleDocument(method.SequencePoints);
            var initialDocumentHandle = initialDocument != null ? GetOrWriteDocument(metadata, initialDocument, documentCache) : default;
            var currentDocumentHandle = initialDocumentHandle;

            // write sequence points and scopes
            WriteSequencePoints(metadata, method, documentCache, out var sequencePointsHandle, ref currentDocumentHandle);
            WriteScopes(metadata, method);

            // final debug information, containing initial document
            var methodDebugHandle = metadata.AddMethodDebugInformation(initialDocumentHandle, sequencePointsHandle);
            Debug.Assert(MetadataTokens.GetRowNumber(methodDebugHandle) == MetadataTokens.GetRowNumber(MetadataTokens.EntityHandle(method.Token)));
        }

        /// <summary>
        /// Returns the single unique <see cref="Document"/> in a set of sequence points, or <see langword="null"/> if
        /// there is no single unique document.
        /// </summary>
        /// <param name="sequencePoints">The sequence points to examine.</param>
        /// <returns>The document shared by all of the sequence points, or <see langword="null"/> if they are in more than one document or there are none.</returns>
        Document GetSingleDocument(IEnumerable<SequencePoint> sequencePoints)
        {
            var s = sequencePoints.Select(i => i.Document).Distinct().Take(2).ToList();
            return s.Count == 1 ? s[0] : null;
        }

        /// <summary>
        /// Writes the sequence points for the method, ordered by IL offset, as a sequence points blob.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="method">The method whose sequence points are written.</param>
        /// <param name="documentCache">The handles of the documents written so far.</param>
        /// <param name="sequencePointHandle">Set to the handle of the sequence points blob, or a nil handle if the method has no sequence points.</param>
        /// <param name="previousDocument">The document the blob starts in; updated as the encoder switches documents.</param>
        void WriteSequencePoints(MetadataBuilder metadata, Method method, Dictionary<Document, DocumentHandle> documentCache, out BlobHandle sequencePointHandle, ref DocumentHandle previousDocument)
        {
            // no sequence points?
            sequencePointHandle = default;
            if (method.SequencePoints.Count == 0)
                return;

            var buf = new BlobBuilder();
            var enc = new SequencePointEncoder(buf);

            // the local signature of the method body, which is nil for a method without locals
            enc.LocalSignature(method.LocalSignatureHandle);

            // add the sequence points recorded on the method
            foreach (var (document, offset, startLine, endLine, startColumn, endColumn) in ExpandSequencePoints(method.SequencePoints).OrderBy(i => i.Offset))
            {
                var doc = GetOrWriteDocument(metadata, document, documentCache);
                enc.SequencePoint(doc, offset, startLine, startColumn, endLine, endColumn, ref previousDocument);
            }

            // add sequence points blob
            sequencePointHandle = metadata.GetOrAddBlob(buf);
        }

        /// <summary>
        /// Expands the set of sequence points with multiple offsets into single sequence point records for each offset.
        /// </summary>
        /// <param name="sequencePoints">The sequence point groups recorded on a method.</param>
        /// <returns>One record per offset, in the order they were defined.</returns>
        IEnumerable<(Document Document, int Offset, int StartLine, int EndLine, int StartColumn, int EndColumn)> ExpandSequencePoints(IReadOnlyList<SequencePoint> sequencePoints)
        {
            for (var i = 0; i < sequencePoints.Count; i++)
                for (var j = 0; j < sequencePoints[i].Offsets.Length; j++)
                    yield return (sequencePoints[i].Document, sequencePoints[i].Offsets[j], sequencePoints[i].Lines[j], sequencePoints[i].EndLines[j], sequencePoints[i].Columns[j], sequencePoints[i].EndColumns[j]);
        }

        /// <summary>
        /// Gets or writes the handle to a document in the metadata.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="document">The document, or <see langword="null"/>.</param>
        /// <param name="documentCache">The handles of the documents written so far; updated when the document is written.</param>
        /// <returns>The handle of the <c>Document</c> row, or a nil handle if <paramref name="document"/> is <see langword="null"/>.</returns>
        DocumentHandle GetOrWriteDocument(MetadataBuilder metadata, Document document, Dictionary<Document, DocumentHandle> documentCache)
        {
            if (document == null)
                return default;

            if (documentCache.TryGetValue(document, out var handle) == false)
                documentCache.Add(document, handle = WriteDocument(metadata, document));

            return handle;
        }

        /// <summary>
        /// Writes the document to the metadata.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="document">The document to write.</param>
        /// <returns>The handle of the added <c>Document</c> row.</returns>
        DocumentHandle WriteDocument(MetadataBuilder metadata, Document document)
        {
            return metadata.AddDocument(
                metadata.GetOrAddDocumentName(document.Url ?? ""),
                document.AlgorithmId != Guid.Empty ? metadata.GetOrAddGuid(document.AlgorithmId) : default,
                document.Checksum != null ? metadata.GetOrAddBlob(document.Checksum) : default,
                document.Language != Guid.Empty ? metadata.GetOrAddGuid(document.Language) : default);
        }

        /// <summary>
        /// Writes the scopes of the method, ordered by start offset and then outermost first, as the <c>LocalScope</c>
        /// table requires.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="method">The method whose scopes are written.</param>
        void WriteScopes(MetadataBuilder metadata, Method method)
        {
            foreach (var scope in method.Scopes.OrderBy(i => i.StartOffset).ThenByDescending(i => i.EndOffset - i.StartOffset))
                WriteScope(metadata, method, scope);
        }

        /// <summary>
        /// Writes the given outermost scope and its nested scopes.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="method">The method that contains the scope.</param>
        /// <param name="scope">The scope to write.</param>
        void WriteScope(MetadataBuilder metadata, Method method, Scope scope) => WriteScope(metadata, method, scope, default);

        /// <summary>
        /// Writes the given scope, its local variables and namespace imports, and then its nested scopes.
        /// </summary>
        /// <param name="metadata">The metadata builder for the Portable PDB.</param>
        /// <param name="method">The method that contains the scope.</param>
        /// <param name="scope">The scope to write.</param>
        /// <param name="parentImportScope">The import scope of the enclosing scope, or a nil handle for an outermost scope.</param>
        void WriteScope(MetadataBuilder metadata, Method method, Scope scope, ImportScopeHandle parentImportScope)
        {
            // insert import scope for this scope
            var importScope = default(ImportScopeHandle);
            if (scope.Namespaces.Count > 0)
            {
                var buf = new BlobBuilder();
                var enc = new ImportsEncoder(buf);

                // add namespace imports
                foreach (var ns in scope.Namespaces)
                    enc.ImportNamespace(metadata.GetOrAddBlobUTF8(ns));

                // add the import scope, nested in the parent import scope
                importScope = metadata.AddImportScope(parentImportScope, metadata.GetOrAddBlob(buf));
            }

            // the scope's variables and constants are the runs of rows starting at the next row of each table, which
            // for a scope without any is where the next scope's begin
            var variableList = MetadataTokens.LocalVariableHandle(metadata.GetRowCount(TableIndex.LocalVariable) + 1);
            var constantList = MetadataTokens.LocalConstantHandle(metadata.GetRowCount(TableIndex.LocalConstant) + 1);
            foreach (var kvp in scope.Locals.OrderBy(i => i.Value.Address1))
                metadata.AddLocalVariable(LocalVariableAttributes.None, kvp.Value.Address1, metadata.GetOrAddString(kvp.Key));

            // add scope
            metadata.AddLocalScope(
                (MethodDefinitionHandle)MetadataTokens.EntityHandle(method.Token),
                importScope,
                variableList,
                constantList,
                scope.StartOffset,
                scope.EndOffset - scope.StartOffset);

            // repeat for children scopes
            foreach (var nestedScope in scope.Scopes.OrderBy(i => i.StartOffset).ThenByDescending(i => i.EndOffset - i.StartOffset))
                WriteScope(metadata, method, nestedScope, importScope);
        }

        #region Not Implemented

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="parent">The metadata type or method token.</param>
        /// <param name="name">The field name.</param>
        /// <param name="attributes">A bitwise combination of the <see cref="System.Reflection.FieldAttributes"/> values.</param>
        /// <param name="signature">The field signature.</param>
        /// <param name="addrKind">The address types for <paramref name="addr1"/> and <paramref name="addr2"/>.</param>
        /// <param name="addr1">The first address for the field specification.</param>
        /// <param name="addr2">The second address for the field specification.</param>
        /// <param name="addr3">The third address for the field specification.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void DefineField(SymbolToken parent, string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="name">The global variable name.</param>
        /// <param name="attributes">A bitwise combination of the <see cref="System.Reflection.FieldAttributes"/> values.</param>
        /// <param name="signature">The global variable signature.</param>
        /// <param name="addrKind">The address types for <paramref name="addr1"/>, <paramref name="addr2"/>, and <paramref name="addr3"/>.</param>
        /// <param name="addr1">The first address for the global variable specification.</param>
        /// <param name="addr2">The second address for the global variable specification.</param>
        /// <param name="addr3">The third address for the global variable specification.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void DefineGlobalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="attributes">A bitwise combination of the <see cref="System.Reflection.ParameterAttributes"/> values.</param>
        /// <param name="sequence">The parameter signature.</param>
        /// <param name="addrKind">The address types for <paramref name="addr1"/>, <paramref name="addr2"/>, and <paramref name="addr3"/>.</param>
        /// <param name="addr1">The first address for the parameter specification.</param>
        /// <param name="addr2">The second address for the parameter specification.</param>
        /// <param name="addr3">The third address for the parameter specification.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void DefineParameter(string name, System.Reflection.ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="startDoc">The document containing the starting position.</param>
        /// <param name="startLine">The starting line number.</param>
        /// <param name="startColumn">The starting column.</param>
        /// <param name="endDoc">The document containing the ending position.</param>
        /// <param name="endLine">The ending line number.</param>
        /// <param name="endColumn">The ending column number.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn) =>
            throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="scopeID">The scope identifier for the scope.</param>
        /// <param name="startOffset">The byte offset of the beginning of the lexical scope.</param>
        /// <param name="endOffset">The byte offset of the end of the lexical scope.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void SetScopeRange(int scopeID, int startOffset, int endOffset) => throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="parent">The metadata token for which the attribute is being defined.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="data">The attribute value.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void SetSymAttribute(SymbolToken parent, string name, byte[] data) => throw new NotImplementedException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="underlyingWriter">A pointer to code that represents the underlying writer.</param>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public void SetUnderlyingWriter(IntPtr underlyingWriter) => throw new NotImplementedException();

        #endregion

        /// <summary>
        /// Identifies the user-defined method as the user entry point for the current module.
        /// </summary>
        /// <param name="entryMethod">The metadata token for the method that is the user entry point.</param>
        public void SetUserEntryPoint(SymbolToken entryMethod) => _userEntryPoint = entryMethod.GetToken();

    }

}
