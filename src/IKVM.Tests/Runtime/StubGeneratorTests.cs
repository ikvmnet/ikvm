using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.Attributes;
using IKVM.CoreLib.Linking;
using IKVM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ClassFile = IKVM.CoreLib.Linking.ClassFile<IKVM.Runtime.RuntimeJvmTypeSystem, IKVM.Runtime.RuntimeJavaType, IKVM.Runtime.RuntimeJavaMember, IKVM.Runtime.RuntimeJavaField, IKVM.Runtime.RuntimeJavaMethod>;

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
            return new ClassFile(new RuntimeJvmTypeSystem(JVM.Context), IKVM.ByteCode.Decoding.ClassFile.Read(stream.ToArray()), clazz.getName(), ClassFileParseOptions.None, []);
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
