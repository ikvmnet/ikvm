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
        /// Appends the content of another builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="other"></param>
        public static void WriteBuffer(this BlobBuilder builder, BlobBuilder other) => other.WriteContentTo(builder);

        /// <summary>
        /// Writes a TypeDef, TypeRef or TypeSpec token as a compressed TypeDefOrRefOrSpecEncoded coded index.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="token"></param>
        public static void WriteTypeDefOrRefEncoded(this BlobBuilder builder, int token) => builder.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(MetadataTokens.EntityHandle(token)));

    }

}
