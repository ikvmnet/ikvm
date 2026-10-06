using System;
using System.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    static class BlobReaderExtensions
    {

        /// <summary>
        /// Reads the next byte without advancing the reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="BadImageFormatException"></exception>
        internal static unsafe byte PeekByte(this ref BlobReader reader)
        {
            if (reader.RemainingBytes == 0)
                throw new BadImageFormatException();

            return *reader.CurrentPointer;
        }

    }

}
