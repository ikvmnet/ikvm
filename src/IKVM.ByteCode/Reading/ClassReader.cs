using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides stateful operations for reading a class file.
    /// </summary>
    public class ClassReader : ReaderBase<ClassRecord>
    {

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(ReadOnlyMemory<byte> buffer, out ClassReader clazz)
        {
            return TryRead(new ReadOnlySequence<byte>(buffer), out clazz);
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static ClassReader Read(ReadOnlyMemory<byte> buffer)
        {
            return TryRead(buffer, out var clazz) ? clazz : throw new ByteCodeException("Failed to open ClassReader. Incomplete class data.");
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(in ReadOnlySequence<byte> buffer, out ClassReader clazz)
        {
            var reader = new ClassFormatReader(buffer);
            return TryRead(ref reader, out clazz);
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static ClassReader Read(in ReadOnlySequence<byte> buffer)
        {
            var reader = new ClassFormatReader(buffer);
            return TryRead(ref reader, out var clazz) ? clazz : throw new ByteCodeException("Failed to open ClassReader. Incomplete class data.");
        }

        /// <summary>
        /// Attempts to read a class from the given reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static bool TryRead(ref ClassFormatReader reader, out ClassReader clazz)
        {
            clazz = null;

            if (ClassRecord.TryRead(ref reader, out var record) == false)
                return false;

            clazz = new ClassReader(record);
            return true;
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public static async Task<ClassReader> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            var reader = PipeReader.Create(stream, new StreamPipeReaderOptions(minimumReadSize: 1, leaveOpen: true));

            try
            {
                return await ReadAsync(reader, cancellationToken);
            }
            catch (Exception e)
            {
                await reader.CompleteAsync(e);
                throw;
            }
            finally
            {
                await reader.CompleteAsync();
            }
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public static async Task<ClassReader> ReadAsync(PipeReader reader, CancellationToken cancellationToken = default)
        {
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
                return clazz;
            }
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public static ClassReader Read(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            var reader = PipeReader.Create(stream, new StreamPipeReaderOptions(minimumReadSize: 1, leaveOpen: true));

            try
            {
                return Read(reader);
            }
            catch (Exception e)
            {
                reader.Complete(e);
                throw;
            }
            finally
            {
                reader.Complete();
            }
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public static ClassReader Read(PipeReader reader)
        {
            while (true)
            {
                var result = reader.ReadAtLeastAsync(30, CancellationToken.None).GetAwaiter().GetResult();
                if (result.IsCanceled)
                    throw new OperationCanceledException();

                // attempt to read at least one class
                if (TryRead(result.Buffer, out var clazz) == false)
                {
                    reader.AdvanceTo(result.Buffer.Start, result.Buffer.End);
                    continue;
                }

                reader.AdvanceTo(result.Buffer.End);
                return clazz;
            }
        }

        internal int MAX_STACK_ALLOC = 1024;

        ClassConstantReader @this;
        ClassConstantReader super;
        ConstantReaderCollection constants;
        InterfaceReaderCollection interfaces;
        FieldReaderCollection fields;
        MethodReaderCollection methods;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="record"></param>
        internal ClassReader(ClassRecord record) :
            base(null, record)
        {

        }

        /// <summary>
        /// Gets the version of the class.
        /// </summary>
        public ClassFormatVersion Version => new ClassFormatVersion(Record.MajorVersion, Record.MinorVersion);

        /// <summary>
        /// Gets the set of constants declared by the class.
        /// </summary>
        public ConstantReaderCollection Constants => LazyGet(ref constants, () => new ConstantReaderCollection(this, Record.Constants));

        /// <summary>
        /// Gets the access flags of the class.
        /// </summary>
        public AccessFlag AccessFlags => Record.AccessFlags;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public ClassConstantReader This => LazyGet(ref @this, () => Constants.Get<ClassConstantReader>(Record.ThisClassIndex));

        /// <summary>
        /// Gets the name of the super class.
        /// </summary>
        public ClassConstantReader Super => LazyGet(ref super, () => Constants.Get<ClassConstantReader>(Record.SuperClassIndex));

        /// <summary>
        /// Gets the set of the interfaces implemented by this class.
        /// </summary>
        public InterfaceReaderCollection Interfaces => LazyGet(ref interfaces, () => new InterfaceReaderCollection(this, Record.Interfaces));

        /// <summary>
        /// Gets the set of fields declared by the class.
        /// </summary>
        public FieldReaderCollection Fields => LazyGet(ref fields, () => new FieldReaderCollection(this, Record.Fields));

        /// <summary>
        /// Gets the set of methods declared by the class.
        /// </summary>
        public MethodReaderCollection Methods => LazyGet(ref methods, () => new MethodReaderCollection(this, Record.Methods));

        /// <summary>
        /// Gets the dictionary of attributes declared by the class.
        /// </summary>
        public AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(this, Record.Attributes));

    }

}
