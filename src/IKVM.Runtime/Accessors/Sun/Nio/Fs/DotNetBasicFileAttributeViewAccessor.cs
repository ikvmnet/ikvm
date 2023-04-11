namespace IKVM.Runtime.Accessors.Sun.Nio.Ch
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'sun.nio.fs.DotNetBasicFileAttributeView' type.
    /// </summary>
    internal sealed class DotNetBasicFileAttributeViewAccessor : Accessor<object>
    {

        FieldAccessor<object, string> path; 

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public DotNetBasicFileAttributeViewAccessor(AccessorTypeResolver resolver) :
            base(resolver, "sun.nio.fs.DotNetBasicFileAttributeView")
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public string GetPath(object self) => GetField(ref path, nameof(path)).GetValue(self);

    }

#endif

}
