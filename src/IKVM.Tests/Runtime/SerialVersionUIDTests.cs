using FluentAssertions;

using IKVM.ByteCode;
using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Encoding;
using IKVM.Runtime;
using IKVM.Runtime.StubGen;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    /// <summary>
    /// Tests the serialVersionUID the stub generator computes for a class that declares none, against the one
    /// ObjectStreamClass computes for the same class. The latter is OpenJDK's own implementation, which orders the
    /// members by String.compareTo, which compares UTF-16 code units.
    /// </summary>
    [TestClass]
    public class SerialVersionUIDTests
    {

        static void ShouldComputeSerialVersionUIDAsJava(global::java.lang.Class clazz)
        {
            // computed first, since it finishes the type
            var expected = global::java.io.ObjectStreamClass.lookup(clazz).getSerialVersionUID();
            SerialVersionUID.Compute(RuntimeJavaType.FromClass(clazz)).Should().Be(expected);
        }

        static AttributeTableBuilder NoAttributes(ClassFileBuilder builder) => new AttributeTableBuilder(builder.Constants);

        [TestMethod]
        public void ShouldComputeSerialVersionUIDOfJavaClass()
        {
            ShouldComputeSerialVersionUIDAsJava(typeof(global::java.util.EnumSet));
        }

        [TestMethod]
        public void ShouldOrderMembersAsJava()
        {
            var builder = new ClassFileBuilder(49, AccessFlag.Public | AccessFlag.Abstract, "SerialVersionUIDOrderTest", "java/lang/Object");
            builder.AddInterface("java/io/Serializable");

            // each pair is ordered one way by a culture-sensitive comparison and the other by String.compareTo, including
            // the property accessor and method names of .NET types, which the exporter computes serialVersionUID for
            foreach (var name in new[] { "bar", "Foo", "a", "B" })
                builder.AddField(AccessFlag.Public, name, "I", NoAttributes(builder));

            foreach (var name in new[] { "alpha", "Beta", "get_Value", "GetValue" })
                builder.AddMethod(AccessFlag.Public | AccessFlag.Abstract, name, "()V", NoAttributes(builder));

            // overloads are ordered by signature, where a culture-sensitive comparison puts '[' before letters
            builder.AddMethod(AccessFlag.Public | AccessFlag.Abstract, "m", "(I)V", NoAttributes(builder));
            builder.AddMethod(AccessFlag.Public | AccessFlag.Abstract, "m", "([I)V", NoAttributes(builder));

            var buffer = new BlobBuilder();
            builder.Serialize(buffer);
            ShouldComputeSerialVersionUIDAsJava(new ByteArrayClassLoader().Define("SerialVersionUIDOrderTest", buffer.ToArray()));
        }

    }

}
