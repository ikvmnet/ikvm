/*
  Copyright (C) 2008-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.IO;

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Read-only stream that presents a windowed view of another stream.
    /// </summary>
    sealed class SkipStream : Stream
    {

        readonly Stream stream;

        long skipOffset;
        long skipLength;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="skipOffset"></param>
        /// <param name="skipLength"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal SkipStream(Stream stream, long skipOffset, long skipLength)
        {
            if (skipOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(skipOffset));
            if (skipLength < 0)
                throw new ArgumentOutOfRangeException(nameof(skipLength));

            this.stream = stream;
            this.skipOffset = skipOffset;
            this.skipLength = skipLength;
        }

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (skipLength != 0 && skipOffset < count)
            {
                if (skipOffset != 0)
                {
                    count = (int)skipOffset;
                }
                else
                {
                    // note that we loop forever if the skipped part lies beyond EOF
                    while (skipLength != 0)
                    {
                        // use the output buffer as scratch space
                        skipLength -= stream.Read(buffer, offset, (int)Math.Min(count, skipLength));
                    }
                }
            }

            var totalBytesRead = stream.Read(buffer, offset, count);
            skipOffset -= totalBytesRead;
            return totalBytesRead;
        }

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush() => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                stream.Dispose();
        }

    }

}
