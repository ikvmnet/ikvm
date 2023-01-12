/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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

using IKVM.Internal;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.util.zip
{

    static class ClassStubZipEntry
    {

#if !FIRST_PASS

        /// <summary>
        /// .NET stream that wraps a Java ZipEntry.
        /// </summary>
        sealed class ZipEntryStream : Stream
        {

            readonly global::java.util.zip.ZipFile zipFile;
            readonly global::java.util.zip.ZipEntry entry;
            global::java.io.InputStream inp;
            long position;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="zipFile"></param>
            /// <param name="entry"></param>
            public ZipEntryStream(global::java.util.zip.ZipFile zipFile, global::java.util.zip.ZipEntry entry)
            {
                this.zipFile = zipFile;
                this.entry = entry;
                inp = zipFile.getInputStream(entry);
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return true; }
            }

            public override long Length
            {
                get { return entry.getSize(); }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                // For compatibility with real file i/o, we try to read the requested number
                // of bytes, instead of returning earlier if the underlying InputStream does so.
                var totalRead = 0;
                while (count > 0)
                {
                    var read = inp.read(buffer, offset, count);
                    if (read <= 0)
                        break;

                    offset += read;
                    count -= read;
                    totalRead += read;
                    position += read;
                }

                return totalRead;
            }

            public override long Position
            {
                get
                {
                    return position;
                }
                set
                {
                    if (value < position)
                    {
                        if (value < 0)
                            throw new System.IO.IOException("Negative seek offset");

                        position = 0;
                        inp.close();
                        inp = zipFile.getInputStream(entry);
                    }

                    var skip = value - position;
                    while (skip > 0)
                    {
                        var skipped = inp.skip(skip);
                        if (skipped == 0)
                        {
                            if (position != entry.getSize())
                                throw new System.IO.IOException("skip failed");

                            // we're actually at EOF in the InputStream, but we set the virtual position beyond EOF
                            position += skip;
                            break;
                        }
                        position += skipped;
                        skip -= skipped;
                    }
                }
            }

            public override void Flush()
            {

            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        Position = offset;
                        break;
                    case SeekOrigin.Current:
                        Position += offset;
                        break;
                    case SeekOrigin.End:
                        Position = entry.getSize() + offset;
                        break;
                }

                return position;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Close()
            {
                base.Close();
                inp.close();
            }

        }

#endif

        /// <summary>
        /// Implements the native method for 'expandIkvmClasses'.
        /// </summary>
        /// <param name="_zipFile"></param>
        /// <param name="_entries"></param>
        public static void expandIkvmClasses(object _zipFile, object _entries)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var zipFile = (global::java.util.zip.ZipFile)_zipFile;
            var entries = (global::java.util.LinkedHashMap)_entries;

            try
            {
                var path = zipFile.getName();
                if (VfsTable.Default.IsPath(path))
                {
                    var entry = (global::java.util.zip.ZipEntry)entries.get(JVM.JarClassList);
                    if (entry != null)
                    {
                        using var stream = new ZipEntryStream(zipFile, entry);
                        entries.remove(entry.name);

                        var br = new BinaryReader(stream);
                        var count = br.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            var classEntry = new global::java.util.zip.ClassStubZipEntry(path, br.ReadString());
                            classEntry.setMethod(global::java.util.zip.ClassStubZipEntry.STORED);
                            classEntry.setTime(entry.getTime());
                            entries.put(classEntry.name, classEntry);
                        }
                    }
                }
            }
            catch (global::java.io.IOException)
            {

            }
            catch (IOException)
            {

            }
#endif
        }

    }

}
