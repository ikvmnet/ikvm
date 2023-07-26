namespace IKVM.Runtime.Accessors.Sun.Nio.Fs
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal class DotNetWatchServiceKeyAccessor : Accessor<object>
    {
        private FieldAccessor<object, object> state;

        internal FieldAccessor<object, object> State => GetField(ref state, "state");

        public DotNetWatchServiceKeyAccessor(AccessorTypeResolver resolver)
            : base(resolver, "sun.nio.fs.DotNetWatchService+DotNetWatchKey")
        {
        }
    }

#endif

}
