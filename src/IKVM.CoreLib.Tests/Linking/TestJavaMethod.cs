namespace IKVM.CoreLib.Tests.Linking
{

    class TestJavaMethod : TestJavaMember
    {

        public TestJavaMethod(TestJavaType declaringType, string name, string signature, TestJavaType returnType) :
            base(declaringType, name, signature)
        {
            ReturnType = returnType;
        }

        public TestJavaType ReturnType { get; }

    }

}
