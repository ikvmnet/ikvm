using System;

namespace IKVM.Runtime.Accessors.Com.Sun.Nio.File
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    internal class SensitivityWatchEventModifierAccessor : Accessor
    {
        public SensitivityWatchEventModifierAccessor(AccessorTypeResolver resolver)
            : base(resolver, "com.sun.nio.file.SensitivityWatchEventModifier")
        {
        }

        public bool Is(object self) => Type.IsInstanceOfType(self);
    }

#endif

}
