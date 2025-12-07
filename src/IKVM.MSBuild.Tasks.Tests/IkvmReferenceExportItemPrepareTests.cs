using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using FluentAssertions;

using IKVM.Tests.Util;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace IKVM.MSBuild.Tasks.Tests
{

    [TestClass]
    public class IkvmReferenceExportItemPrepareTests
    {

        [DataTestMethod]
        [DataRow("net472", ".NETFramework", "4.7.2")]
        [DataRow("net48", ".NETFramework", "4.8")]
        [DataRow("net6.0", ".NET", "6.0")]
        [DataRow("net7.0", ".NET", "7.0")]
        [DataRow("net8.0", ".NET", "8.0")]
        [DataRow("net10.0", ".NET", "10.0")]
        public void CanPrepare(string tfm, string targetFrameworkIdentifier, string targetFrameworkVersion)
        {
            var engine = new Mock<IBuildEngine7>();
            var errors = new List<BuildErrorEventArgs>();
            engine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback((BuildErrorEventArgs e) => errors.Add(e));

            var a = new List<TaskItem>();
            foreach (var i in DotNetSdkUtil.GetPathToReferenceAssemblies(tfm, targetFrameworkIdentifier, targetFrameworkVersion))
                foreach (var r in Directory.GetFiles(i, "*.dll"))
                    a.Add(new TaskItem(r));

            var t = new IkvmReferenceExportItemPrepare();
            t.BuildEngine = engine.Object;
            t.ToolFramework = tfm;
            t.ToolVersion = "";
            t.StateFile = Path.GetTempFileName();
            t.Items = a.ToArray();
            t.References = a.ToArray();
            t.Execute().Should().BeTrue();
        }

    }

}
