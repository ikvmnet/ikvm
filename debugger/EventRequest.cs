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

namespace ikvm.debugger.requests
{
    /// <summary>
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_EventRequest
    /// </summary>
    class EventRequest
    {
        internal const int CmdSet = 1;
        internal const int CmdClear = 2;
        internal const int CmdClearAllBreakpoints = 3;

        private byte eventKind;
        private byte suspendPolicy;
        private List<EventModifier> modifiers;

        private EventRequest(byte eventKind, byte suspendPolicy, List<EventModifier> modifiers)
        {
            this.eventKind = eventKind;
            this.suspendPolicy = suspendPolicy;
            this.modifiers = modifiers;
        }

        internal static EventRequest create(Packet packet)
        {
            byte eventKind = packet.ReadByte(); // class EventKind
            switch (eventKind)
            {
                case EventKind.SINGLE_STEP:
                case EventKind.BREAKPOINT:
                case EventKind.FRAME_POP:
                case EventKind.EXCEPTION:
                case EventKind.USER_DEFINED:
                case EventKind.THREAD_START:
                case EventKind.THREAD_DEATH:
                case EventKind.CLASS_PREPARE:
                case EventKind.CLASS_UNLOAD:
                case EventKind.CLASS_LOAD:
                case EventKind.FIELD_ACCESS:
                case EventKind.FIELD_MODIFICATION:
                case EventKind.EXCEPTION_CATCH:
                case EventKind.METHOD_ENTRY:
                case EventKind.METHOD_EXIT:
                case EventKind.METHOD_EXIT_WITH_RETURN_VALUE:
                case EventKind.MONITOR_CONTENDED_ENTER:
                case EventKind.MONITOR_CONTENDED_ENTERED:
                case EventKind.MONITOR_WAIT:
                case EventKind.MONITOR_WAITED:
                case EventKind.VM_START:
                case EventKind.VM_DEATH:
                case EventKind.VM_DISCONNECTED:
                    break;
                default:
                    return null; //Invalid or not supported EventKind
            }
            byte suspendPolicy = packet.ReadByte();
            int count = packet.ReadInt();
            Console.Error.WriteLine("Set:" + eventKind + "-" + suspendPolicy + "-" + count);
            List<EventModifier> modifiers = new List<EventModifier>();
            for (int i = 0; i < count; i++)
            {
                byte modKind = packet.ReadByte(); // class EventModifierKind
                Console.Error.WriteLine("EventModifierKind:" + modKind);
                EventModifier modifier;
                switch (modKind)
                {
                    case EventModifierKind.Count:
                        modifier = new CountEventModifier(packet);
                        break;
                    case EventModifierKind.Conditional:
                        modifier = new ConditionalEventModifier(packet);
                        break;
                    case EventModifierKind.ThreadOnly:
                        modifier = new ThreadOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.ClassOnly:
                        modifier = new ThreadOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.ClassMatch:
                        modifier = new ClassMatchEventModifier(packet);
                        break;
                    case EventModifierKind.ClassExclude:
                        modifier = new ClassExcludeEventModifier(packet);
                        break;
                    case EventModifierKind.LocationOnly:
                        modifier = new LocationOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.ExceptionOnly:
                        modifier = new ExceptionOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.FieldOnly:
                        modifier = new FieldOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.Step:
                        modifier = new StepEventModifier(packet);
                        break;
                    case EventModifierKind.InstanceOnly:
                        modifier = new InstanceOnlyEventModifier(packet);
                        break;
                    case EventModifierKind.SourceNameMatch:
                        modifier = new SourceNameMatchEventModifier(packet);
                        break;
                    default:
                        return null; //Invalid or not supported EventModifierKind
                }
                modifiers.Add(modifier);
            }
            return new EventRequest(eventKind, suspendPolicy, modifiers);
        }

        public override String ToString()
        {
            //for debugging
            String str = "EventRequest:" + eventKind + "," + suspendPolicy + "[";
            for (int i = 0; i < modifiers.Count; i++)
            {
                str += modifiers[i] + ",";
            }
            str += "]";
            return str;
        }
    }

    abstract class EventModifier
    {
    }

