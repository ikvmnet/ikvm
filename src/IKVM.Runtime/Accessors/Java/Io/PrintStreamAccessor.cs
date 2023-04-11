namespace IKVM.Runtime.Accessors.Java.Io
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.io.PrintStream' type.
    /// </summary>
    internal sealed class PrintStreamAccessor : Accessor
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public PrintStreamAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.io.PrintStream")
        {

        }

    }

#endif

}
