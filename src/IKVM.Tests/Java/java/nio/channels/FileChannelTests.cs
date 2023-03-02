using FluentAssertions;

using java.io;
using java.lang;
using java.nio;
using java.nio.channels;
using java.nio.charset;
using java.nio.file;
using java.util;

using jdk.nashorn.@internal.ir.debug;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.nio.channels
{

    [TestClass]
    public class FileChannelTests
    {

        [TestMethod]
        public void CanTruncateAndWrite()
        {
            var f = new File("a.txt");
            f.createNewFile();
            var o = new HashSet();
            o.add(StandardOpenOption.WRITE);
            o.add(StandardOpenOption.TRUNCATE_EXISTING);
            var c = FileChannel.open(f.toPath(), o);
            var b = ByteBuffer.allocate(1);
            b.put(1);
            b.flip();
            c.write(b);
            c.close();
        }

        [TestMethod]
        public void CanSetPosition()
        {
            var generator = new Random();
            var blah = Files.createTempFile("blah", null);
            blah.toFile().deleteOnExit();

            // write lines of numbers
            using (var awriter = Files.newBufferedWriter(blah, Charset.forName("8859_1")))
            {
                for (int i = 0; i < 4000; i++)
                {
                    awriter.write(i.ToString().PadLeft(4, '0'));
                    awriter.newLine();
                }
            }

            for (int i = 0; i < 10; i++)
            {
                // try to open with either FileChannel.open or FileInputStream.getChannel
                using var fc = generator.nextBoolean() ? FileChannel.open(blah, StandardOpenOption.READ) : new FileInputStream(blah.toFile()).getChannel();

                for (int j = 0; j < 100; j++)
                {
                    var newPos = generator.nextInt(1000);
                    fc.position(newPos);
                    fc.position().Should().Be(newPos);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                // try to open with either FileChannel.open or FileOutputStream.getChannel
                using var fc = generator.nextBoolean() ? FileChannel.open(blah, StandardOpenOption.APPEND) : new FileOutputStream(blah.toFile(), true).getChannel();

                for (int j = 0; j < 10; j++)
                {
                    // append should set position to end of file
                    fc.position().Should().Be(fc.size());

                    // append some data, next round will be open to a new position
                    var buf = new byte[generator.nextInt(100)];
                    fc.write(ByteBuffer.wrap(buf));
                }
            }

            Files.delete(blah);
        }

    }

}
