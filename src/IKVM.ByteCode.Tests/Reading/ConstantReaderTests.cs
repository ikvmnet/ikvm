using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.ByteCode.Reading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests.Reading
{

    [TestClass]
    public class ConstantReaderTests
    {

        ClassReader ReadClass()
        {
            return ClassReader.Read(Path.Combine(Path.GetDirectoryName(typeof(ConstantReaderTests).Assembly.Location), "Reading", "ConstantReaderTests.class"));
        }

        [TestMethod]
        public void CanReadIntegerConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<IntegerConstantReader>().Should().Contain(i => i.Value == 394892);
        }

        [TestMethod]
        public void CanReadLongConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<LongConstantReader>().Should().Contain(i => i.Value == 34182132);
        }

        [TestMethod]
        public void CanReadFloatConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<FloatConstantReader>().Should().Contain(i => i.Value == 221.03f);
        }

        [TestMethod]
        public void CanReadDoubleConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<DoubleConstantReader>().Should().Contain(i => i.Value == 2212133.1d);
        }

        [TestMethod]
        public void CanReadStringConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<Utf8ConstantReader>().Should().Contain(i => i.Value == "STRING");
        }

        [TestMethod]
        public void CanReadClassConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<ClassConstantReader>().Should().Contain(i => i.Name.Value == "java/lang/Object");
        }

        [TestMethod]
        public void CanReadMethodrefConstant()
        {
            var c = ReadClass();
            c.Constants.OfType<MethodrefConstantReader>().Should().Contain(i => i.Class.Name.Value == "java/lang/Object" && i.NameAndType.Name.Value == "<init>" && i.NameAndType.Type.Value == "()V");
        }

    }

}
