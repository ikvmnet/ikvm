using System.IO;


using Microsoft.VisualStudio.TestTools.UnitTesting;

using IKVM.Internal;
using IKVM.ByteCode.Reading;

#if NETCOREAPP
using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Tools.Importer.Tests
{

    [TestClass]
    public class IkvmImporterTests
    {

//        [TestMethod]
//        public async Task Should_convert_simple_jar()
//        {
//            var s = new StreamReader(typeof(IkvmImporterTests).Assembly.GetManifestResourceStream("IKVM.Tools.Importer.Tests.IkvmImporterTests.java")).ReadToEnd();
//            var f = new InMemoryCodeUnit("ikvm.tools.importer.tests.IkvmImporterTests", s);
//            var c = new InMemoryCompiler(new[] { f });
//            c.Compile();
//            var j = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n") + ".jar");
//            c.WriteJar(j);

//#if NETFRAMEWORK
//            var a = new[] { $"-lib:{RuntimeEnvironment.GetRuntimeDirectory()}" };
//#else
//            var a = DependencyContext.Default.CompileLibraries.SelectMany(i => i.ResolveReferencePaths()).Select(i => $"-r:{i}");
//#endif

//            var asm = Path.ChangeExtension(j, ".dll");
//            var ret = await IkvmImporterTool.Main(a.Concat(new[] { "-nostdlib", "-assembly:IKVM.Tools.Importer.Tests.Java", $"-out:{asm}", j }).ToArray(), CancellationToken.None);
//            ret.Should().Be(0);
//            File.Exists(asm).Should().BeTrue();
//        }

        [TestMethod]
        public void Foo()
        {
            var c = ClassReader.Read(File.OpenRead(@"C:\dev\ikvm\src\IKVM.ByteCode.Tests\resources\ThreadDeath.class"));
            var f = new ClassFile(c, "java.lang.ThreadDeath", ClassFileParseOptions.None, null);
        }

    }

}