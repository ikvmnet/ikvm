using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace IKVM.Reflection.Diagnostics
{

    /// <summary>
    /// Extensions to <see cref="ISymbolWriter"/> that provides additional capability for IKVM.
    /// </summary>
    public interface IMetadataSymbolWriter : ISymbolWriter
    {

        /// <summary>
        /// Extended OpenMethod that accepts a local variable signature.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="localVarSignature"></param>
        void OpenMethod(SymbolToken method, byte[] localVarSignature);

        /// <summary>
        /// Writes the debug information to the given metadata.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="userEntryPoint"></param>
        void WriteTo(MetadataBuilder metadata, out int userEntryPoint);

    }

}
