using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikvmc.Tests
{
    [TestClass]
    public class IkvmcCompilerTests
    {
        [TestMethod]
        [DataRow(@"META-INF/foo/bar/baz.class", true)]
        [DataRow(@"META-INF/Person.class", true)]
        [DataRow(@"META-INF/versions/9/Person.class", true)]
        [DataRow(@"META-INF\foo\bar\baz.class", false)]
        [DataRow(@"meta-inf/foo/bar/baz.class", false)]
        [DataRow(@"org/mycompany/myapp/test.class", false)]
        [DataRow(@"module-info.class", false)]
        [DataRow(@"META-INF/versions/9/module-info.class", true)]
        public void Can_detect_META_INF_class_file(string name, bool expected)
        {
            IkvmcCompiler.IsMetaInfClassFile(name).Should().Be(expected);
        }

        [TestMethod]
        [DataRow(@"META-INF/foo/bar/baz.class", false, default(int))]
        [DataRow(@"META-INF/Person.class", false, default(int))]
        [DataRow(@"META-INF/versions/9/Person.class", true, 9)]
        [DataRow(@"META-INF\foo\bar\baz.class", false, default(int))]
        [DataRow(@"meta-inf/foo/bar/baz.class", false, default(int))]
        [DataRow(@"org/mycompany/myapp/test.class", false, default(int))]
        [DataRow(@"module-info.class", false, default(int))]
        [DataRow(@"META-INF/versions/9/module-info.class", true, 9)]
        [DataRow(@"META-INF/versions/17/org/arg/app/SomeClass.class", true, 17)]
        [DataRow(@"META-INF/versions/17a/org/arg/app/SomeClass.class", false, default(int))]
        [DataRow(@"META-INF/versions/17.5/org/arg/app/SomeClass.class", false, default(int))]
        public void Can_detect_multi_release_class_file_and_version(string name, bool expected, int expectedVersion)
        {
            IkvmcCompiler.IsMultiReleaseClassFile(name, out int actualVersion).Should().Be(expected);
            actualVersion.Should().Be(expectedVersion);
        }
    }
}
