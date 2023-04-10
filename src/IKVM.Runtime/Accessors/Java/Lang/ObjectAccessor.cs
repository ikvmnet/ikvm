namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.Object' type.
    /// </summary>
    internal sealed class ObjectAccessor : Accessor<object>
    {

        FieldAccessor<object, bool> __init;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ObjectAccessor(AccessorTypeResolver resolver) :
            base(resolver("java.lang.Object"))
        {

        }

        /// <summary>
        /// Gets the value for the '__<init>' field.
        /// </summary>
        public bool GetInit(object self) => GetField(ref __init, "__<init>").GetValue(self);

    }

#endif

}
