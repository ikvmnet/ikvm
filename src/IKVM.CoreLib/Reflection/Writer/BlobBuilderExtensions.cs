using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Encoding helpers for <see cref="BlobBuilder"/>.
    /// </summary>
    static class BlobBuilderExtensions
    {

        /// <summary>
        /// Writes a TypeDef, TypeRef or TypeSpec token as a compressed TypeDefOrRefOrSpecEncoded coded index.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        public static void WriteTypeDefOrRefEncoded(this BlobBuilder builder, int token) => builder.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(MetadataTokens.EntityHandle(token)));

    }

}
