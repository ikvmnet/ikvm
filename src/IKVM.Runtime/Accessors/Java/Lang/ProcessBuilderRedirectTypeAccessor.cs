namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ProcessBuilderRedirectType' type.
    /// </summary>
    internal sealed class ProcessBuilderRedirectTypeAccessor : Accessor<object>
    {

        FieldAccessor<int> pipe;
        FieldAccessor<int> inherit;
        FieldAccessor<int> read;
        FieldAccessor<int> write;
        FieldAccessor<int> append;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ProcessBuilderRedirectTypeAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ProcessBuilder+Redirect+Type")
        {

        }

        /// <summary>
        /// Gets the value of the PIPE field.
        /// </summary>
        /// <returns></returns>
        public object Pipe() => GetField(ref pipe, "PIPE");

        /// <summary>
        /// Gets the value of the INHERIT field.
        /// </summary>
        /// <returns></returns>
        public object Inherit() => GetField(ref inherit, "INHERIT");

        /// <summary>
        /// Gets the value of the READ field.
        /// </summary>
        /// <returns></returns>
        public object Read() => GetField(ref read, "READ");

        /// <summary>
        /// Gets the value of the WRITE field.
        /// </summary>
        /// <returns></returns>
        public object Write() => GetField(ref write, "WRITE");

        /// <summary>
        /// Gets the value of the APPEND field.
        /// </summary>
        /// <returns></returns>
        public object Append() => GetField(ref append, "APPEND");

    }

#endif

}
