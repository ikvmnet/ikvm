namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.UNIXProcess' type.
    /// </summary>
    internal sealed class UNIXProcessAccessor : Accessor<object>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public UNIXProcessAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.UNIXProcess")
        {

        }

    }

#endif

}
