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

        class TestAssemblyContext : IManagedAssemblyContext
        {

            public ManagedType ResolveType(ManagedAssembly assembly, string typeName)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly)
            {
                throw new System.NotImplementedException();
            }

        }

        [TestMethod]
        public void CanCreatePrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            primitiveType.length.Should().Be(1);
            primitiveType.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            primitiveType.local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
        }

        [TestMethod]
        public void CanCreateType()
        {
            var assemblyContext = new TestAssemblyContext();
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyContext, assemblyName, typeName, null);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);
            type.local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            type.local0.Data.Type_Type.Value.Context.Should().Be(assemblyContext);
            type.local0.Data.Type_Type.Value.AssemblyName.Should().Be(assemblyName);
            type.local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);
            type.local0.Data.Type_Type.Value.ManagedType.Should().BeNull();
        }

        [TestMethod]
        public void CanCreateSZArrayOfPrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            primitiveType.length.Should().Be(1);
            primitiveType.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            primitiveType.local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), primitiveType, null, null);
            arrayType.length.Should().Be(2);
            arrayType.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            arrayType.local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
            arrayType.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
        }

        [TestMethod]
        public void CanCreateSZArrayOfType()
        {
            var assemblyContext = new TestAssemblyContext();
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyContext, assemblyName, typeName, null);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);
            type.length.Should().Be(1);
            type.local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            type.local0.Data.Type_Type.Value.Context.Should().Be(assemblyContext);
            type.local0.Data.Type_Type.Value.AssemblyName.Should().Be(assemblyName);
            type.local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);
            type.local0.Data.Type_Type.Value.ManagedType.Should().BeNull();

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), type, null, null);
            arrayType.length.Should().Be(2);
            arrayType.local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            arrayType.local0.Data.Type_Type.Value.TypeName.Should().Be(typeName);
            arrayType.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
        }

        [TestMethod]
        public void CanCreateArrayOfPrimitiveType()
        {
            var primitiveType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(1, new int[] { 1 }, new int[] { })), primitiveType, null, null);
            arrayType.length.Should().Be(2);
            arrayType.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            arrayType.local0.Data.Primitive_TypeCode.Should().Be(ManagedPrimitiveTypeCode.Int32);
            arrayType.local1.Data.Kind.Should().Be(ManagedSignatureKind.Array);
            arrayType.local1.Data.Array_Shape.Should().NotBeNull();
            arrayType.local1.Data.Array_Shape.Value.Rank.Should().Be(1);
            arrayType.local1.Data.Array_Shape.Value.GetSize(0).Should().Be(1);
            arrayType.local1.Data.Array_Shape.Value.GetLowerBound(0).Should().BeNull();
        }

        [TestMethod]
        public void CanCreateArrayOfType()
        {
            var assemblyContext = new TestAssemblyContext();
            var assemblyName = new AssemblyName("Test.Assembly");
            var typeName = "Test.Assembly";
            var typeRef = new ManagedTypeRef(assemblyContext, assemblyName, typeName, null);
            var type = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, typeRef), null, null, null);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(1, new int[] { 1 }, new int[] { })), type, null, null);
            arrayType.length.Should().Be(2);
            arrayType.local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            arrayType.local0.Data.Type_Type.Should().Be(typeRef);
            arrayType.local1.Data.Kind.Should().Be(ManagedSignatureKind.Array);
            arrayType.local1.Data.Array_Shape.Should().NotBeNull();
            arrayType.local1.Data.Array_Shape.Value.Rank.Should().Be(1);
            arrayType.local1.Data.Array_Shape.Value.GetSize(0).Should().Be(1);
            arrayType.local1.Data.Array_Shape.Value.GetLowerBound(0).Should().BeNull();
        }

        [TestMethod]
        public void CanPackMemory()
        {
            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.PrimitiveType, ManagedPrimitiveTypeCode.Int32), null, null, null);
            local0.length.Should().Be(1);
            local0.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local0.local0.Arg0.Should().Be(0);
            local0.local0.Argv.Count.Should().Be(0);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local0, null, null);
            local1.length.Should().Be(2);
            local1.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local1.local0.Arg0.Should().Be(0);
            local1.local0.Argv.Count.Should().Be(0);
            local1.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local1.local1.Arg0.Should().Be(-1);
            local1.local1.Argv.Count.Should().Be(0);

            var local2= new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local1, null, null);
            local2.length.Should().Be(3);
            local2.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local2.local0.Arg0.Should().Be(0);
            local2.local0.Argv.Count.Should().Be(0);
            local2.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local2.local1.Arg0.Should().Be(-1);
            local2.local1.Argv.Count.Should().Be(0);
            local2.local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local2.local2.Arg0.Should().Be(-1);
            local2.local2.Argv.Count.Should().Be(0);

            var local3 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local2, null, null);
            local3.length.Should().Be(4);
            local3.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            local3.local0.Arg0.Should().Be(0);
            local3.local0.Argv.Count.Should().Be(0);
            local3.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.local1.Arg0.Should().Be(-1);
            local3.local1.Argv.Count.Should().Be(0);
            local3.local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.local2.Arg0.Should().Be(-1);
            local3.local2.Argv.Count.Should().Be(0);
            local3.local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local3.local3.Arg0.Should().Be(-1);
            local3.local3.Argv.Count.Should().Be(0);

            var mem0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local3, null, null);
            mem0.length.Should().Be(5);
            mem0.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            mem0.local0.Arg0.Should().Be(0);
            mem0.local0.Argv.Count.Should().Be(0);
            mem0.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.local1.Arg0.Should().Be(-1);
            mem0.local1.Argv.Count.Should().Be(0);
            mem0.local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.local2.Arg0.Should().Be(-1);
            mem0.local2.Argv.Count.Should().Be(0);
            mem0.local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.local3.Arg0.Should().Be(-1);
            mem0.local3.Argv.Count.Should().Be(0);
            mem0.memory.Count.Should().Be(1);
            mem0.memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem0.memory[0].Span[0].Arg0.Should().Be(-1);
            mem0.memory[0].Span[0].Argv.Count.Should().Be(0);

            var mem1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem0, null, null);
            mem1.length.Should().Be(6);
            mem1.local0.Data.Kind.Should().Be(ManagedSignatureKind.PrimitiveType);
            mem1.local0.Arg0.Should().Be(0);
            mem1.local0.Argv.Count.Should().Be(0);
            mem1.local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.local1.Arg0.Should().Be(-1);
            mem1.local1.Argv.Count.Should().Be(0);
            mem1.local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.local2.Arg0.Should().Be(-1);
            mem1.local2.Argv.Count.Should().Be(0);
            mem1.local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.local3.Arg0.Should().Be(-1);
            mem1.local3.Argv.Count.Should().Be(0);
            mem1.memory.Count.Should().Be(2);
            mem1.memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.memory[0].Span[0].Arg0.Should().Be(-1);
            mem1.memory[0].Span[0].Argv.Count.Should().Be(0);
            mem1.memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem1.memory[1].Span[0].Arg0.Should().Be(-1);
            mem1.memory[1].Span[0].Argv.Count.Should().Be(0);

            var mem2 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem1, null, null);
            mem2.length.Should().Be(7);
            mem2.memory.Count.Should().Be(3);
            mem2.memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.memory[0].Span[0].Arg0.Should().Be(-1);
            mem2.memory[0].Span[0].Argv.Count.Should().Be(0);
            mem2.memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.memory[1].Span[0].Arg0.Should().Be(-1);
            mem2.memory[1].Span[0].Argv.Count.Should().Be(0);
            mem2.memory[2].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem2.memory[2].Span[0].Arg0.Should().Be(-1);
            mem2.memory[2].Span[0].Argv.Count.Should().Be(0);

            var mem3 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem2, null, null);
            mem3.length.Should().Be(8);
            mem3.memory.Count.Should().Be(4);
            mem3.memory[0].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.memory[0].Span[0].Arg0.Should().Be(-1);
            mem3.memory[0].Span[0].Argv.Count.Should().Be(0);
            mem3.memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.memory[1].Span[0].Arg0.Should().Be(-1);
            mem3.memory[1].Span[0].Argv.Count.Should().Be(0);
            mem3.memory[2].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.memory[2].Span[0].Arg0.Should().Be(-1);
            mem3.memory[2].Span[0].Argv.Count.Should().Be(0);
            mem3.memory[3].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem3.memory[3].Span[0].Arg0.Should().Be(-1);
            mem3.memory[3].Span[0].Argv.Count.Should().Be(0);

            var mem4 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem3, null, null);
            mem4.length.Should().Be(9);
            mem4.memory.Count.Should().Be(5);
            mem4.memory[4].Span.Length.Should().Be(1);
            mem4.memory[4].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem4.memory[4].Span[0].Arg0.Should().Be(-1);
            mem4.memory[4].Span[0].Argv.Count.Should().Be(0);

            var mem5 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem4, null, null);
            mem5.length.Should().Be(10);
            mem5.memory.Count.Should().Be(6);
            mem5.memory[5].Span.Length.Should().Be(1);
            mem5.memory[5].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem5.memory[5].Span[0].Arg0.Should().Be(-1);
            mem5.memory[5].Span[0].Argv.Count.Should().Be(0);

            var mem6 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem5, null, null);
            mem6.length.Should().Be(11);
            mem6.memory.Count.Should().Be(7);
            mem6.memory[6].Span.Length.Should().Be(1);
            mem6.memory[6].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem6.memory[6].Span[0].Arg0.Should().Be(-1);
            mem6.memory[6].Span[0].Argv.Count.Should().Be(0);

            var mem7 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem6, null, null);
            mem7.length.Should().Be(12);
            mem7.memory.Count.Should().Be(8);
            mem7.memory[7].Span.Length.Should().Be(1);
            mem7.memory[7].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem7.memory[7].Span[0].Arg0.Should().Be(-1);
            mem7.memory[7].Span[0].Argv.Count.Should().Be(0);

            // we exceed 8 items in the memory, should pack
            var mem8 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), mem7, null, null);
            mem8.length.Should().Be(13);
            mem8.memory.Count.Should().Be(2);
            mem8.memory[0].Span.Length.Should().Be(8);

            for (int i = 0; i < 8; i++)
            {
                mem8.memory[0].Span[i].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
                mem8.memory[0].Span[i].Arg0.Should().Be(-1);
                mem8.memory[0].Span[i].Argv.Count.Should().Be(0);
            }

            mem8.memory[1].Span.Length.Should().Be(1);
            mem8.memory[1].Span[0].Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            mem8.memory[1].Span[0].Arg0.Should().Be(-1);
            mem8.memory[1].Span[0].Argv.Count.Should().Be(0);
        }

    }

}
