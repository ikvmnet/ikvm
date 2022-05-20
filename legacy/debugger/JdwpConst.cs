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

    static class CommandSet
    {
        internal const int VirtualMachine = 1;
        internal const int ReferenceType = 2;
        internal const int ClassType = 3;
        internal const int ArrayType = 4;
        internal const int InterfaceType = 5;
        internal const int Method = 6;
        internal const int Field = 8;
        internal const int ObjectReference = 9;
        internal const int StringReference = 10;
        internal const int ThreadReference = 11;
        internal const int ThreadGroupReference = 12;
        internal const int ArrayReference = 13;
        internal const int ClassLoaderReference = 14;
        internal const int EventRequest = 15;
        internal const int StackFrame = 16;
        internal const int ClassObjectReference = 17;
        internal const int Event = 64;
    }

    static class VirtualMachine
    {
        internal const int Version = 1;
        internal const int ClassesBySignature = 2;
        internal const int AllClasses = 3;
        internal const int AllThreads = 4;
        internal const int TopLevelThreadGroups = 5;
        internal const int Dispose = 6;
        internal const int IDSizes = 7;
        internal const int Suspend = 8;
        internal const int Resume = 9;
        internal const int Exit = 10;
        internal const int CreateString = 11;
        internal const int Capabilities = 12;
        internal const int ClassPaths = 13;
        internal const int DisposeObjects = 14;
        internal const int HoldEvents = 15;
        internal const int ReleaseEvents = 16;
        internal const int CapabilitiesNew = 17;
        internal const int RedefineClasses = 18;
        internal const int SetDefaultStratum = 19;
        internal const int AllClassesWithGeneric = 20;
        internal const int InstanceCounts = 21;
    }

    static class ReferenceType
    {
        internal const int Signature = 1;
        internal const int ClassLoader = 2;
        internal const int Modifiers = 3;
        internal const int Fields = 4;
        internal const int Methods = 5;
        internal const int GetValues = 6;
        internal const int SourceFile = 7;
        internal const int NestedTypes = 8;
        internal const int Status = 9;
        internal const int Interfaces = 10;
        internal const int ClassObject = 11;
        internal const int SourceDebugExtension = 12;
        internal const int SignatureWithGeneric = 13;
        internal const int FieldsWithGeneric = 14;
        internal const int MethodsWithGeneric = 15;
        internal const int Instances = 16;
        internal const int ClassFileVersion = 17;
        internal const int ConstantPool = 18;
    }

    static class Error
    {
        internal const int NONE = 0;
        internal const int INVALID_THREAD = 10;
        internal const int INVALID_THREAD_GROUP = 11;
        internal const int INVALID_PRIORITY = 12;
        internal const int THREAD_NOT_SUSPENDED = 13;
        internal const int THREAD_SUSPENDED = 14;
        internal const int THREAD_NOT_ALIVE = 15;
        internal const int INVALID_OBJECT = 20;
        internal const int INVALID_CLASS = 21;
        internal const int CLASS_NOT_PREPARED = 22;
        internal const int INVALID_METHODID = 23;
        internal const int INVALID_LOCATION = 24;
        internal const int INVALID_FIELDID = 25;
        internal const int INVALID_FRAMEID = 30;
        internal const int NO_MORE_FRAMES = 31;
        internal const int OPAQUE_FRAME = 32;
        internal const int NOT_CURRENT_FRAME = 33;
        internal const int TYPE_MISMATCH = 34;
        internal const int INVALID_SLOT = 35;
        internal const int DUPLICATE = 40;
        internal const int NOT_FOUND = 41;
        internal const int INVALID_MONITOR = 50;
        internal const int NOT_MONITOR_OWNER = 51;
        internal const int INTERRUPT = 52;
        internal const int INVALID_CLASS_FORMAT = 60;
        internal const int CIRCULAR_CLASS_DEFINITION = 61;
        internal const int FAILS_VERIFICATION = 62;
        internal const int ADD_METHOD_NOT_IMPLEMENTED = 63;
        internal const int SCHEMA_CHANGE_NOT_IMPLEMENTED = 64;
        internal const int INVALID_TYPESTATE = 65;
        internal const int HIERARCHY_CHANGE_NOT_IMPLEMENTED = 66;
        internal const int DELETE_METHOD_NOT_IMPLEMENTED = 67;
        internal const int UNSUPPORTED_VERSION = 68;
        internal const int NAMES_DONT_MATCH = 69;
        internal const int CLASS_MODIFIERS_CHANGE_NOT_IMPLEMENTED = 70;
        internal const int METHOD_MODIFIERS_CHANGE_NOT_IMPLEMENTED = 71;
        internal const int NOT_IMPLEMENTED = 99;
        internal const int NULL_POINTER = 100;
        internal const int ABSENT_INFORMATION = 101;
        internal const int INVALID_EVENT_TYPE = 102;
        internal const int ILLEGAL_ARGUMENT = 103;
        internal const int OUT_OF_MEMORY = 110;
        internal const int ACCESS_DENIED = 111;
        internal const int VM_DEAD = 112;
        internal const int INTERNAL = 113;
        internal const int UNATTACHED_THREAD = 115;
        internal const int INVALID_TAG = 500;
        internal const int ALREADY_INVOKING = 502;
        internal const int INVALID_INDEX = 503;
        internal const int INVALID_LENGTH = 504;
        internal const int INVALID_STRING = 506;
        internal const int INVALID_CLASS_LOADER = 507;
        internal const int INVALID_ARRAY = 508;
        internal const int TRANSPORT_LOAD = 509;
        internal const int TRANSPORT_INIT = 510;
        internal const int NATIVE_METHOD = 511;
        internal const int INVALID_COUNT = 512;
    }

    static class EventKind
    {
        internal const int SINGLE_STEP = 1;
        internal const int BREAKPOINT = 2;
        internal const int FRAME_POP = 3;
        internal const int EXCEPTION = 4;
        internal const int USER_DEFINED = 5;
        internal const int THREAD_START = 6;
        internal const int THREAD_DEATH = 7;
        internal const int CLASS_PREPARE = 8;
        internal const int CLASS_UNLOAD = 9;
        internal const int CLASS_LOAD = 10;
        internal const int FIELD_ACCESS = 20;
        internal const int FIELD_MODIFICATION = 21;
        internal const int EXCEPTION_CATCH = 30;
        internal const int METHOD_ENTRY = 40;
        internal const int METHOD_EXIT = 41;
        internal const int METHOD_EXIT_WITH_RETURN_VALUE = 42;
        internal const int MONITOR_CONTENDED_ENTER = 43;
        internal const int MONITOR_CONTENDED_ENTERED = 44;
        internal const int MONITOR_WAIT = 45;
        internal const int MONITOR_WAITED = 46;
        internal const int VM_START = 90;
        internal const int VM_DEATH = 99;
        internal const int VM_DISCONNECTED = 100;
    }

    static class EventModifierKind
    {
        internal const int Count = 1;
        internal const int Conditional = 2;
        internal const int ThreadOnly = 3;
        internal const int ClassOnly = 4;
        internal const int ClassMatch = 5;
        internal const int ClassExclude = 6;
        internal const int LocationOnly = 7;
        internal const int ExceptionOnly = 8;
        internal const int FieldOnly = 9;
        internal const int Step = 10;
        internal const int InstanceOnly = 11;
        internal const int SourceNameMatch = 12;
    }

    static class ThreadStatus
    {
        internal const int ZOMBIE = 0;
        internal const int RUNNING = 1;
        internal const int SLEEPING = 2;
        internal const int MONITOR = 3;
        internal const int WAIT = 4;
    }

    static class SuspendStatus
    {
        internal const int SUSPEND_STATUS_SUSPENDED = 0x1;
    }

    static class ClassStatus
    {
        internal const int VERIFIED = 1;
        internal const int PREPARED = 2;
        internal const int INITIALIZED = 4;
        internal const int ERROR = 8;
    }

    static class TypeTag
    {
        internal const int CLASS = 1;
        internal const int INTERFACE = 2;
        internal const int ARRAY = 3;
    }

    static class Tag
    {
        internal const int ARRAY = 91;
        internal const int BYTE = 66;
        internal const int CHAR = 67;
        internal const int OBJECT = 76;
        internal const int FLOAT = 70;
        internal const int DOUBLE = 68;
        internal const int INT = 73;
        internal const int LONG = 74;
        internal const int SHORT = 83;
        internal const int VOID = 86;
        internal const int BOOLEAN = 90;
        internal const int STRING = 115;
        internal const int THREAD = 116;
        internal const int THREAD_GROUP = 103;
        internal const int CLASS_LOADER = 108;
        internal const int CLASS_OBJECT = 99;
    }

    /// <summary>
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_StepDepth
    /// </summary>
    static class StepDepth
    {
        /// <summary>
        /// Step into any method calls that occur before the end of the step.
        /// </summary>
        internal const int INTO = 0;
        /// <summary>
        /// Step over any method calls that occur before the end of the step.
        /// </summary>
        internal const int OVER = 1;
        /// <summary>
        /// Step out of the current method.
        /// </summary>
        internal const int OUT = 2;
    }

    /// <summary>
    /// http://java.sun.com/javase/6/docs/platform/jpda/jdwp/jdwp-protocol.html#JDWP_StepSize
    /// </summary>
    static class StepSize
    {
        /// <summary>
        /// Step by the minimum possible amount (often a bytecode instruction).   
        /// </summary>
        internal const int MIN = 0;
        /// <summary>
        /// Step to the next source line unless there is no line number information in which case a MIN step is done instead.
        /// </summary>
        internal const int LINE = 1;
    }

    static class SuspendPolicy
    {
        internal const int NONE = 0;
        internal const int EVENT_THREAD = 1;
        internal const int ALL = 2;
    }

    /// <summary>
    /// The invoke options are a combination of zero or more of the following bit flags:
    /// </summary>
    static class InvokeOptions
    {
        internal const int INVOKE_SINGLE_THREADED = 0x01;
        internal const int INVOKE_NONVIRTUAL = 0x02;
    }

}
