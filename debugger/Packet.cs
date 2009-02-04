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
    }
}
