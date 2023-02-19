using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Vfs;

namespace IKVM.Runtime.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetFileSystemProvider'.
    /// </summary>
    static class DotNetFileSystemProvider
    {

#if FIRST_PASS == false

        static SecurityManagerAccessor securityManagerAccessor;
        static SecurityManagerAccessor SecurityManagerAccessor => JVM.BaseAccessors.Get(ref securityManagerAccessor);

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="rights"></param>
        /// <param name="share"></param>
        /// <param name="options"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static object open0(string path, FileMode mode, FileSystemRights rights, FileShare share, FileOptions options, object sm)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else

            if (sm != null)
            {
                if ((rights & FileSystemRights.Read) != 0)
                {
                    SecurityManagerAccessor.InvokeCheckRead(sm, path);
                }
                if ((rights & (FileSystemRights.Write | FileSystemRights.AppendData)) != 0)
                {
                    SecurityManagerAccessor.InvokeCheckWrite(sm, path);
                }
                if ((options & FileOptions.DeleteOnClose) != 0)
                {
                    SecurityManagerAccessor.InvokeCheckDelete(sm, path);
                }
            }

            var access = (FileAccess)0;
            if ((rights & FileSystemRights.Read) != 0)
                access |= FileAccess.Read;
            if ((rights & (FileSystemRights.Write | FileSystemRights.AppendData)) != 0)
                access |= FileAccess.Write;
            if (access == 0)
                access = FileAccess.ReadWrite;

            try
            {
                if (VfsTable.Default.GetEntry(path) is VfsFile vfsFile)
                    return FileDescriptorAccessor.FromStream(vfsFile.Open(mode, access));
                else
#if NETFRAMEWORK
                    return FileDescriptorAccessor.FromStream(new FileStream(path, mode, rights, share, 8, options));
#else
                    return FileDescriptorAccessor.FromStream(new FileStream(path, mode, access, share, 8, options));
#endif
            }
            catch (ArgumentException e)
            {
                throw new global::java.nio.file.FileSystemException(path, null, e.Message);
            }
            catch (FileNotFoundException)
            {
                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (DirectoryNotFoundException)
            {
                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (PlatformNotSupportedException e)
            {
                throw new global::java.lang.UnsupportedOperationException(e.Message);
            }
            catch (IOException) when (mode == FileMode.CreateNew && File.Exists(path))
            {
                throw new global::java.nio.file.FileAlreadyExistsException(path);
            }
            catch (IOException e)
            {
                throw new global::java.nio.file.FileSystemException(path, null, e.Message);
            }
            catch (SecurityException)
            {
                throw new global::java.nio.file.AccessDeniedException(path);
            }
            catch (UnauthorizedAccessException)
            {
                throw new global::java.nio.file.AccessDeniedException(path);
            }
#endif
        }

    }

}
