using System;
using System.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Provides extension methods for <see cref="BlobReader"/>.
    /// </summary>
    static class BlobReaderExtensions
    {

        /// <summary>
        /// Reads the next byte without advancing the reader.
        /// </summary>
        /// <param name="reader">The reader to peek into.</param>
        /// <returns>The byte at the current position of the reader.</returns>
        /// <exception cref="BadImageFormatException">The reader has no bytes remaining.</exception>
        internal static unsafe byte PeekByte(this ref BlobReader reader)
        {
            if (reader.RemainingBytes == 0)
                throw new BadImageFormatException();

            return *reader.CurrentPointer;
        }

    }

}
