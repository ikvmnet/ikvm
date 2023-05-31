using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using System.ComponentModel;
using System.Runtime.InteropServices;

using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Nio;
using IKVM.Runtime;

using Microsoft.Win32.SafeHandles;

using Mono.Unix.Native;
using Mono.Unix;

#if FIRST_PASS == false

using java.io;
using java.nio;
using java.security;
using java.lang;
using java.nio.channels;

using sun.nio.ch;

#endif

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'DotNetAsynchronousFileChannelImpl'.
    /// </summary>
    static class DotNetAsynchronousFileChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        /// <summary>
        /// Implements the native method for 'onCancel0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="task"></param>
        public static void onCancel0(object self, object task)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            OnCancel((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (PendingFuture)task);
#endif
        }

        /// <summary>
        /// Implements the native method for 'close0'.
        /// </summary>
        /// <param name="self"></param>
        public static void close0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Close((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self);
#endif
        }

        /// <summary>
        /// Implements the native method 'size0'.
        /// </summary>
        /// <param name="self"></param>
        public static long size0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return Size((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self);
#endif
        }

        /// <summary>
        /// Implements the native method 'truncate0'.
        /// </summary>
        /// <param name="self"></param>
        public static object truncate0(object self, long size)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return Truncate((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, size);
#endif
        }

        /// <summary>
        /// Implements the native method 'force0'.
        /// </summary>
        /// <param name="self"></param>
        public static void force0(object self, bool metaData)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Force((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, metaData);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implLock0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implLock0(object self, long position, long size, bool shared, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((global::sun.nio.ch.DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, position, size, shared, attachment, (global::java.nio.channels.CompletionHandler)handler, LockAsync);
#endif
        }

        /// <summary>
        /// Implements the native method for 'tryLock0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        public static object tryLock0(object self, long position, long size, bool shared)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return TryLock((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, position, size, shared);
#endif
        }

        /// <summary>
        /// Implements the native method for 'tryLock0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fli"></param>
        public static void implRelease0(object self, object fli)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Release((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (FileLockImpl)fli);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implRead0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dst"></param>
        /// <param name="position"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implRead0(object self, object dst, long position, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((global::sun.nio.ch.DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (global::java.nio.ByteBuffer)dst, position, attachment, (global::java.nio.channels.CompletionHandler)handler, ReadAsync);
#endif
        }

        /// <summary>
        /// Implements the native method for 'implWrite0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="src"></param>
        /// <param name="position"></param>
        /// <param name="attachment"></param>
        /// <param name="handler"></param>
        public static object implWrite0(object self, object src, long position, object attachment, object handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else      
            return ((global::sun.nio.ch.DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (global::java.nio.ByteBuffer)src, position, attachment, (global::java.nio.channels.CompletionHandler)handler, WriteAsync);
#endif
        }


#if FIRST_PASS == false

        /// <summary>
        /// Invoked when the pending future is cancelled.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="future"></param>
        /// <returns></returns>
        static void OnCancel(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, PendingFuture future)
        {
            // signal cancellation on associated cancellation token source
            var cts = (CancellationTokenSource)future.getContext();
            cts?.Cancel();
        }

        /// <summary>
        /// Invoked when the channel is closed.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static void Close(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self)
        {
            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                return;

            try
            {
                self.closeLock.writeLock().@lock();
                FileDescriptorAccessor.SetStream(self.fdObj, null);
                self.closed = true;

                try
                {
                    stream.Close();
                }
                catch
                {
                    // ignore errors closing the stream
                }
            }
            finally
            {
                self.closeLock.writeLock().unlock();
            }

            self.invalidateAllLocks();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static long Size(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self)
        {
            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                throw new global::java.nio.channels.ClosedChannelException();

            try
            {
                self.begin();

                try
                {
                    return stream.Length;
                }
                catch (System.Exception e)
                {
                    throw new global::java.io.IOException(e);
                }
            }
            finally
            {
                self.end();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static AsynchronousFileChannel Truncate(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, long size)
        {
            if (size < 0)
                throw new IllegalArgumentException("Negative size");
            if (self.writing == false)
                throw new global::java.nio.channels.NonWritableChannelException();

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                throw new global::java.nio.channels.AsynchronousCloseException();

            // matters for VFS files
            if (stream.CanWrite == false)
                throw new global::java.nio.channels.NonWritableChannelException();

            try
            {
                self.begin();

                if (size < stream.Length)
                    stream.SetLength(size);
            }
            catch (System.Exception e)
            {
                throw new global::java.io.IOException(e);
            }
            finally
            {
                self.end();
            }

            return self;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static void Force(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, bool metaData)
        {
            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                throw new global::java.nio.channels.AsynchronousCloseException();

            // matters for VFS files
            if (stream.CanWrite == false)
                throw new global::java.nio.channels.NonWritableChannelException();

            try
            {
                self.begin();
                stream.Flush();
            }
            catch (System.Exception e)
            {
                throw new global::java.io.IOException(e);
            }
            finally
            {
                self.end();
            }
        }

        /// <summary>
        /// Invokes the LockFileEx Win32 function.
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwReserved"></param>
        /// <param name="nNumberOfBytesToLockLow"></param>
        /// <param name="nNumberOfBytesToLockHigh"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        static extern unsafe int LockFileEx(SafeFileHandle hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, NativeOverlapped* lpOverlapped);

        /// <summary>
        /// Wraps the LockFileEx function as an async operation.
        /// </summary>
        /// <returns></returns>
        static unsafe ValueTask LockFileExAsync(SafeFileHandle hFile, int dwFlags, int dwReserved, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh, Overlapped overlapped)
        {
            var task = new TaskCompletionSource<object>();
            var iocb = (IOCompletionCallback)((errorCode, numBytes, nativeOverlapped) =>
            {
                try
                {
                    if (errorCode == 0)
                        task.SetResult(null);
                    else
                        task.SetException(new Win32Exception((int)errorCode));
                }
                catch (System.Exception e)
                {
                    task.SetException(e);
                }
                finally
                {
                    Overlapped.Free(nativeOverlapped);
                }
            });

            var optr = overlapped.Pack(iocb, null);
            if (LockFileEx(hFile, dwFlags, dwReserved, nNumberOfBytesToLockLow, nNumberOfBytesToLockHigh, optr) == 0)
            {
                Overlapped.Free(optr);
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return new ValueTask(task.Task);
        }

        /// <summary>
        /// Invokes the UnlockFileEx Win32 function.
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="dwReserved"></param>
        /// <param name="nNumberOfBytesToUnlockLow"></param>
        /// <param name="nNumberOfBytesToUnlockHigh"></param>
        /// <param name="lpOverlapped"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = true)]
        static extern unsafe int UnlockFileEx(SafeFileHandle hFile, int dwReserved, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh, NativeOverlapped* lpOverlapped);

        /// <summary>
        /// Implements the Lock logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<FileLock> LockAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, long position, long size, bool shared, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (shared && self.reading == false)
                throw new global::java.nio.channels.NonReadableChannelException();
            if (shared == false && self.writing == false)
                throw new global::java.nio.channels.NonWritableChannelException();

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                return Task.FromException<FileLock>(new global::java.nio.channels.ClosedChannelException());
            if (stream is not FileStream fs)
                throw new global::java.io.IOException("Stream does not support locking.");

            var fli = self.addToFileLockTable(position, size, shared);
            if (fli == null)
                return Task.FromException<FileLock>(new global::java.nio.channels.ClosedChannelException());

            return ImplAsync();

            async Task<FileLock> ImplAsync()
            {
                try
                {
                    if (RuntimeUtil.IsWindows)
                    {
                        const int LOCKFILE_EXCLUSIVE_LOCK = 2;

                        try
                        {
                            var o = new Overlapped();
                            o.OffsetLow = (int)position;
                            o.OffsetHigh = (int)(position >> 32);

                            var flags = 0;
                            if (shared == false)
                                flags |= LOCKFILE_EXCLUSIVE_LOCK;

                            await LockFileExAsync(fs.SafeFileHandle, flags, 0, (int)size, (int)(size >> 32), o);

                            return fli;
                        }
                        catch (System.Exception) when (IsOpen(self) == false)
                        {
                            throw new global::java.nio.channels.AsynchronousCloseException();
                        }
                        catch (System.Exception e)
                        {
                            throw new global::java.io.IOException(e);
                        }
                    }
                    else if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
                    {
                        while (true)
                        {
                            try
                            {
                                if (cancellationToken.IsCancellationRequested)
                                    return null;

                                try
                                {
                                    self.begin();

                                    var fl = new Flock();
                                    fl.l_whence = SeekFlags.SEEK_SET;
                                    fl.l_len = size == long.MaxValue ? 0 : size;
                                    fl.l_start = position;
                                    fl.l_type = shared ? LockType.F_RDLCK : LockType.F_WRLCK;

                                    // fails immediately with EAGAIN or EACCES if cannot obtain
                                    if (Syscall.fcntl((int)fs.SafeFileHandle.DangerousGetHandle(), FcntlCommand.F_SETLK, ref fl) == 0)
                                        return fli;

                                    var errno = Syscall.GetLastError();
                                    if (errno == Errno.EAGAIN || errno == Errno.EACCES)
                                        continue;

                                    UnixMarshal.ThrowExceptionForError(errno);
                                }
                                finally
                                {
                                    self.end();
                                }
                            }
                            catch (ObjectDisposedException)
                            {
                                throw new global::java.nio.channels.AsynchronousCloseException();
                            }
                            catch (System.Exception) when (IsOpen(self) == false)
                            {
                                throw new global::java.nio.channels.AsynchronousCloseException();
                            }
                            catch (System.Exception e)
                            {
                                throw new global::java.io.IOException(e);
                            }

                            await Task.Delay(TimeSpan.FromMilliseconds(100));
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            try
                            {
                                if (cancellationToken.IsCancellationRequested)
                                    return null;

                                try
                                {
                                    self.begin();

                                    fs.Lock(position, size);
                                    return fli;
                                }
                                catch (System.IO.IOException)
                                {
                                    // we failed to acquire the lock, try again next iteration
                                }
                                finally
                                {
                                    self.end();
                                }

                                // try again shortly
                                await Task.Delay(TimeSpan.FromMilliseconds(100));
                            }
                            catch (ObjectDisposedException)
                            {
                                throw new global::java.nio.channels.AsynchronousCloseException();
                            }
                            catch (System.Exception) when (IsOpen(self) == false)
                            {
                                throw new global::java.nio.channels.AsynchronousCloseException();
                            }
                            catch (System.Exception e)
                            {
                                throw new global::java.io.IOException(e);
                            }
                        }
                    }
                }
                catch
                {
                    self.removeFromFileLockTable(fli);
                }

                return null;
            };
        }

        /// <summary>
        /// Attempts to lock a file region.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        static unsafe FileLock TryLock(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, long position, long size, bool shared)
        {
            if (shared && self.reading == false)
                throw new global::java.nio.channels.NonReadableChannelException();
            if (shared == false && self.writing == false)
                throw new global::java.nio.channels.NonWritableChannelException();

            if (IsOpen(self) == false)
                throw new global::java.nio.channels.ClosedChannelException();

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                throw new global::java.nio.channels.AsynchronousCloseException();
            if (stream is not FileStream fs)
                throw new global::java.io.IOException("File does not support locking.");

            var fli = self.addToFileLockTable(position, size, shared);
            if (fli == null)
                throw new global::java.nio.channels.ClosedChannelException();

            try
            {
                try
                {
                    self.begin();

                    if (RuntimeUtil.IsWindows)
                    {
                        const int LOCKFILE_FAIL_IMMEDIATELY = 1;
                        const int LOCKFILE_EXCLUSIVE_LOCK = 2;

                        var o = new Overlapped();
                        o.OffsetLow = (int)position;
                        o.OffsetHigh = (int)(position >> 32);

                        var flags = LOCKFILE_FAIL_IMMEDIATELY;
                        if (shared == false)
                            flags |= LOCKFILE_EXCLUSIVE_LOCK;

                        // issue async request but wait for synchronous result
                        var t = LockFileExAsync(fs.SafeFileHandle, flags, 0, (int)size, (int)(size >> 32), o);
                        t.GetAwaiter().GetResult();

                        return fli;
                    }
                    else if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
                    {
                        var fl = new Flock();
                        fl.l_whence = SeekFlags.SEEK_SET;
                        fl.l_len = size == long.MaxValue ? 0 : size;
                        fl.l_start = position;
                        fl.l_type = shared ? LockType.F_RDLCK : LockType.F_WRLCK;

                        // fails immediately with EAGAIN or EACCES if cannot obtain
                        if (Syscall.fcntl((int)fs.SafeFileHandle.DangerousGetHandle(), FcntlCommand.F_SETLK, ref fl) == 0)
                            return fli;

                        var errno = Syscall.GetLastError();
                        if (errno == Errno.EAGAIN || errno == Errno.EACCES)
                        {
                            self.removeFromFileLockTable(fli);
                            return null;
                        }

                        UnixMarshal.ThrowExceptionForError(errno);
                    }
                    else
                    {
                        fs.Lock(position, size);
                    }
                }
                catch (ObjectDisposedException)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception) when (IsOpen(self) == false)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception e)
                {
                    throw new global::java.io.IOException(e);
                }
                finally
                {
                    self.end();
                }
            }
            catch
            {
                self.removeFromFileLockTable(fli);
            }

            return null;
        }

        /// <summary>
        /// Implements the Release logic as an asynchronous task.
        /// </summary>
        /// <returns></returns>
        static unsafe void Release(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, FileLockImpl fli)
        {
            if (IsOpen(self) == false)
                return;

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                return;

            if (stream is not FileStream fs)
                throw new global::java.io.IOException("File does not support locking.");

            try
            {
                self.begin();

                var pos = fli.position();
                var size = fli.size();

                if (RuntimeUtil.IsWindows)
                {
                    const int ERROR_NOT_LOCKED = 158;

                    try
                    {
                        var o = new Overlapped();
                        o.OffsetLow = (int)pos;
                        o.OffsetHigh = (int)(pos >> 32);
                        var p = o.Pack(null, null);

                        try
                        {
                            if (UnlockFileEx(fs.SafeFileHandle, 0, (int)size, (int)(size >> 32), p) == 0)
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                        finally
                        {
                            Overlapped.Free(p);
                        }
                    }
                    catch (Win32Exception e) when (e.ErrorCode == ERROR_NOT_LOCKED)
                    {
                        return;
                    }
                }
                else if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
                {
                    var fl = new Flock();
                    fl.l_whence = SeekFlags.SEEK_SET;
                    fl.l_len = size == long.MaxValue ? 0 : size;
                    fl.l_start = pos;
                    fl.l_type = LockType.F_UNLCK;

                    var r = Syscall.fcntl((int)fs.SafeFileHandle.DangerousGetHandle(), FcntlCommand.F_SETLK, ref fl);
                    if (r == -1)
                        UnixMarshal.ThrowExceptionForLastErrorIf(r);
                }
                else
                {
                    fs.Unlock(pos, size);
                }
            }
            catch (System.IO.IOException e) when (NotLockedHack.IsErrorNotLocked(e))
            {
                return;
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.nio.channels.AsynchronousCloseException();
            }
            catch (System.Exception) when (IsOpen(self) == false)
            {
                throw new global::java.nio.channels.AsynchronousCloseException();
            }
            catch (System.Exception e)
            {
                throw new global::java.io.IOException(e);
            }
            finally
            {
                self.end();
            }
        }

        /// <summary>
        /// Implements the Read logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<Integer> ReadAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, global::java.nio.ByteBuffer dst, long position, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.reading == false)
                throw new global::java.nio.channels.NonReadableChannelException();
            if (position < 0)
                throw new global::java.lang.IllegalArgumentException("Negative position");
            if (dst.isReadOnly())
                throw new global::java.lang.IllegalArgumentException("Read-only buffer");

            if (IsOpen(self) == false)
                return Task.FromException<Integer>(new global::java.nio.channels.ClosedChannelException());

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                return Task.FromException<Integer>(new global::java.nio.channels.ClosedChannelException());
            if (stream.CanRead == false)
                return Task.FromException<Integer>(new global::java.io.IOException("Stream does not support reading."));

            // check buffer
            int pos = dst.position();
            int lim = dst.limit();
            int rem = pos <= lim ? lim - pos : 0;
            if (pos > lim)
                return Task.FromException<Integer>(new global::java.lang.IllegalArgumentException("Position after limit."));

            if (cancellationToken.IsCancellationRequested)
                return null;

            return ImplAsync();

            async Task<Integer> ImplAsync()
            {
                var lck = FileDescriptorAccessor.GetSemaphore(self.fdObj);
                if (lck == null)
                {
                    lck = new SemaphoreSlim(1, 1);
                    if (FileDescriptorAccessor.CompareExchangeSemaphore(self.fdObj, lck, null) != null)
                        lck.Dispose();

                    lck = FileDescriptorAccessor.GetSemaphore(self.fdObj);
                }

                try
                {
                    await lck.WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return null;
                }

                try
                {
                    // move file to specified position
                    if (stream.Position != position)
                    {
                        if (stream.CanSeek == false)
                            throw new global::java.lang.IllegalArgumentException("Seek failed.");

                        stream.Seek(position, SeekOrigin.Begin);
                    }

#if NETFRAMEWORK
                    if (dst is DirectBuffer dir)
                    {
                        var tmp = new byte[rem];
                        var n = await stream.ReadAsync(tmp, 0, rem, cancellationToken);
                        dst.put(tmp, 0, n);
                        return new global::java.lang.Integer(n);
                    }
                    else
                    {
                        var n = await stream.ReadAsync(dst.array(), dst.arrayOffset() + pos, rem, cancellationToken);
                        dst.position(pos + n);
                        return new global::java.lang.Integer(n);
                    }
#else
                    if (dst is DirectBuffer dir)
                    {
                        using var mgr = new DirectBufferMemoryManager(dir);
                        var mem = mgr.Memory.Slice(pos, rem);
                        var n = await stream.ReadAsync(mem, cancellationToken);
                        dst.position(pos + n);
                        return new global::java.lang.Integer(n);
                    }
                    else
                    {
                        var n = await stream.ReadAsync(dst.array(), dst.arrayOffset() + pos, rem, cancellationToken);
                        dst.position(pos + n);
                        return new global::java.lang.Integer(n);
                    }
#endif
                }
                catch (OperationCanceledException)
                {
                    return null;
                }
                catch (ObjectDisposedException)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception) when (IsOpen(self) == false)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception) when (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
                catch (System.Exception e)
                {
                    throw new global::java.io.IOException(e);
                }
                finally
                {
                    lck.Release();
                }
            }
        }

        /// <summary>
        /// Implements the Write logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static Task<global::java.lang.Integer> WriteAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, global::java.nio.ByteBuffer src, long position, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.writing == false)
                throw new global::java.nio.channels.NonWritableChannelException();
            if (position < 0)
                throw new global::java.lang.IllegalArgumentException("Negative position");

            if (IsOpen(self) == false)
                return Task.FromException<global::java.lang.Integer>(new global::java.nio.channels.ClosedChannelException());

            var stream = FileDescriptorAccessor.GetStream(self.fdObj);
            if (stream == null)
                return Task.FromException<global::java.lang.Integer>(new global::java.nio.channels.ClosedChannelException());
            if (stream.CanWrite == false)
                return Task.FromException<global::java.lang.Integer>(new global::java.io.IOException("Stream does not support writing."));

            // check buffer
            int pos = src.position();
            int lim = src.limit();
            int rem = pos <= lim ? lim - pos : 0;
            if (rem == 0)
                return Task.FromResult<global::java.lang.Integer>(new global::java.lang.Integer(0));
            if (pos > lim)
                return Task.FromException<global::java.lang.Integer>(new global::java.lang.IllegalArgumentException("Position after limit."));

            if (cancellationToken.IsCancellationRequested)
                return null;

            return ImplAsync();

            async Task<Integer> ImplAsync()
            {
                var lck = FileDescriptorAccessor.GetSemaphore(self.fdObj);
                if (lck == null)
                {
                    lck = new SemaphoreSlim(1, 1);
                    if (FileDescriptorAccessor.CompareExchangeSemaphore(self.fdObj, lck, null) != null)
                        lck.Dispose();

                    lck = FileDescriptorAccessor.GetSemaphore(self.fdObj);
                }

                try
                {
                    await lck.WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return null;
                }

                try
                {
                    // move file to specified position
                    if (stream.Position != position)
                    {
                        if (stream.CanSeek == false)
                            throw new global::java.lang.IllegalArgumentException("Seek failed.");

                        stream.Seek(position, SeekOrigin.Begin);
                    }

#if NETFRAMEWORK
                    if (src is DirectBuffer dir)
                    {
                        var tmp = new byte[rem];
                        src.get(tmp, 0, rem);
                        await stream.WriteAsync(tmp, 0, rem, cancellationToken);
                        return new global::java.lang.Integer(rem);
                    }
                    else
                    {
                        await stream.WriteAsync(src.array(), src.arrayOffset() + pos, rem, cancellationToken);
                        src.position(pos + rem);
                        return new global::java.lang.Integer(rem);
                    }
#else
                    if (src is DirectBuffer dir)
                    {
                        using var mgr = new DirectBufferMemoryManager(dir);
                        var mem = mgr.Memory.Slice(pos, rem);
                        await stream.WriteAsync(mem, cancellationToken);
                        src.position(pos + rem);
                        return new global::java.lang.Integer(rem);
                    }
                    else
                    {
                        await stream.WriteAsync(src.array(), src.arrayOffset() + pos, rem, cancellationToken);
                        src.position(pos + rem);
                        return new global::java.lang.Integer(rem);
                    }
#endif
                }
                catch (OperationCanceledException)
                {
                    return null;
                }
                catch (ObjectDisposedException)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception) when (IsOpen(self) == false)
                {
                    throw new global::java.nio.channels.AsynchronousCloseException();
                }
                catch (System.Exception) when (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
                catch (System.Exception e)
                {
                    throw new global::java.io.IOException(e);
                }
                finally
                {
                    lck.Release();
                }
            }
        }

        static bool IsOpen(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl ch)
        {
            Interlocked.MemoryBarrier();
            return ch.isOpen();
        }

#endif

    }

}
