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
    class TextDiagnosticFormatter : DiagnosticChannelFormatter<TextDiagnosticFormatterOptions>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public TextDiagnosticFormatter(TextDiagnosticFormatterOptions options) :
            base(options)
        {

        }

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void FormatDiagnosticLevel(ref EncodingSpanWriter writer, DiagnosticLevel level)
        {
            writer.Write(level switch
            {
                DiagnosticLevel.Trace => "trace",
                DiagnosticLevel.Informational => "info",
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
        protected virtual void FormatDiagnosticCode(ref EncodingSpanWriter writer, int code)
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
        protected virtual void FormatDiagnosticMessage(ref EncodingSpanWriter writer, CompositeFormat message, ReadOnlySpan<object?> args)
#else
        protected virtual void FormatDiagnosticMessage(ref EncodingSpanWriter writer, string message, ReadOnlySpan<object?> args)
#endif
        {
#if NET8_0_OR_GREATER
            writer.Write(string.Format(null, message, args));
#else
            writer.Write(string.Format(null, message, args.ToArray()));
#endif
        }

        /// <summary>
        /// Formats the <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        protected virtual void FormatDiagnosticEvent(ref EncodingSpanWriter writer, in DiagnosticEvent @event)
        {
            FormatDiagnosticLevel(ref writer, @event.Diagnostic.Level);
            writer.Write(" ");
            FormatDiagnosticCode(ref writer, @event.Diagnostic.Id);
            writer.Write(": ");
            FormatDiagnosticMessage(ref writer, @event.Diagnostic.Message, @event.Args);
            writer.WriteLine();
        }

        /// <inheritdoc />
        protected override void WriteImpl(in DiagnosticEvent @event, IDiagnosticChannel channel)
        {
            var wrt = channel.Writer;
            var enc = channel.Encoding ?? Encoding.Default;

            // stage to a string so we can write it in one go
            var buf = MemoryPool<byte>.Shared.Rent(4096);
            var utf = new EncodingSpanWriter(enc, buf.Memory.Span);

            try
            {
                FormatDiagnosticEvent(ref utf, @event);
            }
            catch (ArgumentOutOfRangeException)
            {
                // ignore message
            }

            channel.Writer.Write(buf, utf.BytesWritten);
            return;
        }

    }

}
