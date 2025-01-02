using System.IO;
using System.Runtime.InteropServices;

using Buildalyzer.Environment;

using FluentAssertions;

using IKVM.MSBuild.Tests.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.MSBuild.Tests
{

    [TestClass]
    public class BasicProjectTests
    {

        static ProjectTestUtil.ProjectState state;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ProjectTestUtil.Init(context, "BasicProject", "BasicProject", Path.Combine("Exe", "ProjectExe.csproj"), out state);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ProjectTestUtil.Clean(state);
        }

        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow(EnvironmentPreference.Core, "net472", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net472", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net472", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net48", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net6.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net7.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Core, "net8.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net472", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net48", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net6.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net7.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-x86", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-x64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "win-arm64", "{0}.exe", "{0}.dll")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-x64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-arm", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "linux-musl-arm64", "{0}", "lib{0}.so")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "osx-x64", "{0}", "lib{0}.dylib")]
        [DataRow(EnvironmentPreference.Framework, "net8.0", "osx-arm64", "{0}", "lib{0}.dylib")]
        public void CanBuildTestProject(EnvironmentPreference env, string tfm, string rid, string exe, string lib)
        {
            // skip framework tests for non-Windows platforms
            if (env == EnvironmentPreference.Framework || tfm == "net472" || tfm == "net48")
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    return;

            var analyzer = state.CreateAnalyzer(TestContext, $"{env}-{tfm}-{rid}");
            var options = new EnvironmentOptions();
            options.WorkingDirectory = state.TestRoot;
            options.Preference = env;
            options.DesignTime = false;
            options.Restore = false;
            options.GlobalProperties["TargetFramework"] = tfm;
            options.GlobalProperties["RuntimeIdentifier"] = rid;
            options.TargetsToBuild.Clear();
            options.TargetsToBuild.Add("Clean");
            options.TargetsToBuild.Add("Build");
            options.TargetsToBuild.Add("Publish");
            options.Arguments.Add("/v:d");

            var result = analyzer.Build(options);
            TestContext.AddResultFile(Path.Combine(state.WorkRoot, $"{env}-{tfm}-{rid}-msbuild.binlog"));
            result.OverallSuccess.Should().Be(true);

            var binDir = Path.Combine(state.TestRoot, "Exe", "bin", "Release", tfm, rid);

            // check in build output and publish output
            foreach (var i in new[] { "", "publish" })
            {
                var outDir = Path.Combine(binDir, i);

                // main artifiacts generated by project
                File.Exists(Path.Combine(outDir, string.Format(exe, "ProjectExe"))).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "ProjectLib.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "helloworld.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "helloworld-2.dll")).Should().BeTrue();

                // ikvm libraries
                File.Exists(Path.Combine(outDir, "IKVM.Runtime.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, "IKVM.Java.dll")).Should().BeTrue();
                File.Exists(Path.Combine(outDir, string.Format(lib, "ikvm"))).Should().BeTrue();

                // some rids imply other rids
                var rids = new[] { rid };
                if (rid == "win-x86")
                    rids = [rid, "win-x64"];

                foreach (var r in rids)
                {
                    // ikvm image directories
                    var ridDir = Path.Combine(outDir, "ikvm", r);
                    Directory.Exists(ridDir).Should().BeTrue();
                    Directory.Exists(Path.Combine(ridDir, "bin")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "TRADEMARK")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "bin", "IKVM.Runtime.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "bin", "IKVM.Java.dll")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "lib", "tzdb.dat")).Should().BeTrue();

                    if (r.StartsWith("win-"))
                        File.Exists(Path.Combine(ridDir, "lib", "tzmappings")).Should().BeTrue();

                    File.Exists(Path.Combine(ridDir, "lib", "currency.data")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "lib", "security", "java.policy")).Should().BeTrue();
                    File.Exists(Path.Combine(ridDir, "lib", "security", "java.security")).Should().BeTrue();

                    // ikvm image bin exeecutables
                    foreach (var exeName in new[] { "jar", "jarsigner", "java", "javac", "javah", "javap", "jdeps", "keytool", "native2ascii", "orbd", "policytool", "rmic", "schemagen", "wsgen", "wsimport", "xjc" })
                        File.Exists(Path.Combine(ridDir, "bin", string.Format(exe, exeName))).Should().BeTrue("binary '{0}' must exist", exeName);

                    // ikvm image native libraries
                    foreach (var libName in new[] { "awt", "fontmanager", "iava", "j2gss", "j2pkcs11", "jawt", "jpeg", "jsound", "jvm", "lcms", "management", "mlib_image", "net", "nio", "sunec", "unpack", "verify", "zip" })
                        File.Exists(Path.Combine(ridDir, "bin", string.Format(lib, libName))).Should().BeTrue("library '{0}' must exist", libName);

                    // Windows specific libraries
                    if (r.StartsWith("win-"))
                        foreach (var libName in new[] { "freetype", "jaas_nt", "jsoundds", "w2k_lsa_auth", "sunmscapi" })
                            File.Exists(Path.Combine(ridDir, "bin", string.Format(lib, libName))).Should().BeTrue("library '{0}' must exist on Windows", libName);

                    // Linux specific libraries
                    if (r.StartsWith("linux-"))
                        foreach (var libName in new[] { "awt_headless", "awt_xawt", "freetype", "jaas_unix", "jsoundalsa", "krb5", "sctp" })
                            File.Exists(Path.Combine(ridDir, "bin", string.Format(lib, libName))).Should().BeTrue("library '{0}' must exist on Linux", libName);

                    // OSX specific libraries
                    if (r.StartsWith("osx-"))
                        foreach (var libName in new[] { "awt_lwawt", "jaas_unix", "osx", "osxapp", "osxui", "osxkrb5" })
                            File.Exists(Path.Combine(ridDir, "bin", string.Format(lib, libName))).Should().BeTrue("library '{0}' must exist on OSX", libName);
                }
            }
        }

    }

}
