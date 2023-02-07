namespace IKVM.ByteCode.Parsing
{
    /// <summary>
    /// Base type for constant records.
    /// </summary>
    internal abstract record ConstantRecord
    {
        protected ConstantRecord() { }

        /// <summary>
        /// Attempts to read the constant at the current position. Returns the the number of index positions to skip.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static bool TryRead(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU1(out byte tag) == false)
                return false;

            return (ConstantTag)tag switch
            {
                ConstantTag.Utf8 => Utf8ConstantRecord.TryReadUtf8Constant(ref reader, out constant, out skip),
                ConstantTag.Integer => IntegerConstantRecord.TryReadIntegerConstant(ref reader, out constant, out skip),
                ConstantTag.Float => FloatConstantRecord.TryReadFloatConstant(ref reader, out constant, out skip),
                ConstantTag.Long => LongConstantRecord.TryReadLongConstant(ref reader, out constant, out skip),
                ConstantTag.Double => DoubleConstantRecord.TryReadDoubleConstant(ref reader, out constant, out skip),
                ConstantTag.Class => ClassConstantRecord.TryReadClassConstant(ref reader, out constant, out skip),
                ConstantTag.String => StringConstantRecord.TryReadStringConstant(ref reader, out constant, out skip),
                ConstantTag.Fieldref => FieldrefConstantRecord.TryReadFieldrefConstant(ref reader, out constant, out skip),
                ConstantTag.Methodref => MethodrefConstantRecord.TryReadMethodrefConstant(ref reader, out constant, out skip),
                ConstantTag.InterfaceMethodref => InterfaceMethodrefConstantRecord.TryReadInterfaceMethodrefConstant(ref reader, out constant, out skip),
                ConstantTag.NameAndType => NameAndTypeConstantRecord.TryReadNameAndTypeConstant(ref reader, out constant, out skip),
                ConstantTag.MethodHandle => MethodHandleConstantRecord.TryReadMethodHandleConstant(ref reader, out constant, out skip),
                ConstantTag.MethodType => MethodTypeConstantRecord.TryReadMethodTypeConstant(ref reader, out constant, out skip),
                ConstantTag.Dynamic => DynamicConstantRecord.TryReadDynamicConstant(ref reader, out constant, out skip),
                ConstantTag.InvokeDynamic => InvokeDynamicConstantRecord.TryReadInvokeDynamicConstant(ref reader, out constant, out skip),
                ConstantTag.Module => ModuleConstantRecord.TryReadModuleConstant(ref reader, out constant, out skip),
                ConstantTag.Package => PackageConstantRecord.TryReadPackageConstant(ref reader, out constant, out skip),
                _ => throw new ByteCodeException($"Encountered an unknown constant tag: '0x{tag:X}'."),
            };
        }

        protected abstract int GetConstantSize();

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public int GetSize() =>
            sizeof(byte) + GetConstantSize();

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            ConstantTag tag = this switch
            {
                Utf8ConstantRecord => ConstantTag.Utf8,
                IntegerConstantRecord => ConstantTag.Integer,
                FloatConstantRecord => ConstantTag.Float,
                LongConstantRecord => ConstantTag.Long,
                DoubleConstantRecord => ConstantTag.Double,
                ClassConstantRecord => ConstantTag.Class,
                StringConstantRecord => ConstantTag.String,
                FieldrefConstantRecord => ConstantTag.Fieldref,
                MethodrefConstantRecord => ConstantTag.Methodref,
                InterfaceMethodrefConstantRecord => ConstantTag.InterfaceMethodref,
                NameAndTypeConstantRecord => ConstantTag.NameAndType,
                MethodHandleConstantRecord => ConstantTag.MethodHandle,
                MethodTypeConstantRecord => ConstantTag.MethodType,
                DynamicConstantRecord => ConstantTag.Dynamic,
                InvokeDynamicConstantRecord => ConstantTag.InvokeDynamic,
                ModuleConstantRecord => ConstantTag.Module,
                PackageConstantRecord => ConstantTag.Package,
                _ => throw new ByteCodeException($"Encountered an unknown constant: '{this.GetType().FullName}'.")
            };

            if (writer.TryWriteU1((byte)tag) == false)
                return false;

            return TryWriteConstant(ref writer);
        }

        protected abstract bool TryWriteConstant(ref ClassFormatWriter writer);
    }
}
