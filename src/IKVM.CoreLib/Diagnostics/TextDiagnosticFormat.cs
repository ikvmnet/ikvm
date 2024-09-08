using System;
using System.Buffers;
using System.IO;
using System.Text;

using IKVM.CoreLib.Buffers;
using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Handles writing diagnostic events to text output.
    /// </summary>
    static class TextDiagnosticFormat
    {

        /// <summary>
        /// Writes the given event data to the specified <see cref="StringWriter"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <param name="writer"></param>
        public static void Write(int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location, StringWriter writer)
        {
            var buffer = MemoryPool<byte>.Shared.Rent(8192);

            try
            {
                var wrt = new MemoryBufferWriter<byte>(buffer.Memory);
                Write(id, level, message, args, exception, location, ref wrt, writer.Encoding);
                writer.Write(writer.Encoding.GetString(buffer.Memory.Span[..wrt.WrittenCount]));
            }
            finally
            {
                buffer.Dispose();
            }
        }

        /// <summary>
        /// Writes the given event data to the specified <see cref="IBufferWriter{byte}"/>.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <param name="writer"></param>
        /// <param name="encoding"></param>
        public static void Write<TWriter>(int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location, ref TWriter writer, Encoding? encoding = null)
            where TWriter : IBufferWriter<byte>
        {
            var encoder = new EncodingByteBufferWriter<TWriter>(encoding ?? Encoding.UTF8, ref writer);
            WriteDiagnosticEvent(ref encoder, id, level, message, args, exception, location);
        }

        /// <summary>
        /// Formats the <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        static void WriteDiagnosticEvent<TWriter>(ref EncodingByteBufferWriter<TWriter> writer, int id, DiagnosticLevel level, string message, object?[] args, Exception? exception, DiagnosticLocation location)
            where TWriter : IBufferWriter<byte>
        {
            WriteDiagnosticLocation(ref writer, location);
            WriteDiagnosticLevel(ref writer, level);
            writer.Write(" ");
            WriteDiagnosticCode(ref writer, id);
            writer.Write(": ");
            WriteDiagnosticMessage(ref writer, message, args);
            writer.WriteLine();
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="location"></param>
        static void WriteDiagnosticLocation<TWriter>(ref EncodingByteBufferWriter<TWriter> writer, in DiagnosticLocation location)
            where TWriter : IBufferWriter<byte>
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
        static void WriteDiagnosticLevel<TWriter>(ref EncodingByteBufferWriter<TWriter> writer, DiagnosticLevel level)
            where TWriter : IBufferWriter<byte>
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
        static void WriteDiagnosticCode<TWriter>(ref EncodingByteBufferWriter<TWriter> writer, int code)
            where TWriter : IBufferWriter<byte>
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
        static void WriteDiagnosticMessage<TWriter>(ref EncodingByteBufferWriter<TWriter> writer, string message, object?[] args)
            where TWriter : IBufferWriter<byte>
        {
            writer.Write(string.Format(null, message, args));
        }

    }

}
