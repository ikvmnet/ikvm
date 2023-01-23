using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueConstantReader : ElementValueReader<ElementValueConstantValueRecord>
    {

        object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueConstantReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the constant value.
        /// </summary>
        public object Value => LazyGet(ref value, ResolveValue);

        /// <summary>
        /// Gets the value of the constant element.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        object ResolveValue() => Record.Tag switch
        {
            ElementValueTag.Byte => (byte)DeclaringClass.ResolveConstant<IntegerConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Char => (char)DeclaringClass.ResolveConstant<IntegerConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Double => DeclaringClass.ResolveConstant<DoubleConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Float => DeclaringClass.ResolveConstant<FloatConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Integer => DeclaringClass.ResolveConstant<IntegerConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Long => DeclaringClass.ResolveConstant<LongConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Short => (short)DeclaringClass.ResolveConstant<IntegerConstantReader>(ValueRecord.Index).Value,
            ElementValueTag.Boolean => DeclaringClass.ResolveConstant<IntegerConstantReader>(ValueRecord.Index).Value != 0,
            ElementValueTag.String => DeclaringClass.ResolveConstant<Utf8ConstantReader>(ValueRecord.Index).Value,
            _ => throw new ByteCodeException($"Unknown element value constant tag '{Record.Tag}'.")
        };

    }

}