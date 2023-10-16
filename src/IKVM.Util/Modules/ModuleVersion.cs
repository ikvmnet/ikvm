using System;
using System.Collections.Generic;

namespace IKVM.Util.Modules
{

    /// <summary>
    /// Port of java.lang.module.ModuleDescriptor+Version from JDK9 to C#.
    /// </summary>
    public class ModuleVersion : IComparable<ModuleVersion>
    {

        /// <summary>
        /// Take a numeric token starting at position i
        /// Append it to the given list
        /// Return the index of the first character not taken
        /// Requires: s.charAt(i) is (decimal) numeric
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        static int TakeNumber(ReadOnlySpan<char> s, int i, List<object> acc)
        {
            char c = s[i];
            int d = c - '0';
            int n = s.Length;
            while (++i < n)
            {
                c = s[i];
                if (c >= '0' && c <= '9')
                {
                    d = d * 10 + (c - '0');
                    continue;
                }
                break;
            }
            acc.Add(d);
            return i;
        }

        /// <summary>
        /// Take a string token starting at position i
        /// Append it to the given list
        /// Return the index of the first character not taken
        /// Requires: s.charAt(i) is not '.'
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        static int TakeString(ReadOnlySpan<char> s, int i, List<object> acc)
        {
            int b = i;
            int n = s.Length;
            while (++i < n)
            {
                char c = s[i];
                if (c != '.' && c != '-' && c != '+' && !(c >= '0' && c <= '9'))
                    continue;
                break;
            }
            acc.Add(s.Slice(b, i - b).ToString());
            return i;
        }

        readonly string version;

        readonly List<object> number;
        readonly List<object> pre;
        readonly List<object> build;

        // Syntax: tok+ ( '-' tok+)? ( '+' tok+)?
        // First token string is sequence, second is pre, third is build
        // Tokens are separated by '.' or '-', or by changes between alpha & numeric
        // Numeric tokens are compared as decimal integers
        // Non-numeric tokens are compared lexicographically
        // A version with a non-empty pre is less than a version with same seq but no pre
        // Tokens in build may contain '-' and '+'
        //
        ModuleVersion(ReadOnlySpan<char> v)
        {
            if (v == null)
                throw new ArgumentNullException(nameof(v));

            int n = v.Length;
            if (n == 0)
                throw new ArgumentException("Empty version string.", nameof(v));

            int i = 0;
            char c = v[i];
            if (!(c >= '0' && c <= '9'))
                throw new ArgumentException($"{v.ToString()}: Version string does not start with a number", nameof(v));

            var sequence = new List<object>(4);
            var pre = new List<object>(2);
            var build = new List<object>(2);

            i = TakeNumber(v, i, sequence);

            while (i < n)
            {
                c = v[i];
                if (c == '.')
                {
                    i++;
                    continue;
                }
                if (c == '-' || c == '+')
                {
                    i++;
                    break;
                }
                if (c >= '0' && c <= '9')
                    i = TakeNumber(v, i, sequence);
                else
                    i = TakeString(v, i, sequence);
            }

            if (c == '-' && i >= n)
                throw new InvalidOperationException($"{v.ToString()}: Empty pre-release");

            while (i < n)
            {
                c = v[i];
                if (c == '.' || c == '-')
                {
                    i++;
                    continue;
                }
                if (c == '+')
                {
                    i++;
                    break;
                }
                if (c >= '0' && c <= '9')
                    i = TakeNumber(v, i, pre);
                else
                    i = TakeString(v, i, pre);
            }

            if (c == '+' && i >= n)
                throw new InvalidOperationException($"{v.ToString()}: Empty pre-release");

            while (i < n)
            {
                c = v[i];
                if (c == '.' || c == '-' || c == '+')
                {
                    i++;
                    continue;
                }
                if (c >= '0' && c <= '9')
                    i = TakeNumber(v, i, build);
                else
                    i = TakeString(v, i, build);
            }

            version = v.ToString();
            this.number = sequence;
            this.pre = pre;
            this.build = build;
        }

