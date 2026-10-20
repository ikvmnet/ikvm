using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Tests.Linking
{

    abstract class TestJavaField : TestJavaMember, ILinkingField<TestJavaType, TestJavaMember, TestJavaField, TestJavaMethod>
    {

        public abstract void Link(LoadMode mode);

    }

}
