using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.util.zip
{

    [TestClass]
    public class ZipTests
    {

        [TestMethod]
        public void TotalInOut()
        {

            const int BUF_SIZE = 1 * 1024 * 1024;
            long dataSize = 128L * 1024L * 1024L;      // 128MB

            var deflater = new global::java.util.zip.Deflater();
            var inflater = new global::java.util.zip.Inflater();

            var dataIn = new byte[BUF_SIZE];
            var dataOut = new byte[BUF_SIZE];
            var tmp = new byte[BUF_SIZE];

            var r = new global::java.util.Random();
            r.nextBytes(dataIn);
            long bytesReadDef = 0;
            long bytesWrittenDef = 0;
            long bytesReadInf = 0;
            long bytesWrittenInf = 0;

            deflater.setInput(dataIn, 0, dataIn.Length);
            while (bytesReadDef < dataSize || bytesWrittenInf < dataSize)
            {
                int len = r.nextInt(BUF_SIZE / 2) + BUF_SIZE / 2;
                if (deflater.needsInput())
                {
                    bytesReadDef += dataIn.Length;
                    bytesReadDef.Should().Be(deflater.getBytesRead());
                    deflater.setInput(dataIn, 0, dataIn.Length);
                }
                int n = deflater.deflate(tmp, 0, len);
                bytesWrittenDef += n;
                bytesWrittenDef.Should().Be(deflater.getBytesWritten());

                inflater.setInput(tmp, 0, n);
                bytesReadInf += n;
                while (!inflater.needsInput())
                {
                    bytesWrittenInf += inflater.inflate(dataOut, 0, dataOut.Length);
                    bytesWrittenInf.Should().Be(inflater.getBytesWritten());
                }
                bytesReadInf.Should().Be(inflater.getBytesRead());
            }
        }

    }

}
