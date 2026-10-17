using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.Attributes;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Runtime
{

    [TestClass]
    public class StubGeneratorTests
    {

        /// <summary>
        /// Generates the stub class file for the specified class and parses it.
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        static ClassFile Stub(global::java.lang.Class clazz)
        {
            using var stream = new MemoryStream();
            JVM.Context.StubGenerator.Write(stream, RuntimeJavaType.FromClass(clazz), true, true, true, true, false);
            return new ClassFile(JVM.Context, JVM.Context.Diagnostics, IKVM.ByteCode.Decoding.ClassFile.Read(stream.ToArray()), clazz.getName(), ClassFileParseOptions.None, null);
        }

        [TestMethod]
        public void ShouldEncodeClassAnnotationDefaultAsJavaDescriptor()
        {
            // the importer stores the default of XmlElement.type() as an assembly qualified .NET name
            using var classFile = Stub(typeof(global::javax.xml.bind.annotation.XmlElement));
            var value = (object[])classFile.Methods.First(i => i.Name == "type").AnnotationDefault;
            value.Should().Equal(AnnotationDefaultAttribute.TAG_CLASS, "Ljavax/xml/bind/annotation/XmlElement$DEFAULT;");
        }

        [TestMethod]
        public void ShouldEncodeClassAnnotationValueAsJavaDescriptor()
        {
            // Addressing is annotated with @WebServiceFeatureAnnotation(id = AddressingFeature.ID, bean = AddressingFeature.class)
            using var classFile = Stub(typeof(global::javax.xml.ws.soap.Addressing));
            var annotation = classFile.Annotations.Cast<object[]>().First(i => (string)i[1] == "Ljavax/xml/ws/spi/WebServiceFeatureAnnotation;");

            var index = System.Array.IndexOf(annotation, "bean", 2);
            index.Should().BeGreaterThan(0);
            ((object[])annotation[index + 1]).Should().Equal(AnnotationDefaultAttribute.TAG_CLASS, "Ljavax/xml/ws/soap/AddressingFeature;");
        }

    }

}
