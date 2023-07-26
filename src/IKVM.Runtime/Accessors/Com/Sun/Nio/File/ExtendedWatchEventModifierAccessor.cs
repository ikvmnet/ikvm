namespace IKVM.Runtime.Accessors.Com.Sun.Nio.File
{
#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal sealed class ExtendedWatchEventModifierAccessor : Accessor<object>
    {
        private FieldAccessor<object> fileTree;

        public object FILE_TREE => GetField(ref fileTree, nameof(FILE_TREE)).GetValue();

        public ExtendedWatchEventModifierAccessor(AccessorTypeResolver resolver)
            : base(resolver, "com.sun.nio.file.ExtendedWatchEventModifier")
        {
        }
    }

#endif
}
