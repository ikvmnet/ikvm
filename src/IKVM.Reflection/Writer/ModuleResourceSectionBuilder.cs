using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Implements a <see cref="ResourceSectionBuilder"/> for writing IKVM reflection resource section data.
    /// </summary>
    class ModuleResourceSectionBuilder : ResourceSectionBuilder
    {

        const int RT_ICON = 3;
        const int RT_GROUP_ICON = 14;
        const int RT_VERSION = 16;
        const int RT_MANIFEST = 24;

        readonly ResourceDirectoryEntry root;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModuleResourceSectionBuilder()
        {
            root = new ResourceDirectoryEntry(new OrdinalOrName("root"));
        }

        /// <summary>
        /// Initializes a new instance, importing the resources from the specified builder.
        /// </summary>
        /// <param name="other"></param>
        public ModuleResourceSectionBuilder(ModuleResourceSectionBuilder other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            root = new ResourceDirectoryEntry(other.root);
        }

        /// <summary>
        /// Adds a VersionInfo structure.
        /// </summary>
        /// <param name="versionInfo"></param>
        internal void AddVersionInfo(ByteBuffer versionInfo)
        {
            root[new OrdinalOrName(RT_VERSION)][new OrdinalOrName(1)][new OrdinalOrName(0)].data = versionInfo;
        }

        /// <summary>
        /// Adds a Win32 ICO file.
        /// </summary>
        /// <param name="iconFile"></param>
        /// <exception cref="ArgumentException"></exception>
        internal void AddIcon(byte[] iconFile)
        {
            var br = new BinaryReader(new MemoryStream(iconFile));
            var idReserved = br.ReadUInt16();
            var idType = br.ReadUInt16();
            var idCount = br.ReadUInt16();
            if (idReserved != 0 || idType != 1)
                throw new ArgumentException("The supplied byte array is not a valid .ico file.");

            var group = new ByteBuffer(6 + 14 * idCount);
            group.Write(idReserved);
            group.Write(idType);
            group.Write(idCount);
            for (int i = 0; i < idCount; i++)
            {
                var bWidth = br.ReadByte();
                var bHeight = br.ReadByte();
                var bColorCount = br.ReadByte();
                var bReserved = br.ReadByte();
                var wPlanes = br.ReadUInt16();
                var wBitCount = br.ReadUInt16();
                var dwBytesInRes = br.ReadUInt32();
                var dwImageOffset = br.ReadUInt32();

                // we start the icon IDs at 2
                var id = (ushort)(2 + i);

                group.Write(bWidth);
                group.Write(bHeight);
                group.Write(bColorCount);
                group.Write(bReserved);
                group.Write(wPlanes);
                group.Write(wBitCount);
                group.Write(dwBytesInRes);
                group.Write(id);

                var icon = new byte[dwBytesInRes];
                Buffer.BlockCopy(iconFile, (int)dwImageOffset, icon, 0, icon.Length);
                root[new OrdinalOrName(RT_ICON)][new OrdinalOrName(id)][new OrdinalOrName(0)].data = ByteBuffer.Wrap(icon);
            }

            root[new OrdinalOrName(RT_GROUP_ICON)][new OrdinalOrName(32512)][new OrdinalOrName(0)].data = group;
        }

        /// <summary>
        /// Adds the native manifest.
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="resourceID"></param>
        internal void AddManifest(byte[] manifest, ushort resourceID)
        {
            root[new OrdinalOrName(RT_MANIFEST)][new OrdinalOrName(resourceID)][new OrdinalOrName(0)].data = ByteBuffer.Wrap(manifest);
        }

        /// <summary>
        /// Imports the resources from the specified Win32 resource file.
        /// </summary>
        /// <param name="buf"></param>
        internal void ImportWin32ResourceFile(byte[] buf)
        {
            var br = new ByteReader(buf, 0, buf.Length);
            while (br.Length >= 32)
            {
                br.Align(4);
                var hdr = new RESOURCEHEADER(br);
                if (hdr.DataSize != 0)
                    root[hdr.TYPE][hdr.NAME][new OrdinalOrName(hdr.LanguageId)].data = ByteBuffer.Wrap(br.ReadBytes(hdr.DataSize));
            }
        }

        /// <inheritdoc />
        protected override void Serialize(BlobBuilder builder, SectionLocation location)
        {
            var bb = new ByteBuffer(1024);
            var linkOffsets = new List<int>();
            root.Write(bb, linkOffsets);

            foreach (int offset in linkOffsets)
            {
                bb.Position = offset;
                bb.Write(bb.GetInt32AtCurrentPosition() + (int)location.RelativeVirtualAddress);
            }

            builder.WriteBytes(bb.ToArray());
        }

    }

}