        /**
         * Parses the given string as a version string.
         *
         * @param  v
         *         The string to parse
         *
         * @return The resulting {@code Version}
         *
         * @throws IllegalArgumentException
         *         If {@code v} is {@code null}, an empty string, or cannot be
         *         parsed as a version string
         */
        public static ModuleVersion Parse(ReadOnlySpan<char> v)
        {
            return new ModuleVersion(v);
        }

        /// <summary>
        /// Attempts to parse the version string.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool TryParse(ReadOnlySpan<char> v, out ModuleVersion version)
        {
            try
            {
                version = Parse(v);
                return true; ;
            }
            catch
            {
                version = null;
                return false;
            }
        }

        int Compare(object o1, object o2)
        {
            return ((IComparable<ModuleVersion>)o1).CompareTo((ModuleVersion)o2);
        }

        int CompareTokens(List<object> ts1, List<object> ts2)
        {
            int n = Math.Min(ts1.Count, ts2.Count);
            for (int i = 0; i < n; i++)
            {
                object o1 = ts1[i];
                object o2 = ts2[i];
                if (o1 is int && o2 is int || o1 is string && o2 is string)
                {
                    int c = Compare(o1, o2);
                    if (c == 0)
                        continue;
                    return c;
                }
                else
                {
                    // Types differ, so convert number to string form
                    int c = o1.ToString().CompareTo(o2.ToString());
                    if (c == 0)
                        continue;
                    return c;
                }
            }
            List<object> rest = ts1.Count > ts2.Count ? ts1 : ts2;
            int e = rest.Count;
            for (int i = n; i < e; i++)
            {
                object o = rest[i];
                if (o is int && (int)o == 0)
                    continue;
                return ts1.Count - ts2.Count;
            }
            return 0;
        }

        /// <summary>
        /// Compares this module version to another module version. Module
        /// versions are compared as described in the class description.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public int CompareTo(ModuleVersion that)
        {
            int c = CompareTokens(number, that.number);
            if (c != 0) return c;
            if (pre.Count == 0)
            {
                if (that.pre.Count != 0) return +1;
            }
            else
            {
                if (that.pre.Count == 0) return -1;
            }
            c = CompareTokens(pre, that.pre);
            if (c != 0) return c;
            return CompareTokens(build, that.build);
        }

        /**
         * Tests this module version for equality with the given object.
         *
         * <p> If the given object is not a {@code Version} then this method
         * returns {@code false}. Two module version are equal if their
         * corresponding components are equal. </p>
         *
         * <p> This method satisfies the general contract of the {@link
         * java.lang.Object#equals(Object) Object.equals} method. </p>
         *
         * @param   ob
         *          the object to which this object is to be compared
         *
         * @return  {@code true} if, and only if, the given object is a module
         *          reference that is equal to this module reference
         */
        public override bool Equals(object ob)
        {
            if (!(ob is ModuleVersion))
                return false;

            return CompareTo((ModuleVersion)ob) == 0;
        }

        /**
         * Computes a hash code for this module version.
         *
         * <p> The hash code is based upon the components of the version and
         * satisfies the general contract of the {@link Object#hashCode
         * Object.hashCode} method. </p>
         *
         * @return The hash-code value for this module version
         */
        public override int GetHashCode()
        {
            return version.GetHashCode();
        }

        /// <summary>
        /// Returns the string from which this version was parsed.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return version;
        }

        /// <summary>
        /// Gets the version number components. Each item can be a string or integer.
        /// </summary>
        public IReadOnlyList<object> Number => number;

        /// <summary>
        /// Gets the prerelease components. Each item can be a string or integer.
        /// </summary>
        public IReadOnlyList<object> Prerelease => pre;

        /// <summary>
        /// Gets the build components. Each item can be a string or integer.
        /// </summary>
        public IReadOnlyList<object> Build => build;

    }

}
