namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ClassLoader+NativeLibrary' type.
    /// </summary>
    internal sealed class ClassLoaderNativeLibraryAccessor : Accessor<object>
    {

        FieldAccessor<object, string> name;
        FieldAccessor<object, long> handle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ClassLoaderNativeLibraryAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ClassLoader+NativeLibrary")
        {

        }

        /// <summary>
        /// Gets the value for the 'name' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public string GetName(object self) => GetField(ref name, nameof(name)).GetValue(self);

        /// <summary>
        /// Gets the value for the 'handle' field.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public long GetHandle(object self) => GetField(ref handle, nameof(handle)).GetValue(self);

    }

#endif

}
