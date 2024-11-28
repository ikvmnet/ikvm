using System;
using System.Text;

namespace IKVM.CoreLib.Text
{

    static class StringBuilderExtensions
    {

#if NETFRAMEWORK

        /// <summary>
        /// Appends the string representation of a specified read-only character span to this instance.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public unsafe static StringBuilder Append(this StringBuilder self, ReadOnlySpan<char> span)
        {
            fixed (char* ptr = &span.GetPinnableReference())
                return self.Append(ptr, span.Length);
        }

#endif

    }

}

