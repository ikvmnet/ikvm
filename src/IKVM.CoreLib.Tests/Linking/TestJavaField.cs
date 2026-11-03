namespace IKVM.CoreLib.Tests.Linking
{

    class TestJavaField : TestJavaMember
    {

        public TestJavaField(TestJavaType declaringType, string name, string signature, TestJavaType fieldType) :
            base(declaringType, name, signature)
        {
            FieldType = fieldType;
        }

        public TestJavaType FieldType { get; }

    }

}
