using FluentAssertions;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Writing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace IKVM.ByteCode.Tests
{
    [TestClass]
    public class ClassWriterTests
    {
        [TestMethod]
        public void CanWriteClass()
        {
            using var stream = new MemoryStream();

            var writer = new ClassWriter(AccessFlag.ACC_FINAL | AccessFlag.ACC_STATIC, "name", null, 1, 2);

            writer.Write(stream);

            var clazz = ClassReader.Read(new System.Buffers.ReadOnlySequence<byte>(stream.ToArray()));

            clazz.Should().NotBeNull();
            clazz.Constants.Count.Should().Be(3);
            clazz.This.Name.Value.Should().Be("name");
            clazz.AccessFlags.Should().Be(AccessFlag.ACC_FINAL | AccessFlag.ACC_STATIC);
        }
    }
}
