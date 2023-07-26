namespace IKVM.Runtime.Accessors.Java.Nio.File
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal sealed class StandardWatchEventKindsAccessor : Accessor<object>
    {
        private FieldAccessor<object> entryCreate;
        private FieldAccessor<object> entryDelete;
        private FieldAccessor<object> entryModify;
        private FieldAccessor<object> overflow;

        public object ENTRY_CREATE => GetField(ref entryCreate, nameof(ENTRY_CREATE)).GetValue();

        public object ENTRY_DELETE => GetField(ref entryDelete, nameof(ENTRY_DELETE)).GetValue();

        public object ENTRY_MODIFY => GetField(ref entryModify, nameof(ENTRY_MODIFY)).GetValue();

        public object OVERFLOW => GetField(ref overflow, nameof(OVERFLOW)).GetValue();

        public StandardWatchEventKindsAccessor(AccessorTypeResolver resolver)
            : base(resolver, "java.nio.file.StandardWatchEventKinds")
        {
        }
    }

#endif

}