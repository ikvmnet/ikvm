using java.io;
using java.nio;
using java.nio.channels;
using java.nio.file;
using java.util;

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

    }

}
