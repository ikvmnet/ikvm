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

        // TODO Create a real implementation
        private readonly TargetVM target;

        internal JdwpHandler(JdwpConnection conn, TargetVM target)
        {
            this.conn = conn;
            this.target = target;
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

        /// <summary>
        /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_VirtualMachine
        /// </summary>
        /// <param name="packet"></param>
        private void CommandSetVirtualMachine(Packet packet)
        {
            switch (packet.Command)
            {
                case VirtualMachine.Version:
                    packet.WriteString("IKVM Debugger");
                    packet.WriteInt(1);
                    packet.WriteInt(6);
                    packet.WriteString("1.6.0");
                    packet.WriteString("IKVM.NET");
                    break;
                case VirtualMachine.AllThreads:
                    int[] ids = target.GetThreadIDs();
                    packet.WriteInt(ids.Length);
                    for (int i = 0; i < ids.Length; i++)
                    {
                        packet.WriteObjectID(ids[i]);
                    }
                    break;
                case VirtualMachine.IDSizes:
                    // TODO 64 Bit
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
                case EventRequest.Set:
                    byte eventKind = packet.ReadByte();
                    byte suspendPolicy = packet.ReadByte();
                    int count = packet.ReadInt();
Console.Error.WriteLine("Set:" + eventKind + "-" + suspendPolicy + "-" + count);
                    for (int i = 0; i < count; i++)
                    {
                        byte modKind = packet.ReadByte();
                        NotImplementedPacket(packet);
                    }
                    conn.SendPacket(packet);
                    break;
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
