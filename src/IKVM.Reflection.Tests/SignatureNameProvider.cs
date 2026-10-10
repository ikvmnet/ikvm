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

        readonly MetadataReader reader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        public SignatureNameProvider(MetadataReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Renders a method signature as <c>convention(params) : return</c>.
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static string Format(MethodSignature<string> signature) =>
            signature.Header.CallingConvention.ToString().ToLowerInvariant() + "(" + string.Join(",", signature.ParameterTypes.Take(signature.RequiredParameterCount)) + (signature.ParameterTypes.Length > signature.RequiredParameterCount ? ",...," + string.Join(",", signature.ParameterTypes.Skip(signature.RequiredParameterCount)) : "") + ") : " + signature.ReturnType;

        public string GetArrayType(string elementType, ArrayShape shape) => elementType + "[" + new string(',', shape.Rank - 1) + "]";

        public string GetByReferenceType(string elementType) => elementType + "&";

        public string GetFunctionPointerType(MethodSignature<string> signature) => "fnptr " + Format(signature);

        public string GetGenericInstantiation(string genericType, ImmutableArray<string> typeArguments) => genericType + "<" + string.Join(",", typeArguments) + ">";

        public string GetGenericMethodParameter(object? genericContext, int index) => "!!" + index;

        public string GetGenericTypeParameter(object? genericContext, int index) => "!" + index;

        public string GetModifiedType(string modifier, string unmodifiedType, bool isRequired) => unmodifiedType + (isRequired ? " modreq(" : " modopt(") + modifier + ")";

        public string GetPinnedType(string elementType) => elementType + " pinned";

        public string GetPointerType(string elementType) => elementType + "*";

        public string GetPrimitiveType(PrimitiveTypeCode typeCode) => typeCode.ToString();

        public string GetSZArrayType(string elementType) => elementType + "[]";

        public string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind)
        {
            var t = reader.GetTypeDefinition(handle);
            return reader.GetString(t.Namespace) + "." + reader.GetString(t.Name);
        }

        public string GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind)
        {
            var t = reader.GetTypeReference(handle);
            return reader.GetString(t.Namespace) + "." + reader.GetString(t.Name);
        }

        public string GetTypeFromSpecification(MetadataReader reader, object? genericContext, TypeSpecificationHandle handle, byte rawTypeKind) => reader.GetTypeSpecification(handle).DecodeSignature(this, genericContext);

    }

}
