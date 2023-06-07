using System;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes a Java Class file format version number, consisting of a major and minor pair.
    /// </summary>
    /// <param name="Major"></param>
    /// <param name="Minor"></param>
    public record struct ClassFormatVersion(int Major, int Minor) : IComparable<ClassFormatVersion>
    {

        public static implicit operator ClassFormatVersion(int major)
        {
            return new ClassFormatVersion(major, 0);
        }

        public static implicit operator string(ClassFormatVersion version)
        {
            return version.ToString();
        }

        public static bool operator >(ClassFormatVersion a, ClassFormatVersion b)
        {
            return a.CompareTo(b) is 1;
        }

        public static bool operator <(ClassFormatVersion a, ClassFormatVersion b)
        {
            return a.CompareTo(b) is -1;
        }

        public static bool operator >=(ClassFormatVersion a, ClassFormatVersion b)
        {
            return a.CompareTo(b) is 0 or 1;
        }

        public static bool operator <=(ClassFormatVersion a, ClassFormatVersion b)
        {
            return a.CompareTo(b) is 0 or -1;
        }

        public int CompareTo(ClassFormatVersion other)
        {
            if (Major < other.Major)
                return -1;
            else if (Major > other.Major)
                return 1;

            if (Minor < other.Minor)
                return -1;
            else if (Minor > other.Minor)
                return 1;

            return 0;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}";
        }

    }

}
