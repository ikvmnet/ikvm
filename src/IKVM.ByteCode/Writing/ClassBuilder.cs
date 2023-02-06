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

        public AttributesBuilder Attributes { get; private set; }

        public ClassBuilder WithAttributes(Action<AttributesBuilder> builderAction)
        {
            Attributes = new AttributesBuilder(DeclaringClass);
            builderAction(Attributes);
            return this;
        }

        private ushort Add(ConstantRecord constant)
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

        public ushort AddUtf8(string str)
        {
            return Add(new Utf8ConstantRecord(MUTF8Encoding.GetMUTF8(majorVersion).GetBytes(str)));
        }

        public ushort AddClass(string classname)
        {
            return Add(new ClassConstantRecord(AddUtf8(classname)));
        }

        public ushort AddMethodRef(string classname, string methodname, string signature)
        {
            return Add(new MethodrefConstantRecord(AddClass(classname), AddNameAndType(methodname, signature)));
        }

        public ushort AddNameAndType(string name, string type)
        {
            return Add(new NameAndTypeConstantRecord(AddUtf8(name), AddUtf8(type)));
        }

        public ushort AddInt(int i)
        {
            return Add(new IntegerConstantRecord(i));
        }

        public ushort AddLong(long l)
        {
            return Add(new LongConstantRecord(l));
        }

        public ushort AddFloat(float f)
        {
            return Add(new FloatConstantRecord(f));
        }

        public ushort AddDouble(double d)
        {
            return Add(new DoubleConstantRecord(d));
        }

        public ushort AddString(string s)
        {
            return Add(new StringConstantRecord(AddUtf8(s)));
        }

        public void AddInterface(string name)
        {
            interfaces.Add(new InterfaceBuilder(name, this));
        }

        public MethodBuilder AddMethod(AccessFlag accessFlags, string name, string signature)
        {
            var method = new MethodBuilder(accessFlags, name, signature, this);
            methods.Add(method);
            return method;
        }

        public FieldBuilder AddField(AccessFlag accessFlag, string name, string descriptor, object constantValue)
        {
            //var field = new FieldWriter(accessFlag, AddUtf8(name), AddUtf8(signature));
            //if (constantValue != null)
            //{
            //    ushort constantValueIndex;
            //    if (constantValue is byte)
            //    {
            //        constantValueIndex = AddInt((sbyte)(byte)constantValue);
            //    }
            //    else if (constantValue is bool)
            //    {
            //        constantValueIndex = AddInt((bool)constantValue ? 1 : 0);
            //    }
            //    else if (constantValue is short)
            //    {
            //        constantValueIndex = AddInt((short)constantValue);
            //    }
            //    else if (constantValue is char)
            //    {
            //        constantValueIndex = AddInt((char)constantValue);
            //    }
            //    else if (constantValue is int)
            //    {
            //        constantValueIndex = AddInt((int)constantValue);
            //    }
            //    else if (constantValue is long)
            //    {
            //        constantValueIndex = AddLong((long)constantValue);
            //    }
            //    else if (constantValue is float)
            //    {
            //        constantValueIndex = AddFloat((float)constantValue);
            //    }
            //    else if (constantValue is double)
            //    {
            //        constantValueIndex = AddDouble((double)constantValue);
            //    }
            //    else if (constantValue is string)
            //    {
            //        constantValueIndex = AddString((string)constantValue);
            //    }
            //    else
            //    {
            //        throw new InvalidOperationException(constantValue.GetType().FullName);
            //    }
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
                Methods: null,
                Attributes: null
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
