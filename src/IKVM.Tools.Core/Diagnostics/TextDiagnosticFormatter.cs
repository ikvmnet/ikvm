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
        protected virtual void WriteDiagnosticLevel(ref EncodingSpanWriter writer, DiagnosticLevel level)
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
        protected virtual void WriteDiagnosticCode(ref EncodingSpanWriter writer, int code)
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
        protected virtual void WriteDiagnosticMessage(ref EncodingSpanWriter writer, CompositeFormat message, ReadOnlySpan<object?> args)
#else
        protected virtual void WriteDiagnosticMessage(ref EncodingSpanWriter writer, string message, ReadOnlySpan<object?> args)
#endif
        {
#if NET8_0_OR_GREATER
            writer.Write(string.Format(null, message, args));
#else
            writer.Write(string.Format(null, message, args.ToArray()));
#endif
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="location"></param>
        protected virtual void WriteDiagnosticLocation(ref EncodingSpanWriter writer, in DiagnosticLocation location)
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
        /// Formats the <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        protected virtual void WriteDiagnosticEvent(ref EncodingSpanWriter writer, in DiagnosticEvent @event)
        {
            WriteDiagnosticLocation(ref writer, @event.Location);
            WriteDiagnosticLevel(ref writer, @event.Diagnostic.Level);
            writer.Write(" ");
            WriteDiagnosticCode(ref writer, @event.Diagnostic.Id);
            writer.Write(": ");
            WriteDiagnosticMessage(ref writer, @event.Diagnostic.Message, @event.Args);
            writer.WriteLine();
        }

        /// <inheritdoc />
        protected override void WriteImpl(in DiagnosticEvent @event, IDiagnosticChannel channel)
        {
            // stage to a string so we can write it in one go
            var buf = MemoryPool<byte>.Shared.Rent(4096);
            var utf = new EncodingSpanWriter(channel.Encoding ?? Encoding.Default, buf.Memory.Span);

            try
            {
                WriteDiagnosticEvent(ref utf, @event);
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
