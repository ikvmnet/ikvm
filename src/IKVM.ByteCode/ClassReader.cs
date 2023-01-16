using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Provides methods to read class data.
    /// </summary>
    public class ClassReader : IDisposable
    {

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(ReadOnlyMemory<byte> buffer, out Class clazz)
        {
            return TryRead(new ReadOnlySequence<byte>(buffer), out clazz);
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(ReadOnlySequence<byte> buffer, out Class clazz)
        {
            var reader = new SequenceReader<byte>(buffer);
            return TryRead(ref reader, out clazz);
        }

        /// <summary>
        /// Attempts to read a class from the given reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(ref SequenceReader<byte> reader, out Class clazz)
        {
            clazz = null;

            if (ClassRecordReader.TryReadClass(ref reader, out var record) == false)
                return false;

            clazz= new Class(record);
            return true;
        }

        readonly Stream stream;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClassReader(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public async Task<Class> ReadAsync(CancellationToken cancellationToken = default)
        {
            var reader = PipeReader.Create(stream, new StreamPipeReaderOptions(minimumReadSize: 1));

            while (true)
            {
                var result = await reader.ReadAtLeastAsync(30, cancellationToken);
                if (result.IsCanceled)
                    throw new OperationCanceledException();

                // attempt to read at least one class
                if (TryRead(result.Buffer, out var clazz) == false)
                {
                    reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                    continue;
                }

                reader.AdvanceTo(result.Buffer.End);
                await reader.CompleteAsync();
                return clazz;
            }
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public Class Read()
        {
            var reader = PipeReader.Create(stream, new StreamPipeReaderOptions(minimumReadSize: 1));

            while (true)
            {
                // attempt to read, continue if no data
                if (reader.TryRead(out var result) == false)
                    continue;
                if (result.IsCanceled)
                    throw new OperationCanceledException();

                // attempt to read at least one class
                if (TryRead(result.Buffer, out var clazz) == false)
                {
                    reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                    continue;
                }

                reader.AdvanceTo(result.Buffer.End);
                reader.Complete();
                return clazz;
            }
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
        }

    }

}
