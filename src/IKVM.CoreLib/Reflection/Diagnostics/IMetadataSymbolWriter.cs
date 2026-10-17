using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Diagnostics
{

    /// <summary>
    /// Extensions to <see cref="ISymbolWriter"/> that provide additional capability for IKVM.
    /// </summary>
    interface IMetadataSymbolWriter : ISymbolWriter
    {

        /// <summary>
        /// Opens a method in which to place symbolic information, recording the signature of its local variables.
        /// </summary>
        /// <param name="method">The metadata token of the method to open.</param>
        /// <param name="localSignatureHandle">The handle of the standalone signature that describes the method's local variables.</param>
        void OpenMethod(SymbolToken method, StandaloneSignatureHandle localSignatureHandle);

        /// <summary>
        /// Writes the debug information to the given metadata.
        /// </summary>
        /// <param name="metadata">The metadata builder that receives the debug information.</param>
        /// <param name="userEntryPoint">When this method returns, contains the metadata token of the user entry point method, or 0 if there is none.</param>
        void WriteTo(MetadataBuilder metadata, out int userEntryPoint);

    }

}
