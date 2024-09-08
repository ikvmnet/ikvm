using System;
using System.IO;
using System.Text;

using FluentAssertions;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IKVM.Tools.Tests.Core.Diagnostics
{

    [TestClass]
    public class TextDiagnosticFormatterTests
    {

        [TestMethod]
        public void CanFormatEvent()
        {
            var mem = new MemoryStream();
            var opt = new TextDiagnosticFormatterOptions(new StreamDiagnosticChannel(mem, Encoding.UTF8, false));
            var wrt = new TextDiagnosticFormatter(opt);
            wrt.Write(DiagnosticEvent.GenericClassLoadingError("ERROR"));
            wrt.Dispose();

            var txt = Encoding.UTF8.GetString(mem.ToArray());
            txt.Should().Be("error IKVM4019: ERROR" + Environment.NewLine);
        }

    }

}
