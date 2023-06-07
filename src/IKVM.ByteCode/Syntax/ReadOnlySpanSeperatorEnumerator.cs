using System;

namespace IKVM.ByteCode.Syntax
{

    /// <summary>
    /// Allows navigation over a Java class name.
    /// </summary>
    ref struct ReadOnlySpanSeperatorEnumerator
    {

        ReadOnlySpan<char> name;
        ReadOnlySpan<char> curr;
        readonly char separator;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="separator"></param>
        public ReadOnlySpanSeperatorEnumerator(ReadOnlySpan<char> name, char separator)
        {
            this.name = name;
            curr = default;
            this.separator = separator;
        }

        /// <summary>
        /// Gets an enumerator.
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpanSeperatorEnumerator GetEnumerator() => this;

        /// <summary>
        /// Gets the current entry.
        /// </summary>
        public ReadOnlySpan<char> Current => curr;

        /// <summary>
        /// Returns whether any more entries exist.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            var span = name;
            if (span.Length == 0)
                return false;

            var index = span.IndexOf(separator);
            if (index == -1)
            {
                name = ReadOnlySpan<char>.Empty;
                curr = span;
                return true;
            }

            curr = span.Slice(0, index);
            name = span.Slice(index + 1);
            return true;
        }

    }
}
