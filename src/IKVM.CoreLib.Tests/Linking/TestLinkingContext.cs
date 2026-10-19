using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;

namespace IKVM.CoreLib.Tests.Linking
{

    class TestLinkingContext : ILinkingContext<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        static readonly TestJavaType JavaLangObjectType = new TestJavaType();
        static readonly TestJavaType VerifierNullType = new TestJavaType();

        readonly bool _importer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="importer"></param>
        public TestLinkingContext(bool importer)
        {
            _importer = importer;
        }

        public bool IsImporter => _importer;

        public IDiagnosticHandler Diagnostics => throw new global::System.NotImplementedException();

        public TestJavaType TypeOfJavaLangObject => JavaLangObjectType;

        public TestJavaType TypeOfVerifierNull => VerifierNullType;

        public bool AllowNonVirtualCalls => throw new global::System.NotImplementedException();

        public TestJavaType CreateUnloadableType(string name) => throw new global::System.NotImplementedException();

    }

}
