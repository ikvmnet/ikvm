/*
  Copyright (C) 2009-2013 Jeroen Frijters

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
using System.Buffers;
using System.IO;
using System.Text;

namespace IKVM.Reflection.Reader
{

    sealed class StreamHeader
    {

        internal uint Offset;
        internal uint Size;
        internal string Name;

        internal void Read(BinaryReader br)
        {
            Offset = br.ReadUInt32();
            Size = br.ReadUInt32();
            Name = ReadName(br);
        }

#if NETFRAMEWORK

        string ReadName(BinaryReader br)
        {
            var buf = ArrayPool<byte>.Shared.Rent(32);

            try
            {
                byte b;
                int len = 0;
                while ((b = br.ReadByte()) != 0)
                    buf[len++] = b;

                int padding = -1 + ((len + 4) & ~3) - len;
                br.BaseStream.Seek(padding, SeekOrigin.Current);
                return Encoding.UTF8.GetString(buf, 0, len);
            }
            finally
            {
                if (buf != null)
                    ArrayPool<byte>.Shared.Return(buf);
            }
        }

#else

        unsafe string ReadName(BinaryReader br)
        {
            var buf = (Span<byte>)stackalloc byte[32];
            byte b;
            int len = 0;
            while ((b = br.ReadByte()) != 0)
                buf[len++] = b;

            int padding = -1 + ((len + 4) & ~3) - len;
            br.BaseStream.Seek(padding, SeekOrigin.Current);
            return Encoding.UTF8.GetString(buf.Slice(0, len));
        }

#endif

    }

}
