/*
  Copyright (C) 2009 Volker Berlin (vberlin@inetsoftware.de)

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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ikvm.debugger
{
    /// <summary>
    /// A JDWP Packet descriped at
    /// http://java.sun.com/javase/6/docs/technotes/guides/jpda/jdwp-spec.html
    /// </summary>
    class Packet
    {
        public const byte NoFlags = 0x0;
        public const byte Reply = 0x80;
        public const byte ReplyNoError = 0x0;

        private byte[] data;
        private int offset;

        private int id;
        private byte flags;
        private byte cmdSet;
        private byte cmd;
        private short errorCode;

        private Stream output = new MemoryStream();

        private Packet() { }

        /// <summary>
        /// Create a packet from the stream.
        /// </summary>
        /// <param name="header">The first 11 bytes of the data.</param>
        /// <param name="stream">The stream with the data</param>
        /// <returns>a new Packet</returns>
        /// <exception cref="IOException">If the data in the stream are invalid.</exception>
        internal static Packet Read(byte[] header, Stream stream)
        {
            Packet packet = new Packet();
            packet.data = header;
            int len = packet.ReadInt();
            if (len < 0)
            {
                throw new IOException("protocol error - invalid length");
            }
            packet.id = packet.ReadInt();
            packet.flags = packet.ReadByte();
            if ((packet.flags & Packet.Reply) == 0)
            {
                packet.cmdSet = packet.ReadByte();
                packet.cmd = packet.ReadByte();
            }
            else
            {
                packet.errorCode = packet.ReadShort();
            }
            packet.data = new byte[len - 11];
Console.Error.WriteLine("Data Size:" + packet.data.Length);
            DebuggerUtils.ReadFully(stream, packet.data);
            packet.offset = 0;
            return packet;
        }

        internal void Send(Stream stream)
        {
            MemoryStream ms = (MemoryStream)output;
            try
            {
                output = stream;
                WriteInt((int)ms.Length + 11);
                WriteInt(id);
                WriteByte(Reply);
                WriteShort(errorCode);
                ms.WriteTo(stream);
            }
            finally
            {
                output = ms; //remove the external stream
            }
        }

        internal int ReadInt()
        {
            return (data[offset++] << 24) |
                   (data[offset++] << 16) |
                   (data[offset++] << 8) |
                   (data[offset++]);
        }

        internal short ReadShort()
        {
            return (short)((data[offset++] << 8) | (data[offset++]));
        }

        internal byte ReadByte()
        {
            return data[offset++];
        }

        internal bool ReadBool()
        {
            return data[offset++] != 0;
        }

        internal String ReadString()
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            int length = ReadInt();
            char[] chars = encoding.GetChars(data, offset, length);
            offset += length;
            return new String(chars);
        }


        internal int ReadObjectID()
        {
            return ReadInt();
        }

        internal Location ReadLocation()
        {
            Location loc = new Location();
            loc.tagType = ReadByte();
            loc.classID = ReadObjectID();
            loc.methodID = ReadObjectID();
            loc.index = new byte[8];
            Array.Copy(data, offset, loc.index, 0, 8);
            offset += 8;
            return loc;
        }

        internal int Id
        {
            get { return id; }
        }

        internal int CommandSet
        {
            get { return cmdSet; }
        }

        internal int Command
        {
            get { return cmd; }
        }

        internal short Error
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        internal void WriteInt(int value)
        {
            output.WriteByte((byte)(value >> 24));
            output.WriteByte((byte)(value >> 16));
            output.WriteByte((byte)(value >> 8));
            output.WriteByte((byte)(value));
        }

        internal void WriteShort(int value)
        {
            output.WriteByte((byte)(value >> 8));
            output.WriteByte((byte)(value));
        }

        internal void WriteByte(int value)
        {
            output.WriteByte((byte)(value));
        }

        internal void WriteBool(bool value)
        {
            output.WriteByte(value ? (byte)1 : (byte)0);
        }

        internal void WriteString(String value)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(value);
            WriteInt(bytes.Length);
            output.Write(bytes, 0, bytes.Length);
        }

        internal void WriteObjectID(int value)
        {
            WriteInt(value);
        }

    }

    struct Location
    {
        internal byte tagType;
        internal int classID;
        internal int methodID;
        internal byte[] index;
    }
}
