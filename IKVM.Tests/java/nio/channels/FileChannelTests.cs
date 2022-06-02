using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.java.nio.channels
{

    [TestClass]
    public class FileChannelTests
    {

        [TestMethod]
        public void Can_open_filechannel_write()
        {
            var f = new global::java.io.File(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            f.createNewFile();
            var o = new global::java.util.HashSet();
            o.add(global::java.nio.file.StandardOpenOption.WRITE);
            o.add(global::java.nio.file.StandardOpenOption.TRUNCATE_EXISTING);
            var c = global::java.nio.channels.FileChannel.open(f.toPath(), o);
            c.close();
        }

    }

}
