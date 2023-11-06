using System;
using System.Drawing;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

using java.io;
using java.lang;
using java.nio;
using java.nio.channels;
using java.nio.file;
using java.util;
using java.util.concurrent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using sun.nio.ch;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class AsynchronousFileChannelTests
    {

        /// <summary>
        /// Tests that we fail correctly attempting to create a file that already exists.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException(typeof(FileAlreadyExistsException))]
        public void ShouldFailOnCreateNewExistingFile()
        {
            var f = new File("AsynchronousFileChannelTests_ShouldFailOnCreateExistingFile.txt");
            if (f.exists())
                f.delete();
            f.createNewFile();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.CREATE_NEW, StandardOpenOption.WRITE);
            c.close();
        }

        /// <summary>
        /// Tests that we can overwrite an existing file.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanCreateNewAndWrite()
        {
            var f = new File("AsynchronousFileChannelTests_CanCreateNewAndWrite.txt");
            if (f.exists())
                f.delete();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.CREATE_NEW, StandardOpenOption.WRITE);
            var b = ByteBuffer.allocate(1);
            b.put(1);
            b.flip();

            var h = new AwaitableCompletionHandler<Integer>();
            c.write(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            using var input = new FileInputStream(f);
            var inputBuf = new byte[1];
            input.read(inputBuf);
            inputBuf[0].Should().Be(1);
        }

        /// <summary>
        /// Tests that we can overwrite an existing file.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanCreateAndWrite()
        {
            var f = new File("AsynchronousFileChannelTests_CanCreateAndWrite.txt");
            if (f.exists())
                f.delete();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.CREATE, StandardOpenOption.WRITE);
            var b = ByteBuffer.allocate(1);
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.put(1);
            b.flip();

            var h = new AwaitableCompletionHandler<Integer>();
            c.write(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            using var input = new FileInputStream(f);
            var inputBuf = new byte[1];
            input.read(inputBuf);
            inputBuf[0].Should().Be(1);
        }

        /// <summary>
        /// Tests that we can overwrite an existing file.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanCreateAndWriteFromDirectBuffer()
        {
            var f = new File("AsynchronousFileChannelTests_CanCreateAndWriteFromDirectBuffer.txt");
            if (f.exists())
                f.delete();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.CREATE, StandardOpenOption.WRITE);
            var b = ByteBuffer.allocateDirect(1);
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.put(1);
            b.flip();

            var h = new AwaitableCompletionHandler<Integer>();
            c.write(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            using var input = new FileInputStream(f);
            var inputBuf = new byte[1];
            input.read(inputBuf);
            inputBuf[0].Should().Be(1);
        }

        /// <summary>
        /// Test that we can open in write/truncate, replace a file, and rewrite with a single byte.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanTruncateAndWrite()
        {
            var f = new File("AsynchronousFileChannelTests_CanTruncateAndWrite.txt");
            if (f.exists())
                f.delete();
            f.createNewFile();

            using var s = new FileOutputStream(f);
            s.write(new byte[] { 2 });
            s.close();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.WRITE, StandardOpenOption.TRUNCATE_EXISTING);
            var b = ByteBuffer.allocate(1);
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.put(1);
            b.flip();

            var h = new AwaitableCompletionHandler<Integer>();
            c.write(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            using var input = new FileInputStream(f);
            var inputBuf = new byte[1];
            input.read(inputBuf);
            inputBuf[0].Should().Be(1);
            input.close();
        }

        /// <summary>
        /// Tests that we can open in read mode and read a single byte.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanRead()
        {
            var f = new File("AsynchronousFileChannelTests_CanRead.txt");
            if (f.exists())
                f.delete();

            f.createNewFile();
            using var s = new FileOutputStream(f);
            s.write(new byte[] { 1 });
            s.close();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.READ);
            var b = ByteBuffer.allocate(1);
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);

            var h = new AwaitableCompletionHandler<Integer>();
            c.read(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            b.flip();
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.get().Should().Be(1);
        }

        /// <summary>
        /// Tests that we can open in read mode and read a single byte.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanReadIntoDirectBuffer()
        {
            var f = new File("AsynchronousFileChannelTests_CanReadIntoDirectBuffer.txt");
            if (f.exists())
                f.delete();

            f.createNewFile();
            using var s = new FileOutputStream(f);
            s.write(new byte[] { 1 });
            s.close();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.READ);
            var b = ByteBuffer.allocateDirect(1);
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.isDirect().Should().Be(true);

            var h = new AwaitableCompletionHandler<Integer>();
            c.read(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(1);
            c.close();

            b.flip();
            b.capacity().Should().Be(1);
            b.position().Should().Be(0);
            b.limit().Should().Be(1);
            b.get().Should().Be(1);

            var r = await Task.Run(() =>
            {
                unsafe
                {
                    var d = (DirectBuffer)b;
                    var span = new Span<byte>((byte*)(IntPtr)d.address(), b.capacity());
                    return span[0] == 1;
                }
            });

            r.Should().BeTrue();
        }

        /// <summary>
        /// Tests that we can open in read mode and read a single byte.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanLock()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var f = new File("AsynchronousFileChannelTests_CanLock.txt");
            if (f.exists())
                f.delete();

            f.createNewFile();
            using var s = new FileOutputStream(f);
            s.write(new byte[] { 1, 2 });
            s.close();

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.WRITE);

            var h = new AwaitableCompletionHandler<FileLock>();
            c.@lock(0, 2, false, null, h);
            var l = await h;

            l.position().Should().Be(0);
            l.size().Should().Be(2);
            l.release();
            c.close();
        }

        [TestMethod]
        public void TryLockShouldThrowOverlappingFileLockException()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var file = File.createTempFile("lockfile", null);
            file.deleteOnExit();
            if (file.exists())
                file.delete();

            file.createNewFile();
            using var s = new FileOutputStream(file);
            s.write(new byte[] { 1, 2, 3, 4 });
            s.close();

            var ch = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.READ, StandardOpenOption.WRITE);
            ch.Should().NotBeNull();

            var fl = (FileLock)ch.@lock().get();
            fl.Should().NotBeNull();
            fl.acquiredBy().Should().BeSameAs(ch);

            ch.Invoking(x => x.tryLock()).Should().Throw<OverlappingFileLockException>();

            ch.close();
            fl.isValid().Should().BeFalse();
        }

        [TestMethod]
        public void LockShouldThrowOverlappingFileLockException()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            var file = File.createTempFile("lockfile", null);
            file.deleteOnExit();
            if (file.exists())
                file.delete();

            file.createNewFile();
            using var s = new FileOutputStream(file);
            s.write(new byte[] { 1, 2, 3, 4 });
            s.close();

            var ch = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.READ, StandardOpenOption.WRITE);
            ch.Should().NotBeNull();

            var fl = ch.tryLock();
            fl.Should().NotBeNull();
            fl.acquiredBy().Should().BeSameAs(ch);

            var awaiter = new AwaitableCompletionHandler<FileLock>();
            ch.Invoking(x => x.@lock(null, awaiter)).Should().Throw<OverlappingFileLockException>();

            ch.close();
            fl.isValid().Should().BeFalse();
        }

        /// <summary>
        /// Creates threads with the name set.
        /// </summary>
        class NamedThreadFactory : ThreadFactory
        {

            readonly string name;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public NamedThreadFactory(string name)
            {
                this.name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public Thread newThread(Runnable r)
            {
                var t = new Thread(r, name);
                t.setDaemon(true);
                return t;
            }

        }

        /// <summary>
        /// Ensures that completions of the channel execute on the specified thread pool.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ShouldExecuteOnThreadPool()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return;

            Thread.currentThread().getName().Should().NotBe("ShouldExecuteOnThreadPool");
            var thisThread = Thread.currentThread();

            var f = new File("AsynchronousFileChannelTests_ShouldExecuteOnThreadPool.txt");
            if (f.exists())
                f.delete();

            f.createNewFile();
            using var s = new FileOutputStream(f);
            s.write(new byte[] { 1 });
            s.close();

            var t = Executors.newSingleThreadExecutor(new NamedThreadFactory("ShouldExecuteOnThreadPool"));
            using var c = AsynchronousFileChannel.open(f.toPath(), Collections.singleton(StandardOpenOption.READ), t);
            var b = ByteBuffer.allocate(1);

            var h = new AwaitableCompletionHandler<Integer>();
            c.read(b, 0, null, h);
            var n = await h;

            // should resume execution either on the same thread (synchronous) or on a thread pool thread
            if (Thread.currentThread() != thisThread)
                Thread.currentThread().getName().Should().Be("ShouldExecuteOnThreadPool");

            n.intValue().Should().Be(1);
            c.close();

            b.flip();
            b.get().Should().Be(1);
        }

        /// <summary>
        /// Check that the asynchronous APIs can read from the VFS.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CanReadVfsAssemblyClass()
        {
            var f = new File(System.IO.Path.Combine(VfsTable.GetAssemblyClassesPath(JVM.Vfs.Context, typeof(global::java.lang.Object).Assembly, JVM.Properties.HomePath), "java", "lang", "Object.class"));

            using var c = AsynchronousFileChannel.open(f.toPath(), StandardOpenOption.READ);
            var b = ByteBuffer.allocate(512);
            b.capacity().Should().Be(512);
            b.position().Should().Be(0);
            b.limit().Should().Be(512);

            var h = new AwaitableCompletionHandler<Integer>();
            c.read(b, 0, null, h);
            var n = await h;
            n.intValue().Should().Be(512);
            c.close();

            b.position().Should().Be(512);
            b.flip();
            b.capacity().Should().Be(512);
            b.position().Should().Be(0);
            b.limit().Should().Be(512);
            b.order(ByteOrder.BIG_ENDIAN);
            var m = b.getInt();
            m.Should().Be(unchecked((int)0xCAFEBABE));
        }

        [TestMethod]
        public void ShouldThrowClosedChannelExceptionOnRead()
        {
            var file = File.createTempFile("test", null);
            file.deleteOnExit();
            if (file.exists())
                file.delete();

            file.createNewFile();
            using var s = new FileOutputStream(file);
            s.write(new byte[] { 1, 2, 3, 4 });
            s.close();

            using var c = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.READ);
            c.close();
            c.isOpen().Should().BeFalse();

            var buf = ByteBuffer.allocateDirect(4);
            c.Invoking(x => x.read(buf, 0L).get()).Should().Throw<ExecutionException>().Where(e => e.getCause() is ClosedChannelException);
        }

        [TestMethod]
        public void ShouldThrowClosedChannelExceptionOnWrite()
        {
            var file = File.createTempFile("test", null);
            file.deleteOnExit();
            if (file.exists())
                file.delete();

            file.createNewFile();
            using var s = new FileOutputStream(file);
            s.write(new byte[] { 1, 2, 3, 4 });
            s.close();

            using var c = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.WRITE);
            c.close();
            c.isOpen().Should().BeFalse();

            var buf = ByteBuffer.allocateDirect(4);
            c.Invoking(x => x.write(buf, 0L).get()).Should().Throw<ExecutionException>().Where(e => e.getCause() is ClosedChannelException);
        }

        [TestMethod]
        public void ShouldThrowClosedChannelExceptionOnLock()
        {
            var file = File.createTempFile("test", null);
            file.deleteOnExit();
            if (file.exists())
                file.delete();

            file.createNewFile();
            using var s = new FileOutputStream(file);
            s.write(new byte[] { 1, 2, 3, 4 });
            s.close();

            using var c = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.READ, StandardOpenOption.WRITE);
            c.close();
            c.isOpen().Should().BeFalse();

            c.Invoking(x => x.@lock().get()).Should().Throw<ExecutionException>().Where(e => e.getCause() is ClosedChannelException);
        }

        [TestMethod]
        public void ShouldThrowAsynchronousCloseExceptionWhenClosedDuringWrite()
        {
            var rng = RandomNumberGenerator.Create();
            var rnd = new System.Random();

            const int size = 1024 * 1024 * 64;
            const int z = 8;

            var file = File.createTempFile("test", null);
            file.deleteOnExit();

            // rewrite file with random data
            System.IO.File.Create(file.getPath()).Close();

            using var c = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.WRITE, StandardOpenOption.SYNC);

            // generate N buffers to read data from
            var buf = new ByteBuffer[z];
            var pos = new int[z];
            for (int i = 0; i < z; i++)
            {
                var d = ByteBuffer.allocateDirect(size);
                var b = new byte[size];
                rng.GetBytes(b);
                d.put(b);
                d.flip();
                buf[i] = d;
                pos[i] = rnd.Next(1, size);
            }

            // start N async write requests
            var result = new Future[z];
            for (int i = 0; i < z; i++)
                result[i] = c.write(buf[i], pos[i]);

            // close channel while writing is ongoing
            c.close();

            // write operations should complete or fail with AsynchronousCloseException
            for (int i = 0; i < z; i++)
            {
                try
                {
                    result[i].get().Should().NotBeNull();
                }
                catch (ExecutionException e) when (e.getCause() is AsynchronousCloseException)
                {
                    // expected
                }
            }
        }

        [TestMethod]
        public void CanCancelDuringWrite()
        {
            var rng = RandomNumberGenerator.Create();
            var rnd = new System.Random();

            const int size = 1024 * 1024 * 64;
            const int z = 8;

            var file = File.createTempFile("test", null);
            file.deleteOnExit();

            // rewrite file with random data
            System.IO.File.Create(file.getPath()).Close();

            using var c = AsynchronousFileChannel.open(file.toPath(), StandardOpenOption.WRITE, StandardOpenOption.SYNC);

            // generate N buffers to read data from
            var buf = new ByteBuffer[z];
            var pos = new int[z];
            for (int i = 0; i < z; i++)
            {
                var d = ByteBuffer.allocateDirect(size);
                var b = new byte[size];
                rng.GetBytes(b);
                d.put(b);
                d.flip();
                buf[i] = d;
                pos[i] = rnd.Next(1, size);
            }

            // start N async write requests
            var result = new Future[z];
            for (int i = 0; i < z; i++)
                result[i] = c.write(buf[i], pos[i]);

            // cancel channel while writing is ongoing
            var cancelled = new bool[z];
            for (int i = 0; i < z; i++)
                cancelled[i] = result[i].cancel(false);

            for (int i = 0; i < z; i++)
            {
                result[i].isDone().Should().BeTrue();
                result[i].isCancelled().Should().Be(cancelled[i]);

                try
                {
                    result[i].get();
                    cancelled[i].Should().BeFalse(); // if we didn't throw, we weren't cancelled
                }
                catch (CancellationException)
                {
                    cancelled[i].Should().BeTrue(); // we did throw, so we should be cancelled
                }
            }
        }

    }

}