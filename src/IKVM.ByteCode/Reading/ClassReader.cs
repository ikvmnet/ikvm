using System;
using System.Buffers;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides stateful operations for reading a class file.
    /// </summary>
    internal sealed class ClassReader : ReaderBase<ClassRecord>
    {

        const int MIN_CLASS_SIZE = 30;

        /// <summary>
        /// Attempts to read a class from the given memory position.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="length"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static unsafe bool TryRead(byte* pointer, int length, out ClassReader clazz)
        {
            using var mmap = new UnmanagedMemoryManager(pointer, length);
            return TryRead(mmap.Memory, out clazz);
        }

        /// <summary>
        /// Attempts to read a class from the given memory location.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static unsafe ClassReader Read(byte* pointer, int length)
        {
            return TryRead(pointer, length, out var clazz) ? clazz : throw new InvalidClassException("Failed to open ClassReader. Incomplete class data.");
        }

        /// <summary>
        /// Attempts to read a class from the given file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static unsafe bool TryRead(string path, out ClassReader clazz)
        {
            using var mmap = MemoryMappedFile.CreateFromFile(path, FileMode.Open, null, 0, MemoryMappedFileAccess.Read);
            using var view = mmap.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);

            try
            {
                byte* pntr = null;
                view.SafeMemoryMappedViewHandle.AcquirePointer(ref pntr);
                return TryRead(pntr, checked((int)view.SafeMemoryMappedViewHandle.ByteLength), out clazz);
            }
            finally
            {
                view.SafeMemoryMappedViewHandle.ReleasePointer();
            }
        }

        /// <summary>
        /// Attempts to read a class from the given file.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static unsafe ClassReader Read(string path)
        {
            return TryRead(path, out var clazz) ? clazz : throw new InvalidClassException("Failed to open ClassReader. Incomplete class data.");
        }

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
            return TryRead(buffer, out var clazz) ? clazz : throw new InvalidClassException("Failed to open ClassReader. Incomplete class data.");
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(in ReadOnlySequence<byte> buffer, out ClassReader clazz)
        {
            return TryRead(buffer, out clazz, out _, out _);
        }

        /// <summary>
        /// Attempts to read a class from the given buffer, returning information about the number of consumed and examined bytes.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="clazz"></param>
        /// <param name="consumed"></param>
        /// <param name="examined"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static bool TryRead(in ReadOnlySequence<byte> buffer, out ClassReader clazz, out SequencePosition consumed, out SequencePosition examined)
        {
            consumed = buffer.Start;

            var reader = new ClassFormatReader(buffer);
            if (TryRead(ref reader, out clazz) == false)
            {
                // examined up to the position of the reader, but consumed nothing
                examined = reader.Position;
                return false;
            }
            else
            {
                // examined up to the point of the reader, consumed the same
                consumed = reader.Position;
                examined = reader.Position;
                return true;
            }
        }

        /// <summary>
        /// Attempts to read a class from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static ClassReader Read(in ReadOnlySequence<byte> buffer)
        {
            return TryRead(buffer, out var clazz) ? clazz : throw new InvalidClassException("Failed to open ClassReader. Incomplete class data.");
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
        public static async ValueTask<ClassReader> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
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
        public static ClassReader Read(Stream stream, CancellationToken cancellationToken = default)
        {
            return ReadAsync(stream, cancellationToken).AsTask().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Reads the next class from the stream.
        /// </summary>
        /// <returns></returns>
        public static async ValueTask<ClassReader> ReadAsync(PipeReader reader, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var result = await reader.ReadAtLeastAsync(MIN_CLASS_SIZE, cancellationToken);
                if (result.IsCanceled)
                    throw new OperationCanceledException();

                // attempt to read at least one class
                if (TryRead(result.Buffer, out var clazz, out var consumed, out var examined) == false)
                {
                    reader.AdvanceTo(consumed, examined);

                    // we couldn't read a full class, and the pipe is at the end
                    if (result.IsCompleted)
                        throw new InvalidClassException("End of stream reached before valid class.");

                    continue;
                }

                reader.AdvanceTo(consumed, examined);
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
