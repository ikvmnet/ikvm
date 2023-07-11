using System.Collections.Generic;
using System.Reflection;

using FluentAssertions;

using IKVM.Compiler.Managed;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed
{

    [TestClass]
    public class ManagedSignatureDataTests
    {

        [TestMethod]
        public void CanCreatePrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            primitiveType.Length.Should().Be(1);
            primitiveType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            primitiveType.Local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
        }

        [TestMethod]
        public void CanCreateType()
        {
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyName, typeName);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);
            type.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            type.Local0.Data.Type_Type.Value.AssemblyName.Should().Be(assemblyName);
            type.Local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);
        }

        [TestMethod]
        public void CanCreateSZArrayOfPrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            primitiveType.Length.Should().Be(1);
            primitiveType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            primitiveType.Local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), primitiveType, null, null);
            arrayType.Length.Should().Be(2);
            arrayType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            arrayType.Local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
            arrayType.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
        }

        [TestMethod]
        public void CanCreateSZArrayOfType()
        {
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyName, typeName);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);
            type.Length.Should().Be(1);
            type.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            type.Local0.Data.Type_Type.Value.AssemblyName.Should().Be(assemblyName);
            type.Local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), type, null, null);
            arrayType.Length.Should().Be(2);
            arrayType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            arrayType.Local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);
            arrayType.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
        }

        [TestMethod]
        public void CanCreateArrayOfPrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(1, new[] { 1 }, new int[] { })), primitiveType, null, null);
            arrayType.Length.Should().Be(2);
            arrayType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            arrayType.Local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
            arrayType.Local1.Data.Kind.Should().Be(ManagedSignatureKind.Array);
            arrayType.Local1.Data.Array_Shape.Should().NotBeNull();
            arrayType.Local1.Data.Array_Shape.Value.Rank.Should().Be(1);
            arrayType.Local1.Data.Array_Shape.Value.GetSize(0).Should().Be(1);
            arrayType.Local1.Data.Array_Shape.Value.GetLowerBound(0).Should().BeNull();
        }

        [TestMethod]
        public void CanCreateArrayOfType()
        {
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyName, typeName);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(1, new int[] { 1 }, new int[] { })), type, null, null);
            arrayType.Length.Should().Be(2);
            arrayType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            arrayType.Local0.Data.Type_Type.Should().Be(typeRef);
            arrayType.Local1.Data.Kind.Should().Be(ManagedSignatureKind.Array);
            arrayType.Local1.Data.Array_Shape.Should().NotBeNull();
            arrayType.Local1.Data.Array_Shape.Value.Rank.Should().Be(1);
            arrayType.Local1.Data.Array_Shape.Value.GetSize(0).Should().Be(1);
            arrayType.Local1.Data.Array_Shape.Value.GetLowerBound(0).Should().BeNull();
        }

        [TestMethod]
        public void CanPackMemory()
        {
            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            local0.Length.Should().Be(1);
            local0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local0.Local0.Arg0.Should().Be(0);
            local0.Local0.Argv.Count.Should().Be(0);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local0, null, null);
            local1.Length.Should().Be(2);
            local1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local1.Local0.Arg0.Should().Be(0);
            local1.Local0.Argv.Count.Should().Be(0);
            local1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local1.Local1.Arg0.Should().Be(-1);
            local1.Local1.Argv.Count.Should().Be(0);

            var local2 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local1, null, null);
            local2.Length.Should().Be(3);
            local2.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local2.Local0.Arg0.Should().Be(0);
            local2.Local0.Argv.Count.Should().Be(0);
            local2.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local2.Local1.Arg0.Should().Be(-1);
            local2.Local1.Argv.Count.Should().Be(0);
            local2.Local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local2.Local2.Arg0.Should().Be(-1);
            local2.Local2.Argv.Count.Should().Be(0);

            var local3 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local2, null, null);
            local3.Length.Should().Be(4);
            local3.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local3.Local0.Arg0.Should().Be(0);
            local3.Local0.Argv.Count.Should().Be(0);
            local3.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.Local1.Arg0.Should().Be(-1);
            local3.Local1.Argv.Count.Should().Be(0);
            local3.Local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.Local2.Arg0.Should().Be(-1);
            local3.Local2.Argv.Count.Should().Be(0);
            local3.Local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.Local3.Arg0.Should().Be(-1);
            local3.Local3.Argv.Count.Should().Be(0);

            var mem0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local3, null, null);
            mem0.Length.Should().Be(5);
            mem0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            mem0.Local0.Arg0.Should().Be(0);
            mem0.Local0.Argv.Count.Should().Be(0);
            mem0.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.Local1.Arg0.Should().Be(-1);
            mem0.Local1.Argv.Count.Should().Be(0);
            mem0.Local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.Local2.Arg0.Should().Be(-1);
            mem0.Local2.Argv.Count.Should().Be(0);
            mem0.Local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.Local3.Arg0.Should().Be(-1);
            mem0.Local3.Argv.Count.Should().Be(0);
            mem0.Memory.Count.Should().Be(1);
            mem0.Memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.Memory[0].Span[0].Arg0.Should().Be(-1);
            mem0.Memory[0].Span[0].Argv.Count.Should().Be(0);

            var mem1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem0, null, null);
            mem1.Length.Should().Be(6);
            mem1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            mem1.Local0.Arg0.Should().Be(0);
            mem1.Local0.Argv.Count.Should().Be(0);
            mem1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.Local1.Arg0.Should().Be(-1);
            mem1.Local1.Argv.Count.Should().Be(0);
            mem1.Local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.Local2.Arg0.Should().Be(-1);
            mem1.Local2.Argv.Count.Should().Be(0);
            mem1.Local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.Local3.Arg0.Should().Be(-1);
            mem1.Local3.Argv.Count.Should().Be(0);
            mem1.Memory.Count.Should().Be(2);
            mem1.Memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.Memory[0].Span[0].Arg0.Should().Be(-1);
            mem1.Memory[0].Span[0].Argv.Count.Should().Be(0);
            mem1.Memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.Memory[1].Span[0].Arg0.Should().Be(-1);
            mem1.Memory[1].Span[0].Argv.Count.Should().Be(0);

            var mem2 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem1, null, null);
            mem2.Length.Should().Be(7);
            mem2.Memory.Count.Should().Be(3);
            mem2.Memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.Memory[0].Span[0].Arg0.Should().Be(-1);
            mem2.Memory[0].Span[0].Argv.Count.Should().Be(0);
            mem2.Memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.Memory[1].Span[0].Arg0.Should().Be(-1);
            mem2.Memory[1].Span[0].Argv.Count.Should().Be(0);
            mem2.Memory[2].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.Memory[2].Span[0].Arg0.Should().Be(-1);
            mem2.Memory[2].Span[0].Argv.Count.Should().Be(0);

            var mem3 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem2, null, null);
            mem3.Length.Should().Be(8);
            mem3.Memory.Count.Should().Be(4);
            mem3.Memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.Memory[0].Span[0].Arg0.Should().Be(-1);
            mem3.Memory[0].Span[0].Argv.Count.Should().Be(0);
            mem3.Memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.Memory[1].Span[0].Arg0.Should().Be(-1);
            mem3.Memory[1].Span[0].Argv.Count.Should().Be(0);
            mem3.Memory[2].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.Memory[2].Span[0].Arg0.Should().Be(-1);
            mem3.Memory[2].Span[0].Argv.Count.Should().Be(0);
            mem3.Memory[3].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.Memory[3].Span[0].Arg0.Should().Be(-1);
            mem3.Memory[3].Span[0].Argv.Count.Should().Be(0);

            var mem4 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem3, null, null);
            mem4.Length.Should().Be(9);
            mem4.Memory.Count.Should().Be(5);
            mem4.Memory[4].Span.Length.Should().Be(1);
            mem4.Memory[4].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem4.Memory[4].Span[0].Arg0.Should().Be(-1);
            mem4.Memory[4].Span[0].Argv.Count.Should().Be(0);

            var mem5 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem4, null, null);
            mem5.Length.Should().Be(10);
            mem5.Memory.Count.Should().Be(6);
            mem5.Memory[5].Span.Length.Should().Be(1);
            mem5.Memory[5].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem5.Memory[5].Span[0].Arg0.Should().Be(-1);
            mem5.Memory[5].Span[0].Argv.Count.Should().Be(0);

            var mem6 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem5, null, null);
            mem6.Length.Should().Be(11);
            mem6.Memory.Count.Should().Be(7);
            mem6.Memory[6].Span.Length.Should().Be(1);
            mem6.Memory[6].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem6.Memory[6].Span[0].Arg0.Should().Be(-1);
            mem6.Memory[6].Span[0].Argv.Count.Should().Be(0);

            var mem7 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem6, null, null);
            mem7.Length.Should().Be(12);
            mem7.Memory.Count.Should().Be(8);
            mem7.Memory[7].Span.Length.Should().Be(1);
            mem7.Memory[7].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem7.Memory[7].Span[0].Arg0.Should().Be(-1);
            mem7.Memory[7].Span[0].Argv.Count.Should().Be(0);

            // we exceed 8 items in the memory, should pack
            var mem8 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem7, null, null);
            mem8.Length.Should().Be(13);
            mem8.Memory.Count.Should().Be(2);
            mem8.Memory[0].Span.Length.Should().Be(8);

            for (int i = 0; i < 8; i++)
            {
                mem8.Memory[0].Span[i].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
                mem8.Memory[0].Span[i].Arg0.Should().Be(-1);
                mem8.Memory[0].Span[i].Argv.Count.Should().Be(0);
            }

            mem8.Memory[1].Span.Length.Should().Be(1);
            mem8.Memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem8.Memory[1].Span[0].Arg0.Should().Be(-1);
            mem8.Memory[1].Span[0].Argv.Count.Should().Be(0);
        }

    }

}
