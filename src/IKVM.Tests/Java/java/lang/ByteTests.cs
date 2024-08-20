using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang
{

    [TestClass]
    public class ByteTests
    {

        [TestMethod]
        public void TestMinMax()
        {
            unchecked((sbyte)global::java.lang.Byte.MIN_VALUE).Should().Be(-128);
            unchecked((sbyte)global::java.lang.Byte.MAX_VALUE).Should().Be(127);
        }

        [TestMethod]
        public void TestMinMaxReflection()
        {
            ((sbyte)((global::java.lang.Class)typeof(global::java.lang.Byte)).getField("MIN_VALUE").getByte(null)).Should().Be(-128);
            ((sbyte)((global::java.lang.Class)typeof(global::java.lang.Byte)).getField("MAX_VALUE").getByte(null)).Should().Be(127);
        }

        [TestMethod]
        public void TestStubFile()
        {
            using var res = ((global::java.lang.Class)typeof(global::java.lang.Byte)).getResourceAsStream("Byte.class");
            var mem = new MemoryStream();
            var buf = new byte[1024];
            var len = 0;
            while ((len = res.read(buf)) > 0)
                mem.Write(buf, 0, len);

            mem.Position = 0;

            using var cls = ClassFile.Read(mem);

            foreach (var field in cls.Fields)
            {
                if (cls.Constants.Get(field.Name).Value == "MIN_VALUE")
                {
                    field.AccessFlags.Should().HaveFlag(AccessFlag.Static);
                    field.AccessFlags.Should().HaveFlag(AccessFlag.Final);
                    cls.Constants.Get(field.Descriptor).Value.Should().Be("B");
                    var c = field.Attributes.First(i => cls.Constants.Get(i.Name).Value == AttributeName.ConstantValue);
                    var h = ((ConstantValueAttribute)c).Value;
                    var z = cls.Constants.Get(h);
                    z.Kind.Should().Be(ConstantKind.Integer);
                    ((IntegerConstant)z).Value.Should().Be(-128);
                    continue;
                }

                if (cls.Constants.Get(field.Name).Value == "MAX_VALUE")
                {
                    field.AccessFlags.Should().HaveFlag(AccessFlag.Static);
                    field.AccessFlags.Should().HaveFlag(AccessFlag.Final);
                    cls.Constants.Get(field.Descriptor).Value.Should().Be("B");
                    var c = field.Attributes.First(i => cls.Constants.Get(i.Name).Value == AttributeName.ConstantValue);
                    var h = ((ConstantValueAttribute)c).Value;
                    var z = cls.Constants.Get(h);
                    z.Kind.Should().Be(ConstantKind.Integer);
                    ((IntegerConstant)z).Value.Should().Be(127);
                    continue;
                }
            }
        }

    }

}
