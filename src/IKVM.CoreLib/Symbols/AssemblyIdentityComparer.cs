using System;

namespace IKVM.CoreLib.Symbols
{

    class AssemblyIdentityComparer
    {

        public static AssemblyIdentityComparer Default { get; } = new AssemblyIdentityComparer();

        public static StringComparer SimpleNameComparer
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }

        public static StringComparer CultureComparer
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }

    }

}
