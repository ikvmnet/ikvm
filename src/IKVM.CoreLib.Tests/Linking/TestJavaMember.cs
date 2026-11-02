namespace IKVM.CoreLib.Tests.Linking
{

    abstract class TestJavaMember
    {

        protected TestJavaMember(TestJavaType declaringType, string name, string signature)
        {
            DeclaringType = declaringType;
            Name = name;
            Signature = signature;
        }

        public TestJavaType DeclaringType { get; }

        public string Name { get; }

        public string Signature { get; }

    }

}
