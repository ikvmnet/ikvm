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
                new ConstantRecord[]
                {
                    //null,
                    //new ClassConstantRecord(1),
                    new Utf8ConstantRecord(new byte[] { 100, 200, 50, 70 })
                },
                AccessFlag.ACC_PUBLIC | AccessFlag.ACC_FINAL,
                2,
                3,
                new InterfaceRecord[]
                {
                    new InterfaceRecord(4),
                    new InterfaceRecord(5)
                },
                new FieldRecord[]
                {
                    new FieldRecord(AccessFlag.ACC_PUBLIC, 6, 7, new AttributeInfoRecord[0])
                },
                new MethodRecord[]
                {
                    new MethodRecord(AccessFlag.ACC_ABSTRACT, 8, 9, new AttributeInfoRecord[0])
                },
                new AttributeInfoRecord[]
                {
                    new AttributeInfoRecord(1, new byte[] { 1, 2, 3 })
                }
            );

            var b1 = new byte[a.GetSize()];
            var w1 = new ClassFormatWriter(b1);
            if (a.TryWrite(ref w1) == false)
                throw new Exception();

            var r = new ClassFormatReader(b1);
            if (ClassRecord.TryRead(ref r, out var b) == false)
                throw new Exception();

            var b2 = new byte[b.GetSize()];
            var w2 = new ClassFormatWriter(b2);
            if (b.TryWrite(ref w2) == false)
                throw new Exception();

            b1.Should().BeEquivalentTo(b2);
        }

    }

}
