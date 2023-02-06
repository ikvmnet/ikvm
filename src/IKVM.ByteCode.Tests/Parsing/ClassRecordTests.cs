using System;

using FluentAssertions;

using IKVM.ByteCode.Parsing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests.Parsing
{

    [TestClass]
    public class ClassRecordTests
    {

        [TestMethod]
        public void CanRoundTripClass()
        {
            var a = new ClassRecord(
                1,
                52,
                new ConstantRecord[0],
                AccessFlag.ACC_PUBLIC | AccessFlag.ACC_FINAL,
                1,
                2,
                new InterfaceRecord[0],
                new FieldRecord[0],
                new MethodRecord[0],
                new AttributeInfoRecord[0]);

            var b1 = new byte[a.GetSize()];
            var w1 = new ClassFormatWriter(b1);
            if (a.TryWrite(ref w1) == false)
                throw new Exception();

            var r = new ClassFormatReader(b1);
            if (ClassRecord.TryRead(ref r, out var b) == false)
                throw new Exception();

            var b2 = new byte[4096];
            var w2 = new ClassFormatWriter(b2);
            if (b.TryWrite(ref w2) == false)
                throw new Exception();

            b1.Should().BeEquivalentTo(b2);
        }

    }

}
