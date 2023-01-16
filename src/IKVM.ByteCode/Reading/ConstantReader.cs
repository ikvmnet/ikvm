using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public abstract class ConstantReader
    {

        /// <summary>
        /// Initializes a <see cref="ConstantReader"/> from a <see cref="ConstantRecord"/>.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        internal static ConstantReader Read(ClassReader declaringClass, ConstantRecord record) => record switch
        {
            Utf8ConstantRecord c => new Utf8ConstantReader(declaringClass, c),
            IntegerConstantRecord c => new IntegerConstantReader(declaringClass, c),
            FloatConstantRecord c => new FloatConstantReader(declaringClass, c),
            LongConstantRecord c => new LongConstantReader(declaringClass, c),
            DoubleConstantRecord c => new DoubleConstantReader(declaringClass, c),
            ClassConstantRecord c => new ClassConstantReader(declaringClass, c),
            StringConstantRecord c => new StringConstantReader(declaringClass, c),
            FieldrefConstantRecord c => new FieldrefConstantReader(declaringClass, c),
            MethodrefConstantRecord c => new MethodrefConstantReader(declaringClass, c),
            InterfaceMethodrefConstantRecord c => new InterfaceMethodrefConstantReader(declaringClass, c),
            NameAndTypeConstantRecord c => new NameAndTypeConstantReader(declaringClass, c),
            MethodHandleConstantRecord c => new MethodHandleConstantReader(declaringClass, c),
            MethodTypeConstantRecord c => new MethodTypeConstantReader(declaringClass, c),
            DynamicConstantRecord c => new DynamicConstantReader(declaringClass, c),
            InvokeDynamicConstantRecord c => new InvokeDynamicConstantReader(declaringClass, c),
            ModuleConstantRecord c => new ModuleConstantReader(declaringClass, c),
            PackageConstantRecord c => new PackageConstantReader(declaringClass, c),
            _ => throw new ByteCodeException("Invalid constant type."),
        };

        readonly ClassReader owner;
        readonly ConstantRecord record;

        /// <summary>
        /// Initializes a new insance.
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ConstantReader(ClassReader owner, ConstantRecord record)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the class of which this constant is a member.
        /// </summary>
        protected ClassReader DeclaringClass => owner;

        /// <summary>
        /// Gets the underlying record of the constant.
        /// </summary>
        protected ConstantRecord Record => record;

    }

    public abstract class Constant<TRecord> : ConstantReader
        where TRecord : ConstantRecord
    {

        readonly TRecord record;

        /// <summary>
        /// Initializes a new insance.
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Constant(ClassReader owner, TRecord record) :
            base(owner, record)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the underlying record of the constant.
        /// </summary>
        protected new TRecord Record => record;

    }

}