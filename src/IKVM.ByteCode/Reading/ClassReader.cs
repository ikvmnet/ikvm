using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides stateful operations for reading a class file.
    /// </summary>
    public sealed class ClassReader
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
        /// <param name="clazz"></param>
        /// <returns></returns>
        public static bool TryRead(ReadOnlySequence<byte> buffer, out ClassReader clazz)
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
        public static bool TryRead(ref SequenceReader<byte> reader, out ClassReader clazz)
        {
            clazz = null;

            if (ClassRecord.TryReadClass(ref reader, out var record) == false)
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

        /// <summary>
        /// Gets the value at the given location or initializes it if null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        internal static T LazyGet<T>(ref T location, Func<T> create)
            where T : class
        {
            if (location == null)
                Interlocked.CompareExchange(ref location, create(), null);

            return location;
        }

        internal int MAX_STACK_ALLOC = 1024;

        readonly ClassRecord record;

        string name;
        string superName;
        ConstantReader[] constants;
        InterfaceReaderCollection interfaces;
        FieldReaderCollection fields;
        MethodReaderCollection methods;
        AttributeReaderCollection attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="record"></param>
        internal ClassReader(ClassRecord record)
        {
            this.record = record;
        }

        /// <summary>
        /// Reference to the underlying record backing this class.
        /// </summary>
        internal ClassRecord Record => record;

        /// <summary>
        /// Gets the minor version of the class.
        /// </summary>
        public ushort MinorVersion => record.MinorVersion;

        /// <summary>
        /// Gets the major version of the class.
        /// </summary>
        public ushort MajorVersion => record.MajorVersion;

        /// <summary>
        /// Gets the access flags of the class.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => LazyGet(ref name, () => ResolveConstant<ClassConstantReader>(record.ThisClassIndex).Name.Value);

        /// <summary>
        /// Gets the name of the super class.
        /// </summary>
        public string SuperName => LazyGet(ref superName, () => ResolveConstant<ClassConstantReader>(record.SuperClassIndex).Name.Value);

        /// <summary>
        /// Gets the name of the interfaces implemented by this class.
        /// </summary>
        public InterfaceReaderCollection Interfaces => LazyGet(ref interfaces, () => new InterfaceReaderCollection(this, record.Interfaces));

        /// <summary>
        /// Gets the set of fields declared by the class.
        /// </summary>
        public FieldReaderCollection Fields => LazyGet(ref fields, () => new FieldReaderCollection(this, record.Fields));

        /// <summary>
        /// Gets the set of methods declared by the class.
        /// </summary>
        public MethodReaderCollection Methods => LazyGet(ref methods, () => new MethodReaderCollection(this, record.Methods));

        /// <summary>
        /// Gets the dictionary of attributes declared by the class.
        /// </summary>
        public AttributeReaderCollection Attributes => LazyGet(ref attributes, () => new AttributeReaderCollection(this, record.Attributes));

        /// <summary>
        /// Resolves the constant of the specified value.
        /// </summary>
        /// <typeparam name="TConstant"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        internal TConstant ResolveConstant<TConstant>(ushort index)
            where TConstant : ConstantReader
        {
            return (TConstant)ResolveConstant(index);
        }

        /// <summary>
        /// Resolves the constant of the specified value.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal ConstantReader ResolveConstant(ushort index)
        {
            if (index < 1 || index >= record.Constants.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            // initialize cache if not initialized
            if (constants == null)
                Interlocked.CompareExchange(ref constants, new ConstantReader[record.Constants.Length], null);

            // cache already constants constant
            if (constants[index] is ConstantReader constant)
                return constant;

            // generate new constant
            constant = ConstantReader.Read(this, record.Constants[index]);

            // atomic set, only one winner
            Interlocked.CompareExchange(ref constants[index], constant, null);
            return constants[index];
        }

    }

}
