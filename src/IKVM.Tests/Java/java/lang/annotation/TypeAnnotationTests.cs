using System.IO;
using System.Linq;

using FluentAssertions;

using IKVM.Tests.Util;

using java.lang.reflect;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.lang.annotation
{

    [TestClass]
    public class TypeAnnotationTests
    {

        static readonly string[] FilesToBuild = new[]
        {
                "TypeAnnotation1",
                "TypeAnnotation2",
                "TypeAnnotationArrayTest",
                "TypeAnnotationClassTypeVarAndField",
                "TypeAnnotationNestedTestOuter",
                "TypeAnnotationNestedTest",
                "TypeAnnotationWildcardTest"
        };

        static readonly string Package = "ikvm.tests.java.lang.annotation";
        static readonly string ResourcePrefix = "IKVM.Tests.Java.java.lang.annotation.";

        global::java.lang.Class TypeAnnotation1Class;
        global::java.lang.reflect.Method TypeAnnotation1ValueMethod;

        global::java.lang.Class TypeAnnotation2Class;
        global::java.lang.reflect.Method TypeAnnotation2ValueMethod;

        global::java.lang.Class TypeAnnotationArrayTestClass;
        global::java.lang.Class TypeAnnotationNestedTestClass;
        global::java.lang.Class TypeAnnotationClassTypeVarAndFieldClass;
        global::java.lang.Class TypeAnnotationWildcardTestClass;


        /// <summary>
        /// Compiles some dynamic types.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var c = new InMemoryCompiler(FilesToBuild.Select(i => new InMemoryCodeUnit($"{Package}.{i}", new StreamReader(typeof(ClassTests).Assembly.GetManifestResourceStream($"{ResourcePrefix}{i}.java")).ReadToEnd())).ToArray());
            c.Compile();

            TypeAnnotation1Class = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotation1");
            TypeAnnotation1ValueMethod = TypeAnnotation1Class.getDeclaredMethod("value");
            TypeAnnotation2Class = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotation2");
            TypeAnnotation2ValueMethod = TypeAnnotation2Class.getDeclaredMethod("value");

            TypeAnnotationArrayTestClass = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotationArrayTest");
            TypeAnnotationNestedTestClass = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotationNestedTest");
            TypeAnnotationClassTypeVarAndFieldClass = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotationClassTypeVarAndField");
            TypeAnnotationWildcardTestClass = c.GetClass("ikvm.tests.java.lang.annotation.TypeAnnotationWildcardTest");

            c.WriteJar(@"C:\Users\jhaltom\out.jar");
        }

        [TestMethod]
        public void TestSuper()
        {
            ((global::java.lang.Class)typeof(object)).getAnnotatedSuperclass().Should().BeNull();
            ((global::java.lang.Class)typeof(global::java.lang.Class)).getAnnotatedSuperclass().getAnnotations().Length.Should().Be(0);

            var a = TypeAnnotationArrayTestClass.getAnnotatedSuperclass();
            var annos = a.getAnnotations();
            annos.Length.Should().Be(2);

            annos[0].annotationType().Should().Be(TypeAnnotation1Class);
            ((string)TypeAnnotation1ValueMethod.invoke(annos[0])).Should().Be("extends");

            annos[1].annotationType().Should().Be(TypeAnnotation2Class);
            ((string)TypeAnnotation2ValueMethod.invoke(annos[1])).Should().Be("extends2");
        }


        [TestMethod]
        public void TestFields()
        {
            var f1 = TypeAnnotationClassTypeVarAndFieldClass.getDeclaredField("field1");
            var annos1 = f1.getAnnotatedType().getAnnotations();
            annos1.Length.Should().Be(2);
            annos1[0].annotationType().Should().BeSameAs(TypeAnnotation1Class);
            ((string)TypeAnnotation1ValueMethod.invoke(annos1[0])).Should().Be("T1 field");
            annos1[1].annotationType().Should().BeSameAs(TypeAnnotation2Class);
            ((string)TypeAnnotation2ValueMethod.invoke(annos1[1])).Should().Be("T2 field");

            var f2 = TypeAnnotationClassTypeVarAndFieldClass.getDeclaredField("field2");
            var annos2 = f2.getAnnotatedType().getAnnotations();
            annos2.Length.Should().Be(0);

            var f3 = TypeAnnotationClassTypeVarAndFieldClass.getDeclaredField("field3");
            var annos3 = f3.getAnnotatedType().getAnnotations();
            annos3.Length.Should().Be(1);
            annos3[0].annotationType().Should().Be(TypeAnnotation1Class);
            ((string)TypeAnnotation1ValueMethod.invoke(annos3[0])).Should().Be("Object field");
        }

        [TestMethod]
        public void TestNested()
        {
            var m = TypeAnnotationNestedTestClass.getDeclaredMethod("foo", null);
            var annos = m.getAnnotatedReturnType().getAnnotations();

            annos.Length.Should().Be(1);
            annos[0].annotationType().Should().BeSameAs(TypeAnnotation1Class);
            ((string)TypeAnnotation1ValueMethod.invoke(annos[0])).Should().Be("array");

            var t = m.getAnnotatedReturnType();
            t = ((AnnotatedArrayType)t).getAnnotatedGenericComponentType();
            annos = t.getAnnotations();
            annos.Length.Should().Be(1);
            annos[0].annotationType().Should().BeSameAs(TypeAnnotation1Class);
            ((string)TypeAnnotation1ValueMethod.invoke(annos[0])).Should().Be("Inner");
        }

        /// <summary>
        /// Tests that we can retrieve the annotations applied to a wildcard parameterized type declaration on a field type.
        /// Examples:
        /// public Class<@TypeAnnotation1("1") ? extends @TypeAnnotation1("2") Annotation> f1;
        /// public Class<@TypeAnnotation1("3") ? super @TypeAnnotation1("4") Annotation> f2;
        /// </summary>
        [TestMethod]
        public void TestWildcardType()
        {
            var m = TypeAnnotationWildcardTestClass.getDeclaredMethod("foo", null);
            var ret = m.getAnnotatedReturnType();

            var t = ((AnnotatedParameterizedType)ret).getAnnotatedActualTypeArguments();
            t.Length.Should().Be(1);
            ret = t[0];

            var f = TypeAnnotationWildcardTestClass.getDeclaredField("f1");
            var w = (AnnotatedWildcardType)((AnnotatedParameterizedType)f.getAnnotatedType()).getAnnotatedActualTypeArguments()[0];
            t = w.getAnnotatedLowerBounds();
            t.Length.Should().Be(0);
            t = w.getAnnotatedUpperBounds();
            t.Length.Should().Be(1);
            var annos = t[0].getAnnotations();
            annos.Length.Should().Be(1);
            ((string)TypeAnnotation1ValueMethod.invoke(annos[0])).Should().Be("2");

            f = TypeAnnotationWildcardTestClass.getDeclaredField("f2");
            w = (AnnotatedWildcardType)((AnnotatedParameterizedType)f.getAnnotatedType()).getAnnotatedActualTypeArguments()[0];
            t = w.getAnnotatedUpperBounds();
            t.Length.Should().Be(0);
            t = w.getAnnotatedLowerBounds();
            t.Length.Should().Be(1);

        }

    }

}
