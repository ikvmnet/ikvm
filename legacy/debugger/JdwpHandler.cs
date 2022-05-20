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
using ikvm.debugger.win;

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
                conn.SendPacket(packet);
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
                case VirtualMachine.ClassesBySignature:
                    String jniClassName = packet.ReadString();
                    IList<TargetType> types = target.FindTypes(jniClassName);
                    packet.WriteInt(types.Count); // count

                    foreach (TargetType type in types)
                    {
                        Console.Error.WriteLine("FindTypes:" + jniClassName + ":" + type.TypeId);
                        packet.WriteByte(TypeTag.CLASS); //TODO can also a interface
                        packet.WriteObjectID(type.TypeId);
                        packet.WriteInt(ClassStatus.INITIALIZED);
                    }

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
                    int size = 4; //we use a size of 4, a value of 8 is also possible
                    packet.WriteInt(size); // fieldID size in bytes
                    packet.WriteInt(size); // methodID size in bytes
                    packet.WriteInt(size); // objectID size in bytes
                    packet.WriteInt(size); // referenceTypeID size in bytes
                    packet.WriteInt(size); // frameID size in bytes
                    break;
                case VirtualMachine.Suspend:
                    target.Suspend();
                    break;
                case VirtualMachine.Resume:
                    target.Resume();
                    break;
                case VirtualMachine.Exit:
                    target.Exit(packet.ReadInt());
                    break;
                case VirtualMachine.Capabilities:
                    packet.WriteBool(false); // Can the VM watch field modification, and therefore can it send the Modification Watchpoint Event?  
                    packet.WriteBool(false); // Can the VM watch field access, and therefore can it send the Access Watchpoint Event?  
                    packet.WriteBool(false); // Can the VM get the bytecodes of a given method? 
                    packet.WriteBool(false); // Can the VM determine whether a field or method is synthetic? (that is, can the VM determine if the method or the field was invented by the compiler?)  
                    packet.WriteBool(false); // Can the VM get the owned monitors infornation for a thread?
                    packet.WriteBool(false); // Can the VM get the current contended monitor of a thread?  
                    packet.WriteBool(false); // Can the VM get the monitor information for a given object?   
                    break;
                case VirtualMachine.CapabilitiesNew:
                    packet.WriteBool(false); // Can the VM watch field modification, and therefore can it send the Modification Watchpoint Event?  
                    packet.WriteBool(false); // Can the VM watch field access, and therefore can it send the Access Watchpoint Event?  
                    packet.WriteBool(false); // Can the VM get the bytecodes of a given method? 
                    packet.WriteBool(false); // Can the VM determine whether a field or method is synthetic? (that is, can the VM determine if the method or the field was invented by the compiler?)  
                    packet.WriteBool(false); // Can the VM get the owned monitors infornation for a thread?
                    packet.WriteBool(false); // Can the VM get the current contended monitor of a thread?  
                    packet.WriteBool(false); // Can the VM get the monitor information for a given object?   
                    packet.WriteBool(false); // Can the VM redefine classes? 
                    packet.WriteBool(false); // Can the VM add methods when redefining classes? 
                    packet.WriteBool(false); // Can the VM redefine classesin arbitrary ways?  
                    packet.WriteBool(false); // Can the VM pop stack frames?  
                    packet.WriteBool(false); // Can the VM filter events by specific object? 
                    packet.WriteBool(false); // Can the VM get the source debug extension? 
                    packet.WriteBool(false); // Can the VM request VM death events?  
                    packet.WriteBool(false); // Can the VM set a default stratum?  
                    packet.WriteBool(false); // Can the VM return instances, counts of instances of classes and referring objects?  
                    packet.WriteBool(false); // Can the VM request monitor events?  
                    packet.WriteBool(false); // Can the VM get monitors with frame depth info?  
                    packet.WriteBool(false); // Can the VM filter class prepare events by source name?
                    packet.WriteBool(false); // Can the VM return the constant pool information?  
                    packet.WriteBool(false); // Can the VM force early return from a method?  
                    packet.WriteBool(false); // reserved22
                    packet.WriteBool(false); // reserved23
                    packet.WriteBool(false); // reserved24
                    packet.WriteBool(false); // reserved25
                    packet.WriteBool(false); // reserved26
                    packet.WriteBool(false); // reserved27
                    packet.WriteBool(false); // reserved28
                    packet.WriteBool(false); // reserved29
                    packet.WriteBool(false); // reserved30
                    packet.WriteBool(false); // reserved31
                    packet.WriteBool(false); // reserved32
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
                case ReferenceType.Signature:
                    int typeID = packet.ReadObjectID();
                    TargetType type = target.FindType(typeID);
                    Console.Error.WriteLine(typeID + ":" + type.GetJniSignature());
                    packet.WriteString(type.GetJniSignature());
                    break;
                case ReferenceType.ClassLoader:
                    int classLoaderID = packet.ReadObjectID();
                    packet.WriteObjectID(0); //TODO 0 - System Classloader, we can use module ID instead
                    break;
                case ReferenceType.MethodsWithGeneric:
                    typeID = packet.ReadObjectID();
                    Console.Error.WriteLine(typeID);
                    type = target.FindType(typeID);
                    IList<TargetMethod> methods = type.GetMethods();
                    packet.WriteInt(methods.Count);
                    foreach (TargetMethod method in methods)
                    {
                        Console.Error.WriteLine(method.MethodId + ":" + method.Name + ":" + method.JniSignature + ":" + method.GenericSignature+":"+method.AccessFlags);
                        packet.WriteObjectID(method.MethodId);
                        packet.WriteString(method.Name);
                        packet.WriteString(method.JniSignature);
                        packet.WriteString(method.GenericSignature);
                        packet.WriteInt(method.AccessFlags);
                    }
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
                        target.AddEventRequest(eventRequest);
                        packet.WriteInt(eventRequest.RequestId);
                    }
                    break;
                default:
                    NotImplementedPacket(packet);
                    break;
            }
        }

        private void NotImplementedPacket(Packet packet)
        {
            Console.Error.WriteLine("================================");
            Console.Error.WriteLine("Not Implemented Packet:" + packet.CommandSet + "-" + packet.Command);
            Console.Error.WriteLine("================================");
            packet.Error = Error.NOT_IMPLEMENTED;
        }
    }
}
