using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using IKVM.ByteCode.Parsing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.ByteCode.Reading.Tests
{

    [TestClass]
    public class ClassReaderTests
    {

        [TestMethod]
        [ExpectedException(typeof(InvalidClassException))]
        public async Task ShouldThrowOnEmptyStream()
        {
            var stream = new MemoryStream();
            await ClassReader.ReadAsync(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidClassMagicException))]
        public async Task ShouldThrowOnSmallStream()
        {
            var stream = new MemoryStream(new byte[10]);
            await ClassReader.ReadAsync(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidClassMagicException))]
        public async Task ShouldThrowOnBadStream()
        {
            var stream = new MemoryStream(new byte[35]);
            await ClassReader.ReadAsync(stream);
        }

        [TestMethod]
        public void CanLoadClass()
        {
            var clazz = ClassReader.Read(Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "0.class"));
            clazz.Should().NotBeNull();
            clazz.This.Name.Value.Should().Be("0");
            clazz.Constants.ToList();
            clazz.Interfaces.ToList();
            clazz.Fields.Should().HaveCount(0);
            clazz.Fields.ToList();
            clazz.Methods.Should().HaveCount(2);
            clazz.Methods.ToList();

            clazz.Methods[0].Attributes.Code.Code.Should().NotBeNull();
            clazz.Methods[1].Attributes.Code.Code.Should().NotBeNull();
        }

        [TestMethod]
        public void CanLoadTestClassFiles()
        {
            var d = Path.Combine(Path.GetDirectoryName(typeof(ClassReaderTests).Assembly.Location), "resources");
            var l = Directory.GetFiles(d, "*.class", SearchOption.AllDirectories);

            foreach (var i in l)
            {
                var c = ClassReader.Read(i);
                c.This.Should().NotBeNull();
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
                    iface.Class.Name.Value.Should().NotBeNull();

                foreach (var field in c.Fields)
                {
                    field.Should().NotBeNull();
                    field.Name.Value.Should().NotBeNull();
                    field.Descriptor.Value.Should().NotBeNull();
                    field.Attributes.ToList();

                    foreach (var attribute in field.Attributes)
                        TestAttribute(attribute);
                }

                foreach (var method in c.Methods)
                {
                    method.Name.Should().NotBeNull();
                    method.Descriptor.Should().NotBeNull();
                    method.Attributes.ToList();

                    foreach (var attribute in method.Attributes)
                        TestAttribute(attribute);
                }

                c.Attributes.ToList();
                foreach (var attribute in c.Attributes)
                    TestAttribute(attribute);
            }
        }

        void TestConstant(IConstantReader constant)
        {
            if (constant is Utf8ConstantReader utf8)
                TestConstant(utf8);
            if (constant is IntegerConstantReader integer)
                TestConstant(integer);
            if (constant is MethodHandleConstantReader methodHandle)
                TestConstant(methodHandle);
        }

        void TestConstant(Utf8ConstantReader utf8)
        {
            utf8.Value.Should().NotBeNull();
        }

        void TestConstant(IntegerConstantReader integer)
        {
            integer.Value.GetType().Should().Be(typeof(int));
        }

        void TestConstant(MethodHandleConstantReader methodHandle)
        {
            if (methodHandle.ReferenceKind is ReferenceKind.GetField or ReferenceKind.GetStatic or ReferenceKind.PutField or ReferenceKind.PutStatic)
                methodHandle.Reference.Should().BeOfType<FieldrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.NewInvokeSpecial)
                methodHandle.Reference.Should().BeOfType<MethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial && methodHandle.DeclaringClass.Version < new ClassFormatVersion(52, 0))
                methodHandle.Reference.Should().BeOfType<MethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial && methodHandle.DeclaringClass.Version >= new ClassFormatVersion(52, 0))
                methodHandle.Reference.Should().Match(i => i is MethodrefConstantReader || i is InterfaceMethodrefConstantReader);
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeInterface)
                methodHandle.Reference.Should().BeOfType<InterfaceMethodrefConstantReader>();
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial or ReferenceKind.InvokeInterface && methodHandle.Reference is MethodrefConstantReader methodRef)
                methodRef.NameAndType.Name.Value.Should().NotBe("<init>").And.NotBe("<clinit>");
            if (methodHandle.ReferenceKind is ReferenceKind.InvokeVirtual or ReferenceKind.InvokeStatic or ReferenceKind.InvokeSpecial or ReferenceKind.InvokeInterface && methodHandle.Reference is InterfaceMethodrefConstantReader interfaceMethodRef)
                interfaceMethodRef.Class.Name.Value.Should().NotBe("<init>").And.NotBe("<clinit>");
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
            attribute.Info.Name.Value.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeInvisibleAnnotationsAttributeReader attribute)
        {
            attribute.Info.Name.Value.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeVisibleTypeAnnotationsAttributeReader attribute)
        {
            attribute.Info.Name.Value.Should().NotBeEmpty();
            TestAnnotations(attribute.Annotations);
        }

        void TestAttribute(RuntimeInvisibleTypeAnnotationsAttributeReader attribute)
        {
            attribute.Info.Name.Value.Should().NotBeEmpty();
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
            annotation.Type.Value.Should().NotBeEmpty();
            TestElementValuePair(annotation.Elements);
        }

        void TestAnnotation(TypeAnnotationReader annotation)
        {
            annotation.Type.Value.Should().NotBeEmpty();
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
            if (value is ElementValueConstantReader elementValueConstantReader)
                TestElementValue(elementValueConstantReader);
            if (value is ElementValueAnnotationReader elementAnnotationValueReader)
                TestElementValue(elementAnnotationValueReader);
            if (value is ElementValueArrayReader elementArrayValueReader)
                TestElementValue(elementArrayValueReader);
            if (value is ElementValueClassReader elementClassInfoValueReader)
                TestElementValue(elementClassInfoValueReader);
        }

        void TestElementValue(ElementValueConstantReader elementValueConstantReader)
        {
            Enum.GetName(typeof(ElementValueTag), elementValueConstantReader.Tag).Should().NotBeNullOrEmpty();
            elementValueConstantReader.Value.Should().NotBeNull();
            TestConstant(elementValueConstantReader.Value);
        }

        void TestElementValue(ElementValueClassReader elementClassInfoValueReader)
        {
            elementClassInfoValueReader.Class.Value.Should().NotBeEmpty();
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
