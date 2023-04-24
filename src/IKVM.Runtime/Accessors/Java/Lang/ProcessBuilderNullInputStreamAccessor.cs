namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ProcessBuilder+NullInputStream' type.
    /// </summary>
    internal sealed class ProcessBuilderNullInputStreamAccessor : Accessor<object>
    {

        FieldAccessor<object> INSTANCE;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ProcessBuilderNullInputStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ProcessBuilder+NullInputStream")
        {

        }

        /// <summary>
        /// Gets the value of the INSTANCE field.
        /// </summary>
        /// <returns></returns>
        public object GetInstance() => GetField(ref INSTANCE, nameof(INSTANCE)).GetValue();

    }

#endif

}
