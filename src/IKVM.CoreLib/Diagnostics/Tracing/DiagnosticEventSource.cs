using System;
using System.Diagnostics.Tracing;

namespace IKVM.CoreLib.Diagnostics.Tracing
{

    /// <summary>
    /// Provides a <see cref="EventSource"/> to receive emitted IKVM diagnostic messages.
    /// </summary>
    [EventSource(Name = "IKVM")]
    unsafe partial class DiagnosticEventSource : EventSource, IDiagnosticHandler
    {

        /// <summary>
        /// Gets the default instance of this event source.
        /// </summary>
        public static readonly DiagnosticEventSource Instance = new DiagnosticEventSource();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        DiagnosticEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat | EventSourceSettings.ThrowOnEventWriteErrors)
        {

        }

        /// <inheritdoc />
        [NonEvent]
        bool IDiagnosticHandler.IsEnabled(Diagnostic diagnostic)
        {
            return IsEnabled(ToLevel(diagnostic.Level), EventKeywords.All);
        }

        /// <summary>
        /// Translates a <see cref="DiagnosticLevel"/> to a <see cref="EventLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [NonEvent]
        EventLevel ToLevel(DiagnosticLevel level)
        {
            return level switch
            {
                DiagnosticLevel.Trace => EventLevel.Verbose,
                DiagnosticLevel.Informational => EventLevel.Informational,
                DiagnosticLevel.Warning => EventLevel.Warning,
                DiagnosticLevel.Error => EventLevel.Error,
                DiagnosticLevel.Fatal => EventLevel.Critical,
                _ => throw new NotImplementedException()
            };
        }

        [NonEvent]
        void WriteEvent(int eventId, string arg0, string arg1, string arg2, string arg3)
        {
            arg0 ??= "";
            arg1 ??= "";
            arg2 ??= "";
            arg3 ??= "";

            fixed (char* arg0Ptr = arg0)
            fixed (char* arg1Ptr = arg1)
            fixed (char* arg2Ptr = arg2)
            fixed (char* arg3Ptr = arg3)
            {
                var ptr = stackalloc EventData[4];
                ptr[0].DataPointer = (IntPtr)arg0Ptr;
                ptr[0].Size = (arg0.Length + 1) * 2;
                ptr[1].DataPointer = (IntPtr)arg1Ptr;
                ptr[1].Size = (arg1.Length + 1) * 2;
                ptr[2].DataPointer = (IntPtr)arg2Ptr;
                ptr[2].Size = (arg2.Length + 1) * 2;
                ptr[3].DataPointer = (IntPtr)arg3Ptr;
                ptr[3].Size = (arg3.Length + 1) * 2;
                WriteEventCore(eventId, 4, ptr);
            }
        }

    }

}
