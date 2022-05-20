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

        private static int eventRequestCounter;

        private readonly byte eventKind;
        private readonly byte suspendPolicy;
        private readonly List<EventModifier> modifiers;
        private readonly int requestId;

        private EventRequest(byte eventKind, byte suspendPolicy, List<EventModifier> modifiers)
        {
            this.eventKind = eventKind;
            this.suspendPolicy = suspendPolicy;
            this.modifiers = modifiers;
            this.requestId = ++eventRequestCounter;
        }

        /// <summary>
        /// Create a new EventRequest with the data in the Packet
        /// </summary>
        /// <param name="packet">a data packet send from the Java debugger</param>
        /// <returns>a new packet or null if there are some unknown types.</returns>
        internal static EventRequest create(Packet packet)
        {
            byte eventKind = packet.ReadByte(); // class EventKind
            switch (eventKind)
            {
                case ikvm.debugger.EventKind.SINGLE_STEP:
                case ikvm.debugger.EventKind.BREAKPOINT:
                case ikvm.debugger.EventKind.FRAME_POP:
                case ikvm.debugger.EventKind.EXCEPTION:
                case ikvm.debugger.EventKind.USER_DEFINED:
                case ikvm.debugger.EventKind.THREAD_START:
                case ikvm.debugger.EventKind.THREAD_DEATH:
                case ikvm.debugger.EventKind.CLASS_PREPARE:
                case ikvm.debugger.EventKind.CLASS_UNLOAD:
                case ikvm.debugger.EventKind.CLASS_LOAD:
                case ikvm.debugger.EventKind.FIELD_ACCESS:
                case ikvm.debugger.EventKind.FIELD_MODIFICATION:
                case ikvm.debugger.EventKind.EXCEPTION_CATCH:
                case ikvm.debugger.EventKind.METHOD_ENTRY:
                case ikvm.debugger.EventKind.METHOD_EXIT:
                case ikvm.debugger.EventKind.METHOD_EXIT_WITH_RETURN_VALUE:
                case ikvm.debugger.EventKind.MONITOR_CONTENDED_ENTER:
                case ikvm.debugger.EventKind.MONITOR_CONTENDED_ENTERED:
                case ikvm.debugger.EventKind.MONITOR_WAIT:
                case ikvm.debugger.EventKind.MONITOR_WAITED:
                case ikvm.debugger.EventKind.VM_START:
                case ikvm.debugger.EventKind.VM_DEATH:
                case ikvm.debugger.EventKind.VM_DISCONNECTED:
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

        internal int RequestId
        {
            get { return requestId; }
        }

        internal int EventKind
        {
            get { return eventKind; }
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

    /// <summary>
    /// Limit the requested event to be reported at most once after a given number of occurrences. 
    /// The event is not reported the first count - 1 times this filter is reached. 
    /// To request a one-off event, call this method with a count of 1.
    /// 
    /// Once the count reaches 0, any subsequent filters in this request are applied. 
    /// If none of those filters cause the event to be suppressed, the event is reported. 
    /// Otherwise, the event is not reported. In either case subsequent events are never reported 
    /// for this request. This modifier can be used with any event kind.  
    /// </summary>
    class CountEventModifier : EventModifier
    {
        /// <summary>
        /// Count before event. One for one-off.  
        /// </summary>
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

    /// <summary>
    /// Conditional on expression  
    /// </summary>
    class ConditionalEventModifier : EventModifier
    {
        /// <summary>
        /// For the future
        /// </summary>
        private readonly int exprID;

        internal ConditionalEventModifier(Packet packet)
        {
            exprID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "Conditional:" + exprID;
        }
    }

    /// <summary>
    /// Restricts reported events to those in the given thread. 
    /// This modifier can be used with any event kind except for class unload.   
    /// </summary>
    class ThreadOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Required thread
        /// </summary>
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

    /// <summary>
    /// For class prepare events, restricts the events generated by this request to be the preparation 
    /// of the given reference type and any subtypes. For monitor wait and waited events, 
    /// restricts the events generated by this request to those whose monitor object 
    /// is of the given reference type or any of its subtypes. For other events, 
    /// restricts the events generated by this request to those whose location is 
    /// in the given reference type or any of its subtypes. An event will be generated 
    /// for any location in a reference type that can be safely cast to the given reference type. 
    /// This modifier can be used with any event kind except class unload, thread start, and thread end.
    /// </summary>
    class ClassOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Required class
        /// </summary>
        private readonly int classID;

        internal ClassOnlyEventModifier(Packet packet)
        {
            classID = packet.ReadObjectID();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassOnly:" + classID;
        }
    }

    /// <summary>
    /// Restricts reported events to those for classes whose name matches the given restricted 
    /// regular expression. For class prepare events, the prepared class name is matched. 
    /// For class unload events, the unloaded class name is matched. For monitor wait and waited events, 
    /// the name of the class of the monitor object is matched. For other events, 
    /// the class name of the event's location is matched. 
    /// This modifier can be used with any event kind except thread start and thread end.
    /// </summary>
    class ClassMatchEventModifier : EventModifier
    {
        /// <summary>
        /// Required class pattern. Matches are limited to exact matches of the given class pattern 
        /// and matches of patterns that begin or end with '*'; for example, "*.Foo" or "java.*".
        /// </summary>
        private readonly String classPattern;

        internal ClassMatchEventModifier(Packet packet)
        {
            classPattern = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassMatch:" + classPattern;
        }
    }

    /// <summary>
    /// Restricts reported events to those for classes whose name does not match the given 
    /// restricted regular expression. For class prepare events, the prepared class name is matched. 
    /// For class unload events, the unloaded class name is matched. For monitor wait and waited events, 
    /// the name of the class of the monitor object is matched. For other events, 
    /// the class name of the event's location is matched. 
    /// This modifier can be used with any event kind except thread start and thread end.
    /// </summary>
    class ClassExcludeEventModifier : EventModifier
    {
        /// <summary>
        /// Disallowed class pattern. Matches are limited to exact matches of the given class pattern 
        /// and matches of patterns that begin or end with '*'; for example, "*.Foo" or "java.*".
        /// </summary>
        private readonly String classPattern;

        internal ClassExcludeEventModifier(Packet packet)
        {
            classPattern = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassExclude:" + classPattern;
        }
    }

    /// <summary>
    /// Restricts reported events to those that occur at the given location. This modifier can be used 
    /// with breakpoint, field access, field modification, step, and exception event kinds.
    /// </summary>
    class LocationOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Required location
        /// </summary>
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

    /// <summary>
    /// Restricts reported exceptions by their class and whether they are caught or uncaught. 
    /// This modifier can be used with exception event kinds only.
    /// </summary>
    class ExceptionOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Exception to report. Null (0) means report exceptions of all types. A non-null type 
        /// restricts the reported exception events to exceptions of the given type or any of its subtypes.
        /// </summary>
        private readonly int refTypeID;
        /// <summary>
        /// Report caught exceptions
        /// </summary>
        private readonly bool caught;
        /// <summary>
        /// Report uncaught exceptions. Note that it is not always possible to determine 
        /// whether an exception is caught or uncaught at the time it is thrown. 
        /// See the exception event catch location under composite events for more information.
        /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_Event_Composite
        /// </summary>
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

    /// <summary>
    /// Restricts reported events to those that occur for a given field. 
    /// This modifier can be used with field access and field modification event kinds only.
    /// </summary>
    class FieldOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Type in which field is declared.
        /// </summary>
        private readonly int refTypeID;
        /// <summary>
        /// Required field
        /// </summary>
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

    /// <summary>
    /// Restricts reported step events to those which satisfy depth and size constraints. 
    /// This modifier can be used with step event kinds only.
    /// </summary>
    class StepEventModifier : EventModifier
    {
        /// <summary>
        /// Thread in which to step
        /// </summary>
        private readonly int threadID;
        /// <summary>
        /// size of each step. See class StepSize
        /// </summary>
        /// <see cref="StepSize"/>
        private readonly int size;
        /// <summary>
        /// relative call stack limit. See class StepDepth  
        /// </summary>
        /// <see cref="StepDepth"/>
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

    /// <summary>
    /// Restricts reported events to those whose active 'this' object is the given object. 
    /// Match value is the null object for static methods. 
    /// This modifier can be used with any event kind except class prepare, 
    /// class unload, thread start, and thread end. Introduced in JDWP version 1.4.
    /// </summary>
    class InstanceOnlyEventModifier : EventModifier
    {
        /// <summary>
        /// Required 'this' object
        /// </summary>
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
