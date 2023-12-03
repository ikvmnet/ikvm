using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Xml.Linq;

using FluentAssertions;

using java.beans;
using java.io;
using java.lang;
using java.nio.charset;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.beans
{

    [TestClass]
    public class XMLEncoderTests
    {

        /// <summary>
        /// Do each test in a class that implements <see cref="ExceptionListener"/>.
        /// </summary>
        class TestEncodeAndDecode : ExceptionListener
        {

            readonly string encoding;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="encoding"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public TestEncodeAndDecode(string encoding)
            {
                this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            }

            /// <summary>
            /// Generate new test string.
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            string CreateTestString(int length)
            {
                var b = new System.Text.StringBuilder(length);
                while (0 < length--)
                    b.Append((char)length);

                return b.ToString();
            }

            /// <summary>
            /// Encodes the specified string to XML data.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            byte[] Encode(string text)
            {
                var dst = new ByteArrayOutputStream();
                var enc = new XMLEncoder(dst, encoding, true, 0);
                enc.setExceptionListener(this);
                enc.writeObject(text);
                enc.close();
                return dst.toByteArray();
            }

            /// <summary>
            /// Decodes the specified string data from XML.
            /// </summary>
            /// <param name="buf"></param>
            /// <returns></returns>
            string Decode(byte[] buf)
            {
                var src = new ByteArrayInputStream(buf);
                var dec = new XMLDecoder(src);
                dec.setExceptionListener(this);
                var res = dec.readObject() as string;
                return res;
            }

            /// <summary>
            /// Runs the test.
            /// </summary>
            /// <exception cref="Error"></exception>
            public void Run()
            {
                try
                {
                    var t = CreateTestString(0x10000);
                    var b = Encode(t);
                    var r = Decode(b);
                    r.Should().Be(t);
                }
                catch (IllegalCharsetNameException)
                {
                    throw;
                }
                catch (UnsupportedCharsetException)
                {
                    throw;
                }
                catch (UnsupportedOperationException)
                {
                    throw;
                }
            }

            public void exceptionThrown(global::java.lang.Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
            }

        }

        /// <summary>
        /// Runs an XML decode and encode operation over a generated string. Duplicated from Test4625418.
        /// </summary>
        /// <param name="encoding"></param>
        [DataTestMethod]
        [DataRow("ASCII")]
        [DataRow("Big5")]
        [DataRow("Big5_Solaris")]
        [DataRow("Cp1006")]
        [DataRow("Cp1046")]
        [DataRow("Cp1047")]
        [DataRow("Cp1097")]
        [DataRow("Cp1098")]
        [DataRow("Cp1250")]
        [DataRow("Cp1251")]
        [DataRow("Cp1252")]
        [DataRow("Cp1253")]
        [DataRow("Cp1254")]
        [DataRow("Cp1255")]
        [DataRow("Cp1256")]
        [DataRow("Cp1257")]
        [DataRow("Cp1258")]
        [DataRow("Cp437")]
        [DataRow("Cp737")]
        [DataRow("Cp775")]
        [DataRow("Cp850")]
        [DataRow("Cp852")]
        [DataRow("Cp855")]
        [DataRow("Cp856")]
        [DataRow("Cp857")]
        [DataRow("Cp858")]
        [DataRow("Cp860")]
        [DataRow("Cp861")]
        [DataRow("Cp862")]
        [DataRow("Cp863")]
        [DataRow("Cp864")]
        [DataRow("Cp865")]
        [DataRow("Cp866")]
        [DataRow("Cp868")]
        [DataRow("Cp869")]
        [DataRow("Cp874")]
        [DataRow("Cp921")]
        [DataRow("Cp922")]
        [DataRow("Cp933")]
        [DataRow("Cp943")]
        [DataRow("Cp948")]
        [DataRow("Cp949")]
        [DataRow("Cp950")]
        [DataRow("Cp964")]
        [DataRow("EUC-KR")]
        [DataRow("EUC_CN")]
        [DataRow("EUC_KR")]
        [DataRow("GB18030")]
        [DataRow("GB2312")]
        [DataRow("GBK")]
        [DataRow("IBM00858")]
        [DataRow("IBM1047")]
        [DataRow("IBM437")]
        [DataRow("IBM775")]
        [DataRow("IBM850")]
        [DataRow("IBM852")]
        [DataRow("IBM855")]
        [DataRow("IBM857")]
        [DataRow("IBM860")]
        [DataRow("IBM861")]
        [DataRow("IBM862")]
        [DataRow("IBM863")]
        [DataRow("IBM864")]
        [DataRow("IBM865")]
        [DataRow("IBM866")]
        [DataRow("IBM868")]
        [DataRow("IBM869")]
        [DataRow("ISO-2022-JP")]
        [DataRow("ISO-2022-KR")]
        [DataRow("ISO-8859-1")]
        [DataRow("ISO-8859-13")]
        [DataRow("ISO-8859-15")]
        [DataRow("ISO-8859-2")]
        [DataRow("ISO-8859-3")]
        [DataRow("ISO-8859-4")]
        [DataRow("ISO-8859-5")]
        [DataRow("ISO-8859-6")]
        [DataRow("ISO-8859-7")]
        [DataRow("ISO-8859-8")]
        [DataRow("ISO-8859-9")]
        [DataRow("ISO2022JP")]
        [DataRow("ISO2022KR")]
        [DataRow("ISO8859_1")]
        [DataRow("ISO8859_13")]
        [DataRow("ISO8859_15")]
        [DataRow("ISO8859_2")]
        [DataRow("ISO8859_3")]
        [DataRow("ISO8859_4")]
        [DataRow("ISO8859_5")]
        [DataRow("ISO8859_6")]
        [DataRow("ISO8859_7")]
        [DataRow("ISO8859_8")]
        [DataRow("ISO8859_9")]
        [DataRow("KOI8-R")]
        [DataRow("KOI8-U")]
        [DataRow("KOI8_R")]
        [DataRow("KOI8_U")]
        [DataRow("MS874")]
        [DataRow("MS949")]
        [DataRow("MS950")]
        [DataRow("MacArabic")]
        [DataRow("MacCentralEurope")]
        [DataRow("MacCroatian")]
        [DataRow("MacCyrillic")]
        [DataRow("MacGreek")]
        [DataRow("MacHebrew")]
        [DataRow("MacIceland")]
        [DataRow("MacRoman")]
        [DataRow("MacRomania")]
        [DataRow("MacThai")]
        [DataRow("MacTurkish")]
        [DataRow("MacUkraine")]
        [DataRow("TIS-620")]
        [DataRow("TIS620")]
        [DataRow("US-ASCII")]
        [DataRow("UTF-16")]
        [DataRow("UTF-16BE")]
        [DataRow("UTF-16LE")]
        [DataRow("UTF-32")]
        [DataRow("UTF-32BE")]
        [DataRow("UTF-32LE")]
        [DataRow("UTF-8")]
        [DataRow("UTF8")]
        [DataRow("UTF_32")]
        [DataRow("UTF_32BE")]
        [DataRow("UTF_32LE")]
        [DataRow("UnicodeBig")]
        [DataRow("UnicodeBigUnmarked")]
        [DataRow("UnicodeLittle")]
        [DataRow("UnicodeLittleUnmarked")]
        [DataRow("windows-1250")]
        [DataRow("windows-1251")]
        [DataRow("windows-1252")]
        [DataRow("windows-1253")]
        [DataRow("windows-1254")]
        [DataRow("windows-1255")]
        [DataRow("windows-1256")]
        [DataRow("windows-1257")]
        [DataRow("windows-1258")]
        [DataRow("x-IBM1006")]
        [DataRow("x-IBM1046")]
        [DataRow("x-IBM1097")]
        [DataRow("x-IBM1098")]
        [DataRow("x-IBM1124")]
        [DataRow("x-IBM737")]
        [DataRow("x-IBM856")]
        [DataRow("x-IBM874")]
        [DataRow("x-IBM921")]
        [DataRow("x-IBM922")]
        [DataRow("x-IBM933")]
        [DataRow("x-IBM943")]
        [DataRow("x-IBM948")]
        [DataRow("x-IBM949")]
        [DataRow("x-IBM950")]
        [DataRow("x-IBM964")]
        [DataRow("x-Johab")]
        [DataRow("x-MacArabic")]
        [DataRow("x-MacCentralEurope")]
        [DataRow("x-MacCroatian")]
        [DataRow("x-MacCyrillic")]
        [DataRow("x-MacGreek")]
        [DataRow("x-MacHebrew")]
        [DataRow("x-MacIceland")]
        [DataRow("x-MacRoman")]
        [DataRow("x-MacRomania")]
        [DataRow("x-MacThai")]
        [DataRow("x-MacTurkish")]
        [DataRow("x-MacUkraine")]
        [DataRow("x-UTF-16LE-BOM")]
        [DataRow("x-iso-8859-11")]
        [DataRow("x-mswin-936")]
        [DataRow("x-windows-874")]
        [DataRow("x-windows-949")]
        [DataRow("x-windows-950")]
        public void CanEncodeAndDecode(string encoding)
        {
            new TestEncodeAndDecode(encoding).Run();
        }

    }

}
