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
using System.Text;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection.Writer
{

    struct RESOURCEHEADER
    {

        internal int DataSize;
        internal int HeaderSize;
        internal OrdinalOrName TYPE;
        internal OrdinalOrName NAME;
        internal int DataVersion;
        internal ushort MemoryFlags;
        internal ushort LanguageId;
        internal int Version;
        internal int Characteristics;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="br"></param>
        internal RESOURCEHEADER(ByteReader br)
        {
            DataSize = br.ReadInt32();
            HeaderSize = br.ReadInt32();
            TYPE = ReadOrdinalOrName(br);
            NAME = ReadOrdinalOrName(br);
            br.Align(4);
            DataVersion = br.ReadInt32();
            MemoryFlags = br.ReadUInt16();
            LanguageId = br.ReadUInt16();
            Version = br.ReadInt32();
            Characteristics = br.ReadInt32();
        }

        static OrdinalOrName ReadOrdinalOrName(ByteReader br)
        {
            var c = br.ReadChar();
            if (c == 0xFFFF)
            {
                return new OrdinalOrName(br.ReadUInt16());
            }
            else
            {
                var sb = new StringBuilder();
                while (c != 0)
                {
                    sb.Append(c);
                    c = br.ReadChar();
                }

                return new OrdinalOrName(sb.ToString());
            }
        }

    }

}
