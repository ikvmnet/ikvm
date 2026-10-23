using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CliWrap;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tests.Tools
{

    [TestClass]
    public class XjcTests
    {

        [TestMethod]
        public async Task CanDisplayHelp()
        {
            var s = new StringBuilder();
            var c = Path.Combine(java.lang.System.getProperty("java.home"), "bin", "xjc");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
#if NET7_0_OR_GREATER
                var mod = File.GetUnixFileMode(c);
                var prm = mod | UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
                if (mod != prm)
                    File.SetUnixFileMode(c, prm);
#else
                var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(c);
                var mod = psx.FileAccessPermissions;
                var prm = mod | Mono.Unix.FileAccessPermissions.UserExecute | Mono.Unix.FileAccessPermissions.GroupExecute | Mono.Unix.FileAccessPermissions.OtherExecute;
                if (mod != prm)
                    psx.FileAccessPermissions = prm;
#endif
            }

            var r = await Cli.Wrap(c).WithArguments("-help").WithStandardOutputPipe(PipeTarget.ToDelegate(i => s.Append(i))).WithValidation(CommandResultValidation.None).ExecuteAsync();
            r.ExitCode.Should().Be(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? -1 : 255);
            s.ToString().Should().StartWith("Usage: xjc");
        }

    }

}
