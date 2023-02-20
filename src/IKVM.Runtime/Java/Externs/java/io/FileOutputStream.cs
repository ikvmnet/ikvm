using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.io
{

    static class FileOutputStream
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileOutputStreamAccessor fileOutputStreamAccessor;
        static FileOutputStreamAccessor FileOutputStreamAccessor => JVM.BaseAccessors.Get(ref fileOutputStreamAccessor);

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="append"></param>
        public static void open0(object self, string name, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                var fileMode = append ? FileMode.Append : FileMode.Create;
                var fileAccess = FileAccess.Write;

                if (VfsTable.Default.IsPath(name))
                {
                    FileDescriptorAccessor.SetStream(fd, VfsTable.Default.Open(name, fileMode, fileAccess));
                    return;
                }

                if (append)
                {
#if NETFRAMEWORK
                    new FileStream(name, fileMode, FileSystemRights.AppendData, FileShare.ReadWrite, 1, FileOptions.None);
#else
                    // the above constructor does not exist in .NET Core
                    new FileStream(name, fileMode, fileAccess, FileShare.ReadWrite, 1, false);
#endif
                }

                FileDescriptorAccessor.SetStream(fd, new FileStream(name, fileMode, fileAccess, FileShare.ReadWrite, 1, false));
            }
            catch (ObjectDisposedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (ArgumentException e)
            {
                throw new global::java.io.FileNotFoundException(e.Message);
            }
            catch (SecurityException e)
            {
                throw new global::java.lang.SecurityException(e.Message);
            }
            catch (UnauthorizedAccessException)
            {
                throw new global::java.io.FileNotFoundException(name + " (Access is denied)");
            }
            catch (IOException e)
            {
                throw new global::java.io.FileNotFoundException(e.Message);
            }
            catch (NotSupportedException e)
            {
                throw new global::java.io.FileNotFoundException(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="append"></param>
        public static void write(object self, int b, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanWrite == false)
                throw new global::java.io.IOException("Write failed.");

            try
            {
                stream.WriteByte((byte)b);
                stream.Flush();
            }
            catch (ObjectDisposedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (NotSupportedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (IOException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'writeBytes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        public static void writeBytes(object self, byte[] b, int off, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if ((off < 0) || (off > b.Length) || (len < 0) || (len > (b.Length - off)))
                throw new global::java.lang.IndexOutOfBoundsException();

            var fd = FileOutputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanWrite == false)
                throw new global::java.io.IOException("Write failed.");

            try
            {
                stream.Write(b, off, len);
                stream.Flush();
            }
            catch (ObjectDisposedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (NotSupportedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (IOException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'close0'.
        /// </summary>
        /// <param name="self"></param>
        public static void close0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                return;

            try
            {
                FileDescriptorAccessor.SetStream(fd, null);
                stream.Close();
            }
            catch (ObjectDisposedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (NotSupportedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (IOException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'initIDs'.
        /// </summary>
        public static void initIDs()
        {

        }

    }

}