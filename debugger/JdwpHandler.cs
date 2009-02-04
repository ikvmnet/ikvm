using System;
using System.Collections.Generic;
using System.Text;

namespace ikvm.debugger
{
    /// <summary>
    /// Implementation of the JDWP Protocol. The documentation is at:
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html
    /// </summary>
    class JdwpHandler
    {

        private readonly JdwpConnection conn;

        internal JdwpHandler(JdwpConnection conn)
        {
            this.conn = conn;
        }

        internal void Run()
        {
            while (true)
            {
                Packet packet = conn.ReadPacket();
                Console.WriteLine(packet.CommandSet + " " + packet.Command);
                switch (packet.CommandSet)
                {
                    case CommandSet.VirtualMachine:
                        CommandSetVirtualMachine(packet);
                        break;
                    case CommandSet.EventRequest:
                        CommandSetEventRequest(packet);
                        break;
                    default:
                        NotImplementedPacket(packet);
                        break;
                }
            }
        }

        private void CommandSetVirtualMachine(Packet packet)
        {
            switch (packet.Command)
            {
                case VirtualMachine.IDSizes:
                    int size = System.IntPtr.Size;
                    packet.WriteInt(size); // fieldID size in bytes
                    packet.WriteInt(size); // methodID size in bytes
                    packet.WriteInt(size); // objectID size in bytes
                    packet.WriteInt(size); // referenceTypeID size in bytes
                    packet.WriteInt(size); // frameID size in bytes
                    conn.SendPacket(packet);
                    break;
                default:
                    NotImplementedPacket(packet);
                    break;
            }
        }

        private void CommandSetEventRequest(Packet packet)
        {
            switch (packet.Command)
            {
                default:
                    NotImplementedPacket(packet);
                    break;
            }
        }

        private void NotImplementedPacket(Packet packet)
        {
            Console.Error.WriteLine("Not Implemented Packet:" + packet.CommandSet + "-" + packet.Command);
            packet.Error = Error.NOT_IMPLEMENTED;
            conn.SendPacket(packet);
        }
    }
}
