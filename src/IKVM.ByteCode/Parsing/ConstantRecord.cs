using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    public abstract record ConstantRecord
    {

        /// <summary>
        /// Attempts to read the constant at the current position.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static bool TryReadConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryRead(out byte tag) == false)
                return false;

            switch ((ConstantTag)tag)
            {
                case ConstantTag.Utf8:
                    return Utf8ConstantRecord.TryReadUtf8Constant(ref reader, out constant);
                case ConstantTag.Integer:
                    return IntegerConstantRecord.TryReadIntegerConstant(ref reader, out constant);
                case ConstantTag.Float:
                    return FloatConstantRecord.TryReadFloatConstant(ref reader, out constant);
                case ConstantTag.Long:
                    return LongConstantRecord.TryReadLongConstant(ref reader, out constant);
                case ConstantTag.Double:
                    return DoubleConstantRecord.TryReadDoubleConstant(ref reader, out constant);
                case ConstantTag.Class:
                    return ClassConstantRecord.TryReadClassConstant(ref reader, out constant);
                case ConstantTag.String:
                    return StringConstantRecord.TryReadStringConstant(ref reader, out constant);
                case ConstantTag.Fieldref:
                    return FieldrefConstantRecord.TryReadFieldrefConstant(ref reader, out constant);
                case ConstantTag.Methodref:
                    return MethodrefConstantRecord.TryReadMethodrefConstant(ref reader, out constant);
                case ConstantTag.InterfaceMethodref:
                    return InterfaceMethodrefConstantRecord.TryReadInterfaceMethodrefConstant(ref reader, out constant);
                case ConstantTag.NameAndType:
                    return NameAndTypeConstantRecord.TryReadNameAndTypeConstant(ref reader, out constant);
                case ConstantTag.MethodHandle:
                    return MethodHandleConstantRecord.TryReadMethodHandleConstant(ref reader, out constant);
                case ConstantTag.MethodType:
                    return MethodTypeConstantRecord.TryReadMethodTypeConstant(ref reader, out constant);
                case ConstantTag.Dynamic:
                    return DynamicConstantRecord.TryReadDynamicConstant(ref reader, out constant);
                case ConstantTag.InvokeDynamic:
                    return InvokeDynamicConstantRecord.TryReadInvokeDynamicConstant(ref reader, out constant);
                case ConstantTag.Module:
                    return ModuleConstantRecord.TryReadModuleConstant(ref reader, out constant);
                case ConstantTag.Package:
                    return PackageConstantRecord.TryReadPackageConstant(ref reader, out constant);
                default:
                    throw new ByteCodeException("Encountered an unknown constant tag.");
            }
        }

    }

}
