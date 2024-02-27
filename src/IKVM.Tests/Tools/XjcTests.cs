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
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(c);
                var prm = psx.FileAccessPermissions;
                prm |= Mono.Unix.FileAccessPermissions.UserExecute;
                prm |= Mono.Unix.FileAccessPermissions.GroupExecute;
                prm |= Mono.Unix.FileAccessPermissions.OtherExecute;
                if (prm != psx.FileAccessPermissions)
                    psx.FileAccessPermissions = prm;
            }
#endif
            var r = await Cli.Wrap(c).WithArguments("-help").WithStandardOutputPipe(PipeTarget.ToDelegate(i => s.Append(i))).WithValidation(CommandResultValidation.None).ExecuteAsync();
            r.ExitCode.Should().Be(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? -1 : 255);
            s.ToString().Should().StartWith("Usage: xjc");
        }

    }

}
