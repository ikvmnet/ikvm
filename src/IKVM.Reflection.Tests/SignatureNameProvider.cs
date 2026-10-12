using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;

namespace IKVM.Reflection.Tests
{

    /// <summary>
    /// Decodes signatures with System.Reflection.Metadata into readable strings, independently of IKVM.Reflection.
    /// </summary>
    sealed class SignatureNameProvider : ISignatureTypeProvider<string, object?>
    {

        /// <summary>
        /// Renders a method signature as <c>convention(params) : return</c>.
        /// </summary>
        /// <param name="signature">The decoded method signature.</param>
        /// <returns>The signature as text, with any vararg parameters after <c>...</c>.</returns>
        public static string Format(MethodSignature<string> signature) =>
            signature.Header.CallingConvention.ToString().ToLowerInvariant() + "(" + string.Join(",", signature.ParameterTypes.Take(signature.RequiredParameterCount)) + (signature.ParameterTypes.Length > signature.RequiredParameterCount ? ",...," + string.Join(",", signature.ParameterTypes.Skip(signature.RequiredParameterCount)) : "") + ") : " + signature.ReturnType;

        /// <inheritdoc />
        public string GetArrayType(string elementType, ArrayShape shape) => elementType + "[" + new string(',', shape.Rank - 1) + "]";

        /// <inheritdoc />
        public string GetByReferenceType(string elementType) => elementType + "&";

        /// <inheritdoc />
        public string GetFunctionPointerType(MethodSignature<string> signature) => "fnptr " + Format(signature);

        /// <inheritdoc />
        public string GetGenericInstantiation(string genericType, ImmutableArray<string> typeArguments) => genericType + "<" + string.Join(",", typeArguments) + ">";

        /// <inheritdoc />
        public string GetGenericMethodParameter(object? genericContext, int index) => "!!" + index;

        /// <inheritdoc />
        public string GetGenericTypeParameter(object? genericContext, int index) => "!" + index;

        /// <inheritdoc />
        public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired) => unmodifiedType + (isRequired ? " modreq(" : " modopt(") + modifier + ")";

        /// <inheritdoc />
        public string GetPinnedType(string elementType) => elementType + " pinned";

        /// <inheritdoc />
        public string GetPointerType(string elementType) => elementType + "*";

        /// <inheritdoc />
        public string GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode.ToString();

        /// <inheritdoc />
        public string GetSZArrayType(string elementType) => elementType + "[]";

        /// <inheritdoc />
        public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var t = reader.GetTypeDefinition(handle);
            return reader.GetString(t.Namespace) + "." + reader.GetString(t.Name);
        }

        /// <inheritdoc />
        public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var t = reader.GetTypeReference(handle);
            return reader.GetString(t.Namespace) + "." + reader.GetString(t.Name);
        }

        /// <inheritdoc />
        public string GetTypeFromSpecification(MetadataReader reader, object? genericContext, TypeSpecificationHandle handle, byte rawTypeKind) => reader.GetTypeSpecification(handle).DecodeSignature(this, genericContext);

    }

}
