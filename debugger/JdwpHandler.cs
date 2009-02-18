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
using ikvm.debugger.requests;

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
                Console.Error.WriteLine("Packet:"+packet.CommandSet + " " + packet.Command);
                switch (packet.CommandSet)
                {
                    case CommandSet.VirtualMachine:
                        CommandSetVirtualMachine(packet);
                        break;
                    case CommandSet.ReferenceType:
                        CommandSetReferenceType(packet);
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
                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.ClassesBySignature:
                    String jniClassName = packet.ReadString();
                    packet.WriteInt(1); // count

                    packet.WriteByte(TypeTag.CLASS);
                    packet.WriteObjectID(target.GetTypeID(jniClassName)); //TODO should be a ID
                    packet.WriteInt(ClassStatus.INITIALIZED);

                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.AllThreads:
                    int[] ids = target.GetThreadIDs();
                    packet.WriteInt(ids.Length);
                    for (int i = 0; i < ids.Length; i++)
                    {
                        packet.WriteObjectID(ids[i]);
                    }
                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.IDSizes:
                    int size = 4; //we use a size of 4, a value of 8 is also possible
                    packet.WriteInt(size); // fieldID size in bytes
                    packet.WriteInt(size); // methodID size in bytes
                    packet.WriteInt(size); // objectID size in bytes
                    packet.WriteInt(size); // referenceTypeID size in bytes
                    packet.WriteInt(size); // frameID size in bytes
                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.Suspend:
                    target.Suspend();
                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.Resume:
                    target.Resume();
                    conn.SendPacket(packet);
                    break;
                case VirtualMachine.Exit:
                    target.Exit(packet.ReadInt());
                    //no SendPacket
                    break;
                default:
                    NotImplementedPacket(packet); // include a SendPacket
                    break;
            }
        }

        /// <summary>
        /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_ReferenceType
        /// </summary>
        /// <param name="packet"></param>
        private void CommandSetReferenceType(Packet packet)
        {
            switch (packet.Command)
            {
                case ReferenceType.MethodsWithGeneric:
                    int refType = packet.ReadInt();
                    Console.Error.WriteLine(refType);
                    NotImplementedPacket(packet);
                    break;
                default:
                    NotImplementedPacket(packet);
                    break;
            }
        }

        /// <summary>
        /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_EventRequest
        /// </summary>
        /// <param name="packet"></param>
        private void CommandSetEventRequest(Packet packet)
        {
            switch (packet.Command)
            {
                case EventRequest.CmdSet:
                    EventRequest eventRequest = EventRequest.create(packet);
                    Console.Error.WriteLine(eventRequest);
                    if (eventRequest == null)
                    {
                        NotImplementedPacket(packet);
                    }
                    else
                    {
                        packet.WriteInt(packet.Id); // should be EventID and not PacketID
                        conn.SendPacket(packet);
                    }
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
