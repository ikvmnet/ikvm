using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Tests.Linking
{

    class TestJavaType : ILinkingType<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        public string Name => throw new global::System.NotImplementedException();

        public bool IsUnloadable => throw new global::System.NotImplementedException();

        public bool IsInterface => throw new global::System.NotImplementedException();

        public ClassFileAccessFlags AccessFlags => throw new global::System.NotImplementedException();

        public TestJavaType? BaseType => throw new global::System.NotImplementedException();

        public bool CheckPackageAccess(TestJavaType type)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaType[] GetArgTypeListFromSignature(string descriptor, LoadMode mode)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaField? GetField(string name, string signature)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaType GetFieldTypeFromSignature(string signature, LoadMode mode)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaMethod? GetInterfaceMethod(string name, string signature)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaMethod? GetMethod(string name, string signature, bool inherit)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaType GetReturnTypeFromSignature(string descriptor, LoadMode mode)
        {
            throw new global::System.NotImplementedException();
        }

        public bool IsSubTypeOf(TestJavaType type)
        {
            throw new global::System.NotImplementedException();
        }

        public TestJavaType LoadType(string name, LoadMode mode)
        {
            throw new global::System.NotImplementedException();
        }

    }

}
