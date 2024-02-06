using System;
using System.IO;
using System.Security;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Accessors.Sun.Nio.Ch;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetDosFileAttributes'.
    /// </summary>
    static class DotNetDosFileAttributes
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;
        static FileTimeAccessor fileTimeAccessor;
        static DotNetDosFileAttributesAccessor dotNetDosFileAttributesAccessor;

        static SystemAccessor SystemAccessor => JVM.Internal.BaseAccessors.Get(ref systemAccessor);

        static FileTimeAccessor FileTimeAccessor => JVM.Internal.BaseAccessors.Get(ref fileTimeAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.Internal.BaseAccessors.Get(ref securityManagerAccessor);

        static DotNetDosFileAttributesAccessor DotNetDosFileAttributesAccessor => JVM.Internal.BaseAccessors.Get(ref dotNetDosFileAttributesAccessor);

#endif

        /// <summary>
        /// Creates a <see cref="global::java.nio.file.attribute.FileTime"/> object from a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        static object ToFileTime(DateTime? dateTime)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETFRAMEWORK
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#else
            var epoch = DateTime.UnixEpoch;
#endif
            return dateTime != null ? FileTimeAccessor.InvokeFromMillis((long)((DateTime)dateTime - epoch).TotalMilliseconds) : null;
#endif
        }

        /// <summary>
        /// Implements the native method 'read'.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="global::java.nio.file.NoSuchFileException"></exception>
        /// <exception cref="global::java.io.IOException"></exception>
        public static object read(string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var sm = SystemAccessor.InvokeGetSecurityManager();
            if (sm != null)
                SecurityManagerAccessor.InvokeCheckRead(sm, path);

            if (JVM.Vfs.IsPath(path))
            {
                var entry = JVM.Vfs.GetEntry(path);

                if (entry is VfsFile vfsFile)
                    return DotNetDosFileAttributesAccessor.Init(
                        FileTimeAccessor.InvokeFromMillis(0),
                        FileTimeAccessor.InvokeFromMillis(0),
                        FileTimeAccessor.InvokeFromMillis(0),
                        null,
                        false,
                        false,
                        true,
                        false,
                        vfsFile.Size,
                        true,
                        false,
                        false,
                        false);

                if (entry is VfsDirectory vfsDirectory)
                    return DotNetDosFileAttributesAccessor.Init(
                        FileTimeAccessor.InvokeFromMillis(0),
                        FileTimeAccessor.InvokeFromMillis(0),
                        FileTimeAccessor.InvokeFromMillis(0),
                        null,
                        true,
                        false,
                        false,
                        false,
                        0,
                        false,
                        false,
                        false,
                        false);

                throw new global::java.nio.file.NoSuchFileException(path);
            }

            try
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                    return DotNetDosFileAttributesAccessor.Init(
                        ToFileTime(fileInfo.CreationTimeUtc),
                        ToFileTime(fileInfo.LastAccessTimeUtc),
                        ToFileTime(fileInfo.LastWriteTimeUtc),
                        null,
                        false,
                        false,
                        true,
                        false,
                        fileInfo.Length,
                        fileInfo.IsReadOnly,
                        (fileInfo.Attributes & FileAttributes.Hidden) != 0,
                        (fileInfo.Attributes & FileAttributes.Archive) != 0,
                        (fileInfo.Attributes & FileAttributes.System) != 0);

                var directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                    return DotNetDosFileAttributesAccessor.Init(
                        ToFileTime(directoryInfo.CreationTimeUtc),
                        ToFileTime(directoryInfo.LastAccessTimeUtc),
                        ToFileTime(directoryInfo.LastWriteTimeUtc),
                        null,
                        true,
                        false,
                        false,
                        false,
                        0,
                        (directoryInfo.Attributes & FileAttributes.ReadOnly) != 0,
                        (directoryInfo.Attributes & FileAttributes.Hidden) != 0,
                        (directoryInfo.Attributes & FileAttributes.Archive) != 0,
                        (directoryInfo.Attributes & FileAttributes.System) != 0);

                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (ArgumentException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (IOException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (NotSupportedException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (SecurityException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

    }

}
