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
    /// Implements the native methods for 'DotNetBasicFileAttributeView'.
    /// </summary>
    static class DotNetBasicFileAttributeView
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;
        static FileTimeAccessor fileTimeAccessor;
        static DotNetBasicFileAttributeViewAccessor dotNetBasicFileAttributeViewAccessor;

        static SystemAccessor SystemAccessor => JVM.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.BaseAccessors.Get(ref securityManagerAccessor);

        static FileTimeAccessor FileTimeAccessor => JVM.BaseAccessors.Get(ref fileTimeAccessor);

        static DotNetBasicFileAttributeViewAccessor DotNetBasicFileAttributeViewAccessor => JVM.BaseAccessors.Get(ref dotNetBasicFileAttributeViewAccessor);

#endif

        /// <summary>
        /// Converts the given <see cref="global::java.nio.file.attribute.FileTime"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="fileTime"></param>
        /// <returns></returns>
        static DateTime? ToDateTime(object fileTime)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETFRAMEWORK
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#else
            var epoch = DateTime.UnixEpoch;
#endif
            return fileTime != null ? epoch + TimeSpan.FromMilliseconds(FileTimeAccessor.InvokeToMillis(fileTime)) : null;
#endif
        }

        /// <summary>
        /// Implements the native method 'setTimes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="lastModifiedTime"></param>
        /// <param name="lastAccessTime"></param>
        /// <param name="createdTime"></param>
        /// <exception cref="global::java.nio.file.NoSuchFileException"></exception>
        /// <exception cref="global::java.io.IOException"></exception>
        public static void setTimes(object self, object lastModifiedTime, object lastAccessTime, object createdTime)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var path = DotNetBasicFileAttributeViewAccessor.GetPath(self);
            if (path == null)
                throw new global::java.lang.NullPointerException();

            var sm = SystemAccessor.InvokeGetSecurityManager();
            if (sm != null)
                SecurityManagerAccessor.InvokeCheckWrite(sm, path);

            if (VfsTable.Default.IsPath(path))
                throw new global::java.io.IOException("Cannot modify VFS entries.");

            try
            {
                if (File.Exists(path))
                {
                    if (lastModifiedTime != null)
                        File.SetLastWriteTimeUtc(path, (DateTime)ToDateTime(lastModifiedTime));
                    if (lastAccessTime != null)
                        File.SetLastAccessTimeUtc(path, (DateTime)ToDateTime(lastAccessTime));
                    if (createdTime != null)
                        File.SetCreationTimeUtc(path, (DateTime)ToDateTime(createdTime));
                }
                else if (Directory.Exists(path))
                {
                    if (lastModifiedTime != null)
                        Directory.SetLastWriteTimeUtc(path, (DateTime)ToDateTime(lastModifiedTime));
                    if (lastAccessTime != null)
                        Directory.SetLastAccessTimeUtc(path, (DateTime)ToDateTime(lastAccessTime));
                    if (createdTime != null)
                        Directory.SetCreationTimeUtc(path, (DateTime)ToDateTime(createdTime));
                }
                else
                {
                    throw new global::java.nio.file.NoSuchFileException(path);
                }
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
