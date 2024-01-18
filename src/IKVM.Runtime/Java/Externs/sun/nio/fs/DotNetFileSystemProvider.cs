using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Text;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Accessors.Sun.Nio.Fs;
using IKVM.Runtime.JNI;
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

        static SystemAccessor SystemAccessor => JVM.Internal.BaseAccessors.Get(ref systemAccessor);

        static SecurityManagerAccessor SecurityManagerAccessor => JVM.Internal.BaseAccessors.Get(ref securityManagerAccessor);

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

        static DotNetPathAccessor DotNetPathAccessor => JVM.Internal.BaseAccessors.Get(ref dotNetPathAccessor);

        static DotNetDirectoryStreamAccessor DotNetDirectoryStreamAccessor => JVM.Internal.BaseAccessors.Get(ref dotNetDirectoryStreamAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__rename0(IntPtr jniEnv, IntPtr clazz, long fromAddress, long toAddress);
        static __jniDelegate__rename0 __jniPtr__rename0;

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
                if (JVM.Vfs.IsPath(path))
                {
                    if (JVM.Vfs.GetEntry(path) is VfsFile vfsFile)
                    {
                        var fdo = FileDescriptorAccessor.Init();
                        FileDescriptorAccessor.SetStream(fdo, vfsFile.Open(mode, access));
                        return fdo;
                    }

                    throw new global::java.lang.UnsupportedOperationException();
                }
                else
                {
#if NETFRAMEWORK
                    var fdo = FileDescriptorAccessor.Init();
                    FileDescriptorAccessor.SetStream(fdo, new FileStream(path, mode, rights, share, bufferSize, options));
                    return fdo;
#else
                    if (RuntimeUtil.IsWindows)
                    {
                        var fdo = FileDescriptorAccessor.Init();
                        FileDescriptorAccessor.SetStream(fdo, new FileStream(path, mode, access, share, bufferSize, options));
                        return fdo;
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

                        var h = new SafeFileHandle((IntPtr)r, false);

                        // .NET Core 5+ maintains an IsAsync property validated by FileStream
                        // this property isn't set properly on Unix
                        // https://github.com/dotnet/runtime/issues/85560
                        if ((options & FileOptions.Asynchronous) != 0)
                            SafeFileHandleIsAsyncPropertySetter?.Invoke(h, true);

                        var fdo = FileDescriptorAccessor.Init();
                        FileDescriptorAccessor.SetStream(fdo, new FileStream(h, access, bufferSize, (options & FileOptions.Asynchronous) != 0));
                        return fdo;
                    }
#endif
                }
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
        /// Invokes the MoveFileEx native function on Windows.
        /// </summary>
        /// <param name="lpExistingFileName"></param>
        /// <param name="lpNewFileName"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Kernel32", SetLastError = true)]
        static extern int MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);

        /// <summary>
        /// Implements the native functionality for rename0.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="global::java.nio.file.NoSuchFileException"></exception>
        /// <exception cref="global::java.nio.file.AccessDeniedException"></exception>
        /// <exception cref="global::java.nio.file.AtomicMoveNotSupportedException"></exception>
        /// <exception cref="global::java.nio.file.FileAlreadyExistsException"></exception>
        /// <exception cref="global::java.nio.file.FileSystemException"></exception>
        public static unsafe void rename0(string source, string target)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const int MOVEFILE_REPLACE_EXISTING = 1;
                if (MoveFileEx(source, target, MOVEFILE_REPLACE_EXISTING) == 0)
                {
                    const int ERROR_FILE_NOT_FOUND = 2;
                    const int ERROR_PATH_NOT_FOUND = 3;
                    const int ERROR_ACCESS_DENIED = 5;
                    const int ERROR_NOT_SAME_DEVICE = 17;
                    const int ERROR_FILE_EXISTS = 80;
                    const int ERROR_ALREADY_EXISTS = 183;

                    int lastError = Marshal.GetLastWin32Error();
                    switch (lastError)
                    {
                        case ERROR_FILE_NOT_FOUND:
                        case ERROR_PATH_NOT_FOUND:
                            throw new global::java.nio.file.NoSuchFileException(source, target, null);
                        case ERROR_ACCESS_DENIED:
                            throw new global::java.nio.file.AccessDeniedException(source, target, null);
                        case ERROR_NOT_SAME_DEVICE:
                            throw new global::java.nio.file.AtomicMoveNotSupportedException(source, target, "Unsupported copy option");
                        case ERROR_FILE_EXISTS:
                        case ERROR_ALREADY_EXISTS:
                            throw new global::java.nio.file.FileAlreadyExistsException(source, target, null);
                        default:
                            throw new global::java.nio.file.FileSystemException(source, target, "Error " + lastError);
                    }
                }

                return;
            }
            else
            {
                try
                {
                    __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::sun.nio.fs.DotNetFileSystemProvider).TypeHandle);
                    __jniPtr__rename0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__rename0>(JNIFrame.GetFuncPtr(__callerID, "sun/nio/fs/UnixNativeDispatcher", nameof(rename0), "(JJ)V"));
                    var jniFrm = new JNIFrame();
                    var jniEnv = jniFrm.Enter(__callerID);
                    try
                    {
                        byte[] sourceBuf = null;
                        byte[] targetBuf = null;

                        try
                        {
                            var sourceLen = Encoding.UTF8.GetByteCount(source) + 1;
                            sourceBuf = ArrayPool<byte>.Shared.Rent(sourceLen);
                            sourceBuf[sourceLen - 1] = 0;
                            Encoding.UTF8.GetBytes(source, 0, source.Length, sourceBuf, 0);

                            var targetLen = Encoding.UTF8.GetByteCount(target) + 1;
                            targetBuf = ArrayPool<byte>.Shared.Rent(targetLen);
                            targetBuf[targetLen - 1] = 0;
                            Encoding.UTF8.GetBytes(target, 0, target.Length, targetBuf, 0);

                            fixed (byte* sourcePtr = sourceBuf)
                            fixed (byte* targetPtr = targetBuf)
                            {
                                __jniPtr__rename0(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::sun.nio.fs.DotNetFileSystemProvider>.Value), (long)(IntPtr)sourcePtr, (long)(IntPtr)targetPtr);
                            }
                        }
                        finally
                        {
                            if (sourceBuf != null)
                                ArrayPool<byte>.Shared.Return(sourceBuf);
                            if (targetBuf != null)
                                ArrayPool<byte>.Shared.Return(targetBuf);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("*** exception in native code ***");
                        System.Console.WriteLine(ex);
                        throw;
                    }
                    finally
                    {
                        jniFrm.Leave();
                    }
                }
                catch (global::sun.nio.fs.UnixException e)
                {
                    const int EXDEV = 18;

                    if (e.errno() == EXDEV)
                        throw new global::java.nio.file.AtomicMoveNotSupportedException(source, target, e.errorString());

                    e.rethrowAsIOException(source, target);
                }
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
                if (JVM.Vfs.IsPath(path))
                {
                    if (JVM.Vfs.GetEntry(path) is not VfsDirectory vfsDirectory)
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
