namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ProcessBuilder' type.
    /// </summary>
    internal sealed class ProcessBuilderAccessor : Accessor<object>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ProcessBuilderAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ProcessBuilder")
        {

        }

    }

#endif

}
