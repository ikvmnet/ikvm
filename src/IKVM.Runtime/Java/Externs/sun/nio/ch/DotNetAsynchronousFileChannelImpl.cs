using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

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
            try
            {
                self.closeLock.writeLock().@lock();
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
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        static long Size(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self)
        {
            try
            {
                self.begin();
                return self.fdObj.length();
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
                throw new NonWritableChannelException();

            try
            {
                self.begin();
                if (size <= self.fdObj.length())
                    ((FileStream)self.fdObj.getStream()).SetLength(size);
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
            try
            {
                self.begin();
                self.fdObj.sync();
            }
            finally
            {
                self.end();
            }
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
        /// Returns an <see cref="ArraySegment{byte}"/> for the given <see cref="ByteBuffer"/>. May be a memory
        /// mapping to the original array, or may be a newly allocated temporary buffer. Optionally, copy the
        /// original data to the temporary location.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        static unsafe ArraySegment<byte> PrepareArraySegment(ByteBuffer buf, bool copy = false)
        {
            int pos = buf.position();
            int lim = buf.limit();
            int rem = pos <= lim ? lim - pos : 0;

            if (buf is DirectBuffer dir)
            {
                var s = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(rem), 0, rem);

                // optionally copy the original buffer data to the new segment
                if (copy)
                    new ReadOnlySpan<byte>((byte*)(IntPtr)dir.address() + pos, rem).CopyTo(s);

                return s;
            }
            else
            {
                return new ArraySegment<byte>(buf.array(), buf.arrayOffset(), rem);
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
        static async Task<Integer> ReadAsync(global::sun.nio.ch.DotNetAsynchronousFileChannelImpl self, ByteBuffer dst, long position, AccessControlContext accessControlContext, CancellationToken cancellationToken)
        {
            if (self.reading == true)
                throw new NonReadableChannelException();
            if (position < 0)
                throw new IllegalArgumentException("Negative position");
            if (dst.isReadOnly())
                throw new IllegalArgumentException("Read-only buffer");

            var stream = (FileStream)self.fdObj.getStream();
            if (stream == null)
                throw new ClosedChannelException();

            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return null;

                // move file to specified position
                if (stream.Position != position)
                {
                    if (stream.CanSeek == false)
                        throw new IllegalArgumentException("Seek failed.");

                    stream.Seek(position, SeekOrigin.Begin);
                }

#if NETFRAMEWORK
                int pos = dst.position();
                int lim = dst.limit();
                int rem = pos <= lim ? lim - pos : 0;

                if (dst is DirectBuffer dir)
                {
                    var tmp = new byte[rem];
                    await stream.ReadAsync(tmp, 0, rem, cancellationToken);
                    dst.put(tmp);
                }
                else
                {
                    await stream.ReadAsync(dst.array(), dst.arrayOffset() + pos, rem, cancellationToken);
                }
#else
                int pos = dst.position();
                int lim = dst.limit();
                int rem = pos <= lim ? lim - pos : 0;

                if (dst is DirectBuffer dir)
                {
                    var tmp = new byte[rem];
                    await stream.ReadAsync(tmp, 0, rem, cancellationToken);
                    dst.put(tmp);
                }
                else
                {
                    await stream.ReadAsync(dst.array(), dst.arrayOffset() + pos, rem, cancellationToken);
                }
#endif

                throw new NotImplementedException();
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
