using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents a symbol (namespace, class, method, parameter, etc.)
    /// exposed by the compiler.
    /// </summary>
    interface ISymbol : IEquatable<ISymbol?>
    {

        /// <summary>
        /// Gets the <see cref="SymbolKind"/> indicating what kind of symbol it is.
        /// </summary>
        SymbolKind Kind { get; }

        /// <summary>
        /// Gets the symbol name. Returns the empty string if unnamed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the name of a symbol as it appears in metadata. Most of the time, this
        /// is the same as the Name property, with the following exceptions:
        /// <list type="number">
        /// <item>
        /// <description>The metadata name of generic types includes the "`1", "`2" etc. suffix that
        /// indicates the number of type parameters (it does not include, however, names of
        /// containing types or namespaces). </description>
        /// </item>
        /// <item>
        /// <description>The metadata name of explicit interface names have spaces removed, compared to
        /// the name property.</description>
        /// </item>
        /// <item>
        /// <description>The length of names is limited to not exceed metadata restrictions.</description>
        /// </item>
        /// </list>
        /// </summary>
        string MetadataName { get; }

        /// <summary>
        /// Gets the metadata token associated with this symbol, or 0 if the symbol is not loaded from metadata.
        /// </summary>
        int MetadataToken { get; }

        /// <summary>
        /// Gets the <see cref="ISymbol"/> for the immediately containing symbol.
        /// </summary>
        ISymbol? ContainingSymbol { get; }

        /// <summary>
        /// Gets the <see cref="IAssemblySymbol"/> for the containing assembly. Returns null if the
        /// symbol is shared across multiple assemblies.
        /// </summary>
        IAssemblySymbol? ContainingAssembly { get; }

        /// <summary>
        /// Gets the <see cref="IModuleSymbol"/> for the containing module. Returns null if the
        /// symbol is shared across multiple modules.
        /// </summary>
        IModuleSymbol? ContainingModule { get; }

        /// <summary>
        /// Gets the <see cref="INamedTypeSymbol"/> for the containing type. Returns null if the
        /// symbol is not contained within a type.
        /// </summary>
        INamedTypeSymbol? ContainingType { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is the original definition. Returns false
        /// if the symbol is derived from another symbol, by type substitution for instance.
        /// </summary>
        bool IsDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is static.
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is virtual.
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is an override of a base class symbol.
        /// </summary>
        bool IsOverride { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is abstract.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is sealed.
        /// </summary>
        bool IsSealed { get; }

        /// <summary>
        /// Gets a value indicating whether the symbol is defined externally.
        /// </summary>
        bool IsExtern { get; }

        /// <summary>
        /// Gets the attributes for the symbol. Returns an empty <see cref="ImmutableArray{AttributeData}"/>
        /// if there are no attributes.
        /// </summary>
        ImmutableArray<AttributeData> GetAttributes();

        /// <summary>
        /// Gets a <see cref="Accessibility"/> indicating the declared accessibility for the symbol.
        /// Returns NotApplicable if no accessibility is declared.
        /// </summary>
        Accessibility DeclaredAccessibility { get; }

        /// <summary>
        /// Gets the <see cref="ISymbol"/> for the original definition of the symbol.
        /// If this symbol is derived from another symbol, by type substitution for instance,
        /// this gets the original symbol, as it was defined in source or metadata.
        /// </summary>
        ISymbol? OriginalDefinition { get; }

        /// <summary>
        /// Returns the Documentation Comment ID for the symbol, or null if the symbol doesn't
        /// support documentation comments.
        /// </summary>
        string? GetDocumentationCommentId();

        /// <summary>
        /// Gets the XML (as text) for the comment associated with the symbol.
        /// </summary>
        /// <param name="preferredCulture">Preferred culture or null for the default.</param>
        /// <param name="expandIncludes">Optionally, expand &lt;include&gt; elements.  No impact on non-source documentation comments.</param>
        /// <param name="cancellationToken">Token allowing cancellation of request.</param>
        /// <returns>The XML that would be written to the documentation file for the symbol.</returns>
        string? GetDocumentationCommentXml(CultureInfo? preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines if this symbol is equal to another, according to the rules of the provided <see cref="SymbolEqualityComparer"/>
        /// </summary>
        /// <param name="other">The other symbol to compare against</param>
        /// <param name="equalityComparer">The <see cref="SymbolEqualityComparer"/> to use when comparing symbols</param>
        /// <returns>True if the symbols are equivalent.</returns>
        bool Equals(ISymbol? other, SymbolEqualityComparer equalityComparer);

    }

}
