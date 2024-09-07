using System;
using System.Buffers;
using System.Text;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles writing diagnostic events to text output.
    /// </summary>
    static class TextDiagnosticFormat
    {

        /// <inheritdoc />
        public static void Write(in DiagnosticEvent @event, IBufferWriter<byte> writer, Encoding? encoding = null)
        {
            var w = new EncodingByteBufferWriter(encoding ?? Encoding.UTF8, writer);
            WriteDiagnosticEvent(ref w, @event);
        }

        /// <summary>
        /// Formats the <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        static void WriteDiagnosticEvent(ref EncodingByteBufferWriter writer, in DiagnosticEvent @event)
        {
            WriteDiagnosticLocation(ref writer, @event.Location);
            WriteDiagnosticLevel(ref writer, @event.Diagnostic.Level);
            writer.Write(" ");
            WriteDiagnosticCode(ref writer, @event.Diagnostic.Id);
            writer.Write(": ");
            WriteDiagnosticMessage(ref writer, @event.Diagnostic.Message, @event.Args);
            writer.WriteLine();
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="location"></param>
        static void WriteDiagnosticLocation(ref EncodingByteBufferWriter writer, in DiagnosticLocation location)
        {
            var any = false;

            if (location.Path != null)
            {
                writer.Write(location.Path);
                any = true;
            }

            if (location.StartLine > 0)
            {
                any = true;

                if (location.StartColumn > 0)
                {
                    if (location.EndColumn > 0)
                    {
                        if (location.EndLine > 0)
                        {
                            writer.Write($"({location.StartLine},{location.StartColumn},{location.EndLine},{location.EndColumn})");
                        }
                        else
                        {
                            writer.Write($"({location.StartLine},{location.StartColumn}-{location.EndColumn})");
                        }
                    }
                    else
                    {
                        writer.Write($"({location.StartLine},{location.StartColumn})");
                    }
                }
                else if (location.EndLine > 0)
                {
                    writer.Write($"({location.StartLine}-{location.EndLine})");
                }
                else
                {
                    writer.Write($"({location.StartLine})");
                }
            }

            if (any)
                writer.Write(" : ");
        }

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static void WriteDiagnosticLevel(ref EncodingByteBufferWriter writer, DiagnosticLevel level)
        {
            writer.Write(level switch
            {
                DiagnosticLevel.Trace => "trace",
                DiagnosticLevel.Info => "info",
                DiagnosticLevel.Warning => "warning",
                DiagnosticLevel.Error => "error",
                DiagnosticLevel.Fatal => "fatal",
                _ => throw new InvalidOperationException(),
            });
        }

        /// <summary>
        /// Formats the output text for the given diagnostic code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        static void WriteDiagnosticCode(ref EncodingByteBufferWriter writer, int code)
        {
            writer.Write("IKVM");
#if NETFRAMEWORK
            writer.Write($"{code:D4}");
#else
            var buf = (Span<char>)stackalloc char[16];
            if (code.TryFormat(buf, out var l, "D4") == false)
                throw new InvalidOperationException();

            writer.Write(buf);
#endif
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
#if NET8_0_OR_GREATER
        static void WriteDiagnosticMessage(ref EncodingByteBufferWriter writer, CompositeFormat message, ReadOnlySpan<object?> args)
#else
        static void WriteDiagnosticMessage(ref EncodingByteBufferWriter writer, string message, ReadOnlySpan<object?> args)
#endif
        {
#if NET8_0_OR_GREATER
            writer.Write(string.Format(null, message, args));
#else
            writer.Write(string.Format(null, message, args.ToArray()));
#endif
        }

    }

}
