using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.AccessControl;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using IKVM.Runtime.Vfs;

using Microsoft.Win32.SafeHandles;

using Mono.Unix;
using Mono.Unix.Native;

namespace IKVM.Java.Externs.sun.nio.fs
{

    /// <summary>
    /// Implements the native methods for 'DotNetFileSystemProvider'.
    /// </summary>
    static class DotNetFileSystemProvider
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;
        static SecurityManagerAccessor securityManagerAccessor;
        static FileDescriptorAccessor fileDescriptorAccessor;
        static DotNetPathAccessor dotNetPathAccessor;
        static DotNetDirectoryStreamAccessor dotNetDirectoryStreamAccessor;

        static SystemAccessor SystemAccessor => JVM.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.BaseAccessors.Get(ref securityManagerAccessor);

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static DotNetPathAccessor DotNetPathAccessor => JVM.BaseAccessors.Get(ref dotNetPathAccessor);

        static DotNetDirectoryStreamAccessor DotNetDirectoryStreamAccessor => JVM.BaseAccessors.Get(ref dotNetDirectoryStreamAccessor);

#if NETCOREAPP

        static readonly PropertyInfo SafeFileHandleIsAsyncProperty = typeof(SafeFileHandle).GetProperty("IsAsync", BindingFlags.Public | BindingFlags.Instance);
        static readonly Action<SafeFileHandle, bool> SafeFileHandleIsAsyncPropertySetter = SafeFileHandleIsAsyncProperty != null ? (o, v) => SafeFileHandleIsAsyncProperty.SetValue(o, v) : null;

#endif

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
        public static object open0(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, object sm)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (sm != null)
            {
                if ((rights & FileSystemRights.Read) != 0)
                    SecurityManagerAccessor.InvokeCheckRead(sm, path);
                if ((rights & FileSystemRights.Write) != 0)
                    SecurityManagerAccessor.InvokeCheckWrite(sm, path);
                if ((rights & FileSystemRights.AppendData) != 0)
                    SecurityManagerAccessor.InvokeCheckWrite(sm, path);
                if ((options & FileOptions.DeleteOnClose) != 0)
                    SecurityManagerAccessor.InvokeCheckDelete(sm, path);
            }

            var access = (FileAccess)0;
            if ((rights & FileSystemRights.Read) != 0)
                access |= FileAccess.Read;
            if ((rights & FileSystemRights.Write) != 0)
                access |= FileAccess.Write;
            if ((rights & FileSystemRights.AppendData) != 0)
                access |= FileAccess.Write;
            if (access == 0)
                access = FileAccess.ReadWrite;

            try
            {
                if (VfsTable.Default.IsPath(path))
                {
                    if (VfsTable.Default.GetEntry(path) is VfsFile vfsFile)
                        return FileDescriptorAccessor.FromStream(vfsFile.Open(mode, access));

                    throw new global::java.lang.UnsupportedOperationException();
                }

#if NETFRAMEWORK
                return FileDescriptorAccessor.FromStream(new FileStream(path, mode, rights, share, bufferSize, options));
#else
                if (RuntimeUtil.IsWindows)
                {
                    return FileDescriptorAccessor.FromStream(new FileStream(path, mode, access, share, bufferSize, options));
                }
                else
                {
                    var flags = (OpenFlags)0;
                    if (mode == FileMode.Create)
                        flags |= OpenFlags.O_CREAT | OpenFlags.O_TRUNC;
                    if (mode == FileMode.Open)
                        flags |= OpenFlags.O_EXCL;
                    if (mode == FileMode.OpenOrCreate)
                        flags |= OpenFlags.O_CREAT;
                    if (mode == FileMode.Append)
                        flags |= OpenFlags.O_APPEND;
                    if (mode == FileMode.CreateNew)
                        flags |= OpenFlags.O_CREAT | OpenFlags.O_EXCL;

                    if ((rights & FileSystemRights.Read) != 0 && (rights & FileSystemRights.Write) != 0)
                        flags |= OpenFlags.O_RDWR;
                    if ((rights & FileSystemRights.Read) != 0 && (rights & FileSystemRights.Write) == 0)
                        flags |= OpenFlags.O_RDONLY;
                    if ((rights & FileSystemRights.Read) == 0 && (rights & FileSystemRights.Write) != 0)
                        flags |= OpenFlags.O_WRONLY;

                    if ((options & FileOptions.Asynchronous) != 0)
                        flags |= OpenFlags.O_ASYNC;
                    if ((options & FileOptions.WriteThrough) != 0)
                        flags |= OpenFlags.O_SYNC;

                    var r = Syscall.open(path, flags, FilePermissions.DEFFILEMODE);
                    if (r == -1)
                    {
                        var error = Stdlib.GetLastError();
                        if (error == Errno.EACCES)
                            throw new global::java.nio.file.AccessDeniedException(path);
                        if (error == Errno.EEXIST)
                            throw new global::java.nio.file.FileAlreadyExistsException(path);
                        if (error == Errno.ENOENT)
                            throw new global::java.nio.file.NoSuchFileException(path);
                        if (error == Errno.ENOTDIR)
                            throw new global::java.nio.file.NoSuchFileException(path);
                        if (error == Errno.EROFS)
                            throw new global::java.nio.file.FileAlreadyExistsException(path);

                        throw new UnixIOException(error);
                    }

                    var h = new SafeFileHandle((IntPtr)r, true);

                    // .NET Core 5+ maintains an IsAsync property validated by FileStream
                    // this property isn't set properly on Unix
                    // https://github.com/dotnet/runtime/issues/85560
                    if ((options & FileOptions.Asynchronous) != 0)
                        SafeFileHandleIsAsyncPropertySetter?.Invoke(h, true);

                    return FileDescriptorAccessor.FromStream(new FileStream(h, access, bufferSize, (options & FileOptions.Asynchronous) != 0));
                }
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

        /// <summary>
        /// Implements the native method 'newDirectoryStream'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dir"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object newDirectoryStream(object self, object dir, object filter)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (dir == null)
                throw new global::java.lang.NullPointerException();
            if (filter == null)
                throw new global::java.lang.NullPointerException();

            var path = DotNetPathAccessor.GetPath(dir);
            if (path == null)
                throw new global::java.lang.NullPointerException();

            var sm = SystemAccessor.InvokeGetSecurityManager();
            if (sm != null)
                SecurityManagerAccessor.InvokeCheckRead(sm, path);

            try
            {
                if (VfsTable.Default.IsPath(path))
                {
                    if (VfsTable.Default.GetEntry(path) is not VfsDirectory vfsDirectory)
                        throw new global::java.nio.file.NotDirectoryException(path);

                    return DotNetDirectoryStreamAccessor.Init(dir, vfsDirectory.List().Select(i => Path.Combine(path, i)), filter);
                }

                if (File.Exists(path))
                    throw new global::java.nio.file.NotDirectoryException(path);

                if (Directory.Exists(path) == false)
                    throw new global::java.nio.file.NotDirectoryException(path);

                return DotNetDirectoryStreamAccessor.Init(dir, Directory.EnumerateFileSystemEntries(path), filter);
            }
            catch (global::java.lang.Throwable)
            {
                throw;
            }
            catch (Exception) when (File.Exists(path))
            {
                throw new global::java.nio.file.NotDirectoryException(path);
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e.Message);
            }
#endif
        }

    }

}
