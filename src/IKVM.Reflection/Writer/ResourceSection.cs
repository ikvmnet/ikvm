/*
  Copyright (C) 2010-2012 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.IO;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Writer
{

    sealed class ResourceSection
    {

        const int RT_ICON = 3;
        const int RT_GROUP_ICON = 14;
        const int RT_VERSION = 16;
        const int RT_MANIFEST = 24;

        ResourceDirectoryEntry root = new ResourceDirectoryEntry(new OrdinalOrName("root"));
        ByteBuffer bb;
        List<int> linkOffsets;

        internal void AddVersionInfo(ByteBuffer versionInfo)
        {
            root[new OrdinalOrName(RT_VERSION)][new OrdinalOrName(1)][new OrdinalOrName(0)].Data = versionInfo;
        }

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
                root[new OrdinalOrName(RT_ICON)][new OrdinalOrName(id)][new OrdinalOrName(0)].Data = ByteBuffer.Wrap(icon);
            }

            root[new OrdinalOrName(RT_GROUP_ICON)][new OrdinalOrName(32512)][new OrdinalOrName(0)].Data = group;
        }

        internal void AddManifest(byte[] manifest, ushort resourceID)
        {
            root[new OrdinalOrName(RT_MANIFEST)][new OrdinalOrName(resourceID)][new OrdinalOrName(0)].Data = ByteBuffer.Wrap(manifest);
        }

        internal void ExtractResources(byte[] buf)
        {
            var br = new ByteReader(buf, 0, buf.Length);
            while (br.Length >= 32)
            {
                br.Align(4);
                var hdr = new RESOURCEHEADER(br);
                if (hdr.DataSize != 0)
                    root[hdr.TYPE][hdr.NAME][new OrdinalOrName(hdr.LanguageId)].Data = ByteBuffer.Wrap(br.ReadBytes(hdr.DataSize));
            }
        }

        internal void Finish()
        {
            if (bb != null)
                throw new InvalidOperationException();

            bb = new ByteBuffer(1024);
            linkOffsets = new List<int>();
            root.Write(bb, linkOffsets);
            root = null;
        }

        internal int Length
        {
            get { return bb.Length; }
        }

        internal void Write(MetadataWriter mw, uint rva)
        {
            foreach (int offset in linkOffsets)
            {
                bb.Position = offset;
                bb.Write(bb.GetInt32AtCurrentPosition() + (int)rva);
            }

            mw.Write(bb);
        }

    }

}