    class CountEventModifier : EventModifier
    {
        private readonly int count;

        internal CountEventModifier(Packet packet)
        {
            count = packet.ReadInt();
        }

        public override String ToString()
        {
            // for debugging
            return "Count:" + count;
        }
    }

    class ConditionalEventModifier : EventModifier
    {
        private readonly int count;

        internal ConditionalEventModifier(Packet packet)
        {
            count = packet.ReadInt();
        }

        public override String ToString()
        {
            // for debugging
            return "Conditional:" + count;
        }
    }

    class ThreadOnlyEventModifier : EventModifier
    {
        private readonly int threadID;

        internal ThreadOnlyEventModifier(Packet packet)
        {
            threadID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "ThreadOnly:" + threadID;
        }
    }

    class ClassOnlyEventModifier : EventModifier
    {
        private readonly int typeID;

        internal ClassOnlyEventModifier(Packet packet)
        {
            typeID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassOnly:" + typeID;
        }
    }

    class ClassMatchEventModifier : EventModifier
    {
        private readonly String className;

        internal ClassMatchEventModifier(Packet packet)
        {
            className = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassMatch:" + className;
        }
    }

    class ClassExcludeEventModifier : EventModifier
    {
        private readonly String className;

        internal ClassExcludeEventModifier(Packet packet)
        {
            className = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassExclude:" + className;
        }
    }

    class LocationOnlyEventModifier : EventModifier
    {
        private readonly Location location;

        internal LocationOnlyEventModifier(Packet packet)
        {
            location = packet.ReadLocation();
        }

        public override String ToString()
        {
            // for debugging
            return "LocationOnly:" + location;
        }
    }

    class ExceptionOnlyEventModifier : EventModifier
    {
        private readonly int refTypeID;
        private readonly bool caught;
        private readonly bool uncaught;

        internal ExceptionOnlyEventModifier(Packet packet)
        {
            refTypeID = packet.ReadObjectID();
            caught = packet.ReadBool();
            uncaught = packet.ReadBool();
        }

        public override String ToString()
        {
            // for debugging
            return "ExceptionOnly:" + refTypeID + "," + caught + "," + uncaught;
        }
    }

    class FieldOnlyEventModifier : EventModifier
    {
        private readonly int refTypeID;
        private readonly int fieldID;

        internal FieldOnlyEventModifier(Packet packet)
        {
            refTypeID = packet.ReadObjectID();
            fieldID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "FieldOnly:" + refTypeID + "," + fieldID;
        }
    }

    class StepEventModifier : EventModifier
    {
        private readonly int threadID;
        private readonly int size;
        private readonly int depth;

        internal StepEventModifier(Packet packet)
        {
            threadID = packet.ReadObjectID();
            size = packet.ReadInt();
            depth = packet.ReadInt();
        }

        public override String ToString()
        {
            // for debugging
            return "Step:" + threadID + "," + size + "," + depth;
        }
    }

    class InstanceOnlyEventModifier : EventModifier
    {
        private readonly int instanceID;

        internal InstanceOnlyEventModifier(Packet packet)
        {
            instanceID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "InstanceOnly:" + instanceID;
        }
    }

    /// <summary>
    /// Restricts reported class prepare events to those for reference types 
    /// which have a source name which matches the given restricted regular expression. 
    /// The source names are determined by the reference type's  SourceDebugExtension. 
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_ReferenceType_SourceDebugExtension
    /// This modifier can only be used with class prepare events. 
    /// Since JDWP version 1.6. Requires the canUseSourceNameFilters capability - see CapabilitiesNew.
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_VirtualMachine_CapabilitiesNew
    /// </summary>
    class SourceNameMatchEventModifier : EventModifier
    {
        /// <summary>
        /// Required source name pattern. Matches are limited to exact matches of the given pattern and matches of patterns 
        /// that begin or end with '*'; for example, "*.Foo" or "java.*".   
        /// </summary>
        private readonly String sourceNamePattern;

        internal SourceNameMatchEventModifier(Packet packet)
        {
            sourceNamePattern = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "SourceNameMatch:" + sourceNamePattern;
        }
    }
}
