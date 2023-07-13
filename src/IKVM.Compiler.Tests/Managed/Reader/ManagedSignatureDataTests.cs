using System.Reflection;

using FluentAssertions;

using IKVM.Compiler.Managed.Reader;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Compiler.Tests.Managed.Reader
{

    [TestClass]
    public class ManagedSignatureDataTests
    {

        [TestMethod]
        public void CanCreateType()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);
            var sig = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);
            sig.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            sig.Local0.Data.Type.Assembly.Should().BeSameAs(assembly);
            sig.Local0.Data.Type.Should().BeSameAs(type);
        }

        [TestMethod]
        public void CanCreateSZArrayOfType()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);

            var sig = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);
            sig.Length.Should().Be(1);
            sig.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            sig.Local0.Data.Type.Assembly.Should().BeSameAs(assembly);
            sig.Local0.Data.Type.Should().BeSameAs(type);

            var arrayType = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), sig, null, null);
            arrayType.Length.Should().Be(2);
            arrayType.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            arrayType.Local0.Data.Type.Should().Be(type);
            arrayType.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
        }

        [TestMethod]
        public void CanCreateArrayOfType()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);

            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Array, new ManagedArrayShape(1, new int[] { 1 }, new int[] { })), local0, null, null);
            local1.Length.Should().Be(2);
            local1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local1.Local0.Data.Type.Should().Be(type);
            local1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.Array);
            local1.Local1.Data.Data.Array_Shape.Should().NotBeNull();
            local1.Local1.Data.Data.Array_Shape.Rank.Should().Be(1);
            local1.Local1.Data.Data.Array_Shape.GetSize(0).Should().Be(1);
            local1.Local1.Data.Data.Array_Shape.GetLowerBound(0).Should().BeNull();
        }

        [TestMethod]
        public void CanPackMemory()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);

            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);
            local0.Length.Should().Be(1);
            local0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local0.Local0.Arg0.Should().Be(0);
            local0.Local0.Argv.Count.Should().Be(0);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local0, null, null);
            local1.Length.Should().Be(2);
            local1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local1.Local0.Arg0.Should().Be(0);
            local1.Local0.Argv.Count.Should().Be(0);
            local1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local1.Local1.Arg0.Should().Be(-1);
            local1.Local1.Argv.Count.Should().Be(0);

            var local2 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local1, null, null);
            local2.Length.Should().Be(3);
            local2.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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
            local3.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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
            mem0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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
            mem1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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

        [TestMethod]
        public void CanExtractLocalArg()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);

            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);
            local0.Length.Should().Be(1);
            local0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local0.Local0.Arg0.Should().Be(0);
            local0.Local0.Argv.Count.Should().Be(0);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local0, null, null);
            local1.Length.Should().Be(2);
            local1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local1.Local0.Arg0.Should().Be(0);
            local1.Local0.Argv.Count.Should().Be(0);
            local1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local1.Local1.Arg0.Should().Be(-1);
            local1.Local1.Argv.Count.Should().Be(0);

            ManagedSignatureData.ExtractCode(local1, 0, out var result);
            result.Length.Should().Be(1);
            result.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            result.Local0.Arg0.Should().Be(0);
            result.Local0.Argv.Count.Should().Be(0);
        }

        [TestMethod]
        public void CanExtractMemoryToLocal()
        {
            var assembly = new ManagedAssembly(null, null, new AssemblyName("Test.Assembly"));
            var type = new ManagedType(assembly, null, "Test.Assembly", TypeAttributes.Public);

            var local0 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.Type, type), null, null, null);
            local0.Length.Should().Be(1);
            local0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local0.Local0.Arg0.Should().Be(0);
            local0.Local0.Argv.Count.Should().Be(0);

            var local1 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local0, null, null);
            local1.Length.Should().Be(2);
            local1.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            local1.Local0.Arg0.Should().Be(0);
            local1.Local0.Argv.Count.Should().Be(0);
            local1.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            local1.Local1.Arg0.Should().Be(-1);
            local1.Local1.Argv.Count.Should().Be(0);

            var local2 = new ManagedSignatureData(new ManagedSignatureCodeData(ManagedSignatureKind.SZArray), local1, null, null);
            local2.Length.Should().Be(3);
            local2.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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
            local3.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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
            mem0.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
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

            // backs up one level, removing memory
            ManagedSignatureData.ExtractCode(mem0, 3, out var result);
            result.Length.Should().Be(4);
            result.Local0.Data.Kind.Should().Be(ManagedSignatureKind.Type);
            result.Local0.Arg0.Should().Be(0);
            result.Local0.Argv.Count.Should().Be(0);
            result.Local1.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            result.Local1.Arg0.Should().Be(-1);
            result.Local1.Argv.Count.Should().Be(0);
            result.Local2.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            result.Local2.Arg0.Should().Be(-1);
            result.Local2.Argv.Count.Should().Be(0);
            result.Local3.Data.Kind.Should().Be(ManagedSignatureKind.SZArray);
            result.Local3.Arg0.Should().Be(-1);
            result.Local3.Argv.Count.Should().Be(0);
            result.Memory.Count.Should().Be(0);
        }

    }

}
