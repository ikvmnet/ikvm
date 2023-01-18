using System.Linq;

using FluentAssertions;

using IKVM.ByteCode.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests.Text
{

    [TestClass]
    public class MUTF8EncodingTests
    {

        string asciiChars = new string(Enumerable.Range(32, 126 - 32).Select(i => (char)i).ToArray());

        [TestMethod]
        public void CanRoundTripASCIICharactersForJDK_1_4()
        {
            var e = MUTF8Encoding.GetMUTF8(48);
            var b = e.GetBytes(asciiChars);
            var c = e.GetString(b);
            c.Should().Be(asciiChars);
        }

        [TestMethod]
        public void CanRoundTripASCIICharactersForJDK_1_0()
        {
            var e = MUTF8Encoding.GetMUTF8(46);
            var b = e.GetBytes(asciiChars);
            var c = e.GetString(b);
            c.Should().Be(asciiChars);
        }

        [TestMethod]
        public void CanEncodeNullByte()
        {
            var l = MUTF8Encoding.GetMUTF8(48).GetByteCount("\0");
            l.Should().Be(2);
            var b = MUTF8Encoding.GetMUTF8(48).GetBytes("\0");
            b.Should().HaveCount(2);
            b[0].Should().Be(0b11000000);
            b[1].Should().Be(0b10000000);
        }

        [TestMethod]
        public void CanDecodeNullByte()
        {
            var s = MUTF8Encoding.GetMUTF8(48).GetChars(new byte[] { 0b11000000, 0b10000000 });
            s.Should().HaveCount(1);
            s[0].Should().Be('\0');
        }

    }

}
