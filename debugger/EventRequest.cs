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
                switch (modKind)
                {
                    case EventModifierKind.Count:
                    case EventModifierKind.Conditional:
                    case EventModifierKind.ThreadOnly:
                    case EventModifierKind.ClassOnly:
                        break;
                    case EventModifierKind.ClassMatch:
                        modifiers.Add(new ClassMatch(packet));
                        break;
                    case EventModifierKind.ClassExclude:
                        modifiers.Add(new ClassExclude(packet));
                        break;
                    case EventModifierKind.LocationOnly:
                    case EventModifierKind.ExceptionOnly:
                    case EventModifierKind.FieldOnly:
                    case EventModifierKind.Step:
                    case EventModifierKind.InstanceOnly:
                    case EventModifierKind.SourceNameMatch:
                        break;
                    default:
                        return null; //Invalid or not supported EventModifierKind
                }
            }
            return new EventRequest(eventKind, suspendPolicy, modifiers);
        }

        public override String ToString()
        {
            //for debugging
            String str = "EventRequest:" + eventKind + "," + suspendPolicy + "[";
            for(int i=0; i<modifiers.Count; i++)
            {
                str += modifiers[i] + ",";
            }
            str += "]";
            return str;
        }
    }

    class EventModifier
    {
    }

    class ClassMatch : EventModifier
    {
        private readonly String className;

        internal ClassMatch(Packet packet)
        {
            className = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassMatch:" + className;
        }
    }

    class ClassExclude : EventModifier
    {
        private readonly String className;

        internal ClassExclude(Packet packet)
        {
            className = packet.ReadString();
        }

        public override String ToString()
        {
            // for debugging
            return "ClassExclude:" + className;
        }
    }
}
