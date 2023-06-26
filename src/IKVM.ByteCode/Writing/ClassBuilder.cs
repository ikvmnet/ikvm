using IKVM.ByteCode.Parsing;
using IKVM.ByteCode.Text;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;

namespace IKVM.ByteCode.Writing
{
    internal sealed class ClassBuilder : BuilderBase<ClassRecord>
    {
        private readonly List<ConstantRecord> constants = new();
        private readonly Dictionary<ConstantRecord, ushort> constantsHashTable = new();
        private readonly List<FieldBuilder> fields = new();
        private readonly List<MethodBuilder> methods = new();
        private readonly List<InterfaceBuilder> interfaces = new();
        private readonly AccessFlag accessFlags;
        private readonly ushort thisClass;
        private readonly ushort superClass;
        private readonly ushort minorVersion;
        private readonly ushort majorVersion;

        public ClassBuilder(AccessFlag accessFlags, string name, string super, ushort minorVersion, ushort majorVersion)
            : base(null)
        {
            if (majorVersion > 63)
                throw new UnsupportedClassVersionException(new ClassFormatVersion(majorVersion, minorVersion));

            constants.Add(null);

            this.accessFlags = accessFlags;
            thisClass = AddClass(name);
            if (super != null)
            {
                superClass = AddClass(super);
            }
            this.minorVersion = minorVersion;
            this.majorVersion = majorVersion;

            Attributes = new AttributesBuilder(this);
        }

        public AttributesBuilder Attributes { get; }

        private ushort AddConstant(ConstantRecord constant)
        {
            if (!constantsHashTable.TryGetValue(constant, out ushort index))
            {
                index = (ushort)constants.Count;
                constants.Add(constant);
                if (constant is DoubleConstantRecord || constant is LongConstantRecord)
                {
                    constants.Add(null);
                }
                constantsHashTable.Add(constant, index);
            }
            return index;
        }

        public ushort AddConstant(int i)
        {
            return AddConstant(new IntegerConstantRecord(i));
        }

        public ushort AddConstant(long l)
        {
            return AddConstant(new LongConstantRecord(l));
        }

        public ushort AddConstant(float f)
        {
            return AddConstant(new FloatConstantRecord(f));
        }

        public ushort AddConstant(double d)
        {
            return AddConstant(new DoubleConstantRecord(d));
        }

        public ushort AddConstant(string s)
        {
            return AddConstant(new StringConstantRecord(AddUtf8(s)));
        }

        public ushort AddConstant(object value) =>
            value switch
            {
                byte v => AddConstant((sbyte)v),
                bool v => AddConstant(v ? 1 : 0),
                short v => AddConstant(v),
                char v => AddConstant(v),
                int v => AddConstant(v),
                long v => AddConstant(v),
                float v => AddConstant(v),
                double v => AddConstant(v),
                string v => AddConstant(v),
                _ => throw new ByteCodeException("Invalid constant type")
            };

        public MethodBuilder AddMethod(AccessFlag accessFlags, string name, string signature)
        {
            var method = new MethodBuilder(accessFlags, name, signature, this);
            methods.Add(method);
            return method;
        }

        // ============

        public ushort AddUtf8(string str)
        {
            return AddConstant(new Utf8ConstantRecord(MUTF8Encoding.GetMUTF8(majorVersion).GetBytes(str)));
        }

        public ushort AddClass(string classname)
        {
            return AddConstant(new ClassConstantRecord(AddUtf8(classname)));
        }

        public ushort AddMethodRef(string classname, string methodname, string signature)
        {
            return AddConstant(new MethodrefConstantRecord(AddClass(classname), AddNameAndType(methodname, signature)));
        }

        public ushort AddNameAndType(string name, string type)
        {
            return AddConstant(new NameAndTypeConstantRecord(AddUtf8(name), AddUtf8(type)));
        }

        public void AddInterface(string name)
        {
            interfaces.Add(new InterfaceBuilder(name, this));
        }

        public FieldBuilder AddField(AccessFlag accessFlag, string name, string descriptor)
        {
            //var field = new FieldWriter(accessFlag, AddUtf8(name), AddUtf8(signature));
            //if (constantValue != null)
            //{
            //    //field.AddAttribute(new ConstantValueAttribute(AddUtf8("ConstantValue"), constantValueIndex));
            //}
            //fields.Add(field);
            return new FieldBuilder(accessFlag, name, descriptor, this);
        }

        public override ClassRecord Build() => new
            (
                MinorVersion: minorVersion,
                MajorVersion: majorVersion,
                Constants: constants
                    .ToArray(),
                AccessFlags: accessFlags,
                ThisClassIndex: thisClass,
                SuperClassIndex: superClass,
                Interfaces: interfaces
                    .Select(x => x.Build())
                    .ToArray(),
                Fields: fields
                    .Select(x => x.Build())
                    .ToArray(),
                Methods: methods
                    .Select(x => x.Build())
                    .ToArray(),
                Attributes: Attributes
                    .Build()
            );

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            var record = Build();

            if (record.TryWrite(ref writer) == false)
                return false;

            return true;
        }

        public void Write(Span<byte> buffer)
        {
            var writer = new ClassFormatWriter(buffer);

            if (TryWrite(ref writer) == false)
                throw new ByteCodeException("Failed to write class data.");
        }

        public void Write(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            var writer = PipeWriter.Create(stream, new StreamPipeWriterOptions(leaveOpen: true));

            try
            {
                var record = Build();
                var size = record.GetSize();

                var span = writer.GetSpan(size);

                Write(span);

                writer.Advance(size);
            }
            catch (Exception e)
            {
                writer.Complete(e);
                throw;
            }
            finally
            {
                writer.Complete();
            }
        }
    }
}
