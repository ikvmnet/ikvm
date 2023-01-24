using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.ByteCode.Reading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Tests
{

    [TestClass]
    public class ClassReaderTests
    {

        [TestMethod]
        public async Task CanLoadClassAsync()
        {
            using var file = File.OpenRead(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            var clazz = await ClassReader.ReadAsync(file);
            clazz.Should().NotBeNull();
            clazz.Name.Should().Be("0");
            clazz.Constants.ToList();
            clazz.Interfaces.ToList();
            clazz.Fields.Should().HaveCount(0);
            clazz.Fields.ToList();
            clazz.Methods.Should().HaveCount(2);
            clazz.Methods.ToList();

            clazz.Methods[0].Attributes.FirstOfType<CodeAttributeReader>().Code.Should().NotBeNull();
            clazz.Methods[1].Attributes.FirstOfType<CodeAttributeReader>().Code.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLoadClass()
        {
            using var file = File.OpenRead(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            var clazz = ClassReader.Read(file);
            clazz.Should().NotBeNull();
            clazz.Name.Should().Be("0");
            clazz.Constants.ToList();
            clazz.Interfaces.ToList();
            clazz.Fields.Should().HaveCount(0);
            clazz.Fields.ToList();
            clazz.Methods.Should().HaveCount(2);
            clazz.Methods.ToList();

            clazz.Methods[0].Attributes.FirstOfType<CodeAttributeReader>().Code.Should().NotBeNull();
            clazz.Methods[1].Attributes.FirstOfType<CodeAttributeReader>().Code.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLoadValidClassFiles()
        {
            var d = Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "resources");
            var l = Directory.GetFiles(d, "*.class", SearchOption.AllDirectories);

            foreach (var i in l)
            {
                using var f = File.OpenRead(i);
                var c = ClassReader.Read(f);
                c.Name.Should().NotBeNull();
                c.Constants.ToList();

                foreach (var constant in c.Constants)
                    TestConstant(constant);

                c.Interfaces.ToList();
                c.Interfaces.Should().OnlyHaveUniqueItems();
                c.Fields.ToList();
                c.Fields.Should().OnlyHaveUniqueItems();
                c.Methods.ToList();
                c.Methods.Should().OnlyHaveUniqueItems();

                foreach (var iface in c.Interfaces)
                    iface.Name.Should().NotBeNull();

                foreach (var field in c.Fields)
                {
                    field.Name.Should().NotBeNull();
                    field.Descriptor.Should().NotBeNull();
                    field.Attributes.ToList();

                    foreach (var attribute in field.Attributes)
                        attribute.Name.Should().NotBeNull();
                }

                foreach (var method in c.Methods)
                {
                    method.Name.Should().NotBeNull();
                    method.Descriptor.Should().NotBeNull();
                    method.Attributes.ToList();

                    foreach (var attribute in method.Attributes)
                        attribute.Name.Should().NotBeNull();
                }

                foreach (var attribute in c.Attributes)
                    TestAttribute(attribute);
            }
        }

        void TestConstant(IConstantReader constant)
        {
            if (constant is MethodHandleConstantReader methodHandle)
                TestConstant(methodHandle);
        }

        void TestConstant(MethodHandleConstantReader methodHandle)
        {
            if (methodHandle.ReferenceKind is ReferenceKind.GetField or ReferenceKind.GetStatic or ReferenceKind.PutField or ReferenceKind.PutStatic)
                methodHandle.Reference.Should().BeOfType<FieldrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.NewInvokeSpecial)
                methodHandle.Reference.Should().BeOfType<MethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial && methodHandle.DeclaringClass.MajorVersion < 52)
                methodHandle.Reference.Should().BeOfType<MethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial && methodHandle.DeclaringClass.MajorVersion >= 52)
                methodHandle.Reference.Should().Match(i => i is MethodrefConstantReader || i is InterfaceMethodrefConstantReader);
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeInterface)
                methodHandle.Reference.Should().BeOfType<InterfaceMethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial or ReferenceKind.InvokeInterface && methodHandle.Reference is MethodrefConstantReader methodRef)
                methodRef.Name.Should().NotBe("<init>").And.NotBe("<clinit>");
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial or ReferenceKind.InvokeInterface && methodHandle.Reference is InterfaceMethodrefConstantReader interfaceMethodRef)
                interfaceMethodRef.Name.Should().NotBe("<init>").And.NotBe("<clinit>");
        }

        void TestAttribute(AttributeReader attribute)
        {
            if (attribute is RuntimeVisibleAnnotationsAttributeReader runtimeVisibleAnnotationsAttributeReader)
                TestAttribute(runtimeVisibleAnnotationsAttributeReader);
            if (attribute is RuntimeInvisibleAnnotationsAttributeReader runtimeInvisibleAnnotationsAttributeReader)
                TestAttribute(runtimeInvisibleAnnotationsAttributeReader);
            if (attribute is RuntimeVisibleTypeAnnotationsAttributeReader runtimeVisibleTypeAnnotationsAttributeReader)
                TestAttribute(runtimeVisibleTypeAnnotationsAttributeReader);
            if (attribute is RuntimeInvisibleTypeAnnotationsAttributeReader runtimeInvisibleTypeAnnotationsAttributeReader)
                TestAttribute(runtimeInvisibleTypeAnnotationsAttributeReader);
        }

        void TestAttribute(RuntimeVisibleAnnotationsAttributeReader attribute)
        {
            attribute.Name.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeInvisibleAnnotationsAttributeReader attribute)
        {
            attribute.Name.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeVisibleTypeAnnotationsAttributeReader attribute)
        {
            attribute.Name.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeInvisibleTypeAnnotationsAttributeReader attribute)
        {
            attribute.Name.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAnnotations(IReadOnlyList<AnnotationReader> annotations)
        {
            foreach (var annotation in annotations)
                TestAnnotation(annotation);
        }

        void TestAnnotations(IReadOnlyList<TypeAnnotationReader> annotations)
        {
            foreach (var annotation in annotations)
                TestAnnotation(annotation);
        }

        void TestAnnotation(AnnotationReader annotation)
        {
            annotation.Type.Should().NotBeEmpty();
            TestElementValuePair(annotation.Elements);
        }

        void TestAnnotation(TypeAnnotationReader annotation)
        {
            annotation.Type.Should().NotBeEmpty();
            TestElementValuePair(annotation.Elements);
        }

        void TestElementValuePair(ElementValueKeyReaderCollection elements)
        {
            elements.Count.Should().BeLessThan(256);

            foreach (var element in elements)
                TestElement(element.Key, element.Value);
        }

        void TestElement(string name, ElementValueReader value)
        {
            name.Should().NotBeEmpty();
            value.Should().NotBeNull();

            TestElementValue(value);
        }

        void TestElementValue(ElementValueReader value)
        {
            if (value is ElementValueAnnotationReader elementAnnotationValueReader)
                TestElementValue(elementAnnotationValueReader);
            if (value is ElementValueArrayReader elementArrayValueReader)
                TestElementValue(elementArrayValueReader);
            if (value is ElementValueClassReader elementClassInfoValueReader)
                TestElementValue(elementClassInfoValueReader);
        }

        void TestElementValue(ElementValueClassReader elementClassInfoValueReader)
        {
            elementClassInfoValueReader.Class.Should().NotBeEmpty();
        }

        void TestElementValue(ElementValueAnnotationReader elementAnnotationValueReader)
        {
            TestAnnotation(elementAnnotationValueReader.Annotation);
        }

        void TestElementValue(ElementValueArrayReader elementArrayValueReader)
        {
            foreach (var value in elementArrayValueReader.Values)
                TestElementValue(value);
        }

    }

}
