using System;
using System.Threading.Tasks;
using System.IO;

#if FIRST_PASS == false

using java.io;
using java.nio;
using java.security;
using java.lang;
using java.nio.channels;
using java.util.concurrent;

using sun.nio.ch;

#endif

namespace IKVM.Java.Externs.sun.nio.ch
{

    static class DotNetAsynchronousFileChannelImpl
    {

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
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, position, size, shared, attachment, (CompletionHandler)handler, LockAsync);
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
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (ByteBuffer)dst, position, attachment, (CompletionHandler)handler, ReadAsync);
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
            return ((DotNetAsynchronousChannelGroup)((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self).group()).Execute((global::sun.nio.ch.DotNetAsynchronousFileChannelImpl)self, (ByteBuffer)src, position, attachment, (CompletionHandler)handler, WriteAsync);
#endif
        }


#if FIRST_PASS == false

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static void Close(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self)
        {
            self.closeLock.writeLock().@lock();

            try
            {
                self.closed = true;
            }
            finally
            {
                self.closeLock.writeLock().unlock();
            }

            self.invalidateAllLocks();
            self.fdObj.close();
        }

        /// <summary>
        /// Implements the Lock logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="future"></param>
        /// <returns></returns>
        static async Task<FileLock> LockAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, long position, long size, bool shared, AccessControlContext accessControlContext, PendingFuture future)
        {
            if (shared && self.reading == false)
                throw new NonReadableChannelException();
            if (shared == false && self.writing == false)
                throw new NonWritableChannelException();

            var fli = self.addToFileLockTable(position, size, shared);
            if (fli == null)
                throw new ClosedChannelException();

            try
            {
                var stream = (FileStream)self.fdObj.getStream();
                if (stream == null)
                    throw new ClosedChannelException();

                while (true)
                {
                    try
                    {
                        if (future.isCancelled())
                            return null;

                        try
                        {
                            stream.Lock(position, fli.size());
                            return fli;
                        }
                        catch (System.IO.IOException)
                        {
                            // we failed to acquire the lock, try again next iteration
                        }

                        // try again shortly
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                    }
                    catch (ClosedChannelException)
                    {
                        throw new AsynchronousCloseException();
                    }
                    catch (System.Exception e)
                    {
                        throw new global::java.io.IOException(e);
                    }
                }
            }
            catch
            {
                self.removeFromFileLockTable(fli);
            }

            // should not be reachable since while() does not exit
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        static FileLock TryLock(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, long position, long size, bool shared)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the Lock logic as an asynchronous task.
        /// </summary>
        /// <returns></returns>
        static async void Release(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, FileLockImpl fli)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the Read logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="future"></param>
        /// <returns></returns>
        static async Task<Integer> ReadAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, ByteBuffer dst, long position, AccessControlContext accessControlContext, PendingFuture future)
        {
            if (self.reading == false)
                throw new NonReadableChannelException();
            if (position < 0)
                throw new IllegalArgumentException("Negative position");
            if (dst.isReadOnly())
                throw new IllegalArgumentException("Read-only buffer");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the Accept logic as an asynchronous task.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="position"></param>
        /// <param name="accessControlContext"></param>
        /// <param name="future"></param>
        /// <returns></returns>
        /// <exception cref="ClosedChannelException"></exception>
        /// <exception cref="RuntimeException"></exception>
        /// <exception cref="NotYetBoundException"></exception>
        /// <exception cref="AcceptPendingException"></exception>
        /// <exception cref="InterruptedIOException"></exception>
        /// <exception cref="AsynchronousCloseException"></exception>
        /// <exception cref="IOException"></exception>
        static async Task<Integer> WriteAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, ByteBuffer src, long position, AccessControlContext accessControlContext, PendingFuture future)
        {
            throw new NotImplementedException();
        }

#endif

    }

}
