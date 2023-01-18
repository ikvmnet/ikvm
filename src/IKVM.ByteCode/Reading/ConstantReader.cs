using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides static methods for reading constants.
    /// </summary>
    internal static class ConstantReader
    {

        /// <summary>
        /// Initializes a <see cref="IConstantReader"/> from a <see cref="ConstantRecord"/>.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        internal static IConstantReader Read(ClassReader declaringClass, ConstantRecord record, ConstantOverride @override = null) => record switch
        {
            Utf8ConstantRecord c => new Utf8ConstantReader(declaringClass, c, (Utf8ConstantOverride)@override),
            IntegerConstantRecord c => new IntegerConstantReader(declaringClass, c, (IntegerConstantOverride)@override),
            FloatConstantRecord c => new FloatConstantReader(declaringClass, c, (FloatConstantOverride)@override),
            LongConstantRecord c => new LongConstantReader(declaringClass, c, (LongConstantOverride)@override),
            DoubleConstantRecord c => new DoubleConstantReader(declaringClass, c, (DoubleConstantOverride)@override),
            ClassConstantRecord c => new ClassConstantReader(declaringClass, c, (ClassConstantOverride)@override),
            StringConstantRecord c => new StringConstantReader(declaringClass, c, (StringConstantOverride)@override),
            FieldrefConstantRecord c => new FieldrefConstantReader(declaringClass, c, (FieldrefConstantOverride)@override),
            MethodrefConstantRecord c => new MethodrefConstantReader(declaringClass, c, (MethodrefConstantOverride)@override),
            InterfaceMethodrefConstantRecord c => new InterfaceMethodrefConstantReader(declaringClass, c, (InterfaceMethodrefConstantOverride)@override),
            NameAndTypeConstantRecord c => new NameAndTypeConstantReader(declaringClass, c, (NameAndTypeConstantOverride)@override),
            MethodHandleConstantRecord c => new MethodHandleConstantReader(declaringClass, c, (MethodHandleConstantOverride)@override),
            MethodTypeConstantRecord c => new MethodTypeConstantReader(declaringClass, c, (MethodTypeConstantOverride)@override),
            DynamicConstantRecord c => new DynamicConstantReader(declaringClass, c, (DynamicConstantOverride)@override),
            InvokeDynamicConstantRecord c => new InvokeDynamicConstantReader(declaringClass, c, (InvokeDynamicConstantOverride)@override),
            ModuleConstantRecord c => new ModuleConstantReader(declaringClass, c, (ModuleConstantOverride)@override),
            PackageConstantRecord c => new PackageConstantReader(declaringClass, c, (PackageConstantOverride)@override),
            _ => throw new ByteCodeException("Invalid constant type."),
        };

    }

    /// <summary>
    /// Base type for constant readers.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    internal abstract class ConstantReader<TRecord> : ReaderBase<TRecord>, IConstantReader<TRecord>
        where TRecord : ConstantRecord
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        protected ConstantReader(ClassReader declaringClass, TRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Returns <c>true</c> if the constant is considered loadable according to the JVM specification.
        /// </summary>
        public virtual bool IsLoadable => false;

    }

    /// <summary>
    /// Base type for constant readers.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    /// <typeparam name="TOverride"></typeparam>
    internal abstract class ConstantReader<TRecord, TOverride> : ConstantReader<TRecord>, IConstantReader<TRecord, TOverride>
        where TRecord : ConstantRecord
        where TOverride : ConstantOverride
    {

        readonly TOverride @override;

        /// <summary>
        /// Initializes a new insance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ConstantReader(ClassReader declaringClass, TRecord record, TOverride pverride) :
            base(declaringClass, record)
        {
            this.@override = pverride;
        }

        /// <summary>
        /// Gets the underlying constant being read.
        /// </summary>
        TRecord IConstantReader<TRecord>.Record => Record;

        /// <summary>
        /// Gets the applied override.
        /// </summary>
        public TOverride Override => @override;

        /// <summary>
        /// Gets the override applied to this constant.
        /// </summary>
        public virtual object OverrideValue => @override != null ? @override.Value : null;

    }

}
