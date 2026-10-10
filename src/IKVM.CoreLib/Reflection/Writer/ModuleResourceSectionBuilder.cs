using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Implements a <see cref="ResourceSectionBuilder"/> that builds the Win32 resource section (<c>.rsrc</c>) of a
    /// module: version information, icons and the native manifest, organized as a type, name and language tree.
    /// </summary>
    class ModuleResourceSectionBuilder : ResourceSectionBuilder
    {

        // the Win32 resource type IDs
        const int RT_ICON = 3;
        const int RT_GROUP_ICON = 14;
        const int RT_VERSION = 16;
        const int RT_MANIFEST = 24;

        readonly ResourceDirectoryEntry _root;

        /// <summary>
        /// Initializes a new instance with an empty resource tree.
        /// </summary>
        public ModuleResourceSectionBuilder()
        {
            _root = new ResourceDirectoryEntry(new OrdinalOrName("root"));
        }

        /// <summary>
        /// Initializes a new instance, importing the resources from the specified builder.
        /// </summary>
        /// <param name="other">The builder whose resource tree is copied.</param>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public ModuleResourceSectionBuilder(ModuleResourceSectionBuilder other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            _root = new ResourceDirectoryEntry(other._root);
        }

        /// <summary>
        /// Gets the number of entries in the root of the builder.
        /// </summary>
        public int Count => _root.Count;

        /// <summary>
        /// Adds a VersionInfo structure, as resource 1 of type <c>RT_VERSION</c>, replacing any existing one.
        /// </summary>
        /// <param name="versionInfo">The encoded VS_VERSIONINFO structure.</param>
        internal void AddVersionInfo(ByteBuffer versionInfo) =>
            _root[new OrdinalOrName(RT_VERSION)][new OrdinalOrName(1)][new OrdinalOrName(0)].Data = versionInfo;

        /// <summary>
        /// Adds a Win32 ICO file. Each image in the file becomes an <c>RT_ICON</c> resource, numbered from 2, and the
        /// directory becomes <c>RT_GROUP_ICON</c> resource 32512.
        /// </summary>
        /// <param name="iconFile">The contents of the .ico file.</param>
        /// <exception cref="ArgumentException"><paramref name="iconFile"/> is not a valid .ico file.</exception>
        internal void AddIcon(byte[] iconFile)
        {
            var br = new BinaryReader(new MemoryStream(iconFile));
            var idReserved = br.ReadUInt16();
            var idType = br.ReadUInt16();
            var idCount = br.ReadUInt16();
            if (idReserved != 0 || idType != 1)
                throw new ArgumentException("The supplied byte array is not a valid .ico file.");

            var group = new ByteBuffer(6 + 14 * idCount);
            group.WriteUInt16(idReserved);
            group.WriteUInt16(idType);
            group.WriteUInt16(idCount);
            for (var i = 0; i < idCount; i++)
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

                group.WriteByte(bWidth);
                group.WriteByte(bHeight);
                group.WriteByte(bColorCount);
                group.WriteByte(bReserved);
                group.WriteUInt16(wPlanes);
                group.WriteUInt16(wBitCount);
                group.WriteUInt32(dwBytesInRes);
                group.WriteUInt16(id);

                var icon = new byte[dwBytesInRes];
                Buffer.BlockCopy(iconFile, (int)dwImageOffset, icon, 0, icon.Length);
                _root[new OrdinalOrName(RT_ICON)][new OrdinalOrName(id)][new OrdinalOrName(0)].Data = ByteBuffer.Wrap(icon);
            }

            _root[new OrdinalOrName(RT_GROUP_ICON)][new OrdinalOrName(32512)][new OrdinalOrName(0)].Data = group;
        }

        /// <summary>
        /// Adds the native manifest, as a resource of type <c>RT_MANIFEST</c>.
        /// </summary>
        /// <param name="manifest">The contents of the manifest.</param>
        /// <param name="resourceID">The resource ID of the manifest.</param>
        internal void AddManifest(byte[] manifest, ushort resourceID) =>
            _root[new OrdinalOrName(RT_MANIFEST)][new OrdinalOrName(resourceID)][new OrdinalOrName(0)].Data = ByteBuffer.Wrap(manifest);

        /// <summary>
        /// Serializes the resource tree, relocating the data entry offsets to the RVA of the section.
        /// </summary>
        /// <param name="builder">The builder to write the section to.</param>
        /// <param name="location">The location of the section.</param>
        protected override void Serialize(BlobBuilder builder, SectionLocation location)
        {
            var bb = new ByteBuffer(1024);
            var linkOffsets = new List<int>();
            _root.Write(bb, linkOffsets);

            foreach (var offset in linkOffsets)
            {
                bb.Position = offset;
                bb.WriteInt32(bb.GetInt32AtCurrentPosition() + (int)location.RelativeVirtualAddress);
            }

            builder.WriteBytes(bb.ToArray());
        }

    }

}
