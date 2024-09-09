using System;
using System.Globalization;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    abstract class Symbol : ISymbol
    {

        /// <summary>
        /// Gets the name of this symbol. Symbols without a name return the empty string; null is
        /// never returned.
        /// </summary>
        public virtual string Name => string.Empty;

        /// <summary>
        /// Gets the name of a symbol as it appears in metadata. Most of the time, this
        /// is the same as the Name property, with the following exceptions:
        /// 1) The metadata name of generic types includes the "`1", "`2" etc. suffix that
        /// indicates the number of type parameters (it does not include, however, names of
        /// containing types or namespaces).
        /// 2) The metadata name of explicit interface names have spaces removed, compared to
        /// the name property.
        /// </summary>
        public virtual string MetadataName => this.Name;

        /// <summary>
        /// Gets the token for this symbol as it appears in metadata. Most of the time this is 0,
        /// as it is when the symbol is not loaded from metadata.
        /// </summary>
        public virtual int MetadataToken => 0;

        /// <summary>
        /// Gets the kind of this symbol.
        /// </summary>
        public abstract SymbolKind Kind { get; }

        /// <summary>
        /// Get the symbol that logically contains this symbol. 
        /// </summary>
        public abstract Symbol? ContainingSymbol { get; }

        ISymbol? ISymbol.ContainingSymbol => ContainingSymbol;

        /// <summary>
        /// Returns the nearest lexically enclosing type, or null if there is none.
        /// </summary>
        public virtual NamedTypeSymbol? ContainingType
        {
            get
            {
                Symbol? container = this.ContainingSymbol;

                NamedTypeSymbol? containerAsType = container as NamedTypeSymbol;

                // NOTE: container could be null, so we do not check
                //       whether containerAsType is not null, but
                //       instead check if it did not change after
                //       the cast.
                if ((object)containerAsType == (object)container)
                {
                    // this should be relatively uncommon
                    // most symbols that may be contained in a type
                    // know their containing type and can override ContainingType
                    // with a more precise implementation
                    return containerAsType;
                }

                // this is recursive, but recursion should be very short 
                // before we reach symbol that definitely knows its containing type.
                return container.ContainingType;
            }
        }

        INamedTypeSymbol? ISymbol.ContainingType => ContainingType;

        /// <summary>
        /// Gets the nearest enclosing namespace for this namespace or type. For a nested type,
        /// returns the namespace that contains its container.
        /// </summary>
        public virtual NamespaceSymbol? ContainingNamespace
        {
            get
            {
                for (var container = this.ContainingSymbol; (object)container != null; container = container.ContainingSymbol)
                {
                    var ns = container as NamespaceSymbol;
                    if ((object)ns != null)
                    {
                        return ns;
                    }
                }

                return null;
            }
        }

        INamespaceSymbol? ISymbol.ContainingNamespace => ContainingNamespace;

        /// <summary>
        /// Returns the assembly containing this symbol. If this symbol is shared across multiple
        /// assemblies, or doesn't belong to an assembly, returns null.
        /// </summary>
        public virtual AssemblySymbol? ContainingAssembly
        {
            get
            {
                var container = this.ContainingSymbol;
                return (object)container != null ? container.ContainingAssembly : null;
            }
        }

        /// <summary>
        /// The original definition of this symbol. If this symbol is constructed from another
        /// symbol by type substitution then OriginalDefinition gets the original symbol as it was defined in
        /// source or metadata.
        /// </summary>
        public Symbol? OriginalDefinition
        {
            get
            {
                return OriginalSymbolDefinition;
            }
        }

        /// <summary>
        /// Returns true if this is the original definition of this symbol.
        /// </summary>
        public bool IsDefinition
        {
            get
            {
                return (object)this == (object)OriginalDefinition;
            }
        }

        /// <summary>
        /// Get this accessibility that was declared on this symbol. For symbols that do not have
        /// accessibility declared on them, returns <see cref="Accessibility.NotApplicable"/>.
        /// </summary>
        public abstract Accessibility DeclaredAccessibility { get; }

        /// <summary>
        /// Returns true if this symbol is "static"; i.e., declared with the <c>static</c> modifier or
        /// implicitly static.
        /// </summary>
        public abstract bool IsStatic { get; }

        /// <summary>
        /// Returns true if this symbol is "virtual", has an implementation, and does not override a
        /// base class member; i.e., declared with the <c>virtual</c> modifier. Does not return true for
        /// members declared as abstract or override.
        /// </summary>
        public abstract bool IsVirtual { get; }

        /// <summary>
        /// Returns true if this symbol was declared to override a base class member; i.e., declared
        /// with the <c>override</c> modifier. Still returns true if member was declared to override
        /// something, but (erroneously) no member to override exists.
        /// </summary>
        /// <remarks>
        /// Even for metadata symbols, <see cref="IsOverride"/> = true does not imply that <see cref="IMethodSymbol.OverriddenMethod"/> will
        /// be non-null.
        /// </remarks>
        public abstract bool IsOverride { get; }

        /// <summary>
        /// Returns true if this symbol was declared as requiring an override; i.e., declared with
        /// the <c>abstract</c> modifier. Also returns true on a type declared as "abstract", all
        /// interface types, and members of interface types.
        /// </summary>
        public abstract bool IsAbstract { get; }

        /// <summary>
        /// Returns true if this symbol was declared to override a base class member and was also
        /// sealed from further overriding; i.e., declared with the <c>sealed</c> modifier. Also set for
        /// types that do not allow a derived class (declared with <c>sealed</c> or <c>static</c> or <c>struct</c>
        /// or <c>enum</c> or <c>delegate</c>).
        /// </summary>
        public abstract bool IsSealed { get; }

        /// <summary>
        /// Returns true if this symbol has external implementation; i.e., declared with the 
        /// <c>extern</c> modifier. 
        /// </summary>
        public abstract bool IsExtern { get; }

        /// <summary>
        /// Returns true if this symbol was automatically created by the compiler, and does not
        /// have an explicit corresponding source code declaration.  
        /// 
        /// This is intended for symbols that are ordinary symbols in the language sense,
        /// and may be used by code, but that are simply declared implicitly rather than
        /// with explicit language syntax.
        /// 
        /// Examples include (this list is not exhaustive):
        ///   the default constructor for a class or struct that is created if one is not provided,
        ///   the BeginInvoke/Invoke/EndInvoke methods for a delegate,
        ///   the generated backing field for an auto property or a field-like event,
        ///   the "this" parameter for non-static methods,
        ///   the "value" parameter for a property setter,
        ///   the parameters on indexer accessor methods (not on the indexer itself),
        ///   methods in anonymous types,
        ///   anonymous functions
        /// </summary>
        public virtual bool IsImplicitlyDeclared
        {
            get { return false; }
        }

        /// <summary>
        /// Returns true if this symbol can be referenced by its name in code. Examples of symbols
        /// that cannot be referenced by name are:
        ///    constructors, destructors, operators, explicit interface implementations,
        ///    accessor methods for properties and events, array types.
        /// </summary>
        public bool CanBeReferencedByName
        {
            get
            {
                switch (this.Kind)
                {
                    case SymbolKind.Local:
                    case SymbolKind.Label:
                        // never imported, and always references by name.
                        return true;

                    case SymbolKind.Namespace:
                    case SymbolKind.Field:
                    case SymbolKind.ErrorType:
                    case SymbolKind.Parameter:
                    case SymbolKind.TypeParameter:
                    case SymbolKind.Event:
                        break;

                    case SymbolKind.NamedType:
                        if (((NamedTypeSymbol)this).IsSubmissionClass)
                        {
                            return false;
                        }
                        break;

                    case SymbolKind.Property:
                        var property = (PropertySymbol)this;
                        if (property.IsIndexer || property.MustCallMethodsDirectly)
                        {
                            return false;
                        }
                        break;

                    case SymbolKind.Method:
                        var method = (MethodSymbol)this;
                        switch (method.MethodKind)
                        {
                            case MethodKind.Ordinary:
                            case MethodKind.LocalFunction:
                            case MethodKind.ReducedExtension:
                                break;
                            case MethodKind.Destructor:
                                // You wouldn't think that destructors would be referenceable by name, but
                                // dev11 only prevents them from being invoked - they can still be assigned
                                // to delegates.
                                return true;
                            case MethodKind.DelegateInvoke:
                                return true;
                            case MethodKind.PropertyGet:
                            case MethodKind.PropertySet:
                                if (!((PropertySymbol)method.AssociatedSymbol).CanCallMethodsDirectly())
                                {
                                    return false;
                                }
                                break;
                            default:
                                return false;
                        }
                        break;

                    case SymbolKind.ArrayType:
                    case SymbolKind.PointerType:
                    case SymbolKind.FunctionPointerType:
                    case SymbolKind.Assembly:
                    case SymbolKind.DynamicType:
                    case SymbolKind.NetModule:
                    case SymbolKind.Discard:
                        return false;

                    default:
                        throw ExceptionUtilities.UnexpectedValue(this.Kind);
                }

                // This will eliminate backing fields for auto-props, explicit interface implementations,
                // indexers, etc.
                // See the comment on ContainsDroppedIdentifierCharacters for an explanation of why
                // such names are not referenceable (or see DevDiv #14432).
                return SyntaxFacts.IsValidIdentifier(this.Name) && !SyntaxFacts.ContainsDroppedIdentifierCharacters(this.Name);
            }
        }

        /// <summary>
        /// Returns the Documentation Comment ID for the symbol, or null if the symbol doesn't
        /// support documentation comments.
        /// </summary>
        public virtual string GetDocumentationCommentId()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the documentation comment for this element with a cancellation token.
        /// </summary>
        /// <param name="preferredCulture">Optionally, retrieve the comments formatted for a particular culture. No impact on source documentation comments.</param>
        /// <param name="expandIncludes">Optionally, expand <![CDATA[<include>]]> elements. No impact on non-source documentation comments.</param>
        /// <param name="cancellationToken">Optionally, allow cancellation of documentation comment retrieval.</param>
        /// <returns>The XML that would be written to the documentation file for the symbol.</returns>
        public virtual string GetDocumentationCommentXml(CultureInfo? preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }

}
