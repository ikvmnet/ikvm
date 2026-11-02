using System;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Jvm;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Tests.Linking
{

    readonly struct TestJvmTypeSystem : IJvmTypeSystem<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        static readonly TestJavaType JavaLangObjectType = new TestJavaType();
        static readonly TestJavaType VerifierNullType = new TestJavaType();

        readonly bool _importer;

        public TestJvmTypeSystem(bool importer)
        {
            _importer = importer;
        }

        public bool IsImporter => _importer;

        public IDiagnosticHandler Diagnostics => throw new NotImplementedException();

        public bool AllowNonVirtualCalls => throw new NotImplementedException();

        public TestJavaType TypeOfJavaLangObject => JavaLangObjectType;

        public TestJavaType TypeOfVerifierNull => VerifierNullType;

        public TestJavaType CreateUnloadableType(string name) => throw new NotImplementedException();

        public string GetName(TestJavaType type) => throw new NotImplementedException();

        public bool IsUnloadable(TestJavaType type) => throw new NotImplementedException();

        public bool IsInterface(TestJavaType type) => throw new NotImplementedException();

        public ClassFileAccessFlags GetAccessFlags(TestJavaType type) => throw new NotImplementedException();

        public TestJavaType? GetBaseType(TestJavaType type) => throw new NotImplementedException();

        public bool IsSubTypeOf(TestJavaType type, TestJavaType baseType) => throw new NotImplementedException();

        public bool CheckPackageAccess(TestJavaType type, TestJavaType target) => throw new NotImplementedException();

        public TestJavaField? GetField(TestJavaType type, string name, string signature) => throw new NotImplementedException();

        public TestJavaMethod? GetMethod(TestJavaType type, string name, string signature, bool inherit) => throw new NotImplementedException();

        public TestJavaMethod? GetInterfaceMethod(TestJavaType type, string name, string signature) => throw new NotImplementedException();

        public TestJavaType LoadType(TestJavaType context, string name, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType GetFieldTypeFromSignature(TestJavaType context, string signature, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType[] GetArgTypeListFromSignature(TestJavaType context, string descriptor, LoadMode mode) => throw new NotImplementedException();

        public TestJavaType GetReturnTypeFromSignature(TestJavaType context, string descriptor, LoadMode mode) => throw new NotImplementedException();

        public void EnsureLinked(TestJavaField field, LoadMode mode) => throw new NotImplementedException();

        public void EnsureLinked(TestJavaMethod method, LoadMode mode) => throw new NotImplementedException();

    }

}
