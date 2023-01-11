using System;

namespace IKVM.Runtime.Extensions
{

    /// <summary>
    /// Provides extension methods for working with <see cref="ReadOnlySpan{Char}"/>.
    /// </summary>
    public static class ReadOnlySpanCharExtensions
    {

        /// <summary>
        /// Returns a hashcode for this string.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static int GetHashCodeExtension(this ReadOnlySpan<char> span)
        {
#if NETFRAMEWORK
            return span.ToString().GetHashCode();
#else
            return string.GetHashCode(span);
#endif
        }

        /// <summary>
        /// Returns <c>true</c> if every member is a number.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static bool OnlyNumbers(this ReadOnlySpan<char> span)
        {
            foreach (var c in span)
                if (char.IsNumber(c) == false)
                    return false;

            return true;
        }

    }

}
