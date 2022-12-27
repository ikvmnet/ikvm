using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace IKVM.JTReg.TestAdapter
{

    static class JTRegTestProperties
    {

        public static readonly TestProperty TestSuiteRootProperty = TestProperty.Register("IkvmJTReg.TestSuiteRoot", "TestSuiteRoot", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestSuiteNameProperty = TestProperty.Register("IkvmJTReg.TestSuiteName", "TestSuiteName", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestPathNameProperty = TestProperty.Register("IkvmJTReg.TestPathName", "TestPathName", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestIdProperty = TestProperty.Register("IkvmJTReg.TestId", "TestId", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestNameProperty = TestProperty.Register("IkvmJTReg.TestName", "TestName", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestTitleProperty = TestProperty.Register("IkvmJTReg.TestTitleProperty", "TestTitleProperty", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestAuthorProperty = TestProperty.Register("IkvmJTReg.TestAuthorProperty", "TestAuthorProperty", typeof(string), TestPropertyAttributes.Immutable, typeof(TestCase));
        public static readonly TestProperty TestPartitionProperty = TestProperty.Register("IkvmJTReg.TestPartition", "TestPartition", typeof(int), TestPropertyAttributes.Hidden, typeof(TestCase));

        public static readonly TestProperty TestCategoryProperty = TestProperty.Register("MSTestDiscoverer.TestCategory", "TestCategory", typeof(string[]), TestPropertyAttributes.Hidden | TestPropertyAttributes.Trait, typeof(TestCase));

    }

}
