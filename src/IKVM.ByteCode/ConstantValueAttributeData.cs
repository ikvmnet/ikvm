namespace IKVM.ByteCode
{

    public class ConstantValueAttributeData : AttributeData<ConstantValueAttributeDataRecord>
    {

        object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ConstantValueAttributeData(Class declaringClass, AttributeInfo info, ConstantValueAttributeDataRecord data) :
            base(declaringClass, info, data)
        {

        }

        public object Value => value ??= ResolveValue();

        object ResolveValue() => DeclaringClass.ResolveConstant(Data.ValueIndex) switch
        {
            LongConstant l => l.Value,
            FloatConstant f => f.Value,
            DoubleConstant d => d.Value,
            IntegerConstant i => i.Value,
            StringConstant s => s.Value,
            _ => throw new ClassReaderException("Invalid constant type for constant attribute."),
        };

    }

}
