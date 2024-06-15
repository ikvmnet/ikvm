using FluentAssertions;

using java.nio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio
{

    [TestClass]
    public class ByteBufferTests
    {

        /// <summary>
        /// Predictably hashes the given integer.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        static long Value(int i)
        {
            int j = i % 54;
            return j + 'a' + ((j > 26) ? 128 : 0);
        }

        /// <summary>
        /// Initializes the given buffer.
        /// </summary>
        /// <param name="b"></param>
        static void Init(ByteBuffer b)
        {
            int n = b.capacity();
            b.clear();
            for (int i = 0; i < n; i++)
                b.put(i, (byte)Value(i));
            b.limit(n);
            b.position(0);
        }

        /// <summary>
        /// Initializes the given array.
        /// </summary>
        /// <param name="a"></param>
        static void Init(byte[] a)
        {
            for (int i = 0; i < a.Length; i++)
                a[i] = (byte)Value(i + 1);
        }

        [TestMethod]
        public void CanCopyMemory()
        {
            var b = ByteBuffer.allocate(1024 * 1024 + 1024);
            var a = new byte[b.capacity()];

            Init(b);
            Init(a);

            // copyFromByteArray (a -> b)
            b.put(a);
            for (int i = 0; i < a.Length; i++)
                b.get(i).Should().Be((byte)Value(i + 1));

            // copyToByteArray (b -> a)
            Init(b);
            Init(a);
            b.get(a);
            for (int i = 0; i < a.Length; i++)
                a[i].Should().Be(b.get(i));
        }

        [TestMethod]
        public void CanCopyDirectMemory()
        {
            var b = ByteBuffer.allocateDirect(1024 * 1024 + 1024);
            var a = new byte[b.capacity()];

            Init(b);
            Init(a);

            // copyFromByteArray (a -> b)
            b.put(a);
            for (int i = 0; i < a.Length; i++)
                b.get(i).Should().Be((byte)Value(i + 1));

            // copyToByteArray (b -> a)
            Init(b);
            Init(a);
            b.get(a);
            for (int i = 0; i < a.Length; i++)
                a[i].Should().Be(b.get(i));
        }

    }

}
