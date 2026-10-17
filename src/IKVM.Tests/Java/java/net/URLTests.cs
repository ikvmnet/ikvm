using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CliWrap;

using FluentAssertions;

using IKVM.Java.Tests.Util;

using javax.net.ssl;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Java.java.net
{

    [TestClass]
    public class URLTests
    {

        const string HandlerPkgsMainSource = """
            package ikvm.tests.handlerpkgs;

            public class Main {

                public static void main(String[] args) throws Exception {
                    System.out.println("main");
                    System.out.println(new java.net.URL(args[0]).openConnection().getClass().getName());
                }

            }
            """;

        const string HandlerPkgsHandlerSource = """
            package ikvm.tests.handlerpkgs.http;

            public class Handler extends java.net.URLStreamHandler {

                protected java.net.URLConnection openConnection(java.net.URL u) {
                    return new Connection(u);
                }

                public static class Connection extends java.net.URLConnection {

                    Connection(java.net.URL u) {
                        super(u);
                    }

                    public void connect() {

                    }

                }

            }
            """;

        /// <summary>
        /// Launches a new JVM with the given 'java.protocol.handler.pkgs' value, running a main class that opens a connection to an http URL.
        /// </summary>
        /// <param name="pkgs"></param>
        /// <returns></returns>
        static async Task<(int ExitCode, string StdOut, string StdErr)> RunWithProtocolHandlerPkgs(string pkgs)
        {
            var c = new InMemoryCompiler(new[] {
                new InMemoryCodeUnit("ikvm.tests.handlerpkgs.Main", HandlerPkgsMainSource),
                new InMemoryCodeUnit("ikvm.tests.handlerpkgs.http.Handler", HandlerPkgsHandlerSource),
            });
            c.Compile();

            var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(dir);
            var jar = Path.Combine(dir, "handlerpkgs.jar");
            c.WriteJar(jar);

            var exe = Path.Combine(global::java.lang.System.getProperty("java.home"), "bin", "java");
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(exe);
                var prm = psx.FileAccessPermissions;
                prm |= Mono.Unix.FileAccessPermissions.UserExecute;
                prm |= Mono.Unix.FileAccessPermissions.GroupExecute;
                prm |= Mono.Unix.FileAccessPermissions.OtherExecute;
                if (prm != psx.FileAccessPermissions)
                    psx.FileAccessPermissions = prm;
            }
#endif

            try
            {
                var o = new StringBuilder();
                var e = new StringBuilder();
                var r = await Cli.Wrap(exe)
                    .WithArguments(new[] { $"-Djava.protocol.handler.pkgs={pkgs}", "-cp", jar, "ikvm.tests.handlerpkgs.Main", "http://www.site.com/" })
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(o))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(e))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync();

                return (r.ExitCode, o.ToString(), e.ToString());
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [TestMethod]
        public async Task CanStartWithUnknownProtocolHandlerPkgs()
        {
            var r = await RunWithProtocolHandlerPkgs("Test");
            r.ExitCode.Should().Be(0, r.StdErr);
            r.StdOut.Should().Contain("main");
            r.StdOut.Should().Contain("sun.net.www.protocol.http.HttpURLConnection");
        }

        [TestMethod]
        public async Task CanUseHandlerFromProtocolHandlerPkgs()
        {
            var r = await RunWithProtocolHandlerPkgs("Test|ikvm.tests.handlerpkgs");
            r.ExitCode.Should().Be(0, r.StdErr);
            r.StdOut.Should().Contain("main");
            r.StdOut.Should().Contain("ikvm.tests.handlerpkgs.http.Handler$Connection");
        }

        [TestMethod]
        public void CanCreateHttpURL()
        {
            var u = new global::java.net.URL("http://www.site.com/");
            u.getProtocol().Should().Be("http");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateHttpsURL()
        {
            var u = new global::java.net.URL("https://www.site.com/");
            u.getProtocol().Should().Be("https");
            u.getHost().Should().Be("www.site.com");
            u.getPath().Should().Be("/");
        }

        [TestMethod]
        public void CanCreateWindowsFileURL()
        {
            var u = new global::java.net.URL("file:///c:/dir");
            u.getProtocol().Should().Be("file");
            u.getHost().Should().Be("");
            u.getPath().Should().Be("/c:/dir");
        }

        [TestMethod]
        public void CanCreateUnixFileURL()
        {
            var u = new global::java.net.URL("file:///tmp/foo");
            u.getProtocol().Should().Be("file");
            u.getHost().Should().Be("");
            u.getPath().Should().Be("/tmp/foo");
        }

        [DataTestMethod]
        [DataRow("tls-v1-2")]
        [DataRow("sha256")]
        //[DataRow("sha384")]
        //[DataRow("sha512")]
        [DataRow("rsa2048")]
        [DataRow("rsa4096")]
        //[DataRow("rsa8192")]
        [DataRow("ecc256")]
        [DataRow("ecc384")]
        //[DataRow("dh480")]
        //[DataRow("dh512")]
        [DataRow("dh1024")]
        public void CanGetFromURL(string prefix)
        {
            var stm = new global::java.net.URL($"https://{prefix}.badssl.com/").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
            str.Should().Contain($"{prefix}.badssl.com");
        }

        [TestMethod]
        public void CanGetFromSecureUrl()
        {
            var stm = new global::java.net.URL("https://repository.jboss.org/nexus/content/repositories/thirdparty-releases/javax/media/jai-core/1.1.3/jai-core-1.1.3.pom").openStream();
            var buf = new byte[8192];
            var bytesRead = stm.read(buf, 0, buf.Length);
            var str = Encoding.UTF8.GetString(buf, 0, bytesRead);
        }

    }

}
