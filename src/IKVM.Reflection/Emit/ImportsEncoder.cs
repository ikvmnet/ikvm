using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Emit
{

    struct ImportsEncoder
    {

        readonly BlobBuilder writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportsEncoder(BlobBuilder writer)
        {
            this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void AliasAssemblyReference(BlobHandle alias, AssemblyReferenceHandle assembly)
        {
            // <import> ::= AliasAssemblyReference <alias> <target-assembly>
            writer.WriteByte((byte)ImportDefinitionKind.AliasAssemblyReference);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(assembly));
        }

        public void AliasType(BlobHandle alias, EntityHandle targetType)
        {
            // <import> ::= AliasType <alias> <target-type>
            writer.WriteByte((byte)ImportDefinitionKind.AliasType);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
            writer.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(targetType));
        }

        public void ImportType(EntityHandle targetType)
        {
            // <import> ::= AliasType <alias> <target-type>
            writer.WriteByte((byte)ImportDefinitionKind.ImportType);
            writer.WriteCompressedInteger(CodedIndex.TypeDefOrRefOrSpec(targetType));
        }

        public void AliasAssemblyNamespace(BlobHandle alias, AssemblyReferenceHandle targetAssembly, BlobHandle namespaceName)
        {
            // <import> ::= AliasAssemblyNamespace <alias> <target-assembly> <target-namespace>
            writer.WriteByte((byte)ImportDefinitionKind.AliasAssemblyNamespace);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(targetAssembly));
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

        public void ImportAssemblyNamespace(AssemblyReferenceHandle targetAssembly, BlobHandle namespaceName)
        {
            // <import> ::= ImportAssemblyNamespace <target-assembly> <target-namespace>
            writer.WriteByte((byte)ImportDefinitionKind.ImportAssemblyNamespace);
            writer.WriteCompressedInteger(MetadataTokens.GetRowNumber(targetAssembly));
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

        public void AliasNamespace(BlobHandle alias, BlobHandle namespaceName)
        {
            // <import> ::= AliasNamespace <alias> <target-namespace>
            writer.WriteByte((byte)ImportDefinitionKind.AliasNamespace);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

        public void ImportNamespace(BlobHandle namespaceName)
        {
            // <import> ::= ImportNamespace <target-namespace>
            writer.WriteByte((byte)ImportDefinitionKind.ImportNamespace);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(namespaceName));
        }

        public void ImportReferenceAlias(BlobHandle alias)
        {
            // <import> ::= ImportReferenceAlias <alias>
            writer.WriteByte((byte)ImportDefinitionKind.ImportAssemblyReferenceAlias);
            writer.WriteCompressedInteger(MetadataTokens.GetHeapOffset(alias));
        }

    }

}
