#if NETCOREAPP

using IKVM.Reflection.Emit;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace IKVM.Reflection.Impl
{


    sealed class PortablePdbWriter : AbstractPdbWriter, ISymbolWriterImpl
    {

        private Guid guid = Guid.NewGuid();
        private uint timestamp = (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        public PortablePdbWriter(ModuleBuilder moduleBuilder) : base(moduleBuilder)
        {
        }

        private string PdbPath => System.IO.Path.ChangeExtension(moduleBuilder.FullyQualifiedName, ".pdb");

        public override void Close()
        {
            var metadataBuilder = new MetadataBuilder();

            foreach (Method method in methods)
            {
                int remappedToken = method.token;
                remap.TryGetValue(remappedToken, out remappedToken);

                // Just a placeholder to generate a row ID
                metadataBuilder.AddMethodDefinition(default, default, default, default, 0, default);

                if (method.document != null)
                {
                    DocumentHandle doc = GetUnmanagedDocument(metadataBuilder, method.document);
                    metadataBuilder.AddMethodDebugInformation(doc, MakeSequencePoints(metadataBuilder, method));
                }
                foreach (Scope scope in method.scopes)
                {
                    //metadataBuilder.AddLocalScope()
                    //WriteScope(scope);
                }
            }

            var serializer = new PortablePdbBuilder(
                metadataBuilder,
                ImmutableArray.CreateRange(Enumerable.Repeat(0, MetadataTokens.TableCount)),
                new MethodDefinitionHandle(),
                blob => new BlobContentId(guid, timestamp)
            );
            BlobBuilder blobBuilder = new BlobBuilder();
            serializer.Serialize(blobBuilder);

            using (FileStream fs = File.Open(PdbPath, FileMode.OpenOrCreate))
            {
                blobBuilder.WriteContentTo(fs);
            }

            documents.Clear();
            methods.Clear();
            remap.Clear();
            reversemap.Clear();
        }

        private BlobHandle MakeSequencePoints(MetadataBuilder builder, Method method)
        {
            // https://github.com/dotnet/runtime/blob/main/docs/design/specs/PortablePdb-Metadata.md#sequence-points-blob
            var writer = new BlobBuilder();

            // header:
            // LocalSignature
            writer.WriteCompressedInteger(0); // Omit for now?

            // Java does not have partial methods so initial document is not needed.

            var length = method.offsets.Length;
            Debug.Assert(method.lines.Length == length);
            Debug.Assert(method.columns.Length == length);
            Debug.Assert(method.endLines.Length == length);
            Debug.Assert(method.endColumns.Length == length);

            // Java does not have hidden sequence points so we can solely rely on previous entry.

            for (int i = 0; i < length; i++)
            {
                // ILOffset
                if (i == 0)
                {
                    writer.WriteCompressedInteger(method.offsets[i]);
                }
                else
                {
                    writer.WriteCompressedInteger(method.offsets[i] - method.offsets[i - 1]);
                }

                // Lines
                writer.WriteCompressedInteger(method.endLines[i] - method.lines[i]);

                // Columns
                if (method.endLines[i] - method.lines[i] == 0)
                {
                    writer.WriteCompressedInteger(method.endColumns[i] - method.columns[i]);
                }
                else
                {
                    writer.WriteCompressedSignedInteger(method.endColumns[i] - method.columns[i]);
                }

                // StartLine & StartColumn
                if (i == 0)
                {
                    writer.WriteCompressedInteger(method.lines[i]);
                    writer.WriteCompressedInteger(method.columns[i]);
                }
                else
                {
                    writer.WriteCompressedSignedInteger(method.lines[i] - method.lines[i - 1]);
                    writer.WriteCompressedSignedInteger(method.columns[i] - method.columns[i - 1]);
                }
            }

            return builder.GetOrAddBlob(writer);
        }

        private DocumentHandle GetUnmanagedDocument(MetadataBuilder builder, Document document)
        {
            string name = document.url;
            Guid language = document.language;

            return builder.AddDocument(
                name: builder.GetOrAddDocumentName(name),
                hashAlgorithm: builder.GetOrAddGuid(default),
                hash: default,
                language: builder.GetOrAddGuid(language));
        }

        public override byte[] GetDebugInfo(ref IMAGE_DEBUG_DIRECTORY idd)
        {
            // https://github.com/dotnet/runtime/blob/main/docs/design/specs/PE-COFF.md#codeview-debug-directory-entry-type-2

            idd.TimeDateStamp = timestamp;
            idd.MajorVersion = 0;
            idd.MinorVersion = 0x504d;
            idd.Type = 2;

            Span<byte> signature = stackalloc byte[] { 0x52, 0x53, 0x44, 0x53 };
            Span<byte> guid = this.guid.ToByteArray();
            Span<byte> age = stackalloc byte[4] { 1, 0, 0, 0 };
            Span<byte> path = Encoding.UTF8.GetBytes(PdbPath);

            idd.SizeOfData = 4 + 16 + 4 + (uint)path.Length + 1;
            byte[] buf = new byte[idd.SizeOfData];

            Span<byte> b = buf;
            signature.CopyTo(b);
            guid.CopyTo(b[4..]);
            age.CopyTo(b[20..]);
            path.CopyTo(b[24..]);
            b[(int)idd.SizeOfData - 1] = 0;

            return buf;
        }
    }
}

#endif
