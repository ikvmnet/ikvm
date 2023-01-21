using System;

using FluentAssertions;

using IKVM.ByteCode.Parsing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests.Parsing
{

    [TestClass]
    public class TypeAnnotationRecordTests
    {

        [TestMethod]
        public void CanRoundTripTypeAnnotation()
        {
            var a = new TypeAnnotationRecord(
                TypeAnnotationTargetType.Field,
                new TypeAnnotationEmptyTargetRecord(),
                new TypePathRecord(new TypePathItemRecord(TypePathKind.ArrayType, 0)),
                1,
                new ElementValuePairRecord(2, new ElementValueRecord(ElementValueTag.Integer, new ElementValueConstantValueRecord(3))));

            var b1 = new byte[a.GetSize()];
            var w1 = new ClassFormatWriter(b1);
            if (a.TryWrite(ref w1) == false)
                throw new Exception();

            var r = new ClassFormatReader(b1);
            if (TypeAnnotationRecord.TryReadTypeAnnotation(ref r, out var b) == false)
                throw new Exception();

            var b2 = new byte[b.GetSize()];
            var w2 = new ClassFormatWriter(b2);
            if (b.TryWrite(ref w2) == false)
                throw new Exception();

            b1.Should().BeEquivalentTo(b2);
        }

    }

}
