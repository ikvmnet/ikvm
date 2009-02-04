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

        private Packet() { }

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

        internal void WriteInt(int size)
        {
            throw new NotImplementedException();
        }
    }
}
