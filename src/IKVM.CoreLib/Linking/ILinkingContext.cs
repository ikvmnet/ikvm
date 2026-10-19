using System.Diagnostics.CodeAnalysis;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Context under which linking occurs.
    /// </summary>
    internal interface ILinkingContext<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// Returns whether this linking context represents the importer.
        /// </summary>
        /// <remarks>
        /// This is non-optimal, and the required options should be more explicitly specified.
        /// </remarks>
        bool IsImporter { get; }

        /// <summary>
        /// Gets the diagnostics handler for this context.
        /// </summary>
        IDiagnosticHandler Diagnostics { get; }

        /// <summary>
        /// Returns the linking type for 'java.lang.Object'.
        /// </summary>
        TLinkingType TypeOfJavaLangObject { get; }

        /// <summary>
        /// Gets the 'null' verifier type.
        /// </summary>
        TLinkingType TypeOfVerifierNull { get; }

        /// <summary>
        /// Gets whether the linking context allows non-virtual calls.
        /// </summary>
        bool AllowNonVirtualCalls { get; }

        /// <summary>
        /// Creates an unloadable linking type with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TLinkingType CreateUnloadableType(string name);

    }

}
