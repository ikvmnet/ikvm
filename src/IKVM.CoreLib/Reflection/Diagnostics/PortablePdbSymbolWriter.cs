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
    /// Symbol writer base implementation that outputs symbols for module builders.
    /// </summary>
    class PortablePdbSymbolWriter : IMetadataSymbolWriter
    {

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
            /// <param name="url"></param>
            /// <param name="language"></param>
            /// <param name="languageVendor"></param>
            /// <param name="documentType"></param>
            internal Document(string url, Guid language, Guid languageVendor, Guid documentType)
            {
                _url = url;
                _language = language;
                _languageVendor = languageVendor;
                _documentType = documentType;
            }

            /// <summary>
            /// Sets the checksum.
            /// </summary>
            /// <param name="algorithmId"></param>
            /// <param name="checksum"></param>
            public void SetCheckSum(Guid algorithmId, byte[] checksum)
            {
                _algorithmId = algorithmId;
                _checksum = checksum;
            }

            /// <summary>
            /// Sets the source.
            /// </summary>
            /// <param name="source"></param>
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
            /// <param name="attributes"></param>
            /// <param name="signature"></param>
            /// <param name="addrKind"></param>
            /// <param name="addr1"></param>
            /// <param name="addr2"></param>
            /// <param name="addr3"></param>
            /// <param name="startOffset"></param>
            /// <param name="endOffset"></param>
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
            /// Gets the first address.
            /// </summary>
            public int Address1 => _addr1;

        }

        /// <summary>
        /// Tracks method scope information.
        /// </summary>
        protected sealed class Scope
        {

            readonly int _startOffset;
            internal int endOffset;
            internal List<Scope> scopes;
            internal Dictionary<string, LocalVar> locals;
            internal List<string> namespaces;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="startOffset"></param>
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
            internal List<SequencePoint> sequencePoints;
            internal List<Scope> scopes;
            internal Stack<Scope> ScopeStack;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="token"></param>
            /// <param name="localSignatureHandle"></param>
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
        /// Describes a sequence point on a method.
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
            /// <param name="offsets"></param>
            /// <param name="lines"></param>
            /// <param name="columns"></param>
            /// <param name="endLines"></param>
            /// <param name="endColumns"></param>
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
            /// Gets the lines
            /// </summary>
            public int[] Lines => _lines;

            /// <summary>
            /// Gets the columns.
            /// </summary>
            public int[] Columns => _columns;

            /// <summary>
            /// Gets the line endings.
            /// </summary>
            public int[] EndLines => _endLines;

            /// <summary>
            /// Gets the column endings.
            /// </summary>
            public int[] EndColumns => _endColumns;

        }

        readonly ModuleBuilder _module;
        Dictionary<string, Document> _documents;
        List<Method> _methods;
        Method _currentMethod;
        int _userEntryPoint;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        public PortablePdbSymbolWriter(ModuleBuilder module)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <inheritdoc />
        public virtual void Initialize(IntPtr emitter, string filename, bool fFullBuild) => throw new NotImplementedException();

        /// <inheritdoc />
        public virtual ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            _documents ??= [];
            if (_documents.TryGetValue(url, out var doc) == false)
                _documents.Add(url, doc = new Document(url, language, languageVendor, documentType));

            return doc;
        }

        /// <inheritdoc />
        public virtual void OpenNamespace(string name) => throw new NotImplementedException();

        /// <inheritdoc />
        public virtual void OpenMethod(SymbolToken method)
        {
            Debug.Assert(ModuleBuilder.IsPseudoToken(method.GetToken()) == false);
            _currentMethod = new Method(method.GetToken(), default);
        }

        /// <summary>
        /// Opens a method to place symbol information into.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="localSignatureHandle"></param>
        public virtual void OpenMethod(SymbolToken method, StandaloneSignatureHandle localSignatureHandle)
        {
            Debug.Assert(ModuleBuilder.IsPseudoToken(method.GetToken()) == false);
            _currentMethod = new Method(method.GetToken(), localSignatureHandle);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public virtual void UsingNamespace(string fullName)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Peek()) ?? throw new InvalidOperationException("No open scope.");

            scope.namespaces ??= [];
            scope.namespaces.Add(fullName);
        }

        /// <inheritdoc />
        public virtual void DefineLocalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Peek()) ?? throw new InvalidOperationException("No open scope.");

            scope.locals ??= [];
            scope.locals[name] = new LocalVar(attributes, signature, addrKind, addr1, addr2, addr3, startOffset, endOffset);
        }

        /// <inheritdoc />
        public virtual void CloseScope(int endOffset)
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            var scope = (_currentMethod.ScopeStack?.Pop()) ?? throw new InvalidOperationException("No open scope.");

            scope.endOffset = endOffset;
        }

        /// <inheritdoc />
        public virtual void CloseMethod()
        {
            if (_currentMethod == null)
                throw new InvalidOperationException("No open method.");

            _methods ??= [];
            _methods.Add(_currentMethod);
            _currentMethod = null;
        }

        /// <inheritdoc />
        public virtual void CloseNamespace() => throw new NotImplementedException();

        /// <inheritdoc />
        public virtual void Close()
        {
        }

        /// <inheritdoc />
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
        /// Writes the given method.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="method"></param>
        /// <param name="documentCache"></param>
        void WriteMethod(MetadataBuilder metadata, Method method, Dictionary<Document, DocumentHandle> documentCache)
        {
            // find first document and set as initial, as it will be directly on method debug information
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
        /// Returns the single unique <see cref="Document"/> in a set of sequence points, or <c>null</c> if there is no single unique document.
        /// </summary>
        /// <param name="sequencePoints"></param>
        /// <returns></returns>
        Document GetSingleDocument(IEnumerable<SequencePoint> sequencePoints)
        {
            var s = sequencePoints.Select(i => i.Document).Distinct().Take(2).ToList();
            return s.Count == 1 ? s[0] : null;
        }

        /// <summary>
        /// Writes the sequence points for the method.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="method"></param>
        /// <param name="documentCache"></param>
        /// <param name="sequencePointHandle"></param>
        /// <param name="previousDocument"></param>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// <param name="sequencePoints"></param>
        /// <returns></returns>
        IEnumerable<(Document Document, int Offset, int StartLine, int EndLine, int StartColumn, int EndColumn)> ExpandSequencePoints(IReadOnlyList<SequencePoint> sequencePoints)
        {
            for (var i = 0; i < sequencePoints.Count; i++)
                for (var j = 0; j < sequencePoints[i].Offsets.Length; j++)
                    yield return (sequencePoints[i].Document, sequencePoints[i].Offsets[j], sequencePoints[i].Lines[j], sequencePoints[i].EndLines[j], sequencePoints[i].Columns[j], sequencePoints[i].EndColumns[j]);
        }

        /// <summary>
        /// Gets or writes the handle to a document in the metadata.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="document"></param>
        /// <param name="documentCache"></param>
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
        /// <param name="metadata"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        DocumentHandle WriteDocument(MetadataBuilder metadata, Document document)
        {
            return metadata.AddDocument(
                metadata.GetOrAddDocumentName(document.Url ?? ""),
                document.AlgorithmId != Guid.Empty ? metadata.GetOrAddGuid(document.AlgorithmId) : default,
                document.Checksum != null ? metadata.GetOrAddBlob(document.Checksum) : default,
                document.Language != Guid.Empty ? metadata.GetOrAddGuid(document.Language) : default);
        }

        /// <summary>
        /// Writes the scopes of the method.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="method"></param>
        void WriteScopes(MetadataBuilder metadata, Method method)
        {
            foreach (var scope in method.Scopes.OrderBy(i => i.StartOffset).ThenByDescending(i => i.EndOffset - i.StartOffset))
                WriteScope(metadata, method, scope);
        }

        /// <summary>
        /// Writes the given scope.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="method"></param>
        /// <param name="scope"></param>
        void WriteScope(MetadataBuilder metadata, Method method, Scope scope) => WriteScope(metadata, method, scope, default);

        /// <summary>
        /// Writes the given scope.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="method"></param>
        /// <param name="scope"></param>
        /// <param name="parentImportScope"></param>
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

                // write nested scope imports
                importScope = metadata.AddImportScope(parentImportScope, metadata.GetOrAddBlob(buf));
            }

            // insert variables for this scope, keeping first entry
            var variableList = default(LocalVariableHandle);
            foreach (var kvp in scope.Locals.OrderBy(i => i.Value.Address1))
            {
                var h = metadata.AddLocalVariable(LocalVariableAttributes.None, kvp.Value.Address1, metadata.GetOrAddString(kvp.Key));
                if (variableList.IsNil)
                    variableList = h;
            }

            // add scope
            metadata.AddLocalScope(
                (MethodDefinitionHandle)MetadataTokens.EntityHandle(method.Token),
                importScope,
                variableList,
                default,
                scope.StartOffset,
                scope.EndOffset - scope.StartOffset);

            // repeat for children scopes
            foreach (var nestedScope in scope.Scopes.OrderBy(i => i.StartOffset).ThenByDescending(i => i.EndOffset - i.StartOffset))
                WriteScope(metadata, method, nestedScope, importScope);
        }

        #region Not Implemented

        /// <inheritdoc />
        public void DefineField(SymbolToken parent, string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public void DefineGlobalVariable(string name, System.Reflection.FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public void DefineParameter(string name, System.Reflection.ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public void SetScopeRange(int scopeID, int startOffset, int endOffset) => throw new NotImplementedException();

        /// <inheritdoc />
        public void SetSymAttribute(SymbolToken parent, string name, byte[] data) => throw new NotImplementedException();

        /// <inheritdoc />
        public void SetUnderlyingWriter(IntPtr underlyingWriter) => throw new NotImplementedException();

        /// <inheritdoc />
        public void SetUserEntryPoint(SymbolToken entryMethod) => _userEntryPoint = entryMethod.GetToken();

        #endregion

    }

}
