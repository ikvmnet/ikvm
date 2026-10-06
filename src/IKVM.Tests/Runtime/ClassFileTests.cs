using System.Buffers;

using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class ClassFileTests
    {

        [TestMethod]
        public void IsValidFieldDescriptor()
        {
            ClassFile.IsValidFieldDescriptor("").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("L").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("L;").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("Lcom;").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Lcom;A").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("B").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Z").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("C").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("S").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("I").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("J").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("F").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("D").Should().BeTrue();
            ClassFile.IsValidFieldDescriptor("Q").Should().BeFalse();
            ClassFile.IsValidFieldDescriptor("B ").Should().BeFalse();
        }

        [TestMethod]
        public void IsValidMethodDescriptor()
        {
            ClassFile.IsValidMethodDescriptor("").Should().BeFalse();
            ClassFile.IsValidMethodDescriptor("()V").Should().BeTrue();
        }

        [TestMethod]
        public void InstructionsDoNotContainDataFromPreviouslyParsedCode()
        {
            // instructions are parsed into a pooled array, so put junk into the pooled arrays first
            for (int size = 16; size <= 1024; size *= 2)
            {
                var junk = ArrayPool<ClassFile.Method.Instruction>.Shared.Rent(size);
                for (int i = 0; i < junk.Length; i++)
                    junk[i].PatchOpCode(NormalizedByteCode.__iinc, 123, 456);

                ArrayPool<ClassFile.Method.Instruction>.Shared.Return(junk);
            }

            var builder = new ClassFileBuilder(49, AccessFlag.Public, "InstructionPoolTest", "java/lang/Object");
            var code = new BlobBuilder();
            new CodeBuilder(code).AconstNull().Pop().Return();
            var attributes = new AttributeTableBuilder(builder.Constants);
            attributes.Code(1, 0, code, e => { }, null);
            builder.AddMethod(AccessFlag.Public | AccessFlag.Static, "m", "()V", attributes);
            var buffer = new BlobBuilder();
            builder.Serialize(buffer);

            using var classFile = new ClassFile(JVM.Context, JVM.Context.Diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(buffer.ToArray()), "InstructionPoolTest", ClassFileParseOptions.None, null);
            var instructions = classFile.Methods[0].Instructions;

            // three instructions and the terminating nop, none of which have operands
            instructions.Should().HaveCount(4);
            foreach (var instruction in instructions)
            {
                instruction.Arg1.Should().Be(0);
                instruction.Arg2.Should().Be(0);
            }
        }

    }

}
