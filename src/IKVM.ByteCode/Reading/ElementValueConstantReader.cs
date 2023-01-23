using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueConstantReader : ElementValueReader<ElementValueConstantValueRecord>
    {

        IConstantReader value;

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
        /// Gets the type of the element value.
        /// </summary>
        public ElementValueTag Tag => Record.Tag;

        /// <summary>
        /// Gets the constant value.
        /// </summary>
        public IConstantReader Value => LazyGet(ref value, ResolveValue);

        /// <summary>
        /// Gets the value of the constant element.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        IConstantReader ResolveValue() => Record.Tag switch
        {
            ElementValueTag.Byte => DeclaringClass.Constants.Get<IntegerConstantReader>(ValueRecord.Index),
            ElementValueTag.Char => DeclaringClass.Constants.Get<IntegerConstantReader>(ValueRecord.Index),
            ElementValueTag.Double => DeclaringClass.Constants.Get<DoubleConstantReader>(ValueRecord.Index),
            ElementValueTag.Float => DeclaringClass.Constants.Get<FloatConstantReader>(ValueRecord.Index),
            ElementValueTag.Integer => DeclaringClass.Constants.Get<IntegerConstantReader>(ValueRecord.Index),
            ElementValueTag.Long => DeclaringClass.Constants.Get<LongConstantReader>(ValueRecord.Index),
            ElementValueTag.Short => DeclaringClass.Constants.Get<IntegerConstantReader>(ValueRecord.Index),
            ElementValueTag.Boolean => DeclaringClass.Constants.Get<IntegerConstantReader>(ValueRecord.Index),
            ElementValueTag.String => DeclaringClass.Constants.Get<Utf8ConstantReader>(ValueRecord.Index),
            _ => throw new ByteCodeException($"Unknown element value constant tag '{Record.Tag}'.")
        };

    }

}